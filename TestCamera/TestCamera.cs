using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DisplayManager;
using System.Drawing;
using System.Threading;
using ExactaEasyEng;
using ExactaEasyCore;
using System.IO;
using System.Diagnostics;
using SPAMI.Util.Logger;

namespace TestCamera {

    public class TestCamera : Camera {

        public event EventHandler RemoteDesktopDisconnected;

        Recipe camerasParams;

        CameraNewStatus currCameraStatus = CameraNewStatus.Unavailable;
        CameraWorkingMode currWorkingMode = CameraWorkingMode.None;
        CameraProcessingMode currProcessingMode = CameraProcessingMode.None;

        public TestCamera(CameraDefinition cameraDefinition, bool scanRequest)
            : base(cameraDefinition) {

            Enabled = true;

            System.Reflection.Assembly startAssembly = System.Reflection.Assembly.GetEntryAssembly();
            string fileName = "";
            if (startAssembly != null && startAssembly.ManifestModule.Name.ToLower() == "exactaeasy.exe")
                fileName = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\TestCameraParams.xml";
            else
                fileName = Environment.CurrentDirectory + @"\DotNet Components\ExactaEasy\TestCameraParams.xml"; // Specifico per IFIX

            camerasParams = Recipe.LoadFromFile(fileName);
        }

        public override void ApplyParameters(ParameterTypeEnum paramType, Cam dataSource) {

            if ((paramType & ParameterTypeEnum.Acquisition) != 0) {
                SetAcquisitionParameters(dataSource.AcquisitionParameters);
            }
            if ((paramType & ParameterTypeEnum.Strobo) != 0) {
                for (int li = 0; li < dataSource.Lights.Count; li++) {
                    LightController camLight = Lights.Find((LightController l) => { return l.Id == dataSource.Lights[li].Id; });
                    SetStrobeParameters(camLight.Id, dataSource.Lights[li].StroboParameters);
                    camLight.Strobe.ApplyParameters(camLight.StrobeChannel);
                }
            }
        }

        public override void ApplyParameters(CameraSetting settings, ParameterTypeEnum paramType, Cam dataSource) {

            if ((paramType & ParameterTypeEnum.Acquisition) != 0) {
                SetAcquisitionParameters(dataSource.AcquisitionParameters);
            }
        }

        public override AcquisitionParameterCollection GetAcquisitionParameters() {

            if (camerasParams == null)
                throw new NotImplementedException();

            AcquisitionParameterCollection newColl = new AcquisitionParameterCollection();
            foreach (Parameter par in camerasParams.Cams[IdCamera].AcquisitionParameters)
                newColl.Add((AcquisitionParameter)par);
            return newColl;
        }

        public override CameraInfo GetCameraInfo() {
            return new CameraInfo();
        }

        public override CameraNewStatus GetCameraStatus() {
            return currCameraStatus;
        }

        public override CameraProcessingMode GetCameraProcessingMode() {
            return currProcessingMode;
        }

        public override CameraClipMode GetClipMode() {
            return CameraClipMode.None;
        }

        public override FeaturesEnableParameterCollection GetFeaturesEnableParameters() {
            return null;
        }

        public override MachineParameterCollection GetMachineParameters() {
            return null;
        }

        public override ParameterCollection<T> GetParameters<T>() {
            return null;
        }

        public override ROIParameterCollection GetROIParameters(int idRoi) {
            return null;
        }

        public override RecipeAdvancedParameterCollection GetRecipeAdvancedParameters() {
            return null;
        }

        public override RecipeSimpleParameterCollection GetRecipeSimpleParameters() {

            if (camerasParams == null)
                throw new NotImplementedException();

            RecipeSimpleParameterCollection newColl = new RecipeSimpleParameterCollection();
            foreach (Parameter par in camerasParams.Cams[IdCamera].AcquisitionParameters)
                newColl.Add((RecipeSimpleParameter)par);
            return newColl;
        }

        public override StroboParameterCollection GetStrobeParameters(CameraSetting camSettings) {

            StroboParameterCollection coll = new StroboParameterCollection();
            if (Lights.Count > 0) {
                foreach (StroboParameter p in Lights[0].Strobe.GetStrobeParameter(0))
                    coll.Add(p.Id, p.Value);
            }
            return coll;
        }

        public override StroboParameterCollection GetStrobeParameters(int lightId) {

            StroboParameterCollection coll = new StroboParameterCollection();
            LightController camLight = Lights.Find((LightController l) => { return l.Id == lightId; });
            if (camLight != null) {
                foreach (StroboParameter p in camLight.Strobe.GetStrobeParameter(camLight.StrobeChannel))
                    coll.Add(p.Id, p.Value);
            }
            return coll;

        }

        public override CameraWorkingMode GetWorkingMode() {

            return currWorkingMode;
        }

        public override string LoadFormat(int formatId) {
            return null;
        }

        //public override int ROICount {
        //    get;
        //}

        public override void SetAcquisitionParameters(ParameterCollection<AcquisitionParameter> parameters) {

            int i = 0;
            while (i < 50) {
                Thread.Sleep(100);
                i++;
            }
            parameters["escapeCharacters"].Value = "Head_0.dxf";
            parameters["escapeCharacters"].Value = Convert.ToBase64String(Encoding.UTF8.GetBytes(parameters["escapeCharacters"].Value));
            camerasParams.Cams[IdCamera].AcquisitionParameters = parameters;
            camerasParams.SaveXml("TestCameraParams.xml");
            string decodedString = Encoding.UTF8.GetString(Convert.FromBase64String(parameters["escapeCharacters"].Value));
        }

        public override void SetClipMode(CameraClipMode clipMode) {

        }

        public override void SetFeaturesEnableParameters(ParameterCollection<FeaturesEnableParameter> parameters) {

        }

        public override void SetMachineParameters(ParameterCollection<MachineParameter> parameters) {

        }

        public override void SetROIParameters(ParameterCollection<ROIParameter> parameters, int idRoi) {

        }

        public override void SetRecipeAdvancedParameters(ParameterCollection<RecipeAdvancedParameter> parameters) {

        }

        public override void SetRecipeSimpleParameters(ParameterCollection<RecipeSimpleParameter> parameters) {

        }

        public override void SetStopCondition(int headNumber, int condition, int timeout) {

        }

        public override void SetStrobeParameters(CameraSetting camSettings, ParameterCollection<StroboParameter> parameters) {

        }

        public override void SetStrobeParameters(int lightId, ParameterCollection<StroboParameter> parameters) {

            
            LightController camLight = Lights.Find((LightController l) => { return l.Id == lightId; });
            if (camLight != null) {
                camLight.Strobe.SetStrobeParameter(camLight.StrobeChannel, parameters);                 
            }            
        }

        public override void SetWorkingMode(CameraWorkingMode mode) {

            currWorkingMode = mode;
            if (currWorkingMode == CameraWorkingMode.ExternalSource)
                startRun();
            else if (currWorkingMode == CameraWorkingMode.Timed)
                startLive();
        }

        void startRun() {

            currCameraStatus = CameraNewStatus.Ready;
            currProcessingMode = CameraProcessingMode.Processing;
            currWorkingMode = CameraWorkingMode.ExternalSource;
        }

        void startLive() {

            currCameraStatus = CameraNewStatus.Ready;
            currProcessingMode = CameraProcessingMode.Processing;
            currWorkingMode = CameraWorkingMode.Timed;
        }

        public override void SoftReset() {

        }

        public override void StartAnalysisOffline() {
        }

        public override void StartLearning() {
        }

        public override void StopAnalysisOffline() {

            //StopGrab();
        }
        public override void StopLearning(bool save) {
        }
        
        public override string DownloadImages(string path, UserLevelEnum userLvl) {

            return "";
        }

        public override void ExportParameters(string path) {

        }

        public override AcquisitionParameterCollection GetAcquisitionParametersList() {
            return null;
        }

        public override FeaturesEnableParameterCollection GetFeaturesEnableParametersList() {
            return null;
        }

        public override MachineParameterCollection GetMachineParametersList() {
            return null;
        }

        public override ROIParameterCollection GetROIParametersList() {
            return null;
        }

        public override RecipeAdvancedParameterCollection GetRecipeAdvancedParametersList() {
            return null;
        }

        public override RecipeSimpleParameterCollection GetRecipeSimpleParametersList() {
            return null;
        }

        public override StroboParameterCollection GetStrobeParametersList() {
            return null;
        }


        public override string GetFirmwareVersion() {
            throw new NotImplementedException();
        }

        public override void ImportParameters(string path) {
            throw new NotImplementedException();
        }

        public override string UploadImages(string path, UserLevelEnum userLvl) {
            throw new NotImplementedException();
        }
    }
}
