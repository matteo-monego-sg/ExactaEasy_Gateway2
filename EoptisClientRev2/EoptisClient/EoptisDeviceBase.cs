using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using DisplayManager;
using ExactaEasyCore;
using ExactaEasyEng;
using SPAMI.Util.Logger;

namespace EoptisClient {

    public class EoptisDeviceBase : Camera {

        //string infoSerialNumber, infoFirmwareVersion, infoSdkVersion, infoIPAddress, infoPort, infoFpgaVer;
        //string spindleCount, workingMode, ledGreenPower;
        public CameraWorkingMode CurrWorkingMode { get; set; }
        //MachineModeEnum machineMode;
        //public new MachineModeEnum MachineMode {
        //    get {
        //        return machineMode;
        //    }
        //    set {
        //        machineMode = value;
        //        try {
        //            if (machineMode == MachineModeEnum.Running)
        //                SetWorkingMode(CameraWorkingMode.ExternalSource);
        //        }
        //        catch {
        //            Log.Line(LogLevels.Warning, "EoptisDeviceBase.MachineMode.set", "");
        //        }
        //    }
        //}

        ParameterCollection<Parameter> acqParams;
        ParameterCollection<Parameter> featEnParams;
        ParameterCollection<Parameter> elaborationParams;
        ParameterCollection<Parameter> recipeAdvancedParams;
        ParameterCollection<Parameter> roiParams;
        ParameterCollection<Parameter> machineParams;
        ParameterCollection<Parameter> strobeParams;

        public EoptisDeviceBase(CameraDefinition cameraDefinition, bool scanRequest)
            : base(cameraDefinition) {

            acqParams = new ParameterCollection<Parameter>(AppEngine.Current.ParametersInfo, AppEngine.Current.CurrentContext.CultureCode);
            featEnParams = new ParameterCollection<Parameter>(AppEngine.Current.ParametersInfo, AppEngine.Current.CurrentContext.CultureCode);
            elaborationParams = new ParameterCollection<Parameter>(AppEngine.Current.ParametersInfo, AppEngine.Current.CurrentContext.CultureCode);
            recipeAdvancedParams = new ParameterCollection<Parameter>(AppEngine.Current.ParametersInfo, AppEngine.Current.CurrentContext.CultureCode);
            roiParams = new ParameterCollection<Parameter>(AppEngine.Current.ParametersInfo, AppEngine.Current.CurrentContext.CultureCode);
            machineParams = new ParameterCollection<Parameter>(AppEngine.Current.ParametersInfo, AppEngine.Current.CurrentContext.CultureCode);
            strobeParams = new ParameterCollection<Parameter>(AppEngine.Current.ParametersInfo, AppEngine.Current.CurrentContext.CultureCode);
            if (cameraDefinition.CameraType == "EoptisTurbidimeter") {
                acqParams.Add("PAR_LASER_GAIN", "");
                acqParams.Add("PAR_VIAL_GAIN", "");
                elaborationParams.Add("TH_WARNING_UPPER", "");
                elaborationParams.Add("TH_WARNING_LOWER", "");
                elaborationParams.Add("TH_ERROR_UPPER", "");
                elaborationParams.Add("TH_ERROR_LOWER", "");
                // machineParams.Add("PAR_FREE_RUN_TIMER", "");
            } else if (cameraDefinition.CameraType == "EoptisSpectrometer") {
                acqParams.Add("PAR_TINT_VIAL", "");
                acqParams.Add("PAR_TINT_BACKGROUND", "");
                acqParams.Add("INITIAL_WAVELENGTH", "");
                acqParams.Add("WAVELENGTH_INCREMENT", "");
                acqParams.Add("FINAL_WAVELENGTH", "");
                //acqParams.Add("FAKE_PARAM_BOOL", "");
                //acqParams.Add("FAKE_PARAM_DOUBLE", "");
                //acqParams.Add("FAKE_PARAM_STRING", "");
                //acqParams.Add("FAKE_PARAM_COMBO_STRING", "");
                elaborationParams.Add("PAR_ALL_RANGE_SUM_REJECT", "");
                elaborationParams.Add("PAR_RANGE_1_L", "");
                elaborationParams.Add("PAR_RANGE_1_H", "");
                elaborationParams.Add("PAR_RANGE_1_SIGN", "");
                elaborationParams.Add("PAR_RANGE_1_TH", "");
                elaborationParams.Add("PAR_RANGE_1_PEAK_REJECT_TH", "");
                elaborationParams.Add("PAR_RANGE_2_L", "");
                elaborationParams.Add("PAR_RANGE_2_H", "");
                elaborationParams.Add("PAR_RANGE_2_SIGN", "");
                elaborationParams.Add("PAR_RANGE_2_TH", "");
                elaborationParams.Add("PAR_RANGE_2_PEAK_REJECT_TH", "");
                elaborationParams.Add("PAR_RANGE_3_L", "");
                elaborationParams.Add("PAR_RANGE_3_H", "");
                elaborationParams.Add("PAR_RANGE_3_SIGN", "");
                elaborationParams.Add("PAR_RANGE_3_TH", "");
                elaborationParams.Add("PAR_RANGE_3_PEAK_REJECT_TH", "");
                elaborationParams.Add("PAR_RANGE_4_L", "");
                elaborationParams.Add("PAR_RANGE_4_H", "");
                elaborationParams.Add("PAR_RANGE_4_SIGN", "");
                elaborationParams.Add("PAR_RANGE_4_TH", "");
                elaborationParams.Add("PAR_RANGE_4_PEAK_REJECT_TH", "");
                elaborationParams.Add("PAR_FULL_SPECTRUM_REF_NORM", "");
            } else
                throw new Exception("Eoptis device not supported");
            elaborationParams.Add("LIGHT_CONTROL_TH", "");
            machineParams.Add("PAR_SPINDLE_COUNT", "");
            machineParams.Add("PAR_0_TORRETTA_OFFSET", "");
            machineParams.Add("PAR_INCREMENTO_SPINDLE_ID", "");
            machineParams.Add("INFO_FIRMWARE_VERSION", "");

            acqParams.PopulateParametersInfo();
            featEnParams.PopulateParametersInfo();
            elaborationParams.PopulateParametersInfo();
            recipeAdvancedParams.PopulateParametersInfo();
            roiParams.PopulateParametersInfo();
            machineParams.PopulateParametersInfo();
            strobeParams.PopulateParametersInfo();

        }

        public override CameraWorkingMode GetWorkingMode() {

            //EoptisWorkingMode temp = EoptisSvc.GetWorkingMode(IP4Address);
            //CurrWorkingMode = MapWorkingMode(temp);
            return CurrWorkingMode;
        }

        public override void SetWorkingMode(CameraWorkingMode mode) {

            return;
            //if (CurrWorkingMode != CameraWorkingMode.None) {
            //    Log.Line(LogLevels.Warning, "EoptisDeviceBase.SetWorkingMode", IP4Address + ": Cannot set different working mode while in this working mode: " + CurrWorkingMode.ToString());
            //    return;
            //}
            //switch (mode) {
            //    case CameraWorkingMode.Timed:
            //        EoptisSvc.SetWorkingMode(IP4Address, EoptisWorkingMode.WORKING_MODE_FREE_RUN);  //posso settare da fuori solo la freerun!
            //        break;
            //    //case CameraWorkingMode.ExternalSource:
            //    //    ewm = EoptisWorkingMode.WORKING_MODE_CONTROL;
            //    //    break;
            //    //case CameraWorkingMode.None:
            //    default:
            //        Log.Line(LogLevels.Warning, "EoptisDeviceBase.SetWorkingMode", IP4Address + ": Impossible to change working mode to: " + mode.ToString());
            //        break;
            //    //    ewm = EoptisWorkingMode.WORKING_MODE_IDLE;
            //    //    break;
            //}
            ////EoptisSvc.SetWorkingMode(IP4Address, ewm);
        }

        internal CameraWorkingMode MapWorkingMode(EoptisWorkingMode ewm) {

            CameraWorkingMode res = CameraWorkingMode.Idle;
            //Log.Line(LogLevels.Debug, "EoptisDeviceBase.MapWorkingMode", IP4Address + ": Working mode: " + ewm.ToString());
            switch (ewm) {
                case EoptisWorkingMode.WORKING_MODE_CONTROL:
                case EoptisWorkingMode.WORKING_MODE_MECHANICAL_TEST:
                    res = CameraWorkingMode.ExternalSource;
                    break;
                case EoptisWorkingMode.WORKING_MODE_FREE_RUN:
                    res = CameraWorkingMode.Timed;
                    break;
                default:
                    res = CameraWorkingMode.Idle;
                    break;
            }
            return res;
        }

        public override CameraClipMode GetClipMode() {
            //pier: TODO
            return CameraClipMode.None;
        }

        public override void SetClipMode(CameraClipMode clipMode) {
            //pier: TODO
        }

        public override string DownloadImages(string path, UserLevelEnum userLvl) {
            throw new NotImplementedException();
        }

        public override string UploadImages(string path, UserLevelEnum userLvl) {
            throw new NotImplementedException();
        }

        public override ParameterCollection<T> GetParameters<T>() {
            throw new NotImplementedException();
        }

        public override CameraInfo GetCameraInfo() {
            throw new NotImplementedException();
        }

        public override void SoftReset() {
            throw new NotImplementedException();
        }

        public override void ApplyParameters(CameraSetting settings, ParameterTypeEnum paramType, ExactaEasyEng.Cam dataSource) {
            throw new NotImplementedException();
        }

        public override void ApplyParameters(ParameterTypeEnum paramType, ExactaEasyEng.Cam dataSource) {
            throw new NotImplementedException();
        }

        public override string LoadFormat(int formatId) {
            throw new NotImplementedException();
        }

        public override void SetStopCondition(int headNumber, int condition, int timeout) {
            throw new NotImplementedException();
        }

        public override CameraNewStatus GetCameraStatus() {
            //pier: TODO
            return CameraNewStatus.Ready;
        }

        public override string GetFirmwareVersion() {
            throw new NotImplementedException();
        }

        public override CameraProcessingMode GetCameraProcessingMode() {
            throw new NotImplementedException();
        }

        public override void StartLearning() {
            throw new NotImplementedException();
        }

        public override void ImportParameters(string path) {
            throw new NotImplementedException();
        }

        public override void ExportParameters(string path) {
            throw new NotImplementedException();
        }

        public override void StopLearning(bool save) {
            throw new NotImplementedException();
        }

        public override void StartAnalysisOffline() {

            EoptisParameterCollection parameters = new EoptisParameterCollection();
            EoptisParameter par1 = new EoptisParameter(SYSTEM_ID.PAR_ACQUISITION_MODE, EoptisSvc.ParamsDictionary.Parameters[SYSTEM_ID.PAR_ACQUISITION_MODE].Type, EoptisSvc.ParamsDictionary.Parameters[SYSTEM_ID.PAR_ACQUISITION_MODE].ByteLength);
            par1.Value = 1.ToString();
            parameters.Add(par1);
            EoptisParameter par2 = new EoptisParameter(SYSTEM_ID.OUT_READY, EoptisSvc.ParamsDictionary.Parameters[SYSTEM_ID.OUT_READY].Type, EoptisSvc.ParamsDictionary.Parameters[SYSTEM_ID.OUT_READY].ByteLength);
            par2.Value = false.ToString();
            parameters.Add(par2);
            EoptisSvc.SetParameters(IP4Address, parameters);
        }

        public override void StopAnalysisOffline() {

            EoptisParameterCollection parameters = new EoptisParameterCollection();
            EoptisParameter par1 = new EoptisParameter(SYSTEM_ID.PAR_ACQUISITION_MODE, EoptisSvc.ParamsDictionary.Parameters[SYSTEM_ID.PAR_ACQUISITION_MODE].Type, EoptisSvc.ParamsDictionary.Parameters[SYSTEM_ID.PAR_ACQUISITION_MODE].ByteLength);
            par1.Value = 0.ToString();
            parameters.Add(par1);
            EoptisParameter par2 = new EoptisParameter(SYSTEM_ID.OUT_READY, EoptisSvc.ParamsDictionary.Parameters[SYSTEM_ID.OUT_READY].Type, EoptisSvc.ParamsDictionary.Parameters[SYSTEM_ID.OUT_READY].ByteLength);
            par2.Value = true.ToString();
            parameters.Add(par2);
            EoptisSvc.SetParameters(IP4Address, parameters);
        }

        private string typeLut(string paramId) {

            string res = "string";
            SYSTEM_ID id;
            if (Enum.TryParse(paramId, out id)) {
                switch (EoptisSvc.ParamsDictionary.Parameters[id].Type) {
                    case SYSTEM_TYPE.TYPEBOOL:
                        res = "bool";
                        break;
                    case SYSTEM_TYPE.TYPEFLOAT:
                    case SYSTEM_TYPE.TYPEDOUBLE:
                        res = "double";
                        break;
                    case SYSTEM_TYPE.TYPEINT:
                    case SYSTEM_TYPE.TYPEUNSIGNEDINT:
                    case SYSTEM_TYPE.TYPELONG:
                    case SYSTEM_TYPE.TYPEUNSIGNEDLONG:
                        res = "int";
                        break;
                    case SYSTEM_TYPE.TYPESTRING:
                    case SYSTEM_TYPE.TYPECHAR:
                    case SYSTEM_TYPE.TYPEUNSIGNEDCHAR:
                    case SYSTEM_TYPE.TYPEMEASURE:
                    case SYSTEM_TYPE.TYPESHORT:
                    case SYSTEM_TYPE.TYPESHORTBUFFER:
                    case SYSTEM_TYPE.TYPEUNSIGNEDSHORTBUFFER:
                        res = "string";
                        break;
                    case SYSTEM_TYPE.TYPEUNKNOWN:
                    default:
                        throw new Exception("Invalid parameter type");
                }
            }
            return res;
        }

        public override ParameterCollection<Parameter> GetAcquisitionParameters() {

            foreach (Parameter param in acqParams) {

                param.Group = "CAMERA;0";
                param.Decimals = 0;
                param.Increment = 1;
                param.IsVisible = 1;
                if (param.Id == "PAR_TINT_BACKGROUND")
                    param.IsVisible = 0;
                param.ParamId = "-1";
                //param.SectionId = "0";
                //param.SubSectionId = "0";
                param.ValueType = typeLut(param.Id);
                switch (param.Id) {
                    case "PAR_TINT_VIAL":
                    case "PAR_TINT_BACKGROUND":
                        param.MeasureUnit = "[ms]";
                        param.MinValue = "1";
                        param.MaxValue = "100";
                        break;
                    case "INITIAL_WAVELENGTH":
                    case "FINAL_WAVELENGTH":
                    case "WAVELENGTH_INCREMENT":
                        param.MeasureUnit = "[nm]";
                        param.MinValue = param.MaxValue = param.Value;
                        break;
                    case "PAR_LASER_GAIN":
                    case "PAR_VIAL_GAIN":
                        param.AdmittedValues = new List<string>(new string[] { "1", "10", "100" });
                        param.Increment = 0;
                        param.MeasureUnit = "[]";
                        param.MinValue = "1";
                        param.MaxValue = "100";
                        break;
                    default:
                        Log.Line(LogLevels.Warning, "EoptisDeviceBase.GetAcquisitionParameters", IP4Address + ": Unexpected parameter: " + param.Id);
                        break;
                }
                format_value(param);
            }
            return acqParams;
        }

        public override ParameterCollection<Parameter> GetDigitizerParameters() {

            return featEnParams;
        }

        public override ParameterCollection<Parameter> GetRecipeSimpleParameters() {

            foreach (Parameter param in elaborationParams) {

                param.Group = "TOOL PARAMS;0";
                param.Decimals = 0;
                param.Increment = 10;
                param.IsVisible = 1;
                if (param.Id == "PAR_FULL_SPECTRUM_REF_NORM")
                    param.IsVisible = 0;
                param.ParamId = "-1";
                param.MeasureUnit = "";
                //param.SectionId = "0";
                //param.SubSectionId = "0";
                param.ValueType = typeLut(param.Id);

                switch (param.Id) {
                    case "LIGHT_CONTROL_TH":
                    case "PAR_ALL_RANGE_SUM_REJECT":
                    case "PAR_RANGE_1_TH":
                    case "PAR_RANGE_2_TH":
                    case "PAR_RANGE_3_TH":
                    case "PAR_RANGE_4_TH":
                    case "PAR_RANGE_1_PEAK_REJECT_TH":
                    case "PAR_RANGE_2_PEAK_REJECT_TH":
                    case "PAR_RANGE_3_PEAK_REJECT_TH":
                    case "PAR_RANGE_4_PEAK_REJECT_TH":
                        param.MinValue = "0";
                        param.MaxValue = "65000";
                        param.Increment = 10;
                        break;
                    case "PAR_RANGE_1_SIGN":
                    case "PAR_RANGE_2_SIGN":
                    case "PAR_RANGE_3_SIGN":
                    case "PAR_RANGE_4_SIGN":
                        param.MinValue = "0";
                        param.MaxValue = "10";
                        param.Increment = 1;
                        break;
                    case "PAR_RANGE_1_L":
                    case "PAR_RANGE_1_H":
                    case "PAR_RANGE_2_L":
                    case "PAR_RANGE_2_H":
                    case "PAR_RANGE_3_L":
                    case "PAR_RANGE_3_H":
                    case "PAR_RANGE_4_L":
                    case "PAR_RANGE_4_H":
                        param.MeasureUnit = "[nm]";
                        param.MinValue = acqParams["INITIAL_WAVELENGTH"].Value;
                        param.MaxValue = acqParams["FINAL_WAVELENGTH"].Value;
                        param.Increment = 10;
                        break;
                    case "PAR_FULL_SPECTRUM_REF_NORM":
                        param.MinValue = param.MaxValue = "";
                        param.Increment = 0;
                        break;
                    case "TH_WARNING_UPPER":
                    case "TH_WARNING_LOWER":
                    case "TH_ERROR_UPPER":
                    case "TH_ERROR_LOWER":
                        param.MinValue = "0";
                        param.MaxValue = "100";
                        param.Increment = 1;
                        param.Decimals = 4;
                        break;
                    default:
                        Log.Line(LogLevels.Warning, "EoptisDeviceBase.GetRecipeSimpleParameters", IP4Address + ": Unexpected parameter: " + param.Id);
                        break;
                }
                format_value(param);
            }
            return elaborationParams;
        }

        public override ParameterCollection<Parameter> GetRecipeAdvancedParameters() {

            //if (CurrWorkingMode == CameraWorkingMode.ExternalSource) {
            //    Log.Line(LogLevels.Warning, "EoptisDeviceBase.GetRecipeAdvancedParameters", IP4Address + ": Cannot get acquisition parameters in this working mode: " + CurrWorkingMode.ToString());
            //}
            return recipeAdvancedParams;
        }

        public override ParameterCollection<Parameter> GetROIParameters(int idROI) {

            //if (CurrWorkingMode == CameraWorkingMode.ExternalSource) {
            //    Log.Line(LogLevels.Warning, "EoptisDeviceBase.GetROIParameters", IP4Address + ": Cannot get acquisition parameters in this working mode: " + CurrWorkingMode.ToString());
            //}
            return roiParams;
        }

        public override ParameterCollection<Parameter> GetMachineParameters() {

            //if (CurrWorkingMode == CameraWorkingMode.ExternalSource) {
            //    Log.Line(LogLevels.Warning, "EoptisDeviceBase.GetMachineParameters", IP4Address + ": Cannot get acquisition parameters in this working mode: " + CurrWorkingMode.ToString());
            //}
            //else {
            //    foreach (Parameter param in machineParams) {
            //        SYSTEM_ID id;
            //        if (Enum.TryParse(param.Id, out id)) {
            //            try {
            //                param.Value = EoptisSvc.GetParameter(IP4Address, new EoptisParameter(id));
            //                Log.Line(LogLevels.Pass, "EoptisDeviceBase.GetMachineParameters", IP4Address + ": " + param.Id + ": " + param.Value);
            //            }
            //            catch (Exception ex) {
            //                Log.Line(LogLevels.Error, "EoptisDeviceBase.GetMachineParameters", IP4Address + ": " + ex.Message);
            //            }
            //        }
            //    }
            //}

            //"PAR_SPINDLE_COUNT"
            //"PAR_0_TORRETTA_OFFSET"
            //"PAR_INCREMENTO_SPINDLE_ID"

            foreach (Parameter param in machineParams) {

                param.Group = "MACHINE;0";
                param.Decimals = 0;
                param.Increment = 1;
                param.IsVisible = 1;
                if (param.Id == "INFO_FIRMWARE_VERSION")
                    param.IsVisible = 0;
                param.ParamId = "-1";
                param.MeasureUnit = "";
                //param.SectionId = "0";
                //param.SubSectionId = "0";
                param.ValueType = typeLut(param.Id);
                param.MinValue = "0";
                param.MaxValue = "100";
                format_value(param);
            }
            return machineParams;
        }

        public override ParameterCollection<Parameter> GetStrobeParameters(CameraSetting camSettings) {

            //if (CurrWorkingMode == CameraWorkingMode.ExternalSource) {
            //    Log.Line(LogLevels.Warning, "EoptisDeviceBase.GetStrobeParameters", IP4Address + ": Cannot get acquisition parameters in this working mode: " + CurrWorkingMode.ToString());
            //}
            return strobeParams;
        }

        public override ParameterCollection<Parameter> GetStrobeParameters(int lightId) {

            //if (CurrWorkingMode == CameraWorkingMode.ExternalSource) {
            //    Log.Line(LogLevels.Warning, "EoptisDeviceBase.GetStrobeParameters", IP4Address + ": Cannot get acquisition parameters in this working mode: " + CurrWorkingMode.ToString());
            //}
            return strobeParams;
        }

        public override void SetAcquisitionParameters(ParameterCollection<Parameter> parameters) {

            setParameters(parameters);
        }

        public override void SetDigitizerParameters(ParameterCollection<Parameter> parameters) {

            setParameters(parameters);
        }

        public override void SetRecipeSimpleParameters(ParameterCollection<Parameter> parameters) {

            setParameters(parameters);
        }

        public override void SetRecipeAdvancedParameters(ParameterCollection<Parameter> parameters) {

            setParameters(parameters);
        }

        public override void SetROIParameters(ParameterCollection<Parameter> parameters, int idROI) {

            setParameters(parameters);
        }

        public override void SetMachineParameters(ParameterCollection<Parameter> parameters) {

            setParameters(parameters);
        }

        public override void SetStrobeParameters(CameraSetting camSettings, ParameterCollection<Parameter> parameters) {

            setParameters(parameters);
        }

        public override void SetStrobeParameters(int lightId, ParameterCollection<Parameter> parameters) {

            setParameters(parameters);
        }

        public void SetParameter(Parameter parameter) {

            setParameters(new ParameterCollection<Parameter> { parameter });
        }

        void setParameters(ParameterCollection<Parameter> parameters) {

            if (CurrWorkingMode == CameraWorkingMode.ExternalSource) {
                Log.Line(LogLevels.Warning, "EoptisDeviceBase.setParameters", IP4Address + ": Cannot set parameters in this working mode: " + CurrWorkingMode.ToString());
                return;
            }
            foreach (Parameter param in parameters) {
                SYSTEM_ID id;
                if (Enum.TryParse(param.Id, out id)) {
                    try {
                        EoptisSvc.SetParameter(IP4Address, new EoptisParameter(id), param.Value);
                    } catch (Exception ex) {
                        Log.Line(LogLevels.Error, "EoptisDeviceBase.setParameters", IP4Address + ": " + ex.Message);
                    }
                }
            }
        }

        void format_value(Parameter param)
        {
            if (string.IsNullOrEmpty(param.Value))
                return;
            if (param.ValueType == "decimal" ||
                param.ValueType == "int" ||
                param.ValueType == "double")
                param.Value = (Convert.ToDouble(param.Value, CultureInfo.InvariantCulture)).ToString("N" + param.Decimals, CultureInfo.InvariantCulture).Replace(",", "");
            else if (param.ValueType == "bool")
            {
                param.Value = (Convert.ToBoolean(param.Value, CultureInfo.InvariantCulture)).ToString(CultureInfo.InvariantCulture).Replace(",", "");
            }
            else
            {
                param.Value = param.Value.ToString();
            }
        }
    }
}
