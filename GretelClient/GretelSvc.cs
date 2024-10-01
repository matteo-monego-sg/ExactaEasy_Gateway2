//#define HVLD2_PACKET_INJECTION

using DisplayManager;
using ExactaEasyCore;
using ExactaEasyEng;
using SPAMI.Util.Logger;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace GretelClients
{
    public static class GretelSvc {

        readonly static ConcurrentQueue<ClientMessage> ClientMsgQueue = new ConcurrentQueue<ClientMessage>();
        internal const int MaxClientsNum = 16;
        internal const int MaxInspectionsNum = 16;
        internal const int MaxToolsNum = 20; // TOOLS (16) + AGGREGATORS (4)
        internal const int MaxMeasuresNum = 512;    //From Gretel 1.21.8.0  64-->512
        internal const int StringLength = 128;       //pier: ERA 64!!!!! 
        static int _exportParamsSize;
        static int _imageInfoSize;
        public static int protocolVersion = 0;
        static bool _isClosing;
        readonly static bool[] IsClientConnected = new bool[MaxClientsNum];
        static ConnectionData _connData;
        //static List<InspectionResults> tempResultList = new List<InspectionResults>();
        //static ResultInspection tempResult;
        //static byte[] tempResultByteArray;
        static int _nClientRemotes;
        static int _stationsCount;
        internal unsafe delegate void RemoteCallback(void* userData, CommEvent evt, int remoteIndex, int inspectionIndex, CommDataType dataType, DataChunk* incData);
        readonly static unsafe RemoteCallback Callback = MyRemoteCallback;
        static Thread _mexDequeuerTh;
        static WaitHandle[] _mexDequeuerHandles;
        readonly static ManualResetEvent KillEv = new ManualResetEvent(false);
        readonly static ManualResetEvent NewMexEv = new ManualResetEvent(false);

        public static event EventHandler<ClientEventArgs> ClientConnected;
        public static event EventHandler<ClientEventArgs> ClientDisconnected;
        public static event EventHandler<ByteArrayEventArgs> ClientImagesResults;
        public static event EventHandler<GretelInspectionResultsEventArgs> ClientInspectionResults;
        // MM-11/01/2024: HVLD2.0 evo.
        public static event EventHandler<ByteArrayEventArgs> ClientHvldDataResults;
        public static event EventHandler<ClientEventArgs> ClientStringMessage;
        public static event EventHandler<ClientEventArgs> ClientStringWarning;
        public static event EventHandler<ClientEventArgs> ClientDisconnectRemoteDesktop;
        public static event EventHandler<ClientEventArgs> ClientReceivedRecipeAck;
        public static event EventHandler<RecipeEventArgs> ClientRecipeReceived;
        public static event EventHandler<ClientEventArgs> ClientReceivedDumpAck;
        public static event EventHandler<ClientEventArgs> ClientReceivedBootDone;
        public static event EventHandler<ClientEventArgs> ClientReceivedGretelInfo;
        public static event EventHandler<ImagesInfoReceivedEventArgs> ClientImagesInfoReceived;
        public static event EventHandler<ExportParametersEventArgs> ClientExportParametersReceived;
        public static event EventHandler<CollectorInfoEventArgs> ClientCollectorInfo;
        public static event EventHandler<ClientEventArgs> ClientAuditMessage;

        public static int ClientsConnected { get; private set; }

        static GretelSvc() {
            //InitPaletteGrayscale();
            //var resultInspectionSize = Marshal.SizeOf(tempResult);
            //tempResultByteArray = new byte[resultInspectionSize];
            //tempResultList.Clear();
            //for (int ss = 0; ss < MaxInspectionsNum; ss++) {
            //    ToolResultsCollection trc = new ToolResultsCollection();
            //    for (int tt = 0; tt < MaxToolsNum; tt++) {
            //        MeasureResultsCollection mrc = new MeasureResultsCollection();
            //        for (int mm = 0; mm < MaxMeasuresNum; mm++)
            //            mrc.Add(new MeasureResults(mm, "", "", false, false, MeasureTypeEnum.STRING, ""));
            //        trc.Add(new ToolResults(tt, false, false, false, mrc));
            //    }
            //    tempResultList.Add(new InspectionResults(-1, ss, -1, 0, false, false, -1, trc));
            //}
            ExportParameter exportParameters = new ExportParameter();
            _exportParamsSize = Marshal.SizeOf(exportParameters);
            InspectionImageSizeAndType imageInfoTemp = new InspectionImageSizeAndType();
            _imageInfoSize = Marshal.SizeOf(imageInfoTemp);
        }

#if HVLD2_PACKET_INJECTION
        /// <summary>
        /// WARNING: TEST FUNCTION / TO BE REMOVED.
        /// </summary>
        public unsafe static void InjectHvldMessage(byte[] dumpData)
        {
            fixed (byte* pData = dumpData)
            {
                DataChunk hlvdData;
                hlvdData.Data = pData;
                hlvdData.DataLen = dumpData.Length;
                ClientMsgQueue.Enqueue(new ClientMessage(CommEvent.ENewdata, 0, 0, CommDataType.CHvldResults, &hlvdData));
            }
        }
#endif

        public static void StartServer(ClientDataCollection clientDataCollection, int stationsCount) {

            ClientsConnected = 0;
            _stationsCount = stationsCount;
            KillEv.Reset();
            if (_mexDequeuerHandles == null) {
                _mexDequeuerHandles = new WaitHandle[2];
                _mexDequeuerHandles[0] = KillEv;
                _mexDequeuerHandles[1] = NewMexEv;
            }
            if (_mexDequeuerTh == null || !_mexDequeuerTh.IsAlive) {
                _mexDequeuerTh = new Thread(MexDequeuer) {
                    Name = "Gretel Messages Dequeuer Thread"
                };
                _mexDequeuerTh.Start();
                _mexDequeuerTh.Priority = ThreadPriority.AboveNormal;
            }

            if (clientDataCollection.Count > MaxClientsNum)
                throw new GretelSvcException("Too many clients! " + clientDataCollection.Count.ToString(CultureInfo.InvariantCulture) + ">" + MaxClientsNum.ToString(CultureInfo.InvariantCulture));

            _connData.LocalIpAddress = new string[MaxClientsNum];
            _connData.RemoteName = new string[MaxClientsNum];
            _connData.RemoteIpAddress = new string[MaxClientsNum];
            _connData.RemotePort = new int[MaxClientsNum];
            for (int i = 0; i < clientDataCollection.Count; i++) {
                var clientData = clientDataCollection[i];
                _connData.LocalIpAddress[i] = clientData.LocalIpAddress;
                _connData.RemoteName[i] = clientData.RemoteName;
                _connData.RemoteIpAddress[i] = clientData.RemoteIpAddress;
                _connData.RemotePort[i] = clientData.RemotePort;
            }
            _nClientRemotes = clientDataCollection.Count;
            Log.Line(LogLevels.Debug, "GretelSvc.StartServer", "DllInitCommServer...");
            var res = DllInitCommServer(ref _connData, _nClientRemotes);
            Log.Line(LogLevels.Debug, "GretelSvc.StartServer", "DllInitCommServer res = " + res.ToString());
            if (res != true) {
                throw new GretelSvcException("Error initialising server!");
            }

            unsafe {
                Log.Line(LogLevels.Debug, "GretelSvc.StartServer", "DllStartCommServer...");
                res = DllStartCommServer(Callback, null);
                Log.Line(LogLevels.Debug, "GretelSvc.StartServer", "DllStartCommServer res = " + res.ToString());
            }
            if (res != true) {
                throw new GretelSvcException("Error starting server!");
            }
        }

        public static void StopServer() {

            _isClosing = true;
            KillEv.Set();
            if (_mexDequeuerTh != null && _mexDequeuerTh.IsAlive) {
                _mexDequeuerTh.Join(2000);
            }
            Log.Line(LogLevels.Debug, "GretelSvc.StopServer", "DllCloseCommServer...");
            var res = DllCloseCommServer();
            Log.Line(LogLevels.Debug, "GretelSvc.StopServer", "DllCloseCommServer res = " + res.ToString());
            if (res != true) {
                throw new GretelSvcException("Error closing server");
            }
            Array.Clear(IsClientConnected, 0, IsClientConnected.Length);
            _isClosing = false;
            ClientsConnected = 0;
        }

        static void MexDequeuer() {

            //Stopwatch timeMon = new Stopwatch();
            //List<long> meanTimeMon = new List<long>();
            int queueFull = 0;
            //DisplayManager.VisionSystemManager.RescanAvailable.WaitOne();   //aspetto che sistema sia inizializzato
            while (true) {

                if (ClientMsgQueue.Count > 0)
                    NewMexEv.Set();

                var retVal = WaitHandle.WaitAny(_mexDequeuerHandles, 1);
                if (retVal == 0) break; //killEv
                NewMexEv.Reset();

                ClientMessage currMessage;
                if (ClientMsgQueue == null || !ClientMsgQueue.TryDequeue(out currMessage))
                    continue;
                //timeMon.Restart();
                if (ClientMsgQueue.Count > 100) {//Math.Max(5, _stationsCount * 2))
                    Log.Line(LogLevels.Debug, "GretelSvc.MexDequeuer", "QUEUE SIZE: " + ClientMsgQueue.Count);
                    queueFull++;
                }
                else {
                    queueFull = Math.Max(0, queueFull - 1);
                }
                var clientIp = _connData.RemoteIpAddress[currMessage.ClientIndex];
                switch (currMessage.Evt) 
                {
                    case CommEvent.EConnected:
                        if (IsClientConnected[currMessage.ClientIndex] == false) ClientsConnected++;
                        IsClientConnected[currMessage.ClientIndex] = true;
                        OnClientConnected(typeof(GretelSvc), new ClientEventArgs(clientIp, currMessage.ClientIndex, ""));
                        UploadReq(currMessage.ClientIndex, "GET GRETEL INFO REQ");
                        break;
                    case CommEvent.EDisconnected:
                        if (IsClientConnected[currMessage.ClientIndex]) ClientsConnected--;
                        IsClientConnected[currMessage.ClientIndex] = false;
                        OnClientDisconnected(typeof(GretelSvc), new ClientEventArgs(clientIp, currMessage.ClientIndex, ""));
                        break;
                    case CommEvent.EDisconnectedreset:
                        if (IsClientConnected[currMessage.ClientIndex]) ClientsConnected--;
                        IsClientConnected[currMessage.ClientIndex] = false;
                        OnClientDisconnected(typeof(GretelSvc), new ClientEventArgs(clientIp, currMessage.ClientIndex, "abruptly"));
                        break;
                    case CommEvent.EDisconnectednoka:
                        if (IsClientConnected[currMessage.ClientIndex]) ClientsConnected--;
                        IsClientConnected[currMessage.ClientIndex] = false;
                        OnClientDisconnected(typeof(GretelSvc), new ClientEventArgs(clientIp, currMessage.ClientIndex, "missing keep alive"));
                        break;
                    case CommEvent.ESocketerror:
                        if (currMessage.DataType == CommDataType.CommString) {
                            string message = Encoding.Default.GetString(currMessage.DataArray).TrimEnd(new[] { '\0' });
                            //Log.Line(LogLevels.Debug, "GretelSvc.mexDequeuer", "Error: " + message);
                            OnClientStringWarning(typeof(GretelSvc), new ClientEventArgs(clientIp, currMessage.ClientIndex, message));
                        }
                        break;
                    case CommEvent.ENewdata:
                    
                        switch (currMessage.DataType)
                        {
                            // MM-11/01/2024: HVLD2.0 evo.
                            case CommDataType.CHvldResults:
                                OnClientHvldSignalData(typeof(GretelSvc), new ByteArrayEventArgs(clientIp, currMessage.ClientIndex, currMessage.InspectionIndex, currMessage.DataArray));
                                break;
                            case CommDataType.CModuleresults:
                            //if (DecodeMeasuresResultsData(currMessage.ClientIndex, currMessage.DataArray)) {
                            //    OnClientInspectionResults(typeof(GretelSvc), new GretelInspectionResultsEventArgs(clientIp, currMessage.ClientIndex, tempResultList));
                            //}
                            if (queueFull == 0)
                            {
                                var inspectionResults = _DecodeMeasuresResultsData(currMessage.ClientIndex, currMessage.DataArray);
                                OnClientInspectionResults(typeof(GretelSvc), new GretelInspectionResultsEventArgs(clientIp, currMessage.ClientIndex, inspectionResults));
                            }
                            break;    
                            case CommDataType.CRecipe:
                                var newRecipeToSave = new Recipe(currMessage.DataArray);
                                OnClientRecipeReceived(typeof(GretelSvc), new RecipeEventArgs(clientIp, currMessage.ClientIndex, newRecipeToSave.IsNew, newRecipeToSave.RecipeName, newRecipeToSave.RecipeV2, true));
                                break;
                            case CommDataType.CInspresults:
                                //var res = DecodeImagesResultsData(currMessage.DataArray);
                                if (queueFull == 0) {
                                    OnClientImagesResults(typeof(GretelSvc), new ByteArrayEventArgs(clientIp, currMessage.ClientIndex, currMessage.InspectionIndex, currMessage.DataArray));
                                }
                                //res.Image.Save(@"C:\image.bmp");
                                //res.Thumbnail.Save(@"C:\thumbnail.bmp");
                                break;
                            case CommDataType.CString:
                                var tempMex = Encoding.Default.GetString(currMessage.DataArray);//.TrimEnd(new char[] { '\0' });
                                var message = tempMex.Substring(0, tempMex.IndexOf('\0'));
                                ParseMessage(clientIp, currMessage.ClientIndex, message);
                                break;

                            //case CommDataType.CCollectorStatus:
                            //    var collectorStatus = _DecodeCollectorInfo(currMessage.ClientIndex, currMessage.DataArray);
                            //    OnClientCollectorInfo(typeof(GretelSvc), new CollectorInfoEventArgs(clientIp, currMessage.ClientIndex, collectorStatus));
                            //    break;
                            case CommDataType.CImageSizeAndType:
                                InspectionImageSizeAndType[] inspectionImageInfo = DecodeImageInfo(currMessage.DataArray);
                                OnClientImagesInfoReceived(typeof(GretelSvc), new ImagesInfoReceivedEventArgs(clientIp, currMessage.ClientIndex, inspectionImageInfo));
                                break;
                            case CommDataType.CExportParameters:
                                if ((currMessage.DataArray.Length % _exportParamsSize) != 0)
                                    Log.Line(LogLevels.Error, "GretelSvc.MexDequeuer", "Export parameters size unexpected: " + currMessage.DataArray.Length + "(" + _exportParamsSize + ")");
                                else {
                                    //System.IO.File.WriteAllBytes(@"C:\expParamIN.bin", currMessage.DataArray);
                                    ExportParameter[] exportParameters = DecodeExportParameters(currMessage.DataArray);
                                    OnClientExportParametersReceived(typeof(GretelSvc), new ExportParametersEventArgs(clientIp, currMessage.ClientIndex, exportParameters));
                                }
                                break;
                            case CommDataType.CAppliedRecipeFile:
                                var newRecipeApplied = new Recipe(currMessage.DataArray);
                                OnClientRecipeReceived(typeof(GretelSvc), new RecipeEventArgs(clientIp, currMessage.ClientIndex, newRecipeApplied.IsNew, newRecipeApplied.RecipeName, newRecipeApplied.RecipeV2, false));
                                break;
                            case CommDataType.CAuditTrailMessage:
                                var auditMex = Encoding.Default.GetString(currMessage.DataArray);//.TrimEnd(new char[] { '\0' });
                                auditMex = auditMex.Substring(0, auditMex.IndexOf('\0'));
                                OnClientAuditMessage(typeof(GretelSvc), new ClientEventArgs(clientIp, currMessage.ClientIndex, auditMex));
                                break;
                            default:
                                Log.Line(LogLevels.Warning, "GretelSvc.MexDequeuer", "Unknown new data. Length: " + currMessage.DataArray.Length);
                                break;
                        }
                        break;
 
                    default:
                        IsClientConnected[currMessage.ClientIndex] = false;
                        break;
                }
                //Thread.Sleep(1);
                //timeMon.Stop();
                //meanTimeMon.Add(timeMon.ElapsedMilliseconds);
                //if (meanTimeMon.Count > 100) meanTimeMon.RemoveAt(0);
                //if (timeMon.ElapsedMilliseconds > 5)
                //    Log.Line(LogLevels.Debug, currMessage.Evt.ToString() + "." + currMessage.DataType.ToString(), "GRETEL MEX DEQUEUE TIME = {0}ms MEAN = {1}ms MAX = {2}ms", timeMon.ElapsedMilliseconds, meanTimeMon.Average(), meanTimeMon.Max());
            }
            Log.Line(LogLevels.Pass, "GretelSvc.MexDequeuer", "Exiting MexDequeuer Thread");
        }

        static void ParseMessage(string ip, int id, string message) {
            if (message.Contains("DISCONNECT RD!"))
                OnClientDisconnectRemoteDesktop(typeof(GretelSvc), new ClientEventArgs(ip, id, message));
            else if (message.Contains("RECIPE RECEIVED"))
                OnClientReceivedRecipeAck(typeof(GretelSvc), new ClientEventArgs(ip, id, message));
            else if (message.Contains("COLLECTOR STATUS"))
                OnClientReceivedDumpAck(typeof(GretelSvc), new ClientEventArgs(ip, id, message));
            else if (message.Contains("BOOT DONE"))
                OnClientReceivedBootDone(typeof(GretelSvc), new ClientEventArgs(ip, id, message));
            else if (message.Contains("GRETEL VERSION"))
                OnClientReceivedGretelInfo(typeof(GretelSvc), new ClientEventArgs(ip, id, message));
            else
                OnClientStringMessage(typeof(GretelSvc), new ClientEventArgs(ip, id, message));
        }

        internal static unsafe void ActivateInspection(int clientId, int inspectionId, bool activate) {

            DataChunk outData;
            var value = activate ? 1 : 0;
            outData.Data = (byte*)&value;
            outData.DataLen = sizeof(int);

            if (IsClientConnected[clientId]) {
                Log.Line(LogLevels.Debug, "GretelSvc.ActivateInspection", "Calling DllCommServerSendData...");
                var res = DllCommServerSendData(clientId, inspectionId, CommDataType.SSetinspdisplay, ref outData);
                Log.Line(LogLevels.Debug, "GretelSvc.ActivateInspection", "DllCommServerSendData res = " + res.ToString());
                if (res) return;
                if (activate) throw new GretelSvcException("Error activating inspection");
                throw new GretelSvcException("Error deactivating inspection");
            }
            else
                throw new GretelSvcException("Client " + clientId.ToString(CultureInfo.InvariantCulture) + " not connected");
        }

        public static unsafe void SendRecipe(int clientId, string recipeName, string recipe) {

            bool res;
            if (recipe == null)
                throw new GretelSvcException("Recipe null");
            if (!IsClientConnected[clientId]) {
                Log.Line(LogLevels.Warning, "GretelSvc.SendRecipe", "Client not connected");
                return;
            }
            var binaryRecipe = new Recipe(recipeName, recipe);
            var arr = binaryRecipe.GetBytes();
            fixed (byte* pData = arr) {
                DataChunk outData;
                outData.Data = pData;
                outData.DataLen = arr.Length;
                Log.Line(LogLevels.Debug, "GretelSvc.SendRecipe", "DllCommServerSendData...");
                res = DllCommServerSendData(clientId, -1, CommDataType.SRecipe, ref outData);
                Log.Line(LogLevels.Debug, "GretelSvc.SendRecipe", "DllCommServerSendData res = " + res.ToString());
            }
            if (!res)
                throw new GretelSvcException("Error sending recipe! Error code: " + false.ToString());
        }

        internal static unsafe void SendParameters(int clientId, ExportParameter[] exportParameters) {

            bool res = false;
            //if (recipe == null)
            //    throw new GretelSvcException("Recipe null");
            if (!IsClientConnected[clientId]) {
                Log.Line(LogLevels.Warning, "GretelSvc.SendRecipe", "Client not connected");
                return;
            }
            byte[] buffer = new byte[_exportParamsSize * exportParameters.Length];
            for (int iEP = 0; iEP < exportParameters.Length; iEP++) {
                byte[] tempBuffer = exportParameters[iEP].GetBytes();
                Array.Copy(tempBuffer, 0, buffer, iEP * _exportParamsSize, _exportParamsSize);
            }
            //System.IO.File.WriteAllBytes(@"C:\expParamOUT.bin", buffer);
            fixed (byte* pData = buffer) {
                DataChunk outData;
                outData.Data = pData;
                outData.DataLen = buffer.Length;
                Log.Line(LogLevels.Debug, "GretelSvc.SendParameters", "DllCommServerSendData...");
                res = DllCommServerSendData(clientId, -1, CommDataType.SSetExportedParam, ref outData);
                Log.Line(LogLevels.Debug, "GretelSvc.SendParameters", "DllCommServerSendData res = " + res.ToString());
            }
            if (res == false)
                throw new GretelSvcException("Error sending parameters! Error code: " + false.ToString());
        }

        public static unsafe void SendUserLevel(int clientId, UserLevelEnum userLevel) {

            DataChunk outData;
            var value = (byte)userLevel;
            outData.Data = &value;
            outData.DataLen = sizeof(byte);
            if (!IsClientConnected[clientId])
                return;
            Log.Line(LogLevels.Debug, "GretelSvc.SendUserLevel", "DllCommServerSendData...");
            var res = DllCommServerSendData(clientId, -1, CommDataType.SUser, ref outData);
            Log.Line(LogLevels.Debug, "GretelSvc.SendUserLevel", "DllCommServerSendData res = " + res.ToString());
            if (!res)
                throw new GretelSvcException("Error sending user level! Error code: " + false.ToString());
        }

        public static unsafe void SendInspectionViewId(int clientId, int inspectionId) {

            DataChunk outData;
            var value = (byte)inspectionId;
            outData.Data = &value;
            outData.DataLen = sizeof(byte);
            if (!IsClientConnected[clientId])
                return;
            Log.Line(LogLevels.Debug, "GretelSvc.SendInspectionViewId", "DllCommServerSendData...");
            var res = DllCommServerSendData(clientId, -1, CommDataType.SInspectionViewId, ref outData);
            Log.Line(LogLevels.Debug, "GretelSvc.SendInspectionViewId", "DllCommServerSendData res = " + res.ToString());
            if (!res)
                throw new GretelSvcException("Error sending inspection view ID! Error code: " + false.ToString());
        }

        public static unsafe void SendAck(int clientId) {

            DataChunk outData;
            var value = 0;
            outData.Data = (byte*)&value;
            outData.DataLen = sizeof(int);
            if (!IsClientConnected[clientId])
                return;
            Log.Line(LogLevels.Debug, "GretelSvc.SendAck", "DllCommServerSendData...");
            var res = DllCommServerSendData(clientId, -1, CommDataType.CommAck, ref outData);
            Log.Line(LogLevels.Debug, "GretelSvc.SendAck", "DllCommServerSendData res = " + res.ToString());
            if (!res)
                throw new GretelSvcException("Error sending ack! Error code: " + false.ToString());
        }

        public static unsafe void SendNAck(int clientId, string mexError)
        {
            bool res;
            if (!IsClientConnected[clientId])
                return;
            byte[] bStr = Encoding.ASCII.GetBytes(mexError);
            fixed (byte* pData = bStr)
            {
                DataChunk outData;
                outData.Data = pData;
                outData.DataLen = bStr.Length;
                Log.Line(LogLevels.Debug, "GretelSvc.SendNAck", "DllCommServerSendData...");
                res = DllCommServerSendData(clientId, -1, CommDataType.CommNAck, ref outData);
                Log.Line(LogLevels.Debug, "GretelSvc.SendNAck", "DllCommServerSendData res = " + res.ToString());
            }
            if (!res)
                throw new GretelSvcException("Error sending NAck! Error code: " + false.ToString());
        }

        public static unsafe void EnableImagesDump(int clientId, List<StationDumpSettings> statDumpSet, bool enable) {

            bool res;
            if (!IsClientConnected[clientId]) {
                Log.Line(LogLevels.Warning, "GretelSvc.EnableImagesDump", "Client not connected");
                return;
            }
            var cp = new CollectParameters(enable, statDumpSet);
            var arr = cp.GetBytes();
            fixed (byte* pData = arr) {
                DataChunk outData;
                outData.Data = pData;
                outData.DataLen = arr.Length;
                Log.Line(LogLevels.Debug, "GretelSvc.EnableImagesDump", "DllCommServerSendData...");
                res = DllCommServerSendData(clientId, -1, CommDataType.SSetcollectparams, ref outData);
                Log.Line(LogLevels.Debug, "GretelSvc.EnableImagesDump", "DllCommServerSendData res = " + res.ToString());
            }
            if (!res)
                throw new GretelSvcException("Error sending collecting parameters! Error code: " + false.ToString());

        }

        public static unsafe void EnableImagesDump2(int clientId, List<StationDumpSettings2> statDumpSet, bool enable)
        {

            bool res;
            if (!IsClientConnected[clientId])
            {
                Log.Line(LogLevels.Warning, "GretelSvc.EnableImagesDump2", "Client not connected");
                return;
            }
            foreach (StationDumpSettings2 set in statDumpSet)
                set.Enable = enable;
            string jsonStr = DumpImagesUserSettings2.GetJSONStr(statDumpSet);
            var arr = Encoding.UTF8.GetBytes(jsonStr);
            fixed (byte* pData = arr)
            {
                DataChunk outData;
                outData.Data = pData;
                outData.DataLen = arr.Length;
                Log.Line(LogLevels.Debug, "GretelSvc.EnableImagesDump2", "DllCommServerSendData...");
                res = DllCommServerSendData(clientId, -1, CommDataType.SSetcollectparams2, ref outData);
                Log.Line(LogLevels.Debug, "GretelSvc.EnableImagesDump2", "DllCommServerSendData res = " + res.ToString());
            }
            if (!res)
                throw new GretelSvcException("Error sending collecting parameters 2! Error code: " + false.ToString());

        }

        public static unsafe void GetImageInfo(int clientId) {

            bool res;
            if (!IsClientConnected[clientId]) {
                Log.Line(LogLevels.Warning, "GretelSvc.GetImageInfo", "Client not connected");
                return;
            }
            string cmdStr = "GET IMG SIZE AND TYPE";
            byte[] tmpCmdData = Encoding.ASCII.GetBytes(cmdStr);
            fixed (byte* pData = tmpCmdData) {
                DataChunk outData;
                outData.Data = pData;
                outData.DataLen = tmpCmdData.Length;
                Log.Line(LogLevels.Debug, "GretelSvc.GetImageInfo", "DllCommServerSendData...");
                res = DllCommServerSendData(clientId, -1, CommDataType.SString, ref outData);
                Log.Line(LogLevels.Debug, "GretelSvc.GetImageInfo", "DllCommServerSendData res = " + res.ToString());
            }
            if (!res)
                throw new GretelSvcException("Error sending collecting parameters! Error code: " + false.ToString());

        }

        public static unsafe void UploadReq(int clientId, string cmdStr) {
            
            bool res;
            if (!IsClientConnected[clientId]) {
                Log.Line(LogLevels.Warning, "GretelSvc.GetImageInfo", "Client not connected");
                return;
            }
            byte[] tmpCmdData = Encoding.ASCII.GetBytes(cmdStr);
            fixed (byte* pData = tmpCmdData) {
                DataChunk outData;
                outData.Data = pData;
                outData.DataLen = tmpCmdData.Length;
                Log.Line(LogLevels.Debug, "GretelSvc.GetImageInfo", "DllCommServerSendData...");
                res = DllCommServerSendData(clientId, -1, CommDataType.SString, ref outData);
                Log.Line(LogLevels.Debug, "GretelSvc.GetImageInfo", "DllCommServerSendData res = " + res.ToString());
            }
            if (!res)
                throw new GretelSvcException("Error sending collecting parameters! Error code: " + false.ToString());

        }

        //internal static bool DecodeMeasuresResultsData(int clientId, byte[] dataToDecode) {

        //    Stopwatch aaa = new Stopwatch(); 
        //    //var resultInspectionSize = Marshal.SizeOf(tempResult);
        //    //tempResultByteArray = new byte[resultInspectionSize];
        //    var nInspectionResultsNum = dataToDecode.Length / tempResultByteArray.Length;
        //    if (nInspectionResultsNum > MaxInspectionsNum) {
        //        Log.Line(LogLevels.Warning, "GretelSvc.DecodeMeasuresResultsData", "Inspections results count: {0}. Max inspections allowed: {1}", nInspectionResultsNum, MaxInspectionsNum);
        //        return false;
        //    }
        //    for (int ii = 0; ii < nInspectionResultsNum; ii++) {
        //        aaa.Restart();
        //        Array.Clear(tempResultByteArray, 0, tempResultByteArray.Length);
        //        Array.Copy(dataToDecode, ii * tempResultByteArray.Length, tempResultByteArray, 0, tempResultByteArray.Length);
        //        tempResult = ByteArrayToStructure<ResultInspection>(tempResultByteArray);
        //        aaa.Stop();
        //        Log.Line(LogLevels.Debug, "", "*** MONITOR *** Byte copy time: {0}ms", aaa.ElapsedMilliseconds);
        //        if (tempResult.IsActive == 0)
        //            continue;
        //        if (tempResult.ResTools.Length > MaxToolsNum) {
        //            Log.Line(LogLevels.Warning, "GretelSvc.DecodeMeasuresResultsData", "Tools results count: {0}. Max tools per inspection allowed: {1}", tempResult.ResTools.Length, MaxToolsNum);
        //            return false;
        //        }
        //        for (int it = 0; it < tempResult.ResTools.Length; it++) {
        //            if (tempResult.ResTools[it].ResMeasures.Length > MaxMeasuresNum) {
        //                Log.Line(LogLevels.Warning, "GretelSvc.DecodeMeasuresResultsData", "Measures results count: {0}. Max measures per tool allowed: {1}", tempResult.ResTools[it].ResMeasures.Length, MaxMeasuresNum);
        //                return false;
        //            }
        //            for (int im = 0; im < tempResult.ResTools[it].ResMeasures.Length; im++) {
        //                var measTypeEnum = MeasureTypeEnum.STRING;
        //                var value = "";
        //                switch (tempResult.ResTools[it].ResMeasures[im].Type) {
        //                    case 0:
        //                        measTypeEnum = MeasureTypeEnum.BOOL;
        //                        bool bValue = (tempResult.ResTools[it].ResMeasures[im].MeasureValue[0] != 0);
        //                        value = bValue.ToString();
        //                        break;
        //                    case 1:
        //                        measTypeEnum = MeasureTypeEnum.INT;
        //                        int iValue = BitConverter.ToInt32(tempResult.ResTools[it].ResMeasures[im].MeasureValue, 0);
        //                        value = iValue.ToString(CultureInfo.InvariantCulture);
        //                        break;
        //                    case 2:
        //                        measTypeEnum = MeasureTypeEnum.DOUBLE;
        //                        double dValue = BitConverter.ToDouble(tempResult.ResTools[it].ResMeasures[im].MeasureValue, 0);
        //                        value = dValue.ToString(CultureInfo.InvariantCulture);
        //                        break;
        //                    case 3:
        //                        measTypeEnum = MeasureTypeEnum.STRING;
        //                        value = Encoding.ASCII.GetString(tempResult.ResTools[it].ResMeasures[im].MeasureValue);
        //                        break;
        //                }
        //                var name = new string(tempResult.ResTools[it].ResMeasures[im].Name);
        //                name = name.Substring(0, name.IndexOf("\0", StringComparison.Ordinal));
        //                var um = new string(tempResult.ResTools[it].ResMeasures[im].MeasureUnit);
        //                um = um.Substring(0, um.IndexOf("\0", StringComparison.Ordinal));
        //                tempResultList[ii].ResToolCollection[it].ResMeasureCollection[im].MeasureName = name;
        //                tempResultList[ii].ResToolCollection[it].ResMeasureCollection[im].MeasureUnit = um;
        //                tempResultList[ii].ResToolCollection[it].ResMeasureCollection[im].IsOk = (tempResult.ResTools[it].ResMeasures[im].IsOk != 0);
        //                tempResultList[ii].ResToolCollection[it].ResMeasureCollection[im].IsUsed = (tempResult.ResTools[it].ResMeasures[im].IsUsed != 0);
        //                tempResultList[ii].ResToolCollection[it].ResMeasureCollection[im].MeasureType = measTypeEnum;
        //                tempResultList[ii].ResToolCollection[it].ResMeasureCollection[im].MeasureValue = value;
        //            }
        //            tempResultList[ii].ResToolCollection[it].ToolId = it;
        //            tempResultList[ii].ResToolCollection[it].IsActive = (tempResult.ResTools[it].IsActive != 0);
        //            tempResultList[ii].ResToolCollection[it].IsReject = (tempResult.ResTools[it].IsReject != 0);
        //            tempResultList[ii].ResToolCollection[it].IsDisplayed = (tempResult.ResTools[it].IsDisplayed != 0);
        //        }
        //        tempResultList[ii].NodeId = clientId;
        //        tempResultList[ii].InspectionId = tempResult.InspectionId;
        //        tempResultList[ii].SpindleId = tempResult.SpindleId;
        //        tempResultList[ii].VialId = tempResult.VialId;
        //        tempResultList[ii].IsActive = (tempResult.IsActive != 0);
        //        tempResultList[ii].IsReject = (tempResult.IsReject != 0);
        //        tempResultList[ii].RejectionCause = tempResult.DefectCode;
        //    }
        //    return true;
        //}

        unsafe internal static List<CollectorInfo> _DecodeCollectorInfo(int clientId, byte[] dataToDecode) {

            var collectorInfoTemp = new CollectorInfoInspection();
            var collectorInfoSize = Marshal.SizeOf(collectorInfoTemp);
            var singleCollectorInfoInspection = new byte[collectorInfoSize];
            var nCollectorInfoNum = dataToDecode.Length / collectorInfoSize;
            var retList = new List<CollectorInfo>();
            for (int i = 0; i < nCollectorInfoNum; i++) {
                Array.Copy(dataToDecode, i * collectorInfoSize, singleCollectorInfoInspection, 0, collectorInfoSize);
                collectorInfoTemp = ByteArrayToStructure<CollectorInfoInspection>(singleCollectorInfoInspection);
                var newItem = new CollectorInfo(clientId, collectorInfoTemp.inspectionId, collectorInfoTemp.imagesSaved);
                retList.Add(newItem);
            }
            return retList;
        }

        internal static ExportParameter[] DecodeExportParameters(byte[] data) {

            int exportParametersNum = data.Length / _exportParamsSize;
            ExportParameter[] res = new ExportParameter[exportParametersNum];
            byte[] tmpExportParametersData = new byte[_exportParamsSize];
            for (int i = 0; i < exportParametersNum; i++) {
                Array.Clear(tmpExportParametersData, 0, _exportParamsSize);
                Array.Copy(data, i * _exportParamsSize, tmpExportParametersData, 0, _exportParamsSize);
                res[i] = ByteArrayToStructure<ExportParameter>(tmpExportParametersData);
            }
            return res;
        }

        internal static List<InspectionResults> _DecodeMeasuresResultsData(int clientId, byte[] dataToDecode) {
            if (protocolVersion == 0)
            {
                var resInspTemp = new ResultInspectionV0();
                var resultInspectionSize = Marshal.SizeOf(resInspTemp);
                var singleResultInspection = new byte[resultInspectionSize];
                var nInspectionResultsNum = dataToDecode.Length / resultInspectionSize;
                var retList = new List<InspectionResults>();
                for (int ii = 0; ii < nInspectionResultsNum; ii++)
                {
                    Array.Clear(singleResultInspection, 0, singleResultInspection.Length);
                    Array.Copy(dataToDecode, ii * resultInspectionSize, singleResultInspection, 0, resultInspectionSize);
                    resInspTemp = ByteArrayToStructure<ResultInspectionV0>(singleResultInspection);
                    if (resInspTemp.IsActive == 0)
                        continue;
                    var toolsResults = new ToolResultsCollection();
                    for (int it = 0; it < resInspTemp.ResTools.Length; it++)
                    {
                        ResultTool currResTool = resInspTemp.ResTools[it];
                        var measuresResults = new MeasureResultsCollection();
                        for (int im = 0; im < currResTool.ResMeasures.Length; im++)
                        {
                            var currResMeasure = currResTool.ResMeasures[im];
                            var measTypeEnum = MeasureTypeEnum.STRING;
                            var value = "";
                            switch (currResMeasure.Type)
                            {
                                case 0:
                                    measTypeEnum = MeasureTypeEnum.BOOL;
                                    bool bValue = currResMeasure.MeasureValue[0] != 0;
                                    value = bValue.ToString();
                                    break;
                                case 1:
                                    measTypeEnum = MeasureTypeEnum.INT;
                                    int iValue = BitConverter.ToInt32(currResMeasure.MeasureValue, 0);
                                    value = iValue.ToString(CultureInfo.InvariantCulture);
                                    break;
                                case 2:
                                    measTypeEnum = MeasureTypeEnum.DOUBLE;
                                    double dValue = BitConverter.ToDouble(currResMeasure.MeasureValue, 0);
                                    value = dValue.ToString(CultureInfo.InvariantCulture);
                                    break;
                                case 3:
                                    measTypeEnum = MeasureTypeEnum.STRING;
                                    value = Encoding.ASCII.GetString(currResMeasure.MeasureValue);
                                    break;
                            }
                            //string value = Encoding.ASCII.GetString(currResMeasure.measureValue).TrimEnd(new char[] { '\0' });
                            var name = new string(currResMeasure.Name);
                            name = name.Substring(0, name.IndexOf("\0", StringComparison.Ordinal));
                            var um = new string(currResMeasure.MeasureUnit);
                            um = um.Substring(0, um.IndexOf("\0", StringComparison.Ordinal));
                            measuresResults.Add(new MeasureResults(im, name, um, currResMeasure.IsOk != 0, currResMeasure.IsUsed != 0, measTypeEnum, value, -1));
                        }
                        toolsResults.Add(new ToolResults(it, currResTool.Tool.IsActive != 0, currResTool.Tool.IsReject != 0, currResTool.Tool.IsDisplayed != 0, measuresResults));
                    }
                    var mr = new InspectionResults(clientId, resInspTemp.InspectionId, resInspTemp.SpindleId, resInspTemp.VialId, resInspTemp.IsActive != 0, resInspTemp.IsReject != 0, resInspTemp.DefectCode, toolsResults);
                    retList.Add(mr);
                }
                return retList;
            }
            else if (protocolVersion == 1)
            {
                var resInspTemp = new ResultInspectionV1();
                var resultInspectionSize = Marshal.SizeOf(resInspTemp);
                var singleResultInspection = new byte[resultInspectionSize];
                var nInspectionResultsNum = dataToDecode.Length / resultInspectionSize;
                var retList = new List<InspectionResults>();
                for (int ii = 0; ii < nInspectionResultsNum; ii++)
                {
                    Array.Clear(singleResultInspection, 0, singleResultInspection.Length);
                    Array.Copy(dataToDecode, ii * resultInspectionSize, singleResultInspection, 0, resultInspectionSize);
                    resInspTemp = ByteArrayToStructure<ResultInspectionV1>(singleResultInspection);
                    if (resInspTemp.ResultInspection.IsActive == 0)
                        continue;
                    var toolsResults = new ToolResultsCollection();
                    for (int it = 0; it < resInspTemp.ResTools.Length; it++)
                    {
                        ResultTool currResTool = resInspTemp.ResTools[it];
                        var measuresResults = new MeasureResultsCollection();
                        for (int im = 0; im < currResTool.ResMeasures.Length; im++)
                        {
                            var currResMeasure = currResTool.ResMeasures[im];
                            var measTypeEnum = MeasureTypeEnum.STRING;
                            var value = "";
                            switch (currResMeasure.Type)
                            {
                                case 0:
                                    measTypeEnum = MeasureTypeEnum.BOOL;
                                    bool bValue = currResMeasure.MeasureValue[0] != 0;
                                    value = bValue.ToString();
                                    break;
                                case 1:
                                    measTypeEnum = MeasureTypeEnum.INT;
                                    int iValue = BitConverter.ToInt32(currResMeasure.MeasureValue, 0);
                                    value = iValue.ToString(CultureInfo.InvariantCulture);
                                    break;
                                case 2:
                                    measTypeEnum = MeasureTypeEnum.DOUBLE;
                                    double dValue = BitConverter.ToDouble(currResMeasure.MeasureValue, 0);
                                    value = dValue.ToString(CultureInfo.InvariantCulture);
                                    break;
                                case 3:
                                    measTypeEnum = MeasureTypeEnum.STRING;
                                    value = Encoding.ASCII.GetString(currResMeasure.MeasureValue);
                                    break;
                            }
                            //string value = Encoding.ASCII.GetString(currResMeasure.measureValue).TrimEnd(new char[] { '\0' });
                            var name = new string(currResMeasure.Name);
                            name = name.Substring(0, name.IndexOf("\0", StringComparison.Ordinal));
                            var um = new string(currResMeasure.MeasureUnit);
                            um = um.Substring(0, um.IndexOf("\0", StringComparison.Ordinal));
                            measuresResults.Add(new MeasureResults(im, name, um, currResMeasure.IsOk != 0, currResMeasure.IsUsed != 0, measTypeEnum, value, -1));
                        }
                        toolsResults.Add(new ToolResults(it, currResTool.Tool.IsActive != 0, currResTool.Tool.IsReject != 0, currResTool.Tool.IsDisplayed != 0, measuresResults));
                    }
                    var mr = new InspectionResults(clientId, resInspTemp.ResultInspection.InspectionId, resInspTemp.ResultInspection.SpindleId, resInspTemp.ResultInspection.VialId, resInspTemp.ResultInspection.IsActive != 0,
                        resInspTemp.ResultInspection.IsReject != 0, resInspTemp.ResultInspection.DefectCode, toolsResults);
                    retList.Add(mr);
                }
                return retList;
            }
            else
                return null;
        }

        internal static InspectionImageSizeAndType[] DecodeImageInfo(byte[] data) {

            int inspectionResultsNum = data.Length / _imageInfoSize;
            InspectionImageSizeAndType[] res = new InspectionImageSizeAndType[inspectionResultsNum];
            byte[] tmpImageInfoData = new byte[_imageInfoSize];

            for (int i = 0; i < inspectionResultsNum; i++) {
                Array.Clear(tmpImageInfoData, 0, _imageInfoSize);
                Array.Copy(data, i * _imageInfoSize, tmpImageInfoData, 0, _imageInfoSize);
                res[i] = ByteArrayToStructure<InspectionImageSizeAndType>(tmpImageInfoData);
            }
            return res;
        }

        //static Color[] _paletteGrayscale;
        //static void InitPaletteGrayscale() {

        //    _paletteGrayscale = new Color[256];

        //    for (int i = 0; i < 256; i++) {
        //        _paletteGrayscale[i] = Color.FromArgb((byte)i, (byte)i, (byte)i);
        //    }
        //}

        //static void ImpostaPaletteGrayscale(Image b) {

        //    var palette = b.Palette;
        //    Array.Copy(_paletteGrayscale, palette.Entries, 256);
        //    b.Palette = palette;
        //}

        //internal static unsafe ImageAvailableEventArgs DecodeImagesResultsData(byte[] dataToDecode) {

        //    const bool planar2PackedConv = true;  //pier: parametrizzare
        //    InspResultInfo resultInfo;
        //    int offsImageData = sizeof(InspResultInfo);
        //    int offsTbnData;
        //    Image<Rgb, byte> image = null, thumbnail = null;
        //    int imageSourceSize, tbnSourceSize;
        //    fixed (byte* pIncData = &dataToDecode[0]) {
        //        //int absInspectionIndex = 0;
        //        var ptrIncData = (IntPtr)pIncData;
        //        resultInfo = (InspResultInfo)Marshal.PtrToStructure(ptrIncData, typeof(InspResultInfo));
        //        imageSourceSize = resultInfo.ImageWidth * resultInfo.ImageHeight * (resultInfo.ImageDepth / 8);
        //        tbnSourceSize = resultInfo.TbnWidth * resultInfo.TbnHeight * (resultInfo.TbnDepth / 8);
        //        //int measuresSize = resultInfo.nMeasures * sizeof(ResultData);
        //        offsTbnData = offsImageData + imageSourceSize;
        //        //int offsMeasuresData = offsTbnData + tbnSourceSize;
        //    }
        //    if (imageSourceSize != 0) {
        //        //immagine
        //        //image = new Bitmap(resultInfo.ImageWidth, resultInfo.ImageHeight, GetPixelDepth(resultInfo.ImageDepth));
        //        image = new Image<Rgb, byte>(resultInfo.ImageWidth, resultInfo.ImageHeight); 
        //        var rect1 = new Rectangle(0, 0, resultInfo.ImageWidth, resultInfo.ImageHeight);
        //        //var bmpData1 = image.LockBits(rect1, ImageLockMode.WriteOnly, image.PixelFormat);
        //        if (planar2PackedConv && (resultInfo.ImageDepth > 8)) {
        //            Planar2PackedConvert(ref dataToDecode, offsImageData, imageSourceSize, resultInfo.ImageDepth / 8, image.Ptr /*bmpData1.Scan0*/);
        //        }
        //        else {
        //            //ImpostaPaletteGrayscale(image);
        //            Marshal.Copy(dataToDecode, offsImageData, image.Ptr/*bmpData1.Scan0*/, resultInfo.ImageWidth * resultInfo.ImageDepth / 8 * resultInfo.ImageHeight);
        //        }
        //        //image.UnlockBits(bmpData1);
        //    }
        //    if (tbnSourceSize != 0) {
        //        //thumbnail
        //        //thumbnail = new Bitmap(resultInfo.TbnWidth, resultInfo.TbnHeight, GetPixelDepth(resultInfo.TbnDepth));
        //        thumbnail = new Image<Rgb,byte>(resultInfo.TbnWidth, resultInfo.TbnHeight);
        //        var rect2 = new Rectangle(0, 0, resultInfo.TbnWidth, resultInfo.TbnHeight);
        //        //var bmpData2 = thumbnail.LockBits(rect2, ImageLockMode.WriteOnly, thumbnail.PixelFormat);
        //        if (planar2PackedConv && (resultInfo.TbnDepth > 8)) {
        //            Planar2PackedConvert(ref dataToDecode, offsTbnData, tbnSourceSize, resultInfo.TbnDepth / 8, thumbnail.Ptr/*bmpData2.Scan0*/);
        //        }
        //        else {
        //            //ImpostaPaletteGrayscale(thumbnail);
        //            Marshal.Copy(dataToDecode, offsTbnData, thumbnail.Ptr/*bmpData2.Scan0*/, resultInfo.TbnWidth * resultInfo.TbnDepth / 8 * resultInfo.TbnHeight);
        //        }
        //        //thumbnail.UnlockBits(bmpData2);
        //        //thumbnail.RotateFlip(RotateFlipType.Rotate90FlipNone);  //pier: parametrizzare
        //        //}
        //    }
        //    return new ImageAvailableEventArgs(image, thumbnail);
        //}

        static PixelFormat GetPixelDepth(int bpp) {

            switch (bpp) {
                case 8:
                    return PixelFormat.Format8bppIndexed;
                //case 16:
                //    return PixelFormat.Format16bppRgb565;
                case 24:
                    return PixelFormat.Format24bppRgb;
                //break;
                default:
                    throw new NotImplementedException("PixelFormat not yet supported");
            }
        }

        static unsafe void MyRemoteCallback(void* userData, CommEvent evt, int remoteIndex, int inspectionIndex, CommDataType dataType, DataChunk* incData) {

            if (_isClosing) return;

            ClientMsgQueue.Enqueue(new ClientMessage(evt, remoteIndex, inspectionIndex, dataType, incData));
            string len = (incData != null) ? incData->DataLen.ToString() : "NaN";
            // Log.Line(LogLevels.Debug, "GretelSvc.MyRemoteCallback", "New ethercomm message: " + evt.ToString() + "." + dataType + " from client " + remoteIndex + " station " + inspectionIndex + " length: " + len);
            NewMexEv.Set();
        }

        static void OnClientConnected(object sender, ClientEventArgs args) {
            if (ClientConnected != null)
                ClientConnected(sender, args);
        }

        static void OnClientDisconnected(object sender, ClientEventArgs args) {
            if (ClientDisconnected != null)
                ClientDisconnected(sender, args);
        }

        static void OnClientImagesResults(object sender, ByteArrayEventArgs args) {
            if (ClientImagesResults != null)
                ClientImagesResults(sender, args);
        }

        /// <summary>
        /// MM-11/01/2024: HVLD2.0 signal binary array with sampled data.
        /// </summary>
        static void OnClientHvldSignalData(object sender, ByteArrayEventArgs args) {
            if (ClientHvldDataResults != null)
                ClientHvldDataResults(sender, args);
        }

        static void OnClientInspectionResults(object sender, GretelInspectionResultsEventArgs args) {
            if (ClientInspectionResults != null)
                ClientInspectionResults(sender, args);
        }

        static void OnClientCollectorInfo(object sender, CollectorInfoEventArgs args) {
            if (ClientCollectorInfo != null)
                ClientCollectorInfo(sender, args);
        }

        static void OnClientStringMessage(object sender, ClientEventArgs args) {
            if (ClientStringMessage != null)
                ClientStringMessage(sender, args);
        }

        static void OnClientStringWarning(object sender, ClientEventArgs args) {
            if (ClientStringWarning != null)
                ClientStringWarning(sender, args);
        }

        static void OnClientRecipeReceived(object sender, RecipeEventArgs args) {
            if (ClientRecipeReceived != null)
                ClientRecipeReceived(sender, args);
        }

        static void OnClientReceivedGretelInfo(object sender, ClientEventArgs args)
        {
            if (ClientReceivedGretelInfo != null)
                ClientReceivedGretelInfo(sender, args);
        }

        static void OnClientDisconnectRemoteDesktop(object sender, ClientEventArgs args) {
            if (ClientDisconnectRemoteDesktop != null)
                ClientDisconnectRemoteDesktop(sender, args);
        }

        static void OnClientReceivedRecipeAck(object sender, ClientEventArgs args) {
            if (ClientReceivedRecipeAck != null)
                ClientReceivedRecipeAck(sender, args);
        }

        static void OnClientReceivedDumpAck(object sender, ClientEventArgs args) {
            if (ClientReceivedDumpAck != null)
                ClientReceivedDumpAck(sender, args);
        }

        static void OnClientReceivedBootDone(object sender, ClientEventArgs args) {
            if (ClientReceivedBootDone != null)
                ClientReceivedBootDone(sender, args);
        }

        static void OnClientImagesInfoReceived(object sender, ImagesInfoReceivedEventArgs e) {

            if (ClientImagesInfoReceived != null)
                ClientImagesInfoReceived(sender, e);
        }

        static void OnClientExportParametersReceived(object sender, ExportParametersEventArgs e) {

            if (ClientExportParametersReceived != null)
                ClientExportParametersReceived(sender, e);
        }

        static void OnClientAuditMessage(object sender, ClientEventArgs args)
        {
            if (ClientAuditMessage != null)
                ClientAuditMessage(sender, args);
        }

        [DllImport("EtherComm.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool DllInitCommServer(ref ConnectionData connData, int nConnections);
        /* 
	    Inizializzazione della comunicazione lato server, creazione istanza oggetto CGretelServer, passaggio di una struttura dati con gli ip dei client 
	    che vanno connessi
        */
        [DllImport("EtherComm.dll")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static unsafe extern bool DllStartCommServer(MulticastDelegate pCallbackFunc, void* userData);
        /* 
            Passaggio argomento la callback di notifica lato server. Avvio dellla comunicazione server. Ascolto dei client
            La funzione ritorna subito. Quando i client si connettono l'evento viene notificato nella callback. 
            Per ogni client connesso viene aperto un socket e viene creato un thread di comunicazione (ascolto).
            Inoltre viene creato un thread di keep_alive (5 secondi), allo scoccare del quale viene verificata la connessione di ogni client
        */
        [DllImport("EtherComm.dll")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool DllCommServerSendData(int remoteIndex, int inspectionIndex, CommDataType dataType, ref DataChunk outData);
        /* 
            Invio dati tramite server
            Argomenti: ip del client a cui inviare i dati, puntatore struttura DataChunk(tipo dati, lunghezza, dati)
        */
        [DllImport("EtherComm.dll")]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool DllCloseCommServer();
        /* 
	        Chiusura del server, chiusura di tutti i thread di ascolto
        */

        internal enum CommEvent {

            EConnected = 0,
            EDisconnected = 1,
            EDisconnectedreset = 2,
            EDisconnectednoka = 3,
            ENewdata = 4,
            ESocketerror = 5
        }

        internal enum CommError {

            ErrInvalidsocket = -1,
            ErrConnclosed = 1,
            ErrConnreset = 2,
            ErrConnresetnoka = 3,
            ErrUnknown = 4,
            ErrZerosize = 5
        }

        internal enum CommDataType {

            CommNull = 0,			        // Null
            CommKa = 1,			            // Pacchetto KeepAlive
            CommAck = 2,			        // Ack recipe updated
            CommString = 3,			        // Stringa generica
            SRecipe = 10,			        // Ricetta da server a client
            SSetinspdisplay = 11,			// Abilitazione/disabilitazione invio risultati ispezione
            SSetExportedParam = 12,			// Set parametri esportati
            SUser = 13,			            // Set livello utente x desktop remoto
            SString = 14,			        // Stringa generica da server
            SSetcollectparams = 15,			// Set parametri salvataggio
            SInspectionViewId = 16,         // Invio ID della ispezione da visualizzare ricevuto dal server alla connessione del desktop remoto
            CommNAck = 17,                  // Ack recipe not updated
            SSetcollectparams2 = 18,        // Set save parameters new version


            CRecipe = 20,			        // Ricetta da client a server
            CInspresults = 21,			    // Risultato ispezione: immagine + thumbnail
            CModuleresults = 22,			// Risultati numerici di tutte le ispezioni del modulo client
            CUser = 23,			            // Utente da client x disconnessione da desktop remoto
            CString = 24,			        // Stringa generica da client
            CImageSizeAndType = 25,         // Specifiche Width Height e Type( color, mono)  di immagine risultato e thumbnail
            CExportParameters = 26,			// Parametri esportati dal sistema di visione al supervisore
	        CAppliedRecipeFile = 27,		// Ricetta da client a server dei valori applicati, non salvati
            CAuditTrailMessage = 28,        // Status del collector di tutte le ispezioni del modulo client
            CHvldResults = 29               // MM-11/01/2024: HVLD2.0 signal binary array with sampled data.
        }

        internal struct ConnectionData {

            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = MaxClientsNum)]
            internal string[] LocalIpAddress;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = MaxClientsNum)]
            internal string[] RemoteName;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = MaxClientsNum)]
            internal string[] RemoteIpAddress;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = MaxClientsNum)]
            internal int[] RemotePort;
        }

        internal unsafe struct DataChunk {

            internal int DataLen;
            internal byte* Data;
        }

        internal struct Recipe {

            internal string RecipeName;
            internal string RecipeV2;
            internal bool IsNew;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = StringLength)]
            readonly byte[] _name;
            readonly byte _isNew;
            readonly int _recipeLength;
            readonly byte[] _binRecipe;

            internal Recipe(string recipeName, string recipe) {

                RecipeName = recipeName;
                RecipeV2 = recipe;
                _isNew = 0;
                IsNew = false;
                var tempName = Encoding.GetEncoding(1252).GetBytes(RecipeName);
                _name = new byte[StringLength];
                Array.Copy(tempName, _name, Math.Min(tempName.Length, StringLength));
                _binRecipe = Encoding.GetEncoding(1252).GetBytes(RecipeV2);
                _recipeLength = RecipeV2.Length;
            }

            internal Recipe(byte[] stream) {

                _name = new byte[StringLength];
                var index = 0;
                Array.Copy(stream, index, _name, 0, StringLength);
                index += StringLength;
                _isNew = stream[index];
                index += sizeof(byte);
                _recipeLength = BitConverter.ToInt32(stream, index);
                index += sizeof(int);
                _binRecipe = new byte[_recipeLength];
                Array.Copy(stream, index, _binRecipe, 0, _recipeLength);
                //index += _recipeLength;
                RecipeName = Encoding.GetEncoding(1252).GetString(_name);
                RecipeName = RecipeName.Substring(0, RecipeName.IndexOf('\0'));
                RecipeV2 = Encoding.GetEncoding(1252).GetString(_binRecipe);
                IsNew = (_isNew != 0);
            }

            internal byte[] GetBytes() {

                var index = 0;
                var ret = new byte[_name.Length + sizeof(byte) + _recipeLength + sizeof(int)];
                Array.Copy(_name, ret, StringLength);
                index += StringLength;
                ret[index] = _isNew;
                index += sizeof(byte);
                var datalen = BitConverter.GetBytes(_binRecipe.Length);
                Array.Copy(datalen, 0, ret, index, sizeof(int));
                index += sizeof(int);
                Array.Copy(_binRecipe, 0, ret, index, _recipeLength);
                //index += recipeLength;
                return ret;
            }
        }

        internal struct InspResultInfo {
            internal int ImageWidth;
            internal int ImageHeight;
            internal int ImageDepth;
            internal int TbnWidth;
            internal int TbnHeight;
            internal int TbnDepth;
            internal uint sample_id;

            public static int GetSize() {
                return Marshal.SizeOf(typeof(InspResultInfo));
            }
        }

        internal struct InspectionImageSizeAndType {
            public int imageWidth;
            public int imageHeight;
            public int imageDepth; // 8 = Mono, 24 = Color
            public int tbnWidth;
            public int tbnHeight;
            public int tbnDepth; // 8 = Mono, 24 = Color
        };

        #region Tools grid new struct definitions
        // Matteo - 09/07/2024: Tools grid bug (random numbers without meaning being shown on the grid)
        // was an alignment problem due to a different definition of the involved structs:
        // 
        // - ResultMeasure
        // - ResultTool
        // - ResultInspectionV1

        /// <summary>
        /// 
        /// </summary>
        internal struct ResultMeasure
        {
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = StringLength)]
            internal char[] Name;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = StringLength)]
            internal char[] MeasureUnit;
            internal byte IsOk;
            internal byte IsUsed;
            internal byte Type;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = StringLength)]
            internal byte[] MeasureValue;
        }
        /// <summary>
        /// 
        /// </summary>
        internal struct ResultTool
        {
            internal BaseResultTool Tool;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = MaxMeasuresNum)]
            internal ResultMeasure[] ResMeasures;
        }
        /// <summary>
        /// 
        /// </summary>
        internal struct BaseResultTool
        {
            internal byte IsActive;
            internal byte IsReject;
            internal byte IsDisplayed;
        }
        /// <summary>
        /// 
        /// </summary>
        internal struct ResultInspectionV1
        {
            internal BaseResultInspectionV1 ResultInspection;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = MaxToolsNum)]
            internal ResultTool[] ResTools;
        }
        /// <summary>
        /// 
        /// </summary>
        internal struct BaseResultInspectionV1
        {
            internal byte IsActive;
            internal byte IsReject;
            internal byte InspectionId;
            internal uint SpindleId;
            internal uint VialId;
            internal ushort DefectCode;
        }
        #endregion

        #region Tools grid old struct definitions
        //internal struct ResultMeasure {
        //    [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = StringLength)]
        //    internal char[] Name;
        //    [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = StringLength)]
        //    internal char[] MeasureUnit;
        //    internal byte IsOk;
        //    internal byte IsUsed;
        //    internal byte Type;
        //    [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = StringLength)]
        //    internal byte[] MeasureValue;
        //}

        //internal struct ResultTool {
        //    internal byte IsActive;
        //    internal byte IsReject;
        //    internal byte IsDisplayed;
        //    [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = MaxMeasuresNum)]
        //    internal ResultMeasure[] ResMeasures;
        //}

        //internal struct ResultInspectionV1
        //{
        //    internal byte IsActive;
        //    internal byte IsReject;
        //    internal byte InspectionId;
        //    internal uint SpindleId;
        //    internal uint VialId;
        //    internal ushort DefectCode;
        //    [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = MaxToolsNum)]
        //    internal ResultTool[] ResTools;
        //}
        #endregion

        internal struct ResultInspectionV0
        {
            internal byte IsActive;
            internal byte IsReject;
            internal byte InspectionId;
            internal uint SpindleId;
            internal uint VialId;
            internal byte DefectCode;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = MaxToolsNum)]
            internal ResultTool[] ResTools;
        }

        internal struct CollectorInfoInspection {

            internal int imagesSaved;
            internal byte inspectionId;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = 27)]
            internal byte[] spare;
        }

        internal struct ExportParameter {

            internal double Increment;
            internal byte hInspectionId;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = 2)]
            internal byte[] hGroupId;
            internal byte hId;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = StringLength)]
            internal char[] hRealName;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = StringLength)]
            internal char[] hDisplayName;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = StringLength)]
            internal char[] hParentName;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = StringLength)]
            internal char[] ExportName;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = 1024)]
            internal char[] Description;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = StringLength)]
            internal byte[] MinValue;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = StringLength)]
            internal byte[] MaxValue;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = 64)]
            internal char[] UnitMeasure;
            internal byte Type;
            internal byte Decimals;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = (16 * StringLength))]
            internal char[] StringList;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = StringLength)]
            internal byte[] Value;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = 50)]
            internal byte[] spare;

            internal ExportParameter(int inspectionId, Parameter p) {

                hGroupId = new byte[2];
                Value = new byte[StringLength];
                MinValue = new byte[StringLength];
                MaxValue = new byte[StringLength];
                spare = new byte[178];
                hRealName = new char[StringLength];
                hDisplayName = new char[StringLength];
                hParentName = new char[StringLength];
                ExportName = new char[StringLength];
                Description = new char[1024];
                UnitMeasure = new char[64];
                StringList = new char[16 * StringLength];
                byte[] tmpVal;
                char[] tmpText;

                string[] group = p.Group.Split(';');
                hGroupId[0] = GretelSvc.ExportParameter.groupStringToId(group[0]);
                hGroupId[1] = Convert.ToByte(group[1]);
                Increment = p.Increment;
                hInspectionId = (byte)inspectionId;
                Decimals = (byte)p.Decimals;
                tmpText = p.Description.ToCharArray(0, Math.Min(p.Description.Length, Description.Length));
                Array.Copy(tmpText, 0, Description, 0, tmpText.Length);
                tmpText = p.Id.ToCharArray(0, Math.Min(p.Id.Length, GretelSvc.StringLength));
                Array.Copy(tmpText, 0, hRealName, 0, tmpText.Length);
                tmpText = p.Label.ToCharArray(0, Math.Min(p.Label.Length, GretelSvc.StringLength));
                Array.Copy(tmpText, 0, hDisplayName, 0, tmpText.Length);
                tmpText = p.ExportName.ToCharArray(0, Math.Min(p.ExportName.Length, GretelSvc.StringLength));
                Array.Copy(tmpText, 0, ExportName, 0, tmpText.Length);
                hId = Convert.ToByte(p.ParamId);
                tmpText = p.ParentName.ToCharArray(0, Math.Min(p.ParentName.Length, GretelSvc.StringLength));
                Array.Copy(tmpText, 0, hParentName, 0, tmpText.Length);
                int stringCount = Math.Min(p.AdmittedValues.Count, StringList.Length / GretelSvc.StringLength);
                for (int iS = 0; iS < stringCount; iS++) {
                    tmpText = p.AdmittedValues[iS].ToCharArray(0, Math.Min(p.AdmittedValues[iS].Length, GretelSvc.StringLength));
                    Array.Copy(tmpText, 0, StringList, iS * GretelSvc.StringLength, tmpText.Length);
                }
                Type = (byte)GretelSvc.GetType(p.ValueType);
                tmpText = p.MeasureUnit.ToCharArray(0, Math.Min(p.MeasureUnit.Length, UnitMeasure.Length));
                Array.Copy(tmpText, 0, UnitMeasure, 0, tmpText.Length);
                
                try {
                    switch (p.ValueType) {
                        case "bool":
                            tmpVal = BitConverter.GetBytes(bool.Parse(p.Value));
                            Array.Copy(tmpVal, Value, Math.Min(tmpVal.Length, StringLength));
                            tmpVal = BitConverter.GetBytes(bool.Parse(p.MinValue));
                            Array.Copy(tmpVal, MinValue, Math.Min(tmpVal.Length, StringLength));
                            tmpVal = BitConverter.GetBytes(bool.Parse(p.MaxValue));
                            Array.Copy(tmpVal, MaxValue, Math.Min(tmpVal.Length, StringLength));
                            break;
                        case "int":
                            tmpVal = BitConverter.GetBytes(int.Parse(p.Value));
                            Array.Copy(tmpVal, Value, Math.Min(tmpVal.Length, StringLength));
                            tmpVal = BitConverter.GetBytes(int.Parse(p.MinValue));
                            Array.Copy(tmpVal, MinValue, Math.Min(tmpVal.Length, StringLength));
                            tmpVal = BitConverter.GetBytes(int.Parse(p.MaxValue));
                            Array.Copy(tmpVal, MaxValue, Math.Min(tmpVal.Length, StringLength));
                            break;
                        case "double":
                            tmpVal = BitConverter.GetBytes(double.Parse(p.Value, CultureInfo.InvariantCulture));
                            Array.Copy(tmpVal, Value, Math.Min(tmpVal.Length, StringLength));
                            tmpVal = BitConverter.GetBytes(double.Parse(p.MinValue, CultureInfo.InvariantCulture));
                            Array.Copy(tmpVal, MinValue, Math.Min(tmpVal.Length, StringLength));
                            tmpVal = BitConverter.GetBytes(double.Parse(p.MaxValue, CultureInfo.InvariantCulture));
                            Array.Copy(tmpVal, MaxValue, Math.Min(tmpVal.Length, StringLength));
                            break;
                        case "string":
                            tmpVal = Encoding.ASCII.GetBytes(p.Value);
                            Array.Copy(tmpVal, Value, Math.Min(tmpVal.Length, StringLength));
                            tmpVal = Encoding.ASCII.GetBytes(p.MinValue);
                            Array.Copy(tmpVal, MinValue, Math.Min(tmpVal.Length, StringLength));
                            tmpVal = Encoding.ASCII.GetBytes(p.MaxValue);
                            Array.Copy(tmpVal, MaxValue, Math.Min(tmpVal.Length, StringLength));
                            break;
                        default:
                            Log.Line(LogLevels.Error, "GretelStationBase.SetParameters", "Parameter type unknown!");
                            break;
                    }
                }
                catch (Exception ex) {
                    Log.Line(LogLevels.Warning, "GretelStationBase.SetParameters", "Error while casting parameter value: " + ex.Message);
                }

            }

            internal byte[] GetBytes() {

                int size = Marshal.SizeOf(this);
                var res = new byte[size];
                IntPtr ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(this, ptr, true);
                Marshal.Copy(ptr, res, 0, size);
                Marshal.FreeHGlobal(ptr);
                return res;
            }

            internal static string groupIdToString(byte groupId) {

                string res = "";
                switch (groupId) {
                    case 0:         // CAMERA
                        res = "CAMERA";
                        break;
                    case 1:         // DIGITIZER
                        res = "DIGITIZER";
                        break;
                    case 2:         // TOOL ENABLE
                        res = "TOOL ENABLE";
                        break;
                    case 3:         // TOOL PARAMS
                        res = "TOOL PARAMS";
                        break;
                    case 4:         // TOOL OUTPUT
                        res = "TOOL OUTPUT";
                        break;
                    case 5:         // STROBO
                        res = "STROBO";
                        break;
                    case 6:         // LIGHT SENSOR
                        res = "LIGHT SENSOR";
                        break;
                    default:
                        Log.Line(LogLevels.Error, "GretelCameraBase.groupIdToString", "Parameter group unknown!");
                        break;
                }
                return res;
            }

            internal static byte groupStringToId(string groupId) {

                byte res = 0;
                switch (groupId) {
                    case "CAMERA":              // CAMERA
                        res = 0;
                        break;
                    case "DIGITIZER":           // DIGITIZER
                        res = 1;
                        break;
                    case "TOOL ENABLE":         // TOOL ENABLE
                        res = 2;
                        break;
                    case "TOOL PARAMS":         // TOOL PARAMS
                        res = 3;
                        break;
                    case "TOOL OUTPUT":         // TOOL OUTPUT
                        res = 4;
                        break;
                    case "STROBO":              // STROBO
                        res = 5;
                        break;
                    case "LIGHT SENSOR":        // LIGHT SENSOR
                        res = 6;
                        break;
                    default:
                        Log.Line(LogLevels.Error, "GretelCameraBase.groupStringToId", "Parameter group unknown!");
                        break;
                }
                return res;
            }
        }

        //[StructLayout(LayoutKind.Sequential)]
        internal struct CollectingData {
            internal int Type;          // Cosa salvare: 0 Frames, 1 Thumb, 2 Result
            internal int Condition;     // Quando salvare: 0 Never, 1 AnyCase, 2 OnReject, 3 OnGood, 4 OnTool
            internal int ToSave;        // Numero immagini da salvare
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = MaxToolsNum)]
            internal byte[] OnToolId;   // Salvataggio onTool: false disabilitato, true attivato

            internal CollectingData(int type, int condition, int vialsToSave, Dictionary<int, bool> toolSelected) {
                Type = type;
                Condition = condition;
                ToSave = vialsToSave;
                OnToolId = new byte[MaxToolsNum];
                if (toolSelected != null && toolSelected.Count > 0) {
                    foreach (KeyValuePair<int, bool> tool in toolSelected) {
                        if (tool.Key < MaxToolsNum)
                            OnToolId[tool.Key] = tool.Value ? (byte)1 : (byte)0;
                    }
                }
            }
        }

        //[StructLayout(LayoutKind.Sequential, Pack=1)]
        internal struct CollectParameters {
            internal byte Enable;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStr, SizeConst = MaxInspectionsNum)]
            internal CollectingData[] CollData;

            internal CollectParameters(bool enable, List<StationDumpSettings> statDumpSet) {
                Enable = enable ? (byte)1 : (byte)0;
                CollData = new CollectingData[MaxInspectionsNum];
                if (enable && statDumpSet != null) {
                    for (int isp = 0; isp < MaxInspectionsNum; isp++) {
                        StationDumpSettings currSet = statDumpSet.Find(ss => ss.Id == isp);
                        if (currSet != null) {
                            CollData[isp] = new CollectingData((int)currSet.Type, (int)currSet.Condition, currSet.VialsToSave, currSet.ToolsSelectedForDump);
                        }
                    }
                }
            }

            internal byte[] GetBytes() {

                int size = Marshal.SizeOf(this);
                var ret = new byte[size];
                IntPtr ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(this, ptr, true);
                Marshal.Copy(ptr, ret, 0, size);
                Marshal.FreeHGlobal(ptr);
                return ret;
            }
        }

        internal static T ByteArrayToStructure<T>(byte[] bytes) where T : struct {
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            var stuff = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(),
                typeof(T));
            handle.Free();
            return stuff;
        }

        public static string GetType(int type) {

            string res = "";
            switch (type) {
                case 0:         // BOOL
                    res = "bool";
                    break;
                case 1:         // INT
                    res = "int";
                    break;
                case 2:         // DOUBLE 
                    res = "double";
                    break;
                case 3:         // STRING
                    res = "string";
                    break;
                case 4:         // STRING (2023-06-21  Type wizard - temporary set as string)
                    res = "string";
                    break;
                case 5:         // STRING (2023-06-21  Type header - temporary set as string)
                    res = "string";
                    break;
                default:
                    Log.Line(LogLevels.Error, "NodeRecipe.GetType", "Parameter type unknown!");
                    break;
            }
            return res;
        }

        public static int GetType(string type) {

            int res = -1;
            switch (type) {
                case "bool":         // BOOL
                    res = 0;
                    break;
                case "int":         // INT
                    res = 1;
                    break;
                case "double":         // DOUBLE 
                    res = 2;
                    break;
                case "string":         // STRING
                    res = 3;
                    break;
                /*2023-06-21 Temporary, will need to add these types*/
                //case "wizard":         // STRING
                //    res = 4;
                //    break;
                //case "header":         // STRING
                //    res = 5;
                //    break;
                default:
                    Log.Line(LogLevels.Error, "NodeRecipe.GetType", "Parameter type unknown!");
                    break;
            }
            return res;
        }

        internal unsafe class ClientMessage {
            internal CommEvent Evt; //Tipo di evento (connessione, disconnessione, nuovo dato, ecc..)
            internal int ClientIndex;
            internal int InspectionIndex;
            internal CommDataType DataType; //Tipo di dato (ricetta, parametro, ack, ecc..)
            internal DataChunk Dchunk = new DataChunk();
            internal byte[] DataArray;

            internal ClientMessage(CommEvent evt, int clientIndex, int inspectionIndex, CommDataType dataType, DataChunk* data) {

                Evt = evt;
                ClientIndex = clientIndex;
                InspectionIndex = inspectionIndex;
                DataType = dataType;
                if (data != null) {
                    Dchunk.DataLen = data->DataLen;
                    DataArray = new byte[Dchunk.DataLen];
                    Marshal.Copy(new IntPtr(data->Data), DataArray, 0, Dchunk.DataLen);
                }
            }
        }
    }

    public class GretelSvcException : Exception {

        public GretelSvcException(string message)
            : base(message) {
        }
    }

    public class ClientEventArgs : EventArgs {

        public string Ip;
        public int Id;
        public string Notes;

        public ClientEventArgs(string ip, int id, string notes) {
            Ip = ip;
            Id = id;
            Notes = notes;
        }
    }

    public class RecipeEventArgs : EventArgs {

        public string Ip;
        public int Id;
        public string RecipeV2;
        public bool IsNew;
        public string RecipeName;
        public bool ToSave;

        public RecipeEventArgs(string ip, int id, bool isNew, string recipeName, string recipe, bool toSave) {
            Ip = ip;
            Id = id;
            RecipeV2 = recipe;
            IsNew = isNew;
            RecipeName = recipeName;
            ToSave = toSave;
        }
    }

    public class ByteArrayEventArgs : EventArgs {

        public string Ip;
        public int Id;
        public int InspectionId;
        public byte[] Res;

        public ByteArrayEventArgs(string ip, int id, int inspectionId, byte[] res) {
            Ip = ip;
            Id = id;
            InspectionId = inspectionId;
            Res = res;
        }
    }

    public class GretelInspectionResultsEventArgs : EventArgs {

        public string Ip;
        public int Id;
        public List<InspectionResults> InspectionResultsCollection;

        public GretelInspectionResultsEventArgs(string ip, int id, List<InspectionResults> measResCollection) {

            Ip = ip;
            Id = id;
            InspectionResultsCollection = measResCollection;
        }
    }

    //public class GretelInspectionResults : InspectionResults {

    //    internal GretelInspectionResults(int nodeId, int inspectionId, int spindleId, uint vialId, bool isActive, bool isReject, int rejectionCause, ToolResultsCollection toolsResults)
    //        : base(nodeId, inspectionId, spindleId, vialId, isActive, isReject, rejectionCause, toolsResults) {

    //    }
    //}

    public class ImagesInfoReceivedEventArgs : EventArgs {

        public string Ip { get; private set; }
        public int Id { get; private set; }
        internal GretelSvc.InspectionImageSizeAndType[] ImageInfo { get; private set; }
        //public int ImageWidth { get; private set; }
        //public int ImageHeight { get; private set; }
        //public int ImageDepth { get; private set; } // 8 = Mono, 24 = Color
        //public int TbnWidth { get; private set; }
        //public int TbnHeight { get; private set; }
        //public int TbnDepth { get; private set; } // 8 = Mono, 24 = Color

        internal ImagesInfoReceivedEventArgs(string ip, int id, GretelSvc.InspectionImageSizeAndType[] imageInfo) {

            Ip = ip;
            Id = id;
            ImageInfo = imageInfo;
            //ImageWidth = imageInfo.imageWidth;
            //ImageHeight = imageInfo.imageHeight;
            //ImageDepth = imageInfo.imageDepth;
            //TbnWidth = imageInfo.tbnWidth;
            //TbnHeight = imageInfo.tbnHeight;
            //TbnDepth = imageInfo.tbnDepth;

        }
    }

    public class ExportParametersEventArgs : EventArgs {

        public string Ip { get; private set; }
        public int Id { get; private set; }
        internal GretelSvc.ExportParameter[] ExportParams { get; private set; }

        internal ExportParametersEventArgs(string ip, int id, GretelSvc.ExportParameter[] exportParams) {

            Ip = ip;
            Id = id;
            ExportParams = exportParams;
        }
    }

}

