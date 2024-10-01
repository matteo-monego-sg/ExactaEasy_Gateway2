using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ExactaEasyCore {

    public class DumpImagesConfiguration {

        public bool DumpImagesToolsAvailable { get; set; }
        public int CurrentUserSettingsId { get; set; }
        public List<DumpImagesUserSettings> UserSettings { get; set; }

        public static DumpImagesConfiguration LoadFromFile(string filePath, MachineConfiguration machineConfig) {

            DumpImagesConfiguration conf;
            using (var reader = new StreamReader(filePath)) {
                var xmlSer = new XmlSerializer(typeof(DumpImagesConfiguration));
                conf = (DumpImagesConfiguration)xmlSer.Deserialize(reader);
            }
            foreach (DumpImagesUserSettings dius in conf.UserSettings) {
                // ADD MACHINE CONFIG STATIONS
                foreach (CameraSetting camSetting in machineConfig.CameraSettings) {
                    NodeSetting nodeSetting = machineConfig.NodeSettings.Find(nn => nn.Id == camSetting.Node);
                    StationSetting stationSetting = machineConfig.StationSettings.Find(ss => (ss.Id == camSetting.Station && ss.Node == camSetting.Node));
                    if (nodeSetting != null && stationSetting != null) {
                        string description = nodeSetting.NodeDescription + " - " + stationSetting.StationDescription + " - " + camSetting.CameraDescription;
                        StationDumpSettings newSds = dius.StationsDumpSettings.Find(sds => (sds.Node == nodeSetting.Id && sds.Id == stationSetting.Id));
                        if (newSds == null) {
                            newSds = new StationDumpSettings(camSetting.Node, camSetting.Station, description);
                            dius.StationsDumpSettings.Add(newSds);
                        }
                        else {
                            newSds.Description = description;
                        }
                    }
                }
                // REMOVE MACHINE CONFIG STATIONS
                int statCount = dius.StationsDumpSettings.Count;
                for (int i = 0; i < statCount; i++) {
                    StationDumpSettings sds = dius.StationsDumpSettings[i];
                    CameraSetting camSetting = machineConfig.CameraSettings.Find(cc => (cc.Station == sds.Id && cc.Node == sds.Node));
                    if (camSetting == null) {
                        dius.StationsDumpSettings.RemoveAt(i);
                        statCount--;
                        i--;
                    }
                }
                // SORT
                List<StationDumpSettings> oldSdsList = new List<StationDumpSettings>();
                foreach (StationDumpSettings sds in dius.StationsDumpSettings) {
                    oldSdsList.Add(sds.Clone());
                }
                dius.StationsDumpSettings.Clear();
                foreach (CameraSetting camSetting in machineConfig.CameraSettings) {
                    StationDumpSettings newSds = oldSdsList.Find(sds => (sds.Node == camSetting.Node && sds.Id == camSetting.Station));
                    dius.StationsDumpSettings.Add(newSds);
                }
            }
            return conf;
        }

        public DumpImagesConfiguration() {
        }

        public void SaveXml(string filePath) {

            var writer = new StreamWriter(filePath);
            var xmlSer = new XmlSerializer(typeof(DumpImagesConfiguration));
            xmlSer.Serialize(writer, this);
            writer.Close();
        }

        public override string ToString() {

            var xmlSer = new XmlSerializer(typeof(DumpImagesConfiguration));
            var writer = new StringWriter();
            xmlSer.Serialize(writer, this);
            var xmlStr = writer.ToString();
            writer.Close();
            return xmlStr;
        }

    }

    public class DumpImagesUserSettings {

        public int Id { get; set; }
        public string Label { get; set; }
        public List<StationDumpSettings> StationsDumpSettings { get; set; }

        public DumpImagesUserSettings Clone() {

            var newUsrSettings = new DumpImagesUserSettings {
                Id = Id,
                Label = Label
            };
            if (StationsDumpSettings == null) return newUsrSettings;
            newUsrSettings.StationsDumpSettings = new List<StationDumpSettings>();
            foreach (StationDumpSettings sds in StationsDumpSettings) {
                StationDumpSettings newSds = sds.Clone();
                newUsrSettings.StationsDumpSettings.Add(newSds);
            }
            return newUsrSettings;
        }
    }

    public class StationDumpSettings {

        public int Node { get; set; }
        public int Id { get; set; }
        [XmlIgnore]
        public string Description { get; set; }
        public ImagesDumpTypes Type { get; set; }
        public SaveConditions Condition { get; set; }
        //public int Divisor { get; set; }
        public int VialsToSave { get; set; }
        public int MaxImages { get; set; }
        [XmlIgnore]
        public int VialsSaved { get; set; }
        [XmlIgnore]
        public Dictionary<int, bool> ToolsSelectedForDump = new Dictionary<int, bool>();

        public StationDumpSettings() {

            Type = ImagesDumpTypes.Frames;
            Condition = SaveConditions.Reject;
            VialsToSave = 100;
            VialsSaved = 0;
            MaxImages = 5000;
        }

        public StationDumpSettings(int node, int id, string description)
            : this() {

            Node = node;
            Id = id;
            Description = description;
        }

        public StationDumpSettings Clone() {

            var newSds = new StationDumpSettings {
                Node = this.Node,
                Id = this.Id,
                Description = this.Description,
                Type = this.Type,
                Condition = this.Condition,
                //Divisor = Divisor,
                VialsToSave = this.VialsToSave,
                VialsSaved = this.VialsSaved,
                MaxImages = this.MaxImages
            };
            return newSds;
        }
    }

    public enum ImagesDumpTypes {
        Frames = 0,
        Thumbnail = 1,
        Result = 2
    }

    public enum SaveConditions {
        Never = 0,
        Any = 1,
        Good = 2,
        Reject = 3
        //OnToolReject = 4
    }
}
