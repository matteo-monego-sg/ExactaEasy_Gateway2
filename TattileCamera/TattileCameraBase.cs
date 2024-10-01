using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SPAMI.Util.Logger;
using SPAMI.Util;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using DisplayManager;
using System.Collections.Concurrent;
using System.Diagnostics;
using ExactaEasyEng;
using System.IO;
using System.Globalization;
using ExactaEasyCore;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;

namespace TattileCameras {

    public abstract class TattileCameraBase : Camera, IStation, INode {

        //public event EventHandler<ImagesResultsAvailableEventArgs> ImagesResultsAvailable;
        public event EventHandler<MeasuresAvailableEventArgs> MeasuresAvailable;

        protected internal static CultureInfo culture = CultureInfo.GetCultureInfo("en-GB");
        System.Timers.Timer stopOnCondTimer;

        public int DisplayPosition { get; set; }
        public string Description { get; set; }
        protected int IdTattile { get; set; }
        public CameraCollection Cameras { get; private set; }

        public int IdNode { get; set; }
        public StationCollection Stations { get; private set; }
        public string Address { get; set; }
        public int Port { get; set; }
        public string ProviderName { get; set; }
        public string ServerIP4Address { get; set; }
        public bool DumpResultsEnabled { get; set; }
        public bool HasMeasures { get; set; }
        public bool IsDisposed { get; private set; }
        int width = 0, height = 0, currBpp = 0;
        Gray blackBN = new Gray(0);
        readonly int MAX_INSPECTION_QUEUE_LENGTH;
        RawInspectionBuffer inspectionBuffer = null;

        protected int port;
        TattileIdentity cameraIdentity;

        protected int imageAvailableHandlerCount;
        event EventHandler<ImageAvailableEventArgs> _imageAvailable;
        public event EventHandler<ImageAvailableEventArgs> ImageAvailable {
            add {
                imageAvailableHandlerCount++;
                _imageAvailable += value;
            }
            remove {
                _imageAvailable -= value;
                imageAvailableHandlerCount--;
                if (imageAvailableHandlerCount > 0) imageAvailableHandlerCount = 0;
            }
        }

        protected int alternativeImageAvailableHandlerCount;
        event EventHandler<ImageAvailableEventArgs> _alternativeImageAvailable;
        public event EventHandler<ImageAvailableEventArgs> AlternativeImageAvailable {
            add {
                alternativeImageAvailableHandlerCount++;
                _alternativeImageAvailable += value;
            }
            remove {
                _alternativeImageAvailable -= value;
                alternativeImageAvailableHandlerCount--;
                if (alternativeImageAvailableHandlerCount > 0) alternativeImageAvailableHandlerCount = 0;
            }
        }

        public TattileCameraBase()
            : base() {
        }

        public TattileCameraBase(CameraDefinition cameraDefinition, bool scanRequest)
            : base(cameraDefinition) {

            if (!dllInitialized)
                initTattileCameras();
            if (scanRequest || scanNeverDone)
                scanTattileCameras();

            Enabled = true;

            ROIModeAvailable = false;
            Stations = new StationCollection();
            Stations.Add(this);
            IdStation = StationId;
            IdNode = IdStation;
            Description = CameraDescription = cameraDefinition.CameraDescription;
            queueCapacity = cameraDefinition.BufferSize;
            cameraIdentity = new TattileIdentity() {
                IP = cameraDefinition.IP4Address,
                Head = cameraDefinition.Head
            };
            IdTattile = -1;
            if (cameraInfoDict.Keys.Count > 0) {
                if (!cameraInfoDict.ContainsKey(cameraIdentity)) {
                    Initialized = false;
                    //staticallyInitialized = false;
                    Log.Line(LogLevels.Error, "TattileCameraBase", "Searching: " + cameraIdentity.ToString() /*+ " Dictionary: " + cameraInfoDict.ToString()*/);
                    throw new CameraException("Camera not found, check configuration or connections");
                }
                //cameraInfo = getInfo();
                //cameraInfo = cameraInfoDict[cameraIdentity];
            }
            updateIdTattile();

            if (IdTattile == -1) {
                Initialized = false;
                //staticallyInitialized = false;
                throw new CameraException("Camera not found, check configuration or connections");
            }
            string camType = cameraInfoDict[cameraIdentity].description;
            camType = camType.Replace("\0", string.Empty);
            camType = camType.Split(new string[] { " ", ",", ".", "\t" }, StringSplitOptions.RemoveEmptyEntries)[0];
            if (cameraIdentity.Head != cameraInfoDict[cameraIdentity].headNumber) {
                Initialized = false;
                throw new CameraException("Camera head unexpected, check configuration");
            }
            Head = cameraInfoDict[cameraIdentity].headNumber;
            Cameras = new CameraCollection();
            if (CameraType != null && camType != CameraType) {
                Initialized = false;
                throw new CameraException("Camera type unexpected, check configuration");
            }
            CameraType = camType;
            if (CameraType == "M9") port = 20000;
            else port = 12345;
            if ((IP4Address != null) && (cameraInfoDict[cameraIdentity].IpAddress != IP4Address)) {
                Initialized = false;
                throw new CameraException("Camera IP unexpected, check configuration");
            }
            IP4Address = cameraInfoDict[cameraIdentity].IpAddress;
            MAX_INSPECTION_QUEUE_LENGTH = cameraDefinition.MaxInspectionQueueLength;
            //if ((cameraDefinition.Id >= 0) &&
            //    (cameraDefinition.Id < CameraCount))
            //{
            Init();
            //}
        }

        static TattileCameraBase() {

            initPaletteAntares();
            //initLut();
        }

        void updateIdTattile() {

            if (cameraInfoDict.Keys.Count > 0) {
                List<TattileIdentity> keyList = new List<TattileIdentity>(cameraInfoDict.Keys);
                foreach (TattileIdentity key in keyList) {
                    if (key.Equals(cameraIdentity)) {
                        IdTattile = key.ID;
                        break;
                    }
                    else IdTattile = -1;
                }
            }
            else IdTattile = -1;
        }

        public override void LoadExternalDependencies(List<string> extDependencies) {

            base.LoadExternalDependencies(extDependencies);

            if (ExtDependencies.Count > 0) {
                TattileTagFilterSvc.LoadTagFilter(ExtDependencies[0]);
            }
            else {
                throw new CameraException("Invalid TagFilter library, check configuration");
            }
        }

        static bool dllInitialized = false;
        unsafe static void initTattileCameras() {

            //if (TattileInterfaceSvc.TI_CloseDll() != 0)
            //    throw new CameraException("Tattile Interface not found");

            if (TattileInterfaceSvc.TI_InitDll() != 0)
                throw new CameraException("Tattile Interface not found");

            Log.Line(LogLevels.Pass, "TattileCameraBase.initTattileCameras", "Tattile Interface initialized");
            dllInitialized = true;
        }

        static bool scanNeverDone = true;
        unsafe static void scanTattileCameras() {

            if (!dllInitialized)
                initTattileCameras();
            fixed (int* pCamCount = &_cameraCount) {
                int ret = TattileInterfaceSvc.TI_ConnectionOpen(pCamCount);
                if (ret != 0)
                    throw new CameraException("TI_ConnectionOpen failed with error " + ((ITF_ERROR_CODE)ret).ToString());
            }
            Log.Line(LogLevels.Pass, "TattileCameraBase", "Camera found: " + _cameraCount.ToString());
            //trovo corrispondenza Id -> Id Tattile
            cameraInfoDict.Clear();
            for (int iC = 0; iC < CameraCount; iC++) {
                CameraInfo camInfo = GetInfo(iC);
                TattileIdentity cameraIdentity = new TattileIdentity() {
                    IP = camInfo.IpAddress,
                    Head = camInfo.headNumber,
                    ID = iC
                };
                if (cameraInfoDict.ContainsKey(cameraIdentity))
                    throw new CameraException("IP conflict error");
                cameraInfoDict.Add(cameraIdentity, camInfo);
            }
            scanNeverDone = false;
        }


        static int _cameraCount;
        public static int CameraCount {
            get {
                return _cameraCount;
            }
            private set {
                _cameraCount = value;
            }
        }
        static Dictionary<string, string> NICCameraDict = new Dictionary<string, string>();
        static Dictionary<TattileIdentity, CameraInfo> cameraInfoDict = new Dictionary<TattileIdentity, CameraInfo>();
        //public event BmpEventHandler GrabbedImage;
        public delegate void BmpEventHandler(object sender, BmpEventArgs args);

        //CameraInfo cameraInfo;
        ImageProtocol RxProtocol;

        Thread acqThread, alertThread;
        ManualResetEvent grabEv = new ManualResetEvent(false);
        ManualResetEvent snapEv = new ManualResetEvent(false);
        ManualResetEvent killEv = new ManualResetEvent(false);
        AutoResetEvent readyEv = new AutoResetEvent(false);
        WaitHandle[] acqEvs = new WaitHandle[3];
        WaitHandle[] alertEvs = new WaitHandle[2];
        int queueCapacity;
        //ConcurrentQueue<Image<Rgb, byte>> InputBuffer = new ConcurrentQueue<Image<Rgb, byte>>();
        Image<Rgb, byte> imgToShow;
        Image<Rgb, byte> recImage;
        Image<Gray, byte> recImageBN;
        public AutoResetEvent AcquisitionComplete = new AutoResetEvent(false);
        public AutoResetEvent ResultAcquisitionComplete = new AutoResetEvent(false);
        AutoResetEvent ImageSnapped = new AutoResetEvent(false);
        bool grabbing = false;
        uint rejectionCause;

        //variabili tattiliane
        int m_Camera_handle, m_Eth_port_handle, pImageBufferPtr;
        byte[] byteArray = new byte[10];
        byte[] imgByteArray;
        int imageUnlock;

        enum AcqEvents {
            Kill = 0,
            Grab,
            Snap
        }

        enum AlertEvents {
            Kill = 0,
            Ready
        }

        public void Init() {
            Destroy();
            killEv.Reset();
            m_Camera_handle = m_Eth_port_handle = 0;
            pImageBufferPtr = 0;
            imageUnlock = 0;
            //InputBuffer.Clear();
            acqEvs[(int)AcqEvents.Kill] = killEv;
            acqEvs[(int)AcqEvents.Grab] = grabEv;
            acqEvs[(int)AcqEvents.Snap] = snapEv;
            alertEvs[(int)AlertEvents.Kill] = killEv;
            alertEvs[(int)AlertEvents.Ready] = readyEv;
            Initialized = true;
        }

        public override void Dispose() {

            if (!IsDisposed) {
                Destroy();
                base.Dispose();
                IsDisposed = true;
            }
        }

        void Destroy() {
            try {
                if (Connected) startRun();
            }
            catch {
            }
            snapEv.Reset();
            grabEv.Reset();
            killEv.Set();
            Thread.Sleep(100);
            if (acqThread != null && acqThread.IsAlive) {
                acqThread.Join(1000);
            }
            if (alertThread != null && alertThread.IsAlive) {
                alertThread.Join(1000);
            }
            //InputBuffer.Clear();
            try {
                Disconnect();
                FreeImages();
            }
            catch {
            }
            Initialized = false;
        }

        static void AddConnection(string NIC_IPV4, string CamIPV4) {
            foreach (KeyValuePair<string, string> kvp in NICCameraDict) {
                if (kvp.Key == NIC_IPV4 && kvp.Value == CamIPV4) return;
                if (kvp.Key == NIC_IPV4)
                    throw new CameraException("NIC already in use");
                if (kvp.Value == CamIPV4)
                    throw new CameraException("Camera already in use");
            }
            NICCameraDict.Add(NIC_IPV4, CamIPV4);
        }

        static void RemoveConnection(string NIC_IPV4, string CamIPV4) {
            if (NICCameraDict.ContainsKey(NIC_IPV4) && NICCameraDict[NIC_IPV4] == CamIPV4)
                NICCameraDict.Remove(NIC_IPV4);
        }

        public void Connect() {
            //CameraConnect();
        }

        /// <summary>
        /// Connect to camera
        /// </summary>
        public override void CameraConnect() {
            if (!Initialized)
                throw new CameraException("Camera not statically initialized");
            //InputBuffer.Clear();
            //if (Connected) Disconnect();
            if (alertThread == null || !alertThread.IsAlive) {
                alertThread = new Thread(new ThreadStart(AlertThread));
                alertThread.Name = "TattileCamera Alert Thread";
            }
            if (acqThread == null || !acqThread.IsAlive) {
                acqThread = new Thread(new ThreadStart(AcquisitionThread));
                acqThread.Name = "TattileCamera Acquisition Thread";
            }
            int res = 0;
            //long Value = 0;

            int RxBufferSize = 0;
            int RxQueueSize = 0;
            int RxQueueSizeMax = 0;
            int channels = 1;
            if (CameraType == "M12")
                channels = 3;

            cameraInfoDict[cameraIdentity] = GetCameraInfo();
            RxBufferSize = cameraInfoDict[cameraIdentity].widthImage * cameraInfoDict[cameraIdentity].heightImage * channels;

            RxQueueSize = 0;
            RxQueueSizeMax = queueCapacity;
            ServerIP4Address = cameraInfoDict[cameraIdentity].NicAddress;

            if (NICCameraDict.ContainsKey(cameraInfoDict[cameraIdentity].NicAddress)) {
                Connected = true;
                return;
            }

            Log.Line(LogLevels.Pass, "TattileCameraBase.CameraConnect", "Connecting to Tattile " + CameraType + " camera " + cameraInfoDict[cameraIdentity].IpAddress);
            res = TattileTagFilterSvc.ConnectAdapter(cameraInfoDict[cameraIdentity].NicAddress, ref m_Eth_port_handle);
            if (res != 0)
                throw new CameraException("TAG_ConnectAdapter return " + ((TAGFILTER_ERROR_CODE)res).ToString());

            res = TattileTagFilterSvc.ConnectDevice(m_Eth_port_handle, cameraInfoDict[cameraIdentity].IpAddress, RxBufferSize, RxQueueSizeMax, ref RxQueueSize, ref m_Camera_handle);
            if (res != 0)
                throw new CameraException("TAG_ConnectDevice return " + ((TAGFILTER_ERROR_CODE)res).ToString());

            //long LiveRun_port = 0;

            RxProtocol = ImageProtocol.IMAGE_PROTOCOL_TOJECT;

            if (CameraType == "M9") {
                //port = 20000;
                RxProtocol = ImageProtocol.IMAGE_PROTOCOL_TOJECT;
            }
            else if (CameraType == "M12") {
                //port = 12345;
                RxProtocol = ImageProtocol.IMAGE_PROTOCOL_GIGE;
            }
            else {
                Log.Line(LogLevels.Error, "TattileCamera.Connect", "Protocol not supported yet or invalid protocol");
                throw new CameraException("Protocol not supported yet or invalid protocol");
            }

            res = TattileTagFilterSvc.SetMode(m_Camera_handle, (int)RxProtocol, port);
            if (res != 0)
                throw new CameraException("TAG_SetMode return " + ((TAGFILTER_ERROR_CODE)res).ToString());

            if (!alertThread.IsAlive)
                alertThread.Start();
            if (!acqThread.IsAlive)
                acqThread.Start();

            try {
                AddConnection(cameraInfoDict[cameraIdentity].NicAddress, cameraInfoDict[cameraIdentity].IpAddress);
            }
            catch {
                throw;
            }

            Connected = true;
        }

        /// <summary>
        /// Disconnect camera
        /// </summary>
        public void Disconnect() {

            Connected = false;
            StopGrab();
            //while (wait4kill) 
            Thread.Sleep(500);
            int res = 0;
            if (m_Camera_handle != 0) {
                Log.Line(LogLevels.Pass, "TattileCameraBase.Disconnect", "Disconnecting from Tattile " + CameraType + " camera " + IP4Address);
                res = TattileTagFilterSvc.ResetDeviceQueue(m_Camera_handle, 1);
                if (res != 0)
                    throw new CameraException("TAG_ResetDeviceQueue return " + ((TAGFILTER_ERROR_CODE)res).ToString());

                res = TattileTagFilterSvc.DisconnectDevice(ref m_Camera_handle);
                if (res != 0)
                    throw new CameraException("TAG_DisconnectDevice return " + ((TAGFILTER_ERROR_CODE)res).ToString());

                if (m_Eth_port_handle != 0) {
                    res = TattileTagFilterSvc.DisconnectAdapter(ref m_Eth_port_handle);
                    if (res != 0)
                        throw new CameraException("TAG_DisconnectAdapter return " + ((TAGFILTER_ERROR_CODE)res).ToString());
                }
            }
            m_Camera_handle = 0;
            m_Eth_port_handle = 0;
            try {
                if (cameraInfoDict.Count>0)
                    RemoveConnection(cameraInfoDict[cameraIdentity].NicAddress, cameraInfoDict[cameraIdentity].IpAddress);
            }
            catch {
                throw;
            }
        }

        /// <summary>
        /// Get stream of images from camera
        /// </summary>
        public void Grab() {

            if (Initialized && Connected) {
                _recursiveApplyFunc(() => { return TattileInterfaceSvc.TI_ResultImageDirectX(IdTattile, (int)TI_DirectxResultAction.DIRECTX_RESULT_IMAGE_START, port); });
                //InputBuffer.Clear();
                grabbing = true;
                grabEv.Set();
            }
            else
                throw new CameraException("Camera not connected");
        }

        /// <summary>
        /// Stops stream of images from camera
        /// </summary>
        public void StopGrab() {

            grabEv.Reset();
            grabbing = false;
            try {
                _recursiveApplyFunc(() => { return TattileInterfaceSvc.TI_ResultImageDirectX(IdTattile, (int)TI_DirectxResultAction.DIRECTX_RESULT_IMAGE_STOP, port); });
            }
            catch {
            }
        }

        /// <summary>
        /// Get single image from camera
        /// </summary>
        public Bitmap Snap() {

            Bitmap bmp = null;
            Image<Rgb, byte> cvImg;
            if (Initialized && Connected) {
                _recursiveApplyFunc(() => { return TattileInterfaceSvc.TI_ResultImageDirectX(IdTattile, (int)TI_DirectxResultAction.DIRECTX_RESULT_IMAGE_START, port); });
                //InputBuffer.Clear();
                ImageSnapped.Reset();
                snapEv.Set();
                if ((ImageSnapped.WaitOne(5000) == true) && imgToShow != null
                    /*(InputBuffer.Count > 0)*/) {
                    //if (grabbing)
                    //    bmp = InputBuffer.TryPeek(out cvImg) ? cvImg.ToBitmap() : null;
                    //else
                    //    bmp = InputBuffer.TryDequeue(out cvImg) ? cvImg.ToBitmap() : null;
                    bmp = imgToShow.ToBitmap();

                }
                //RecursiveApplyFunc(stopStreamTattile, ref _params, 3, 15);
            }
            else
                throw new CameraException("Camera not connected");
            if (bmp != null)
                return (Bitmap)bmp.Clone();
            return null;
        }


        protected CameraClipMode cameraClipMode;
        //metto da parte il clipMode dato dalla ricetta
        protected CameraClipMode cameraStartUpClipMode;

        public override void SetClipMode(CameraClipMode clipMode) {
            try {
                if (clipMode == CameraClipMode.None) {
                    //aplica il settaggio originale della camera
                    clipMode = cameraStartUpClipMode;
                }
                if (clipMode != cameraClipMode) {
                    //pier: da scommentare quando tattile ha risolto l'hardware reset
                    AcquisitionParameterCollection apc = GetAcquisitionParameters();
                    if (clipMode == CameraClipMode.Full)
                        apc["aoiEnable"].Value = 0.ToString();
                    if (clipMode == CameraClipMode.Custom)
                        apc["aoiEnable"].Value = 1.ToString();
                    SetAcquisitionParameters(apc);
                    _recursiveApplyFunc(() => { return TattileInterfaceSvc.TI_CommitChanges(IdTattile); });
                    HardReset();
                }
                cameraClipMode = clipMode;
            }
            catch {
                throw;
            }
        }

        protected CameraWorkingMode cameraWorkingMode;

        public override void SetWorkingMode(CameraWorkingMode workingMode) {
            //CameraWorkingMode.
            //object[] _params = new object[] { IdTattile, !fullframe };
            //RecursiveApplyFunc(StartLiveTattile, ref _params);
            //if (cameraWorkingMode != workingMode)
            //    SoftReset();
            cameraWorkingMode = GetWorkingMode();
            // DA TESTARE
            //il set working mode fa solo il setworkingmode
            //setCamera(workingMode, cameraClipMode);
            try {

                //quindi manda la camera nel working mode desiderato
                if (workingMode != cameraWorkingMode) {
                    if (workingMode == CameraWorkingMode.ExternalSource)
                    //GR: cambiato perchè occorre sistemare lo startRun
                    //serve lo stopLive in corrispondenza ad uno startLive
                    //startRun();
                    {
                        if (cameraWorkingMode != CameraWorkingMode.Timed)
                            startLive(cameraClipMode == CameraClipMode.Full);

                        stopLive();
                    }
                    else if (workingMode == CameraWorkingMode.Timed)
                        //GR attenzione se voglio il live full screen devo controllare cosi
                        //è da sistemare la gestione live che si vuole ottenere
                        startLive(cameraClipMode == CameraClipMode.Full);
                }
                cameraWorkingMode = workingMode;
            }
            catch {
                throw;
            }

        }

        public override CameraWorkingMode GetWorkingMode() {
            Int32 status = -1;
            Int32 programStatus = -1;
            DateTime now = DateTime.Now;
            if ((now - getCameraStatusDateTime).TotalMilliseconds > 200F) {
                try {
                    object[] _params = new object[] { IdTattile, status };
                    RecursiveApplyFunc(getStatusTattile, ref _params, 3, 15);
                    status = (int)_params[1];
                    RecursiveApplyFunc(getProgramStatusTattile, ref _params, 3, 15);
                    programStatus = (int)_params[1];
                }
                catch {
                    status = -1;
                    programStatus = -1;
                }
                finally {
                    prevCameraStatus = status;
                    prevCameraProgramStatus = programStatus;
                    getCameraStatusDateTime = now;
                }
            }
            else {
                status = prevCameraStatus;
                programStatus = prevCameraProgramStatus;
            }
            return MapWorkingMode(status, programStatus);
        }


        //static TattileTools assistente = new TattileTools();
        //public override AssistantControl GetAssistant() {
        //    return assistente;
        //}

        public override string DownloadImages(string rootPath, UserLevelEnum userLvl) {

            if (!Directory.Exists(rootPath))
                throw new CameraException("Invalid path: \"" + rootPath + "\"");
            string outputDir = rootPath + "/" + IP4Address;
            try {
                string datetimeFilename = Utilities.DateTimeString();
                if (!System.IO.Directory.Exists(outputDir))
                    System.IO.Directory.CreateDirectory(outputDir);
                outputDir += "/" + datetimeFilename;
                if (!System.IO.Directory.Exists(outputDir))
                    System.IO.Directory.CreateDirectory(outputDir);
            }
            catch {
                throw;
            }

            OnPreDownloadImages(EventArgs.Empty);
            Log.Line(LogLevels.Pass, "TattileCamera.DownloadImages", "Starting images download from " + IP4Address);
            int camSizeX = cameraInfoDict[cameraIdentity].widthImage;
            int camSizeY = cameraInfoDict[cameraIdentity].heightImage;
            int bufferMaxSize = camSizeX * camSizeY * 2;
            byte[] buffer = new byte[bufferMaxSize];
            int bufferSize = 0;
            //int bufType = (int)TBufSel.BufferType;
            //int iBufBegin = 0;
            //int iBufEnd = BufTypeStringValue.Length;
            //if (!TBufSel.ALL_BUFFERS) {
            //    iBufBegin = bufType;
            //    iBufEnd = bufType + 1;
            //}
            int[] bufsToDownload = new int[] { (int)TI_BufferType.BUFFER_ACQUISITION, (int)TI_BufferType.BUFFER_RESULTS };
            string[] bufsTypeStringValue = new string[] { "Acquisition", "Results" };
            //if (userLvl > UserLevelEnum.Operator) {
            //    bufsToDownload = new int[] { (int)TI_BufferType.BUFFER_ACQUISITION, (int)TI_BufferType.BUFFER_MENISCUS, (int)TI_BufferType.BUFFER_RESULTS, };
            //    bufsTypeStringValue = new string[] { "Acquisition", "Meniscus", "Results" };
            //}
            List<Bitmap> downloadedImages = new List<Bitmap>();
            unsafe {
                for (int ibt = 0; ibt < bufsToDownload.Length; ibt++) {
                    Log.Line(LogLevels.Pass, "TattileCameraBase.DownloadImages", "Buffer type: " + bufsTypeStringValue[ibt]);
                    int id = 0;
                    int error = 0;
                    bool download = true;
                    int counter = 0;
                    while (download) {
                        object[] _params = new object[] { IdTattile, bufsToDownload[ibt], id, bufferMaxSize, buffer, bufferSize };
                        try {
                            RecursiveApplyFunc(getImageBuffer, ref _params, 10, 15);
                            buffer = (byte[])_params[4];
                            bufferSize = (int)_params[5];
                        }
                        catch (CameraException ex) {
                            if (ex.ErrorCode == -1) {
                                download = false;
                                System.Threading.Thread.Sleep(15);
                                continue;
                            }
                            else {
                                Log.Line(LogLevels.Warning, "TattileCameraBase.DownloadImages", "Buffer type: " + bufsTypeStringValue[ibt] + " id: {0} " + ex.Message, id);
                                throw new CameraException("Images download failed");
                            }
                        }
                        Bitmap bmp;
                        IntPtr ptrBuffer;
                        fixed (void* ptr = &buffer[40]) {
                            ptrBuffer = new IntPtr(ptr);
                            if (CameraType == "M9")
                                bmp = new Bitmap(camSizeX, camSizeY, camSizeX, System.Drawing.Imaging.PixelFormat.Format8bppIndexed, ptrBuffer);
                            else
                                throw new CameraException("Images download not supported yet for camera other than M9");
                            downloadedImages.Add(bmp);
                            impostaPaletteAntares(bmp);
                            string saveFilename = outputDir + "/";
                            switch (bufsTypeStringValue[ibt]) {
                                case "Acquisition":
                                    saveFilename += counter.ToString("d2");
                                    break;
                                case "Meniscus":
                                    saveFilename += "Meniscus";
                                    if (!System.IO.Directory.Exists(saveFilename))
                                        System.IO.Directory.CreateDirectory(saveFilename);
                                    saveFilename += "/" + counter.ToString("d2");
                                    break;
                                case "Results":
                                    saveFilename += "RES";
                                    download = false;
                                    break;
                                default:
                                    throw new CameraException("Save buffer type not implemented yet");
                            }
                            saveFilename += ".bmp";
                            bmp.Save(saveFilename, ImageFormat.Bmp);
                            Log.Line(LogLevels.Pass, "TattileCameraBase.DownloadImages", "Image saved: \"" + saveFilename + "\"");
                            counter++;
                        }
                        id++;
                    }
                }
            }
            OnPostDownloadImages(new BitmapListEventArgs(downloadedImages));
            Log.Line(LogLevels.Pass, "TattileCameraBase.DownloadImages", "Download finished");
            return outputDir;
        }

        public override string UploadImages(string rootPath, UserLevelEnum userLvl) {

            return rootPath;
            //if (!Directory.Exists(rootPath))
            //    throw new CameraException("Invalid path: \"" + rootPath + "\"");

            //OnPreUploadImages(EventArgs.Empty);
            //Log.Line(LogLevels.Pass, "TattileCameraBase.UploadImages", "Starting images upload to " + IP4Address);
            ////int camSizeX = cameraInfoDict[cameraIdentity].widthImage;
            ////int camSizeY = cameraInfoDict[cameraIdentity].heightImage;
            ////int bufferMaxSize = camSizeX * camSizeY * 2;
            ////byte[] buffer = new byte[bufferMaxSize];
            ////int bufferSize = 0;
            //int[] bufsToUpload = new int[] { (int)TI_BufferType.BUFFER_ACQUISITION };
            //string[] bufsTypeStringValue = new string[] { "Acquisition" };
            ////List<Bitmap> downloadedImages = new List<Bitmap>();
            //unsafe {
            //    for (int ibt = 0; ibt < bufsToUpload.Length; ibt++) {
            //        Log.Line(LogLevels.Pass, "TattileCameraBase.UploadImages", "Buffer type: " + bufsTypeStringValue[ibt]);
            //        try {
            //            int count = 0;
            //            foreach (String filename in Directory.GetFiles(rootPath)) {
            //                if (filename.ToLower().EndsWith(".bmp") && !filename.Contains("RES")) {
            //                    int bufferNumber = System.Convert.ToInt32(count);
            //                    _recursiveApplyFunc(() => TattileInterfaceSvc.TI_BufferSet(IdTattile, bufsToUpload[ibt], count, filename));

            //                    if (ret == 0)
            //                        resultLabel.Text = "Image uploaded successfully!";
            //                    else
            //                        resultLabel.Text = "Image upload error: " + ret;

            //                    count++;
            //                    if (count >= 25)
            //                        break;
            //                }
            //            }
            //        }
            //        catch (Exception ex) {
            //            Log.Line(LogLevels.Warning, "TattileCameraBase.UploadImages", "Buffer type: " + bufsTypeStringValue[ibt] + " id: {0} " + ex.Message, id);
            //            throw new CameraException("Images download failed");
            //        }








            //        int id = 0;
            //        int error = 0;
            //        bool download = true;
            //        int counter = 0;
            //        while (download) {
            //            object[] _params = new object[] { IdTattile, bufsToUpload[ibt], id, bufferMaxSize, buffer, bufferSize };
            //            try {
            //                RecursiveApplyFunc(setImageBuffer, ref _params, 10, 15);
            //                buffer = (byte[])_params[4];
            //                bufferSize = (int)_params[5];
            //            }
            //            catch (CameraException ex) {
            //                if (ex.ErrorCode == -1) {
            //                    download = false;
            //                    System.Threading.Thread.Sleep(15);
            //                    continue;
            //                }
            //                else {
            //                    Log.Line(LogLevels.Warning, "TattileCamera.DownloadImages", "Buffer type: " + bufsTypeStringValue[ibt] + " id: {0} " + ex.Message, id);
            //                    throw new CameraException("Images download failed");
            //                }
            //            }
            //            Bitmap bmp;
            //            IntPtr ptrBuffer;
            //            fixed (void* ptr = &buffer[40]) {
            //                ptrBuffer = new IntPtr(ptr);
            //                if (CameraType == "M9")
            //                    bmp = new Bitmap(camSizeX, camSizeY, camSizeX, System.Drawing.Imaging.PixelFormat.Format8bppIndexed, ptrBuffer);
            //                else
            //                    throw new CameraException("Images download not supported yet for camera other than M9");
            //                downloadedImages.Add(bmp);
            //                impostaPaletteAntares(bmp);
            //                string saveFilename = outputDir + "/";
            //                switch (bufsTypeStringValue[ibt]) {
            //                    case "Acquisition":
            //                        saveFilename += counter.ToString("d2");
            //                        break;
            //                    case "Meniscus":
            //                        saveFilename += "Meniscus";
            //                        if (!System.IO.Directory.Exists(saveFilename))
            //                            System.IO.Directory.CreateDirectory(saveFilename);
            //                        saveFilename += "/" + counter.ToString("d2");
            //                        break;
            //                    case "Results":
            //                        saveFilename += "RES";
            //                        download = false;
            //                        break;
            //                    default:
            //                        throw new CameraException("Save buffer type not implemented yet");
            //                }
            //                saveFilename += ".bmp";
            //                bmp.Save(saveFilename, ImageFormat.Bmp);
            //                Log.Line(LogLevels.Pass, "TattileCameraBase.DownloadImages", "Image saved: \"" + saveFilename + "\"");
            //                counter++;
            //            }
            //            id++;
            //        }
            //    }
            //}
            //OnPostDownloadImages(new BitmapListEventArgs(downloadedImages));
            //Log.Line(LogLevels.Pass, "TattileCameraBase.DownloadImages", "Download finished");
            //return outputDir;
        }

        public override CameraClipMode GetClipMode() {
            AcquisitionParameterCollection apc = GetAcquisitionParameters();
            if (apc == null)
                throw new CameraException("Invalid acquisition parameter collection");
            int aoiEn = Convert.ToInt32(apc["aoiEnable"].Value);
            if (aoiEn == 0)
                cameraClipMode = CameraClipMode.Full;
            else
                cameraClipMode = CameraClipMode.Custom;
            return cameraClipMode;
        }

        protected abstract void startRun();

        //protected void setCamera(CameraWorkingMode workingMode, CameraClipMode clipMode) {
        //    //se cambio modalità clip (fullframe o AOI) applica parametro e riavvia la camera
        //    try {
        //        if (clipMode != cameraClipMode) {
        //            //pier: da scommentare quando tattile ha risolto l'hardware reset
        //            //AcquisitionParameterCollection apc = GetAcquisitionParameters();
        //            //if (clipMode == CameraClipMode.None)
        //            //    apc["aoiEnable"].Value = 0.ToString();
        //            //if (clipMode == CameraClipMode.Custom)
        //            //    apc["aoiEnable"].Value = 1.ToString();
        //            //SetAcquisitionParameters(apc);
        //            //HardReset();
        //        }
        //        //quindi manda la camera nel working mode desiderato
        //        if (workingMode != cameraWorkingMode) {
        //            if (workingMode == CameraWorkingMode.ExternalSource)
        //                //GR: cambiato perchè occorre sistemare lo startRun
        //                //serve lo stopLive in corrispondenza ad uno startLive
        //                //startRun();
        //            {
        //                startLive(clipMode == CameraClipMode.None);
        //                StopLive();
        //            }
        //            else if (workingMode == CameraWorkingMode.Timed)
        //                //GR attenzione se voglio il live full screen devo controllare cosi
        //                //è da sistemare la gestione live che si vuole ottenere
        //                startLive(clipMode == CameraClipMode.None);
        //        }
        //    }
        //    catch {
        //        throw;
        //    }
        //}
        //questa funzione non dovrebbe piu servire
        //protected void setCamera(CameraWorkingMode workingMode, CameraClipMode clipMode){}

        void AlertThread() {
            while (true) {
                int res = WaitHandle.WaitAny(alertEvs);
                AlertEvents ret = (AlertEvents)Enum.Parse(typeof(AlertEvents), res.ToString());
                if (ret == AlertEvents.Kill) break; //kill ev
                if (ret == AlertEvents.Ready) {
                    //Image<Rgb, byte> cvImg;
                    //if (InputBuffer.TryDequeue(out cvImg))
                    bool isReject = (rejectionCause == 0) ? false : true;
                    OnImageAvailable(this, new ImageAvailableEventArgs(imgToShow, null, isReject));
                }
            }
        }

        //bool wait4kill = false;
        unsafe void AcquisitionThread() {
            if (CameraType == "M9") {
                byteArray = new byte[sizeof(THeaderObjectFixed)];
            }
            else if (CameraType == "M12") {
                byteArray = new byte[sizeof(TLeaderImage)];
            }
            else {
                Log.Line(LogLevels.Error, "TattileCameraBase.AcquisitionThread", "Invalid camera type");
                throw new CameraException("Invalid camera type");
            }

            int res = TattileTagFilterSvc.ResetDeviceQueue(m_Camera_handle, 0);
            if (res != 0)
                throw new CameraException("TAG_ResetDeviceQueue return " + ((TAGFILTER_ERROR_CODE)res).ToString());

            while (true) {
                //wait4kill = false;
                res = WaitHandle.WaitAny(acqEvs);
                AcqEvents ret = (AcqEvents)Enum.Parse(typeof(AcqEvents), res.ToString());
                if (ret == AcqEvents.Kill) break; //kill ev
                //wait4kill = true;
                Image<Rgb, byte> newImage = null;
                if (RxProtocol == ImageProtocol.IMAGE_PROTOCOL_TOJECT)
                    TOJECT_AcqProtocol(out newImage);
                else if (RxProtocol == ImageProtocol.IMAGE_PROTOCOL_GIGE)
                    GIGE_AcqProtocol(out newImage);
                else {
                    Log.Line(LogLevels.Error, "TattileCameraBase.Connect", "Protocol not supported yet or invalid protocol");
                    throw new CameraException("Protocol not supported yet or invalid protocol");
                }
                if (newImage != null) {
                    if (ret == AcqEvents.Grab) {
                        lock (imgToShow) {
                            CvInvoke.cvCopy(newImage.Ptr, imgToShow.Ptr, IntPtr.Zero);
                            //imgToShow = newImage.Copy();
                        }
                        readyEv.Set();
                    }
                    if (ret == AcqEvents.Snap) {
                        //InputBuffer.Enqueue(newImage);
                        lock (imgToShow) {
                            CvInvoke.cvCopy(newImage.Ptr, imgToShow.Ptr, IntPtr.Zero);
                        }
                        //imgToShow = newImage.Copy();
                        snapEv.Reset();
                        //ImageSnapped.Set();
                    }
                    ImageSnapped.Set();
                    //OnImageAvailable(this, new ImageAvailableEventArgs(imgToShow, null));
                    //Debug.WriteLine("NEW IMAGE");
                }
                //Debug.WriteLine("NO IMAGE");
                //Thread.Sleep(50);
            }
        }

        protected virtual void OnImageAvailable(object sender, ImageAvailableEventArgs e) {

            EventHandler<ImageAvailableEventArgs> handler = _imageAvailable;
            CurrentImage = e.Image;
            CurrentIsReject = e.IsReject;
            if (inspectionBuffer != null)
                inspectionBuffer.ResetAdditionalIndex();
            if (handler != null)
                handler(sender, e);
        }

        protected virtual void OnAlternativeImageAvailable(object sender, ImageAvailableEventArgs e) {

            EventHandler<ImageAvailableEventArgs> handler = _alternativeImageAvailable;
            AlternativeImage = e.Image;
            if (handler != null)
                handler(sender, e);
        }

        public void SetPrevImage() {

            if (inspectionBuffer != null && inspectionBuffer.Count > 0) {
                RawInspectionData rid = inspectionBuffer.GetPrevData();
                if (rid == null) return;
                Image<Rgb, byte> newImg = new Image<Rgb, byte>(rid.ImageWidth, rid.ImageHeight);
                newImg.Bytes = rid.Image;
                OnAlternativeImageAvailable(this, new ImageAvailableEventArgs(newImg, null, rid.Result.IsReject));
            }
        }

        public void SetMainImage() {

            OnImageAvailable(this, new ImageAvailableEventArgs(CurrentImage, CurrentThumbnail, CurrentIsReject));
        }

        unsafe void TOJECT_AcqProtocol(out Image<Rgb, byte> incomingImage) {
            int res = 51;
            long Code = 0;
            long Subcode = 0;
            uint intImageBPP = 0, imgFrameID = 0;
            bool newImage = false;
            //uint intTemp = 0;
            TAGLostImageInfos stcInfo = new TAGLostImageInfos();
            incomingImage = null;
            Exception exx = null;

            //uint delta = 0;

            if (m_Camera_handle != 0) {
                try {
                    res = TattileTagFilterSvc.ImageGet(m_Camera_handle, ref pImageBufferPtr, ref stcInfo, 1000);
                    //if (res != 0) {
                    //    Log.Line(LogLevels.Debug, "TattileCameraBase.TOJECT_AcqProtocol", "TAG_ImageGet returns {0}", res);
                    //}
                }
                catch (Exception ex) {
                    Log.Line(LogLevels.Error, "TattileCameraBase.TOJECT_AcqProtocol", "Error: " + ex.Message);
                    throw;
                }
            }

            if (res == 0 && pImageBufferPtr != 0) {

                try {
                    //Array.Resize<byte>(byteArray, sizeof(THeaderObjectFixed));
                    IntPtr pImageBufferIntPtr = new IntPtr(pImageBufferPtr);
                    Marshal.Copy(pImageBufferIntPtr, byteArray, 0, sizeof(THeaderObjectFixed));
                    Code = (long)byteArray[2] | ((long)byteArray[3] << 8);
                    Subcode = (long)byteArray[0] | ((long)byteArray[1] << 8);
                    //Log.Line(LogLevels.Debug, "TattileCameraBase.TOJECT_AcqProtocol", "Code arrived: {0}", Code);
                    switch (Code) {
                        case 100:
                        case 10:
                            switch (Subcode) {
                                case 1320:
                                    intImageBPP = 8;
                                    break;
                                case 11275:
                                case 11280:
                                    intImageBPP = 16;
                                    break;
                                case 1409:
                                    intImageBPP = 24;
                                    break;
                                case 1291280:
                                    intImageBPP = 24;
                                    break;
                                default:
                                    intImageBPP = 8;
                                    break;
                            }
                            imgFrameID = ((uint)(byteArray[8]) + (uint)(byteArray[9] * 256) + (uint)(byteArray[10] * 256 * 256) + (uint)(byteArray[11] * (256 * 256 * 256)));
                            long payloadX = (long)byteArray[4] | ((long)byteArray[5] << 8);
                            long payloadY = (long)byteArray[6] | ((long)byteArray[7] << 8);
                            if ((int)payloadX != width || (int)payloadY != height || intImageBPP != currBpp || recImage == null) {
                                if (!AllocateImages((int)payloadX, (int)payloadY, (int)intImageBPP)) {
                                    res = TattileTagFilterSvc.ImageUnlock(m_Camera_handle, pImageBufferPtr);
                                    return;
                                }
                            }
                            newImage = true;
                            width = (int)payloadX;
                            height = (int)payloadY;
                            Marshal.Copy(pImageBufferIntPtr + sizeof(THeaderObjectFixed), imgByteArray, 0, imgByteArray.Length);
                            //Log.Line(LogLevels.Debug, "TattileCameraBase.TOJECT_AcqProtocol", "Image arrived: {0}", imgFrameID);
                            break;
                        case 144:
                            imgFrameID = ((uint)(byteArray[8]) + (uint)(byteArray[9] * 256) + (uint)(byteArray[10] * 256 * 256) + (uint)(byteArray[11] * (256 * 256 * 256)));
                            uint resLength = ((uint)(byteArray[4]) + (uint)(byteArray[5] * 256) + (uint)(byteArray[6] * 256 * 256) + (uint)(byteArray[7] * (256 * 256 * 256)));
                            //questo IF è tricky...in attesa di documento Tattile su gestione risultati
                            if (resLength > 0 && resLength > 96) {
                                byte[] results = new byte[resLength];
                                Marshal.Copy(pImageBufferIntPtr + sizeof(THeaderObjectFixed), results, 0, (int)resLength);
                                //pier: il bit GOOD/REJECT l'ho estratto facendo reverse engineering...DA CONTROLLARE!
                                rejectionCause = ((uint)(results[92]) + (uint)(results[93] * 256) + (uint)(results[94] * 256 * 256) + (uint)(results[95] * (256 * 256 * 256)));
                                //Debug.WriteLine(Encoding.ASCII.GetString(results, 0, results.Length));
                                //for (int i = 0; i < results.Length; i++)
                                //    Debug.Write(results[i]);
                                //Debug.WriteLine("\n-------------------------------------------------");
                            }
                            break;
                    }
                }
                catch (Exception ex) {
                    exx = ex;
                }
            }
            res = TattileTagFilterSvc.ImageUnlock(m_Camera_handle, pImageBufferPtr);
            if (res != 0) {
                imageUnlock++;
            }
            if (exx != null) {
                Log.Line(LogLevels.Error, "TattileCameraBase.TOJECT_AcqProtocol", "Error: " + exx.Message);
                throw exx;
            }
            if (newImage) {
                //if (intImageBPP != 8) {
                //    Log.Line(LogLevels.Error, "TattileCameraBase.TOJECT_AcqProtocol", "TO OBJECT TATTILE BPP ERROR ! imgFrameID = {0}", imgFrameID);
                //}
                try {
                    incomingImage = FromRawToCvImage(imgByteArray, width, height, (int)intImageBPP, true);
                    ChangeImageInfo(width, height, 24);
                    if (inspectionBuffer != null) {
                        bool isReject = (rejectionCause == 0) ? false : true;
                        InspectionResults ir = new InspectionResults(IdCamera, 0, imgFrameID, true, isReject, (int)rejectionCause, null);
                        if (isToBufferResult(ImagesDumpConditions.Reject, ir))
                            inspectionBuffer.AddResultData(imgFrameID, ir);
                        if (inspectionBuffer.Contains(imgFrameID)) {
                            inspectionBuffer.AddImageData(imgFrameID, incomingImage.Bytes, incomingImage.Width, incomingImage.Height, 24);
                        }
                    }

                }
                catch {
                    Log.Line(LogLevels.Error, "TattileCameraBase.TOJECT_AcqProtocol", "BACKWALL: Image Buffer Ptr cpy");
                }
                //setta l'evento di fine acquisizione immagine
                AcquisitionComplete.Set();
            }
            pImageBufferPtr = 0;
        }

        unsafe void GIGE_AcqProtocol(out Image<Rgb, byte> incomingImage) {
            int res = 51;
            long Code = 0;
            long Subcode = 0;
            uint intImageBPP = 0;
            bool newImage = false;
            uint intTemp = 0;
            TAGLostImageInfos stcInfo = new TAGLostImageInfos();
            incomingImage = null;
            Exception exx = null;

            if (m_Camera_handle != 0) {
                try {
                    res = TattileTagFilterSvc.ImageGet(m_Camera_handle, ref pImageBufferPtr, ref stcInfo, 1000);
                    //if (res != 0) {
                    //    Log.Line(LogLevels.Debug, "TattileCameraBase.GIGE_AcqProtocol", "TAG_ImageGet returns {0}", res);
                    //}
                }
                catch (Exception ex) {
                    Log.Line(LogLevels.Error, "TattileCameraBase.GIGE_AcqProtocol", "Error: " + ex.Message);
                    throw;
                }
            }

            uint imgVialID = 0;
            if (res == 0 && pImageBufferPtr != 0) {
                try {
                    IntPtr pImageBufferIntPtr = new IntPtr(pImageBufferPtr);
                    Marshal.Copy(pImageBufferIntPtr, byteArray, 0, sizeof(TLeaderImage));
                    Code = (long)byteArray[11] | ((long)byteArray[10] << 8);
                    Subcode = (long)byteArray[0] | ((long)byteArray[1] << 8);
                    imgVialID = (uint)byteArray[3] | ((uint)byteArray[2] << 8);
                    //Log.Line(LogLevels.Debug, "TattileCameraBase.GIGE_AcqProtocol", "Code arrived: {0}", Code);
                    intTemp = ((uint)byteArray[23] | ((uint)byteArray[22] << 8) | ((uint)byteArray[21] << 16) + ((uint)byteArray[20] << 24));

                    switch (Code) {
                        case 0x8000:
                        case 0x1:
                            long payloadX = ((long)byteArray[27] | ((long)byteArray[26] << 8) | ((long)byteArray[25] << 16) + ((long)byteArray[24] << 24));
                            long payloadY = ((long)byteArray[31] | ((long)byteArray[30] << 8) | ((long)byteArray[29] << 16) + ((long)byteArray[28] << 24));
                            switch (intTemp) {
                                case (uint)ImageBppCode.Image_8Bit_BW:
                                    intImageBPP = 8;
                                    break;
                                case (uint)ImageBppCode.Image_16Bit_RGB565:
                                    intImageBPP = 16;
                                    break;
                                case (uint)ImageBppCode.Image_24Bit_RGB888:
                                    intImageBPP = 24;
                                    break;
                            }
                            //Log.Line(LogLevels.Debug, "TattileCameraBase.GIGE_AcqProtocol", "Image arrived");
                            try {
                                if ((int)payloadX != width || (int)payloadY != height || intImageBPP != currBpp || recImage == null) {
                                    if (!AllocateImages((int)payloadX, (int)payloadY, (int)intImageBPP)) {
                                        res = TattileTagFilterSvc.ImageUnlock(m_Camera_handle, pImageBufferPtr);
                                        return;
                                    }
                                }
                                newImage = true;
                                width = (int)payloadX;
                                height = (int)payloadY;
                                Marshal.Copy(pImageBufferIntPtr + sizeof(TLeaderImage), imgByteArray, 0, imgByteArray.Length);
                            }
                            catch {
                                Log.Line(LogLevels.Error, "TattileCameraBase.GIGE_AcqProtocol", "BACKWALL: Image Buffer Ptr cpy");
                            }
                            break;
                        case 0x8001:
                            //Log.Line(LogLevels.Debug, "TattileCameraBase.GIGE_AcqProtocol", "Results arrived");

                            //1)      => fisso ad 1 [LONG 32bit]
                            //2)      => torretta (sperimentale, un long che indica la torretta) [LONG 32bit]
                            //3)      => Vial ID [LONG 32bit]
                            //4)      => Risultato testa 0 [LONG 32bit]
                            //5)      => Risultato testa 1 [LONG 32bit]
                            //6)      => Risultato testa 2 [LONG 32bit]
                            //7)      => Risultato testa 3 [LONG 32bit]
                            //8)      => Tempo di analisi in ms [LONG 32bit]
                            //9)      => Numero dell’immagine [LONG 32bit]

                            //pier: il bit GOOD/REJECT l'ho estratto facendo reverse engineering...DA CONTROLLARE!
                            const int MAX_RESULT_BUFFER_SIZE = 200000;
                            uint resLength = ((uint)(byteArray[23]) + (uint)(byteArray[22] * 256) + (uint)(byteArray[21] * 256 * 256) + (uint)(byteArray[20] * (256 * 256 * 256)));
                            if (resLength > 0 && resLength < MAX_RESULT_BUFFER_SIZE) {
                                byte[] results = new byte[resLength];
                                Marshal.Copy(pImageBufferIntPtr + sizeof(TLeaderImage), results, 0, (int)resLength);
                                //imgVialID = ((uint)(results[92]) + (uint)(results[93] * 256) + (uint)(results[94] * 256 * 256) + (uint)(results[95] * (256 * 256 * 256)));
                                int[] resHeads = new int[4];
                                bool isReject = false;
                                for (int i = 0; i < 4; i++) {
                                    resHeads[i] = ((int)(results[104 + i * 32]) + (int)(results[105 + i * 32] * 256) + (int)(results[106 + i * 32] * 256 * 256) + (int)(results[107 + i * 32] * (256 * 256 * 256)));
                                    if (resHeads[i] != 0) isReject = true;
                                }
                                rejectionCause = (isReject == true) ? 1u : 0u;// ((uint)(byteArray[8]) + (uint)(byteArray[9] * 256) + (uint)(byteArray[10] * 256 * 256) + (uint)(byteArray[11] * (256 * 256 * 256)));
                                //Debug.WriteLine(Encoding.ASCII.GetString(results, 0, results.Length));
                                //for (int i = 0; i < results.Length; i++)
                                //    Debug.Write(results[i]);
                                //Debug.WriteLine("\n-------------------------------------------------");
                                //memcpy(&(*pResultBuffer)[0], &byteArray[8], 4 * sizeof(BYTE));  /// copio il frame ID nel pacchetto risultati
                                //memcpy(&(*pResultBuffer)[4], pImageBufferPtr + sizeof(THeaderObjectFixed), __min(intTemp, MAX_RESULT_BUFFER_SIZE) * sizeof(BYTE));
                            }
                            //pier: da scommentare e prendere i risultati quando e se serviranno
                            /*try
                            {
                                memcpy(*pResultBuffer,pImageBufferPtr+sizeof(TLeaderImage),__min(intTemp, MAX_RESULT_BUFFER_SIZE) * sizeof(BYTE));
                            }
                            catch (...)
                            {
                                MessageBox(NULL, L"GIGE: Results Buffer Ptr cpy", (LPCWSTR)L"Exception GIGE Protocol", MB_ICONEXCLAMATION | MB_OK );
                            }*/

                            ResultAcquisitionComplete.Set();
                            break;
                    }
                }
                catch (Exception ex) {
                    exx = ex;
                }
            }
            res = TattileTagFilterSvc.ImageUnlock(m_Camera_handle, pImageBufferPtr);
            if (res != 0) {
                imageUnlock++;
            }
            if (exx != null) {
                Log.Line(LogLevels.Error, "TattileCameraBase.GIGE_AcqProtocol", "Error: " + exx.Message);
                throw exx;
            }
            if (newImage) {
                //if (intImageBPP != 8) {
                //    Log.Line(LogLevels.Error, "TattileCameraBase.TOJECT_AcqProtocol", "TO OBJECT TATTILE BPP ERROR !");
                //}
                try {
                    //incomingImage = FromRawToCvImage(pImageBufferIntPtr + sizeof(THeaderObjectFixed), (int)payloadX, (int)payloadY, 8);
                    incomingImage = FromRawToCvImage(imgByteArray, width, height, (int)intImageBPP, true);
                    //incomingImage.Save(@"D:\ReverseEngTattileResM12\M12.bmp");
                    ChangeImageInfo(width, height, 24);
                    if (inspectionBuffer != null) {
                        bool isReject = (rejectionCause == 0) ? false : true;
                        InspectionResults ir = new InspectionResults(IdCamera, 0, imgVialID, true, isReject, (int)rejectionCause, null);
                        if (isToBufferResult(ImagesDumpConditions.Reject, ir))
                            inspectionBuffer.AddResultData(imgVialID, ir);
                        if (inspectionBuffer.Contains(imgVialID)) {
                            inspectionBuffer.AddImageData(imgVialID, incomingImage.Bytes, incomingImage.Width, incomingImage.Height, 24);
                        }
                    }
                }
                catch {
                    Log.Line(LogLevels.Error, "TattileCameraBase.GIGE_AcqProtocol", "BACKWALL: Image Buffer Ptr cpy");
                }
                //setta l'evento di fine acquisizione immagine
                AcquisitionComplete.Set();
            }
            pImageBufferPtr = 0;
        }

        bool AllocateImages(int w, int h, int bpp) {
            try {
                width = w;
                height = h;
                currBpp = bpp;
                if (recImage != null) recImage.Dispose();
                if (imgToShow != null) imgToShow.Dispose();
                if (recImageBN != null) recImageBN.Dispose();
                imgByteArray = new byte[width * height * bpp / 8];
                recImage = new Image<Rgb, byte>(width, height);
                imgToShow = new Image<Rgb, byte>(width, height);
                if (bpp == 8)
                    recImageBN = new Image<Gray, byte>(width, height);
                return true;
            }
            catch (OutOfMemoryException ex) {
                Log.Line(LogLevels.Error, "TattileCameraBase.AllocateImages", "OutOfMemoryException: " + ex.Message);
                imgByteArray = null;
                recImage = null;
                imgToShow = null;
                recImageBN = null;
            }
            return false;
        }

        void FreeImages() {
            width = 0;
            height = 0;
            if (recImage != null) {
                lock (recImage) {
                    recImage.Dispose();
                }
            }
            if (imgToShow != null) imgToShow.Dispose();
            if (recImageBN != null) recImageBN.Dispose();
            recImage = imgToShow = null;
            recImageBN = null;
        }

        //Bitmap FromRawToBMP(IntPtr sourceAddress, int width, int height, int bpp) {
        //    // Prepare required image's metadata.
        //    /*PixelFormat imagesPixelFormat = PixelFormat.Format16bppGrayScale; // Known type.

        //    Bitmap bitmap = new Bitmap(width, height, pixelFormat);
        //    Rectangle wholeBitmap = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
        //    BitmapData bitmapData = bitmap.LockBits(wholeBitmap, ImageLockMode.WriteOnly, pixelFormat);
        //    Marshal.Copy(buffer, 0, bitmapData.Scan0, buffer.Length);
        //    bitmap.UnlockBits(bitmapData);
        //    return bitmap;*/
        //    //static int lastDepth = -1;
        //    //static ColorPalette Pal;
        //    PixelFormat pixFormat = PixelFormat.Format8bppIndexed;
        //    int size = width * height * bpp / 8;
        //    byte[] nums = new byte[size];

        //    /*Static bmp As Bitmap
        //    Static intW As Integer = 0
        //    Static intH As Integer = 0

        //    Dim objReturnBMP As Bitmap = Nothing

        //    Dim offset As Integer = 0
        //    Dim i As Integer
        //    Dim ptr As Long
        //    Dim lngTimer As Long = Environment.TickCount*/

        //    if (sourceAddress != IntPtr.Zero) {
        //        switch (bpp) {
        //            case 8:
        //                pixFormat = PixelFormat.Format8bppIndexed;
        //                break;
        //            case 16:
        //                pixFormat = PixelFormat.Format16bppRgb565;
        //                break;
        //            case 24:
        //                pixFormat = PixelFormat.Format24bppRgb;
        //                break;
        //            default:
        //                throw new NotImplementedException("PixelFormat not yet supported");
        //        }
        //    }
        //    Bitmap bmp = new Bitmap(width, height, width * (bpp / 8), pixFormat, sourceAddress);
        //    if (bpp == 8)
        //        impostaPaletteAntares(bmp); //pier: da ottimizzare, basta creare una volta la palette...
        //    //ATTENZIONE!!! Ricontrollare il comportamento negli altri casi di pixelformat
        //    if (pixFormat == PixelFormat.Format24bppRgb) {
        //        Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
        //        Marshal.Copy(sourceAddress, nums, 0, size);
        //        BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
        //        long ptr = bmpData.Scan0.ToInt64();
        //        int offset = 0;
        //        for (int ih = 0; ih < height; ih++) {
        //            System.Runtime.InteropServices.Marshal.Copy(nums, offset, new IntPtr(ptr), width);
        //            offset += width;
        //            ptr += width;
        //        }
        //        bmp.UnlockBits(bmpData);
        //    }
        //    return bmp;
        //}

        Image<Rgb, byte> FromRawToCvImage(byte[] srcByte, int width, int height, int bpp, bool bgr2rgb) {
            if (bpp == 8) {
                if (recImage != null) {
                    try {
                        recImageBN.Bytes = srcByte;
                        lock (recImage) {
                            CvInvoke.cvMerge(recImageBN.Ptr, recImageBN.Ptr, recImageBN.Ptr, IntPtr.Zero, recImage.Ptr);
                            CvInvoke.cvLUT(recImage.Ptr, recImage.Ptr, lutMatrixRGB.Ptr);
                        }
                    }
                    catch (OutOfMemoryException ex) {
                        Log.Line(LogLevels.Error, "TattileCameraBase.FromRawToCvImage", "OutOfMemoryException: " + ex.Message);
                    }
                }
            }
            else if (bpp == 24) {
                recImage.Bytes = srcByte;
                if (bgr2rgb)
                    CvInvoke.cvCvtColor(recImage.Ptr, recImage.Ptr, COLOR_CONVERSION.BGR2RGB);
            }
            else
                Log.Line(LogLevels.Error, "TattileCameraBase.FromRawToCvImage", "TATTILE BPP ERROR!");
            return recImage;
        }

        #region Palette
        static Color[] paletteAntares;
        static Matrix<Byte> bTable, gTable, rTable, aTable;
        static void initPaletteAntares() {

            paletteAntares = new Color[256];
            float sngOffset = 255 / 243;
            byte[] bLut = new byte[256];
            byte[] gLut = new byte[256];
            byte[] rLut = new byte[256];

            for (int i = 0; i <= 243; i++) {
                paletteAntares[i] = Color.FromArgb((int)(i * sngOffset), (int)(i * sngOffset), (int)(i * sngOffset));
                bLut[i] = (byte)(i * sngOffset);
                gLut[i] = (byte)(i * sngOffset);
                rLut[i] = (byte)(i * sngOffset);
            }

            paletteAntares[244] = Color.FromArgb(128, 0, 0);
            paletteAntares[245] = Color.FromArgb(0, 128, 0);
            paletteAntares[246] = Color.FromArgb(128, 128, 0);
            paletteAntares[247] = Color.FromArgb(0, 0, 128);
            paletteAntares[248] = Color.FromArgb(128, 0, 128);
            paletteAntares[249] = Color.FromArgb(0, 128, 128);
            paletteAntares[250] = Color.FromArgb(255, 0, 0);
            paletteAntares[251] = Color.FromArgb(0, 255, 0);
            paletteAntares[252] = Color.FromArgb(255, 255, 0);
            paletteAntares[253] = Color.FromArgb(0, 0, 255);
            paletteAntares[254] = Color.FromArgb(255, 0, 255);
            paletteAntares[255] = Color.FromArgb(0, 255, 255);

            rLut[244] = 128; gLut[244] = 0; bLut[244] = 0;
            rLut[245] = 0; gLut[245] = 128; bLut[245] = 0;
            rLut[246] = 128; gLut[246] = 128; bLut[246] = 0;
            rLut[247] = 0; gLut[247] = 0; bLut[247] = 128;
            rLut[248] = 128; gLut[248] = 0; bLut[248] = 128;
            rLut[249] = 0; gLut[249] = 128; bLut[249] = 128;
            rLut[250] = 255; gLut[250] = 0; bLut[250] = 0;
            rLut[251] = 0; gLut[251] = 255; bLut[251] = 0;
            rLut[252] = 255; gLut[252] = 255; bLut[252] = 0;
            rLut[253] = 0; gLut[253] = 0; bLut[253] = 255;
            rLut[254] = 255; gLut[254] = 0; bLut[254] = 255;
            rLut[255] = 0; gLut[255] = 255; bLut[255] = 255;
            bTable = new Matrix<byte>(bLut);
            gTable = new Matrix<byte>(gLut);
            rTable = new Matrix<byte>(rLut);
            lutMatrixRGB = new Matrix<byte>(bTable.Rows, bTable.Cols, 3);
            //lutMatrixBGR = new Matrix<byte>(bTable.Rows, bTable.Cols, 3);
            CvInvoke.cvMerge(rTable.Ptr, gTable.Ptr, bTable.Ptr, IntPtr.Zero, lutMatrixRGB);
            //CvInvoke.cvMerge(bTable.Ptr, gTable.Ptr, rTable.Ptr, IntPtr.Zero, lutMatrixBGR);
            //Bitmap bmp = new Bitmap(@"D:\IMMAGINI\ExactaEasy\Tattile_2013_11_06\Stream\Device_192.168.12.22_0000\_img_28_RESULTS.bmp");
            //impostaPaletteAntares(bmp);
            //CvToolbox.ColorPaletteToLookupTable(bmp.Palette, out bTable, out gTable, out rTable, out aTable);
        }
        #endregion
        static Matrix<Byte> lutMatrixRGB;
        //static Matrix<Byte> lutMatrixBGR;

        bool isToBufferResult(ImagesDumpConditions condition, InspectionResults e) {

            bool ris = false;
            switch (condition) {
                case ImagesDumpConditions.Reject:
                    ris = e.IsReject;
                    break;
                default:
                    ris = false;
                    break;
            }
            return ris;
        }

        static void impostaPaletteAntares(Bitmap b) {

            ColorPalette palette;
            palette = b.Palette;
            Array.Copy(paletteAntares, palette.Entries, 256);
            b.Palette = palette;
        }

        public int NodeId {
            get;
            set;
        }

        public virtual void SetStreamMode(int headNumber, CameraWorkingMode cwm) {

        }


        public int IdStation { get; set; }

        protected void startLive(bool fullframe) {

            _recursiveApplyFunc(() => { return TattileInterfaceSvc.TI_LiveStart(IdTattile, !fullframe); });

        }

        protected void stopLive() {

            _recursiveApplyFunc(() => { return TattileInterfaceSvc.TI_LiveStop(IdTattile); });
        }

        public override CameraInfo GetCameraInfo() {
            TI_CameraInfo _ci = getInfo();
            return new CameraInfo() {
                bitDepth = _ci.bitDepth,
                description = new string(_ci.description),
                gainMax = _ci.gainMax,
                gainMin = _ci.gainMin,
                headNumber = _ci.headNumber,
                heightImage = _ci.heightImage,
                IpAddress = Utilities.IPV4AddressUInt2String(_ci.ipAddress),
                model = new string(_ci.model),
                NicAddress = Utilities.IPV4AddressUInt2String(_ci.pcIfAddress),
                serialNumber = _ci.serialNumber,
                shutterMax = _ci.shutterMax,
                shutterMin = _ci.shutterMin,
                type = _ci.type,
                vendor = new string(_ci.vendor),
                widthImage = _ci.widthImage
            };

        }

        TI_CameraInfo getInfo() {
            object[] _params = new object[] { IdTattile, new TI_CameraInfo() };
            RecursiveApplyFunc(getInfoTattile, ref _params, 3, 15);
            return (TI_CameraInfo)_params[1];
        }

        public override string GetFirmwareVersion() {
            return getFirmwareVersion() + " (" + getProgramVersion() + ")";
        }

        string getFirmwareVersion() {
            string version = "";
            object[] _params = new object[] { IdTattile, version };
            RecursiveApplyFunc(getFirmwareVersionTattile, ref _params, 3, 15);
            return (string)_params[1];
        }

        string getProgramVersion() {
            string version = "";
            object[] _params = new object[] { IdTattile, version };
            RecursiveApplyFunc(getProgramVersionTattile, ref _params, 3, 15);
            return (string)_params[1];
        }

        protected static CameraInfo GetInfo(int id) {
            TI_CameraInfo _ci = new TI_CameraInfo();
            int ret = -1;
            int hitRetries = 10;
            while (ret != 0 && hitRetries > 0) {
                ret = TattileInterfaceSvc.TI_InfoGet(id, ref _ci);
                hitRetries--;
                System.Threading.Thread.Sleep(20);
            }
            if (ret != 0)
                throw new CameraException("TI_InfoGet failed with error " + ((ITF_ERROR_CODE)ret).ToString());

            return new CameraInfo() {
                bitDepth = _ci.bitDepth,
                description = new string(_ci.description),
                gainMax = _ci.gainMax,
                gainMin = _ci.gainMin,
                headNumber = _ci.headNumber,
                heightImage = _ci.heightImage,
                IpAddress = Utilities.IPV4AddressUInt2String(_ci.ipAddress),
                model = new string(_ci.model),
                NicAddress = Utilities.IPV4AddressUInt2String(_ci.pcIfAddress),
                serialNumber = _ci.serialNumber,
                shutterMax = _ci.shutterMax,
                shutterMin = _ci.shutterMin,
                type = _ci.type,
                vendor = new string(_ci.vendor),
                widthImage = _ci.widthImage
            };
        }

        public override int ROICount {
            get {
                int numberOfROI = 0;
                object[] _params = new object[] { IdTattile, numberOfROI };
                RecursiveApplyFunc(getNumberOfROITattile, ref _params, 3, 15);
                //numberOfROI = (int)_params[1];
                //return numberOfROI;
                return (int)_params[1];
            }
        }

        public override void HardReset() {

            bool wasGrabbing = grabbing;
            object[] _params = new object[] { IdTattile };
            //RecursiveApplyFunc(stopStreamTattile, ref _params, 3, 15);
            StopGrab();
            _recursiveApplyFunc(() => { return TattileInterfaceSvc.TI_Reset(IdTattile); });
            //RecursiveApplyFunc(hardwareResetTattile, ref _params, 3, 15);
            Thread.Sleep(10000);
            int hit = 20;
            bool ok = false;
            while (!ok && hit > 0) {
                try {
                    if (GetCameraStatus() == CameraNewStatus.Ready) {
                        ok = true;
                    }
                }
                catch (Exception ex) {
                    ok = false;
                }
                hit--;
                System.Threading.Thread.Sleep(2000);
            }
            //_params = new object[] { IdTattile };
            //RecursiveApplyFunc(startStreamTattile, ref _params, 3, 15);
            //Init();
            //try {
            //    Connect();
            //}
            //catch (Exception ex) {
            //    Log.Line(LogLevels.Error, "TattileCameraBase.HardReset", "CONNECT FAILED AFTER RESET");
            //    throw ex;
            //}
            Grab();
            if (hit == 0)   //reset fallito
                throw new CameraException("Timeout hard reset camera");
        }

        public override void SoftReset() {

            _recursiveApplyFunc(() => { return TattileInterfaceSvc.TI_ResetCounters(IdTattile); });
            startRun();
        }

        protected virtual void preApplyParametersTasks() { }

        protected virtual void postApplyParametersTasks() {
            if (RebootRequired) {
                Log.Line(LogLevels.Pass, "TattileCameraBase.postApplyParametersTasks", IP4Address + ": Starting camera HARD reset...");
                HardReset();
                Log.Line(LogLevels.Pass, "TattileCameraBase.postApplyParametersTasks", IP4Address + ": Camera HARD reset completed successfully");
            }
        }

        public override void ApplyParameters(ParameterTypeEnum paramType, Cam dataSource) {

            object[] _params = new object[] { IdTattile };
            try {
                preApplyParametersTasks();
            }
            catch {
                throw;
            }
            try {
                if ((paramType & ParameterTypeEnum.Acquisition) != 0) {
                    Log.Line(LogLevels.Pass, "TattileCameraBase.ApplyParameters", "CAM {0}: Setting acquisition parameters...", IdCamera);
                    SetAcquisitionParameters(dataSource.AcquisitionParameters);
                    Log.Line(LogLevels.Pass, "TattileCameraBase.ApplyParameters", "CAM {0}: Acquisition parameters set successfully", IdCamera);
                }
                if ((paramType & ParameterTypeEnum.FeatureEnabled) != 0) {
                    Log.Line(LogLevels.Pass, "TattileCameraBase.ApplyParameters", "CAM {0}: Setting feature enable parameters...", IdCamera);
                    SetFeaturesEnableParameters(dataSource.FeaturesEnableParameters);
                    Log.Line(LogLevels.Pass, "TattileCameraBase.ApplyParameters", "CAM {0}: Feature enable parameters set successfully", IdCamera);
                }
                if ((paramType & ParameterTypeEnum.RecipeSimple) != 0) {
                    Log.Line(LogLevels.Pass, "TattileCameraBase.ApplyParameters", "CAM {0}: Setting recipe simple parameters...", IdCamera);
                    SetRecipeSimpleParameters(dataSource.RecipeSimpleParameters);
                    Log.Line(LogLevels.Pass, "TattileCameraBase.ApplyParameters", "CAM {0}: Recipe simple parameters set successfully", IdCamera);
                }
                if ((paramType & ParameterTypeEnum.RecipeAdvanced) != 0) {
                    Log.Line(LogLevels.Pass, "TattileCameraBase.ApplyParameters", "CAM {0}: Setting recipe advanced parameters...", IdCamera);
                    SetRecipeAdvancedParameters(dataSource.RecipeAdvancedParameters);
                    Log.Line(LogLevels.Pass, "TattileCameraBase.ApplyParameters", "CAM {0}: Recipe advanced parameters set successfully", IdCamera);
                }
                if ((paramType & ParameterTypeEnum.ROI) != 0) {
                    for (int ir = 0; ir < dataSource.ROIParameters.Count; ir++) {
                        Log.Line(LogLevels.Pass, "TattileCameraBase.ApplyParameters", "CAM {0}: Setting ROI {1} parameters...", IdCamera, ir);
                        SetROIParameters(dataSource.ROIParameters[ir], ir);
                        Log.Line(LogLevels.Pass, "TattileCameraBase.ApplyParameters", "CAM {0}: ROI {1} parameters set successfully", IdCamera, ir);
                    }
                }
                if ((paramType & ParameterTypeEnum.Machine) != 0) {
                    Log.Line(LogLevels.Pass, "TattileCameraBase.ApplyParameters", "CAM {0}: Setting machine parameters...", IdCamera);
                    SetMachineParameters(dataSource.MachineParameters);
                    Log.Line(LogLevels.Pass, "TattileCameraBase.ApplyParameters", "CAM {0}: Machine parameters set successfully", IdCamera);
                }
                if ((paramType & ParameterTypeEnum.Strobo) != 0) {
                    for (int li = 0; li < dataSource.Lights.Count; li++) {
                        Log.Line(LogLevels.Pass, "TattileCameraBase.ApplyParameters", "CAM {0}: Setting strobo {1} parameters...", IdCamera, li);
                        LightController camLight = Lights.Find((LightController l) => { return l.Id == dataSource.Lights[li].Id; });
                        SetStrobeParameters(camLight.Id, dataSource.Lights[li].StroboParameters);
                        camLight.Strobe.ApplyParameters(camLight.StrobeChannel);
                        Log.Line(LogLevels.Pass, "TattileCameraBase.ApplyParameters", "CAM {0}: Strobo {1} parameters set successfully", IdCamera, li);
                    }
                }
            }
            catch {
                throw;
            }
            finally {
                try {
                    _recursiveApplyFunc(() => { return TattileInterfaceSvc.TI_CommitChanges(IdTattile); });
                    postApplyParametersTasks();
                    if (RebootRequired) {
                        RebootRequired = false;
                        ApplyParameters(paramType, dataSource);
                    }
                }
                catch {
                    throw;
                }
            }
        }

        public override void ApplyParameters(CameraSetting settings, ParameterTypeEnum paramType, Cam dataSource) {

            object[] _params = new object[] { IdTattile };
            try {
                preApplyParametersTasks();
            }
            catch {
                throw;
            }
            Exception exx = null;
            try {
                if ((paramType & ParameterTypeEnum.Acquisition) != 0) {
                    Log.Line(LogLevels.Pass, "TattileCameraBase.ApplyParameters", "CAM {0}: Setting acquisition parameters...", IdCamera);
                    SetAcquisitionParameters(dataSource.AcquisitionParameters);
                    Log.Line(LogLevels.Pass, "TattileCameraBase.ApplyParameters", "CAM {0}: Acquisition parameters set successfully", IdCamera);
                }
                if ((paramType & ParameterTypeEnum.FeatureEnabled) != 0) {
                    Log.Line(LogLevels.Pass, "TattileCameraBase.ApplyParameters", "CAM {0}: Setting feature enable parameters...", IdCamera);
                    SetFeaturesEnableParameters(dataSource.FeaturesEnableParameters);
                    Log.Line(LogLevels.Pass, "TattileCameraBase.ApplyParameters", "CAM {0}: Feature enable parameters set successfully", IdCamera);
                }
                if ((paramType & ParameterTypeEnum.RecipeSimple) != 0) {
                    Log.Line(LogLevels.Pass, "TattileCameraBase.ApplyParameters", "CAM {0}: Setting recipe simple parameters...", IdCamera);
                    SetRecipeSimpleParameters(dataSource.RecipeSimpleParameters);
                    Log.Line(LogLevels.Pass, "TattileCameraBase.ApplyParameters", "CAM {0}: Recipe simple parameters set successfully", IdCamera);
                }
                if ((paramType & ParameterTypeEnum.RecipeAdvanced) != 0) {
                    Log.Line(LogLevels.Pass, "TattileCameraBase.ApplyParameters", "CAM {0}: Setting recipe advanced parameters...", IdCamera);
                    SetRecipeAdvancedParameters(dataSource.RecipeAdvancedParameters);
                    Log.Line(LogLevels.Pass, "TattileCameraBase.ApplyParameters", "CAM {0}: Recipe advanced parameters set successfully", IdCamera);
                }
                if ((paramType & ParameterTypeEnum.ROI) != 0) {
                    for (int ir = 0; ir < dataSource.ROIParameters.Count; ir++) {
                        Log.Line(LogLevels.Pass, "TattileCameraBase.ApplyParameters", "CAM {0}: Setting ROI {1} parameters...", IdCamera, ir);
                        SetROIParameters(dataSource.ROIParameters[ir], ir); //AT/mod+ "dataSource ricalcolato per la ROI"
                        Log.Line(LogLevels.Pass, "TattileCameraBase.ApplyParameters", "CAM {0}: ROI {1} parameters set successfully", IdCamera, ir);
                    }
                }
                if ((paramType & ParameterTypeEnum.Machine) != 0) {
                    Log.Line(LogLevels.Pass, "TattileCameraBase.ApplyParameters", "CAM {0}: Setting machine parameters...", IdCamera);
                    SetMachineParameters(dataSource.MachineParameters);
                    Log.Line(LogLevels.Pass, "TattileCameraBase.ApplyParameters", "CAM {0}: Machine parameters set successfully", IdCamera);
                }
                if ((paramType & ParameterTypeEnum.Strobo) != 0 &&
                    settings.StroboIP4Address != null) {
                    Log.Line(LogLevels.Pass, "TattileCameraBase.ApplyParameters", "CAM {0}: Setting strobo parameters...", IdCamera);
                    SetStrobeParameters(settings, dataSource.StroboParameters);
                    if (settings.LightCtrl != null) {
                        settings.LightCtrl.SyncSendToController(settings.StroboChannel);
                        for (int i = 0; i < 100; i++) {
                            settings.LightCtrl.SimulateInputTrigger(settings.StroboChannel);
                            System.Threading.Thread.Sleep(10);
                        }
                    }
                    else throw new CameraException("No light controller associated");
                    Log.Line(LogLevels.Pass, "TattileCameraBase.ApplyParameters", "CAM {0}: Strobo parameters set successfully", IdCamera);
                }
            }
            catch (Exception ex) {
                exx = new Exception("Error sending parameters", ex);
            }
            finally {
                try {
                    _recursiveApplyFunc(() => { return TattileInterfaceSvc.TI_CommitChanges(IdTattile); });
                    postApplyParametersTasks();
                    if (RebootRequired) {
                        RebootRequired = false;
                        ApplyParameters(paramType, dataSource);
                    }
                }
                catch {
                    throw;
                }
                if (exx != null)
                    throw exx;
            }
        }

        public override string LoadFormat(int formatId) {
            string formatDesc = "";
            object[] _params = new object[] { IdTattile, formatId, formatDesc };
            RecursiveApplyFunc(loadFormatTattile, ref _params, 3, 15);
            return (string)_params[2];
        }

        public override void SetStopCondition(int headNumber, int condition, int timeout) {
            if (stopOnCondTimer != null) {
                stopOnCondTimer.Enabled = false;
                stopOnCondTimer = null;
            }
            stopOnCondTimer = new System.Timers.Timer(timeout * 1000D);
            stopOnCondTimer.Elapsed += new System.Timers.ElapsedEventHandler(stopOnCondTimer_Elapsed);
            stopOnCondTimer.Enabled = true;
            object[] _params = new object[] { IdTattile, headNumber, condition };
            RecursiveApplyFunc(setStopConditionTattile, ref _params, 3, 15);
        }

        void stopOnCondTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            if (GetCameraProcessingMode() == CameraProcessingMode.GoingToStopOnCondition) {
                StopAnalysisOffline();
            }
            stopOnCondTimer.Enabled = false;
        }

        void reloadModel() {
            object[] _params = new object[] { IdTattile };
            RecursiveApplyFunc(reloadModelTattile, ref _params, 3, 15);
        }

        int prevCameraStatus = -1;
        int prevCameraProgramStatus = -1;
        DateTime getCameraStatusDateTime = new DateTime();
        public override CameraNewStatus GetCameraStatus() {

            updateIdTattile();
            Int32 status = -1;
            Int32 programStatus = -1;
            DateTime now = DateTime.Now;
            if ((now - getCameraStatusDateTime).TotalMilliseconds > 200F) {
                try {
                    object[] _params = new object[] { IdTattile, status };
                    RecursiveApplyFunc(getStatusTattile, ref _params, 3, 15);
                    status = (int)_params[1];
                    RecursiveApplyFunc(getProgramStatusTattile, ref _params, 3, 15);
                    programStatus = (int)_params[1];
                }
                catch {
                    status = -1;
                    programStatus = -1;
                }
                finally {
                    prevCameraStatus = status;
                    prevCameraProgramStatus = programStatus;
                    getCameraStatusDateTime = now;
                }
            }
            else {
                status = prevCameraStatus;
                programStatus = prevCameraProgramStatus;
            }
            CameraNewStatus retStatus = CameraNewStatus.Unavailable;
            try {
                retStatus = MapStatus(status, programStatus);
            }
            catch {
                retStatus = CameraNewStatus.Error;
            }
            return retStatus;
        }

        public override CameraProcessingMode GetCameraProcessingMode() {
            Int32 status = -1;
            Int32 programStatus = -1;
            DateTime now = DateTime.Now;
            if ((now - getCameraStatusDateTime).TotalMilliseconds > 200F) {
                try {
                    object[] _params = new object[] { IdTattile, status };
                    RecursiveApplyFunc(getStatusTattile, ref _params, 3, 15);
                    status = (int)_params[1];
                    RecursiveApplyFunc(getProgramStatusTattile, ref _params, 3, 15);
                    programStatus = (int)_params[1];
                }
                catch {
                    status = -1;
                    programStatus = -1;
                }
                finally {
                    prevCameraStatus = status;
                    prevCameraProgramStatus = programStatus;
                    getCameraStatusDateTime = now;
                }
            }
            else {
                status = prevCameraStatus;
                programStatus = prevCameraProgramStatus;
            }
            return MapProcessingMode(status, programStatus);
        }

        protected abstract CameraWorkingMode MapWorkingMode(int tattileStatus, int tattileProgramStatus);

        protected abstract CameraNewStatus MapStatus(int tattileStatus, int tattileProgramStatus);

        protected abstract CameraProcessingMode MapProcessingMode(int tattileStatus, int tattileProgramStatus);

        public override void StartLearning() {
            object[] _params = new object[] { IdTattile };
            RecursiveApplyFunc(setStartLearningTattile, ref _params, 3, 15);
        }

        public override void StopLearning(bool save) {

            _recursiveApplyFunc(() => { return TattileInterfaceSvc.TI_AssistantStop(IdTattile, save); });
        }

        public override void StartAnalysisOffline() {

            _recursiveApplyFunc(() => { return TattileInterfaceSvc.TI_AnalysisOffLineStart(IdTattile); });
        }

        public override void StopAnalysisOffline() {

            _recursiveApplyFunc(() => { return TattileInterfaceSvc.TI_AnalysisOffLineStop(IdTattile); });
        }

        public override void ImportParameters(string path) {
        }

        internal void LoadVialAxis(string path) {
            VialAxis vialAxis = VialAxis.LoadFromFile(path);
            TI_AxisBuffer axisBuffer = new TI_AxisBuffer();
            axisBuffer.data = new TI_VialAxisData[63];
            for (int iS = 0; iS < 63; iS++) //pier: 63 è un numero magico...ATTENZIONE!!!
            {
                axisBuffer.data[iS] = vialAxis.VialAxisData[iS];
            }
            axisBuffer.code = 0xAA01;
            axisBuffer.filledDataNumber = 63;
            object[] _params = new object[] { IdTattile, axisBuffer };
            RecursiveApplyFunc(loadVialAxisTattile, ref _params, 3, 15);
        }

        #region GetSetTattileParameters

        public override ParameterCollection<T> GetParameters<T>() {

            ParameterCollection<T> coll = new ParameterCollection<T>(AppEngine.Current.ParametersInfo, AppEngine.Current.CurrentContext.CultureCode);
            return coll;
        }

        public override AcquisitionParameterCollection GetAcquisitionParametersList() {
            TI_AcquisitionParameters par = new TI_AcquisitionParameters();
            return par.ToParamCollection(culture);
        }

        public override AcquisitionParameterCollection GetAcquisitionParameters() {
            return getParameters<TI_AcquisitionParameters, AcquisitionParameterCollection>();
        }

        public override ROIParameterCollection GetROIParametersList() {
            TI_ROI par = new TI_ROI();
            return par.ToParamCollection(culture);
        }

        public override ROIParameterCollection GetROIParameters(int idROI) {
            return getParameters<TI_ROI, ROIParameterCollection>(idROI);
        }

        public override StroboParameterCollection GetStrobeParametersList() {
            StroboParameterCollection list = new StroboParameterCollection();
            list.Add("stroboOperatingMode", "");
            list.Add("stroboCurrent", "");
            list.Add("stroboPulseDelay", "");
            list.Add("stroboPulseWidth", "");
            list.Add("stroboRetriggerTime", "");
            list.Add("stroboContinuousMaxCurrent", "");
            list.Add("stroboPulsedMaxCurrent", "");
            return list;
        }

        public override StroboParameterCollection GetStrobeParameters(int lightId) {

            StroboParameterCollection coll = new StroboParameterCollection();
            LightController camLight = Lights.Find((LightController l) => { return l.Id == lightId; });
            ParameterCollection<StroboParameter> param = null;
            if (camLight != null)
                param = camLight.Strobe.GetStrobeParameter(camLight.StrobeChannel);
            foreach (StroboParameter p in param)
                coll.Add(p.Id, p.Value);
            return coll;
        }

        public override StroboParameterCollection GetStrobeParameters(CameraSetting camSettings) {

            int chanID = camSettings.StroboChannel;
            if (camSettings.LightCtrl != null) {
                if (camSettings.StroboChannel < 0 || camSettings.StroboChannel > camSettings.LightCtrl.ControllerSpecs.ChanConfig.Count - 1)
                    throw new CameraException("Invalid strobe associated with the camera");

                StroboParameterCollection list = new StroboParameterCollection();
                list.Add("stroboOperatingMode", camSettings.LightCtrl.ControllerSpecs.GetStringOperatingMode(chanID));
                list.Add("stroboCurrent", camSettings.LightCtrl.ControllerSpecs.ChanConfig[chanID].OperatingCurrent.ToString(culture));
                list.Add("stroboPulseDelay", camSettings.LightCtrl.ControllerSpecs.ChanConfig[chanID].OperatingDelay.ToString(culture));
                list.Add("stroboPulseWidth", camSettings.LightCtrl.ControllerSpecs.ChanConfig[chanID].OperatingPulseWidth.ToString(culture));
                list.Add("stroboRetriggerTime", camSettings.LightCtrl.ControllerSpecs.ChanConfig[chanID].OperatingRetrigger.ToString(culture));
                list.Add("stroboContinuousMaxCurrent", camSettings.LightCtrl.ControllerSpecs.ChanConfig[chanID].MaxCurrentContinuousModeA.ToString(culture));
                list.Add("stroboPulsedMaxCurrent", camSettings.LightCtrl.ControllerSpecs.ChanConfig[chanID].MaxCurrentPulsedModeA.ToString(culture));
                return list;
            }
            return null;
        }

        public override void SetAcquisitionParameters(ParameterCollection<AcquisitionParameter> parameters) {

            try {
                _setParameters(new object[] { ParameterTypeEnum.Acquisition, parameters });
            }
            catch {
                throw;
            }
            //setParameters<TI_AcquisitionParameters, ParameterCollection<AcquisitionParameter>>(parameters);
        }

        public override void SetFeaturesEnableParameters(ParameterCollection<FeaturesEnableParameter> parameters) {
            try {
                _setParameters(new object[] { ParameterTypeEnum.FeatureEnabled, parameters });
            }
            catch {
                throw;
            }
            //if (CameraType == "M9") setParameters<TI_FeaturesEnableParticles, FeaturesEnableParameterCollection>(parameters);
            //else if (CameraType == "M12") setParameters<TI_FeaturesEnableCosmetic, FeaturesEnableParameterCollection>(parameters);            
            //else throw new CameraException("Camera type not supported yet or invalid camera type");
        }

        public override void SetRecipeSimpleParameters(ParameterCollection<RecipeSimpleParameter> parameters) {

            try {
                _setParameters(new object[] { ParameterTypeEnum.RecipeSimple, parameters });
            }
            catch {
                throw;
            }
            //if (CameraType == "M9") setParameters<TI_RecipeSimpleParticles, RecipeSimpleParameterCollection>(parameters);
            //else if (CameraType == "M12") setParameters<TI_RecipeSimpleCosmetic, RecipeSimpleParameterCollection>(parameters);
            //else throw new CameraException("Camera type not supported yet or invalid camera type");
        }

        public override void SetRecipeAdvancedParameters(ParameterCollection<RecipeAdvancedParameter> parameters) {

            try {
                _setParameters(new object[] { ParameterTypeEnum.RecipeAdvanced, parameters });
            }
            catch {
                throw;
            }

            //if (CameraType == "M9") setParameters<TI_RecipeAdvancedParticles, RecipeAdvancedParameterCollection>(parameters);
            //else if (CameraType == "M12") setParameters<TI_RecipeAdvancedCosmetic, RecipeAdvancedParameterCollection>(parameters);
            //else throw new CameraException("Camera type not supported yet or invalid camera type");
        }

        public override void SetROIParameters(ParameterCollection<ROIParameter> parameters, int idROI) {

            try {
                _setParameters(new object[] { ParameterTypeEnum.ROI, parameters, idROI });
            }
            catch {
                throw;
            }

            //setParameters<TI_ROI, ROIParameterCollection>(parameters, idROI);
        }

        public override void SetMachineParameters(ParameterCollection<MachineParameter> parameters) {

            try {
                _setParameters(new object[] { ParameterTypeEnum.Machine, parameters });
            }
            catch {
                throw;
            }
            //setParameters<TI_MachineParameters, MachineParameterCollection>(parameters);
        }

        public override void SetStrobeParameters(int lightId, ParameterCollection<StroboParameter> parameters) {

            LightController camLight = Lights.Find((LightController l) => { return l.Id == lightId; });
            if (camLight != null)
                camLight.Strobe.SetStrobeParameter(camLight.StrobeChannel, parameters);
        }

        public override void SetStrobeParameters(CameraSetting camSettings, ParameterCollection<StroboParameter> parameters) {
            int chanID = camSettings.StroboChannel;
            if (camSettings.LightCtrl == null || chanID < 0 || chanID > camSettings.LightCtrl.ControllerSpecs.ChanConfig.Count - 1)
                throw new CameraException("Invalid strobe associated with the camera");
            camSettings.LightCtrl.ControllerSpecs.ChanConfig[chanID].IDchan = chanID;
            try {
                camSettings.LightCtrl.ControllerSpecs.SetStringOperatingMode(chanID, parameters["stroboOperatingMode"].Value);
                camSettings.LightCtrl.ControllerSpecs.ChanConfig[chanID].OperatingCurrent = Convert.ToDouble(parameters["stroboCurrent"].GetValue(), culture);
                camSettings.LightCtrl.ControllerSpecs.ChanConfig[chanID].OperatingDelay = Convert.ToDouble(parameters["stroboPulseDelay"].GetValue(), culture);
                camSettings.LightCtrl.ControllerSpecs.ChanConfig[chanID].OperatingPulseWidth = Convert.ToDouble(parameters["stroboPulseWidth"].GetValue(), culture);
                camSettings.LightCtrl.ControllerSpecs.ChanConfig[chanID].OperatingRetrigger = Convert.ToDouble(parameters["stroboRetriggerTime"].GetValue(), culture);
                camSettings.LightCtrl.ControllerSpecs.ChanConfig[chanID].MaxCurrentContinuousModeA = Convert.ToDouble(parameters["stroboContinuousMaxCurrent"].GetValue(), culture);
                camSettings.LightCtrl.ControllerSpecs.ChanConfig[chanID].MaxCurrentPulsedModeA = Convert.ToDouble(parameters["stroboPulsedMaxCurrent"].GetValue(), culture);
            }
            catch {
                throw;
            }
        }

        //public void SetLearningStrobo(StroboAssistantParameterCollection parameters) {
        //    if (CameraType == "M9") setParameters<TI_LearningStrobo, StroboAssistantParameterCollection>(parameters);
        //    else throw new CameraException("Camera type not supported yet or invalid camera type");
        //}

        //public void SetLearningNormalization(NormalizationAssistantParameterCollection parameters) {
        //    if (CameraType == "M9") setParameters<TI_LearningNormalization, NormalizationAssistantParameterCollection>(parameters);
        //    else throw new CameraException("Camera type not supported yet or invalid camera type");
        //}

        //public void SetLearningVialAxis(VialAxisAssistantParameterCollection parameters) {
        //    if (CameraType == "M9") setParameters<TI_LearningVialAxis, VialAxisAssistantParameterCollection>(parameters);
        //    else throw new CameraException("Camera type not supported yet or invalid camera type");
        //}

        //public void SetCheckVialAxis(VialAxisAssistantParameterCollection parameters) {
        //    if (CameraType == "M9") setParameters<TI_CheckVialAxis, VialAxisAssistantParameterCollection>(parameters);
        //    else throw new CameraException("Camera type not supported yet or invalid camera type");
        //}

        //public void SetLearningResetTriggerEncoder(ResetTriggerEncAssistantParameterCollection parameters) {
        //    if (CameraType == "M9") setParameters<TI_LearningResetTriggerEncoder, ResetTriggerEncAssistantParameterCollection>(parameters);
        //    else throw new CameraException("Camera type not supported yet or invalid camera type");
        //}

        protected S getParameters<T, S>(params object[] args) where T : new() {
            int formatNum = -1;
            string formatDesc = "";
            T tattileParams = new T();
            object[] _params;
            switch (typeof(T).Name) {
                //case "TI_RecipeAdvancedCosmetic":
                //    _params = new object[] { IdTattile, tattileParams };
                //    ((TI_RecipeAdvancedCosmetic)_params[1]).Init();
                //    break;
                case "TI_ROI":  //parametro opzionale: ID roi
                    _params = new object[] { IdTattile, (int)args[0], tattileParams };
                    break;
                default:    //no optional parameters
                    _params = new object[] { IdTattile, tattileParams };
                    break;
            }
            RecursiveApplyFunc(getParametersTattile<T>, ref _params, 3, 15);
            object[] formatParams;
            if (tattileParams.GetType().Name == "TI_AcquisitionParameters" &&
                CameraType == "M9") {
                formatParams = new object[] { IdTattile, formatNum, formatDesc };
                RecursiveApplyFunc(getFormatReadLoadedTattile, ref formatParams, 3, 15);
                formatNum = (int)formatParams[1];
                formatDesc = (string)formatParams[2];
            }
            object list = new object();
            try {
                switch (typeof(T).Name) {
                    case "TI_AcquisitionParameters":
                        list = ((TI_AcquisitionParameters)_params[1]).ToParamCollection(culture);
                        break;
                    case "TI_FeaturesEnableParticles":
                        list = ((TI_FeaturesEnableParticles)_params[1]).ToParamCollection(culture);
                        break;
                    case "TI_FeaturesEnableCosmetic":
                        list = ((TI_FeaturesEnableCosmetic)_params[1]).ToParamCollection(culture);
                        break;
                    case "TI_RecipeSimpleParticles":
                        list = ((TI_RecipeSimpleParticles)_params[1]).ToParamCollection(culture);
                        break;
                    case "TI_RecipeSimpleCosmetic":
                        list = ((TI_RecipeSimpleCosmetic)_params[1]).ToParamCollection(culture);
                        break;
                    case "TI_RecipeAdvancedParticles":
                        list = ((TI_RecipeAdvancedParticles)_params[1]).ToParamCollection(culture);
                        break;
                    case "TI_RecipeAdvancedCosmetic":
                        list = ((TI_RecipeAdvancedCosmetic)_params[1]).ToParamCollection(culture);
                        break;
                    case "TI_ROI":
                        list = ((TI_ROI)_params[2]).ToParamCollection(culture);
                        break;
                    case "TI_MachineParameters":
                        list = ((TI_MachineParameters)_params[1]).ToParamCollection(culture);
                        break;
                    case "TI_MachineParametersCosmetic":
                        list = ((TI_MachineParametersCosmetic)_params[1]).ToParamCollection(culture);
                        break;
                    default:
                        throw new CameraException("Invalid parameter section or not implemented yet");
                }
            }
            catch (Exception ex) {
                list = null;
                return (S)(object)list;
            }
            return (S)(object)list;
        }

        unsafe protected virtual int _setParameters(object[] args) {

            ParameterTypeEnum paramType = (ParameterTypeEnum)args[0];
            IParameterCollection parameters = (IParameterCollection)args[1];
            int ret = -1;
            switch (paramType) {
                case ParameterTypeEnum.Acquisition:
                    TI_AcquisitionParameters tattileAcqParams = new TI_AcquisitionParameters(parameters);
                    if (CameraType == "M9") {
                        TI_AcquisitionParameters tattileAcqOldParams = new TI_AcquisitionParameters();
                        _recursiveApplyFunc(() => { return TattileInterfaceSvc.TI_AcquisitionParametersGet(IdTattile, ref tattileAcqOldParams); });
                        RebootRequired |= calcRebootRequired(tattileAcqOldParams, tattileAcqParams);
                        if (RebootRequired)
                            Log.Line(LogLevels.Pass, "TattileCameraBase._setParameters", "Camera reboot required!");
                    }
                    _recursiveApplyFunc(() => { return TattileInterfaceSvc.TI_AcquisitionParametersSet(IdTattile, ref tattileAcqParams); });
                    break;
                case ParameterTypeEnum.ROI:
                    TI_ROI tattileROIParams = new TI_ROI(parameters);
                    _recursiveApplyFunc(() => { return TattileInterfaceSvc.TI_ROISet(IdTattile, (int)args[2], ref tattileROIParams); });
                    break;
                default:
                    throw new CameraException("Unknown parameter section");
                //break;
            }
            return ret;
        }

        protected virtual bool calcRebootRequired<T>(T oldParams, T newParams) {
            return false;
        }

        //unsafe private void setParameters<T, S>(S parameters, params object[] args) where T : new() {
        //    object[] _params;
        //    switch (typeof(T).Name) {
        //        case "TI_LearningStrobo":
        //            TI_LearningStrobo learningStrobo = new TI_LearningStrobo();
        //            StroboAssistantParameterCollection sapParams = (StroboAssistantParameterCollection)(object)parameters;
        //            learningStrobo.size = sizeof(TI_LearningStrobo);
        //            learningStrobo.maxDensityReference = Convert.ToInt32(sapParams["maxDensityReference"].GetValue());
        //            learningStrobo.stroboTime = Convert.ToInt32(sapParams["stroboTime"].GetValue());
        //            learningStrobo.shutterTime = Convert.ToInt32(sapParams["shutterTime"].GetValue());
        //            _params = new object[] { IdTattile, (int)TI_PatriclesMode.MODE_LEARNING_STROBO, learningStrobo };
        //            RecursiveApplyFunc(setParametersTattile<T>, ref _params, 3, 15);
        //            break;
        //        case "TI_LearningNormalization":
        //            TI_LearningNormalization learningNormalization = new TI_LearningNormalization();
        //            NormalizationAssistantParameterCollection napParams = (NormalizationAssistantParameterCollection)(object)parameters;
        //            learningNormalization.size = sizeof(TI_LearningNormalization);
        //            learningNormalization.pgaRawStart = Convert.ToInt32(napParams["pgaRawStart"].GetValue());
        //            learningNormalization.pgaRawEnd = Convert.ToInt32(napParams["pgaRawEnd"].GetValue());
        //            _params = new object[] { IdTattile, (int)TI_PatriclesMode.MODE_LEARNING_NORMALIZATION, learningNormalization };
        //            RecursiveApplyFunc(setParametersTattile<T>, ref _params, 3, 15);
        //            break;
        //        case "TI_LearningVialAxis":
        //            TI_LearningVialAxis learningVialAxis = new TI_LearningVialAxis();
        //            VialAxisAssistantParameterCollection vaapParams = (VialAxisAssistantParameterCollection)(object)parameters;
        //            learningVialAxis.size = sizeof(TI_LearningVialAxis);
        //            learningVialAxis.samplesVialAxis = Convert.ToInt32(vaapParams["samplesVialAxis"].GetValue());
        //            learningVialAxis.findVialAxisX = Convert.ToInt32(vaapParams["findVialAxisX"].GetValue());
        //            learningVialAxis.findVialAxisY = Convert.ToInt32(vaapParams["findVialAxisY"].GetValue());
        //            _params = new object[] { IdTattile, (int)TI_PatriclesMode.MODE_LEARNING_VIALAXIS, learningVialAxis };
        //            RecursiveApplyFunc(setParametersTattile<T>, ref _params, 3, 15);
        //            break;
        //        case "TI_CheckVialAxis":
        //            TI_CheckVialAxis checkVialAxis = new TI_CheckVialAxis();
        //            _params = new object[] { IdTattile, (int)TI_PatriclesMode.MODE_CHECK_VIALAXIS, checkVialAxis };
        //            RecursiveApplyFunc(setParametersTattile<T>, ref _params, 3, 15);
        //            break;
        //        case "TI_LearningResetTriggerEncoder":
        //            TI_LearningResetTriggerEncoder learningResetTriggerEncoder = new TI_LearningResetTriggerEncoder();
        //            ResetTriggerEncAssistantParameterCollection rteapParams = (ResetTriggerEncAssistantParameterCollection)(object)parameters;
        //            learningResetTriggerEncoder.size = sizeof(TI_LearningResetTriggerEncoder);
        //            learningResetTriggerEncoder.findVialAxisX = Convert.ToInt32(rteapParams["findVialAxisX"].GetValue());
        //            learningResetTriggerEncoder.findVialAxisY = Convert.ToInt32(rteapParams["findVialAxisY"].GetValue());
        //            learningResetTriggerEncoder.encoderTrigger = Convert.ToInt32(rteapParams["encoderTrigger"].GetValue());
        //            _params = new object[] { IdTattile, (int)TI_PatriclesMode.MODE_LEARNING_RESET_TRIGGER_ENCODER, learningResetTriggerEncoder };
        //            RecursiveApplyFunc(setParametersTattile<T>, ref _params, 3, 15);
        //            break;
        //        default:
        //            throw new CameraException("Invalid parameter section or not implemented yet");
        //    }

        //}

        unsafe int getParametersTattile<T>(params object[] args) {
            int ret = -1;
            switch (typeof(T).Name) {
                case "TI_AcquisitionParameters":
                    TI_AcquisitionParameters acqParams = (TI_AcquisitionParameters)args[1];
                    ret = TattileInterfaceSvc.TI_AcquisitionParametersGet(IdTattile, ref acqParams);
                    args[1] = acqParams;
                    break;
                case "TI_FeaturesEnableParticles":
                    TI_FeaturesEnableParticles fepParams = (TI_FeaturesEnableParticles)args[1];
                    ret = TattileInterfaceSvc.TI_FeaturesEnableGet(IdTattile, &fepParams);
                    args[1] = fepParams;
                    break;
                case "TI_FeaturesEnableCosmetic":
                    TI_FeaturesEnableCosmetic fecParams = (TI_FeaturesEnableCosmetic)args[1];
                    ret = TattileInterfaceSvc.TI_FeaturesEnableGet(IdTattile, &fecParams);
                    args[1] = fecParams;
                    break;
                case "TI_RecipeSimpleParticles":
                    TI_RecipeSimpleParticles rspParams = (TI_RecipeSimpleParticles)args[1];
                    ret = TattileInterfaceSvc.TI_RecipeSimpleGet(IdTattile, &rspParams);
                    args[1] = rspParams;
                    break;
                case "TI_RecipeSimpleCosmetic":
                    TI_RecipeSimpleCosmetic rscParams = (TI_RecipeSimpleCosmetic)args[1];
                    ret = TattileInterfaceSvc.TI_RecipeSimpleGet(IdTattile, &rscParams);
                    args[1] = rscParams;
                    break;
                case "TI_RecipeAdvancedParticles":
                    TI_RecipeAdvancedParticles rapParams = (TI_RecipeAdvancedParticles)args[1];
                    ret = TattileInterfaceSvc.TI_RecipeAdvancedGet(IdTattile, &rapParams);
                    args[1] = rapParams;
                    break;
                case "TI_RecipeAdvancedCosmetic":
                    TI_RecipeAdvancedCosmetic racParams = (TI_RecipeAdvancedCosmetic)args[1];
                    racParams.ARGSetTipModelPath = new TI_TextData() {
                        data_size = 512 * Marshal.SizeOf(typeof(char)),
                        data = Marshal.AllocHGlobal(512 * Marshal.SizeOf(typeof(char))),
                        data_used = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int))),
                    };
                    racParams.ARGBottleSleeveLearning = new TI_BinaryData() {
                        data_size = 1024 * 1024 * Marshal.SizeOf(typeof(char)),
                        data = Marshal.AllocHGlobal(1024 * 1024 * Marshal.SizeOf(typeof(char))),
                        data_used = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int))),
                    };

                    racParams.ARGBottleFlipOffLearning = new TI_BinaryData() {
                        data_size = 1024 * 1024 * Marshal.SizeOf(typeof(char)),
                        data = Marshal.AllocHGlobal(1024 * 1024 * Marshal.SizeOf(typeof(char))),
                        data_used = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int))),
                    };
                    ret = TattileInterfaceSvc.TI_RecipeAdvancedGet(IdTattile, &racParams);
                    args[1] = racParams;
                    break;
                case "TI_ROI":
                    TI_ROI roiParams = (TI_ROI)args[2];
                    ret = TattileInterfaceSvc.TI_ROIGet(IdTattile, (int)args[1], ref roiParams);
                    args[2] = roiParams;
                    break;
                case "TI_MachineParameters":
                    TI_MachineParameters machineParams = (TI_MachineParameters)args[1];
                    ret = TattileInterfaceSvc.TI_MachineParametersGet(IdTattile, &machineParams);
                    args[1] = machineParams;
                    break;
                case "TI_MachineParametersCosmetic":
                    TI_MachineParametersCosmetic machineParamsCosm = (TI_MachineParametersCosmetic)args[1];
                    ret = TattileInterfaceSvc.TI_MachineParametersGet(IdTattile, &machineParamsCosm);
                    args[1] = machineParamsCosm;
                    break;
                default:
                    throw new CameraException("Invalid parameter section or not implemented yet");
            }
            return ret;
        }

        //unsafe int _setParametersTattile(object[] args) {

        //    int ret = -1;
        //    ParameterTypeEnum pType = (ParameterTypeEnum)args[0];
        //    switch (pType) {
        //        case ParameterTypeEnum.Acquisition:
        //            TI_AcquisitionParameters acqParams = (TI_AcquisitionParameters)args[1];
        //            ret = TattileInterfaceSvc.TI_AcquisitionParametersSet(IdTattile, ref acqParams);
        //            break;
        //    }
        //    return ret;
        //}

        //unsafe int setParametersTattile<T>(params object[] args) {
        //    int ret = -1;
        //    switch (typeof(T).Name) {
        //        case "TI_AcquisitionParameters":
        //            TI_AcquisitionParameters acqParams = (TI_AcquisitionParameters)args[1];
        //            ret = TattileInterfaceSvc.TI_AcquisitionParametersSet(IdTattile, ref acqParams);
        //            break;
        //        case "TI_FeaturesEnableParticles":
        //            TI_FeaturesEnableParticles fepParams = (TI_FeaturesEnableParticles)args[1];
        //            ret = TattileInterfaceSvc.TI_FeaturesEnableSet(IdTattile, &fepParams);
        //            break;
        //        case "TI_FeaturesEnableCosmetic":
        //            TI_FeaturesEnableCosmetic fecParams = (TI_FeaturesEnableCosmetic)args[1];
        //            ret = TattileInterfaceSvc.TI_FeaturesEnableSet(IdTattile, &fecParams);
        //            break;
        //        case "TI_RecipeSimpleParticles":
        //            TI_RecipeSimpleParticles rspParams = (TI_RecipeSimpleParticles)args[1];
        //            ret = TattileInterfaceSvc.TI_RecipeSimpleSet(IdTattile, &rspParams);
        //            break;
        //        case "TI_RecipeSimpleCosmetic":
        //            TI_RecipeSimpleCosmetic rscParams = (TI_RecipeSimpleCosmetic)args[1];
        //            ret = TattileInterfaceSvc.TI_RecipeSimpleSet(IdTattile, &rscParams);
        //            break;
        //        case "TI_RecipeAdvancedParticles":
        //            TI_RecipeAdvancedParticles rapParams = (TI_RecipeAdvancedParticles)args[1];
        //            ret = TattileInterfaceSvc.TI_RecipeAdvancedSet(IdTattile, &rapParams);
        //            break;
        //        case "TI_RecipeAdvancedCosmetic":
        //            TI_RecipeAdvancedCosmetic racParams = (TI_RecipeAdvancedCosmetic)args[1];
        //            ret = TattileInterfaceSvc.TI_RecipeAdvancedSet(IdTattile, &racParams);
        //            break;
        //        case "TI_ROI":
        //            TI_ROI ROIParams = (TI_ROI)args[2];
        //            ret = TattileInterfaceSvc.TI_ROISet(IdTattile, (int)args[1], ref ROIParams);
        //            break;
        //        case "TI_MachineParameters":
        //            TI_MachineParameters machineParams = (TI_MachineParameters)args[1];
        //            ret = TattileInterfaceSvc.TI_MachineParametersSet(IdTattile, &machineParams);
        //            break;
        //        case "TI_LearningStrobo":
        //            TI_LearningStrobo learningStrobo = (TI_LearningStrobo)args[2];
        //            ret = TattileInterfaceSvc.TI_AssistantSet(IdTattile, (int)args[1], &learningStrobo);
        //            args[2] = learningStrobo;
        //            break;
        //        case "TI_LearningNormalization":
        //            TI_LearningNormalization learningNormalization = (TI_LearningNormalization)args[2];
        //            ret = TattileInterfaceSvc.TI_AssistantSet(IdTattile, (int)args[1], &learningNormalization);
        //            args[2] = learningNormalization;
        //            break;
        //        case "TI_LearningVialAxis":
        //            TI_LearningVialAxis learningVialAxis = (TI_LearningVialAxis)args[2];
        //            ret = TattileInterfaceSvc.TI_AssistantSet(IdTattile, (int)args[1], &learningVialAxis);
        //            args[2] = learningVialAxis;
        //            break;
        //        case "TI_CheckVialAxis":
        //            ret = TattileInterfaceSvc.TI_AssistantSet(IdTattile, (int)args[1], (void*)null);
        //            break;
        //        case "TI_LearningResetTriggerEncoder":
        //            TI_LearningResetTriggerEncoder learningResetTriggerEnc = (TI_LearningResetTriggerEncoder)args[2];
        //            ret = TattileInterfaceSvc.TI_AssistantSet(IdTattile, (int)args[1], &learningResetTriggerEnc);
        //            break;
        //        default:
        //            throw new CameraException("Invalid parameter section or not implemented yet");
        //    }
        //    return ret;
        //}
        #endregion

        internal void _recursiveApplyFunc(Func<int> function, int hitRetries = 3, int sleepTimeMs = 15) {

            int ret = -1;
            while (ret != 0 && hitRetries > 0) {
                ret = function();
                hitRetries--;
                System.Threading.Thread.Sleep(sleepTimeMs);
            }
            if (ret != 0)
                throw new CameraException(function.Method.Name + " failed with error " + ((ITF_ERROR_CODE)ret).ToString());
        }

        protected void RecursiveApplyFunc(Func<object[], int> function, ref object[] args, int hitRetries, int sleepTimeMs) {
            int ret = -1;
            while (ret != 0 && hitRetries > 0) {
                ret = function(args);
                hitRetries--;
                System.Threading.Thread.Sleep(sleepTimeMs);
            }
            if (ret != 0)
                throw new CameraException(function.Method.Name + " failed with error " + ((ITF_ERROR_CODE)ret).ToString(), ret);
        }

        protected int loadFormatTattile(params object[] args) {
            byte[] buffer = new byte[256];
            int ret = TattileInterfaceSvc.TI_FormatLoad(IdTattile, (int)args[1], buffer);
            args[2] = Encoding.ASCII.GetString(buffer).TrimEnd(new char[] { '\0' });
            return ret;
        }

        protected int setStopConditionTattile(params object[] args) {
            TI_StopCondition stopOnCondition = new TI_StopCondition();
            stopOnCondition.headNumber = (int)args[1];
            stopOnCondition.resultType = (int)args[2];
            return TattileInterfaceSvc.TI_StopOnCondition(IdTattile, ref stopOnCondition);
        }

        protected int setStartLearningTattile(params object[] args) {
            return TattileInterfaceSvc.TI_AssistantStart(IdTattile);
        }

        unsafe protected int getFormatReadLoadedTattile(params object[] args) {
            int formatNum = -1;
            byte[] buffer = new byte[256];
            int ret = TattileInterfaceSvc.TI_FormatReadLoaded(IdTattile, &formatNum, buffer);
            args[1] = formatNum;
            string formatDesc = Encoding.ASCII.GetString(buffer).TrimEnd(new char[] { '\0' });
            if (formatDesc == "") formatDesc = "<Empty>";
            args[2] = formatDesc;
            return ret;
        }

        protected int getInfoTattile(params object[] args) {
            TI_CameraInfo cameraInfo = new TI_CameraInfo();
            int ret = TattileInterfaceSvc.TI_InfoGet(IdTattile, ref cameraInfo);
            args[1] = cameraInfo;
            return ret;
        }

        unsafe protected int getStatusTattile(params object[] args) {
            int ret = -1;
            int status = -1;
            ret = TattileInterfaceSvc.TI_StatusGet(IdTattile, &status);
            args[1] = status;
            return ret;
        }

        unsafe protected int getProgramStatusTattile(params object[] args) {
            int ret = -1;
            int prgStatus = -1;
            ret = TattileInterfaceSvc.TI_GetRunStatus(IdTattile, &prgStatus);
            args[1] = prgStatus;
            return ret;
        }

        unsafe protected int getFirmwareVersionTattile(params object[] args) {
            int ret = -1;
            byte[] buffer = new byte[256];
            ret = TattileInterfaceSvc.TI_GetFirmwareVersion(IdTattile, buffer, 256);
            string firmwareVersion = Encoding.ASCII.GetString(buffer).TrimEnd(new char[] { '\0' });
            args[1] = firmwareVersion;
            return ret;
        }

        unsafe protected int getProgramVersionTattile(params object[] args) {
            int ret = -1;
            byte[] buffer = new byte[256];
            ret = TattileInterfaceSvc.TI_GetProgramVersion(IdTattile, buffer, 256);
            string firmwareVersion = Encoding.ASCII.GetString(buffer).TrimEnd(new char[] { '\0' });
            args[1] = firmwareVersion;
            return ret;
        }

        protected virtual int reloadModelTattile(params object[] args) {
            return 0;
        }

        unsafe protected int getNumberOfROITattile(params object[] args) {
            int roiNum = 0;
            int ret = TattileInterfaceSvc.TI_ROIGetNumber(IdTattile, &roiNum);
            args[1] = roiNum;
            return ret;
        }

        protected int loadVialAxisTattile(params object[] args) {
            TI_AxisBuffer axisBuffer = (TI_AxisBuffer)args[1];
            return TattileInterfaceSvc.TI_AxisSet(IdTattile, ref axisBuffer);
        }

        protected int saveVialAxisTattile(params object[] args) {
            TI_AxisBuffer axisBuffer = (TI_AxisBuffer)args[1];
            int ret = TattileInterfaceSvc.TI_AxisGet(IdTattile, ref axisBuffer);
            args[1] = axisBuffer;
            return ret;
        }

        unsafe protected int getImageBuffer(params object[] args) {
            byte[] buffer = (byte[])args[4];
            int bufferSize;
            int ret = TattileInterfaceSvc.TI_BufferGet(IdTattile, (int)args[1], (int)args[2], (int)args[3], buffer, &bufferSize);
            args[4] = buffer;
            args[5] = bufferSize;
            return ret;
        }

        public void RemoteDesktopConnect() {
        }

        public bool RemoteDesktopConnected {
            get { return false; }
        }

        public void RemoteDesktopDisconnect() {
        }

        public Image<Rgb, byte> CurrentImage { get; private set; }
        public Image<Rgb, byte> CurrentThumbnail { get; private set; }
        public bool CurrentIsReject { get; private set; }
        public Image<Rgb, byte> AlternativeImage { get; internal set; }
        public Rectangle ZoomAOI { get; set; }

        protected Image<Rgb, byte> appImage1 = null;
        protected Image<Rgb, byte> appImage2 = null;
        Stopwatch AdjustImageTimer = new Stopwatch();
        public virtual void AdjustImage(Image<Rgb, byte> srcImage, int width, int height, bool keepProp, Emgu.CV.CvEnum.INTER interpolationType, ref Image<Rgb, byte> dstImage, out Rectangle aoiUsed) {

            aoiUsed = new Rectangle(0, 0, width, height);
            if (srcImage == null || srcImage.Width == 0 || srcImage.Height == 0 || width <= 0 || height <= 0)
                return;
            AdjustImageTimer.Restart();
            long appTimer1 = 0, appTimer2 = 0;
            lock (srcImage) {
                try {
                    appImage1 = srcImage;
                    if (Cameras != null && Cameras.Count > 0 && Cameras.First().Rotation != 0) {
                        int rotation = Cameras.First().Rotation;
                        if (rotation % 90 == 0) {
                            if (rotation == 90 || rotation == 270) {
                                AllocImage(ref appImage2, appImage1.Height, appImage1.Width);
                                CvInvoke.cvTranspose(appImage1.Ptr, appImage2.Ptr);
                                if (rotation == 270)
                                    CvInvoke.cvFlip(appImage2.Ptr, appImage2.Ptr, 1);
                                else if (rotation == 90)
                                    CvInvoke.cvFlip(appImage2.Ptr, appImage2.Ptr, 0);
                            }
                            if (rotation == 180) {
                                AllocImage(ref appImage2, appImage1.Width, appImage1.Height);
                                CvInvoke.cvFlip(appImage1.Ptr, appImage2.Ptr, -1);
                            }
                        }
                        appTimer2 = AdjustImageTimer.ElapsedMilliseconds - appTimer1;
                        //Log.Line(LogLevels.Debug, "Station.AdjustImage", "Rotation time [ms]: " + appTimer2);
                        //Log.Line(LogLevels.Debug, "TestCamera.rotateImage", "Rot = {0}°. Tempo rotazione = {1}ms", rotation, rotateImgWatch.ElapsedMilliseconds);
                    }
                    else
                        appImage2 = appImage1;
                    //Image<Rgb, byte> resizedImage = null;

                    int w = (ZoomAOI.Width > 0) ? ZoomAOI.Width : appImage2.Width;
                    int h = (ZoomAOI.Height > 0) ? ZoomAOI.Height : appImage2.Height;
                    if (ZoomAOI.Width > 0 && ZoomAOI.Height > 0)
                        appImage2.ROI = ZoomAOI;

                    //Image<Rgb, byte> destinationImg = null;
                    if (keepProp) {
                        //resizedImage = appImage2.Resize(Math.Min((double)width / appImage2.Width, (double)height / appImage2.Height), interpolationType);
                        dstImage = appImage2.Resize(Math.Min((double)width / w, (double)height / h), interpolationType);
                        //CvInvoke.cvResize(appImage2.Ptr, dstImage.Ptr, interpolationType);
                    }
                    else {
                        //resizedImage = appImage2.Resize(width, height, interpolationType);
                        dstImage = appImage2.Resize(w, h, interpolationType);
                        //CvInvoke.cvResize(appImage2.Ptr, dstImage.Ptr, interpolationType);
                    }
                    //CvInvoke.cvCopy(destinationImg.Ptr, dstImage.Ptr, IntPtr.Zero);
                    //destinationImg.Dispose();

                    appImage2.ROI = Rectangle.Empty;
                    aoiUsed = calcAOIUsed(w, h, width, height, keepProp);

                    Log.Line(LogLevels.Debug, "Station.AdjustImage", "Resize time [ms]: " + (AdjustImageTimer.ElapsedMilliseconds - appTimer2 - appTimer1));
                }
                catch (Exception ex) {
                    Log.Line(LogLevels.Debug, "Station.AdjustImage", "Error adjusting image: " + ex.Message);
                }
            }
        }

        protected virtual Rectangle calcAOIUsed(int imgWidth, int imgHeight, int displayWidth, int displayHeight, bool keepProp) {
            Rectangle res;
            if (keepProp) {
                double zoomScale = Math.Min((double)displayWidth / imgWidth, (double)displayHeight / imgHeight);
                if ((double)imgWidth / imgHeight < (double)displayWidth / displayHeight)
                    res = new Rectangle((int)((displayWidth - zoomScale * imgWidth) / 2 + 0.5), 0, (int)(zoomScale * imgWidth + 0.5), displayHeight);
                else
                    res = new Rectangle(0, (int)((displayHeight - zoomScale * imgHeight) / 2 + 0.5), displayWidth, (int)(zoomScale * imgHeight + 0.5));
            }
            else {
                res = new Rectangle(0, 0, displayWidth, displayHeight);
            }
            return res;
        }

        protected virtual bool AllocImage(ref Image<Rgb, byte> imgToAlloc, int w, int h) {
            if (imgToAlloc != null && imgToAlloc.Width == w && imgToAlloc.Height == h) return true;
            try {
                if (imgToAlloc != null) imgToAlloc.Dispose();
                imgToAlloc = new Image<Rgb, byte>(w, h);
                return true;
            }
            catch (OutOfMemoryException ex) {
                Log.Line(LogLevels.Error, "Station.AllocImage", "OutOfMemoryException: " + ex.Message);
                imgToAlloc = null;
            }
            return false;
        }

        void ChangeImageInfo(int width, int height, int bpp) {

            if (inspectionBuffer == null)
                inspectionBuffer = new RawInspectionBuffer(MAX_INSPECTION_QUEUE_LENGTH);

            int maxImageSize = width * height * (bpp / 8);

            if (inspectionBuffer.MaxImageSize != maxImageSize) {
                inspectionBuffer.Clear();
                //else
                //    inspectionBuffer.Reset();
                inspectionBuffer.AllocImageBuffer(maxImageSize);
            }
        }

        public void SaveBufferedImages(string path) {

            string stationPath = path + Description.Replace("-", "");
            if (!Directory.Exists(stationPath))
                Directory.CreateDirectory(stationPath);
            if (!Directory.Exists(stationPath + @"\Results"))
                Directory.CreateDirectory(stationPath + @"\Results");
            //if (!Directory.Exists(stationPath + @"\Thumbnails"))
            //    Directory.CreateDirectory(stationPath + @"\Thumbnails");

            foreach (string fileName in Directory.GetFiles(stationPath + @"\Results"))
                File.Delete(fileName);

            //foreach (string fileName in Directory.GetFiles(stationPath + @"\Thumbnails"))
            //    File.Delete(fileName);

            if (inspectionBuffer != null) {
                for (int i = 0; i < inspectionBuffer.Capacity; i++) {
                    RawInspectionData data = inspectionBuffer[i];
                    if (data.VialId >= 0)
                        saveBufferedImages(data, stationPath);
                }
            }
        }

        void saveBufferedImages(RawInspectionData inspectionData, string path) {

            Image<Rgb, byte> imgToSave = FromRawToCvImage(inspectionData.Image, inspectionData.ImageWidth, inspectionData.ImageHeight, inspectionData.ImageBpp, false);
            string dateStr = DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss");
            string statDescr = Description.Replace("-", "");
            if (imgToSave != null)
                imgToSave.Save(path + @"\Results\Result_" + statDescr + "_" + dateStr + "_SP_" + inspectionData.Result.SpindleId + "_ID_" + inspectionData.VialId.ToString().PadLeft(3, '0') + ".tiff");
            //thumbToShow.Save(path + @"\Thumbnails\Thumb_" + statDescr + "_" + dateStr + "_SP_" + +inspectionData.Result.SpindleId + "_ID_" + inspectionData.VialId.ToString().PadLeft(3, '0') + ".tiff");
        }

        public void ResetImagesBuffer() {
            if (inspectionBuffer != null) {
                inspectionBuffer.Reset();
            }
        }
    }

    public class BmpEventArgs : EventArgs {
        public Bitmap bmp;
        public BmpEventArgs(Bitmap _bmp) {
            bmp = _bmp;
        }
    }

    internal static class ConcurrentQueueExtensions {
        public static void Clear<T>(this ConcurrentQueue<T> queue) {
            T item;
            while (queue.TryDequeue(out item)) {
                // do nothing
            }
        }
    }

    public class TattileIdentity {
        internal string IP;
        internal int Head;
        internal int ID;

        public override string ToString() {
            return IP + "_" + Head.ToString();
        }
        public override bool Equals(object obj) {
            if (!(obj is TattileIdentity)) return false;
            TattileIdentity other = (TattileIdentity)obj;
            return (IP == other.IP && Head == other.Head);
        }

        public override int GetHashCode() {
            unchecked {
                return ((IP != null ? IP.GetHashCode() : 0) * 397) ^ (Head != null ? Head.GetHashCode() : 0);
            }
        }
    }

    public enum CameraProgramStatus {
        TATTILE_PROGRAM_ERROR = -1,
        TATTILE_PROGRAM_STOP = 0,
        TATTILE_PROGRAM_RUN = 1,
        TATTILE_PROGRAM_STOP_AND_GRABBING = 3
    }

    class RawInspectionData {

        public Int64 VialId { get; internal set; }
        public byte[] Image { get; internal set; }
        public int ImageWidth { get; internal set; }
        public int ImageHeight { get; internal set; }
        public int ImageBpp { get; internal set; }
        public InspectionResults Result { get; internal set; }
        public DateTime VialInspectionTime { get; internal set; }

        public RawInspectionData(int vialId, byte[] image, InspectionResults result, int width, int height, int bpp) {

            VialId = vialId;
            Image = image;
            Result = result;
            ImageWidth = width;
            ImageHeight = height;
            ImageBpp = bpp;
        }

        public RawInspectionData()
            : this(-1, null, null, 0, 0, 24) {
        }
    }

    class RawInspectionBuffer {

        List<RawInspectionData> buffer = null;
        SortedList<Int64, int> vialToBuffer = null;
        int _capacity = 0;
        int nextFreeIndex = 0;
        int _maxImageSize;
        int _additionalIndex = 0;

        public int Capacity {
            get {
                return _capacity;
            }
        }

        public int MaxImageSize {
            get {
                return _maxImageSize;
            }
        }

        public int Count {
            get {
                if (vialToBuffer != null)
                    return vialToBuffer.Count;
                else
                    return 0;
            }
        }

        public RawInspectionBuffer(int capacity) {

            _capacity = capacity;
            buffer = new List<RawInspectionData>(capacity);
            for (int i = 0; i < capacity; i++)
                buffer.Add(new RawInspectionData());
            vialToBuffer = new SortedList<Int64, int>(capacity);
        }

        public void AllocImageBuffer(int maxImageSize) {

            foreach (RawInspectionData iData in buffer) {
                iData.Image = new byte[maxImageSize];
                Array.Clear(iData.Image, 0, maxImageSize);
            }
            _maxImageSize = maxImageSize;
        }

        public void Clear() {

            buffer.Clear();
            vialToBuffer = null;
            GC.Collect();
            for (int i = 0; i < _capacity; i++)
                buffer.Add(new RawInspectionData());
            vialToBuffer = new SortedList<Int64, int>(_capacity);
            nextFreeIndex = 0;
        }

        public void Reset() {

            for (int i = 0; i < _capacity; i++) {
                RawInspectionData data = buffer[i];
                data.VialId = -1;
            }
            vialToBuffer = new SortedList<Int64, int>(_capacity);
            nextFreeIndex = 0;
        }

        public RawInspectionData GetPrevData() {

            RawInspectionData res = null;
            if (buffer[_additionalIndex] != null && buffer[_additionalIndex].Result != null) {
                res = buffer[_additionalIndex];
                int nextAdditionalIndex = _additionalIndex - 1;
                if (nextAdditionalIndex < 0) nextAdditionalIndex = _capacity - 1;
                if (buffer[nextAdditionalIndex] != null && buffer[nextAdditionalIndex].Result != null)
                    _additionalIndex = nextAdditionalIndex;

            }
            return res;
        }

        public void ResetAdditionalIndex() {
            _additionalIndex = nextFreeIndex - 1;
            if (_additionalIndex < 0) _additionalIndex = _capacity - 1;
        }

        public void AddResultData(Int64 vialId, InspectionResults resultData) {

            int index = -1;
            if (vialToBuffer.ContainsKey(vialId))
                index = vialToBuffer[vialId];
            else {
                index = nextFreeIndex;
                RawInspectionData iData = buffer[index];
                // Eliminazione riferimento al precedente vialId dall'indice
                Int64 prevVialId = iData.VialId;
                if (prevVialId >= 0)
                    vialToBuffer.Remove(prevVialId);
                // Inserimento del nuovo vialId nell'indice
                vialToBuffer.Add(vialId, index);
                iData.VialId = vialId;
                iData.VialInspectionTime = DateTime.Now;
                _additionalIndex = nextFreeIndex;
                nextFreeIndex = (nextFreeIndex + 1) % _capacity;
            }
            buffer[index].Result = resultData;
        }

        public void AddImageData(Int64 vialId, byte[] imageData, int width, int height, int bpp) {

            int index = -1;
            if (vialToBuffer.ContainsKey(vialId))
                index = vialToBuffer[vialId];
            else {
                index = nextFreeIndex;
                RawInspectionData iData = buffer[index];
                // Eliminazione riferimento al precedente vialId dall'indice
                Int64 prevVialId = iData.VialId;
                if (prevVialId >= 0)
                    vialToBuffer.Remove(prevVialId);
                // Inserimento del nuovo vialId nell'indice
                vialToBuffer.Add(vialId, index);
                iData.VialId = vialId;
                iData.VialInspectionTime = DateTime.Now;
                _additionalIndex = nextFreeIndex;
                nextFreeIndex = (nextFreeIndex + 1) % _capacity;
            }
            Array.Copy(imageData, buffer[index].Image, imageData.Length);
            buffer[index].ImageWidth = width;
            buffer[index].ImageHeight = height;
            buffer[index].ImageBpp = bpp;
        }

        public bool Contains(Int64 vialId) {

            return vialToBuffer.ContainsKey(vialId);
        }

        public RawInspectionData this[int id] {
            get {
                return buffer[id];
            }
        }
    }
}
