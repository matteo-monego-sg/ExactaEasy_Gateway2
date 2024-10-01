#include "NetworkControl.h"

#ifdef WIN32
WSADATA gWsaData;
#endif

NetworkControl::NetworkControl()
{
	communicationSocket_ = INVALID_SOCKET;
	communicationProtocol_ = TCP;
	port_ = DEFAULT_PORT;
	memset((char*)(&serverAddressInfo_), 0, sizeof(serverAddressInfo_));
	lastError_ = 0;
}

void NetworkControl::initializeWinsock()
{
#ifdef WIN32
	int error;

	error = WSAStartup(MAKEWORD(2,2), &gWsaData);
	if (error != 0)
	{
		char exceptionBuffer[32];
		sprintf(exceptionBuffer, "WSAStartup failed with error: %d\n", error);
		lastError_ = error;
		throw runtime_error(exceptionBuffer);
	}
	printf("WSAStartup done.\n");
#endif
}

SOCKET NetworkControl::createSocket()
{
	SOCKET ret;

	if(communicationProtocol_ == TCP)
	{
		ret = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	}
	else
	{
		ret = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
	}

	if(ret == INVALID_SOCKET)
	{
		char exceptionBuffer[32];
#ifdef WIN32
		sprintf(exceptionBuffer, "Could not create socket (%d).", WSAGetLastError());
		lastError_ = WSAGetLastError();
#else
		sprintf(exceptionBuffer, "Could not create socket (%d).", errno);
		lastError_ = errno;
#endif
		throw runtime_error(exceptionBuffer);
	}
	printf("Socket created.\n");

#ifdef WIN32
	//int set = 1;
	setsockopt(ret, SOL_SOCKET, 0, 0, sizeof(int));

	//pier
	//FD_ZERO(&readfds);
	//FD_SET(communicationSocket_, &readfds);
	/*int timeout = 50;
	setsockopt(ret, SOL_SOCKET, SO_RCVTIMEO, reinterpret_cast<char*>(&timeout), sizeof(timeout));
	setsockopt(ret, SOL_SOCKET, SO_SNDTIMEO, reinterpret_cast<char*>(&timeout), sizeof(timeout));*/

	//setsockopt(ret, SOL_SOCKET, SO_NOSIGPIPE, (void *)&set, sizeof(int));
#else
	//signal(SIGPIPE, SIG_IGN);
#endif

	return ret;
}

int NetworkControl::closeSocket(SOCKET socket)
{
#ifdef WIN32
	closesocket(socket);
#else
	close(socket);
#endif
	return 0;
}

void NetworkControl::bindSocket()
{
	int error;
	int reuse = 1;
	error = setsockopt(communicationSocket_, SOL_SOCKET, SO_REUSEADDR, reinterpret_cast<char*>(&reuse), sizeof(reuse));


	if(communicationProtocol_ == TCP || computerFunction_ == COMPUTER_FUNCTION_SERVER)
		error = bind(communicationSocket_, (struct sockaddr*)(&serverAddressInfo_), sizeof(serverAddressInfo_));
	else
		error = bind(communicationSocket_, (struct sockaddr*)(&localAddressInfo_), sizeof(localAddressInfo_));
	if(error == SOCKET_ERROR)
	{
		char exceptionBuffer[32];
#ifdef WIN32
		sprintf(exceptionBuffer, "Error connecting socket: %ld\n", WSAGetLastError());
		lastError_ = WSAGetLastError();
#else
		sprintf(exceptionBuffer, "Error connecting socket: %d\n", errno);
		lastError_ = errno;
#endif
		closeSocket(communicationSocket_);
#ifdef WIN32
		WSACleanup();
#endif
		throw runtime_error(exceptionBuffer);
	}

	printf("Bind done.\n");
}

void NetworkControl::setSocketTimeout(unsigned int timeoutMilliseconds)
{
	int error;

#ifdef WIN32
	error = setsockopt(communicationSocket_, SOL_SOCKET, SO_RCVTIMEO, reinterpret_cast<char*>(&timeoutMilliseconds), sizeof(timeoutMilliseconds));
#else
	timeval timeout;
	timeout.tv_sec = timeoutMilliseconds/1000;
	timeout.tv_usec = 1000*(timeoutMilliseconds%1000);
	error = setsockopt(communicationSocket_, SOL_SOCKET, SO_RCVTIMEO, reinterpret_cast<char*>(&timeout), sizeof(timeout));
#endif
	if(error != 0)
	{
		char exceptionBuffer[250];
#ifdef WIN32
		sprintf(exceptionBuffer, "Timeout setting failed with error: %d\n", WSAGetLastError());
		lastError_ = WSAGetLastError();
#else
		sprintf(exceptionBuffer, "Timeout setting failed with error: %d\n", errno);
		lastError_ = errno;
#endif
		closeSocket(communicationSocket_);
#ifdef WIN32
		WSACleanup();
#endif
		throw runtime_error(exceptionBuffer);
	}
}

void NetworkControl::initAddressInfo(sockaddr_in& addressInfo)
{
	memset((&addressInfo), 0, sizeof(addressInfo));
	addressInfo.sin_family = AF_INET;
	addressInfo.sin_port = htons(port_);
}

void NetworkControl::closeInterface()
{
	shutdownConnection();
	terminateSession();
}

void NetworkControl::shutdownConnection()
{
	if(communicationSocket_ == INVALID_SOCKET)
		return;

	int error;

	error = closeSocket(communicationSocket_);

	if(error < 0)
	{
		char exceptionBuffer[32];
#ifdef WIN32
		sprintf(exceptionBuffer, "Error closing connection (%d).", WSAGetLastError());
		lastError_ = WSAGetLastError();
#else
		sprintf(exceptionBuffer, "Error closing connection (%d).", errno);
		lastError_ = errno;
#endif
		throw runtime_error(exceptionBuffer);
	}

	communicationSocket_ = INVALID_SOCKET;
}

void NetworkControl::terminateSession()
{
#ifdef WIN32
	WSACleanup();
#endif
}

int NetworkControl::sendData(char* buffer, unsigned int bufferLength)
{
	int error;

	if(communicationProtocol_ == TCP)
	{
		try
		{
#ifdef WIN32
			error = send(communicationSocket_, buffer, bufferLength, 0);
#else
			error = send(communicationSocket_, buffer, bufferLength, MSG_NOSIGNAL );
#endif
		}
		catch(...)
		{
          printf("error!!");
		}
	}
	else
	{
		if(computerFunction_ == COMPUTER_FUNCTION_SERVER)
			error = sendto(communicationSocket_, buffer, bufferLength, 0, (struct sockaddr*)(&clientAddressInfo_), sizeof(clientAddressInfo_));
		else
			error = sendto(communicationSocket_, buffer, bufferLength, 0, (struct sockaddr*)(&serverAddressInfo_), sizeof(serverAddressInfo_));
	}

	if (error == SOCKET_ERROR)
	{
		char exceptionBuffer[32];
#ifdef WIN32
		sprintf(exceptionBuffer, "send failed with error: %d\n", WSAGetLastError());
		lastError_ = WSAGetLastError();
#else
		sprintf(exceptionBuffer, "send failed with error: %d\n", errno);
		lastError_ = errno;
#endif
		closeSocket(communicationSocket_);
#ifdef WIN32
		WSACleanup();
#endif
		throw runtime_error(exceptionBuffer);
	}

	return error;
}

int NetworkControl::receiveData(char* buffer, unsigned int bufferLength)
{
#ifdef WIN32
	int infoLen;
#else
	unsigned int infoLen;
#endif
	int dataLength;

	char exceptionBuffer[250];

	if(communicationProtocol_ == TCP)
	{
		//dataLength = recv(communicationSocket_, buffer, bufferLength, 0);	//OLD
		
		//TURBIDIMETER (WINDOWS 7)
		//dataLength = recv(communicationSocket_, buffer, bufferLength, MSG_PEEK);
		//sprintf_s(debugStr, "post recv, dataLength = %d", dataLength);
		//if (dataLength>(int)bufferLength)
		//{
		//	printf("ERROR! BUFFER OVERFLOW!\n");
		//	return 0;
		//}
		//if (dataLength>0)
		//{
		//	dataLength = recv(communicationSocket_, buffer, dataLength, MSG_WAITALL);
		//}

		//COLORIMETER (WINDOWS XP/7?)
		dataLength = recv(communicationSocket_, buffer, sizeof(unsigned int), MSG_PEEK);
		//sprintf_s(debugStr, "PEEK: dataLength = %d", dataLength);
		//MessageBoxA(NULL, debugStr, "", 0);
		if (dataLength>0)
		{
			unsigned int msgSize = 0;
			memcpy(&msgSize, buffer, sizeof(unsigned int));
			//sprintf_s(debugStr, "msgSize = %d", msgSize);
			//MessageBoxA(NULL, debugStr, "", 0);
			if (msgSize>(int)bufferLength)
			{
				printf("ERROR! BUFFER OVERFLOW!\n");
				OutputDebugString("ERROR! BUFFER OVERFLOW!\n");
				return 0;
			}
			if (msgSize>0)
			{
				//dataLength = recv(communicationSocket_, buffer, msgSize, MSG_WAITALL);
				dataLength = 0;
				unsigned int origMsgSize = msgSize;
				while (dataLength<origMsgSize)
				{
					dataLength += recv(communicationSocket_, &buffer[dataLength], msgSize, 0);
					msgSize = origMsgSize - dataLength;
				}
				// HARD DEBUG
				char debugStr[250];
				sprintf_s(debugStr, "NetCtrl.receiveData: dataLength = %d\n", dataLength);
				OutputDebugString(debugStr);
				//MessageBoxA(NULL, debugStr, "", 0);
			/*	if (dataLength!=msgSize)
				{
					sprintf_s(debugStr, "dataLength = %d, msgSize = %d", dataLength, msgSize);
					MessageBoxA(NULL, debugStr, "", 0);
				}*/
			}
		}

		//dataLength = recv(communicationSocket_, buffer, bufferLength, 0);
		//sprintf_s(debugStr, "1: dataLength = %d", dataLength);
		//MessageBoxA(NULL, debugStr, "", 0);
		//if (dataLength>=sizeof(unsigned int))
		//{
		//	unsigned int msgSize = 0;
		//	memcpy(&msgSize, buffer, sizeof(unsigned int));
		//	sprintf_s(debugStr, "msgSize = %d", msgSize);
		//	MessageBoxA(NULL, debugStr, "", 0);
		//	if (msgSize>(int)bufferLength)
		//	{
		//		printf("ERROR! BUFFER OVERFLOW!\n");
		//		return 0;
		//	}
		//	if (msgSize>0)
		//	{
		//		//dataLength = recv(communicationSocket_, buffer, msgSize, MSG_WAITALL);
		//		while (dataLength < msgSize)
		//		{
		//			dataLength += recv(communicationSocket_, &buffer[dataLength], msgSize, 0);
		//		}
		//		sprintf_s(debugStr, "WAITALL: dataLength = %d", dataLength);
		//		MessageBoxA(NULL, debugStr, "", 0);
		//		if (dataLength!=msgSize)
		//		{
		//			sprintf_s(debugStr, "dataLength = %d, msgSize = %d", dataLength, msgSize);
		//			MessageBoxA(NULL, debugStr, "", 0);
		//		}
		//	}
		//}
	}
	else
	{
		infoLen = sizeof(clientAddressInfo_);
		if(computerFunction_ == COMPUTER_FUNCTION_SERVER)
			dataLength = recvfrom(communicationSocket_, buffer, bufferLength, 0, (struct sockaddr*)(&clientAddressInfo_), &infoLen);
		else
			dataLength = recvfrom(communicationSocket_, buffer, bufferLength, 0, (struct sockaddr*)(&serverAddressInfo_), &infoLen);
	}

	if(dataLength == SOCKET_ERROR)
	{
		//if(WSAGetLastError() == WSAETIMEDOUT)
		//throw runtime_error("Timeout reached");
		//char exceptionBuffer[32];

#ifdef WIN32
		lastError_ = WSAGetLastError();
#else
		sprintf_s(exceptionBuffer, "recv failed with error: %d\n", errno);
		lastError_ = errno;
#endif
		// HARD DEBUG
		char debugStr[250];
		sprintf_s(debugStr, "NetCtrl.receiveData - socket error: %d\n", lastError_);
		OutputDebugString(debugStr);

		if (lastError_ != WSAETIMEDOUT && lastError_ != 0)
		{
			sprintf_s(exceptionBuffer, "SOCKET ERROR! recv failed with error: %d\n", lastError_);
			closeSocket(communicationSocket_);

#ifdef WIN32
			WSACleanup();
#endif
			//throw runtime_error(exceptionBuffer);
			// HARD DEBUG
			sprintf_s(debugStr, "NetCtrl.receiveData: %s\n", exceptionBuffer);
			OutputDebugString(debugStr);
			//MessageBoxA(nullptr, exceptionBuffer, "", 0);
		}
	}
	//else
	//	printf("Received packet from %s:%d\n", inet_ntoa(serverAddressInfo_.sin_addr), ntohs(serverAddressInfo_.sin_port));

	return dataLength;
}

int NetworkControl::getLastError()
{
	return lastError_;
}
