#include "DataChunk.h"

const unsigned int MAX_NUMBER_OF_DATA = 100;

unsigned int DataChunk::getMaxNumberOfData()
{
    return MAX_NUMBER_OF_DATA;
}

DataChunk::DataChunk()
{
    data_ = 0;
    clear();
}

DataChunk::~DataChunk()
{
    clear();
}

ERROR_CODE DataChunk::setTotData(unsigned int totData)
{
    if(totData > getMaxNumberOfData())
        return ERROR_CODE_PARAMETER_NOT_VALID;

    clear();

    totData_ = totData;
    data_ = new VariantDatum[totData_];

    return ERROR_CODE_OK;
}

unsigned int DataChunk::getTotData()
{
    return totData_;
}

ERROR_CODE DataChunk::setData(const VariantDatum &data, unsigned int index)
{
    if(index >= getTotData())
        return ERROR_CODE_PARAMETER_NOT_VALID;

    data_[index] = data;
    return ERROR_CODE_OK;
}

ERROR_CODE DataChunk::getData(VariantDatum& data, unsigned int index)
{
    if(index >= getTotData())
        return ERROR_CODE_PARAMETER_NOT_VALID;

    data = data_[index];

    return ERROR_CODE_OK;
}

ERROR_CODE DataChunk::getDataValue(DATA_BYTE* data, unsigned int index)
{
    ERROR_CODE ret;

    ret = data_[index].getData(data);

    return ret;
}

void DataChunk::readValuesFromFile(const char *fileName)
{
    FILE * file;
    file = fopen(fileName, "r");
    char prova[1024] = {0, };

    if (file == NULL)
      {
        printf("Failed to load the file with default values!\n");
        fclose (file);
      }

    unsigned int totId = 0;
    fscanf (file, "%d", &totId);
    fscanf (file, "%c", prova);

    setTotData(totId);

    for(unsigned int i=0; i<totId; i++)
    {
        unsigned int idFile_ = 0;
        fscanf (file, "%d", &idFile_);
        VariantDatum variantDatumDefault_;
        SYSTEM_ID id_ = static_cast<SYSTEM_ID>(idFile_);
        DataDescriptor dataDescriptorDefault_ = variantDatumDefault_.getDataDescriptorGivenID(id_);

        if(dataDescriptorDefault_.type_ == TYPEINT || dataDescriptorDefault_.type_ == TYPEUNSIGNEDINT)
        {
            int val;
            fscanf(file, "%d", &val);
            fscanf(file, "%c", prova);
            variantDatumDefault_.setData(id_, reinterpret_cast<DATA_BYTE*>(&val));
        }
        else if(dataDescriptorDefault_.type_ == TYPEDOUBLE)
        {
            float val;
            fscanf(file, "%f", &val);
            fscanf(file, "%c", prova);
            variantDatumDefault_.setData(id_, reinterpret_cast<DATA_BYTE*>(&val));
        }
        else if(dataDescriptorDefault_.type_ == TYPEBOOL)
        {
            char val;
            fscanf(file, "%c", &val);
            fscanf(file, "%c", prova);
            variantDatumDefault_.setData(id_, reinterpret_cast<DATA_BYTE*>(&val));
        }
        else if(dataDescriptorDefault_.type_ == TYPECHAR || dataDescriptorDefault_.type_ == TYPEUNSIGNEDCHAR || dataDescriptorDefault_.type_ == TYPESTRING)
        {
            DATA_BYTE val[1024] = {0, };
            fgets(val, 1024, file);
            variantDatumDefault_.setData(id_, val);
        }
        else
        {
            continue;
        }
        setData(variantDatumDefault_, i);
    }
}

void DataChunk::clear()
{
    if(data_)
        delete [] data_;
    data_ = 0;

    totData_ = 0;
}

void DataChunk::print()
{
    printf("TotData: %d\n", totData_);
    for (unsigned int i=0; i<totData_; i++)
    {
        printf("VariantDatum [%d]:\n", i);
        data_[i].print();
    }
}

void DataChunk::writeValuesOnFile(const char* fileName)
{

    FILE * file;
    file = fopen(fileName, "a");

    if (file == NULL)
      {
        printf("Failed to open the file for writing received data!\n");
        fclose (file);
      }

    fprintf(file, "\n -----------------\n");
    fprintf(file, "TotData: %d\n", totData_);
    for (unsigned int i=0; i<totData_; i++)
    {
        fprintf(file, "VariantDatum [%d]:\n", i);
        data_[i].writeValuesOnFile(file);
    }

    fclose(file);
}

