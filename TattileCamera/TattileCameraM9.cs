using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SPAMI.Util.Logger;
using SPAMI.Util;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using DisplayManager;
using System.Collections.Concurrent;
using System.Diagnostics;
using ExactaEasyEng;
using System.IO;
using ExactaEasyCore;

namespace TattileCameras {

    public enum CameraStatusM9 {
        TATTILE_INITIALIZING = 0,
        TATTILE_RUNNING = 100,
        TATTILE_LIVE = 2200,
        TATTILE_SET_STOP_ON_CONDITION = 1000,
        TATTILE_GOING_TO_STOP_ON_CONDITION = 1001,
        TATTILE_STOP_ON_CONDITION = 1002,
        TATTILE_START_ANALYSIS = 1100,
        TATTILE_CAMERA_NOT_EXIST = -1,
        TATTILE_CAMERA_ERROR = -1000
    }

    public class TattileCameraM9 : TattileCameraBase {

        public TattileCameraM9()
            : base() {
        }

        public TattileCameraM9(CameraDefinition cameraDefinition, bool scanRequest)
            : base(cameraDefinition, scanRequest) {

            port = 20000;

            //per correttezza questa variabile andrebbe aggiornata con il valore del parametro da ricetta
            //ROI Enable - Modalità lavoro allo startup
            cameraStartUpClipMode = GetClipMode();

            if (cameraStartUpClipMode == CameraClipMode.Full)
                ROIModeAvailable = false;
            else
                ROIModeAvailable = true;

            cameraClipMode = cameraStartUpClipMode;
        }

        static TattileCameraM9() {
            //AssistantControl assistantCamUI = new TattileTools();

        }

        public override FeaturesEnableParameterCollection GetFeaturesEnableParameters() {
            return getParameters<TI_FeaturesEnableParticles, FeaturesEnableParameterCollection>();
        }

        public override FeaturesEnableParameterCollection GetFeaturesEnableParametersList() {
            TI_FeaturesEnableParticles par = new TI_FeaturesEnableParticles();
            return par.ToParamCollection(culture);
        }

        public override RecipeSimpleParameterCollection GetRecipeSimpleParameters() {
            return getParameters<TI_RecipeSimpleParticles, RecipeSimpleParameterCollection>();
        }

        public override RecipeSimpleParameterCollection GetRecipeSimpleParametersList() {
            TI_RecipeSimpleParticles par = new TI_RecipeSimpleParticles();
            return par.ToParamCollection(culture);
        }

        public override RecipeAdvancedParameterCollection GetRecipeAdvancedParameters() {
            return getParameters<TI_RecipeAdvancedParticles, RecipeAdvancedParameterCollection>();
        }

        public override RecipeAdvancedParameterCollection GetRecipeAdvancedParametersList() {
            TI_RecipeAdvancedParticles par = new TI_RecipeAdvancedParticles();
            return par.ToParamCollection(culture);
        }

        public override MachineParameterCollection GetMachineParameters() {
            return getParameters<TI_MachineParameters, MachineParameterCollection>();
        }

        public override MachineParameterCollection GetMachineParametersList() {
            TI_MachineParameters par = new TI_MachineParameters();
            return par.ToParamCollection(culture);
        }

        public override void SetStreamMode(int headNumber, CameraWorkingMode cwm) { }

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
                        CameraStatusM9 camStatusM12;
                        if (Enum.TryParse(tattileStatus.ToString(), out camStatusM12)) {
                            switch (camStatusM12) {
                                case CameraStatusM9.TATTILE_CAMERA_NOT_EXIST:
                                case CameraStatusM9.TATTILE_INITIALIZING:
                                case CameraStatusM9.TATTILE_CAMERA_ERROR:
                                    return CameraWorkingMode.None;
                                case CameraStatusM9.TATTILE_RUNNING:
                                case CameraStatusM9.TATTILE_SET_STOP_ON_CONDITION:
                                case CameraStatusM9.TATTILE_GOING_TO_STOP_ON_CONDITION:
                                case CameraStatusM9.TATTILE_STOP_ON_CONDITION:
                                case CameraStatusM9.TATTILE_START_ANALYSIS:
                                    return CameraWorkingMode.ExternalSource;
                                case CameraStatusM9.TATTILE_LIVE:
                                    return CameraWorkingMode.Timed;
                                default:
                                    Log.Line(LogLevels.Warning, "TattileCameraM9.MapWorkingMode", "CAM " + IP4Address + ": Working mode unknown");
                                    throw new CameraException("Working mode unknown");
                            }
                        }
                        Log.Line(LogLevels.Warning, "TattileCameraM9.MapWorkingMode", "CAM " + IP4Address + ": Working mode unknown");
                        throw new CameraException("Working mode unknown");
                }
            }
            Log.Line(LogLevels.Warning, "TattileCameraM9.MapWorkingMode", "CAM " + IP4Address + ": Working mode unknown");
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
                        CameraStatusM9 camStatusM9;
                        if (Enum.TryParse(tattileStatus.ToString(), out camStatusM9)) {
                            switch (camStatusM9) {
                                case CameraStatusM9.TATTILE_INITIALIZING:
                                case CameraStatusM9.TATTILE_CAMERA_NOT_EXIST:
                                case CameraStatusM9.TATTILE_CAMERA_ERROR:
                                    return CameraProcessingMode.None;
                                case CameraStatusM9.TATTILE_RUNNING:
                                    return CameraProcessingMode.Processing;
                                case CameraStatusM9.TATTILE_SET_STOP_ON_CONDITION:
                                case CameraStatusM9.TATTILE_GOING_TO_STOP_ON_CONDITION:
                                    return CameraProcessingMode.GoingToStopOnCondition;
                                case CameraStatusM9.TATTILE_STOP_ON_CONDITION:
                                    return CameraProcessingMode.StopOnCondition;
                                case CameraStatusM9.TATTILE_START_ANALYSIS:
                                    return CameraProcessingMode.OfflineAnalysis;
                                case CameraStatusM9.TATTILE_LIVE:
                                    return CameraProcessingMode.Acquiring;
                                default:
                                    Log.Line(LogLevels.Warning, "TattileCameraM9.MapProcessingMode", "CAM " + IP4Address + ": Processing mode unknown");
                                    throw new CameraException("Processing mode unknown");
                            }
                        }
                        Log.Line(LogLevels.Warning, "TattileCameraM9.MapProcessingMode", "CAM " + IP4Address + ": Processing mode unknown");
                        throw new CameraException("Processing mode unknown");
                }
            }
            Log.Line(LogLevels.Warning, "TattileCameraM9.MapProcessingMode", "CAM " + IP4Address + ": Processing mode unknown");
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
            CameraStatusM9 camStatusM9;
            if (Enum.TryParse(tattileStatus.ToString(), out camStatusM9)) {
                switch (camStatusM9) {
                    case CameraStatusM9.TATTILE_INITIALIZING:
                        return CameraNewStatus.Initializing;
                    case CameraStatusM9.TATTILE_LIVE:
                    case CameraStatusM9.TATTILE_RUNNING:
                    case CameraStatusM9.TATTILE_SET_STOP_ON_CONDITION:
                    case CameraStatusM9.TATTILE_GOING_TO_STOP_ON_CONDITION:
                    case CameraStatusM9.TATTILE_STOP_ON_CONDITION:
                    case CameraStatusM9.TATTILE_START_ANALYSIS:
                        return CameraNewStatus.Ready;
                    case CameraStatusM9.TATTILE_CAMERA_NOT_EXIST:
                        return CameraNewStatus.Unavailable;
                    case CameraStatusM9.TATTILE_CAMERA_ERROR:
                        return CameraNewStatus.Error;
                    default:
                        Log.Line(LogLevels.Warning, "TattileCameraM9.MapStatus", "CAM " + IP4Address + ": Status unknown");
                        throw new CameraException("Status unknown");
                }
            }
            Log.Line(LogLevels.Warning, "TattileCameraM9.MapStatus", "CAM " + IP4Address + ": Status unknown");
            throw new CameraException("Status unknown");
        }

        public override void ImportParameters(string path) {

            LoadVialAxis(path);
            _recursiveApplyFunc(() => { return TattileInterfaceSvc.TI_CommitChanges(IdTattile); });
        }

        public override void ExportParameters(string path) {

            //if (System.IO.File.Exists(path)) {
            //    //faccio backup vecchio file vial axis
            //    string folder = System.IO.Path.GetDirectoryName(path);
            //    System.IO.File.Move(path, folder + "/" + Utilities.DateTimeString() + "_backupVialAxis.xml");
            //}
            object[] _params = new object[] { IdTattile, new TI_AxisBuffer() };
            RecursiveApplyFunc(saveVialAxisTattile, ref _params, 3, 15);
            TI_AxisBuffer axisBuffer = (TI_AxisBuffer)_params[1];
            VialAxis vialAxis = new VialAxis();
            for (int iS = 0; iS < 63; iS++)   //pier: 63 è un numero magico...ATTENZIONE!!!
            {
                TI_VialAxisData data = axisBuffer.data[iS];
                vialAxis.VialAxisData.Add(data);
            }
            vialAxis.SaveXml(path);
        }

        public override void SoftReset() {

            _recursiveApplyFunc(() => { return TattileInterfaceSvc.TI_AssistantResetError(IdTattile); });
            base.SoftReset();
        }

        protected override void startRun() {

            _recursiveApplyFunc(() => { return TattileInterfaceSvc.TI_StartRun(IdTattile); });
        }

        unsafe protected override int _setParameters(object[] args) {

            ParameterTypeEnum paramType = (ParameterTypeEnum)args[0];
            IParameterCollection parameters = (IParameterCollection)args[1];
            int ret = -1;

            switch (paramType) {

                case ParameterTypeEnum.FeatureEnabled:
                    TI_FeaturesEnableParticles tattileFepParams = new TI_FeaturesEnableParticles(parameters);
                    _recursiveApplyFunc(() => {
                        TI_FeaturesEnableParticles _params = tattileFepParams;
                        return TattileInterfaceSvc.TI_FeaturesEnableSet(IdTattile, &_params);
                    });
                    break;
                case ParameterTypeEnum.RecipeSimple:
                    TI_RecipeSimpleParticles tattileRspParams = new TI_RecipeSimpleParticles(parameters);
                    _recursiveApplyFunc(() => {
                        TI_RecipeSimpleParticles _params = tattileRspParams;
                        return TattileInterfaceSvc.TI_RecipeSimpleSet(IdTattile, &_params);
                    });
                    break;
                case ParameterTypeEnum.RecipeAdvanced:
                    TI_RecipeAdvancedParticles tattileRapParams = new TI_RecipeAdvancedParticles(parameters, culture);
                    _recursiveApplyFunc(() => {
                        TI_RecipeAdvancedParticles _params = tattileRapParams;
                        return TattileInterfaceSvc.TI_RecipeAdvancedSet(IdTattile, &_params);
                    });
                    break;
                case ParameterTypeEnum.Machine:
                    TI_MachineParameters tattileMacParams = new TI_MachineParameters(parameters, culture);
                    _recursiveApplyFunc(() => {
                        TI_MachineParameters _params = tattileMacParams;
                        return TattileInterfaceSvc.TI_MachineParametersSet(IdTattile, &_params);
                    });
                    break;
                default:
                    base._setParameters(args);
                    break;
            }


            return ret;
        }

        protected override void preApplyParametersTasks() {

            _recursiveApplyFunc(() => { return TattileInterfaceSvc.TI_ResultImageDirectX(IdTattile, (int)TI_DirectxResultAction.DIRECTX_RESULT_IMAGE_STOP, port); });
        }

        protected override void postApplyParametersTasks() {

            _recursiveApplyFunc(() => { return TattileInterfaceSvc.TI_ResultImageDirectX(IdTattile, (int)TI_DirectxResultAction.DIRECTX_RESULT_IMAGE_START, port); });
            base.postApplyParametersTasks();
        }

        protected override bool calcRebootRequired<T>(T oldParams, T newParams) {
            if (typeof(T).Name=="TI_AcquisitionParameters")
                return calcRebootRequiredAcqParams((TI_AcquisitionParameters)(object)oldParams, (TI_AcquisitionParameters)(object)newParams);
            return false;
        }

        bool calcRebootRequiredAcqParams(TI_AcquisitionParameters oldParams, TI_AcquisitionParameters newParams) {

            if (((TI_AcquisitionParameters)oldParams).aoiEnable != ((TI_AcquisitionParameters)newParams).aoiEnable)
                return true;
            if (((TI_AcquisitionParameters)oldParams).aoiResX != ((TI_AcquisitionParameters)newParams).aoiResX)
                return true;
            if (((TI_AcquisitionParameters)oldParams).aoiResY != ((TI_AcquisitionParameters)newParams).aoiResY)
                return true;
            if (((TI_AcquisitionParameters)oldParams).aoiStartCol != ((TI_AcquisitionParameters)newParams).aoiStartCol)
                return true;
            if (((TI_AcquisitionParameters)oldParams).aoiStartRow != ((TI_AcquisitionParameters)newParams).aoiStartRow)
                return true;
            return false;
        }
    }
}
