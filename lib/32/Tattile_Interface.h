/*!
	@file Tattile_Interface.h
	@brief Tattile APIs.
*/

#pragma once

/*! 
	Il seguente blocco ifdef rappresenta il modo standard di creare macro che semplificano l'esportazione da una DLL. 
	Tutti i file all'interno della DLL sono compilati con il simbolo TATTILE_INTERFACE_EXPORTS definito nella riga di 
	comando. Questo simbolo non deve essere definito in alcun progetto che utilizza questa DLL. In questo modo qualsiasi 
	altro progetto i cui file di origine includono questo file vedranno	le funzioni	TATTILE_INTERFACE_API come importate 
	da una DLL, mentre la DLL vedra' i simboli definiti con questa macro	come esportati.	La DLL potra' essere compilata 
	in modo statico o dinamico: nel caso venga compilata in modo statico i progetti che utilizzeranno questa DLL 
	dovranno definire il simbolo TATTILE_INTERFACE_STATIC.
*/

#ifdef TATTILE_INTERFACE_EXPORTS
#ifndef	TATTILE_INTERFACE_STATIC
#define		TATTILE_INTERFACE_API extern "C" __declspec(dllexport)
#else
#define		TATTILE_INTERFACE_API 
#endif
#else
#ifndef	TATTILE_INTERFACE_STATIC
#define		TATTILE_INTERFACE_API extern "C" __declspec(dllimport)
#else
#define		TATTILE_INTERFACE_API
#endif
#endif

/*!
	Camera info struct
	@brief Structure for camera infos management
*/
typedef struct {
	int	serialNumber;			/**< Device serial number */
	int	type;					/**< F0XXXX code as on the camera */
	char description[256];		/**< Device software description. Example: "Antares compatible device" */
	char model[64];				/**< F0XXXX code as on the camera */
	char vendor[64];			/**< Tattile s.r.l. */
	int	widthImage;				/**< Image width in pixel */
	int	heightImage;			/**< Image height in pixel */
	int	shutterMin;				/**< Minimum valid shutter value */
	int	shutterMax;				/**< Maximum valid shutter value */
	int bitDepth;				/**< Color depth */
	int headNumber;				/**< Head number in case of multi-camera device */
	int	gainMin;				/**< Minimum valid gain value */
	int	gainMax;				/**< Maximum valid gain value */
	unsigned long ipAddress;	/**< Camera IP address (A1.A2.A3.A4) in a 32 bit format 0xAABBCCDD where: \n
								*	 AA = byte A1 => IP & 0xFF000000 \n
								*	 BB = byte A2 => IP & 0x00FF0000 \n
								*	 CC = byte A3 => IP & 0x0000FF00 \n
								*	 DD = byte A4 => IP & 0x000000FF \n
								*/
	unsigned long pcIfAddress;	/**< PC interface IP (A1.A2.A3.A4) address to which the camera is connected
								*    in a 32 bit format 0xAABBCCDD where: \n
								*	 AA = byte A1 => IP & 0xFF000000 \n
								*	 BB = byte A2 => IP & 0x00FF0000 \n
								*	 CC = byte A3 => IP & 0x0000FF00 \n
								*	 DD = byte A4 => IP & 0x000000FF \n
								*/
} TI_CameraInfo;

/*!
	Camera parameters struct
	@brief Structure for camera parameter management
*/
typedef struct {
	int	numberOfFrames;		/**< Number of captured frames (from acquisition start) */
	int shutter;			/**< Shutter time value in us */
	int gain;				/**< Gain value */
	int strobeEnable;		/**< Strobe enable = 1, disable = 0 */ 
	int	strobeTime;			/**< Strobe time value in us */
	int acquisitionDelay;	/**< Delay time (in us) between trigger rise and acquisition start */
	int gainBlue;			/**< Blue channel gain */
	int gainRed;			/**< Red channel gain */
	int gainGreen;			/**< Green channel gain */
	int strobeOut;			/**< Current strobe output signal (0-3) */
	int triggerInput;		/**< Current trigger input signal (0-7) */
	int aoiEnable;			/**< Enable/disable AOI */
	int aoiStartCol;		/**< Start AOI column */
	int aoiStartRow;		/**< Start AOI row */
	int aoiResX;			/**< AOI X resolution */
	int aoiResY;			/**< AOI Y resolution*/
	int illuminationSetup;	/**< [0,1]: 0 backlight, 1 luce da sotto */
} TI_AcquisitionParameters;

/*!
	ROI struct
	@brief Structure for ROI management
*/
typedef struct  {
	int	id;		/**< ROI id value */
	int	type;	/**< ROI type: 0->rectangular, 1->circular, etc... */
	int	x1;		/**< Region start, X coordinate (top left x) */
	int x2;		/**< Region end, X coordinate (bottom right x) */
	int y1;		/**< Region start, Y coordinate (top left y) */
	int y2;		/**< Region end, Y coordinate (bottom right y) */ 
	int	color;	/**< Mark color for region highlight */
	int fill;	/**< Enable/disable region filling */
	int plot;	/**< Enable/disable region plot on image (0 hide, 1 plot) */
} TI_ROI;

/*! 
	Result image directx action: start or stop result image directx stream
*/
typedef enum {
	DIRECTX_RESULT_IMAGE_STOP,	/**< Stop result image stream */
	DIRECTX_RESULT_IMAGE_START,	/**< Start result image stream */
} TI_DirectxResultAction;

/*!
	Tattile camera type
*/
typedef enum {
	TYPE_M9 = 576,		/**< Camera type M9 */
	TYPE_M12 = 535,		/**< Camera type M12 */
} TI_CameraType;

#define	M12_MAX_HEAD_NUM	4

/* These are internal utility functions */
long TI_ReadVariableInt(HANDLE channel, char* tableName, long sheetIndex, char* varName, int *value);
long TI_WriteVariableInt(HANDLE channel, char* tableName, long sheetIndex, char* varName, int value);

/*!
	@fn void TI_InitDll()
	@brief This must be the first dll function call. It makes the needed dll initialization and memory allocation.

*/
TATTILE_INTERFACE_API long TI_InitDll();

/*!
	@fn void TI_CloseDll()
	@brief This function must be called before program termination. This function automatically
		closes the open connections and deallocates the memory.
*/
TATTILE_INTERFACE_API long TI_CloseDll();

/*!
	@fn long TI_ConnectionOpen(int *totalNumberCameras)
	@brief Returns the number of connected cameras. Any time you call this function
		the camera enumaration could change.

	@param totalNumberCamera [out] Total number of connected cameras. Connected camera IDs go from 0 to (totalNumberCamera - 1)
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_ConnectionOpen(int *totalNumberCameras);

/*!
	@fn long TI_ConnectionClose(int idCamera)
	@brief Close the connection with a specified camera. If -1 is passed all connections are closed.

	@param idCamera Close the connection with the camera with specified ID. ID = -1 if you want to close all the connections.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_ConnectionClose(int idCamera);

/*!
	@fn long TI_InfoGet(int idCamera, TI_CameraInfo *cameraInfo)
	@brief Returns the camera related informations.

	@param idCamera Get informations from the camera with specified ID.
	@param cameraInfo [out] Structure filled with requested informations.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_InfoGet(int idCamera, TI_CameraInfo *cameraInfo);

/*!
	@fn long TI_AcquisitionParametersGet(int idCamera, TI_AcquisitionParameters *acquisitionParameters)
	@brief Returns the camera related parameters.

	@param idCamera Get parameters from the camera with specified ID.
	@param acquisitionParameters [out] Structure filled with requested parameters.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_AcquisitionParametersGet(int idCamera, TI_AcquisitionParameters *acquisitionParameters);

/*!
	@fn long TI_AcquisitionParametersSet(int idCamera, TI_AcquisitionParameters *acquisitionParameters)
	@brief Set the camera parameters specified in the input structure. If ID = -1 is specified, parameters will be
		applied to all connected cameras.

	@param idCamera Set parameters of the camera with specified ID. ID = -1 to set parameters to all connected cameras.
	@param acquisitionParameters Structure with parameters values to set.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_AcquisitionParametersSet(int idCamera, TI_AcquisitionParameters *acquisitionParameters);

/*!
	@fn long TI_Reset(int idCamera)
	@brief Reset the camera with the specified ID. ID = -1 to reset all cameras.

	@param idCamera ID of the camera to reset. If -1 is passed reset command is send to all cameras.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_Reset(int idCamera);

/*!
	@fn long TI_ResultImageDirectX(int idCamera, long action, long adapterPort)
	@brief Starts or stops the result image streaming from the selected camera to the chosen port.

	@param idCamera ID of the camera on which the action must be executed.
	@param action DIRECTX_RESULT_IMAGE_STOP or DIRECTX_RESULT_IMAGE_START
	@param adapterPort The port to which the stream must be sent
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_ResultImageDirectX(int idCamera, long action, long adapterPort);

/*!
	@fn long TI_StartRun(int idCamera)
	@brief Starts the loaded program on the target device.

	@param idCamera ID of the camera on which the action must be executed.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_StartRun(int idCamera);

/*!
	@fn long TI_StopRun(int idCamera)
	@brief Stop the loaded program on the target device.

	@param idCamera ID of the camera on which the action must be executed.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_StopRun(int idCamera);

/*!
	@fn long TI_GetRunStatus(int idCamera, long *programStatus)
	@brief Get the program status of the camera.

	@param idCamera ID of the camera which must be query for status.
	@param programStatus 
			-1 if the device is in Error,
			0 the program is stopped,
			1 the program is running,
			3 the program is running, but the device is grabbing from current camera. 

	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_GetRunStatus(int idCamera, long *programStatus);

/*!
	@fn long TI_GetFirmwareVersion(int idCamera, char *firmwareVersion, long size)
	@brief Get the firmware version of the camera.

	@param idCamera ID of the camera which must be query for firmware version.
	@param firmwareVersion Array which will be filled with firmware version string. 
	@param size Size of firmwareVersion array.

	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_GetFirmwareVersion(int idCamera, char *firmwareVersion, long size);