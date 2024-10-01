using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Globalization;
using System.Diagnostics;
using SPAMI.Util.Logger;
using SPAMI.Util;
using System.Drawing.Imaging;
using TattileCameras;
using DisplayManager;
using ExactaEasyCore;
using ExactaEasyEng;
using System.Security;

namespace TattileCameras {
    //public static class TattileAAA
    //{
    #region TattileTagFilterStructs

    enum ITF_ERROR_CODE {
        ITF_OK = 0,
        ITF_ERROR = 1,
        ITF_ERROR_TIMEOUT = 2,
        ITF_ERROR_DIMENSION = 3,
        ITF_ERROR_RACK = 4,
        ITF_ERROR_MEMORY = 5,
        ITF_ERROR_VALUE = 6,
        ITF_ERROR_FEWDATA = 7,
        ITF_ERROR_MUCHDATA = 8,
        ITF_ERROR_RACK_CODE = 9,
        ITF_ERROR_READ_EXIT = 10,
        ITF_ERROR_READ_ABORT = 11,
        ITF_ERROR_READ_BUFFER = 12,
        ITF_ERROR_CHECKSUM = 13,
        ITF_ERROR_CMD_DIFFERENT = 14,
        ITF_ERROR_WRITE_ABORT = 15,
        ITF_ERROR_CLEAN_READ_BUFFER = 16,
        ITF_ERROR_CH_HANDLE = 17,
        ITF_ERROR_NULL_POINTER = 18,
        ITF_ERROR_WRONG_STRUCT_SIZE = 19,
        ITF_ERROR_CODE_NOT_FOUND = 20,
        ITF_ERROR_CH_CLOSE = 21,
        ITF_TRACE = 22,
        ITF_ERROR_CMD_UNKNOWN = 2008
    }

    enum TAGFILTER_ERROR_CODE {
        /** No errors                                                   */
        TAGFILTER_OK = 0,
        /** Generic error                                                               */
        TAGFILTER_ERROR = 1,
        /** Read/write timeout error                            */
        TAGFILTER_ERROR_TIMEOUT = 2,
        /** The buffer dimension is too small    */
        TAGFILTER_ERROR_DIMENSION = 3,
        /** Unknown rack                                                                */
        TAGFILTER_ERROR_RACK = 4,
        /** Not enough memory                                           */
        TAGFILTER_ERROR_MEMORY = 5,
        /** Wrong parameter passed to the function              */
        TAGFILTER_ERROR_VALUE = 6,
        /** Less data riceived regarding to the data declared in the
         *  header */
        TAGFILTER_ERROR_FEWDATA = 7,
        /** More data ricieved regarding to the data declared in the header      */
        TAGFILTER_ERROR_MUCHDATA = 8,
        /** Rack with the same code                                             */
        TAGFILTER_ERROR_RACK_CODE = 9,
        /** Exit return during a reading                                   */
        TAGFILTER_ERROR_READ_EXIT = 10,
        /** Abort return during a reading                               */
        TAGFILTER_ERROR_READ_ABORT = 11,
        /** Exceed the riceive buffer dimension                 */
        TAGFILTER_ERROR_READ_BUFFER = 12,
        /** Wrong checksum received                                   */
        TAGFILTER_ERROR_CHECKSUM = 13,
        /** Different cmd from the request and the reply recieved    */
        TAGFILTER_ERROR_CMD_DIFFERENT = 14,
        /** Abort return during a writing                                                                        */
        TAGFILTER_ERROR_WRITE_ABORT = 15,
        /** CleanBuffer function has found data in read buffer       */
        TAGFILTER_ERROR_CLEAN_READ_BUFFER = 16,
        /** Corrupted comminication channel             */
        TAGFILTER_ERROR_CH_HANDLE = 17,
        /** NULL pointer passed to a function           */
        TAGFILTER_ERROR_NULL_POINTER = 18,
        /** Wrong struct size                           */
        TAGFILTER_ERROR_WRONG_STRUCT_SIZE = 19,
        /** Error code not found                        */
        TAGFILTER_ERROR_CODE_NOT_FOUND = 19,
        /** Channel is closed                           */
        TAGFILTER_ERROR_CH_CLOSE = 20,
        /** Channel status unknown (maybe connection in progress)   */
        TAGFILTER_ERROR_CH_UNKNOWN_STATUS = 21,
        /** Error while sending data to device                      */
        TAGFILTER_WRITE_ERROR = 22,
        /** Error during read incoming data                         */
        TAGFILTER_READ_ERROR = 25,
        /** Resource busy */
        TAGFILTER_ERROR_DEVICE_BUSY = 26,
        /** No incoming data on socket or on TagFilter connection. */
        TAGFILTER_READ_NO_DATA = 51,
        /** Filter lost at least one frame while reading the image */
        TAGFILTER_FRAME_LOST = 52,
        /** Buffer is a BW histogram, not an image (TAG1 only) */
        TAGFILTER_TYPE_HISTOGRAM = 53,
        /** Buffer is a RED histogram, not an image (TAG1 only) */
        TAGFILTER_TYPE_HISTOGRAM_RED = 54,
        /** Buffer is a GREEN histogram, not an image (TAG1 only) */
        TAGFILTER_TYPE_HISTOGRAM_GREEN = 55,
        /** Buffer is a BLUE histogram, not an image (TAG1 only) */
        TAGFILTER_TYPE_HISTOGRAM_BLUE = 56,
        /** Unknown command  */
        TAGFILTER_ERROR_CMD_UNKNOWN = 2008,
        TAGFILTER_ERROR_FILTER_NOT_INSTALLED = 9998,
        /** DLL not initialized, check TagFilter.sys version and dll
         version for possible mismatch. */
        TAGFILTER_ERROR_NOT_INITIALIZED,
        TAGFILTER_ERROR_LAST
    }

    enum ImageProtocol {
        IMAGE_PROTOCOL_DIRECT_X = 0,    // This is the old communication format 
        IMAGE_PROTOCOL_REGULAR,         // Not used in Antares but only in TAG devices
        IMAGE_PROTOCOL_GIGE,            // GigeVision communication protocol
        IMAGE_PROTOCOL_TOJECT           // Tattile Backwall communication protocol
    }

    struct TAGLostImageInfos {
        uint imgTimeStamp;
        uint imgCounter;
        uint eventType;
    }

    struct THeaderObjectFixed {
        ushort object_code;
        ushort object_sub_code;
        uint object_number;
        ushort creation_agent_status;
        ushort creation_agent_id;
        uint creation_agent_number;
        long creation_timestamp;
        uint header_size;
        uint payload_size;
        uint unit;
        uint semaphore_write;
        uint semaphore_read;
        uint command_object_address;
        uint contraints_object_address;
        uint description_objectaddress;
        uint destination_object_address;
        uint free;
    }

    struct TLeaderImage {
        ushort Status;
        ushort BlockID;
        uint PacketFormatID;
        ushort Reserved;
        ushort PayloadType;
        uint TimeStampH;
        uint TimeStampL;
        uint PixelType;
        uint SizeX;
        uint SizeY;
        uint OffsetX;
        uint OffsetY;
        ushort PaddingX;
        ushort PaddingY;
    }

    enum ImageBppCode : uint {
        Image_8Bit_BW = 0x1080001,
        Image_16Bit_RGB565 = 0x80100001,
        Image_24Bit_RGB888 = 0x2180014
    }
    #endregion

    #region TattileInterfaceStructs
    //public enum CameraStatus
    //{
    //    TATTILE_RUNNING = 100,
    //    TATTILE_LIVE = 2200,
    //    TATTILE_SET_STOP_ON_CONDITION = 1000,
    //    TATTILE_GOING_TO_STOP_ON_CONDITION = 1001,
    //    TATTILE_STOP_ON_CONDITION = 1002,
    //    TATTILE_START_ANALYSIS = 1100,
    //    TATTILE_CAMERA_NOT_EXIST = -1,
    //    TATTILE_CAMERA_ERROR = -1000
    //    //  0 inizializzazione
    //    // 1000  stop on condition req
    //    // 1002 camera in stop

    //}

    public struct TI_CameraInfo {
        public int serialNumber;
        public int type;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public char[] description;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public char[] model;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public char[] vendor;
        public int widthImage;
        public int heightImage;
        public int shutterMin;
        public int shutterMax;
        public int bitDepth;
        public int headNumber;
        public int gainMin;
        public int gainMax;
        public UInt32 ipAddress;
        public UInt32 pcIfAddress;
    };

    unsafe struct TI_BinaryData {
        public int data_size;
        public IntPtr data_used;
        public IntPtr data;

        internal TI_BinaryData(int size1, int size2) {
            data_size = size1 * size2 * Marshal.SizeOf(typeof(char));
            data = Marshal.AllocHGlobal(size1 * size2 * Marshal.SizeOf(typeof(char)));
            data_used = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
        }

        internal void FromString(string s) {
            //SecurityElement se = SecurityElement.FromString(s);
            //s = se.Text;
            s = Encoding.ASCII.GetString(Convert.FromBase64String(s));
            byte* c = (byte*)data;
            int* ps1 = (int*)data_used;
            data_size = s.Length;
            *ps1 = s.Length;

            for (int i = 0; i < s.Length; i++) {
                c[i] = (byte)s[i];
            }

            c[s.Length] = (byte)'\0';
        }

        public override string ToString() {
            string s = "";
            if (data_size > 0) {
                int* ps3 = (int*)data_used;
                int size = (*ps3 < 32 ? *ps3 : 32);
                byte[] byteString = new byte[size];

                Marshal.Copy(data, byteString, 0, size);
                //s = ASCIIEncoding.ASCII.GetString(byteString);
                s = Convert.ToBase64String(byteString);
            }
            //s = Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
            return s;
        }
    };

    //pier: struttura fittizia equivalente a TI_BinaryData per dati testuali non soggetti a codifica base 64
    unsafe struct TI_TextData {
        public int data_size;
        public IntPtr data_used;
        public IntPtr data;

        internal TI_TextData(int size1, int size2) {
            data_size = size1 * size2 * Marshal.SizeOf(typeof(char));
            data = Marshal.AllocHGlobal(size1 * size2 * Marshal.SizeOf(typeof(char)));
            data_used = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
        }

        internal void FromString(string s) {
            byte* c = (byte*)data;
            int* ps1 = (int*)data_used;
            data_size = s.Length;
            *ps1 = s.Length;

            for (int i = 0; i < s.Length; i++) {
                c[i] = (byte)s[i];
            }

            c[s.Length] = (byte)'\0';
        }

        public override string ToString() {
            string s = "";
            if (data_size > 0) {
                int* ps3 = (int*)data_used;
                int size = (*ps3 < 32 ? *ps3 : 32);
                byte[] byteString = new byte[size];

                Marshal.Copy(data, byteString, 0, size);
                s = ASCIIEncoding.ASCII.GetString(byteString);
                //s = Convert.ToBase64String(byteString);
            }
            //s = Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
            return s;
        }
    };

    struct TI_AcquisitionParameters {
        public int numberOfFrames;
        public int cameraShutter;
        public int cameraGain;
        public int strobeEnable;
        public int strobeTime;
        public int acquisitionDelay;	/**< Delay time (in us) between trigger rise and acquisition start */
        public int gainBlue;			/**< Blue channel gain */
        public int gainRed;			/**< Red channel gain */
        public int gainGreen;			/**< Green channel gain */
        public int strobeOut;			/**< Current strobe output signal (0-3) */
        public int triggerInput;		/**< Current trigger input signal (0-7) */
        public int aoiEnable;
        public int aoiStartCol;
        public int aoiStartRow;
        public int aoiResX;
        public int aoiResY;
        public int illuminationSetup;   // 0 bright field; 1 dark field


        public TI_AcquisitionParameters(IParameterCollection parameters) {

            this.numberOfFrames = parameters.Contains("numberOfFrames") ? Convert.ToInt32(parameters["numberOfFrames"].GetValue()) : 18;
            this.cameraShutter = parameters.Contains("cameraShutter") ? Convert.ToInt32(parameters["cameraShutter"].GetValue()) : 30;
            this.cameraGain = parameters.Contains("cameraGain") ? Convert.ToInt32(parameters["cameraGain"].GetValue()) : 0;
            this.strobeEnable = parameters.Contains("strobeEnable") ? Convert.ToInt32(parameters["strobeEnable"].GetValue()) : 1;
            this.strobeTime = parameters.Contains("strobeEnable") ? Convert.ToInt32(parameters["strobeTime"].GetValue()) : 30;
            this.acquisitionDelay = parameters.Contains("acquisitionDelay") ? Convert.ToInt32(parameters["acquisitionDelay"].GetValue()) : 0;
            this.gainBlue = parameters.Contains("gainBlue") ? Convert.ToInt32(parameters["gainBlue"].GetValue()) : 0;
            this.gainRed = parameters.Contains("gainRed") ? Convert.ToInt32(parameters["gainRed"].GetValue()) : 0;
            this.gainGreen = parameters.Contains("gainGreen") ? Convert.ToInt32(parameters["gainGreen"].GetValue()) : 0;
            this.strobeOut = parameters.Contains("strobeOut") ? Convert.ToInt32(parameters["strobeOut"].GetValue()) : 0;
            this.triggerInput = parameters.Contains("triggerInput") ? Convert.ToInt32(parameters["triggerInput"].GetValue()) : 0;
            this.aoiEnable = parameters.Contains("aoiEnable") ? Convert.ToInt32(parameters["aoiEnable"].GetValue()) : 1;
            this.aoiStartCol = parameters.Contains("aoiStartCol") ? Convert.ToInt32(parameters["aoiStartCol"].GetValue()) : 576;
            this.aoiStartRow = parameters.Contains("aoiStartRow") ? Convert.ToInt32(parameters["aoiStartRow"].GetValue()) : 0;
            this.aoiResX = parameters.Contains("aoiResX") ? Convert.ToInt32(parameters["aoiResX"].GetValue()) : 896;
            this.aoiResY = parameters.Contains("aoiResY") ? Convert.ToInt32(parameters["aoiResY"].GetValue()) : 2048;
            this.illuminationSetup = parameters.Contains("illuminationSetup") ? Convert.ToInt32(parameters["illuminationSetup"].GetValue()) : 0;
        }

        internal AcquisitionParameterCollection ToParamCollection(CultureInfo culture) {
            AcquisitionParameterCollection list = new AcquisitionParameterCollection();
            list.Add("numberOfFrames", numberOfFrames.ToString(culture));
            list.Add("cameraShutter", cameraShutter.ToString(culture));
            list.Add("cameraGain", cameraGain.ToString(culture));
            list.Add("strobeEnable", strobeEnable.ToString(culture));
            list.Add("strobeTime", strobeTime.ToString(culture));
            list.Add("acquisitionDelay", acquisitionDelay.ToString(culture));
            list.Add("gainBlue", gainBlue.ToString(culture));
            list.Add("gainRed", gainRed.ToString(culture));
            list.Add("gainGreen", gainGreen.ToString(culture));
            list.Add("strobeOut", strobeOut.ToString(culture));
            list.Add("triggerInput", triggerInput.ToString(culture));
            list.Add("aoiEnable", aoiEnable.ToString(culture));
            list.Add("aoiStartCol", aoiStartCol.ToString(culture));
            list.Add("aoiStartRow", aoiStartRow.ToString(culture));
            list.Add("aoiResX", aoiResX.ToString(culture));
            list.Add("aoiResY", aoiResY.ToString(culture));
            list.Add("illuminationSetup", illuminationSetup.ToString(culture));
            return list;
        }
    };

    struct TI_FeaturesEnableParticles {
        public int analysisEnable;
        public int overlayInformation;
        public int printBlobs;
        public int printBubbles;
        public int saveDifferenceImages;
        public int saveTrajectory;
        public int sendDecimatedLive;
        public int sendResultImage;
        public int sendTrajectories;
        public int meniscusAnalysis;
        public int meniscusAnalysisPlot;
        public int pgaEnable;
        public int ROIDynamic;
        public int ROIDynamicOverlay;
        public int ROIBottomExpansion;
        public int trackingKalmanPrediction;
        public int fillingLevelEnable;
        public int maskingBlobs;				/**< Enable/disable blob masking */
        public int maskingBlobsPrint;			/**< Enable/disable blob masking print */
        public int trajectoryPrint;
        public int bottomAnalysis;
        public int trackingEnable;
        public int speedOptimization;
        public int bottomSearchEnable;
        public int upperMeniscusRoiEnable;
        public int recurrentBlobsAnalysis;
        public int upperMeniscusRoiEnable2;
        public int upperMeniscusRleAnalysis;
        public int ROIDynamicMeniscusSearchType;
        public int ROIDynamicBottomSearchCurve;
        public int blobsDeleteStripeEnable;

        public TI_FeaturesEnableParticles(IParameterCollection parameters) {

            this.analysisEnable = parameters.Contains("analysisEnable") ? Convert.ToInt32(parameters["analysisEnable"].Value) : 1;
            this.overlayInformation = parameters.Contains("overlayInformation") ? Convert.ToInt32(parameters["overlayInformation"].Value) : 1;
            this.printBlobs = parameters.Contains("printBlobs") ? Convert.ToInt32(parameters["printBlobs"].Value) : 0;
            this.printBubbles = parameters.Contains("printBubbles") ? Convert.ToInt32(parameters["printBubbles"].Value) : 0;
            this.saveDifferenceImages = parameters.Contains("saveDifferenceImages") ? Convert.ToInt32(parameters["saveDifferenceImages"].Value) : 0;
            this.saveTrajectory = parameters.Contains("saveTrajectory") ? Convert.ToInt32(parameters["saveTrajectory"].Value) : 0;
            this.sendDecimatedLive = parameters.Contains("sendDecimatedLive") ? Convert.ToInt32(parameters["sendDecimatedLive"].Value) : 0;
            this.sendResultImage = parameters.Contains("sendResultImage") ? Convert.ToInt32(parameters["sendResultImage"].Value) : 1;
            this.sendTrajectories = parameters.Contains("sendTrajectories") ? Convert.ToInt32(parameters["sendTrajectories"].Value) : 0;
            this.meniscusAnalysis = parameters.Contains("meniscusAnalysis") ? Convert.ToInt32(parameters["meniscusAnalysis"].Value) : 0;
            this.meniscusAnalysisPlot = parameters.Contains("meniscusAnalysisPlot") ? Convert.ToInt32(parameters["meniscusAnalysisPlot"].Value) : 0;
            this.pgaEnable = parameters.Contains("meniscusAnalysisPlot") ? Convert.ToInt32(parameters["pgaEnable"].Value) : 1;
            this.ROIDynamic = parameters.Contains("ROIDynamic") ? Convert.ToInt32(parameters["ROIDynamic"].Value) : 1;
            this.ROIDynamicOverlay = parameters.Contains("ROIDynamicOverlay") ? Convert.ToInt32(parameters["ROIDynamicOverlay"].Value) : 0;
            this.ROIBottomExpansion = parameters.Contains("ROIBottomExpansion") ? Convert.ToInt32(parameters["ROIBottomExpansion"].Value) : 0;
            this.trackingKalmanPrediction = parameters.Contains("trackingKalmanPrediction") ? Convert.ToInt32(parameters["trackingKalmanPrediction"].Value) : 0;
            this.fillingLevelEnable = parameters.Contains("fillingLevelEnable") ? Convert.ToInt32(parameters["fillingLevelEnable"].Value) : 1;
            this.maskingBlobs = parameters.Contains("maskingBlobs") ? Convert.ToInt32(parameters["maskingBlobs"].Value) : 1;
            this.maskingBlobsPrint = parameters.Contains("maskingBlobsPrint") ? Convert.ToInt32(parameters["maskingBlobsPrint"].Value) : 0;
            this.trajectoryPrint = parameters.Contains("trajectoryPrint") ? Convert.ToInt32(parameters["trajectoryPrint"].Value) : 0;
            this.bottomAnalysis = parameters.Contains("bottomAnalysis") ? Convert.ToInt32(parameters["bottomAnalysis"].Value) : 0;
            this.trackingEnable = parameters.Contains("trackingEnable") ? Convert.ToInt32(parameters["trackingEnable"].Value) : 1;
            this.speedOptimization = parameters.Contains("speedOptimization") ? Convert.ToInt32(parameters["speedOptimization"].Value) : 0;
            this.bottomSearchEnable = parameters.Contains("bottomSearchEnable") ? Convert.ToInt32(parameters["bottomSearchEnable"].Value) : 1;
            this.upperMeniscusRoiEnable = parameters.Contains("upperMeniscusRoiEnable") ? Convert.ToInt32(parameters["upperMeniscusRoiEnable"].Value) : 0;
            this.recurrentBlobsAnalysis = parameters.Contains("recurrentBlobsAnalysis") ? Convert.ToInt32(parameters["recurrentBlobsAnalysis"].Value) : 1;
            this.upperMeniscusRoiEnable2 = parameters.Contains("upperMeniscusRoiEnable2") ? Convert.ToInt32(parameters["upperMeniscusRoiEnable2"].Value) : 0;
            this.upperMeniscusRleAnalysis = parameters.Contains("upperMeniscusRleAnalysis") ? Convert.ToInt32(parameters["upperMeniscusRleAnalysis"].Value) : 0;
            this.ROIDynamicMeniscusSearchType = parameters.Contains("ROIDynamicMeniscusSearchType") ? Convert.ToInt32(parameters["ROIDynamicMeniscusSearchType"].Value) : 0;
            this.ROIDynamicBottomSearchCurve = parameters.Contains("ROIDynamicBottomSearchCurve") ? Convert.ToInt32(parameters["ROIDynamicBottomSearchCurve"].Value) : 1;
            this.blobsDeleteStripeEnable = parameters.Contains("blobsDeleteStripeEnable") ? Convert.ToInt32(parameters["blobsDeleteStripeEnable"].Value) : 0;
        }

        internal FeaturesEnableParameterCollection ToParamCollection(CultureInfo culture) {
            FeaturesEnableParameterCollection list = new FeaturesEnableParameterCollection();
            list.Add("analysisEnable", analysisEnable.ToString(culture));
            list.Add("overlayInformation", overlayInformation.ToString(culture));
            list.Add("printBlobs", printBlobs.ToString(culture));
            list.Add("printBubbles", printBubbles.ToString(culture));
            list.Add("saveDifferenceImages", saveDifferenceImages.ToString(culture));
            list.Add("saveTrajectory", saveTrajectory.ToString(culture));
            list.Add("sendDecimatedLive", sendDecimatedLive.ToString(culture));
            list.Add("sendResultImage", sendResultImage.ToString(culture));
            list.Add("sendTrajectories", sendTrajectories.ToString(culture));
            list.Add("meniscusAnalysis", meniscusAnalysis.ToString(culture));
            list.Add("meniscusAnalysisPlot", meniscusAnalysisPlot.ToString(culture));
            list.Add("pgaEnable", pgaEnable.ToString(culture));
            list.Add("ROIDynamic", ROIDynamic.ToString(culture));
            list.Add("ROIDynamicOverlay", ROIDynamicOverlay.ToString(culture));
            list.Add("ROIBottomExpansion", ROIBottomExpansion.ToString(culture));
            list.Add("trackingKalmanPrediction", trackingKalmanPrediction.ToString(culture));
            list.Add("fillingLevelEnable", fillingLevelEnable.ToString(culture));
            list.Add("maskingBlobs", maskingBlobs.ToString(culture));
            list.Add("maskingBlobsPrint", maskingBlobsPrint.ToString(culture));
            list.Add("trajectoryPrint", trajectoryPrint.ToString(culture));
            list.Add("bottomAnalysis", bottomAnalysis.ToString(culture));
            list.Add("trackingEnable", trackingEnable.ToString(culture));
            list.Add("speedOptimization", speedOptimization.ToString(culture));
            list.Add("bottomSearchEnable", bottomSearchEnable.ToString(culture));
            list.Add("upperMeniscusRoiEnable", upperMeniscusRoiEnable.ToString(culture));
            list.Add("recurrentBlobsAnalysis", recurrentBlobsAnalysis.ToString(culture));
            list.Add("upperMeniscusRoiEnable2", upperMeniscusRoiEnable2.ToString(culture));
            list.Add("upperMeniscusRleAnalysis", upperMeniscusRleAnalysis.ToString(culture));
            list.Add("ROIDynamicMeniscusSearchType", ROIDynamicMeniscusSearchType.ToString(culture));
            list.Add("ROIDynamicBottomSearchCurve", ROIDynamicBottomSearchCurve.ToString(culture));
            list.Add("blobsDeleteStripeEnable", blobsDeleteStripeEnable.ToString(culture));
            return list;
        }

    };

    struct TI_FeaturesEnableCosmetic {
        public int analysis;
        public int displayMode;
        public int ARGCheckRingDrawring;
        public int enableHead;
        public int ARGBottleFlipOffInspectionType;
        public int ARGBottleSleeveInspectionType;
        public int ARGBottleWeldingInspectionType;
        public int ARGBottleTopDetect;
        public int ARGBottleTopInspectionType;
        public int ARGColorConversionChannel;
        public int ARGBottleFlipOffPresent;
        public int debugPrint;
        public int logImages;

        public TI_FeaturesEnableCosmetic(IParameterCollection parameters) {

            this.analysis = parameters.Contains("analysis") ? Convert.ToInt32(parameters["analysis"].Value) : 1;
            this.displayMode = parameters.Contains("displayMode") ? Convert.ToInt32(parameters["displayMode"].Value) : 0;
            this.ARGCheckRingDrawring = parameters.Contains("ARGCheckRingDrawring") ? Convert.ToInt32(parameters["ARGCheckRingDrawring"].Value) : 0;
            this.enableHead = parameters.Contains("enableHead") ? Convert.ToInt32(parameters["enableHead"].Value) : 1;
            this.ARGBottleFlipOffInspectionType = parameters.Contains("ARGBottleFlipOffInspectionType") ? Convert.ToInt32(parameters["ARGBottleFlipOffInspectionType"].Value) : 0;
            this.ARGBottleSleeveInspectionType = parameters.Contains("ARGBottleSleeveInspectionType") ? Convert.ToInt32(parameters["ARGBottleSleeveInspectionType"].Value) : 0;
            this.ARGBottleWeldingInspectionType = parameters.Contains("ARGBottleWeldingInspectionType") ? Convert.ToInt32(parameters["ARGBottleWeldingInspectionType"].Value) : 0;
            this.ARGBottleTopDetect = parameters.Contains("ARGBottleTopDetect") ? Convert.ToInt32(parameters["ARGBottleTopDetect"].Value) : 0;
            this.ARGBottleTopInspectionType = parameters.Contains("ARGBottleTopInspectionType") ? Convert.ToInt32(parameters["ARGBottleTopInspectionType"].Value) : 0;
            this.ARGColorConversionChannel = parameters.Contains("ARGColorConversionChannel") ? Convert.ToInt32(parameters["ARGColorConversionChannel"].Value) : 0;
            this.ARGBottleFlipOffPresent = parameters.Contains("ARGBottleFlipOffPresent") ? Convert.ToInt32(parameters["ARGBottleFlipOffPresent"].Value) : 0;
            this.debugPrint = parameters.Contains("debugPrint") ? Convert.ToInt32(parameters["debugPrint"].Value) : 1;
            this.logImages = parameters.Contains("logImages") ? Convert.ToInt32(parameters["logImages"].Value) : 0;
        }

        internal FeaturesEnableParameterCollection ToParamCollection(CultureInfo culture) {
            FeaturesEnableParameterCollection list = new FeaturesEnableParameterCollection();
            list.Add("analysis", analysis.ToString(culture));
            list.Add("displayMode", displayMode.ToString(culture));
            list.Add("ARGCheckRingDrawring", ARGCheckRingDrawring.ToString(culture));
            list.Add("enableHead", enableHead.ToString(culture));
            list.Add("ARGBottleFlipOffInspectionType", ARGBottleFlipOffInspectionType.ToString(culture));
            list.Add("ARGBottleSleeveInspectionType", ARGBottleSleeveInspectionType.ToString(culture));
            list.Add("ARGBottleWeldingInspectionType", ARGBottleWeldingInspectionType.ToString(culture));
            list.Add("ARGBottleTopDetect", ARGBottleTopDetect.ToString(culture));
            list.Add("ARGBottleTopInspectionType", ARGBottleTopInspectionType.ToString(culture));
            list.Add("ARGColorConversionChannel", ARGColorConversionChannel.ToString(culture));
            list.Add("ARGBottleFlipOffPresent", ARGBottleFlipOffPresent.ToString(culture));
            list.Add("debugPrint", debugPrint.ToString(culture));
            list.Add("logImages", logImages.ToString(culture));
            return list;
        }
    };

    struct TI_RecipeSimpleParticles {

        public int rleLowThreshold;
        public int ROIDynamicHighThreshold;
        public int ROIDynamicLowThreshold;
        public int ROIDynamicBottonReductcion;
        public int ROIDynamicMeniscusReductcion;
        public int ROIDynamicBottomExpansionYStart;
        public int meniscusPUMThreshold;
        public int blobMaxAreaReject;
        public int blobsMinArea;
        public int blobMaxAreaRejectLowThreshold;
        public int fillingLevelMin;
        public int fillingLevelMax;
        public int minDensityReference;
        public int blobMaxAreaRejectConsecutiveFramesNumber;
        public int bottomAnalysisThreshold;
        public int blobsMaxNumber;
        public int ROIDynamicMeniscusSearchYStart;
        public int ROIDynamicMeniscusSearchYEnd;
        public int ROIDynamicBottomSearchYStart;
        public int ROIDynamicBottomSearchThreshold;
        public int bottomAnalysisAreaThreshold;
        public int blobsDeleteStripe1YStart;
        public int blobsDeleteStripe1YEnd;
        public int blobsDeleteStripe2YStart;
        public int blobsDeleteStripe2YEnd;

        public TI_RecipeSimpleParticles(IParameterCollection parameters) {

            this.rleLowThreshold = parameters.Contains("rleLowThreshold") ? Convert.ToInt32(parameters["rleLowThreshold"].GetValue()) : 10;
            this.ROIDynamicHighThreshold = parameters.Contains("ROIDynamicHighThreshold") ? Convert.ToInt32(parameters["ROIDynamicHighThreshold"].GetValue()) : 243;
            this.ROIDynamicLowThreshold = parameters.Contains("ROIDynamicLowThreshold") ? Convert.ToInt32(parameters["ROIDynamicLowThreshold"].GetValue()) : 80;
            this.ROIDynamicBottonReductcion = parameters.Contains("ROIDynamicBottonReductcion") ? Convert.ToInt32(parameters["ROIDynamicBottonReductcion"].GetValue()) : 10;
            this.ROIDynamicMeniscusReductcion = parameters.Contains("ROIDynamicMeniscusReductcion") ? Convert.ToInt32(parameters["ROIDynamicMeniscusReductcion"].GetValue()) : 50;
            this.ROIDynamicBottomExpansionYStart = parameters.Contains("ROIDynamicBottomExpansionYStart") ? Convert.ToInt32(parameters["ROIDynamicBottomExpansionYStart"].GetValue()) : 10;
            this.meniscusPUMThreshold = parameters.Contains("meniscusPUMThreshold") ? Convert.ToInt32(parameters["meniscusPUMThreshold"].GetValue()) : 110;
            this.blobMaxAreaReject = parameters.Contains("blobMaxAreaReject") ? Convert.ToInt32(parameters["blobMaxAreaReject"].GetValue()) : 80;
            this.blobsMinArea = parameters.Contains("blobsMinArea") ? Convert.ToInt32(parameters["blobsMinArea"].GetValue()) : 20;
            this.blobMaxAreaRejectLowThreshold = parameters.Contains("blobMaxAreaRejectLowThreshold") ? Convert.ToInt32(parameters["blobMaxAreaRejectLowThreshold"].GetValue()) : 60;
            this.fillingLevelMin = parameters.Contains("fillingLevelMin") ? Convert.ToInt32(parameters["fillingLevelMin"].GetValue()) : 200;
            this.fillingLevelMax = parameters.Contains("fillingLevelMax") ? Convert.ToInt32(parameters["fillingLevelMax"].GetValue()) : 400;
            this.minDensityReference = parameters.Contains("minDensityReference") ? Convert.ToInt32(parameters["minDensityReference"].GetValue()) : 100;
            this.blobMaxAreaRejectConsecutiveFramesNumber = parameters.Contains("blobMaxAreaRejectConsecutiveFramesNumber") ? Convert.ToInt32(parameters["blobMaxAreaRejectConsecutiveFramesNumber"].GetValue()) : 2;
            this.bottomAnalysisThreshold = parameters.Contains("bottomAnalysisThreshold") ? Convert.ToInt32(parameters["bottomAnalysisThreshold"].GetValue()) : 40;
            this.blobsMaxNumber = parameters.Contains("blobsMaxNumber") ? Convert.ToInt32(parameters["blobsMaxNumber"].GetValue()) : 100;
            this.ROIDynamicMeniscusSearchYStart = parameters.Contains("ROIDynamicMeniscusSearchYStart") ? Convert.ToInt32(parameters["ROIDynamicMeniscusSearchYStart"].GetValue()) : 0;
            this.ROIDynamicMeniscusSearchYEnd = parameters.Contains("ROIDynamicMeniscusSearchYEnd") ? Convert.ToInt32(parameters["ROIDynamicMeniscusSearchYEnd"].GetValue()) : 0;
            this.ROIDynamicBottomSearchYStart = parameters.Contains("ROIDynamicBottomSearchYStart") ? Convert.ToInt32(parameters["ROIDynamicBottomSearchYStart"].GetValue()) : 0;
            this.ROIDynamicBottomSearchThreshold = parameters.Contains("ROIDynamicBottomSearchThreshold") ? Convert.ToInt32(parameters["ROIDynamicBottomSearchThreshold"].GetValue()) : 0;
            this.bottomAnalysisAreaThreshold = parameters.Contains("bottomAnalysisAreaThreshold") ? Convert.ToInt32(parameters["bottomAnalysisAreaThreshold"].GetValue()) : 0;
            this.blobsDeleteStripe1YStart = parameters.Contains("blobsDeleteStripe1YStart") ? Convert.ToInt32(parameters["blobsDeleteStripe1YStart"].GetValue()) : 0;
            this.blobsDeleteStripe1YEnd = parameters.Contains("blobsDeleteStripe1YEnd") ? Convert.ToInt32(parameters["blobsDeleteStripe1YEnd"].GetValue()) : 0;
            this.blobsDeleteStripe2YStart = parameters.Contains("blobsDeleteStripe2YStart") ? Convert.ToInt32(parameters["blobsDeleteStripe2YStart"].GetValue()) : 0;
            this.blobsDeleteStripe2YEnd = parameters.Contains("blobsDeleteStripe2YEnd") ? Convert.ToInt32(parameters["blobsDeleteStripe2YEnd"].GetValue()) : 0;
        }

        internal RecipeSimpleParameterCollection ToParamCollection(CultureInfo culture) {
            RecipeSimpleParameterCollection list = new RecipeSimpleParameterCollection();
            list.Add("rleLowThreshold", rleLowThreshold.ToString(culture));
            list.Add("ROIDynamicHighThreshold", ROIDynamicHighThreshold.ToString(culture));
            list.Add("ROIDynamicLowThreshold", ROIDynamicLowThreshold.ToString(culture));
            list.Add("ROIDynamicBottonReductcion", ROIDynamicBottonReductcion.ToString(culture));
            list.Add("ROIDynamicMeniscusReductcion", ROIDynamicMeniscusReductcion.ToString(culture));
            list.Add("ROIDynamicBottomExpansionYStart", ROIDynamicBottomExpansionYStart.ToString(culture));
            list.Add("meniscusPUMThreshold", meniscusPUMThreshold.ToString(culture));
            list.Add("blobMaxAreaReject", blobMaxAreaReject.ToString(culture));
            list.Add("blobsMinArea", blobsMinArea.ToString(culture));
            list.Add("blobMaxAreaRejectLowThreshold", blobMaxAreaRejectLowThreshold.ToString(culture));
            list.Add("fillingLevelMin", fillingLevelMin.ToString(culture));
            list.Add("fillingLevelMax", fillingLevelMax.ToString(culture));
            list.Add("minDensityReference", minDensityReference.ToString(culture));
            list.Add("blobMaxAreaRejectConsecutiveFramesNumber", blobMaxAreaRejectConsecutiveFramesNumber.ToString(culture));
            list.Add("bottomAnalysisThreshold", bottomAnalysisThreshold.ToString(culture));
            list.Add("blobsMaxNumber", blobsMaxNumber.ToString(culture));
            list.Add("ROIDynamicMeniscusSearchYStart", ROIDynamicMeniscusSearchYStart.ToString(culture));
            list.Add("ROIDynamicMeniscusSearchYEnd", ROIDynamicMeniscusSearchYEnd.ToString(culture));
            list.Add("ROIDynamicBottomSearchYStart", ROIDynamicBottomSearchYStart.ToString(culture));
            list.Add("ROIDynamicBottomSearchThreshold", ROIDynamicBottomSearchThreshold.ToString(culture));
            list.Add("bottomAnalysisAreaThreshold", bottomAnalysisAreaThreshold.ToString(culture));
            list.Add("blobsDeleteStripe1YStart", blobsDeleteStripe1YStart.ToString(culture));
            list.Add("blobsDeleteStripe1YEnd", blobsDeleteStripe1YEnd.ToString(culture));
            list.Add("blobsDeleteStripe2YStart", blobsDeleteStripe2YStart.ToString(culture));
            list.Add("blobsDeleteStripe2YEnd", blobsDeleteStripe2YEnd.ToString(culture));
            return list;
        }
    };

    struct TI_RecipeSimpleCosmetic {

        public int ARGBinBlobLowValue;
        public int ARGBinBlobMinAreaGlobal;
        public int ARGSetTipLutEndIndex;
        public int ARGSetTipLutStartIndex;
        public int ARGCheckRingContrast;
        public int ARGCheckRingHeight;
        public int ARGCheckRingHUE_1;
        public int ARGCheckRingHUE_2;
        public int ARGCheckRingHUE_3;
        public int ARGTipControlEdgeSumThreshold;
        public int ARGTipControlEdgeTollerance;
        public int THRGrayMin;
        public int THRBinBlobMaxBlobAreaMin;
        public int THRBinBlobMaxBlobAreaMax;
        public int THRBinBlobNumberOfBlobsMin;
        public int THRBinBlobNumberOfBlobsMax;
        public int THRBinBlobSumOfBlobsAreaMin;
        public int THRBinBlobSumOfBlobsAreaMax;
        public int THRTipControlIntegralSumMin;
        public int THRTipControlIntegralSumMax;
        public int THRTipControlMaxAngularDeviationMin;
        public int THRTipControlMaxAngularDeviationMax;
        public int THRTipControlMaxDistanceFromModelMin;
        public int THRTipControlMaxDistanceFromModelMax;
        public int THRTipControlVialHeightMin;
        public int THRTipControlVialHeightMax;
        public int THRCheckRingNumberOfRingsMin;
        public int THRCheckRingNumberOfRingsMax;
        public int THRCheckRingColorRing1Min;
        public int THRCheckRingColorRing1Max;
        public int THRCheckRingColorRing2Min;
        public int THRCheckRingColorRing2Max;
        public int THRCheckRingColorRing3Min;
        public int THRCheckRingColorRing3Max;
        public int time;
        public int ARGBottleFlipOffBadBoxArea;
        public int ARGBottleFlipOffBlobMinArea;
        public int ARGBottleFlipOffBlobMaxArea;
        public int ARGBottleFlipOffGrayMin;
        public int ARGBottleFlipOffGrayMax;
        public int ARGBottleFlipOffGrayRelative;
        public int ARGBottleFlipOffGridSensibility;
        public int ARGBottleSleeveBadBoxArea;
        public int ARGBottleSleeveBlobMinArea;
        public int ARGBottleSleeveBlobMaxArea;
        public int ARGBottleSleeveGrayMin;
        public int ARGBottleSleeveGrayMax;
        public int ARGBottleSleeveGrayRelative;
        public int ARGBottleSleeveGridSensibility;
        public int ARGBottleWeldingBlendAngle;
        public int ARGBottleWeldingDisplayMode;
        public int ARGBottleWeldingGradientTransition;
        public int ARGBottleWeldingSmoothFilterSize;
        public int ARGBottleWeldingSensibility;
        public int ARGBottleTopBlobMinArea;
        public int ARGBottleTopBlobMaxArea;
        public int ARGBottleTopColorMin;
        public int ARGBottleTopColorMax;
        public int ARGBottleTopGrayMin;
        public int ARGBottleTopGrayMax;
        public int ARGBottleTopGrayRelative;
        public int ARGBottleTopRay;
        public int THRBottleFlipOffWidthMin;
        public int THRBottleFlipOffWidthMax;
        public int THRBottleHeightMin;
        public int THRBottleHeightMax;
        public int THRBottleSleeveWidthMin;
        public int THRBottleSleeveWidthMax;
        public int THRBottleSleeveHeightMin;
        public int THRBottleSleeveHeightMax;
        public int THRBottleTopGrayMin;
        public int THRBottleTopGrayMax;
        public int THRBottleTopAreaMin;
        public int THRBottleTopAreaMax;


        public TI_RecipeSimpleCosmetic(IParameterCollection parameters) {

            this.ARGBinBlobLowValue = parameters.Contains("ARGBinBlobLowValue") ? Convert.ToInt32(parameters["ARGBinBlobLowValue"].GetValue()) : 80;
            this.ARGBinBlobMinAreaGlobal = parameters.Contains("ARGBinBlobMinAreaGlobal") ? Convert.ToInt32(parameters["ARGBinBlobMinAreaGlobal"].GetValue()) : 50;
            this.ARGSetTipLutEndIndex = parameters.Contains("ARGSetTipLutEndIndex") ? Convert.ToInt32(parameters["ARGSetTipLutEndIndex"].GetValue()) : 200;
            this.ARGSetTipLutStartIndex = parameters.Contains("ARGSetTipLutStartIndex") ? Convert.ToInt32(parameters["ARGSetTipLutStartIndex"].GetValue()) : 0;
            this.ARGCheckRingContrast = parameters.Contains("ARGCheckRingContrast") ? Convert.ToInt32(parameters["ARGCheckRingContrast"].GetValue()) : 50;
            this.ARGCheckRingHeight = parameters.Contains("ARGCheckRingHeight") ? Convert.ToInt32(parameters["ARGCheckRingHeight"].GetValue()) : 100;
            this.ARGCheckRingHUE_1 = parameters.Contains("ARGCheckRingHUE_1") ? Convert.ToInt32(parameters["ARGCheckRingHUE_1"].GetValue()) : 100;
            this.ARGCheckRingHUE_2 = parameters.Contains("ARGCheckRingHUE_2") ? Convert.ToInt32(parameters["ARGCheckRingHUE_2"].GetValue()) : 150;
            this.ARGCheckRingHUE_3 = parameters.Contains("ARGCheckRingHUE_3") ? Convert.ToInt32(parameters["ARGCheckRingHUE_3"].GetValue()) : 200;
            this.ARGTipControlEdgeSumThreshold = parameters.Contains("ARGTipControlEdgeSumThreshold") ? Convert.ToInt32(parameters["ARGTipControlEdgeSumThreshold"].GetValue()) : 10;
            this.ARGTipControlEdgeTollerance = parameters.Contains("ARGTipControlEdgeTollerance") ? Convert.ToInt32(parameters["ARGTipControlEdgeTollerance"].GetValue()) : 40;
            this.THRGrayMin = parameters.Contains("THRGrayMin") ? Convert.ToInt32(parameters["THRGrayMin"].GetValue()) : 60;
            this.THRBinBlobMaxBlobAreaMin = parameters.Contains("THRBinBlobMaxBlobAreaMin") ? Convert.ToInt32(parameters["THRBinBlobMaxBlobAreaMin"].GetValue()) : 0;
            this.THRBinBlobMaxBlobAreaMax = parameters.Contains("THRBinBlobMaxBlobAreaMax") ? Convert.ToInt32(parameters["THRBinBlobMaxBlobAreaMax"].GetValue()) : 200;
            this.THRBinBlobNumberOfBlobsMin = parameters.Contains("THRBinBlobNumberOfBlobsMin") ? Convert.ToInt32(parameters["THRBinBlobNumberOfBlobsMin"].GetValue()) : 0;
            this.THRBinBlobNumberOfBlobsMax = parameters.Contains("THRBinBlobNumberOfBlobsMax") ? Convert.ToInt32(parameters["THRBinBlobNumberOfBlobsMax"].GetValue()) : 100;
            this.THRBinBlobSumOfBlobsAreaMin = parameters.Contains("THRBinBlobSumOfBlobsAreaMin") ? Convert.ToInt32(parameters["THRBinBlobSumOfBlobsAreaMin"].GetValue()) : 0;
            this.THRBinBlobSumOfBlobsAreaMax = parameters.Contains("THRBinBlobSumOfBlobsAreaMax") ? Convert.ToInt32(parameters["THRBinBlobSumOfBlobsAreaMax"].GetValue()) : 0;
            this.THRTipControlIntegralSumMin = parameters.Contains("THRTipControlIntegralSumMin") ? Convert.ToInt32(parameters["THRTipControlIntegralSumMin"].GetValue()) : 0;
            this.THRTipControlIntegralSumMax = parameters.Contains("THRTipControlIntegralSumMax") ? Convert.ToInt32(parameters["THRTipControlIntegralSumMax"].GetValue()) : 200;
            this.THRTipControlMaxAngularDeviationMin = parameters.Contains("THRTipControlMaxAngularDeviationMin") ? Convert.ToInt32(parameters["THRTipControlMaxAngularDeviationMin"].GetValue()) : 0;
            this.THRTipControlMaxAngularDeviationMax = parameters.Contains("THRTipControlMaxAngularDeviationMax") ? Convert.ToInt32(parameters["THRTipControlMaxAngularDeviationMax"].GetValue()) : 30;
            this.THRTipControlMaxDistanceFromModelMin = parameters.Contains("THRTipControlMaxDistanceFromModelMin") ? Convert.ToInt32(parameters["THRTipControlMaxDistanceFromModelMin"].GetValue()) : 0;
            this.THRTipControlMaxDistanceFromModelMax = parameters.Contains("THRTipControlMaxDistanceFromModelMax") ? Convert.ToInt32(parameters["THRTipControlMaxDistanceFromModelMax"].GetValue()) : 15;
            this.THRTipControlVialHeightMin = parameters.Contains("THRTipControlVialHeightMin") ? Convert.ToInt32(parameters["THRTipControlVialHeightMin"].GetValue()) : 650;
            this.THRTipControlVialHeightMax = parameters.Contains("THRTipControlVialHeightMax") ? Convert.ToInt32(parameters["THRTipControlVialHeightMax"].GetValue()) : 750;
            this.THRCheckRingNumberOfRingsMin = parameters.Contains("THRCheckRingNumberOfRingsMin") ? Convert.ToInt32(parameters["THRCheckRingNumberOfRingsMin"].GetValue()) : 0;
            this.THRCheckRingNumberOfRingsMax = parameters.Contains("THRCheckRingNumberOfRingsMax") ? Convert.ToInt32(parameters["THRCheckRingNumberOfRingsMax"].GetValue()) : 0;
            this.THRCheckRingColorRing1Min = parameters.Contains("THRCheckRingColorRing1Min") ? Convert.ToInt32(parameters["THRCheckRingColorRing1Min"].GetValue()) : 0;
            this.THRCheckRingColorRing1Max = parameters.Contains("THRCheckRingColorRing1Max") ? Convert.ToInt32(parameters["THRCheckRingColorRing1Max"].GetValue()) : 0;
            this.THRCheckRingColorRing2Min = parameters.Contains("THRCheckRingColorRing2Min") ? Convert.ToInt32(parameters["THRCheckRingColorRing2Min"].GetValue()) : 0;
            this.THRCheckRingColorRing2Max = parameters.Contains("THRCheckRingColorRing2Max") ? Convert.ToInt32(parameters["THRCheckRingColorRing2Max"].GetValue()) : 0;
            this.THRCheckRingColorRing3Min = parameters.Contains("THRCheckRingColorRing3Min") ? Convert.ToInt32(parameters["THRCheckRingColorRing3Min"].GetValue()) : 0;
            this.THRCheckRingColorRing3Max = parameters.Contains("THRCheckRingColorRing3Max") ? Convert.ToInt32(parameters["THRCheckRingColorRing3Max"].GetValue()) : 0;
            this.time = parameters.Contains("time") ? Convert.ToInt32(parameters["time"].GetValue()) : 100;
            this.ARGBottleFlipOffBadBoxArea = parameters.Contains("ARGBottleFlipOffBadBoxArea") ? Convert.ToInt32(parameters["ARGBottleFlipOffBadBoxArea"].GetValue()) : 5;
            this.ARGBottleFlipOffBlobMinArea = parameters.Contains("ARGBottleFlipOffBlobMinArea") ? Convert.ToInt32(parameters["ARGBottleFlipOffBlobMinArea"].GetValue()) : 100;
            this.ARGBottleFlipOffBlobMaxArea = parameters.Contains("ARGBottleFlipOffBlobMaxArea") ? Convert.ToInt32(parameters["ARGBottleFlipOffBlobMaxArea"].GetValue()) : 10000;
            this.ARGBottleFlipOffGrayMin = parameters.Contains("ARGBottleFlipOffGrayMin") ? Convert.ToInt32(parameters["ARGBottleFlipOffGrayMin"].GetValue()) : 100;
            this.ARGBottleFlipOffGrayMax = parameters.Contains("ARGBottleFlipOffGrayMax") ? Convert.ToInt32(parameters["ARGBottleFlipOffGrayMax"].GetValue()) : 10000;
            this.ARGBottleFlipOffGrayRelative = parameters.Contains("ARGBottleFlipOffGrayRelative") ? Convert.ToInt32(parameters["ARGBottleFlipOffGrayRelative"].GetValue()) : -1;
            this.ARGBottleFlipOffGridSensibility = parameters.Contains("ARGBottleFlipOffGridSensibility") ? Convert.ToInt32(parameters["ARGBottleFlipOffGridSensibility"].GetValue()) : 2;
            this.ARGBottleSleeveBadBoxArea = parameters.Contains("ARGBottleSleeveBadBoxArea") ? Convert.ToInt32(parameters["ARGBottleSleeveBadBoxArea"].GetValue()) : 50;
            this.ARGBottleSleeveBlobMinArea = parameters.Contains("ARGBottleSleeveBlobMinArea") ? Convert.ToInt32(parameters["ARGBottleSleeveBlobMinArea"].GetValue()) : 100;
            this.ARGBottleSleeveBlobMaxArea = parameters.Contains("ARGBottleSleeveBlobMaxArea") ? Convert.ToInt32(parameters["ARGBottleSleeveBlobMaxArea"].GetValue()) : 50000;
            this.ARGBottleSleeveGrayMin = parameters.Contains("ARGBottleSleeveGrayMin") ? Convert.ToInt32(parameters["ARGBottleSleeveGrayMin"].GetValue()) : 0;
            this.ARGBottleSleeveGrayMax = parameters.Contains("ARGBottleSleeveGrayMax") ? Convert.ToInt32(parameters["ARGBottleSleeveGrayMax"].GetValue()) : 255;
            this.ARGBottleSleeveGrayRelative = parameters.Contains("ARGBottleSleeveGrayRelative") ? Convert.ToInt32(parameters["ARGBottleSleeveGrayRelative"].GetValue()) : 40;
            this.ARGBottleSleeveGridSensibility = parameters.Contains("ARGBottleSleeveGridSensibility") ? Convert.ToInt32(parameters["ARGBottleSleeveGridSensibility"].GetValue()) : 2;
            //18.05 GR nuove aggiunte per sistemazione crimpatura
            this.ARGBottleWeldingBlendAngle = parameters.Contains("ARGBottleWeldingBlendAngle") ? Convert.ToInt32(parameters["ARGBottleWeldingBlendAngle"].GetValue()) : 60;
            this.ARGBottleWeldingDisplayMode = parameters.Contains("ARGBottleWeldingDisplayMode") ? Convert.ToInt32(parameters["ARGBottleWeldingDisplayMode"].GetValue()) : 2;
            this.ARGBottleWeldingGradientTransition = parameters.Contains("ARGBottleWeldingGradientTransition") ? Convert.ToInt32(parameters["ARGBottleWeldingGradientTransition"].GetValue()) : 0;
            this.ARGBottleWeldingSmoothFilterSize = parameters.Contains("ARGBottleWeldingSmoothFilterSize") ? Convert.ToInt32(parameters["ARGBottleWeldingSmoothFilterSize"].GetValue()) : 2;
            //
            this.ARGBottleWeldingSensibility = parameters.Contains("ARGBottleWeldingSensibility") ? Convert.ToInt32(parameters["ARGBottleWeldingSensibility"].GetValue()) : 60;
            this.ARGBottleTopBlobMinArea = parameters.Contains("ARGBottleTopBlobMinArea") ? Convert.ToInt32(parameters["ARGBottleTopBlobMinArea"].GetValue()) : 100;
            this.ARGBottleTopBlobMaxArea = parameters.Contains("ARGBottleTopBlobMaxArea") ? Convert.ToInt32(parameters["ARGBottleTopBlobMaxArea"].GetValue()) : 1000;
            this.ARGBottleTopColorMin = parameters.Contains("ARGBottleTopColorMin") ? Convert.ToInt32(parameters["ARGBottleTopColorMin"].GetValue()) : 0;
            this.ARGBottleTopColorMax = parameters.Contains("ARGBottleTopColorMax") ? Convert.ToInt32(parameters["ARGBottleTopColorMax"].GetValue()) : 0;
            this.ARGBottleTopGrayMin = parameters.Contains("ARGBottleTopGrayMin") ? Convert.ToInt32(parameters["ARGBottleTopGrayMin"].GetValue()) : -1;
            this.ARGBottleTopGrayMax = parameters.Contains("ARGBottleTopGrayMax") ? Convert.ToInt32(parameters["ARGBottleTopGrayMax"].GetValue()) : -1;
            this.ARGBottleTopGrayRelative = parameters.Contains("ARGBottleTopGrayRelative") ? Convert.ToInt32(parameters["ARGBottleTopGrayRelative"].GetValue()) : -1;
            this.ARGBottleTopRay = parameters.Contains("ARGBottleTopRay") ? Convert.ToInt32(parameters["ARGBottleTopRay"].GetValue()) : 25;
            this.THRBottleFlipOffWidthMin = parameters.Contains("THRBottleFlipOffWidthMin") ? Convert.ToInt32(parameters["THRBottleFlipOffWidthMin"].GetValue()) : 226;
            this.THRBottleFlipOffWidthMax = parameters.Contains("THRBottleFlipOffWidthMax") ? Convert.ToInt32(parameters["THRBottleFlipOffWidthMax"].GetValue()) : 266;
            this.THRBottleHeightMin = parameters.Contains("THRBottleHeightMin") ? Convert.ToInt32(parameters["THRBottleHeightMin"].GetValue()) : 160;
            this.THRBottleHeightMax = parameters.Contains("THRBottleHeightMax") ? Convert.ToInt32(parameters["THRBottleHeightMax"].GetValue()) : 200;
            this.THRBottleSleeveWidthMin = parameters.Contains("THRBottleSleeveWidthMin") ? Convert.ToInt32(parameters["THRBottleSleeveWidthMin"].GetValue()) : 200;
            this.THRBottleSleeveWidthMax = parameters.Contains("THRBottleSleeveWidthMax") ? Convert.ToInt32(parameters["THRBottleSleeveWidthMax"].GetValue()) : 212;
            this.THRBottleSleeveHeightMin = parameters.Contains("THRBottleSleeveHeightMin") ? Convert.ToInt32(parameters["THRBottleSleeveHeightMin"].GetValue()) : 121;
            this.THRBottleSleeveHeightMax = parameters.Contains("THRBottleSleeveHeightMax") ? Convert.ToInt32(parameters["THRBottleSleeveHeightMax"].GetValue()) : 161;
            this.THRBottleTopGrayMin = parameters.Contains("THRBottleTopGrayMin") ? Convert.ToInt32(parameters["THRBottleTopGrayMin"].GetValue()) : 0;
            this.THRBottleTopGrayMax = parameters.Contains("THRBottleTopGrayMax") ? Convert.ToInt32(parameters["THRBottleTopGrayMax"].GetValue()) : 0;
            this.THRBottleTopAreaMin = parameters.Contains("THRBottleTopAreaMin") ? Convert.ToInt32(parameters["THRBottleTopAreaMin"].GetValue()) : 0;
            this.THRBottleTopAreaMax = parameters.Contains("THRBottleTopAreaMax") ? Convert.ToInt32(parameters["THRBottleTopAreaMax"].GetValue()) : 0;
        }

        internal RecipeSimpleParameterCollection ToParamCollection(CultureInfo culture) {
            RecipeSimpleParameterCollection list = new RecipeSimpleParameterCollection();
            list.Add("ARGBinBlobLowValue", ARGBinBlobLowValue.ToString(culture));
            list.Add("ARGBinBlobMinAreaGlobal", ARGBinBlobMinAreaGlobal.ToString(culture));
            list.Add("ARGSetTipLutEndIndex", ARGSetTipLutEndIndex.ToString(culture));
            list.Add("ARGSetTipLutStartIndex", ARGSetTipLutStartIndex.ToString(culture));
            list.Add("ARGCheckRingContrast", ARGCheckRingContrast.ToString(culture));
            list.Add("ARGCheckRingHeight", ARGCheckRingHeight.ToString(culture));
            list.Add("ARGCheckRingHUE_1", ARGCheckRingHUE_1.ToString(culture));
            list.Add("ARGCheckRingHUE_2", ARGCheckRingHUE_2.ToString(culture));
            list.Add("ARGCheckRingHUE_3", ARGCheckRingHUE_3.ToString(culture));
            list.Add("ARGTipControlEdgeSumThreshold", ARGTipControlEdgeSumThreshold.ToString(culture));
            list.Add("ARGTipControlEdgeTollerance", ARGTipControlEdgeTollerance.ToString(culture));
            list.Add("THRGrayMin", THRGrayMin.ToString(culture));
            list.Add("THRBinBlobMaxBlobAreaMin", THRBinBlobMaxBlobAreaMin.ToString(culture));
            list.Add("THRBinBlobMaxBlobAreaMax", THRBinBlobMaxBlobAreaMax.ToString(culture));
            list.Add("THRBinBlobNumberOfBlobsMin", THRBinBlobNumberOfBlobsMin.ToString(culture));
            list.Add("THRBinBlobNumberOfBlobsMax", THRBinBlobNumberOfBlobsMax.ToString(culture));
            list.Add("THRBinBlobSumOfBlobsAreaMin", THRBinBlobSumOfBlobsAreaMin.ToString(culture));
            list.Add("THRBinBlobSumOfBlobsAreaMax", THRBinBlobSumOfBlobsAreaMax.ToString(culture));
            list.Add("THRTipControlIntegralSumMin", THRTipControlIntegralSumMin.ToString(culture));
            list.Add("THRTipControlIntegralSumMax", THRTipControlIntegralSumMax.ToString(culture));
            list.Add("THRTipControlMaxAngularDeviationMin", THRTipControlMaxAngularDeviationMin.ToString(culture));
            list.Add("THRTipControlMaxAngularDeviationMax", THRTipControlMaxAngularDeviationMax.ToString(culture));
            list.Add("THRTipControlMaxDistanceFromModelMin", THRTipControlMaxDistanceFromModelMin.ToString(culture));
            list.Add("THRTipControlMaxDistanceFromModelMax", THRTipControlMaxDistanceFromModelMax.ToString(culture));
            list.Add("THRTipControlVialHeightMin", THRTipControlVialHeightMin.ToString(culture));
            list.Add("THRTipControlVialHeightMax", THRTipControlVialHeightMax.ToString(culture));
            list.Add("THRCheckRingNumberOfRingsMin", THRCheckRingNumberOfRingsMin.ToString(culture));
            list.Add("THRCheckRingNumberOfRingsMax", THRCheckRingNumberOfRingsMax.ToString(culture));
            list.Add("THRCheckRingColorRing1Min", THRCheckRingColorRing1Min.ToString(culture));
            list.Add("THRCheckRingColorRing1Max", THRCheckRingColorRing1Max.ToString(culture));
            list.Add("THRCheckRingColorRing2Min", THRCheckRingColorRing2Min.ToString(culture));
            list.Add("THRCheckRingColorRing2Max", THRCheckRingColorRing2Max.ToString(culture));
            list.Add("THRCheckRingColorRing3Min", THRCheckRingColorRing3Min.ToString(culture));
            list.Add("THRCheckRingColorRing3Max", THRCheckRingColorRing3Max.ToString(culture));
            list.Add("time", time.ToString(culture));
            list.Add("ARGBottleFlipOffBadBoxArea", ARGBottleFlipOffBadBoxArea.ToString(culture));
            list.Add("ARGBottleFlipOffBlobMinArea", ARGBottleFlipOffBlobMinArea.ToString(culture));
            list.Add("ARGBottleFlipOffBlobMaxArea", ARGBottleFlipOffBlobMaxArea.ToString(culture));
            list.Add("ARGBottleFlipOffGrayMin", ARGBottleFlipOffGrayMin.ToString(culture));
            list.Add("ARGBottleFlipOffGrayMax", ARGBottleFlipOffGrayMax.ToString(culture));
            list.Add("ARGBottleFlipOffGrayRelative", ARGBottleFlipOffGrayRelative.ToString(culture));
            list.Add("ARGBottleFlipOffGridSensibility", ARGBottleFlipOffGridSensibility.ToString(culture));
            list.Add("ARGBottleSleeveBadBoxArea", ARGBottleSleeveBadBoxArea.ToString(culture));
            list.Add("ARGBottleSleeveBlobMinArea", ARGBottleSleeveBlobMinArea.ToString(culture));
            list.Add("ARGBottleSleeveBlobMaxArea", ARGBottleSleeveBlobMaxArea.ToString(culture));
            list.Add("ARGBottleSleeveGrayMin", ARGBottleSleeveGrayMin.ToString(culture));
            list.Add("ARGBottleSleeveGrayMax", ARGBottleSleeveGrayMax.ToString(culture));
            list.Add("ARGBottleSleeveGrayRelative", ARGBottleSleeveGrayRelative.ToString(culture));
            list.Add("ARGBottleSleeveGridSensibility", ARGBottleSleeveGridSensibility.ToString(culture));
            list.Add("ARGBottleWeldingBlendAngle", ARGBottleWeldingBlendAngle.ToString(culture));
            list.Add("ARGBottleWeldingDisplayMode", ARGBottleWeldingDisplayMode.ToString(culture));
            list.Add("ARGBottleWeldingGradientTransition", ARGBottleWeldingGradientTransition.ToString(culture));
            list.Add("ARGBottleWeldingSmoothFilterSize", ARGBottleWeldingSmoothFilterSize.ToString(culture));
            list.Add("ARGBottleWeldingSensibility", ARGBottleWeldingSensibility.ToString(culture));
            list.Add("ARGBottleTopBlobMinArea", ARGBottleTopBlobMinArea.ToString(culture));
            list.Add("ARGBottleTopBlobMaxArea", ARGBottleTopBlobMaxArea.ToString(culture));
            list.Add("ARGBottleTopColorMin", ARGBottleTopColorMin.ToString(culture));
            list.Add("ARGBottleTopColorMax", ARGBottleTopColorMax.ToString(culture));
            list.Add("ARGBottleTopGrayMin", ARGBottleTopGrayMin.ToString(culture));
            list.Add("ARGBottleTopGrayMax", ARGBottleTopGrayMax.ToString(culture));
            list.Add("ARGBottleTopGrayRelative", ARGBottleTopGrayRelative.ToString(culture));
            list.Add("ARGBottleTopRay", ARGBottleTopRay.ToString(culture));
            list.Add("THRBottleFlipOffWidthMin", THRBottleFlipOffWidthMin.ToString(culture));
            list.Add("THRBottleFlipOffWidthMax", THRBottleFlipOffWidthMax.ToString(culture));
            list.Add("THRBottleHeightMin", THRBottleHeightMin.ToString(culture));
            list.Add("THRBottleHeightMax", THRBottleHeightMax.ToString(culture));
            list.Add("THRBottleSleeveWidthMin", THRBottleSleeveWidthMin.ToString(culture));
            list.Add("THRBottleSleeveWidthMax", THRBottleSleeveWidthMax.ToString(culture));
            list.Add("THRBottleSleeveHeightMin", THRBottleSleeveHeightMin.ToString(culture));
            list.Add("THRBottleSleeveHeightMax", THRBottleSleeveHeightMax.ToString(culture));
            list.Add("THRBottleTopGrayMin", THRBottleTopGrayMin.ToString(culture));
            list.Add("THRBottleTopGrayMax", THRBottleTopGrayMax.ToString(culture));
            list.Add("THRBottleTopAreaMin", THRBottleTopAreaMin.ToString(culture));
            list.Add("THRBottleTopAreaMax", THRBottleTopAreaMax.ToString(culture));
            return list;
        }
    };

    struct TI_RecipeAdvancedParticles {

        public int morphologicalFilter1;
        public int morphologicalFilter2;
        public int rleBufferSize;
        public int ROIDynamicMorphIter;
        public int meniscusAnalysisMinCount;
        public int meniscusAnalysisThreshold;
        public int meniscusPUMOffset;
        //public int blobsMaxNumber;
        public float bubbleMaxShapeFactor;
        public int bubbleOpticalDensityMinDiff;
        public int bubbleSizeMaximum;
        public int bubbleSizeMinimum;
        public float trackingMaxAcceleration;
        public float trackingMaxAreaRatio;
        public float trackingMaxHeightRatio;
        public float trackingMaxWidthRatio;
        public int trackingMaxSegmentAngle;
        public int trackingMaxTrajectoryAngle;
        public int trackingMinDistance;
        public int trackingNumberFramesBackward;
        public int trackingFrameBackwardReduction;
        public int trajectoriesBufferSize;
        public float trajectoryAllSegmentsMaxLength;
        public float trajectoryAllSegmentsMinLength;
        public int trajectoryRecurrentBackward;
        public int trajectoryRecurrentMaxAngle;
        public int trajectoryRecurrentMaxDistance;
        public int trajectoryRecurrentMinCount;
        public int trajectoryRecurrentMinLength;
        //public int maskingBlobsHighThreshold;
        //public int maskingBlobsLowThreshold;
        public int maskingBlobsSensibility;
        public int maskingBlobsThreshold;
        public int encoderCounterFirstImage;
        public int bottomAnalysisMinCount;
        public int trackingMaxDistance;
        public int trajectoryMaxAngle;
        public int trajectoryMinArea;
        public int trajectoryMinSegmentNumber;
        public int upperMeniscusRoiOffset;
        public int upperMeniscusRoiType;
        public int upperMeniscusRoiWidth;
        public int upperMeniscusRoiHeight;
        public int bottomAnalysisFilterSize;
        public int meniscusAnalysisFilterSize;
        public int upperMeniscusMaxAreaReject;
        public int upperMeniscusFramesNumber;
        public int recurrentMinNumber;
        public int recurrentMinBlobsArea;
        public int recurrentMaxBlobsArea;
        public int recurrentMaxBlobsDistance;
        public int recurrentMaxAreaRatioPerc;
        public int upperMeniscusRoiOffset2;
        public int upperMeniscusRoiWidth2;
        public int upperMeniscusRoiHeight2;
        public int upperMeniscusMaxAreaReject2;
        public int upperMeniscusFramesNumber2;
        public int upperMeniscusRleThreshold;


        public TI_RecipeAdvancedParticles(IParameterCollection parameters, CultureInfo culture) {

            this.morphologicalFilter1 = parameters.Contains("morphologicalFilter1") ? Convert.ToInt32(parameters["morphologicalFilter1"].GetValue()) : 0;
            this.morphologicalFilter2 = parameters.Contains("morphologicalFilter2") ? Convert.ToInt32(parameters["morphologicalFilter2"].GetValue()) : 0;
            this.rleBufferSize = parameters.Contains("rleBufferSize") ? Convert.ToInt32(parameters["rleBufferSize"].GetValue()) : 1000000;
            this.ROIDynamicMorphIter = parameters.Contains("ROIDynamicMorphIter") ? Convert.ToInt32(parameters["ROIDynamicMorphIter"].GetValue()) : 0;
            this.meniscusAnalysisMinCount = parameters.Contains("meniscusAnalysisMinCount") ? Convert.ToInt32(parameters["meniscusAnalysisMinCount"].GetValue()) : 5;
            this.meniscusAnalysisThreshold = parameters.Contains("meniscusAnalysisThreshold") ? Convert.ToInt32(parameters["meniscusAnalysisThreshold"].GetValue()) : 20;
            this.meniscusPUMOffset = parameters.Contains("meniscusPUMOffset") ? Convert.ToInt32(parameters["meniscusPUMOffset"].GetValue()) : 50;
            //this.blobsMaxNumber = parameters.Contains("blobsMaxNumber") ? Convert.ToInt32(parameters["blobsMaxNumber"].GetValue()) : 100;
            this.bubbleMaxShapeFactor = parameters.Contains("bubbleMaxShapeFactor") ? (float)Convert.ToDouble(parameters["bubbleMaxShapeFactor"].GetValue(), culture) : 1.5F;
            this.bubbleOpticalDensityMinDiff = parameters.Contains("bubbleOpticalDensityMinDiff") ? Convert.ToInt32(parameters["bubbleOpticalDensityMinDiff"].GetValue()) : 200;
            this.bubbleSizeMaximum = parameters.Contains("bubbleSizeMaximum") ? Convert.ToInt32(parameters["bubbleSizeMaximum"].GetValue()) : 50;
            this.bubbleSizeMinimum = parameters.Contains("bubbleSizeMinimum") ? Convert.ToInt32(parameters["bubbleSizeMinimum"].GetValue()) : 7;
            this.trackingMaxAcceleration = parameters.Contains("trackingMaxAcceleration") ? (float)Convert.ToDouble(parameters["trackingMaxAcceleration"].GetValue(), culture) : 3;
            this.trackingMaxAreaRatio = parameters.Contains("trackingMaxAreaRatio") ? (float)Convert.ToDouble(parameters["trackingMaxAreaRatio"].GetValue(), culture) : 3;
            this.trackingMaxHeightRatio = parameters.Contains("trackingMaxHeightRatio") ? (float)Convert.ToDouble(parameters["trackingMaxHeightRatio"].GetValue(), culture) : 3;
            this.trackingMaxWidthRatio = parameters.Contains("trackingMaxWidthRatio") ? (float)Convert.ToDouble(parameters["trackingMaxWidthRatio"].GetValue(), culture) : 3;
            this.trackingMaxSegmentAngle = parameters.Contains("trackingMaxSegmentAngle") ? Convert.ToInt32(parameters["trackingMaxSegmentAngle"].GetValue()) : 45;
            this.trackingMaxTrajectoryAngle = parameters.Contains("trackingMaxTrajectoryAngle") ? Convert.ToInt32(parameters["trackingMaxTrajectoryAngle"].GetValue()) : 30;
            this.trackingMinDistance = parameters.Contains("trackingMinDistance") ? Convert.ToInt32(parameters["trackingMinDistance"].GetValue()) : 5;
            this.trackingNumberFramesBackward = parameters.Contains("trackingNumberFramesBackward") ? Convert.ToInt32(parameters["trackingNumberFramesBackward"].GetValue()) : 2;
            this.trackingFrameBackwardReduction = parameters.Contains("trackingFrameBackwardReduction") ? Convert.ToInt32(parameters["trackingFrameBackwardReduction"].GetValue()) : 25;
            this.trajectoriesBufferSize = parameters.Contains("trajectoriesBufferSize") ? Convert.ToInt32(parameters["trajectoriesBufferSize"].GetValue()) : 1000000;
            this.trajectoryAllSegmentsMaxLength = parameters.Contains("trajectoryAllSegmentsMaxLength") ? (float)Convert.ToDouble(parameters["trajectoryAllSegmentsMaxLength"].GetValue(), culture) : 0;
            this.trajectoryAllSegmentsMinLength = parameters.Contains("trajectoryAllSegmentsMinLength") ? (float)Convert.ToDouble(parameters["trajectoryAllSegmentsMinLength"].GetValue(), culture) : 0;
            this.trajectoryRecurrentBackward = parameters.Contains("trajectoryRecurrentBackward") ? Convert.ToInt32(parameters["trajectoryRecurrentBackward"].GetValue()) : 10;
            this.trajectoryRecurrentMaxAngle = parameters.Contains("trajectoryRecurrentMaxAngle") ? Convert.ToInt32(parameters["trajectoryRecurrentMaxAngle"].GetValue()) : 15;
            this.trajectoryRecurrentMaxDistance = parameters.Contains("trajectoryRecurrentMaxDistance") ? Convert.ToInt32(parameters["trajectoryRecurrentMaxDistance"].GetValue()) : 20;
            this.trajectoryRecurrentMinCount = parameters.Contains("trajectoryRecurrentMinCount") ? Convert.ToInt32(parameters["trajectoryRecurrentMinCount"].GetValue()) : 2;
            this.trajectoryRecurrentMinLength = parameters.Contains("trajectoryRecurrentMinLength") ? Convert.ToInt32(parameters["trajectoryRecurrentMinLength"].GetValue()) : 3;
            //this.maskingBlobsHighThreshold = parameters.Contains("maskingBlobsHighThreshold") ? Convert.ToInt32(parameters["maskingBlobsHighThreshold"].GetValue()) : 10;
            //this.maskingBlobsLowThreshold = parameters.Contains("maskingBlobsLowThreshold") ? Convert.ToInt32(parameters["maskingBlobsLowThreshold"].GetValue()) : 5;
            this.maskingBlobsSensibility = parameters.Contains("maskingBlobsSensibility") ? Convert.ToInt32(parameters["maskingBlobsSensibility"].GetValue()) : 0;  //pier: cambiare default value
            this.maskingBlobsThreshold = parameters.Contains("maskingBlobsThreshold") ? Convert.ToInt32(parameters["maskingBlobsThreshold"].GetValue()) : 0;        //pier: cambiare default value
            this.encoderCounterFirstImage = parameters.Contains("encoderCounterFirstImage") ? Convert.ToInt32(parameters["encoderCounterFirstImage"].GetValue()) : 320;
            this.bottomAnalysisMinCount = parameters.Contains("bottomAnalysisMinCount") ? Convert.ToInt32(parameters["bottomAnalysisMinCount"].GetValue()) : 5;
            this.trackingMaxDistance = parameters.Contains("trackingMaxDistance") ? Convert.ToInt32(parameters["trackingMaxDistance"].GetValue()) : 100;
            this.trajectoryMaxAngle = parameters.Contains("trajectoryMaxAngle") ? Convert.ToInt32(parameters["trajectoryMaxAngle"].GetValue()) : 70;
            this.trajectoryMinArea = parameters.Contains("trajectoryMinArea") ? Convert.ToInt32(parameters["trajectoryMinArea"].GetValue()) : 5;
            this.trajectoryMinSegmentNumber = parameters.Contains("trajectoryMinSegmentNumber") ? Convert.ToInt32(parameters["trajectoryMinSegmentNumber"].GetValue()) : 3;
            this.upperMeniscusRoiOffset = parameters.Contains("upperMeniscusRoiOffset") ? Convert.ToInt32(parameters["upperMeniscusRoiOffset"].GetValue()) : 0;  //pier: cambiare default value
            this.upperMeniscusRoiType = parameters.Contains("upperMeniscusRoiType") ? Convert.ToInt32(parameters["upperMeniscusRoiType"].GetValue()) : 0;  //pier: cambiare default value
            this.upperMeniscusRoiWidth = parameters.Contains("upperMeniscusRoiWidth") ? Convert.ToInt32(parameters["upperMeniscusRoiWidth"].GetValue()) : 0;  //pier: cambiare default value
            this.upperMeniscusRoiHeight = parameters.Contains("upperMeniscusRoiHeight") ? Convert.ToInt32(parameters["upperMeniscusRoiHeight"].GetValue()) : 0;  //pier: cambiare default value
            this.bottomAnalysisFilterSize = parameters.Contains("bottomAnalysisFilterSize") ? Convert.ToInt32(parameters["bottomAnalysisFilterSize"].GetValue()) : 0;  //pier: cambiare default value
            this.meniscusAnalysisFilterSize = parameters.Contains("meniscusAnalysisFilterSize") ? Convert.ToInt32(parameters["meniscusAnalysisFilterSize"].GetValue()) : 0;  //pier: cambiare default value
            this.upperMeniscusMaxAreaReject = parameters.Contains("upperMeniscusMaxAreaReject") ? Convert.ToInt32(parameters["upperMeniscusMaxAreaReject"].GetValue()) : 0;  //pier: cambiare default value
            this.upperMeniscusFramesNumber = parameters.Contains("upperMeniscusFramesNumber") ? Convert.ToInt32(parameters["upperMeniscusFramesNumber"].GetValue()) : 0;  //pier: cambiare default value
            this.recurrentMinNumber = parameters.Contains("recurrentMinNumber") ? Convert.ToInt32(parameters["recurrentMinNumber"].GetValue()) : 0;  //pier: cambiare default value
            this.recurrentMinBlobsArea = parameters.Contains("recurrentMinBlobsArea") ? Convert.ToInt32(parameters["recurrentMinBlobsArea"].GetValue()) : 0;  //pier: cambiare default value
            this.recurrentMaxBlobsArea = parameters.Contains("recurrentMaxBlobsArea") ? Convert.ToInt32(parameters["recurrentMaxBlobsArea"].GetValue()) : 0;  //pier: cambiare default value
            this.recurrentMaxBlobsDistance = parameters.Contains("recurrentMaxBlobsDistance") ? Convert.ToInt32(parameters["recurrentMaxBlobsDistance"].GetValue()) : 0;  //pier: cambiare default value
            this.recurrentMaxAreaRatioPerc = parameters.Contains("recurrentMaxAreaRatioPerc") ? Convert.ToInt32(parameters["recurrentMaxAreaRatioPerc"].GetValue()) : 0;  //pier: cambiare default value
            this.upperMeniscusRoiOffset2 = parameters.Contains("upperMeniscusRoiOffset2") ? Convert.ToInt32(parameters["upperMeniscusRoiOffset2"].GetValue()) : 0;  //pier: cambiare default value
            this.upperMeniscusRoiWidth2 = parameters.Contains("upperMeniscusRoiWidth2") ? Convert.ToInt32(parameters["upperMeniscusRoiWidth2"].GetValue()) : 0;  //pier: cambiare default value
            this.upperMeniscusRoiHeight2 = parameters.Contains("upperMeniscusRoiHeight2") ? Convert.ToInt32(parameters["upperMeniscusRoiHeight2"].GetValue()) : 0;  //pier: cambiare default value
            this.upperMeniscusMaxAreaReject2 = parameters.Contains("upperMeniscusMaxAreaReject2") ? Convert.ToInt32(parameters["upperMeniscusMaxAreaReject2"].GetValue()) : 0;  //pier: cambiare default value
            this.upperMeniscusFramesNumber2 = parameters.Contains("upperMeniscusFramesNumber2") ? Convert.ToInt32(parameters["upperMeniscusFramesNumber2"].GetValue()) : 0;  //pier: cambiare default value
            this.upperMeniscusRleThreshold = parameters.Contains("upperMeniscusRleThreshold") ? Convert.ToInt32(parameters["upperMeniscusRleThreshold"].GetValue()) : 0;  //pier: cambiare default value
        }

        internal RecipeAdvancedParameterCollection ToParamCollection(CultureInfo culture) {
            RecipeAdvancedParameterCollection list = new RecipeAdvancedParameterCollection();
            list.Add("morphologicalFilter1", morphologicalFilter1.ToString(culture));
            list.Add("morphologicalFilter2", morphologicalFilter2.ToString(culture));
            list.Add("rleBufferSize", rleBufferSize.ToString(culture));
            list.Add("ROIDynamicMorphIter", ROIDynamicMorphIter.ToString(culture));
            list.Add("meniscusAnalysisMinCount", meniscusAnalysisMinCount.ToString(culture));
            list.Add("meniscusAnalysisThreshold", meniscusAnalysisThreshold.ToString(culture));
            list.Add("meniscusPUMOffset", meniscusPUMOffset.ToString(culture));
            //list.Add("blobsMaxNumber", blobsMaxNumber.ToString(culture));
            list.Add("bubbleMaxShapeFactor", bubbleMaxShapeFactor.ToString(culture));
            list.Add("bubbleOpticalDensityMinDiff", bubbleOpticalDensityMinDiff.ToString(culture));
            list.Add("bubbleSizeMaximum", bubbleSizeMaximum.ToString(culture));
            list.Add("bubbleSizeMinimum", bubbleSizeMinimum.ToString(culture));
            list.Add("trackingMaxAcceleration", trackingMaxAcceleration.ToString(culture));
            list.Add("trackingMaxAreaRatio", trackingMaxAreaRatio.ToString(culture));
            list.Add("trackingMaxHeightRatio", trackingMaxHeightRatio.ToString(culture));
            list.Add("trackingMaxWidthRatio", trackingMaxWidthRatio.ToString(culture));
            list.Add("trackingMaxSegmentAngle", trackingMaxSegmentAngle.ToString(culture));
            list.Add("trackingMaxTrajectoryAngle", trackingMaxTrajectoryAngle.ToString(culture));
            list.Add("trackingMinDistance", trackingMinDistance.ToString(culture));
            list.Add("trackingNumberFramesBackward", trackingNumberFramesBackward.ToString(culture));
            list.Add("trackingFrameBackwardReduction", trackingFrameBackwardReduction.ToString(culture));
            list.Add("trajectoriesBufferSize", trajectoriesBufferSize.ToString(culture));
            list.Add("trajectoryAllSegmentsMaxLength", trajectoryAllSegmentsMaxLength.ToString(culture));
            list.Add("trajectoryAllSegmentsMinLength", trajectoryAllSegmentsMinLength.ToString(culture));
            list.Add("trajectoryRecurrentBackward", trajectoryRecurrentBackward.ToString(culture));
            list.Add("trajectoryRecurrentMaxAngle", trajectoryRecurrentMaxAngle.ToString(culture));
            list.Add("trajectoryRecurrentMaxDistance", trajectoryRecurrentMaxDistance.ToString(culture));
            list.Add("trajectoryRecurrentMinCount", trajectoryRecurrentMinCount.ToString(culture));
            list.Add("trajectoryRecurrentMinLength", trajectoryRecurrentMinLength.ToString(culture));
            //list.Add("maskingBlobsHighThreshold", maskingBlobsHighThreshold.ToString(culture));
            //list.Add("maskingBlobsLowThreshold", maskingBlobsLowThreshold.ToString(culture));
            list.Add("maskingBlobsSensibility", maskingBlobsSensibility.ToString(culture));
            list.Add("maskingBlobsThreshold", maskingBlobsThreshold.ToString(culture));
            list.Add("encoderCounterFirstImage", encoderCounterFirstImage.ToString(culture));
            list.Add("bottomAnalysisMinCount", bottomAnalysisMinCount.ToString(culture));
            list.Add("trackingMaxDistance", trackingMaxDistance.ToString(culture));
            list.Add("trajectoryMaxAngle", trajectoryMaxAngle.ToString(culture));
            list.Add("trajectoryMinArea", trajectoryMinArea.ToString(culture));
            list.Add("trajectoryMinSegmentNumber", trajectoryMinSegmentNumber.ToString(culture));
            list.Add("upperMeniscusRoiOffset", upperMeniscusRoiOffset.ToString(culture));
            list.Add("upperMeniscusRoiType", upperMeniscusRoiType.ToString(culture));
            list.Add("upperMeniscusRoiWidth", upperMeniscusRoiWidth.ToString(culture));
            list.Add("upperMeniscusRoiHeight", upperMeniscusRoiHeight.ToString(culture));
            list.Add("bottomAnalysisFilterSize", bottomAnalysisFilterSize.ToString(culture));
            list.Add("meniscusAnalysisFilterSize", meniscusAnalysisFilterSize.ToString(culture));
            list.Add("upperMeniscusMaxAreaReject", upperMeniscusMaxAreaReject.ToString(culture));
            list.Add("upperMeniscusFramesNumber", upperMeniscusFramesNumber.ToString(culture));
            list.Add("recurrentMinNumber", recurrentMinNumber.ToString(culture));
            list.Add("recurrentMinBlobsArea", recurrentMinBlobsArea.ToString(culture));
            list.Add("recurrentMaxBlobsArea", recurrentMaxBlobsArea.ToString(culture));
            list.Add("recurrentMaxBlobsDistance", recurrentMaxBlobsDistance.ToString(culture));
            list.Add("recurrentMaxAreaRatioPerc", recurrentMaxAreaRatioPerc.ToString(culture));
            list.Add("upperMeniscusRoiOffset2", upperMeniscusRoiOffset2.ToString(culture));
            list.Add("upperMeniscusRoiWidth2", upperMeniscusRoiWidth2.ToString(culture));
            list.Add("upperMeniscusRoiHeight2", upperMeniscusRoiHeight2.ToString(culture));
            list.Add("upperMeniscusMaxAreaReject2", upperMeniscusMaxAreaReject2.ToString(culture));
            list.Add("upperMeniscusFramesNumber2", upperMeniscusFramesNumber2.ToString(culture));
            list.Add("upperMeniscusRleThreshold", upperMeniscusRleThreshold.ToString(culture));
            return list;
        }
    };

    unsafe struct TI_RecipeAdvancedCosmetic {
        public int ARGBinBlobHighValue;
        public int ARGBinBlobInArea;
        public int ARGBinBlobIsBorderOFF;
        public int ARGBinBlobMask;
        public int ARGBinBlobOperationType;
        public int ARGSetTipEdgeFillGap;
        public int ARGSetTipEdgeMinLen;
        public int ARGSetTipEdgeSmooth;
        public int ARGSetTipFilterLen;
        public int ARGSetTipFilterSigma;
        public int ARGSetTipLutBitDepth;
        public int ARGSetTipLutEndValue;
        public int ARGSetTipLutStartValue;
        public int ARGSetTipOrientation;
        public int ARGTipControlFastEdge;
        public int ARGTipControlIDraw;
        public int ARGTipControlInArea;
        public int ARGTipControlMaskInTollerance;
        public int ARGToolTagType;
        public int ARGBottleFlipOffBinarizationBounding;
        public int ARGBottleFlipOffGridSquareSize;
        public int ARGBottleFlipOffGridType;
        public int ARGBottleSleeveBinarizationBounding;
        public int ARGBottleSleeveGridSquareSize;
        public int ARGBottleSleeveGridType;
        public int ARGBottleTopBinarizationBounding;
        public int ARGBottleTopFinderGrayMin;
        public int ARGBottleTopFinderGrayMax;
        public int backgroundType;
        public int orientation;
        public TI_TextData ARGSetTipModelPath;
        public TI_BinaryData ARGBottleFlipOffLearning;
        public TI_BinaryData ARGBottleSleeveLearning;

        //internal void Init() {
        //    ARGSetTipModelPath = new TI_BinaryData(512,1);
        //    ARGBottleSleeveLearning = new TI_BinaryData(1024,1024);
        //    ARGBottleFlipOffLearning = new TI_BinaryData(1024,1024);
        //}

        public TI_RecipeAdvancedCosmetic(IParameterCollection parameters) {

            ARGSetTipModelPath = new TI_TextData(512, 1);
            ARGBottleSleeveLearning = new TI_BinaryData(1024, 1024);
            ARGBottleFlipOffLearning = new TI_BinaryData(1024, 1024);
            this.ARGBinBlobHighValue = parameters.Contains("ARGBinBlobHighValue") ? Convert.ToInt32(parameters["ARGBinBlobHighValue"].GetValue()) : 50;
            this.ARGBinBlobInArea = parameters.Contains("ARGBinBlobInArea") ? Convert.ToInt32(parameters["ARGBinBlobInArea"].GetValue()) : 1;
            this.ARGBinBlobIsBorderOFF = parameters.Contains("ARGBinBlobIsBorderOFF") ? Convert.ToInt32(parameters["ARGBinBlobIsBorderOFF"].GetValue()) : 1;
            this.ARGBinBlobMask = parameters.Contains("ARGBinBlobMask") ? Convert.ToInt32(parameters["ARGBinBlobMask"].GetValue()) : 1;
            this.ARGBinBlobOperationType = parameters.Contains("ARGBinBlobOperationType") ? Convert.ToInt32(parameters["ARGBinBlobOperationType"].GetValue()) : 4;
            this.ARGSetTipEdgeFillGap = parameters.Contains("ARGSetTipEdgeFillGap") ? Convert.ToInt32(parameters["ARGSetTipEdgeFillGap"].GetValue()) : 5;
            this.ARGSetTipEdgeMinLen = parameters.Contains("ARGSetTipEdgeMinLen") ? Convert.ToInt32(parameters["ARGSetTipEdgeMinLen"].GetValue()) : 1;
            this.ARGSetTipEdgeSmooth = parameters.Contains("ARGSetTipEdgeSmooth") ? Convert.ToInt32(parameters["ARGSetTipEdgeSmooth"].GetValue()) : 60;
            this.ARGSetTipFilterLen = parameters.Contains("ARGSetTipFilterLen") ? Convert.ToInt32(parameters["ARGSetTipFilterLen"].GetValue()) : 15;
            this.ARGSetTipFilterSigma = parameters.Contains("ARGSetTipFilterSigma") ? Convert.ToInt32(parameters["ARGSetTipFilterSigma"].GetValue()) : 5;
            this.ARGSetTipLutBitDepth = parameters.Contains("ARGSetTipLutBitDepth") ? Convert.ToInt32(parameters["ARGSetTipLutBitDepth"].GetValue()) : 8;
            this.ARGSetTipLutEndValue = parameters.Contains("ARGSetTipLutEndValue") ? Convert.ToInt32(parameters["ARGSetTipLutEndValue"].GetValue()) : 255;
            this.ARGSetTipLutStartValue = parameters.Contains("ARGSetTipLutStartValue") ? Convert.ToInt32(parameters["ARGSetTipLutStartValue"].GetValue()) : 0;
            this.ARGSetTipOrientation = parameters.Contains("ARGSetTipOrientation") ? Convert.ToInt32(parameters["ARGSetTipOrientation"].GetValue()) : 1;
            this.ARGTipControlFastEdge = parameters.Contains("ARGSetTipOrientation") ? Convert.ToInt32(parameters["ARGTipControlFastEdge"].GetValue()) : 0;
            this.ARGTipControlIDraw = parameters.Contains("ARGTipControlIDraw") ? Convert.ToInt32(parameters["ARGTipControlIDraw"].GetValue()) : 1;
            this.ARGTipControlInArea = parameters.Contains("ARGTipControlInArea") ? Convert.ToInt32(parameters["ARGTipControlInArea"].GetValue()) : 1;
            this.ARGTipControlMaskInTollerance = parameters.Contains("ARGTipControlMaskInTollerance") ? Convert.ToInt32(parameters["ARGTipControlMaskInTollerance"].GetValue()) : 10;
            this.ARGToolTagType = parameters.Contains("ARGToolTagType") ? Convert.ToInt32(parameters["ARGToolTagType"].GetValue()) : 0;
            this.ARGBottleFlipOffBinarizationBounding = parameters.Contains("ARGBottleFlipOffBinarizationBounding") ? Convert.ToInt32(parameters["ARGBottleFlipOffBinarizationBounding"].GetValue()) : 0;
            this.ARGBottleFlipOffGridSquareSize = parameters.Contains("ARGBottleFlipOffGridSquareSize") ? Convert.ToInt32(parameters["ARGBottleFlipOffGridSquareSize"].GetValue()) : 5;
            this.ARGBottleFlipOffGridType = parameters.Contains("ARGBottleFlipOffGridType") ? Convert.ToInt32(parameters["ARGBottleFlipOffGridType"].GetValue()) : 0;
            this.ARGBottleSleeveBinarizationBounding = parameters.Contains("ARGBottleSleeveBinarizationBounding") ? Convert.ToInt32(parameters["ARGBottleSleeveBinarizationBounding"].GetValue()) : 0;
            this.ARGBottleSleeveGridSquareSize = parameters.Contains("ARGBottleSleeveGridSquareSize") ? Convert.ToInt32(parameters["ARGBottleSleeveGridSquareSize"].GetValue()) : 2;
            this.ARGBottleSleeveGridType = parameters.Contains("ARGBottleSleeveGridType") ? Convert.ToInt32(parameters["ARGBottleSleeveGridType"].GetValue()) : 1;
            this.ARGBottleTopBinarizationBounding = parameters.Contains("ARGBottleTopBinarizationBounding") ? Convert.ToInt32(parameters["ARGBottleTopBinarizationBounding"].GetValue()) : 1;
            this.ARGBottleTopFinderGrayMin = parameters.Contains("ARGBottleTopFinderGrayMin") ? Convert.ToInt32(parameters["ARGBottleTopFinderGrayMin"].GetValue()) : -1;
            this.ARGBottleTopFinderGrayMax = parameters.Contains("ARGBottleTopFinderGrayMax") ? Convert.ToInt32(parameters["ARGBottleTopFinderGrayMax"].GetValue()) : -1;
            this.backgroundType = parameters.Contains("backgroundType") ? Convert.ToInt32(parameters["backgroundType"].GetValue()) : 0;
            this.orientation = parameters.Contains("orientation") ? Convert.ToInt32(parameters["orientation"].GetValue()) : 1;
            //this.ARGSetTipModelPath = Encoding.ASCII.GetBytes(parameters["ARGSetTipModelPath"].GetValue().ToString());
            if (parameters.Contains("ARGSetTipModelPath") && parameters["ARGSetTipModelPath"].GetValue() != null)
                this.ARGSetTipModelPath.FromString(parameters["ARGSetTipModelPath"].GetValue().ToString());
            else
                this.ARGSetTipModelPath.FromString("");
            if (parameters.Contains("ARGBottleFlipOffLearning") && parameters["ARGBottleFlipOffLearning"].GetValue() != null)
                this.ARGBottleFlipOffLearning.FromString(parameters["ARGBottleFlipOffLearning"].GetValue().ToString());
            else
                this.ARGBottleFlipOffLearning.FromString("");
            if (parameters.Contains("ARGBottleSleeveLearning") && parameters["ARGBottleSleeveLearning"].GetValue() != null)
                this.ARGBottleSleeveLearning.FromString(parameters["ARGBottleSleeveLearning"].GetValue().ToString());
            else
                this.ARGBottleFlipOffLearning.FromString("");


            /*int* ps1 = (int*)ARGSetTipModelPath.data_used;
            int* ps2 = (int*)ARGBottleSleeveLearning.data_used;
            int* ps3 = (int*)ARGBottleFlipOffLearning.data_used;
            unsafe {
                byte* c = (byte*)ARGSetTipModelPath.data;
                ARGSetTipModelPath.data_size = s.Length;
                *ps1 = s.Length;

                for (int i = 0; i < s.Length; i++) {
                    c[i] = (byte)s[i];
                }

                c[s.Length] = (byte)'\0';
                p += sizeof(TI_BinaryData);
            }*/
            //fixed (byte* c = this.ARGSetTipModelPath) {

            //    for (int i = 0; i < s.Length; i++) {
            //        c[i] = (byte)s[i];
            //    }
            //}
        }

        internal RecipeAdvancedParameterCollection ToParamCollection(CultureInfo culture) {
            RecipeAdvancedParameterCollection list = new RecipeAdvancedParameterCollection();
            list.Add("ARGBinBlobHighValue", ARGBinBlobHighValue.ToString(culture));
            list.Add("ARGBinBlobInArea", ARGBinBlobInArea.ToString(culture));
            list.Add("ARGBinBlobIsBorderOFF", ARGBinBlobIsBorderOFF.ToString(culture));
            list.Add("ARGBinBlobMask", ARGBinBlobMask.ToString(culture));
            list.Add("ARGBinBlobOperationType", ARGBinBlobOperationType.ToString(culture));
            list.Add("ARGSetTipEdgeFillGap", ARGSetTipEdgeFillGap.ToString(culture));
            list.Add("ARGSetTipEdgeMinLen", ARGSetTipEdgeMinLen.ToString(culture));
            list.Add("ARGSetTipEdgeSmooth", ARGSetTipEdgeSmooth.ToString(culture));
            list.Add("ARGSetTipFilterLen", ARGSetTipFilterLen.ToString(culture));
            list.Add("ARGSetTipFilterSigma", ARGSetTipFilterSigma.ToString(culture));
            list.Add("ARGSetTipLutBitDepth", ARGSetTipLutBitDepth.ToString(culture));
            list.Add("ARGSetTipLutEndValue", ARGSetTipLutEndValue.ToString(culture));
            list.Add("ARGSetTipLutStartValue", ARGSetTipLutStartValue.ToString(culture));
            list.Add("ARGSetTipOrientation", ARGSetTipOrientation.ToString(culture));
            list.Add("ARGTipControlFastEdge", ARGTipControlFastEdge.ToString(culture));
            list.Add("ARGTipControlIDraw", ARGTipControlIDraw.ToString(culture));
            list.Add("ARGTipControlInArea", ARGTipControlInArea.ToString(culture));
            list.Add("ARGTipControlMaskInTollerance", ARGTipControlMaskInTollerance.ToString(culture));
            list.Add("ARGToolTagType", ARGToolTagType.ToString(culture));
            list.Add("ARGBottleFlipOffBinarizationBounding", ARGBottleFlipOffBinarizationBounding.ToString(culture));
            list.Add("ARGBottleFlipOffGridSquareSize", ARGBottleFlipOffGridSquareSize.ToString(culture));
            list.Add("ARGBottleFlipOffGridType", ARGBottleFlipOffGridType.ToString(culture));
            list.Add("ARGBottleSleeveBinarizationBounding", ARGBottleSleeveBinarizationBounding.ToString(culture));
            list.Add("ARGBottleSleeveGridSquareSize", ARGBottleSleeveGridSquareSize.ToString(culture));
            list.Add("ARGBottleSleeveGridType", ARGBottleSleeveGridType.ToString(culture));
            list.Add("ARGBottleTopBinarizationBounding", ARGBottleTopBinarizationBounding.ToString(culture));
            list.Add("ARGBottleTopFinderGrayMin", ARGBottleTopFinderGrayMin.ToString(culture));
            list.Add("ARGBottleTopFinderGrayMax", ARGBottleTopFinderGrayMax.ToString(culture));
            list.Add("backgroundType", backgroundType.ToString(culture));
            list.Add("orientation", orientation.ToString(culture));
            list.Add("ARGSetTipModelPath", ARGSetTipModelPath.ToString());
            list.Add("ARGBottleFlipOffLearning", ARGBottleFlipOffLearning.ToString());
            list.Add("ARGBottleSleeveLearning", ARGBottleSleeveLearning.ToString());
            return list;
        }
    };

    struct TI_ROI {
        public int roiID;
        public int roiType;
        public int roiX1;
        public int roiX2;
        public int roiY1;
        public int roiY2;
        public int roiColor;
        public int roiFill;
        public int roiPlot;

        public TI_ROI(IParameterCollection parameters) {
            this.roiID = parameters.Contains("roiID") ? Convert.ToInt32(parameters["roiID"].GetValue()) : 1;
            this.roiType = parameters.Contains("roiType") ? Convert.ToInt32(parameters["roiType"].GetValue()) : 0;
            this.roiX1 = parameters.Contains("roiX1") ? Convert.ToInt32(parameters["roiX1"].GetValue()) : 68;
            this.roiX2 = parameters.Contains("roiX2") ? Convert.ToInt32(parameters["roiX2"].GetValue()) : 350;
            this.roiY1 = parameters.Contains("roiY1") ? Convert.ToInt32(parameters["roiY1"].GetValue()) : 40;
            this.roiY2 = parameters.Contains("roiY2") ? Convert.ToInt32(parameters["roiY2"].GetValue()) : 323;
            this.roiColor = parameters.Contains("roiColor") ? Convert.ToInt32(parameters["roiColor"].GetValue()) : 0;
            this.roiFill = parameters.Contains("roiFill") ? Convert.ToInt32(parameters["roiFill"].GetValue()) : 0;
            this.roiPlot = parameters.Contains("roiPlot") ? Convert.ToInt32(parameters["roiPlot"].GetValue()) : 1;
        }

        internal ROIParameterCollection ToParamCollection(CultureInfo culture) {
            ROIParameterCollection list = new ROIParameterCollection();
            list.Add("roiID", roiID.ToString(culture));
            list.Add("roiType", roiType.ToString(culture));
            list.Add("roiX1", roiX1.ToString(culture));
            list.Add("roiX2", roiX2.ToString(culture));
            list.Add("roiY1", roiY1.ToString(culture));
            list.Add("roiY2", roiY2.ToString(culture));
            list.Add("roiColor", roiColor.ToString(culture));
            list.Add("roiFill", roiFill.ToString(culture));
            list.Add("roiPlot", roiPlot.ToString(culture));
            return list;
        }
    };

    struct TI_MachineParameters {
        public int cameraOrientation;
        public int encoderSwapPhase;
        public float encoderToPixelFactor;
        public int flipImage;
        public int headOneOffset;
        public int headsNumber;
        public int IOOutDataValidDelay;
        public int IOOutDataValidWidth;
        public int triggerMode;
        public int triggerSlope;
        public int cycleTime;

        public TI_MachineParameters(IParameterCollection parameters, CultureInfo culture) {

            this.cameraOrientation = parameters.Contains("cameraOrientation") ? Convert.ToInt32(parameters["cameraOrientation"].GetValue()) : 0;
            this.encoderSwapPhase = parameters.Contains("encoderSwapPhase") ? Convert.ToInt32(parameters["encoderSwapPhase"].GetValue()) : 1;
            this.encoderToPixelFactor = parameters.Contains("encoderToPixelFactor") ? (float)Convert.ToDouble(parameters["encoderToPixelFactor"].GetValue(), culture) : 0.64F;
            this.flipImage = parameters.Contains("flipImage") ? Convert.ToInt32(parameters["flipImage"].GetValue()) : 2;
            this.headOneOffset = parameters.Contains("headOneOffset") ? Convert.ToInt32(parameters["headOneOffset"].GetValue()) : 34;
            this.headsNumber = parameters.Contains("headsNumber") ? Convert.ToInt32(parameters["headsNumber"].GetValue()) : 40;
            this.IOOutDataValidDelay = parameters.Contains("IOOutDataValidDelay") ? Convert.ToInt32(parameters["IOOutDataValidDelay"].GetValue()) : 100;
            this.IOOutDataValidWidth = parameters.Contains("IOOutDataValidWidth") ? Convert.ToInt32(parameters["IOOutDataValidWidth"].GetValue()) : 20000;
            this.triggerMode = parameters.Contains("triggerMode") ? Convert.ToInt32(parameters["triggerMode"].GetValue()) : 3;
            this.triggerSlope = parameters.Contains("triggerSlope") ? Convert.ToInt32(parameters["triggerSlope"].GetValue()) : 1;
            this.cycleTime = parameters.Contains("cycleTime") ? Convert.ToInt32(parameters["cycleTime"].GetValue()) : 150;
        }

        internal MachineParameterCollection ToParamCollection(CultureInfo culture) {
            MachineParameterCollection list = new MachineParameterCollection();
            list.Add("cameraOrientation", cameraOrientation.ToString(culture));
            list.Add("encoderSwapPhase", encoderSwapPhase.ToString(culture));
            list.Add("encoderToPixelFactor", encoderToPixelFactor.ToString(culture));
            list.Add("flipImage", flipImage.ToString(culture));
            list.Add("headOneOffset", headOneOffset.ToString(culture));
            list.Add("headsNumber", headsNumber.ToString(culture));
            list.Add("IOOutDataValidDelay", IOOutDataValidDelay.ToString(culture));
            list.Add("IOOutDataValidWidth", IOOutDataValidWidth.ToString(culture));
            list.Add("triggerMode", triggerMode.ToString(culture));
            list.Add("triggerSlope", triggerSlope.ToString(culture));
            list.Add("cycleTime", cycleTime.ToString(culture));
            return list;
        }

    };

    struct TI_MachineParametersCosmetic {
        public int triggerMode;
        public int turbo;
        public int headsNumber;

        public TI_MachineParametersCosmetic(IParameterCollection parameters, CultureInfo culture) {

            this.triggerMode = parameters.Contains("triggerMode") ? Convert.ToInt32(parameters["triggerMode"].GetValue()) : 1;
            this.turbo = parameters.Contains("turbo") ? Convert.ToInt32(parameters["turbo"].GetValue()) : 0;
            this.headsNumber = parameters.Contains("headsNumber") ? Convert.ToInt32(parameters["headsNumber"].GetValue()) : 36;
        }

        internal MachineParameterCollection ToParamCollection(CultureInfo culture) {
            MachineParameterCollection list = new MachineParameterCollection();
            list.Add("triggerMode", triggerMode.ToString(culture));
            list.Add("turbo", turbo.ToString(culture));
            list.Add("headsNumber", headsNumber.ToString(culture));
            return list;
        }
    };

    struct TI_StopCondition {
        public int headNumber;
        public int resultType;
    };

    enum TI_BufferType {
        BUFFER_ACQUISITION,
        BUFFER_MENISCUS,
        BUFFER_DIFF,
        BUFFER_RESULTS,
        BUFFER_TRACKING,
        BUFFER_TRAJECTORIES,
        BUFFER_TEMPORARY,
    };

    enum TI_DirectxResultAction {
        DIRECTX_RESULT_IMAGE_STOP,	/**< Stop result image stream */
        DIRECTX_RESULT_IMAGE_START,	/**< Start result image stream */
    }

    enum TI_PatriclesMode {
        MODE_STOP_ON_CONDITION = 1000,
        MODE_LEARNING_ANALYSIS_PARAMETER = 1300,
        MODE_LEARNING_STROBO = 1400,
        MODE_LEARNING_VIALAXIS = 1500,
        MODE_LEARNING_RESET_TRIGGER_ENCODER = 1700,
        MODE_LEARNING_NORMALIZATION = 1900,
        MODE_CHECK_VIALAXIS = 2000,
        MODE_CHECK_CONTROROTATION = -1,
        MODE_LIVE = 2200,
    };

    struct TI_LearningStrobo {
        public int size;
        public int maxDensityReference;
        public int stroboTime;
        public int shutterTime;
    };

    struct TI_LearningNormalization {
        public int size;
        public int pgaRawStart;
        public int pgaRawEnd;
    };

    struct TI_LearningAnalysisParameters {
        public int size;
        public int samples;
        public int blobsMaxArea;
        public int blobsMinArea;
        public int trajectoryMinArea;
        public int fillingLevelMin;
        public int fillingLevelMax;
    };

    struct TI_LearningVialAxis {
        public int size;
        public int samplesVialAxis;
        public int findVialAxisX;
        public int findVialAxisY;
    };

    struct TI_CheckVialAxis {
    };

    struct TI_LearningResetTriggerEncoder {
        public int size;
        public int findVialAxisX;
        public int findVialAxisY;
        public int encoderTrigger;
    };

    struct TI_AxisBuffer {
        public int code;
        public int filledDataNumber;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 63)]
        public TI_VialAxisData[] data;
    };
    #endregion

    #region TattileInterfaceFunctions
    internal static class TattileInterfaceSvc {
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_InitDll();
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_CloseDll();
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_ConnectionOpen(int* totalNumberCameras);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_InfoGet(int idCamera, ref TI_CameraInfo cameraInfo);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_Reset(int idCamera);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_ResultImageDirectX(int idCamera, Int32 action, Int32 adapterPort);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_LiveStart(int idCamera, bool sensorWindowed);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_LiveStop(int idCamera);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_ResetCounters(int idCamera);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_AssistantSet(int idCamera, int mode, void* inputVars);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_AssistantStart(int idCamera);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_AssistantStop(int idCamera, bool saveParam);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_AssistantResetError(int idCamera);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_AnalysisOffLineStart(int idCamera);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_AnalysisOffLineStop(int idCamera);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_KnappSingleCameraStop(int idCamera);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_KnappSingleCameraStart(int idCamera);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_StopOnCondition(int idCamera, ref TI_StopCondition stopCondition);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_StatusGet(int idCamera, int* cameraStatus);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_GetRunStatus(int idCamera, int* programStatus);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_GetFirmwareVersion(int idCamera, byte[] firmwareVersion, int size);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_GetProgramVersion(int idCamera, byte[] programVersion, int size);

        //PARAMETRI
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_AcquisitionParametersGet(int idCamera, ref TI_AcquisitionParameters acquisitionParameters);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_AcquisitionParametersSet(int idCamera, ref TI_AcquisitionParameters acquisitionParameters);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_FeaturesEnableSet(int idCamera, void* featuresEnable);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_FeaturesEnableGet(int idCamera, void* featuresEnable);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_RecipeSimpleGet(int idCamera, void* recipeSimple);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_RecipeSimpleSet(int idCamera, void* recipeSimple);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_RecipeAdvancedGet(int idCamera, void* recipeAdvanced);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_RecipeAdvancedSet(int idCamera, void* recipeAdvanced);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_ROIGet(int idCamera, int idROI, ref TI_ROI ROIValue);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_ROISet(int idCamera, int idROI, ref TI_ROI ROIValue);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_ROIGetNumber(int idCamera, int* ROINumber);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_FormatSave(int idCamera, int formatNumber, [MarshalAs(UnmanagedType.LPStr)] string description);
        [DllImport("Tattile_Interface.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_FormatLoad(int idCamera, int formatNumber, byte[] buffer);
        [DllImport("Tattile_Interface.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_FormatReadLoaded(int idCamera, int* formatNumber, byte[] buffer);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_MachineParametersSet(int idCamera, void* machineParameters);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_MachineParametersGet(int idCamera, void* machineParameters);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_StartRun(int idCamera);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_StopRun(int idCamera);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_CommitChanges(int idCamera);

        //BUFFER
        [DllImport("Tattile_Interface.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_BufferGet(int idCamera, int bufferType, int bufferNumber,
            int maxBufferSize, byte[] buffer, int* bufferSize);
        [DllImport("Tattile_Interface.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_BufferSet(int idCamera, int bufferType, int bufferNumber,
            [MarshalAs(UnmanagedType.LPStr)] string fileName);
        [DllImport("Tattile_Interface.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_BufferSave(int idCamera, int bufferType, int bufferNumber,
            [MarshalAs(UnmanagedType.LPStr)] string fileName, long imageType);

        //VIAL AXIS
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_AxisGet(int idCamera, ref TI_AxisBuffer axisBuffer);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_AxisSet(int idCamera, ref TI_AxisBuffer axisBuffer);

        //COSMETIC
        [DllImport("Tattile_Interface.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_StreamModeGet(int idCamera, int* resultMode);
        [DllImport("Tattile_Interface.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_StreamModeSet(int idCamera, int resultMode);
        [DllImport("Tattile_Interface.dll", CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_CosmeticLiveOverlay(int idCamera, int roiEnable, int infoEnable);
        [DllImport("Tattile_Interface.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_CosmeticLoadModel(int idCamera);
        [DllImport("Tattile_Interface.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        internal unsafe static extern Int32 TI_CheckModelLoaded(int idCamera, int* modelLoaded);
    }
    #endregion

    #region TattileTagFilter
    internal static class TattileTagFilterSvc {

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr LoadLibrary(string libname);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool FreeLibrary(IntPtr hModule);
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        delegate int TAG_ConnectAdapterDelegate(string bindDevice, ref int adapterHandle);
        delegate int TAG_DisconnectAdapterDelegate(ref int adapterHandle);
        delegate int TAG_ConnectDeviceDelegate(int adapterHandle, string sIpAddressDevice, int RxBufferSize, int RxQueueSizeMax, ref int RxQueueSize, ref int DeviceHandle);
        delegate int TAG_DisconnectDeviceDelegate(ref int deviceHandle);
        delegate int TAG_ResetDeviceQueueDelegate(int deviceHandle, int resetBufferOnly);
        delegate int TAG_SetModeDelegate(int deviceHandle, int mode, int port);
        delegate int TAG_ImageGetDelegate(int deviceHandle, ref int image, ref TAGLostImageInfos lost, int waitTime);
        delegate int TAG_ImageUnlockDelegate(int deviceHandle, int image);

        static string tagFilterFilename;
        static IntPtr tagFilterLibHandle;

        internal static void LoadTagFilter(string tagFilterPath) {

            if (tagFilterLibHandle == IntPtr.Zero) {
                tagFilterFilename = tagFilterPath;
                tagFilterLibHandle = LoadLibrary(tagFilterFilename);
                if (tagFilterLibHandle == IntPtr.Zero) {
                    int errorCode = Marshal.GetLastWin32Error();
                    string error = string.Format("Failed to load library (ErrorCode: {0})", errorCode);
                    Log.Line(LogLevels.Error, "TattileTagFilterSvc.LoadTagFilter", error);
                    throw new Exception(error);
                }
                Log.Line(LogLevels.Pass, "TattileTagFilterSvc.LoadTagFilter", "TAG_Filter library loaded successfully");
                AppDomain.CurrentDomain.ProcessExit += TattileTagFilterSvc_Dtor;
            }
        }

        static void TattileTagFilterSvc_Dtor(object sender, EventArgs e) {
            freeTagFilter();
        }

        static void freeTagFilter() {
            if (tagFilterLibHandle != IntPtr.Zero) {
                for (int i = 0; i < 3; i++) {
                    if (!FreeLibrary(tagFilterLibHandle)) {
                        tagFilterLibHandle = IntPtr.Zero;
                        Log.Line(LogLevels.Pass, "TattileTagFilterSvc.freeTagFilter", "TAG_Filter library unloaded successfully");
                    }
                }
            }
        }

        internal static int ConnectAdapter(string bindDevice, ref int adapterHandle) {
            IntPtr funcaddr = GetProcAddress(tagFilterLibHandle, "TAG_ConnectAdapter");
            TAG_ConnectAdapterDelegate function = Marshal.GetDelegateForFunctionPointer(funcaddr, typeof(TAG_ConnectAdapterDelegate)) as TAG_ConnectAdapterDelegate;
            Log.Line(LogLevels.Debug, "TattileTagFilterSvc.ConnectAdapter", "Connecting adapter " + bindDevice + "...");
            return function.Invoke(bindDevice, ref adapterHandle);
        }

        internal static int DisconnectAdapter(ref int adapterHandle) {
            IntPtr funcaddr = GetProcAddress(tagFilterLibHandle, "TAG_DisconnectAdapter");
            TAG_DisconnectAdapterDelegate function = Marshal.GetDelegateForFunctionPointer(funcaddr, typeof(TAG_DisconnectAdapterDelegate)) as TAG_DisconnectAdapterDelegate;
            Log.Line(LogLevels.Debug, "TattileTagFilterSvc.DisconnectAdapter", "Disconnecting adapter (nic handle: {0})...", adapterHandle);
            return function.Invoke(ref adapterHandle);
        }

        internal static int ConnectDevice(int adapterHandle, string sIpAddressDevice, int RxBufferSize, int RxQueueSizeMax, ref int RxQueueSize, ref int deviceHandle) {
            IntPtr funcaddr = GetProcAddress(tagFilterLibHandle, "TAG_ConnectDevice");
            TAG_ConnectDeviceDelegate function = Marshal.GetDelegateForFunctionPointer(funcaddr, typeof(TAG_ConnectDeviceDelegate)) as TAG_ConnectDeviceDelegate;
            Log.Line(LogLevels.Debug, "TattileTagFilterSvc.ConnectDevice", "Connecting device (nic handle: {0}) " + sIpAddressDevice + " (BUFFER SIZE = {1}; QUEUE SIZE MAX = {2})...", adapterHandle, RxBufferSize, RxQueueSizeMax);
            return function.Invoke(adapterHandle, sIpAddressDevice, RxBufferSize, RxQueueSizeMax, ref RxQueueSize, ref deviceHandle);
        }

        internal static int DisconnectDevice(ref int deviceHandle) {
            IntPtr funcaddr = GetProcAddress(tagFilterLibHandle, "TAG_DisconnectDevice");
            TAG_DisconnectDeviceDelegate function = Marshal.GetDelegateForFunctionPointer(funcaddr, typeof(TAG_DisconnectDeviceDelegate)) as TAG_DisconnectDeviceDelegate;
            Log.Line(LogLevels.Debug, "TattileTagFilterSvc.DisconnectDevice", "Disconnecting device (device handle: {0})...", deviceHandle);
            return function.Invoke(ref deviceHandle);
        }

        internal static int ResetDeviceQueue(int deviceHandle, int resetBufferOnly) {
            IntPtr funcaddr = GetProcAddress(tagFilterLibHandle, "TAG_ResetDeviceQueue");
            TAG_ResetDeviceQueueDelegate function = Marshal.GetDelegateForFunctionPointer(funcaddr, typeof(TAG_ResetDeviceQueueDelegate)) as TAG_ResetDeviceQueueDelegate;
            Log.Line(LogLevels.Debug, "TattileTagFilterSvc.ResetDeviceQueue", "Resetting device queue (device handle: {0}; reset buffer only: {1})...", deviceHandle, resetBufferOnly);
            return function.Invoke(deviceHandle, resetBufferOnly);
        }

        internal static int SetMode(int deviceHandle, int mode, int port) {
            IntPtr funcaddr = GetProcAddress(tagFilterLibHandle, "TAG_SetMode");
            TAG_SetModeDelegate function = Marshal.GetDelegateForFunctionPointer(funcaddr, typeof(TAG_SetModeDelegate)) as TAG_SetModeDelegate;
            Log.Line(LogLevels.Debug, "TattileTagFilterSvc.SetMode", "Setting mode (device handle: {0}; mode: {1}; port: {2})...", deviceHandle, mode, port);
            return function.Invoke(deviceHandle, mode, port);
        }

        internal static int ImageGet(int deviceHandle, ref int image, ref TAGLostImageInfos lost, int waitTime) {
            IntPtr funcaddr = GetProcAddress(tagFilterLibHandle, "TAG_ImageGet");
            TAG_ImageGetDelegate function = Marshal.GetDelegateForFunctionPointer(funcaddr, typeof(TAG_ImageGetDelegate)) as TAG_ImageGetDelegate;
            //Log.Line(LogLevels.Debug, "TattileTagFilterSvc.ImageGet", "Getting image (device handle: {0}; wait time: {1})...", deviceHandle, waitTime);
            return function.Invoke(deviceHandle, ref image, ref lost, waitTime);
        }

        internal static int ImageUnlock(int deviceHandle, int image) {
            IntPtr funcaddr = GetProcAddress(tagFilterLibHandle, "TAG_ImageUnlock");
            TAG_ImageUnlockDelegate function = Marshal.GetDelegateForFunctionPointer(funcaddr, typeof(TAG_ImageUnlockDelegate)) as TAG_ImageUnlockDelegate;
            //Log.Line(LogLevels.Debug, "TattileTagFilterSvc.ImageUnlock", "Unlocking image (device handle: {0}; image: {1})...", deviceHandle, image);
            return function.Invoke(deviceHandle, image);
        }
    }
    #endregion
}
