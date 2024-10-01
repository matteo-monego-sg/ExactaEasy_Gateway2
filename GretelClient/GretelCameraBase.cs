using System;
using DisplayManager;
using ExactaEasyCore;
using ExactaEasyEng;

namespace GretelClients {
    public class GretelCameraBase : Camera/*, IStation*/ {

        internal int IdGretel { get; set; }
        internal int BufferSize { get; private set; }

        internal ParameterCollection<Parameter> acquisitionParams;
        internal ParameterCollection<Parameter> digitizerParams;

        public GretelCameraBase(CameraDefinition cameraDefinition, bool scanRequest)
            : base(cameraDefinition) {

            acquisitionParams = new ParameterCollection<Parameter>();
            digitizerParams = new ParameterCollection<Parameter>();

            //ROIModeAvailable = false;
            BufferSize = cameraDefinition.BufferSize;
            //StationId = cameraDefinition.Station;    //per Gretel ogni camera è una stazione (di default)
            if (GetCameraStatus() != CameraNewStatus.Ready)
                throw new CameraException("Camera not ready!");
            //IdStation = StationId;
            //Cameras = new CameraCollection();
            //Cameras.Add(this);
            //Initialized = true;
        }

        //Camera
        public override void ApplyParameters(CameraSetting settings, ParameterTypeEnum paramType, Cam dataSource) {
            throw new NotImplementedException();
        }

        public override string DownloadImages(string path, UserLevelEnum userLevel) {
            throw new NotImplementedException();
        }

        public override void ExportParameters(string path) {
            throw new NotImplementedException();
        }

        public override ParameterCollection<Parameter> GetAcquisitionParameters() {

            return acquisitionParams;
        }

        //public override AssistantControl GetAssistant() {
        //    return null;
        //}

        public override CameraInfo GetCameraInfo() {
            throw new NotImplementedException();
        }

        public override sealed CameraNewStatus GetCameraStatus() {
            //pier: TODO
            return CameraNewStatus.Ready;
        }

        public override CameraClipMode GetClipMode() {
            //pier: TODO
            return CameraClipMode.None;
        }

        public override ParameterCollection<Parameter> GetDigitizerParameters() {
		
            return digitizerParams;
        }

        public override ParameterCollection<Parameter> GetMachineParameters() {
            throw new NotImplementedException();
        }

        public override ParameterCollection<T> GetParameters<T>() {
            throw new NotImplementedException();
        }

        public override ParameterCollection<Parameter> GetROIParameters(int idRoi) {
            throw new NotImplementedException();
        }

        public override ParameterCollection<Parameter> GetRecipeAdvancedParameters() {
            throw new NotImplementedException();
        }

        public override ParameterCollection<Parameter> GetRecipeSimpleParameters() {
            throw new NotImplementedException();
        }

        public override ParameterCollection<Parameter> GetStrobeParameters(CameraSetting camSettings) {
            throw new NotImplementedException();
        }

        public override CameraWorkingMode GetWorkingMode() {
            //pier: TODO
            return CameraWorkingMode.Idle;
        }

        //public override void HardReset() {
        //    throw new NotImplementedException();
        //}

        public override string LoadFormat(int formatId) {
            throw new NotImplementedException();
        }

        public override void SetAcquisitionParameters(ParameterCollection<Parameter> parameters) {
            throw new NotImplementedException();
        }

        public override void SetClipMode(CameraClipMode clipMode) {
            //pier: TODO
        }

        public override void SetDigitizerParameters(ParameterCollection<Parameter> parameters) {
            throw new NotImplementedException();
        }

        public override void SetMachineParameters(ParameterCollection<Parameter> parameters) {
            throw new NotImplementedException();
        }

        public override void SetROIParameters(ParameterCollection<Parameter> parameters, int idRoi) {
            throw new NotImplementedException();
        }

        public override void SetRecipeAdvancedParameters(ParameterCollection<Parameter> parameters) {
            throw new NotImplementedException();
        }

        public override void SetRecipeSimpleParameters(ParameterCollection<Parameter> parameters) {
            throw new NotImplementedException();
        }

        public override void SetStopCondition(int headNumber, int condition, int timeout) {
            throw new NotImplementedException();
        }

        //public override void SetStrobeParameters(CameraSetting camSettings, StroboParameterCollection parameters) {
        //    throw new NotImplementedException();
        //}

        public override void SetWorkingMode(CameraWorkingMode mode) {
            //pier: TODO
        }

        public override void SoftReset() {
            throw new NotImplementedException();
        }

        public override void StartAnalysisOffline() {
            throw new NotImplementedException();
        }

        public override void StartLearning() {
            throw new NotImplementedException();
        }

        public override void StopLearning(bool save) {
            throw new NotImplementedException();
        }

        public override void ApplyParameters(ParameterTypeEnum paramType, Cam dataSource) {
            throw new NotImplementedException();
        }

        public override CameraProcessingMode GetCameraProcessingMode() {
            throw new NotImplementedException();
        }

        public override ParameterCollection<Parameter> GetStrobeParameters(int lightId) {
            throw new NotImplementedException();
        }

        public override void SetStrobeParameters(int lightId, ParameterCollection<Parameter> parameters) {
            throw new NotImplementedException();
        }

        public override void SetStrobeParameters(CameraSetting camSettings, ParameterCollection<Parameter> parameters) {
            throw new NotImplementedException();
        }

        public override void StopAnalysisOffline() {
            throw new NotImplementedException();
        }

        public override string UploadImages(string path, UserLevelEnum userLvl) {
            throw new NotImplementedException();
        }

        public override string GetFirmwareVersion() {
            throw new NotImplementedException();
        }

        public override void ImportParameters(string path) {
            throw new NotImplementedException();
        }
    }
}
