#ifndef CONSOLEAPP_H
#define CONSOLEAPP_H

#include <stdlib.h>
#include <stdio.h>
#include <stdexcept>
#include <DataChunk.h>
#include <opacimetro.h>
#include <eoptisdevicemanager.h>
#include "InputParameters.h"

using namespace eoptis_device_manager;

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
    void printSizes();
};

#endif // CONSOLEAPP_H
