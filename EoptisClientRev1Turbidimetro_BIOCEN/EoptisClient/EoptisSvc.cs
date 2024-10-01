using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using DisplayManager;
using SPAMI.Util.Logger;
using System.Diagnostics;

namespace EoptisClient {
    public static class EoptisSvc {

        internal static bool isOffline = false;
        static uint dummyVialCounter = 0;

        internal static EoptisParameters Params;
        internal static byte[] rxBuffer = new byte[1024 * 1024];
        private static readonly object EoptisMng = new object();

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        internal unsafe struct VariantDatumStruct {

            internal SYSTEM_ID id_;
            internal uint length_;
            internal SYSTEM_TYPE type_;
            internal IntPtr data_;
        }

        internal class VariantDatum {

            internal VariantDatumStruct vds;
            //public byte[] Data { get; internal set; }

            internal VariantDatum(SYSTEM_ID id, int length, SYSTEM_TYPE type) {

                vds.id_ = id;
                vds.length_ = (uint)length;
                vds.type_ = type;
                vds.data_ = Marshal.AllocHGlobal((int)vds.length_);
                //Data = new byte[length];
                //Marshal.Copy(vds.data_, Data, 0, Data.Length);
            }

            internal VariantDatum(VariantDatumStruct varData)
                : this(varData.id_, (int)varData.length_, varData.type_) {
            }

            internal void Free() {

                Marshal.FreeHGlobal(vds.data_);
            }

            //internal byte[] DataToBuffer() {

            //    int length = (int)vds.length_;
            //    byte[] res = new byte[length];
            //    Marshal.Copy(vds.data_, res, 0, length);
            //    return res;
            //}
        }

        internal unsafe struct DataChunkStruct {

            internal uint totData_;
            internal IntPtr data_;
        }

        internal unsafe class DataChunk {

            internal DataChunkStruct dcs;
            internal VariantDatum[] vdArray;

            internal DataChunk(EoptisParameterCollection epc) {

                dcs = new DataChunkStruct();
                dcs.totData_ = (uint)epc.Count;
                int vdsSize = Marshal.SizeOf(typeof(VariantDatumStruct));
                dcs.data_ = Marshal.AllocHGlobal(vdsSize * epc.Count);
                vdArray = new VariantDatum[epc.Count];
                for (int i = 0; i < dcs.totData_; i++) {
                    vdArray[i] = new VariantDatum(epc[i].Id, epc[i].Size, epc[i].Type);
                    IntPtr ptr = new IntPtr(dcs.data_.ToInt64() + i * vdsSize);
                    //VariantDatumStruct vds = (VariantDatumStruct)Marshal.PtrToStructure(ptr, typeof(VariantDatumStruct));
                    //vds.id_ = vdArray[i].vds.id_;
                    //vds.type_ = vdArray[i].vds.type_;
                    //vds.length_ = vdArray[i].vds.length_;
                    //vds.data_ = vdArray[i].vds.data_;
                    Marshal.StructureToPtr(vdArray[i].vds, ptr, false);
                }
            }

            internal void Free() {

                if (vdArray != null) {
                    for (int i = 0; i < vdArray.Length; i++) {
                        vdArray[i].Free();
                    }
                }
                Marshal.FreeHGlobal(dcs.data_);
            }
        }

        //internal class DataChunk {

        //    public int Size { get; internal set; }
        //    public List<VariantDatum> VarDataCollection { get; internal set; }

        //    internal DataChunk() {

        //        VarDataCollection = new List<VariantDatum>();
        //    }

        //    void Free() {
        //        Size = 0;
        //        foreach (VariantDatum vd in VarDataCollection)
        //            vd.Free();
        //        VarDataCollection = null;
        //    }
        //}

        [DllImport("EoptisDeviceManager.dll", CharSet = CharSet.Ansi)]
        internal static extern int connectDevice(string ip, int port, bool activeCallback);
        [DllImport("EoptisDeviceManager.dll", CharSet = CharSet.Ansi)]
        internal static extern int disconnect(string ip);
        [DllImport("EoptisDeviceManager.dll", CharSet = CharSet.Ansi)]
        internal static unsafe extern int getParameter(string ip, ref VariantDatumStruct param);
        [DllImport("EoptisDeviceManager.dll", CharSet = CharSet.Ansi)]
        internal static unsafe extern int setParameter(string ip, ref VariantDatumStruct param);
        [DllImport("EoptisDeviceManager.dll", CharSet = CharSet.Ansi)]
        internal static unsafe extern int getParameters(string ip, ref DataChunkStruct parameters);
        [DllImport("EoptisDeviceManager.dll", CharSet = CharSet.Ansi)]
        internal static unsafe extern int setParameters(string ip, ref DataChunkStruct parameters);
        [DllImport("EoptisDeviceManager.dll", CharSet = CharSet.Ansi)]
        internal static unsafe extern int setWorkingMode(string ip, int wm);
        [DllImport("EoptisDeviceManager.dll", CharSet = CharSet.Ansi)]
        internal static unsafe extern int getWorkingMode(string ip, ref int wm);

        [DllImport("EoptisDeviceManager.dll", CharSet = CharSet.Ansi)]
        internal static unsafe extern void receiveData(string ip, ref DataChunkStruct dataChunk);
        [DllImport("EoptisDeviceManager.dll", CharSet = CharSet.Ansi)]
        internal static unsafe extern void freeData(ref DataChunkStruct dataChunk);

        //[DllImport("EoptisDeviceManager.dll", CharSet = CharSet.Ansi)]
        //internal static extern int getAllInfo(DataChunk chunk);
        //[DllImport("EoptisDeviceManager.dll", CharSet = CharSet.Ansi)]
        //internal static unsafe extern int getParamDummy(string ip, ref VariantDatumStruct param);
        //[DllImport("EoptisDeviceManager.dll", CharSet = CharSet.Ansi)]
        //internal static unsafe extern int getParamsDummy(string ip, ref DataChunkStruct parameters);

        private static Thread mainResThread;
        private static Dictionary<string, ConcurrentQueue<EoptisInspectionResults>> resultsQueues = new Dictionary<string, ConcurrentQueue<EoptisInspectionResults>>();
        private static Dictionary<string, EoptisWorkingMode> workingModes = new Dictionary<string, EoptisWorkingMode>();
        private static WaitHandle[] _mexDequeuerHandles;
        private static ManualResetEvent KillEv = new ManualResetEvent(false);
        internal static ManualResetEvent PollEv = new ManualResetEvent(false);

        static EoptisSvc() {

            Params = EoptisParameters.LoadFromFile("EoptisParameters.xml");
            if (Params == null)
                throw new Exception("File not found: EoptisParameters.xml");

            KillEv.Reset();
            PollEv.Reset();
            if (_mexDequeuerHandles == null) {
                _mexDequeuerHandles = new WaitHandle[2];
                _mexDequeuerHandles[0] = KillEv;
                _mexDequeuerHandles[1] = PollEv;
            }
            if (mainResThread == null || !mainResThread.IsAlive) {
                mainResThread = new Thread(MainResThread) {
                    Name = "Eoptis Main Results Thread"
                };
                mainResThread.Start();
                mainResThread.Priority = ThreadPriority.Normal;
            }
        }

        internal static unsafe int Connect(string ip, int port, bool activeCallback) {

            if (isOffline) {
                Thread.Sleep(200);
                return 0;
            }
            int res = connectDevice(ip, port, activeCallback);
            lock (EoptisMng) {
                if (!resultsQueues.ContainsKey(ip)) {
                    resultsQueues.Add(ip, new ConcurrentQueue<EoptisInspectionResults>());
                }
                if (!workingModes.ContainsKey(ip)) {
                    workingModes.Add(ip, EoptisWorkingMode.UNKNOWN);
                }
            }
            return res;
        }

        internal static int Disconnect(string ip) {

            lock (EoptisMng) {
                if (resultsQueues.ContainsKey(ip)) {
                    resultsQueues.Remove(ip);
                }
                if (workingModes.ContainsKey(ip)) {
                    workingModes.Remove(ip);
                }
            }
            if (isOffline) {
                Thread.Sleep(200);
                return 0;
            }
            return disconnect(ip);
        }

        public static void Test(string ip) {

            string srNum, fwVer, sdkVer, ipAddr, portRead, fpgaVer, spindleCount, workingMode, parLedGreenPower;
            srNum = GetParameter(ip, new EoptisParameter(SYSTEM_ID.INFO_SERIAL_NUMBER));
            fwVer = GetParameter(ip, new EoptisParameter(SYSTEM_ID.INFO_FIRMWARE_VERSION));
            sdkVer = GetParameter(ip, new EoptisParameter(SYSTEM_ID.INFO_SDK_VERSION));
            ipAddr = GetParameter(ip, new EoptisParameter(SYSTEM_ID.INFO_IP_ADDRESS));
            //uint portRead, fpgaVer;
            portRead = GetParameter(ip, new EoptisParameter(SYSTEM_ID.INFO_PORT));
            fpgaVer = GetParameter(ip, new EoptisParameter(SYSTEM_ID.INFO_FPGA_VERSION));
            //int spindleCount, workingMode;
            spindleCount = GetParameter(ip, new EoptisParameter(SYSTEM_ID.PAR_SPINDLE_COUNT));
            int spindleC = Convert.ToInt32(spindleCount);
            SetParameter(ip, new EoptisParameter(SYSTEM_ID.PAR_SPINDLE_COUNT), (spindleC + 1).ToString());
            spindleCount = GetParameter(ip, new EoptisParameter(SYSTEM_ID.PAR_SPINDLE_COUNT));
            workingMode = GetParameter(ip, new EoptisParameter(SYSTEM_ID.PAR_WORKING_MODE));
            //bool parLedGreenPower;
            parLedGreenPower = GetParameter(ip, new EoptisParameter(SYSTEM_ID.PAR_LED_GREEN_POWER));
            bool led = Convert.ToBoolean(parLedGreenPower);
            SetParameter(ip, new EoptisParameter(SYSTEM_ID.PAR_LED_GREEN_POWER), (!led).ToString());

            //(ip, (int)WorkingMode.WORKING_MODE_FREE_RUN);
            //setWorkingMode(ip, (int)WorkingMode.WORKING_MODE_CONTROL);

            //DataChunkStruct dcs = new DataChunkStruct();
            //DataChunk dc;
            //lock (EoptisMng) {
            //    receiveData(ip, ref dcs);
            //    parseData(dcs, out dc);
            //    freeData(ref dcs);
            //}
        }

        //internal static bool ReadResults(string ip, int id, out EoptisInspectionResults eir, out EoptisWorkingMode ewm) {

        //    ewm = EoptisWorkingMode.UNKNOWN;
        //    if (isOffline) {
        //        Thread.Sleep(200);
        //        Random rnd = new Random();
        //        double d1 = rnd.Next(7000), d2 = rnd.Next((int)d1), d3 = d2 / d1, d4 = d3;
        //        MeasureResults vialRawRes = new MeasureResults(0, "Opacity vial raw value", "", true, true, ExactaEasyCore.MeasureTypeEnum.DOUBLE, d1.ToString(CultureInfo.InvariantCulture));
        //        MeasureResults laserRawRes = new MeasureResults(1, "Opacity laser raw value", "", true, true, ExactaEasyCore.MeasureTypeEnum.DOUBLE, d2.ToString(CultureInfo.InvariantCulture));
        //        MeasureResults preCalibrationRes = new MeasureResults(2, "Opacity pre calibration", "", true, true, ExactaEasyCore.MeasureTypeEnum.DOUBLE, d3.ToString(CultureInfo.InvariantCulture));
        //        MeasureResults postCalibrationRes = new MeasureResults(3, "Opacity post calibration", "", true, true, ExactaEasyCore.MeasureTypeEnum.DOUBLE, d4.ToString(CultureInfo.InvariantCulture));
        //        MeasureResultsCollection mrc = new MeasureResultsCollection();
        //        mrc.Add(vialRawRes);
        //        mrc.Add(laserRawRes);
        //        mrc.Add(preCalibrationRes);
        //        mrc.Add(postCalibrationRes);
        //        bool isRej = rnd.NextDouble() < 0.5;
        //        int rejCause = isRej ? 1 : 0;
        //        ToolResults toolRes = new ToolResults(0, true, isRej, true, mrc);
        //        ToolResultsCollection trc = new ToolResultsCollection();
        //        trc.Add(toolRes);
        //        eir = new EoptisInspectionResults(id, 0, (int)((dummyVialCounter * 2) % 40), dummyVialCounter++, true, isRej, rejCause, trc);
        //        return true;
        //    }
        //    bool res = false;
        //    lock (EoptisMng) {
        //        try {
        //            DataChunkStruct temp = new DataChunkStruct();
        //            receiveData(ip, ref temp);
        //            res = parseData(ip, id, temp, out eir, out ewm);
        //            freeData(ref temp);
        //        }
        //        catch (Exception ex) {
        //            eir = new EoptisInspectionResults(id, 0, -1, 0, false, false, 0, new ToolResultsCollection());
        //            Log.Line(LogLevels.Error, "EoptisSvc.ReadData", ip + ": Read failed. Error: " + ex.Message);
        //        }
        //    }
        //    return res;
        //}

        static void MainResThread() {

            Stopwatch diagnosticTimer = new Stopwatch();
            PollEv.Set();
            bool machineRunning = false, machineRunningPrev = false;
            bool oneMoreRead = true;
            while (true) {
                var retVal = WaitHandle.WaitAny(_mexDequeuerHandles);
                if (retVal == 0) break; //killEv
                lock (EoptisMng) {
                    bool machineRunningNew = false;
                    bool dataReceived = false;
                    foreach (KeyValuePair<string, ConcurrentQueue<EoptisInspectionResults>> resQueue in resultsQueues) {
                        string ip = resQueue.Key;
                        EoptisWorkingMode currEwm = EoptisWorkingMode.UNKNOWN;
                        if (workingModes.ContainsKey(ip))
                            currEwm = workingModes[ip];
                        // SE LA MACCHINA E' IN RUN LEGGO SOLO DA OPACIMETRI IN RUN
                        if (machineRunning &&
                            currEwm != EoptisWorkingMode.WORKING_MODE_CONTROL &&
                            !oneMoreRead)
                            continue;
                        EoptisInspectionResults eir;
                        try {
                            DataChunkStruct temp = new DataChunkStruct();
                            diagnosticTimer.Start();
                            receiveData(ip, ref temp);
                            diagnosticTimer.Stop();
                            //if (diagnosticTimer.ElapsedMilliseconds > 320)
                            //    Log.Line(LogLevels.Warning, ip + ": ACQ TIME = {0}ms", diagnosticTimer.ElapsedMilliseconds.ToString());
                            diagnosticTimer.Reset();
                            bool res = parseData(ip, temp, out eir);
                            freeData(ref temp);
                            if (res) {
                                dataReceived = true;
                                if (eir.WorkingMode != EoptisWorkingMode.UNKNOWN)
                                    workingModes[ip] = eir.WorkingMode;
                                if (eir.WorkingMode == EoptisWorkingMode.WORKING_MODE_CONTROL)
                                    machineRunningNew = true;
                                resQueue.Value.Enqueue(eir);
                            }
                        }
                        catch (Exception ex) {
                            //eir = new EoptisInspectionResults(id, 0, -1, 0, false, false, 0, new ToolResultsCollection());
                            Log.Line(LogLevels.Error, "EoptisSvc.ReadData", ip + ": Read failed. Error: " + ex.Message);
                        }
                    }
                    // quando hai letto da tutti...
                    oneMoreRead = false;
                    if (dataReceived)
                        machineRunning = machineRunningNew;
                    if (machineRunningPrev != machineRunning) {
                        Log.Line(LogLevels.Pass, "EoptisSvc.ReadData", "Machine running: " + machineRunning.ToString());
                        machineRunningPrev = machineRunning;
                        if (machineRunning) {
                            oneMoreRead = true;
                            Thread.Sleep(20);
                        }
                    }
                }
                Thread.Sleep(1);
            }
        }

        internal static void Dispose() {

            PollEv.Reset();
            KillEv.Set();
            if (mainResThread.IsAlive)
                mainResThread.Join(3000);
        }

        internal static bool ReadResults(string ip, int id, out EoptisInspectionResults eir) {

            if (isOffline) {
                Thread.Sleep(200);
                Random rnd = new Random();
                double d1 = rnd.Next(7000), d2 = rnd.Next((int)d1), d3 = d2 / d1, d4 = d3;
                MeasureResults vialRawRes = new MeasureResults(0, "Opacity vial raw value", "", true, true, ExactaEasyCore.MeasureTypeEnum.DOUBLE, d1.ToString(CultureInfo.InvariantCulture));
                MeasureResults laserRawRes = new MeasureResults(1, "Opacity laser raw value", "", true, true, ExactaEasyCore.MeasureTypeEnum.DOUBLE, d2.ToString(CultureInfo.InvariantCulture));
                MeasureResults preCalibrationRes = new MeasureResults(2, "Opacity pre calibration", "", true, true, ExactaEasyCore.MeasureTypeEnum.DOUBLE, d3.ToString(CultureInfo.InvariantCulture));
                MeasureResults postCalibrationRes = new MeasureResults(3, "Opacity post calibration", "", true, true, ExactaEasyCore.MeasureTypeEnum.DOUBLE, d4.ToString(CultureInfo.InvariantCulture));
                MeasureResultsCollection mrc = new MeasureResultsCollection();
                mrc.Add(vialRawRes);
                mrc.Add(laserRawRes);
                mrc.Add(preCalibrationRes);
                mrc.Add(postCalibrationRes);
                bool isRej = rnd.NextDouble() < 0.5;
                int rejCause = isRej ? 1 : 0;
                ToolResults toolRes = new ToolResults(0, true, isRej, true, mrc);
                ToolResultsCollection trc = new ToolResultsCollection();
                trc.Add(toolRes);
                eir = new EoptisInspectionResults(id, 0, (int)((dummyVialCounter * 2) % 40), dummyVialCounter++, true, isRej, rejCause, EoptisWorkingMode.WORKING_MODE_CONTROL, trc);
                return true;
            }

            //bool res = false;
            //lock (EoptisMng) {
            //    DataChunkStruct temp = new DataChunkStruct();
            //    //diagnosticTimer.Start();
            //    receiveData(ip, ref temp);
            //    //diagnosticTimer.Stop();
            //    //if (diagnosticTimer.ElapsedMilliseconds > 320)
            //    //    Log.Line(LogLevels.Warning, ip + ": ACQ TIME = {0}ms", diagnosticTimer.ElapsedMilliseconds.ToString());
            //    //diagnosticTimer.Reset();
            //    res = parseData(ip, temp, out eir);
            //    eir.NodeId = id;
            //    freeData(ref temp);
            //}
            //return res;

            if (resultsQueues[ip].Count > 0) {
                if (resultsQueues[ip].TryDequeue(out eir)) {
                    eir.NodeId = id;
                    return true;
                }
                return false;
            }
            else {
                eir = new EoptisInspectionResults(0, 0, 0, 0, false, false, 0, EoptisWorkingMode.UNKNOWN, new ToolResultsCollection());
                return false;
            }
        }

        private static bool parseData(string ip, DataChunkStruct dcs, out EoptisInspectionResults eir) {

            bool res = false;
            //DataChunk dc = new DataChunk();
            //dc.Size = (int)dcs.totData_;
            if (dcs.totData_ > 0)
                Log.Line(LogLevels.Debug, "EoptisSvc.parseData", "TotData: {0}", dcs.totData_);
            int spindleId = 0, rejectionCause = 0;
            uint vialId = 0;
            bool isReject = false;
            EoptisWorkingMode ewm = EoptisWorkingMode.UNKNOWN;
            //double measValueDouble = 0;
            string postCalibrationValue = "", vialRawValue = "", laserRawValue = "", preCalibrationValue = "";

            int offset = 0;
            for (uint i = 0; i < dcs.totData_; i++) {
                res = true;
                Log.Line(LogLevels.Debug, "EoptisSvc.parseData", "VariantDatum {0}: ", i);
                IntPtr ptr = new IntPtr(dcs.data_.ToInt64() + offset);
                VariantDatumStruct vds = (VariantDatumStruct)Marshal.PtrToStructure(ptr, typeof(VariantDatumStruct));
                //VariantDatumStruct vds = dcs.data_[i];
                Log.Line(LogLevels.Debug, "EoptisSvc.parseData", "ID: {0}", vds.id_);
                Log.Line(LogLevels.Debug, "EoptisSvc.parseData", "Size: {0}", vds.length_);
                Log.Line(LogLevels.Debug, "EoptisSvc.parseData", "Type: {0}", vds.type_);
                EoptisParameter ep = Params.Parameters[vds.id_];
                if (ep.Size != vds.length_ || ep.Type != vds.type_) {
                    Log.Line(LogLevels.Error, "EoptisSvc.parseData", "Parameter type or size is different from what expected! Expected type: {0}. Read type: {1}. Expected size: {2}. Read size: {3}", ep.Type, vds.type_, ep.Size, vds.length_);
                    res = false;
                    break;
                }
                byte[] value = new byte[vds.length_];
                Marshal.Copy(vds.data_, value, 0, (int)vds.length_);
                ep.Value = getValue(ep, value);
                offset += Marshal.SizeOf(vds);
                int pad = (offset % 8 == 0) ? 0 : 8 - offset % 8;
                offset += pad;
                Log.Line(LogLevels.Debug, "EoptisSvc.parseData", "Value: " + ep.Value);
                //VariantDatum vd = new VariantDatum(vds);
                if (ep.Id == SYSTEM_ID.PAR_WORKING_MODE && !Enum.TryParse<EoptisWorkingMode>(ep.Value, out ewm))
                    Log.Line(LogLevels.Error, "EoptisSvc.parseData", "Error while parsing current working mode");
                if (ep.Id == SYSTEM_ID.ANALYSIS_VIAL_ID_COUNTER && !uint.TryParse(ep.Value, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out vialId))
                    Log.Line(LogLevels.Error, "EoptisSvc.parseData", "Error while parsing vial id counter");
                if (ep.Id == SYSTEM_ID.PAR_VIAL_SPINDLE_NUMBER && !int.TryParse(ep.Value, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out spindleId))
                    Log.Line(LogLevels.Error, "EoptisSvc.parseData", "Error while parsing spindle id counter");
                if (ep.Id == SYSTEM_ID.ANALYSIS_IS_REJECT && !bool.TryParse(ep.Value, out isReject))
                    Log.Line(LogLevels.Error, "EoptisSvc.parseData", "Error while parsing good/reject boolean value");
                if (ep.Id == SYSTEM_ID.ANALYSIS_REJECTION_CAUSE_NUMBER && !int.TryParse(ep.Value, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out rejectionCause))
                    Log.Line(LogLevels.Error, "EoptisSvc.parseData", "Error while parsing rejection cause");
                //if (ep.Id == SYSTEM_ID.LOCKIN_AMPLITUDE_POST_CALIBRATION && !double.TryParse(ep.Value, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out measValueDouble))
                //    Log.Line(LogLevels.Error, "EoptisSvc.parseData", "Error while parsing post calibration value");
                //else
                //    measValue = measValueDouble.ToString();
                if (ep.Id == SYSTEM_ID.LOCKIN_VIAL_AMPLITUDE_RAW)
                    vialRawValue = ep.Value;
                if (ep.Id == SYSTEM_ID.LOCKIN_LASER_AMPLITUDE_RAW)
                    laserRawValue = ep.Value;
                if (ep.Id == SYSTEM_ID.LOCKIN_AMPLITUDE_FACTOR_COMPENSATED)
                    preCalibrationValue = ep.Value;
                if (ep.Id == SYSTEM_ID.LOCKIN_AMPLITUDE_POST_CALIBRATION)
                    postCalibrationValue = ep.Value;

            }
            MeasureResults vialRawRes = new MeasureResults(0, "Opacity vial raw value", "", true, true, ExactaEasyCore.MeasureTypeEnum.DOUBLE, vialRawValue);
            MeasureResults laserRawRes = new MeasureResults(1, "Opacity laser raw value", "", true, true, ExactaEasyCore.MeasureTypeEnum.DOUBLE, laserRawValue);
            MeasureResults preCalibrationRes = new MeasureResults(2, "Opacity pre calibration", "", true, true, ExactaEasyCore.MeasureTypeEnum.DOUBLE, preCalibrationValue);
            MeasureResults postCalibrationRes = new MeasureResults(3, "Opacity post calibration", "", true, true, ExactaEasyCore.MeasureTypeEnum.DOUBLE, postCalibrationValue);
            MeasureResultsCollection mrc = new MeasureResultsCollection();
            mrc.Add(vialRawRes);
            mrc.Add(laserRawRes);
            mrc.Add(preCalibrationRes);
            mrc.Add(postCalibrationRes);
            ToolResults toolRes = new ToolResults(0, true, isReject, true, mrc);
            ToolResultsCollection trc = new ToolResultsCollection();
            trc.Add(toolRes);
            eir = new EoptisInspectionResults(-1, 0, spindleId, vialId, true, isReject, rejectionCause, ewm, trc);
            return res;
        }

        //static int getInt(IntPtr pointer) {

        //    byte[] idBuf = new byte[sizeof(int)];
        //    Marshal.Copy(pointer, idBuf, 0, sizeof(int));
        //    return BitConverter.ToInt32(idBuf, 0);
        //}

        //static string getValue(EoptisParameter ep, IntPtr pointer) {

        //    byte[] valueBuf = new byte[ep.Size];
        //    Marshal.Copy(pointer, valueBuf, 0, ep.Size);
        //    return getValue(ep, valueBuf);
        //}

        #region GetSetParameters

        //internal static void GetParameters(string ip, ref EoptisParameterCollection paramCollection) {

        //    foreach (EoptisParameter param in paramCollection) {
        //        GetParameter(ip, ref param);
        //    }
        //}

        //internal static void GetParameter(string ip, ref EoptisParameter param) {

        //    byte[] buffer = new byte[param.Size];
        //    VariantDatum vd = new VariantDatum(param.Id, param.Size, param.Type);
        //    getParamDummy(ip, ref vd.vds);
        //    Marshal.Copy(vd.vds.data_, buffer, 0, param.Size);
        //    vd.Free();
        //}

        //PIER: FUNZIONAVA MA GENERALIZZO
        //internal static void GetParameter(string ip, EoptisParameter param, out bool value) {

        //    byte[] buffer;
        //    getParameter(ip, param.Id, out buffer);
        //    value = BitConverter.ToBoolean(buffer, 0);
        //    param.Value = value.ToString();
        //    param.Type = Params.Parameters[param.Id].Type;
        //    param.Size = Params.Parameters[param.Id].Size;
        //}

        //internal static void SetParameter(string ip, EoptisParameter param, bool value) {

        //    byte[] buffer = new byte[1];
        //    buffer[0] = value ? (byte)1 : (byte)0;
        //    setParameter(ip, param.Id, buffer);
        //    param.Value = value.ToString();
        //    param.Type = Params.Parameters[param.Id].Type;
        //    param.Size = Params.Parameters[param.Id].Size;
        //}

        //internal static void GetParameter(string ip, EoptisParameter param, out char value) {

        //    byte[] buffer;
        //    getParameter(ip, param.Id, out buffer);
        //    value = BitConverter.ToChar(buffer, 0);
        //    param.Value = value.ToString();
        //    param.Type = Params.Parameters[param.Id].Type;
        //    param.Size = Params.Parameters[param.Id].Size;
        //}

        ////internal static void SetParameter(string ip, SYSTEM_ID paramId, byte value) {

        ////    byte[] buffer = new byte[1];
        ////    buffer[0] = value;
        ////    setParameter(ip, paramId, buffer);
        ////}

        //internal static void GetParameter(string ip, EoptisParameter param, out short value) {

        //    byte[] buffer;
        //    getParameter(ip, param.Id, out buffer);
        //    value = BitConverter.ToInt16(buffer, 0);
        //    param.Value = value.ToString();
        //    param.Type = Params.Parameters[param.Id].Type;
        //    param.Size = Params.Parameters[param.Id].Size;
        //}

        //internal static void SetParameter(string ip, EoptisParameter param, short value) {

        //    byte[] buffer = BitConverter.GetBytes(value);
        //    setParameter(ip, param.Id, buffer);
        //    param.Value = value.ToString();
        //    param.Type = Params.Parameters[param.Id].Type;
        //    param.Size = Params.Parameters[param.Id].Size;
        //}

        //internal static void GetParameter(string ip, EoptisParameter param, out int value) {

        //    byte[] buffer;
        //    getParameter(ip, param.Id, out buffer);
        //    value = BitConverter.ToInt32(buffer, 0);
        //    param.Value = value.ToString();
        //    param.Type = Params.Parameters[param.Id].Type;
        //    param.Size = Params.Parameters[param.Id].Size;
        //}

        //internal static void SetParameter(string ip, EoptisParameter param, int value) {

        //    byte[] buffer = BitConverter.GetBytes(value);
        //    setParameter(ip, param.Id, buffer);
        //    param.Value = value.ToString();
        //    param.Type = Params.Parameters[param.Id].Type;
        //    param.Size = Params.Parameters[param.Id].Size;
        //}

        //internal static void GetParameter(string ip, EoptisParameter param, out uint value) {

        //    byte[] buffer;
        //    getParameter(ip, param.Id, out buffer);
        //    value = BitConverter.ToUInt32(buffer, 0);
        //    param.Value = value.ToString();
        //    param.Type = Params.Parameters[param.Id].Type;
        //    param.Size = Params.Parameters[param.Id].Size;
        //}

        //internal static void SetParameter(string ip, EoptisParameter param, uint value) {

        //    byte[] buffer = BitConverter.GetBytes(value);
        //    setParameter(ip, param.Id, buffer);
        //    param.Value = value.ToString();
        //    param.Type = Params.Parameters[param.Id].Type;
        //    param.Size = Params.Parameters[param.Id].Size;
        //}

        //internal static void GetParameter(string ip, EoptisParameter param, out float value) {

        //    byte[] buffer;
        //    getParameter(ip, param.Id, out buffer);
        //    value = BitConverter.ToSingle(buffer, 0);
        //    param.Value = value.ToString();
        //    param.Type = Params.Parameters[param.Id].Type;
        //    param.Size = Params.Parameters[param.Id].Size;
        //}

        //internal static void SetParameter(string ip, EoptisParameter param, float value) {

        //    byte[] buffer = BitConverter.GetBytes(value);
        //    setParameter(ip, param.Id, buffer);
        //    param.Value = value.ToString();
        //    param.Type = Params.Parameters[param.Id].Type;(
        //    param.Size = Params.Parameters[param.Id].Size;
        //}

        //internal static void GetParameter(string ip, EoptisParameter param, out double value) {

        //    byte[] buffer;
        //    getParameter(ip, param.Id, out buffer);
        //    value = BitConverter.ToDouble(buffer, 0);
        //    param.Value = value.ToString();
        //    param.Type = Params.Parameters[param.Id].Type;
        //    param.Size = Params.Parameters[param.Id].Size;
        //}

        //internal static void SetParameter(string ip, EoptisParameter param, double value) {

        //    byte[] buffer = BitConverter.GetBytes(value);
        //    setParameter(ip, param.Id, buffer);
        //    param.Value = value.ToString();
        //    param.Type = Params.Parameters[param.Id].Type;
        //    param.Size = Params.Parameters[param.Id].Size;
        //}

        //internal static void GetParameter(string ip, EoptisParameter param, out string value) {

        //    byte[] buffer;
        //    getParameter(ip, param.Id, out buffer);
        //    value = Encoding.Default.GetString(buffer).Split('\n', '\0')[0];
        //    param.Value = value.ToString();
        //    param.Type = Params.Parameters[param.Id].Type;
        //    param.Size = Params.Parameters[param.Id].Size;
        //}

        //internal static void SetParameter(string ip, EoptisParameter param, string value) {

        //    byte[] buffer = Encoding.Default.GetBytes(value);
        //    setParameter(ip, param.Id, buffer);
        //    param.Value = value.ToString();
        //    param.Type = Params.Parameters[param.Id].Type;
        //    param.Size = Params.Parameters[param.Id].Size;
        //}

        //static void getParameter(string ip, SYSTEM_ID paramId, out byte[] buffer) {

        //    EoptisParameter param = Params.Parameters[paramId];
        //    if (param == null)
        //        throw new Exception("Parameter not found");

        //    buffer = new byte[param.Size];
        //    VariantDatum vd = new VariantDatum(param.Id, param.Size, param.Type);
        //    //getParamDummy(ip, ref vd.vds);
        //    getParameter(ip, ref vd.vds);
        //    Marshal.Copy(vd.vds.data_, buffer, 0, param.Size);
        //    vd.Free();
        //}

        //static void setParameter(string ip, SYSTEM_ID paramId, byte[] buffer) {

        //    EoptisParameter param = Params.Parameters[paramId];
        //    if (param == null)
        //        throw new Exception("Parameter not found");

        //    VariantDatum vd = new VariantDatum(param.Id, param.Size, param.Type);
        //    byte[] safeBuffer = new byte[param.Size];
        //    Array.Copy(buffer, 0, safeBuffer, 0, Math.Min(param.Size, buffer.Length));
        //    Marshal.Copy(safeBuffer, 0, vd.vds.data_, param.Size);
        //    setParameter(ip, ref vd.vds);
        //    vd.Free();
        //}

        //PIER: 8/4/2015 NUOVE FUNZIONI DI GET E SET PARAMETRI

        internal static string GetParameter(string ip, EoptisParameter param) {

            if (param == null)
                throw new Exception(ip + ": Get parameter failed. Invalid parameter");

            if (isOffline) {
                Thread.Sleep(200);
                return "";
            }
            string res = "";
            int err = 0;
            lock (EoptisMng) {
                param = Params.Parameters[param.Id];
                byte[] buffer = new byte[param.Size];
                VariantDatum vd = new VariantDatum(param.Id, param.Size, param.Type);
                //getParamDummy(ip, ref vd.vds);
                err = getParameter(ip, ref vd.vds);
                Marshal.Copy(vd.vds.data_, buffer, 0, param.Size);
                vd.Free();
                res = getValue(param, buffer);
            }
            if (err != 0)
                throw new Exception(ip + ": Get parameter failed. Error: " + err.ToString());
            return res;
        }

        internal static void GetParameters(string ip, ref EoptisParameterCollection parameters) {

            if (isOffline) {
                foreach (EoptisParameter offEoParam in parameters) {
                    offEoParam.Value = "0.57";
                }
                Thread.Sleep(200);
                return;
            }
            lock (EoptisMng) {

                DataChunk dc = new DataChunk(parameters);
                getParameters(ip, ref dc.dcs);

                int offset = 0;
                for (uint i = 0; i < dc.dcs.totData_; i++) {
                    Log.Line(LogLevels.Debug, "EoptisSvc.GetParameters", "VariantDatum {0}: ", i);
                    IntPtr ptr = new IntPtr(dc.dcs.data_.ToInt64() + offset);
                    VariantDatumStruct vds = (VariantDatumStruct)Marshal.PtrToStructure(ptr, typeof(VariantDatumStruct));
                    Log.Line(LogLevels.Debug, "EoptisSvc.GetParameters", "ID: {0}", vds.id_);
                    Log.Line(LogLevels.Debug, "EoptisSvc.GetParameters", "Size: {0}", vds.length_);
                    Log.Line(LogLevels.Debug, "EoptisSvc.GetParameters", "Type: {0}", vds.type_);
                    EoptisParameter ep = parameters[vds.id_];
                    if (ep == null) {
                        Log.Line(LogLevels.Error, "EoptisSvc.GetParameters", "Parameter type unexpected");
                        break;//throw new.....
                    }
                    if (ep.Size != vds.length_ || ep.Type != vds.type_) {
                        Log.Line(LogLevels.Error, "EoptisSvc.GetParameters", "Parameter type or size is different from what expected! Expected type: {0}. Read type: {1}. Expected size: {2}. Read size: {3}", ep.Type, vds.type_, ep.Size, vds.length_);
                        break;//throw new.....
                    }
                    byte[] value = new byte[vds.length_];
                    Marshal.Copy(vds.data_, value, 0, (int)vds.length_);
                    ep.Value = getValue(ep, value);
                    offset += Marshal.SizeOf(vds);
                    int pad = (offset % 8 == 0) ? 0 : 8 - offset % 8;
                    offset += pad;
                    Log.Line(LogLevels.Debug, "EoptisSvc.GetParameters", "Value: " + ep.Value);
                }
                freeData(ref dc.dcs);
            }
        }

        internal static void SetParameter(string ip, EoptisParameter param, string value) {

            if (param == null)
                throw new Exception(ip + ": Set parameter failed. Invalid parameter");

            if (isOffline) {
                Thread.Sleep(200);
                return;
            }
            int err = 0;
            lock (EoptisMng) {
                param = Params.Parameters[param.Id];
                VariantDatum vd = new VariantDatum(param.Id, param.Size, param.Type);
                byte[] safeBuffer = setValue(param, value);
                //Array.Copy(buffer, 0, safeBuffer, 0, Math.Min(param.Size, buffer.Length));
                Marshal.Copy(safeBuffer, 0, vd.vds.data_, param.Size);
                err = setParameter(ip, ref vd.vds);
                vd.Free();
            }
            if (err != 0)
                throw new Exception(ip + ": Set parameter failed. Error: " + err.ToString());
        }

        internal static void SetParameters(string ip, EoptisParameterCollection parameters) {

            if (isOffline) {
                Thread.Sleep(200);
                return;
            }
            lock (EoptisMng) {

                DataChunk dc = new DataChunk(parameters);
                setParameters(ip, ref dc.dcs);
            }
        }

        //internal static EoptisWorkingMode GetWorkingMode(string ip) {

        //    EoptisWorkingMode res = EoptisWorkingMode.UNKNOWN;
        //    if (isOffline) {
        //        Thread.Sleep(200);
        //        return EoptisWorkingMode.WORKING_MODE_IDLE;
        //    }
        //    int err = 0;
        //    lock (EoptisMng) {
        //        int workMode = 0;
        //        err = getWorkingMode(ip, ref workMode);
        //        Enum.TryParse<EoptisWorkingMode>(workMode.ToString(), out res);
        //    }
        //    if (err != 0)
        //        throw new Exception(ip + ": Get working mode failed. Error: " + err.ToString());
        //    return res;
        //}

        internal static void SetWorkingMode(string ip, EoptisWorkingMode wm) {

            if (isOffline) {
                Thread.Sleep(200);
                return;
            }
            int err = 0;
            lock (EoptisMng) {
                err = setWorkingMode(ip, (int)wm);
            }
            if (err != 0)
                throw new Exception(ip + ": Set parameter failed. Error: " + err.ToString());
        }

        static string getValue(EoptisParameter ep, byte[] value) {

            string res = "";
            switch (ep.Type) {
                case SYSTEM_TYPE.TYPEBOOL:
                    res = BitConverter.ToBoolean(value, 0).ToString(CultureInfo.InvariantCulture);
                    break;
                case SYSTEM_TYPE.TYPECHAR:
                    break;
                case SYSTEM_TYPE.TYPEUNSIGNEDCHAR:
                    res = value[0].ToString();
                    break;
                case SYSTEM_TYPE.TYPESHORT:
                    res = BitConverter.ToInt16(value, 0).ToString(CultureInfo.InvariantCulture);
                    break;
                case SYSTEM_TYPE.TYPESHORTBUFFER:
                    break;
                case SYSTEM_TYPE.TYPEINT:
                    res = BitConverter.ToInt32(value, 0).ToString(CultureInfo.InvariantCulture);
                    break;
                case SYSTEM_TYPE.TYPEUNSIGNEDINT:
                    res = BitConverter.ToUInt32(value, 0).ToString(CultureInfo.InvariantCulture);
                    break;
                case SYSTEM_TYPE.TYPELONG:
                    break;
                case SYSTEM_TYPE.TYPEUNSIGNEDLONG:
                    break;
                case SYSTEM_TYPE.TYPEFLOAT:
                    res = BitConverter.ToSingle(value, 0).ToString(CultureInfo.InvariantCulture);
                    break;
                case SYSTEM_TYPE.TYPEDOUBLE:
                    res = BitConverter.ToDouble(value, 0).ToString(CultureInfo.InvariantCulture);
                    break;
                case SYSTEM_TYPE.TYPEMEASURE:
                    for (int i = 0; i < ep.Size / sizeof(short); i++) {
                        short val = BitConverter.ToInt16(value, i * sizeof(short));
                        res += i.ToString() + ";" + "0x" + val.ToString("X2", CultureInfo.InvariantCulture) + ";" + val.ToString(CultureInfo.InvariantCulture) + "\r\n";
                    }
                    break;
                case SYSTEM_TYPE.TYPESTRING:
                    res = Encoding.Default.GetString(value).Split('\n', '\0')[0];
                    break;
                case SYSTEM_TYPE.TYPEUNKNOWN:
                    throw new Exception("Invalid parameter type");
                //break;
                default:
                    break;
            }
            return res;
        }

        static byte[] setValue(EoptisParameter ep, string value) {

            byte[] res = new byte[ep.Size];
            byte[] buffer;
            switch (ep.Type) {
                case SYSTEM_TYPE.TYPEBOOL:
                    buffer = new byte[1];
                    buffer[0] = Convert.ToBoolean(value) ? (byte)1 : (byte)0;
                    Array.Copy(buffer, 0, res, 0, buffer.Length);
                    break;
                case SYSTEM_TYPE.TYPECHAR:
                    byte b1 = Convert.ToByte(value);
                    buffer = BitConverter.GetBytes(b1);
                    Array.Copy(buffer, 0, res, 0, buffer.Length);
                    break;
                case SYSTEM_TYPE.TYPEUNSIGNEDCHAR:
                    byte b2 = Convert.ToByte(value);
                    buffer = BitConverter.GetBytes(b2);
                    Array.Copy(buffer, 0, res, 0, buffer.Length);
                    break;
                case SYSTEM_TYPE.TYPESHORT:
                    short w0 = Convert.ToInt16(value);
                    buffer = BitConverter.GetBytes(w0);
                    Array.Copy(buffer, 0, res, 0, buffer.Length);
                    break;
                case SYSTEM_TYPE.TYPESHORTBUFFER:
                    break;
                case SYSTEM_TYPE.TYPEINT:
                    int dw0 = Convert.ToInt32(value);
                    buffer = BitConverter.GetBytes(dw0);
                    Array.Copy(buffer, 0, res, 0, buffer.Length);
                    break;
                case SYSTEM_TYPE.TYPEUNSIGNEDINT:
                    uint dw1 = Convert.ToUInt32(value);
                    buffer = BitConverter.GetBytes(dw1);
                    Array.Copy(buffer, 0, res, 0, buffer.Length);
                    break;
                case SYSTEM_TYPE.TYPELONG:
                    break;
                case SYSTEM_TYPE.TYPEUNSIGNEDLONG:
                    break;
                case SYSTEM_TYPE.TYPEFLOAT:
                    float fVal = Convert.ToSingle(value, CultureInfo.InvariantCulture);
                    buffer = BitConverter.GetBytes(fVal);
                    Array.Copy(buffer, 0, res, 0, buffer.Length);
                    break;
                case SYSTEM_TYPE.TYPEDOUBLE:
                    double dVal = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                    buffer = BitConverter.GetBytes(dVal);
                    Array.Copy(buffer, 0, res, 0, buffer.Length);
                    break;
                case SYSTEM_TYPE.TYPEMEASURE:
                    string[] values = value.Split(';');
                    for (int i = 0; i < values.Length; i++) {
                        short w1 = Convert.ToInt16(values[i]);
                        buffer = BitConverter.GetBytes(w1);
                        Array.Copy(buffer, 0, res, i * sizeof(short), buffer.Length);
                    }
                    break;
                case SYSTEM_TYPE.TYPESTRING:
                    buffer = Encoding.Default.GetBytes(value);
                    Array.Copy(buffer, 0, res, 0, Math.Min(buffer.Length, ep.Size));
                    break;
                case SYSTEM_TYPE.TYPEUNKNOWN:
                    throw new Exception("Invalid parameter type");
                    break;
                default:
                    break;
            }
            return res;
        }

        #endregion
    }

    public class EoptisInspectionResultsEventArgs : EventArgs {

        public string Ip;
        public int Id;

        public List<EoptisInspectionResults> InspectionResultsCollection;

        public EoptisInspectionResultsEventArgs(string ip, int id, List<EoptisInspectionResults> measResCollection) {
            Ip = ip;
            Id = id;
            InspectionResultsCollection = measResCollection;
        }
    }

    public class EoptisInspectionResults : InspectionResults {

        public EoptisWorkingMode WorkingMode;

        internal EoptisInspectionResults(int nodeId, int inspectionId, int spindleId, uint vialId, bool isActive, bool isReject, int rejectionCause, EoptisWorkingMode ewm, ToolResultsCollection toolsResults)
            : base(nodeId, inspectionId, spindleId, vialId, isActive, isReject, rejectionCause, toolsResults) {

            WorkingMode = ewm;
        }
    }

    public enum EoptisWorkingMode {

        UNKNOWN = -2,
        WORKING_MODE_IDLE = -1,                         /*!< -1: Modalità di funzionamento idle: all'accensione si mette in attesa per la modifica di parametri e della modalità d'utilizzo*/
        WORKING_MODE_CONTROL,                           /*!< 0: Modalità di funzionamento controllo*/
        WORKING_MODE_FREE_RUN,                          /*!< 1: Modalità di funzionamento free run*/
        WORKING_MODE_MECHANICAL_TEST,                   /*!< 2: Modalità di funzionamento test meccanico*/
        WORKING_MODE_TOT,                               /*!< 3: Parametro che tiene conto di tutte le modalità di funzionamento*/
        FORCE_WORKING_MODE_MAX_INT = 0x7FFFFFFF         /*!< 0x7FFFFFFF: Parametro inutilizzato per forzare gli altri ID ad un valore di tipo int */
    }
}
