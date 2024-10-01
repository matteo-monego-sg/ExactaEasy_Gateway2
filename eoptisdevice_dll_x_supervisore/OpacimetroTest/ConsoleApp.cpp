#include "ConsoleApp.h"
#include <time.h>
#include <bitset>
#include <iostream>

using namespace std;

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
	ERROR_CODE err;
	inputParameters_.parseCommandLineParameters(argc, argv);
	clear();

	printSizes();

	err = connectDevice(inputParameters_.serverAddress_, inputParameters_.port_, false);
	int paramType, paramSize;
	getParameterType(LIGHT_CONTROL+2, &paramType, &paramSize);
	if(err != ERROR_CODE_OK)
	{
		printf("Can't connect to device %s (%d)\n", inputParameters_.serverAddress_, err);
		return -1;
	}
	//err = connectDevice("10.200.0.151", inputParameters_.port_, false);
	//if(err != ERROR_CODE_OK)
	//{
	//	printf("Can't connect to device %s (%d)\n", "10.200.0.151", err);
	//	return -1;
	//}
	//int aaa = sizeof(VariantDatumStruct);
	DataChunkStruct dcs;
	dcs.totData_ = 2;
	dcs.data_ = new VariantDatumStruct[dcs.totData_];

	dcs.data_[0].id_ = PAR_FULL_SPECTRUM_MIS;
	dcs.data_[0].length_ = 400 * sizeof(unsigned short);
	dcs.data_[0].type_ = TYPEUNSIGNEDSHORTBUFFER;
	/*dcs.data_[0].id_ = PAR_SPINDLE_COUNT;
	dcs.data_[0].length_ = sizeof(int);
	dcs.data_[0].type_ = TYPEINT;*/

	dcs.data_[0].data_ = new DATA_BYTE[dcs.data_[0].length_];
	dcs.data_[1].id_ = INITIAL_WAVELENGTH;
	dcs.data_[1].length_ = sizeof(uint);
	dcs.data_[1].type_ = TYPEUNSIGNEDINT;
	dcs.data_[1].data_ = new DATA_BYTE[dcs.data_[1].length_];
	getParameters(inputParameters_.serverAddress_, &dcs);
	receiveData(inputParameters_.serverAddress_, &dcs);
	freeData(&dcs);

	receiveData(inputParameters_.serverAddress_, &dcs);
	freeData(&dcs);
	dcs.totData_ = 1;
	dcs.data_ = new VariantDatumStruct[dcs.totData_];
	dcs.data_[0].id_ = PAR_SPINDLE_COUNT;
	dcs.data_[0].length_ = sizeof(int);
	dcs.data_[0].type_ = TYPEINT;
	dcs.data_[0].data_ = new DATA_BYTE[dcs.data_[0].length_];
	int spindleCount = 69;
	memcpy(dcs.data_[0].data_, &spindleCount, sizeof(int));
	setParameters(inputParameters_.serverAddress_, &dcs);
	//receiveData(inputParameters_.serverAddress_, &dcs);
	dcs.data_[0].id_ = PAR_SPINDLE_COUNT;
	dcs.data_[0].length_ = sizeof(int);
	dcs.data_[0].type_ = TYPEINT;
	dcs.data_[0].data_ = new DATA_BYTE[dcs.data_[0].length_];
	dcs.totData_ = 1;
	getParameters(inputParameters_.serverAddress_, &dcs);
	receiveData(inputParameters_.serverAddress_, &dcs);
	unsigned char aaa = 0x2;
	std::bitset<8> x(aaa);
	std::cout << x;
	std::cout << "\n";

	//TEST PIER...
	/*VariantDatumStruct* vds = new VariantDatumStruct();
	vds->id_ = PAR_SPINDLE_COUNT;
	vds->type_ = TYPEINT;
	vds->length_ = sizeof(int);
	vds->data_ = new DATA_BYTE[vds->length_];
	getParameter(inputParameters_.serverAddress_, vds);
	vds->print();
	int workMode;
	getWorkingMode(inputParameters_.serverAddress_, &workMode);
	printf("Working mode: %d\n", workMode);

	SYSTEMTIME sysTime;
	GetLocalTime(&sysTime);
	for(unsigned int i = 0; i < 100; i++)
	{
	GetLocalTime(&sysTime);
	printf("TIME = %d:%d:%d:%d\n", sysTime.wHour, sysTime.wMinute, sysTime.wSecond, sysTime.wMilliseconds);
	VariantDatumStruct* vds2 = new VariantDatumStruct();
	vds2->id_ = PAR_SPINDLE_COUNT;
	vds2->type_ = TYPEINT;
	vds2->length_ = sizeof(int);
	vds2->data_ = new DATA_BYTE[vds2->length_];
	getParameter(inputParameters_.serverAddress_, vds2);
	vds2->print();
	delete vds2;
	}*/

	/*DataChunkStruct* dcs = new DataChunkStruct();
	dcs->totData_ = 100;
	dcs->data_ = new VariantDatumStruct[dcs->totData_];
	SYSTEMTIME sysTime;
	GetLocalTime(&sysTime);
	printf("TIME = %d:%d:%d:%d\n", sysTime.wHour, sysTime.wMinute, sysTime.wSecond, sysTime.wMilliseconds);
	for(unsigned int i = 0; i < 100; i++)
	{

	//printf("TIME = %d:%d:%d:%d", sysTime.wHour, sysTime.wMinute, sysTime.wSecond, sysTime.wMilliseconds);
	//VariantDatumStruct* vds2 = new VariantDatumStruct();
	//vds2->id_ = PAR_SPINDLE_COUNT;
	//vds2->type_ = TYPEINT;
	//vds2->length_ = sizeof(int);
	//vds2->data_ = new DATA_BYTE[vds2->length_];
	//dcs->data_[i] = vds2;
	dcs->data_[i].id_ = PAR_SPINDLE_COUNT;
	dcs->data_[i].length_ = sizeof(int);
	dcs->data_[i].type_ = TYPEINT;
	dcs->data_[i].data_ = new DATA_BYTE[dcs->data_[i].length_];
	//getParameter(inputParameters_.serverAddress_, vds2);
	//dcs->data_ = vds2;
	//delete vds2;
	//vds->print();
	}
	getParameters(inputParameters_.serverAddress_, dcs);
	GetLocalTime(&sysTime);
	printf("TIME = %d:%d:%d:%d\n", sysTime.wHour, sysTime.wMinute, sysTime.wSecond, sysTime.wMilliseconds);
	dcs->clear();*/
	//system("pause");
	//int spindleCount = 41;
	//memcpy(vds->data_, &spindleCount, sizeof(int));
	//setParameter(inputParameters_.serverAddress_, vds);
	//getParameter(inputParameters_.serverAddress_, vds);
	//int spindleCount2 = -1;
	//memcpy(&spindleCount2, vds->data_, sizeof(int));
	//printf("SPINDLE COUNT = %d\n", spindleCount2);
	//delete vds;
	//vds = new VariantDatumStruct();
	//vds->id_ = INFO_IP_ADDRESS;
	//vds->type_ = TYPESTRING;
	//vds->length_ = 32*sizeof(char);
	//vds->data_ = new DATA_BYTE[vds->length_];
	//getParameter(inputParameters_.serverAddress_, vds);
	//vds->print();

	/*
	err = connectDevice("127.0.0.1", 23456, false);
	if(err != ERROR_CODE_OK)
	{
	printf("Can't connect to device %s (%d)\n", inputParameters_.serverAddress_, err);
	return -1;
	}
	*/

	//unsigned int val = 2;
	//VariantDatum variantDatum1, variantDatum2;
	//variantDatum1.setData(INFO_SERIAL_NUMBER, reinterpret_cast<DATA_BYTE*>(&val));
	//variantDatum2.setData(PAR_WORKING_MODE, reinterpret_cast<DATA_BYTE*>(&val));

	//getInfo(inputParameters_.serverAddress_, variantDatum1);

	//setWorkingMode(inputParameters_.serverAddress_, WORKING_MODE_FREE_RUN);

	//printf("-----------> dato LOCALE\n");
	//variantDatum1.print();
	//printf("-----------> dato SCHEDA\n");
	//variantDatum2.print();

	//disconnect(inputParameters_.serverAddress_);
	//disconnect("192.168.0.101");

	//return 0;



	Opacimetro opacimetro;
	DataChunkStruct dataChunkStruct;
	memset(&dataChunkStruct, 0, sizeof(dataChunkStruct));
	DataChunk dataChunk;

	//APRO, CANCELLO IL CONTENUTO PRECEDENTE E CHIUDO IL FILE IN CUI SCRIVERE I DATI RICEVUTI

	FILE * file;
	file = fopen("receivedData.txt", "w");
	fclose(file);

	//====================================

	//opacimetro.connect(inputParameters_.serverAddress_, inputParameters_.port_, false);

	SYSTEMTIME startRecTime, stopRecTime;
	for(unsigned int i = 0; i < 3; i++)
	{
		/*	printf("\n---------------------------------\n");
		printf("---------------------------------\n");
		printf("Cycle: %d\n", i);*/
		//receiveDataOld(inputParameters_.serverAddress_, dataChunk);

		GetLocalTime(&startRecTime);
		int resRec = receiveData(inputParameters_.serverAddress_, &dataChunkStruct);
		GetLocalTime(&stopRecTime);
		if (resRec>0)
		{
			//dataChunk.print();
			//dataChunkStruct.print();
		}
		freeData(&dataChunkStruct);
		__int64 recTimeMs = (stopRecTime.wHour-startRecTime.wHour)*60*60*1000 +
			(stopRecTime.wMinute-startRecTime.wMinute)*60*1000 +
			(stopRecTime.wSecond-startRecTime.wSecond)*1000 +
			(stopRecTime.wMilliseconds-startRecTime.wMilliseconds);
		if (recTimeMs>320)
		{
			printf("REC TIME = %llu\n", recTimeMs);
		}


		//GetLocalTime(&startRecTime);
		//resRec = receiveData("10.200.0.151", &dataChunkStruct);
		//GetLocalTime(&stopRecTime);
		//if (resRec>0)
		//{
		//	//dataChunk.print();
		//	//dataChunkStruct.print();
		//}
		//freeData(&dataChunkStruct);
		//recTimeMs = (stopRecTime.wHour-startRecTime.wHour)*60*60*1000 +
		//	(stopRecTime.wMinute-startRecTime.wMinute)*60*1000 +
		//	(stopRecTime.wSecond-startRecTime.wSecond)*1000 +
		//	(stopRecTime.wMilliseconds-startRecTime.wMilliseconds);
		//if (recTimeMs>320)
		//{
		//	printf("REC TIME = %llu\n", recTimeMs);
		//}



		//dataChunk.writeValuesOnFile("receivedData.txt");
		Sleep(5);
	}

	//opacimetro.disconnect(inputParameters_.serverAddress_);

	err = disconnect(inputParameters_.serverAddress_);
	printf("DONE!\n");
	system("pause");

	return 0;
}

void ConsoleApp::clear()
{

}

void ConsoleApp::printSizes()
{
	printf("sizeof(bool) = %d\n", sizeof(bool));
	printf("sizeof(char) = %d\n", sizeof(char));
	printf("sizeof(unsigned char) = %d\n", sizeof(unsigned char));
	printf("sizeof(short) = %d\n", sizeof(short));
	printf("sizeof(int) = %d\n", sizeof(int));
	printf("sizeof(unsigned int) = %d\n", sizeof(unsigned int));
	printf("sizeof(float) = %d\n", sizeof(float));
	printf("sizeof(double) = %d\n", sizeof(double));
}
