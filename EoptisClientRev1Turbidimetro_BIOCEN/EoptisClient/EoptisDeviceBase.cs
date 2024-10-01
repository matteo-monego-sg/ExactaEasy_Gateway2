using System;
using System.Collections.Generic;
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
        ParameterCollection<Parameter> recipeSimpleParams;
        ParameterCollection<Parameter> recipeAdvancedParams;
        ParameterCollection<Parameter> roiParams;
        ParameterCollection<Parameter> machineParams;
        ParameterCollection<Parameter> strobeParams;

        public EoptisDeviceBase(CameraDefinition cameraDefinition, bool scanRequest)
            : base(cameraDefinition) {

            acqParams = new ParameterCollection<Parameter>();
            acqParams.Add("PAR_LASER_GAIN", "");
            acqParams.Add("PAR_VIAL_GAIN", "");
            featEnParams = new ParameterCollection<Parameter>();
            recipeSimpleParams = new ParameterCollection<Parameter>();
            recipeSimpleParams.Add("TH_WARNING_UPPER", "");
            recipeSimpleParams.Add("TH_WARNING_LOWER", "");
            recipeSimpleParams.Add("TH_ERROR_UPPER", "");
            recipeSimpleParams.Add("TH_ERROR_LOWER", "");
            recipeAdvancedParams = new ParameterCollection<Parameter>();
            roiParams = new ParameterCollection<Parameter>();
            machineParams = new ParameterCollection<Parameter>();
            machineParams.Add("PAR_SPINDLE_COUNT", "");
            machineParams.Add("PAR_0_TORRETTA_OFFSET", "");
            machineParams.Add("PAR_INCREMENTO_SPINDLE_ID", "");
            machineParams.Add("PAR_FREE_RUN_TIMER", "");
            strobeParams = new ParameterCollection<Parameter>();
        }

        public override CameraWorkingMode GetWorkingMode() {

            //EoptisWorkingMode temp = EoptisSvc.GetWorkingMode(IP4Address);
            //CurrWorkingMode = MapWorkingMode(temp);
            return CurrWorkingMode;
        }

        public override void SetWorkingMode(CameraWorkingMode mode) {

            if (CurrWorkingMode != CameraWorkingMode.None) {
                Log.Line(LogLevels.Warning, "EoptisDeviceBase.SetWorkingMode", IP4Address + ": Cannot set different working mode while in this working mode: " + CurrWorkingMode.ToString());
                return;
            }
            switch (mode) {
                case CameraWorkingMode.Timed:
                    EoptisSvc.SetWorkingMode(IP4Address, EoptisWorkingMode.WORKING_MODE_FREE_RUN);  //posso settare da fuori solo la freerun!
                    break;
                //case CameraWorkingMode.ExternalSource:
                //    ewm = EoptisWorkingMode.WORKING_MODE_CONTROL;
                //    break;
                //case CameraWorkingMode.None:
                default:
                    Log.Line(LogLevels.Warning, "EoptisDeviceBase.SetWorkingMode", IP4Address + ": Impossible to change working mode to: " + mode.ToString());
                    break;
                //    ewm = EoptisWorkingMode.WORKING_MODE_IDLE;
                //    break;
            }
            //EoptisSvc.SetWorkingMode(IP4Address, ewm);
        }

        internal CameraWorkingMode MapWorkingMode(EoptisWorkingMode ewm) {

            CameraWorkingMode res = CameraWorkingMode.None;
            Log.Line(LogLevels.Debug, "EoptisDeviceBase.MapWorkingMode", IP4Address + ": Working mode: " + ewm.ToString());
            switch (ewm) {
                case EoptisWorkingMode.WORKING_MODE_CONTROL:
                case EoptisWorkingMode.WORKING_MODE_MECHANICAL_TEST:
                    res = CameraWorkingMode.ExternalSource;
                    break;
                case EoptisWorkingMode.WORKING_MODE_FREE_RUN:
                    res = CameraWorkingMode.Timed;
                    break;
                default:
                    res = CameraWorkingMode.None;
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
            throw new NotImplementedException();
        }

        public override void StopAnalysisOffline() {
            throw new NotImplementedException();
        }

        public override ParameterCollection<Parameter> GetAcquisitionParameters() {

            //if (CurrWorkingMode == CameraWorkingMode.ExternalSource) {
            //    Log.Line(LogLevels.Warning, "EoptisDeviceBase.GetAcquisitionParameters", IP4Address + ": Cannot get acquisition parameters in this working mode: " + CurrWorkingMode.ToString());
            //}
            //else {
            //    foreach (Parameter param in acqParams) {
            //        SYSTEM_ID id;
            //        if (Enum.TryParse(param.Id, out id)) {
            //            try {
            //                param.Value = EoptisSvc.GetParameter(IP4Address, new EoptisParameter(id));
            //                Log.Line(LogLevels.Pass, "EoptisDeviceBase.GetAcquisitionParameters", IP4Address + ": " + param.Id + ": " + param.Value);
            //            }
            //            catch (Exception ex) {
            //                Log.Line(LogLevels.Error, "EoptisDeviceBase.GetAcquisitionParameters", IP4Address + ": " + ex.Message);
            //            }
            //        }
            //    }
            //}
            return acqParams;
        }

        public override ParameterCollection<Parameter> GetFeaturesEnableParameters() {

            //if (CurrWorkingMode == CameraWorkingMode.ExternalSource) {
            //    Log.Line(LogLevels.Warning, "EoptisDeviceBase.GetFeaturesEnableParameters", IP4Address + ": Cannot get acquisition parameters in this working mode: " + CurrWorkingMode.ToString());
            //}
            return featEnParams;
        }

        public override ParameterCollection<Parameter> GetRecipeSimpleParameters() {

            //if (CurrWorkingMode == CameraWorkingMode.ExternalSource) {
            //    Log.Line(LogLevels.Warning, "EoptisDeviceBase.GetRecipeSimpleParameters", IP4Address + ": Cannot get acquisition parameters in this working mode: " + CurrWorkingMode.ToString());
            //}
            //else {
            //    foreach (Parameter param in recipeSimpleParams) {
            //        SYSTEM_ID id;
            //        if (Enum.TryParse(param.Id, out id)) {
            //            try {
            //                param.Value = EoptisSvc.GetParameter(IP4Address, new EoptisParameter(id));
            //                Log.Line(LogLevels.Pass, "EoptisDeviceBase.GetRecipeSimpleParameters", IP4Address + ": " + param.Id + ": " + param.Value);
            //            }
            //            catch (Exception ex) {
            //                Log.Line(LogLevels.Error, "EoptisDeviceBase.GetRecipeSimpleParameters", IP4Address + ": " + ex.Message);
            //            }
            //        }
            //    }
            //}
            return recipeSimpleParams;
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

        public override void SetFeaturesEnableParameters(ParameterCollection<Parameter> parameters) {

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
                    }
                    catch (Exception ex) {
                        Log.Line(LogLevels.Error, "EoptisDeviceBase.setParameters", IP4Address + ": " + ex.Message);
                    }
                }
            }
        }
    }
}
