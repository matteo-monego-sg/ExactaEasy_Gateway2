#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <stdexcept>

#ifndef _INPUT_PARAMETERS_H
#define _INPUT_PARAMETERS_H

class InputParameters
{
public:
    unsigned int totData_;
    unsigned int totCycles_;
public:
    InputParameters();
    void parseCommandLineParameters(int argc, char* args[]);
private:
  void throwInvalidArgumentException(const char* programName);
};

#endif
