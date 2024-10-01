#include "ConsoleApp.h"

using namespace std;

DefaultValues DEFAULT_VALUES;

ConsoleApp::ConsoleApp()
{
  clear();
}

ConsoleApp::~ConsoleApp()
{
  clear();
}

int ConsoleApp::main(int argc, char* argv[])
{
  inputParameters_.parseCommandLineParameters(argc, argv);

  clear();

  VariantDatum variantDatumProva;
  unsigned int val;
  ERROR_CODE ret;

  for(unsigned int c = 0; c < inputParameters_.totCycles_; c++)
  {
      printf("Cycle %d\n", c);

      DataChunk dataChunkProva;
      printf("Create %d data... ", inputParameters_.totData_);
      ret = dataChunkProva.setTotData(inputParameters_.totData_);
      if(ret != ERROR_CODE_OK)
      {
          printf("ERROR setting tot data! (%d)\n", ret);
          return -1;
      }

      if(dataChunkProva.getTotData() != inputParameters_.totData_)
      {
          printf("ERROR! (%d)\n", dataChunkProva.getTotData());
          return -1;
      }
      else
        printf("DONE\n");

      printf("Fill data... ");
      for(unsigned int i = 0; i < dataChunkProva.getTotData(); i++)
      {
          val = i*1000;
          ret = variantDatumProva.setData(INFO_SERIAL_NUMBER, reinterpret_cast<DATA_BYTE*>(&val));
          if(ret != ERROR_CODE_OK)
          {
              printf("ERROR setting VariantDatum data! (%d)\n", ret);
              return -1;
          }
          ret = dataChunkProva.setData(variantDatumProva, i);
          if(ret != ERROR_CODE_OK)
          {
              printf("ERROR setting DataChunk data! (%d)\n", ret);
              return -1;
          }
      }
      printf("DONE\n");

      printf("Check data...\n");
      for(unsigned int i = 0; i < dataChunkProva.getTotData(); i++)
      {
          dataChunkProva.getData(variantDatumProva, i);
          if(ret != ERROR_CODE_OK)
          {
              printf("ERROR getting DataChunk data! (%d)\n", ret);
              return -1;
          }
          variantDatumProva.getData(reinterpret_cast<DATA_BYTE*>(&val));
          if(ret != ERROR_CODE_OK)
          {
              printf("ERROR getting VarinatDatum data! (%d)\n", ret);
              return -1;
          }

          if(val != i*1000)
          {
              printf("WRONG DATA! (%d = %d)\n", i, val);
              return -2;
          }
      }
      printf("DONE\n");
  }

  DataChunk dataChunkFile;
  dataChunkFile.readValuesFromFile("defaultIdWithValues.txt");

  dataChunkFile.print();

  printf("\nTest terminated succesfully!\n");

  return 0;
}

void ConsoleApp::clear()
{

}
