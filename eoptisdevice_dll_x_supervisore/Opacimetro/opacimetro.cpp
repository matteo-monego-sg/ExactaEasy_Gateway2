#include "opacimetro.h"

const unsigned int MAX_BUFFER_SIZE = 1024*1024;

Opacimetro::Opacimetro()
{
	printf("Creato opacimetro.\n");
	receiveDataBuffer_ = new char[MAX_BUFFER_SIZE];
	sendDataBuffer_ = new char[MAX_BUFFER_SIZE];

	sprintf(ip_, "0.0.0.0");
}

Opacimetro::~Opacimetro()
{
	communicationInterface_.closeClient();
	clear();
}

int Opacimetro::GetDevices(const char* ip)
{
	printf("dentro la funzione getdevices!\n");
	communicationInterface_.openClient(UDP, 23456, 23456, ip);

	char modality = 0;
	communicationInterface_.sendData(&modality, 1);

	return 0;
}

ERROR_CODE Opacimetro::connect(const char* ip, unsigned int port, bool activeCallback)
{
	printf("Dentro la funzione connect!\n");

	if (activeCallback == true)
	{printf("Callback set TRUE! Send data to supervisor!\n");}    //da completare
	else
	{printf("Callback set FALSE! No data to supervisor!\n");}     //da completare

	try
	{
		communicationInterface_.openClient(TCP, port, port, ip);
	}
	catch(...)
	{
		return ERROR_CODE_UNABLE_TO_CONNECT;
	}

	printf("Client connesso a %s!\n", ip);
	sprintf(ip_, "%s", ip);

	return ERROR_CODE_OK;
}

ERROR_CODE Opacimetro::disconnect()
{
	try
	{ communicationInterface_.closeClient(); }
	catch(...)
	{ return ERROR_CODE_GENERIC_FAILURE; }

	printf("Client disconnesso da IP %s!\n", ip_);

	sprintf(ip_, "0.0.0.0");

	return ERROR_CODE_OK;
}

int Opacimetro::dataChunkToBuffer(DataChunk& dataChunk, const bool isWrite)
{
	unsigned int cont = dataChunk.getTotData();

	VariantDatum variantDatum;
	DataDescriptor dataDescriptor;

	if (isWrite == false)
	{
		printf("solo in lettura\n");
	}
	else
	{
		printf("in scrittura\n");
	}

	unsigned int msgSize = 0;   //pier: aggiungo msgSize in testa
	unsigned int len = 0;
	unsigned int sum = len;
	unsigned int valTotData = 0;

	SYSTEM_ID id = SYSTEM_ID_UNKNOWN;
	SYSTEM_TYPE type = TYPEUNKNOWN;
	unsigned int length = 0;

	len = sizeof(msgSize);      //pier: aggiungo msgSize in testa
    memcpy(&sendDataBuffer_[sum], &msgSize, len);
    sum += len;

    len = sizeof(isWrite);
    memcpy(&sendDataBuffer_[sum], &isWrite, len);
    sum += len;

	valTotData = dataChunk.getTotData();
	len = sizeof(valTotData);
	memcpy(&sendDataBuffer_[sum], &valTotData, len);
	sum += len;


	for (unsigned int i = 0; i<cont; i++)
	{
		dataChunk.getData(variantDatum, i);


		len = sizeof(variantDatum.getID());
		id = variantDatum.getID();
		memcpy(&sendDataBuffer_[sum], &id, len);
		sum += len;

		dataDescriptor = variantDatum.getDataDescriptorGivenID(id);

		variantDatum.getDataDescriptorGivenID(id);
		type = dataDescriptor.type_;
		len = sizeof(type);
		memcpy(&sendDataBuffer_[sum], &type, len);
		sum += len;

		variantDatum.getDataDescriptorGivenID(id);
		length = dataDescriptor.length_;
		len = sizeof(length);
		memcpy(&sendDataBuffer_[sum], &length, len);
		sum += len;

		len = length;
		variantDatum.getData(&sendDataBuffer_[sum]);
		sum += len;

		/*
		if(type == TYPEINT || type == TYPEUNSIGNEDINT)
		{
		int val;
		variantDatum.getData(reinterpret_cast<DATA_BYTE*>(&val));
		memcpy(&dataBuffer_[sum], &val, len);
		sum += len;
		}
		else if(type == TYPEFLOAT)
		{
		float val;
		variantDatum.getData(reinterpret_cast<DATA_BYTE*>(&val));
		memcpy(&dataBuffer_[sum], &val, len);
		sum += len;
		}
		else if(type == TYPECHAR || type == TYPEUNSIGNEDCHAR || type == TYPESTRING)
		{
		DATA_BYTE val[1024];
		variantDatum.getData(val);
		//variantDatum.getData(&val);
		//memcpy(&dataBuffer_[sum], &val, len);
		memcpy(&dataBuffer_[sum], val, len);
		sum += len;
		}
		else if(type == TYPESHORT)
		{
		short val;
		variantDatum.getData(reinterpret_cast<DATA_BYTE*>(&val));
		memcpy(&dataBuffer_[sum], &val, len);
		sum += len;
		}
		else if(type == TYPESHORTBUFFER || type == TYPEMEASURE)
		{
		variantDatum.getData(&dataBuffer_[sum]);
		sum += len;
		}
		*/
	}

	msgSize = sum;
	memcpy(&sendDataBuffer_[0], &msgSize, sizeof(msgSize));   //pier: aggiungo msgSize in testa

	return sum;
}

void Opacimetro::bufferToDataChunk(DataChunk& dataChunk)
{
	DataDescriptor dataDescriptor;
	SYSTEM_ID id = SYSTEM_ID_UNKNOWN;
	SYSTEM_TYPE type = TYPEUNKNOWN;
	unsigned int length = 0;

	unsigned int msgSize = static_cast<int>(receiveDataBuffer_[0]);   //pier: aggiungo msgSize in testa
    int len = sizeof(unsigned int);
    int sum = len;

    bool isWrite = static_cast<bool>(receiveDataBuffer_[sum]);
    len = sizeof(bool);
    sum += len;

	int* p = reinterpret_cast<int*>(&(receiveDataBuffer_[sum]));
	dataChunk.setTotData(*p);
	len = sizeof(*p);
	sum += len;

	//char debugStr[250];
	//sprintf_s(debugStr, "p = %d", *p); 
	//MessageBoxA(nullptr, debugStr, "", 0);

	VariantDatum variantDatum;
	//printf("aaa = %d\n", *p);
	//if (*p!=4)
	//	int pluto = 1;
	//if (*p> 0)
	//{
	//	char aaa[260];
	//	sprintf(aaa,"---> bufferToDataChunk *p = %d\n", *p);
	//	OutputDebugStringA(aaa);
	//}
	for(int j = 0; j < *p; j++)
	{
		len = sizeof(SYSTEM_ID);
		memcpy(&id, &receiveDataBuffer_[sum], len);
		sum += len;

		variantDatum.getDataDescriptorGivenID(id);

		len = sizeof(dataDescriptor.type_);
		memcpy(&type, &receiveDataBuffer_[sum], len);
		sum += len;

		len = sizeof(dataDescriptor.length_);
		memcpy(&length, &receiveDataBuffer_[sum], len);
		sum += len;

		len = length;
		variantDatum.setData(id, reinterpret_cast<DATA_BYTE*>(&receiveDataBuffer_[sum]));
		sum += len;
		dataChunk.setData(variantDatum, j);

		

		/*
		if(type == TYPEINT || type == TYPEUNSIGNEDINT)
		{
		int val;
		memcpy(&val, &dataBuffer_[sum], len);
		//variantDatum.getData(reinterpret_cast<DATA_BYTE*>(&val));
		sum += len;
		variantDatum.setData(id, reinterpret_cast<DATA_BYTE*>(&val));
		}
		else if(type == TYPECHAR || type == TYPEUNSIGNEDCHAR || type == TYPESTRING)
		{
		DATA_BYTE val[1024];
		memcpy(val, &dataBuffer_[sum], len);
		//variantDatum.getData(val);
		sum += len;
		variantDatum.setData(id, val);
		}
		else if(type == TYPESHORT)
		{
		short val;
		memcpy(&val, &dataBuffer_[sum], len);
		//variantDatum.getData(reinterpret_cast<DATA_BYTE*>(&val));
		sum += len;
		variantDatum.setData(id, reinterpret_cast<DATA_BYTE*>(&val));
		}
		else if(type == TYPEFLOAT)
		{
		float val;
		memcpy(&val, &dataBuffer_[sum], len);
		sum += len;
		variantDatum.setData(id, reinterpret_cast<DATA_BYTE*>(&val));
		}
		else if (type == TYPESHORTBUFFER || type == TYPEMEASURE)
		{
		variantDatum.setData(id, reinterpret_cast<DATA_BYTE*>(&dataBuffer_[sum]));
		sum += len;
		}

		dataChunk.setData(variantDatum, j);
		*/
	}
}

ERROR_CODE Opacimetro::getStatus(DataChunk& status, bool continuousUpdate)
{
	if (continuousUpdate == true)
	{
		//inviare ogni cambiamento, comprese le variazioni dei led della scheda, tramite callback
	}

	exchangeDataChunk(status, false);

	return ERROR_CODE_OK;
}

ERROR_CODE Opacimetro::getInfo(VariantDatum& info)
{
	DataChunk dataChunk_;
	dataChunk_.setTotData(1);

	SYSTEM_ID id = info.getID();

	if (checkIdInfo(id) != ERROR_CODE_OK)
		return ERROR_CODE_ID_NOT_VALID;

	dataChunk_.setData(info, 0);

	exchangeDataChunk(dataChunk_, false);

	dataChunk_.getData(info, 0);

	return ERROR_CODE_OK;
}

ERROR_CODE Opacimetro::getAllInfo(DataChunk& dataChunk)
{
	VariantDatum variantDatum;
	dataChunk.setTotData(6);
	char buffer[32];
	unsigned int val;

	variantDatum.setData(INFO_SERIAL_NUMBER, reinterpret_cast<DATA_BYTE*>(&val));
	dataChunk.setData(variantDatum, 0);
	variantDatum.setData(INFO_FIRMWARE_VERSION, reinterpret_cast<DATA_BYTE*>(buffer));
	dataChunk.setData(variantDatum, 1);
	variantDatum.setData(INFO_SDK_VERSION, reinterpret_cast<DATA_BYTE*>(buffer));
	dataChunk.setData(variantDatum, 2);
	variantDatum.setData(INFO_IP_ADDRESS, reinterpret_cast<DATA_BYTE*>(buffer));
	dataChunk.setData(variantDatum, 3);
	variantDatum.setData(INFO_PORT, reinterpret_cast<DATA_BYTE*>(&val));
	dataChunk.setData(variantDatum, 4);
	variantDatum.setData(INFO_FPGA_VERSION, reinterpret_cast<DATA_BYTE*>(buffer)                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           );
	dataChunk.setData(variantDatum, 5);

	exchangeDataChunk(dataChunk, false);

	return ERROR_CODE_OK;
}

ERROR_CODE Opacimetro::getParameter(VariantDatum& parameter)
{
	// HARD DEBUG
	char debugStr[250];
	sprintf_s(debugStr, "Opacimetro.getParameter: INIT\n");
	OutputDebugString(debugStr);
	DataChunk dataChunk_;
	dataChunk_.setTotData(1);

	SYSTEM_ID id = parameter.getID();

	if (checkIdParameter(id) != ERROR_CODE_OK)
		return ERROR_CODE_ID_NOT_VALID;

	dataChunk_.setData(parameter, 0);

	exchangeDataChunk(dataChunk_, false);

	dataChunk_.getData(parameter, 0);

	// HARD DEBUG
	sprintf_s(debugStr, "Opacimetro.getParameter: END\n");
	OutputDebugString(debugStr);

	return ERROR_CODE_OK;
}

ERROR_CODE Opacimetro::setParameter(VariantDatum& parameter)
{
	// HARD DEBUG
	char debugStr[250];
	sprintf_s(debugStr, "Opacimetro.setParameter: INIT\n");
	OutputDebugString(debugStr);

	DataChunk dataChunk_;
	dataChunk_.setTotData(1);

	SYSTEM_ID id = parameter.getID();

	if (checkIdParameter(id) != ERROR_CODE_OK)
		return ERROR_CODE_ID_NOT_VALID;

	dataChunk_.setData(parameter, 0);

	exchangeDataChunk(dataChunk_, true);

	dataChunk_.getData(parameter, 0);

	// HARD DEBUG
	sprintf_s(debugStr, "Opacimetro.setParameter: END\n");
	OutputDebugString(debugStr);

	return ERROR_CODE_OK;
}

ERROR_CODE Opacimetro::getParameters(DataChunk& dataChunk)
{
	// HARD DEBUG
	char debugStr[250];
	sprintf_s(debugStr, "Opacimetro.getParameters: INIT\n");
	OutputDebugString(debugStr);

	unsigned int cont = dataChunk.getTotData();
	VariantDatum variantDatum;
	DataDescriptor dataDescriptor;

	//int laserGain = 1;
	//int vialGain = 10;
	//double warningUpper = 3333;
	//double warningLower = 1111;
	//double errorUpper = 3344;
	//double errorLower = 1100;
	//int spindleCount = 40;
	//unsigned int torrOffset = 16;
	//unsigned int incremento = 2;
	//unsigned int freeruntimer = 3000;
	for (unsigned int i=0; i<cont; i++)
	{
		dataChunk.getData(variantDatum, i);
		dataDescriptor = variantDatum.getDataDescriptorGivenID(variantDatum.getID());
		if(dataDescriptor.type_ == TYPEUNKNOWN || !(dataDescriptor.length_ > 0))
			return ERROR_CODE_ID_NOT_VALID;


	//	if (variantDatum.getID()==PAR_LASER_GAIN) {
	//		variantDatum.setData(PAR_LASER_GAIN, reinterpret_cast<DATA_BYTE*>(&laserGain));
	//	}
	//	if (variantDatum.getID()==PAR_VIAL_GAIN) {
	//		variantDatum.setData(PAR_VIAL_GAIN, reinterpret_cast<DATA_BYTE*>(&vialGain));
	//	}
	//	if (variantDatum.getID()==TH_WARNING_UPPER) {
	//		variantDatum.setData(TH_WARNING_UPPER, reinterpret_cast<DATA_BYTE*>(&warningUpper));
	//	}
	//	if (variantDatum.getID()==TH_WARNING_LOWER) {
	//		variantDatum.setData(TH_WARNING_LOWER, reinterpret_cast<DATA_BYTE*>(&warningLower));
	//	}
	//	if (variantDatum.getID()==TH_ERROR_UPPER) {
	//		variantDatum.setData(TH_ERROR_UPPER, reinterpret_cast<DATA_BYTE*>(&errorUpper));
	//	}
	//	if (variantDatum.getID()==TH_ERROR_LOWER) {
	//		variantDatum.setData(TH_ERROR_LOWER, reinterpret_cast<DATA_BYTE*>(&errorLower));
	//	}
	//	if (variantDatum.getID()==PAR_SPINDLE_COUNT) {
	//		variantDatum.setData(PAR_SPINDLE_COUNT, reinterpret_cast<DATA_BYTE*>(&spindleCount));
	//	}
	//	if (variantDatum.getID()==PAR_0_TORRETTA_OFFSET) {
	//		variantDatum.setData(PAR_0_TORRETTA_OFFSET, reinterpret_cast<DATA_BYTE*>(&torrOffset));
	//	}
	//	if (variantDatum.getID()==PAR_INCREMENTO_SPINDLE_ID) {
	//		variantDatum.setData(PAR_INCREMENTO_SPINDLE_ID, reinterpret_cast<DATA_BYTE*>(&incremento));
	//	}
	//	if (variantDatum.getID()==PAR_FREE_RUN_TIMER) {
	//		variantDatum.setData(PAR_FREE_RUN_TIMER, reinterpret_cast<DATA_BYTE*>(&freeruntimer));
	//	}
	//	dataChunk.setData(variantDatum, i);
	}

	//exchangeDataChunk(dataChunk, false);
	//PIER: per il colorimetro cambio gestione: get parameters diventa asincrono, i parametri vengono ricevuti nel thread di acquisizione
	sendData(dataChunk, false);

	// HARD DEBUG
	sprintf_s(debugStr, "Opacimetro.getParameters: END\n");
	OutputDebugString(debugStr);

	return ERROR_CODE_OK;
}

ERROR_CODE Opacimetro::setParameters(DataChunk& dataChunk)
{
	// HARD DEBUG
	char debugStr[250];
	sprintf_s(debugStr, "Opacimetro.setParameters: INIT\n");
	OutputDebugString(debugStr);

	unsigned int cont = dataChunk.getTotData();
	VariantDatum variantDatum;
	DataDescriptor dataDescriptor;

	for (unsigned int i=0; i<cont; i++)
	{
		dataChunk.getData(variantDatum, i);
		dataDescriptor = variantDatum.getDataDescriptorGivenID(variantDatum.getID());
		if(dataDescriptor.type_ == TYPEUNKNOWN || !(dataDescriptor.length_ > 0))
			return ERROR_CODE_ID_NOT_VALID;
	}

	exchangeDataChunk(dataChunk, true);

	// HARD DEBUG
	sprintf_s(debugStr, "Opacimetro.setParameters: END\n");
	OutputDebugString(debugStr);

	return ERROR_CODE_OK;
}

ERROR_CODE Opacimetro::setWorkingMode(int mode)
{
	// HARD DEBUG
	char debugStr[250];
	sprintf_s(debugStr, "Opacimetro.setWorkingMode: INIT\n");
	OutputDebugString(debugStr);

	if ((mode == -1) || (mode == 0) || (mode == 1) || (mode == 2))
	{
		VariantDatum variantDatum;

		variantDatum.setData(PAR_WORKING_MODE, reinterpret_cast<DATA_BYTE*>(&mode));

		setParameter(variantDatum);

		// HARD DEBUG
		char debugStr[250];
		sprintf_s(debugStr, "Opacimetro.setWorkingMode: END\n");
		OutputDebugString(debugStr);

		return ERROR_CODE_OK;
	}

	else
		return ERROR_CODE_PARAMETER_NOT_VALID;

}

ERROR_CODE Opacimetro::getWorkingMode(int* mode)
{
	// HARD DEBUG
	char debugStr[250];
	sprintf_s(debugStr, "Opacimetro.getWorkingMode: INIT\n");
	OutputDebugString(debugStr);

	unsigned int value;
	VariantDatum variantDatum;
	variantDatum.setData(PAR_WORKING_MODE, reinterpret_cast<DATA_BYTE*>(&value));

	getParameter(variantDatum);

	variantDatum.getData(reinterpret_cast<DATA_BYTE*>(mode));

	// HARD DEBUG
	sprintf_s(debugStr, "Opacimetro.getWorkingMode: END\n");
	OutputDebugString(debugStr);

	return ERROR_CODE_OK;
}

ERROR_CODE Opacimetro::getInputs(DataChunk &inputs, bool continuousUpdate)
{
	if (continuousUpdate == true)
	{
		//inviare ogni cambiamento, comprese le variazioni dei led della scheda, tramite callback
	}

	VariantDatum variantDatum;
	inputs.setTotData(4);
	unsigned int val;

	variantDatum.setData(IN_TRIGGER, reinterpret_cast<DATA_BYTE*>(&val));
	inputs.setData(variantDatum, 0);
	variantDatum.setData(IN_ZERO_TORRETTA, reinterpret_cast<DATA_BYTE*>(&val));
	inputs.setData(variantDatum, 1);
	variantDatum.setData(IN_VIAL_ID_0, reinterpret_cast<DATA_BYTE*>(&val));
	inputs.setData(variantDatum, 2);
	variantDatum.setData(IN_VIAL_ID_1, reinterpret_cast<DATA_BYTE*>(&val));
	inputs.setData(variantDatum, 3);

	exchangeDataChunk(inputs, false);

	return ERROR_CODE_OK;
}

ERROR_CODE Opacimetro::getOutputs(DataChunk &outputs, bool continuousUpdate)
{
	if (continuousUpdate == true)
	{
		//inviare ogni cambiamento, comprese le variazioni dei led della scheda, tramite callback
	}

	VariantDatum variantDatum;
	outputs.setTotData(8);
	unsigned int val;

	variantDatum.setData(OUT_READY, reinterpret_cast<DATA_BYTE*>(&val));
	outputs.setData(variantDatum, 0);
	variantDatum.setData(OUT_ACQ_READY, reinterpret_cast<DATA_BYTE*>(&val));
	outputs.setData(variantDatum, 1);
	variantDatum.setData(OUT_VIAL_ID_0, reinterpret_cast<DATA_BYTE*>(&val));
	outputs.setData(variantDatum, 2);
	variantDatum.setData(OUT_VIAL_ID_1, reinterpret_cast<DATA_BYTE*>(&val));
	outputs.setData(variantDatum, 3);
	variantDatum.setData(OUT_DATA_VALID, reinterpret_cast<DATA_BYTE*>(&val));
	outputs.setData(variantDatum, 4);
	variantDatum.setData(OUT_REJECT_1, reinterpret_cast<DATA_BYTE*>(&val));
	outputs.setData(variantDatum, 5);
	variantDatum.setData(OUT_REJECT_2, reinterpret_cast<DATA_BYTE*>(&val));
	outputs.setData(variantDatum, 6);
	variantDatum.setData(OUT_REJECT_3, reinterpret_cast<DATA_BYTE*>(&val));
	outputs.setData(variantDatum, 7);

	exchangeDataChunk(outputs, false);

	return ERROR_CODE_OK;
}

ERROR_CODE Opacimetro::setOutputs(DataChunk &outputs)
{
	unsigned int cont = outputs.getTotData();

	if (cont<8)
		return ERROR_CODE_PARAMETER_NOT_VALID;

	VariantDatum variantDatum;

	for(unsigned int j = 0; j<cont; j++)
	{
		outputs.getData(variantDatum, j);
		if (checkIdOutputs(variantDatum.getID()) != ERROR_CODE_OK)
			return ERROR_CODE_ID_NOT_VALID;
	}

	exchangeDataChunk(outputs, true);

	return ERROR_CODE_OK;
}

ERROR_CODE Opacimetro::setOutputPattern(DataChunk &patternArray)
{
	if(checkOutputPattern(patternArray) != ERROR_CODE_OK)
		return ERROR_CODE_PARAMETER_NOT_VALID;

	exchangeDataChunk(patternArray, true);

	return ERROR_CODE_OK;
}

ERROR_CODE Opacimetro::stopOnCondition(int stopCondition, int spindleID)
{
	if (stopCondition<0 || stopCondition>6)
		return ERROR_CODE_PARAMETER_NOT_VALID;

	if (spindleID<-1 || spindleID>1000)
		return ERROR_CODE_PARAMETER_NOT_VALID;

	DataChunk dataChunk;
	dataChunk.setTotData(3);
	VariantDatum variantDatum;

	variantDatum.setData(PAR_STOP_ON_CONDITION_MODE, reinterpret_cast<DATA_BYTE*>(&stopCondition));
	dataChunk.setData(variantDatum, 0);

	variantDatum.setData(PAR_STOP_ON_CONDITION_SPINDLE, reinterpret_cast<DATA_BYTE*>(&spindleID));
	dataChunk.setData(variantDatum, 1);

	int goingToStopOnCondition = 1;

	variantDatum.setData(PM_GOING_TO_STOP_ON_CONDITION, reinterpret_cast<DATA_BYTE*>(&goingToStopOnCondition));
	dataChunk.setData(variantDatum, 2);

	setParameters(dataChunk);

	return ERROR_CODE_OK;
}

void Opacimetro::exchangeDataChunk(DataChunk &dataChunk, bool isWrite)
{
	sendData(dataChunk, isWrite);
	receiveData(dataChunk);
}

void Opacimetro::sendData(DataChunk& dataChunk, bool isWrite)
{
	int len = dataChunkToBuffer(dataChunk, isWrite);
	communicationInterface_.sendData(sendDataBuffer_, len);
}

int Opacimetro::receiveData(DataChunk& dataChunk)
{	
	// HARD DEBUG
	char debugStr[250];
	sprintf_s(debugStr, "Opacimetro.receiveData: INIT\n");
	OutputDebugString(debugStr);

	int length = communicationInterface_.receiveData(receiveDataBuffer_, MAX_BUFFER_SIZE);
	if (length>0)
	{
		bufferToDataChunk(dataChunk);
	}
	// HARD DEBUG
	sprintf_s(debugStr, "Opacimetro.receiveData: END\n");
	OutputDebugString(debugStr);
	return length;
}

const char *Opacimetro::getIP()
{
	return ip_;
}

void Opacimetro::clear()
{
	if(receiveDataBuffer_)
		delete [] receiveDataBuffer_;
	receiveDataBuffer_ = 0;
	if(sendDataBuffer_)
		delete [] sendDataBuffer_;
	sendDataBuffer_ = 0;
}

ERROR_CODE Opacimetro::checkIdInfo(SYSTEM_ID id)
{
	unsigned int id_ = static_cast<unsigned int>(id);

	if ((id_ >= 200) || (id_ <= 205))
		return ERROR_CODE_OK;
	else
		return ERROR_CODE_ID_NOT_VALID;
}

ERROR_CODE Opacimetro::checkIdParameter(SYSTEM_ID id)
{
	unsigned int id_ = static_cast<unsigned int>(id);

	if ((id_ >= 300) || (id_ <= 332))
		return ERROR_CODE_OK;
	else
		return ERROR_CODE_ID_NOT_VALID;
}

ERROR_CODE Opacimetro::checkIdStatus(SYSTEM_ID id)
{
	unsigned int id_ = static_cast<unsigned int>(id);

	if ((id_ >= 400) || (id_ <= 404))
		return ERROR_CODE_OK;
	else
		return ERROR_CODE_ID_NOT_VALID;
}

ERROR_CODE Opacimetro::checkIdOutputs(SYSTEM_ID id)
{
	unsigned int id_ = static_cast<unsigned int>(id);

	if ((id_ >= 800) || (id_ <= 807))
		return ERROR_CODE_OK;
	else
		return ERROR_CODE_ID_NOT_VALID;
}

ERROR_CODE Opacimetro::checkOutputPattern(DataChunk &patternArray)
{
	if(patternArray.getTotData() < 4)
		return ERROR_CODE_PARAMETER_NOT_VALID;

	VariantDatum variantDatum;

	patternArray.getData(variantDatum, 0);
	if (variantDatum.getID() != PAR_REJECT_PATTERN_CODE_1)
		return ERROR_CODE_ID_NOT_VALID;
	patternArray.getData(variantDatum, 1);
	if (variantDatum.getID() != PAR_REJECT_COUNT_CODE_1)
		return ERROR_CODE_ID_NOT_VALID;
	patternArray.getData(variantDatum, 3);
	if (variantDatum.getID() != PAR_REJECT_PATTERN_CODE_2)
		return ERROR_CODE_ID_NOT_VALID;
	patternArray.getData(variantDatum, 4);
	if (variantDatum.getID() != PAR_REJECT_COUNT_CODE_2)
		return ERROR_CODE_ID_NOT_VALID;

	return ERROR_CODE_OK;
}
