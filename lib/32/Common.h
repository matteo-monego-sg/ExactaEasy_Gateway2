/*!
	@file Common.h
	@brief Common APIs.
*/

#pragma once

#include "Tattile_Interface.h"

typedef struct {
	int data_size;
	int *data_used;
	unsigned char *data;
} TI_BinaryData;

typedef struct  {
	int data_size;
	int *data_used;
	unsigned char *data;
} TI_TextData;


/*!
	Stop on condition struct
	@brief Structure to set stop condition
*/
typedef struct {
	int	headNumber;		/**< Head number on which condtion must be applied. 
						0 = any head (Particles); -1 = any head (Cosmetic) */
	int resultType;		/**< 0 = good, 1 = reject, 2 = any condition (good/reject), 3 = P.U.M. (only particles) */
} TI_StopCondition;

/*!
	@fn long TI_StatusGet(int idCamera, int *cameraStatus)
	@brief Get the working status of the camera specified as input parameter.

	@param idCamera ID of the camera from which retrieve information.
	@param cameraStatus [out] Camera working status.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE TI_ParticlesStatus
*/
TATTILE_INTERFACE_API long TI_StatusGet(int idCamera, int *cameraStatus);

/*!
	@fn long TI_RecipeSimpleSet(int idCamera, void *recipeSimple)
	@brief Set a subset of recipe parameters on the selected camera.

	@param idCamera ID of the camera for which you want to set the parameters.
	@param recipeSimple Pointer to a structure containing the parameters.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE TI_RecipeSimple TI_RecipeSimpleCosmetic
*/
TATTILE_INTERFACE_API long TI_RecipeSimpleSet(int idCamera, void *recipeSimple);

/*!
	@fn long TI_RecipeSimpleGet(int idCamera, void *recipeSimple)
	@brief Get a subset of recipe parameters from the selected camera.

	@param idCamera ID of the camera from which you want to get the parameters.
	@param recipeSimple [out] Pointer to a structure which will be filled with the parameters.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE TI_RecipeSimple TI_RecipeSimpleCosmetic
*/
TATTILE_INTERFACE_API long TI_RecipeSimpleGet(int idCamera, void *recipeSimple);

/*!
	@fn long TI_RecipeAdvancedSet(int idCamera, void *recipeAdvanced)
	@brief Set a subset of recipe parameters on the selected camera.

	@param idCamera ID of the camera for which you want to set the parameters.
	@param recipeAdvanced Pointer to a structure containing the parameters.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE TI_RecipeAdvanced TI_RecipeAdvancedCosmetic
*/
TATTILE_INTERFACE_API long TI_RecipeAdvancedSet(int idCamera, void *recipeAdvanced);

/*!
	@fn long TI_RecipeAdvancedGet(int idCamera, void *recipeAdvanced)
	@brief Get a subset of recipe parameters from the selected camera.

	@param idCamera ID of the camera from which you want to get the parameters.
	@param recipeAdvanced [out] Pointer to a structure which will be filled with the parameters.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE TI_RecipeAdvanced TI_RecipeAdvancedCosmetic
*/
TATTILE_INTERFACE_API long TI_RecipeAdvancedGet(int idCamera, void *recipeAdvanced);

/*!
	@fn long TI_FeaturesEnableSet(int idCamera, void *featuresEnable)
	@brief Set a subset of parameters used to enable/disable some features on the selected camera.

	@param idCamera ID of the camera on which you want to set the parameters.
	@param featuresEnable Pointer to a structure containing the parameters.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE TI_FeaturesEnable TI_FeaturesEnableCosmetic
*/
TATTILE_INTERFACE_API long TI_FeaturesEnableSet(int idCamera, void *featuresEnable);

/*!
	@fn long TI_FeaturesEnableGet(int idCamera, void *featuresEnable)
	@brief Get a subset of parameters used to enable/disable some features from the selected camera.

	@param idCamera ID of the camera from which you want to get the parameters.
	@param featuresEnable [out] Pointer to a structure which will be filled with the parameters.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE TI_FeaturesEnable TI_FeaturesEnableCosmetic
*/
TATTILE_INTERFACE_API long TI_FeaturesEnableGet(int idCamera, void *featuresEnable);

/*!
	@fn long TI_MachineParametersSet(int idCamera, void *machineParameters)
	@brief Set a subset of parameters used to configure the machine on the selected camera.

	@param idCamera ID of the camera on which you want to set the parameters.
	@param machineParameters Pointer to a structure containing the parameters.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE TI_MachineParameters TI_MachineParametersCosmetic
*/
TATTILE_INTERFACE_API long TI_MachineParametersSet(int idCamera, void *machineParameters);

/*!
	@fn long TI_MachineParametersGet(int idCamera, void *machineParameters)
	@brief Get a subset of parameters used to configure the machine from the selected camera.

	@param idCamera ID of the camera from which you want to get the parameters.
	@param machineParameters [out] Pointer to a structure which will be filled with the parameters.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE TI_MachineParameters TI_MachineParametersCosmetic
*/
TATTILE_INTERFACE_API long TI_MachineParametersGet(int idCamera, void *machineParameters);

/*!
	@fn long TI_ResetCounters(int idCamera)
	@brief Reset the statistics on the selected camera.

	@param idCamera ID of the camera on which you want to reset the statistics.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_ResetCounters(int idCamera);

/*!
	@fn long TI_StopOnCondition(int idCamera, TI_StopCondition *stopCondition)
	@brief Stop the normal work on the selected camera when the set condition is matched.

	@param idCamera ID of the camera on which stop condition must be set.
	@param stopCondition Pointer to a structure containing the stop condition parameters.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE TI_StopCondition
*/
TATTILE_INTERFACE_API long TI_StopOnCondition(int idCamera, TI_StopCondition *stopCondition);

/*!
	@fn long TI_StopOnConditionExit(int idCamera)
	@brief Disable the stop on condition mode.

	@param idCamera ID of the camera on which stop condition must be disabled.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_StopOnConditionExit(int idCamera);

/*!
	@fn long TI_BufferSet(int idCamera, int bufferType, int bufferNumber, char *fileName)
	@brief Set the buffer of "bufferType" with the image contained in the file specified as input parameter.

	@param idCamera ID of the camera from which you want to set the buffer.
	@param bufferType Type of buffer as specified by TI_BufferType.
	@param bufferNumber Number of buffer to be set.
	@param fileName Name of the file containing the images to set.
	@return 0 if ok, -1 if bufferNumber is out of range, else interface error code
	@see ITF_ERROR_CODE TI_BufferType
*/
TATTILE_INTERFACE_API long TI_BufferSet(int idCamera, int bufferType, int bufferNumber, char *fileName);

/*!
	@fn long TI_BufferGet(int idCamera, int bufferType, int bufferNumber, int maxBufferSize, unsigned char *buffer, int *bufferSize)
	@brief Get a number of buffer, as specified in bufferNumber, from "bufferType" bank. The buffer are placed in the memory location
		pointed by buffer and their size is placed in bufferSize. If bufferNumber = -1, all the buffer of type "bufferType" are returned.

	@param idCamera ID of the camera from which you want to retrieve the buffers.
	@param bufferType Type of buffer as specified by TI_BufferType.
	@param bufferNumber Position of requested buffer, 0 = last captured image.
	@param maxBufferSize Size of allocated memory pointed by "buffer".
	@param buffer [out] Buffer containing the requested images.
	@param bufferSize [out] Size of the output buffer actually allocated.
	@return 0 if ok, -1 if bufferNumber is out of range, else interface error code
	@see ITF_ERROR_CODE TI_BufferType
*/
TATTILE_INTERFACE_API long TI_BufferGet(int idCamera, int bufferType, int bufferNumber, int maxBufferSize, unsigned char *buffer, int *bufferSize);

/*!
	@fn long TI_LiveStart(int idCamera, bool sensorWindowed)
	@brief Start live mode with full sized sensor or windowed sensor, depending on input parameter.

	@param idCamera Start live mode on the camera with specified ID.
	@param sensorWindowed Live mode can run with full sized sensor or windowed sensor.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_LiveStart(int idCamera, bool sensorWindowed);

/*!
	@fn long TI_LiveStop(int idCamera)
	@brief Stop live mode on specified camera.

	@param idCamera Stop live mode on the camera with specified ID.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_LiveStop(int idCamera);

/*!
	@fn long TI_ROIGetNumber(int idCamera, int *ROINumber)
	@brief Get the number of ROI from the camera specified with the ID.

	@param idCamera ID of the camera from which retrieve the number of ROI.
	@param ROINumber [out] The number of ROI of the specified camera.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE TI_ROI
*/
TATTILE_INTERFACE_API long TI_ROIGetNumber(int idCamera, int *ROINumber);

/*!
	@fn long TI_ROIGet(int idCamera, int idROI, TI_ROI *ROIValue)
	@brief Get the ROI properties from camera with specified ID.

	@param idCamera ID of the camera from which retrieve ROI properties.
	@param ROIValue [out] ROI properties data. The structure is filled with retrieved data.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE TI_ROI
*/
TATTILE_INTERFACE_API long TI_ROIGet(int idCamera, int idROI, TI_ROI *ROIValue);

/*!
	@fn long TI_ROISet(int idCamera, int idROI, TI_ROI *ROIValue)
	@brief Set the ROI properties from camera with specified ID.

	@param idCamera ID of the camera for which ROI parameters must be set.
	@param ROIValue ROI properties data. The structure must be filled with data which must be set
		on the specified camera.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE TI_ROI
*/
TATTILE_INTERFACE_API long TI_ROISet(int idCamera, int idROI, TI_ROI *ROIValue);

/*!
	@fn long TI_AnalysisOffLineStart(int idCamera)
	@brief Switch the camera modality to offline analysis.

	@param idCamera ID of the camera on which set the offline mode.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_AnalysisOffLineStart(int idCamera);

/*!
	@fn long TI_AnalysisOffLineStop(int idCamera)
	@brief Stop the offline analysis mode on the selected camera

	@param idCamera ID of the camera on which stop the offline mode.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_AnalysisOffLineStop(int idCamera);

/*!
	@fn long TI_AssistantPrepare(int idCamera, int mode)
	@brief Prepare the procedure you want to execute on the camera.

	@param idCamera ID of the camera for which you want to start the procedure.
	@param mode The procedure to start choosed between valid TI_PatriclesMode or TI_CosmeticMode.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE TI_PatriclesMode
*/
TATTILE_INTERFACE_API long TI_AssistantPrepare(int idCamera, int mode);

/*!
	@fn long TI_AssistantSet(int idCamera, int mode, void *inputVars)
	@brief Set the parameters of the assisted modality selected by TI_AssistantPrepare. 
		The parameters depend on the choosed modality you are currently executing on the camera.

	@param idCamera ID of the camera for which you want to set the procedure parameters.
	@param mode The procedure to start choosed between valid TI_PatriclesMode or TI_CosmeticMode.
	@param inputVars Pointer to a structure containing the procedure parameters.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE TI_PatriclesMode
*/
TATTILE_INTERFACE_API long TI_AssistantSet(int idCamera, int mode, void *inputVars);

/*!
	@fn long TI_AssistantStart(int idCamera)
	@brief Start the assisted modality selected by TI_AssistantPrepare.

	@param idCamera ID of the camera on which you want to start the procedure.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_AssistantStart(int idCamera);

/*!
	@fn long TI_AssistantStop(int idCamera, bool saveParam)
	@brief Stop the assisted modality started by the TI_AssistantStart. You can save the found parameters or discard them.

	@param idCamera ID of the camera for which you want to stop the procedure.
	@param saveParam Choose to save or discard parameters found during procedure execution.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_AssistantStop(int idCamera, bool saveParam);

/*!
	@fn long TI_GetProgramVersion(int idCamera, char *firmwareVersion, long size)
	@brief Get the program version of the camera.

	@param idCamera ID of the camera which must be query for program version.
	@param programVersion Array which will be filled with program version  string.
	@param size Size of programVersion array.

	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_GetProgramVersion(int idCamera, char *programVersion, long size);

/*!
	@fn long TI_CommitChanges(int idCamera)
	@brief Apply parameters modification on the camera.

	@param idCamera ID of the camera which parameters must be committed.

	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_CommitChanges(int idCamera);