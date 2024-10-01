using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using ExactaEasyCore;

namespace DisplayManager {
    public class VisionSystemConfig {

        public static VisionSystemConfig LoadFromFile(string filePath) {
            VisionSystemConfig newAssistants = null;
            using (StreamReader reader = new StreamReader(filePath)) {
                newAssistants = buildAssistants(reader);
            }
            return newAssistants;
        }

        public static VisionSystemConfig LoadFromXml(string xmlString) {
            VisionSystemConfig newAssistants = null;
            using (StringReader reader = new StringReader(xmlString)) {
                newAssistants = buildAssistants(reader);
            }
            return newAssistants;
        }

        static VisionSystemConfig buildAssistants(TextReader reader) {
            try {
                XmlSerializer xmlSer = new XmlSerializer(typeof(VisionSystemConfig));
                VisionSystemConfig newAssistants = (VisionSystemConfig)xmlSer.Deserialize(reader);
                return newAssistants;
            }
            catch {
                return null;
            }
        }

        public int NodeBootTimeoutSec { get; set; }
        public int MaxInspectionQueueLength { get; set; }
        public bool EnableImgToSave { get; set; }
        public int MaxInspectionImgToSave { get; set; }
        public CameraProviders CameraProviders { get; set; }
        public StationProviders StationProviders { get; set; }
        public NodeProviders NodeProviders { get; set; }
        public NodeDefinitionCollection NodesDefinition { get; set; }
        public StationDefinitionCollection StationDefinition { get; set; }
        public CameraDefinitionCollection CamerasDefinition { get; set; }
        public List<StrobeControllerDefinition> StrobeControllersDefinition { get; set; }
        public KnappSettings KnappSettings { get; set; }

        public VisionSystemConfig() {

            NodesDefinition = new NodeDefinitionCollection();
            StationDefinition = new StationDefinitionCollection();
            CamerasDefinition = new CameraDefinitionCollection();
            StrobeControllersDefinition = new List<StrobeControllerDefinition>();
            KnappSettings = new KnappSettings();
        }

        public void SaveXml(string filePath) {
            XmlSerializer xmlSer = new XmlSerializer(typeof(VisionSystemConfig));

            StreamWriter writer = new StreamWriter(filePath);
            xmlSer.Serialize(writer, this);
            writer.Close();
        }

        public override string ToString() {
            XmlSerializer xmlSer = new XmlSerializer(typeof(VisionSystemConfig));

            StringWriter writer = new StringWriter();
            xmlSer.Serialize(writer, this);
            string xmlStr = writer.ToString();
            writer.Close();

            return xmlStr;
        }

    }
}
