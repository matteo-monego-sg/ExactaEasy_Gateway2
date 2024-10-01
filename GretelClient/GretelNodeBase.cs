using DisplayManager;
using ExactaEasyCore;
using ExactaEasyEng;
using ExactaEasyEng.AppDebug;
using SPAMI.Util.Logger;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace GretelClients
{
    public class GretelNodeBase : DisplayManager.Node 
    {
        private static readonly ClientDataCollection ClientDataCollection = new ClientDataCollection();
        private static bool _serverInitialized;
        private static readonly GretelNodeBaseCollection GretelClients = new GretelNodeBaseCollection();
        private static readonly object GrLock = new object();
       

        static GretelNodeBase() {

            GretelSvc.ClientConnected += GretelSvc_ClientConnected;
            GretelSvc.ClientDisconnected += GretelSvc_ClientDisconnected;
            GretelSvc.ClientImagesResults += GretelSvc_ClientImagesResults;
            // MM-11/01/2024: HVLD2.0 evo.
            GretelSvc.ClientHvldDataResults += GretelSvc_ClientHvldDataResults;
            GretelSvc.ClientInspectionResults += GretelSvc_ClientInspectionResults;
            GretelSvc.ClientStringMessage += GretelSvc_ClientStringMessage;
            GretelSvc.ClientStringWarning += GretelSvc_ClientStringWarning;
            GretelSvc.ClientDisconnectRemoteDesktop += GretelSvc_ClientDisconnectRD;
            GretelSvc.ClientReceivedRecipeAck += GretelSvc_ClientReceivedRecipeAck;
            GretelSvc.ClientRecipeReceived += GretelSvc_ClientRecipeReceived;
            GretelSvc.ClientReceivedDumpAck += GretelSvc_ClientReceivedDumpAck;
            GretelSvc.ClientReceivedBootDone += GretelSvc_ClientReceivedBootDone;
            GretelSvc.ClientReceivedGretelInfo += GretelSvc_ClientReceivedGretelInfo;
            GretelSvc.ClientImagesInfoReceived += GretelSvc_ClientImagesInfoReceived;
            GretelSvc.ClientCollectorInfo += GretelSvc_ClientCollectorInfo; 
            GretelSvc.ClientExportParametersReceived += GretelSvc_ClientExportParametersReceived;
            GretelSvc.ClientAuditMessage += GretelSvc_ClientAuditMessage;
        }

        static void GretelSvc_ClientAuditMessage(object sender, ClientEventArgs e) {
            if (GretelClients == null || GretelClients.Count <= 0 || GretelClients[e.Id] == null) return;
            lock (GrLock) {
                GretelClients[e.Id].OnClientAuditMessage(e);
            }
        }

        private static void GretelSvc_ClientConnected(object sender, ClientEventArgs e) {
            if (GretelClients == null || GretelClients.Count <= 0 || GretelClients[e.Id] == null) return;
            lock (GrLock) {
                GretelClients[e.Id].OnClientConnected(e);
            }
        }

        private static void GretelSvc_ClientDisconnected(object sender, ClientEventArgs e) {
            if (GretelClients == null || GretelClients.Count <= 0 || GretelClients[e.Id] == null) return;
            lock (GrLock) {
                GretelClients[e.Id].OnClientDisconnected(e);
            }
        }

        private static void GretelSvc_ClientImagesResults(object sender, ByteArrayEventArgs e) {
            if (GretelClients == null || GretelClients.Count <= 0 || GretelClients[e.Id] == null) return;
            lock (GrLock) {
                GretelClients[e.Id].OnClientImagesResults(e);
            }
        }

        /// <summary>
        /// MM-11/01/2024: HVLD2.0 evo.
        /// </summary>
        private static void GretelSvc_ClientHvldDataResults(object sender, ByteArrayEventArgs e) {
            if (GretelClients == null || GretelClients.Count <= 0 || GretelClients[e.Id] == null) return;
            lock (GrLock) {
                GretelClients[e.Id].OnClientHvldDataResults(e);
            }
        }

        private static void GretelSvc_ClientInspectionResults(object sender, GretelInspectionResultsEventArgs e) {
            if (GretelClients == null || GretelClients.Count <= 0 || e.Id >= GretelClients.Count || GretelClients[e.Id] == null) {
                if (e.Id >= GretelClients.Count)
                    Log.Line(LogLevels.Error, "GretelNodeBase.ClientInspectionResults", "Invalid id {0} received!", e.Id);
                return;
            }
            lock (GrLock) {
                GretelClients[e.Id].OnClientInspectionResults(e);
            }
        }

        private static void GretelSvc_ClientCollectorInfo(object sender, CollectorInfoEventArgs e) {
            if (GretelClients == null || GretelClients.Count <= 0 || e.Id >= GretelClients.Count || GretelClients[e.Id] == null) {
                if (e.Id >= GretelClients.Count)
                    Log.Line(LogLevels.Error, "GretelNodeBase.ClientCollectorInfo", "Invalid id {0} received!", e.Id);
                return;
            }
            lock (GrLock) {
                GretelClients[e.Id].OnClientCollectorInfo(sender, e);
            }
        }

        private static void GretelSvc_ClientStringMessage(object sender, ClientEventArgs e) {
            if (GretelClients == null || GretelClients.Count <= 0 || GretelClients[e.Id] == null) return;
            lock (GrLock) {
                GretelClients[e.Id].OnClientStringMessage(e);
            }
        }

        private static void GretelSvc_ClientStringWarning(object sender, ClientEventArgs e) {
            if (GretelClients == null || GretelClients.Count <= 0 || GretelClients[e.Id] == null) return;
            lock (GrLock) {
                GretelClients[e.Id].OnClientStringWarning(e);
            }
        }

        private static void GretelSvc_ClientDisconnectRD(object sender, ClientEventArgs e) {
            if (GretelClients == null || GretelClients.Count <= 0 || GretelClients[e.Id] == null) return;
            lock (GrLock) {
                GretelClients[e.Id].OnClientDisconnectRemoteDesktop(e);
            }
        }

        private static void GretelSvc_ClientReceivedRecipeAck(object sender, ClientEventArgs e) {
            if (GretelClients == null || GretelClients.Count <= 0 || GretelClients[e.Id] == null) return;
            
            lock (GrLock) 
            {
                switch (GretelClients[e.Id].SetParametersStatus)
                {
                    case SetParametersStatusEnum.TimeoutError:
                        // If the node is timeouted, drops the ACK.
                        Log.Line(LogLevels.Warning, "GretelSvc_ClientReceivedRecipeAck", $"ACK from {GretelClients[e.Id].Address} dropped because of a SetParameters timeout.");
                        return;
                }

                GretelClients[e.Id].OnClientReceivedRecipeAck(e);
                Log.Line(LogLevels.Pass, "GretelSvc_ClientReceivedRecipeAck", "Getting img info from " + GretelClients[e.Id].Address);
                GretelSvc.GetImageInfo(GretelClients[e.Id].IdGretel);
            }
        }

        private static void GretelSvc_ClientReceivedDumpAck(object sender, ClientEventArgs e) {
            if (GretelClients == null || GretelClients.Count <= 0 || GretelClients[e.Id] == null) return;
            lock (GrLock) {
                GretelClients[e.Id].OnClientReceivedDumpAck(e);
            }
        }

        private static void GretelSvc_ClientReceivedBootDone(object sender, ClientEventArgs e) {
            if (GretelClients == null || GretelClients.Count <= 0 || GretelClients[e.Id] == null) return;
            lock (GrLock) {
                GretelClients[e.Id].OnClientReceivedBootDone(e);
            }
        }

        private static void GretelSvc_ClientReceivedGretelInfo(object sender, ClientEventArgs e)
        {
            if (GretelClients == null || GretelClients.Count <= 0 || GretelClients[e.Id] == null) return;
            lock (GrLock)
            {
                GretelClients[e.Id].OnClientReceivedGretelInfo(e);
            }
        }

        private static void GretelSvc_ClientRecipeReceived(object sender, RecipeEventArgs e) {
            if (GretelClients == null || GretelClients.Count <= 0 || GretelClients[e.Id] == null) return;
            lock (GrLock) {
                GretelClients[e.Id].OnClientRecipeReceived(e);
            }
        }

        static void GretelSvc_ClientImagesInfoReceived(object sender, ImagesInfoReceivedEventArgs e) {
            if (GretelClients == null || GretelClients.Count <= 0 || GretelClients[e.Id] == null) return;
            lock (GrLock) {
                GretelClients[e.Id].ChangeImageInfo(e);
            }
        }

        static void GretelSvc_ClientExportParametersReceived(object sender, ExportParametersEventArgs e) {
            if (GretelClients == null || GretelClients.Count <= 0 || GretelClients[e.Id] == null) return;
            lock (GrLock) {
                GretelClients[e.Id].UpdateExportParameters(e);
            }
        }

        private int _idGretel;
        internal int IdGretel {
            get { return _idGretel; }
            set {
                _idGretel = value;
                foreach (var station in Stations) {
                    var s = (GretelStationBase)station;
                    s.IdGretel = _idGretel;
                }
            }
        }
        private ParameterInfoCollection _pic;
        private string _cultureCode;
        protected int StationsCount { get; private set; }

        private readonly ManualResetEvent _connectedEv = new ManualResetEvent(false);
        private ManualResetEvent UploadEv = new ManualResetEvent(false);
        //private readonly ManualResetEvent _readyToReceiveReceipt = new ManualResetEvent(false);
        private NodeRecipe dataSource;                  //parametri di ricetta
        private NodeRecipe currentNodeParameters;       //parametri device
        //private bool currentNodeParametersReceived;
        private List<StationDumpSettings> _currStatDumpSettingsCollection;
        private List<StationDumpSettings2> _currStatDumpSettingsCollection2;
        bool disableWarning;
        //System.Timers.Timer connectionReadyDelay;

        public GretelNodeBase(NodeDefinition nodeDefinition)
            : base(nodeDefinition) {

            StationsCount = nodeDefinition.StationsCount;
            var newClientData = new ClientData() {
                LocalIpAddress = ServerIP4Address,
                RemoteName = Description,
                RemoteIpAddress = Address,
                RemotePort = Port
            };
            ClientDataCollection.Add(newClientData);
            lock (GrLock) {
                if (GretelClients[this] == null)
                    GretelClients.Add(this);
            }
        }

        public override void Dispose() {
            lock (GrLock) {
                GretelClients.Remove(this);
                Log.Line(LogLevels.Warning, "GretelNodeBase.Dispose", "Client " + ServerIP4Address + ": Disposed!");

                


                if (GretelClients.Count == 0) {
                    try {
                        GretelSvc.StopServer();
                    }
                    catch (Exception ex) {
                        Log.Line(LogLevels.Warning, "GretelNodeBase.Dispose", "Ethernet stop server failed. Error: " + ex.Message);
                    }
                    GretelSvc.ClientConnected -= GretelSvc_ClientConnected;
                    GretelSvc.ClientDisconnected -= GretelSvc_ClientDisconnected;
                    GretelSvc.ClientImagesResults -= GretelSvc_ClientImagesResults;
                    GretelSvc.ClientInspectionResults -= GretelSvc_ClientInspectionResults;
                    GretelSvc.ClientStringMessage -= GretelSvc_ClientStringMessage;
                    GretelSvc.ClientStringWarning -= GretelSvc_ClientStringWarning;
                    GretelSvc.ClientDisconnectRemoteDesktop -= GretelSvc_ClientDisconnectRD;
                    GretelSvc.ClientReceivedRecipeAck -= GretelSvc_ClientReceivedRecipeAck;
                    GretelSvc.ClientRecipeReceived -= GretelSvc_ClientRecipeReceived;
                    GretelSvc.ClientReceivedDumpAck -= GretelSvc_ClientReceivedDumpAck;
                    GretelSvc.ClientReceivedBootDone -= GretelSvc_ClientReceivedBootDone;
                    GretelSvc.ClientImagesInfoReceived -= GretelSvc_ClientImagesInfoReceived;
                    GretelSvc.ClientCollectorInfo -= GretelSvc_ClientCollectorInfo;
                }
            }
        }

        public override void StartNewBatch() {

            Log.Line(LogLevels.Pass, "GretelNodeBase.StartNewBatch", "Getting img info from " + Address);
            GretelSvc.GetImageInfo(IdGretel);
            base.StartNewBatch();
        }

        public override void SaveBufferedImages(string path, SaveConditions sc, int toSave) {

            //OnSavingBufferedImages(this, new DisplayManager.MessageEventArgs("1"));
            foreach (IStation s in Stations)
                s.SaveBufferedImages(path, sc, toSave);
            //OnSavingBufferedImages(this, new DisplayManager.MessageEventArgs("0"));
        }

        public override void ResetImagesBuffer(string path, bool freeFolders) {

            foreach (IStation s in Stations)
                s.ResetImagesBuffer(path, freeFolders);
        }

        public override void SendUserLevelClass(ExactaEasyEng.UserLevelEnum userLevel) {

            Log.Line(LogLevels.Pass, "GretelNodeBase.SendUserLevelClass",
                "Client {0}: Sending user level " + userLevel.ToString(), IdGretel);
            GretelSvc.SendUserLevel(IdGretel, userLevel);
        }

        public override void SendInspectionViewId(int inspectionId) {

            Log.Line(LogLevels.Pass, "GretelNodeBase.SendInspectionViewId",
                "Client {0}: Sending view inspection ID " + inspectionId.ToString(), IdGretel);
            GretelSvc.SendInspectionViewId(IdGretel, inspectionId);
        }

        private void OnClientConnected(ClientEventArgs e) {

            Log.Line(LogLevels.Pass, "GretelNodeBase.OnClientConnected",
                "Client " + e.Ip + " (" + e.Id.ToString(CultureInfo.InvariantCulture) + ") connected!");
            IdGretel = e.Id;
            Connected = true;
            disableWarning = false;
            _connectedEv.Set();
            //connectionReadyDelay = new System.Timers.Timer(4 * 1000D);
            //connectionReadyDelay.Elapsed += new System.Timers.ElapsedEventHandler(connectionReadyDelay_Elapsed);
            //connectionReadyDelay.Enabled = true;
            OnRescanRequested(this, EventArgs.Empty);
        }

        //void connectionReadyDelay_Elapsed(object sender, EventArgs e) {

        //    _readyToReceiveReceipt.Set();
        //    //if (connectionReadyDelay != null) {
        //    //    connectionReadyDelay.Enabled = false;
        //    //    connectionReadyDelay = null;
        //    //}
        //    if (DataSource != null && _pic != null && !string.IsNullOrEmpty(_cultureCode))
        //        SetParameters("", DataSource, _pic, _cultureCode);
        //}

        private void OnClientDisconnected(ClientEventArgs e) {
            Log.Line(LogLevels.Pass, "GretelNodeBase.OnClientDisconnected",
                "Client " + e.Ip + " (" + e.Id.ToString(CultureInfo.InvariantCulture) + ") disconnected!");
            Connected = false;
            Version = "?";
            _connectedEv.Reset();
            //_readyToReceiveReceipt.Reset();
            OnRescanRequested(this, EventArgs.Empty);
        }

        //double imagescounter = 0;
        private void OnClientImagesResults(ByteArrayEventArgs e) {

            //Log.Line(LogLevels.Warning, "GretelNodeBase.OnClientImagesResults", e.Ip + ": " + e.Res.Message);
            var inspId = e.InspectionId; //relativeToAbsolute(e.InspectionId);
            //Log.Line(LogLevels.Debug, "GretelNodeBase.OnClientImagesResults", e.Ip + " INCOMING IMAGE FROM {0}", inspId);
            if (Stations[inspId] is null) 
            {
                if (Stations.Count > 0)
                    Log.Line(LogLevels.Error, "GretelNodeBase.OnClientImagesResults", "Unexpected station id");
                //throw new Exception("Unexpected station id");
                return;
            }

            //debug image size
            DebugSizeImagesResults sizeImageResult = AppDebug.SizeImagesResults[e.Id, e.InspectionId];
            if(sizeImageResult != null)
                sizeImageResult.AddSize(e.Res.Length);

            //live filter
            GretelStationBase station = (GretelStationBase)Stations[inspId];
            LiveImageFilter live = station.LiveImageFilter;

            if(live.Counter >= live.FrequencyInt)
            {
                station.OnClientImagesResults(e);
                live.ResetCounter();
            }

            //imagescounter++;
        }
		/// <summary>
        /// MM-11/01/2024: HVLD2.0 evo.
        /// </summary>
        private void OnClientHvldDataResults(ByteArrayEventArgs e)
        {
            var inspId = e.InspectionId; //relativeToAbsolute(e.InspectionId);
            //Log.Line(LogLevels.Debug, "GretelNodeBase.OnClientImagesResults", e.Ip + " INCOMING IMAGE FROM {0}", inspId);
            if (Stations[inspId] is null)
            {
                if (Stations.Count > 0)
                    Log.Line(LogLevels.Error, "GretelNodeBase.OnClientHvldDataResults", "Unexpected station id");
                return;
            }
            ((GretelStationBase)Stations[inspId]).OnClientHvldDataResults(e);
        }

        //int relativeToAbsolute(int IdRelative) {
        //    foreach (Station st in Stations) {
        //        foreach (Camera cam in st.Cameras) {
        //            if (cam.Head == IdRelative)
        //                return st.IdStation;
        //        }
        //    }
        //    return -1;
        //}

        private void OnClientInspectionResults(GretelInspectionResultsEventArgs e) {
            foreach (InspectionResults gmr in e.InspectionResultsCollection) {

                if (Stations[gmr.InspectionId] == null) {
                    if (Stations.Count > 0)
                        Log.Line(LogLevels.Error, "GretelNodeBase.OnClientInspectionResults", "Unexpected station id. Node: {0} - Station: {1}", gmr.NodeId, gmr.InspectionId);
                    //throw new Exception("Unexpected station id");
                    return;
                }

                GretelStationBase station = (GretelStationBase)Stations[gmr.InspectionId];
                LiveImageFilter live = station.LiveImageFilter;

                switch (live.Mode)
                {
                    case LiveImageFilterMode.All:
                        live.IncrementCounter();
                        break;
                    case LiveImageFilterMode.Good:
                        if (gmr.IsReject == false)
                            live.IncrementCounter();
                        break;
                    case LiveImageFilterMode.Reject:
                        if (gmr.IsReject && gmr.RejectionCause != 0 && gmr.RejectionCause != 16384 && gmr.RejectionCause != 32768) //16384=Light  32768=TechReject
                            live.IncrementCounter();
                        break;
                    case LiveImageFilterMode.Light:
                        if (gmr.IsReject && gmr.RejectionCause == 16384)
                            live.IncrementCounter();
                        break;
                    case LiveImageFilterMode.TechReject:
                        if (gmr.IsReject && gmr.RejectionCause == 32768)
                            live.IncrementCounter();
                        break;
                }

                if(live.Counter >= live.FrequencyInt)
                {
                    station.OnInspectionResults(new InspectionResults(IdNode, gmr.InspectionId, gmr.SpindleId, 
                        gmr.VialId, gmr.IsActive, gmr.IsReject, gmr.RejectionCause, gmr.ResToolCollection));
                    //live.ResetCounter(); //reset in OnImageResult
                }
            }
        }

        private void ChangeImageInfo(ImagesInfoReceivedEventArgs e) {
            for (int i = 0; i < e.ImageInfo.Length; i++) {
                int inspId = i; // relativeToAbsolute(gmr.InspectionId);

                //if (Stations[inspId] == null) {
                //    if (Stations.Count > 0)
                //        Log.Line(LogLevels.Warning, "GretelNodeBase.RasieClientImageInfoReceived", "Unexpected station id");
                //    //throw new Exception("Unexpected station id");
                //    //return;
                //}
                //else
                if (Stations[inspId] != null)
                    ((GretelStationBase)Stations[inspId]).ChangeImageInfo(e.ImageInfo[i]);
            }
        }

        private void UpdateExportParameters(ExportParametersEventArgs e) {

            foreach (GretelStationBase s in Stations)
            {
                s.ExportParamList.Clear();
            }
            for (int i = 0; i < e.ExportParams.Length; i++) {
                int inspId = e.ExportParams[i].hInspectionId;
                if (inspId > -1 && Stations[inspId] != null)
                {
                    ((GretelStationBase)Stations[inspId]).ExportParamList.Add(e.ExportParams[i]); 
                }
            }
            foreach (GretelStationBase s in Stations) {
                s.UpdateExportParameter();
            }
        }


        private void OnClientStringMessage(ClientEventArgs e) {
            Log.Line(LogLevels.Pass, "GretelNodeBase.OnClientStringMessage", e.Ip + " SAYS \"" + e.Notes + "\"");
        }

        private void OnClientStringWarning(ClientEventArgs e) {
            if (e.Notes.Contains("unable to bind") && !disableWarning) {
                Log.Line(LogLevels.Warning, "GretelNodeBase.OnClientStringWarning", e.Ip + " WARNING: \"" + e.Notes + "\"");
                disableWarning = true;
            }
            if (e.Notes.Contains("unable to bind") == false)
            {
                Log.Line(LogLevels.Warning, "GretelNodeBase.OnClientStringWarning", e.Ip + " WARNING: \"" + e.Notes + "\"");
            }
        }

        private void OnClientDisconnectRemoteDesktop(ClientEventArgs e) {
            Log.Line(LogLevels.Pass, "GretelNodeBase.OnClientDisconnectRD", e.Ip + " SAYS \"" + e.Notes + "\"");
            //if (currentNodeParametersReceived == false) {
            //    Log.Line(LogLevels.Warning, "GretelNodeBase.OnClientDisconnectRD", e.Ip + ": Current parameters NOT received");
            //}
            //currentNodeParametersReceived = false;
            RemoteDesktopDisconnect();
            GretelSvc.GetImageInfo(IdGretel);
        }

        private void OnClientReceivedRecipeAck(ClientEventArgs e)
        {
            try
            {
                switch (SetParametersStatus)
                {
                    case SetParametersStatusEnum.TimeoutError:
                        {
                            Log.Line(
                                LogLevels.Error,
                                "GretelNodeBase.OnClientReceivedRecipeAck", $"{e.Ip} SAYS '{e.Notes}', BUT THIS ACK IS GOING TO BE DROPPED DUE TO A RECIPE UPLOAD TIMEOUT.");
                        }
                        return;

                    case SetParametersStatusEnum.Uploading:
                        {
                            var successString = e.Notes.Replace("RECIPE RECEIVED: ", "");
                            int success;
                            int.TryParse(successString, out success);

                            if (success != 0)
                            {
                                SetParametersStatus = SetParametersStatusEnum.UploadedOK;
                                Log.Line(
                                    LogLevels.Pass,
                                    "GretelNodeBase.OnClientReceivedRecipeAck", $"{e.Ip} SAYS '{e.Notes}': RECIPE UPLOAD SUCCEDEED.");
                            }
                            else
                            {
                                SetParametersStatus = SetParametersStatusEnum.UploadError;
                                Log.Line(
                                    LogLevels.Error,
                                    "GretelNodeBase.OnClientReceivedRecipeAck", $"{e.Ip} SAYS '{e.Notes}': RECIPE UPLOAD FAILED.");
                            }
                        }
                        return;

                    default:
                        {
                            Log.Line(
                                LogLevels.Warning,
                                "GretelNodeBase.OnClientReceivedRecipeAck", $"{e.Ip} SAYS '{e.Notes}', BUT THE NODE WAS NOT IN 'UPLOADING' STATE [ACK DROPPED].");
                        }
                        return;
                }
            }
            finally
            {
                SetParametersDone.Set();
            }
        }

        private void OnClientReceivedDumpAck(ClientEventArgs e) 
        {
            Log.Line(LogLevels.Pass, "GretelNodeBase.OnClientReceivedDumpAck", e.Ip + " SAYS \"" + e.Notes + "\"");

            var successString = e.Notes.Replace("COLLECTOR STATUS: ", "");
            int dumping;
            int.TryParse(successString, out dumping);
            Dumping = dumping != 0;
            //DumpAckReceived.Set();
            OnDumpingChanged(this, EventArgs.Empty);
        }

        private void OnClientReceivedBootDone(ClientEventArgs e) 
        {
            Log.Line(LogLevels.Pass, "GretelNodeBase.OnClientReceivedBootDone", e.Ip + " SAYS \"" + e.Notes + "\"");
            //se stava registrando ed è crashato mi allineo alla ripartenza
            if (Dumping) {
                Dumping = false;
                OnDumpingChanged(this, EventArgs.Empty);
            }
            //_readyToReceiveReceipt.Set();
            //Trace.WriteLine("OnBootDone: cultura: " + (_cultureCode != null) + " PIC: " + (_pic != null) + " dataSource: " + (dataSource != null));
            //if (dataSource != null && _pic != null && !string.IsNullOrEmpty(_cultureCode)) {
            OnBootDone(this, EventArgs.Empty);
            //}
        }

        private void OnClientReceivedGretelInfo(ClientEventArgs e)
        {
            Log.Line(LogLevels.Pass, "GretelNodeBase.OnClientReceivedGretelInfo", e.Ip + " SAYS \"" + e.Notes + "\"");
            try
            {
                var info = e.Notes.Split('\t');
                try
                {
                    int protocolV = int.Parse(info[2].Split(':')[1]);
                    GretelSvc.protocolVersion = protocolV;
                }
                catch (Exception)
                {
                    GretelSvc.protocolVersion = 0;
                }
                int resultInspectionSize = 0;

                if (GretelSvc.protocolVersion == 0)
                {
                    var resInspTemp = new GretelSvc.ResultInspectionV0();
                    resultInspectionSize = Marshal.SizeOf(resInspTemp);
                }
                else if (GretelSvc.protocolVersion == 1)
                {
                    var resInspTemp = new GretelSvc.ResultInspectionV1();
                    resultInspectionSize = Marshal.SizeOf(resInspTemp);
                }

                int crc_received = int.Parse(info[1].Split(':')[1]);
                if (crc_received != resultInspectionSize)
                {
                    OnClientStringWarning(new ClientEventArgs(e.Ip, e.Id, info[0] + " WRONG CRC. Expected: " + resultInspectionSize + " Received: " + crc_received));
                    MessageBox.Show(e.Ip + ": " + info[0] + "\r\nFATAL ERROR! WRONG CRC. Expected: " + resultInspectionSize + " Received: " + crc_received, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
                Version = info[0].Split(':')[1];
                
                OnReceivedGretelInfo(this, new DisplayManager.MessageEventArgs(e.Notes));
                OnRescanRequested(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Warning, "GretelNodeBase.OnClientReceivedGretelInfo", e.Ip + " FATAL ERROR: Cannot parse Gretel CRC. Error: " + ex.Message);
                MessageBox.Show(e.Ip + " FATAL ERROR: Cannot parse Gretel CRC");
                Application.Exit();
            }
        }

        private void OnClientRecipeReceived(RecipeEventArgs e) {
            //checking special characters in recipe name
            if(e.RecipeName.Contains("'") || e.RecipeName.Contains("@") || e.RecipeName.Contains("%") || e.RecipeName.Contains("&"))
            {
                Log.Line(LogLevels.Error, "GretelNodeBase.OnClientRecipeReceived", e.Ip + " Can't save to file Gretel recipe: the recipe name cannot contain these characters: [' , @ , % , &]");
                RecipeUpdatedError("recipe name can't contain characters: [',@,%,&]"); //max about 45 characters
                return;
            }

            //checking special characters in recipe parameters
            if (e.RecipeV2.Contains("@")/* || e.RecipeV2.Contains("'") || e.RecipeV2.Contains("%") || e.RecipeV2.Contains("&")*/)
            {
                Log.Line(LogLevels.Error, "GretelNodeBase.OnClientRecipeReceived", e.Ip + " Can't save to file Gretel recipe: the recipe parameters cannot contain these characters: [@]");
                RecipeUpdatedError("recipe parameters can't contain symbol: [@]"); //max about 45 characters
                return;
            }

            if (e.RecipeV2.Contains("'") || e.RecipeV2.Contains("&apos"))            
                Log.Line(LogLevels.Warning, "GretelNodeBase.OnClientRecipeReceived", e.Ip + " Gretel recipe contains the following characters: ['] Avoid using this character unless strictly necessary");
            

            Log.Line(LogLevels.Pass, "GretelNodeBase.OnClientRecipeReceived",
                e.Ip + ": RECIPE RECEIVED. Name: " + e.RecipeName);

            //try
            //{
            //    string fileName = (e.ToSave == true) ? "GretelRecipeToSave_" : "GretelRecipeApplied_";
            //    fileName += IdNode + ".xml";
            //    var writer = new StreamWriter(fileName);
            //    writer.Write(e.RecipeV2);
            //    writer.Close();
            //}
            //catch
            //{
            //    Log.Line(LogLevels.Warning, "GretelNodeBase.OnClientRecipeReceived", e.Ip + " Can't save to file Gretel recipe.");
            //}

            Task tkUpdateRecipe = new Task(new Action(() => { updateRecipe(e); }));
            tkUpdateRecipe.Start();
            
            //NodeRecipe newNodeRecipe = ReadNodeRecipeV2(e.RecipeV2);
            //currNodeRecipe = newNodeRecipe.Clone(
        }

        private void updateRecipe(RecipeEventArgs e) {

            // OnEnableNavigation(this, new DisplayManager.MessageEventArgs(false.ToString()));
            AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Busy);

            Log.Line(LogLevels.Pass, "GretelNodeBase.OnClientRecipeReceived.updateRecipe", "Starting ReadNodeRecipeV2 " + Address);
            currentNodeParameters = ReadNodeRecipeV2(e.RecipeV2);
            Log.Line(LogLevels.Pass, "GretelNodeBase.OnClientRecipeReceived.updateRecipe", "Completed ReadNodeRecipeV2 " + Address);

            currentNodeParameters.Description = e.RecipeName;
            currentNodeParameters.Id = e.Id;
            //if (e.IsNew)
            //currentNodeParametersReceived = true;
            OnNodeRecipeUpdate(this, new NodeRecipeEventArgs(currentNodeParameters, e.ToSave));
            Log.Line(LogLevels.Pass, "GretelNodeBase.OnClientRecipeReceived", "Getting img info from " + Address);
            if (e.ToSave == true) {
                GretelSvc.GetImageInfo(IdGretel);
            }
            UploadEv.Set();
            // OnEnableNavigation(this, new DisplayManager.MessageEventArgs(true.ToString()));
        }

        private void OnClientAuditMessage(ClientEventArgs e) {

            OnClientAuditMessage(this, new DisplayManager.MessageEventArgs(Description + " - " + e.Notes));
        }

        public override void Connect() {

            if (!_serverInitialized) {
                _serverInitialized = true;
                int stationsCount = 0;
                if (GretelClients != null) {
                    foreach (GretelNodeBase gns in GretelClients) {
                        stationsCount += gns.StationsCount;
                    }
                }
                try {
                    GretelSvc.StartServer(ClientDataCollection, stationsCount);
                    Thread.Sleep(100);
                }
                catch {
                    _serverInitialized = false;
                    //throw;
                }
            }
            if (!_connectedEv.WaitOne(5000))
                throw new Exception("Connection with node failed.");
        }

        public override void Disconnect() {
        }

        public override NodeRecipe GetParameters() {

            //string gretelXml = "";
            //// TODO: Estrazione ricetta nodo Gretel
            //using (StreamReader reader = new StreamReader("BootRecipe.xml")) {
            //    gretelXml = reader.ReadToEnd();
            //}
            IdGretel = _idGretel;       // necessario per aggiornare quello delle stazioni
            if (currentNodeParameters == null) {
                currentNodeParameters = dataSource;
            }
            return currentNodeParameters;
        }

        public override void SetParameters(string recipeName, NodeRecipe _dataSource, ParameterInfoCollection pic, string cultureCode) 
        {
            SetParametersDone.Reset();
            // Matteo - 27/06/2024.
            SetParametersStatus = SetParametersStatusEnum.Uploading;
            //SetParametersCompletedSuccessfully = false;

            var writer = new StreamWriter("GretelNode_" + IdNode + ".xml");
            //writer.Write(SaveRecipeV2(dataSource));
            if (string.IsNullOrEmpty(_dataSource.RawRecipe) == true)
            {
                writer.Write(SaveRecipeV2(_dataSource));
            }
            else
            {
                writer.Write(_dataSource.RawRecipe);
            }
            writer.Close();
            dataSource = _dataSource.Clone(pic, cultureCode);
            _cultureCode = cultureCode;
            _pic = pic;

            if (Connected) 
            {
                Log.Line(LogLevels.Pass, "GretelNodeBase.SetParameters", "Client {0}: Sending recipe...", IdGretel);
                //GretelSvc.SendRecipe(IdGretel, recipeName + "_client_" + IdGretel.ToString("d2"), SaveRecipeV2(_dataSource));
                if (string.IsNullOrEmpty(_dataSource.RawRecipe) == true) {
                    GretelSvc.SendRecipe(IdGretel, recipeName + "_client_" + IdGretel.ToString("d2"), SaveRecipeV2(_dataSource));
                }
                else {
					// DA CONCORDARE CON LUCA
                    //_dataSource.RawRecipe = _dataSource.RawRecipe.Replace("qwertyuiop", "&crarr");
                    _dataSource.RawRecipe = _dataSource.RawRecipe.Replace("&#8629;", "&crarr");
                    GretelSvc.SendRecipe(IdGretel, recipeName + "_client_" + IdGretel.ToString("d2"), _dataSource.RawRecipe);
                }
            }
            else 
            {
                SetParametersStatus = SetParametersStatusEnum.UploadError;
                Log.Line(LogLevels.Error, "GretelNodeBase.SetParameters", "Client {0} not connected or not ready to receive receipt!", IdGretel);
                throw new Exception("Client " + IdGretel + " not connected or not ready to receive receipt!");
            }
        }


        //public NodeRecipe LoadFromFileV2(string filePath) {

        //    using (StreamReader reader = new StreamReader(filePath, Encoding.UTF7)) {
        //        return readNodeRecipeV2(reader.ReadToEnd());
        //    }
        //}

        //public NodeRecipe MyReadNodeRecipeV2(string xmlString) {

        //    var rn = new NodeRecipe();
        //    xmlString = xmlString.Replace("&crarr", "qwertyuiop");
        //    var doc = new XmlDocument();
        //    doc.LoadXml(xmlString);
        //    var mapNode = doc.SelectSingleNode("./map");
        //    //GetGrabbers(rn, mapNode);
        //    //GetStations(rn, mapNode);
        //    rn.RawRecipe = xmlString;
        //    Stopwatch diagTimer = new Stopwatch();
        //    diagTimer.Start();
        //    string[] fields = mapNode.InnerXml.ToLower().Split(new string[] { "<", ">", "/>" }, StringSplitOptions.RemoveEmptyEntries);
        //    foreach (string seed in fields) {

        //    }
        //    diagTimer.Stop();
        //    return rn;
        //}

        //private static string MyParseLine(string , string attributeName) {

        //    string res = "";
        //    foreach (string attribute in attributes) {
        //        if (attribute.Contains(attributeName) == true) {
        //            string[] attrval = attribute.Split(new string[] { "name=", "value=" }, StringSplitOptions.RemoveEmptyEntries);
        //            res = Regex.Replace(attrval[attrval.Length - 1], "[@,\\.\";'\\\\ ]", string.Empty);
        //            attributes.Remove(attribute);
        //            return res;
        //        }
        //    }
        //    return res;
        //}

        public static NodeRecipe ReadNodeRecipeV2(string xmlString) {

            var rn = new NodeRecipe();
            xmlString = xmlString.Replace("&crarr", "&#8629;");
            var doc = new XmlDocument();
            doc.LoadXml(xmlString);
            var mapNode = doc.SelectSingleNode("./map");
            GetGrabbers(rn, mapNode);
            GetStations(rn, mapNode);
            rn.RawRecipe = xmlString;
            return rn;
        }

        private static void GetGrabbers(NodeRecipe currentNode, XmlNode mapNode) {

            XmlNode grabbersEl =
                mapNode.SelectSingleNode(
                    "./class[translate(@type,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='frame grabbers setup']");
            for (int i = 0; i < 16; i++) {
                if (grabbersEl != null) {
                    XmlNode boardElActive =
                        grabbersEl.SelectSingleNode(
                            "./attribute[translate(@name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='board_" +
                            i + "_isactive']");
                    if (boardElActive != null) {
                        var frameRecipe = new FrameGrabberRecipe { BoardId = i };
                        if (boardElActive.Attributes != null && boardElActive.Attributes["value"] != null &&
                            boardElActive.Attributes["value"].Value == "1")
                            frameRecipe.Active = true;
                        XmlNode boardElCfg =
                            grabbersEl.SelectSingleNode(
                                "./attribute[translate(@name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='board_" +
                                i + "_cfgfile']");
                        if (boardElCfg != null && boardElCfg.Attributes != null &&
                            boardElCfg.Attributes["value"] != null)
                            frameRecipe.ConfigFileName = boardElCfg.Attributes["value"].Value;
                        XmlNode boardElType =
                            grabbersEl.SelectSingleNode(
                                "./attribute[translate(@name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='board_" +
                                i + "_type']");
                        if (boardElType != null && boardElType.Attributes != null &&
                            boardElType.Attributes["value"] != null)
                            frameRecipe.Type = boardElType.Attributes["value"].Value;
                        XmlNode boardElId =
                            grabbersEl.SelectSingleNode(
                                "./attribute[translate(@name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='board_" +
                                i + "_id']");
                        if (boardElId != null && boardElId.Attributes != null &&
                            boardElId.Attributes["value"] != null)
                            frameRecipe.Id = Convert.ToInt32(boardElId.Attributes["value"].Value);

                        currentNode.FrameGrabbers.Add(frameRecipe);
                    }
                }
            }
        }

        private static void GetStations(NodeRecipe currentNode, XmlNode mapNode) {

            XmlNode stationsEl =
                mapNode.SelectSingleNode(
                    "./class[translate(@type,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='inspectionsetup']");
            for (int i = 0; i < 16/*GretelSvc.MaxInspectionsNum*/; i++) { //PIER: NUMERO MAGICO!!!!
                if (stationsEl != null) {
                    XmlNode stationElLabel =
                        stationsEl.SelectSingleNode(
                            "./attribute[translate(@name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='inspection_" +
                            i + "_label']");
                    if (stationElLabel != null && stationElLabel.Attributes != null &&
                        stationElLabel.Attributes["value"] != null && stationElLabel.Attributes["value"].Value != "") {
                        var newSRecipe = new StationRecipe {
                            Id = i,
                            Description = stationElLabel.Attributes["value"].Value
                        };

                        XmlNode stationElActive =
                            stationsEl.SelectSingleNode(
                                "./attribute[translate(@name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='inspection_" +
                                i + "_isactive']");
                        if (stationElActive != null && stationElActive.Attributes != null &&
                            stationElActive.Attributes["value"] != null &&
                            stationElActive.Attributes["value"].Value == "1")
                            newSRecipe.Enable = true;

                        currentNode.Stations.Add(newSRecipe);
                        GetStationParameter(newSRecipe, mapNode);

                    }
                }
            }
        }

        private static void GetStationParameter(StationRecipe currentStation, XmlNode mapNode) {

            XmlNode stationEl = mapNode.SelectSingleNode("./class[@id='" + currentStation.Id + "']");
            if (stationEl != null) {
                currentStation.InspectionType = stationEl.OuterXml.Remove(stationEl.OuterXml.IndexOf('>'));
                currentStation.InspectionType = currentStation.InspectionType.Split(new char[] { '=', '\\', '"' }, StringSplitOptions.RemoveEmptyEntries)[1];
                if (currentStation.InspectionType.ToLower() == "inspection default")
                    currentStation.Default = true;  //pier: 2/12/2015 solo per retrocompatibilità
                //XmlNode stationEl =
                //    mapNode.SelectSingleNode(
                //        "./class[translate(@type,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='inspection particle' and @id='" +
                //        currentStation.Id + "']");
                //if (stationEl == null) {
                //    stationEl =
                //        mapNode.SelectSingleNode(
                //            "./class[translate(@type,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='inspection default' and @id='" +
                //            currentStation.Id + "']");
                //    if (stationEl == null)
                //        return;
                //    currentStation.Default = true;
                //}
                XmlNodeList camEls =
                    stationEl.SelectNodes(
                        "./class[translate(@type,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='cam']");
                if (camEls != null)
                    foreach (XmlNode camEl in camEls) {
                        if (camEl.Attributes != null && camEl.Attributes["id"] != null && camEl.Attributes["id"].Value != "") {
                            var newCRecipe = new CameraRecipe { Id = Convert.ToInt32(camEl.Attributes["id"].Value) };
                            GetParams(newCRecipe.AcquisitionParameters, camEl, "CAM");
                            Parameter activePar = newCRecipe.AcquisitionParameters.Find(par => par.Id == "Active");
                            if (activePar != null) {
                                if (activePar.Value == "1") {
                                    newCRecipe.Enabled = true;
                                }
                                else {
                                    newCRecipe.Enabled = false;
                                }
                                activePar.Value = newCRecipe.Enabled.ToString();
                                newCRecipe.AcquisitionParameters.RemoveAll(par => par.Id == "Active");
                                newCRecipe.AcquisitionParameters.Insert(0, activePar);
                            }
                            XmlNode grabberEl =
                                stationEl.SelectSingleNode(
                                    "./class[translate(@type,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='grabber' and @id='" +
                                    newCRecipe.Id + "']");
                            if (grabberEl != null)
                                GetParams(newCRecipe.AcquisitionParameters, grabberEl, "GRABBER");
                            XmlNode digitzerEl =
                                stationEl.SelectSingleNode(
                                    "./class[translate(@type,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='digitizer' and @id='" +
                                    newCRecipe.Id + "']");
                            if (digitzerEl != null)
                                GetParams(newCRecipe.DigitizerParameters, digitzerEl, "DIGITIZER");
                            XmlNode newNetEl =
                                stationEl.SelectSingleNode(
                                    "./class[translate(@type,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='neuralnetwork' and @id='" +
                                    newCRecipe.Id + "']");
                            if (newNetEl != null)
                                GetParams(newCRecipe.RecipeAdvancedParameters, newNetEl, "NEURALNETWORK");
                            XmlNode newImgStEl =
                                stationEl.SelectSingleNode(
                                    "./class[translate(@type,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='imagestack' and @id='" +
                                    newCRecipe.Id + "']");
                            if (newImgStEl != null)
                                GetParams(newCRecipe.RecipeAdvancedParameters, newImgStEl, "IMAGESTACK");
                            XmlNode lightSensEl =
                                stationEl.SelectSingleNode(
                                    "./class[translate(@type,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='light sensor' and @id='" +
                                    newCRecipe.Id + "']");
                            if (lightSensEl != null) {
                                GetParams(newCRecipe.LightSensor.LightSensorParameters, lightSensEl, "");
                                XmlNodeList shapeList =
                                    lightSensEl.SelectNodes(
                                        "./class[translate(@type,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='shape']");
                                if (shapeList != null)
                                    foreach (XmlNode shapeEl in shapeList) {
                                        if (shapeEl.Attributes != null && shapeEl.Attributes["id"] != null &&
                                            shapeEl.Attributes["id"].Value != "") {
                                            var newShape = new Shape { Id = Convert.ToInt32(shapeEl.Attributes["id"].Value) };
                                            GetParams(newShape.ShapeParameters, shapeEl, "");
                                            newCRecipe.LightSensor.Shapes.Add(newShape);
                                        }
                                    }
                            }
                            XmlNodeList stroboElList =
                                stationEl.SelectNodes(
                                    "./class[translate(@type,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='strobo']");
                            if (stroboElList != null)
                                foreach (XmlNode stroboEl in stroboElList) {
                                    var newLight = new LightRecipe();
                                    if (stroboEl.Attributes != null)
                                        newLight.Id = Convert.ToInt32(stroboEl.Attributes["id"].Value);
                                    GetParams(newLight.StroboParameters, stroboEl, "");
                                    newCRecipe.Lights.Add(newLight);
                                }
                            currentStation.Cameras.Add(newCRecipe);
                        }
                    }
                XmlNodeList toolsElList =
                    stationEl.SelectNodes(
                        "./class[translate(@type,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='tool']");
                if (toolsElList == null) return;
                foreach (XmlNode toolEl in toolsElList) {
                    var newTool = new Tool();
                    if (toolEl.Attributes != null) {
                        newTool.Id = Convert.ToInt32(toolEl.Attributes["id"].Value);
                        GetParams(newTool.ToolParameters, toolEl, "");
                        Parameter labelPar = newTool.ToolParameters.Find(par => par.Id == "Label");
                        if (labelPar != null) {
                            newTool.Label = labelPar.Value;
                        }
                        Parameter activePar = newTool.ToolParameters.Find(par => par.Id == "Active");
                        if (activePar != null) {
                            if (activePar.Value == "1") {
                                newTool.Active = true;
                            }
                            else {
                                newTool.Active = false;
                            }
                            activePar.Value = newTool.Active.ToString();
                            newTool.ToolParameters.RemoveAll(par => par.Id == "Active");
                            newTool.ToolParameters.Insert(0, activePar);
                        }

                        GetToolsParameter(newTool, toolEl);
                        var shapeList =
                            toolEl.SelectNodes(
                                "./class[translate(@type,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='shape']");
                        if (shapeList != null)
                            foreach (XmlNode shapeEl in shapeList) {
                                if (shapeEl.Attributes != null && shapeEl.Attributes["id"] != null &&
                                    shapeEl.Attributes["id"].Value != "") {
                                    var newShape = new Shape { Id = Convert.ToInt32(shapeEl.Attributes["id"].Value) };
                                    GetParams(newShape.ShapeParameters, shapeEl, "");
                                    newTool.Shapes.Add(newShape);
                                }
                            }
                    }
                    currentStation.Tools.Add(newTool);
                }
            }
        }

        private static void GetToolsParameter(Tool tool, XmlNode camNode) {

            XmlNode cntNode =
                camNode.SelectSingleNode(
                    "./attribute[translate(@name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='tooloutput_cnt']");
            int cnt = (cntNode == null || cntNode.Attributes == null || cntNode.Attributes["value"] == null)
                ? 0
                : Convert.ToInt32(cntNode.Attributes["value"].Value);
            for (int i = 0; i < cnt; i++) {
                string nodePrefix = "tooloutput_" + i.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
                XmlNode nameNode =
                    camNode.SelectSingleNode(
                        "./attribute[translate(@name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_','abcdefghijklmnopqrstuvwxyz0123456789_')='" +
                        nodePrefix + "_name']");
                string toName = nameNode == null || nameNode.Attributes["value"] == null
                    ? ""
                    : nameNode.Attributes["value"].Value;
                XmlNode muNode =
                    camNode.SelectSingleNode(
                        "./attribute[translate(@name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_','abcdefghijklmnopqrstuvwxyz0123456789_')='" +
                        nodePrefix + "_measureunit']");
                string toMu = muNode == null || muNode.Attributes["value"] == null
                    ? ""
                    : muNode.Attributes["value"].Value;
                //toMu = toMu.Replace("[", "");
                //toMu = toMu.Replace("]", "");
                XmlNode usedNode =
                    camNode.SelectSingleNode(
                        "./attribute[translate(@name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_','abcdefghijklmnopqrstuvwxyz0123456789_')='" +
                        nodePrefix + "_isused']");
                bool toUsed = (usedNode != null && usedNode.Attributes["value"] != null &&
                               usedNode.Attributes["value"].Value != "0");
                XmlNode typeNode =
                    camNode.SelectSingleNode(
                        "./attribute[translate(@name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_','abcdefghijklmnopqrstuvwxyz0123456789_')='" +
                        nodePrefix + "_type']");
                byte toType = typeNode == null || typeNode.Attributes["value"] == null
                    ? (byte)0
                    : Convert.ToByte(typeNode.Attributes["value"].Value);
                XmlNode valueNode =
                    camNode.SelectSingleNode(
                        "./attribute[translate(@name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_','abcdefghijklmnopqrstuvwxyz0123456789_')='" +
                        nodePrefix + "_value']");
                string toValue = valueNode == null || valueNode.Attributes["value"] == null
                    ? ""
                    : valueNode.Attributes["value"].Value;
                XmlNode exportNode =
                    camNode.SelectSingleNode(
                        "./attribute[translate(@name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_','abcdefghijklmnopqrstuvwxyz0123456789_')='" +
                        nodePrefix + "_export']");
                int toExport = exportNode == null || exportNode.Attributes["value"] == null
                    ? 0
                    : Convert.ToInt32(exportNode.Attributes["value"].Value);
                if (toExport == 0) {
                    exportNode =
                        camNode.SelectSingleNode(
                            "./attribute[translate(@name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_','abcdefghijklmnopqrstuvwxyz0123456789_')='" +
                            nodePrefix + "_e_is_exported']");
                    toExport = exportNode == null || exportNode.Attributes["value"] == null || string.IsNullOrEmpty(exportNode.Attributes["value"].Value)
                        ? 0
                        : Convert.ToInt32(exportNode.Attributes["value"].Value);
                }
                XmlNode exportNameNode =
                    camNode.SelectSingleNode(
                        "./attribute[translate(@name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_','abcdefghijklmnopqrstuvwxyz0123456789_')='" +
                        nodePrefix + "e_export_name']");
                string toExportName = exportNameNode == null || exportNameNode.Attributes["value"] == null
                    ? ""
                    : exportNameNode.Attributes["value"].Value;
                XmlNode exportMinValueNode =
                    camNode.SelectSingleNode(
                        "./attribute[translate(@name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_','abcdefghijklmnopqrstuvwxyz0123456789_')='" +
                        nodePrefix + "e_min']");
                string toMinValue = exportMinValueNode == null || exportMinValueNode.Attributes["value"] == null
                    ? ""
                    : exportMinValueNode.Attributes["value"].Value;
                XmlNode exportMaxValueNode =
                    camNode.SelectSingleNode(
                        "./attribute[translate(@name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_','abcdefghijklmnopqrstuvwxyz0123456789_')='" +
                        nodePrefix + "e_max']");
                string toMaxValue = exportMaxValueNode == null || exportMaxValueNode.Attributes["value"] == null
                    ? ""
                    : exportMaxValueNode.Attributes["value"].Value;
                XmlNode exportIncrement =
                    camNode.SelectSingleNode(
                        "./attribute[translate(@name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_','abcdefghijklmnopqrstuvwxyz0123456789_')='" +
                        nodePrefix + "_export']");
                double toIncrement = exportIncrement == null || exportIncrement.Attributes["value"] == null
                    ? 0.0
                    : Convert.ToDouble(exportIncrement.Attributes["value"].Value);
                //dispName
                XmlNode exportDispNmae =
                    camNode.SelectSingleNode(
                        "./attribute[translate(@name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_','abcdefghijklmnopqrstuvwxyz0123456789_')='" +
                        nodePrefix + "_dispname']"); //original = 'dispName'
                string toDispName = exportDispNmae == null || exportDispNmae.Attributes["value"] == null
                    ? null
                    : exportDispNmae.Attributes["value"].Value;
                //OutputCondition condition = null;
                //if (toConditionName.Length > 0)
                //    condition = new OutputCondition(toConditionName, toConditionValue);
                //check {...}_
                if (toDispName == null || string.IsNullOrEmpty(toDispName))
                {
                    toDispName = (string)toName.Clone();
                    const string str1 = "{";
                    const string str2 = "}_";
                    if (toDispName.StartsWith(str1) && toDispName.Contains(str2))
                    {
                        int start1 = toDispName.IndexOf(str1);
                        int start2 = toDispName.IndexOf(str2);
                        string rep = toDispName.Substring(start1, start2 - start1 + str2.Length);
                        toDispName = toDispName.Replace(rep, "");
                    }
                }
                var to = new ToolOutput(i.ToString(), toName, toDispName, toMu, toUsed, toExport, GretelSvc.GetType(toType), toValue/*, condition*/, false);

                //OutputCondition condition = null;
                //if (toConditionName.Length > 0)
                //    condition = new OutputCondition(toConditionName, toConditionValue);

                to.MinValue = toMinValue;
                to.MaxValue = toMaxValue;
                to.Increment = toIncrement;
                tool.ToolOutputs.Add(to);
            }
            for (int j = 0; j < 32; j++) {
                string conditionNodePrefix = "tooloutput_condition_" + j.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
                XmlNode conditionNameNode =
                    camNode.SelectSingleNode(
                        "./attribute[translate(@name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_','abcdefghijklmnopqrstuvwxyz0123456789_')='" +
                        conditionNodePrefix + "_name']");
                string toConditionName = conditionNameNode == null || conditionNameNode.Attributes["value"] == null
                    ? ""
                    : conditionNameNode.Attributes["value"].Value;
                XmlNode conditionValueNode =
                    camNode.SelectSingleNode(
                        "./attribute[translate(@name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_','abcdefghijklmnopqrstuvwxyz0123456789_')='" +
                        conditionNodePrefix + "_value']");
                string toConditionValue = conditionValueNode == null || conditionValueNode.Attributes["value"] == null
                    ? ""
                    : conditionValueNode.Attributes["value"].Value;
                if (toConditionName.Length > 0) {
                    var oc = new OutputCondition(j, toConditionName, toConditionValue);
                    tool.ToolOutputsConditions.Add(oc);
                }
            }
        }

        //private static string MySelectSingleNode(List<string> attributes, string attributeName) {

        //    string res = "";
        //    foreach (string attribute in attributes) {
        //        if (attribute.Contains(attributeName) == true) {
        //            string[] attrval = attribute.Split(new string[] { "name=", "value=" }, StringSplitOptions.RemoveEmptyEntries);
        //            res = Regex.Replace(attrval[attrval.Length - 1], "[@,\\.\";'\\\\ ]", string.Empty);
        //            attributes.Remove(attribute);
        //            return res;
        //        }
        //    }
        //    return res;
        //}

        private static void GetParams(List<Parameter> paramColl, XmlNode camNode, string group) {

            foreach (XmlNode node in camNode.ChildNodes) {
                if (node.Attributes != null && node.Attributes["name"] != null && node.Attributes["name"].Value != "") {
                    string name = node.Attributes["name"].Value;
                    string value = "";
                    string parId = "";
                    string displayName = "";
                    int export = -1;
                    string exportName = "";
                    string minValue = "", maxValue = "", description = "";
                    double increment = 0.0;
                    //if ((name.EndsWith("_value") && name.StartsWith("Par_")) || name.StartsWith("ToolOutput_") || name.EndsWith("_display_name") || name.EndsWith("_export") || )
                    //    continue;
                    //if (name.StartsWith("Par_")==true) {
                    if (name.StartsWith("Par_") && name.EndsWith("_name") && !name.EndsWith("_display_name") && !name.EndsWith(" _export_name") &&
                        (name.Length == ("Par_".Length + "_name".Length + 2))) {
                        string name2 = node.Attributes["name"].Value;
                        if (name2 != "" && node.Attributes["value"] != null) {
                            string[] namePart = name2.Split(new[] { '_' });
                            if (namePart.Length >= 1)
                                parId = namePart[1];
                            name = node.Attributes["value"].Value;

                            XmlNode nodeValue = camNode.SelectSingleNode("./attribute[@name='" + name2.Replace("name", "value") + "']");
                            value = (nodeValue == null || nodeValue.Attributes == null ||
                               nodeValue.Attributes["value"] == null)
                           ? ""
                           : nodeValue.Attributes["value"].Value;

                            XmlNode nodeDisplayName = camNode.SelectSingleNode("./attribute[@name='" + name2.Replace("name", "display_name") + "']");
                            displayName = (nodeDisplayName == null || nodeDisplayName.Attributes == null || nodeDisplayName.Attributes["value"] == null)
                                ? ""
                                : nodeDisplayName.Attributes["value"].Value;

                            XmlNode nodeExport = camNode.SelectSingleNode("./attribute[@name='" + name2.Replace("name", "export") + "']");
                            try {
                                export = (nodeExport == null || nodeExport.Attributes == null || nodeExport.Attributes["value"] == null || string.IsNullOrEmpty(nodeExport.Attributes["value"].Value))
                                    ? 0
                                    : int.Parse(nodeExport.Attributes["value"].Value);
                            }
                            catch (Exception ex) {
                                Log.Line(LogLevels.Error, "GretelNodeBase.GetParams", "Export parsing failed: " + ex.Message);
                            }
                            if (export == 0) {
                                nodeExport = camNode.SelectSingleNode("./attribute[@name='" + name2.Replace("name", "e_is_exported") + "']");
                                try {
                                    export = (nodeExport == null || nodeExport.Attributes == null || nodeExport.Attributes["value"] == null || string.IsNullOrEmpty(nodeExport.Attributes["value"].Value))
                                        ? 0
                                        : int.Parse(nodeExport.Attributes["value"].Value);
                                }
                                catch (Exception ex) {
                                    Log.Line(LogLevels.Error, "GretelNodeBase.GetParams", "Export parsing failed: " + ex.Message);
                                }
                            }
                            XmlNode nodeExportName = camNode.SelectSingleNode("./attribute[@name='" + name2.Replace("name", "e_export_name") + "']");
                            try {
                                exportName = (nodeExportName == null || nodeExportName.Attributes == null || nodeExportName.Attributes["value"] == null || string.IsNullOrEmpty(nodeExportName.Attributes["value"].Value))
                                    ? ""
                                    : nodeExportName.Attributes["value"].Value;
                            }
                            catch (Exception ex) {
                                Log.Line(LogLevels.Error, "GretelNodeBase.GetParams", "Export parsing failed: " + ex.Message);
                            }
                            XmlNode nodeMinValue = camNode.SelectSingleNode("./attribute[@name='" + name2.Replace("name", "e_min") + "']");
                            try {
                                minValue = (nodeMinValue == null || nodeMinValue.Attributes == null || nodeMinValue.Attributes["value"] == null || string.IsNullOrEmpty(nodeMinValue.Attributes["value"].Value))
                                    ? ""
                                    : nodeMinValue.Attributes["value"].Value;
                            }
                            catch (Exception ex) {
                                Log.Line(LogLevels.Error, "GretelNodeBase.GetParams", "Export parsing failed: " + ex.Message);
                            }
                            XmlNode nodeMaxValue = camNode.SelectSingleNode("./attribute[@name='" + name2.Replace("name", "e_max") + "']");
                            try {
                                maxValue = (nodeMaxValue == null || nodeMaxValue.Attributes == null || nodeMaxValue.Attributes["value"] == null || string.IsNullOrEmpty(nodeMaxValue.Attributes["value"].Value))
                                    ? ""
                                    : nodeMaxValue.Attributes["value"].Value;
                            }
                            catch (Exception ex) {
                                Log.Line(LogLevels.Error, "GretelNodeBase.GetParams", "Export parsing failed: " + ex.Message);
                            }
                            XmlNode nodeDescription = camNode.SelectSingleNode("./attribute[@name='" + name2.Replace("name", "e_description") + "']");
                            try {
                                description = (nodeDescription == null || nodeDescription.Attributes == null || nodeDescription.Attributes["value"] == null || string.IsNullOrEmpty(nodeDescription.Attributes["value"].Value))
                                    ? ""
                                    : nodeDescription.Attributes["value"].Value;
                            }
                            catch (Exception ex) {
                                Log.Line(LogLevels.Error, "GretelNodeBase.GetParams", "Export parsing failed: " + ex.Message);
                            }
                            XmlNode nodeIncrement = camNode.SelectSingleNode("./attribute[@name='" + name2.Replace("name", "e_increment") + "']");
                            try {
                                increment = (nodeIncrement == null || nodeIncrement.Attributes == null || nodeIncrement.Attributes["value"] == null || string.IsNullOrEmpty(nodeIncrement.Attributes["value"].Value))
                                    ? 0
                                    : double.Parse(nodeIncrement.Attributes["value"].Value, CultureInfo.InvariantCulture);
                            }
                            catch (Exception ex) {
                                Log.Line(LogLevels.Error, "GretelNodeBase.GetParams", "Export parsing failed: " + ex.Message);
                            }
                        }
                    }
                    else {
                        value = node.Attributes["value"] != null ? node.Attributes["value"].Value : "";
                    }
                    if ((name.StartsWith("Par_") == true && name.EndsWith("_name") == true && name.Length == ("Par_".Length + "_name".Length + 2)) ||
                        (name.StartsWith("Par_") == false)) {
                        var param = new Parameter {
                            Id = name,
                            Label = displayName,
                            Value = value,
                            IsVisible = export,
                            Group = @group,
                            ParamId = parId,
                            ExportName = exportName,
                            MinValue = minValue,
                            MaxValue = maxValue,
                            Increment = increment,
                            Description = description
                        };
                        paramColl.Add(param);
                    }
                }
            }
        }

        public static string SaveRecipeV2(NodeRecipe nodeRecipe) {

            var str = new StringBuilder();
            str.AppendLine(@"<?xml version=""1.0""?>");
            str.AppendLine("<map>");
            str.AppendLine(@"   <class type=""Frame Grabbers Setup"" id=""102"">");
            foreach (FrameGrabberRecipe fr in nodeRecipe.FrameGrabbers) {
                str.AppendLine(string.Format(@"     <attribute name=""Board_{0}_isActive"" value=""{1}""/>", fr.BoardId,
                    fr.Active ? "1" : "0"));
                str.AppendLine(string.Format(@"     <attribute name=""Board_{0}_cfgFile"" value=""{1}""/>", fr.BoardId,
                    fr.ConfigFileName));
                str.AppendLine(string.Format(@"     <attribute name=""Board_{0}_Type"" value=""{1}""/>", fr.BoardId,
                    fr.Type));
                str.AppendLine(string.Format(@"     <attribute name=""Board_{0}_Id"" value=""{1}""/>", fr.BoardId,
                    fr.Id));
            }
            str.AppendLine("\t</class>");
            str.AppendLine(@"   <class type=""InspectionSetup"" id=""101"">");
            for (int i = 0; i < 16; i++) {
                StationRecipe st = nodeRecipe.Stations.Find(s => s.Id == i);
                if (st != null) {
                    str.AppendLine(string.Format(@"     <attribute name=""Inspection_{0}_isActive"" value=""{1}""/>",
                        st.Id, st.Enable ? "1" : "0"));
                    str.AppendLine(string.Format(@"     <attribute name=""Inspection_{0}_label"" value=""{1}""/>", st.Id,
                        st.Description));
                }
                else {
                    str.AppendLine(string.Format(@"     <attribute name=""Inspection_{0}_isActive"" value=""{1}""/>", i,
                        "0"));
                    str.AppendLine(string.Format(@"     <attribute name=""Inspection_{0}_label"" value=""{1}""/>", i, ""));
                }
            }
            str.AppendLine("\t</class>");
            foreach (StationRecipe st in nodeRecipe.Stations) {
                if (string.IsNullOrEmpty(st.InspectionType) == true) {    //pier: 23/12/2015 solo per retrocompatibilità
                    str.AppendLine(st.Default
                        ? string.Format(@"<class type=""Inspection Default"" id=""{0}"">", st.Id)
                        : string.Format(@"<class type=""Inspection Particle"" id=""{0}"">", st.Id));
                }
                else
                    str.AppendLine(string.Format(@"<class type=""{0}"" id=""{1}"">", st.InspectionType, st.Id));
                foreach (CameraRecipe c in st.Cameras) {
                    str.AppendLine(string.Format(@"<class type=""CAM"" id=""{0}"">", c.Id));
                    SaveParameter(str, c.AcquisitionParameters, "CAM");
                    str.AppendLine("</class>");
                    str.AppendLine(string.Format(@"<class type=""GRABBER"" id=""{0}"">", c.Id));
                    SaveParameter(str, c.AcquisitionParameters, "GRABBER");
                    str.AppendLine("</class>");
                    str.AppendLine(string.Format(@"<class type=""DIGITIZER"" id=""{0}"">", c.Id));
                    SaveParameter(str, c.DigitizerParameters, "DIGITIZER");
                    str.AppendLine("</class>");
                    str.AppendLine(string.Format(@"<class type=""NEURALNETWORK"" id=""{0}"">", c.Id));
                    SaveParameter(str, c.RecipeAdvancedParameters, "NEURALNETWORK");
                    str.AppendLine("</class>");
                    str.AppendLine(string.Format(@"<class type=""IMAGESTACK"" id=""{0}"">", c.Id));
                    SaveParameter(str, c.RecipeAdvancedParameters, "IMAGESTACK");
                    str.AppendLine("</class>");
                    str.AppendLine(string.Format(@"<class type=""LIGHT SENSOR"" id=""{0}"">", c.Id));
                    SaveParameter(str, c.LightSensor.LightSensorParameters, "");
                    foreach (Shape s in c.LightSensor.Shapes) {
                        str.AppendLine(string.Format(@"<class type=""SHAPE"" id=""{0}"">", s.Id));
                        SaveParameter(str, s.ShapeParameters, "");
                        str.AppendLine("</class>");
                    }
                    str.AppendLine("</class>");
                    for (int i = 0; i < c.Lights.Count; i++) {
                        LightRecipe light = c.Lights.Find(l => l.Id == i);
                        str.AppendLine(string.Format(@"<class type=""STROBO"" id=""{0}"">", light.Id));
                        SaveParameter(str, light.StroboParameters, "");
                        str.AppendLine("</class>");
                    }
                }
                for (int i = 0; i < st.Tools.Count; i++) {
                    Tool tool = st.Tools.Find(t => t.Id == i);
                    str.AppendLine(string.Format(@"<class type=""TOOL"" id=""{0}"">", tool.Id));
                    SaveParameter(str, tool.ToolParameters, "");
                    foreach (ToolOutput t in tool.ToolOutputs) {
                        str.AppendLine(string.Format(@"<attribute name=""ToolOutput_{0}_name"" value=""{1}""/>",
                            t.ParamId.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'), t.Label));
                        str.AppendLine(string.Format(@"<attribute name=""ToolOutput_{0}_MeasureUnit"" value=""{1}""/>",
                            t.ParamId.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'), t.MeasureUnit));
                        str.AppendLine(string.Format(@"<attribute name=""ToolOutput_{0}_IsUsed"" value=""{1}""/>",
                            t.ParamId.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'), t.IsUsed ? "1" : "0"));
                        str.AppendLine(string.Format(@"<attribute name=""ToolOutput_{0}_Type"" value=""{1}""/>",
                            t.ParamId.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'), GretelSvc.GetType(t.ValueType)));
                        str.AppendLine(string.Format(@"<attribute name=""ToolOutput_{0}_value"" value=""{1}""/>",
                            t.ParamId.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'), t.Value));
                        //str.AppendLine(string.Format(@"<attribute name=""ToolOutput_{0}_export"" value=""{1}""/>",
                        //    t.Id.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'), t.IsEditable));
                    }
                    str.AppendLine(string.Format(@"<attribute name=""ToolOutput_CNT"" value=""{0}""/>",
                        tool.ToolOutputs.Count));
                    foreach (OutputCondition oc in tool.ToolOutputsConditions) {
                        str.AppendLine(
                            string.Format(@"<attribute name=""ToolOutput_Condition_{0}_name"" value=""{1}""/>",
                                oc.Id.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'), oc.Name));
                        str.AppendLine(
                            string.Format(@"<attribute name=""ToolOutput_Condition_{0}_value"" value=""{1}""/>",
                                oc.Id.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0'), oc.Value));
                    }

                    foreach (Shape s in tool.Shapes) {
                        str.AppendLine(string.Format(@"<class type=""SHAPE"" id=""{0}"">", s.Id));
                        SaveParameter(str, s.ShapeParameters, "");
                        str.AppendLine("</class>");
                    }
                    str.AppendLine("</class>");
                }
                str.AppendLine("</class>");
            }
            str.AppendLine("</map>");

            return str.ToString();
        }

        private static void SaveParameter(StringBuilder str, List<Parameter> collection, string groupName) {

            foreach (Parameter p in collection.FindAll(p => p.Group == groupName)) {
                if (p.ParamId == "")
                    str.AppendLine(string.Format(@"<attribute name=""{0}"" value=""{1}""/>", p.Id, p.Value));
                else {
                    str.AppendLine(string.Format(@"<attribute name=""Par_{0}_name"" value=""{1}""/>", p.ParamId, p.Id));
                    str.AppendLine(string.Format(@"<attribute name=""Par_{0}_value"" value=""{1}""/>", p.ParamId, p.Value));
                    if (string.IsNullOrEmpty(p.Label) == false) {
                        str.AppendLine(string.Format(@"<attribute name=""Par_{0}_display_name"" value=""{1}""/>", p.ParamId, p.Label));
                    }
                    //if (p.IsEditable > -1) {
                    //    str.AppendLine(string.Format(@"<attribute name=""Par_{0}_export"" value=""{1}""/>", p.ParamId, p.IsEditable));
                    //}
                }
            }
        }

        public override void StartImagesDump(List<StationDumpSettings> statDumpSettingsCollection) {

            _currStatDumpSettingsCollection = statDumpSettingsCollection;
            foreach (StationDumpSettings sds in _currStatDumpSettingsCollection) {
                Log.Line(LogLevels.Pass, "GretelNodeBase.StartImagesDump", Address + " - ST{0}: " +
                    sds.Condition.ToString() + "/" + sds.Type.ToString() + "/{1}", sds.Id, sds.VialsToSave);
            }
            GretelSvc.EnableImagesDump(IdGretel, _currStatDumpSettingsCollection, true);
        }

        public override void StartImagesDump2(List<StationDumpSettings2> statDumpSettingsCollection2)
        {
            _currStatDumpSettingsCollection2 = statDumpSettingsCollection2;
            foreach (StationDumpSettings2 sds in _currStatDumpSettingsCollection2)
            {
                Log.Line(LogLevels.Pass, "GretelNodeBase.StartImagesDump2", Address + " - ST{0}: " +
                    sds.Condition.ToString() + "/" + sds.Type.ToString() + "/{1}", sds.Station, sds.ToSave);
            }
            GretelSvc.EnableImagesDump2(IdGretel, _currStatDumpSettingsCollection2, true);
        }

        public override void StopImagesDump() {

            if (_currStatDumpSettingsCollection != null) {
                foreach (StationDumpSettings sds in _currStatDumpSettingsCollection) {
                    Log.Line(LogLevels.Pass, "GretelNodeBase.StopImagesDump", Address + " - ST{0}: STOP", sds.Id);
                }
            }
            GretelSvc.EnableImagesDump(IdGretel, _currStatDumpSettingsCollection, false);
        }

        public override void StopImagesDump2()
        {
            if (_currStatDumpSettingsCollection2 != null)
                foreach (StationDumpSettings2 sds in _currStatDumpSettingsCollection2)
                    Log.Line(LogLevels.Pass, "GretelNodeBase.StopImagesDump2", Address + " - ST{0}: STOP", sds.Station);
            GretelSvc.EnableImagesDump2(IdGretel, _currStatDumpSettingsCollection2, false);
        }

        public override bool Lock() {
            return true;
            //throw new NotImplementedException();
        }

        public override bool Unlock() {
            return true;
            //throw new NotImplementedException();
        }

        public override NodeRecipe UploadParameters()
        {
            NodeRecipe res = null;
            UploadEv.Reset();
            GretelSvc.UploadReq(IdGretel, "UPLOAD RECIPE REQ\0");
            UploadEv.WaitOne(10000);
            return res;
        }

        public override void RefreshExportedParams() {

            GretelSvc.UploadReq(IdGretel, "UPLOAD EXPORTED PARAMS REQ\0");
        }


        // ACK - NACK
        public override void RecipeUpdatedOK()
        {
            GretelSvc.SendAck(this.IdNode);
        }

        public override void RecipeUpdatedError(string message)
        {
            GretelSvc.SendNAck(this.IdNode, message);
        }
    }

    public class GretelNodeBaseCollection : List<GretelNodeBase> {

        public GretelNodeBase this[GretelNodeBase node] {
            get { return Find(n => n.Address == node.Address); }
        }
    }

    public class ClientData {

        public string LocalIpAddress { get; internal set; }
        public string RemoteName { get; internal set; }
        public string RemoteIpAddress { get; internal set; }
        public int RemotePort { get; internal set; }
    }

    public class ClientDataCollection : List<ClientData> {

        public ClientData this[ClientData node] {
            get { return Find(n => n.RemoteIpAddress == node.RemoteIpAddress); }
        }
    }
}
