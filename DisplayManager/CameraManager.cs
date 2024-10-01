using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace DisplayManager {
    public class CameraManager {
        CameraManagerConfig _configuration;

        public CameraManagerConfig Configuration {
            get { return _configuration; }
            private set {
                _configuration = value;
                init();
            }
        }

        public NodeCollection Nodes { get; private set; }
        public StationCollection Stations { get; private set; }
        public CameraCollection Cameras { get; private set; }
        public DisplayCollection Displays { get; private set; }
        public bool Initialized { get; private set; }

        public CameraManager(string configurationFile)
            : this() {

            if (File.Exists(configurationFile))
                Configuration = CameraManagerConfig.LoadFromFile(configurationFile);
            else
                Configuration = null; // Innescare eccezione ?
        }

        public CameraManager(CameraManagerConfig configuration)
            : this() {

            Configuration = configuration;
        }

        public CameraManager(VisionSystemConfig configuration)
            : this() {

            initVS(configuration);
        }

        CameraManager() {

            Cameras = new CameraCollection();
            Displays = new DisplayCollection();
        }

        private void init() {

            foreach (CameraDefinition camDef in Configuration.CamerasDefinition) {
                try {
                    Camera newCamera = Camera.CreateCamera(camDef);
                    Cameras.Add(newCamera);
                }
                catch {
                    throw;
                }
            }
        }

        // TODO: Da rimuovere il parametro in definitivo, e modificare la proprieta
        // Configuration
        private void initVS(VisionSystemConfig configuration) {

            foreach (NodeDefinition nodeDef in configuration.NodesDefinition) {
                // TODO: creazione nodi
            }
            foreach (StationDefinition statDef in configuration.StationDefinition) {
                // TODO: creazione stazioni
            }
            foreach (CameraDefinition camDef in Configuration.CamerasDefinition) {
                try {
                    Camera newCamera = Camera.CreateCamera(camDef);
                    // Se la camera incorpora il concetto di nodo deve essere aggiunta 
                    // alla collection dei nodi
                    if (newCamera is INode) {
                        addNode((INode)newCamera);
                    }
                    // Se la camera incorpora il concetto di stazione deve essere aggiunta 
                    // alla collection di stazioni
                    if (newCamera is IStation) {
                        addStation((IStation)newCamera);
                    }
                    Cameras.Add(newCamera);
                }
                catch {
                    throw;
                }
            }

        }

        void addNode(INode newNode) {

            if (Nodes[newNode.IdNode] == null)
                Nodes.Add(newNode);
        }

        void addStation(IStation newStation) {

            IStation st = Stations[newStation.IdStation];
            if (st == null) {
                Stations.Add(newStation);
                st = newStation;
            }
            if (Nodes[st.NodeId] == null && newStation is INode)
                addNode((INode)newStation);
            else
                throw new ArgumentOutOfRangeException("Id node not valid!");
            Nodes[st.NodeId].Stations.Add(newStation);

        }

        void addCamera(ICamera newCamera) {

            ICamera cm = Cameras[newCamera.IdCamera];
            if (cm != null)
                throw new ArgumentOutOfRangeException("Duplicate camera! Id : " + newCamera.IdCamera);

            //Cameras.Add(ICamera newCamera);
        }

        public void RedrawDisplay(string displayName) {

            Display d = Displays[displayName];
            d.DoRender();
        }

        public void StartLive() {

            foreach (Camera c in Cameras) {
                startLive(c);
            }
        }

        public void StartLive(int cameraId) {

            Camera c = (Camera)Cameras[cameraId];
            startLive(c);
        }

        void startLive(Camera camera) {

            if (camera != null) {
                //if (!camera.Connected)
                //    camera.Connect();
                //if (camera.Connected)   //pier: aggiunto per M12
                //    camera.Grab();
            }
        }

        public void CreateScreenGridDisplay(string name, IntPtr windowHandle, int[] cameraIds, int rowsCount, int columnsCount) {

            CameraCollection srcCam = new CameraCollection();
            for (int i = 0; i < cameraIds.GetLength(0); i++) {
                Camera c = (Camera)Cameras[cameraIds[i]];
                if (c != null)
                    srcCam.Add(c);
            }
            ScreenGridDisplay sgd = new ScreenGridDisplay(name, windowHandle, srcCam, rowsCount, columnsCount);
            Displays.Add(sgd);
        }

        public void CreateScreenSingleDisplay(string name, IntPtr windowHandle, int cameraId) {

            SingleDisplay sd = new SingleDisplay(name, windowHandle, (Camera)Cameras[cameraId]);
            Displays.Add(sd);
        }


    }

    public class CameraManagerConfig {

        public static CameraManagerConfig LoadFromFile(string filePath) {
            CameraManagerConfig newAssistants = null;
            using (StreamReader reader = new StreamReader(filePath)) {
                newAssistants = buildAssistants(reader);
            }
            return newAssistants;
        }

        public static CameraManagerConfig LoadFromXml(string xmlString) {
            CameraManagerConfig newAssistants = null;
            using (StringReader reader = new StringReader(xmlString)) {
                newAssistants = buildAssistants(reader);
            }
            return newAssistants;
        }

        static CameraManagerConfig buildAssistants(TextReader reader) {
            try {
                XmlSerializer xmlSer = new XmlSerializer(typeof(CameraManagerConfig));
                CameraManagerConfig newAssistants = (CameraManagerConfig)xmlSer.Deserialize(reader);
                return newAssistants;
            }
            catch (Exception ex) {
                return null;
            }
        }

        public CameraProviders CameraProviders { get; set; }
        public CameraDefinitionCollection CamerasDefinition { get; set; }

        public void SaveXml(string filePath) {
            XmlSerializer xmlSer = new XmlSerializer(typeof(CameraManagerConfig));

            StreamWriter writer = new StreamWriter(filePath);
            xmlSer.Serialize(writer, this);
            writer.Close();
        }

        public override string ToString() {
            XmlSerializer xmlSer = new XmlSerializer(typeof(CameraManagerConfig));

            StringWriter writer = new StringWriter();
            xmlSer.Serialize(writer, this);
            string xmlStr = writer.ToString();
            writer.Close();

            return xmlStr;
        }
    }
}
