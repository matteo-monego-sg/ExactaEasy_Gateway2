using DisplayManager;
using ExactaEasyCore;
using SPAMI.Util.Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EoptisClient
{

    public class EoptisNodeBase2 : Node {

        private static readonly EoptisDevCollection EoptisDevCollection = new EoptisDevCollection();
        internal static readonly EoptisNodeBaseCollection EoptisClients = new EoptisNodeBaseCollection();
        private static readonly object EoLock = new object();
        private NodeRecipe dataSource;                  //parametri di ricetta
        private NodeRecipe currentNodeParameters;       //parametri device
        private ParameterInfoCollection _pic;
        private string _cultureCode;
        private Thread _mexDequeuerTh;
        private WaitHandle[] _mexDequeuerHandles;
        private ManualResetEvent KillEv = new ManualResetEvent(false);
        private ManualResetEvent PollEv = new ManualResetEvent(false);
        private ManualResetEvent GetParametersEv = new ManualResetEvent(false);
        private ManualResetEvent RunEv = new ManualResetEvent(false);
        internal EoptisWorkingMode workingMode = EoptisWorkingMode.UNKNOWN;
        private ManualResetEvent initWorkingModeSetEv = new ManualResetEvent(false);
        bool hardGetRequested = true;
        EoptisParameterCollection currentEoptisNodeParameters = null;
        //bool exit = false;
        //AutoResetEvent exitEvt = new AutoResetEvent(false);
        //Thread diagThread;

        public EoptisNodeBase2(NodeDefinition nodeDefinition)
            : base(nodeDefinition) {

            var newEoptisDev = new EoptisDev() {
                LocalIpAddress = ServerIP4Address,
                RemoteName = Description,
                RemoteIpAddress = Address,
                RemotePort = Port
            };
            EoptisDevCollection.Add(newEoptisDev);
            lock (EoLock) {
                if (EoptisClients[this] == null)
                    EoptisClients.Add(this);
            }
            KillEv.Reset();
            PollEv.Reset();
            if (_mexDequeuerHandles == null) {
                _mexDequeuerHandles = new WaitHandle[2];
                _mexDequeuerHandles[0] = KillEv;
                _mexDequeuerHandles[1] = PollEv;
            }
            if (_mexDequeuerTh == null || !_mexDequeuerTh.IsAlive) {
                _mexDequeuerTh = new Thread(MexDequeuer) {
                    Name = "Eoptis " + Address + " Messages Dequeuer Thread"
                };
                _mexDequeuerTh.Start();
                _mexDequeuerTh.Priority = ThreadPriority.Normal;
            }
        }

        public override void Dispose() {

            lock (EoLock) {
                EoptisClients.Remove(this);
                Log.Line(LogLevels.Warning, "EoptisNodeBase.Dispose", "Client " + ServerIP4Address + ": Disposed!");

                if (EoptisClients.Count == 0) {
                    EoptisSvc.Dispose();
                }
                KillEv.Set();
                PollEv.Reset();
                if (_mexDequeuerTh != null && _mexDequeuerTh.IsAlive) {
                    _mexDequeuerTh.Join(2000);
                }
            }
        }

        public override void Connect() {

            Exception exx = null;
            if (!Connected) {
                try {
                    //EoptisSvc.setWorkingMode(5);
                    //EoptisSvc.disconnect(new StringBuilder(Address));
                    int res = EoptisSvc.Connect(Address, Port, false);
                    if (res == 0) {
                        Log.Line(LogLevels.Pass, "EoptisNodeBase.Connect", "EOPTIS BOARD " + Address + " CONNECTED!");
                        EoptisSvc.workingModesOffCounter[Address] = new Stopwatch();
                        EoptisSvc.workingModesOffCounter[Address].Start();
                        Connected = true;
                        //PollEv.Set();
                        initWorkingModeSetEv.Set();
                        initWorkingModeSetEv.WaitOne(10000);
                    }
                    else
                        exx = new Exception("Connection with node " + Address + " failed. Error: " + res.ToString());
                }
                catch (Exception ex) {
                    exx = ex;
                }
            }
            if (exx != null)
                throw exx;
            //diagThread = new Thread(new ThreadStart(diagnosticThread));
            //diagThread.Start();
        }

        public override void Disconnect() {

            Exception exx = null;
            try {
                PollEv.Reset();
                int res = EoptisSvc.Disconnect(Address);
                if (res == 0) {
                    Log.Line(LogLevels.Error, "EoptisNodeBase.Disconnect", "EOPTIS BOARD " + Address + " DISCONNECTED!");
                    Connected = false;
                    Version = "?";
                } else {
                    exx = new Exception("Disconnection from node " + Address + " failed. Error: " + res.ToString());
                }
            }
            catch (Exception ex) {
                exx = ex;
            }
            if (exx != null)
                throw exx;
        }

        List<EoptisData> lastEoptisDataCollection = new List<EoptisData>() { new EoptisData(-1, -1, -1, 0, false, false, -1, EoptisWorkingMode.UNKNOWN, null, null) };

        void MexDequeuer() {
            Task rescanTask = new Task(new Action(() => { OnRescanRequested(this, EventArgs.Empty); }));

            while (true) {
                var retVal = WaitHandle.WaitAny(_mexDequeuerHandles);
                if (retVal == 0) break; //killEv
                EoptisData eir;
                bool newRes = EoptisSvc.ReadResults(Address, IdNode, out eir);
                //diagnosticTimer.Restart();
                bool newMeasures = false;
                /*if (Connected == true && EoptisSvc.workingModesOffCounter[Address].ElapsedMilliseconds > 5000) {
                    try {
                        Disconnect();
                    } catch {
                    }
                    rescanTask = new Task(new Action(() => { OnRescanRequested(this, EventArgs.Empty); }));
                    rescanTask.Start();
                    Log.Line(LogLevels.Error, "EoptisNodeBase.Disconnect", "EOPTIS BOARD " + Address + " DISCONNECTED!");
                }*/
                if (Connected == false && rescanTask.Status.Equals(System.Threading.Tasks.TaskStatus.Running) == false) {
                    if (EoptisSvc.workingModesOffCounter.ContainsKey(Address))
                        EoptisSvc.workingModesOffCounter[Address].Restart();
                    try {
                        Connect();
                        if (Connected == true) {
                            rescanTask = new Task(new Action(() => { OnRescanRequested(this, EventArgs.Empty); }));
                            rescanTask.ContinueWith(antecedent => OnBootDone(this, EventArgs.Empty) );
                            rescanTask.Start();
                        }
                    } catch {
                    }
                }
                if (newRes == true) {
                    EoptisSvc.workingModesOffCounter[Address].Restart();
                    foreach (EoptisStationBase s in Stations)
                        foreach (EoptisDeviceBase d in s.Cameras)
                            d.CurrWorkingMode = d.MapWorkingMode(eir.WorkingMode);
                    workingMode = eir.WorkingMode;
                    if (eir.Parameters != null && eir.Parameters.Count > 0) {
                        foreach (EoptisParameter ep in eir.Parameters) {
                            EoptisParameter par = currentEoptisNodeParameters[ep.Id];
                            if (par != null) {
                                par.Value = ep.Value;
                            }
                        }
                        GetParametersEv.Set();
                    }
                    initWorkingModeSetEv.Set();
                    //Log.Line(LogLevels.Debug, "EoptisNodeBase.MexDequeuer", Address + ": VIAL ID = " + eir.VialId);
                   // Log.Line(LogLevels.Debug, "EoptisNodeBase.MexDequeuer", Address + ": SPINDLE ID = " + eir.SpindleId);
                    //Log.Line(LogLevels.Debug, "EoptisNodeBase.MexDequeuer", Address + ": VALUE = " + eir.ResToolCollection[0].ResMeasureCollection[0].MeasureValue);
                    //Log.Line(LogLevels.Debug, "EoptisNodeBase.MexDequeuer", Address + ": IS REJECT = " + eir.IsReject);
                    //Log.Line(LogLevels.Debug, "EoptisNodeBase.MexDequeuer", Address + ": REJECTION CAUSE = " + eir.RejectionCause);
                    if ((eir.ResToolCollection != null) && (eir.ResToolCollection[0].ResMeasureCollection.Count > 0)) {
                        foreach (MeasureResults measRes in eir.ResToolCollection[0].ResMeasureCollection) {
                            //Log.Line(LogLevels.Debug, "EoptisNodeBase.MexDequeuer", Address + ": " + measRes.MeasureName + " = " + measRes.MeasureValue);
                            if (!string.IsNullOrEmpty(measRes.MeasureValue))
                                newMeasures = true;
                        }
                    }
                    if (newMeasures) {
                        RunEv.Set();
                        lock (lastEoptisDataCollection) {
                            lastEoptisDataCollection = new List<EoptisData>() { eir };
                            OnClientInspectionResults(new EoptisDataEventArgs(Address, IdNode, lastEoptisDataCollection));
                        }
                    }
                }
                Thread.Sleep(50);
            }
        }

        private void OnClientInspectionResults(EoptisDataEventArgs e) {

            foreach (EoptisData eir in e.InspectionResultsCollection) {
                int inspId = eir.InspectionId; // relativeToAbsolute(gmr.InspectionId);
                //Log.Line(LogLevels.Debug, "GretelNodeBase.OnClientInspectionResults",
                //    e.Ip + " INCOMING INSPECTION RESULTS FROM {0}:" +
                //    " SPINDLE ID = " + gmr.SpindleId +
                //    " VIAL ID = " + gmr.VialId +
                //    " ACTIVE = " + gmr.IsActive.ToString() +
                //    " REJECTED = " + gmr.IsReject.ToString(), inspId);

                if (Stations[inspId] == null) {
                    if (Stations.Count > 0)
                        Log.Line(LogLevels.Error, "GretelNodeBase.OnClientInspectionResults", "Unexpected station id");
                    //throw new Exception("Unexpected station id");
                    return;
                }
                ((EoptisStationBase)Stations[inspId]).OnInspectionResults(new InspectionResults(IdNode, inspId, eir.SpindleId,
                    eir.VialId, eir.IsActive, eir.IsReject, eir.RejectionCause, eir.ResToolCollection));
            }
        }

        public override void Run(int inspectionId) {

            //success = success + MeasDevices.GetSpectrumLong(Index, COLORIMETRO_SPECTRUM_RAW_ID, COLORIMETRO_SPECTRUM_LENGTH, SpectrumRaw(0), 1)   'GET SPETTRO RAW
            //success = success + MeasDevices.GetSpectrumLong(Index, COLORIMETRO_SPECTRUM_MIS_ID, COLORIMETRO_SPECTRUM_LENGTH, SpectrumMis(0), 1)   'GET SPETTRO MIS
            //success = success + MeasDevices.GetSpectrumLong(Index, COLORIMETRO_SPECTRUM_RIF_ID, COLORIMETRO_SPECTRUM_LENGTH, SpectrumRef(0), 1)   'GET SPETTRO RIF
            //success = success + MeasDevices.GetSpectrumLong(Index, COLORIMETRO_SPECTRUM_ELAB_ID, COLORIMETRO_SPECTRUM_LENGTH, SpectrumElab(0), 1) 'GET SPETTRO ELAB
            //success = success + MeasDevices.GetResults(Index, RangeResults(0), RejectionReason, LightLevel, 1)
            Exception exx = null;
            EoptisDeviceBase d = Stations.First().Cameras.First() as EoptisDeviceBase;
            RunEv.Reset();
            var runResults = EoptisSvc.GetResultsParamList(d.CameraType);
            if (d.CameraType == "EoptisTurbidimeter") {
                Thread.Sleep(1000);
                return;
            }
            EoptisSvc.Run(Address, ref runResults);
            bool res = RunEv.WaitOne(10000);
            if (res == false) {
                exx = new Exception("Board " + Address + ": Run Timeout Error");
            }
            if (exx != null) throw exx;
        }

        public override void Live(int inspectionId, bool enable) {

            //success = success + MeasDevices.GetSpectrumLong(Index, COLORIMETRO_SPECTRUM_RAW_ID, COLORIMETRO_SPECTRUM_LENGTH, SpectrumRaw(0), 1)   'GET SPETTRO RAW
            //success = success + MeasDevices.GetSpectrumLong(Index, COLORIMETRO_SPECTRUM_MIS_ID, COLORIMETRO_SPECTRUM_LENGTH, SpectrumMis(0), 1)   'GET SPETTRO MIS
            //success = success + MeasDevices.GetSpectrumLong(Index, COLORIMETRO_SPECTRUM_RIF_ID, COLORIMETRO_SPECTRUM_LENGTH, SpectrumRef(0), 1)   'GET SPETTRO RIF
            //success = success + MeasDevices.GetSpectrumLong(Index, COLORIMETRO_SPECTRUM_ELAB_ID, COLORIMETRO_SPECTRUM_LENGTH, SpectrumElab(0), 1) 'GET SPETTRO ELAB
            //success = success + MeasDevices.GetResults(Index, RangeResults(0), RejectionReason, LightLevel, 1)
            //Exception exx = null;
            EoptisDeviceBase d = Stations.First().Cameras.First() as EoptisDeviceBase;
            //LiveEv.Reset();
            EoptisSvc.Live(Address, enable);
            //bool res = LiveEv.WaitOne(10000);
            //if (res == false) {
            //    exx = new Exception("Board " + Address + ": Run Timeout Error");
            //}
            //if (exx != null) throw exx;
        }

        public override NodeRecipe GetParameters() {

            EoptisDeviceBase d = Stations.First().Cameras.First() as EoptisDeviceBase;
            if (currentEoptisNodeParameters == null)
                currentEoptisNodeParameters = EoptisSvc.ChooseParameters(d.CameraType);
            PollEv.Set();
            if (!Connected) {
                throw new Exception("Board " + Address + ": GetParameters Error. Board not connected");
                //return currentNodeParameters;
            }
            //d.CurrWorkingMode = d.MapWorkingMode(workingMode);
            //foreach (EoptisStationBase s in Stations)
            //    foreach (EoptisDeviceBase d in s.Cameras)
            //        d.CurrWorkingMode = d.MapWorkingMode(workingMode);

            if (!hardGetRequested) {
                return currentNodeParameters;
            }
            bool lockRes = Lock();
            if (!lockRes) {
                Log.Line(LogLevels.Warning, "EoptisNodeBase.GetParameters", "Board " + Address + ": Error while resetting board ready bit");
                return currentNodeParameters;
            }
            Exception exx = null;
            try {
                //PollEv.Reset();
                //EoptisSvc.PollEv.Reset();
                currentNodeParameters = new NodeRecipe();
                currentNodeParameters.Id = IdNode;
                currentNodeParameters.Description = "Eoptis Device parameters";
                currentNodeParameters.Stations = new List<StationRecipe>();

                //currentEoptisNodeParameters = EoptisSvc.ChooseParameters(d.CameraType);
                GetParametersEv.Reset();
                EoptisSvc.GetParameters(Address, ref currentEoptisNodeParameters);
                bool res = GetParametersEv.WaitOne(10000);
                if (res == false) {
                    exx = new Exception("Board " + Address + ": GetParameters Timeout Error");
                }
                if (exx == null) {

                    hardGetRequested = false;
                    Version = (currentEoptisNodeParameters[SYSTEM_ID.INFO_FIRMWARE_VERSION]!=null) ? 
                        "FPGA: " + currentEoptisNodeParameters[SYSTEM_ID.INFO_FIRMWARE_VERSION].Value : "?";
                    foreach (IStation s in Stations) {
                        StationRecipe sr = new StationRecipe();
                        sr.Cameras = new List<CameraRecipe>();
                        foreach (ICamera c in s.Cameras) {
                            //c.Address = Address;
                            CameraRecipe cr = new CameraRecipe();
                            c.GetWorkingMode();
                            cr.AcquisitionParameters = c.GetAcquisitionParameters();
                            unfoldParameters(cr.AcquisitionParameters, currentEoptisNodeParameters);
                            //cr.AcquisitionParameters["FAKE_PARAM_BOOL"].Value = true.ToString();
                            //cr.AcquisitionParameters["FAKE_PARAM_DOUBLE"].Value = "105.7";
                            //cr.AcquisitionParameters["FAKE_PARAM_STRING"].Value = "FORZA JUVE!";
                            //cr.AcquisitionParameters["FAKE_PARAM_COMBO_STRING"].Value = "PAPERINO";
                            cr.DigitizerParameters = c.GetDigitizerParameters();
                            unfoldParameters(cr.DigitizerParameters, currentEoptisNodeParameters);
                            cr.RecipeSimpleParameters = c.GetRecipeSimpleParameters();
                            unfoldParameters(cr.RecipeSimpleParameters, currentEoptisNodeParameters);
                            cr.RecipeAdvancedParameters = c.GetRecipeAdvancedParameters();
                            unfoldParameters(cr.RecipeAdvancedParameters, currentEoptisNodeParameters);
                            cr.MachineParameters = c.GetMachineParameters();
                            unfoldParameters(cr.MachineParameters, currentEoptisNodeParameters);
                            sr.Cameras.Add(cr);
                        }
                        sr.Tools = new List<Tool>();
                        Tool newTool = new Tool {
                            Id = 0,
                            Label = d.CameraType,
                            Active = true
                        };
                        //newTool.ToolParameters.Add(new Parameter {
                        //    Id = "Active",
                        //    Value = true.ToString()
                        //});
                        newTool.ToolOutputs = new ParameterCollection<ToolOutput>();
                        List<SYSTEM_ID> resList = null;
                        if (d.CameraType == "EoptisSpectrometer")
                            resList = EoptisSvc.SpectrometerResultsId;
                        if (d.CameraType == "EoptisTurbidimeter")
                            resList = EoptisSvc.OpacimetroResultsId;
                        for (int i = 0; i < resList.Count; i++) {
                            SYSTEM_ID sysID = resList[i];
                            bool enableGraph = (sysID == SYSTEM_ID.PAR_FULL_SPECTRUM_BUFFER || 
                                sysID == SYSTEM_ID.PAR_FULL_SPECTRUM_MIS || 
                                sysID == SYSTEM_ID.PAR_FULL_SPECTRUM_ELAB || 
                                sysID == SYSTEM_ID.LOCKIN_AMPLITUDE_POST_CALIBRATION) ? true : false;
                            newTool.ToolOutputs.Add(new ToolOutput {
                                ParamId = i.ToString(),
                                Id = sysID.ToString(),
                                //Label = sysID.ToString(),
                                //MeasureUnit = "",
                                IsUsed = true,
                                ValueType = "string",
                                //Value = ""
                                EnableGraph = enableGraph,
                            });
                        }
                        newTool.ToolOutputs.PopulateParametersInfo();
                        sr.Tools.Add(newTool);
                        currentNodeParameters.Stations.Add(sr);
                    }
                }
            }
            catch (Exception ex) {
                exx = new Exception("Error: " + ex.Message);
            }
            lockRes = Unlock();
            //PollEv.Set();
            //EoptisSvc.PollEv.Set();
            if (!lockRes) {
                Log.Line(LogLevels.Warning, "EoptisNodeBase.GetParameters", "Board " + Address + ": Error while setting board ready bit");
            }
            if (exx != null) {
                _uploading = false;
                throw exx;
            }
            if (_uploading == true)
                OnNodeRecipeUpdate(this, new NodeRecipeEventArgs(currentNodeParameters, false));
            return currentNodeParameters;
        }

        bool _uploading = false;

        public override NodeRecipe UploadParameters() {

            _uploading = true;
            hardGetRequested = true;
            NodeRecipe NRres = GetParameters();
            Run(0);
            Parameter refSpectrum = NRres.Stations[0].Cameras.First().RecipeSimpleParameters.Find(pp => pp.Id == "PAR_FULL_SPECTRUM_REF_NORM");
            if (refSpectrum != null) {
                lock (lastEoptisDataCollection) {
                    MeasureResults mr = lastEoptisDataCollection[0].ResToolCollection[0].ResMeasureCollection.Find(mm => mm.MeasureName == "PAR_FULL_SPECTRUM_MIS");
                    refSpectrum.Value = mr.MeasureValue;
                }
            }
            OnNodeRecipeUpdate(this, new NodeRecipeEventArgs(currentNodeParameters, true));
            _uploading = false;
            return NRres;
        }

        public override void SetParameters(string recipeName, NodeRecipe _dataSource, ParameterInfoCollection pic, string cultureCode) {

            bool lockRes = Lock();
            if (!lockRes || !Connected) {
                Log.Line(LogLevels.Warning, "EoptisNodeBase.SetParameters", "Board " + Address + ": Error while resetting board ready bit");
                throw new Exception("Client " + Address + " not connected or not ready to receive parameters!");
            }
            Exception exx = null;
            try {
                //PollEv.Reset();
                //EoptisSvc.PollEv.Reset();
                SetParametersDone.Reset();
                //SetParametersCompletedSuccessfully = false;
                // Matteo - 27/06/2024.
                SetParametersStatus = SetParametersStatusEnum.Uploading;

                dataSource = _dataSource.Clone(pic, cultureCode);
                _cultureCode = cultureCode;
                _pic = pic;
                if (Connected) {    //si può togliere, verifica fatta prima
                    Log.Line(LogLevels.Pass, "EoptisNodeBase.SetParameters", "Board " + Address + ": Sending recipe...");
                    EoptisParameterCollection paramsToSend = new EoptisParameterCollection();
                    foreach (IStation s in Stations) {
                        StationRecipe sr = dataSource.Stations[s.IdStation];
                        foreach (ICamera c in s.Cameras) {
                            CameraRecipe cr = sr.Cameras.First();
                            foldParameters(cr.AcquisitionParameters, paramsToSend);
                            foldParameters(cr.DigitizerParameters, paramsToSend);
                            foldParameters(cr.RecipeSimpleParameters, paramsToSend);
                            foldParameters(cr.RecipeAdvancedParameters, paramsToSend);
                            foldParameters(cr.MachineParameters, paramsToSend);
                            //c.SetAcquisitionParameters(cr.AcquisitionParameters);
                            //c.SetDigitizerParameters(cr.DigitizerParameters);
                            //c.SetRecipeSimpleParameters(cr.RecipeSimpleParameters);
                            //c.SetRecipeAdvancedParameters(cr.RecipeAdvancedParameters);
                            //c.SetMachineParameters(cr.MachineParameters);
                        }
                    }
                    EoptisSvc.SetParameters(Address, paramsToSend);
                    hardGetRequested = true;
                    // AUDIT
                    NodeRecipe prevNodeParameters = currentNodeParameters.Clone(_pic, _cultureCode);
                    GetParameters();
                    List<ParameterDiff> paramDiffList = new List<ParameterDiff>();
                    bool comparisonRes = currentNodeParameters.Compare(prevNodeParameters, _cultureCode, "", paramDiffList);
                    if ((comparisonRes == true) && (paramDiffList.Count > 0)) {
                        foreach (ParameterDiff parDiff in paramDiffList) {
                            string message = Description + " - " + Stations.First().Description + " - " + Stations.First().Cameras.First().CameraDescription + " Parameter: \"" + parDiff.ParameterLocLabel + "\" Changed from: \"" + parDiff.ComparedValue + "\" to: \"" + parDiff.CurrentValue + "\"";
                            OnClientAuditMessage(this, new DisplayManager.MessageEventArgs(message));
                        }
                    }
                    prevNodeParameters = currentNodeParameters.Clone(_pic, _cultureCode);
                    ///
                    //SetParametersCompletedSuccessfully = true;
                    // Matteo - 27/06/2024.
                    SetParametersStatus = SetParametersStatusEnum.UploadedOK;
                    SetParametersDone.Set();
                }
                else {
                    Log.Line(LogLevels.Error, "EoptisNodeBase.SetParameters", "Board " + Address + ": not connected or not ready to receive receipt!");
                    exx = new Exception("Board " + Address + ": not connected or not ready to receive receipt!");
                }
            }
            catch (Exception ex) {
                exx = new Exception("Error: " + ex.Message);
            }
            lockRes = Unlock();
            //PollEv.Set();
            //EoptisSvc.PollEv.Set();
            if (!lockRes) {
                Log.Line(LogLevels.Warning, "EoptisNodeBase.SetParameters", "Board " + Address + ": Error while setting board ready bit");
            }
            if (exx != null) throw exx;
        }

        private void unfoldParameters(ParameterCollection<Parameter> dest, EoptisParameterCollection src) {

            foreach (Parameter destParam in dest) {
                EoptisParameter srcParam = src.Find(pp => pp.Id.ToString() == destParam.Id);
                try {
                    if (destParam.Id.Contains("FAKE"))
                        srcParam.Value = destParam.Value;
                    else
                        destParam.Value = (srcParam != null) ? srcParam.Value : "";
                }
                catch (Exception ex) {
                    Log.Line(LogLevels.Warning, "EoptisNodeBase.unfoldParameters", "Board " + Address + ": " + ex.Message);
                }
            }
        }

        private void foldParameters(ParameterCollection<Parameter> src, EoptisParameterCollection dest) {

            foreach (Parameter srcParam in src) {
                SYSTEM_ID id;
                if (Enum.TryParse(srcParam.Id, out id)) {
                    EoptisParameter destParam = new EoptisParameter(id, EoptisSvc.ParamsDictionary.Parameters[id].Type, EoptisSvc.ParamsDictionary.Parameters[id].ByteLength);
                    destParam.Value = srcParam.Value;
                    dest.Add(destParam);
                }
            }
        }

        public override void StartImagesDump(List<StationDumpSettings> statDumpSettingsCollection) {

            //PollEv.Set();
        }

        public override void StartImagesDump2(List<StationDumpSettings2> statDumpSettingsCollection)
        {
            //
        }

        public override void StopImagesDump() {

            //PollEv.Reset();
        }

        public override void StopImagesDump2()
        {
            //
        }



        public override bool Lock() {

            //if (EoptisSvc.isOffline) {
            //    return true;
            //}
            if (Stations.Count == 0 || Stations.First().Cameras.Count == 0) {
                Log.Line(LogLevels.Warning, "EoptisNodeBase.Lock", Address + ": Initialization error");
                return false;
            }
            //CameraWorkingMode workMode = (Stations.First().Cameras.First() as EoptisDeviceBase).CurrWorkingMode;
            if (workingMode == EoptisWorkingMode.WORKING_MODE_CONTROL) {
                Log.Line(LogLevels.Warning, "EoptisNodeBase.Lock", Address + ": Cannot lock in this working mode: " + workingMode.ToString());
                return false;
            }
            //else {
            //    try {
            //        PollEv.Reset();
            //        EoptisSvc.PollEv.Reset();
            //        EoptisSvc.SetParameter(Address, new EoptisParameter(SYSTEM_ID.OUT_READY), false.ToString());
            //        PollEv.Set();
            //        EoptisSvc.PollEv.Set();
            //    }
            //    catch (Exception ex) {
            //        Log.Line(LogLevels.Warning, "EoptisNodeBase.Lock", Address + ": " + ex.Message);
            //        return false;
            //    }
            //}
            return true;
        }

        public override bool Unlock() {

            //if (EoptisSvc.isOffline) {
            //    return true;
            //}
            if (Stations.Count == 0 || Stations.First().Cameras.Count == 0) {
                Log.Line(LogLevels.Warning, "EoptisNodeBase.Unlock", Address + ": Initialization error");
                return false;
            }
            //CameraWorkingMode workMode = (Stations.First().Cameras.First() as EoptisDeviceBase).CurrWorkingMode;
            if (workingMode == EoptisWorkingMode.WORKING_MODE_CONTROL) {
                Log.Line(LogLevels.Warning, "EoptisNodeBase.Unlock", Address + ": Cannot unlock in this working mode: " + workingMode.ToString());
                return false;
            }
            //else {
            //    try {
            //        PollEv.Reset();
            //        EoptisSvc.PollEv.Reset();
            //        EoptisSvc.SetParameter(Address, new EoptisParameter(SYSTEM_ID.OUT_READY), true.ToString());
            //        PollEv.Set();
            //        EoptisSvc.PollEv.Set();
            //    }
            //    catch (Exception ex) {
            //        Log.Line(LogLevels.Warning, "EoptisNodeBase.Unlock", Address + ": " + ex.Message);
            //        return false;
            //    }
            //}
            return true;
        }

        public override void SaveBufferedImages(string path, SaveConditions sc, int toSave) {

            foreach (IStation s in Stations)
                s.SaveBufferedImages(path, sc, toSave);
        }
    }

    public class EoptisNodeBaseCollection : List<EoptisNodeBase2> {

        public EoptisNodeBase2 this[EoptisNodeBase2 node] {
            get { return Find(n => n.Address == node.Address); }
        }

        public EoptisNodeBase2 this[string ip] {
            get { return Find(n => n.Address == ip); }
        }
    }

    public class EoptisDev {

        public string LocalIpAddress { get; internal set; }
        public string RemoteName { get; internal set; }
        public string RemoteIpAddress { get; internal set; }
        public int RemotePort { get; internal set; }
    }

    public class EoptisDevCollection : List<EoptisDev> {

        public EoptisDev this[EoptisDev node] {
            get { return Find(n => n.RemoteIpAddress == node.RemoteIpAddress); }
        }
    }
}
