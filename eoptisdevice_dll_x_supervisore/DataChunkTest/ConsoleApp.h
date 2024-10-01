#ifndef CONSOLEAPP_H
#define CONSOLEAPP_H

#include <stdlib.h>
#include <stdio.h>
#include <stdexcept>
#include <string.h>
#include <DataChunk.h>
#include "InputParameters.h"

struct DefaultValues
{
    unsigned int infoserialnumber_;
    char infofirmwareversion_[32];
    char infosdkversion_[32];
    char infoipaddress_[32];
    unsigned int infoport_;
    unsigned int infofpgaversion_;
    DefaultValues()
    {
        infoserialnumber_ = 123456;
        sprintf(infofirmwareversion_, "1.0.0.0");
        sprintf(infosdkversion_, "1.0.0.0");
        sprintf(infoipaddress_, "192.168.0.102");
        infoport_ = 1234;
        infofpgaversion_ = 123456;
    }
};

class ConsoleApp
{
  public:
    ConsoleApp();
    ~ConsoleApp();
    int main(int argc, char* argv[]);

  private:
    InputParameters inputParameters_;
  private:
    void clear();
};

#endif // CONSOLEAPP_H
