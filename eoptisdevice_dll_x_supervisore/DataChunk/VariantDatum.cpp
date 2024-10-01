#include "VariantDatum.h"

using namespace std;

DataDescriptor VariantDatum::getDataDescriptorGivenID(SYSTEM_ID id)
{
    DataDescriptor dataDescriptor;

    switch(id)
    {
        case INFO_SERIAL_NUMBER:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
        case INFO_FIRMWARE_VERSION:
            dataDescriptor.type_ = TYPESTRING;
            dataDescriptor.length_ = 32*sizeof(char);
            break;
        case INFO_SDK_VERSION:
            dataDescriptor.type_ = TYPESTRING;
            dataDescriptor.length_ = 32*sizeof(char);
            break;
        case INFO_IP_ADDRESS:
            dataDescriptor.type_ = TYPESTRING;
            dataDescriptor.length_ = 32*sizeof(char);
            break;
        case INFO_PORT:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
        case INFO_FPGA_VERSION:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
        case INFO_OFFLINE:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
        break;
        case PAR_SPINDLE_COUNT:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;
        case PAR_WORKING_MODE:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;
        case PAR_CURRENT_INPUT_DWORD:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
        case PAR_CURRENT_OUTPUT_DWORD:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
        case PAR_REJECT_PATTERN_CODE_1:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
        case PAR_REJECT_COUNT_CODE_1:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
        case PAR_REJECT_PATTERN_CODE_2:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
        case PAR_REJECT_COUNT_CODE_2:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
        case PAR_STOP_ON_CONDITION_MODE:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;
        case PAR_STOP_ON_CONDITION_SPINDLE:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;
        case PAR_CALIBRATION_TABLE:                     //da aggiustare, array di caratteri conta fino ad un certo punto
            dataDescriptor.type_ = TYPECHAR;
            dataDescriptor.length_ = 244*sizeof(char);
            break;
        case PAR_CHANNEL_2_ACTIVE:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case PAR_CHANNEL_1_ACTIVE:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case PAR_SENSOR_2_STATUS:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case PAR_LASER_2_STATUS:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case PAR_SENSOR_1_STATUS:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case PAR_LASER_1_STATUS:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case PAR_LED_RED_POWER:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case PAR_LED_GREEN_POWER:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case PAR_LED_RED_CHANNEL_2:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case PAR_LED_GREEN_CHANNEL_2:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case PAR_LED_RED_CHANNEL_1:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case PAR_LED_GREEN_CHANNEL_1:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case PAR_ENABLE_LASER_2:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case PAR_ENABLE_LASER_1:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case PAR_GAIN_CHANNEL_2_HIGH:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case PAR_GAIN_CHANNEL_2_LOW:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case PAR_GAIN_CHANNEL_1_HIGH:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case PAR_GAIN_CHANNEL_1_LOW:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case PAR_LASER_EXPOTIME:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
        case PAR_VIAL_EXPOTIME:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;

            //PARAMETRO RELATIVO AD UN PACCHETTO DI 50 VALORI

        case PAR_MEASURE_50_VALUES:
            dataDescriptor.type_ = TYPEMEASURE;
            dataDescriptor.length_ = 50*sizeof(short);
            break;

            //PARAMETRI RELATIVI AL CONTEGGIO DEI MANDRINI

        case PAR_0_TORRETTA_OFFSET:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
        case PAR_INCREMENTO_SPINDLE_ID:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
        case PAR_VIAL_SPINDLE_NUMBER:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
        case PAR_FREE_RUN_TIMER:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
		case PAR_RECORDING_MODE:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;
		case PAR_RECORDING_TO_SAVE:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(int);
            break;
		case PAR_RECORDING_SAVED:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(int);
            break;
        case PAR_ACQUISITION_MODE:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;

            //STATUS

        case ST_UNAVAILABLE:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case ST_AVAILABLE:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case ST_INITIALIZING:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case ST_READY:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case ST_ERROR:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;

            //WORKING MODE

        case WM_NONE:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case WM_CONTROL:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case WM_FREE_RUN:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case WM_MECHANICAL_TEST:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;

            //PROCESSING MODE

        case PM_NORMAL:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case PM_GOING_TO_STOP_ON_CONDITION:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case PM_STOP_ON_CONDITION:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;

            //GPINPUT

        case IN_TRIGGER:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case IN_ZERO_TORRETTA:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case IN_VIAL_ID_0:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case IN_VIAL_ID_1:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;

            //GPOUTPUT

        case OUT_READY:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case OUT_ACQ_READY:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case OUT_VIAL_ID_0:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case OUT_VIAL_ID_1:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case OUT_DATA_VALID:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case OUT_REJECT_1:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case OUT_REJECT_2:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case OUT_REJECT_3:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;

            //CALIBRATION TABLE

        case CAL_SPLINE_REGION_USED_COUNT:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;
        case CAL_A0_REGION0:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_B0_REGION0:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_C0_REGION0:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_D0_REGION0:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_X0_LOWER:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_X0_UPPER:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_A1_REGION1:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_B1_REGION1:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_C1_REGION1:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_D1_REGION1:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_X1_LOWER:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_X1_UPPER:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_A2_REGION2:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_B2_REGION2:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_C2_REGION2:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_D2_REGION2:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_X2_LOWER:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_X2_UPPER:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_A3_REGION3:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_B3_REGION3:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_C3_REGION3:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_D3_REGION3:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_X3_LOWER:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_X3_UPPER:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_A4_REGION4:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_B4_REGION4:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_C4_REGION4:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_D4_REGION4:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_X4_LOWER:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case CAL_X4_UPPER:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;

            //ID RELATIVI AI CALCOLI DEL LOCKIN

        case LOCKIN_SAMPLES_PER_PERIOD:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
        case LOCKIN_VIAL_PERIODS:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
        case LOCKIN_LASER_PERIODS:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
        case LOCKIN_VIAL_AMPLITUDE_RAW:
            dataDescriptor.type_ = TYPEFLOAT;
            dataDescriptor.length_ = sizeof(float);
            break;
        case LOCKIN_LASER_AMPLITUDE_RAW:
            dataDescriptor.type_ = TYPEFLOAT;
            dataDescriptor.length_ = sizeof(float);
            break;
        case LOCKIN_AMPLITUDE_FACTOR_COMPENSATED:
            dataDescriptor.type_ = TYPEFLOAT;
            dataDescriptor.length_ = sizeof(float);
            break;
        case LOCKIN_AMPLITUDE_POST_CALIBRATION:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;

            //STOP ON CONDITION

        case STOP_ON_CONDITION_FULL_VALUE_BUFFER:
            dataDescriptor.type_ = TYPESHORTBUFFER;
            dataDescriptor.length_ = 115000*sizeof(short);
            break;
        case STOP_ON_CONDITION_REAL_VALUE_BUFFER:
            dataDescriptor.type_ = TYPESHORTBUFFER;
            dataDescriptor.length_ = 29000*sizeof(short);      //29000 = circa (500+80)*50
            break;

            //DATI RELATIVI ALL'ANALISI

        case ANALYSIS_VIAL_ID_COUNTER:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
        case ANALYSIS_IS_REJECT:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case ANALYSIS_REJECTION_CAUSE_NUMBER:
            dataDescriptor.type_ = TYPEUNSIGNEDCHAR;
            dataDescriptor.length_ = sizeof(unsigned char);
            break;

            //DIAGNOSTIC

        case DIAGNOSTIC_FULL_REGISTER_SL_STATUS:
            dataDescriptor.type_ = TYPEUNSIGNEDCHAR;
            dataDescriptor.length_ = sizeof(unsigned char);
            break;
        case DIAGNOSTIC_FULL_REGISTER_LED_STATUS:
            dataDescriptor.type_ = TYPEUNSIGNEDCHAR;
            dataDescriptor.length_ = sizeof(unsigned char);
            break;
        case DIAGNOSTIC_FULL_REGISTER_GPOUTPUT:
            dataDescriptor.type_ = TYPEUNSIGNEDCHAR;
            dataDescriptor.length_ = sizeof(unsigned char);
            break;
        case DIAGNOSTIC_FULL_REGISTER_GPINPUT:
            dataDescriptor.type_ = TYPEUNSIGNEDCHAR;
            dataDescriptor.length_ = sizeof(unsigned char);
            break;
        case DIAGNOSTIC_FULL_REGISTER_LASER_CTRL:
            dataDescriptor.type_ = TYPEUNSIGNEDCHAR;
            dataDescriptor.length_ = sizeof(unsigned char);
            break;
        case DIAGNOSTIC_FULL_REGISTER_SENS_CTRL:
            dataDescriptor.type_ = TYPEUNSIGNEDCHAR;
            dataDescriptor.length_ = sizeof(unsigned char);
            break;

            //GAIN PARAMETERS
        case PAR_VIAL_GAIN:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;
        case PAR_LASER_GAIN:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;

            //BROADCAST
        case BROADCAST_MESSAGE:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;

            //ANALYSIS RESPONSE
        case ANALYSIS_RESPONSE:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;

            //THRESHOLD
        case TH_WARNING_UPPER:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case TH_WARNING_LOWER:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case TH_ERROR_UPPER:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;
        case TH_ERROR_LOWER:
            dataDescriptor.type_ = TYPEDOUBLE;
            dataDescriptor.length_ = sizeof(double);
            break;

            //parametri aggiunti per il progetto COLORIMETRO (07/07/15)

        case PAR_SM_STATUS_ACQ:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case PAR_SM_STATUS_READY:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case IN_PRESENCE:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
        case INFO_LOOK_UP_TABLE:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;
        case PAR_TINT_VIAL:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
        case PAR_TINT_BACKGROUND:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
        case PAR_FULL_SPECTRUM_BUFFER:
            dataDescriptor.type_ = TYPEUNSIGNEDSHORTBUFFER;
            dataDescriptor.length_ = 400*sizeof(unsigned short);
            break;
        case PAR_REFERENCE_SPECTRUM_VALUES:
            dataDescriptor.type_ = TYPEUNSIGNEDSHORTBUFFER;
            dataDescriptor.length_ = 400*sizeof(unsigned short);
            break;
        case PAR_BACKGROUND_SPECTRUM_VALUES:
            dataDescriptor.type_ = TYPEUNSIGNEDSHORTBUFFER;
            dataDescriptor.length_ = 400*sizeof(unsigned short);
            break;
        case PAR_CALIBRATION_REFERENCE:
            dataDescriptor.type_ = TYPESTRING;
            dataDescriptor.length_ = 100*sizeof(char);
            break;
        case PAR_CALIBRATION_BACKGROUND:
            dataDescriptor.type_ = TYPESTRING;
            dataDescriptor.length_ = 100*sizeof(char);
            break;
        case PAR_REFERENCE_ACTUAL_CALIBRATION:
            dataDescriptor.type_ = TYPESTRING;
            dataDescriptor.length_ = 100*sizeof(char);
            break;
        case PAR_BACKGROUND_ACTUAL_CALIBRATION:
            dataDescriptor.type_ = TYPESTRING;
            dataDescriptor.length_ = 100*sizeof(char);
            break;

            //aggiunti dopo documento specifiche 24/08/2015

        case PAR_RANGE_1_L:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
        case PAR_RANGE_1_H:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
        case PAR_RANGE_1_SIGN:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;
        case PAR_RANGE_1_TH:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;
        case PAR_RANGE_1_PEAK_REJECT_TH:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;
        case PAR_RANGE_2_L:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
        case PAR_RANGE_2_H:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
        case PAR_RANGE_2_SIGN:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;
        case PAR_RANGE_2_TH:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;
        case PAR_RANGE_2_PEAK_REJECT_TH:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;
        case PAR_RANGE_3_L:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
        case PAR_RANGE_3_H:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
        case PAR_RANGE_3_SIGN:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;
        case PAR_RANGE_3_TH:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;
        case PAR_RANGE_3_PEAK_REJECT_TH:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;
        case PAR_RANGE_4_L:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
        case PAR_RANGE_4_H:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
        case PAR_RANGE_4_SIGN:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;
        case PAR_RANGE_4_TH:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;
        case PAR_RANGE_4_PEAK_REJECT_TH:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;
        case PAR_ALL_RANGE_SUM_REJECT:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;

        case PAR_FULL_SPECTRUM_MIS:
            dataDescriptor.type_ = TYPEUNSIGNEDSHORTBUFFER;
            dataDescriptor.length_ = 400*sizeof(unsigned short);
            break;
        case PAR_FULL_SPECTRUM_ELAB:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = 400*sizeof(int);
            break;
        case PAR_FULL_SPECTRUM_REF_NORM:
            dataDescriptor.type_ = TYPEUNSIGNEDSHORTBUFFER;
            dataDescriptor.length_ = 400*sizeof(unsigned short);
            break;
        case PAR_FULL_SPECTRUM_BACK_NORM:
            dataDescriptor.type_ = TYPEUNSIGNEDSHORTBUFFER;
            dataDescriptor.length_ = 400*sizeof(unsigned short);
            break;

        case PAR_REF_SPECTRUM_FROM_CLIENT:
            dataDescriptor.type_ = TYPEUNSIGNEDSHORTBUFFER;
            dataDescriptor.length_ = 400*sizeof(unsigned short);
            break;
        case PAR_REF_SPECTRUM_FROM_CLIENT_FILENAME:
            dataDescriptor.type_ = TYPESTRING;
            dataDescriptor.length_ = 64*sizeof(char);
            break;
        case PAR_REF_SPECTRUM_FROM_CLIENT_OVERWRITE:
            dataDescriptor.type_ = TYPEBOOL;
            dataDescriptor.length_ = sizeof(bool);
            break;
		case PAR_RANGE_1_MEAN:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;
        case PAR_RANGE_2_MEAN:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;
        case PAR_RANGE_3_MEAN:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;
        case PAR_RANGE_4_MEAN:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;
		case PAR_ALL_RANGE_MEAN:
            dataDescriptor.type_ = TYPEINT;
            dataDescriptor.length_ = sizeof(int);
            break;
		case LIGHT_CONTROL:
        case LIGHT_CONTROL_TH:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
		case INITIAL_WAVELENGTH:
        case WAVELENGTH_INCREMENT:
		case FINAL_WAVELENGTH:
            dataDescriptor.type_ = TYPEUNSIGNEDINT;
            dataDescriptor.length_ = sizeof(unsigned int);
            break;
        default:
            dataDescriptor.type_ = TYPEUNKNOWN;
            dataDescriptor.length_ = 0;
            break;
    }

    return dataDescriptor;
}

VariantDatum::VariantDatum()
{
    data_ = 0;
    clear();
}

VariantDatum::~VariantDatum()
{
    clear();
}

VariantDatum &VariantDatum::operator=(const VariantDatum &data)
{
    clear();

    id_ = data.id_;
    type_ = data.type_;
    length_ = data.length_;
    data_ = new DATA_BYTE[length_];
    memcpy(data_, data.data_, length_);

    return *this;
}

ERROR_CODE VariantDatum::setData(SYSTEM_ID id, DATA_BYTE* data)
{
    clear();

    DataDescriptor dataDescriptor;

    dataDescriptor = getDataDescriptorGivenID(id);
    if(dataDescriptor.type_ == TYPEUNKNOWN || !(dataDescriptor.length_ > 0))
        return ERROR_CODE_ID_NOT_VALID;

    type_ = dataDescriptor.type_;
    length_ = dataDescriptor.length_;
    id_ = id;
    data_ = new DATA_BYTE[length_];
    memcpy(data_, data, length_);

    return ERROR_CODE_OK;
}

SYSTEM_ID VariantDatum::getID()
{
    return id_;
}

unsigned int VariantDatum::getLength()
{
    return length_;
}

SYSTEM_TYPE VariantDatum::getType()
{
    return type_;
}

ERROR_CODE VariantDatum::getData(DATA_BYTE *data)
{
    try
    {
        memcpy(data, data_, length_);
    }
    catch(...)
    {
        return ERROR_CODE_GENERIC_FAILURE;
    }
    return ERROR_CODE_OK;
}

void VariantDatum::clear()
{
    type_ = TYPEUNKNOWN;
    length_ = 0;
    id_ = SYSTEM_ID_UNKNOWN;
    if(data_)
        delete [] data_;
    data_ = 0;
}

void VariantDatum::print()
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
    case TYPEUNSIGNEDSHORTBUFFER:
        printTypeUnsignedShortBuffer();
        break;
    default:
        printf("Unknown data!\n");
        break;
    }
}

void VariantDatum::printTypeMeasure()
{
    short* pShort = reinterpret_cast<short*>(data_);
    int* pInt = reinterpret_cast<int*>(data_);
    DataDescriptor measureDataDescriptor = VariantDatum::getDataDescriptorGivenID(id_);

    for(unsigned int i=0; i<measureDataDescriptor.length_; i++)
    {
        printf("[%d] value: 0x%02X ------ %d [int]\n", i, pShort[i], pInt[i]);
    }
}

void VariantDatum::printTypeShortBuffer()
{
    short* pShort = reinterpret_cast<short*>(data_);
    int* pInt = reinterpret_cast<int*>(data_);
    DataDescriptor measureDataDescriptor = VariantDatum::getDataDescriptorGivenID(id_);

    unsigned int dimension = (measureDataDescriptor.length_)/(sizeof(short));

    //for(unsigned int i=0; i<measureDataDescriptor.length_; i++)
    for(unsigned int i=0; i<dimension; i++)
    {
        printf("[%d] value: 0x%02X ------ %d [int]\n", i, pShort[i], pInt[i]);
    }
}

void VariantDatum::printTypeUnsignedShortBuffer()
{
    unsigned short* pShort = reinterpret_cast<unsigned short*>(data_);
    int* pInt = reinterpret_cast<int*>(data_);
    DataDescriptor measureDataDescriptor = VariantDatum::getDataDescriptorGivenID(id_);

    unsigned int dimension = (measureDataDescriptor.length_)/(sizeof(unsigned short));

    //for(unsigned int i=0; i<measureDataDescriptor.length_; i++)
    for(unsigned int i=0; i<dimension; i++)
    {
        printf("[%d] value: 0x%02X ------ %d [int]\n", i, pShort[i], pInt[i]);
    }
}

void VariantDatum::printType(SYSTEM_TYPE type)
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
    case TYPESHORTBUFFER:
        printf("TYPE: SHORT BUFFER\n");
        break;
    case TYPEUNSIGNEDSHORTBUFFER:
        printf("TYPE: UNSIGNED SHORT BUFFER");
        break;
    default:
        break;
    }
}

void VariantDatum::writeValuesOnFile(FILE* file)
{

    fprintf(file, "ID: %d\n", static_cast<unsigned int>(id_));
    printTypeOnFile(type_, file);
    fprintf(file, "LENGTH: %d BYTE\n", static_cast<unsigned int>(length_));
    switch (type_) {
    case TYPEBOOL:
        fprintf(file, "DATA: %d\n", *(reinterpret_cast<bool*>(data_)));
        break;
    case TYPEINT:
        fprintf(file, "DATA: %d\n", *(reinterpret_cast<int*>(data_)));
        break;
    case TYPEUNSIGNEDINT:
        fprintf(file, "DATA: %d\n", *(reinterpret_cast<unsigned int*>(data_)));
        break;
    case TYPECHAR:
        fprintf(file, "DATA: %c\n", *(reinterpret_cast<char*>(data_)));
        break;
    case TYPEUNSIGNEDCHAR:
        fprintf(file, "DATA: 0x%02X\n", *(reinterpret_cast<unsigned char*>(data_)));
        break;
    case TYPEDOUBLE:
        fprintf(file, "DATA: %f\n", *(reinterpret_cast<double*>(data_)));
        break;
    case TYPEFLOAT:
        fprintf(file, "DATA: %f\n", *(reinterpret_cast<float*>(data_)));
        break;
    case TYPELONG:
        fprintf(file, "DATA: %ld\n", *(reinterpret_cast<long*>(data_)));
        break;
    case TYPEUNSIGNEDLONG:
        fprintf(file, "DATA: %lu\n", *(reinterpret_cast<unsigned long*>(data_)));
        break;
    case TYPESTRING:
        fprintf(file, "DATA: %s\n", reinterpret_cast<char*>(data_));
        break;
    case TYPESHORT:
        fprintf(file, "DATA: 0x%02X ------ %d [int]\n", *(reinterpret_cast<short*>(data_)), *(reinterpret_cast<int*>(data_)));
        break;
    case TYPEMEASURE:
        printTypeMeasureOnFile(file);
        break;
    case TYPESHORTBUFFER:
        printTypeShortBufferOnFile(file);
        break;
    case TYPEUNSIGNEDSHORTBUFFER:
        printTypeUnsignedShortBufferOnFile(file);
        break;
    default:
        printf("Unknown data!\n");
        break;
    }
}

void VariantDatum::printTypeOnFile(SYSTEM_TYPE type, FILE* file)
{
    switch (type) {
    case TYPEINT:
        fprintf(file, "TYPE: INT\n");
        break;
    case TYPEUNSIGNEDINT:
        fprintf(file, "TYPE: UNSIGNED INT\n");
        break;
    case TYPECHAR:
        fprintf(file, "TYPE: CHAR\n");
        break;
    case TYPEUNSIGNEDCHAR:
        fprintf(file, "TYPE: UNSIGNED CHAR\n");
        break;
    case TYPEFLOAT:
        fprintf(file, "TYPE: FLOAT\n");
        break;
    case TYPEDOUBLE:
        fprintf(file, "TYPE: DOUBLE\n");
        break;
    case TYPEBOOL:
        fprintf(file, "TYPE: BOOL\n");
        break;
    case TYPELONG:
        fprintf(file, "TYPE: LONG\n");
        break;
    case TYPEUNSIGNEDLONG:
        fprintf(file, "TYPE: UNSIGNED LONG\n");
        break;
    case TYPESTRING:
        fprintf(file, "TYPE: STRING\n");
        break;
    case TYPEMEASURE:
        fprintf(file, "TYPE: MEASURE\n");
        break;
    case TYPESHORT:
        fprintf(file, "TYPE: SHORT\n");
        break;
    case TYPESHORTBUFFER:
        fprintf(file, "TYPE: SHORT BUFFER\n");
        break;
    case TYPEUNSIGNEDSHORTBUFFER:
        fprintf(file, "TYPE: UNSIGNED SHORT BUFFER\n");
        break;
    default:
        break;
    }
}

void VariantDatum::printTypeMeasureOnFile(FILE* file)
{
    short* pShort = reinterpret_cast<short*>(data_);
    int* pInt = reinterpret_cast<int*>(data_);
    DataDescriptor measureDataDescriptor = VariantDatum::getDataDescriptorGivenID(id_);

    for(unsigned int i=0; i<measureDataDescriptor.length_; i++)
    {
        fprintf(file, "[%d] value: 0x%02X ------ %d [int]\n", i, pShort[i], pInt[i]);
    }
}

void VariantDatum::printTypeShortBufferOnFile(FILE* file)
{
    short* pShort = reinterpret_cast<short*>(data_);
    int* pInt = reinterpret_cast<int*>(data_);
    DataDescriptor measureDataDescriptor = VariantDatum::getDataDescriptorGivenID(id_);

    for(unsigned int i=0; i<measureDataDescriptor.length_; i++)
    {
        fprintf(file, "[%d] value: 0x%02X ------ %d [int]\n", i, pShort[i], pInt[i]);
    }
}

void VariantDatum::printTypeUnsignedShortBufferOnFile(FILE* file)
{
    unsigned short* pShort = reinterpret_cast<unsigned short*>(data_);
    int* pInt = reinterpret_cast<int*>(data_);
    DataDescriptor measureDataDescriptor = VariantDatum::getDataDescriptorGivenID(id_);

    for(unsigned int i=0; i<measureDataDescriptor.length_; i++)
    {
        fprintf(file, "[%d] value: 0x%02X ------ %d [int]\n", i, pShort[i], pInt[i]);
    }
}
