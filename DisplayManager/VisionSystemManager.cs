using ExactaEasyCore;
using ExactaEasyEng;
using SPAMI.Util.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;

namespace DisplayManager
{
    /// <summary>
    /// Refactored on 27/06/2024 by Matteo M. to solve the 
    /// communication problems with GRETEL (recipe received ACKs coming after timeout).
    /// </summary>
    public class VisionSystemManager : IObservable 
    {
        /// <summary>
        /// 
        /// </summary>
        private const int MAX_NODES_COUNT = 8;
        /// <summary>
        /// 
        /// </summary>
        private const int MAX_STATIONS_COUNT = 32;
        /// <summary>
        /// Monitor lock object for the rescanning function.
        /// </summary>
        private static object _rescanLockObject = new object();
        /// <summary>
        /// 
        /// </summary>
        private static object _batchLockObject = new object();
        /// <summary>
        /// 
        /// </summary>
        private static ResourceManager uiStrings = null;
        /// <summary>
        /// 
        /// </summary>
        private DateTime _nodesDateTime;
        /// <summary>
        /// 
        /// </summary>
        private bool _settingParameters = false;
        /// <summary>
        /// 
        /// </summary>
        public static ResourceManager UIStrings 
        {
            get
            {
                if (uiStrings is null) 
                    uiStrings = new ResourceManager("DisplayManager.UIStrings", Assembly.GetExecutingAssembly());
                return uiStrings;
            }
        }
        /// <summary>
        /// Vision system manager configuration.
        /// </summary>
        private VisionSystemConfig _configuration;
        /// <summary>
        /// 
        /// </summary>
        public VisionSystemConfig Configuration
        {
            get
            {
                return _configuration;
            }

            private set
            {
                _configuration = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public NodeCollection Nodes { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public StationCollection Stations { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public CameraCollection Cameras { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public DisplayCollection Displays { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public GraphCollection Graphs { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public MeasuresContainerCollection MeasuresContainer { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public MeasuresContainerCollection DBMeasures { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public KnappManager KnappManager { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Initialized { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public bool SavingBatch { get; set; }   //almeno un nodo sta salvando
        /// <summary>
        /// 
        /// </summary>
        public bool SavingBatchAllNodes { get; set; }   //tutti i nodi stanno salvando
        /// <summary>
        /// 
        /// </summary>
        private CameraDefinitionCollection cameraBlackList = new CameraDefinitionCollection();
        /// <summary>
        /// 
        /// </summary>
        private NodeDefinitionCollection nodeBlackList = new NodeDefinitionCollection();
        /// <summary>
        /// 
        /// </summary>
        private ManualResetEvent _rescanAvailable = new ManualResetEvent(false);
        /// <summary>
        /// 
        /// </summary>
        public ManualResetEvent SavingBatchAllNodesEvt = new ManualResetEvent(false);
        /// <summary>
        /// 
        /// </summary>
        public ManualResetEvent NotSavingBatchAllNodesEvt = new ManualResetEvent(false);
        /// <summary>
        /// 
        /// </summary>
        public List<IStrobeController> StrobeControllers { get; private set; }

        #region Events

        //public event EventHandler<MessageEventArgs> CameraStatusChanged;
        public event EventHandler<TaskStatusEventArgs> TaskStatusUpdate;
        public event EventHandler OnStartRescan;
        public event EventHandler RescanCompleted;
        public event EventHandler InitializationCompleted;
        public event EventHandler SavingBatchChanged;
        public event EventHandler RemoteDesktopDisconnection;
        public event EventHandler<NodeRecipeEventArgs> ChangeRecipe;
        //public event EventHandler DeviceError;
        public event EventHandler NewNodeBootDone;
        public event EventHandler<MessageEventArgs> SavingBufferedImages;
        public event EventHandler<MessageEventArgs> NotifyGUI;
        public event EventHandler<MessageEventArgs> EnableNavigation;
        //public event EventHandler RescanRequested;

        #endregion Events

        /// <summary>
        /// 
        /// </summary>
        private static IStrobeController CreateStroboController(int stroboId, string stroboType, string stroboAddress, string configFileName) 
        {
            if (string.IsNullOrWhiteSpace(stroboType))
                return null;

            switch (stroboType.ToUpper())
            {
                case "GARDASOFT":
                    GardaSoft g = new GardaSoft(stroboId, stroboAddress, configFileName);
                    return g;

                case "TGARDASOFT":
                    TGardaSoft tg = new TGardaSoft(stroboId, stroboAddress, configFileName);
                    return tg;

                case "TESTSTROBO":
                    TestStrobeController t = new TestStrobeController(stroboId, stroboAddress, configFileName);
                    return t;

                case "SMARTEK":
                    Smartek st = new Smartek(stroboId, stroboAddress, "");
                    return st;

                default:
                    return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static VisionSystemManager CreateVisionSystemManager(MachineConfiguration machineConfiguration) 
        {
            VisionSystemConfig vsConf = new VisionSystemConfig();

            //CameraManagerConfig cmConf = new CameraManagerConfig();
            vsConf.NodeBootTimeoutSec = machineConfiguration.NodeBootTimeoutSec;
            vsConf.MaxInspectionQueueLength = machineConfiguration.MaxInspectionQueueLength;
            vsConf.EnableImgToSave = machineConfiguration.EnableImgToSave;
            vsConf.MaxInspectionImgToSave = machineConfiguration.MaxInspectionImgToSave;
            vsConf.CameraProviders = machineConfiguration.CameraProviders;
            foreach (StrobeSetting ss in machineConfiguration.StrobeSettings) {
                vsConf.StrobeControllersDefinition.Add(new StrobeControllerDefinition() {
                    Id = ss.Id,
                    StrobeProviderName = ss.StrobeProviderName,
                    Path = machineConfiguration.ConfigPath + ss.Path,
                    IP = ss.IP
                });
            }
            vsConf.StationProviders = machineConfiguration.StationProviders;
            vsConf.NodeProviders = machineConfiguration.NodeProviders;
            vsConf.KnappSettings = machineConfiguration.KnappSettings;
            if (vsConf.KnappSettings != null)
                vsConf.KnappSettings.NumberOfSpindles = machineConfiguration.NumberOfSpindles;
            CameraDefinitionCollection cdm = new CameraDefinitionCollection();
            foreach (CameraSetting cs in machineConfiguration.CameraSettings) {
                CameraDefinition newCd = new CameraDefinition() {
                    Id = cs.Id,
                    Head = cs.Head,
                    BufferSize = cs.BufferSize,
                    CameraDescription = cs.CameraDescription,
                    CameraProviderName = cs.CameraProviderName,
                    CameraType = cs.CameraType,
                    IP4Address = cs.IP4Address,
                    Spindle = cs.Spindle,
                    Station = cs.Station,
                    Node = cs.Node,
                    Visualizer = cs.Visualizer,
                    Rotation = cs.Rotation
                };
                foreach (LightControllerSettings ls in cs.LightControllers) {
                    newCd.LightControllers.Add(new LightControllerDefinition() {
                        Id = ls.Id,
                        Description = ls.Description,
                        StrobeId = ls.StrobeId,
                        StrobeChannel = ls.StrobeChannel
                    });
                }
                cdm.Add(newCd);
            }
            StationDefinitionCollection sdm = new StationDefinitionCollection();
            foreach (StationSetting ss in machineConfiguration.StationSettings) {
                StationDefinition newSd = new StationDefinition() {
                    Id = ss.Id,
                    StationDescription = ss.StationDescription,
                    StationProviderName = ss.StationProviderName,
                    Node = ss.Node,
                    ToolResultsToStoreCollection = ss.ToolResultsToStoreCollection
                };
                //if (newSd.ToolResultsToStoreCollection != null && newSd.ToolResultsToStoreCollection.Count > 0)
                //    sendMeasures2Scada = true;
                sdm.Add(newSd);
            }
            NodeDefinitionCollection ndm = new NodeDefinitionCollection();
            foreach (NodeSetting ns in machineConfiguration.NodeSettings) {
                NodeDefinition newNd = new NodeDefinition() {
                    Id = ns.Id,
                    NodeDescription = ns.NodeDescription,
                    NodeProviderName = ns.NodeProviderName,
                    ServerIP4Address = ns.ServerIP4Address,
                    IP4Address = ns.IP4Address,
                    Port = ns.Port,
                    RemoteDesktopType = ns.RemoteDesktopType,
                    User = ns.User,
                    Key = ns.Key,
                    RDPort = ns.RDPort
                };
                ndm.Add(newNd);
            }
            vsConf.CamerasDefinition = cdm;
            vsConf.StationDefinition = sdm;
            vsConf.NodesDefinition = ndm;
            VisionSystemManager visionSystemMgr = new VisionSystemManager(vsConf);
            if (machineConfiguration.SaveData2DB) {
                _DBConnection = new DBConnection(machineConfiguration.DBServer, machineConfiguration.DBName, machineConfiguration.DBTable);
            }
            return visionSystemMgr;
        }
        /// <summary>
        /// 
        /// </summary>
        public VisionSystemManager(string configurationFile) : this() 
        {
            if (File.Exists(configurationFile))
                Configuration = VisionSystemConfig.LoadFromFile(configurationFile);
            else
                Configuration = null; // Innescare eccezione ?
        }
        /// <summary>
        /// 
        /// </summary>
        public VisionSystemManager(VisionSystemConfig configuration) : this() 
        {
            Configuration = configuration;
            KnappManager = new KnappManager(Configuration.KnappSettings, Stations);
            TaskObserver.TheObserver.AddObservable(this);
            
            if (Configuration.NodesDefinition == null ||
                Configuration.NodesDefinition.Count == 0) 
            {
                foreach (CameraDefinition camDef in Configuration.CamerasDefinition) 
                {
                    OnTaskStatusUpdate(this, 
                        new TaskStatusEventArgs("VisionSysManagerCameraCreation" + camDef.Id.ToString(), 
                        ExactaEasyCore.TaskStatus.Running, 
                        camDef.CameraDescription + ": Creating..."));
                }
            }
            else 
            {
                foreach (NodeDefinition nodeDef in Configuration.NodesDefinition)
                {
                    OnTaskStatusUpdate(this,
                           new TaskStatusEventArgs("VisionSysManagerNodeConnection" + nodeDef.Id.ToString(),
                           ExactaEasyCore.TaskStatus.Running,
                           nodeDef.NodeDescription + ": Creating..."));
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        VisionSystemManager() 
        {
            Cameras = new CameraCollection();
            Stations = new StationCollection();
            Nodes = new NodeCollection();
            Displays = new DisplayCollection();
            Graphs = new GraphCollection();
            MeasuresContainer = new MeasuresContainerCollection();
            DBMeasures = new MeasuresContainerCollection();
            StrobeControllers = new List<IStrobeController>();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Init() 
        {
            if (Configuration.NodesDefinition is null ||
                Configuration.NodesDefinition.Count == 0) 
            {
                foreach (CameraDefinition camDef in Configuration.CamerasDefinition)
                {
                    OnTaskStatusUpdate(this,
                        new TaskStatusEventArgs("VisionSysManagerCameraCreation" + camDef.Id.ToString(),
                        ExactaEasyCore.TaskStatus.Running,
                        camDef.CameraDescription + ": Creating..."));
                }
            }
            else 
            {
                foreach (NodeDefinition nodeDef in Configuration.NodesDefinition) 
                {
                    OnTaskStatusUpdate(this, 
                        new TaskStatusEventArgs("VisionSysManagerNodeConnection" + nodeDef.Id.ToString(), 
                        ExactaEasyCore.TaskStatus.Running, 
                        nodeDef.NodeDescription + ": Creating..."));
                }
            }
            foreach (StrobeControllerDefinition strobeDef in Configuration.StrobeControllersDefinition) 
            {
                try 
                {
                    IStrobeController newStrobo = CreateStroboController(strobeDef.Id, strobeDef.StrobeProviderName, strobeDef.IP, strobeDef.Path);
                    if (newStrobo != null) 
                    {
                        StrobeControllers.Add(newStrobo);
                        if (newStrobo is Device) 
                        {
                            Device d = (Device)newStrobo;
                            d.CommunicationStateChanged += d_CommunicationStateChanged;
                            //d.StartConnect(true);   //BUG YURIA  13/01/2015
                            d.Connect();
                        }
                    }
                }
                catch (Exception ex) 
                {
                    Log.Line(
                        LogLevels.Error,
                        "VisionSystemManager.Init", "Invalid strobe controller: " + ex.Message);
                }
            }

            if (Configuration.NodesDefinition != null) 
            {
                foreach (NodeDefinition nodeDef in Configuration.NodesDefinition)
                {
                    try 
                    {
                        nodeDef.StationsCount = 0;
                        foreach (StationDefinition statDef in Configuration.StationDefinition) 
                        {
                            if (statDef.Node == nodeDef.Id)
                                nodeDef.StationsCount++;
                        }
                        Node newNode = Node.CreateNode(nodeDef);
                        newNode.NodeRecipeUpdate += newNode_NodeRecipeUpdate;
                        newNode.ClientAuditMessage += newNode_ClientAuditMessage;
                        newNode.DumpingChanged += newNode_DumpingChanged;
                        newNode.RemoteDesktopDisconnected += newNode_RemoteDesktopDisconnected;
                        newNode.BootDone += newNode_BootDone;
                        newNode.ReceivedGretelInfo += NewNode_ReceivedGretelInfo;
                        newNode.SavingBufferedImages += newNode_SavingBufferedImages;
                        newNode.EnableNavigation += newNode_EnableNavigation;
                        addNode((INode)newNode);

                        if (nodeBlackList.Exists(nodeD => nodeD.Id == nodeDef.Id)) 
                            nodeBlackList.Remove(nodeDef);
                        //setto la property DumpingEnable in funzione dei settaggi di knapp delle varie stazioni
                        DumpImagesUserSettings diusKnapp = new DumpImagesUserSettings();
                        diusKnapp.StationsDumpSettings = new List<StationDumpSettings>();
                        foreach (KnappStationSettings kss in Configuration.KnappSettings.KnappStationsSettings) 
                        {
                            if (kss.IdNode == newNode.IdNode && kss.EnableKnapp)
                                newNode.DumpingEnable = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        string error = $"{nodeDef.NodeDescription}: creation failed!";
                        if (ex.Message.ToLower().Contains("check configuration"))
                            error += " Check configuration.";

                        Log.Line(LogLevels.Error, "VisionSystemManager.Init", $"Node {nodeDef.Id} creation failed. Error: {error}");

                        OnTaskStatusUpdate(this, new TaskStatusEventArgs("VisionSysManagerNodeConnection" + nodeDef.Id.ToString(), ExactaEasyCore.TaskStatus.Failed, error));
                        nodeBlackList.Add(nodeDef);
                    }
                }
                foreach (NodeDefinition nodeDef in Configuration.NodesDefinition) 
                {
                    INode newNode = Nodes[nodeDef.Id];
                    try 
                    {
                        if (newNode != null) 
                            newNode.Connect();

                        if (nodeBlackList.Exists(nodeD => nodeD.Id == nodeDef.Id))
                            nodeBlackList.Remove(nodeDef);
                    
                        OnTaskStatusUpdate(this, 
                            new TaskStatusEventArgs("VisionSysManagerNodeConnection" + nodeDef.Id.ToString(),
                            ExactaEasyCore.TaskStatus.Completed, 
                            nodeDef.NodeDescription + ": Initialized!"));
                    }
                    catch (Exception ex) 
                    {
                        string error = $"{nodeDef.NodeDescription}: connection failed!";
                        if (ex.Message.ToLower().Contains("check configuration"))
                            error += " Check configuration.";
                        Log.Line(LogLevels.Error, "VisionSystemManager.Init", $"Node {nodeDef.Id} connection failed. Error: {error}");
                        OnTaskStatusUpdate(this, new TaskStatusEventArgs("VisionSysManagerNodeConnection" + nodeDef.Id.ToString(), ExactaEasyCore.TaskStatus.Failed, error));
                        nodeBlackList.Add(nodeDef);
                    }
                    if (newNode != null)
                        newNode.RescanRequested += newNode_RescanRequested; //PIER
                }
            }

            if (Configuration.StationDefinition != null) 
            {
                foreach (StationDefinition statDef in Configuration.StationDefinition)
                {
                    try 
                    {
                        statDef.MaxInspectionQueueLength = Configuration.MaxInspectionQueueLength;
                        //statDef.MaxInspectionImgToSave = Configuration.MaxInspectionImgToSave;
                        Station newStation = Station.CreateStation(statDef);
                        addStation((IStation)newStation);
                    }
                    catch 
                    {
                        Log.Line(LogLevels.Warning, "VisionSystemManager.Init", $"Station {statDef.Id} creation failed.");
                    }
                }
            }
            //Stopwatch timer = new Stopwatch();
            bool cameraScanRequest = true;
            foreach (CameraDefinition camDef in Configuration.CamerasDefinition) 
            {
                //timer.Restart();
                //const double timeoutSec = 5 * 60;
                //double timeoutMs = Math.Max(2F * 1000F, (timeoutSec * 1000F) - (UpTime.TotalSeconds * 1000F));       // timeout in millisecondi
                //bool ok = false;
                //while (!ok) {
                try 
                {
                    camDef.IP4Address = Configuration.NodesDefinition[camDef.Node].IP4Address;
                    Log.Line(LogLevels.Pass, "VisionSystemManager.Init", "Creating new camera. ID: " + camDef.Id.ToString());
                    Camera newCamera = Camera.CreateCamera(camDef, cameraScanRequest);

                    foreach (LightControllerDefinition ld in camDef.LightControllers) 
                    {
                        IStrobeController sc = StrobeControllers.Find((IStrobeController s) => { return s.Id == ld.StrobeId; });
                        if (sc != null)
                            newCamera.Lights.Add(new LightController(ld.Id, ld.Description, sc, ld.StrobeChannel));
                    }

                    addCamera(newCamera);

                    if (cameraBlackList.Exists(camD => camD.Id == camDef.Id))
                        cameraBlackList.Remove(camDef);
                   
                    //ok = true;
                    //OnCameraStatusChanged(this, new MessageEventArgs(new LogMessage(LogLevels.Pass, camDef.IP4Address.ToString() + ": Initialized!")));
                    //pier: da rivedere...serve per monitorare su Menarini solo i nodi
                    if (Configuration.NodesDefinition == null || Configuration.NodesDefinition.Count == 0)
                        OnTaskStatusUpdate(this, new TaskStatusEventArgs("VisionSysManagerCameraCreation" + camDef.Id.ToString(), ExactaEasyCore.TaskStatus.Completed, camDef.CameraDescription + ": Initialized!"));
                    /*                  // Se la camera incorpora il concetto di nodo deve essere aggiunta 
                                        // alla collection dei nodi
                                        if (newCamera is INode) {
                                            addNode((INode)newCamera);
                                        }
                                        // Se la camera incorpora il concetto di stazione deve essere aggiunta 
                                        // alla collection di stazioni
                                        if (newCamera is IStation) {
                                            addStation((IStation)newCamera);
                                        } */
                }
                catch (Exception ex) 
                {
                    Log.Line(LogLevels.Error, "VisionSystemManager.Init", $"Creation failed! Camera: {camDef.CameraDescription} Error: {ex.Message}");
                    
                    string error = camDef.CameraDescription.ToString() + ": initialization failed!";
                    if (ex.Message.ToLower().Contains("check configuration"))
                        error += " Check configuration.";
                    //pier: da rivedere...serve per monitorare su Menarini solo i nodi
                    if (Configuration.NodesDefinition == null || Configuration.NodesDefinition.Count == 0)
                        OnTaskStatusUpdate(this, new TaskStatusEventArgs("VisionSysManagerCameraCreation" + camDef.Id.ToString(), ExactaEasyCore.TaskStatus.Failed, error));
                    cameraBlackList.Add(camDef);
                }
                finally 
                {
                    cameraScanRequest = false;
                }
            }
            OnInitializationCompleted(this, new EventArgs());

            try 
            {
                StartLive();
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "VisionSystemManager.Init", $"StartLive failed! Error: {ex.Message}");
            }
            _nodesDateTime = DateTime.Now;
            //WaitForNodesConnection();
            _rescanAvailable.Set();
        }
        /// <summary>
        /// 
        /// </summary>
        private void NewNode_ReceivedGretelInfo(object sender, MessageEventArgs e)
        {
            // TODO
        }
        /// <summary>
        /// 
        /// </summary>
        private void newNode_ClientAuditMessage(object sender, MessageEventArgs e) {

            if (AppEngine.Current.MachineConfiguration.EnableAudit == true) {
                if (AppEngine.Current.TryAuditRecipe(e.Message) == false) {
                    Log.Line(LogLevels.Error, "frmMain.changeRecipe2HMI", "AUDIT sent failed");
                }
                //else {
                //    Log.Line(LogLevels.Pass, "frmMain.changeRecipe2HMI", "AUDIT sent successfully");
                //}
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void newNode_EnableNavigation(object sender, MessageEventArgs e) 
        {
            OnEnableNavigation(sender, e);
        }
        /// <summary>
        /// 
        /// </summary>
        private void d_CommunicationStateChanged(object sender, CommunicationStateChangedEventArgs e) 
        {
            if ((e.PreviousState == DeviceCommunicationStates.Connected || e.PreviousState == DeviceCommunicationStates.Connecting) &&
                e.CurrentState == DeviceCommunicationStates.Faulted) 
            {
                Device d = (Device)sender;
                if (!d.ConnectingInProgress)
                    d.StartConnect(true);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void newNode_RemoteDesktopDisconnected(object sender, EventArgs e) 
        {
            OnRemoteDesktopDisconnection(sender, e);
        }
        /// <summary>
        /// 
        /// </summary>
        void newNode_BootDone(object sender, EventArgs e) 
        {
            if (_settingParameters)
                return;
            OnNewNodeBootDone(sender, e);
        }
        /// <summary>
        /// 
        /// </summary>
        private void newNode_SavingBufferedImages(object sender, MessageEventArgs e) 
        {
            OnSavingBufferedImages(sender, e);
        }
        /// <summary>
        /// 
        /// </summary>
        private void newNode_NodeRecipeUpdate(object sender, NodeRecipeEventArgs e) 
        {
            if (AppEngine.Current.CurrentContext.ActiveRecipe == null) 
            {
                Log.Line(LogLevels.Error, "VisionSystemManager.newNode_NodeRecipeUpdate", "Active recipe is not valid. Cannot apply node recipe");
                INode node = Nodes[e.NodeRecipe.Id];
                if (node != null)
                    node.RecipeUpdatedError("exacta easy error: active recipe is not valid"); //max about 45 characters
                return;
            }

            if (AppEngine.Current.CurrentContext.ActiveRecipe.Nodes == null)
                AppEngine.Current.CurrentContext.ActiveRecipe.Nodes = new List<NodeRecipe>();

            NodeRecipe currNodeRecipe = AppEngine.Current.CurrentContext.ActiveRecipe.Nodes.Find(nr => nr.Id == e.NodeRecipe.Id);
            if (currNodeRecipe == null) 
            {
                currNodeRecipe = new NodeRecipe();
                currNodeRecipe.Id = e.NodeRecipe.Id;
                AppEngine.Current.CurrentContext.ActiveRecipe.Nodes.Add(currNodeRecipe);
            }
            currNodeRecipe.Description = e.NodeRecipe.Description;
            if (e.NodeRecipe.FrameGrabbers != null) 
            {
                currNodeRecipe.FrameGrabbers = new List<FrameGrabberRecipe>();
                foreach (FrameGrabberRecipe fgr in e.NodeRecipe.FrameGrabbers)
                    currNodeRecipe.FrameGrabbers.Add(fgr.Clone(AppEngine.Current.ParametersInfo, AppEngine.Current.CurrentContext.CultureCode));
            }

            if (e.NodeRecipe.Stations != null) 
            {
                currNodeRecipe.Stations = new List<StationRecipe>();
                foreach (StationRecipe sr in e.NodeRecipe.Stations)
                    currNodeRecipe.Stations.Add(sr.Clone(AppEngine.Current.ParametersInfo, AppEngine.Current.CurrentContext.CultureCode));
            }
            currNodeRecipe.RawRecipe = e.NodeRecipe.RawRecipe;
            OnChangedRecipe(this, e);
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnChangedRecipe(object sender, NodeRecipeEventArgs e) 
        {
            if (ChangeRecipe != null) 
                ChangeRecipe(sender, e);
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnNewNodeBootDone(object sender, EventArgs e)
        {
            if (NewNodeBootDone != null) 
                NewNodeBootDone(sender, e);
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnEnableNavigation(object sender, MessageEventArgs e) 
        {
            if (EnableNavigation != null) 
                EnableNavigation(sender, e);
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnSavingBufferedImages(object sender, MessageEventArgs e) 
        {
            if (SavingBufferedImages != null) 
                SavingBufferedImages(sender, e);
        }
        /// <summary>
        /// 
        /// </summary>
        private void OnNotifyGUI(object sender, MessageEventArgs e) {

            if (NotifyGUI != null) 
                NotifyGUI(sender, e);
        }
        /// <summary>
        /// 
        /// </summary>
        public void ResetGlobalRecipeUploadStatus()
        {
            // Get all the enabled nodes.
            var enabledNodes = Nodes.Where(n => IsNodeEnabled(n.IdNode));
            // If undefined/uploading, we need to update all the nodes.
            foreach (var node in enabledNodes)
                node.SetParametersStatus = SetParametersStatusEnum.Undefined;
        }
        /// <summary>
        /// Matteo - 27/06/2024.
        /// </summary>
        public void TryChangeSupervisorStatus(bool bypass)
        {
            // Get all the enabled nodes.
            var enabledNodes = Nodes.Where(n => IsNodeEnabled(n.IdNode));
            // Check there is at least one enabled node.
            if (enabledNodes.Count() == 0)
            {
                // There are no enabled nodes.
                AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.ReadyToRun);
                return;
            }
            // Get all the disconnected nodes.
            var disconnectedNodes = enabledNodes.Where(n => !n.Connected);

            if (disconnectedNodes.Count() > 0)
            {
                AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Error);
                return;
            }
            // *** If we are here, then all the nodes are currently connected ***
            // Check if some of the nodes are still in uploading state.
            if (enabledNodes.Any(n => n.SetParametersStatus == SetParametersStatusEnum.Uploading))
            {
                AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Busy);
                return;
            }
            // If the user is in recipe bypass mode, we don't care if some nodes are in an error state.
            // Check if all the nodes received a recipe correctly.
            if (enabledNodes.All(n => n.SetParametersStatus == SetParametersStatusEnum.UploadedOK) || bypass)
            {
                AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.ReadyToRun);
                return;
            }
            // Check if some nodes are in an error state.
            if (enabledNodes.Any(n => n.SetParametersStatus == SetParametersStatusEnum.UploadError ||
                                      n.SetParametersStatus == SetParametersStatusEnum.TimeoutError))
            {
                AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Error);
                return;
            }
            // The bottom case, lets set it to unknown just to understand this happened.
            AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Error);
        }
        /// <summary>
        /// Marks all the nodes that are not OK or KO as Timeouted.
        /// </summary>
        private void MarkTimeoutedNodes(List<NodeRecipe> nodes)
        {
            foreach (var node in nodes)
            {
                INode nn = Nodes.Find(n => n.IdNode == node.Id && n.SetParametersStatus != SetParametersStatusEnum.UploadedOK && n.SetParametersStatus != SetParametersStatusEnum.UploadError);
                if (nn is null)
                    continue;
                nn.SetParametersStatus = SetParametersStatusEnum.TimeoutError;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void SetParameters(Recipe recipe) 
        {
            Exception cachedEx = null;

            if (recipe.Nodes != null && recipe.Nodes.Count > 0 && Nodes.Count > 0) 
            {
                _settingParameters = true;
                WaitHandle[] recRecipeEvts = new WaitHandle[Math.Min(Nodes.Count, recipe.Nodes.Count)];
                int inn = 0;
                WaitForNodesConnection();
                foreach (NodeRecipe nodeRecipe in recipe.Nodes) 
                {
                    INode nn = Nodes.Find(n => n.IdNode == nodeRecipe.Id);
                    if (nn != null) 
                    {
                        try 
                        {
                            recRecipeEvts[inn] = nn.SetParametersDone;
                            inn++;
                            nn.SetParameters(recipe.RecipeName, nodeRecipe, AppEngine.Current.ParametersInfo, AppEngine.Current.CurrentContext.CultureCode);
                        }
                        catch (Exception ex) 
                        {
                            if (IsNodeEnabled(nn.IdNode))
                                cachedEx = ex;
                            
                            Log.Line(LogLevels.Warning, "frmMain.sendCameraParams", "Error while sending parameters to a disabled node: " + ex.Message);
                            nn.SetParametersDone.Set(); //nodi disabilitati settano l'evento a prescindere
                        }
                    }
                }
                bool res = WaitHandle.WaitAll(recRecipeEvts, AppEngine.Current.MachineConfiguration.RecipeDownloadTimeOut * 1000);
                _settingParameters = false;

                Rescan();   //se nel frattempo un nodo si è connesso lo rilevo

                if (!res) 
                {
                    // Matteo - 27/06/2024: sets all the nodes still waiting for an ACK from GRETEL to timeouted, so
                    // that all the incoming ACKs are dropped.
                    MarkTimeoutedNodes(recipe.Nodes);
                    //timeout
                    Log.Line(LogLevels.Warning, "VisionSystemManager.SetParameters", "Parameters set failed: timeout!");
                    throw new Exception("Parameters set failed: timeout!");
                }
                else 
                {
                    bool allOK = true;

                    foreach (NodeRecipe node in recipe.Nodes) 
                    {
                        INode nn = Nodes.Find(n => n.IdNode == node.Id);

                        if (!IsNodeEnabled(nn.IdNode))
                            continue;

                        if (nn != null) 
                        {
                            switch (nn.SetParametersStatus)
                            {
                                case SetParametersStatusEnum.TimeoutError:
                                case SetParametersStatusEnum.UploadError:
                                case SetParametersStatusEnum.Uploading:
                                case SetParametersStatusEnum.Undefined:
                                    allOK = false;
                                    Log.Line(LogLevels.Error, "VisionSystemManager.SetParameters", "Parameters set failed");
                                    throw new Exception("Parameters set to node " + node.Id + " failed");
                            }
                        }
                    }
                    if (allOK)
                        Log.Line(LogLevels.Pass, "VisionSystemManager.SetParameters", "Parameters set completed successfully");
                    else
                        Log.Line(LogLevels.Error, "VisionSystemManager.SetParameters", "Parameters set failed");
                }
            }

            if (recipe.Cams != null && recipe.Cams.Count > 0) {
                foreach (Cam cam in recipe.Cams) {
                    try {
                        //se camera in errore mettiamo SPV in errore
                        if (CameraReady(cam.Id) != CameraNewStatus.Ready) {
                            if (!IsStationEnabled(cam.Id) || !cam.Enabled) {
                                Log.Line(LogLevels.Debug, "frmMain.sendCameraParams", "Camera ID {0} not enabled by HMI or Recipe.", cam.Id);
                                Log.Line(LogLevels.Warning, "frmMain.sendCameraParams", "Camera ID {0} not Ready but not enabled by HMI or Recipe (maybe you can ignore this warning?).", cam.Id);
                                continue;
                            }
                            throw new CameraException("Camera not ready");
                        }
                        Log.Line(LogLevels.Warning, "frmMain.sendCameraParams", "Sending parameters to Camera ID: {0}. Please wait...", cam.Id);
                        CameraSetting cs = AppEngine.Current.MachineConfiguration.CameraSettings.Find(c => c.Id == cam.Id);
                        if (cs == null) throw new CameraException("No settings for camera " + cam.Id.ToString());
                        Camera vCamera = (Camera)Cameras[cam.Id];
                        if (vCamera.Lights.Count > 0)
                            vCamera.ApplyParameters(ParameterTypeEnum.All, cam);
                        else
                            vCamera.ApplyParameters(cs, ParameterTypeEnum.All, cam);
                        //headerStrip.ErrorText = "";
                    }
                    catch (Exception ex) {
                        if (IsStationEnabled(cam.Id))
                            cachedEx = ex;
                        else
                            Log.Line(LogLevels.Warning, "frmMain.sendCameraParams", "Error while sending parameters to a disabled camera: " + ex.Message);
                    }
                    Log.Line(LogLevels.Pass, "frmMain.sendCameraParams", "Sent parameters to Camera ID: {0}.", cam.Id);
                }

                if (cachedEx != null)
                    throw cachedEx;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void StartRescan() 
        {
            if (AppEngine.Current.CurrentContext.SupervisorMode != SupervisorModeEnum.Busy)
            {
                if (AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Busy))
                {
                    Task initializeVisionTask = new Task(new Action(() => Rescan()));
                    initializeVisionTask.Start();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsStationEnabled(int idCamera) 
        {
            if (idCamera >= Cameras.Count) return false;
            int nodeId = Cameras[idCamera].NodeId;
            int stationId = Cameras[idCamera].StationId;
            if (Nodes[nodeId] == null || Nodes[nodeId].Stations[stationId] == null) return false;
            return Nodes[nodeId].Stations[stationId].Enabled;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsNodeEnabled(int idNode) 
        {
            INode nn = Nodes[idNode];
            if (nn == null) return false;
            bool enable = false;
            foreach (IStation s in Nodes[idNode].Stations)
                enable |= s.Enabled;
            return enable;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Rescan() 
        {
            // Check if we passed the Init() and rescanning function is available.
            _rescanAvailable.WaitOne();

            if (Monitor.TryEnter(_rescanLockObject))
            {
                OnStartRescan?.Invoke(this, new EventArgs());
                // We got the lock: no other Rescan is running.
                Log.Line(LogLevels.Pass, "VisionSystemManager.Rescan", "Starting hardware rescan...");
                AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Busy);

                try
                {
                    //pier: da rivedere...serve per monitorare su Menarini solo i nodi
                    if (Configuration.NodesDefinition == null)
                    {
                        foreach (CameraDefinition camDef in cameraBlackList)
                        {
                            OnTaskStatusUpdate(
                                this,
                                new TaskStatusEventArgs(
                                    "VisionSysManagerCameraCreation" + camDef.Id.ToString(),
                                    ExactaEasyCore.TaskStatus.Running, camDef.CameraDescription + ": Creating..."));
                        }
                    }
                    else
                    {
                        foreach (NodeDefinition nodeDef in nodeBlackList)
                        {
                            OnTaskStatusUpdate(
                                this, 
                                new TaskStatusEventArgs("VisionSysManagerNodeConnection" + nodeDef.Id.ToString(), 
                                ExactaEasyCore.TaskStatus.Running, 
                                nodeDef.NodeDescription + ": Creating..."));
                        }
                    }

                    //Stopwatch timer = new Stopwatch();
                    NodeDefinitionCollection nodeBlackListToRemove = new NodeDefinitionCollection();
                    NodeDefinitionCollection nodeBlackListToAdd = new NodeDefinitionCollection();
                    CameraDefinitionCollection cameraBlackListToRemove = new CameraDefinitionCollection();
                    CameraDefinitionCollection cameraBlackListToAdd = new CameraDefinitionCollection();
                    bool cameraScanRequest = true;
                    if (Configuration.NodesDefinition != null)
                    {
                        foreach (NodeDefinition nodeDef in nodeBlackList)
                        {
                            try
                            {
                                Log.Line(LogLevels.Pass, "VisionSystemManager.Rescan", "Creating new node. ID: " + nodeDef.Id);
                                //Node newNode = Node.CreateNode(nodeDef);  //PIER
                                //if (Nodes[nodeDef.Id] == null)    //PIER
                                //    addNode((INode)newNode);  //PIER
                                //newNode.Connect();
                                if (nodeBlackList.Exists(nodeD => nodeD.Id == nodeDef.Id))
                                {
                                    nodeBlackListToRemove.Add(nodeDef);
                                }
                                OnTaskStatusUpdate(this, new TaskStatusEventArgs("VisionSysManagerNodeConnection" + nodeDef.Id.ToString(), ExactaEasyCore.TaskStatus.Completed, nodeDef.NodeDescription + ": Initialized!"));
                            }
                            catch (Exception ex)
                            {
                                string error = nodeDef.NodeDescription + ": Connection failed!";
                                if (ex.Message.ToLower().Contains("check configuration"))
                                    error += " Check configuration.";
                                Log.Line(LogLevels.Error, "VisionSystemManager.Init", "Node {0} creation failed. Error: " + error, nodeDef.Id);
                                OnTaskStatusUpdate(this, new TaskStatusEventArgs("VisionSysManagerNodeConnection" + nodeDef.Id.ToString(), ExactaEasyCore.TaskStatus.Failed, error));
                                if (!nodeBlackList.Exists(nodeD => nodeD.Id == nodeDef.Id))
                                {
                                    nodeBlackListToAdd.Add(nodeDef);
                                }
                            }
                        }
                        foreach (NodeDefinition nodeDef in nodeBlackListToRemove)
                            nodeBlackList.Remove(nodeDef);
                  
                        foreach (NodeDefinition nodeDef in nodeBlackListToAdd)
                            nodeBlackList.Add(nodeDef);
                    }


                    foreach (CameraDefinition camDef in cameraBlackList)
                    {
                        try
                        {
                            Log.Line(LogLevels.Pass, "VisionSystemManager.Rescan", "Creating new camera. ID: " + camDef.Id);
                            Camera newCamera = Camera.CreateCamera(camDef, cameraScanRequest);
                            if (Cameras[camDef.Id] == null)
                                addCamera(newCamera);
                            if (cameraBlackList.Exists(camD => camD.Id == camDef.Id))
                            {
                                cameraBlackListToRemove.Add(camDef);
                            }
                            //OnCameraStatusChanged(this, new MessageEventArgs(new LogMessage(LogLevels.Pass, camDef.IP4Address.ToString() + ": Initialized!")));
                            OnTaskStatusUpdate(this, new TaskStatusEventArgs("VisionSysManagerCameraCreation" + camDef.Id.ToString(), ExactaEasyCore.TaskStatus.Completed, camDef.CameraDescription + ": Initialized!"));
                            /*                  // Se la camera incorpora il concetto di nodo deve essere aggiunta 
                                                // alla collection dei nodi
                                                if (newCamera is INode) {
                                                    addNode((INode)newCamera);
                                                }
                                                // Se la camera incorpora il concetto di stazione deve essere aggiunta 
                                                // alla collection di stazioni
                                                if (newCamera is IStation) {
                                                    addStation((IStation)newCamera);
                                                } */
                        }
                        catch (Exception ex)
                        {
                            Log.Line(LogLevels.Error, "VisionSystemManager.Rescan", "Creation failed! IP: " + camDef.CameraDescription + " Error: " + ex.Message);
                            string error = camDef.CameraDescription.ToString() + ": Initialization failed!";
                            if (ex.Message.ToLower().Contains("check configuration"))
                                error += " Check configuration.";
                            //OnCameraStatusChanged(this, new MessageEventArgs(new LogMessage(LogLevels.Error, error)));
                            OnTaskStatusUpdate(this, new TaskStatusEventArgs("VisionSysManagerCameraCreation" + camDef.Id.ToString(), ExactaEasyCore.TaskStatus.Failed, error));
                            
                            if (!cameraBlackList.Exists(camD => camD.Id == camDef.Id))
                                cameraBlackListToAdd.Add(camDef);
                        }
                        finally
                        {
                            cameraScanRequest = false;
                        }
                    }
                    foreach (CameraDefinition camDef in cameraBlackListToRemove)
                        cameraBlackList.Remove(camDef);

                    foreach (CameraDefinition camDef in cameraBlackListToAdd)
                        cameraBlackList.Add(camDef);
                   
                    // ORA RICONTROLLA TUTTO
                    if (Configuration.NodesDefinition != null)
                    {
                        foreach (INode node in Nodes)
                        {
                            if (node.Connected)
                            {
                                OnTaskStatusUpdate(
                                    this, 
                                    new TaskStatusEventArgs("VisionSysManagerNodeConnection" + node.IdNode.ToString(), 
                                    ExactaEasyCore.TaskStatus.Completed, node.Description + ": Connected!"));
                            }
                            else
                                OnTaskStatusUpdate(
                                    this, 
                                    new TaskStatusEventArgs("VisionSysManagerNodeConnection" + node.IdNode.ToString(), 
                                    ExactaEasyCore.TaskStatus.Failed, node.Description + ": Connection failed!"));
                        }
                    }
                    foreach (ICamera cam in Cameras)
                    {
                        try
                        {
                            CameraNewStatus camStatus = cam.GetCameraStatus();
                            //CameraProcessingMode camProcMode = cam.GetCameraProcessingMode();
                            if (camStatus == CameraNewStatus.Unavailable)
                                throw new CameraException("Camera not available");
                            OnTaskStatusUpdate(this, new TaskStatusEventArgs("VisionSysManagerCameraCreation" + cam.IdCamera.ToString(), ExactaEasyCore.TaskStatus.Completed, cam.CameraDescription + ": Initialized!"));
                        }
                        catch
                        {
                            OnTaskStatusUpdate(this, new TaskStatusEventArgs("VisionSysManagerCameraCreation" + cam.IdCamera.ToString(), ExactaEasyCore.TaskStatus.Failed, cam.CameraDescription + ": Connection lost!"));
                            if (!cameraBlackList.Exists(camD => camD.Id == cam.IdCamera))
                            {
                                CameraDefinition newCamDef = Configuration.CamerasDefinition.Find(camD => camD.Id == cam.IdCamera);
                                cameraBlackList.Add(newCamDef);
                            }
                        }
                    }

                    try
                    {
                        StartLive();
                    }
                    catch(Exception ex)
                    {
                        Log.Line(LogLevels.Error, "VisionSystemManager.Rescan", $"StartLive failed! Error: {ex.Message}");
                    }
                }
                finally
                {
                    Log.Line(LogLevels.Pass, "VisionSystemManager.Rescan", "Hardware rescan completed.");
                    Monitor.Exit(_rescanLockObject);
                    // Raises an event to the client class.
                    OnRescanCompleted(this, EventArgs.Empty);
                }
            }
        }
        //void OnCameraStatusChanged(object sender, MessageEventArgs e) {

        //    if (CameraStatusChanged != null)
        //        CameraStatusChanged(sender, e);
        //}

        public virtual void OnRescanCompleted(object sender, EventArgs e) {

            if (RescanCompleted != null) 
                RescanCompleted(sender, e);
        }

        //public void WaitAtStartup(int waitFromStartupSec) {

        //    double timeoutMs = Math.Max(1F * 1000F, (waitFromStartupSec * 1000F) - (UpTime.TotalSeconds * 1000F));       // timeout in millisecondi
        //    Log.Line(LogLevels.Warning, "VisionSystemManager.WaitAtStartup", "Wait {0} seconds for software startup...", timeoutMs/1000);
        //    Thread.Sleep((int)timeoutMs);
        //}

        public CameraNewStatus CameraReady(int IdCamera) 
        {
            try 
            {
                if (!Cameras.Exists(_camera => _camera.IdCamera == IdCamera))
                    return CameraNewStatus.Unavailable;
                //CameraNewStatus camStatus = Cameras[IdCamera].GetCameraStatus();
                //if (camStatus != CameraNewStatus.Ready) {
                //    return false;
                //}
                //return true;
                return Cameras[IdCamera].GetCameraStatus();
            }
            catch 
            {
                //return false;
                return CameraNewStatus.Unavailable;
            }
        }

        void OnTaskStatusUpdate(object sender, TaskStatusEventArgs e) {

            if (TaskStatusUpdate != null)
                TaskStatusUpdate(sender, e);
        }

        void OnInitializationCompleted(object sender, EventArgs e) {

            if (InitializationCompleted != null)
                InitializationCompleted(sender, e);
        }

        void OnRemoteDesktopDisconnection(object sender, EventArgs e) {
            if (RemoteDesktopDisconnection != null)
                RemoteDesktopDisconnection(sender, e);
        }

        void addNode(INode newNode) {

            if (Nodes[newNode] == null) {
                Nodes.Add(newNode);
            }
        }

        void newNode_RescanRequested(object sender, EventArgs e) 
        {
            if (!_settingParameters)
                Rescan();

            if (!(sender as Node).Connected)
                _nodesDateTime = DateTime.Now;
        }

        void newNode_DumpingChanged(object sender, EventArgs e) {

            lock (_batchLockObject) {
                bool savingBatch = false;
                bool savingBatchAllNodes = true;
                foreach (INode n in Nodes) {
                    if (n.Dumping) savingBatch = true;
                    if (!n.Dumping && n.DumpingEnable) savingBatchAllNodes = false;
                }
                SavingBatch = savingBatch;
                SavingBatchAllNodes = savingBatchAllNodes;
                if (SavingBatchAllNodes) SavingBatchAllNodesEvt.Set();
                else SavingBatchAllNodesEvt.Reset();
                if (SavingBatch) NotSavingBatchAllNodesEvt.Reset();
                else NotSavingBatchAllNodesEvt.Set();
                OnSavingBatchChanged(this, EventArgs.Empty);
            }
        }

        private void OnSavingBatchChanged(object sender, EventArgs e) {

            if (SavingBatchChanged != null) SavingBatchChanged(sender, e);
        }

        void addStation(IStation newStation) {

            IStation st = Stations[newStation];
            if (st == null) {
                Stations.Add(newStation);
                st = newStation;
            }
            if (newStation is INode && Nodes[(INode)st] == null)
                addNode((INode)newStation);
            //else
            //    throw new ArgumentOutOfRangeException("Id node not valid!");
            if (newStation is INode)
                Nodes[(INode)newStation].Stations.Add(newStation);
            else
                Nodes[newStation.NodeId].Stations.Add(newStation);
        }

        void addCamera(ICamera newCamera) {

            ICamera cm = Cameras[newCamera.IdCamera];
            if (cm != null)
                throw new ArgumentOutOfRangeException("Duplicate camera! Id : " + newCamera.IdCamera);
            else
                Cameras.Add(newCamera);
            if (newCamera is IStation && Stations[(IStation)newCamera] == null)
                addStation((IStation)newCamera);
            if (newCamera is IStation)
                Stations[(IStation)newCamera].Cameras.Add(newCamera);
            else {
                INode node = Nodes[newCamera.NodeId];
                if (node != null) {
                    IStation station = node.Stations[newCamera.StationId];
                    if (station != null) {
                        station.Cameras.Add(newCamera);
                    }
                }
            }
        }

        public void WaitForNodesConnection() {

            if (Configuration.NodeBootTimeoutSec <= 0)
                return;
            //int status = 0;
            foreach (NodeDefinition nodeDef in Configuration.NodesDefinition) {
                if (IsNodeEnabled(nodeDef.Id)) {
                    int timerSec = (DateTime.Now - _nodesDateTime).Seconds;
                    while (timerSec < Configuration.NodeBootTimeoutSec) {
                        if (Nodes[nodeDef.Id].Connected == true) {
                            Log.Line(LogLevels.Pass, "VisionSystemManager.WaitForNodesConnection", nodeDef.IP4Address + ": node connected.");
                            break;
                        }
                        Log.Line(LogLevels.Debug, "VisionSystemManager.WaitForNodesConnection", nodeDef.IP4Address + ": waiting for node connection ({0}).", timerSec);
                        Thread.Sleep(1000);
                        string errorText = string.Format(UIStrings.GetString("WaitingNodeConnection"), nodeDef.IP4Address, (Configuration.NodeBootTimeoutSec - timerSec).ToString());
                        OnNotifyGUI(this, new MessageEventArgs(errorText));
                        timerSec = (int)(DateTime.Now - _nodesDateTime).TotalSeconds;
                    }
                }
            }
            OnNotifyGUI(this, new MessageEventArgs(""));
        }

        public void RedrawDisplay(string displayName) {

            Display d = Displays[displayName];
            if (d != null) d.DoRender();
            else Log.Line(LogLevels.Debug, "VisionSystemManager.RedrawDisplay", "Trying to redraw a null display (" + displayName + ")...");
        }

        public void RedrawHeader(string displayName) {

            Display d = Displays[displayName];
            if (d != null) d.DrawHeader();
            else Log.Line(LogLevels.Debug, "VisionSystemManager.RedrawHeader", "Trying to redraw a null display (" + displayName + ")...");
        }

        public void StartLive() {

            foreach (INode n in Nodes) {
                startLive(n);
            }
        }


        void startLive(INode node) {

            if (node == null) return;

            node.GetParameters();
            if (!node.Connected || node.Stations == null || node.Stations.Count == 0) return;
            foreach (IStation s in node.Stations) {
                startLive(s);
            }
        }

        void startLive(IStation station) {

            if (station == null) return;
            if (!station.Connected)
                station.Connect();
            //if (camera.Connected)   //pier: aggiunto per M12
            station.Grab();
        }

        public void SaveBufferedImages(string path, SaveConditions saveCondition, int toSave) {

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            foreach (INode n in Nodes) {
                if (n.ProviderName == "GretelNodeBase")
                    n.SaveBufferedImages(path, saveCondition, toSave);
            }
        }

        public void ResetImagesBuffer(string path, bool freeFolders) {

            foreach (INode n in Nodes)
                n.ResetImagesBuffer(path, freeFolders);
        }

        public void StartStopBatch(DumpImagesUserSettings userSettings, bool start) {

            if (Nodes == null || Nodes.Count == 0) 
                return;

            if (start) {
                foreach (INode n in Nodes)
                    n.StartNewBatch();
            }

            if (SavingBatch && start) return;
            //if (!SavingBatch && !start) return;

            lock (_batchLockObject) {
                SavingBatchAllNodes = false;
                SavingBatchAllNodesEvt.Reset();
                NotSavingBatchAllNodesEvt.Set();
                foreach (INode n in Nodes) {
                    if (start) {
                        List<StationDumpSettings> sdsList = new List<StationDumpSettings>();
                        sdsList = n.Stations.Select(s => userSettings.StationsDumpSettings.Find(ss => ss.Node == n.IdNode && ss.Id == s.IdStation)).ToList();


                        //n.DumpingEnable = ((sdsList == null) || (sdsList.Count == 0)) ? false : true;
                        Log.Line(LogLevels.Pass, "VisionSystemManager.StartStopBatch", n.Address + ": DUMPING ENABLE = " + n.DumpingEnable.ToString());
                        if (n.DumpingEnable)
                            n.StartImagesDump(sdsList);
                    }
                    else {
                        //if (n.DumpingEnable)
                        n.StopImagesDump();
                    }
                }
            }
        }

        public void StartStopBatch2(DumpImagesUserSettings2 userSettings, bool start)
        {
            if (Nodes == null || Nodes.Count == 0)
                return;

            if (start)
            {
                foreach (INode n in Nodes)
                    n.StartNewBatch();
            }

            if (SavingBatch && start)
                return;

            lock (_batchLockObject)
            {
                SavingBatchAllNodes = false;
                SavingBatchAllNodesEvt.Reset();
                NotSavingBatchAllNodesEvt.Set();
                foreach (INode n in Nodes)
                {
                    if (start)
                    {
                        List<StationDumpSettings2> sdsList = userSettings.GetSettingsByNode(n.IdNode);
                        Log.Line(LogLevels.Pass, "VisionSystemManager.StartStopBatch2", n.Address + ": DUMPING ENABLE = " + n.DumpingEnable.ToString());
                        if (n.DumpingEnable)
                            n.StartImagesDump2(sdsList);
                    }
                    else
                    {
                        n.StopImagesDump();
                    }
                }
            }
        }

        //public void TryChangeSupervisorStatus(bool bypass) 
        //{
        //    // Get all the enabled nodes.
        //    var enabledNodes = Nodes.Where(n => IsNodeEnabled(n.IdNode));
        //    // Check there is at least one enabled node.
        //    if (enabledNodes.Count() == 0)
        //    {
        //        // There are no enabled nodes.
        //        AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.ReadyToRun);
        //        return;
        //    }

        //    // Get all the disconnected nodes.
        //    var disconnectedNodes = enabledNodes.Where(n => !n.Connected);

        //    if (disconnectedNodes.Count() > 0)
        //    {
        //        AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Error);
        //        return;
        //    }

        //    // *** If we are here, then all the nodes are currently connected ***

        //    // Check if some of the nodes are still in uploading state.
        //    if (enabledNodes.Any(n => n.SetParametersStatus == SetParametersStatusEnum.Uploading))
        //    {
        //        AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Busy);
        //        return;
        //    }

        //    // If the user is in recipe bypass mode, we don't care if some nodes are in an error state.
        //    // Check if all the nodes received a recipe correctly.
        //    if (enabledNodes.All(n => n.SetParametersStatus == SetParametersStatusEnum.UploadedOK) || bypass)
        //    {
        //        AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.ReadyToRun);
        //        return;
        //    }

        //    // Check if some nodes are in an error state.
        //    if (enabledNodes.Any(n => n.SetParametersStatus == SetParametersStatusEnum.UploadError || 
        //                              n.SetParametersStatus == SetParametersStatusEnum.TimeoutError ))
        //    {
        //        AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Error);
        //        return;
        //    }

        //    // The bottom case, lets set it to unknown just to understand this happened.
        //    AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Unknown);

        //    //bool ok = true;

        //    //foreach (INode node in Nodes)
        //    //{
        //    //    if (IsNodeEnabled(node.IdNode) &&
        //    //            (!node.Connected ||
        //    //            (!node.SetParametersCompletedSuccessfully && !bypass)))
        //    //        ok = false;
        //    //}

        //    //if (ok) AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.ReadyToRun);
        //    //else AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Error);
        //}

        public void Dispose() {

            foreach (Camera camera in Cameras) {
                try {
                    camera.Dispose();
                }
                catch (CameraException ex) {
                    Log.Line(LogLevels.Error, "frmMain.Destroy", "Error: " + ex.Message);
                }
            }
            foreach (IStation station in Stations) {
                try {
                    station.Dispose();
                }
                catch (CameraException ex) {
                    Log.Line(LogLevels.Error, "frmMain.Destroy", "Error: " + ex.Message);
                }
            }
            foreach (INode node in Nodes) {
                try {
                    node.Dispose();
                }
                catch (CameraException ex) {
                    Log.Line(LogLevels.Error, "frmMain.Destroy", "Error: " + ex.Message);
                }
            }
            foreach (IStrobeController str in StrobeControllers) {
                str.Dispose();
            }
        }

        public void SetWorkingMode(MachineModeEnum machineMode) {

            foreach (INode n in Nodes)
                n.MachineMode = machineMode;
        }

        #region DATABASE
        static DBConnection _DBConnection;
        private bool isResultsDBOpen;
        public bool IsResultsDBOpen {
            get {
                return isResultsDBOpen;
            }
            protected set {
                isResultsDBOpen = value;
            }
        }
        private int databaseCounter;
        public int DatabaseCounter {
            get {
                return databaseCounter;
            }
            protected set {
                databaseCounter = value;
            }
        }

        public void OpenDatabase(int DBcounter) {

            if (_DBConnection == null) {
                Log.Line(LogLevels.Error, "VisionSystemManager.OpenDatabase", "Database connection not initialized");
                return;
            }
            if (AppEngine.Current.CurrentContext.ActiveRecipe == null) {
                CloseDatabase();
                Log.Line(LogLevels.Warning, "VisionSystemManager.OpenDatabase", "Load recipe needed");
                return;
            }
            if (IsResultsDBOpen) {
                Log.Line(LogLevels.Warning, "VisionSystemManager.OpenDatabase", "Database already started");
                return;
            }
            AppEngine.Current.TrySaveRecipe();
            DatabaseCounter = DBcounter;
            CloseDatabase();
            foreach (MeasuresContainer mc in DBMeasures) {
                if (mc is MeasuresDB) {
                    MeasuresDB mdb = mc as MeasuresDB;
                    mdb.DBfilled -= measDb_DBfilled;
                    mdb.Dispose();
                }
            }
            DBMeasures.Clear();
            foreach (INode n in Nodes) {
                foreach (IStation s in n.Stations) {
                    if (s.HasMeasures && s.Enabled) {
                        //if (DBMeasures["Node_" + n.IdNode + "_Stat_" + s.IdStation] != null) {
                        //    (DBMeasures["Node_" + n.IdNode + "_Stat_" + s.IdStation] as MeasuresDB).DBfilled -= measDb_DBfilled;
                        //    DBMeasures.Remove(DBMeasures["Node_" + n.IdNode + "_Stat_" + s.IdStation]);
                        //    Log.Line(LogLevels.Pass, "VisionSystemManager.OpenDatabase", "Removed DB Measures: " + "Node_" + n.IdNode + "_Stat_" + s.IdStation);
                        //}
                        MeasuresDB measDb = new MeasuresDB("Node_" + n.IdNode + "_Stat_" + s.IdStation, s, _DBConnection);
                        measDb.DBfilled += measDb_DBfilled;
                        DBMeasures.Add(measDb);
                        //Log.Line(LogLevels.Pass, "VisionSystemManager.OpenDatabase", "Added DB Measures: " + "Node_" + n.IdNode + "_Stat_" + s.IdStation);
                    }
                }
            }
            foreach (MeasuresDB mc in DBMeasures) {
                mc.CurrRecipe = AppEngine.Current.CurrentContext.ActiveRecipe;
            }
            _dataFillTable = new int[MAX_NODES_COUNT, MAX_STATIONS_COUNT];
            IsResultsDBOpen = _DBConnection.OpenConnection();
            if (IsResultsDBOpen) {
                _DBConnection.DeleteTable();
                _DBConnection.Ready = true;
                Log.Line(LogLevels.Pass, "VisionSystemManager.OpenDatabase", "Detailed report started successfully");
            }
        }

        int[,] _dataFillTable;
        void measDb_DBfilled(object sender, EventArgs e) {

            if (_DBConnection == null) {
                Log.Line(LogLevels.Error, "VisionSystemManager.measDb_DBfilled", "Database connection not initialized");
                return;
            }
            lock (_DBConnection.SqlConnection) {
                IStation currStation = (sender as IStation);
                _dataFillTable[currStation.NodeId, currStation.IdStation]++;
                bool dataReceivedFromAllStations = true;
                foreach (INode n in Nodes) {
                    foreach (IStation s in n.Stations) {
                        if (s.HasMeasures && s.Enabled && _dataFillTable[s.NodeId, s.IdStation] < _dataFillTable[currStation.NodeId, currStation.IdStation]) {
                            dataReceivedFromAllStations = false;
                        }
                    }
                }
                //if (dataReceivedFromAllStations)
                //    Log.Line(LogLevels.Pass, "VisionSystemManager.measDb_DBfilled", "Nodo: " + currStation.NodeId + " Stazione:" + currStation.IdStation + ": SI");
                //else
                //    Log.Line(LogLevels.Pass, "VisionSystemManager.measDb_DBfilled", "Nodo: " + currStation.NodeId + " Stazione:" + currStation.IdStation + ": NO");
                if (dataReceivedFromAllStations) {
                    DatabaseCounter--;
                    Log.Line(LogLevels.Pass, "VisionSystemManager.measDb_DBfilled", "DatabaseCounter = " + DatabaseCounter.ToString());
                    foreach (MeasuresDB measDB in DBMeasures) {
                        measDB.DatabaseVialId++;
                        //Log.Line(LogLevels.Pass, "VisionSystemManager.measDb_DBfilled", measDB.Name + ": DatabaseVialId = " + measDB.DatabaseVialId.ToString());
                    }
                }
                if (DatabaseCounter == 0) {
                    _DBConnection.Ready = false;
                    CloseDatabase();
                }
            }
        }

        public void CloseDatabase() {

            if (_DBConnection == null) {
                Log.Line(LogLevels.Error, "VisionSystemManager.CloseDatabase", "Database connection not initialized");
                return;
            }
            if (_DBConnection.CloseConnection()) {
                Log.Line(LogLevels.Pass, "VisionSystemManager.CloseDatabase", "Detailed report completed");
                IsResultsDBOpen = false;
            }
        }
        #endregion
    }
}
