using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace ExactaEasyCore {


    public class GlobalConfiguration {
        public static GlobalConfiguration LoadFromFile(string filePath) {
            filePath = System.IO.Path.GetFullPath(filePath);
            GlobalConfiguration conf = null;
            try {
                using (StreamReader reader = new StreamReader(filePath)) {
                    XmlSerializer xmlSer = new XmlSerializer(typeof(GlobalConfiguration));
                    conf = (GlobalConfiguration)xmlSer.Deserialize(reader);
                }
                return conf;
            }
            catch {
                return null;
            }
        }

        public string SettingsFolder { get; set; }
        public string RecipesFolder { get; set; }
    }

    public class MachineConfiguration {

        public static MachineConfiguration LoadFromFile(string filePath) {

            MachineConfiguration conf = null;
            using (StreamReader reader = new StreamReader(filePath)) {
                XmlSerializer xmlSer = new XmlSerializer(typeof(MachineConfiguration));
                conf = (MachineConfiguration)xmlSer.Deserialize(reader);
            }
            conf.ConfigPath = Path.GetDirectoryName(filePath);
            if (conf != null)
                conf.SaveToXml(filePath + ".backup");
            return conf;
        }

        public void SaveToXml(string filePath) {

            StreamWriter writer = new StreamWriter(filePath);
            XmlSerializer xmlSer = new XmlSerializer(typeof(MachineConfiguration));
            xmlSer.Serialize(writer, this);
            writer.Close();
        }

        public override string ToString() {

            XmlSerializer xmlSer = new XmlSerializer(typeof(MachineConfiguration));
            StringWriter writer = new StringWriter();
            xmlSer.Serialize(writer, this);
            string xmlStr = writer.ToString();
            writer.Close();

            return xmlStr;
        }

        //public int GetCameraStationId(int IdCamera) {
        //    if (CameraSettings == null || CameraSettings.Count == 0)
        //        return -1;
        //    foreach (CameraSetting camSetting in CameraSettings) {
        //        if (camSetting.Id == IdCamera)
        //            return camSetting.Station;
        //    }
        //    return -1;
        //}

        [XmlIgnore]
        public string ConfigPath { get; set; }

        public string MachineType { get; set; }                 /* Tipo di macchina (non usato) */
        public string Customer { get; set; }                    /* Nome del cliente (non usato) */
        public int StartupDelaySec { get; set; }                /* Ritardo all'avvio di Windows (obsoleto) */
        public int NodeBootTimeoutSec { get; set; }             /* Timeout di attesa di avvio dei vari nodi (Gretel) */
        public List<RegistryKeyCtrl> RegKeyToCheckAtStartup { get; set; }   /* Chiavi del registro da verificare all'avvio di Windows */
        public int ViewSplashScreen { get; set; }               /* Modalità di visualizzazione della splash screen: -1: no | 0: yes | 1: TopMost */
        public bool BypassHMIsendRecipe { get; set; }           /* Bypass della ricetta che arriva da SCADA, diventa ricetta attiva, ma non viene scaricata ai device connessi */
        public bool EnableAudit { get; set; }                   /* Enable Audit ricetta */
        public bool BypassHMIKnapp { get; set; }                /* La richiesta di Knapp da SCADA viene ricevuta ma non inoltrata ai device connessi (Gretel non memorizza tutti i frame del Knapp) */
        public int HMIKnappTurnsLimit { get; set; }             /* Numero di giri massimi ammessi per il knapp (se di più non viene inviata a Gretel la richiesta di salvare tutti i frame del Knapp) */
        public bool BypassHMIsendResults { get; set; }          /* Non vengono inviati i risultati delle stazioni allo SCADA */
        public bool EnableOnTopCommunication { get; set; }      /* Abilita la comunicazione con OnTop */

        public bool UseConsole { get; set; }                    /* Usa console del logger all'avvio */
        public int ConsoleLevel { get; set; }                   /* Livello di visualizzazione messaggi della console */
        public bool WriteLogToFile { get; set; }                /* Scrivi log su file */
        public int LogToFileLevel { get; set; }                 /* Livello di scrittura dei messaggi su file di log */

        public bool DisplayResultsPage { get; set; }
        public bool DumpResults { get; set; }
        public bool DumpImagesAvailable { get; set; }

        // Da rimuovere appena sistemato l'id camera nelle ricette
        public int FirstStationIndex { get; set; }

        public List<string> AdaptersCheck { get; set; }
        public int TriggerMode { get; set; }
        public bool TriggerSourceManual { get; set; }

        public bool UseExternalImageViewer { get; set; }

        public bool VisualizerKeepRatio { get; set; }           /* Mantieni proporzioni nelle immagini del sinottico */
        public int VisualizerImageQuality { get; set; }         /* Interpolazione immagini: 0: Nearest-neighbor | 1: bilinear | 2: cubic | 3: bicubic | 4: LANCZOS */
        public bool VisualizerShowColoredBorder { get; set; }   /* Visualizzazione bordo immagini distinto per buone(verde) / scarte(rosso) */
        public int VisualizerNumberThumbnails { get; set; }         /* Number of result thumbinails that can be viewed during inspection 4-15 */
        public List<ScreenGridDisplaySettings> DisplaySettings { get; set; }    /* Gestione separata delle singole pagine dell'aggregato */

        public int NumberOfSpindles { get; set; }
        public int NumberOfCamera { get; set; }
        public int MaxInspectionQueueLength { get; set; }
        public bool EnableImgToSave { get; set; }
        public int MaxInspectionImgToSave { get; set; }
        string bufferedInspectionImagesPath;
        public string BufferedInspectionImagesPath {
            get {
                return bufferedInspectionImagesPath;
            }
            set {
                bufferedInspectionImagesPath = value;
                if (string.IsNullOrEmpty(value) == true) {
                    bufferedInspectionImagesPath = @"C:\Images\";
                }
                if (bufferedInspectionImagesPath.EndsWith(@"\") == false) {
                    bufferedInspectionImagesPath = value + @"\";
                }
            }
        }
        public NodeProviders NodeProviders { get; set; }
        public List<NodeSetting> NodeSettings { get; set; }
        public StationProviders StationProviders { get; set; }
        public List<StationSetting> StationSettings { get; set; }
        public CameraProviders CameraProviders { get; set; }
        public List<CameraSetting> CameraSettings { get; set; }
        public int NumberOfStrobo { get; set; }
        public List<StrobeSetting> StrobeSettings { get; set; }

        public bool HideRemoteDesktopToolbar { get; set; }      /* Nascondi barra del desktop remoto */

        public bool ResultAverageCalculation { get; set; }
        public bool ResultMaxCalculation { get; set; }
        public bool ResultMinCalculation { get; set; }
        public bool ResultVarianceCalculation { get; set; }

        public bool SaveData2DB { get; set; }                   /* Abilita salvataggio risultati su database */
        public string DBServer { get; set; }                    /* Nome server database */
        public string DBName { get; set; }                      /* Nome database */
        public string DBTable { get; set; }                     /* Nome tabella (es: db0.detailedReports) */
        public string HistoricizedToolsFolder { get; set; }

        public bool SaveImagesWhileRunning { get; set; } = false;  /*Default false*/
        public int RecipeDownloadTimeOut { get; set; } = 180;  /*default 180 - time limit for downloading recipe*/

        public bool TrendDataEnable { get; set; } = true;
        public string FolderStorageTrendData { get; set; } = "E:\\TrendingTool";
        public int MaxSimultaneosSavesTrendData { get; set; } = 3;

        public bool RedisEnable { get; set; } = false;
        public string RedisHostname { get; set; }
        public int RedisPort { get; set; }
        public string RedisPassword { get; set; }

        public bool IsRedisVisibleStationFilter { get; set; } = false;
        public bool IsRedisCaronteEnable { get; set; } = false;

        /// <summary>
        /// <br>0=Flow Scene Json disabled</br>
        /// <br>1=Flow Scene Json enabled</br>
        /// </summary>
        public int PrintReportJSONMode { get; set; } = 0;

        public bool IsLiveImageFilter { get; set; } = false;

        /// <summary>
        /// <br>0=OLD</br>
        /// <br>1=NEW (ALL PARAMETERS)</br>
        /// </summary>
        public int TypeDumpImages { get; set; } = 0;



        public List<HistoricizedToolSetting> HistoricizedToolsSettings { get; set; }



        public string HMIServiceAddress { get; set; }
        public string SupervisorServiceAddress { get; set; }

        public KnappSettings KnappSettings { get; set; }
        public List<RejectionCause> RejectionCauses { get; set; }

        [XmlIgnore]
        public GlobalConfiguration GlobalConfig { get; set; }








    }

}
