/*!
	@file Cosmetic.h
	@brief Particles APIs.
*/

#pragma once

#define TABLE_COSMETIC_HEADS				"Heads"
#define TABLE_COSMETIC_VOLATILE				"Volatile"
#define TABLE_COSMETIC_LEARNING				"Learning"
#define TABLE_COSMETIC_SYSTEM				"System"
#define	TABLE_COSMETIC_STOP_ON_CONDITION	"StopOnCondition"

//TODO: unificare gli stati con il Particellare!!!!!

/*! 
	Camera modes: to be used with TI_AssistantPrepare and TI_AssistantSet for learning procedures.
*/
typedef enum {
	COSMETIC_MODE_INITIALIZATION = 0,				/**< Initialization mode */
	COSMETIC_MODE_NORMAL_WORK = 100,				/**< Normal operation work */
	COSMETIC_MODE_LOAD_FORMAT = 900,				/**< Load format parameters */
	COSMETIC_MODE_STOP_ON_CONDITION = 1000,			/**< stop_on_condition status */
	COSMETIC_MODE_STOP_ON_CONDITION_START = 1001,
	COSMETIC_MODE_STOP_ON_CONDITION_STOP = 1002,
	COSMETIC_MODE_ANALYSIS_OFF_LINE = 1100,			/**< OFF-line analysis status */
	COSMETIC_MODE_LIVE = 2200,						/**< Live status */

	COSMETIC_MODE_LEARNING_BOTTLE = 4000,
	COSMETIC_MODE_LEARNING_BOTTLE_BUSY = 4001,
	COSMETIC_MODE_LEARNING_BOTTLE_TEST = 4002,
	
	COSMETIC_MODE_LEARNING_BOTTLE_FLIPOFF = 5000,
	COSMETIC_MODE_LEARNING_BOTTLE_FLIPOFF_BUSY = 5001,
	COSMETIC_MODE_LEARNING_BOTTLE_FLIPOFF_TEST = 5002,
	
	COSMETIC_MODE_TEST_IO = 10000,				/**< Switch to I/O test mode */
	COSMETIC_SAVE_CONFIGURATION = 11000,		/**< Save current configuration on file */
} TI_CosmeticMode;

typedef enum {
	COSMETIC_COMMAND_REINIT_HEAD = 1000,
	COSMETIC_COMMAND_LIVE = 3000,
	COSMETIC_COMMAND_LOAD_MODEL = 4000,
	COSMETIC_COMMAND_SAVE_SW_CONFIG = 5000,
	COSMETIC_COMMAND_SIM_TRIGGER = 6000,
	COSMETIC_COMMAND_IO = 7000,
	COSMETIC_COMMAND_LEARNING_BOTTLE = 8000,
	COSMETIC_COMMAND_LEARNING_BOTTLE_FLIPOFF = 9000,
	COSMETIC_COMMAND_LEARNING_VIAL = 10000,
	COSMETIC_COMMAND_WHITE_BALANCE = 11000,
	COSMETIC_COMMAND_ANALYSIS_OFFLINE = 14000,
};

typedef enum {
	ROI_DEFAULT_AREA_COSMETIC,		/**< First ROI area enumeration */
	ROI_AREA_FLIPOFF,				/**< Flipoff ROI area enumeration */
	ROI_AREA_SLEEVE,				/**< Sleeve ROI area enumeration */
	ROI_AREA_WELDING,				/**< Welding ROI area enumeration */
	ROI_AREA_BOTTLETOP,				/**< BottleTop ROI area enumeration */
	COSMETIC_ROI_AREA_NUMBER,		/**< Totoal ROI area of Cosmetic application */
} TI_ROIAreaCosmetic;

typedef enum {
	COSMETIC_STREAM_DISABLE = -1,		/**< Camera stream disable */
	COSMETIC_STREAM_LIVE_0 = 0,			/**< Live stream from head 0 */
	COSMETIC_STREAM_LIVE_1 = 1,			/**< Live stream from head 1 */
	COSMETIC_STREAM_LIVE_2 = 2,			/**< Live stream from head 2 */
	COSMETIC_STREAM_LIVE_3 = 3,			/**< Live stream from head 3 */
	COSMETIC_STREAM_LIVE_QUAD = 4,		/**< Live stream from all heads */
	COSMETIC_STREAM_RESULT_0 = 5,		/**< Result stream from head 0 */
	COSMETIC_STREAM_RESULT_1 = 6,		/**< Result stream from head 1 */
	COSMETIC_STREAM_RESULT_2 = 7,		/**< Result stream from head 2 */
	COSMETIC_STREAM_RESULT_3 = 8,		/**< Result stream from head 3 */
	COSMETIC_STREAM_RESULT_QUAD = 9,	/**< Result stream from all heads */
} TI_StreamModeCosmetic;

/*!
	Recipe simple variable struct for Cosmetic
	@brief Structure to retrieve and set simple recipe variable
*/
typedef struct {
	int ARGBinBlobLowValue; 					/**< Set the LowValue of BinBlob tool */
	int ARGBinBlobMinAreaGlobal;				/**< Set the MinAreaGlobal of BinBlob tool */
	int ARGSetTipLutEndIndex;					/**< Set the LUT end index of SetTip tool */ 
	int ARGSetTipLutStartIndex;					/**< Set the LUT start index of SetTip tool */
	int ARGCheckRingContrast;					/**< Set the threshold contrast value */
	int ARGCheckRingHeight;						/**< Set the threshold height value */
	int ARGCheckRingHUE_1;						/**< Set the hue value for ring 1 */
	int ARGCheckRingHUE_2;						/**< Set the hue value for ring 2 */
	int ARGCheckRingHUE_3;						/**< Set the hue value for ring 3 */ 
	int ARGTipControlEdgeSumThreshold;			/**< Set the "edge sum threshold" of TipControl tool */
	int ARGTipControlEdgeTollerance;			/**< Set the "edge tollerance" of TipControl tool */
	int THRGrayMin;								/**< Set min gray level below which rise an alarm of illuminator switched off */ 
	int THRBinBlobMaxBlobAreaMin;				/**< Set the limit to "max blob area" result of BinBlob tool */ 
	int THRBinBlobMaxBlobAreaMax;				/**< Set the limit to "max blob area" result of BinBlob tool */ 
	int THRBinBlobNumberOfBlobsMin; 			/**< Set the limit to "number of blobs" result of BinBlob tool (always 0) */ 
	int THRBinBlobNumberOfBlobsMax;				/**< Set the limit to "number of blobs" result of BinBlob tool */ 
	int THRBinBlobSumOfBlobsAreaMin; 			/**< Set the limit to "sum of blobs area" result of BinBlob tool (always 0) */ 
	int THRBinBlobSumOfBlobsAreaMax;			/**< Set the limit to "sum of blobs area" result of BinBlob tool (always 0) */ 
	int THRTipControlIntegralSumMin; 			/**< Set the limit to "integral sum" result of TipControl tool (always 0) */ 
	int THRTipControlIntegralSumMax;			/**< Set the limit to "integral sum" result of TipControl tool */ 
	int THRTipControlMaxAngularDeviationMin; 	/**< Set the limit to "max angular deviation" result of TipControl tool (always 0) */ 
	int THRTipControlMaxAngularDeviationMax;	/**< Set the limit to "max angular deviation" result of TipControl tool */ 
	int THRTipControlMaxDistanceFromModelMin; 	/**< Set the limit to "max distance from model" result of TipControl tool (always 0) */ 
	int THRTipControlMaxDistanceFromModelMax;	/**< Set the limit to "max distance from model" result of TipControl tool */ 
	int THRTipControlVialHeightMin;				/**< Set the limit to "vial height" result of TipControl tool */ 
	int THRTipControlVialHeightMax;				/**< Set the limit to "vial height" result of TipControl tool */ 
	int THRCheckRingNumberOfRingsMin;			/**< Set the limit to "number of rings" result */
	int THRCheckRingNumberOfRingsMax;			/**< Set the limit to "number of rings" result */
	int THRCheckRingColorRing1Min;				/**< Set the limit to "color ring 1" result */
	int THRCheckRingColorRing1Max;				/**< Set the limit to "color ring 1" result */
	int THRCheckRingColorRing2Min;				/**< Set the limit to "color ring 2" result */
	int THRCheckRingColorRing2Max;				/**< Set the limit to "color ring 2" result */
	int THRCheckRingColorRing3Min;				/**< Set the limit to "color ring 3" result */
	int THRCheckRingColorRing3Max;				/**< Set the limit to "color ring 3" result */
	int time;									/**< Computational time for the analysis process on selected head */
	int ARGBottleFlipOffBadBoxArea;				/**< Indica la dimensione minima di un agglomerato di boxes da segnalare come scarto nel controllo Grid (l’equivalente dell’area minima di un blob) */
	int ARGBottleFlipOffBlobMinArea;			/**< Indicano I valori di area (minimo e massimo), espressi in pixel,  entro cui un blob, se rilevato, darà origine ad uno scarto */
	int ARGBottleFlipOffBlobMaxArea;			/**< Indicano I valori di area (minimo e massimo), espressi in pixel,  entro cui un blob, se rilevato, darà origine ad uno scarto */
	int ARGBottleFlipOffGrayMin;				/**< Indicano I livelli di grigio minimo e Massimo utilizzati nella binarizzazione assoluta */
	int ARGBottleFlipOffGrayMax;				/**< Indicano I livelli di grigio minimo e Massimo utilizzati nella binarizzazione assoluta */
	int ARGBottleFlipOffGrayRelative;			/**< Indica il valore di soglia per la binarizzazione relativa */
	int ARGBottleFlipOffGridSensibility;		/**< Indica il valore di sensibilità del controllo grid */
	int ARGBottleSleeveBadBoxArea;				/**< Indica la dimensione minima di un agglomerato di boxes da segnalare come scarto nel controllo Grid (l’equivalente dell’area minima di un blob) */
	int ARGBottleSleeveBlobMinArea;				/**< Indicano I valori di area (minimo e massimo), espressi in pixel,  entro cui un blob, se rilevato, darà origine ad uno scarto */
	int ARGBottleSleeveBlobMaxArea;				/**< Indicano I valori di area (minimo e massimo), espressi in pixel,  entro cui un blob, se rilevato, darà origine ad uno scarto */
	int ARGBottleSleeveGrayMin;					/**< Indicano I livelli di grigio minimo e Massimo utilizzati nella binarizzazione assoluta */
	int ARGBottleSleeveGrayMax;					/**< Indicano I livelli di grigio minimo e Massimo utilizzati nella binarizzazione assoluta */
	int ARGBottleSleeveGrayRelative;			/**< Indica il valore di soglia per la binarizzazione relativa */
	int ARGBottleSleeveGridSensibility;			/**< Indica il valore di sensibilità del controllo grid */
	int ARGBottleWeldingBlendAngle;				/**< Angolo massimo della tangente alla curva della crimpatura */
	int ARGBottleWeldingDisplayMode;			/**< Modalità di visualizzazione del risultato del tool (0=disabilitato, 1=curva reale, 2=curva reale + interpolata) */
	int ARGBottleWeldingGradientTransition;		/**< Livello di grigio che identifica la transizione che corrisponde alla crimpatura (-1 automatica, 0-254) */
	int ARGBottleWeldingSmoothFilterSize;		/**< Dimensione del kernel del filtro di smoot dei picchi */
	int ARGBottleWeldingSensibility;			/**< Indica la sensibilità del tool per la verifica della saldatura, aumentando questo valore il controllo tenderà a rilevare imperfezioni di 
												minor entità, abbassandolo verranno rilevate solo imperfezioni grossolane */
	int ARGBottleTopBlobMinArea;				/**< Indicano I valori di area (minimo e massimo), espressi in pixel,  entro cui un blob, se rilevato, darà origine ad uno scarto */
	int ARGBottleTopBlobMaxArea;				/**< Indicano I valori di area (minimo e massimo), espressi in pixel,  entro cui un blob, se rilevato, darà origine ad uno scarto */
	int ARGBottleTopColorMin;					/**< Indicano le soglie entro le quali deve trovarsi il colore dell’oggetto in analisi */
	int ARGBottleTopColorMax;					/**< Indicano le soglie entro le quali deve trovarsi il colore dell’oggetto in analisi */
	int ARGBottleTopGrayMin;					/**< Indicano I livelli di grigio minimo e Massimo utilizzati nella binarizzazione assoluta */
	int ARGBottleTopGrayMax;					/**< Indicano I livelli di grigio minimo e Massimo utilizzati nella binarizzazione assoluta */
	int ARGBottleTopGrayRelative;				/**< Indica il valore di soglia per la binarizzazione relativa */
	int ARGBottleTopRay;						/**< Indica la dimensione, espressa in pixels, della regione di interesse circolare contenente il flipoff */
	int THRBottleFlipOffWidthMin;				/**< Indicano I valori minimo e massimo, espressi in punti, della larghezza del flipoff */
	int THRBottleFlipOffWidthMax;				/**< Indicano I valori minimo e massimo, espressi in punti, della larghezza del flipoff */
	int THRBottleHeightMin;						/**< Indicano I valori minimo e Massimo, espressi in punti, della altezza della bottiglia */
	int THRBottleHeightMax;						/**< Indicano I valori minimo e Massimo, espressi in punti, della altezza della bottiglia */
	int THRBottleSleeveWidthMin;				/**< Indicano I valori minimo e Massimo, espressi in punti, della larghezza dello sleeve */
	int THRBottleSleeveWidthMax;				/**< Indicano I valori minimo e Massimo, espressi in punti, della larghezza dello sleeve */
	int THRBottleSleeveHeightMin;				/**< Indicano I valori minimo e Massimo, espressi in punti, della altezza dello sleeve */
	int THRBottleSleeveHeightMax;				/**< Indicano I valori minimo e Massimo, espressi in punti, della altezza dello sleeve */
	int THRBottleTopGrayMin;					/**< Indicano le soglie di accettazione del valore di saturazione del FlipOff */
	int THRBottleTopGrayMax;					/**< Indicano le soglie di accettazione del valore di saturazione del FlipOff */
	int THRBottleTopAreaMin;					/**< Indicano le soglie di accettazione per l’area rilevata del FlipOff */
	int THRBottleTopAreaMax;					/**< Indicano le soglie di accettazione per l’area rilevata del FlipOff */
} TI_RecipeSimpleCosmetic;

/*!
	Recipe advanced variable struct for Cosmetic
	@brief Structure to retrieve and set advanced recipe variable
*/
typedef struct {
	int ARGBinBlobHighValue;					/**< Set the "high value" of BinBlob tool */
	int ARGBinBlobInArea;						/**< Set the "in area" value of BinBlob tool */
	int ARGBinBlobIsBorderOFF;					/**< Set the "is border off" value of BinBlob tool */
	int ARGBinBlobMask; 						/**< Set the "mask" value of BinBlob tool */
    int ARGBinBlobOperationType;				/**< Set the "operation type" value of BinBlob tool */
	int ARGSetTipEdgeFillGap; 					/**< Set the "edge fill gap" value of SetTip tool */
	int ARGSetTipEdgeMinLen;					/**< Set the "edge min length" value of SetTip tool */
	int ARGSetTipEdgeSmooth;					/**< Set the "edge smooth" value of SetTip tool */
	int ARGSetTipFilterLen;		 				/**< Set the "filter length" value of SetTip tool */
	int ARGSetTipFilterSigma;					/**< Set the "filter sigma" value of SetTip tool */
	int ARGSetTipLutBitDepth;					/**< Set the "LUT bit depth" value of SetTip tool */
	int ARGSetTipLutEndValue;					/**< Set the "LUT end value" value of SetTip tool */
	int ARGSetTipLutStartValue;					/**< Set the "LUT start value" value of SetTip tool */
	int ARGSetTipOrientation;					/**< Set the "orientation" value of SetTip tool */
	int ARGTipControlFastEdge;					/**< Set the "fast edge" value of TipControl tool */
	int ARGTipControlIDraw;						/**< Set the "IDraw" value of TipControl tool */
	int ARGTipControlInArea;					/**< Set the "inArea" value of TipControl tool */
	int ARGTipControlMaskInTollerance;			/**< Set the "mask tollerance" value of TipControl tool */
	int ARGToolTagType;							/**< Set the "type" value of ToolTag tool */
	int ARGBottleFlipOffBinarizationBounding;	/**< Indica la modalità con cui viene effettuata la binarizzazione: 
												0 indica che i livelli di grigio interessati sono quelli inclusi tra i livelli di grigio minimo e massimo, 
												1 indica che i livello di grigi interessati sono quelli i cui valori sono inclusi tra 0 ed il valore di grigio minimo e tra il valore di grigio massimo e 255 */
	int ARGBottleFlipOffGridSquareSize;			/**< Indica la dimensione degli elementi in cui è campionato il controllo grid, minore è la dimensione del lato del quadrato maggiore è la sensibilità risultante 
												e maggiore è il tempo di analisi NOTA: modificabile solo in learning */
	int ARGBottleFlipOffGridType;				/**< Indica la tipologia di controllo utilizzato nel controllo Grid: 0 = Brightness 1 = Edge */
	int ARGBottleSleeveBinarizationBounding;	/**< Indica la modalità con cui viene effettuata la binarizzazione: 0 indica che i livelli di grigio interessati sono quelli 
												inclusi tra i livelli di grigio minimo e massimo, 1 indica che i livello di grigi interessati sono quelli i cui valori sono inclusi 
												tra 0 ed il valore di grigio minimo e quelli tra il valore di grigio massimo e 255 */
	int ARGBottleSleeveGridSquareSize;			/**< Indica la dimensione degli elementi in cui è campionato il controllo grid, minore è la dimensione del lato del quadrato maggiore è la sensibilità risultante 
												e maggiore è il tempo di analisi NOTA: modificabile solo in learning */
	int ARGBottleSleeveGridType;				/**< Indica la tipologia di controllo utilizzato nel controllo Grid: 0 = Brightness 1 = Edge */
	int ARGBottleTopBinarizationBounding;		/**< Indica la modalità con cui viene effettuata la binarizzazione: 
												0 indica che i livelli di grigio interessati sono quelli inclusi tra i livelli di grigio minimo e massimo,
												1 indica che i livello di grigi interessati sono quelli i cui valori sono inclusi tra 0 ed il valore di grigio minimo e tra il valore di grigio massimo e 255 */
	int ARGBottleTopFinderGrayMin;				/**< Indicano I livelli di grigio minimo e Massimo appartenenti all’oggetto in analisi perché il tool di ricerca lo rilevi */
	int ARGBottleTopFinderGrayMax;				/**< Indicano I livelli di grigio minimo e Massimo appartenenti all’oggetto in analisi perché il tool di ricerca lo rilevi */
	int backgroundType;							/**< Indica il tipo di fondo dell’immagine: 0 = Bianco, 1 = Nero */
	int orientation;							/**< Indica l’orientameno della testina: 0 = Verticale, 1 = Orizzontale */
	TI_TextData ARGSetTipModelPath;				/**< Set the file path containing the model to load in the SetTip tool */
	TI_BinaryData ARGBottleFlipOffLearning;		/**< Contiene il learning del controllo grid applicato al flipoff (non editabile) */
	TI_BinaryData ARGBottleSleeveLearning;		/**< Contiene il learning del controllo grid applicato al sleeve (non editrbile) */
} TI_RecipeAdvancedCosmetic;

/*!
	Enable features struct for Cosmetic
	@brief Structure to retrieve and set enable features
*/
typedef struct {
	int analysis;							/**< Set the working algorithm:
											0 = no algorithm
											1 = pin analysis
											2 = ring color analysis
											3 = Controllo laterale ghiera 
											4 = Controllo superiore ghiera 
											5 = Nuovo controllo punta */
	int displayMode;						/**< Set the desired overlay result informations (bit code format):
											Bit 0 = Tip Check result 
											Bit 1 = Tip Mask result
											Bit 2 = BinBlob result 
											Bit 3 = BinBlob Mask result */
	int ARGCheckRingDrawring;				/**< Set the DrawRing value */
	int enableHead;							/**< Enable a specific head */
	int ARGBottleFlipOffInspectionType;		/**< Indica la tipologia di controlli da effettuare sulla regione di interesse:
											0 = nessun controllo, 1 = binarizzazione assoluta, 2 = binarizzazione relativa, 4 = analisi colore, 8 = analisi dimensionale
											NOTA: il campo è codificato a bit in modo da poter combinare tutti i controlli (ad esempio se il campo è pari a 5 verranno 
											effettuati i controlli basati su binarizzazione assoluta e grid */
	int ARGBottleSleeveInspectionType;		/**< Indica la tipologia di controlli da effettuare sulla regione di interesse:
											0 = nessun controllo, 1 = binarizzazione assoluta, 2 = binarizzazione relativa, 4 = grid
											NOTA: il campo è codificato a bit in modo da poter combinare tutti i controlli (ad esempio se il campo è pari a 5 verranno effettuati i controlli basati 
											su binarizzazione assoluta e grid */
	int ARGBottleWeldingInspectionType;		/**< Indica la tipologia di controlli da effettuare sulla regione di interesse: 0 = nessun controllo, 1 = tool controllo crimpatura */
	int ARGBottleTopDetect;					/**< Indica se il finder del tappo funziona in modalità automatica (autodetect del tappo abilitato) o manuale: 0 = Manuale 1 = Automatico */
	int ARGBottleTopInspectionType;			/**< Indica la tipologia di controlli da effettuare sulla regione di interesse:
											0 = nessun controllo, 1 = binarizzazione assoluta, 2 = binarizzazione relativa
											NOTA: il campo è codificato a bit in modo da poter combinare tutti i controlli 
											(ad esempio se il campo è pari a 3 verranno effettuati i controlli basati su binarizzazione assoluta e relativa) */
	int ARGColorConversionChannel;			/**< Indica la modalità di conversione da RGB a BW:
											0 = brightness, 1 = Red, 2 = Green, 3 = Blue, 4 = Hue, 
											5 = HLS interleaved, 6 = Hue, 7 = Luminance, 8 = Saturation */
	int ARGBottleFlipOffPresent;			/**< Indica che il flipoff è presente (1) o assente (0) */
	int debugPrint;							/**< Abilita le stampe delle informazioni di debug sul banco immagine: 0 = disabilitato, 1 = abilitato*/
	int logImages;							/**< Abilita il salvataggio di tutte le immagini acquisite (originali ed elaborate) di tutte le testine su SD: 0 = disabilitato, 1 = abilitato
											NOTA: per ogni testina ci vogliono circa 500 millisecondi per effettuare questa operazione per cui è necessario abilitarla 
											unicamente quando la macchina stà andando a velocità bassissime. */
} TI_FeaturesEnableCosmetic;

/*!
	Machine parameters struct for Cosmetic
	@brief Structure to retrieve and set machine parameters
*/
typedef struct {
	int triggerMode;		/* 0 = Freerun, 1 = Trigger Mode */
	int speedMode;			/* 0 = Disable turbo, 1 = Enable Turbo */
	int headsNumber;			/* Number of heads on cosmetic star, for soho 36 */
} TI_MachineParametersCosmetic;

/*!
	Bottle learning parameters
	@brief Structure for strobo learning procedure
*/
typedef struct {
	int size;								/**< Size in bytes of structure */

	int ARGBottleHeight;					/**< Indica l’altezza della bottiglie espressa in pixel */
	int RESBottleFlipOffDefects;			/**< Indica la presenza di difetti superficiali, se diverso da zero, sul flipoff */
	int RESBottleHeightDefects;				/**< Indica, se diverso da zero, che la bottiglia è fuori range di altezza */
	int RESBottleSleeveDefects;				/**< Indica la presenza di difetti superficiali, se diverso da zero, sullo sleeve */
	int RESBottleWeldingDefects;			/**< Indica la presenza di difetti, se diverso da zero, nella crimpatura */
} TI_CosmeticLearningBottle;

/*!
	Bottle Flip Off learning parameters
	@brief Structure for normalization learning procedure
*/
typedef struct {
	int size;							/**< Size in bytes of structure */
} TI_CosmeticLearningBottleFlipOff;

/*!
	@fn long TI_StreamModeSet(int idCamera, int resultMode)
	@brief Set the stream mode to enable on the camera specified with the ID.

	@param idCamera ID of the camera which stream mode must be set.
	@param resultMode The desired result mode stream to set on device (TI_ResultModeCosmetic).
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE TI_StreamModeCosmetic
*/
TATTILE_INTERFACE_API long TI_StreamModeSet(int idCamera, int resultMode);

/*!
	@fn long TI_StreamModeGet(int idCamera, int *resultMode)
	@brief Get the stream mode currently set on the camera specified with the ID.

	@param idCamera ID of the camera which stream mode must be retrieved.
	@param resultMode The current stream mode set on device (TI_ResultModeCosmetic).
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE TI_StreamModeCosmetic
*/
TATTILE_INTERFACE_API long TI_StreamModeGet(int idCamera, int *resultMode);

/*!
	@fn long TI_CheckModelLoaded(int idCamera, int *modelLoaded)
	@brief Check if the model was correctly loaded.

	@param idCamera ID of the camera which model must be checked.
	@param modelLoaded If the model was correctly loaded it contains 1, otherwise 0.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_CheckModelLoaded(int idCamera, int *modelLoaded);

/*!
	@fn long TI_CosmeticLoadModel(int idCamera)
	@brief Force a model reload.

	@param idCamera ID of the camera which model must be reloaded.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_CosmeticLoadModel(int idCamera);

/*!
	@fn long TI_CosmeticLiveOverlay(int idCamera, int roiEnable, int infoEnable)
	@brief Enable or disable overlay on live image.

	@param idCamera ID of the camera which overlay must be set.
	@param roiEnable If set to 1 enables ROI display on the live image. 0 to disable ROI display.
	@param infoEnable If set to 1 enables informations display on live image. 0 to disable informations display.
	@return 0 if ok, else interface error code
	@see ITF_ERROR_CODE
*/
TATTILE_INTERFACE_API long TI_CosmeticLiveOverlay(int idCamera, int roiEnable, int infoEnable);