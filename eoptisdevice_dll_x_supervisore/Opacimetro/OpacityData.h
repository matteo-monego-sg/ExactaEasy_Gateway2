#ifndef OPACITYDATA_H
#define OPACITYDATA_H

/*

typedef enum {
    TYPEINT,
    TYPEUNSIGNEDINT,
    TYPECHAR,
    TYPEUNSIGNEDCHAR,
    TYPEFLOAT,
    TYPEDOUBLE,
    TYPEBOOL,
    TYPELONG,
    TYPEUNSIGNEDLONG,
} SYSTEM_TYPE;

*/


/*
typedef enum  //lista dei parametri utilizzabili nel sw e relativo ID da utilizzare
{
    INFO_SERIAL_NUMBER = 200,
    INFO_FIRMWARE_VERSION,
    INFO_SDK_VERSION,
    INFO_IP_ADDRESS,
    INFO_PORT,
    INFO_FPGA_VERSION,
    PAR_SPINDLE_COUNT = 300,
    PAR_WORKING_MODE,
    PAR_CURRENT_INPUT_DWORD,
    PAR_CURRENT_OUTPUT_DWORD,
    PAR_REJECT_PATTERN_CODE_1,
    PAR_REJECT_COUNT_CODE_1,
    PAR_REJECT_PATTERN_CODE_2,
    PAR_REJECT_COUNT_CODE_2,
    PAR_STOP_ON_CONDITION_MODE,
    PAR_STOP_ON_CONDITION_SPINDLE,
    PAR_CALIBRATION_TABLE,
    PAR_CHANNEL_2_ACTIVE,           //parametri del sistema embedded - aggiunti 15/12/14 (311)
    PAR_CHANNEL_1_ACTIVE,
    PAR_SENSOR_2_STATUS,
    PAR_LASER_2_STATUS,
    PAR_SENSOR_1_STATUS,
    PAR_LASER_1_STATUS,
    PAR_LED_RED_POWER,
    PAR_LED_GREEN_POWER,
    PAR_LED_RED_CHANNEL_2,
    PAR_LED_GREEN_CHANNEL_2,
    PAR_LED_RED_CHANNEL_1,
    PAR_LED_GREEN_CHANNEL_1,
    PAR_ENABLE_LASER_2,
    PAR_ENABLE_LASER_1,
    PAR_GAIN_CHANNEL_2_HIGH,
    PAR_GAIN_CHANNEL_2_LOW,
    PAR_GAIN_CHANNEL_1_HIGH,
    PAR_GAIN_CHANNEL_1_LOW,
    PAR_LASER_AMPLITUDE_START,
    PAR_LASER_AMPLITUDE_OSCILLATION_PERCENTAGE,
    PAR_LASER_EXPOTIME,
    PAR_VIAL_EXPOTIME,              // fine parametri sistema embedded (328)
    ST_UNAVAILABLE = 400,
    ST_AVAILABLE,
    ST_INITIALIZING,
    ST_READY,
    ST_ERROR,
    WM_NONE = 500,
    WM_EXTERNAL_SOURCE,
    WM_TIMED,
    WM_MECHANICAL_TEST,
    PM_NORMAL = 600,
    PM_GOING_TO_STOP_ON_CONDITION,
    PM_STOP_ON_CONDITION,
    IN_TRIGGER = 700,
    IN_ZERO_TORRETTA,
    IN_VIAL_ID_0,
    IN_VIAL_ID_1,
    OUT_READY = 800,
    OUT_ACQ_READY,
    OUT_VIAL_ID_0,
    OUT_VIAL_ID_1,
    OUT_DATA_VALID,
    OUT_REJECT_1,
    OUT_REJECT_2,
    OUT_REJECT_3,
    CAL_SPLINE_REGION_USED_COUNT = 900,
    CAL_A0_REGION0,
    CAL_B0_REGION0,
    CAL_C0_REGION0,
    CAL_D0_REGION0,
    CAL_X0_LOWER,
    CAL_X0_UPPPER,
    CAL_A1_REGION1,
    CAL_B1_REGION1,
    CAL_C1_REGION1,
    CAL_D1_REGION1,
    CAL_X1_LOWER,
    CAL_X1_UPPPER,
    CAL_A2_REGION2,
    CAL_B2_REGION2,
    CAL_C2_REGION2,
    CAL_D2_REGION2,
    CAL_X2_LOWER,
    CAL_X2_UPPPER,
    CAL_A3_REGION3,
    CAL_B3_REGION3,
    CAL_C3_REGION3,
    CAL_D3_REGION3,
    CAL_X3_LOWER,
    CAL_X3_UPPPER,
    CAL_A4_REGION4,
    CAL_B4_REGION4,
    CAL_C4_REGION4,
    CAL_D4_REGION4,
    CAL_X4_LOWER,
    CAL_X4_UPPPER,
    FORCE_MAX_INT = 0xFFFF,
} SYSTEM_ID;
*/

/*
struct VariantDatum
{
    SYSTEM_ID ID_;
    SYSTEM_TYPE type_;
    int length_; // grandezza in byte del parametro
    char* datum_;
};

struct DataChunk
{
    int totData_; // corrisponde al numero di VariantDatum presenti in DataChunk
    VariantDatum* data_;
};

struct DefaultValues            //con parametri sistema embedded
{
    unsigned int infoserialnumber_;
    char infofirmwareversion_[32];
    char infosdkversion_[32];
    char infoipaddress_[32];
    unsigned int infoport_;
    unsigned int infofpgaversion_;
};
*/

#endif // OPACITYDATA_H
