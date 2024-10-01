#ifndef EOPTISDEVICEMANAGERPRIVATE_H
#define EOPTISDEVICEMANAGERPRIVATE_H

#include "eoptisdevicemanager.h"
#include <opacimetro.h>

Opacimetro* CALL_CONV getOpacimetroGivenIP(const char* ip);
ERROR_CODE CALL_CONV connectDevicePrivate(const char* ip, unsigned int port, bool activeCallback);

bool CALL_CONV createVD(VariantDatum* vd, const VariantDatumStruct* vds);
void CALL_CONV populateVDS(VariantDatum* vd, VariantDatumStruct* vds);
void CALL_CONV destroyVD(VariantDatum* vd);

bool CALL_CONV createDC(DataChunk* vd, const DataChunkStruct* vds);
void CALL_CONV populateDCS(DataChunk* vd, DataChunkStruct* vds);
void CALL_CONV destroyDC(DataChunk* vd);

#endif // EOPTISDEVICEMANAGERPRIVATE_H
