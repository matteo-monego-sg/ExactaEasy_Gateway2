using ExactaEasyCore;
using ExactaEasyEng;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace DisplayManager
{
    /// <summary>
    /// Status of the current recipe.
    /// </summary>
    public enum SetParametersStatusEnum : byte
    {
        Undefined = 0,
        Uploading,
        UploadError,
        TimeoutError,
        UploadedOK
    }

    /// <summary>
    /// 
    /// </summary>
    public interface INode 
    {
        event EventHandler<NodeRecipeEventArgs> NodeRecipeUpdate;
        event EventHandler RescanRequested;
        event EventHandler DumpingChanged;
        event EventHandler RemoteDesktopDisconnected;
        event EventHandler BootDone;
        ManualResetEvent SetParametersDone { get; }

        int IdNode { get; set; }
        bool Enabled { get; set; }
        string Address { get; set; }
        string Description { get; set; }
        string ProviderName { get; set; }
        int Port { get; set; }
        bool Connected { get; }
        string Version { get; }
        //bool RemoteDesktopConnected { get; }
        //bool SetParametersCompletedSuccessfully { get; }
        SetParametersStatusEnum SetParametersStatus { get; set; }
        bool Dumping { get; }
        bool DumpingEnable { get; set;  }
        string ServerIP4Address { get; set; }
        MachineModeEnum MachineMode { get; set;  }
        StationCollection Stations { get; }
        bool Lock();    // interblocco per la macchina -> non partire!
        bool Unlock();  // interblocco per la macchina -> puoi partire!

        void Connect();
        void Disconnect();
        void RemoteDesktopConnect(bool showToolbar);
        void RemoteDesktopDisconnect();
        void Dispose();
        void SetParameters(string recipeName, NodeRecipe dataSource, ParameterInfoCollection pic, string cultureCode);
        NodeRecipe GetParameters();
        NodeRecipe UploadParameters();
        void SendUserLevelClass(UserLevelEnum userLevel);
        void SendInspectionViewId(int inspectionId);

        void StartImagesDump(List<StationDumpSettings> statDumpSettingsCollection);
        void StartImagesDump2(List<StationDumpSettings2> statDumpSettingsCollection);
        void StopImagesDump();
        void StopImagesDump2();

        void StartNewBatch();
        void SaveBufferedImages(string path, SaveConditions saveCondition, int toSave);
        void ResetImagesBuffer(string path, bool freeFolders);
        void RefreshExportedParams();

        void Run(int inspectionId);
        void Live(int inspectionId, bool enable);

        void RecipeUpdatedOK();
        void RecipeUpdatedError(string message);
    }

    public class CollectorInfoEventArgs : EventArgs {

        public string Ip;
        public int Id;
        public List<CollectorInfo> CollectorInfoCollection;

        public CollectorInfoEventArgs(string ip, int id, List<CollectorInfo> collectorInfoCollection) {

            Ip = ip;
            Id = id;
            CollectorInfoCollection = collectorInfoCollection;
        }
    }

    public abstract class Node : INode 
    {

        public event EventHandler<NodeRecipeEventArgs> NodeRecipeUpdate;
        public event EventHandler RescanRequested;
        public event EventHandler DumpingChanged;
        public event EventHandler RemoteDesktopDisconnected;
        public event EventHandler BootDone;
        public event EventHandler<MessageEventArgs> ReceivedGretelInfo;
        public event EventHandler<MessageEventArgs> SavingBufferedImages;
        public event EventHandler<MessageEventArgs> EnableNavigation;
        public event EventHandler<CollectorInfoEventArgs> ClientCollectorInfo;
        public event EventHandler<MessageEventArgs> ClientAuditMessage;

        public ManualResetEvent SetParametersDone {
            get {
                return _setParametersDone;

            }
            protected set {
                _setParametersDone = value;
            }

        }
        public ManualResetEvent _setParametersDone = new ManualResetEvent(false);

        public int IdNode { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string ProviderName { get; set; }
        public int Port { get; set; }
        public int RemoteDesktopType { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int RDPort { get; set; }
        public bool Connected { get; protected set; }
        public string Version { get; protected set; }
        //public bool RemoteDesktopConnected { get; private set; }
        //public bool SetParametersCompletedSuccessfully { get; protected set; }
        public SetParametersStatusEnum SetParametersStatus { get; set; }
        public bool Dumping { get; protected set; }
        public bool DumpingEnable { get; set; }
        public string ServerIP4Address { get; set; }
        MachineModeEnum machineMode;
        public MachineModeEnum MachineMode {
            get {
                return machineMode;
            }
            set {
                machineMode = value;
                foreach (IStation s in Stations)
                    s.MachineMode = value;
            }
        }

        public StationCollection Stations { get; private set; }
        //public event EventHandler<MeasuresAvailableEventArgs> MeasuresAvailable;
        IRemoteDesktopForm remDesktopForm;

        public bool Enabled { get; set; }

        public Node(NodeDefinition nodeDefinition) 
        {
            SetParametersStatus = SetParametersStatusEnum.Undefined;
            IdNode = nodeDefinition.Id;
            ServerIP4Address = nodeDefinition.ServerIP4Address;
            Address = nodeDefinition.IP4Address;
            Description = nodeDefinition.NodeDescription;
            ProviderName = nodeDefinition.NodeProviderName;
            Port = nodeDefinition.Port;
            RemoteDesktopType = nodeDefinition.RemoteDesktopType;
            User = nodeDefinition.User;
            Password = nodeDefinition.Key;
            RDPort = nodeDefinition.RDPort;
            Stations = new StationCollection();
            Version = "?";
            //Initialized = false;
            //Connected = false;
        }

        public static Node CreateNode(NodeDefinition nodeDefinition) {

            NodeProvider np = NodeProviderCollection.GetProvider(nodeDefinition.NodeProviderName);
            Node node = null;

            if (np != null) {
                string[] typeData = np.Type.Split(new char[] { ',' });
                if (typeData.Length == 2) {
                    string assemblyName = typeData[1];
                    string typeName = typeData[0];
                    Assembly assembly = Assembly.Load(assemblyName);
                    try {
                        node = (Node)Activator.CreateInstance(assembly.GetType(typeName), new object[] { nodeDefinition });
                    }
                    catch {
                        throw;
                    }
                }
            }
            return node;
        }

        public virtual void Connect() { }
        public virtual void Disconnect() { }
        public virtual void Dispose() { }

        public virtual void RemoteDesktopConnect(bool showToolbar) {

            if (remDesktopForm == null || !remDesktopForm.Connected) {
                if (remDesktopForm != null) {
                    (remDesktopForm as Form).Close();
                    remDesktopForm = null;
                }
                if (RemoteDesktopType == 0)
                    remDesktopForm = new FormVNCClient(showToolbar);
                else {
                    remDesktopForm = new FormRemoteDesktop(showToolbar);
                }
                (remDesktopForm as Form).TopMost = true;
                remDesktopForm.RemoteConnectionError += remDesktopForm_RemoteConnectionError;
            }

            //remDesktopForm.Opacity = 0;
            (remDesktopForm as Form).Show();
            try {
                remDesktopForm.Connect(Address, User, Password, RDPort);
                (remDesktopForm as Form).BringToFront();
                //RemoteDesktopConnected = true;
            }
            catch {
                throw;
            }
        }

        void remDesktopForm_RemoteConnectionError(object sender, MessageEventArgs e) {
            throw new Exception(e.Message);
        }

        public virtual void RemoteDesktopDisconnect() {

            //try {
            if (remDesktopForm != null && remDesktopForm.Connected)
                remDesktopForm.Disconnect();
            //RemoteDesktopConnected = false;
            OnRemoteDesktopDisconnected(this, EventArgs.Empty);
            //}
            //catch {
            //    throw;
            //}
        }

        public virtual void StartNewBatch() { }
        public virtual void SaveBufferedImages(string path, SaveConditions saveCondition, int toSave) { }
        public virtual void ResetImagesBuffer(string path, bool freeFolders) { }
        public virtual void RefreshExportedParams() { }

        public virtual void OnNodeRecipeUpdate(object sender, NodeRecipeEventArgs e) {

            if (NodeRecipeUpdate != null) NodeRecipeUpdate(sender, e);
        }

        protected virtual void OnEnableNavigation(object sender, MessageEventArgs e) {

            if (EnableNavigation != null) EnableNavigation(sender, e);
        }

        protected virtual void OnRescanRequested(object sender, EventArgs e) {

            if (RescanRequested != null) RescanRequested(sender, e);
        }

        protected virtual void OnBootDone(object sender, EventArgs e) {

            if (BootDone != null) BootDone(sender, e);
        }

        protected virtual void OnReceivedGretelInfo(object sender, MessageEventArgs e)
        {

            if (ReceivedGretelInfo != null) ReceivedGretelInfo(sender, e);
        }

        protected virtual void OnDumpingChanged(object sender, EventArgs e) {
            if (DumpingChanged != null) DumpingChanged(sender, e);
        }

        protected virtual void OnRemoteDesktopDisconnected(object sender, EventArgs e) {
            if (RemoteDesktopDisconnected != null) RemoteDesktopDisconnected(sender, e);
        }

        protected virtual void OnSavingBufferedImages(object sender, MessageEventArgs e) {

            if (SavingBufferedImages != null) SavingBufferedImages(sender, e);
        }

        protected virtual void OnClientCollectorInfo(object sender, CollectorInfoEventArgs e) {

            if (ClientCollectorInfo != null) ClientCollectorInfo(sender, e);
        }

        protected virtual void OnClientAuditMessage(object sender, MessageEventArgs e)
        {
            if (ClientAuditMessage != null) ClientAuditMessage(sender, e);
        }

        public virtual void SetParameters(string recipeName, NodeRecipe dataSource, ParameterInfoCollection pic, string cultureCode) {
            throw new NotImplementedException();
        }

        public virtual NodeRecipe GetParameters() {
            throw new NotImplementedException();
        }

        public virtual NodeRecipe UploadParameters() {
            throw new NotImplementedException();
        }

        public virtual void SendUserLevelClass(UserLevelEnum userLevel) {
            throw new NotImplementedException();
        }

        public virtual void SendInspectionViewId(int inspectionId) {
            throw new NotImplementedException();
        }

        public abstract void StartImagesDump(List<StationDumpSettings> statDumpSettingsCollection);
        public abstract void StartImagesDump2(List<StationDumpSettings2> statDumpSettingsCollection);
        public abstract void StopImagesDump();
        public abstract void StopImagesDump2();
        public abstract bool Lock();
        public abstract bool Unlock();

        public virtual void Run(int inspectionId) {
            throw new NotImplementedException();
        }

        public virtual void Live(int inspectionId, bool enable) {
            throw new NotImplementedException();
        }

        public virtual void RecipeUpdatedOK()
        {

        }

        public virtual void RecipeUpdatedError(string message)
        {

        }
    }

    public class NodeCollection : List<INode> {

        public INode this[INode node] {
            get { return this.Find(n => { return n.IdNode == node.IdNode; }); }
        }

        public int NodesConnected {
            get {
                int res = 0;
                foreach (INode n in this) {
                    if (n.Connected == true)
                        res++;
                }
                return res;
            }
        }
    }

}
