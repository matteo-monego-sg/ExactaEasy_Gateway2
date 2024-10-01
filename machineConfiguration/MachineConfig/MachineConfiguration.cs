using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace machineConfiguration.MachineConfig
{
    public class MachineConfiguration
    {
        /*SET DEFALULT VALUE!!*/


        [InfoProperty("Delay when starting the pc", true, InfoPropertyAttributeTypeData.INT)]
        public string StartupDelaySec { get; set; } = "210";

        [InfoProperty("Specifies how the splash screen is displayed when starting the supervisor (-1: Minimize; 0: Normal; 1: TopMost)", true, InfoPropertyAttributeTypeData.INT)]
        public string ViewSplashScreen { get; set; } = "-1";

        [InfoProperty("Time to turn on vision PCs", true, InfoPropertyAttributeTypeData.INT)]
        public string NodeBootTimeoutSec { get; set; } = "30";

        [InfoProperty("Set the 'bypass recipe' when the supervisor starts", true, InfoPropertyAttributeTypeData.BOOL)]
        public string BypassHMIsendRecipe { get; set; } = "false";

        [InfoProperty("Audit of Gretel", true, InfoPropertyAttributeTypeData.BOOL)]
        public string EnableAudit { get; set; } = "true";

        [InfoProperty("-", true, InfoPropertyAttributeTypeData.BOOL)]
        public string BypassHMIsendResults { get; set; } = "true";

        [InfoProperty("Customer name", true, InfoPropertyAttributeTypeData.STRING)]
        public string Customer { get; set; } = "SPAMI";

        [InfoProperty("Deprecated", true, InfoPropertyAttributeTypeData.STRING)]
        public string DefaultFolder { get; set; } = "D:\\SPAMI";

        [InfoProperty("Enable debug", true, InfoPropertyAttributeTypeData.BOOL)]
        public string DebugEnabled { get; set; } = "true";

        [InfoProperty("Use a console that appears on the right side", true, InfoPropertyAttributeTypeData.BOOL)]
        public string UseConsole { get; set; } = "false";

        [InfoProperty("Console level (0 - 3)", true, InfoPropertyAttributeTypeData.INT)]
        public string ConsoleLevel { get; set; } = "0";

        [InfoProperty("Enabled write log to file", true, InfoPropertyAttributeTypeData.BOOL)]
        public string WriteLogToFile { get; set; } = "true";

        [InfoProperty("Log level (0 - 3)", true, InfoPropertyAttributeTypeData.INT)]
        public string LogToFileLevel { get; set; } = "2";

        [InfoProperty("-", true, InfoPropertyAttributeTypeData.BOOL)]
        public string UseExternalImageViewer { get; set; } = "false";

        [InfoProperty("Deprecated", true, InfoPropertyAttributeTypeData.INT)]
        public string TriggerMode { get; set; } = "1";

        [InfoProperty("Deprecated", true, InfoPropertyAttributeTypeData.BOOL)]
        public string TriggerSourceManual { get; set; } = "false";

        [InfoProperty("Ping to gretel", true, InfoPropertyAttributeTypeData.BOOL)]
        public string PingCheck { get; set; } = "false";

        [InfoProperty("Can save images", true, InfoPropertyAttributeTypeData.BOOL)]
        public string DumpImagesAvailable { get; set; } = "true";

        [InfoProperty("-", true, InfoPropertyAttributeTypeData.BOOL)]
        public string DumpResults { get; set; } = "false";

        [InfoProperty("Number of cameras (set automatcly)", false, InfoPropertyAttributeTypeData.INT)]
        public string NumberOfCamera { get; set; } = "0";

        [InfoProperty("Number of stations (set automatcly)", false, InfoPropertyAttributeTypeData.INT)]
        public string NumberOfStation { get; set; } = "0";

        [InfoProperty("Number of spindles", true, InfoPropertyAttributeTypeData.INT)]
        public string NumberOfSpindles { get; set; } = "40";

        [InfoProperty("-", true, InfoPropertyAttributeTypeData.STRING)]
        public string MachineType { get; set; } = "ExactaEasy";

        [InfoProperty("Keep of ratio on visualizer image", true, InfoPropertyAttributeTypeData.BOOL)]
        public string VisualizerKeepRatio { get; set; } = "true";

        [InfoProperty("0: Nearest-neighbor | 1: bilinear | 2: cubic | 3: bicubic | 4: LANCZOS ", true, InfoPropertyAttributeTypeData.INT)]
        public string VisualizerImageQuality { get; set; } = "0";

        [InfoProperty("Visualize the border on visualizer image (green = accepted, red = defect)", true, InfoPropertyAttributeTypeData.BOOL)]
        public string VisualizerShowColoredBorder { get; set; } = "true";

        [InfoProperty("-", true, InfoPropertyAttributeTypeData.INT)]
        public string VisualizerNumberThumbnails { get; set; } = "16";

        [InfoProperty("Address of HMI", true, InfoPropertyAttributeTypeData.STRING)]
        public string HMIServiceAddress { get; set; } = "net.tcp://127.0.0.1:8095/ExactaEasyUI/HMI";

        [InfoProperty("Address of Supervisor", true, InfoPropertyAttributeTypeData.STRING)]
        public string SupervisorServiceAddress { get; set; } = "net.tcp://127.0.0.1:8090/ExactaEasyUI/Supervisor";

        [InfoProperty("Always true", true, InfoPropertyAttributeTypeData.BOOL)]
        public string EnableOnTopCommunication { get; set; } = "true";

        [InfoProperty("Deprecated", true, InfoPropertyAttributeTypeData.STRING)]
        public string SupervisorAddressVsModules { get; set; } = "localhost";

        [InfoProperty("-", true, InfoPropertyAttributeTypeData.STRING)]
        public string BufferedInspectionImagesPath { get; set; } = "D:\\Images";

        [InfoProperty("Number of maximum images that the supervisor keeps in memory", true, InfoPropertyAttributeTypeData.INT)]
        public string MaxInspectionQueueLength { get; set; } = "40";

        [InfoProperty("-", true, InfoPropertyAttributeTypeData.BOOL)]
        public string EnableImgToSave { get; set; } = "false";

        [InfoProperty("Deprecated", true, InfoPropertyAttributeTypeData.INT)]
        public string MaxInspectionImgToSave { get; set; } = "100";

        [InfoProperty("Always true", true, InfoPropertyAttributeTypeData.BOOL)]
        public string HideRemoteDesktopToolbar { get; set; } = "true";

        [InfoProperty("Enables the calculation of the average in the results grid", true, InfoPropertyAttributeTypeData.BOOL)]
        public string ResultAverageCalculation { get; set; } = "true";

        [InfoProperty("Enables the calculation of the maximum value in the results grid", true, InfoPropertyAttributeTypeData.BOOL)]
        public string ResultMaxCalculation { get; set; } = "true";

        [InfoProperty("Enables the calculation of the minimum value in the results grid", true, InfoPropertyAttributeTypeData.BOOL)]
        public string ResultMinCalculation { get; set; } = "true";

        [InfoProperty("Enables the calculation of the variance value in the results grid", true, InfoPropertyAttributeTypeData.BOOL)]
        public string ResultVarianceCalculation { get; set; } = "true";

        [InfoProperty("Possibility of saving images while the machine is running", true, InfoPropertyAttributeTypeData.BOOL)]
        public string SaveImagesWhileRunning { get; set; } = "false";

        [InfoProperty("Time limit for downloading recipe [s]", true, InfoPropertyAttributeTypeData.INT)]
        public string RecipeDownloadTimeOut { get; set; } = "180";

        [InfoProperty("Historical file destination path", true, InfoPropertyAttributeTypeData.STRING)]
        public string HistoricizedToolsFolder { get; set; } = "D:\\Reports\\HistoricizedTools";

        [InfoProperty("Deprecated", true, InfoPropertyAttributeTypeData.BOOL)]
        public string SaveData2DB { get; set; } = "false";

        [InfoProperty("Deprecated", true, InfoPropertyAttributeTypeData.STRING)]
        public string DBServer { get; set; } = "";

        [InfoProperty("Deprecated", true, InfoPropertyAttributeTypeData.STRING)]
        public string DBName { get; set; } = "";

        [InfoProperty("Deprecated", true, InfoPropertyAttributeTypeData.STRING)]
        public string DBTable { get; set; } = "";

        [InfoProperty("Enable TrendData", true, InfoPropertyAttributeTypeData.BOOL)]
        public string TrendDataEnable { get; set; } = "true";

        [InfoProperty("Folder storage Trend Data", true, InfoPropertyAttributeTypeData.STRING)]
        public string FolderStorageTrendData { get; set; } = "E:\\TrendingInspectionTools";

        [InfoProperty("Maximum simultaneous saves trend tada", true, InfoPropertyAttributeTypeData.INT)]
        public string MaxSimultaneosSavesTrendData { get; set; } = "3";

        [InfoProperty("Enable Redis", true, InfoPropertyAttributeTypeData.BOOL)]
        public string RedisEnable { get; set; } = "false";

        [InfoProperty("Redis Hostname", true, InfoPropertyAttributeTypeData.STRING)]
        public string RedisHostname { get; set; }

        [InfoProperty("Redis port (1-65535)", true, InfoPropertyAttributeTypeData.INT)]
        public string RedisPort { get; set; }

        [InfoProperty("Redis password", true, InfoPropertyAttributeTypeData.STRING)]
        public string RedisPassword { get; set; }

        [InfoProperty("true to enable the visible station filter via redis", true, InfoPropertyAttributeTypeData.BOOL)]
        public string IsRedisVisibleStationFilter { get; set; }

        [InfoProperty("true to enable/disable caronte via redis", true, InfoPropertyAttributeTypeData.BOOL)]
        public string IsRedisCaronteEnable { get; set; }

        [InfoProperty("Print Flow Scene Json: 0=disabled 1=enabled", true, InfoPropertyAttributeTypeData.INT)]
        public string PrintReportJSONMode { get; set; }

        [InfoProperty("True to show the page Live Image Filter", true, InfoPropertyAttributeTypeData.BOOL)]
        public string IsLiveImageFilter { get; set; }

        [InfoProperty("0=OLD 1=NEW (ALL PARAMETERS)", true, InfoPropertyAttributeTypeData.INT)]
        public string TypeDumpImages { get; set; }




        //LISTS
        public List<HistoricizedToolSetting> HistoricizedToolsSettings { get; set; } = new List<HistoricizedToolSetting>();
        public List<ScreenGridDisplaySettings> DisplaySettings { get; set; } = new List<ScreenGridDisplaySettings>();
        public List<NodeSetting> NodeSettings { get; set; } = new List<NodeSetting>();
        public List<StationSetting> StationSettings { get; set; } = new List<StationSetting>();
        public List<CameraSetting> CameraSettings { get; set; } = new List<CameraSetting>();
    }
}
