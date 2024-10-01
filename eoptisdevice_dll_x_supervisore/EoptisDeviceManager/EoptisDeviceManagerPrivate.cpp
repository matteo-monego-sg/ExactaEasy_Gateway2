#include "EoptisDeviceManagerPrivate.h"

Opacimetro gOpacimetri[MAX_DEVICES];

Opacimetro* CALL_CONV getOpacimetroGivenIP(const char* ip)
{
	Opacimetro* opacimetro = 0;

	for(unsigned int i = 0; i < MAX_DEVICES && opacimetro == 0; i++)
	{
		if(strcmp(gOpacimetri[i].getIP(), ip) == 0)
			opacimetro = &(gOpacimetri[i]);
	}

	return opacimetro;
}

ERROR_CODE CALL_CONV connectDevicePrivate(const char* ip, unsigned int port, bool activeCallback)
{
	Opacimetro* opacimetro;

	opacimetro = getOpacimetroGivenIP("0.0.0.0");

	if(opacimetro == 0)
		return ERROR_CODE_MAX_CONNECTION;

	return opacimetro->connect(ip, port, activeCallback);
}


bool CALL_CONV createVD(VariantDatum* vd, const VariantDatumStruct* vds)
{
	vd->data_ = 0;
	if (vds->length_<=0)
		return false;

	//vd = new VariantDatum();
	vd->id_ = vds->id_;
	vd->type_ = vds->type_;
	vd->length_ = vds->length_;
	vd->data_ = new DATA_BYTE[vds->length_];
	memset(vd->data_, 0, sizeof(DATA_BYTE)*vds->length_);
	return true;
}

void CALL_CONV destroyVD(VariantDatum* vd)
{
	if(vd->data_ != nullptr)
		delete [] vd->data_;
	vd->data_ = nullptr;
}

void CALL_CONV populateVDS(VariantDatum* vd, VariantDatumStruct* vds)
{
	vds->id_ = vd->id_;
	vds->type_ = vd->type_;
	vds->length_ = vd->length_;
	memcpy(vds->data_, vd->data_, vds->length_);
}

bool CALL_CONV createDC(DataChunk* dc, const DataChunkStruct* dcs)
{
	dc->data_ = 0;
	if (dcs->totData_<=0)
		return false;
	//dc = new DataChunk();
	dc->totData_ = dcs->totData_;
	dc->data_ = new VariantDatum[dcs->totData_];
	for (unsigned int i=0; i<dcs->totData_; i++)
	{
		if (!createVD(&dc->data_[i], &dcs->data_[i]))
			return false;
	}
	return true;
}

void CALL_CONV destroyDC(DataChunk* dc)
{
	for (unsigned int i=0; i<dc->totData_; i++)
	{
		destroyVD(&dc->data_[i]);
	}
	delete[] dc->data_;
	dc->data_ = nullptr;
}

void CALL_CONV populateDCS(DataChunk* dc, DataChunkStruct* dcs)
{
	dcs->totData_ = dc->totData_;
	dcs->data_ = new VariantDatumStruct[dcs->totData_];
	for (unsigned int i=0; i<dcs->totData_; i++)
	{
		dcs->data_[i].data_ = new DATA_BYTE[dc->data_[i].length_];
		populateVDS(&dc->data_[i], &dcs->data_[i]);
	}
}
