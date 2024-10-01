#include "InputParameters.h"

using namespace std;

InputParameters::InputParameters()
{
  totCycles_ = 0;
  totData_ = 0;
}

void InputParameters::throwInvalidArgumentException(const char* programName)
{
  char exceptionBuffer[216];

  sprintf(exceptionBuffer,
          "Usage:\n"
          "  %s <tot_cycles> <tot_data>\n"
          "\n"
          "  tot_cycles  numero di cicli\n"
          "  tot_data    numero di dati da scrivere\n",
          programName);
  throw invalid_argument(exceptionBuffer);
}

void InputParameters::parseCommandLineParameters(int argc, char* argv[])
{
  if(argc != 3)
    throwInvalidArgumentException(argv[0]);

  totCycles_ = atoi(argv[1]);
  totData_ =  atoi(argv[2]);
}
