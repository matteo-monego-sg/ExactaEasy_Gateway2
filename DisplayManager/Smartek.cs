using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using ExactaEasyCore;
using System.Net.NetworkInformation;
using sclib;
using SPAMI.Util.Logger;

namespace DisplayManager {
    public class Smartek : Device, IStrobeController {

        protected internal static CultureInfo culture = CultureInfo.GetCultureInfo("en-GB");

        static sclib.IScLibAPI scLibApi = null;

        static Smartek() {

            sclib.ScLibSDK.InitScLibAPI();
            scLibApi = sclib.ScLibSDK.GetScLibAPI();
        }

        static void smartek_Event(IntPtr hApi, IntPtr hScController, uint type, ref CallbackInfo info) {
        }

        sclib.IScController controller = null;
        sclib.IScPhysOutput output = null;
        sclib.IScTiming timing = null;

        int channelCount = 0;
        int triggerCount = 0;

        int[] _triggerMap;

        public int Id { get; private set; }
        public string StrobeAddress { get; private set; }

        public Smartek(int strobeId, string strobeAddress, string settingsPath, int[] triggerMap)
            : this(strobeId, strobeAddress, settingsPath) {

            if (triggerMap.Length != channelCount)
                throw new ArgumentOutOfRangeException("triggerMap");
            else
                for (int i = 0; i < channelCount; i++) {
                    if (triggerMap[i] >= 0 && triggerMap[i] < triggerCount) {
                        _triggerMap[i] = triggerMap[i];
                        setTriggerIndex(_triggerMap[i], i);
                    }
                    else
                        throw new ArgumentOutOfRangeException("triggerMap[" + i.ToString() + "]");
                }
        }

        public Smartek(int strobeId, string strobeAddress, string settingsPath) {

            Id = strobeId;
            StrobeAddress = strobeAddress;
            scLibApi.RegisterCallback(new _callbackEvent(this));
        }

        protected override void ExecDeviceConnection() {

            if (CommunicationStatus == DeviceCommunicationStates.Disconnected || CommunicationStatus == DeviceCommunicationStates.Faulted) {

                try {
                    CommunicationStatus = DeviceCommunicationStates.Connecting;
                    controller = findController(StrobeAddress);
                    controller.Connect();

                    output = controller.GetPhysOutput();
                    channelCount = output.GetCurrentOutputsCount();

                    timing = controller.GetTiming();
                    triggerCount = timing.GetTriggerInputCount();

                    _triggerMap = new int[channelCount];
                    for (int i = 0; i < channelCount; i++) {
                        _triggerMap[i] = i;
                        setTriggerIndex(i, i);
                    }
                    CommunicationStatus = DeviceCommunicationStates.Connected;
                    Log.Line(LogLevels.Pass, "Smartek.ExecDeviceConnection", "Connected to " + StrobeAddress);
                }
                catch {
                    CommunicationStatus = DeviceCommunicationStates.Faulted;
                    Log.Line(LogLevels.Warning, "Smartek.ExecDeviceConnection", "Connection to " + StrobeAddress + " failed");
                }
            }
        }

        public override void CheckDeviceCommunicationState() {

            try {
                controller = findController(StrobeAddress);
                if (controller == null)
                    CommunicationStatus = DeviceCommunicationStates.Faulted;
                else {
                    if (!controller.IsConnected())
                        CommunicationStatus = DeviceCommunicationStates.Faulted;
                }
            }
            catch {
                CommunicationStatus = DeviceCommunicationStates.Faulted;
            }
        }

        //protected override void OnCommunicationStateChanged(object sender, CommunicationStateChangedEventArgs e) {

        //    base.OnCommunicationStateChanged(sender, e);
        //    //if ((e.PreviousState == DeviceCommunicationStates.Connected || e.PreviousState == DeviceCommunicationStates.Connecting) &&
        //    //    e.CurrentState == DeviceCommunicationStates.Faulted) {

        //    //        StartConnect(true);
        //    //}
        //}

        public ExactaEasyCore.ParameterCollection<ExactaEasyCore.Parameter> GetStrobeParameter(int channelId) {

            if (channelId >= 4)
                channelId = 0; // prendo il primo perchè dovrebbero essere tutti uguali          

            ParameterCollection<Parameter> list = new ParameterCollection<Parameter>();

            list.Add("stroboOperatingMode", getOperatingMode());
            list.Add("stroboMaxOutputVoltage", getMaxOutputVoltage().ToString(culture));
            list.Add("stroboCurrent", getStroboCurrent(channelId).ToString(culture));
            list.Add("stroboPulseDelay", getTriggerDelayTime(_triggerMap[channelId]).ToString(culture));
            list.Add("stroboPulseWidth", getTriggerOnTime(_triggerMap[channelId]).ToString(culture));
            //list.Add("stroboRetriggerTime", lc.ControllerSpecs.ChanConfig[channelId].OperatingRetrigger.ToString(culture));
            //list.Add("stroboContinuousMaxCurrent", lc.ControllerSpecs.ChanConfig[channelId].MaxCurrentContinuousModeA.ToString(culture));
            //list.Add("stroboPulsedMaxCurrent", lc.ControllerSpecs.ChanConfig[channelId].MaxCurrentPulsedModeA.ToString(culture));
            return list;

        }

        public void SetStrobeParameter(int channelId, ExactaEasyCore.ParameterCollection<ExactaEasyCore.Parameter> parameters) {

            try {
                setOperatingMode(parameters["stroboOperatingMode"].Value);
                if (parameters["stroboMaxOutputVoltage"] != null)
                    setMaxOutputVoltage(Convert.ToDouble(parameters["stroboMaxOutputVoltage"].Value, culture));

                if (channelId >= channelCount) {
                    //13.08.2014 GR attenzione Patch! per gli illuminatori dal retro
                    //si usano tutti i canali di uscita con un solo input
                    for (int ind = 0; ind < channelCount; ind++) {
                        setStroboCurrent(ind, Convert.ToDouble(parameters["stroboCurrent"].Value, culture));
                        setTriggerDelayTime(_triggerMap[ind], Convert.ToDouble(parameters["stroboPulseDelay"].Value, culture));
                        setTriggerOnTime(_triggerMap[ind], Convert.ToDouble(parameters["stroboPulseWidth"].Value, culture));
                    }
                }
                else {
                    setStroboCurrent(channelId, Convert.ToDouble(parameters["stroboCurrent"].Value, culture));
                    setTriggerDelayTime(_triggerMap[channelId], Convert.ToDouble(parameters["stroboPulseDelay"].Value, culture));
                    setTriggerOnTime(_triggerMap[channelId], Convert.ToDouble(parameters["stroboPulseWidth"].Value, culture));
                }

                //lc.ControllerSpecs.ChanConfig[channelId].OperatingRetrigger = Convert.ToDouble(parameters["stroboRetriggerTime"].GetValue(), culture);
                //lc.ControllerSpecs.ChanConfig[channelId].MaxCurrentContinuousModeA = Convert.ToDouble(parameters["stroboContinuousMaxCurrent"].GetValue(), culture);
                //lc.ControllerSpecs.ChanConfig[channelId].MaxCurrentPulsedModeA = Convert.ToDouble(parameters["stroboPulsedMaxCurrent"].GetValue(), culture);
            }
            catch {
                throw;
            }

        }

        public void ApplyParameters(int channelId) {

            controller.BeginTransaction(sclib.Transaction.TR_SEND_PARAMS);
            controller.WaitForTransaction();
        }

        public void Dispose() {

            if (controller!=null && controller.IsConnected()) {
                controller.Disconnect();
            sclib.ScLibSDK.ExitScLibAPI();
            }
        }

        sclib.IScController findController(string strobeAddress) {

            scLibApi.FindControllers(sclib.ConnectionType.CONNECTION_ALL, "*");
            sclib.IScController[] controllers = scLibApi.GetAllControllers();

            sclib.IScController controller = null;

            for (int i = 0; i < controllers.Length; i++)
                if (controllers[i].GetConnectionParam(0) == strobeAddress)
                    controller = controllers[i];

            return controller;
        }

        public string getOperatingMode() {

            controller.BeginTransaction(sclib.Transaction.TR_READ_ALL);
            controller.WaitForTransaction();

            string sRunningMode = "";
            sclib.RunMode rMode = (sclib.RunMode)controller.GetRunningMode();
            switch (rMode) {
                case sclib.RunMode.RUN_MODE_OFF:
                    sRunningMode = "OFF";
                    break;
                case sclib.RunMode.RUN_MODE_CONT:
                    sRunningMode = "Continuous";
                    break;
                case sclib.RunMode.RUN_MODE_EXT_TRG:
                    sRunningMode = "Pulsed";
                    break;
            }
            return sRunningMode;
        }

        public void setOperatingMode(string operatingMode) {

            sclib.RunMode oMode = sclib.RunMode.RUN_MODE_ALL;
            switch (operatingMode.ToLower()) {
                case "off":
                    oMode = sclib.RunMode.RUN_MODE_OFF;
                    break;
                case "continuous":
                    oMode = sclib.RunMode.RUN_MODE_CONT;
                    break;
                case "pulsed":
                    oMode = sclib.RunMode.RUN_MODE_EXT_TRG;
                    break;
            }
            if (oMode != sclib.RunMode.RUN_MODE_ALL)
                controller.SetRunningMode(oMode);

            if (oMode == sclib.RunMode.RUN_MODE_EXT_TRG)
                setTriggerMode(sclib.TriggerMode.TRG_MODE_ENABLE);
            else
                setTriggerMode(sclib.TriggerMode.TRG_MODE_DISABLE);

        }

        sclib.TriggerMode getTriggerMode(int triggerIndex) {

            return (sclib.TriggerMode)timing.GetTriggerMode(triggerIndex);
        }

        public void setTriggerMode(sclib.TriggerMode triggerMode) {

            for (int i = 0; i < channelCount; i++)
                setTriggerMode(_triggerMap[i], triggerMode);
        }

        public void setTriggerMode(int triggerIndex, sclib.TriggerMode triggerMode) {

            timing.SetTriggerMode(triggerIndex, triggerMode);
        }

        public void setTriggerIndex(int triggerIndex, int channelId) {

            //GR attenzione impostato tutti i canali di uscita con lo stesso trigger
            //Da sistemare e fare in modo che sia configurabile!
            if (triggerIndex < 0 || triggerIndex >= triggerCount)
                throw new ArgumentOutOfRangeException("triggerIndex");

            if (channelId < 0 || channelId >= channelCount)
                throw new ArgumentOutOfRangeException("channelId");

            // output.SetTriggerIndex(channelId, triggerIndex);
            output.SetTriggerIndex(channelId, 0);
        }

        double getStroboCurrent(int channelId) {

            double current = 0;
            if (channelId >= 0 && channelId < channelCount) {
                current = output.GetCurrentOutputParams(channelId).GetValue();
            }
            else {
                throw new ArgumentOutOfRangeException("channelId");
            }
            return current;
        }

        void setStroboCurrent(int channelId, double stroboCurrent) {

            if (channelId >= 0 && channelId < channelCount) {
                sclib.IRangeValue param = output.GetCurrentOutputParams(channelId);
                if (stroboCurrent >= param.GetMin() && stroboCurrent <= param.GetMax())
                    output.GetCurrentOutputParams(channelId).SetValue(stroboCurrent);
                else
                    throw new ArgumentOutOfRangeException("stroboCurrent");
            }
            else {
                throw new ArgumentOutOfRangeException("channelId");
            }
        }

        double getTriggerDelayTime(int triggerId) {

            //delay time in ms sul supervisore in sec sul controller

            double delay = 0;
            if (triggerId >= 0 && triggerId < triggerCount) {
                delay = timing.GetTriggerDelayTime(triggerId).GetValue() * 1000;
            }
            else {
                throw new ArgumentOutOfRangeException("triggerId");
            }
            return delay;
        }

        void setTriggerDelayTime(int triggerId, double triggerDelay) {

            //delay time in ms sul supervisore in sec sul controller
            triggerDelay /= 1000;

            if (triggerId >= 0 && triggerId < triggerCount) {
                sclib.IRangeValue param = timing.GetTriggerDelayTime(triggerId);
                if (triggerDelay >= param.GetMin() && triggerDelay <= param.GetMax())
                    timing.GetTriggerDelayTime(triggerId).SetValue(triggerDelay);
                else
                    throw new ArgumentOutOfRangeException("triggerDelay");
            }
            else {
                throw new ArgumentOutOfRangeException("triggerId");
            }
        }

        double getTriggerOnTime(int triggerId) {

            //Trigger on time in ms sul supervisore in sec sul controller

            double onTime = 0;
            if (triggerId >= 0 && triggerId < triggerCount) {
                onTime = timing.GetTriggerOnTime(triggerId).GetValue() * 1000;
            }
            else {
                throw new ArgumentOutOfRangeException("triggerId");
            }
            return onTime;
        }

        void setTriggerOnTime(int triggerId, double triggerOnTime) {

            //Trigger on time in ms sul supervisore in sec sul controller
            triggerOnTime /= 1000;

            if (triggerId >= 0 && triggerId < triggerCount) {
                sclib.IRangeValue param = timing.GetTriggerDelayTime(triggerId);
                if (triggerOnTime >= param.GetMin() && triggerOnTime <= param.GetMax())
                    timing.GetTriggerOnTime(triggerId).SetValue(triggerOnTime);
                else
                    throw new ArgumentOutOfRangeException("triggerOnTime");
            }
            else {
                throw new ArgumentOutOfRangeException("triggerId");
            }
        }

        double getMaxOutputVoltage() {

            sclib.IRangeValue param = output.GetVoltageOutputParams(0);
            return param.GetValue();
        }

        void setMaxOutputVoltage(double maxVoltage) {

            sclib.IRangeValue param = output.GetVoltageOutputParams(0);
            if (maxVoltage >= param.GetMin() && maxVoltage <= param.GetMax())
                output.GetVoltageOutputParams(0).SetValue(maxVoltage);
            else
                throw new ArgumentOutOfRangeException("maxVoltage");

        }

        class _callbackEvent : ICallbackEvent {

            public Smartek sc;

            public _callbackEvent(Smartek stroboController) {

                sc = stroboController;
            }

            public void OnConnect(IScController device) {


            }

            public void OnDisconnect(IScController device) {


            }

            public void OnLog(IScController device, EventMessage message) {


            }
        }

    }

}
