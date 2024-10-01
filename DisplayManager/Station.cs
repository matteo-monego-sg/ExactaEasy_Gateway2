using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Reflection;
using SPAMI.Util.Logger;
using ExactaEasyCore;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Diagnostics;
using ExactaEasyEng;

namespace DisplayManager {

    public interface IStation {

        event EventHandler<ImageAvailableEventArgs> ImageAvailable;
        event EventHandler<ImageAvailableEventArgs> AlternativeImageAvailable;
        //event EventHandler<ImagesResultsAvailableEventArgs> ImagesResultsAvailable;
        event EventHandler<MeasuresAvailableEventArgs> MeasuresAvailable;
        event EventHandler<RecordStateEventArgs> RecBufferState;
        event EventHandler<MessageEventArgs> SetParametersCompleted;
        event EventHandler ExportedParametersUpdated;
        event EventHandler<HvldDataAvailableEventArgs> HvldDataAvailable;

        int IdStation { get; }
        //int DisplayPosition { get; set; }
        bool Enabled { get; set; }
        int NodeId { get; }
        string Description { get; set; }
        string ProviderName { get; set; }
        CameraCollection Cameras { get; }
        bool Initialized { get; }
        bool Connected { get; }
        Image<Rgb, byte> CurrentImage { get; }
        Image<Rgb, byte> CurrentThumbnail { get; }
        bool? CurrentIsReject { get; }
        int CurrentRejectionBit { get; }
        Image<Rgb, byte> AlternativeImage { get; }
        Rectangle ZoomAOI { get; set; }
        MachineModeEnum MachineMode { get; set; }
        int ToolsCount { get; }
        LiveImageFilter LiveImageFilter { get; }

        void Connect();
        void Disconnect();
        void Dispose();

        void HardReset();
        void Grab();
        void StopGrab();
        Bitmap Snap();
        void SetStreamMode(int headNumber, CameraWorkingMode cwm);
        void AdjustImage(Image<Rgb, byte> srcImage, int width, int height, bool keepProp, Emgu.CV.CvEnum.INTER interpolationType, ref Image<Rgb, byte> dstImage, out Rectangle aoiUsed);

        bool HasMeasures { get; set; }
        bool DumpResultsEnabled { get; set; }
        void SetPrevImage();
        void SetMainImage();
        void SetParameters(ParameterCollection<Parameter> parameters);

        Tool GetTool(int toolId);

        // IMAGES DUMP
        //Dictionary<int, string> GetImagesDumpTypes();
        //Dictionary<int, string> GetSaveConditions();
       // bool IsNumImagesEditable { get; }
        //int DefaultNumImages { get; }
        //void StartImagesDump(ImagesDumpTypes type, SaveConditions condition, int toSave);
        //void StopImagesDump();

        void SaveBufferedImages(string path, SaveConditions saveCondition, int toSave);
        void ResetImagesBuffer(string path, bool freeFolders);
    }

    public abstract class Station : IStation {

        public int IdStation { get; set; }
        public int NodeId { get; set; }
        public string Description { get; set; }
        public string ProviderName { get; set; }
        public bool Enabled { get; set; }
        public CameraCollection Cameras { get; set; }
        public bool Initialized { get; set; }
        public bool Connected { get; set; }
        public int CurrentCamera { get; protected set; }
        //public int DisplayPosition { get; set; }
        public bool HasMeasures { get; set; }
        //public bool IsNumImagesEditable { get; protected set; }
        //public int DefaultNumImages { get; protected set; }
        public LiveImageFilter LiveImageFilter { get; private set; }

        public Image<Rgb, byte> CurrentImage { get; internal set; }
        public Image<Rgb, byte> CurrentThumbnail { get; internal set; }
        public bool? CurrentIsReject { get; internal set; }
        public int CurrentRejectionBit { get; internal set; }
        public Image<Rgb, byte> AlternativeImage { get; internal set; }
        public Rectangle ZoomAOI { get; set; }
        MachineModeEnum machineMode;
        public MachineModeEnum MachineMode {
            get {
                return machineMode;
            }
            set {
                machineMode = value;
                foreach (ICamera c in Cameras)
                    c.MachineMode = value;
            }
        }

        public virtual int ToolsCount {
            get {
                return 0;
            }
        }

        bool dumpResultsEnabled;
        public bool DumpResultsEnabled {
            get {
                return dumpResultsEnabled;
            }
            set {
                dumpResultsEnabled = value;
                Log.Line(LogLevels.Debug, "Station.DumpResultsEnabled.set", "Dump Results Enabled = " + dumpResultsEnabled.ToString());
            }
        }

        public Station(StationDefinition stationDefinition) {

            IdStation = stationDefinition.Id;
            NodeId = stationDefinition.Node;
            Description = stationDefinition.StationDescription;
            ProviderName = stationDefinition.StationProviderName;
            Enabled = true;
            Cameras = new CameraCollection();
            LiveImageFilter = new LiveImageFilter(this);
        }

        public static Station CreateStation(StationDefinition stationDefinition) {

            StationProvider sp = StationProviderCollection.GetProvider(stationDefinition.StationProviderName);
            Station station = null;

            if (sp != null) {
                string[] typeData = sp.Type.Split(new char[] { ',' });
                if (typeData.Length == 2) {
                    string assemblyName = typeData[1];
                    string typeName = typeData[0];
                    Assembly assembly = Assembly.Load(assemblyName);
                    try {
                        station = (Station)Activator.CreateInstance(assembly.GetType(typeName), new object[] { stationDefinition });
                    }
                    catch {
                        throw;
                    }
                }
            }
            return station;
        }

        public virtual void Dispose() {
        }

        protected int imageAvailableHandlerCount;
        protected event EventHandler<ImageAvailableEventArgs> _imageAvailable;
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
        protected event EventHandler<ImageAvailableEventArgs> _alternativeImageAvailable;
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

        public event EventHandler<HvldDataAvailableEventArgs> HvldDataAvailable;
        public event EventHandler<MeasuresAvailableEventArgs> MeasuresAvailable;
        public event EventHandler<RecordStateEventArgs> RecBufferState;
        public event EventHandler<MessageEventArgs> SetParametersCompleted;
        public event EventHandler ExportedParametersUpdated;

        protected virtual void OnImageAvailable(object sender, ImageAvailableEventArgs e) {

            CurrentImage = e.Image;
            CurrentThumbnail = e.Thumbnail;
            CurrentIsReject = e.IsReject;
            CurrentRejectionBit = e.RejectionBit;
            EventHandler<ImageAvailableEventArgs> handler = _imageAvailable;
            if (handler != null)
                handler(sender, e);
        }

        protected virtual void OnAlternativeImageAvailable(object sender, ImageAvailableEventArgs e) {

            AlternativeImage = e.Image;
            EventHandler<ImageAvailableEventArgs> handler = _alternativeImageAvailable;
            if (handler != null)
                handler(sender, e);
        }

        protected virtual void OnHvldDataAvailable(object sender, HvldDataAvailableEventArgs e) {

            if (HvldDataAvailable != null) 
                HvldDataAvailable(sender, e);
        }

        protected virtual void OnMeasuresAvailable(object sender, MeasuresAvailableEventArgs e) {

            if (MeasuresAvailable != null)
                MeasuresAvailable(sender, e);
        }

        protected virtual void OnRecBufferState(object sender, RecordStateEventArgs e) {

            if (RecBufferState != null) RecBufferState(sender, e);
        }

        protected virtual void OnSetParametersCompleted(object sender, MessageEventArgs e) {

            if (SetParametersCompleted != null) SetParametersCompleted(sender, e);
        }

        protected virtual void OnExportedParametersUpdated(object sender, EventArgs e) {

            if (ExportedParametersUpdated != null) ExportedParametersUpdated(sender, e);
        }

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
						
                    int w = (ZoomAOI.Width > 0) ? ZoomAOI.Width : appImage2.Width;
                    int h = (ZoomAOI.Height > 0) ? ZoomAOI.Height : appImage2.Height;
                    if (ZoomAOI.Width > 0 && ZoomAOI.Height > 0)
                        appImage2.ROI = ZoomAOI;

                    //Image<Rgb, byte> destinationImg = null;
                    if (keepProp) {
                        dstImage = appImage2.Resize(Math.Min((double)width / w, (double)height / h), interpolationType);
                    }
                    else {
                        dstImage = appImage2.Resize(w, h, interpolationType);
                    }

                    appImage2.ROI = Rectangle.Empty;
                    aoiUsed = calcAOIUsed(w, h, width, height, keepProp);

                    //Log.Line(LogLevels.Debug, "Station.AdjustImage", "Resize time [ms]: " + (AdjustImageTimer.ElapsedMilliseconds - appTimer2 - appTimer1));
                }
                catch (Exception ex) {
                    Log.Line(LogLevels.Debug, "Station.AdjustImage", "Error adjusting image: " + ex.Message);
                }
            }
        }

        protected virtual Rectangle calcAOIUsed(int imgWidth, int imgHeight, int displayWidth, int displayHeight, bool keepProp) {
            return new Rectangle();
        }

        //void Planar2PackedConvert(Image<Rgb, byte> src, ref Image<Rgb, byte> dst) {

        //    //var packed = new byte[arraySize];
        //    if (dst == null) {
        //        Log.Line(LogLevels.Error, "Station.Planar2PackedConvert", "Invalid destination image");
        //        return;
        //    }
        //    int sizePerChannel = src.Bytes.Length / src.NumberOfChannels;
        //    const bool bgr = false;          //pier: parametrizzare
        //    CvInvoke.cvCvtPlaneToPix(src[0].Ptr, src[1].Ptr, src[2].Ptr, IntPtr.Zero, dst);
        //    //for (int i = 0; i < sizePerChannel; i++) {
        //    //    for (int j = 0; j < src.NumberOfChannels; j++) {
        //    //        int band = j;
        //    //        if (bgr)
        //    //            band = src.NumberOfChannels - 1 - j;
        //    //        //dst.Bytes[i * channels + band] = src[srcOffset + i + j * sizePerChannel];
        //    //        //int pix = i / channels;
        //    //        dst.Data[i / dst.Width, i % dst.Width, band] = src[i + j * sizePerChannel];
        //    //    }
        //    //    //imagePacked[i*3+1] = imageSourceBits[i+1*resultInfo.imageWidth*resultInfo.imageHeight];
        //    //    //imagePacked[i*3+2] = imageSourceBits[i+0*resultInfo.imageWidth*resultInfo.imageHeight];
        //    //}
        //    //Marshal.Copy(packed, 0, dst, arraySize);
        //}

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

        protected virtual bool AllocImage(ref Image<Gray, byte> imgToAlloc, int w, int h) {
            if (imgToAlloc != null && imgToAlloc.Width == w && imgToAlloc.Height == h) return true;
            try {
                if (imgToAlloc != null) imgToAlloc.Dispose();

                imgToAlloc = new Image<Gray, byte>(w, h);
                return true;
            }
            catch (OutOfMemoryException ex) {
                Log.Line(LogLevels.Error, "Station.AllocImage", "OutOfMemoryException: " + ex.Message);
                imgToAlloc = null;
            }
            return false;
        }

        public abstract void Connect();
        public abstract void Disconnect();
        public virtual void HardReset() { }
        public abstract void Grab();
        public abstract void StopGrab();
        public abstract Bitmap Snap();
        public abstract void SetStreamMode(int headNumber, CameraWorkingMode cwm);

        public abstract void SetPrevImage();
        public abstract void SetMainImage();
        public virtual void SetParameters(ParameterCollection<Parameter> parameters) { }
        public virtual void SaveBufferedImages(string path, SaveConditions saveCondition, int toSave) { }
        public virtual void ResetImagesBuffer(string path, bool freeFolders) { }
        public virtual Tool GetTool(int toolId) { return null; }
    }

    public class StationCollection : List<IStation> {

        public IStation this[IStation station] {
            get {
                return this.Find(s => { return (s.IdStation == station.IdStation && s.NodeId == station.NodeId); });
            }
        }

        public new IStation this[int IdStation] {
            get {
                return this.Find(s => { return s.IdStation == IdStation; });
            }
        }

        /// <summary>
        /// Ordina le camere di cui fare il display
        /// </summary>
        /// <param name="camSettingsList"></param>
        /// <param name="maxDisplayPerPage"></param>
        /// <returns></returns>
        //public StationCollection Sort(List<CameraSetting> camSettingsList, int maxDisplayPerPage) {

        //    StationCollection scOrdered = new StationCollection();
        //    //int maxDisplayPerPage = 0;
        //    int pageNum = 0;
        //    // int maxStations = 0;
        //    foreach (CameraSetting camSetting in camSettingsList) {
        //        //maxDisplayPerPage = Math.Max(maxDisplayPerPage, camSetting.DisplayPosition + 1);
        //        pageNum = Math.Max(pageNum, camSetting.PageNumberPosition + 1);
        //        //maxStations = Math.Max(maxStations, camSetting.Station + 1);
        //    }
        //    //new
        //    int[] camPosition = new int[camSettingsList.Count];
        //    for (int i = 0; i < camPosition.Length; i++) camPosition[i] = -1;
        //    foreach (CameraSetting camSetting in camSettingsList) {
        //        camPosition[camSetting.Id] = camSetting.PageNumberPosition * maxDisplayPerPage + camSetting.DisplayPosition;
        //    }
        //    for (int ics = 0; ics < camSettingsList.Count; ics++) {
        //        int min = 10000;
        //        int idCamX = 0;
        //        for (int i = 0; i < camPosition.Length; i++) {
        //            if (camPosition[i] > -1 && camPosition[i] < min) {
        //                min = camPosition[i];
        //                idCamX = i;

        //            }
        //        }
        //        if (min > -1 && idCamX < camPosition.Length) {
        //            camPosition[idCamX] = -1;
        //            int idStatX = -1;
        //            int idNodeX = -1;
        //            //da id camera a id stazione
        //            foreach (IStation s in this) {
        //                foreach (ICamera c in s.Cameras) {
        //                    if (c.IdCamera == idCamX) {
        //                        idStatX = c.StationId;
        //                        idNodeX = c.NodeId;
        //                        break;
        //                    }
        //                }
        //            }
        //            if (idStatX >= 0) {
        //                IStation station = (IStation)this.Find(ss => ss.NodeId == idNodeX && ss.IdStation == idStatX);
        //                station.DisplayPosition = min;
        //                scOrdered.Add(station);
        //            }
        //        }
        //    }

        //    //old
        //    //for (int iS = 0; iS < this.Count; iS++) {
        //    //    IStation station;
        //    //    foreach (CameraSetting camSetting in camSettingsList) {
        //    //        if (iS == (camSetting.PageNumberPosition * maxDisplayPerPage + camSetting.DisplayPosition)) {
        //    //            station = (IStation)this[camSetting.Station];
        //    //            scOrdered.Insert(iS, (IStation)station);
        //    //            break;
        //    //        }
        //    //    }
        //    //}
        //    return scOrdered;
        //}

    }

    public class RecordStateEventArgs : EventArgs {

        public bool Recording { get; private set; }
        public double BufferSizePerCent { get; private set; }

        public RecordStateEventArgs(bool recording, double bufferSizePerCent) {

            Recording = recording;
            BufferSizePerCent = bufferSizePerCent;
        }
    }
}
