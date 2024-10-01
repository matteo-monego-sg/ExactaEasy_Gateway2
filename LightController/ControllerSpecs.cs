using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPAMI.Util;
using SPAMI.Util.Logger;

namespace SPAMI.LightControllers
{
    [Serializable]
    //[TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class ControllerSpecs : ICommon
    {
        public string ClassName { get; set; }
        public ControllerVendor ControllerVendor { get; set; }
        public ControllerFamily ControllerFam { get; set; }
        public ControllerSpecs()
        {
            ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
        }

        public virtual void Reset()
        {
            ChanConfig = new List<ChanConfig>(FixedOutputChannelNumber);
            for (int i = 0; i < FixedOutputChannelNumber; i++)
            {
                ChanConfig.Add(new ChanConfig(i));
            }
        }

        public double GetMaxBright(int channel)
        {
            if (ControllerFam == ControllerFamily.Gardasoft_RT_SERIES)
            {
                switch ((LightModeGardasoftRT)ChanConfig[channel].OperatingMode)
                {
                    case LightModeGardasoftRT.Continuous:
                        return ChanConfig[channel].MaxCurrentContinuousModeA;
                    case LightModeGardasoftRT.Pulsed:
                        return ChanConfig[channel].MaxCurrentPulsedModeA;
                    case LightModeGardasoftRT.Switched:
                        return ChanConfig[channel].MaxCurrentPulsedModeA;
                    case LightModeGardasoftRT.Selected:
                        return ChanConfig[channel].MaxCurrentPulsedModeA;
                    case LightModeGardasoftRT.OFF:
                        return 0;
                    default:
                        return Math.Min(ChanConfig[channel].MaxCurrentContinuousModeA, ChanConfig[channel].MaxCurrentPulsedModeA);
                }
            }
            if (ControllerFam == ControllerFamily.Gardasoft_PP_SERIES)
            {
                switch ((LightModeGardasoftPP)ChanConfig[channel].OperatingMode)
                {
                    case LightModeGardasoftPP.Continuous:
                        return ChanConfig[channel].MaxCurrentContinuousModeA;
                    case LightModeGardasoftPP.Pulsed:
                        return ChanConfig[channel].MaxCurrentPulsedModeA;
                    case LightModeGardasoftPP.Switched:
                        return ChanConfig[channel].MaxCurrentPulsedModeA;
                    case LightModeGardasoftPP.None:
                    case LightModeGardasoftPP.OFF:
                        return 0;
                    default:
                        return Math.Min(ChanConfig[channel].MaxCurrentContinuousModeA, ChanConfig[channel].MaxCurrentPulsedModeA);
                }
            }
            return -1;
        }

        public void SetMaxBright(int channel, double value)
        {
            if (ControllerFam == ControllerFamily.Gardasoft_RT_SERIES)
            {
                switch ((LightModeGardasoftRT)ChanConfig[channel].OperatingMode)
                {
                    case LightModeGardasoftRT.Continuous:
                        ChanConfig[channel].MaxCurrentContinuousModeA = value;
                        break;
                    case LightModeGardasoftRT.Pulsed:
                        ChanConfig[channel].MaxCurrentPulsedModeA = value;
                        break;
                    case LightModeGardasoftRT.Switched:
                        ChanConfig[channel].MaxCurrentContinuousModeA = value;
                        break;
                    case LightModeGardasoftRT.Selected:
                        ChanConfig[channel].MaxCurrentContinuousModeA = value;
                        break;
                    case LightModeGardasoftRT.OFF:
                        break;
                    default:
                        Log.Line(LogLevels.Error, ClassName + ".SetMaxBright", "Modalità operativa non contemplata.");
                        //throw new LightControllerException("Modalità operativa non contemplata.");
                        break;
                }
            }
            if (ControllerFam == ControllerFamily.Gardasoft_PP_SERIES)
            {
                switch ((LightModeGardasoftPP)ChanConfig[channel].OperatingMode)
                {
                    case LightModeGardasoftPP.Continuous:
                        ChanConfig[channel].MaxCurrentContinuousModeA = value;
                        break;
                    case LightModeGardasoftPP.Pulsed:
                        ChanConfig[channel].MaxCurrentPulsedModeA = value;
                        break;
                    case LightModeGardasoftPP.Switched:
                        ChanConfig[channel].MaxCurrentContinuousModeA = value;
                        break;
                    case LightModeGardasoftPP.None:
                    case LightModeGardasoftPP.OFF:
                        break;
                    default:
                        Log.Line(LogLevels.Error, ClassName + ".SetMaxBright", "Modalità operativa non contemplata.");
                        break;
                }
            }
            CheckOperatingCurrent(channel);
        }

        public void CheckOperatingCurrent(int channel)
        {
            if (ControllerFam == ControllerFamily.Gardasoft_RT_SERIES)
            {
                switch ((LightModeGardasoftRT)ChanConfig[channel].OperatingMode)
                {
                    case LightModeGardasoftRT.Continuous:
                        if (ChanConfig[channel].OperatingCurrent > ChanConfig[channel].MaxCurrentContinuousModeA)
                            ChanConfig[channel].OperatingCurrent = ChanConfig[channel].MaxCurrentContinuousModeA;
                        break;
                    case LightModeGardasoftRT.Pulsed:
                        if (ChanConfig[channel].OperatingCurrent > ChanConfig[channel].MaxCurrentPulsedModeA)
                            ChanConfig[channel].OperatingCurrent = ChanConfig[channel].MaxCurrentPulsedModeA;
                        break;
                    case LightModeGardasoftRT.Switched:
                        if (ChanConfig[channel].OperatingCurrent > ChanConfig[channel].MaxCurrentContinuousModeA)
                            ChanConfig[channel].OperatingCurrent = ChanConfig[channel].MaxCurrentContinuousModeA;
                        break;
                    case LightModeGardasoftRT.Selected:
                        if (ChanConfig[channel].OperatingCurrent > ChanConfig[channel].MaxCurrentContinuousModeA)
                            ChanConfig[channel].OperatingCurrent = ChanConfig[channel].MaxCurrentContinuousModeA;
                        break;
                    case LightModeGardasoftRT.OFF:
                        if (ChanConfig[channel].OperatingCurrent != 0)
                            ChanConfig[channel].OperatingCurrent = 0;
                        break;
                    default:
                        Log.Line(LogLevels.Error, ClassName + ".CheckOperatingCurrent", "Modalità operativa non contemplata.");
                        break;
                }
            }
            if (ControllerFam == ControllerFamily.Gardasoft_PP_SERIES)
            {
                switch ((LightModeGardasoftPP)ChanConfig[channel].OperatingMode)
                {
                    case LightModeGardasoftPP.Continuous:
                        if (ChanConfig[channel].OperatingCurrent > ChanConfig[channel].MaxCurrentContinuousModeA)
                            ChanConfig[channel].OperatingCurrent = ChanConfig[channel].MaxCurrentContinuousModeA;
                        break;
                    case LightModeGardasoftPP.Pulsed:
                        if (ChanConfig[channel].OperatingCurrent > ChanConfig[channel].MaxCurrentPulsedModeA)
                            ChanConfig[channel].OperatingCurrent = ChanConfig[channel].MaxCurrentPulsedModeA;
                        break;
                    case LightModeGardasoftPP.Switched:
                        if (ChanConfig[channel].OperatingCurrent > ChanConfig[channel].MaxCurrentContinuousModeA)
                            ChanConfig[channel].OperatingCurrent = ChanConfig[channel].MaxCurrentContinuousModeA;
                        break;
                    case LightModeGardasoftPP.None:
                    case LightModeGardasoftPP.OFF:
                        if (ChanConfig[channel].OperatingCurrent != 0)
                            ChanConfig[channel].OperatingCurrent = 0;
                        break;
                    default:
                        Log.Line(LogLevels.Error, ClassName + ".CheckOperatingCurrent", "Modalità operativa non contemplata.");
                        break;
                }
            }
        }
        public string GetStringOperatingMode(int channel)
        {
            if (ControllerFam == ControllerFamily.Gardasoft_PP_SERIES)
                return ((LightModeGardasoftPP)ChanConfig[channel].OperatingMode).ToString();
            if (ControllerFam == ControllerFamily.Gardasoft_RT_SERIES)
                return ((LightModeGardasoftRT)ChanConfig[channel].OperatingMode).ToString();
            return "";
        }
        public void SetStringOperatingMode(int channel, string value)
        {
            bool success = false;
            int retVal = 0;
            value = value.ToLower().UppercaseFirst();
            if (ControllerFam == ControllerFamily.Gardasoft_PP_SERIES)
            {
                LightModeGardasoftPP retValue;
                success = Enum.TryParse<LightModeGardasoftPP>(value, true, out retValue);
                if (success) retVal = (int)retValue;
            }
            if (ControllerFam == ControllerFamily.Gardasoft_RT_SERIES)
            {
                LightModeGardasoftRT retValue;
                success = Enum.TryParse<LightModeGardasoftRT>(value, true, out retValue);
                if (success) retVal = (int)retValue;
            }
            if (success) ChanConfig[channel].OperatingMode = retVal;
            else Log.Line(LogLevels.Error, ".SetStringOperatingMode", "Errore! Operating mode \"" + value + "\" non supportato!");
        }
        public void SetOperatingDelay(int channel, double value)
        {
            if (value < FixedMinPulseWidthMSec) value = FixedMinPulseWidthMSec;
            if (value > FixedMaxPulseWidthMSec) value = FixedMaxPulseWidthMSec;
            ChanConfig[channel].OperatingDelay = value;
        }

        public void SetOperatingPulseWidth(int channel, double value)
        {
            if (value < FixedMinPulseWidthMSec) value = FixedMinPulseWidthMSec;
            if (value > FixedMaxPulseWidthMSec) value = FixedMaxPulseWidthMSec;
            ChanConfig[channel].OperatingPulseWidth = value;
        }

        
        public string IP { get; set; }
        public int Port { get; set; }
        public int FixedOutputChannelNumber { get; set; }          // []
        public int FixedInputChannelNumber { get; set; }           // []
        public double FixedCurrentResolutionA { get; set; }        // [A]
        public double FixedCurrentFullScaleA { get; set; }         // [A]
        public double FixedMinPulseWidthMSec { get; set; }         // [ms]
        public double FixedMaxPulseWidthMSec { get; set; }         // [ms]
        public double FixedMinInternalTriggerFreq { get; set; }    // [Hz]
        public double FixedMaxInternalTriggerFreq { get; set; }    // [Hz]
        public int SendTimeoutSec { get; set; }                    // [s]
        public ComMode ComMode { get; set; }
        public string HeartBeatString { get; set; }
        public string CmdSeparator { get; set; }
        public string CmdSaveToFlash { get; set; }
        public string DecimalSeparator { get; set; }
        public string MulNumericValSeparator { get; set; }
        public string ReplyTerminator { get; set; }
        public string HelpStringPath { get; set; }
        public bool InternalTrigger { get; set; }
        public double InternalTriggerMs                             // [ms]
        {
            get
            {
                return InternalTriggerMsVal;
            }
            set
            {
                InternalTriggerMsVal = value;
                if (InternalTriggerMsVal > (1000.0 / FixedMinInternalTriggerFreq)) InternalTriggerMsVal = 1000.0 / FixedMinInternalTriggerFreq;
                if (InternalTriggerMsVal < (1000.0 / FixedMaxInternalTriggerFreq)) InternalTriggerMsVal = 1000.0 / FixedMaxInternalTriggerFreq;
            }
        }
        public int ExternalTriggerType { get; set; }       
        public List<ChanConfig> ChanConfig { get; set; }

        private double InternalTriggerMsVal;
    }

    [Serializable]
    //[TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class ChanConfig
    {
        public ChanConfig()
        {
        }
        public ChanConfig(int _IDchan)
        {
            IDchan = _IDchan;
        }
        //private ControllerSpecs parent;
        public int IDchan { get; set; }
        public int OperatingMode { get; set; }                                  // []
        public double OperatingCurrent { get; set; }                            // [A]
        public double MaxCurrentContinuousModeA { get; set; }                   // [A]
        public double MaxCurrentPulsedModeA { get; set; }                       // [A]
        public double OperatingSelectedModeAddParam { get; set; }               // [A] usata solo in "Selected mode"
        public double OperatingDelay { get; set; }                              // [ms]
        public double OperatingPulseWidth { get; set; }                         // [ms]
        public double OperatingRetrigger { get; set; }                          // [ms]
        public int OperatingInputTrigger { get; set; }
        public bool E { get; set; }
        public bool P { get; set; }

        public List<int> PossibleInputTrigger { get; set; }
    }

    [Serializable]
    public class RT820F_2 : ControllerSpecs
    {
        public RT820F_2()
        {
            ControllerFam = LightControllers.ControllerFamily.Gardasoft_RT_SERIES;
            IP = "192.168.1.1";
            Port = 30313;
            FixedOutputChannelNumber = 8;               // []
            FixedCurrentResolutionA = 0.005;            // [A]
            FixedCurrentFullScaleA = 3;                 // [A]
            FixedMinPulseWidthMSec = 0.001;             // [ms]
            FixedMaxPulseWidthMSec = 999.0;             // [ms]
            FixedMinInternalTriggerFreq = 0.2;          // [Hz]
            FixedMaxInternalTriggerFreq = 5000;         // [Hz]
            SendTimeoutSec = 8;                         // [s]
            ComMode = LightControllers.ComMode.Ethernet;
            HeartBeatString = "VR";
            CmdSeparator = ";";
            CmdSaveToFlash = "AW";
            DecimalSeparator = ".";
            MulNumericValSeparator = ",";
            ReplyTerminator = ">";
        }

        public override void Reset()
        {
            base.Reset();
            foreach (ChanConfig cc in base.ChanConfig)
            {
                int[] Range;
                Range = (cc.IDchan < 4) ? new int[4] { 0, 1, 2, 3 } : new int[4] { 4, 5, 6, 7 };
                cc.PossibleInputTrigger = new List<int>(4);
                cc.PossibleInputTrigger.AddRange(Range);
            }
        }
    }

    [Serializable]
    public class RT820F_20 : ControllerSpecs
    {
        public RT820F_20()
        {
            ControllerFam = LightControllers.ControllerFamily.Gardasoft_RT_SERIES;
            IP = "192.168.1.1";
            Port = 30313;
            FixedOutputChannelNumber = 8;               // []
            FixedCurrentResolutionA = 0.005;            // [A]
            FixedCurrentFullScaleA = 3;                 // [A]
            FixedMinPulseWidthMSec = 0.001;             // [ms]
            FixedMaxPulseWidthMSec = 999.0;             // [ms]
            FixedMinInternalTriggerFreq = 0.2;          // [Hz]
            FixedMaxInternalTriggerFreq = 5000;         // [Hz]
            SendTimeoutSec = 8;                         // [s]
            ComMode = LightControllers.ComMode.Ethernet;
            HeartBeatString = "VR";
            CmdSeparator = ";";
            CmdSaveToFlash = "AW";
            DecimalSeparator = ".";
            MulNumericValSeparator = ",";
            ReplyTerminator = ">";
        }

        public override void Reset()
        {
            base.Reset();
            foreach (ChanConfig cc in base.ChanConfig)
            {
                int[] Range;
                Range = (cc.IDchan < 4) ? new int[4] { 0, 1, 2, 3 } : new int[4] { 4, 5, 6, 7 };
                cc.PossibleInputTrigger = new List<int>(4);
                cc.PossibleInputTrigger.AddRange(Range);
            }
        }
    }

    [Serializable]
    public class PP820 : ControllerSpecs
    {
        public PP820()
        {
            ControllerFam = LightControllers.ControllerFamily.Gardasoft_PP_SERIES;
            IP = "192.168.1.1";
            Port = 30313;
            FixedOutputChannelNumber = 8;               // []
            FixedCurrentResolutionA = 0.100;            // [A]
            FixedCurrentFullScaleA = 20;                // [A]
            FixedMinPulseWidthMSec = 0.001;             // [ms]
            FixedMaxPulseWidthMSec = 300.0;             // [ms]
            FixedMinInternalTriggerFreq = 0.2;          // [Hz]
            FixedMaxInternalTriggerFreq = 5000;         // [Hz]
            SendTimeoutSec = 8;                         // [s]
            ComMode = LightControllers.ComMode.Ethernet;
            HeartBeatString = "VR";
            CmdSeparator = ";";
            CmdSaveToFlash = "AW";
            DecimalSeparator = ".";
            MulNumericValSeparator = ",";
            ReplyTerminator = ">";
        }

        public override void Reset()
        {
            base.Reset();
            foreach (ChanConfig cc in base.ChanConfig)
            {
                int[] Range;
                Range = (cc.IDchan < 4) ? new int[4] { 0, 1, 2, 3 } : new int[4] { 4, 5, 6, 7 };
                cc.PossibleInputTrigger = new List<int>(4);
                cc.PossibleInputTrigger.AddRange(Range);
            }
        }

        /*public string GetStringOperatingMode()
        { 
            (LightModeGardasoftPP)this.OperatingMode
        }*/
    }

    [Serializable]
    public class DeviceDetected
    {
        public ControllerName Model;
        public string SerialNumber { get; private set; }
        public string IP { get; set; }
        public ControllerVendor Vendor { get; private set; }
        //int port;

        public DeviceDetected()
        {
        }
        public DeviceDetected(string vendor, string model, string serialNumber, string ip)
        {
            ControllerVendor ctrlVendor;
            bool success = Enum.TryParse<ControllerVendor>(vendor, true, out ctrlVendor);
            if (success) Vendor = ctrlVendor;
            else Vendor = ControllerVendor.NotSet;
            Model = StringToModel(model);
            SerialNumber = serialNumber;
            IP = ip;
        }
        public override string ToString()
        {
            return (Vendor.ToString() + " " + Model.ToString() + " " + IP + " SN: " + SerialNumber);
            //return base.ToString();
        }
        private ControllerName StringToModel(string model)
        {
            if (model == "RT820F-2") return ControllerName.RT820F_2;
            else if (model == "RT820F-20") return ControllerName.RT820F_20;
            else if (model == "PP820") return ControllerName.PP820;
            else return ControllerName.None;
        }
    }

    public class LightControllerException : Exception
    {
        public LightControllerException(string message)
            : base(message) 
        {
        }
    }

    [Serializable]
    public enum ControllerName
    {
        None,
        RT820F_2,
        RT820F_20,
        PP820
    }

    [Serializable]
    public enum ControllerFamily
    {
        Gardasoft_RT_SERIES,
        Gardasoft_PP_SERIES,
    }

    [Serializable]
    public enum ControllerVendor
    {
        NotSet,
        Gardasoft,
    }

    [Serializable]
    public enum LightModeGardasoftRT
    {
        Continuous,
        Pulsed,
        Switched,
        Selected,
        OFF
    }

    [Serializable]
    public enum LightModeGardasoftPP
    {
        None,
        Pulsed,
        Continuous,
        Switched,
        OFF
    }

    [Serializable]
    public enum ComMode
    {
        Ethernet,
        RS232
    }
}
