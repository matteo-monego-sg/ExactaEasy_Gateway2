using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using DisplayManager;
using ExactaEasyCore;
using SPAMI.Util.Logger;

namespace EoptisClient {

    public class EoptisNodeBase : Node {

        private static readonly EoptisDevCollection EoptisDevCollection = new EoptisDevCollection();
        private static readonly EoptisNodeBaseCollection EoptisClients = new EoptisNodeBaseCollection();
        private static readonly object EoLock = new object();
        private NodeRecipe DataSource;                  //parametri di ricetta
        private NodeRecipe _actualNodeParameters;       //parametri device
        private ParameterInfoCollection _pic;
        private string _cultureCode;
        private Thread _mexDequeuerTh;
        private WaitHandle[] _mexDequeuerHandles;
        private ManualResetEvent KillEv = new ManualResetEvent(false);
        private ManualResetEvent PollEv = new ManualResetEvent(false);
        internal EoptisWorkingMode workingMode = EoptisWorkingMode.UNKNOWN;
        private ManualResetEvent initWorkingModeSetEv = new ManualResetEvent(false);
        bool hardGetRequested = true;
        //bool exit = false;
        //AutoResetEvent exitEvt = new AutoResetEvent(false);
        //Thread diagThread;

        public EoptisNodeBase(NodeDefinition nodeDefinition)
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
                        Connected = true;
                        PollEv.Set();
                        initWorkingModeSetEv.WaitOne(5000);
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
                if (res == 0)
                    Connected = false;
                else
                    exx = new Exception("Connection with node " + Address + " failed. Error: " + res.ToString());
            }
            catch (Exception ex) {
                exx = new Exception("Disconnection from node " + Address + " failed. Error: " + ex.Message);
            }
            if (exx != null)
                throw exx;
        }

        void MexDequeuer() {

            while (true) {
                var retVal = WaitHandle.WaitAny(_mexDequeuerHandles);
                if (retVal == 0) break; //killEv
                EoptisInspectionResults eir;
                bool newRes = EoptisSvc.ReadResults(Address, IdNode, out eir);
                //diagnosticTimer.Restart();
                bool newMeasures = false;
                if (newRes) {
                    foreach (EoptisStationBase s in Stations)
                        foreach (EoptisDeviceBase d in s.Cameras)
                            d.CurrWorkingMode = d.MapWorkingMode(eir.WorkingMode);
                    workingMode = eir.WorkingMode;
                    initWorkingModeSetEv.Set();
                    Log.Line(LogLevels.Debug, "EoptisNodeBase.MexDequeuer", Address + ": VIAL ID = " + eir.VialId);
                    Log.Line(LogLevels.Debug, "EoptisNodeBase.MexDequeuer", Address + ": SPINDLE ID = " + eir.SpindleId);
                    //Log.Line(LogLevels.Debug, "EoptisNodeBase.MexDequeuer", Address + ": VALUE = " + eir.ResToolCollection[0].ResMeasureCollection[0].MeasureValue);
                    Log.Line(LogLevels.Debug, "EoptisNodeBase.MexDequeuer", Address + ": IS REJECT = " + eir.IsReject);
                    Log.Line(LogLevels.Debug, "EoptisNodeBase.MexDequeuer", Address + ": REJECTION CAUSE = " + eir.RejectionCause);
                    foreach (MeasureResults measRes in eir.ResToolCollection[0].ResMeasureCollection) {
                        Log.Line(LogLevels.Debug, "EoptisNodeBase.MexDequeuer", Address + ": " + measRes.MeasureName + " = " + measRes.MeasureValue);
                        if (!string.IsNullOrEmpty(measRes.MeasureValue))
                            newMeasures = true;
                    }
                    if (newMeasures)
                        OnClientInspectionResults(new EoptisInspectionResultsEventArgs(Address, IdNode, new List<EoptisInspectionResults>() { eir }));
                }
                Thread.Sleep(50);
            }
        }

        private void OnClientInspectionResults(EoptisInspectionResultsEventArgs e) {

            foreach (EoptisInspectionResults eir in e.InspectionResultsCollection) {
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

        public override NodeRecipe GetParameters() {

            foreach (EoptisStationBase s in Stations)
                foreach (EoptisDeviceBase d in s.Cameras)
                    d.CurrWorkingMode = d.MapWorkingMode(workingMode);

            if (!hardGetRequested)
                return _actualNodeParameters;
            bool lockRes = Lock();
            if (!lockRes) {
                Log.Line(LogLevels.Warning, "EoptisNodeBase.GetParameters", "Board " + Address + ": Error while resetting board ready bit");
                return _actualNodeParameters;
            }
            Exception exx = null;
            try {
                PollEv.Reset();
                EoptisSvc.PollEv.Reset();
                _actualNodeParameters = new NodeRecipe();
                _actualNodeParameters.Id = IdNode;
                _actualNodeParameters.Description = "Device parameters";
                _actualNodeParameters.Stations = new List<StationRecipe>();

                EoptisParameterCollection nodeParameters = chooseParameters();
                EoptisSvc.GetParameters(Address, ref nodeParameters);
                hardGetRequested = false;

                foreach (IStation s in Stations) {
                    StationRecipe sr = new StationRecipe();
                    sr.Cameras = new List<CameraRecipe>();
                    foreach (ICamera c in s.Cameras) {
                        //c.Address = Address;
                        CameraRecipe cr = new CameraRecipe();
                        c.GetWorkingMode();
                        cr.AcquisitionParameters = c.GetAcquisitionParameters();
                        unfoldParamaters(cr.AcquisitionParameters, nodeParameters);
                        cr.FeaturesEnableParameters = c.GetFeaturesEnableParameters();
                        unfoldParamaters(cr.FeaturesEnableParameters, nodeParameters);
                        cr.RecipeSimpleParameters = c.GetRecipeSimpleParameters();
                        unfoldParamaters(cr.RecipeSimpleParameters, nodeParameters);
                        cr.RecipeAdvancedParameters = c.GetRecipeAdvancedParameters();
                        unfoldParamaters(cr.RecipeAdvancedParameters, nodeParameters);
                        cr.MachineParameters = c.GetMachineParameters();
                        unfoldParamaters(cr.MachineParameters, nodeParameters);

                        sr.Cameras.Add(cr);
                    }
                    _actualNodeParameters.Stations.Add(sr);
                }
            }
            catch (Exception ex) {
                exx = new Exception("Error: " + ex.Message);
            }
            lockRes = Unlock();
            PollEv.Set();
            EoptisSvc.PollEv.Set();
            if (!lockRes) {
                Log.Line(LogLevels.Warning, "EoptisNodeBase.GetParameters", "Board " + Address + ": Error while setting board ready bit");
            }
            if (exx != null) throw exx;
            return _actualNodeParameters;
        }

        public override void SetParameters(string recipeName, NodeRecipe dataSource, ParameterInfoCollection pic, string cultureCode) {

            bool lockRes = Lock();
            if (!lockRes || !Connected) {
                Log.Line(LogLevels.Warning, "EoptisNodeBase.SetParameters", "Board " + Address + ": Error while resetting board ready bit");
                throw new Exception("Client " + Address + " not connected or not ready to receive parameters!");
            }
            Exception exx = null;
            try {
                PollEv.Reset();
                EoptisSvc.PollEv.Reset();
                SetParametersDone.Reset();
                SetParametersCompletedSuccessfully = false;
                DataSource = dataSource.Clone(pic, cultureCode);
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
                            foldParameters(cr.FeaturesEnableParameters, paramsToSend);
                            foldParameters(cr.RecipeSimpleParameters, paramsToSend);
                            foldParameters(cr.RecipeAdvancedParameters, paramsToSend);
                            foldParameters(cr.MachineParameters, paramsToSend);
                            c.SetAcquisitionParameters(cr.AcquisitionParameters);
                            c.SetFeaturesEnableParameters(cr.FeaturesEnableParameters);
                            c.SetRecipeSimpleParameters(cr.RecipeSimpleParameters);
                            c.SetRecipeAdvancedParameters(cr.RecipeAdvancedParameters);
                            c.SetMachineParameters(cr.MachineParameters);
                            //c.SetROIParameters(cr.ROIParameters, 
                            //c.SetStrobeParameters
                        }
                    }
                    //EoptisSvc.SetParameters(Address, paramsToSend);   // NON FUNZIONA...
                    hardGetRequested = true;
                    SetParametersCompletedSuccessfully = true;
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
            PollEv.Set();
            EoptisSvc.PollEv.Set();
            if (!lockRes) {
                Log.Line(LogLevels.Warning, "EoptisNodeBase.SetParameters", "Board " + Address + ": Error while setting board ready bit");
            }
            if (exx != null) throw exx;
        }

        private void unfoldParamaters(ParameterCollection<Parameter> dest, EoptisParameterCollection src) {

            foreach (Parameter destParam in dest) {
                EoptisParameter srcParam = src.Find(pp => pp.Id.ToString() == destParam.Id);
                destParam.Value = srcParam.Value;
            }
        }

        private void foldParameters(ParameterCollection<Parameter> src, EoptisParameterCollection dest) {

            foreach (Parameter srcParam in src) {
                SYSTEM_ID id;
                if (Enum.TryParse(srcParam.Id, out id)) {
                    EoptisParameter destParam = new EoptisParameter(id);
                    destParam.Size = EoptisSvc.Params.Parameters[id].Size;
                    destParam.Type = EoptisSvc.Params.Parameters[id].Type;
                    destParam.Value = srcParam.Value;
                    dest.Add(destParam);
                }
            }
        }

        private EoptisParameterCollection chooseParameters() {

            EoptisParameterCollection parameters = new EoptisParameterCollection();
            parameters.Add(EoptisSvc.Params.Parameters[SYSTEM_ID.PAR_LASER_GAIN]);
            parameters.Add(EoptisSvc.Params.Parameters[SYSTEM_ID.PAR_VIAL_GAIN]);
            parameters.Add(EoptisSvc.Params.Parameters[SYSTEM_ID.TH_WARNING_UPPER]);
            parameters.Add(EoptisSvc.Params.Parameters[SYSTEM_ID.TH_WARNING_LOWER]);
            parameters.Add(EoptisSvc.Params.Parameters[SYSTEM_ID.TH_ERROR_UPPER]);
            parameters.Add(EoptisSvc.Params.Parameters[SYSTEM_ID.TH_ERROR_LOWER]);
            parameters.Add(EoptisSvc.Params.Parameters[SYSTEM_ID.PAR_SPINDLE_COUNT]);
            parameters.Add(EoptisSvc.Params.Parameters[SYSTEM_ID.PAR_0_TORRETTA_OFFSET]);
            parameters.Add(EoptisSvc.Params.Parameters[SYSTEM_ID.PAR_INCREMENTO_SPINDLE_ID]);
            parameters.Add(EoptisSvc.Params.Parameters[SYSTEM_ID.PAR_FREE_RUN_TIMER]);
            return parameters;
        }

        //void diagnosticThread() {
        //    int counter = 0;
        //    while (true) {
        //        if (exitEvt.WaitOne(10))
        //            break;
        //        Debug.WriteLine("CNT = " + counter.ToString());
        //        counter++;
        //    }
        //}

        public override void StartImagesDump(List<StationDumpSettings> statDumpSettingsCollection) {

            //PollEv.Set();
        }

        public override void StopImagesDump() {

            //PollEv.Reset();
        }

        public override bool Lock() {

            if (EoptisSvc.isOffline) {
                return true;
            }
            if (Stations.Count == 0 || Stations.First().Cameras.Count == 0) {
                Log.Line(LogLevels.Warning, "EoptisNodeBase.Lock", Address + ": Initialization error");
                return false;
            }
            //CameraWorkingMode workMode = (Stations.First().Cameras.First() as EoptisDeviceBase).CurrWorkingMode;
            if (workingMode == EoptisWorkingMode.WORKING_MODE_CONTROL) {
                Log.Line(LogLevels.Warning, "EoptisNodeBase.Lock", Address + ": Cannot lock in this working mode: " + workingMode.ToString());
                return false;
            }
            else {
                try {
                    PollEv.Reset();
                    EoptisSvc.PollEv.Reset();
                    EoptisSvc.SetParameter(Address, new EoptisParameter(SYSTEM_ID.OUT_READY), false.ToString());
                    PollEv.Set();
                    EoptisSvc.PollEv.Set();
                }
                catch (Exception ex) {
                    Log.Line(LogLevels.Warning, "EoptisNodeBase.Lock", Address + ": " + ex.Message);
                    return false;
                }
            }
            return true;
        }

        public override bool Unlock() {

            if (EoptisSvc.isOffline) {
                return true;
            }
            if (Stations.Count == 0 || Stations.First().Cameras.Count == 0) {
                Log.Line(LogLevels.Warning, "EoptisNodeBase.Unlock", Address + ": Initialization error");
                return false;
            }
            //CameraWorkingMode workMode = (Stations.First().Cameras.First() as EoptisDeviceBase).CurrWorkingMode;
            if (workingMode == EoptisWorkingMode.WORKING_MODE_CONTROL) {
                Log.Line(LogLevels.Warning, "EoptisNodeBase.Unlock", Address + ": Cannot unlock in this working mode: " + workingMode.ToString());
                return false;
            }
            else {
                try {
                    PollEv.Reset();
                    EoptisSvc.PollEv.Reset();
                    EoptisSvc.SetParameter(Address, new EoptisParameter(SYSTEM_ID.OUT_READY), true.ToString());
                    PollEv.Set();
                    EoptisSvc.PollEv.Set();
                }
                catch (Exception ex) {
                    Log.Line(LogLevels.Warning, "EoptisNodeBase.Unlock", Address + ": " + ex.Message);
                    return false;
                }
            }
            return true;
        }
    }

    public class EoptisNodeBaseCollection : List<EoptisNodeBase> {

        public EoptisNodeBase this[EoptisNodeBase node] {
            get { return Find(n => n.Address == node.Address); }
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
