/** \mainpage 

 *  @section intro INTRODUCTION
 *  
 *  Global Interface is a full set of API used to control Tattile's Devices.
 *   
 *   \image html Tattile.png
 *
 */

// itf_Antares_Global_Utility.h : file di intestazione principale per la DLL itf_Antares_Global_Utility
//

/*--------------------------------------------------------------
FILE ABSTRACT
                                                               
	Date:                                                   
		05 July 2002
                                                               
	Author:
		Luca Biancardi

	Description:                                      
		Defines the entry point for the DLL application
                                                               
	Release:
					
		Date:		 26 february 2003
		Author:		 Luca Biancardi
		Description: Modified enum ITF_ERROR_CODE.

		Date:
			20 march 2003
		Author:
			Luca Biancardi
		Description:
			Modified enum ITF_CHANNEL_CODE_PARAMETER.

  		Date:
			28 march 2003
		Author:
			Luca Biancardi
		Description:
			Added structs ITF_Time and ITF_Date.

		Date:
			13 june 2003
		Author:
			Luca Biancardi
		Description:
			ver.1.2.1
			Modified enum ITF_IMAGE_TYPE.

		Date:
			05 august 2003
		Author:
			Luca Biancardi
		Description:
			ver.1.3.2
			Modified enum ITF_CHANNEL_CODE_PARAMETER.
  
		Date:
			18 august 2003
		Author:
			Luca Biancardi
		Description:
			ver.1.4.0
			Modified enum ITF_CHANNEL_CODE_PARAMETER.

   		Date:
			26 september 2003
		Author:
			Luca Biancardi
		Description:
			Modified enum ITF_CHANNEL_CODE_PARAMETER.

   		Date:
			15 october 2003
		Author:
			Luca Biancardi
		Description:
			Modified enum ITF_CHANNEL_CODE_PARAMETER.

   		Date:
			09 december 2003
		Author:
			Luca Biancardi
		Description:
			Modified enum ITF_CHANNEL_CODE_PARAMETER.
			Enum ITF_DIRECTX_MESSAGE_FUNCTION has been added.

		Date:
			11 March 2004
		Author:
			Giuseppe Zilioli
		Description:
			ver.1.5.2
			Add ITF_ERROR_CMD_UNKNOWN to ITF_ERROR_CODE enum

		Date:
			04 May 2004
		Author:
			Giuseppe Zilioli
		Description:
			ver.1.5.3
			Add parameter ITF_CHANNEL_CODE_BUSY_TIME_OUT to
			ITF_CHANNEL_CODE_PARAMETER enum
			Add ITF_DEVICE_BUSY constant
Note:

---------------------------------------------------------------*/
#pragma once

extern "C"
{

#if 0
#ifndef __AFXWIN_H__
	#error "inclusione di 'stdafx.h' del file corrente per PCH"
#endif

#include "resource.h"		// simboli principali


// Citf_Antares_Global_UtilityApp
// Vedere itf_Antares_Global_Utility.cpp per l'implementazione di questa classe
//

class Citf_Antares_Global_UtilityApp : public CWinApp
{
public:
	Citf_Antares_Global_UtilityApp();

// Override
public:
	virtual BOOL InitInstance();

	DECLARE_MESSAGE_MAP()
};
#endif


// Indirizzo del client
#define ITF_ADDRESS_ID_PC 0xFFFFFFFF

// Special error from device if it is busy and cannot process our request
#define ITF_DEVICE_BUSY					123123
#define ITF_MIN_TIMEOUT_BUSY			200 // valore in ms

/*--------------------------------------------------------------
DATA ABSTRACT
                                                               
	Date:                                                   
		12 August 2002
                                                               
	Author:
		Luca Biancardi

	Structure name:
		ITF_ERROR_CODE

	Type:
		ITF_GLOBAL
                                                               
	Description:                                      
		Codici di errore globali
		Codici riservati da 0 a 1999
		
	Release:
		Date: 28 August 2002
		Author: Luca Biancardi
		Description: Added the error code ITF_ERROR_CODE_NOT_FOUND

		Date:		26 February 2003
		Author:		Luca Biancardi
		Description: Added the error code ITF_ERROR_CH_CLOSE
                                                               
	Note:

---------------------------------------------------------------*/
/** 
 *  
 * @defgroup ERROR_CODES Error codes
 */
/**@{*/
typedef enum
{
    ITF_OK						= 0,	// nessun errore
    ITF_ERROR					= 1,	// errore generico
    ITF_ERROR_TIMEOUT			= 2,	// timeout di lettura o scrittura
    ITF_ERROR_DIMENSION			= 3,	// dimensione del buffer troppo piccola
    ITF_ERROR_RACK				= 4,	// rack sconosciuto
    ITF_ERROR_MEMORY			= 5,	// menoria insufficiente
    ITF_ERROR_VALUE				= 6,	// Parametro passato alla funzione errato
    ITF_ERROR_FEWDATA			= 7,	// sono stati ricevuti meno dati di quelli dichiarati nell'header
    ITF_ERROR_MUCHDATA			= 8,	// sono stati ricevuti piu' dati di quelli dichiarati nell'header
    ITF_ERROR_RACK_CODE			= 9,	// rack con lo stesso codice
    ITF_ERROR_READ_EXIT			=10,	// ricevuto exit durante la lettura
    ITF_ERROR_READ_ABORT		=11,	// ricevuto abort durante la lettura
    ITF_ERROR_READ_BUFFER		=12,	// superate le dimensioni del buffer di ricezione
    ITF_ERROR_CHECKSUM			=13,	// checksum errato
    ITF_ERROR_CMD_DIFFERENT		=14,	// cmd diverso fra comando inviato e risposta ricevuta
    ITF_ERROR_WRITE_ABORT		=15,	// ricevuto abort durante la scrittura
    ITF_ERROR_CLEAN_READ_BUFFER	=16,	// la funzione CleanBuffer ha trovato dati nel buffer di lettura
    ITF_ERROR_CH_HANDLE	        =17,	// HANDLE del canale di comunicazione corrotto
    ITF_ERROR_NULL_POINTER	    =18,	// NULL pointer passed to a function
	ITF_ERROR_WRONG_STRUCT_SIZE =19,	// Wrong struct size
	ITF_ERROR_CODE_NOT_FOUND    =20,	// Error code not found
	ITF_ERROR_CH_CLOSE			=21,	// Channel is closed
	ITF_TRACE					=22,	// This is just a trace not an error
	ITF_ERROR_CMD_UNKNOWN		=2008   // commando sconosciuto
} ITF_ERROR_CODE;
/**@}*/ // end of group ERROR_CODES

/*--------------------------------------------------------------
DATA ABSTRACT
                                                               
	Date:                                                   
		12 August 2002
                                                               
	Author:
		Luca Biancardi

	Structure name:
		ITF_PARAMETER_CODE

	Type:
		ITF_GLOBAL
                                                               
	Description:                                      
		Elenco dei codici dei parametri
		
	Release:
		Date:
		Author:
		Description:
                                                               
	Note:

---------------------------------------------------------------*/
/** 
 *  
 * @defgroup PARAMETER_CODES Parameter codes
 */
/**@{*/
typedef enum
{
	// codici dei parametri da passare alla funzione itf_enelReadParameters()
	ITF_CODE_NULL        = -1,		// Nessun Dato
	ITF_CODE_SN_DEVICE   = 0,		// Serial Number.
	ITF_CODE_SW_DESC,				// Descrizione Software.
	ITF_CODE_SW_DATE,				// Data Creazione del Software.
	ITF_CODE_SW_HOUR,				// Ora Creazione del Software.
	ITF_CODE_SW_VERSION,			// Versione del Software 
	ITF_CODE_HW_MODEL,				// Modello Hardware (Codifica Scheda).
	ITF_CODE_HW_VERSION,			// Versione Hardware.
	ITF_CODE_TASK_STATE,		    // Stato del task di lavoro
                                    // 0 = in attesa di comando, 1 = occupato
	ITF_CODE_DATE,					// Data interna Dispositivo.
	ITF_CODE_HOUR,					// Ora interna Dispositivo.
	ITF_CODE_FLASH_SIZE,            // Dimensione della flash in byte
	ITF_CODE_RAM_SIZE,              // Dimensione della memoria in byte
	ITF_CODE_CAMERA_HEAD,           // Numero di telecamere (testine) collegate
	ITF_CODE_IMAGE_INFO,			// Informazioni sulla immagine acquisita 
									// struttura ITF_ParameterImageInfo
	ITF_CODE_DIRECTX,               // 0 non gestisce diretta veloce, 1 si
	ITF_CODE_IP_CONFIGURATION,      // configurazione ip del dispositivo
									// struttura ITF_ParameterIPConfiguration
	ITF_CODE_RACK_ID,				// RackID del dispositivo
	ITF_CODE_DEVICE_TYPE = 20,		// Codice del dispositivo (16/20 -> TriLinear)
	ITF_CODE_MAC_ADDRESS = 10000,
	ITF_CODE_IP_ADDRESS,
    ITF_CODE_NETMASK,
    ITF_CODE_GATEWAY,
    ITF_CODE_DNS,
    ITF_CODE_WINS,
    ITF_CODE_DHCP,
    ITF_CODE_NETBIOSNAME = 10008,
    ITF_CODE_TIMESERVER,
    ITF_CODE_DEVICE_INFORMATIONS,
	ITF_CODE_WIFI_RADIO_SNR,
	ITF_CODE_WIFI_STATUS,
	ITF_CODE_BOARD_CODE,
	ITF_CODE_BOARD_REVISION,
	ITF_CODE_FFS_TOTAL_SIZE,
	ITF_CODE_FFS_USED_SIZE,
	ITF_CODE_FFS_FREE_SIZE,
	ITF_CODE_FFS_DEAD_SIZE,
	ITF_CODE_TEMPERATURE,
	ITF_CODE_FPGA_VERSION,
	ITF_CODE_CPU_TIME,
	ITF_CODE_MEMORY_TOTAL_SIZE,
	ITF_CODE_MEMORY_USED_SIZE,
	ITF_CODE_MEMORY_FREE_SIZE,
    ITF_CODE_DATE2,
    ITF_CODE_TIME,
	ITF_CODE_HD_FREE_SIZE,
	ITF_CODE_HD_TOTAL_SIZE,
	ITF_CODE_HD_USED_SIZE,
	ITF_CODE_HD_TOTAL_CLUSTERS,
	ITF_CODE_HD_BYTES_PER_SECTOR,
	ITF_CODE_HD_SECTORS_PER_CLUSTER,
} ITF_PARAMETER_CODE;
/**@}*/ // end of group PARAMETER_CODES

    
    

/*--------------------------------------------------------------
DATA ABSTRACT
                                                               
	Date:                                                   
		05 August 2002
                                                               
	Author:
		Luca Biancardi

	Structure name:
		ITF_COM_PARITY

	Type:
		ITF_GLOBAL
                                                               
	Description:                                      
		codici parita' porta seriale su hardware TATTILE
		
	Release:
		Date:
		Author:
		Description:
                                                               
	Note:

---------------------------------------------------------------*/
typedef enum
{
	ITF_PARITY_NONE = 0,
	ITF_PARITY_ODD  = 1,
	ITF_PARITY_EVEN = 3
} ITF_COM_PARITY;

/*--------------------------------------------------------------
DATA ABSTRACT
                                                               
	Date:                                                   
		25 July 2002
                                                               
	Author:
		Luca Biancardi

	Structure name:
		ITF_IMAGE_FORMAT

	Type:
		ITF_GLOBAL
                                                               
	Description:                                      
		Codes of the format image type
		
	Release:
		Date:
		Author:
		Description:
                                                               
	Note:

---------------------------------------------------------------*/
typedef enum
{
	ITF_IMAGE_ROW      = 0,
	ITF_IMAGE_BMP      = 1,
	ITF_IMAGE_JPEG     = 2
} ITF_IMAGE_FORMAT;

/*--------------------------------------------------------------
DATA ABSTRACT
                                                               
	Date:                                                   
		09 december 2003
                                                               
	Author:
		Luca Biancardi

	Structure name:
		ITF_DIRECTX_MESSAGE_FUNCTION

	Type:
		ITF_GLOBAL
                                                               
	Description:                                      
		Codes of the function that the directx thread has to use to
		send message.		
		ITF_DIRECTX_POST_MESSAGE : PostMessage function is used 
		ITF_DIRECTX_SEND_MESSAGE : SendMessage function is used

	Release:
		Date:
		Author:
		Description:
                                                               
	Note:

---------------------------------------------------------------*/
typedef enum
{
	ITF_DIRECTX_POST_MESSAGE      = 0,
	ITF_DIRECTX_SEND_MESSAGE      = 1
} ITF_DIRECTX_MESSAGE_FUNCTION;

/*--------------------------------------------------------------
DATA ABSTRACT
                                                               
	Date:                                                   
		25 July 2002
                                                               
	Author:
		Luca Biancardi

	Structure name:
		ITF_CHANNEL_CODE_PARAMETER

	Type:
		ITF_GLOBAL
                                                               
	Description:                                      
		List of the parameter codes for functions itf_SetParameter()
		and itf_GetParameter().
		
	Release:
		Date:
			20 march 2003
		Author:
			Luca Biancardi
		Description:
			Added code ITF_CHANNEL_CODE_OPEN_PARAMETERS
                                                               
		Date:
			28 march 2003
		Author:
			Luca Biancardi
		Description:
			Added codes 				
				ITF_CHANNEL_CODE_DIRECTX_TCP
				ITF_CHANNEL_CODE_DIRECTX_DEVICE_PORT
				ITF_CHANNEL_CODE_ASYNCH			
				ITF_CHANNEL_CODE_HWND					
				ITF_CHANNEL_CODE_HWND_WM_MESSAGE		

		Date:
			05 august 2003
		Author:
			Luca Biancardi
		Description:
			ITF_CHANNEL_CODE_DIRECTX_EVENT code is added.

  		Date:
			18 august 2003
		Author:
			Luca Biancardi
		Description:
			ITF_CHANNEL_CODE_AUTOMATIC_OPEN,
			ITF_CHANNEL_CODE_DIRECTX_ARRAY_MESSAGE_SIZE and
			ITF_CHANNEL_CODE_DIRECTX_KEEP_ALIVE_ANSWER
			codes are added.

  		Date:
			26 september 2003
		Author:
			Luca Biancardi
		Description:
			ITF_CHANNEL_CODE_DIRECTX_MODE code is added.
  
  		Date:
			15 october 2003
		Author:
			Luca Biancardi
		Description:
			ITF_CHANNEL_CODE_DIRECTX_ANTARES code is added.

  		Date:
			09 december 2003
		Author:
			Luca Biancardi
		Description:
			ITF_CHANNEL_CODE_DIRECTX_MESSAGE_FUNCTION and 
			ITF_CHANNEL_CODE_DIRECTX_ALLOWED codes have been added.
Note:

---------------------------------------------------------------*/
/** 
 *  
 * @defgroup CHANNEL_CODE_PARAMETERS Channel code parameters
 */
/**@{*/
typedef enum
{
	ITF_CHANNEL_CODE_TIMEOUT_RX					    = 1, // timeout di lettura in ms
	ITF_CHANNEL_CODE_TIMEOUT_TX						= 2, // timeout di scrittura in ms
	ITF_CHANNEL_CODE_RACK_ID						= 3, // RackID del dispositivo
	ITF_CHANNEL_CODE_BUFFER_SIZE_RX					= 4, // dimension, in bytes, of the rx buffer	
	ITF_CHANNEL_CODE_BUFFER_SIZE_TX					= 5, // dimension, in bytes, of the tc buffer	
	ITF_CHANNEL_CODE_DELAY_BETWEEN_PACKETS			= 6, // delay, in microseconds, between packets
	ITF_CHANNEL_CODE_BROADCAST						= 7, // Allows transmission of broadcast messages. 
	ITF_CHANNEL_CODE_TRIES							= 8, // number of tries in case of error
	ITF_CHANNEL_CODE_DISABLE						= 9, // enable / disable the channel
	ITF_CHANNEL_CODE_LAN							=10, // Lan type (ITF_CHANNEL_LAN_TYPE). Read only
	ITF_CHANNEL_CODE_DIRECTX_BUFFER_SIZE_RX			=11, // dimension, in bytes, of the rx buffer for directX socket
	ITF_CHANNEL_CODE_DIRECTX_STATUS					=12, // DirectX status: 0 off, 1 on
	ITF_CHANNEL_CODE_DIRECTX_TIMER					=13, // DirectX time, in ms, for keep alive command
	ITF_CHANNEL_CODE_DIRECTX_RECEIVED_MESSAGE		=14, // DirectX received messages
	ITF_CHANNEL_CODE_DIRECTX_LOST_MESSAGE			=15, // DirectX lost messages
    ITF_CHANNEL_CODE_DIRECTX_HWND					=16, // receiving window message
    ITF_CHANNEL_CODE_DIRECTX_HWND_WM_MESSAGE		=17, // message to send
    ITF_CHANNEL_CODE_OPEN_PARAMETERS				=18, // Open parameters
    ITF_CHANNEL_CODE_DIRECTX_TCP					=19, // enable TCP directx
    ITF_CHANNEL_CODE_DIRECTX_DEVICE_PORT			=20, // Device Ethernet Port
	ITF_CHANNEL_CODE_ASYNCH							=21, // Allows asynchronous write and read
    ITF_CHANNEL_CODE_HWND							=22, // receiving window message
    ITF_CHANNEL_CODE_HWND_WM_MESSAGE				=23, // message to send when a message is received or an error happens
    ITF_CHANNEL_CODE_DIRECTX_EVENT					=24, // event to signal when a message is received or an error happens
    ITF_CHANNEL_CODE_AUTOMATIC_OPEN					=25, // dll automatically closes and opens the TCP channel 
    ITF_CHANNEL_CODE_DIRECTX_ARRAY_MESSAGE_SIZE		=26, // size of the array for the message read
    ITF_CHANNEL_CODE_DIRECTX_KEEP_ALIVE_ANSWER		=27, // Allows wait for Keep Alive Answer.
    ITF_CHANNEL_CODE_DIRECTX_MODE					=28, // ITF_ETH_DIRECTX_PORT_MODE or ITF_ETH_DIRECTX_PASSIVE_MODE 
    ITF_CHANNEL_CODE_DIRECTX_ANTARES_COMPATIBILITY	=29, // set to 1 for antares device compatibility
    ITF_CHANNEL_CODE_DIRECTX_MESSAGE_FUNCTION	    =30, // Possible values are: ITF_DIRECTX_POST_MESSAGE and ITF_DIRECTX_SEND_MESSAGE.
    ITF_CHANNEL_CODE_DIRECTX_ALLOWED				=31, // ReadOnly Properties = TRUE if DirectX is allowed
	ITF_CHANNEL_CODE_DIRECTX_ABORT_EVENT			=32, // event to signal when directX stop unexpectedly
	ITF_CHANNEL_CODE_SLOT_ID						=33, // SlotID del dispositivo
	ITF_CHANNEL_CODE_BUSY_TIME_OUT					=34	 //busy time out
} ITF_CHANNEL_CODE_PARAMETER;
/**@}*/ // end of group CHANNEL_CODE_PARAMETERS

/*--------------------------------------------------------------
DATA ABSTRACT
                                                               
	Date:                                                   
		25 July 2002
                                                               
	Author:
		Luca Biancardi

	Structure name:
		ITF_CHANNEL_LAN_TYPE

	Type:
		ITF_GLOBAL
                                                               
	Description:                                      
		List of the code of the different lan.
		
	Release:
		Date:
			20 march 2003
		Author:
			Luca Biancardi
		Description:
			Added code ITF_CHANNEL_CODE_OPEN_PARAMETERS
                                                               
	Note:

---------------------------------------------------------------*/
typedef enum
{
	ITF_CHANNEL_LAN_FIREWIRE				= 0, // rete firewire
	ITF_CHANNEL_LAN_COM						= 1, // seriale RS232, RS485
	ITF_CHANNEL_LAN_ETHERNET				= 2  // rete ethernet
} ITF_CHANNEL_LAN_TYPE;

/*--------------------------------------------------------------
Structure
---------------------------------------------------------------*/
#pragma pack(1) 

/*--------------------------------------------------------------
DATA ABSTRACT
                                                               
	Date:                                                   
		25 July 2002
                                                               
	Author:
		Luca Biancardi

	Structure name:
		ITF_AddressID

	Type:
		ITF_GLOBAL
                                                               
	Description:                                      
		Structure for the Device address
		
	Release:
		Date:
		Author:
		Description:
                                                               
	Note:

---------------------------------------------------------------*/
typedef union
{
    ULONG  Value;
    struct
    {
        unsigned short 
			SlotID,
			RackID;
    }sr;
}ITF_AddressID;

/*--------------------------------------------------------------
DATA ABSTRACT
                                                               
	Date:                                                   
		25 July 2002
                                                               
	Author:
		Luca Biancardi

	Structure name:
		ITF_CommandHeader

	Type:
		ITF_GLOBAL
                                                               
	Description:                                      
		
	Release:
		Date:
		Author:
		Description:
                                                               
	Note:

---------------------------------------------------------------*/
typedef struct{
	long			HeaderDimension;		// Dimensione dell'header in bytes
	long			Code;					// Codice comando	
	ITF_AddressID	Sender;				// ID mittente
	ITF_AddressID	Receiver;				// ID destinatario
	long			error;				// codice di errore
	long			DataDimension;		// Dimensione dei dati in bytes			
} ITF_CommandHeader;

/*--------------------------------------------------------------
DATA ABSTRACT
                                                               
	Date:                                                   
		25 July 2002
                                                               
	Author:
		Luca Biancardi

	Structure name:
		ITF_CommandLong

	Type:
		ITF_GLOBAL
                                                               
	Description:                                      
		
	Release:
		Date:
		Author:
		Description:
                                                               
	Note:

---------------------------------------------------------------*/
typedef struct{
	ITF_CommandHeader 
		header;
	long	          
		value;
} ITF_CommandLong;

/*--------------------------------------------------------------
DATA ABSTRACT
                                                               
	Date:                                                   
		25 July 2002
                                                               
	Author:
		Luca Biancardi

	Structure name:
		ITF_StructHeader

	Type:
		ITF_GLOBAL
                                                               
	Description:                                      
		
	Release:
		Date:
		Author:
		Description:
                                                               
	Note:

---------------------------------------------------------------*/
typedef struct
{
    ULONG 
		code,   	// Codice identificativo del tipo oggetto
		dimension,  // Dimensione header di oggetto in byte 
		version;    // numero della versione
} ITF_StructHeader;

#pragma pack() 

typedef enum
{
	FORMAT_RAW,
	FORMAT_BMP,
} Enum_Image_Format;

/* StreamingChannelType as supported by hardware */
#define IMAGE_PROTOCOL_DIRECT_X 0   /* This is the old communication format */ 
#define IMAGE_PROTOCOL_REGULAR  1   /* Not used in Antares but only in TAG devices */
#define IMAGE_PROTOCOL_GIGE     2   /* GigeVision communication protocol */
#define IMAGE_PROTOCOL_TOJECT   3   /* Tattile Backwall communication protocol */


/* -------------------------------------------------------------------------------- */
/* DirectX + TObject                                                                */
/* -------------------------------------------------------------------------------- */
typedef enum
{
    /** none error                                                   */   
    TAG_FILTER_OK                        = 0,  
    /** No incoming data on socket                                  */  
    TAG_FILTER_READ_NO_DATA              =51,  
    /** Filter lost a frame in reading                              */  
    TAG_FILTER_FRAME_LOST                =52,  
}  TAG_FILTER_ERROR_CODE;


enum RESULT_OBJECT_CODE
{
    RESULT_OBJECT_CODE_UNKNOWN          = 0x0000,
    RESULT_OBJECT_CODE_LIVE_IMAGE       = 0x0001,   // Original grabbed image
    RESULT_OBJECT_CODE_PROCESS_IMAGE    = 0x000a,   // Processed image
    RESULT_OBJECT_CODE_PROCESS_RESULTS  = 0x0090,   // Process result data
    RESULT_OBJECT_CODE_LAST
};
/* -------------------------------------------------------------------------------- */




//-----------------------------------------------------------------------------
//--- Exported functions prototypes -------------------------------------------
//-----------------------------------------------------------------------------
#pragma pack(1)
//--- Numerazione codici parametri da rimandare al chiamante
typedef enum
{
        ITF_REQUEST_CAMERA_HEAD         = 0,
        ITF_REQUEST_DEVICE_TYPE         = 1,
        ITF_REQUEST_HW_MODEL            = 2,
        ITF_REQUEST_HW_VERSION          = 3,
        ITF_REQUEST_NUM_COLOR_BANKS     = 4,
        ITF_REQUEST_NUM_BW_BANKS        = 5,
        ITF_REQUEST_SERIAL_NUMBER       = 6,
        ITF_REQUEST_IMAGE_HEIGHT        = 7,
        ITF_REQUEST_IMAGE_WIDTH         = 8,
        ITF_REQUEST_IMAGE_TYPE          = 9,
        ITF_REQUEST_IMAGE_PROTOCOL      = 10,   /** This is to detect if device manage DirectX or TObject or GigeVision  */
        ITF_REQUEST_LAST_ENUM           = 11    // NON USARE! AGGIORNARE CON ULTIMA DEFINIZIONE + 1
} Enum_Parameter_Request;

//--- Image format and type management
#define GRAY_8		 0
#define	RGB_565		 1
#define	RGB_323		 2
#define	RGB_888		 3
#define	BAYER		 4
#define	MOSAIC		 5
#define	YCBCR420	 6
#define BGR888		 7
#define	CRYCBY422	 8
#define	RGBY_32BIT	 9		/* Used only for TAPIX trilinear camera                            */
#define GRAY_16     10
#define	BAYER_10BIT	11
#define RGB0_32BIT	12		/* TAG2 Trilinear GigaBit (RGB+8bit a 0)                           */
#define BGR888_HF	13		/* TAG2 Trilinear GigaBit (BGR888 + Header + Footer)               */
#define GRAY_8_HF   14		/* TAG2 PSeries e Quadrilinear GigaBit (GRAY_8 + Header + Footer)  */                           
#define BGR565_HF	15		/* TAG2 Trilinear GigaBit (BGR565 + Header + Footer)               */
#define YCBCR_111	16
#define BAYER_GR    20      /* Bayer filter output starting with Green and Red pixel           */
#define BAYER_RG    21      /* Bayer filter output starting with Red and Green pixel           */
#define BAYER_GB    22      /* Bayer filter output starting with Green and Blue pixel          */
#define BAYER_BG    23      /* Bayer filter output starting with Blue and Green pixel          */

//Image format defininitions
#define TIL_IMAGE_FORMAT_RAW			0
#define TIL_IMAGE_FORMAT_JPG			1
#define TIL_IMAGE_FORMAT_BMP			2
#define TIL_IMAGE_FORMAT_H261			3
#define TIL_IMAGE_FORMAT_BINICO			4
#define TIL_IMAGE_FORMAT_WAVELET_601	5
#define TIL_IMAGE_FORMAT_MPEG4			6
#define TIL_IMAGE_FORMAT_MPEG4_FIELD	7
#define TIL_IMAGE_FORMAT_JPG_FIELD		8
#define TIL_IMAGE_FORMAT_IME6400		9
#define TIL_IMAGE_FORMAT_IME6400_FIELD	10

// TAG SPECIFIC IMAGE TYPES
#define TIL_IMAGE_FORMAT_RAW_LINEAR		100

//Pixel type defininitions
#define TIL_PIXEL_TYPE_Y						GRAY_8		// 0
#define TIL_PIXEL_TYPE_RGB565_INTERLEAVED		RGB_565		// 1
#define TIL_PIXEL_TYPE_RGB323_INTERLEAVED		RGB_323		// 2
#define TIL_PIXEL_TYPE_RGB888_INTERLEAVED		RGB_888		// 3
#define TIL_PIXEL_TYPE_BAYER					BAYER		// 4
#define TIL_PIXEL_TYPE_MOSAIC					MOSAIC		// 5
#define TIL_PIXEL_TYPE_YCBCR420					YCBCR420	// 6
#define TIL_PIXEL_TYPE_BGR888_INTERLEAVED		BGR888		// 7
#define TIL_PIXEL_TYPE_CRYCBY422_INTERLEAVED	CRYCBY422	// 8
#define TIL_PIXEL_TYPE_RGBY_INTERLEAVED			RGBY_32BIT	// 9
#define TIL_PIXEL_TYPE_BAYER_10BIT				BAYER_10BIT	// 11
#define TIL_PIXEL_TYPE_RGB0_32BIT				RGB0_32BIT	// 12
#define TIL_PIXEL_TYPE_BGR888_HF				BGR888_HF	// 13
#define TIL_PIXEL_TYPE_Y_H						GRAY_8_H	// 14

typedef enum
{
    etv_Unknow              = -1,
    etv_Long                = 0,                 //Long (integer 32 bits)
    etv_SingleFloat         = 1,                 //Single
    etv_DoubleFloat         = 2,                 //Double
    etv_String              = 3,                 //String
    etv_Binary              = 4                  //binary or complex variable (Manhattan)
} Enum_Data_Type;

typedef struct
{
        long                            dataType;               // Enum_Data_Type
        char                            stringa[256];
} tatStringa;

typedef struct 
{
        char tableName[256];
        char variableName[256];
        char value[256];
        long sheet;
        Enum_Data_Type varType;
} ITF_VariableType;

#define LONG_ALIGN(x) ((x) % 4 ? ((x) + (4 - ((x) % 4))) : x)

typedef union 
{
	long datumLong;
	float datumFloat;
} CustomDatum;

typedef struct
{
	unsigned long code;
	unsigned long num;
	unsigned long headerDim;
	unsigned long dim;
} TObjInfo;

typedef struct 
{
	TObjInfo hd;
	Enum_Data_Type type;
	CustomDatum value;
} TTabField;

/*--------------------------------------------------------------
DATA ABSTRACT
                                                               
	Date:                                                   
		25 July 2002
                                                               
	Author:
		Luca Biancardi

	Structure name:
		ITF_AddressIP

	Type:
		ITF_GLOBAL
                                                               
	Description:                                      
		
	Release:
		Date:
		Author:
		Description:
                                                               
	Note:

---------------------------------------------------------------*/
typedef union
{
    ULONG  
		Value;
    struct
    {
		unsigned char	
			adr4,
			adr3,
			adr2,
			adr1;
    }adr;
}ITF_AddressIP;

/*--------------------------------------------------------------
DATA ABSTRACT
                                                               
	Date:                                                   
		25 July 2002
                                                               
	Author:
		Luca Biancardi

	Structure name:
		ITF_MacAddress

	Type:
		ITF_GLOBAL
                                                               
	Description:                                      
		
	Release:
		Date:
		Author:
		Description:
                                                               
	Note:

---------------------------------------------------------------*/
typedef union
{
    struct
    {
		long
			l_high,
			l_low;
	}un_l;
    struct
    {
		unsigned char	
			b[6],
			b_empty2,
			b_empty1;
    }un_b;
}ITF_MacAddress;

typedef struct
{
	unsigned long  dDay;			// 1...31
    unsigned long  dMonth;			// 1...12
    unsigned long  dYear;			// 00...99 in lettura e accetta 2xxx in scrittura
} ITF_Date;

typedef struct
{
	unsigned long tSecond;			// 0...59
	unsigned long tMinute;			// 0...59
	unsigned long tHour;			// 0...23
	unsigned long tMillisecond;		// 0...999
} ITF_Time;

typedef struct
{
        long    TypeImage;                                                                              // long Type (enum ITF_IMAGE_TYPE)
        long    Width;                                                                                  // long Width dimensione x in pixel della immagine
        long    Height;                                                                                 // long Height dimensione y in pixel della immagine
        long    ScaledWidth;                                                                    // dimensione x in pixel della immagine scalata
        long    ScaledHeight;                                                                   // dimensione y in pixel della immagine scalata
} TDevImgInfo;

typedef struct
{
	ITF_AddressIP IPDevice;
	ITF_AddressIP NetMask;
	ITF_AddressIP Gateway;
	ITF_AddressIP TimeServer;
	ITF_AddressIP DNS;
	ITF_AddressIP Wins;
	bool Dhcp;
	unsigned long SerialNumber;
	ITF_MacAddress MACAddress;
	char Description[128];
	char NetBiosName[128];
	int WiFiRadioSNR;
	int WiFiStatus;
	int BoardCode;
	int BoardRevision;
	unsigned long FFSTotalSize;
	unsigned long FFSFreeSize;
	unsigned long FFSUsedSize;
	unsigned long FFSDeadSize;
	int Temperature;
	int FPGAVersion;
	unsigned long CPUTime;
	unsigned long MemoryTotalSize;
	unsigned long MemoryFreeSize;
	unsigned long MemoryUsedSize;
	ITF_Date DateDevice;
	ITF_Time TimeDevice;
	unsigned long HDTotalSize;
	unsigned long HDFreeSize;
	unsigned long HDUsedSize;
	unsigned long HDTotalClusters;
	unsigned long HDBytesPerSector;
	unsigned long HDSectorsPerClusters;
	int RackID;
	int Num_Banchi_Image;
	int Num_Banchi_Color;
	int Num_Banchi_BW;
	TDevImgInfo LiveImgInfo;
	TDevImgInfo ResultImgInfo;
	TDevImgInfo LiveResultImgInfo;
	int StreamingChannelType;

	bool FlagIPDevice;
	bool FlagNetMask;
	bool FlagGateway;
	bool FlagTimeServer;
	bool FlagDNS;
	bool FlagWins;
	bool FlagDhcp;
	bool FlagSerialNumber;
	bool FlagMacAddress;
	bool FlagDescription;
	bool FlagNetBiosName;
	bool FlagWiFiRadioSNR;
	bool FlagWiFiStatus;
	bool FlagBoardCode;
	bool FlagBoardRevision;
	bool FlagFFSTotalSize;
	bool FlagFFSFreeSize;
	bool FlagFFSUsedSize;
	bool FlagFFSDeadSize;
	bool FlagTemperature;
	bool FlagFPGAVersion;
	bool FlagCPUTime;
	bool FlagMemoryTotalSize;
	bool FlagMemoryFreeSize;
	bool FlagMemoryUsedSize;
	bool FlagDate;
	bool FlagTime;
	bool FlagHDTotalSize;
	bool FlagHDFreeSize;
	bool FlagHDUsedSize;
	bool FlagHDTotalClusters;
	bool FlagHDBytesPerSector;
	bool FlagHDSectorsPerClusters;
	bool FlagRackID;
	bool FlagNum_Banchi_Image;
	bool FlagNum_Banchi_Color;
	bool FlagNum_Banchi_BW;
	bool FlagLiveImgInfo;
	bool FlagResultImgInfo;
	bool FlagLiveResultImgInfo;
	bool FlagStreamingChannelType;
} Type_DeviceInfo;

#pragma pack()

/** 
 *  
 * @defgroup COMUNICATION_FUNCTIONS Comunication functions
 */
/**@{*/

/**
 * <b><i>Open a comunication channel with the selected device
 * (by ip_address)</i></b><br>
 * 
 * This is the first API to use to access to an device where is
 * running Antares Engine. With this API we can get the Handle
 * of the Channel and the RackID of the Device. If the array
 * DeviceInformations is filled with the Request Codes
 * (Enum_Parameter_Request), the function translate the request
 * code in to the corresponding values.
 * 
 * @param IP_Address device IP address in dotted string format
 *                   (ie 192.168.0.111)
 * @param Handle     (out) resulting HANDLE (null if error)
 * @param RackID     (out) Pointer for the resulting RackID
 *                   (use NULL if you don't need)
 * @param DeviceInformations
 *                   array of required info. Each code
 *                   is replaced by his value. Use NULL
 *                   if this is not necessary
 * @param DevInfosCount
 *                   numer of long in the array (use 0 if not
 *                   needed)
 * @param BufferDimension
 *                   (optional) use -1 to let the function
 *                   allocate the buffer.
 * 
 * @return 0 (ITF_OK) or an error code
 * @see Enum_Parameter_Request
 * @see itf_CloseChannel
 * @see itf_Read_Device_Informations
 */

long WINAPI itf_OpenChannel(char* IP_Address, HANDLE* Handle, long* RackID, void* DeviceInformations, long DevInfosCount, long BufferDimension  = -1 );
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>Close the communication channel</i></b><br>
 * 
 * @param Handle (in)address of the channel handle created by
 *               itf_OpenChannel
 * 
 * @return 0 (ITF_OK) or an error code
 * @see itf_OpenChannel
 */

long WINAPI itf_CloseChannel(HANDLE* Handle);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>Get RackID from the handle</i></b><br>
 * Because this parameter is stored inside the Handle, no
 * operation on communication channel are required.
 * 
 * @param Handle (in)channel handle as from
 *               itf_OpenChannel
 * @param RackID (out) resulting RackId
 * 
 * @return 0 (ITF_OK) or an error code
 * @see itf_OpenChannel
 */

long WINAPI itf_GetChannelRackID(HANDLE Handle, long* RackID);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>Read the TSK version from the Device</i></b><br>
 * 
 * @param Handle channel handle as from itf_OpenChannel
 * @param TSK_Version
 *               NULL is not valid,
 *               if *TSK_Version == NULL the memory is allocated internally
 *               and ift_Free must be used to release memory
 *               Otherwise memory can be allocated by the caller
 *               function. Use a buffer of at least 128 bytes
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_Free
 */

long WINAPI itf_Read_TSK_Version(HANDLE Handle, char **TSK_Version);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>Read device information and print everything in a string</i></b><br>
 * <p>_u is the UNICODE implementation</p>
 * Every value is printed on a line There is a description text
 * followed by column (:) and by the value.
 * 
 * @param Handle communication handle as from
 *               itf_OpenChannel
 * @param buffer pointer to a buffer allocated by the
 *               caller function
 * @param buffer_byte_sz
 *               (in)buffer size. If not enough the text
 *               is clipped and terminated
 * @param output_byte_sz
 *               (out) pointer to the value for the
 *               resulting text size. Use this parameter to
 *               know how much memory is necessary
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 */

long WINAPI itf_Read_Device_Informations_u(HANDLE Handle, unsigned short* buffer, long buffer_byte_sz, long* output_byte_sz);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>Read device information and print everything in a string</i></b><br>
 * Every value is printed on a line There is a description text
 * followed by column (:) and by the value.
 * 
 * @param Handle communication handle as from
 *               itf_OpenChannel
 * @param buffer pointer to a buffer allocated by the
 *               caller function
 * @param buffer_byte_sz
 *               (in)buffer size. If not enough the text
 *               is clipped and terminated
 * @param output_byte_sz
 *               (out) pointer to the value for the
 *               resulting text size. Use this parameter to
 *               know how much memory is necessary to store
 *               the results
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 */

long WINAPI itf_Read_Device_Informations(HANDLE Handle, char* buffer, long buffer_byte_sz, long* output_byte_sz);
//------------------------------------------------------------------------------------------------------------------

/**
 * <b><i>Read device information and print everything in a string</i></b><br>
 * <p>_u is the UNICODE implementation</p>
 * Every value is printed on a line There is a description text
 * followed by column (:) and by the value.
 * 
 * @param Handle communication handle as from
 *               itf_OpenChannel
 * @param device    pointer to a buffer allocated by the
 *                  caller function
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 */

long WINAPI itf_Read_Device_Informations_Ex(HANDLE Handle, Type_DeviceInfo *device);
//------------------------------------------------------------------------------------------------------------------

/**
 * <b><i>The function close a communication channel. 
 * The pointer chHandle is setted to NULL.</i></b><br>
 * 
 * @param chHandle (in/out) communication handle as from
 *               itf_OpenChannel
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 */

long WINAPI itf_Close(HANDLE *chHandle);


/**
 * <b><i>Function for the parameters configuration</i></b><br>
 * 
 * @param chHandle	communication handle as from
 *					itf_OpenChannel
 * @param code		parameter code (enum ITF_CHANNEL_CODE_PARAMETER)
 * @param value		(in) pointer to the parameter
 * @param size		(in) dimension, in byte, of the parameter value
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 */

long WINAPI itf_SetParameter(HANDLE chHandle, long code, const void *value, long size);


/**
 * <b><i>Function to read the configuration channel parameters.</i></b><br>
 * 
 * @param chHandle	communication handle as from
 *					itf_OpenChannel
 * @param code		parameter code (enum ITF_CHANNEL_CODE_PARAMETER)
 * @param value		(out) pointer to the parameter
 * @param size		(in) dimension, in byte, of the parameter value
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 */

long WINAPI itf_GetParameter(HANDLE chHandle, long code, void *value, long size);
/**@}*/ // end of group COMUNICATION_FUNCTIONS

/** 
 *  
 * @defgroup DIRECTX_FUNCTIONS DirectX functions
 */
/**@{*/

typedef void (*ImageCallback)(unsigned long);

typedef struct
{
	unsigned long RxBufferDim;
	int Timer;
	int ArrayMsgSixe;
	int DevEthPort;
	int AntaresCompatibility;
	int WaitKeepAlive;
	int PassiveMode;
	int ImageSampling;
	ImageCallback ImgCallback;
} Type_Directx_Parameters;

long WINAPI itf_StartDirectX(HANDLE Handle, HWND hWnd, int messageCode, Type_Directx_Parameters *directxParams);


/**
 * <b><i>The function starts DirectX mode.</i></b><br>
 * 
 * @param chHandle		communication handle as from
 *						itf_OpenChannel
 * @param SlotID		(in) Slot index of the device
 * @param CommandCode	(in) Code of the command to send to start the directx mode
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 */

long WINAPI itf_DirectXStart(HANDLE chHandle, long SlotID, long CommandCode);


/**
 * <b><i>The function stops DirectX mode.</i></b><br>
 * 
 * @param chHandle		communication handle as from
 *						itf_OpenChannel
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 */

long WINAPI itf_DirectXStop(HANDLE chHandle);


/**
 * <b><i>This function read the data from DirectX</i></b><br>
 * 
 * @param chHandle		communication handle as from
 *						itf_OpenChannel
 * @param pUserData		(out) Pointer to the read buffer
 * @param UserDataSize	(in) dimension, in bytes, of pUserData buffer
 * @param lpBytesRead	(out) number of the byte read
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 */

long WINAPI itf_DirectXRead(HANDLE chHandle, void *pUserData, long UserDataSize, long *lpBytesRead);
long WINAPI itf_DirectXReadEx(HANDLE chHandle, int format, long UserDataSize, long *lpBytesRead,
							  int *imageType, int *width, int *height, int *bpp, int *code, HBITMAP *hBitmap);


/**
 * <b><i>This function return the number of images already arrived but not read from the application</i></b><br>
 * 
 * @param chHandle		communication handle as from
 *						itf_OpenChannel
 * @param queueLen		(out) number of images not read
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 */

long WINAPI itf_DirectXQueueLen(HANDLE chHandle, long *queueLen);


/**
 * <b><i>This function returns dimensione of the first unread image</i></b><br>
 * 
 * @param chHandle		communication handle as from
 *						itf_OpenChannel
 * @param imageSize		(out) image size
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 */

long WINAPI itf_DirectXImageSize(HANDLE chHandle, long *imageSize);

/**@}*/ // end of group DIRECTX_FUNCTIONS

//-----------------------------------------------------------------------------

/** 
 *  
 * @defgroup IMAGE_BANK_FUNCTIONS Image bank functions
 */
/**@{*/
//-----------------------------------------------------------------------------
//--- IMAGE COMMANDS ----------------------------------------------------------
//-----------------------------------------------------------------------------

/**
 * <b><i>Load a picture (FileName) on the specified Image Bank (BankToUpdate)</i></b><br>
 * 
 * @param Handle     communication handle as from
 *                   itf_OpenChannel()
 * @param BankToUpdate
 *               
 * @param FileName
 *               (in)
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 */

long WINAPI itf_LoadImageBank(
                                   HANDLE                  Handle,
                                   long                    BankToUpdate,
                                   char                    *FileName);

/**
 * <b><i>Read an Image Bank (BankToRead) and visualize it on the specified hDC</i></b><br>
 * 
 * @param Handle     communication handle as from
 *                   itf_OpenChannel()
 * @param BankToRead number of the bank to read
 *                   - -1 Is an internal bank
 *                   - -2 Ask a snapshot to the device
 * @param Width      (in) this is the "desired" horizontal
 *                   dimension of the image. This information
 *                   could be retrived from the
 *                   itf_OpenChannel()
 * @param Height     (in) this is the "desired" vertical
 *                   dimension of the image. This information
 *                   could be retrived from the
 *                   itf_OpenChannel()
 * @param ImageType  (out) this is the returned image type an it
 *                   is one of the following: GRAY_8, RGB_565,
 *                   RGB_888, RGBY_32BIT               
 * @param hDC_Dest
 *               
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 */

long WINAPI itf_ReadImageBank(
                                   HANDLE                  Handle,
                                   long                    BankToRead,
                                   long                    Width,
                                   long                    Height,
                                   long                    ImageType,
                                   long                    hDC_Dest);


/**
 * <b><i>Get an ImageBuffer form a Bank (BankToRead) on the
 * device, with the desired image width and height</i></b><br>
 *  
 * - This is a buffer with an header and a payload. Use 
 * TIL_get_image_stream_ptr() to get the address of the first
 * byte of the first pixel. This format is to avoid one copy 
 * inside the dll. 
 * - The resampling is done from the AntaresEngine. There is
 * just nearest neiborough resampling, no scaling.
 * 
 * @param Handle     communication handle as from
 *                   itf_OpenChannel()
 * @param BankToRead number of the bank to read
 *                   - -1 Is an internal bank
 *                   - -2 Ask a snapshot to the device
 * @param Width      (in) this is the "desired" horizontal
 *                   dimension of the image. This information
 *                   could be retrived from the
 *                   itf_OpenChannel()
 * @param Height     (in) this is the "desired" vertical
 *                   dimension of the image. This information
 *                   could be retrived from the
 *                   itf_OpenChannel()
 * @param ImageType  (out) this is the returned image type an it
 *                   is one of the following: GRAY_8, RGB_565,
 *                   RGB_888, RGBY_32BIT
 * @param imageBuffer
 *                   pointer to a preallocated buffer
 *                   where the API can copy the image header and
 *                   the image pixel. Use
 *                   TIL_get_image_stream_ptr() to gain access
 *                   to the first pixel.
 * @param imageBufferByteSz
 *                   this is the dimension of the image
 *                   bank in bytes
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 * @see TIL_get_image_stream_ptr
 */

long WINAPI itf_ReadImageBuffer(HANDLE Handle, long BankToRead, long Width, long Height, long* ImageType,
                                unsigned char* imageBuffer, long imageBufferByteSz);
//------------------------------------------------------------------------------------------------------------------

/**
 * <b><i>Get an ImageHandle form an Antares Bank (BankToRead) 
 * on the device, with the desired image dimension</i></b><br> 
 * 
 * It's a simple Wrapper for itf_ReadImageBuffer()
 * 
 * @param Handle     communication handle as from
 *                   itf_OpenChannel
 * @param NumBanco number of the bank to read
 * @param startX     (in) this is the horizontal
 *                   start point of the image
 * @param startY     (in) this is the "desired" vertical
 *                   start point of the image. Use the 
 * @param Width      (in) this is the horizontal
 *                   dimension of the image
 * @param Height     (in) this is the "desired" vertical
 *                   dimension of the image. Use the 
 *  
 * @param ImageType  (out) resulting image type
 * @param Buffer	 pointer to a preallocated buffer
 *                   where the API can copy the image.
 * @param BufferSize this is the dimension of the image
 *                   bank in bytes
 * 
 * @return 0 if OK or an error code
 */

long WINAPI itf_GetImageBankROI(HANDLE Handle, long NumBanco, long startX, long startY, long width, long height,
								long *ImageType, unsigned char *Buffer, long BufferSize);


/**
 * <b><i>Get an ImageHandle form an Antares Bank (BankToRead) 
 * on the device, with the desired image dimension</i></b><br> 
 * 
 * It's a simple Wrapper for itf_ReadImageBuffer()
 * 
 * @param Handle     communication handle as from
 *                   itf_OpenChannel
 * @param BankToRead number of the bank to read
 * @param Width      (in) this is the horizontal
 *                   dimension of the image
 * @param Height     (in) this is the "desired" vertical
 *                   dimension of the image. Use the 
 *  
 * @param ImageType  (out) resulting image type
 * @param Buffer	 pointer to a preallocated buffer
 *                   where the API can copy the image.
 * @param BufferSize this is the dimension of the image
 *                   bank in bytes
 * 
 * @return 0 if OK or an error code
 */

long WINAPI itf_GetImageBank(HANDLE Handle, long NumBanco, long Width, long Height, long *ImageType,
							 unsigned char *Buffer, long BufferSize);
//------------------------------------------------------------------------------------------------------------------

/**
 * <b><i>Save an Image Bank (BankToSave) on the PC in the specified path (FileName)</i></b><br>
 * 
 * @param Handle     communication handle as from
 *                   itf_OpenChannel()
 * @param BankToSave number of the bank to save
 *                   - -1 Is an internal bank
 *                   - -2 Ask a snapshot to the device
 * @param FileName   (in) the name of the file that you want to
					 fill with the image
 * @param Width      (in) this is the "desired" horizontal
 *                   dimension of the image. This information
 *                   could be retrived from the
 *                   itf_OpenChannel()
 * @param Height     (in) this is the "desired" vertical
 *                   dimension of the image. This information
 *                   could be retrived from the
 *                   itf_OpenChannel()
 * @param ImageType  (out) this is the returned image type an it
 *                   is one of the following: GRAY_8, RGB_565,
 *                   RGB_888, RGBY_32BIT
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 * @see TIL_get_image_stream_ptr
 */

long WINAPI itf_SaveImageBank(
                                   HANDLE                  Handle,
                                   long                    BankToSave,
                                   char                    *FileName,
                                   long                    Width,
                                   long                    Height,
                                   long                    ImageType);
//-----------------------------------------------------------------------------

/**@}*/ // end of group IMAGE_BANK_FUNCTIONS


/** 
 *  
 * @defgroup PROGRAM_FUNCTIONS Program functions
 */
/**@{*/
//-----------------------------------------------------------------------------
//--- PROGRAM COMMANDS --------------------------------------------------------
//-----------------------------------------------------------------------------


/**
 * <b><i>Return corrent program status (1 RUN, 0 STOP, 3 LIVE
 * -1 ERROR)</i></b><br>
 * 
 * This is a wrapper that read the content of the variable:
 * Default[0].DeviceStatus
 * 
 * @param Handle communication handle as from
 *               itf_OpenChannel
 * @param programStatus Possible values are: 
 *  - (-1) if the device is in Error
 *  - 0 the program is stopped
 *  - 1 the program is running
 *  - 3 the program is running, but the device is grabbing from
 *      current camera.
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 * @see itf_StatusProgram
 * @see itf_StartProgram
 * @see itf_StopProgram
 */

long WINAPI itf_StatusProgram(HANDLE Handle, long* programStatus);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>Start the program loaded on the selected
 * device</i></b><br>
 * 
 * @param Handle communication handle as from
 *               itf_OpenChannel
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 * @see itf_StatusProgram
 * @see itf_StartProgram
 * @see itf_StopProgram
 */

long WINAPI itf_StartProgram(HANDLE Handle);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>Stop the program loaded on the selected device</i></b><br>
 * 
 * @param Handle communication handle as from
 *               itf_OpenChannel
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 * @see itf_StatusProgram
 * @see itf_StartProgram
 * @see itf_StopProgram
 */

long WINAPI itf_StopProgram(HANDLE Handle);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>Delete the selected program from the flash</i></b><br>
 * 
 * @param Handle communication handle as from
 *               itf_OpenChannel
 * @param ProgramNumber ID of the program to delete (0 <= ProgramNumber <= 99)
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 * @see itf_StatusProgram
 * @see itf_StartProgram
 * @see itf_StopProgram
 */

long WINAPI itf_DeleteProgram(HANDLE Handle, long ProgramNumber);


/**
 * <b><i>Save the program loaded on the selected device in flash, at the specified
 * index (ProgramNumber) and with the specified name (FileName) that must have
 * a maximum lenght of 7 characters</i></b><br>
 * 
 * @param Handle communication handle as from
 *               itf_OpenChannel
 * @param FileName  
 * @param ProgramNumber ID of the program to save (0 <= ProgramNumber <= 99)
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 * @see itf_StatusProgram
 * @see itf_StartProgram
 * @see itf_StopProgram
 */

long WINAPI itf_SaveProgramInFlash(
                                   HANDLE                  Handle,
                                   char                    *FileName,
                                   long                    ProgramNumber);


/**
 * <b><i>Save a Program from the selected Device Flash Index to the selected PC Path</i></b><br>
 * 
 * @param Handle communication handle as from
 *               itf_OpenChannel
 * @param ProgramIndex  ID of the program to save (0 <= ProgramNumber <= 99)
 * @param FileName		Path where to save the program on the PC
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 * @see itf_StatusProgram
 * @see itf_StartProgram
 * @see itf_StopProgram
 */

long WINAPI itf_SaveProgramFromFlash(
                                   HANDLE                  Handle,
                                   long                    ProgramIndex,
                                   char                    *FileName);


/**
 * <b><i>Send to the Device a Program situated in the PC</i></b><br>
 * 
 * @param Handle communication handle as from
 *               itf_OpenChannel
 * @param FileName		File Name of the program on the PC
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 * @see itf_StatusProgram
 * @see itf_StartProgram
 * @see itf_StopProgram
 */

long WINAPI itf_SendProgramToDevice(HANDLE Handle, char *FileName);
//-----------------------------------------------------------------------------
/**@}*/ // end of group PROGRAM_FUNCTIONS

/** 
 *  
 * @defgroup TABLE_FUNCTIONS Table functions
 */
/**@{*/
//-----------------------------------------------------------------------------
//--- TABLE COMMANDS ----------------------------------------------------------
//-----------------------------------------------------------------------------

/**
 * <b><i>Read a list of Table Parameters (arTable) from the Device, starting from
 * the StartIndex end ending to the EndIndex passed</i></b><br>
 * 
 * @param Handle		communication handle as from
 *						itf_OpenChannel
 * @param startIndex	(in)
 * @param stopIndex		(in)
 * @param arTable		(out) Parameters table
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 * @see itf_StatusProgram
 * @see itf_StartProgram
 * @see itf_StopProgram
 */

long WINAPI itf_ReadTableParameters(
                                   HANDLE                  Handle,
                                   long                    startIndex,
                                   long                    stopIndex,
                                   tatStringa              *arTable);

/**
 * <b><i>Write a list of Table Parameters (arTable) on the Device, starting from the StartIndex passed</i></b><br>
 * 
 * @param Handle		communication handle as from
 *						itf_OpenChannel
 * @param startIndex	(in)
 * @param arData		(in) Parameters table
 * @param numOfElements	(in) Number of entry into this table
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 * @see itf_StatusProgram
 * @see itf_StartProgram
 * @see itf_StopProgram
 */

long WINAPI itf_WriteTableParameters(
                                   HANDLE                  Handle,
                                   int                     startIndex,
                                   tatStringa              *arData,
                                   int                     numOfElements);



/**
 * <b><i>Read a Table Variable from Device</i></b><br>
 * 
 * @param Handle     (in)channel handle as from
 *                   itf_OpenChannel
 * @param TableName  (in)Name of the table to read, for
 *                   instance "Default"
 * @param SheetIndex (in)Index of the table Sheet to read
 * @param VarName    (in)Name of the variable to read
 * @param VarValue   (out) this is a char buffer representing
 *                   the value. NULL is not valid,
 *                   if *VarValue == NULL the memory is
 *                   allocated internally and ift_Free must
 *                   be used to release memory. Use a buffer
 *                   of at least 128 bytes
 * 
 * @return 0 (ITF_OK) or an ITF_ERROR_CODE error code
 * @see itf_OpenChannel
 * @see itf_Free
 */

long WINAPI itf_ReadVariable(HANDLE Handle, char* TableName, long SheetIndex, char* VarName, char** VarValue);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>Write a Table Variable on Device</i></b><br>
 * 
 * @param Handle     (in)channel handle as from
 *                   itf_OpenChannel
 * @param TableName  (in)Name of the table to write, for
 *                   instance "Default"
 * @param SheetIndex (in)Index of the table Sheet to write
 * @param VarName    (in)Name of the variable to write
 * @param ValueToWrite
 *                   (in) this is a char buffer representing
 *                   the value. NULL is not valid.
 * @param VarType    Data type used to convert the string in
 *                   a value. Use etv_Unknow is allowed, with
 *                   the advantage that the conversion is
 *                   done automatically, but it require a
 *                   read and a write, so it require twice
 *                   the time.
 * 
 * @return 
 */

long WINAPI itf_WriteVariable(HANDLE Handle, char* TableName, long SheetIndex, char* VarName, char* ValueToWrite, Enum_Data_Type VarType);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>Return a string with the list of tables available on the device</i></b><br>
 * The table list is a string of names separated by ;.
 * 
 * Basically this API read the content of the variable specified
 * by the following expression: Default[0].TableList
 * 
 * @param Handle     communication handle as from
 *                   itf_OpenChannel.
 * @param Buffer     pointer to a buffer allocated by the caller
 *                   function.
 * @param BufferByteSz
 *                   (in)buffer size. If not enough an error is
 *                   returned. Check TextByteSz to see ho much
 *                   the size of the buffer should be.
 * @param TextByteSz (out) resulting text dimension in bytes.
 *                   Use this result to check if the buffer
 *                   dimension is enough or not.
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 */

long WINAPI itf_TablesList(HANDLE Handle, char* Buffer, long BufferByteSz, 
                              long* TextByteSz);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>Return a string with the list of all variables names
 * inside a specified table. </i></b><br>
 * 
 * There is just one variable name for each line
 * 
 * @param Handle     communication handle as from
 *                   itf_OpenChannel
 * @param TableName  Name of the table to analyze
 * @param Buffer     pointer to a buffer allocated by the
 *                   caller function
 * @param BufferByteSz
 *                   (in)buffer size. If not enough the text
 *                   is clipped and terminated. An error code is
 *                   returned.
 * @param TextByteSz (out) resulting text dimension in bytes.
 *                   Use this result to check if the buffer
 *                   dimension is enough or not.
 * @param SheetCount (out) resulting number of sheet in the
 *                   table. Use NULL if not required
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 */

long WINAPI itf_TableVariablesName(HANDLE Handle, char* TableName, char* Buffer, 
                                     long BufferByteSz, long* TextByteSz, long* SheetCount);
//------------------------------------------------------------------------------------------------------------------


/**
 *  <b><i>Dump (or convert) the content of the table in a text
 *  buffer, that is human readable.</i></b><br>
 * 
 *  The aim of this API is to provide a fast way to compare the
 *  contents of two different tables.
 * 
 *  For each sheet there is an header with the following set of
 *  data:
 *    - [Sheet #]
 *    - [Visible]
 *    - [Execute]
 * 
 * For each variable there are the following fields:
 *    -  [VariableName]
 *    -  [Value]
 *    -  [Typ] - Variable type in numeric form
 *    -  [RdO] - Read only flag
 *    -  [DefaultValue]
 *    -  [MinimumValue]
 *    -  [MaximumValue]
 *    -  [Description]
 * 
 *  After the sheet section there is an header with the fields
 *  name for the following varibles. Each variable is on one
 *  line. The end of the line is marked by CRLF.
 *  Each variable field is separated by the specified Token.
 * 
 *  In case of binary variable, there is the marker <BIN>
 *  In case of unknown type there is the marker <Unknown_xx>
 * 
 * @param Handle     communication handle as from
 *                   itf_OpenChannel.
 * @param TableName  Name of the table to dump.
 * @param Buffer     pointer to a buffer allocated by the
 *                   caller function
 * @param BufferByteSz
 *                   (in)buffer size. If not enough the text
 *                   is clipped and terminated. An error code is
 *                   returned.
 * @param TextByteSz (out) this is the resulting string size
 *                   (length + the terminator)
 * @param SheetCount (out) resulting number of sheet in the
 *                   table. Use NULL if not required
 * @param Token      separator string between two fields on the
 *                   same line. Suggested value is " "
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 * @see itf_TableVariablesName
 */

long WINAPI itf_TableVariablesText(HANDLE Handle, char* TableName, char* Buffer, 
                                     long BufferByteSz, long* TextByteSz, long* SheetCount, 
                                     char* Token);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>Read a variable using an antares expression and return
 * the variable buffer in binary format. </i></b><br>
 * 
 * This function work without asking info about the variable
 * type, or size and so on.
 * 
 * In case of binary variable, the size is unknown, so it is
 * suggested to allocate a buffer, make a trial and in case of
 * error, check the VarByteSz result. If this is not 0, it
 * represent the required buffer size.
 * 
 * This can be used to reallocate the buffer and make a second
 * successfull trial.
 * 
 * The expression syntax is very simple: <br>
 * 
 *  - TableName[sheet_index].VarName
 * 
 * If sheet index is not defined as in case of defaultTable, use
 * 0. Also this variations are accepted:
 * 
 *  - TableName[].VarName => became TableName[0].VarName
 *  - TableName.VarName => became TableName[0].VarName
 * 
 * @param Handle    communication handle as from
 *                  itf_OpenChannel.
 * @param VarExpression
 *                  antares expression used to indentify the
 *                  variable (Table[sheet].VariableName
 * @param Buffer    pointer to a buffer allocated by the caller
 *                  function
 * @param BufferByteSz
 *                  (in)buffer size. It must be bigger than the
 *                  variable dimension
 * @param VarByteSz
 *                  (out) pointer to the value for the resulting
 *                  data size. Use this parameter to know how much
 *                  memory is necessary
 * @param VarType   (out) this is the type of the required
 *                  variable. Use NULL if not required.
 * @param ReadOnly  (out) this is the read only property of the
 *                  variable. Use NULL if not required.
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 * @see itf_TableVariableText
 */

long WINAPI itf_TableVariableRead(HANDLE Handle, char* VarExpression, unsigned char* Buffer, 
                                     long BufferByteSz, long* VarByteSz, long* VarType, long* ReadOnly);
//------------------------------------------------------------------------------------------------------------------

/**
 * <b><i>Read a variable using an antares expression. And
 * convert it in text format</i></b><br>
 * 
 * The output of this API is always a Text string.
 * 
 * This function work without asking info about the variable
 * type and size and so on.
 * In case of binary variable the output is <BIN>
 * In case of unknown format, the output is <Unknown_xx> where
 * xx is the code of the format.
 * 
 * It the BufferByteSz is not enough, the function return an
 * error, BUT TextByteSz contains the required dimension. This
 * can be used to reallocate the buffer and make a second trial.
 * 
 * The expression syntax is very simple: <br>
 * 
 * - TableName[sheet_index].VarName
 * 
 * If sheet index is not defined as in case of defaultTable, use
 * 0. Also this variations are accepted:
 * 
 * - TableName[].VarName => became TableName[0].VarName
 * - TableName.VarName => became TableName[0].VarName
 * 
 * @param Handle     communication handle as from
 *                   itf_OpenChannel.
 * @param VarExpression
 *                   antares expression used to indentify the
 *                   variable (Table[sheet].VariableName).
 * @param Buffer     pointer to a buffer allocated by the caller
 *                   function.
 * @param BufferByteSz
 *                   (in)buffer size. It must be bigger than the
 *                   variable dimension.
 * @param TextByteSz (out) this is the resulting string size
 *                   (length + the terminator). This not the the
 *                   size of the variable, but just of the
 *                   translation.
 * @param VarType    (out) this is the type of the required
 *                   variable. Use NULL if not required.
 * @param ReadOnly   (out) this is the read only property of the
 *                   variable. Use NULL if not required.
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 * @see itf_TableVariableRead
 */

long WINAPI itf_TableVariableText(HANDLE Handle, char* VarExpression, char* Buffer, 
                                     long BufferByteSz, long* TextByteSz, long* VarType, 
                                     long* ReadOnly);
//------------------------------------------------------------------------------------------------------------------
    
    
/**
 * <b><i>Write a binary value on a table using an antares
 * expression.</i></b><br>
 * 
 * The expression syntiax is very simple: <br>
 * 
 * - TableName[sheet_index].VarName
 * 
 * If sheet index is not defined (as in case of defaultTable,
 * use 0)
 * 
 * @param Handle    communication handle as from
 *                  itf_OpenChannel
 * @param VarExpression
 *                  antares expression used to indentify the
 *                  variable (Table[sheet].VariableName
 * @param Buffer
 * @param VarByteSz
 * 
 * @return 0 if OK or an error code
 * @see itf_TableVariableWriteText
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 */

long WINAPI itf_TableVariableWrite(HANDLE Handle, char* VarExpression, unsigned char* Buffer, long VarByteSz);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>Write a value on a table using an antares
 * expression.</i></b><br>
 * 
 * The input text is converted in a value accordingly to the
 * variable type.
 * 
 * If the VarType is unknown, use etv_Unknow (-1). The API
 * provide to read the variable and detect the varible type.
 * This slowdown the API speed, so is better if the variable
 * type is known
 * 
 * This api can manage only the variable types tha can be
 * represented in text form:
 *     - etv_Long            (0)  //Long (integer 32 bits)<br>
 *     - etv_SingleFloat     (1)  //Single<br>
 *     - etv_DoubleFloat     (2), //Double<br>
 *     - etv_String          (3), //String<br>
 * 
 * The expression syntiax is very simple: <br>
 * 
 *  - TableName[sheet_index].VarName
 * 
 * If sheet index is not defined (as in case of defaultTable,
 * use 0)
 * 
 * @param Handle  communication handle as from
 *                itf_OpenChannel
 * @param VarExpression
 *                antares expression used to indentify the
 *                variable (Table[sheet].VariableName
 * @param Text    pointer to the text string (null terminated)
 * @param VarType this is the type of the variable. If the type
 *                is unknown use etv_Unknow (-1). In this case
 *                the varType will be detected automatically,
 *                but this require an additional request to the
 *                device.
 * 
 * @return 0 if OK or an error code
 * @see itf_TableVariableWrite
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 */

long WINAPI itf_TableVariableWriteText(HANDLE Handle, char* VarExpression, char* Text, long VarType);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>Read from device a set of tables specified into tableList</i></b><br>
 * The table list is a string of table names separated by ";"
 * char. The received buffer can be managed using
 * itf_TablesetInfo, itf_TablesetGetVars,
 * itf_TablesetReadVarList, itf_TablesetWriteVarList
 * 
 * @param Handle    (in)channel handle as from itf_OpenChannel
 * @param TableList (in) string with the desired tables. The
 *                  table list is a string of table names
 *                  separated by ";"
 * @param buffer    (out) pointer to the received buffer (*buffer must be NULL)
 * 
 * @return ITF_OK or ITF_ERROR
 * @see itf_OpenChannel
 * @see itf_TablesetFree
 * @see itf_TablesetGetVars
 * @see itf_TablesetInfo
 * @see itf_TablesetReadVarList
 * @see itf_TablesetWriteVarList
 */

long WINAPI itf_TablesetRead(HANDLE Handle, char* TableList, void** buffer);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>Free the buffer used to allocate
 * the tableset</i></b><br>
 * Don't use the itf_free or you risk to let some allocated
 * memory around.
 * 
 * @param buffer (the same used for itf_TablesetRead)
 * 
 * @return 
 * @see itf_TablesetRead
 */

void WINAPI itf_TablesetFree(void** buffer);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>Scan the buffer and extract information for the specified table</i></b><br>
 * 
 * @param tableName (in) the table to look for
 * @param buffer    (in) the buffer received from
 *                  itf_TablesetRead
 * @param totalSheets
 *                  (out) number of sheets of the table
 * @param totalVars (out) number of variables in the table
 * 
 * @return ITF_OK or ITF_ERROR
 * @see itf_TablesetRead
 * @see itf_TablesetFree
 */

long WINAPI itf_TablesetInfo(char* tableName, void* buffer, long *totalSheets, long *totalVars);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>Read a variable from a tableset using an antares
 * expression and return the variable buffer in binary format.
 * </i></b><br>
 * 
 * This function work without asking info about the variable
 * type, or size and so on.
 * 
 * <B>WARNING:</B> In case of binary variable, the result is a
 * "<BIN>" text string. Use itf_TableVariableRead() to override
 * this limitation.
 * 
 * The expression syntax is very simple: <br>
 * 
 *  - TableName[sheet_index].VarName
 * 
 * If sheet index is not defined as in case of defaultTable, use
 * 0. Also this variations are accepted:
 * 
 *  - TableName[].VarName => became TableName[0].VarName
 *  - TableName.VarName => became TableName[0].VarName
 * 
 * @param VarExpression
 *                  antares expression used to indentify the
 *                  variable (Table[sheet].VariableName
 * @param buffer    (in) the buffer received from
 *                  itf_TablesetRead()
 * @param VarValue  pointer to a buffer allocated by the
 *                  caller function
 * @param VarValueMaxSz
 *                  (in)buffer size. It must be bigger than the
 *                  variable dimension
 * @param VarByteSz
 *                  (out) pointer to the value for the resulting
 *                  data size. Use this parameter to know how much
 *                  memory is necessary
 * @param VarType   (out) this is the type of the required
 *                  variable. Use NULL if not required.
 * @param ReadOnly  (out) this is the read only property of the
 *                  variable. Use NULL if not required.
 *                  <B>Currently not implemented</B>
 * 
 * @return ITF_OK or ITF_ERROR
 * @see itf_TablesetRead
 * @see itf_TablesetGetVars
 * @see itf_TablesetInfo
 * @see itf_TablesetReadVarList
 * @see itf_TablesetWriteVarList
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 * @see itf_TableVariableText
 */

long WINAPI itf_TablesetVariableRead(char* VarExpression, void* buffer, unsigned char* VarValue, 
                                     long VarValueMaxSz, long* VarByteSz, long* VarType, long* ReadOnly);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>Scan the buffer and extract all variable names, for the
 *  specified table</i></b><br>
 * 
 * @param tableName (in) the table to look for
 * @param buffer    (in) the buffer received from
 *                  itf_TablesetRead
 * @param variableList
 *                  (out) allocated vector of
 *                  ITF_VariableType
 * @param totalVariablesToRead
 *                  (in) number of variables in the
 *                  variable list vector
 * 
 * @return ITF_OK or ITF_ERROR
 * @see itf_TablesetRead
 * @see itf_TablesetGetVars
 * @see itf_TablesetInfo
 * @see itf_TablesetReadVarList
 * @see itf_TablesetWriteVarList
 */

long WINAPI itf_TablesetGetVars(char* tableName, void* buffer, ITF_VariableType* variableList, long totalVariablesToRead);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>Read a set of variables from the tableset in buffer</i></b><br>
 *  Get values and types of a list of variable indexed by table
 *    name, sheet number and variable name and specified into an
 *      array  of ITF_VariableType
 * 
 * @param buffer (in) the buffer received from itf_TablesetRead
 * @param variableList
 *               (in) pointer to the first element in the array
 * @param totalVariablesToRead
 *               (in) number of item in the array of ITF_VariableType
 * 
 * @return ITF_OK or ITF_ERROR
 */

long WINAPI itf_TablesetReadVarList(void* buffer, ITF_VariableType* variableList, long totalVariablesToRead);
//------------------------------------------------------------------------------------------------------------------



/**
 *  <b><i>Change the variable as specified in variableList and
 *  save it to the device</i></b><br>
 *  <b>BE CAREFULL</b>: This does not affect the current
 *  tableset. tableset must be reloaded to see the changes
 *  applied.
 * 
 * @param Handle (in)channel handle as from itf_OpenChannel
 * @param buffer (in) the buffer received from itf_TablesetRead
 * @param variableList
 *               (in) pointer to the first element in the array
 *               that contains all variable to be written on the
 *               file.
 * @param totalVariablesToWrite
 *               (in) number of variable to write
 * 
 * @return 0 (ITF_OK) or ITF_ERROR
 * @see itf_OpenChannel
 * @see itf_TablesetRead
 */

long WINAPI itf_TablesetWriteVarList(HANDLE Handle, void *buffer, ITF_VariableType *variableList, long totalVariablesToWrite);
//------------------------------------------------------------------------------------------------------------------
/**@}*/ // end of group TABLE_FUNCTIONS


/** 
 *  
 * @defgroup RING_BUFFER_FUNCTIONS Ring buffer functions
 */
/**@{*/
/**
 * <b><i>Create an handle to manage a ring buffer of images
 * </i></b><br>
 * This API set can be used to download images from a circular
 * or ring buffer generated on the device using some of the
 * available images bank.
 * In the typical implementation of a ring buffer there is a
 * producer (that is the antares engine) and a consumer (that is
 * the API and the interface). The producer increment the
 * writeIndex once there is a new image, the consumer try to
 * download all the available images. If the creation speed is
 * bigger than the downlad speed, the typical behavior is that
 * the old images are overwrite by the new one, and the
 * application interface skip images.
 * If the engine restart, the ImageCounter will restart from 0.
 * 
 * @param Handle     communication handle as from
 *                   itf_OpenChannel
 * @param tableName  this is name of the table which contains all
 *                   the variables used to manage the ring buffer
 * @param SheetIndex table sheet index. Use 0 as default value
 * @param ringBufferBaseIndexVarName
 *                   this is the name of the variable in the
 *                   table above where there is the base index of
 *                   the ring buffer. This means that this is the
 *                   index of the first image bank that belong to
 *                   the buffer.
 * @param writeIndexVarName
 *                   this is the name of the variable in the
 *                   table where there is the relative index
 *                   where the new image will be stored. This
 *                   mean that the absolute index is<br>
 *                   <i>ringBufferBaseIndex + writeIndex</i>
 * @param ringBufferLengthVarName
 *                   this is the name of the variable in the
 *                   table where is written how many elements the
 *                   buffer can contain.
 * @param ringBufferImageCounterVarName
 *                   this is the name of the variable in the
 *                   table where is written how many images have
 *                   been taken by the system. If the system
 *                   restart, this counter must be reset
 * @param ringBufferHandle
 *                   this is the resulting handle that must be
 *                   used in the ift_RingBufferGetNext and in the
 *                   ift_RingBufferClose
 * 
 * @return 0 if OK or an error code
 * @see http://en.wikipedia.org/wiki/Circular_buffer
 * @see itf_OpenChannel
 * @see itf_RingBufferClose
 * @see itf_RingBufferNextImageGet
 * @see itf_RingBufferNextImageSave
 */

long WINAPI itf_RingBufferOpen( HANDLE Handle, char* tableName, long SheetIndex, 
                                      char* ringBufferBaseIndexVarName, char* writeIndexVarName, 
                                      char* ringBufferLengthVarName, char* ringBufferImageCounterVarName, 
                                      HANDLE* ringBufferHandle);
//------------------------------------------------------------------------------------------------------------------

/**
 * <b><i>Close the handle which managed a ring buffer of images
 * </i></b><br>
 * 
 * @param ringBufferHandle
 *               handle returned by itf_ringBufferOpen
 * 
 * @return 0 if OK or an error code
 * @see itf_RingBufferOpen
 * @see itf_RingBufferNextImageGet
 * @see itf_RingBufferNextImageSave
 */

long WINAPI itf_RingBufferClose(HANDLE* ringBufferHandle);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>Copy next image from the ring buffer in to a
 * local buffer</i></b><br>
 * The image width, height and imageType are available from the
 * itf_OpenChannel, or from the itf_Read_Device_Informations
 * 
 * @param ringBufferHandle
 *               handle returned by itf_ringBufferOpen
 * @param image  pointer to a preallocated buffer where the API
 *               can copy the image.
 * @param imageByteSz
 *               this is the dimension of the image bank in
 *               bytes
 * 
 * @return 0 if OK or an error code: ITF_ERROR_FEWDATA if no
 *         images are available
 * @see itf_RingBufferOpen
 * @see itf_RingBufferClose
 * @see itf_RingBufferNextImageSave
 * @see itf_Read_Device_Informations
 * @see TIL_get_buffer_size
 */

long WINAPI itf_RingBufferNextImageGet(HANDLE ringBufferHandle, unsigned char* image, 
                                             long imageByteSz);
//------------------------------------------------------------------------------------------------------------------

/**
 * <b><i> Save next Image from the ring buffer to the desired
 * file</i></b><br>
 * 
 * @param ringBufferHandle
 *               handle returned by itf_ringBufferOpen
 * @param imageFileName
 *               string containing the path which you want to save the
 *               image to.
 * @param imageFormat
 *               desired file format type. Available format
 *               are: TIL_IMAGE_FORMAT_BMP,
 *               TIL_IMAGE_FORMAT_JPG
 * @param JPEGQuality
 *               this is used to change the compression of
 *               the JPEG. Values range from 20 to 100.
 *               Default value is 80
 * 
 * @return 0 if OK or an error code
 * @see itf_RingBufferOpen
 * @see itf_RingBufferClose
 * @see itf_RingBufferNextImageGet
 */

long WINAPI itf_RingBufferNextImageSave(HANDLE ringBufferHandle, char* imageFileName,
                                              long imageFormat, long JPEGQuality);
//------------------------------------------------------------------------------------------------------------------
/**@}*/ // end of group RING_BUFFER_FUNCTIONS


/**
 * <b><i>Write a TSK on the selected Device and wait that
 *  device restarts correctly</i></b><br>
 * 
 * @param Handle   communication handle as from
 *                 itf_OpenChannel
 * @param FileName full file name of the TSK
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 */

long WINAPI itf_WriteTSK(HANDLE Handle, char* FileName);
//------------------------------------------------------------------------------------------------------------------


/** 
 *  
 * @defgroup FORMAT_FUNCTIONS Format functions
 */
/**@{*/
//-----------------------------------------------------------------------------
//--- FORMAT COMMANDS ---------------------------------------------------------
//-----------------------------------------------------------------------------

/**
 * <b><i>Send a Format from the PC to the RAM of the Device</i></b><br>
 * 
 * @param Handle   communication handle as from
 *                 itf_OpenChannel
 * @param PathFile full path of the format
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 */

long WINAPI itf_SendFormat(HANDLE Handle, char *PathFile);

/**
 * <b><i>Read a Format from the RAM of the Device and save it on PC</i></b><br>
 * 
 * @param Handle   communication handle as from
 *                 itf_OpenChannel
 * @param PathFile full path of the format
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 */

long WINAPI itf_SaveFormat(HANDLE Handle, char *PathFile);


/**
 * <b><i>Store the Format loaded on the RAM of the Device into the Flash</i></b><br> 
 * 
 * @param Handle		 communication handle as from
 *						 itf_OpenChannel
 * @param FormatName	 full file name of the format
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 */

long WINAPI itf_SaveFormatInFlash(HANDLE Handle, char *FormatName);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>Store the Format loaded on the RAM of the Device into the Secure Digital</i></b><br> 
 * 
 * @param Handle		 communication handle as from
 *						 itf_OpenChannel
 * @param FormatName	 full file name of the format
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 */

long WINAPI itf_SaveFormatInSD(HANDLE Handle, char *FormatName);


/**
 * <b><i>Get a Format from Flash and load it on the RAM of the Device</i></b><br> 
 * 
 * @param Handle		 communication handle as from
 *						 itf_OpenChannel
 * @param FormatName	 full file name of the format
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 */

long WINAPI itf_LoadFormatFromFlash(HANDLE Handle, char *FormatName);


/**
 * <b><i>Get a Format from Secure Digital and load it on the RAM of the Device</i></b><br> 
 * 
 * @param Handle		 communication handle as from
 *						 itf_OpenChannel
 * @param FormatName	 full file name of the format
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 */

long WINAPI itf_LoadFormatFromSD(HANDLE Handle, char *FormatName);


/**
 * <b><i>Get the list of Format stored into the Flash</i></b><br> 
 * 
 * @param Handle		 communication handle as from
 *						 itf_OpenChannel
 * @param arFormat		 (out)format list
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 */

long WINAPI itf_DirFormatInFlash(HANDLE       Handle, void    **arFormat);


/**
 * <b><i>Get the list of Format stored into the Secure Digital</i></b><br> 
 * 
 * @param Handle		 communication handle as from
 *						 itf_OpenChannel
 * @param arFormat		 (out)format list
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 */

long WINAPI itf_DirFormatInSD(HANDLE  Handle, void    **arFormat);


/**
 * <b><i>Delete a Format from Flash</i></b><br> 
 * 
 * @param Handle		 communication handle as from
 *						 itf_OpenChannel
 * @param FormatName	 full file name of the format
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 */

long WINAPI itf_DeleteFormatFromFlash(HANDLE Handle, char *FormatName);


/**
 * <b><i>Delete a Format from Secure Digital</i></b><br> 
 * 
 * @param Handle		 communication handle as from
 *						 itf_OpenChannel
 * @param FormatName	 full file name of the format
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 */

long WINAPI itf_DeleteFormatFromSD(HANDLE Handle, char *FormatName);
/**@}*/ // end of group FORMAT_FUNCTIONS

/** 
 *  
 * @defgroup IMAGE_MANIPULATION_APIs Image manipulation APIs
 */
/**@{*/
//-----------------------------------------------------------------------------
//--- IMAGE MANIPULATION APIs -------------------------------------------------
//-----------------------------------------------------------------------------
        
    
/**
 * <b><i>The function obtains the width in pixel of the image
 * .</i></b><br>
 *  
 * The function detect automatically the image format between 
 * TBanco, Antares image, Backwall and TObject, GigeImage 
 *  
 * @param width  [out] return the image width in pixel.
 * @param hImage reference to the TIL image.
 * 
 * @return interface error code
 * @see ITF_ERROR_CODE
 */

long WINAPI TIL_get_image_width(long *width, HANDLE hImage);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>The function obtains the height in pixel of the image
 * </i></b><br>
 *  
 * The function detect automatically the image format between 
 * TBanco, Antares image, Backwall and TObject, GigeImage 
 *  
 * @param height [out] return the image height in pixel.
 * @param hImage reference to the TIL image.
 * 
 * @return interface error code
 * @see ITF_ERROR_CODE
 */

long WINAPI TIL_get_image_height(long *height, HANDLE hImage);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>The function obtains the image Type.</i></b><br>
 *  
 * The function detect automatically the image format between 
 * TBanco, Antares image, Backwall and TObject, GigeImage 
 *  
 * @param pixel_type [out] return the current image type. Used 
 *                  values are (GRAY_8, RGB_888, ecc).
 * @param hImage reference to the TIL image.
 *
 * @return  interface error code
 * @see ITF_ERROR_CODE
 */

long WINAPI TIL_get_image_pixel_type(long *pixel_type, HANDLE hImage);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>The function obtains bit deep of each pixel of the
 * image.</i></b><br>
 *  
 * The function detect automatically the image format between 
 * TBanco, Antares image, Backwall and TObject, GigeImage 
 *  
 * @param bpp    [out] return the number of bit per pixel. For 
 *               instance an BGR888 has 24 bits per pixel
 * @param hImage reference to the TIL image.
 * 
 * @return interface error code
 * @see ITF_ERROR_CODE
 */

long WINAPI TIL_get_image_bits_pixel(long *bpp, HANDLE hImage);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>The function obtains the image format. This is 
 * necessary to understand if this is a plain image or a 
 * compessed one (JPEG).</i></b><br> 
 *  
 * The function detect automatically the image format between 
 * TBanco, Antares image, Backwall and TObject, GigeImage 
 *  
 * @param image_format [out] return the image format between RAW
 *                     (0), JPEG(1), BMP (2).
 * @param hImage reference to the TIL image.
 *
 * @return  interface error code
 * @see ITF_ERROR_CODE
 */

long WINAPI TIL_get_image_format(long *image_format, HANDLE hImage);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>The function get the progressive counter stored in the
 * image HANDLE.</i></b><br>
 * This counter is automatically increased by the camera, each
 * time a new picture is taken.
 * Every itf_clStartStopDevice() the counter is reset
 *  
 * The function detect automatically the image format between 
 * TBanco, Antares image, Backwall and TObject, GigeImage 
 *  
 * @param counter [out] return progressive counter.
 * @param hImage  reference to the TIL image.
 * 
 * @return interface error code
 * @see ITF_ERROR_CODE
 * @see itf_clStartStopDevice
 */

long WINAPI TIL_get_image_counter(long *counter, HANDLE hImage);
//------------------------------------------------------------------------------------------------------------------


/**
 * <B><I>Get the image time stamp [ms].</I></B><BR> 
 *  
 * The function detect automatically the image format between 
 * TBanco, Antares image, Backwall and TObject, GigeImage 
 *  
 * @param timeStamp image time stamp in milli seconds from the 
 *                  camera boot-up
 * @param hImage    reference to the TIL image.
 * 
 * @return interface error code
 * @see ITF_ERROR_CODE 
 *  
 */

long WINAPI TIL_get_image_timestamp(long *timeStamp, HANDLE hImage);
//------------------------------------------------------------------------------------------------------------------


/**
 * <B><I>The function obtains the dimension in byte of the
 * image raw buffer.</I></B><BR>
 *  
 * The function detect automatically the image format between 
 * TBanco, Antares image, Backwall and TObject, GigeImage 
 *  
 * Depending on the image type each pixel is code in 1, or 2 or
 * 3 bytes (using TIL_get_image_bits_pixel() we can see how many
 * pixels, dividing by 8 we get the number of bytes) 
 * 
 * @param stream_size
 *               [out] the dimension in byte.
 * @param hImage reference to the TIL image.
 * 
 * @return interface error code
 * @see ITF_ERROR_CODE
 * @see TIL_get_image_bits_pixel
 */

long WINAPI TIL_get_image_stream_size(long* stream_size, HANDLE hImage);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>The function return a pointer to the image plain
 * buffer.</i></b><br> 
 *  
 * The function detect automatically the image format between 
 * TBanco, Antares image, Backwall and TObject, GigeImage 
 * 
 * @param stream_ptr [out] pointer to the first byte of the 
 *                   first pixel (Top = 0, Left = 0)
 * @param hImage     reference to the image.
 * 
 * @return interface error code
 * @see ITF_ERROR_CODE
 */

long WINAPI TIL_get_image_stream_ptr(unsigned char** stream_ptr, HANDLE hImage);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>Eval the necessary buffer dimension to be allocated for 
 * an incoming image of given width height and type</i></b><br> 
 *  
 * The function take care of the worst case between TBanco, 
 * Antares image, Backwall and TObject, GigeImage 
 * 
 * @param img_width     desired image width
 * @param img_height    desired image width
 * @param pixel_type    desired image color space output
 *                      (GRAY_8, RGB_565, RGB_888)
 * 
 * @return the resulting minimum byte size 
 *  
 */

long WINAPI TIL_get_buffer_size(long img_width, long img_height, long pixel_type);
//------------------------------------------------------------------------------------------------------------------
    

/**
 * <b><i>Process results from the hImage grabbed
 * using itf_DirectXImageGet()</i></b><br>
 * 
 * @param hImage reference to the buffer received from the 
 *               device.
 * @param resultIndex
 *                from 1 to 10 included, as in the step
 *                Display bank step.
 * @param buffer  preallocated buffer where the api copy
 *                data from the device
 * @param bufferByteSz
 *                preallocated buffer size in bytes
 * @param VarType variable type accordingly to the enum
 *                Enum_Data_Type
 * 
 * @return 0 if OK or an error code
 */

long WINAPI TIL_get_processResults(HANDLE hImage, long resultIndex, unsigned char* buffer, long bufferByteSz, long* VarType);
//------------------------------------------------------------------------------------------------------------------

/**@}*/ // end of group IMAGE_MANIPULATION_APIs


/**
 *  
 * @defgroup MISCELLANEOUS Miscellaneous
 */
/**@{*/

/*--------------------------------------------------------------
DATA ABSTRACT
                                                               
	Date:                                                   
		27 July 2002
                                                               
	Author:
		Luca Biancardi

	Structure name:
		ITF_ethNamingOpenParameter

	Type:
		ITF_ETHERNET
                                                               
	Description:                                      
		If an application does not care what local address is assigned, 
		specify the manifest constant value ADDR_ANY for the value member 
		of the IPAddressPC parameter. 
		If the PortPC is specified as zero, the service provider assigns 
		a unique port to the application. 
                                                               
	Release:
		Date:
			14 january 2003
		Author:
			Luca Biancardi
		Description:
			Modified the type of the parameter SocketType.
                                                               
	Note:

---------------------------------------------------------------*/
typedef struct{
	ITF_AddressIP 
		IPAddressPC;	// Local socket ip address
	long 
		PortPC;			// Local socket port
	ITF_AddressIP 
		IPAddressDevice;// Device socket IP address
	long 
		PortDevice;		// Device socket port
	long 
		SocketType;		//  Socket Type: ITF_ETHERNET_SOCKET_TYPE
} ITF_ethNamingOpenParameter;

/*--------------------------------------------------------------
DATA ABSTRACT
                                                               
	Date:                                                   
		02 december 2002
                                                               
	Author:
		Luca Biancardi

	Structure name:
		ITF_ethNamingStatus

	Type:
		ITF_ETHERNET
                                                               
	Description:                                      
                                                               
	Release:
		Date:
			14 january 2003
		Author:
			Luca Biancardi
		Description:
			Modified the type of the parameters TSKWriteMode and TSKStatus.
                                                               
	Note:

---------------------------------------------------------------*/
typedef struct{
	long
		TSKWriteMode; // type: ITF_ETHERNET_NAMING_TSK_WRITE_MODE
	long
		TSKStatus;	  // type: ITF_ETHERNET_NAMING_TSK_STATUS
} ITF_ethNamingStatus;

/*--------------------------------------------------------------
DATA ABSTRACT
                                                               
	Date:                                                   
		10 march 2003
                                                               
	Author:
		Luca Biancardi

	Structure name:
		ITF_ethNamingAsynchStatus

	Type:
		ITF_ETHERNET
                                                               
	Description:                                      
                                                               
	Release:
		Date:
		Author:
		Description:
                                                               
	Note:

---------------------------------------------------------------*/
typedef struct{
	long
		BytesToRead,
		BytesToWrite,
		BytesWritten,
		BytesRead,
		IsFinished,
		Status;
} ITF_ethNamingAsynchStatus;

long WINAPI itf_DiffSystemTime(SYSTEMTIME *OutSystemTime, SYSTEMTIME *InSystemTime, double ms);


long WINAPI itf_SearchParameter(long *Offset, BYTE * pBuffer, long BufferSize, long code);


BOOL WINAPI itf_MessageError(long ErrCode, TCHAR *ErrMsg, long SizeErrMsg);


long WINAPI itf_PutCommandHeader(ITF_CommandHeader* Header, long Code, long RackID, long SlotID, long DataSize);


long WINAPI itf_ethNamingInitOpenParameter	(ITF_ethNamingOpenParameter *op);


HANDLE WINAPI itf_ethNamingOpen				(ITF_ethNamingOpenParameter *op);


long WINAPI itf_ethNamingClose				(HANDLE *chHandle);


long WINAPI itf_ethNamingSetParameter		(HANDLE chHandle, long code, const void *value, long size);


long WINAPI itf_ethNamingGetParameter		(HANDLE chHandle, long code, void *value, long size);


long WINAPI itf_ethNamingSearch				(HANDLE chHandle, long *nDevices, long *MaxSize);


long WINAPI itf_ethNamingGetInfo			(HANDLE chHandle, long DeviceIndex, BYTE *pData, long DataSize, long *DataRead);


long WINAPI itf_ethNamingResetDevice		(HANDLE chHandle, long DeviceIndex);


long WINAPI itf_ethNamingDigitalIO			(HANDLE chHandle, long DeviceIndex, long State);


long WINAPI itf_ethNamingWriteFile   		(HANDLE chHandle, long DeviceIndex, TCHAR *FileName, BYTE *pData, long DataSize);


long WINAPI itf_ethNamingAsynchWriteFile	(HANDLE chHandle, long DeviceIndex, TCHAR *FileName, BYTE *pData, long DataSize);


long WINAPI itf_ethNamingWriteFileAsynch   	(HANDLE chHandle, long DeviceIndex, TCHAR *FileName, BYTE *pData, long DataSize);


long WINAPI itf_ethNamingAsynchWriteFileAsynch(HANDLE chHandle, long DeviceIndex, TCHAR *FileName, BYTE *pData, long DataSize);


long WINAPI itf_ethNamingReadFile   		(HANDLE chHandle, long DeviceIndex, TCHAR *FileName, BYTE *pData, long DataSize, long *DataRead);


long WINAPI itf_ethNamingAsynchReadFile		(HANDLE chHandle, long DeviceIndex, TCHAR *FileName);


long WINAPI itf_ethNamingDeleteFile   		(HANDLE chHandle, long DeviceIndex, TCHAR *FileName);


long WINAPI itf_ethNamingWriteTsk			(HANDLE chHandle, long DeviceIndex, TCHAR *FileName);


long WINAPI itf_ethNamingAsynchWriteTsk		(HANDLE chHandle, long DeviceIndex, TCHAR *FileName);


long WINAPI itf_ethNamingAsynchPacketWriteTsk(HANDLE chHandle, long DeviceIndex, TCHAR *FileName, long PacketSize);


long WINAPI itf_ethNamingAsynchPacketWriteTsk(HANDLE chHandle, long DeviceIndex, TCHAR *FileName, long PacketSize);


long WINAPI itf_ethNamingReadStatus			(HANDLE chHandle, long DeviceIndex, ITF_ethNamingStatus *status);


long WINAPI itf_ethNamingFormatFFS			(HANDLE chHandle, long DeviceIndex);


long WINAPI itf_ethNamingDefragFFS			(HANDLE chHandle, long DeviceIndex);


long WINAPI itf_ethNamingFormatHD			(HANDLE chHandle, long DeviceIndex, long FormatMode);


long WINAPI itf_ethNamingLinker				(HANDLE chHandle, const ITF_MacAddress *MacAddress);


long WINAPI itf_ethNamingWriteDate			(HANDLE chHandle, long DeviceIndex, ITF_Date *date);


long WINAPI itf_ethNamingReadDate			(HANDLE chHandle, long DeviceIndex, ITF_Date *date);


long WINAPI itf_ethNamingWriteTime			(HANDLE chHandle, long DeviceIndex, ITF_Time *time);


long WINAPI itf_ethNamingReadTime			(HANDLE chHandle, long DeviceIndex, ITF_Time *time);


long WINAPI itf_ethNamingWriteDateTime		(HANDLE chHandle, long DeviceIndex, ITF_Date *date, ITF_Time *time);


long WINAPI itf_ethNamingReadDateTime		(HANDLE chHandle, long DeviceIndex, ITF_Date *date, ITF_Time *time);


long WINAPI itf_ethNamingReadErrors			(HANDLE chHandle, long DeviceIndex, long ErrorsNumber, BYTE *pData, long DataSize, long *DataRead);


long WINAPI itf_ethNamingAsynchReadErrors	(HANDLE chHandle, long DeviceIndex, long ErrorsNumber);


long WINAPI itf_ethNamingReadErrorsNIOSM12  (HANDLE chHandle, long DeviceIndex, long ErrorsNumber, BYTE *pData, long DataSize, long *DataRead);


long WINAPI itf_ethNamingAsynchReadErrorsNIOSM12(HANDLE chHandle, long DeviceIndex, long ErrorsNumber);


long WINAPI itf_ethNamingResetErrors		(HANDLE chHandle, long DeviceIndex);


long WINAPI itf_ethNamingWriteReadCmd		(HANDLE chHandle, long DeviceIndex, long Code, void *pUserDataTX, long UserDataSizeTX,void **pUserDataRX, long UserDataSizeRX, long *lpBytesRead);


long WINAPI itf_ethNamingAsynchAbort		(HANDLE chHandle);


long WINAPI itf_ethNamingAsynchGetData		(HANDLE chHandle, BYTE *pData, long DataSize, long *DataRead);


long WINAPI itf_ethNamingAsynchGetStatus	(HANDLE chHandle, ITF_ethNamingAsynchStatus *status);


long WINAPI itf_ethNamingFreeMemory			(HANDLE chHandle, long DeviceIndex);


long WINAPI itf_ethNamingShutDown			(HANDLE chHandle, long DeviceIndex);


long WINAPI itf_ethGetHostByName			(TCHAR *ipStr, ITF_AddressIP *ipNum, const TCHAR *Address);


long WINAPI itf_ethNamingReadTsk			(HANDLE chHandle, long DeviceIndex, TCHAR *FileName);


long WINAPI itf_ethNamingDir				(HANDLE chHandle, long DeviceIndex, BYTE *pData, long DataSize, long *DataRead);


long WINAPI itf_ethNamingAsynchDir			(HANDLE chHandle, long DeviceIndex);


long WINAPI itf_ethNamingReadFlashBinary	(HANDLE chHandle, long DeviceIndex, TCHAR *FileName);


long WINAPI itf_ethNamingWriteFlashBinary	(HANDLE chHandle, long DeviceIndex, TCHAR *FileName);


long WINAPI itf_WriteCommand(HANDLE chHandle, long SlotID, long Code);


long WINAPI itf_WriteRead(HANDLE chHandle, void *pUserDataTX, long UserDataSizeTX, void **pUserDataRX, long UserDataSizeRX, long *lpBytesRead);


long WINAPI itf_Write(HANDLE chHandle, void *pUserData, long UserDataSize);


long WINAPI itf_Read(HANDLE chHandle, void **pUserData, long UserDataSize, long *lpBytesRead);


long WINAPI itf_CleanReadBuffer(HANDLE chHandle, long *lpBytesRead, long timeout);


long WINAPI itf_BusReset(HANDLE chHandle);

/**@}*/ // end of group MISCELLANEOUS


//-----------------------------------------------------------------------------
//-----------------------------------------------------------------------------
//--- DEPRECATED FUNCTION -----------------------------------------------------
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
//-----------------------------------------------------------------------------
//--- DATA BLOCKS COMMANDS ----------------------------------------------------
//-----------------------------------------------------------------------------

/**
 * <b><i>Ask to the Device the list of the Data Blocks (BufferData)</i></b><br>
 * Returns a buffer containing the list of the Data Blocks and the respective type
 * and size
 * 
 * @param Handle communication handle as from
 *               itf_OpenChannel
 * @param NumberOfBlocks
 *               (out)
 * @param arDataBlocks
 *               (out)
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 * @deprecated
 */

long WINAPI itf_DataBlocksList(
                                   HANDLE                  Handle,
                                   long                    *NumberOfBlocks,
                                   long                    *arDataBlocks);

/**
 * <b><i>Delete a list of Data Blocks (arDataBlocks)</i></b><br>
 * 
 * @param Handle communication handle as from
 *               itf_OpenChannel
 * @param NumberOfBlocks
 *               
 * @param arDataBlocks
 *               (in)
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 * @deprecated
 */
//--- Delete a list of Data Blocks (arDataBlocks)

long WINAPI itf_DeleteDataBlock(
                                   HANDLE                  Handle,
                                   long                    NumberOfBlocks,                         // NEW PARAMETER!!!!!
                                   long                    *arDataBlocks);

/**
 * <b><i>Read a Data Block (DataBlockID) from the Device</i></b><br>
 * Returns the relative buffer (BufferData)
 * 
 * @param Handle communication handle as from
 *               itf_OpenChannel
 * @param DataBlockID
 *               
 * @param BufferData
 *               (out)
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 * @deprecated
 */

long WINAPI itf_ReadDataBlock(
                                   HANDLE                  Handle,
                                   long                    DataBlockID,
                                   void                    **BufferData);

/**
 * <b><i>Read a Data Block (DataBlockID) from the Device and save it on the specified path (FileName)</i></b><br>
 * 
 * @param Handle communication handle as from
 *               itf_OpenChannel
 * @param DataBlockID
 *               
 * @param FileName
 *               (in)
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 * @deprecated
 */

long WINAPI itf_SaveDataBlock(
                                   HANDLE                  Handle,
                                   long                    DataBlockID,
                                   char                    *FileName);

/**
 * <b><i>Write the selected Data Block on the Device</i></b><br>
 * 
 * @param Handle communication handle as from
 *               itf_OpenChannel
 * @param DataBlockID
 *               
 * @param FileName
 *               (in)
 * 
 * @return 0 if OK or an error code
 * @see itf_OpenChannel
 * @see itf_CloseChannel
 * @deprecated
 */

long WINAPI itf_WriteDataBlock(
                                   HANDLE                  Handle,
                                   long                    DataBlockID,
                                   char                    *FileName);

/**
 * <b><i>Set the directX mode used to send images from the device to the 
 * supervisor</i></b><br>
 * The directX mode elapse after 2 secs, so before it expire, a
 * keep alive call must be issued.
 * 
 * i.e.: itf_ResultImageDirectX(Handle, 2, NULL, 0);
 * 
 * The use of this API is deprecated. Use the itf_DirectXOpen() and 
 *  associated API to manage a directX channel in a easier way
 *
 * @param Handle     communication handle as from itf_OpenChannel()
 * @param action     0 = Stop, 1 = Start, 2 = Keep Alive
 * @param AdapterIP  used only with Action = start, this is the stream destination IP.
                     Use NULL if not necessary.
 * @param AdapterPort   used only with Action = start. Use NULL if
 *                   not necessary.
 *                    -   Use DEVICE_TOJECT_TX_PORT in case of TObject
 *                    -   Use DEVICE_GIGE_TX_PORT in case of GigeVision
 *
 * @return 0 if OK or an error code
 * @see itf_DirectXOpen
 * @see itf_DirectXClose
 * @see itf_DirectXAction
 * @deprecated
 *
 */

long WINAPI itf_ResultImageDirectX(HANDLE Handle, long action, char* AdapterIP, long AdapterPort);
//------------------------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------
//--- DirectX + BackWall + Gige APIs -------------------------------------------------
//-----------------------------------------------------------------------------


/**
 * <b><i>Open a direct X channel, checking the 
 * device capability and allowed protocol</i></b><br> 
 * 
 * @param DeviceIP  IP Address of the device 
 * @param AdapterIP IP address for the adapter that is
 *                  the destination of the stream that
 *                  comes from the device. Use NULL to detect
 *                  automatically
 * @param DirectXHandle
 *                  (out) resulting channel handle
 * @param useFilter true make the connection using the
 *                  filter, false to use the DirectX over
 *                  standard socket
 * @param RxQueueSizeMax
 *                  (in) number of desired incoming
 *                  buffer. Suggested value is 6
 * 
 * @return 0 if OK or an error code
 * @see itf_DirectXOpen
 * @see itf_DirectXAction
 * @see itf_DirectXClose
 * @see itf_DirectXImageGet
 * @see itf_DirectXImageUnlock
 * @see itf_DirectXImageType
 * @deprecated
 */

long WINAPI itf_DirectXOpen(char* DeviceIP, char* AdapterIP, HANDLE*  DirectXHandle, bool useFilter, long RxQueueSizeMax);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>Close current DirectX connection</i></b><br>
 * 
 * @param DirectXHandle as from itf_DirectXOpen()
 * 
 * @return 0 if OK or an error code
 * @see itf_DirectXOpen
 * @see itf_DirectXAction
 * @see itf_DirectXClose
 * @see itf_DirectXImageGet
 * @see itf_DirectXImageUnlock
 * @see itf_DirectXImageType
 * @deprecated
 */

long WINAPI itf_DirectXClose         (HANDLE* DirectXHandle);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>Tell the device to start DirectX or send a
 * keep-alive messagge</i></b><br>
 * 
 * This is a wrapper for the itf_ResultImageDirectX()
 * 
 * @param DirectXHandle
 *               as from itf_DirectXOpen()
 * @param action 0 = Stop, 1 = Start, 2 = Keep Alive
 * 
 * @return 0 if OK or an error code
 * @see itf_DirectXOpen
 * @see itf_DirectXAction
 * @see itf_DirectXClose
 * @see itf_DirectXImageGet
 * @see itf_DirectXImageUnlock
 * @see itf_DirectXImageType
 * @deprecated
 */

long WINAPI itf_DirectXAction        (HANDLE DirectXHandle, long action);
//------------------------------------------------------------------------------------------------------------------
    

/**
 * <b><i>Wait for a new image from the directX
 * channel</i></b><br>
 * 
 * @param DirectXHandle
 *                  as from itf_DirectXOpen()
 * @param hImage    image buffer handle
 * @param LostFrame this is the ID, if available, of the lost
 *                  frame, if the result is
 *                  TAG_FILTER_FRAME_LOST
 * @param WaitTime_ms
 *                  this is the timeOut in ms
 * 
 * @return TAG_FILTER_OK if there is a new image or an error 
 *         code.
 * @see TAG_FILTER_ERROR_CODE
 * @see itf_DirectXOpen
 * @see itf_DirectXAction
 * @see itf_DirectXClose
 * @see itf_DirectXImageGet
 * @see itf_DirectXImageUnlock
 * @see itf_DirectXImageType
 * @deprecated
 */

long WINAPI itf_DirectXImageGet      (HANDLE DirectXHandle, HANDLE* hImage, long* LostFrame, long WaitTime_ms);
//------------------------------------------------------------------------------------------------------------------


/**
 *  <b><i>Release the lock on current image</i></b><br>
 * 
 * @param DirectXHandle
 *               as from itf_DirectXOpen()
 * @param hImage image handle as from itf_DirectXImageGet()
 * 
 * @return 0 if OK or an error code
 * @see itf_DirectXOpen
 * @see itf_DirectXAction
 * @see itf_DirectXClose
 * @see itf_DirectXImageGet
 * @see itf_DirectXImageUnlock
 * @see itf_DirectXImageType
 * @deprecated
 */

long WINAPI itf_DirectXImageUnlock   (HANDLE DirectXHandle, HANDLE hImage);
//------------------------------------------------------------------------------------------------------------------


/**
 * <b><i>Detect the content of the hImage buffer</i></b><br> 
 * These are all possible types of data in the buffer: 
 *  
 * - RESULT_OBJECT_CODE_UNKNOWN 
 * - RESULT_OBJECT_CODE_LIVE_IMAGE 
 * - RESULT_OBJECT_CODE_PROCESS_IMAGE 
 * - RESULT_OBJECT_CODE_PROCESS_RESULTS 
 *  
 * @param hImage this is the image handle as from 
 *               itf_DirectXImageGet()
 * 
 * @return RESULT_OBJECT_CODE
 *  
 * @see RESULT_OBJECT_CODE 
 * @see itf_DirectXOpen 
 * @see itf_DirectXAction 
 * @see itf_DirectXClose 
 * @see itf_DirectXImageGet 
 * @see itf_DirectXImageUnlock 
 * @see itf_DirectXImageType 
 * @deprecated
 */

long WINAPI itf_DirectXImageType(HANDLE DirectXHandle, HANDLE hImage);
//------------------------------------------------------------------------------------------------------------------

} //extern "C"