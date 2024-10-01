/*!
	@file Particles.h
	@brief Particles APIs.
*/

#pragma once

#include "Tattile_Interface.h"

#define	FORMAT_MAX_NUMBER			8

#define	TABLE_PARTICLES_GENERAL		"Particles_general"
#define	TABLE_PARTICLES_RECIPE		"Particles_recipe"
#define	TABLE_PARTICLES_STATUS		"Particles_status"
#define	TABLE_PARTICLES_LEARNING	"Particles_learning"
#define	TABLE_PARTICLES_MACHINE		"Particles_Machine"
#define	TABLE_PARTICLES_VIALAXIS	"Particles_VialAxis"
#define	TABLE_PARTICLES_KNAPP		"Particles_knapp"
#define	TABLE_PARTICLES_TEMP		"Particles_temp"
#define	TABLE_PARTICLES_SYSTEM		"System"

/*! 
	Camera modes: to be used with TI_AssistantPrepare and TI_AssistantSet for learning procedures.
*/
typedef enum {
	MODE_INITIALIZATION = 0,					/**< Initialization mode */
	MODE_NORMAL_WORK = 100,						/**< Normal operation work */
	MODE_LOAD_FORMAT = 900,						/**< Load format parameters */
	MODE_STOP_ON_CONDITION = 1000,				/**< stop_on_condition status */
	MODE_ANALYSIS_OFF_LINE = 1100,				/**< OFF-line analysis status */
	MODE_LEARNING_ANALYSIS_PARAMETER = 1300,	/**< Learning analysis_parameter status */
	MODE_LEARNING_STROBO = 1400,				/**< Learning strobo status */
	MODE_LEARNING_VIALAXIS = 1500,				/**< Learning vial axis status */
	MODE_LEARNING_RESET_TRIGGER_ENCODER = 1700,	/**< Learning reset trigger encoder status */
	MODE_LEARNING_NORMALIZATION = 1900,			/**< Learning normalization status */
	MODE_CHECK_VIALAXIS = 2000,					/**< Check vial axis status */
	MODE_CHECK_CONTROROTATION = -1,				/**< Check controrotation status */
	MODE_LIVE = 2200,							/**< Live status */
	MODE_GENERIC_ERROR = -1000,					/**< Generic error status */
} TI_PatriclesMode;

/*! 
	Buffer types: to be used with TI_BufferGet and TI_BufferSet.
*/
typedef enum {
	BUFFER_ACQUISITION,		/**< Acquisition images buffers */
	BUFFER_MENISCUS,		/**< Meniscus images buffers */
	BUFFER_DIFF,			/**< Difference images buffers */
	BUFFER_RESULTS,			/**< Result images buffer */
	BUFFER_TRACKING,		/**< Tracking image buffer */
	BUFFER_TRAJECTORIES,	/**< Trajectories buffer */
	BUFFER_TEMPORARY,		/**< Temporary image buffer (for debug purpose) */
} TI_BufferType;

/*! 
	Camera ROI available area: to be used with ROI functions when idROI is required.
*/
typedef enum {
	ROI_DEFAULT_AREA,				/**< First ROI area enumeration */
	ROI_OD_AREA,					/**< Second ROI area enumeration */
	PARTICLES_ROI_AREA_NUMBER,		/**< Totoal ROI area of Particles application */
} TI_ROIAreaParticles;

typedef enum {
	STATUS_INIT,						/**< Initializing camera */
	STATUS_ERROR,						/**< Error status */
	STATUS_WAITING_TRIGGER,				/**< Waiting for trigger */
	STATUS_ACQUISITION,					/**< Acquisition */
	STATUS_ANALYSIS_WAITING_TRIGGER,	/**< Running analysis and waiting for trigger */
	STATUS_ACQUISITION_AND_ANALYSIS,	/**< Acquisition and analysing */
	STATUS_LEARNING,					/**< Learning */
} TI_ParticlesStatus;

/*!
	Strobo learning parameters
	@brief Structure for strobo learning procedure
*/
typedef struct {
	int size;					/**< Size in bytes of structure */
	int maxDensityReference;	/**< Reference level for learning process */
	int stroboTime;				/**< Strobo time */
	int shutterTime;			/**< Shutter time */
} TI_LearningStrobo;

/*!
	Normalization learning parameters
	@brief Structure for normalization learning procedure
*/
typedef struct {
	int size;					/**< Size in bytes of structure */
	int pgaRawStart;
	int pgaRawEnd;
} TI_LearningNormalization;

/*!
	Analysis learning parameters
	@brief Structure for analysis parameters learning procedure
*/
typedef struct {
	int size;					/**< Size in bytes of structure */
	int samples;				/**< Number of sample for learning procedure */
	int blobsMaxArea;
	int blobsMinArea;
	int trajectoryMinArea;
	int fillingLevelMin;
	int fillingLevelMax;
} TI_LearningAnalysisParameters;

/*!
	Vial axis learning parameters
	@brief Structure for vial axis learning procedure
*/
typedef struct {
	int size;					/**< Size in bytes of structure */
	int samples;				/**< Number of sample for learning procedure */
	int findAxisX;
	int findAxisY;
} TI_LearningVialAxis;

/*!
	Reset trigger encoder learning parameters
	@brief Structure for reset trigger encoder procedure
*/
typedef struct {
	int size;					/**< Size in bytes of structure */
	int findAxisX;
	int findAxisY;
	int encoderTrigger;
} TI_LearningResetTriggerEncoder;

/*!
	Knapp head struct
	@brief Structure for Knapp variable management
*/
typedef struct {
	int	Samples;
	float RejectedPerc;
	float BlobsArea;
	float BlobsNumber;	
	float BlobsRejected;	
	float BubblesNumber;	
	float PUM;
	float POB;
	float TrajectoriesNumber;
	int	TrajectoriesRejectMinArea;
	float TrajectoriesRejectSegmentNumber;
} TI_KnappHead;

#define	KNAPP_HEAD_NUM	64

/*!
	Knapp result struct
	@brief Structure for Knapp results retrievement and management
*/
typedef struct {
	long totalHeads;					/**< Total number of sheets of Knapp data */
	TI_KnappHead head[KNAPP_HEAD_NUM];	/**< Pointer to a struct containing variable data */
} TI_KnappResults;

/*!
	Vial axis data struct
	@brief Structure for vial axis data
*/
typedef struct {
	int cnt;
	int dummy;
	int sum;
	int temp;
	int x;
} TI_VialAxisData;

#define	VIALAXIS_RESULT_NUM		63
#define VIALAXIS_STRUCT_CODE	0xAA01
/*!
	Vial axis result struct
	@brief Structure for vial axis results retrievement and management
*/
typedef struct {
	int code;				/**< Must always match VIALAXIS_STRUCT_CODE */
	int filledDataNumber;	/**< The number of valid entry in the data[] field */
	TI_VialAxisData data[VIALAXIS_RESULT_NUM];
} TI_AxisBuffer;

/*!
	Recipe simple variable struct for Particles
	@brief Structure to retrieve and set simple recipe variable
*/
typedef struct {
	int	rleLowThreshold;							/**< Binary threshold for FPGA rle extraction. */
	int	ROIDynamicHighThreshold;					/**< Image binary threshold. */ 
	int	ROIDynamicLowThreshold; 					/**< Image binary threshold. */ 
	int ROIDynamicBottonReductcion;	
	int ROIDynamicMeniscusReductcion;
	int ROIDynamicBottomExpansionYStart;			/**<  0: espande in automatico nella sola regione del fondo
													*	maggiore di 0: espande a partire dal valore di Y specificato
													*	minore di 0: espande a partire dal livello di riempimento applicando un offset pari al valore specificato */
	int	meniscusPUMThreshold;						/**< Binary threshold of vertical ruler for particles search under the meniscus.
													 Vertical ruler stops when it finds a gray level lower than the one set as threshold. */
	int	blobMaxAreaReject; 							/**< If blob area is greater than this value the sample is rejected and 
													 Particles_RAWDATA[*].reject_for_blob_area_max is set to 1 */
	int	blobsMinArea; 								/**< Blobs which area is lower than this value are discarded and they won't be used for tracking. */
	int blobMaxAreaRejectLowThreshold;
	int fillingLevelMin;							/**< Minimum filling level of sample */
	int fillingLevelMax;							/**< Maximum filling level of sample */
	int minDensityReference;						/**< Optival density low threshold. Under this value the device rises an alarm. */
	int blobMaxAreaRejectConsecutiveFramesNumber;
	int bottomAnalysisThreshold;
	int	blobsMaxNumber;								/**< If blobs number exceeds this value sample will be rejected. */
	int ROIDynamicMeniscusSearchYStart;				/**< Coordinata y iniziale che definisce la fascia in cui cercare la regione del menisco */
	int ROIDynamicMeniscusSearchYEnd;				/**< Coordinata y finale che definisce la fascia in cui cercare la regione del menisco. Per non impostare una 
													fascia in cui cercare il menisco, ma lasciare eseguire la ricerca su tutta la roi di analisi come in precedenza 
													è sufficiente lasciare entrambe queste coordinate a 0, o assegnare a questi parametri valori uguali */
	int ROIDynamicBottomSearchYStart;				/**< Coordinata y iniziale che definisce la fascia in cui cercare la regione del fondo. Se viene lasciata a 0 
													oppure ad un valore fuori dalla roi di analisi, viene stabilita in modo automatico, come nelle versioni precedenti, 
													la coordinata y di partenza. La coordinata di fine ricerca è il bordo inferiore della regione di analisi */
	int ROIDynamicBottomSearchThreshold;			/**< Soglia di ricerca dei righelli verticali di rilevamento del profilo del fondo */
	int bottomAnalysisAreaThreshold;				/**< Soglia sul cumulo dell’area delle sporgenze del profilo del fondo */
} TI_RecipeSimple;

/*!
	Recipe advanced variable struct for Particles
	@brief Structure to retrieve and set advanced recipe variable
*/
typedef struct {
	int	morphologicalFilter1;				/**< First morphological filter of FPGA. */
	int	morphologicalFilter2; 				/**< Second morphological filter of FPGA. */
	int	rleBufferSize;						/**< Buffer dimnesion of Particles_RLE[*]. */
	int	ROIDynamicMorphIter;				/**< If illumination_setup == 0, number of erosion of minimum image,
											 If illumination_setup == 1, number of expansion of maximum image. */
	int	meniscusAnalysisMinCount;			/**< Minimum number of images which must contain the particles. Sample is rejected if exceed this value. */
	int	meniscusAnalysisThreshold;			/**< Sensitivity of meniscus analysis */
	int	meniscusPUMOffset;					/**< Number of pixel under the meniscus where particles search is extended. */
	float bubbleMaxShapeFactor;				/**< 0 -> 1 : max rate between the two edges of the rectangle containing the blob. */
	int	bubbleOpticalDensityMinDiff;		/**< Difference between	the density of blob center and external areola. */
	int	bubbleSizeMaximum;					/**< Maximum size of a blob to be checked. */
	int	bubbleSizeMinimum; 					/**< Minimum size of a blob to be checked. */
	float trackingMaxAcceleration;			/**< Maximum allowed acceleration for a blob. 0 -> infinity */
	float trackingMaxAreaRatio;				/**< Maximum ratio between max and min area of two different blobs. 1 -> infinity */
	float trackingMaxHeightRatio;			/**< Maximum ratio between max and min height of two different blobs. 1 -> infinity */
	float trackingMaxWidthRatio;			/**< Maximum ratio between max and min width of two different blobs. 1 -> infinity */
	int	trackingMaxSegmentAngle;			/**< Maximum angle of a segment which virtually connects two blobs respect to horizontal line. 0 -> 90 */ 
	int	trackingMaxTrajectoryAngle;			/**< Maximum angle between two sequential segments. 0 -> 180 */
	int	trackingMinDistance;				/**< Minimum distance between blobs. It actually limits the minium particles velocity. */
	int	trackingNumberFramesBackward;		/**< Backward frame number to which a blob can be connected. 1 -> 3 */ 
	int	trackingFrameBackwardReduction;		/**< Backward frame parameter reduction. */ 
	int	trajectoriesBufferSize;				/**< Maximum size, in bytes, of Particles_temp.trajectories_buffer variable */
	float trajectoryAllSegmentsMaxLength;	/**< If the size of all segments is included between these boundaries, the trajectory is delete. */
	float trajectoryAllSegmentsMinLength;	/**< If the size of all segments is included between these boundaries, the trajectory is delete. */
	int	trajectoryRecurrentBackward; 		/**< Number of bacward samples with which the current trajectory must be compared. */
	int	trajectoryRecurrentMaxAngle; 		/**< Maximum mean angle of trajectory. */
	int	trajectoryRecurrentMaxDistance;		/**< Maximum distance between two blobs belonging to two different trajectories */
	int	trajectoryRecurrentMinCount; 		/**< Minimum number of similar trajectories that must be found to consider the current trajectory recurrent. */
	int	trajectoryRecurrentMinLength; 		/**< Minimum number of blobs which position must be similar */
	int	maskingBlobsSensibility; 			/**< Masking sensibility. Over this value blobs are discarded. */
	int	maskingBlobsThreshold;				/**< Masking threshold. Under this value blobs are discarded. */
	int encoderCounterFirstImage;			/**< Encoder counter value corresponding to first image. */
	int bottomAnalysisMinCount;
	int	trackingMaxDistance; 				/**< Maximum distance between blobs. It actually limits the maximum particles velocity. */
	int	trajectoryMaxAngle; 				/**< Maximum (mean) angle for a trajectory. It is used to discard trajectories with a too much accentuated 
											 vertical component. */
	int	trajectoryMinArea; 					/**< Minimum size of searched particles. */
	int	trajectoryMinSegmentNumber;			/**< Minimum number of segment for a trajectory to be accepted. */
	int upperMeniscusRoiOffset;
	int upperMeniscusRoiType;
	int upperMeniscusRoiWidth;
	int upperMeniscusRoiHeight;
	int bottomAnalysisFilterSize;
	int meniscusAnalysisFilterSize;
	int upperMeniscusMaxAreaReject;
	int upperMeniscusFramesNumber;
	int recurrentMinNumber;					/**< Numero minimo di ricorrenze nello storico degli ultimi 32 campioni affinché un blob sia considerato ricorrente e quindi venga scartato. */
	int recurrentMinBlobsArea;				/**< Area minima dei blob che devono passare attraverso il controllo dei blob ricorrenti. */
	int recurrentMaxBlobsArea;				/**< Area massima dei blob che devono passare attraverso il controllo dei blob ricorrenti. */
	int recurrentMaxBlobsDistance;			/**< Distanza massima tra due blob in treni successivi affinché siano considerati lo stesso blob. */
	int recurrentMaxAreaRatioPerc;			/**< Rapporto massimo percentuale ammesso tra due blob in treni successivi affinché siano considerati lo stesso blob. 
											Per esempio un valore di 200 indica che i due blob in treni successivi possono avere un’area al massimo doppia per 
											poter essere considerati lo stesso blob.*/
	int upperMeniscusRoiOffset2;			/**< Imposta offset bordo inferiore regione di analisi a partire dal livello di riempimento. */
	int upperMeniscusRoiWidth2;				/**< Larghezza roi aggiuntiva (viene comunque limitata alla larghezza della roi di analisi rossa) */
	int upperMeniscusRoiHeight2;			/**< Altezza roi aggiuntiva (viene comunque limitata in altezza in modo che non esca dalla roi di analisi rossa) */
	int upperMeniscusMaxAreaReject2;		/**< Soglia di scarto per area massima per I blob nella seconda regione sopra il menisco. */
	int upperMeniscusFramesNumber2;			/**< Numero di frame consecutivi in cui devo vedere un blob con area maggiore di upperMeniscusMaxAreaReject2 per dare scarto. */
	int upperMeniscusRleThreshold;			/**< Soglia di sensibilità rle per analisi aggiuntiva nella prima regione sopra menisco */
} TI_RecipeAdvanced;

/*!
	Machine variable struct
	@brief Structure to retrieve and set machine parameters
*/
typedef struct {
	int	cameraOrientation;		/**< 0 = vial is vertical, 1 = vial is horizontal */
	int	encoderSwapPhase;	
	float encoderToPixelFactor;	
	int	flipImage;				/**< 0 = none, 1 = horizontal, 2 = vertical, 3 = both */
	int	headOneOffset;
	int	headsNumber;			/**< Number of heads */
	int	IOOutDataValidDelay;	/**< Delay, in us, after result output is set */
	int	IOOutDataValidWidth;	/**< Impulse width, in us, of data valid output */
	int	triggerMode;			/**< 0 = software, 1 = digital input, 2 = encoder, 3 = encoder single photo */
	int	triggerSlope;			/**< 0 = falling, 1 = rising */
	int cycleTime;				/**< Cycle time of device in ms */
} TI_MachineParameters;

/*!
	Enable features struct for Particles
	@brief Structure to retrieve and set enable features
*/
typedef struct {
	int	analysisEnable;				/**< 0 = disable analysis, 1 = enable analysis */
	int	overlayInformation;			/**< 0 = disable, 1 = enable */
	int	printBlobs;					/**< 0 = disable, 1 = enable */
	int	printBubbles;				/**< 0 = disable, 1 = enable */
	int	saveDifferenceImages;		/**< enable saving of the image of differences */
	int	saveTrajectory;				/**< enable saving trajectories information in the binary variable */
	int	sendDecimatedLive;			/**< 0 = disable, 1 = enable */
	int	sendResultImage;			/**< 0 = no send, 1 = send image from system RAM, 2 = send image from FPGA RAM */
	int	sendTrajectories;			/**< 0 = disable, 1 = enable */	
	int	meniscusAnalysis;			/**< Enable/disable analysis under the meniscus */
	int	meniscusAnalysisPlot;
	int	pgaEnable;
	int	ROIDynamic;					/**< Enable/disable ROI dynamic search inside rectangular analysis ROI */
	int	ROIDynamicOverlay;			/**< Enable/disable ROI plot on results images */
	int	ROIBottomExpansion; 		/**< Enable/disable ROI expansion until ROI_bottom */
	int	trackingKalmanPrediction;	/**< Enable/disable Kalman filter in tracking procedure */
	int fillingLevelEnable;			/**< Enable/disable filling analysis */
	int maskingBlobs;				/**< Enable/disable blob masking */
	int maskingBlobsPrint;			/**< Enable/disable blob masking print */
	int printTrajectories;			/**< 0 = show valid and recurrent trajectories, 1 = show all trajectories (show also trajectory under threshold) */
	int bottomAnalysis;
	int trackingEnable;
	int speedOptimization;
	int bottomSearchEnable;
	int upperMeniscusRoiEnable;
	int recurrentBlobsAnalysis;			/**< Flag che abilita questo filtraggio. */
	int upperMeniscusRoiEnable2;		/**< Abilita/disabilita regione di analisi aggiuntiva sopra il menisco */
	int upperMeniscusRleAnalysis;		/**< Flag che abilita analisi aggiuntiva con soglia rle diversa nella prima regione sopra il menisco */
	int ROIDynamicMeniscusSearchType;	/**< Direzione di ricerca della regione del menisco. Un valore di 0 indica una ricerca dall’alto verso il basso,
										come nelle precedenti versioni, mentre un valore di 1 indica una ricerca dal basso verso l’alto */
	int ROIDynamicBottomSearchCurve;		/**< Tipo di curva con la quale interpolare il profilo del fondo: 0 curva generica, 1 curva parabolica usata nelle versioni precedenti */
} TI_FeaturesEnable;

/*!
	@fn long TI_KnappSingleCameraStart(int idCamera)
	@brief Start single camera acquisition in KNAPP mode. 

	@param idCamera Start KNAPP mode on the camera with specified ID.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_KnappSingleCameraStart(int idCamera);

/*!
	@fn long TI_KnappSingleCameraStop(int idCamera)
	@brief Stop single camera acquisition in KNAPP mode. 

	@param idCamera Stop KNAPP mode on the camera with specified ID.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_KnappSingleCameraStop(int idCamera);

/*!
	@fn long TI_KnappSingleCameraReset(int idCamera)
	@brief Reset statistisc of KNAPP mode.

	@param idCamera Reset KNAPP mode statistics on the camera with specified ID.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_KnappSingleCameraReset(int idCamera);

/*!
	@fn long TI_KnappSingleCameraGetResults(int idCamera, TI_KnappResults *knappResult)
	@brief Get the Knapp result data from the camera specified with the ID.

	@param idCamera ID of the camera from which retrieve Knapp data.
	@param knappResult [out] Pointer to knapp result structure that will be filled with camera data. 
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE TI_KnappResults
*/
TATTILE_INTERFACE_API long TI_KnappSingleCameraGetResults(int idCamera, TI_KnappResults *knappResults);

/*!
	@fn long TI_AssistantResetError(int idCamera)
	@brief Reset error on selected camera.

	@param idCamera ID of the camera on which you want to reset error.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_AssistantResetError(int idCamera);

/*!
	@fn long TI_AxisSet(int idCamera, TI_AxisBuffer *axisBuffer)
	@brief Set the vial axis table parameters on the specified camera.

	@param idCamera ID of the camera on which you want to set the parameters.
	@param axisBuffer Pointer to a structure containing the parameters.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE TI_AxisBuffer
*/
TATTILE_INTERFACE_API long TI_AxisSet(int idCamera, TI_AxisBuffer *axisBuffer);

/*!
	@fn long TI_AxisGet(int idCamera, TI_AxisBuffer *axisBuffer)
	@brief Get the vial axis table parameters obtained by learning procedure on the specified camera.

	@param idCamera ID of the camera from which you want to get the parameters.
	@param axisBuffer [out] Pointer to a structure which will be filled with the parameters.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE TI_AxisBuffer
*/
TATTILE_INTERFACE_API long TI_AxisGet(int idCamera, TI_AxisBuffer *axisBuffer);

/*!
	@fn long TI_FormatSave(int idCamera, int formatNumber, char *description)
	@brief Store the parameters modifications to the specified format. A description can
		be supplied to identify the specific format.

	@param idCamera ID of the camera on which starts parameters save.
	@param formatNumber Number of the format that must be saved. There is up to 8 available format (0-7).
	@param description Description of the format that must be saved.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_FormatSave(int idCamera, int formatNumber, char *description);

/*!
	@fn long TI_FormatLoad(int idCamera, int formatNumber, char *description)
	@brief Load the parameters into memory from the format specfied as input argument.

	@param idCamera ID of the camera on which load saved parameters.
	@param formatNumber Number of the format that must be loaded. There is up to 8 available format (0-7).
	@param description Description of the format that has been loaded.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_FormatLoad(int idCamera, int formatNumber, char *description);

/*!
	@fn long TI_FormatReadLoaded(int idCamera, int *formatNumber, char *description)
	@brief Load the loaded format.

	@param idCamera ID of the camera on which load saved parameters.
	@param formatNumber [out] Number of the format that has been loaded. There is up to 8 available format (0-7).
	@param description [out] Description of the format that has been loaded.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_FormatReadLoaded(int idCamera, int *formatNumber, char *description);