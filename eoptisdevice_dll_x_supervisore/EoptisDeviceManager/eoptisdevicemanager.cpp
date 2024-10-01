#include "eoptisdevicemanager.h"
#include "EoptisDeviceManagerPrivate.h"
#include <opacimetro.h>

using namespace eoptis_device_manager;

extern "C"
	EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV connectDevice(const char* ip, unsigned int port, bool activeCallback)
{
	return connectDevicePrivate(ip, port, activeCallback);
}

extern "C"
	EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV disconnect(const char* ip)
{
	Opacimetro* opacimetro;

	opacimetro = getOpacimetroGivenIP(ip);
	if(opacimetro == 0)
		return ERROR_CODE_UNABLE_TO_CONNECT;

	return opacimetro->disconnect();
}

extern "C"
	EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV getInfo(const char *ip, VariantDatumStruct* info)
{
	ERROR_CODE res = ERROR_CODE_UNABLE_TO_CONNECT;
	Opacimetro* opacimetro;

	opacimetro = getOpacimetroGivenIP(ip);
	if(opacimetro == 0)
		return res;

	VariantDatum* infoObj = new VariantDatum();
	if (!createVD(infoObj, info))
		return ERROR_CODE_GENERIC_FAILURE;

	res = opacimetro->getInfo(*infoObj);
	populateVDS(infoObj, info);
	destroyVD(infoObj);
	delete infoObj;
	return res;
}

extern "C"
	EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV getAllInfo(const char *ip, DataChunkStruct* dataChunk)
{
	ERROR_CODE res = ERROR_CODE_UNABLE_TO_CONNECT;
	Opacimetro* opacimetro;

	opacimetro = getOpacimetroGivenIP(ip);
	if(opacimetro == 0)
		return res;

	DataChunk* dataChunkObj = new DataChunk();
	if (!createDC(dataChunkObj, dataChunk))
		return ERROR_CODE_GENERIC_FAILURE;

	res = opacimetro->getAllInfo(*dataChunkObj);
	populateDCS(dataChunkObj, dataChunk);
	destroyDC(dataChunkObj);
	return res;
}

extern "C"
	EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV getParameter(const char *ip, VariantDatumStruct* parameter)
{
	ERROR_CODE res = ERROR_CODE_UNABLE_TO_CONNECT;
	Opacimetro* opacimetro;

	opacimetro = getOpacimetroGivenIP(ip);
	if(opacimetro == 0)
		return res;

	VariantDatum* parameterObj  = new VariantDatum();
	if (!createVD(parameterObj, parameter))
		return ERROR_CODE_GENERIC_FAILURE;

	res = opacimetro->getParameter(*parameterObj);
	populateVDS(parameterObj, parameter);
	destroyVD(parameterObj);
	delete parameterObj;
	return res;
}

extern "C"
	EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV getParameterType(int paramId, int* paramType, int* paramSize)
{
	ERROR_CODE res = ERROR_CODE_UNABLE_TO_CONNECT;

	VariantDatum* parameterObj  = new VariantDatum();
	DataDescriptor dd = parameterObj->getDataDescriptorGivenID((SYSTEM_ID)paramId);
	*paramType = (int)dd.type_;
	*paramSize = (int)dd.length_;
	destroyVD(parameterObj);
	delete parameterObj;
	return res;
}

extern "C"
	EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV setParameter(const char *ip, const VariantDatumStruct* parameter)
{
	ERROR_CODE res = ERROR_CODE_UNABLE_TO_CONNECT;
	Opacimetro* opacimetro;

	opacimetro = getOpacimetroGivenIP(ip);
	if(opacimetro == 0)
		return res;

	VariantDatum* parameterObj = new VariantDatum();
	if (!createVD(parameterObj, parameter))
		return ERROR_CODE_GENERIC_FAILURE;

	memcpy(parameterObj->data_, parameter->data_, parameter->length_);
	res = opacimetro->setParameter(*parameterObj);
	//populateVDS(parameterObj, parameter);
	destroyVD(parameterObj);
	delete parameterObj;
	return res;
}

extern "C"
	EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV getParameters(const char *ip, DataChunkStruct* dataChunk)
{
	ERROR_CODE res = ERROR_CODE_UNABLE_TO_CONNECT;
	Opacimetro* opacimetro;

	opacimetro = getOpacimetroGivenIP(ip);
	if(opacimetro == 0)
		return res;

	//dataChunk->print();	//PIER: RIMUOVERE
	DataChunk* dataChunkObj = new DataChunk();
	if (!createDC(dataChunkObj, dataChunk))
		return ERROR_CODE_GENERIC_FAILURE;

	res = opacimetro->getParameters(*dataChunkObj);
	//OutputDebugStringA("post getParameters!");
	populateDCS(dataChunkObj, dataChunk);
	destroyDC(dataChunkObj);
	return res;
}

extern "C"
	EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV setParameters(const char *ip, const DataChunkStruct* dataChunk)
{
	ERROR_CODE res = ERROR_CODE_UNABLE_TO_CONNECT;
	Opacimetro* opacimetro;

	opacimetro = getOpacimetroGivenIP(ip);
	if(opacimetro == 0)
		return res;

	DataChunk* dataChunkObj = new DataChunk();
	if (!createDC(dataChunkObj, dataChunk))
		return ERROR_CODE_GENERIC_FAILURE;

	dataChunkObj->totData_ = dataChunk->totData_;
	for (int i=0; i<dataChunk->totData_; i++)
	{
		memcpy(dataChunkObj->data_[i].data_, dataChunk->data_[i].data_, dataChunk->data_[i].length_);
	}
	memcpy(dataChunkObj->data_, dataChunk->data_, sizeof(dataChunk));
    res = opacimetro->setParameters(*dataChunkObj);
    //populateDCS(dataChunkObj, dataChunk);
    destroyDC(dataChunkObj);
    return res;
}

extern "C"
	EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV getStatus(const char *ip, DataChunkStruct* status, bool continuousUpdate)
{
	ERROR_CODE res = ERROR_CODE_UNABLE_TO_CONNECT;
	Opacimetro* opacimetro;

	opacimetro = getOpacimetroGivenIP(ip);
	if(opacimetro == 0)
		return res;

	DataChunk* dataChunkObj = new DataChunk();
	if (!createDC(dataChunkObj, status))
		return ERROR_CODE_GENERIC_FAILURE;

	res = opacimetro->getStatus(*dataChunkObj, continuousUpdate);
	populateDCS(dataChunkObj, status);
	destroyDC(dataChunkObj);
	return res;
}

extern "C"
	EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV getInputs(const char *ip, DataChunkStruct* inputs, bool continuousUpdate)
{
	ERROR_CODE res = ERROR_CODE_UNABLE_TO_CONNECT;
	Opacimetro* opacimetro;

	opacimetro = getOpacimetroGivenIP(ip);
	if(opacimetro == 0)
		return res;

	DataChunk* dataChunkObj = new DataChunk();
	if (!createDC(dataChunkObj, inputs))
		return ERROR_CODE_GENERIC_FAILURE;

	res = opacimetro->getInputs(*dataChunkObj, continuousUpdate);
	populateDCS(dataChunkObj, inputs);
	destroyDC(dataChunkObj);
	return res;
}

extern "C"
	EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV getOutputs(const char *ip, DataChunkStruct* outputs, bool continuousUpdate)
{
	ERROR_CODE res = ERROR_CODE_UNABLE_TO_CONNECT;
	Opacimetro* opacimetro;

	opacimetro = getOpacimetroGivenIP(ip);
	if(opacimetro == 0)
		return res;

	DataChunk* dataChunkObj = new DataChunk();
	if (!createDC(dataChunkObj, outputs))
		return ERROR_CODE_GENERIC_FAILURE;

	res = opacimetro->getOutputs(*dataChunkObj, continuousUpdate);
	populateDCS(dataChunkObj, outputs);
	destroyDC(dataChunkObj);
	return res;
}

extern "C"
	EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV setOutputs(const char *ip, const DataChunkStruct* outputs)
{
	ERROR_CODE res = ERROR_CODE_UNABLE_TO_CONNECT;
	Opacimetro* opacimetro;

	opacimetro = getOpacimetroGivenIP(ip);
	if(opacimetro == 0)
		return res;

	DataChunk* dataChunkObj = new DataChunk();
	if (!createDC(dataChunkObj, outputs))
		return ERROR_CODE_GENERIC_FAILURE;

	res = opacimetro->setOutputs(*dataChunkObj);
	//populateDCS(dataChunkObj, outputs);
	destroyDC(dataChunkObj);
	return res;
}

extern "C"
	EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV setOutputPattern(const char *ip, const DataChunkStruct* patternArray)
{
	ERROR_CODE res = ERROR_CODE_UNABLE_TO_CONNECT;
	Opacimetro* opacimetro;

	opacimetro = getOpacimetroGivenIP(ip);
	if(opacimetro == 0)
		return res;

	DataChunk* dataChunkObj = new DataChunk();
	if (!createDC(dataChunkObj, patternArray))
		return ERROR_CODE_GENERIC_FAILURE;

	res = opacimetro->setOutputPattern(*dataChunkObj);
	//populateDCS(dataChunkObj, outputs);
	destroyDC(dataChunkObj);
	return res;
}

extern "C"
	EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV setWorkingMode(const char *ip, int mode)   //da completare
{
	ERROR_CODE res = ERROR_CODE_UNABLE_TO_CONNECT;
	Opacimetro* opacimetro;

	opacimetro = getOpacimetroGivenIP(ip);
	if(opacimetro == 0)
		return res;

	res = opacimetro->setWorkingMode(mode);
	return res;
}

extern "C"
	EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV getWorkingMode(const char *ip, int *mode)   //da completare
{
	ERROR_CODE res = ERROR_CODE_UNABLE_TO_CONNECT;
	Opacimetro* opacimetro;

	opacimetro = getOpacimetroGivenIP(ip);
	if(opacimetro == 0)
		return res;

	res = opacimetro->getWorkingMode(mode);
	return res;
}

extern "C"
	EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV stopOnCondition(const char *ip, int stopCondition, int spindleID)   //da completare
{
	ERROR_CODE res = ERROR_CODE_UNABLE_TO_CONNECT;
	Opacimetro* opacimetro;

	opacimetro = getOpacimetroGivenIP(ip);
	if(opacimetro == 0)
		return res;

	res = opacimetro->stopOnCondition(stopCondition, spindleID);
	return res;
}

extern "C"
	EOPTISDEVICEMANAGERSHARED_EXPORT int CALL_CONV receiveData(const char *ip, DataChunkStruct* dataChunk)
{
	int res = -1;
	Opacimetro* opacimetro;

	opacimetro = getOpacimetroGivenIP(ip);
	if(opacimetro == 0)
		return res;

	DataChunk* dataChunkObj = new DataChunk();

    res = opacimetro->receiveData(*dataChunkObj);
    populateDCS(dataChunkObj, dataChunk);
    destroyDC(dataChunkObj);
    delete dataChunkObj;
    dataChunkObj = nullptr;
	return res;
}

extern "C"
	EOPTISDEVICEMANAGERSHARED_EXPORT void CALL_CONV freeData(DataChunkStruct* dataChunk)
{
	dataChunk->clear();
}

/////////////// DA BUTTARE ////////////////
///
///
///
//extern "C"
//EOPTISDEVICEMANAGERSHARED_EXPORT void receiveDataOld(const char *ip, DataChunk &dataChunk)
//{
//    Opacimetro* opacimetro;
//
//    opacimetro = getOpacimetroGivenIP(ip);
//    if(opacimetro == 0)
//        return;
//
//    return opacimetro->receiveData(dataChunk);
//}

//extern "C"
//EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV getParamDummy(const char *ip, VariantDatumStruct* parameter)
//{
//    ERROR_CODE res = ERROR_CODE_UNABLE_TO_CONNECT;
//    Opacimetro* opacimetro;
//
//    opacimetro = getOpacimetroGivenIP(ip);
//    if(opacimetro == 0)
//        return res;
//
//    VariantDatum* parameterObj = new VariantDatum();
//    if (!createVD(parameterObj, parameter))
//        return ERROR_CODE_GENERIC_FAILURE;
//
//    opacimetro->getParameter(*parameterObj);
//    int value = 123456;
//    memcpy(parameterObj->data_, &value, sizeof(int));
//    //memset(parameterObj->data_, 1, sizeof(DATA_BYTE)*parameterObj->length_);
//    populateVDS(parameterObj, parameter);
//    destroyVD(parameterObj);
//    delete parameterObj;
//    return ERROR_CODE_OK;
//}

/*extern "C"
EOPTISDEVICEMANAGERSHARED_EXPORT ERROR_CODE CALL_CONV getParamsDummy(const char *ip, DataChunkStruct* dataChunk)
{
ERROR_CODE res = ERROR_CODE_UNABLE_TO_CONNECT;
Opacimetro* opacimetro;

opacimetro = getOpacimetroGivenIP(ip);
if(opacimetro == 0)
return res;

DataChunk* dataChunkObj = new DataChunk();
if (!createDC(dataChunkObj, dataChunk))
return ERROR_CODE_GENERIC_FAILURE;

res = opacimetro->getParameters(*dataChunkObj);
int[] value = {123, 456};
for (int i=0; i<dataChunkObj->totData_; i++)
{
memcpy(dataChunkObj->data_[i], &value[i%2], sizeof(int));
}
populateDCS(dataChunkObj, dataChunk);
destroyDC(dataChunkObj);
return res;
}*/

