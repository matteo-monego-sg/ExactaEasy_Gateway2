#include "ClientControl.h"

const char* STREAM_FILE_NAME = "input.txt";

ClientControl::ClientControl()
{
  computerFunction_ = COMPUTER_FUNCTION_CLIENT;
}

void ClientControl::openClient(CommunicationProtocol communicationProtocol, unsigned int sendPort, unsigned int receivePort, const char* serverAddress)
{
  communicationProtocol_ = communicationProtocol;
  port_ = sendPort;
  receivePort_ = receivePort;
  if(strlen(serverAddress) > sizeof(serverAddress_))
    throw invalid_argument("Not a valid server address.");
  sprintf_s(serverAddress_, "%s", serverAddress);

  initializeWinsock();
  communicationSocket_ = createSocket();

  setServerAddressInfo();

  setSocketTimeout(250);	//pier: portato da 500 a 250...

  if(communicationProtocol_ == TCP)
	connectToServer();
  else if(communicationProtocol_ == UDP)
    connectToServer();

  printf("Client ready.\n");
}

void ClientControl::closeClient()
{
  closeInterface();

  printf("Client closed\n");
}

void ClientControl::setServerAddressInfo()
{
  initAddressInfo(serverAddressInfo_);
  serverAddressInfo_.sin_addr.s_addr = inet_addr(serverAddress_);
  initAddressInfo(localAddressInfo_);
  localAddressInfo_.sin_addr.s_addr = INADDR_ANY;
  localAddressInfo_.sin_port = htons(receivePort_);
}

void ClientControl::connectToServer()
{
  int error;
  char exceptionBuffer[MAX_PATH];

  error = connect(communicationSocket_, (struct sockaddr*)(&serverAddressInfo_), sizeof(serverAddressInfo_));

  if(error < 0)
  {
#ifdef WIN32
    sprintf_s(exceptionBuffer, sizeof(exceptionBuffer), "Error connecting socket: (%d).", WSAGetLastError());
#else
    sprintf_s(exceptionBuffer, sizeof(exceptionBuffer), "Error connecting socket: (%d).", errno);
#endif
    throw runtime_error(exceptionBuffer);
  }
}
