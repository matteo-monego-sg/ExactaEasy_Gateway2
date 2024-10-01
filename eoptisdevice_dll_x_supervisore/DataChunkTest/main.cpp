#include "main.h"

using namespace std;

int main(int argc, char *argv[])
{
  int ret;
  ConsoleApp consoleapp;

  try
  {
    ret = consoleapp.main(argc, argv);
  }
  catch(exception& e)
  {
    //SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), FOREGROUND_RED);
    printf("An exception occured:\n%s\n", e.what());
    //SetConsoleTextAttribute(GetStdHandle(STD_OUTPUT_HANDLE), FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE);
    return -1;
  }

  return ret;
}
