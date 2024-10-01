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
using ExactaEasyCore;

namespace EoptisClient
{
    public static class EoptisSvc
    {

        //internal static bool isOffline = false;
        //static uint dummyVialCounter = 0;

        internal static EoptisParameters ParamsDictionary = new EoptisParameters();
        internal static List<SYSTEM_ID> SpectrometerParamsId = new List<SYSTEM_ID>() {

            SYSTEM_ID.PAR_SPINDLE_COUNT,
            SYSTEM_ID.PAR_0_TORRETTA_OFFSET,
            SYSTEM_ID.PAR_INCREMENTO_SPINDLE_ID,
            SYSTEM_ID.PAR_TINT_VIAL,
            SYSTEM_ID.PAR_TINT_BACKGROUND,
            SYSTEM_ID.PAR_ALL_RANGE_SUM_REJECT,
            SYSTEM_ID.LIGHT_CONTROL_TH,
            SYSTEM_ID.PAR_RANGE_1_L,
            SYSTEM_ID.PAR_RANGE_1_H,
            SYSTEM_ID.PAR_RANGE_1_SIGN,
            SYSTEM_ID.PAR_RANGE_1_TH,
            SYSTEM_ID.PAR_RANGE_1_PEAK_REJECT_TH,
            SYSTEM_ID.PAR_RANGE_2_L,
            SYSTEM_ID.PAR_RANGE_2_H,
            SYSTEM_ID.PAR_RANGE_2_SIGN,
            SYSTEM_ID.PAR_RANGE_2_TH,
            SYSTEM_ID.PAR_RANGE_2_PEAK_REJECT_TH,
            SYSTEM_ID.PAR_RANGE_3_L,
            SYSTEM_ID.PAR_RANGE_3_H,
            SYSTEM_ID.PAR_RANGE_3_SIGN,
            SYSTEM_ID.PAR_RANGE_3_TH,
            SYSTEM_ID.PAR_RANGE_3_PEAK_REJECT_TH,
            SYSTEM_ID.PAR_RANGE_4_L,
            SYSTEM_ID.PAR_RANGE_4_H,
            SYSTEM_ID.PAR_RANGE_4_SIGN,
            SYSTEM_ID.PAR_RANGE_4_TH,
            SYSTEM_ID.PAR_RANGE_4_PEAK_REJECT_TH,
            SYSTEM_ID.INITIAL_WAVELENGTH,
            SYSTEM_ID.WAVELENGTH_INCREMENT,
            SYSTEM_ID.FINAL_WAVELENGTH,
            SYSTEM_ID.PAR_FULL_SPECTRUM_REF_NORM,
            SYSTEM_ID.INFO_FIRMWARE_VERSION
        };

        internal static List<SYSTEM_ID> OpacimetroParamsId = new List<SYSTEM_ID>() {

            SYSTEM_ID.PAR_SPINDLE_COUNT,
            SYSTEM_ID.PAR_0_TORRETTA_OFFSET,
            SYSTEM_ID.PAR_INCREMENTO_SPINDLE_ID,
            SYSTEM_ID.PAR_LASER_GAIN,
            SYSTEM_ID.PAR_VIAL_GAIN,
            SYSTEM_ID.TH_WARNING_UPPER,
            SYSTEM_ID.TH_WARNING_LOWER,
            SYSTEM_ID.TH_ERROR_UPPER,
            SYSTEM_ID.TH_ERROR_LOWER,
            SYSTEM_ID.LIGHT_CONTROL_TH
        };

        internal static EoptisParameterCollection ChooseParameters(string deviceType) {

            EoptisParameterCollection parameters = new EoptisParameterCollection();
            List<SYSTEM_ID> currentList = null;
            if (deviceType == "EoptisSpectrometer") {
                currentList = SpectrometerParamsId;
            }
            if (deviceType == "EoptisTurbidimeter") {
                currentList = OpacimetroParamsId;
            }
            foreach (SYSTEM_ID sysId in currentList) {
                parameters.Add(ParamsDictionary.Parameters[sysId]);
            }
            return parameters;
        }

        internal static List<SYSTEM_ID> SpectrometerResultsId = new List<SYSTEM_ID>() {

            SYSTEM_ID.PAR_FULL_SPECTRUM_MIS,    //deve essere primo perchè su questa richiesta viene acquisito un nuovo spettro (senza trigger da PLC)
            SYSTEM_ID.PAR_FULL_SPECTRUM_BUFFER,
            SYSTEM_ID.PAR_FULL_SPECTRUM_ELAB,
            SYSTEM_ID.PAR_RANGE_1_MEAN,
            SYSTEM_ID.PAR_RANGE_2_MEAN,
            SYSTEM_ID.PAR_RANGE_3_MEAN,
            SYSTEM_ID.PAR_RANGE_4_MEAN,
            SYSTEM_ID.PAR_ALL_RANGE_MEAN,
            SYSTEM_ID.LIGHT_CONTROL,
            SYSTEM_ID.ANALYSIS_REJECTION_CAUSE_NUMBER,
            SYSTEM_ID.PAR_VIAL_SPINDLE_NUMBER
        };

        internal static List<SYSTEM_ID> OpacimetroResultsId = new List<SYSTEM_ID>() {

            SYSTEM_ID.LOCKIN_VIAL_AMPLITUDE_RAW,
            SYSTEM_ID.LOCKIN_LASER_AMPLITUDE_RAW,
            SYSTEM_ID.LOCKIN_AMPLITUDE_FACTOR_COMPENSATED,
            SYSTEM_ID.LOCKIN_AMPLITUDE_POST_CALIBRATION,
            SYSTEM_ID.LIGHT_CONTROL,
            SYSTEM_ID.ANALYSIS_REJECTION_CAUSE_NUMBER,
            SYSTEM_ID.PAR_VIAL_SPINDLE_NUMBER
        };

        internal static ToolResults ChooseResults(string deviceType) {

            ToolResults results = new ToolResults(0, true, false, true, new MeasureResultsCollection());
            int measureId = 0;
            List<SYSTEM_ID> currentList = null;
            if (deviceType == "EoptisSpectrometer") {
                currentList = SpectrometerResultsId;
            }
            if (deviceType == "EoptisTurbidimeter") {
                currentList = OpacimetroResultsId;
            }
            foreach (SYSTEM_ID sysId in currentList) {
                MeasureResults mr = new MeasureResults(measureId, sysId.ToString(), "", true, true, ExactaEasyCore.MeasureTypeEnum.STRING, "", 0);
                results.ResMeasureCollection.Add(mr);
                measureId++;
            }
            return results;
        }

        internal static EoptisParameterCollection GetResultsParamList(string deviceType) {

            EoptisParameterCollection parameters = new EoptisParameterCollection();
            List<SYSTEM_ID> currentList = null;
            if (deviceType == "EoptisSpectrometer") {
                currentList = SpectrometerResultsId;
            }
            if (deviceType == "EoptisTurbidimeter") {
                currentList = OpacimetroResultsId;
            }
            foreach (SYSTEM_ID sysId in currentList) {
                parameters.Add(ParamsDictionary.Parameters[sysId]);
            }
            return parameters;
        }

        //internal static List<SYSTEM_ID> SpectrometerRunId = new List<SYSTEM_ID>() {

        //    SYSTEM_ID.PAR_FULL_SPECTRUM_BUFFER,
        //    SYSTEM_ID.PAR_FULL_SPECTRUM_MIS,
        //    SYSTEM_ID.PAR_FULL_SPECTRUM_REF_NORM,
        //    SYSTEM_ID.PAR_FULL_SPECTRUM_ELAB
        //};

        internal static byte[] rxBuffer = new byte[1024 * 1024];
        private static readonly object EoptisMng = new object();

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        internal unsafe struct VariantDatumStruct
        {

            internal SYSTEM_ID id_;
            internal uint length_;
            internal SYSTEM_TYPE type_;
            internal IntPtr data_;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct ResultsData
        {

            [FieldOffset(0)]
            internal bool boolValue;
            [FieldOffset(0)]
            internal byte byteValue;
            [FieldOffset(0)]
            internal float fValue;
            [FieldOffset(0)]
            internal int iValue;
            [FieldOffset(0)]
            internal uint uiValue;
            [FieldOffset(0)]
            internal double dValue;
            [FieldOffset(sizeof(double))]
            internal string sValue;
        }

        internal class VariantDatum
        {

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

        internal unsafe struct DataChunkStruct
        {

            internal uint totData_;
            internal IntPtr data_;
        }

        internal unsafe class DataChunk
        {

            internal DataChunkStruct dcs;
            internal VariantDatum[] vdArray;

            internal DataChunk(EoptisParameterCollection epc) {

                dcs = new DataChunkStruct();
                dcs.totData_ = (uint)epc.Count;
                int vdsSize = Marshal.SizeOf(typeof(VariantDatumStruct));
                dcs.data_ = Marshal.AllocHGlobal(vdsSize * epc.Count);
                vdArray = new VariantDatum[epc.Count];
                for (int i = 0; i < dcs.totData_; i++) {
                    vdArray[i] = new VariantDatum(epc[i].Id, epc[i].ByteLength, epc[i].Type);
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
        //[DllImport("EoptisDeviceManager.dll", CharSet = CharSet.Ansi)]
        //internal static unsafe extern int getParameter(string ip, ref VariantDatumStruct param);
        //[DllImport("EoptisDeviceManager.dll", CharSet = CharSet.Ansi)]
        //internal static unsafe extern int setParameter(string ip, ref VariantDatumStruct param);
        [DllImport("EoptisDeviceManager.dll", CharSet = CharSet.Ansi)]
        internal static unsafe extern int getParameters(string ip, ref DataChunkStruct parameters);
        [DllImport("EoptisDeviceManager.dll", CharSet = CharSet.Ansi)]
        internal static unsafe extern int setParameters(string ip, ref DataChunkStruct parameters);
        //[DllImport("EoptisDeviceManager.dll", CharSet = CharSet.Ansi)]
        //internal static unsafe extern int setWorkingMode(string ip, int wm);
        //[DllImport("EoptisDeviceManager.dll", CharSet = CharSet.Ansi)]
        //internal static unsafe extern int getWorkingMode(string ip, ref int wm);

        [DllImport("EoptisDeviceManager.dll", CharSet = CharSet.Ansi)]
        internal static unsafe extern void receiveData(string ip, ref DataChunkStruct dataChunk);
        [DllImport("EoptisDeviceManager.dll", CharSet = CharSet.Ansi)]
        internal static unsafe extern void freeData(ref DataChunkStruct dataChunk);

        private static Thread mainResThread;
        private static Dictionary<string, ConcurrentQueue<EoptisData>> resultsQueues = new Dictionary<string, ConcurrentQueue<EoptisData>>();
        public static Dictionary<string, Stopwatch> workingModesOffCounter = new Dictionary<string, Stopwatch>();
        private static WaitHandle[] _mexDequeuerHandles;
        private static ManualResetEvent KillEv = new ManualResetEvent(false);
        internal static ManualResetEvent PollEv = new ManualResetEvent(false);

        static EoptisSvc() {

            ////Params = EoptisParameters.LoadFromFile("EoptisParameters.xml");
            //if (Params == null) {
            //    //throw new Exception("File not found: EoptisParameters.xml");
            //    Params = new EoptisParameters();
            //}

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

            //if (isOffline) {
            //    Thread.Sleep(200);
            //    return 0;
            //}
            int res = connectDevice(ip, port, activeCallback);
            lock (EoptisMng) {
                if (!resultsQueues.ContainsKey(ip)) {
                    resultsQueues.Add(ip, new ConcurrentQueue<EoptisData>());
                }
                //if (!workingModesOffCounter.ContainsKey(ip)) {
                //    workingModesOffCounter.Add(ip, 0);
                //}
            }
            PollEv.Set();
            return res;
        }

        internal static int Disconnect(string ip) {

            lock (EoptisMng) {
                //if (resultsQueues.ContainsKey(ip)) {
                //    resultsQueues.Remove(ip);
                //}
                //if (workingModesOffCounter.ContainsKey(ip)) {
                //    workingModesOffCounter.Remove(ip);
                //}
            }
            //if (isOffline) {
            //    Thread.Sleep(200);
            //    return 0;
            //}
            return disconnect(ip);
        }

        //public static void Test(string ip) {

        //    string srNum, fwVer, sdkVer, ipAddr, portRead, fpgaVer, spindleCount, workingMode, parLedGreenPower;
        //    srNum = GetParameter(ip, new EoptisParameter(SYSTEM_ID.INFO_SERIAL_NUMBER));
        //    fwVer = GetParameter(ip, new EoptisParameter(SYSTEM_ID.INFO_FIRMWARE_VERSION));
        //    sdkVer = GetParameter(ip, new EoptisParameter(SYSTEM_ID.INFO_SDK_VERSION));
        //    ipAddr = GetParameter(ip, new EoptisParameter(SYSTEM_ID.INFO_IP_ADDRESS));
        //    //uint portRead, fpgaVer;
        //    portRead = GetParameter(ip, new EoptisParameter(SYSTEM_ID.INFO_PORT));
        //    fpgaVer = GetParameter(ip, new EoptisParameter(SYSTEM_ID.INFO_FPGA_VERSION));
        //    //int spindleCount, workingMode;
        //    spindleCount = GetParameter(ip, new EoptisParameter(SYSTEM_ID.PAR_SPINDLE_COUNT));
        //    int spindleC = Convert.ToInt32(spindleCount);
        //    SetParameter(ip, new EoptisParameter(SYSTEM_ID.PAR_SPINDLE_COUNT), (spindleC + 1).ToString());
        //    spindleCount = GetParameter(ip, new EoptisParameter(SYSTEM_ID.PAR_SPINDLE_COUNT));
        //    workingMode = GetParameter(ip, new EoptisParameter(SYSTEM_ID.PAR_WORKING_MODE));
        //    //bool parLedGreenPower;
        //    parLedGreenPower = GetParameter(ip, new EoptisParameter(SYSTEM_ID.PAR_LED_GREEN_POWER));
        //    bool led = Convert.ToBoolean(parLedGreenPower);
        //    SetParameter(ip, new EoptisParameter(SYSTEM_ID.PAR_LED_GREEN_POWER), (!led).ToString());

        //}

        static void MainResThread() {

            Stopwatch diagnosticTimer = new Stopwatch();
            // bool oneMoreRead = true;
            DataChunkStruct temp = new DataChunkStruct();
            while (true) {
                var retVal = WaitHandle.WaitAny(_mexDequeuerHandles);
                if (retVal == 0) break; //killEv
                lock (EoptisMng) {
                    //bool dataReceived = false;
                    foreach (KeyValuePair<string, ConcurrentQueue<EoptisData>> resQueue in resultsQueues) {
                        string ip = resQueue.Key;
                        //EoptisWorkingMode currEwm = EoptisWorkingMode.UNKNOWN;
                        //if (workingModes.ContainsKey(ip))
                        //    currEwm = workingModes[ip];
                        EoptisData eir;
                        try {
                            //DataChunkStruct temp = new DataChunkStruct();
                            diagnosticTimer.Start();
                            bool res = false;
                            lock (EoptisMng) {
                                receiveData(ip, ref temp);
                                if (temp.totData_ > 1)
                                    Trace.WriteLine("RECEIVE: " + temp.totData_);
                                diagnosticTimer.Stop();
                                //if (diagnosticTimer.ElapsedMilliseconds > 320)
                                //    Log.Line(LogLevels.Warning, ip + ": ACQ TIME = {0}ms", diagnosticTimer.ElapsedMilliseconds.ToString());
                                diagnosticTimer.Reset();
                                res = parseData(ip, temp, out eir);
                                freeData(ref temp);
                            }

                            //dataReceived = true;
                            //Trace.WriteLine(eir.WorkingMode);
                            if (eir.WorkingMode != EoptisWorkingMode.UNKNOWN) {
                                workingModesOffCounter[ip].Restart();
                            }
                            if (res == true) {
                                resQueue.Value.Enqueue(eir);
                            }
                        } catch (Exception ex) {
                            //eir = new EoptisData(id, 0, -1, 0, false, false, 0, new ToolResultsCollection());
                            Log.Line(LogLevels.Error, "EoptisSvc.ReadData", ip + ": Read failed. Error: " + ex.Message);
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

        //internal static void SetRecordingMode(int howMuch, int condition) {
        //}

        internal static bool ReadResults(string ip, int id, out EoptisData eir) {

            if (resultsQueues[ip].Count > 0) {
                if (resultsQueues[ip].TryDequeue(out eir)) {
                    eir.NodeId = id;
                    return true;
                }
                return false;
            } else {
                eir = new EoptisData(0, 0, 0, 0, false, false, 0, EoptisWorkingMode.UNKNOWN, new List<EoptisParameter>(), new ToolResultsCollection());
                return false;
            }
        }

        private static bool parseData(string ip, DataChunkStruct dcs, out EoptisData eir) {

            eir = new EoptisData(-1, 0, -1, 0, false, false, -1, EoptisWorkingMode.UNKNOWN, new List<EoptisParameter>(), new ToolResultsCollection());
            bool res = false;
            if (dcs.totData_ > 0) {
                //Log.Line(LogLevels.Debug, "EoptisSvc.parseData", "TotData: {0}", dcs.totData_);
                if (dcs.data_ == IntPtr.Zero) {
                    Log.Line(LogLevels.Error, "EoptisSvc.parseData", "Null data pointer");
                    return res;
                }
            } else {
                return res;
            }
            int offset = 0;
            int spindleId = 0, rejectionCause = 0;
            uint vialId = 0;
            bool isReject = false;
            //int measureId = 0;
            EoptisWorkingMode ewm = EoptisWorkingMode.UNKNOWN;
            List<EoptisParameter> parameters = new List<EoptisParameter>();
            ToolResults tr = new ToolResults(0, true, false, true, null);
            for (uint iD = 0; iD < dcs.totData_; iD++) {
                //Log.Line(LogLevels.Debug, "EoptisSvc.parseData", "VariantDatum {0}: ", iD);
                IntPtr ptr = new IntPtr(dcs.data_.ToInt64() + offset);
                VariantDatumStruct vds = (VariantDatumStruct)Marshal.PtrToStructure(ptr, typeof(VariantDatumStruct));
                EoptisParameter ep = ParamsDictionary.Parameters[vds.id_];
                if (ep == null || ep.ByteLength != vds.length_ || ep.Type != vds.type_) {
                    Log.Line(LogLevels.Error, "EoptisSvc.parseData", "Parameter type or size is different from what expected! Expected type: {0}. Read type: {1}. Expected size: {2}. Read size: {3}", ep.Type, vds.type_, ep.ByteLength, vds.length_);
                    res = false;
                    break;
                }
                byte[] value = new byte[vds.length_];
                ResultsData[] rd = new ResultsData[ep.Length];
                Marshal.Copy(vds.data_, value, 0, (int)vds.length_);
                ep.Value = ep.GetValue(value);
                if (ep.Id == SYSTEM_ID.PAR_WORKING_MODE) {
                    if (Enum.TryParse<EoptisWorkingMode>(ep.Value, out ewm) == false)
                        Log.Line(LogLevels.Error, "EoptisSvc.parseData", "Error while parsing current working mode");
                } else if (ep.Id == SYSTEM_ID.ANALYSIS_VIAL_ID_COUNTER) {
                    if (uint.TryParse(ep.Value, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out vialId) == false)
                        Log.Line(LogLevels.Error, "EoptisSvc.parseData", "Error while parsing vial id counter");
                } else if (ep.Id == SYSTEM_ID.PAR_VIAL_SPINDLE_NUMBER) {
                    if (int.TryParse(ep.Value, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out spindleId) == false)
                        Log.Line(LogLevels.Error, "EoptisSvc.parseData", "Error while parsing spindle id counter");
                }
                  //if (ep.Id == SYSTEM_ID.ANALYSIS_IS_REJECT && !bool.TryParse(ep.Value, out isReject))
                  //    Log.Line(LogLevels.Error, "EoptisSvc.parseData", "Error while parsing good/reject boolean value");
                  else if (SpectrometerParamsId.Contains(ep.Id) == true || OpacimetroParamsId.Contains(ep.Id) == true) {
                    //parametro
                    parameters.Add(ep);
                    Trace.WriteLine("PARAMETER RECEIVED: " + ep.Id.ToString() + " = " + ep.Value);
                } else if (SpectrometerResultsId.Contains(ep.Id) == true || OpacimetroResultsId.Contains(ep.Id) == true) {
                    string deviceType = (EoptisNodeBase2.EoptisClients[ip].Stations[0].Cameras.First() as Camera).CameraType;
                    int measureId = -1;
                    bool isOk = true;
                    if (deviceType == "EoptisSpectrometer")
                        measureId = SpectrometerResultsId.FindIndex(spid => spid == ep.Id);
                    if (deviceType == "EoptisTurbidimeter")
                        measureId = OpacimetroResultsId.FindIndex(spid => spid == ep.Id);
                    //if (ep.Id == SYSTEM_ID.LIGHT_CONTROL) {
                    //    Parameter lightControlThParam = EoptisNodeBase2.EoptisClients[ip].GetParameters().Stations[0].Cameras[0].RecipeSimpleParameters.Find(pp => pp.Id == ep.Id.ToString());
                    //    double lightControlTh, lightControlValue;
                    //    if (lightControlThParam != null &&
                    //        double.TryParse(lightControlThParam.Value, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out lightControlTh) == true &&
                    //        double.TryParse(ep.Value, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out lightControlValue) == true) {
                    //        isOk = (lightControlValue < lightControlTh) ? false : true;
                    //    }
                    //    else {
                    //        Log.Line(LogLevels.Warning, "EoptisSvc.parseData", "Cannot compare light value with corresponding threshold");
                    //    }
                    //}
                    if (ep.Id == SYSTEM_ID.ANALYSIS_REJECTION_CAUSE_NUMBER) {
                        if (int.TryParse(ep.Value, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out rejectionCause) == false)
                            Log.Line(LogLevels.Error, "EoptisSvc.parseData", "Error while parsing rejection cause");
                        else {
                            isReject = (rejectionCause != 0) ? true : false;
                            tr.IsReject = isReject;
                        }
                    }
                    if ((ep.Threshold.Count > 0) &&
                        ((ep.Id >= SYSTEM_ID.PAR_RANGE_1_MEAN && ep.Id <= SYSTEM_ID.PAR_ALL_RANGE_MEAN) ||
                        (ep.Id == SYSTEM_ID.LIGHT_CONTROL))) {
                        Parameter rangeThParam = EoptisNodeBase2.EoptisClients[ip].GetParameters().Stations[0].Cameras.First().RecipeSimpleParameters.Find(pp => pp.Id == ep.Threshold[0].ToString());
                        double rangeTh, rangeValue;
                        if (rangeThParam != null &&
                            double.TryParse(rangeThParam.Value, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out rangeTh) == true &&
                            double.TryParse(ep.Value, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out rangeValue) == true) {
                            if (ep.Id == SYSTEM_ID.LIGHT_CONTROL)
                                isOk = (rangeValue > rangeTh) ? true : false;
                            else
                                isOk = (rangeValue < rangeTh) ? true : false;
                        } else {
                            Log.Line(LogLevels.Warning, "EoptisSvc.parseData", "Cannot compare result value with corresponding threshold: " + ep.Id.ToString());
                        }
                    }
                    int numOfPoints = ep.Value.Count(c => c == ';');
                    if ((numOfPoints % 2) == 1) numOfPoints++;
                    numOfPoints = Math.Max(numOfPoints / 2, 1);
                    MeasureResults mr = new MeasureResults(measureId, ep.Id.ToString(), "", isOk, true, ExactaEasyCore.MeasureTypeEnum.STRING, ep.Value, numOfPoints);
                    if (tr.ResMeasureCollection == null)
                        tr.ResMeasureCollection = new MeasureResultsCollection();
                    tr.ResMeasureCollection.Add(mr);
                    //measureId++;
                } else {
                    Log.Line(LogLevels.Error, "EoptisSvc.parseData", "Not expected data from Eoptis board");
                }
                //tecnicismo per passare al prossimo dato
                offset += Marshal.SizeOf(vds);
                int pad = (offset % 8 == 0) ? 0 : 8 - offset % 8;
                offset += pad;
                res = true;
            }
            ToolResultsCollection trc = (tr.ResMeasureCollection != null && tr.ResMeasureCollection.Count > 0) ? new ToolResultsCollection() { tr } : null;
            eir = new EoptisData(-1, 0, spindleId, vialId, true, isReject, rejectionCause, ewm, parameters, trc);
            return res;
        }

        #region GetSetParameters

        internal static string GetParameter(string ip, EoptisParameter param) {

            throw new NotImplementedException("EoptisSvc.GetParameter not implemented");
            //if (param == null)
            //    throw new Exception(ip + ": Get parameter failed. Invalid parameter");

            //if (isOffline) {
            //    Thread.Sleep(200);
            //    return "";
            //}
            //string res = "";
            //int err = 0;
            //lock (EoptisMng) {
            //    param = ParamsDictionary.Parameters[param.Id];
            //    byte[] buffer = new byte[param.ByteLength];
            //    VariantDatum vd = new VariantDatum(param.Id, param.ByteLength, param.Type);
            //    //getParamDummy(ip, ref vd.vds);
            //    err = getParameter(ip, ref vd.vds);
            //    Marshal.Copy(vd.vds.data_, buffer, 0, param.ByteLength);
            //    vd.Free();
            //    res = param.GetValue(buffer);
            //}
            //if (err != 0)
            //    throw new Exception(ip + ": Get parameter failed. Error: " + err.ToString());
            //return res;
        }

        internal static void Run(string ip, ref EoptisParameterCollection parameters) {

            lock (EoptisMng) {

                DataChunk dc = new DataChunk(parameters);
                getParameters(ip, ref dc.dcs);
            }
        }

        internal static void Live(string ip, bool enable) {

            EoptisParameterCollection parameters = new EoptisParameterCollection();
            EoptisParameter live_param = new EoptisParameter(SYSTEM_ID.PAR_WORKING_MODE, EoptisSvc.ParamsDictionary.Parameters[SYSTEM_ID.PAR_WORKING_MODE].Type, EoptisSvc.ParamsDictionary.Parameters[SYSTEM_ID.PAR_WORKING_MODE].ByteLength);
            live_param.Value = (enable == true) ? ((int)EoptisWorkingMode.WORKING_MODE_FREE_RUN).ToString() : ((int)EoptisWorkingMode.WORKING_MODE_IDLE).ToString();
            parameters.Add(live_param);
            EoptisSvc.SetParameters(ip, parameters);
        }

        internal static void GetParameters(string ip, ref EoptisParameterCollection parameters) {

            //if (isOffline) {
            //    foreach (EoptisParameter offEoParam in parameters) {
            //        offEoParam.Value = "0.57";
            //    }
            //    Thread.Sleep(200);
            //    return;
            //}
            lock (EoptisMng) {

                DataChunk dc = new DataChunk(parameters);
                getParameters(ip, ref dc.dcs);
            }
        }

        internal static void SetParameter(string ip, EoptisParameter param, string value) {

            throw new NotImplementedException("EoptisSvc.SetParameter not implemented");
            //if (param == null)
            //    throw new Exception(ip + ": Set parameter failed. Invalid parameter");

            ////if (isOffline) {
            ////    Thread.Sleep(200);
            ////    return;
            ////}
            //int err = 0;
            //lock (EoptisMng) {
            //    param = ParamsDictionary.Parameters[param.Id];
            //    VariantDatum vd = new VariantDatum(param.Id, param.ByteLength, param.Type);
            //    byte[] safeBuffer = param.SetValue(value);
            //    Marshal.Copy(safeBuffer, 0, vd.vds.data_, param.ByteLength);
            //    err = setParameter(ip, ref vd.vds);
            //    vd.Free();
            //}
            //if (err != 0)
            //    throw new Exception(ip + ": Set parameter failed. Error: " + err.ToString());
        }

        internal static void SetParameters(string ip, EoptisParameterCollection parameters) {

            //if (isOffline) {
            //    foreach (EoptisParameter offEoParam in parameters) {
            //        offEoParam.Value = "0.57";
            //    }
            //    Thread.Sleep(200);
            //    return;
            //}
            lock (EoptisMng) {

                DataChunk dc = new DataChunk(parameters);
                int offset = 0;
                for (uint iD = 0; iD < dc.dcs.totData_; iD++) {
                    EoptisParameter param = parameters[(int)iD];
                    byte[] safeBuffer = param.SetValue(parameters[param.Id].Value);
                    IntPtr ptr = new IntPtr(dc.dcs.data_.ToInt64() + offset);
                    VariantDatumStruct vds = (VariantDatumStruct)Marshal.PtrToStructure(ptr, typeof(VariantDatumStruct));
                    Marshal.Copy(safeBuffer, 0, vds.data_, param.ByteLength);

                    //tecnicismo per passare al prossimo dato
                    offset += Marshal.SizeOf(vds);
                    int pad = (offset % 8 == 0) ? 0 : 8 - offset % 8;
                    offset += pad;
                }
                setParameters(ip, ref dc.dcs);

                //freeData(ref dc.dcs);
            }
        }

        internal static void SetWorkingMode(string ip, EoptisWorkingMode wm) {

            throw new NotImplementedException("EoptisSvc.SetWorkingMode not implemented");
            //if (isOffline) {
            //    Thread.Sleep(200);
            //    return;
            //}
            //int err = 0;
            //lock (EoptisMng) {
            //    err = setWorkingMode(ip, (int)wm);
            //}
            //if (err != 0)
            //    throw new Exception(ip + ": Set parameter failed. Error: " + err.ToString());
        }
        #endregion
    }

    public class EoptisDataEventArgs : EventArgs
    {

        public string Ip;
        public int Id;

        public List<EoptisData> InspectionResultsCollection;

        public EoptisDataEventArgs(string ip, int id, List<EoptisData> measResCollection) {
            Ip = ip;
            Id = id;
            InspectionResultsCollection = measResCollection;
        }
    }

    public class EoptisData : InspectionResults
    {

        public EoptisWorkingMode WorkingMode;
        public List<EoptisParameter> Parameters;

        internal EoptisData(int nodeId, int inspectionId, int spindleId, uint vialId, bool isActive, bool isReject, int rejectionCause, EoptisWorkingMode ewm, List<EoptisParameter> parameters, ToolResultsCollection toolsResults)
            : base(nodeId, inspectionId, Convert.ToUInt32(spindleId), vialId, isActive, isReject, Convert.ToUInt16(rejectionCause), toolsResults) {

            WorkingMode = ewm;
            Parameters = parameters;
        }
    }

    public enum EoptisWorkingMode
    {

        UNKNOWN = -2,
        WORKING_MODE_IDLE = -1,                         /*!< -1: Modalità di funzionamento idle: all'accensione si mette in attesa per la modifica di parametri e della modalità d'utilizzo*/
        WORKING_MODE_CONTROL,                           /*!< 0: Modalità di funzionamento controllo*/
        WORKING_MODE_FREE_RUN,                          /*!< 1: Modalità di funzionamento free run*/
        WORKING_MODE_MECHANICAL_TEST,                   /*!< 2: Modalità di funzionamento test meccanico*/
        WORKING_MODE_TOT,                               /*!< 3: Parametro che tiene conto di tutte le modalità di funzionamento*/
        FORCE_WORKING_MODE_MAX_INT = 0x7FFFFFFF         /*!< 0x7FFFFFFF: Parametro inutilizzato per forzare gli altri ID ad un valore di tipo int */
    }
}
