using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DisplayManager;
using ExactaEasyEng;
using ExactaEasyCore;
using System.Threading;
using SPAMI.Util.Logger;

namespace TattileCameras {

    public enum CameraStatusM12 {

        //COSMETIC_RUNNING = 100,			//Normal operation work
        //COSMETIC_MODE_NORMAL_WORK = 1000,			//Normal operation work
        //COSMETIC_MODE_LIVE = 3000,					//Live status
        TATTILE_INITIALIZING = 0,
        TATTILE_RUNNING = 100,
        TATTILE_LIVE = 2200,
        TATTILE_SET_STOP_ON_CONDITION = 1000,
        TATTILE_GOING_TO_STOP_ON_CONDITION = 1001,
        TATTILE_STOP_ON_CONDITION = 1002,
        TATTILE_START_ANALYSIS = 1100,
        TATTILE_LEARNING_BOTTLE = 4000,
        TATTILE_LEARNING_BOTTLE_BUSY = 4001,
        TATTILE_LEARNING_BOTTLE_TEST = 4002,
        TATTILE_LEARNING_BOTTLE_TOP = 5000,
        TATTILE_LEARNING_BOTTLE_TOP_BUSY = 5001,
        TATTILE_LEARNING_BOTTLE_TOP_TEST = 5002,
        TATTILE_TEST_IO = 10000,
        TATTILE_SAVING = 11000,
        TATTILE_CAMERA_NOT_EXIST = -1,
        TATTILE_CAMERA_ERROR = -1000
    }

    public enum StreamModeM12 {

        Disabled = -1,
        LiveHead0 = 0,
        LiveHead1 = 1,
        LiveHead2 = 2,
        LiveHead3 = 3,
        LiveAll = 4,
        ResultHead0 = 5,
        ResultHead1 = 6,
        ResultHead2 = 7,
        ResultHead3 = 8,
        ResultAll = 9
    }

    public class TattileCameraM12 : TattileCameraBase {

        public TattileCameraM12()
            : base() {
        }

        public TattileCameraM12(CameraDefinition cameraDefinition, bool scanRequest)
            : base(cameraDefinition, scanRequest) {
            port = 12345;
            cameraStartUpClipMode = cameraClipMode = CameraClipMode.Full;
            try {
                startRun();     //workaround altrimenti non va in run all'avvio
            }
            catch {
            }

        }

        public override FeaturesEnableParameterCollection GetFeaturesEnableParameters() {
            return getParameters<TI_FeaturesEnableCosmetic, FeaturesEnableParameterCollection>();
        }

        public override FeaturesEnableParameterCollection GetFeaturesEnableParametersList() {
            TI_FeaturesEnableCosmetic par = new TI_FeaturesEnableCosmetic();
            return par.ToParamCollection(culture);
        }

        public override RecipeSimpleParameterCollection GetRecipeSimpleParameters() {
            return getParameters<TI_RecipeSimpleCosmetic, RecipeSimpleParameterCollection>();
        }

        public override RecipeSimpleParameterCollection GetRecipeSimpleParametersList() {
            TI_RecipeSimpleCosmetic par = new TI_RecipeSimpleCosmetic();
            return par.ToParamCollection(culture);
        }

        public override RecipeAdvancedParameterCollection GetRecipeAdvancedParameters() {
            return getParameters<TI_RecipeAdvancedCosmetic, RecipeAdvancedParameterCollection>();
        }

        public override RecipeAdvancedParameterCollection GetRecipeAdvancedParametersList() {
            TI_RecipeAdvancedCosmetic par = new TI_RecipeAdvancedCosmetic();
            return par.ToParamCollection(culture);
        }

        public override MachineParameterCollection GetMachineParameters() {
            return getParameters<TI_MachineParametersCosmetic, MachineParameterCollection>();
        }

        public override MachineParameterCollection GetMachineParametersList() {
            TI_MachineParametersCosmetic par = new TI_MachineParametersCosmetic();
            return par.ToParamCollection(culture);
        }

        protected override CameraWorkingMode MapWorkingMode(int tattileStatus, int tattileProgramStatus) {
            CameraProgramStatus camPrgStatus;
            if (Enum.TryParse(tattileProgramStatus.ToString(), out camPrgStatus)) {
                switch (camPrgStatus) {
                    case CameraProgramStatus.TATTILE_PROGRAM_ERROR:
                    case CameraProgramStatus.TATTILE_PROGRAM_STOP:
                    case CameraProgramStatus.TATTILE_PROGRAM_STOP_AND_GRABBING:
                        return CameraWorkingMode.None;
                    case CameraProgramStatus.TATTILE_PROGRAM_RUN:
                    default:
                        CameraStatusM12 camStatusM12;
                        if (Enum.TryParse(tattileStatus.ToString(), out camStatusM12)) {
                            switch (camStatusM12) {
                                case CameraStatusM12.TATTILE_CAMERA_NOT_EXIST:
                                case CameraStatusM12.TATTILE_INITIALIZING:
                                case CameraStatusM12.TATTILE_CAMERA_ERROR:
                                    return CameraWorkingMode.None;
                                case CameraStatusM12.TATTILE_RUNNING:
                                case CameraStatusM12.TATTILE_SET_STOP_ON_CONDITION:
                                case CameraStatusM12.TATTILE_GOING_TO_STOP_ON_CONDITION:
                                case CameraStatusM12.TATTILE_STOP_ON_CONDITION:
                                case CameraStatusM12.TATTILE_START_ANALYSIS:
                                case CameraStatusM12.TATTILE_LEARNING_BOTTLE:
                                case CameraStatusM12.TATTILE_LEARNING_BOTTLE_BUSY:
                                case CameraStatusM12.TATTILE_LEARNING_BOTTLE_TEST:
                                case CameraStatusM12.TATTILE_LEARNING_BOTTLE_TOP:
                                case CameraStatusM12.TATTILE_LEARNING_BOTTLE_TOP_BUSY:
                                case CameraStatusM12.TATTILE_LEARNING_BOTTLE_TOP_TEST:
                                case CameraStatusM12.TATTILE_TEST_IO:
                                case CameraStatusM12.TATTILE_SAVING:
                                    return CameraWorkingMode.ExternalSource;
                                case CameraStatusM12.TATTILE_LIVE:
                                    return CameraWorkingMode.Timed;
                                default:
                                    Log.Line(LogLevels.Warning, "TattileCameraM12.MapWorkingMode", "CAM " + IP4Address + ": Working mode unknown");
                                    throw new CameraException("Working mode unknown");
                            }
                        }
                        Log.Line(LogLevels.Warning, "TattileCameraM12.MapWorkingMode", "CAM " + IP4Address + ": Working mode unknown");
                        throw new CameraException("Working mode unknown");
                }
            }
            Log.Line(LogLevels.Warning, "TattileCameraM12.MapWorkingMode", "CAM " + IP4Address + ": Working mode unknown");
            throw new CameraException("Working mode unknown");
        }

        protected override CameraProcessingMode MapProcessingMode(int tattileStatus, int tattileProgramStatus) {
            CameraProgramStatus camPrgStatus;
            if (Enum.TryParse(tattileProgramStatus.ToString(), out camPrgStatus)) {
                switch (camPrgStatus) {
                    case CameraProgramStatus.TATTILE_PROGRAM_ERROR:
                    case CameraProgramStatus.TATTILE_PROGRAM_STOP:
                    case CameraProgramStatus.TATTILE_PROGRAM_STOP_AND_GRABBING:
                        return CameraProcessingMode.None;
                    case CameraProgramStatus.TATTILE_PROGRAM_RUN:
                    default:
                        CameraStatusM12 camStatusM12;
                        if (Enum.TryParse(tattileStatus.ToString(), out camStatusM12)) {
                            switch (camStatusM12) {
                                case CameraStatusM12.TATTILE_INITIALIZING:
                                case CameraStatusM12.TATTILE_CAMERA_NOT_EXIST:
                                case CameraStatusM12.TATTILE_CAMERA_ERROR:
                                    return CameraProcessingMode.None;
                                case CameraStatusM12.TATTILE_RUNNING:
                                    return CameraProcessingMode.Processing;
                                case CameraStatusM12.TATTILE_SET_STOP_ON_CONDITION:
                                case CameraStatusM12.TATTILE_GOING_TO_STOP_ON_CONDITION:
                                    return CameraProcessingMode.GoingToStopOnCondition;
                                case CameraStatusM12.TATTILE_STOP_ON_CONDITION:
                                    return CameraProcessingMode.StopOnCondition;
                                case CameraStatusM12.TATTILE_START_ANALYSIS:
                                    return CameraProcessingMode.OfflineAnalysis;
                                case CameraStatusM12.TATTILE_LEARNING_BOTTLE:
                                case CameraStatusM12.TATTILE_LEARNING_BOTTLE_BUSY:
                                case CameraStatusM12.TATTILE_LEARNING_BOTTLE_TEST:
                                case CameraStatusM12.TATTILE_LEARNING_BOTTLE_TOP:
                                case CameraStatusM12.TATTILE_LEARNING_BOTTLE_TOP_BUSY:
                                case CameraStatusM12.TATTILE_LEARNING_BOTTLE_TOP_TEST:
                                case CameraStatusM12.TATTILE_TEST_IO:
                                case CameraStatusM12.TATTILE_SAVING:
                                    return CameraProcessingMode.Learning;
                                case CameraStatusM12.TATTILE_LIVE:
                                    return CameraProcessingMode.Acquiring;
                                default:
                                    Log.Line(LogLevels.Warning, "TattileCameraM12.MapProcessingMode", "CAM " + IP4Address + ": Processing mode unknown");
                                    throw new CameraException("Processing mode unknown");
                            }
                        }
                        Log.Line(LogLevels.Warning, "TattileCameraM12.MapProcessingMode", "CAM " + IP4Address + ": Processing mode unknown");
                        throw new CameraException("Processing mode unknown");
                }
            }
            Log.Line(LogLevels.Warning, "TattileCameraM12.MapProcessingMode", "CAM " + IP4Address + ": Processing mode unknown");
            throw new CameraException("Processing mode unknown");
        }

        protected override CameraNewStatus MapStatus(int tattileStatus, int tattileProgramStatus) {
            CameraProgramStatus camPrgStatus;
            if (Enum.TryParse(tattileProgramStatus.ToString(), out camPrgStatus)) {
                switch (camPrgStatus) {
                    case CameraProgramStatus.TATTILE_PROGRAM_ERROR:
                        return CameraNewStatus.Unavailable;
                    case CameraProgramStatus.TATTILE_PROGRAM_STOP:
                    case CameraProgramStatus.TATTILE_PROGRAM_STOP_AND_GRABBING:
                        return CameraNewStatus.Stop;
                    case CameraProgramStatus.TATTILE_PROGRAM_RUN:
                        break;  //tutto ok
                }
            }
            CameraStatusM12 camStatusM12;
            if (Enum.TryParse(tattileStatus.ToString(), out camStatusM12)) {
                switch (camStatusM12) {
                    case CameraStatusM12.TATTILE_INITIALIZING:
                        return CameraNewStatus.Initializing;
                    case CameraStatusM12.TATTILE_RUNNING:
                    case CameraStatusM12.TATTILE_LIVE:
                    case CameraStatusM12.TATTILE_SET_STOP_ON_CONDITION:
                    case CameraStatusM12.TATTILE_GOING_TO_STOP_ON_CONDITION:
                    case CameraStatusM12.TATTILE_STOP_ON_CONDITION:
                    case CameraStatusM12.TATTILE_START_ANALYSIS:
                    case CameraStatusM12.TATTILE_LEARNING_BOTTLE:
                    case CameraStatusM12.TATTILE_LEARNING_BOTTLE_BUSY:
                    case CameraStatusM12.TATTILE_LEARNING_BOTTLE_TEST:
                    case CameraStatusM12.TATTILE_LEARNING_BOTTLE_TOP:
                    case CameraStatusM12.TATTILE_LEARNING_BOTTLE_TOP_BUSY:
                    case CameraStatusM12.TATTILE_LEARNING_BOTTLE_TOP_TEST:
                    case CameraStatusM12.TATTILE_TEST_IO:
                    case CameraStatusM12.TATTILE_SAVING:
                        return CameraNewStatus.Ready;
                    case CameraStatusM12.TATTILE_CAMERA_NOT_EXIST:
                        return CameraNewStatus.Unavailable;
                    case CameraStatusM12.TATTILE_CAMERA_ERROR:
                        return CameraNewStatus.Error;
                    default:
                        Log.Line(LogLevels.Warning, "TattileCameraM12.MapStatus", "CAM " + IP4Address + ": Status unknown");
                        throw new CameraException("Status unknown");
                }
            }
            Log.Line(LogLevels.Warning, "TattileCameraM12.MapStatus", "CAM " + IP4Address + ": Status unknown");
            throw new CameraException("Status unknown");
        }

        protected override void startRun() {

            stopLive();
            SetStreamMode(-1, CameraWorkingMode.ExternalSource);
        }

        public override CameraClipMode GetClipMode() {


            return cameraClipMode;
        }

        public override void SetClipMode(CameraClipMode clipMode) {


            cameraClipMode = CameraClipMode.Full;
        }

        public override void SetStreamMode(int headNumber, CameraWorkingMode cwm) {

            StreamModeM12 sm12 = StreamModeM12.ResultAll;
            if (cwm == CameraWorkingMode.ExternalSource) {
                if (headNumber == 0) sm12 = StreamModeM12.ResultHead0;
                else if (headNumber == 1) sm12 = StreamModeM12.ResultHead1;
                else if (headNumber == 2) sm12 = StreamModeM12.ResultHead2;
                else if (headNumber == 3) sm12 = StreamModeM12.ResultHead3;
                else sm12 = StreamModeM12.ResultAll;
            }
            if (cwm == CameraWorkingMode.Timed) {
                if (headNumber >= 0 && Head == 0) sm12 = StreamModeM12.LiveHead0;
                else if (headNumber == 1) sm12 = StreamModeM12.LiveHead1;
                else if (headNumber == 2) sm12 = StreamModeM12.LiveHead2;
                else if (headNumber == 3) sm12 = StreamModeM12.LiveHead3;
                else sm12 = StreamModeM12.LiveAll;
            }
            object[] _params = new object[] { IdTattile, (int)sm12 };
            RecursiveApplyFunc(setStreamMode, ref _params, 3, 15);
        }

        protected int setStreamMode(params object[] args) {
            int iSm = (int)args[1];
            return TattileInterfaceSvc.TI_StreamModeSet(IdTattile, iSm);
        }

        protected int setLiveOverlay(params object[] args) {
            int roiEnable = (int)args[1];
            int infoEnable = (int)args[2];
            return TattileInterfaceSvc.TI_CosmeticLiveOverlay(IdTattile, roiEnable, infoEnable);
        }

        public override void ExportParameters(string path) {
            //TODO...
        }

        protected override int reloadModelTattile(params object[] args) {
            return TattileInterfaceSvc.TI_CosmeticLoadModel(IdTattile);
        }

        public override void ApplyParameters(ParameterTypeEnum paramType, Cam dataSource) {

            //patch per tornare in live dopo aver modificato i parametri
            Exception exx = null;
            //CameraWorkingMode oldWorkMode = GetWorkingMode();
            try {
                base.ApplyParameters(paramType, dataSource);
            }
            catch (Exception ex) {
                exx = new Exception("Error sending parameters", ex);
            }
            //if (oldWorkMode == CameraWorkingMode.Timed) {
            //    cameraWorkingMode = CameraWorkingMode.ExternalSource;
            //    //base.SetClipMode(CameraClipMode.None);
            //    base.SetWorkingMode(oldWorkMode);
            //}
            if (exx != null)
                throw exx;
        }

        public override void ApplyParameters(CameraSetting settings, ParameterTypeEnum paramType, Cam dataSource) {

            //patch per tornare in live dopo aver modificato i parametri
            Exception exx = null;
            //CameraWorkingMode oldWorkMode = GetWorkingMode();
            try {
                base.ApplyParameters(settings, paramType, dataSource);
            }
            catch (Exception ex) {
                exx = new Exception("Error sending parameters", ex);
            }
            //if (oldWorkMode == CameraWorkingMode.Timed) {
            //    cameraWorkingMode = CameraWorkingMode.ExternalSource;
            //    base.SetClipMode(CameraClipMode.None);
            //    //base.SetClipMode(CameraClipMode.None);
            //    base.SetWorkingMode(oldWorkMode);
            //}
            if (exx != null)
                throw exx;
        }

        protected override unsafe int _setParameters(object[] args) {

            ParameterTypeEnum paramType = (ParameterTypeEnum)args[0];
            IParameterCollection parameters = (IParameterCollection)args[1];
            int ret = -1;
            switch (paramType) {
                case ParameterTypeEnum.FeatureEnabled:
                    TI_FeaturesEnableCosmetic tattileFepParams = new TI_FeaturesEnableCosmetic(parameters);
                    _recursiveApplyFunc(() => {
                        TI_FeaturesEnableCosmetic _params = tattileFepParams;
                        return TattileInterfaceSvc.TI_FeaturesEnableSet(IdTattile, &_params);
                    }, 3, 15);
                    break;
                case ParameterTypeEnum.RecipeSimple:
                    TI_RecipeSimpleCosmetic tattileRspParams = new TI_RecipeSimpleCosmetic(parameters);
                    _recursiveApplyFunc(() => {
                        TI_RecipeSimpleCosmetic _params = tattileRspParams;
                        return TattileInterfaceSvc.TI_RecipeSimpleSet(IdTattile, &_params);
                    });
                    break;
                case ParameterTypeEnum.RecipeAdvanced:
                    TI_RecipeAdvancedCosmetic tattileRacParams = new TI_RecipeAdvancedCosmetic(parameters);
                    _recursiveApplyFunc(() => {
                        TI_RecipeAdvancedCosmetic _params = tattileRacParams;
                        return TattileInterfaceSvc.TI_RecipeAdvancedSet(IdTattile, &_params);
                    });
                    break;
                case ParameterTypeEnum.Machine:
                    TI_MachineParametersCosmetic tattileMpcParams = new TI_MachineParametersCosmetic(parameters, culture);
                    _recursiveApplyFunc(() => {
                        TI_MachineParametersCosmetic _params = tattileMpcParams;
                        return TattileInterfaceSvc.TI_MachineParametersSet(IdTattile, &_params);
                    });
                    break;
                default:
                    ret = base._setParameters(args);
                    break;
            }
            return ret;
        }

        protected override void postApplyParametersTasks() {

            SetWorkingMode(CameraWorkingMode.Timed);
            Thread.Sleep(200);
            SetWorkingMode(CameraWorkingMode.ExternalSource);
        }
    }
}
