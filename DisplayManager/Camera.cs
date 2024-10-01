using Emgu.CV;
using Emgu.CV.Structure;
using ExactaEasyCore;
using ExactaEasyEng;
using Hvld.Parser;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace DisplayManager
{
    public enum CameraWorkingMode {

        Idle = 0,
        ExternalSource,
        Timed
    }

    public enum CameraClipMode {

        None = 0,
        Full,
        Custom
    }

    public enum CameraVisulizerType
    {
        Undefine = 0,
        Gretel = 1,
        Data = 2,
        Spectrometer = 3,
        Hvld = 5,
    }

    public interface ICamera {

        int IdCamera { get; }
        int StationId { get; }
        int NodeId { get; }
        string CameraDescription { get; }
        bool Enabled { get; set; }
        int Rotation { get; }
        MachineModeEnum MachineMode { get; set; }
        List<LightController> Lights { get; }
        //bool ROIModeAvailable { get; }
        ParameterCollection<T> GetParameters<T>() where T : IParameter, new();
        List<ValuesTurbidimeter> eoptisPoints { get; set; }
        //int ROICount { get; }

        void SoftReset();

        CameraWorkingMode GetWorkingMode();
        void SetWorkingMode(CameraWorkingMode mode);
        CameraClipMode GetClipMode();
        void SetClipMode(CameraClipMode clipMode);
        void ImportParameters(string path);
        void ExportParameters(string path);
        void ApplyParameters(CameraSetting settings, ParameterTypeEnum paramType, Cam dataSource);
        void ApplyParameters(ParameterTypeEnum paramType, Cam dataSource);
        string LoadFormat(int formatId);
        void SetStopCondition(int headNumber, int condition, int timeout);
        CameraInfo GetCameraInfo();
        CameraNewStatus GetCameraStatus();
        string GetFirmwareVersion();
        CameraProcessingMode GetCameraProcessingMode();
        void StartLearning();
        void StopLearning(bool save);
        void StartAnalysisOffline();
        void StopAnalysisOffline();
        ParameterCollection<Parameter> GetAcquisitionParameters();
        ParameterCollection<Parameter> GetDigitizerParameters();
        ParameterCollection<Parameter> GetRecipeSimpleParameters();
        ParameterCollection<Parameter> GetRecipeAdvancedParameters();
        ParameterCollection<Parameter> GetROIParameters(int idROI);
        ParameterCollection<Parameter> GetMachineParameters();
        ParameterCollection<Parameter> GetStrobeParameters(CameraSetting camSettings);
        ParameterCollection<Parameter> GetStrobeParameters(int lightId);
        void SetAcquisitionParameters(ParameterCollection<Parameter> parameters);
        void SetDigitizerParameters(ParameterCollection<Parameter> parameters);
        void SetRecipeSimpleParameters(ParameterCollection<Parameter> parameters);
        void SetRecipeAdvancedParameters(ParameterCollection<Parameter> parameters);
        void SetROIParameters(ParameterCollection<Parameter> parameters, int idROI);
        void SetMachineParameters(ParameterCollection<Parameter> parameters);
        void SetStrobeParameters(CameraSetting camSettings, ParameterCollection<Parameter> parameters);
        void SetStrobeParameters(int lightId, ParameterCollection<Parameter> parameters);

        //AssistantControl GetAssistant();
        string DownloadImages(string path, UserLevelEnum userLvl);
        string UploadImages(string path, UserLevelEnum userLvl);

        CameraVisulizerType GetVisualizerType();
    }

    public abstract class Camera : ICamera, IDisposable {

        public delegate void BitmapListEventHandler(object sender, BitmapListEventArgs args);
        public event EventHandler PreDownloadImages;
        public event BitmapListEventHandler PostDownloadImages;

        public static Camera CreateCamera(string providerName, bool scanRequest) {

            CameraProvider cp = CameraProviderCollection.GetProvider(providerName);
            Camera camera = null;

            if (cp != null) {
                string[] typeData = cp.Type.Split(new char[] { ',' });
                if (typeData.Length == 2) {
                    string assemblyName = typeData[1];
                    string typeName = typeData[0];
                    Assembly assembly = Assembly.Load(assemblyName);
                    try {
                        camera = (Camera)Activator.CreateInstance(assembly.GetType(typeName));
                        //camera.loadExternalDependencies(cp.ExternalDependencies);
                    }
                    catch {

                        throw;
                    }
                }
            }
            return camera;
        }

        public static Camera CreateCamera(CameraDefinition cameraDefinition, bool scanRequest) {

            CameraProvider cp = CameraProviderCollection.GetProvider(cameraDefinition.CameraProviderName);
            Camera camera = null;

            if (cp != null) {
                string[] typeData = cp.Type.Split(new char[] { ',' });
                if (typeData.Length == 2) {
                    string assemblyName = typeData[1];
                    string typeName = typeData[0];
                    Assembly assembly = Assembly.Load(assemblyName);
                    try {
                        camera = (Camera)Activator.CreateInstance(assembly.GetType(typeName), new object[] { cameraDefinition, scanRequest });                        
                        camera.LoadExternalDependencies(cp.ExternalDependencies);                        
                    }
                    catch {

                        throw;
                    }
                }
            }
            return camera;
        }

        public int IdCamera { get; protected set; }
        public string CameraType { get; protected set; }
        public string CameraDescription { get; protected set; }
        public int StationId { get; protected set; }
        public int NodeId { get; protected set; }
        public int Spindle { get; protected set; }
        public string IP4Address { get; protected set; }
        public int Head { get; protected set; }
        public List<LightController> Lights { get; internal set; }
        public string Visualizer { get; protected set; }
        public int Rotation { get; protected set; }
        public bool Initialized { get; protected set; }
        public bool Connected { get; protected set; }
        public bool RebootRequired { get; protected set; }
        public MachineModeEnum MachineMode { get; set; }
        public List<ValuesTurbidimeter> eoptisPoints { get; set; }
        //public bool ROIModeAvailable { get; protected set; }

        protected List<string> ExtDependencies = new List<string>();

        public Camera() {
        }

        public Camera(CameraDefinition cameraDefinition) {

            IdCamera = cameraDefinition.Id;
            Head = cameraDefinition.Head;
            CameraType = cameraDefinition.CameraType;
            CameraDescription = cameraDefinition.CameraDescription;
            StationId = cameraDefinition.Station;
            NodeId = cameraDefinition.Node;
            Spindle = cameraDefinition.Spindle;
            IP4Address = cameraDefinition.IP4Address;
            Lights = new List<LightController>();
            Visualizer = cameraDefinition.Visualizer;
            Rotation = cameraDefinition.Rotation;
            eoptisPoints = new List<ValuesTurbidimeter>();

            Initialized = false;
            Connected = false;
        }

        public virtual void LoadExternalDependencies(List<string> extDependencies) {

            Assembly startAssembly = Assembly.GetEntryAssembly();
            string filename = "";
            string outputFolder = "";
            if (startAssembly != null && startAssembly.ManifestModule.Name.ToLower() == "exactaeasy.exe")
                outputFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            else
                outputFolder = Environment.CurrentDirectory + @"\DotNet Components\ExactaEasy"; // Specifico per IFIX
            foreach (string library in extDependencies) {
                filename = Path.GetFullPath(outputFolder + "/" + library);
                if (File.Exists(filename)) {
                    ExtDependencies.Add(filename);
                }
            }
        }

        //public abstract void Connect();
        //public abstract void Disconnect();

        //public abstract void Grab();
        //public abstract void StopGrab();

        //public abstract Bitmap Snap();

        public abstract CameraWorkingMode GetWorkingMode();
        public abstract void SetWorkingMode(CameraWorkingMode mode);
        public abstract CameraClipMode GetClipMode();
        public abstract void SetClipMode(CameraClipMode clipMode);
        //public abstract AssistantControl GetAssistant();
        public abstract string DownloadImages(string path, UserLevelEnum userLvl);
        public abstract string UploadImages(string path, UserLevelEnum userLvl);

        public abstract ParameterCollection<T> GetParameters<T>() where T : IParameter, new();

        //#region ChiamateSpecificheDaGeneralizzare

        public abstract CameraInfo GetCameraInfo();

        public abstract void SoftReset();

        public abstract void ApplyParameters(CameraSetting settings, ParameterTypeEnum paramType, Cam dataSource);
        public abstract void ApplyParameters(ParameterTypeEnum paramType, Cam dataSource);

        public abstract string LoadFormat(int formatId);

        public abstract void SetStopCondition(int headNumber, int condition, int timeout);

        public abstract CameraNewStatus GetCameraStatus();

        public abstract string GetFirmwareVersion();

        public abstract CameraProcessingMode GetCameraProcessingMode();

        public abstract void StartLearning();

        public abstract void ImportParameters(string path);    //per M9 importo i vial axis

        public abstract void ExportParameters(string path);    //per M9 esporto i vial axis

        public abstract void StopLearning(bool save);

        public abstract void StartAnalysisOffline();

        public abstract void StopAnalysisOffline();

        public abstract ParameterCollection<Parameter> GetAcquisitionParameters();
        public abstract ParameterCollection<Parameter> GetDigitizerParameters();
        public abstract ParameterCollection<Parameter> GetRecipeSimpleParameters();
        public abstract ParameterCollection<Parameter> GetRecipeAdvancedParameters();
        public abstract ParameterCollection<Parameter> GetROIParameters(int idROI);
        public abstract ParameterCollection<Parameter> GetMachineParameters();
        public abstract ParameterCollection<Parameter> GetStrobeParameters(CameraSetting camSettings);
        public abstract ParameterCollection<Parameter> GetStrobeParameters(int lightId);

        public abstract void SetAcquisitionParameters(ParameterCollection<Parameter> parameters);
        public abstract void SetDigitizerParameters(ParameterCollection<Parameter> parameters);
        public abstract void SetRecipeSimpleParameters(ParameterCollection<Parameter> parameters);
        public abstract void SetRecipeAdvancedParameters(ParameterCollection<Parameter> parameters);
        public abstract void SetROIParameters(ParameterCollection<Parameter> parameters, int idROI);
        public abstract void SetMachineParameters(ParameterCollection<Parameter> parameters);
        public abstract void SetStrobeParameters(CameraSetting camSettings, ParameterCollection<Parameter> parameters);
        public abstract void SetStrobeParameters(int lightId, ParameterCollection<Parameter> parameters);

        protected virtual void OnPreDownloadImages(EventArgs e) {

            EventHandler handler = PreDownloadImages;
            if (handler != null) {
                handler(this, e);
            }
        }

        protected virtual void OnPostDownloadImages(BitmapListEventArgs e) {

            BitmapListEventHandler handler = PostDownloadImages;
            if (handler != null) {
                handler(this, e);
            }
        }

        public virtual void  Dispose() {
        }


        public bool Enabled {
            get;
            set;
        }

        //public virtual int ROICount {
        //    get {
        //        return 0;
        //    }
        //}

        public CameraVisulizerType GetVisualizerType()
        {
            switch (Visualizer)
            {
                case "Gretel":
                    return CameraVisulizerType.Gretel;
                case "Data":
                    return CameraVisulizerType.Data;
                case "Spectrometer":
                    return CameraVisulizerType.Spectrometer;
                case "Hvld":
                    return CameraVisulizerType.Hvld;
                default:
                    return CameraVisulizerType.Undefine;
            }
        }
    }

    public class CameraCollection : List<ICamera> {

        public new ICamera this[int id] {
            get {
                return this.Find(c => { return c.IdCamera == id; });
            }
        }

        /// <summary>
        /// Ordina le camere di cui fare il display in base a PageNumberPosition e DisplayPosition
        /// </summary>
        /// <param name="camSettingsList"></param>
        /// <returns></returns>
        //public CameraCollection Sort(List<CameraSetting> camSettingsList) {

        //    CameraCollection ccOrdered = new CameraCollection();
        //    int maxDisplayPerPage = 0;
        //    int pageNum = 0;
        //    foreach (CameraSetting camSetting in camSettingsList) {
        //        maxDisplayPerPage = Math.Max(maxDisplayPerPage, camSetting.DisplayPosition + 1);
        //        pageNum = Math.Max(pageNum, camSetting.PageNumberPosition + 1);
        //    }
        //    for (int iC = 0; iC < this.Count; iC++) {
        //        Camera camera;
        //        foreach (CameraSetting camSetting in camSettingsList) {
        //            if (iC == (camSetting.PageNumberPosition * maxDisplayPerPage + camSetting.DisplayPosition)) {
        //                camera = (Camera)this[camSetting.Id];
        //                ccOrdered.Insert(iC, camera);
        //                break;
        //            }
        //        }
        //    }
        //    return ccOrdered;
        //}
    }

    /// <summary>
    /// 
    /// </summary>
    public class HvldDataAvailableEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public HvldFrame Frame { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public HvldDataAvailableEventArgs(HvldFrame frame)
        {
            Frame = frame;
        }
    }

    public class ImageAvailableEventArgs : EventArgs {
        
        public Image<Rgb, byte> Image { get; private set; }
        public Image<Rgb, byte> Thumbnail { get; private set; }
        public bool? IsReject { get; private set; }
        public int RejectionBit { get; private set; }

        public ImageAvailableEventArgs(Image<Rgb, byte> image, Image<Rgb, byte> thumbnail, bool? isReject, int rejectionBit) {

            Image = image;
            Thumbnail = thumbnail;
            IsReject = isReject;
            RejectionBit = rejectionBit;
        }
    }

    //public class ImagesResultsAvailableEventArgs : EventArgs {

    //    public ImagesResults ImagesRes { get; private set; }

    //    public ImagesResultsAvailableEventArgs(ImagesResults imgsRes) {

    //        ImagesRes = imgsRes;
    //    }
    //}

    public class BitmapListEventArgs : EventArgs {

        public List<Bitmap> BitmapList;

        public BitmapListEventArgs(List<Bitmap> bitmapList) {
            BitmapList = bitmapList;
        }
    }

    public class CameraException : Exception {
        public int ErrorCode;
        public CameraException(string message)
            : base(message) {
            ErrorCode = 0;
        }

        public CameraException(string message, int errorCode)
            : base(message) {
            ErrorCode = errorCode;
        }
    }

    //public enum CameraStatus {
    //    INITIALIZING = 0,
    //    RUNNING = 100,
    //    LIVE = 2200,
    //    SET_STOP_ON_CONDITION = 1000,
    //    GOING_TO_STOP_ON_CONDITION = 1001,
    //    STOP_ON_CONDITION = 1002,
    //    START_ANALYSIS = 1100,
    //    CAMERA_NOT_EXIST = -1,
    //    ERROR = -1000
    //}

    public enum CameraProcessingMode {
        None = 0,
        Acquiring,
        Processing,
        GoingToStopOnCondition,
        StopOnCondition,
        OfflineAnalysis,
        Learning
    }

    public enum CameraNewStatus {
        Unavailable     = 0,
        Available       = 1,
        Initializing    = 2,
        Ready           = 3,
        Error           = 4,
        Stop
    }

    public struct CameraInfo {
        public int serialNumber;
        public int type;
        public string description;
        public string model;
        public string vendor;
        public int widthImage;
        public int heightImage;
        public int shutterMin;
        public int shutterMax;
        public int bitDepth;
        public int headNumber;
        public int gainMin;
        public int gainMax;
        public string IpAddress;
        public string NicAddress;
    }

    public class ValuesTurbidimeter
    {
        public int Spindle { get; set; }
        public int Turn { get; set; }
        public double Value { get; set; }
    }

    #region SpecificoDelleTattileCamera
    public struct TI_VialAxisData {
        public int cnt;
        public int dummy;
        public int sum;
        public int temp;
        public int x;
    };

    public class VialAxis {
        public VialAxis() {
            VialAxisData = new List<TI_VialAxisData>();
        }

        public static VialAxis LoadFromFile(string filePath) {
            VialAxis newVialAxis = null;
            using (StreamReader reader = new StreamReader(filePath)) {
                newVialAxis = buildVialAxis(reader);
            }
            return newVialAxis;
        }

        public static VialAxis LoadFromXml(string xmlString) {
            VialAxis newVialAxis = null;
            using (StringReader reader = new StringReader(xmlString)) {
                newVialAxis = buildVialAxis(reader);
            }
            return newVialAxis;
        }

        static VialAxis buildVialAxis(TextReader reader) {
            try {
                XmlSerializer xmlSer = new XmlSerializer(typeof(VialAxis));
                VialAxis newVialAxis = (VialAxis)xmlSer.Deserialize(reader);
                return newVialAxis;
            }
            catch {
                return null;
            }
        }

        public List<TI_VialAxisData> VialAxisData;
        //public List<TI_VialAxisData> VialAxisData { get; set; }

        public override string ToString() {
            XmlSerializer xmlSer = new XmlSerializer(typeof(VialAxis));

            StringWriter writer = new StringWriter();
            xmlSer.Serialize(writer, this);
            string xmlStr = writer.ToString();
            writer.Close();

            return xmlStr;
        }

        public void SaveXml(string filePath) {
            XmlSerializer xmlSer = new XmlSerializer(typeof(VialAxis));
            StreamWriter writer = new StreamWriter(filePath);
            xmlSer.Serialize(writer, this);
            writer.Close();
        }
    }
    #endregion
}
