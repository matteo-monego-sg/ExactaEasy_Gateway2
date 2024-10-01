#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <stdexcept>

#ifndef _INPUT_PARAMETERS_H
#define _INPUT_PARAMETERS_H

class InputParameters
{
public:
  char serverAddress_[16];
  unsigned int port_;
public:
	InputParameters();
  void parseCommandLineParameters(int argc, char* args[]);
private:
  void throwInvalidArgumentException(const char* programName);
};

#endif
