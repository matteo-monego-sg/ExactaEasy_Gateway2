#include "InputParameters.h"

using namespace std;

InputParameters::InputParameters()
{
  memset(serverAddress_, 0, sizeof(serverAddress_));
  port_ = 0;
}

void InputParameters::throwInvalidArgumentException(const char* programName)
{
  char exceptionBuffer[216];

  sprintf(exceptionBuffer,
          "Usage:\n"
          "  %s <port> <serverAddress>\n"
          "\n"
          "  port: inserire il valore della porta di comunicazione (solitamente 23456)\n"
          "  serverAddress\t\tMust specify server IP in client mode\n",
          programName);
  throw invalid_argument(exceptionBuffer);
}

void InputParameters::parseCommandLineParameters(int argc, char* argv[])
{
  if(argc != 3)
    throwInvalidArgumentException(argv[0]);

  port_ = atoi(argv[1]);

  bool validIP = strlen(argv[2]) < 16;
  if(!validIP)
    throwInvalidArgumentException(argv[0]);

  sprintf(serverAddress_, "%s", argv[2]);
}
