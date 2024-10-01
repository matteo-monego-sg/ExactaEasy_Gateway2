using DisplayManager;
using Emgu.CV;
using Emgu.CV.Structure;
using ExactaEasyCore;
using ExactaEasyEng;
using Hvld.Parser;
using SPAMI.Util.Logger;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GretelClients
{
    class GretelStationBase : DisplayManager.Station 
    {
        // MM-20/08/2024: data structure to hold HVLD2 packets.
        private readonly ConcurrentQueue<byte[]> _hvldFrameBuffer = new ConcurrentQueue<byte[]>();
        // MM-20/08/2024: HVLD2 mex dequeuer.
        private Thread _hvldDequeuer;
        // MM-20/08/2024: are to signal HVLD2 data arrival.
        private readonly AutoResetEvent _hvldResultsAvailable = new AutoResetEvent(false);
        // MM-20/08/2024: waithandles to control the dequeuer thread.
        private WaitHandle[] _hvldResultsEvs = new WaitHandle[2];
        //readonly ConcurrentQueue<ImageAvailableEventArgs> _imagesBuffer = new ConcurrentQueue<ImageAvailableEventArgs>();
        readonly ConcurrentQueue<InspectionResults> _resultsBuffer = new ConcurrentQueue<InspectionResults>();
        readonly AutoResetEvent _imageSnapped = new AutoResetEvent(false);
        const int DefaultQueueCapacity = 10;
        bool _grabbing;
        bool _snapping;
        //bool _setParametersCompletedSuccessfully;
        readonly int _queueCapacity = DefaultQueueCapacity;
        readonly ManualResetEvent _killEv = new ManualResetEvent(false);
        readonly ManualResetEvent _setParametersCompleted = new ManualResetEvent(false);
        readonly AutoResetEvent _readyImagesEv = new AutoResetEvent(false);
        readonly AutoResetEvent _readyResultsEv = new AutoResetEvent(false);
        readonly WaitHandle[] _alertImagesEvs = new WaitHandle[2];
        readonly WaitHandle[] _alertResultsEvs = new WaitHandle[2];
        Thread _alertImagesThread;
        Thread _alertResultsThread;

        //Thread _imagesRecorderThread;
        Rgb blackColor = new Rgb(255, 0, 0);
        internal List<GretelSvc.ExportParameter> ExportParamList = new List<GretelSvc.ExportParameter>();
        List<Tool> toolsParamsList = new List<Tool>();

        Image<Rgb, byte> imgToShow = null;
        Image<Rgb, byte> thumbToShow = null;
        Image<Rgb, byte> lastImgToShow = null;
        Image<Rgb, byte> lastThumbToShow = null;
        Image<Gray, byte> imgToShowRed = null;
        Image<Gray, byte> imgToShowGreen = null;
        Image<Gray, byte> imgToShowBlue = null;
        Image<Gray, byte> thumbToShowRed = null;
        Image<Gray, byte> thumbToShowGreen = null;
        Image<Gray, byte> thumbToShowBlue = null;
        byte[] imgToShowBufferR, imgToShowBufferG, imgToShowBufferB, thumbToShowBufferR, thumbToShowBufferG, thumbToShowBufferB;
        int rejectionCause, lastRejectionCause;
        int TbnWidth, TbnHeight;
        /*int _imgNotJustSaved, _imgSaved, _imgToSave;*/
        bool _isFirstAfterSizeChange;

        //bool _rec = false;
        string _stationSavePath = "";
        //pier: commentato per medline
        //RawInspectionBuffer inspectionBuffer = null;
        readonly AutoResetEvent _writeImagesEv = new AutoResetEvent(false);
        readonly WaitHandle[] _imagesRecorderEvs = new WaitHandle[2];
        readonly int MaxInspectionQueueLength;
        //int inspectionImagesBufferLength = 0;
        //pier: commentato per medline
        //public double RecBufferSize {
        //    get {
        //        return (inspectionBuffer == null || inspectionBuffer.Capacity == 0) ? 0 : (double)inspectionBuffer.Count / (double)inspectionBuffer.Capacity * 100;
        //    }
        //}

        public override int ToolsCount {
            get {
                return GretelSvc.MaxToolsNum;
            }
        }

        private int _idGretel;
        internal int IdGretel
        {
            get
            {
                return _idGretel;
            }
            set
            {
                _idGretel = value;
                foreach (var c in Cameras.Cast<GretelCameraBase>())
                {
                    c.IdGretel = _idGretel;
                }
            }
        }

        private bool _hvldEnabled;

        enum AlertEvents {
            Kill = 0,
            Ready
        }

        public GretelStationBase(StationDefinition stationDefinition)
            : base(stationDefinition) {

            NodeId = stationDefinition.Node;
            HasMeasures = true;
            TbnWidth = TbnHeight = 192;
            //PlanarImage = true;
            //IsNumImagesEditable = true;
            //DefaultNumImages = 1;
            _alertImagesEvs[(int)AlertEvents.Kill] = _killEv;
            _alertImagesEvs[(int)AlertEvents.Ready] = _readyImagesEv;
            _alertResultsEvs[(int)AlertEvents.Kill] = _killEv;
            _alertResultsEvs[(int)AlertEvents.Ready] = _readyResultsEv;
            _imagesRecorderEvs[(int)AlertEvents.Kill] = _killEv;
            _imagesRecorderEvs[(int)AlertEvents.Ready] = _writeImagesEv;
            _hvldResultsEvs[(int)AlertEvents.Kill] = _killEv;
            _hvldResultsEvs[(int)AlertEvents.Ready] = _hvldResultsAvailable;

            if (Cameras.Count > 0) {
                foreach (var c in Cameras.Cast<GretelCameraBase>()) {
                    _queueCapacity = Math.Max(c.BufferSize, _queueCapacity);
                }
            }
            _hvldFrameBuffer.Clear();
            //_imagesBuffer.Clear();
            _resultsBuffer.Clear();
            MaxInspectionQueueLength = stationDefinition.MaxInspectionQueueLength;
            //_imgToSave = stationDefinition.MaxInspectionImgToSave;
        }

        public override void Dispose() {
            Destroy();
            base.Dispose();
        }

        void Destroy() {
            //if (Connected) startRun();
            //snapEv.Reset();
            //readyEv.Reset();
            _killEv.Set();
            Thread.Sleep(100);
            //_imagesBuffer.Clear();
            Disconnect();
            //Initialized = false;
        }

        //public override Dictionary<int, string> GetImagesDumpTypes() {

        //    //Dictionary<int, string> typesDict = new Dictionary<int, string>();
        //    ////string[] values = Enum.GetNames(typeof(ImagesDumpTypes));
        //    //foreach (int i in Enum.GetValues(typeof(ImagesDumpTypes))) {
        //    //    typesDict.Add(i, Enum.GetName(typeof(ImagesDumpTypes), i));
        //    //}
        //    //return typesDict;
        //    return getDictionary<ImagesDumpTypes>();
        //}

        //public override Dictionary<int, string> GetSaveConditions() {

        //    return getDictionary<SaveConditions>();
        //}

        //Dictionary<int, string> getDcitionary<T>() {

        //    Dictionary<int, string> typesDict = new Dictionary<int, string>();
        //    //string[] values = Enum.GetNames(typeof(ImagesDumpTypes));
        //    foreach (int i in Enum.GetValues(typeof(T))) {
        //        typesDict.Add(i, Enum.GetName(typeof(T), i));
        //    }
        //    return typesDict;
        //}

        //public override void StartImagesDump(List<StationDumpSettings> statDumpSettingsCollection, ImagesDumpTypes type, SaveConditions condition, int toSave) {
        //    //TODO
        //}

        //public override void StopImagesDump() {
        //    //TODO
        //}

        public override void Connect() {

            //_imagesBuffer.Clear();
            _resultsBuffer.Clear();
            _killEv.Reset();
            // MM-20/08/2024: check if a camera in the collection has an HVLD visualizer.
            _hvldEnabled = Cameras.Where(x => (x as Camera).Visualizer.Equals("Hvld")).Count() > 0;

            if (_hvldEnabled)
            {
                // Check if the thread object has been instantiated.
                if (_hvldDequeuer is null)
                {
                    // Create a new thread.
                   _hvldDequeuer = new Thread(HvldResultsThread)
                   {
                       //IsBackground = true,
                       //Priority = ThreadPriority.AboveNormal,
                       Name = Description + " HVLD2 Dequeuer Thread",
                   };
                }
                // Check whether the thread is running or not.
                if (!_hvldDequeuer.IsAlive)
                    _hvldDequeuer.Start();
            }

            if (_alertImagesThread == null || _alertImagesThread.IsAlive == false) {
                _alertImagesThread = new Thread(AlertImagesThread) {
                    Name = Description + " Alert Images Thread"
                };
            }
            if (_alertImagesThread.IsAlive == false)
                _alertImagesThread.Start();

            if (HasMeasures == true) {
                if (_alertResultsThread == null || _alertResultsThread.IsAlive == false) {
                    _alertResultsThread = new Thread(AlertResultsThread) {
                        Name = Description + " Alert Results Thread"
                    };
                }
                if (_alertResultsThread.IsAlive == false)
                    _alertResultsThread.Start();
            }

            // pier: commentato per medline
            //if (_imagesRecorderThread == null || _imagesRecorderThread.IsAlive == false) {
            //    _imagesRecorderThread = new Thread(ImagesRecorderThread) {
            //        Name = Description + " Images Recorder",
            //        IsBackground = true
            //    };
            //}
            
            // pier: commentato per medline
            //if (_imagesRecorderThread.IsAlive == false)
            //    _imagesRecorderThread.Start();
        }

        public override void Disconnect() {

            _killEv.Set();
            if (_alertImagesThread != null && _alertImagesThread.IsAlive == true) {
                _alertImagesThread.Join(1000);
            }
            if (_alertResultsThread != null && _alertResultsThread.IsAlive == true) {
                _alertResultsThread.Join(1000);
            }
            // pier: commentato per medline
            //if (_imagesRecorderThread != null && _imagesRecorderThread.IsAlive == true) {
            //    _imagesRecorderThread.Join(1000);
            //}
        }

        /// <summary>
        /// Get stream of images from camera
        /// </summary>
        public override void Grab() {

            //_imagesBuffer.Clear();
            _grabbing = true;
        }

        public override void SetStreamMode(int headNumber, CameraWorkingMode cwm) {
            //if (headNumber != GretelSvc.AllInspectionsActivationNum) {    // valore di default
            //    //headNumber = 0;         // al momento abilito l'ispezione 0...TO THINK and...TODO!
            //    //GretelSvc.ActivateInspection(IdGretel, headNumber, true);   //pier: non serve tenere il bool...????
            //}
            //GretelSvc.ActivateInspection(IdGretel, -1, false);
            GretelSvc.ActivateInspection(IdGretel, IdStation, true);   //pier: non serve tenere il bool...????
        }

        public override Bitmap Snap() {

            Bitmap bmp = null;
            //ImageAvailableEventArgs imgRes = null;
            _snapping = true;
            _imageSnapped.Reset();
            if (_imageSnapped.WaitOne(5000)) {
                //if (_grabbing)
                //    imgRes = _imagesBuffer.TryPeek(out imgRes) ? imgRes : null;
                //else
                //    imgRes = _imagesBuffer.TryDequeue(out imgRes) ? imgRes : null;
                bmp = lastImgToShow.ToBitmap();
            }
            if (bmp != null)
                return (Bitmap)bmp.Clone();
            return null;
        }

        public override void StopGrab() {

            _grabbing = false;
        }

        internal void OnInspectionResults(InspectionResults e) {

            //pier: commentato per medline
            //if (inspectionBuffer != null) {
            //    if (isToBufferResult(SaveConditions.Reject, e) && _rec == true)
            //        inspectionBuffer.AddResultData(e.VialId, e);
            //}
            rejectionCause = e.RejectionCause;
            _resultsBuffer.Enqueue(e);
            _readyResultsEv.Set();
        }

        internal void ChangeImageInfo(GretelSvc.InspectionImageSizeAndType newImageInfo) {

            TbnWidth = newImageInfo.tbnWidth;
            TbnHeight = newImageInfo.tbnHeight;
            Log.Line(LogLevels.Pass, "GretelStationBase.ChangeImageInfo", Description + ": Img(w, h, bpp) = ({0}, {1}, {2}); Thumb(w, h, bpp) = ({3}, {4}, {5})",
                newImageInfo.imageWidth, newImageInfo.imageHeight, newImageInfo.imageDepth, newImageInfo.tbnWidth, newImageInfo.tbnHeight, newImageInfo.tbnDepth);
            //pier: commentato per medline
            //if (inspectionBuffer == null)
            //    inspectionBuffer = new RawInspectionBuffer(MaxInspectionQueueLength);

            int maxImageSize = newImageInfo.imageWidth * newImageInfo.imageHeight * (newImageInfo.imageDepth / 8) +
                               newImageInfo.tbnWidth * newImageInfo.tbnHeight * (newImageInfo.tbnDepth / 8) +
                               GretelSvc.InspResultInfo.GetSize();

            //pier: commentato per medline
            //if (inspectionBuffer.MaxImageSize != maxImageSize) {
            //    inspectionBuffer.Clear();

            //    inspectionBuffer.AllocImageBuffer(maxImageSize);
            //    _isFirstAfterSizeChange = true;
            //}
        }

        internal void UpdateExportParameter() {

            GretelCameraBase _camera = (Cameras.First() as GretelCameraBase);
            _camera.acquisitionParams.Clear();
            _camera.digitizerParams.Clear();
            lock (toolsParamsList) {
                toolsParamsList.Clear();
            }

            foreach (GretelSvc.ExportParameter exportParameter in ExportParamList) {

                string expParamGroupId = GretelSvc.ExportParameter.groupIdToString(exportParameter.hGroupId[0]);
                Parameter newParam = new Parameter();
                newParam.Group = expParamGroupId + ";" + exportParameter.hGroupId[1];
                newParam.ValueType = GretelSvc.GetType(exportParameter.Type);
                newParam.ParamId = ((int)exportParameter.hId).ToString();
                var tempStr = new string(exportParameter.hParentName);
                newParam.ParentName = tempStr.Substring(0, tempStr.IndexOf("\0", StringComparison.Ordinal));
                tempStr = new string(exportParameter.ExportName);
                newParam.ExportName = tempStr.Substring(0, tempStr.IndexOf("\0", StringComparison.Ordinal));
                tempStr = new string(exportParameter.hRealName);
                newParam.Id = tempStr.Substring(0, tempStr.IndexOf("\0", StringComparison.Ordinal));
                tempStr = new string(exportParameter.hDisplayName);
                newParam.Label = tempStr.Substring(0, tempStr.IndexOf("\0", StringComparison.Ordinal));
                tempStr = new string(exportParameter.Description);
                newParam.Description = tempStr.Substring(0, tempStr.IndexOf("\0", StringComparison.Ordinal));
                int stringCount = exportParameter.StringList.Length / GretelSvc.StringLength;
                char[] tempCharArray = new char[GretelSvc.StringLength];
                newParam.AdmittedValues = new List<string>();
                for (int iS = 0; iS < stringCount; iS++) {
                    Array.Copy(exportParameter.StringList, iS * GretelSvc.StringLength, tempCharArray, 0, GretelSvc.StringLength);
                    tempStr = new string(tempCharArray);
                    tempStr = tempStr.Substring(0, tempStr.IndexOf("\0", StringComparison.Ordinal));
                    if (string.IsNullOrEmpty(tempStr) == false)
                        newParam.AdmittedValues.Add(tempStr);
                }
                newParam.Decimals = (int)exportParameter.Decimals;
                newParam.Increment = exportParameter.Increment;
                if (string.IsNullOrEmpty(newParam.Id)) {
                    newParam.Id = newParam.Label;
                }
                try {
                    switch (newParam.ValueType) {
                        case "bool":
                            bool bValue = (exportParameter.MinValue[0] != 0);
                            newParam.MinValue = bValue.ToString();
                            bValue = (exportParameter.MaxValue[0] != 0);
                            newParam.MaxValue = bValue.ToString();
                            bValue = (exportParameter.Value[0] != 0);
                            newParam.Value = bValue.ToString();
                            break;
                        case "int":
                            int iValue = BitConverter.ToInt32(exportParameter.MinValue, 0);
                            newParam.MinValue = iValue.ToString(CultureInfo.InvariantCulture).Replace(",", "");
                            iValue = BitConverter.ToInt32(exportParameter.MaxValue, 0);
                            newParam.MaxValue = iValue.ToString(CultureInfo.InvariantCulture).Replace(",", "");
                            iValue = BitConverter.ToInt32(exportParameter.Value, 0);
                            newParam.Value = iValue.ToString(CultureInfo.InvariantCulture).Replace(",", "");
                            break;
                        case "double":
                            double dValue = BitConverter.ToDouble(exportParameter.MinValue, 0);
                            newParam.MinValue = dValue.ToString("N" + newParam.Decimals, CultureInfo.InvariantCulture).Replace(",", "");
                            dValue = BitConverter.ToDouble(exportParameter.MaxValue, 0);
                            newParam.MaxValue = dValue.ToString("N" + newParam.Decimals, CultureInfo.InvariantCulture).Replace(",", "");
                            dValue = BitConverter.ToDouble(exportParameter.Value, 0);
                            newParam.Value = dValue.ToString("N" + newParam.Decimals, CultureInfo.InvariantCulture).Replace(",", "");
                            break;
                        case "string":
                            tempStr = Encoding.ASCII.GetString(exportParameter.MinValue);
                            newParam.MinValue = tempStr.Substring(0, tempStr.IndexOf("\0", StringComparison.Ordinal));
                            tempStr = Encoding.ASCII.GetString(exportParameter.MaxValue);
                            newParam.MaxValue = tempStr.Substring(0, tempStr.IndexOf("\0", StringComparison.Ordinal));
                            tempStr = Encoding.ASCII.GetString(exportParameter.Value);
                            newParam.Value = tempStr.Substring(0, tempStr.IndexOf("\0", StringComparison.Ordinal));
                            break;
                        default:
                            newParam.MinValue = newParam.MaxValue = newParam.Value = "";
                            Log.Line(LogLevels.Error, "GretelStationBase.UpdateExportParameter", "Parameter type unknown!");
                            break;
                    }
                }
                catch (Exception ex) {
                    Log.Line(LogLevels.Warning, "GretelStationBase.UpdateExportParameter", "Error while casting parameter value: " + ex.Message);
                }
                tempStr = new string(exportParameter.UnitMeasure);
                newParam.MeasureUnit = tempStr.Substring(0, tempStr.IndexOf("\0", StringComparison.Ordinal));
                newParam.IsVisible = 1;
                Tool currTool = null;

                switch (expParamGroupId) {
                    case "CAMERA":         // CAMERA
                        _camera.acquisitionParams.Add(newParam);
                        break;
                    case "DIGITIZER":         // DIGITIZER
                        _camera.digitizerParams.Add(newParam);
                        break;
                    case "TOOL ENABLE":         // TOOL ENABLE
                        currTool = toolsParamsList.Find(t => t.Id == exportParameter.hId);
                        if (currTool == null) {
                            currTool = new Tool();
                            currTool.Id = exportParameter.hId;
                            currTool.ToolParameters = new ParameterCollection<Parameter>();
                            lock (toolsParamsList) {
                                toolsParamsList.Add(currTool);
                            }
                        }
                        currTool.Label = newParam.ExportName;
                        //currTool.ExportName = newParam.ExportName;
                        //currTool.TypeName = newParam.Label;
                        bool bEnable;
                        if (bool.TryParse(newParam.Value, out bEnable) == false) {
                            Log.Line(LogLevels.Warning, "GretelStationBase.UpdateExportParameter", "Error while parsing tool enable");
                        }
                        currTool.Active = bEnable;
                        newParam.Id = "Active";
                        newParam.ExportName = "Active";
                        currTool.ToolParameters.Insert(0, newParam);
                        break;
                    case "TOOL PARAMS":         // TOOL PARAMS
                        currTool = toolsParamsList.Find(t => t.Id == exportParameter.hGroupId[1]);
                        if (currTool == null) {
                            currTool = new Tool();
                            currTool.Id = exportParameter.hGroupId[1];
                            currTool.ToolParameters = new ParameterCollection<Parameter>();
                            lock (toolsParamsList) {
                                toolsParamsList.Add(currTool);
                            }
                        }
                        currTool.Label = newParam.ParentName;
                        currTool.ToolParameters.Add(newParam);
                        break;
                    case "TOOL OUTPUT":         // TOOL OUTPUT
                        currTool = toolsParamsList.Find(t => t.Id == exportParameter.hGroupId[1]);
                        if (currTool == null) {
                            currTool = new Tool();
                            currTool.Id = exportParameter.hGroupId[1];
                            currTool.ToolParameters = new ParameterCollection<Parameter>();
                            lock (toolsParamsList) {
                                toolsParamsList.Add(currTool);
                            }
                        }
                        currTool.Label = newParam.ParentName;
                        ToolOutput currToolOut = currTool.ToolOutputs.Find(to => to.ParamId == newParam.ParamId);
                        if (currToolOut == null) {
                            currToolOut = new ToolOutput(newParam);
                        }
                        //currToolOut.Decimals = newParam.Decimals;
                        //currToolOut.Description = newParam.Description;
                        //currToolOut.ExportName = newParam.ExportName;
                        //currToolOut.Group = newParam.Group;
                        //currToolOut.Id = newParam.Id;
                        //currToolOut.Label = newParam.Label;
                        //currToolOut.Increment = newParam.Increment;
                        currToolOut.IsUsed = true;
                        //currToolOut.IsVisible = newParam.IsVisible;
                        //currToolOut.MaxValue = newParam.MaxValue;
                        //currToolOut.MinValue = newParam.MinValue;
                        //currToolOut.MeasureUnit = newParam.MeasureUnit;
                        //currToolOut.ParamId = newParam.ParamId;
                        //currToolOut.ValueType = newParam.ValueType;
                        //currToolOut.Value = newParam.Value;
                        currTool.ToolOutputs.Add(currToolOut);
                        break;
                    case "STROBO":         // STROBO
                        Log.Line(LogLevels.Warning, "GretelStationBase.UpdateExportParameter", "Parameter group exportation not supported yet!");
                        break;
                    case "LIGHT SENSOR":         // LIGHT SENSOR
                        Log.Line(LogLevels.Warning, "GretelStationBase.UpdateExportParameter", "Parameter group exportation not supported yet!");
                        break;
                    default:
                        Log.Line(LogLevels.Error, "GretelStationBase.UpdateExportParameter", "Parameter group unknown!");
                        break;
                }

                //}
                //else {
                //}
            }
            _setParametersCompleted.Set();
            // Log.Line(LogLevels.Pass, "GretelStationBase.UpdateExportParameter", "Node {0} Station {1}: New exported parameters: {2}", NodeId, IdStation, ExportParamList.Count);
            OnExportedParametersUpdated(this, EventArgs.Empty);
        }

        public override void SetParameters(ParameterCollection<Parameter> parameters) {

            //if (parameters.Count == 0)
            //    return;

            _setParametersCompleted.Reset();

            Task setParametersTask = new Task(new Action(() => {

                CultureInfo ci = new CultureInfo(AppEngine.Current.CurrentContext.CultureCode);
                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;

                GretelSvc.ExportParameter[] exportParameters = new GretelSvc.ExportParameter[parameters.Count];
                for (int iP = 0; iP < parameters.Count; iP++) {
                    Parameter p = parameters[iP];
                    GretelSvc.ExportParameter newExportParameter = new GretelSvc.ExportParameter(IdStation, p);
                    exportParameters[iP] = newExportParameter;
                }
                
                //_setParametersCompletedSuccessfully = false;
                Stopwatch timeMon = new Stopwatch();
                //timeMon.Start();
                if (parameters.Count > 0)
                {
                    Log.Line(LogLevels.Pass, "GretelStationBase.SetParameters", "Node: {0} Station {1} SetParameters", NodeId, IdStation);
                    GretelSvc.SendParameters(IdGretel, exportParameters);
                }
                else
                {
                    _setParametersCompleted.Set();
                }
                bool _setParametersCompletedSuccessfully = _setParametersCompleted.WaitOne(5000);
                if (_setParametersCompletedSuccessfully == true) {
                    OnSetParametersCompleted(this, new DisplayManager.MessageEventArgs("SUCCESS"));
                }
                else {
                    OnSetParametersCompleted(this, new DisplayManager.MessageEventArgs("TIMEOUT"));
                }
                //timeMon.Stop();
                //Log.Line(LogLevels.Pass, "GretelStationBase.SetParameters", "Set parameters elapsed time: " + timeMon.ElapsedMilliseconds + " ms");
            }));
            setParametersTask.Start();
        }

        public override Tool GetTool(int toolId) {

            Tool res = new Tool();
            lock (toolsParamsList) {
                res = toolsParamsList.Find(t => t.Id == toolId);
                //if (tool != null) {
                //    //res.Add(vfdbgnndhf        ENABLE
                //    res.AddRange(tool.ToolParameters);
                //}
            }
            return res;
        }

        //string groupIdToString(byte groupId) {

        //    string res = "";
        //    switch (groupId) {
        //        case 0:         // CAMERA
        //            res = "CAMERA";
        //            break;
        //        case 1:         // DIGITIZER
        //            res = "DIGITIZER";
        //            break;
        //        case 2:         // TOOL ENABLE
        //            res = "TOOL ENABLE";
        //            break;
        //        case 3:         // TOOL PARAMS
        //            res = "TOOL PARAMS";
        //            break;
        //        case 4:         // TOOL OUTPUT
        //            res = "TOOL OUTPUT";
        //            break;
        //        case 5:         // STROBO
        //            res = "STROBO";
        //            break;
        //        case 6:         // LIGHT SENSOR
        //            res = "LIGHT SENSOR";
        //            break;
        //        default:
        //            Log.Line(LogLevels.Error, "GretelCameraBase.groupIdToString", "Parameter group unknown!");
        //            break;
        //    }
        //    return res;
        //}

        bool isToBufferResult(SaveConditions condition, InspectionResults e) {

            bool ris = false;
            switch (condition) {
                case SaveConditions.Reject:
                    ris = e.IsReject;
                    break;
                default:
                    ris = false;
                    break;
            }
            // se è la prima dopo un cambio size era ancora in pancia a Gretel prima del cambio e non la salvo
            if (_isFirstAfterSizeChange == true) {
                _isFirstAfterSizeChange = false;
                ris = false;
            }
            return ris;
        }

        internal void OnClientImagesResults(ByteArrayEventArgs e) {

            try {
                GretelSvc.InspResultInfo resInfo = DecodeImagesResultDataHeader(e.Res);
                //pier: commentato per medline
                //if (inspectionBuffer != null && inspectionBuffer.Contains(resInfo.sample_id)) {
                //    inspectionBuffer.AddImageData(resInfo.sample_id, e.Res);
                //    _imgNotJustSaved++;
                //    _writeImagesEv.Set();
                //    if (_imgNotJustSaved >= _imgToSave) {
                //        _rec = false;
                //        OnRecBufferState(this, new RecordStateEventArgs(_rec, RecBufferSize));
                //    }
                //}
                if (!_grabbing && !_snapping) return;

                DecodeImagesResultsData(resInfo, e.Res);
                lastRejectionCause = rejectionCause;
                _readyImagesEv.Set();
                if (_snapping)
                    _imageSnapped.Set();
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "GretelStationBase.OnClientImagesResults", Description + ": Error decoding Gretel images: " + ex.Message);
            }
        }
        /// <summary>
        /// MM-11/01/2024: HVLD2.0 evo.
        /// MM-20/08/2024: HVLD2.0 => added a queue to decouple signal elaboration from the main queue.
        /// 
        /// OnClientHvldDataResults are fired so fast the _hvldFrameBuffer is always locked.
        /// </summary>
        internal void OnClientHvldDataResults(ByteArrayEventArgs e)
        {
            // Creates a new frame object by parsing the array data.
            //var frame = new HvldFrame(e.Res);
            // If we reached 5 elements, pop the first element as its deprecated.
            if (_hvldFrameBuffer.Count >= 5)
                _hvldFrameBuffer.TryDequeue(out _);
            // Add the new item to the queue.
            _hvldFrameBuffer.Enqueue(e.Res);
            _hvldResultsAvailable.Set();
        }
        /// <summary>
        /// 
        /// </summary>
        private void createStationSavePath(string path) {

            /*_imgSaved = _imgNotJustSaved = 0;*/
            _stationSavePath = path + Description.Replace("-", "");
            if (!Directory.Exists(_stationSavePath))
                Directory.CreateDirectory(_stationSavePath);
            if (!Directory.Exists(_stationSavePath + @"\Results"))
                Directory.CreateDirectory(_stationSavePath + @"\Results");
            if (!Directory.Exists(_stationSavePath + @"\Thumbnails"))
                Directory.CreateDirectory(_stationSavePath + @"\Thumbnails");

        }

        public override void SaveBufferedImages(string path, SaveConditions sc, int toSave) {

            //medline
            return;
            //_imgToSave = toSave;
            //createStationSavePath(path);
            //_rec = (sc != SaveConditions.Never);
            ////OnRecBufferState(this, new RecordStateEventArgs(_rec, RecBufferSize));

            //if (_rec == true) {
            //    ResetImagesBuffer(path, false);
            //}
            //if (_imgNotJustSaved != _imgSaved) {
            //    Log.Line(LogLevels.Warning, "GretelStationBase.SaveBufferedImages", "Images to save: {0}, Images saved: {1}", _imgNotJustSaved, _imgSaved);
            //}
        }

        void writeImage(RawInspectionData inspectionData, string path) {

            try {
                GretelSvc.InspResultInfo resInfo = DecodeImagesResultDataHeader(inspectionData.Image);
                DecodeImagesResultsData(resInfo, inspectionData.Image);
                string dateStr = DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss");
                string statDescr = Description.Replace("-", "");
                imgToShow.Save(path + @"\Results\Result_" + statDescr + "_" + dateStr + "_SP_" + inspectionData.Result.SpindleId + "_ID_" + inspectionData.VialId.ToString().PadLeft(3, '0') + ".tiff");
                thumbToShow.Save(path + @"\Thumbnails\Thumb_" + statDescr + "_" + dateStr + "_SP_" + +inspectionData.Result.SpindleId + "_ID_" + inspectionData.VialId.ToString().PadLeft(3, '0') + ".tiff");
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "GretelStationBase.OnClientImagesResults", Description + ": Error saving Gretel images: " + ex.Message);
                //pier: commentato per medline
                //_rec = false;
                //OnRecBufferState(this, new RecordStateEventArgs(_rec, RecBufferSize));
            }
        }

        public override void ResetImagesBuffer(string path, bool freeFolders) {

            //medline
            return;
            //createStationSavePath(path);

            ////if (inspectionBuffer != null) {
            ////    inspectionBuffer.Reset();
            ////}
            //if (freeFolders == true && Directory.Exists(_stationSavePath) == true) {
            //    foreach (string fileName in Directory.GetFiles(_stationSavePath + @"\Results"))
            //        File.Delete(fileName);

            //    foreach (string fileName in Directory.GetFiles(_stationSavePath + @"\Thumbnails"))
            //        File.Delete(fileName);
            //}
        }

        internal unsafe GretelSvc.InspResultInfo DecodeImagesResultDataHeader(byte[] dataToDecode) {

            GretelSvc.InspResultInfo resultInfo;
            fixed (byte* pIncData = &dataToDecode[0]) {
                var ptrIncData = (IntPtr)pIncData;
                resultInfo = (GretelSvc.InspResultInfo)Marshal.PtrToStructure(ptrIncData, typeof(GretelSvc.InspResultInfo));
            }
            return resultInfo;
        }

        internal unsafe void DecodeImagesResultsData(GretelSvc.InspResultInfo resultInfo, byte[] dataToDecode) 
        {

            int offsImageData = sizeof(GretelSvc.InspResultInfo);
            int offsTbnData;
            int imageSourceSize, tbnSourceSize;
            imageSourceSize = resultInfo.ImageWidth * resultInfo.ImageHeight * (resultInfo.ImageDepth / 8);
            tbnSourceSize = resultInfo.TbnWidth * resultInfo.TbnHeight * (resultInfo.TbnDepth / 8);
            offsTbnData = offsImageData + imageSourceSize;

            int wImgCv4byte = resultInfo.ImageWidth / 4 * 4 + (Math.Min(1, (resultInfo.ImageWidth % 4)) * 4);
            int wTbnCv4byte = resultInfo.TbnWidth / 4 * 4 + (Math.Min(1, (resultInfo.TbnWidth % 4)) * 4);

            if (resultInfo.ImageWidth > 0 && resultInfo.ImageHeight > 0 && (imgToShow == null || resultInfo.ImageWidth != imgToShow.Width || resultInfo.ImageHeight != imgToShow.Height)) {

                imgToShowBufferR = new byte[wImgCv4byte * resultInfo.ImageHeight];
                imgToShowBufferG = new byte[wImgCv4byte * resultInfo.ImageHeight];
                imgToShowBufferB = new byte[wImgCv4byte * resultInfo.ImageHeight];
                if (!AllocImage(ref imgToShow, resultInfo.ImageWidth, resultInfo.ImageHeight))
                    return;
                if (!AllocImage(ref imgToShowRed, resultInfo.ImageWidth, resultInfo.ImageHeight))
                    return;
                if (!AllocImage(ref imgToShowGreen, resultInfo.ImageWidth, resultInfo.ImageHeight))
                    return;
                if (!AllocImage(ref imgToShowBlue, resultInfo.ImageWidth, resultInfo.ImageHeight))
                    return;
            }
            if (resultInfo.TbnWidth > TbnWidth || resultInfo.TbnHeight > TbnHeight) {
                Log.Line(LogLevels.Warning, "GretelStationBase.DecodeImagesResultsData", "Thumbnail size higher than expected (w,h)=({0}x{1})", TbnWidth, TbnHeight);
                tbnSourceSize = 0;  //forzo a 0 in modo da non considerare il santino nella decodifica
            }
            else {
                if (resultInfo.TbnWidth > 0 && resultInfo.TbnHeight > 0 && (thumbToShow == null/* || resultInfo.TbnWidth != thumbToShow.Width || resultInfo.TbnHeight != thumbToShow.Height*/)) {

                    thumbToShowBufferR = new byte[TbnWidth * TbnHeight];
                    thumbToShowBufferG = new byte[TbnWidth * TbnHeight];
                    thumbToShowBufferB = new byte[TbnWidth * TbnHeight];
                    if (!AllocImage(ref thumbToShow, TbnWidth, TbnHeight))
                        return;
                    if (!AllocImage(ref thumbToShowRed, TbnWidth, TbnHeight))
                        return;
                    if (!AllocImage(ref thumbToShowGreen, TbnWidth, TbnHeight))
                        return;
                    if (!AllocImage(ref thumbToShowBlue, TbnWidth, TbnHeight))
                        return;
                }
            }
            if (imageSourceSize != 0 && imgToShow != null) {
                int chSize = resultInfo.ImageWidth * resultInfo.ImageHeight;
                if (wImgCv4byte == resultInfo.ImageWidth) {
                    Array.Copy(dataToDecode, offsImageData, imgToShowBufferR, 0, chSize);
                    if (resultInfo.ImageDepth == 24) {
                        Array.Copy(dataToDecode, offsImageData + chSize, imgToShowBufferG, 0, chSize);
                        Array.Copy(dataToDecode, offsImageData + 2 * chSize, imgToShowBufferB, 0, chSize);
                    }
                }
                else {
                    for (int iH = 0; iH < resultInfo.ImageHeight; iH++) {
                        int iHsrcOffset = iH * resultInfo.ImageWidth;
                        int iHdstOffset = iH * wImgCv4byte;
                        Array.Copy(dataToDecode, offsImageData + iHsrcOffset, imgToShowBufferR, iHdstOffset, resultInfo.ImageWidth);
                        if (resultInfo.ImageDepth == 24) {
                            Array.Copy(dataToDecode, offsImageData + chSize + iHsrcOffset, imgToShowBufferG, iHdstOffset, resultInfo.ImageWidth);
                            Array.Copy(dataToDecode, offsImageData + 2 * chSize + iHsrcOffset, imgToShowBufferB, iHdstOffset, resultInfo.ImageWidth);
                        }
                    }
                }
                imgToShowRed.Bytes = imgToShowBufferR;
                imgToShowGreen.Bytes = imgToShowBufferG;
                imgToShowBlue.Bytes = imgToShowBufferB;
                lock (imgToShow) {
                    if (resultInfo.ImageDepth == 24)
                        CvInvoke.cvMerge(imgToShowRed.Ptr, imgToShowGreen.Ptr, imgToShowBlue.Ptr, IntPtr.Zero, imgToShow.Ptr);
                    else
                        CvInvoke.cvMerge(imgToShowRed.Ptr, imgToShowRed.Ptr, imgToShowRed.Ptr, IntPtr.Zero, imgToShow.Ptr);
                    lastImgToShow = imgToShow;
                }
            }
            else
                lastImgToShow = null;

            if (tbnSourceSize != 0 && thumbToShow != null) {
                Array.Clear(thumbToShowBufferR, 0, thumbToShowBufferR.Length);
                Array.Clear(thumbToShowBufferG, 0, thumbToShowBufferG.Length);
                Array.Clear(thumbToShowBufferB, 0, thumbToShowBufferB.Length);
                int chSize = resultInfo.TbnWidth * resultInfo.TbnHeight;
                if ((wTbnCv4byte == resultInfo.TbnWidth) && (resultInfo.TbnWidth == thumbToShowRed.Width)) {

                    Array.Copy(dataToDecode, offsTbnData, thumbToShowBufferR, 0, chSize);
                    if (resultInfo.TbnDepth == 24) {
                        Array.Copy(dataToDecode, offsTbnData + chSize, thumbToShowBufferG, 0, chSize);
                        Array.Copy(dataToDecode, offsTbnData + 2 * chSize, thumbToShowBufferB, 0, chSize);
                    }
                }
                else {
                    for (int iH = 0; iH < resultInfo.TbnHeight; iH++) {
                        int iHsrcOffset = iH * resultInfo.TbnWidth;
                        int iHdstOffset = iH * TbnWidth;
                        Array.Copy(dataToDecode, offsTbnData + iHsrcOffset, thumbToShowBufferR, iHdstOffset, resultInfo.TbnWidth);
                        if (resultInfo.TbnDepth == 24) {
                            Array.Copy(dataToDecode, offsTbnData + chSize + iHsrcOffset, thumbToShowBufferG, iHdstOffset, resultInfo.TbnWidth);
                            Array.Copy(dataToDecode, offsTbnData + 2 * chSize + iHsrcOffset, thumbToShowBufferB, iHdstOffset, resultInfo.TbnWidth);
                        }
                    }
                }

                thumbToShowRed.Bytes = thumbToShowBufferR;
                thumbToShowGreen.Bytes = thumbToShowBufferG;
                thumbToShowBlue.Bytes = thumbToShowBufferB;
                lock (thumbToShow) {
                    if (resultInfo.TbnDepth == 24)
                        CvInvoke.cvMerge(thumbToShowRed.Ptr, thumbToShowGreen.Ptr, thumbToShowBlue.Ptr, IntPtr.Zero, thumbToShow.Ptr);
                    else
                        CvInvoke.cvMerge(thumbToShowRed.Ptr, thumbToShowRed.Ptr, thumbToShowRed.Ptr, IntPtr.Zero, thumbToShow.Ptr);
                    lastThumbToShow = thumbToShow;
                }
            }
            else
                lastThumbToShow = null;
        }

        //protected override void FreeImages() {

        //    if (imgToShow != null) imgToShow.Dispose();
        //    if (thumbToShow != null) thumbToShow.Dispose();
        //    if (imgToShowRed != null) imgToShowRed.Dispose();
        //    if (imgToShowGreen != null) imgToShowGreen.Dispose();
        //    if (imgToShowBlue != null) imgToShowBlue.Dispose();
        //    if (thumbToShowRed != null) thumbToShowRed.Dispose();
        //    if (thumbToShowGreen != null) thumbToShowGreen.Dispose();
        //    if (thumbToShowBlue != null) thumbToShowBlue.Dispose();
        //    imgToShow = thumbToShow = null;
        //    imgToShowRed = imgToShowGreen = imgToShowBlue = null;
        //    thumbToShowRed = thumbToShowGreen = thumbToShowBlue = null;
        //    imgToShowBufferR = imgToShowBufferG = imgToShowBufferB = null;
        //    base.FreeImages();
        //}

        //void EnqueueImagesResults(ImageAvailableEventArgs imgRes) {
        //    if (imageAvailableHandlerCount <= 0) return;
        //    if (_imagesBuffer.Count > 3)
        //        Debug.Print(IdStation.ToString(CultureInfo.InvariantCulture) + " - " + _imagesBuffer.Count);
        //    if (_imagesBuffer.Count < _queueCapacity) {
        //        _imagesBuffer.Enqueue(imgRes);
        //        //Log.Line(LogLevels.Pass, "ScreenGridDisplay.RenderImage", "ENQUEUE!!!!");
        //    }
        //    //ImagesBuffer.Enqueue(imgRes.Clone());
        //    else {
        //        Log.Line(LogLevels.Warning, "GretelStationBase.EnqueueImagesResults", "Queue full");
        //        _imagesBuffer.Clear();
        //    }
        //}


        void AlertImagesThread() {

            while (true) {
                var res = WaitHandle.WaitAny(_alertImagesEvs, 20);
                if (res == WaitHandle.WaitTimeout) continue;
                var ret = (AlertEvents)Enum.Parse(typeof(AlertEvents), res.ToString(CultureInfo.InvariantCulture));
                if (ret == AlertEvents.Kill) break; //kill ev
                if (ret != AlertEvents.Ready) continue;
                bool isReject = (lastRejectionCause == 0) ? false : true;
                OnImageAvailable(this, new ImageAvailableEventArgs(lastImgToShow, lastThumbToShow, isReject, lastRejectionCause));
            }
        }

        void AlertResultsThread() {

            while (true) {
                var res = WaitHandle.WaitAny(_alertResultsEvs, 20);
                if (res == WaitHandle.WaitTimeout) continue;
                var ret = (AlertEvents)Enum.Parse(typeof(AlertEvents), res.ToString(CultureInfo.InvariantCulture));
                if (ret == AlertEvents.Kill) break; //kill ev
                if (ret != AlertEvents.Ready) continue;
                InspectionResults measuresRes;
                if (_resultsBuffer.TryDequeue(out measuresRes)) {
                    OnMeasuresAvailable(this, new MeasuresAvailableEventArgs(measuresRes));
                }
            }
        }

        private void HvldResultsThread()
        {
            while (true)
            {
                var res = WaitHandle.WaitAny(_hvldResultsEvs, 20);
                if (res == WaitHandle.WaitTimeout)
                    continue;
                var ret = (AlertEvents)Enum.Parse(typeof(AlertEvents), res.ToString(CultureInfo.InvariantCulture));
                if (ret == AlertEvents.Kill)
                    break; //kill ev
                if (ret != AlertEvents.Ready)
                    continue;

                byte[] hvldParams;

                if (_hvldFrameBuffer.TryDequeue(out hvldParams))
                {
                    //if (_framesDisplayed >= FRAME_DROP_COUNT)
                    //{
                    //    _framesDisplayed = 0;
                    //    continue;
                    //}
                    // Creates a new frame object by parsing the array data.
                    OnHvldDataAvailable(this, new HvldDataAvailableEventArgs(new HvldFrame(hvldParams)));
                    //_framesDisplayed++;
                }
            }
        }

        //pier: commentato per medline
        //void ImagesRecorderThread() {

        //    while (true) {

        //        if (inspectionBuffer == null) {
        //            Thread.Sleep(50);
        //            continue;
        //        }
        //        int queueSize = inspectionBuffer.Count;
        //        if (queueSize > 0)
        //            _readyResultsEv.Set();
        //        if (queueSize > inspectionBuffer.Capacity - 1) {
        //            Log.Line(LogLevels.Warning, "GretelStationBase.AlertResultsThread", Description + ": Images recorder queue full ({0}/{1})", queueSize, inspectionBuffer.Capacity);
        //        }
        //        var res = WaitHandle.WaitAny(_imagesRecorderEvs, 20);
        //        if (res == WaitHandle.WaitTimeout) continue;
        //        var ret = (AlertEvents)Enum.Parse(typeof(AlertEvents), res.ToString(CultureInfo.InvariantCulture));
        //        if (ret == AlertEvents.Kill) break; //kill ev
        //        if (ret != AlertEvents.Ready) continue;

        //        if (inspectionBuffer != null) {
        //            OnRecBufferState(this, new RecordStateEventArgs(_rec, RecBufferSize));
        //            for (int i = 0; i < inspectionBuffer.Capacity; i++) {
        //                RawInspectionData data = inspectionBuffer[i];
        //                if (data.VialId >= 0 && data.Completed == true) {
        //                    writeImage(data, _stationSavePath);
        //                    data.Reset();
        //                    inspectionBuffer.Count--;
        //                    _imgSaved++;
        //                }
        //            }
        //            if (_imgNotJustSaved - _imgSaved > 5) {
        //                Log.Line(LogLevels.Debug, "GretelStationBase.ImagesRecorderThread", "Images to save: {0}, Images saved: {1}", _imgNotJustSaved, _imgSaved);
        //            }
        //            Thread.Sleep(5);
        //        }
        //    }
        //}

        public override void HardReset() {
            throw new NotImplementedException();
        }

        public override void SetPrevImage() {

            //if (inspectionBuffer != null && inspectionBuffer.Count > 0) {
            //    RawInspectionData rid = inspectionBuffer.GetPrevData();
            //    if (rid == null) return;
            //    Image<Rgb, byte> newImg = new Image<Rgb, byte>(rid.ImageWidth, rid.ImageHeight);
            //    newImg.Bytes = rid.Image;
            //    OnAlternativeImageAvailable(this, new ImageAvailableEventArgs(newImg, null, rid.Result.IsReject));
            //}
        }

        public override void SetMainImage() {

            OnImageAvailable(this, new ImageAvailableEventArgs(CurrentImage, CurrentThumbnail, CurrentIsReject, CurrentRejectionBit));
        }
    }

    class RawInspectionData {

        public Int64 VialId { get; internal set; }
        public byte[] Image { get; internal set; }
        public InspectionResults Result { get; internal set; }
        public DateTime VialInspectionTime { get; internal set; }
        public bool Completed { get; internal set; }

        public RawInspectionData(int vialId, byte[] image, InspectionResults result) {

            VialId = vialId;
            Image = image;
            Result = result;
        }

        public RawInspectionData()
            : this(-1, null, null) {
        }

        public void Reset() {

            VialId = -1;
            Completed = false;
            Result = null;
        }
    }

    class RawInspectionBuffer {

        List<RawInspectionData> buffer = null;
        SortedList<Int64, int> vialToBuffer = null;
        int _capacity = 0;
        int nextFreeIndex = 0;
        int _maxImageSize;
        int _count = 0;

        public int Count {
            get {
                return _count;
            }
            set {
                _count = value;
            }
        }

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
            _count = 0;
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
                data.Reset();
            }
            _count = 0;
            vialToBuffer = new SortedList<Int64, int>(_capacity);
            nextFreeIndex = 0;
        }

        public void AddResultData(Int64 vialId, InspectionResults resultData) {

            if (buffer.Count <= 0)
                return;
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
                calcNextFreeIndex();
            }
            buffer[index].Result = resultData;
        }

        public void AddImageData(Int64 vialId, byte[] imageData) {

            if (buffer.Count <= 0)
                return;
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
                calcNextFreeIndex();
            }
            if (imageData.Length > buffer[index].Image.Length)
                Log.Line(LogLevels.Error, "RawInspectionBuffer.AddImageData", "Input image length ({0} B) does not correspond to the expected one ({1} B)", imageData.Length, buffer[index].Image.Length);
            Array.Copy(imageData, buffer[index].Image, imageData.Length);
            buffer[index].Completed = true;
            _count++;
        }

        public bool Contains(Int64 vialId) {

            return vialToBuffer.ContainsKey(vialId);
        }

        public RawInspectionData this[int id] {
            get {
                return buffer[id];
            }
        }

        void calcNextFreeIndex() {

            nextFreeIndex = (nextFreeIndex + 1) % _capacity;
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
}