#include "ServerControl.h"

void* waitForIncomingClient(void* params);

struct WaitForIncomingClientParams
{
    SOCKET* pCommunicationSocket_;
    bool* pClientConnected_;
};

WaitForIncomingClientParams gWaitForIncomingClientParams;

ServerControl::ServerControl()
{
  computerFunction_ = COMPUTER_FUNCTION_SERVER;
  clientConnected_ = false;
}

void ServerControl::openServer(CommunicationProtocol communicationProtocol, unsigned int port)
{
  communicationProtocol_ = communicationProtocol;
  port_ = port;
  clientConnected_ = false;

  initializeWinsock();
  communicationSocket_ = createSocket();

  setServerAddressInfo();

  //setSocketTimeout(0);
  setSocketTimeout(200);

  bindSocket();

  if(communicationProtocol_ == TCP)
    createWaitForIncomingClientThread();

  //shutdownConnection();
  //terminateSession();

  //printf("Client connected\n");
}

bool ServerControl::isClientConnected()
{
    return clientConnected_;
}

bool ServerControl::closeServer()
{
    closeInterface();
    clientConnected_ = false;

    printf("Server closed\n");
    return true;
}

void ServerControl::setServerAddressInfo()
{
  initAddressInfo(serverAddressInfo_);
  serverAddressInfo_.sin_addr.s_addr = INADDR_ANY;
}

void ServerControl::createWaitForIncomingClientThread()
{
    int ret;
    char exceptionBuffer[32];

    gWaitForIncomingClientParams.pCommunicationSocket_ = &communicationSocket_;
    gWaitForIncomingClientParams.pClientConnected_ = &clientConnected_;

    ret = pthread_create(&waitClientThread_, 0, waitForIncomingClient, 0);

    if(ret != 0)
    {
        sprintf(exceptionBuffer, "Can't create wait client thread (%d).", ret);
        throw runtime_error(exceptionBuffer);
    }
}

void* waitForIncomingClient(void *params)   //params passati per far funzionare il thread, ma realmente non utilizzati
{
  char exceptionBuffer[32];
  int clientSocket;
  int timeoutReached;

  if (params==NULL) { } //solo per evitare warning

  printf("Wait for an incoming client...\n");

  listen(*(gWaitForIncomingClientParams.pCommunicationSocket_), SOMAXCONN);

  do
  {
      // Accept a client socket
      clientSocket = accept(*(gWaitForIncomingClientParams.pCommunicationSocket_), NULL, NULL);
      int lastError = errno;
      //printf("CLIENT SOCKET = %d - %d\n", clientSocket, lastError);
      if(clientSocket < 0)
      {
    #ifdef WIN32
        sprintf(exceptionBuffer, "Accept failed (%d)", WSAGetLastError());
        closesocket(*(gWaitForIncomingClientParams.pCommunicationSocket_));
        throw runtime_error(exceptionBuffer);
    #else
        if (lastError == EAGAIN)
        {
            timeoutReached = true;
        }
        else if (lastError == EINTR)
        {
            printf("WAITING ...");
        }
        else
        {
            sprintf(exceptionBuffer, "Accept failed (%d)", lastError);
            close(*(gWaitForIncomingClientParams.pCommunicationSocket_));
            throw runtime_error(exceptionBuffer);
        }
    #endif
      }
      else
      {
          timeoutReached = false;
      }
  } while(timeoutReached);

  // No longer need server socket
#ifdef WIN32
  closesocket(*(gWaitForIncomingClientParams.pCommunicationSocket_));
#else
  close(*(gWaitForIncomingClientParams.pCommunicationSocket_));
#endif

  *(gWaitForIncomingClientParams.pCommunicationSocket_)= clientSocket;
  *(gWaitForIncomingClientParams.pClientConnected_) = true;

  printf("Client connected!\n");

  return 0;
}

