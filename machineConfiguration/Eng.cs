using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using machineConfiguration.MachineConfig;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Reflection;
using System.Net;

namespace machineConfiguration
{
    public sealed class Eng
    {
        //singleton
        readonly static Eng _Current = new Eng();
        public static Eng Current
        {
            get
            {
                return _Current;
            }
        }
        private Eng() { }



        //eventes
        public event EventHandler LoadedMachineConfig;

        //public
        string _PathFile = "\\";
        public string PathFile
        {
            get => _PathFile;
        }

        int _NLoaded = 0;
        public int NLoaded
        {
            get => _NLoaded;
        }

        //private
        MachineConfiguration _MachineConfig = new MachineConfiguration();
        List<GeneralElement> _GeneralElements = new List<GeneralElement>();  //to load from xml




        //methods
        void LoadSetMchineconfig(MachineConfiguration conf, string xmlFileTextLoaded)
        {
            if (conf == null)
                _MachineConfig = new MachineConfiguration();
            else
                _MachineConfig = conf;

            //load properties
            _GeneralElements.Clear();
            foreach (PropertyInfo prop in _MachineConfig.GetType().GetProperties())
            {
                if (!prop.PropertyType.Name.StartsWith("List"))
                {
                    Attribute attribute = prop.GetCustomAttribute(typeof(InfoPropertyAttribute));
                    GeneralElement gen = new GeneralElement();
                    gen.Name = prop.Name;
                    gen.Value = Mng.GetVal(prop.GetValue(_MachineConfig));
                    gen.IsManagedByProgram = true;

                    if (attribute != null)
                    {
                        gen.Title = ((InfoPropertyAttribute)attribute).Title;
                        gen.ValueIsEditable = ((InfoPropertyAttribute)attribute).IsEditable;
                        gen.TypeData = ((InfoPropertyAttribute)attribute).TypeData;
                    }
                    else
                    {
                        gen.Title = "";
                        gen.ValueIsEditable = true;
                        gen.TypeData = InfoPropertyAttributeTypeData.STRING;
                    }

                    _GeneralElements.Add(gen);
                }
            }

            //load other fields in xml
            if(xmlFileTextLoaded != null)
            {
                foreach (XElement xele in XDocument.Parse(xmlFileTextLoaded).Element("MachineConfiguration").Elements())
                {
                    if (xele.HasElements == false)
                    {
                        bool fnd = false;

                        foreach (PropertyInfo prop in _MachineConfig.GetType().GetProperties())
                        {
                            if (!prop.PropertyType.Name.StartsWith("List"))
                            {
                                if (xele.HasElements == false && xele.Name == prop.Name)
                                {
                                    fnd = true;
                                }
                            }
                        }

                        if (!fnd)
                        {
                            GeneralElement gen = new GeneralElement();
                            gen.Name = xele.Name.LocalName;
                            gen.Value = xele.Value;
                            gen.Title = "field not managed in the program";
                            gen.IsManagedByProgram = false;
                            _GeneralElements.Add(gen);
                        }
                    }
                }
            }

            //checks-------------------------------------------------------------------------------
            GetCheckAll();

            //invoke loaded
            _NLoaded++;
            LoadedMachineConfig?.Invoke(this, EventArgs.Empty);
        }



        public void LoadXml(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"File in {path} not found");

            //deserialize
            string xmlLoaded;
            MachineConfiguration machConf;
            XmlSerializer serializer = new XmlSerializer(typeof(MachineConfiguration));
            using(StreamReader reader = new StreamReader(path))
            {
                xmlLoaded = reader.ReadToEnd();
                using (TextReader tx = new StringReader(xmlLoaded))
                {
                    machConf = (MachineConfiguration)serializer.Deserialize(tx);
                }
            }

            _PathFile = path;
            LoadSetMchineconfig(machConf, xmlLoaded);
        }


        public void NewMachineConfig()
        {
            _PathFile = "";
            LoadSetMchineconfig(new MachineConfiguration(), null);
        }


        public void SaveXml(string path)
        {
            if (_NLoaded == 0)
                return;

            /*  USE machineConfigExample in Properties */

            XDocument xdocExample = XDocument.Parse(Properties.Resources.machineConfigExample);


            //GENERAL
            List<XElement> generalNodes = new List<XElement>();
            XElement lastNode = null;
            foreach (XElement xele in xdocExample.Element("MachineConfiguration").Elements())
            {
                if(xele.HasElements == false)
                {
                    generalNodes.Add(xele);
                }
            }

            foreach(GeneralElement gen in _GeneralElements)
            {
                if(gen.IsManagedByProgram == true) //
                {
                    bool fnd = false;
                    foreach (XElement xele in generalNodes)
                    {
                        if (xele.Name == gen.Name)
                        {
                            xele.Value = Mng.GetVal(gen.Value);
                            fnd = true;
                            lastNode = xele;
                        }
                    }
                    if (!fnd)
                    {
                        XElement xele = new XElement(gen.Name, Mng.GetVal(gen.Value));
                        lastNode.AddAfterSelf(xele);
                        lastNode = xele;
                    }
                }
                else //
                {
                    bool fnd = false;
                    foreach (XElement xele in generalNodes)
                    {
                        if (xele.Name == gen.Name)
                        {
                            xele.Value = Mng.GetVal(gen.Value);
                            fnd = true;
                            lastNode = xele;
                        }
                    }
                    if (!fnd)
                    {
                        XElement xele = new XElement(gen.Name, Mng.GetVal(gen.Value));
                        lastNode.AddAfterSelf(xele);
                        lastNode = xele;
                    }
                }
            }


            //SCREENS
            xdocExample.Element("MachineConfiguration").Element("DisplaySettings").RemoveAll();
            foreach (ScreenGridDisplaySettings screen in _MachineConfig.DisplaySettings)
            {
                XElement xele = new XElement("ScreenGridDisplaySettings",
                                    new XElement("Id", screen.Id),
                                    new XElement("Rows", screen.Rows),
                                    new XElement("Cols", screen.Cols));

                xdocExample.Element("MachineConfiguration").Element("DisplaySettings").Add(xele);
            }


            //NODES
            xdocExample.Element("MachineConfiguration").Element("NodeSettings").RemoveAll();
            foreach (NodeSetting node in _MachineConfig.NodeSettings)
            {
                XElement xele = new XElement("NodeSetting",
                                    new XElement("Id", node.Id),
                                    new XElement("NodeProviderName", node.NodeProviderName),
                                    new XElement("NodeDescription", node.NodeDescription),
                                    new XElement("ServerIP4Address", node.ServerIP4Address),
                                    new XElement("IP4Address", node.IP4Address),
                                    new XElement("Port", node.Port),
                                    new XElement("User", node.User),
                                    new XElement("Key", node.Key),
                                    new XElement("RemoteDesktopType", node.RemoteDesktopType));

                xdocExample.Element("MachineConfiguration").Element("NodeSettings").Add(xele);
            }


            //STATIONS
            xdocExample.Element("MachineConfiguration").Element("StationSettings").RemoveAll();
            foreach (StationSetting st in _MachineConfig.StationSettings)
            {
                XElement xele = new XElement("StationSetting",
                                    new XElement("Node", st.Node),
                                    new XElement("Id", st.Id),
                                    new XElement("StationProviderName", st.StationProviderName),
                                    new XElement("StationDescription", st.StationDescription));

                xdocExample.Element("MachineConfiguration").Element("StationSettings").Add(xele);
            }


            //CAMERAS
            xdocExample.Element("MachineConfiguration").Element("CameraSettings").RemoveAll();
            foreach (CameraSetting cam in _MachineConfig.CameraSettings)
            {
                XElement xele = new XElement("CameraSetting",
                                    new XElement("Id", cam.Id),
                                    new XElement("Station", cam.Station),
                                    new XElement("Node", cam.Node),
                                    new XElement("Head", cam.Head),
                                    new XElement("BufferSize", cam.BufferSize),
                                    new XElement("CameraType", cam.CameraType),
                                    new XElement("CameraDescription", cam.CameraDescription),
                                    new XElement("CameraProviderName", cam.CameraProviderName),
                                    new XElement("PageNumberPosition", cam.PageNumberPosition),
                                    new XElement("DisplayPositionRow", cam.DisplayPositionRow),
                                    new XElement("DisplayPositionCol", cam.DisplayPositionCol),
                                    new XElement("Visualizer", cam.Visualizer),
                                    new XElement("Rotation", cam.Rotation));

                xdocExample.Element("MachineConfiguration").Element("CameraSettings").Add(xele);
            }


            //HISTORICISED TOOLS
            xdocExample.Element("MachineConfiguration").Element("HistoricizedToolsSettings").RemoveAll();
            foreach (HistoricizedToolSetting histo in _MachineConfig.HistoricizedToolsSettings)
            {
                XElement xele = new XElement("HistoricizedToolSetting",
                                    new XElement("Label", histo.Label),
                                    new XElement("NodeId", histo.NodeId),
                                    new XElement("StationId", histo.StationId),
                                    new XElement("ToolIndex", histo.ToolIndex),
                                    new XElement("ParameterIndex", histo.ParameterIndex));

                xdocExample.Element("MachineConfiguration").Element("HistoricizedToolsSettings").Add(xele);
            }


            //save
            xdocExample.Save(path);
        }


        public void SaveDumpImageConfig(string path)
        {
            XDocument xdoc = new XDocument();
            List<XElement> dumps = new List<XElement>();

            //from station
            foreach(StationSetting st in _MachineConfig.StationSettings)
            {
                dumps.Add(new XElement("StationDumpSettings",
                              new XElement("Node", st.Node),
                              new XElement("Id", st.Id),
                              new XElement("Type", "Result"),
                              new XElement("Condition", "Reject"),
                              new XElement("VialsToSave", 1000),
                              new XElement("MaxImages", 5000)));
            }

            //XNamespace ns1 = "xsi";
            XElement xele = new XElement("DumpImagesConfiguration", new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"), new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                                new XElement("DumpImagesToolsAvailable", true),
                                new XElement("CurrentUserSettingsId", 1),
                                new XElement("UserSettings",
                                    new XElement("DumpImagesUserSettings",
                                        new XElement("Id", 1),
                                        new XElement("Label", "Production"),
                                            new XElement("StationsDumpSettings",
                                            dumps.ToArray()))));

            xdoc.Add(xele);
            xdoc.Save(path);
        }




        //****************************************************************************************************
        //Other
        //****************************************************************************************************
        void GetCheckAll()
        {
            GetCheckGenerals(out _);
            GetCheckNodes(out _);
            GetCheckStations(out _);
            GetCheckCameras(out _);
            GetCheckDisplays(out _);
            GetCheckHistoricizedTools(out _);
        }







        //****************************************************************************************************
        //GENERALS
        //****************************************************************************************************
        public void GetCheckGenerals(out List<GeneralElement> generals)
        {
            generals = _GeneralElements;

            //set same values
            foreach(GeneralElement gen in _GeneralElements)
            {
                if(gen.Name == "NumberOfStation")
                {
                    gen.Value = _MachineConfig.StationSettings.Count.ToString();
                    continue;
                }
                if(gen.Name == "NumberOfCamera")
                {
                    gen.Value = _MachineConfig.CameraSettings.Count.ToString();
                }
            }
        }

        public GeneralElement GetGeneralElement(int index)
        {
            return _GeneralElements[index];
        }

        public void SetGeneralElementValue(int index, string value)
        {
            _GeneralElements[index].Value = value;
        }

        public void SetGeneralElementName(int index, string name)
        {
            if(_GeneralElements[index].IsManagedByProgram == true)
            {
                throw new ApplicationException("Cannot change the name of this element");
            }
            else
            {
                _GeneralElements[index].Name = name;
            }
        }

        public void AddGeneral()
        {
            GeneralElement ele = new GeneralElement();
            ele.Name = "customField";
            ele.Value = "";
            //ele.Title
            ele.IsManagedByProgram = false;
            _GeneralElements.Add(ele);
        }

        public void DeleteGeneral(int index)
        {
            if (index >= 0 && index < _GeneralElements.Count)
            {
                if (_GeneralElements[index].IsManagedByProgram == true)
                {
                    throw new ApplicationException("This element cannot be removed");
                }
                _GeneralElements.RemoveAt(index);
            }
        }

        public bool MoveGeneralElement(int index, int direction)
        {
            GeneralElement gen = _GeneralElements[index];
            if(index + direction > _GeneralElements.Count - 1)
            {
                return false;
            }
            if(_GeneralElements[index + direction].IsManagedByProgram == false)
            {
                _GeneralElements.RemoveAt(index);
                _GeneralElements.Insert(index + direction, gen);
                return true;
            }
            return false;
        }





        //****************************************************************************************************
        //NODES
        //****************************************************************************************************
        public void GetCheckNodes(out List<NodeSetting> nodes)
        {
            nodes = _MachineConfig.NodeSettings;

            for (int i = 0; i < nodes.Count; i++)
            {
                //check providers
                if (NodeSetting.Providers.Contains(nodes[i].NodeProviderName) == false)
                {
                    nodes[i].NodeProviderName = null;
                }
            }
        }

        public NodeSetting GetNode(int index)
        {
            return _MachineConfig.NodeSettings[index];
        }

        public string GetNodeDescription(string id)
        {
            foreach(NodeSetting node in _MachineConfig.NodeSettings)
            {
                if (node.Id == id)
                    return node.NodeDescription;
            }
            return null;
        }

        public string GetNodeId(string description)
        {
            foreach(NodeSetting node in _MachineConfig.NodeSettings)
            {
                if (node.NodeDescription == description)
                    return node.Id;
            }
            return null;
        }

        public string[] GetArrayAvaibleNameNodes()
        {
            List<string> list = new List<string>();
            foreach (NodeSetting node in _MachineConfig.NodeSettings)
            {
                if(Mng.IsEmptyOrNull(node.Id) == false)
                {
                    list.Add(node.NodeDescription);
                }
            }
            return list.ToArray();
        }

        public void SetNode(int index, string id, string name, string providername, string serverip4, string ip4, string port, string user, string key, string desktoptype)
        {
            NodeSetting node = _MachineConfig.NodeSettings[index];

            if(node.Id != id)
            {
                //clear in station and cameras
                foreach(StationSetting st in _MachineConfig.StationSettings)
                {
                    if(st.Node == node.Id)
                    {
                        st.Node = null;
                    }
                }
                foreach(CameraSetting cam in _MachineConfig.CameraSettings)
                {
                    if(cam.Node == node.Id)
                    {
                        cam.Node = null;
                        cam.Station = null;
                    }
                }

                node.Id = id;
            }
            node.NodeDescription = name;
            if(node.NodeProviderName != providername)
            {
                node.NodeProviderName = providername;
                if (node.NodeProviderName == NodeSetting.Providers[0]) //GretelNodeBase
                {
                    node.Port = "27015";
                    node.User = "User";
                    node.Key = "spami";
                    node.RemoteDesktopType = "1";
                }
                else if (node.NodeProviderName == NodeSetting.Providers[1]) //EoptisNodeBase2
                {
                    node.Port = "23456";
                    node.User = "root";
                    node.Key = "root";
                    node.RemoteDesktopType = "2";
                }
                else
                {
                    node.NodeProviderName = null;
                }
            }
            else
            {
                node.Port = port;
                node.User = user;
                node.Key = key;
                node.RemoteDesktopType = desktoptype;
            }
            node.ServerIP4Address = serverip4;
            node.IP4Address = ip4;

            GetCheckStations(out _);
            GetCheckCameras(out _);
        }

        public void AddNode()
        {
            NodeSetting node = new NodeSetting();
            _MachineConfig.NodeSettings.Add(node);
            //auto complete
            node.Id = (_MachineConfig.NodeSettings.Count - 1).ToString();
            node.NodeDescription = $"PC{node.Id}";
            //node.NodeProviderName = NodeSetting.Providers[0];
            node.ServerIP4Address = $"10.10.{node.Id}.100";
            node.IP4Address = $"10.10.{node.Id}.1";

            //for update the provider
            SetNode(_MachineConfig.NodeSettings.Count - 1, node.Id, node.NodeDescription, NodeSetting.Providers[0], node.ServerIP4Address, node.IP4Address, node.Port, node.User, node.Key, node.RemoteDesktopType);
        }

        public void DeleteNode(int index)
        {
            if(index >= 0 && index < _MachineConfig.NodeSettings.Count)
            {
                _MachineConfig.NodeSettings.RemoveAt(index);
                GetCheckStations(out _);
                GetCheckCameras(out _);
            }
        }

        public void MoveNode(int index, int direction)
        {
            NodeSetting node = _MachineConfig.NodeSettings[index];
            _MachineConfig.NodeSettings.RemoveAt(index);
            _MachineConfig.NodeSettings.Insert(index + direction, node);
        }

        public void DeleteAllNodes()
        {
            _MachineConfig.NodeSettings.Clear();
            GetCheckStations(out _);
            GetCheckCameras(out _);
        }

        public void AutoCompleteNodeId()
        {
            for(int i = 0; i < _MachineConfig.NodeSettings.Count; i++)
            {
                _MachineConfig.NodeSettings[i].Id = i.ToString();
            }
        }

        public void AddSetStationNamesToNode()
        {
            //remove
            RemoveStationNamesToNode();

            foreach(NodeSetting node in _MachineConfig.NodeSettings)
            {
                string toMod = node.NodeDescription;
                if(toMod.EndsWith(" ") == false)
                {
                    toMod += " ";
                }

                string toAdd = $"(";
                bool fnd = false;
                foreach(StationSetting st in _MachineConfig.StationSettings)
                {
                    if(st.Node == node.Id)
                    {
                        toAdd += $"{st.StationDescription}, ";
                        fnd = true;
                    }
                }

                if(fnd == true)
                {
                    toAdd = toAdd.Remove(toAdd.Length - 2, 2); //to remove last ', '
                    toAdd += ")";
                    toMod += toAdd;
                    node.NodeDescription = toMod;
                }
            }
        }

        public void RemoveStationNamesToNode()
        {
            foreach (NodeSetting node in _MachineConfig.NodeSettings)
            {
                string toMod = node.NodeDescription;
                if(toMod.Contains("(") || toMod.Contains(")"))
                {
                    //for (
                    int first = toMod.IndexOf("(");
                    if(first > 0)
                    {
                        toMod = toMod.Substring(0, first - 1);
                        node.NodeDescription = toMod;
                    }

                    //for )
                    first = toMod.IndexOf(")");
                    if (first > 0)
                    {
                        toMod = toMod.Substring(0, first - 1);
                        node.NodeDescription = toMod;
                    }
                }
            }
        }




        //****************************************************************************************************
        //STATIONS
        //****************************************************************************************************
        public void GetCheckStations(out List<StationSetting> stations)
        {
            stations = _MachineConfig.StationSettings;

            foreach(StationSetting st in stations)
            {
                //check id of nodes
                bool found = false;
                foreach(NodeSetting node in _MachineConfig.NodeSettings)
                {
                    if(node.Id == st.Node)
                    {
                        found = true;
                    }
                }
                if(found == false)
                {
                    st.Node = null;
                }

                //providers
                if(StationSetting.Providers.Contains(st.StationProviderName) == false)
                {
                    st.StationProviderName = null;
                }
            }
        }

        public StationSetting GetStation(int index)
        {
            return _MachineConfig.StationSettings[index];
        }

        public StationSetting GetStation(string name)
        {
            foreach(StationSetting st in _MachineConfig.StationSettings)
            {
                if(st.StationDescription == name)
                {
                    return st;
                }
            }
            return null;
        }

        public string GetstationDescription(string idNode, string idStation)
        {
            foreach (StationSetting st in _MachineConfig.StationSettings)
            {
                if (st.Node == idNode && st.Id == idStation)
                {
                    return st.StationDescription;
                }
            }
            return null;
        }

        public string GetStationId(string nameNode, string nameStation)
        {
            foreach (NodeSetting node in _MachineConfig.NodeSettings)
            {
                if (node.NodeDescription == nameNode)
                {
                    foreach (StationSetting st in _MachineConfig.StationSettings)
                    {
                        if (st.StationDescription == nameStation)
                        {
                            return st.Id;
                        }
                    }
                }
            }
            return null;
        }

        public string[] GetArrayAvaibleNameStations()
        {
            List<string> list = new List<string>();
            foreach (StationSetting st in _MachineConfig.StationSettings)
            {
                list.Add(st.StationDescription);
            }
            return list.ToArray();
        }

        public string[] GetArrayAvaibleNameStations(string idNode)
        {
            List<string> list = new List<string>();
            foreach(StationSetting st in _MachineConfig.StationSettings)
            {
                if(st.Node == idNode && Mng.IsEmptyOrNull(st.StationDescription) == false)
                {
                    list.Add(st.StationDescription);
                }
            }
            return list.ToArray();
        }

        public void SetStation(int index, string nameNode, string id, string name, string provider)
        {
            StationSetting station = _MachineConfig.StationSettings[index];
            string idNode = GetNodeId(nameNode);

            if (station.Node != idNode)
            {
                //clear cams
                foreach(CameraSetting cam in _MachineConfig.CameraSettings)
                {
                    if(cam.Node == station.Node/* && cam.Station == station.Id*/)
                    {
                        //cam.Node = null;
                        cam.Station = null;
                    }
                }

                station.Node = idNode;
            }
            if(station.Id != id)
            {
                //clear cams
                foreach (CameraSetting cam in _MachineConfig.CameraSettings)
                {
                    if (cam.Node == station.Node && cam.Station == station.Id)
                    {
                        //cam.Node = null;
                        cam.Station = null;
                    }
                }

                station.Id = id;
            }
            station.StationDescription = name;
            station.StationProviderName = provider;

            _MachineConfig.StationSettings[index] = station;
            GetCheckCameras(out _);
        }

        public void AddStation()
        {
            StationSetting st = new StationSetting();
            _MachineConfig.StationSettings.Add(st);
            //autocomplete
            st.StationDescription = "ST";
            st.StationProviderName = StationSetting.Providers[0];
        }

        public void DeleteStation(int index)
        {
            if (index >= 0 && index < _MachineConfig.StationSettings.Count)
            {
                _MachineConfig.StationSettings.RemoveAt(index);
                GetCheckCameras(out _);
            }
        }

        public void MoveStation(int index, int direction)
        {
            StationSetting st = _MachineConfig.StationSettings[index];
            _MachineConfig.StationSettings.Remove(st);
            _MachineConfig.StationSettings.Insert(index + direction, st);
        }

        public void DeleteAllStations()
        {
            _MachineConfig.StationSettings.Clear();
            GetCheckCameras(out _);
        }

        public void AutoCompleteStationId()
        {
            foreach(NodeSetting node in _MachineConfig.NodeSettings)
            {
                int inc = 0;
                foreach(StationSetting station in _MachineConfig.StationSettings)
                {
                    if(node.Id == station.Node)
                    {
                        station.Id = inc.ToString();
                        inc++;
                    }
                }
            }
        }




        //****************************************************************************************************
        //CAMERAS
        //****************************************************************************************************
        public void GetCheckCameras(out List<CameraSetting> cameras)
        {
            cameras = _MachineConfig.CameraSettings;

            foreach(CameraSetting cam in cameras)
            {
                //check id of nodes
                bool foundNode = false;
                foreach (NodeSetting node in _MachineConfig.NodeSettings)
                {
                    if (node.Id == cam.Node)
                    {
                        foundNode = true;
                    }
                }
                if (foundNode == false)
                {
                    cam.Node = null;
                    cam.Station = null;
                }

                //check id of node and stations
                bool foundStation = false;
                foreach(NodeSetting node in _MachineConfig.NodeSettings)
                {
                    if(node.Id == cam.Node)
                    {
                        foreach (StationSetting st in _MachineConfig.StationSettings)
                        {
                            if (st.Id == cam.Station)
                            {
                                foundStation = true;
                            }
                        }
                    }
                }
                if(foundStation == false)
                {
                    cam.Station = null;
                }

                //check displays
                if(DisplayExists(cam.PageNumberPosition, cam.DisplayPositionCol, cam.DisplayPositionRow) == false)
                {
                    cam.PageNumberPosition = null;
                    cam.DisplayPositionCol = null;
                    cam.DisplayPositionRow = null;
                }

                //check types
                if (CameraSetting.Types.Contains(cam.CameraType) == false)
                {
                    cam.CameraType = null;
                }
                else
                {
                    if (cam.CameraType == CameraSetting.Types[0]) //GretelCamera
                    {
                        cam.CameraProviderName = "GretelCameraBase";
                        cam.Visualizer = "Gretel";
                    }
                    else if (cam.CameraType == CameraSetting.Types[1]) //EoptisTurbidimeter
                    {
                        cam.CameraProviderName = "EoptisDeviceBase";
                        cam.Visualizer = "Data";
                    }
                    else if(cam.CameraType == CameraSetting.Types[2]) //Hvld
                    {
                        cam.CameraProviderName = "GretelCameraBase";
                        cam.Visualizer = "Hvld";
                    }
                    else
                    {
                        cam.CameraType = null;
                    }
                }

                cam.BufferSize = "10"; //always
                cam.Head = cam.Station; //always
            }
        }

        public CameraSetting GetCamera(int index)
        {
            return _MachineConfig.CameraSettings[index];
        }

        public CameraSetting GetCameraByNameStation(string nameSt)
        {
            StationSetting st = GetStation(nameSt);
            if (st == null)
                return null;

            foreach(CameraSetting cam in _MachineConfig.CameraSettings)
            {
                if(cam.Node == st.Node && cam.Station == st.Id)
                {
                    return cam;
                }
            }
            return null;
        }

        public CameraSetting GetCameraDisplayAssociated(string idDisplay, string col, string row)
        {
            foreach (CameraSetting cam in _MachineConfig.CameraSettings)
            {
                if (cam.PageNumberPosition == idDisplay)
                {
                    if (cam.DisplayPositionCol == col)
                    {
                        if (cam.DisplayPositionRow == row)
                        {
                            return cam;
                        }
                    }
                }
            }
            return null;
        }

        public void SetCamera(int index, string nameNode, string nameStation, string id, string defect, string type, string rotation)
        {
            CameraSetting camera = _MachineConfig.CameraSettings[index];
            string idNode = GetNodeId(nameNode);
            string idStation = GetStationId(nameNode, nameStation);
            bool isToCleardisplays = false;

            if(camera.Node != idNode)
            {
                camera.Node = idNode;
                camera.Station = null;
                isToCleardisplays = true;
            }
            else
            {
                if (camera.Station != idStation)
                {
                    camera.Station = idStation;
                    isToCleardisplays = true;
                }
            }
            if(camera.Id != id)
            {
                isToCleardisplays = true;
                camera.Id = id;
            }
            camera.CameraDescription = defect;
            if(camera.CameraType != type)
            {
                camera.CameraType = type;
                GetCheckCameras(out _);  //to update the providers
            }
            camera.Rotation = rotation;

            //clear dysplays
            if (isToCleardisplays)
            {
                camera.PageNumberPosition = null;
                camera.DisplayPositionCol = null;
                camera.DisplayPositionRow = null;
            }

            _MachineConfig.CameraSettings[index] = camera;
        }

        public void AddCamera()
        {
            CameraSetting cam = new CameraSetting();
            _MachineConfig.CameraSettings.Add(cam);
            //autocomplete
            cam.CameraType = CameraSetting.Types[0];
            cam.Rotation = "270";
            cam.Id = (_MachineConfig.CameraSettings.Count - 1).ToString();
        }

        public void DeleteCamera(int index)
        {
            if (index >= 0 && index < _MachineConfig.CameraSettings.Count)
            {
                _MachineConfig.CameraSettings.RemoveAt(index);
            }
        }

        public void MoveCamera(int index, int direction)
        {
            CameraSetting cam = _MachineConfig.CameraSettings[index];
            _MachineConfig.CameraSettings.Remove(cam);
            _MachineConfig.CameraSettings.Insert(index + direction, cam);
        }

        public void DeleteAllCameras()
        {
            _MachineConfig.CameraSettings.Clear();
        }

        public void CreateCamerasFromStations()
        {
            //memorize values of oldes cams if exists
            List<Tuple<string, string, string, string, string, string>> vals = new List<Tuple<string, string, string, string, string, string>>();
            foreach (CameraSetting cam in _MachineConfig.CameraSettings)
            {
                vals.Add(new Tuple<string, string, string, string, string, string>(cam.CameraDescription, cam.CameraType, cam.Rotation, cam.PageNumberPosition, cam.DisplayPositionCol, cam.DisplayPositionRow));
            }

            //clear and add from stations
            _MachineConfig.CameraSettings.Clear();
            int i = 0;
            foreach (StationSetting st in _MachineConfig.StationSettings)
            {
                CameraSetting cam = new CameraSetting();
                cam.Node = st.Node;
                cam.Station = st.Id;
                cam.Id = i.ToString();
                cam.CameraType = CameraSetting.Types[0];
                cam.Rotation = "270";

                if (i < vals.Count)
                {
                    cam.CameraDescription = vals[i].Item1;
                    cam.CameraType = vals[i].Item2;
                    cam.Rotation = vals[i].Item3;
                    cam.PageNumberPosition = vals[i].Item4;
                    cam.DisplayPositionCol = vals[i].Item5;
                    cam.DisplayPositionRow = vals[i].Item6;
                }

                _MachineConfig.CameraSettings.Add(cam);
                i++;
            }
        }

        public void AutoCompleteCameraId()
        {
            for (int i = 0; i < _MachineConfig.CameraSettings.Count; i++)
            {
                _MachineConfig.CameraSettings[i].Id = i.ToString();
            }
        }




        //****************************************************************************************************
        //DISPLAYS
        //****************************************************************************************************
        public void GetCheckDisplays(out List<ScreenGridDisplaySettings> displays)
        {
            displays = _MachineConfig.DisplaySettings;

            int i = 0;
            foreach(ScreenGridDisplaySettings disp in displays)
            {
                //check the id, if it is wrong fix it
                if(disp.Id != i.ToString())
                {
                    disp.Id = i.ToString();
                }

                //check cols and rows, if they are wrong set them to default 1, if they are greater than the maximum value or less than 1 set them to the maximum or minimum value
                if(int.TryParse(disp.Cols, out int resCol))
                {
                    if (resCol < 1)
                        disp.Cols = "1";
                    if (resCol > 3)
                        disp.Cols = "3";
                }
                else
                    disp.Cols = "1";

                if (int.TryParse(disp.Rows, out int resRow))
                {
                    if (resRow < 1)
                        disp.Rows = "1";
                    if (resRow > 2)
                        disp.Rows = "2";
                }
                else
                    disp.Rows = "1";

                i++;
            }
        }

        public bool DisplayExists(string displayId, string colId, string rowId)
        {
            if(int.TryParse(colId, out int col) == true && int.TryParse(rowId, out int row) == true)
            {
                foreach(ScreenGridDisplaySettings display in _MachineConfig.DisplaySettings)
                {
                    if(display.Id == displayId)
                    {
                        if (int.TryParse(display.Cols, out int Ncols) == true && int.TryParse(display.Rows, out int NRows) == true)
                        {
                            if(col >= 0 && col < Ncols && row >= 0 && row < NRows)
                            {
                                return true;
                            }
                        }
                        else
                            return false;
                    }
                }
            }
            return false;
        }

        public void SetDisplayDial(string idDisplay, string col, string row, string nameSt)
        {
            //remove display from camera which has this display
            foreach(CameraSetting cam in _MachineConfig.CameraSettings)
            {
                if(cam.PageNumberPosition == idDisplay && cam.DisplayPositionCol == col && cam.DisplayPositionRow == row)
                {
                    cam.PageNumberPosition = null;
                    cam.DisplayPositionCol = null;
                    cam.DisplayPositionRow = null;
                }
            }

            CameraSetting camTo = GetCameraByNameStation(nameSt);
            if(camTo != null)
            {
                camTo.PageNumberPosition = idDisplay;
                camTo.DisplayPositionCol = col;
                camTo.DisplayPositionRow = row;
            }
        }

        public void AddDisplay(string nCols, string nRows)
        {
            ScreenGridDisplaySettings screen = new ScreenGridDisplaySettings();
            screen.Cols = nCols;
            screen.Rows = nRows;
            screen.Id = (_MachineConfig.DisplaySettings.Count - 1).ToString();
            _MachineConfig.DisplaySettings.Add(screen);
        }

        public void DeleteDisplay(int index)
        {
            if (index >= 0 && index < _MachineConfig.DisplaySettings.Count)
            {
                _MachineConfig.DisplaySettings.RemoveAt(index);
                GetCheckCameras(out _);
            }
        }

        public void DeleteAllDisplays()
        {
            _MachineConfig.DisplaySettings.Clear();
            GetCheckCameras(out _);
        }

        public void MoveDisplay(int index, int direction)
        {
            ScreenGridDisplaySettings disp1 = _MachineConfig.DisplaySettings[index];
            ScreenGridDisplaySettings disp2 = _MachineConfig.DisplaySettings[index + direction];
            _MachineConfig.DisplaySettings.Remove(disp1);
            _MachineConfig.DisplaySettings.Insert(index + direction, disp1);

            //refresh the id display in cameras
            foreach(CameraSetting cam in _MachineConfig.CameraSettings)
            {
                if(cam.PageNumberPosition == disp1.Id)
                {
                    cam.PageNumberPosition = disp2.Id;
                    continue;
                }
                if (cam.PageNumberPosition == disp2.Id)
                {
                    cam.PageNumberPosition = disp1.Id;
                    continue;
                }
            }
        }




        //****************************************************************************************************
        //HISTORICIZED TOOLS
        //****************************************************************************************************
        public void GetCheckHistoricizedTools(out List<HistoricizedToolSetting> histoTool)
        {
            histoTool = _MachineConfig.HistoricizedToolsSettings;

            foreach(HistoricizedToolSetting histo in histoTool)
            {
                //check id of nodes
                bool foundNode = false;
                foreach (NodeSetting node in _MachineConfig.NodeSettings)
                {
                    if (node.Id == histo.NodeId)
                        foundNode = true;
                }
                if (foundNode == false)
                {
                    histo.NodeId = null;
                    histo.StationId = null;
                }

                //check id of node and stations
                bool foundStation = false;
                foreach (NodeSetting node in _MachineConfig.NodeSettings)
                {
                    if (node.Id == histo.NodeId)
                    {
                        foreach (StationSetting st in _MachineConfig.StationSettings)
                        {
                            if (st.Id == histo.StationId)
                                foundStation = true;
                        }
                    }
                }
                if (foundStation == false)
                {
                    histo.StationId = null;
                }
            }
        }

        public void SetHistoricizedTool(int index, string label, string nameNode, string nameStation, string toolIndex, string parameterIndex)
        {
            HistoricizedToolSetting histo = _MachineConfig.HistoricizedToolsSettings[index];
            string idNode = GetNodeId(nameNode);
            string idStation = GetStationId(nameNode, nameStation);

            if (histo.NodeId != idNode)
            {
                histo.NodeId = idNode;
                histo.StationId = null;
            }
            else
            {
                if (histo.StationId != idStation)
                    histo.StationId = idStation;
            }

            histo.Label = label;
            histo.ToolIndex = toolIndex;
            histo.ParameterIndex = parameterIndex;
        }

        public void AddHistoricizedTool()
        {
            HistoricizedToolSetting histo = new HistoricizedToolSetting();
            _MachineConfig.HistoricizedToolsSettings.Add(histo);
            //auto complete
            histo.NodeId = null;
            histo.StationId = null;
            histo.ToolIndex = "0";
            histo.ParameterIndex = "0";
        }

        public void DeleteHistoricizedTool(int index)
        {
            if (index >= 0 && index < _MachineConfig.HistoricizedToolsSettings.Count)
            {
                _MachineConfig.HistoricizedToolsSettings.RemoveAt(index);
            }
        }

        public void DeleteAllHistoricizedTools()
        {
            _MachineConfig.HistoricizedToolsSettings.Clear();
        }









        public List<ErrorElement> CheckErrors()
        {
            //check
            GetCheckAll();

            List<ErrorElement> errors = new List<ErrorElement>();
            string subject = "";
            int n = 0; //-> for out arguments

            //GENERALS
            subject = "GENERAL";
            //check only not managed names
            List<string> namesGeneral = new List<string>();
            foreach (GeneralElement gen in _GeneralElements)
            {
                if(gen.IsManagedByProgram == false)
                {
                    bool flag = false;
                    foreach (GeneralElement gen2 in _GeneralElements)
                    {
                        if(gen2.IsManagedByProgram == false)
                        {
                            if (gen.Name == gen2.Name)
                            {
                                if (flag)
                                {
                                    if (namesGeneral.Contains(gen.Name) == false)
                                        namesGeneral.Add(gen.Name);
                                }
                                flag = true;
                            }
                        }
                    }
                }
            }
            foreach (string st in namesGeneral)
            {
                int i = 0;
                List<int> vals = new List<int>();
                foreach (GeneralElement gen in _GeneralElements)
                {
                    if(gen.IsManagedByProgram == false)
                    {
                        if (st == gen.Name)
                            vals.Add(i);
                    }
                    i++;
                }
                errors.Add(new ErrorElement(MenuEnum.GENERAL, subject, "Have same Name", vals));
            }

            //check type (only managed by program)
            for(int i = 0; i < _GeneralElements.Count; i++)
            {
                GeneralElement gen = _GeneralElements[i];
                if (gen.IsManagedByProgram)
                {
                    if (gen.TypeData == InfoPropertyAttributeTypeData.INT)
                    {
                        if (int.TryParse(gen.Value, out _) == false)
                        {
                            errors.Add(new ErrorElement(MenuEnum.GENERAL, subject, "Must be an integer number", i));
                        }
                    }
                    else if(gen.TypeData == InfoPropertyAttributeTypeData.DOUBLE)
                    {
                        if(double.TryParse(gen.Value, out _) == false)
                        {
                            errors.Add(new ErrorElement(MenuEnum.GENERAL, subject, "Must be a decimal number", i));
                        }
                    }
                    else if(gen.TypeData == InfoPropertyAttributeTypeData.BOOL)
                    {
                        if(bool.TryParse(gen.Value, out _) == false)
                        {
                            errors.Add(new ErrorElement(MenuEnum.GENERAL, subject, "Must be true or false", i));
                        }
                    }
                }
            }


            //SINLE NODE
            subject = "NODE";
            for (int i = 0; i < _MachineConfig.NodeSettings.Count; i++)
            {
                NodeSetting node = _MachineConfig.NodeSettings[i];

                if (Mng.IsEmptyOrNull(node.NodeDescription))
                {
                    errors.Add(new ErrorElement(MenuEnum.NODE, subject, "The Name cannot be empty or only with white spaces", i));
                }

                if (Int32.TryParse(node.Id, out n))
                {
                    if (n < 0)
                        errors.Add(new ErrorElement(MenuEnum.NODE, subject, "The Id must be >= 0", i));
                }
                else
                {
                    errors.Add(new ErrorElement(MenuEnum.NODE, subject, "The Id must be a number", i));
                }

                if (IPAddress.TryParse(node.ServerIP4Address, out _) == false)
                {
                    errors.Add(new ErrorElement(MenuEnum.NODE, subject, "The ServerIp4Adress must be a valid ip adress", i));
                }

                if (IPAddress.TryParse(node.IP4Address, out _) == false)
                {
                    errors.Add(new ErrorElement(MenuEnum.NODE, subject, "The IP4Address must be a valid ip adress", i));
                }

                if (Mng.IsEmptyOrNull(node.NodeProviderName))
                {
                    errors.Add(new ErrorElement(MenuEnum.NODE, subject, "The ProviderName cannot be empty", i));
                }

                if (Mng.IsEmptyOrNull(node.User))
                {
                    errors.Add(new ErrorElement(MenuEnum.NODE, subject, "The User cannot be empty", i));
                }

                if (Mng.IsEmptyOrNull(node.Key))
                {
                    errors.Add(new ErrorElement(MenuEnum.NODE, subject, "The Key cannot be empty", i));
                }

                if (Int32.TryParse(node.Port, out n))
                {
                    if (n < 0)
                        errors.Add(new ErrorElement(MenuEnum.NODE, subject, "The Port must be >= 0", i));
                }
                else
                {
                    errors.Add(new ErrorElement(MenuEnum.NODE, subject, "The Port must be a number", i));
                }
            }


            //ALL NODES
            AllNodes();
            void AllNodes()
            {
                //count=0
                if(_MachineConfig.NodeSettings.Count == 0)
                {
                    errors.Add(new ErrorElement(MenuEnum.NODE, subject, "No nodes are present"));
                }

                //acending order id
                for(int i = 0; i < _MachineConfig.NodeSettings.Count; i++)
                {
                    if(Mng.IsEmptyOrNull(_MachineConfig.NodeSettings[i].Id) || _MachineConfig.NodeSettings[i].Id.ToString() != i.ToString())
                    {
                        errors.Add(new ErrorElement(MenuEnum.NODE, subject, "Elements Id must start at 0 and continue ascending"));
                        break;
                    }
                }

                //equal id
                List<string> ids = new List<string>();
                foreach(NodeSetting node in _MachineConfig.NodeSettings)
                {
                    bool flag = false;
                    foreach(NodeSetting node2 in _MachineConfig.NodeSettings)
                    {
                        if(node.Id == node2.Id)
                        {
                            if (flag)
                            {
                                if (ids.Contains(node.Id) == false)
                                    ids.Add(node.Id);
                            }
                            flag = true;
                        }
                    }
                }
                foreach (string st in ids)
                {
                    int i = 0;
                    List<int> vals = new List<int>();
                    foreach (NodeSetting node in _MachineConfig.NodeSettings)
                    {
                        if (st == node.Id)
                            vals.Add(i);
                        i++;
                    }
                    errors.Add(new ErrorElement(MenuEnum.NODE, subject, "Have same Id", vals));
                }

                //equal description
                List<string> descriptions = new List<string>();
                foreach (NodeSetting node in _MachineConfig.NodeSettings)
                {
                    bool flag = false;
                    foreach (NodeSetting node2 in _MachineConfig.NodeSettings)
                    {
                        if (node.NodeDescription == node2.NodeDescription)
                        {
                            if (flag)
                            {
                                if (descriptions.Contains(node.NodeDescription) == false)
                                    descriptions.Add(node.NodeDescription);
                            }
                            flag = true;
                        }
                    }
                }
                foreach (string st in descriptions)
                {
                    int i = 0;
                    List<int> vals = new List<int>();
                    foreach (NodeSetting node in _MachineConfig.NodeSettings)
                    {
                        if (st == node.NodeDescription)
                            vals.Add(i);
                        i++;
                    }
                    errors.Add(new ErrorElement(MenuEnum.NODE, subject, "Have same Name", vals));
                }

                ////////////equal serverip4
                //////////List<string> serverip4s = new List<string>();
                //////////foreach (NodeSetting node in _MachineConfig.NodeSettings)
                //////////{
                //////////    bool flag = false;
                //////////    foreach (NodeSetting node2 in _MachineConfig.NodeSettings)
                //////////    {
                //////////        if (node.ServerIP4Address == node2.ServerIP4Address)
                //////////        {
                //////////            if (flag)
                //////////            {
                //////////                if (serverip4s.Contains(node.ServerIP4Address) == false)
                //////////                    serverip4s.Add(node.ServerIP4Address);
                //////////            }
                //////////            flag = true;
                //////////        }
                //////////    }
                //////////}
                //////////foreach (string st in serverip4s)
                //////////{
                //////////    int i = 0;
                //////////    List<int> vals = new List<int>();
                //////////    foreach (NodeSetting node in _MachineConfig.NodeSettings)
                //////////    {
                //////////        if (st == node.ServerIP4Address)
                //////////            vals.Add(i);
                //////////        i++;
                //////////    }
                //////////    errors.Add(new ErrorElement(MenuEnum.NODE, subject, "Have same ServerIP4Address", vals));
                //////////}

                //equal ip4
                List<string> ip4s = new List<string>();
                foreach (NodeSetting node in _MachineConfig.NodeSettings)
                {
                    bool flag = false;
                    foreach (NodeSetting node2 in _MachineConfig.NodeSettings)
                    {
                        if (node.IP4Address == node2.IP4Address)
                        {
                            if (flag)
                            {
                                if (ip4s.Contains(node.IP4Address) == false)
                                    ip4s.Add(node.IP4Address);
                            }
                            flag = true;
                        }
                    }
                }
                foreach (string st in ip4s)
                {
                    int i = 0;
                    List<int> vals = new List<int>();
                    foreach (NodeSetting node in _MachineConfig.NodeSettings)
                    {
                        if (st == node.IP4Address)
                            vals.Add(i);
                        i++;
                    }
                    errors.Add(new ErrorElement(MenuEnum.NODE, subject, "Have same IP4Address", vals));
                }
            }


            //SINGLE STATION
            subject = "STATION";
            for(int i = 0; i < _MachineConfig.StationSettings.Count; i++)
            {
                StationSetting station = _MachineConfig.StationSettings[i];

                if (Mng.IsEmptyOrNull(station.Node))
                {
                    errors.Add(new ErrorElement(MenuEnum.STATION, subject, "The Node Name cannot be empty", i));
                }

                if (Int32.TryParse(station.Id, out n))
                {
                    if (n < 0)
                        errors.Add(new ErrorElement(MenuEnum.STATION, subject, "The Id must be >= 0", i));
                }
                else
                {
                    errors.Add(new ErrorElement(MenuEnum.STATION, subject, "The Id must be a number", i));
                }

                if (Mng.IsEmptyOrNull(station.StationDescription))
                {
                    errors.Add(new ErrorElement(MenuEnum.STATION, subject, "The Name cannot be empty or only with white spaces", i));
                }

                if (Mng.IsEmptyOrNull(station.StationProviderName))
                {
                    errors.Add(new ErrorElement(MenuEnum.STATION, subject, "The ProviderName cannot be empty", i));
                }
            }

            //ALL STATIONS
            AllStations();
            void AllStations()
            {
                //count=0
                if (_MachineConfig.StationSettings.Count == 0)
                {
                    errors.Add(new ErrorElement(MenuEnum.STATION, subject, "No stations are present"));
                }

                //equal description
                List<string> descriptions = new List<string>();
                foreach (StationSetting station in _MachineConfig.StationSettings)
                {
                    bool flag = false;
                    foreach (StationSetting station2 in _MachineConfig.StationSettings)
                    {
                        if (station.StationDescription == station2.StationDescription)
                        {
                            if (flag)
                            {
                                if (descriptions.Contains(station.StationDescription) == false)
                                    descriptions.Add(station.StationDescription);
                            }
                            flag = true;
                        }
                    }
                }
                foreach (string st in descriptions)
                {
                    int i = 0;
                    List<int> vals = new List<int>();
                    foreach (StationSetting station in _MachineConfig.StationSettings)
                    {
                        if (st == station.StationDescription)
                            vals.Add(i);
                        i++;
                    }
                    errors.Add(new ErrorElement(MenuEnum.STATION, subject, "Have same Name", vals));
                }

                //Nodes and id
                List<string> nodeIds = new List<string>();
                foreach (StationSetting station in _MachineConfig.StationSettings)
                {
                    if (!nodeIds.Contains(station.Node))
                    {
                        nodeIds.Add(station.Node);
                    }
                }
                bool isWrong = false;
                foreach (string item in nodeIds)
                {
                    int indexForItem = 0;
                    int indexStation = 0;
                    foreach (StationSetting station in _MachineConfig.StationSettings)
                    {
                        if (station.Node == item)
                        {
                            if (station.Id != indexForItem.ToString())
                            {
                                errors.Add(new ErrorElement(MenuEnum.STATION, subject, "The Id is wrong", indexStation));
                                isWrong = true;
                            }
                            indexForItem++;
                        }
                        indexStation++;
                    }
                }
                if (isWrong)
                {
                    errors.Add(new ErrorElement(MenuEnum.STATION, subject, "for each NODE the Id must start at 0 and continue ascending"));
                }
            }


            //SINGLE CAMERA
            subject = "CAMERA";
            for(int i = 0; i < _MachineConfig.CameraSettings.Count; i++)
            {
                CameraSetting camera = _MachineConfig.CameraSettings[i];

                if (Mng.IsEmptyOrNull(camera.Node))
                {
                    errors.Add(new ErrorElement(MenuEnum.CAMERA, subject, "The Node Name cannot be empty", i));
                }

                if (Mng.IsEmptyOrNull(camera.Station))
                {
                    errors.Add(new ErrorElement(MenuEnum.CAMERA, subject, "The Station Name cannot be empty", i));
                }

                if (Int32.TryParse(camera.Id, out n))
                {
                    if (n < 0)
                        errors.Add(new ErrorElement(MenuEnum.CAMERA, subject, "The Id must be >= 0", i));
                }
                else
                {
                    errors.Add(new ErrorElement(MenuEnum.CAMERA, subject, "The Id must be a number", i));
                }

                if (Mng.IsEmptyOrNull(camera.CameraDescription))
                {
                    errors.Add(new ErrorElement(MenuEnum.CAMERA, subject, "The Defect cannot be empty or only with white spaces", i));
                }

                if (Mng.IsEmptyOrNull(camera.CameraType))
                {
                    errors.Add(new ErrorElement(MenuEnum.CAMERA, subject, "The Type cannot be empty", i));
                }

                if (Int32.TryParse(camera.Rotation, out n))
                {
                    if (n < 0 || n > 360)
                        errors.Add(new ErrorElement(MenuEnum.CAMERA, subject, "The Rotation must be between 0 and 360", i));
                }
                else
                {
                    errors.Add(new ErrorElement(MenuEnum.CAMERA, subject, "The Rotation must be a number", i));
                }

                /////////////////////////////////////////////////////////////////////////// display
                if(DisplayExists(camera.PageNumberPosition, camera.DisplayPositionCol, camera.DisplayPositionRow) == false)
                {
                    errors.Add(new ErrorElement(MenuEnum.CAMERA, subject, "The camera is not associated with any display", i));
                }
            }

            //ALL CAMERAS
            AllCameras();
            void AllCameras()
            {
                //count=0
                if (_MachineConfig.CameraSettings.Count == 0)
                {
                    errors.Add(new ErrorElement(MenuEnum.CAMERA, subject, "No cameras are present"));
                }

                //acending order id
                for (int i = 0; i < _MachineConfig.CameraSettings.Count; i++)
                {
                    if (Mng.IsEmptyOrNull(_MachineConfig.CameraSettings[i].Id) || _MachineConfig.CameraSettings[i].Id.ToString() != i.ToString())
                    {
                        errors.Add(new ErrorElement(MenuEnum.CAMERA, subject, "Elements Id must start at 0 and continue ascending"));
                        break;
                    }
                }

                //equal id
                List<string> ids = new List<string>();
                foreach (CameraSetting camera in _MachineConfig.CameraSettings)
                {
                    bool flag = false;
                    foreach (CameraSetting camera2 in _MachineConfig.CameraSettings)
                    {
                        if (camera.Id == camera2.Id)
                        {
                            if (flag)
                            {
                                if (ids.Contains(camera.Id) == false)
                                    ids.Add(camera.Id);
                            }
                            flag = true;
                        }
                    }
                }
                foreach (string st in ids)
                {
                    int i = 0;
                    List<int> vals = new List<int>();
                    foreach (CameraSetting camera in _MachineConfig.CameraSettings)
                    {
                        if (st == camera.Id)
                            vals.Add(i);
                        i++;
                    }
                    errors.Add(new ErrorElement(MenuEnum.CAMERA, subject, "Have same Id", vals));
                }

                //equal node and station
                List<string> list = new List<string>();
                foreach (CameraSetting camera in _MachineConfig.CameraSettings)
                {
                    bool flag = false;
                    foreach (CameraSetting camera2 in _MachineConfig.CameraSettings)
                    {
                        if(camera.Node != null && camera.Station != null)
                        {
                            if ((camera.Node + camera.Station) == (camera2.Node + camera2.Station))
                            {
                                if (flag)
                                {
                                    if (!list.Contains(camera.Node + camera.Station))
                                        list.Add(camera.Node + camera.Station);
                                }
                                flag = true;
                            }
                        }
                    }
                }
                foreach (string val in list)
                {
                    int i = 0;
                    List<int> indixces = new List<int>();
                    foreach (CameraSetting camera in _MachineConfig.CameraSettings)
                    {
                        if (camera.Node + camera.Station == val)
                            indixces.Add(i);
                        i++;
                    }
                    errors.Add(new ErrorElement(MenuEnum.CAMERA, subject, "Have same Node and Station", indixces));
                }
            }


            //DISPLAYS
            subject = "DISPLAY";
            int indexdisp = 0;
            foreach(ScreenGridDisplaySettings disp in _MachineConfig.DisplaySettings)
            {
                //simulate dials
                if(int.TryParse(disp.Cols, out int NCols) && int.TryParse(disp.Rows, out int NRows))
                {
                    for (int iRow = 0; iRow < NRows; iRow++)
                    //for(int iCol = 0; iCol < NCols; iCol++)
                    {
                        for (int iCol = 0; iCol < NCols; iCol++)
                        //for(int iRow = 0; iRow < NRows; iRow++)
                        {
                            bool found = false;

                            foreach (CameraSetting cam in _MachineConfig.CameraSettings)
                            {
                                if(cam.PageNumberPosition == disp.Id && cam.DisplayPositionCol == iCol.ToString() && cam.DisplayPositionRow == iRow.ToString())
                                {
                                    found = true;
                                }
                            }

                            if (!found)
                            {
                                errors.Add(new ErrorElement(MenuEnum.DISPLAY, subject, "Display is not associated with any camera", indexdisp, disp.Id, iCol.ToString(), iRow.ToString()));
                            }

                            indexdisp++;
                        }
                    }
                }
                //else
                //{
                //    errors.Add(new ErrorElement(MenuEnum.DISPLAY, subject, "Error", indexdisp));
                //}
            }

            //count Display=0
            if (_MachineConfig.DisplaySettings.Count == 0)
            {
                errors.Add(new ErrorElement(MenuEnum.DISPLAY, subject, "No displays are present"));
            }



            //SINGLE HISTORICIZED TOOLS
            subject = "HISTORICISED TOOL";
            for (int i = 0; i < _MachineConfig.HistoricizedToolsSettings.Count; i++)
            {
                HistoricizedToolSetting histo = _MachineConfig.HistoricizedToolsSettings[i];

                if (Mng.IsEmptyOrNull(histo.NodeId))
                {
                    errors.Add(new ErrorElement(MenuEnum.HISTORICIZED_TOOLS, subject, "The Node Name cannot be empty", i));
                }

                if (Mng.IsEmptyOrNull(histo.StationId))
                {
                    errors.Add(new ErrorElement(MenuEnum.HISTORICIZED_TOOLS, subject, "The Station Name cannot be empty", i));
                }

                if (Int32.TryParse(histo.ToolIndex, out n))
                {
                    if (n < 0)
                        errors.Add(new ErrorElement(MenuEnum.HISTORICIZED_TOOLS, subject, "The Tool Index must be >= 0", i));
                }
                else
                {
                    errors.Add(new ErrorElement(MenuEnum.HISTORICIZED_TOOLS, subject, "The Tool Index must be a number", i));
                }

                if (Int32.TryParse(histo.ParameterIndex, out n))
                {
                    if (n < 0)
                        errors.Add(new ErrorElement(MenuEnum.HISTORICIZED_TOOLS, subject, "The Parameter Index must be >= 0", i));
                }
                else
                {
                    errors.Add(new ErrorElement(MenuEnum.HISTORICIZED_TOOLS, subject, "The Parameter Index must be a number", i));
                }
            }

            //ALL HISTORICIZED TOOLS
            AllHistoricizedTools();
            void AllHistoricizedTools()
            {
                List<string> list = new List<string>();
                foreach (HistoricizedToolSetting histo in _MachineConfig.HistoricizedToolsSettings)
                {
                    bool flag = false;
                    foreach (HistoricizedToolSetting histo2 in _MachineConfig.HistoricizedToolsSettings)
                    {
                        string fullHisto = histo.NodeId + histo.StationId + histo.ToolIndex + histo.ParameterIndex;
                        string fullHisto2 = histo2.NodeId + histo2.StationId + histo2.ToolIndex + histo2.ParameterIndex;
                        if (fullHisto == fullHisto2)
                        {
                            if (flag)
                            {
                                if (!list.Contains(fullHisto))
                                    list.Add(fullHisto);
                            }
                            flag = true;
                        }
                    }
                }
                foreach (string val in list)
                {
                    int i = 0;
                    List<int> indixces = new List<int>();
                    foreach (HistoricizedToolSetting histo in _MachineConfig.HistoricizedToolsSettings)
                    {
                        string fullHisto = histo.NodeId + histo.StationId + histo.ToolIndex + histo.ParameterIndex;
                        if (fullHisto == val)
                            indixces.Add(i);
                        i++;
                    }
                    errors.Add(new ErrorElement(MenuEnum.HISTORICIZED_TOOLS, subject, "Have the same values", indixces));
                }
            }

            return errors;
        }
    }
}
