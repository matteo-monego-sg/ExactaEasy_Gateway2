#ifndef EOPTISDEVICEMANAGER_GLOBAL_H
#define EOPTISDEVICEMANAGER_GLOBAL_H

//#include <QtCore/qglobal.h>

#if defined(EOPTISDEVICEMANAGER_LIBRARY)
#  define EOPTISDEVICEMANAGERSHARED_EXPORT Q_DECL_EXPORT
#else
#  define EOPTISDEVICEMANAGERSHARED_EXPORT Q_DECL_IMPORT
#endif

#ifdef WIN32
#define CALL_CONV __stdcall
#else
#define CALL_CONV
#endif


struct VariantDatumStruct
{
    SYSTEM_ID id_;
    unsigned int length_;
    SYSTEM_TYPE type_;
    DATA_BYTE* data_;

    void printTypeMeasure()
    {
        short* pShort = reinterpret_cast<short*>(data_);
        int* pInt = reinterpret_cast<int*>(data_);
        for(unsigned int i=0; i<50; i++)
        {
            printf("[%d] value: 0x%02X ------ %d [int]\n", i, pShort[i], pInt[i]);
        }
    }

    void printTypeShortBuffer()
    {
        short* pShort = reinterpret_cast<short*>(data_);
        int* pInt = reinterpret_cast<int*>(data_);
        DataDescriptor measureDataDescriptor = VariantDatum::getDataDescriptorGivenID(id_);

        for(unsigned int i=0; i<measureDataDescriptor.length_; i++)
        {
            printf("[%d] value: 0x%02X ------ %d [int]\n", i, pShort[i], pInt[i]);
        }
    }

    void printType(SYSTEM_TYPE type)
    {
        switch (type) {
        case TYPEINT:
            printf("TYPE: INT\n");
            break;
        case TYPEUNSIGNEDINT:
            printf("TYPE: UNSIGNED INT\n");
            break;
        case TYPECHAR:
            printf("TYPE: CHAR\n");
            break;
        case TYPEUNSIGNEDCHAR:
            printf("TYPE: UNSIGNED CHAR\n");
            break;
        case TYPEFLOAT:
            printf("TYPE: FLOAT\n");
            break;
        case TYPEDOUBLE:
            printf("TYPE: DOUBLE\n");
            break;
        case TYPEBOOL:
            printf("TYPE: BOOL\n");
            break;
        case TYPELONG:
            printf("TYPE: LONG\n");
            break;
        case TYPEUNSIGNEDLONG:
            printf("TYPE: UNSIGNED LONG\n");
            break;
        case TYPESTRING:
            printf("TYPE: STRING\n");
            break;
        case TYPEMEASURE:
            printf("TYPE: MEASURE\n");
            break;
        case TYPESHORT:
            printf("TYPE: SHORT\n");
            break;
        default:
            break;
        }
    }

    void print()
    {
        printf("ID: %d\n", static_cast<unsigned int>(id_));
        printType(type_);
        printf("LENGTH: %d BYTE\n", static_cast<unsigned int>(length_));
        switch (type_) {
        case TYPEBOOL:
            printf("DATA: %d\n", *(reinterpret_cast<bool*>(data_)));
            break;
        case TYPEINT:
            printf("DATA: %d\n", *(reinterpret_cast<int*>(data_)));
            break;
        case TYPEUNSIGNEDINT:
            printf("DATA: %d\n", *(reinterpret_cast<unsigned int*>(data_)));
            break;
        case TYPECHAR:
            printf("DATA: %c\n", *(reinterpret_cast<char*>(data_)));
            break;
        case TYPEUNSIGNEDCHAR:
            printf("DATA: 0x%02X\n", *(reinterpret_cast<unsigned char*>(data_)));
            break;
        case TYPEDOUBLE:
            printf("DATA: %f\n", *(reinterpret_cast<double*>(data_)));
            break;
        case TYPEFLOAT:
            printf("DATA: %f\n", *(reinterpret_cast<float*>(data_)));
            break;
        case TYPELONG:
            printf("DATA: %ld\n", *(reinterpret_cast<long*>(data_)));
            break;
        case TYPEUNSIGNEDLONG:
            printf("DATA: %lu\n", *(reinterpret_cast<unsigned long*>(data_)));
            break;
        case TYPESTRING:
            printf("DATA: %s\n", reinterpret_cast<char*>(data_));
            break;
        case TYPESHORT:
            printf("DATA: 0x%02X ------ %d [int]\n", *(reinterpret_cast<short*>(data_)), *(reinterpret_cast<int*>(data_)));
            break;
        case TYPEMEASURE:
            printTypeMeasure();
            break;
        case TYPESHORTBUFFER:
            printTypeShortBuffer();
            break;
        default:
            printf("Unknown data!\n");
            break;
        }
    }
};

struct DataChunkStruct
{
    unsigned int totData_;
    VariantDatumStruct* data_;

    void print()
    {
        printf("TotData: %d\n", totData_);
        for (unsigned int i=0; i<totData_; i++)
        {
            printf("VariantDatum [%d]:\n", i);
            data_[i].print();
        }
    }

    void clear()
    {
        if (totData_>0 && data_!=nullptr)
        {
            for (unsigned int i=0; i<totData_; i++)
            {
                VariantDatumStruct* currVDS = &data_[i];
                if(currVDS->data_ != nullptr)
                    delete [] currVDS->data_;
                currVDS->data_ = nullptr;
            }
        }
        delete []data_;
        data_ = nullptr;
    }
};



#endif // EOPTISDEVICEMANAGER_GLOBAL_H
