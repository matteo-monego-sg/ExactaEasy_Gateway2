using Hvld.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExactaEasyEng;
using DisplayManager;
using System.IO;
using ExactaEasyCore;
using SPAMI.Util.Logger;
using System.Drawing;

namespace ExactaEasy {

    public class CameraViewerController {

        frmMain _mainForm;
        Panel _viewerPanel;
        MachineConfiguration _machineConfig;
        VisionSystemManager _visionSystemManager;
        ExtStatusStrip _statusStrip;
        ExtHeader _headerStrip;
        VertMenuBar _mainMenuBar;
        Dictionary<int, ICamViewer> camCtrlList = new Dictionary<int, ICamViewer>();
        int currCamCtrlIndex = -1;
        Recipe dataSource = null;
        public event EventHandler<NewFolderEventArgs> ChangedDownloadedImages;

        public int CurrentCameraIndex {
            get { return currCamCtrlIndex; }
        }

        public ICamViewer CurrentCamera {
            get { return currCamCtrlIndex >= 0 ? camCtrlList[currCamCtrlIndex] : null; }
        }

        public Recipe DataSource {
            get {
                return dataSource;
            }
            set {
                dataSource = value;
                initControl();
                for (int i = 0; i < _machineConfig.NumberOfCamera; i++) {
                    try {
                        createCamViewer(i);
                    }
                    catch (Exception ex) {
                        Log.Line(LogLevels.Warning, "CameraViewerController.DataSource.set", "Error device " + i + ": " + ex.Message);
                    }
                }
            }
        }

        public CameraViewerController(frmMain mainForm, VisionSystemManager visionSystemManager, MachineConfiguration machineConfig) {

            _mainForm = mainForm;
            _viewerPanel = _mainForm.pnlMain;
            _visionSystemManager = visionSystemManager;
            _machineConfig = machineConfig;
            _statusStrip = _mainForm.statusStrip;
            _headerStrip = _mainForm.headerStrip;
            _mainMenuBar = _mainForm.vertMenuBar1;
        }

        public void Destroy() {
            foreach (KeyValuePair<int, ICamViewer> camViewer in camCtrlList) {
                camViewer.Value.Destroy();
            }
        }

        public void BindToPrevCamera(bool show) {

            if (currCamCtrlIndex - 1 >= 0)
                BindToCamera(currCamCtrlIndex - 1, show);
        }

        public void BindToNextCamera(bool show) {

            if (currCamCtrlIndex + 1 < _visionSystemManager.Cameras.Count)
                BindToCamera(currCamCtrlIndex + 1, show);
        }

        public void BindToCamera(int cameraIndex, bool show) {

            ICamViewer currCamCtrl = null;
            if (currCamCtrlIndex >= 0) {
                currCamCtrl = camCtrlList[currCamCtrlIndex];
                //if (currCamCtrl.IsInLiveMode)
                //    throw new InvalidWhenCameraInLiveModeException();
                //currCamCtrl.StopRunningVisualization();
                ((UserControl)currCamCtrl).Hide();
            }
            ICamViewer bindCamCtrl = createCamViewer(cameraIndex);

            bindCamCtrl.DoRefresh();
            if (show) {
                ((UserControl)bindCamCtrl).Show();
                bindCamCtrl.StartRunningVisualization();
                ((UserControl)bindCamCtrl).BringToFront();
            }

            Camera c = (Camera)_visionSystemManager.Cameras[cameraIndex];
            int station = c.StationId;
            int node = c.NodeId;
            int head = c.Head;
            DisplayManager.Station s = (DisplayManager.Station)_visionSystemManager.Stations[station];
            DisplayManager.Node n = (DisplayManager.Node)_visionSystemManager.Nodes[node];
            //_visionSystemManager.Cameras[cameraIndex].SetStreamMode(false, CameraWorkingMode.ExternalSource);
            try {
                if (n == null /*|| !n.Initialized*/ || !n.Connected)
                    throw new Exception("Station not connected");
                _visionSystemManager.Stations[station].SetStreamMode(head, CameraWorkingMode.ExternalSource);
                _headerStrip.ErrorText = "";
            }
            catch (Exception ex) {
                //se camera non è abilitata da HMI  o da ricetta => non dare errore
                if (_visionSystemManager.Nodes[node].Stations[station].Enabled &&
                    AppEngine.Current.CurrentContext.IsCameraRecipeEnabled(cameraIndex)) {
                    _headerStrip.ErrorText = ex.Message;
                    Log.Line(LogLevels.Warning, "CameraViewerController.BindToCamera", "Camera ID {0} failed setting stream mode.", cameraIndex);
                }
                else
                    Log.Line(LogLevels.Debug, "CameraViewerController.BindToCamera", "Camera ID {0} not enabled by HMI or Recipe.", cameraIndex);
            }

            Display d = _visionSystemManager.Displays["Cam_" + cameraIndex.ToString()];
            if (d != null) {
                d.DoRender();
                if (show == true)
                    d.Resume();
            }
            //                foreach (Graph graph in _visionSystemManager.Graphs) {
            //    if (DataSource==null || DataSource.Nodes==null) 
            //        continue;
            //    foreach (NodeRecipe nr in DataSource.Nodes) {
            //        foreach (StationRecipe sr in nr.Stations) {
            //            foreach (Tool tool in sr.Tools) {
            //                foreach (ToolOutput to in tool.ToolOutputs) {
            //                    if (to.EnableGraph) {

            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            bindCamCtrl.DumpResultsChecked = _visionSystemManager.Stations[station].DumpResultsEnabled;
            currCamCtrlIndex = cameraIndex;
        }

        public void UnbindCurrentCamera() {

            Display d = _visionSystemManager.Displays["Cam_" + CurrentCameraIndex.ToString()];
            if (currCamCtrlIndex >= 0 && d != null) {
                d.Suspend();
                camCtrlList[currCamCtrlIndex].Hide();
                int nodeId = _visionSystemManager.Cameras[currCamCtrlIndex].NodeId;
                int stationId = _visionSystemManager.Cameras[currCamCtrlIndex].StationId;
                try {
                    _visionSystemManager.Nodes[nodeId].Stations[stationId].SetStreamMode(-1, CameraWorkingMode.ExternalSource);
                    _headerStrip.ErrorText = "";
                }
                catch (Exception ex) {
                    //se camera non è abilitata da HMI  o da ricetta => non dare errore
                    if (AppEngine.Current.CurrentContext.ActiveRecipe != null &&
                        AppEngine.Current.CurrentContext.ActiveRecipe.Cams != null) {
                        Cam cam = AppEngine.Current.CurrentContext.ActiveRecipe.Cams.Find(c => c.Id == currCamCtrlIndex);
                        if (cam != null) {
                            if (_visionSystemManager.Nodes[nodeId].Stations[stationId].Enabled &&
                                cam.Enabled) {
                                _headerStrip.ErrorText = ex.Message;
                                Log.Line(LogLevels.Warning, "CameraViewerController.BindToCamera", "Camera ID {0} failed setting stream mode.", cam.Id);
                            }
                            else
                                Log.Line(LogLevels.Debug, "CameraViewerController.BindToCamera", "Camera ID {0} not enabled by HMI or Recipe.", cam.Id);
                        }
                    }
                }
            }
            currCamCtrlIndex = -1;
        }

        /// <summary>
        /// Ricalcola lo stato degli elementi grafici del CamViewer corrente
        /// </summary>
        public void UpdateCurrentCamViewer() {

            if (currCamCtrlIndex > -1)
                camCtrlList[currCamCtrlIndex].DoRefresh();
        }

        public void RefreshStationStatus() {

            if (currCamCtrlIndex > -1)
                camCtrlList[currCamCtrlIndex].RefreshStationStatus();
        }

        public void SetParametersPage() {

            setParametersPage(-1);
        }

        void setParametersPage(int camViewerIndex) {

            if (camViewerIndex < 0)
                foreach (ICamViewer cv in camCtrlList.Values)
                    cv.SetParametersPage(ParameterTypeEnum.Acquisition);
        }

        public void EnableCurrentCamera() {

            if (currCamCtrlIndex >= 0)
                camCtrlList[currCamCtrlIndex].Enabled = true;
        }

        public void DisableCurrentCamera() {

            if (currCamCtrlIndex >= 0)
                camCtrlList[currCamCtrlIndex].Enabled = false;
        }

        void bindCamCtrl_CamViewerError(object sender, CamViewerErrorEventArgs e) {

            _headerStrip.ErrorText = e.ErrorText;
        }

        void newCamCtrl_CamViewerMessage(object sender, CamViewerMessageEventArgs e) {

            int cameraId = Convert.ToInt32(e.Parameter);
            int stationId = _visionSystemManager.Cameras[cameraId].StationId;
            int nodeId = _visionSystemManager.Cameras[cameraId].NodeId;
            switch (e.Message) {
                //case "SetDownloadedImagesSaveFolderRoot":
                //    //if (camCtrlList[currCamCtrlIndex].SaveFolderRoot == null ||
                //    //    !Directory.Exists(camCtrlList[currCamCtrlIndex].SaveFolderRoot)) {
                //    camCtrlList[currCamCtrlIndex].SaveFolderRoot = CreateSaveFolder();
                //    //}
                //    break;
                case "DownloadedImagesFolder":
                    if (e.Parameter != null) {
                        OnChangedDownloadedImages(this, new NewFolderEventArgs(e.Parameter));
                        _statusStrip.StatusMessage = frmBase.UIStrings.GetString("ImagesDownloaded") + "!";
                    }
                    else
                        _statusStrip.StatusMessage = frmBase.UIStrings.GetString("ImagesNotDownloaded") + "!";
                    break;
                case "CameraResetBegin":
                    _headerStrip.ErrorText = frmBase.UIStrings.GetString("CameraResetting");
                    Application.DoEvents();
                    break;
                case "CameraResetEnd":
                    _headerStrip.ErrorText = "";
                    Application.DoEvents();
                    break;
                case "RemoteDesktopConnect":
                    try {
                        _visionSystemManager.Nodes[nodeId].SendUserLevelClass(AppEngine.Current.CurrentContext.UserLevel);
                        _visionSystemManager.Nodes[nodeId].SendInspectionViewId(stationId);
                        if (_mainForm.InvokeRequired && _mainForm.IsHandleCreated)
                            _mainForm.Invoke(new Action(() => { _mainForm.TopMost = false; }));
                        else
                            _mainForm.TopMost = false;
                        _visionSystemManager.Nodes[nodeId].RemoteDesktopConnect(!AppEngine.Current.MachineConfiguration.HideRemoteDesktopToolbar);
                    }
                    catch (Exception ex) {
                        _headerStrip.ErrorText = frmBase.UIStrings.GetString("RemoteDesktopError") + " " + ex.Message;
                        _mainForm.setSupervisorOnTop();
                    }
                    break;
                case "EnableSavingResultsOnFile":
                    saveResultsOnFile(nodeId, stationId, true);
                    break;
                case "DisableSavingResultsOnFile":
                    saveResultsOnFile(nodeId, stationId, false);
                    break;
                case "SuspendLayout":
                    DrawingControl.SuspendDrawing(_mainForm.GetMainPanel());
                    break;
                case "ResumeLayout":
                    DrawingControl.ResumeDrawing(_mainForm.GetMainPanel());
                    //UpdateCurrentCamViewer();
                    break;
                default:
                    _statusStrip.StatusMessage = e.Message;
                    break;
            }
        }

        private void saveResultsOnFile(int nodeId, int stationId, bool save) {
            if (save &&
                _visionSystemManager.Nodes[nodeId].Stations[stationId].HasMeasures)
                _visionSystemManager.Nodes[nodeId].Stations[stationId].DumpResultsEnabled = true;
            else
                _visionSystemManager.Nodes[nodeId].Stations[stationId].DumpResultsEnabled = false;

        }

        public bool CameraUsed(int cameraIndex) {

            bool retval = true;
            //CamViewer currCamCtrl = null;
            //if (currCamCtrlIndex >= 0) {
            //    try {
            //        int nodeId = _visionSystemManager.Cameras[cameraIndex].NodeId;
            //        int stationId = _visionSystemManager.Cameras[cameraIndex].StationId;
            //        if (_visionSystemManager.Nodes[nodeId].Stations[stationId].Enabled &&
            //            AppEngine.Current.CurrentContext.IsCameraRecipeEnabled(cameraIndex)) {
            //            CameraNewStatus cameraStatus = _visionSystemManager.Cameras[cameraIndex].GetCameraStatus();
            //            CameraWorkingMode cameraWorkingMode = _visionSystemManager.Cameras[cameraIndex].GetWorkingMode();
            //            CameraProcessingMode cameraProcessingMode = _visionSystemManager.Cameras[cameraIndex].GetCameraProcessingMode();
            //            //if (!(cameraStatus == DisplayManager.CameraStatus.RUNNING) &&
            //            //    !(cameraStatus == DisplayManager.CameraStatus.CAMERA_NOT_EXIST) &&
            //            //    !(cameraStatus == DisplayManager.CameraStatus.ERROR) &&
            //            //    !(cameraStatus == DisplayManager.CameraStatus.STOP_ON_CONDITION))
            //            if ((cameraProcessingMode != CameraProcessingMode.Processing) &&
            //                (cameraStatus != CameraNewStatus.Unavailable) &&
            //                (cameraStatus != CameraNewStatus.Error) &&
            //                (cameraProcessingMode != CameraProcessingMode.StopOnCondition) &&
            //                (cameraProcessingMode != CameraProcessingMode.OfflineAnalysis))
            //                retval = false;
            //        }
            //    }
            //    catch {
            //        //retval = false;
            //    }
            //}
            return retval;
        }

        public void ReloadParameters(int cameraIndex, ParameterTypeEnum parType) {
            ICamViewer reloadCamCtrl = createCamViewer(cameraIndex);
            reloadCamCtrl.ReloadParameters(parType);
        }

        //public void EditParameterFromCamera(int cameraIndex, List<ParameterTypeEnum> parametersToChange)
        //{
        //    CamViewer editRecipeCamCtrl = createCamViewer(cameraIndex);
        //    editRecipeCamCtrl.EditParameterFromCamera(parametersToChange);
        //}

        ICamViewer createCamViewer(int cameraIndex) {
            //Cam cam = null;
            object viewerDS = null;

            int stationId = _visionSystemManager.Cameras[cameraIndex].StationId;
            int nodeId = _visionSystemManager.Cameras[cameraIndex].NodeId;
            //Cam cam = null;
            if (DataSource != null) {
                if (DataSource.Nodes != null && DataSource.Nodes.Count >= 0) {
                    if (_visionSystemManager.Cameras[cameraIndex] != null) {
                        ICamera c = _visionSystemManager.Cameras[cameraIndex];
                        //IStation s = _visionSystemManager.Stations[c.StationId];
                        NodeRecipe nodeRecipe = DataSource.Nodes.Find(nr => nr.Id == c.NodeId);
                        if (nodeRecipe != null)
                            viewerDS = DataSource.Nodes[c.NodeId];
                    }
                    else
                        throw new Exception("Camera index not valid Exception");
                }
                else {
                    viewerDS = DataSource.Cams.Find(c => c.Id == cameraIndex);
                    if (viewerDS == null)
                        throw new Exception("Camera recipe not exists");
                }
            }

            ICamViewer newCamCtrl = null;
            if (!camCtrlList.ContainsKey(cameraIndex)) {
                //newCamCtrl = new CamViewer(_configuration.CameraSettings[cameraIndex]);   
                if (_visionSystemManager.Cameras[cameraIndex] == null)
                    throw new Exception("Camera not exists");

                //foreach (Control c in _viewerPanel.Controls) {
                //    if (c is ICamViewer && (c as ICamViewer).Settings.Id == cameraIndex) {
                //        _viewerPanel.Controls.Remove(c);
                //    }
                //}
                Display d = _visionSystemManager.Displays["Cam_" + cameraIndex.ToString()];
                Graph g = _visionSystemManager.Graphs["Cam_" + cameraIndex];
                switch (((Camera)_visionSystemManager.Cameras[cameraIndex]).Visualizer) {
                    case "Gretel":
                        newCamCtrl = new CamViewerGretel(_visionSystemManager, AppEngine.Current.MachineConfiguration, cameraIndex);
                        newCamCtrl.Description = _visionSystemManager.Nodes[nodeId].Description + " - " + _visionSystemManager.Nodes[nodeId].Stations[stationId].Description + " - " + ((Camera)_visionSystemManager.Cameras[cameraIndex]).CameraDescription;
                        if (d != null) {
                            _visionSystemManager.Displays.Remove(d);
                            d.Dispose();
                        }
                        _visionSystemManager.Displays.Add(new ThumbScreenDisplay("Cam_" + cameraIndex.ToString(),
                            newCamCtrl.GetLivePictureBox(),
                            (IStation)_visionSystemManager.Nodes[nodeId].Stations[stationId],
                            newCamCtrl.GetThumbPanel(),
                            192,
                            (_machineConfig.VisualizerNumberThumbnails >16) ? _machineConfig.VisualizerNumberThumbnails : 16,
                            true,
                            _machineConfig.VisualizerImageQuality,
                            _machineConfig.VisualizerShowColoredBorder,
                            _machineConfig.VisualizerNumberThumbnails));
                        break;
                    // MM-11/01/2024: HVLD2.0 evo.
                    // MM-21/02/2024: been requested to add a visualization of the OptrelSignalStackerControl shown in the ScreenHvldDisplay on
                    // the main page of ExactaEasy, therefore we create Hvld2Graph too.
                    case "Hvld":
                        newCamCtrl = new HvldViewer(_visionSystemManager, AppEngine.Current.MachineConfiguration, cameraIndex);
                        // Sets up the description/title for the new control. 
                        newCamCtrl.Description = _visionSystemManager.Nodes[nodeId].Description +
                            " - " + _visionSystemManager.Nodes[nodeId].Stations[stationId].Description +
                            " - " + ((Camera)_visionSystemManager.Cameras[cameraIndex]).CameraDescription;
                        // Checks if a display with the same camera index already exists.
                        if (d != null)
                        {
                            _visionSystemManager.Displays.Remove(d);
                            d.Dispose();
                        }
                        // Adds a Display for HVLD2.0 on the vsmanager.
                        _visionSystemManager.Displays.Add(
                            new ScreenHvldDisplay("Cam_" + cameraIndex.ToString(),
                            newCamCtrl.GetSignalStacker(), _visionSystemManager.Nodes[nodeId].Stations[stationId]));
                        // Checks if the graph already exists.
                        if (g != null)
                        {
                            _visionSystemManager.Graphs.Remove(g);
                            g.Dispose();
                        }
                        Hvld2Graph hg = new Hvld2Graph("Cam_" + cameraIndex,
                            newCamCtrl.GetSignalStacker(),
                            _visionSystemManager.Nodes[nodeId].Stations[stationId]);
                        _visionSystemManager.Graphs.Add(hg);
                        break;
                    case "Data":
                        newCamCtrl = new DataViewer(_visionSystemManager, AppEngine.Current.MachineConfiguration, cameraIndex);
                        newCamCtrl.Description = _visionSystemManager.Nodes[nodeId].Description + " - " + _visionSystemManager.Nodes[nodeId].Stations[stationId].Description + " - " + ((Camera)_visionSystemManager.Cameras[cameraIndex]).CameraDescription;
                        if (g != null) {
                            _visionSystemManager.Graphs.Remove(g);
                            g.Dispose();
                        }
                        LineGraph lg = new LineGraph("Cam_" + cameraIndex,
                            newCamCtrl.GetGraphBox()[0],
                            (IStation)_visionSystemManager.Nodes[nodeId].Stations[stationId]);
                        if (DataSource == null || DataSource.Nodes == null) {
                            lg.SetTitles("", "WAITING FOR A VALID RECIPE", "", "", 0, _machineConfig.NumberOfSpindles + 1, 0, 10);
                        }
                        else {
                            NodeRecipe nr = DataSource.Nodes.Find(nn => nn.Id == nodeId);
                            CameraRecipe cr = DataSource.Nodes.Find(nn => nn.Id == nodeId).Stations.First().Cameras.First();
                            newCamCtrl.CreateGraphs(lg, nr);
                        }
                        _visionSystemManager.Graphs.Add(lg);
                        break;
                    case "Spectrometer":
                        newCamCtrl = new SpectrometerViewer(_visionSystemManager, AppEngine.Current.MachineConfiguration, cameraIndex);
                        newCamCtrl.Description = _visionSystemManager.Nodes[nodeId].Description + " - " + _visionSystemManager.Nodes[nodeId].Stations[stationId].Description + " - " + ((Camera)_visionSystemManager.Cameras[cameraIndex]).CameraDescription;
                        if (g != null) {
                            _visionSystemManager.Graphs.Remove(g);
                            g.Dispose();
                        }
                        SpectrometerGraphs sg;
                        if (DataSource == null || DataSource.Nodes == null) {
                            sg = new SpectrometerGraphs("Cam_" + cameraIndex, newCamCtrl.GetGraphBox(), 0, 1, (IStation)_visionSystemManager.Nodes[nodeId].Stations[stationId]);
                            sg.SetTitles("Spectra", "WAITING FOR A VALID RECIPE", "", "", -1, -1, 0, 65000);            //parametrizzare
                            sg.SetTitles("Elaboration", "WAITING FOR A VALID RECIPE", "", "", -1, -1, -10000, 10000);   //parametrizzare
                        }
                        else {
                            NodeRecipe nr = DataSource.Nodes.Find(nn => nn.Id == nodeId);
                            CameraRecipe cr = DataSource.Nodes.Find(nn => nn.Id == nodeId).Stations.First().Cameras.First();
                            int initialWavelength = 0, finalWavelength = 0;
                            if (cr.AcquisitionParameters != null && cr.AcquisitionParameters.Count > 0) {
                                Parameter initialWavelengthParam = cr.AcquisitionParameters.Find(pp => pp.Id == "INITIAL_WAVELENGTH");
                                Parameter finalWavelengthParam = cr.AcquisitionParameters.Find(pp => pp.Id == "FINAL_WAVELENGTH");
                                if (initialWavelengthParam != null && finalWavelengthParam != null) {
                                    if (int.TryParse(initialWavelengthParam.Value, out initialWavelength) == false || int.TryParse(finalWavelengthParam.Value, out finalWavelength) == false) {
                                        Log.Line(LogLevels.Error, "SpectrometerViewer.CreateGraph", "Error while casting wavelength. Check parameter in recipe!");
                                    }
                                }
                                else {
                                    Log.Line(LogLevels.Error, "SpectrometerViewer.CreateGraph", "Error while casting wavelength. Add parameter to recipe!");
                                }
                            }
                            sg = new SpectrometerGraphs("Cam_" + cameraIndex, newCamCtrl.GetGraphBox(), initialWavelength, finalWavelength, (IStation)_visionSystemManager.Nodes[nodeId].Stations[stationId]);
                            newCamCtrl.CreateGraphs(sg, nr);
                        }
                        _visionSystemManager.Graphs.Add(sg);
                        break;
                    default:
                        newCamCtrl = new CamViewer(_visionSystemManager, AppEngine.Current.MachineConfiguration, cameraIndex);
                        newCamCtrl.Description = ((Camera)_visionSystemManager.Cameras[cameraIndex]).CameraDescription;
                        if (d != null)
                            _visionSystemManager.Displays.Remove(d);
                        _visionSystemManager.Displays.Add(new SingleDisplay("Cam_" + cameraIndex.ToString(),
                            newCamCtrl.GetLivePictureBox(),
                            (IStation)_visionSystemManager.Nodes[nodeId].Stations[stationId],
                            _machineConfig.CameraSettings.Find(cs => cs.Id == cameraIndex),
                            true,
                            _machineConfig.VisualizerImageQuality,
                            true));
                        break;
                }
                ((UserControl)newCamCtrl).Dock = DockStyle.Fill;
                //newCamCtrl.DataSource = cam;
                newCamCtrl.SetDataSource(viewerDS);
                newCamCtrl.CamViewerError += new EventHandler<CamViewerErrorEventArgs>(bindCamCtrl_CamViewerError);
                newCamCtrl.CamViewerMessage += new EventHandler<CamViewerMessageEventArgs>(newCamCtrl_CamViewerMessage);
                newCamCtrl.ParameterEditingStateChanged += new EventHandler(newCamCtrl_ParameterEditingStateChanged);
                newCamCtrl.ParametersUploaded += new EventHandler<CamViewerMessageEventArgs>(newCamCtrl_ParametersUploaded);
                camCtrlList.Add(cameraIndex, newCamCtrl);
                _viewerPanel.Controls.Add(((UserControl)newCamCtrl));
            }
            else {
                newCamCtrl = camCtrlList[cameraIndex];
                newCamCtrl.SetDataSource(viewerDS);
            }

            return newCamCtrl;
        }

        void newCamCtrl_ParameterEditingStateChanged(object sender, EventArgs e) {

            ICamViewer curr = (ICamViewer)sender;
            if (curr.ParameterEditingState == ParameterEditingStates.Editing) {
                _mainMenuBar.SetNavigationAvailable(false);
                _mainMenuBar.Enabled = false;
            }
            else {
                _mainMenuBar.SetNavigationAvailable(true);
                _mainMenuBar.Enabled = true;
            }           
        }

        void newCamCtrl_ParametersUploaded(object sender, CamViewerMessageEventArgs e) {

            (sender as ICamViewer).ParametersUploaded -= newCamCtrl_ParametersUploaded;
            DrawingControl.ResumeDrawing(_mainForm.GetMainPanel());
            //UpdateCurrentCamViewer();
            if (e.Message == "SUCCESSFULL") {
                camCtrlList[currCamCtrlIndex].UpdateLogCtrl("PARAMETERS RETRIEVED SUCCESSFULLY", Color.Green);
            }
            else {
                camCtrlList[currCamCtrlIndex].UpdateLogCtrl("PARAMETERS UPLOAD FAILED", Color.Red);
            }
        }

        void initControl() {

            int lastCamCtrlIndex = currCamCtrlIndex;
            foreach (ICamViewer cv in camCtrlList.Values) {
                cv.CamViewerError -= bindCamCtrl_CamViewerError;
                cv.CamViewerMessage -= newCamCtrl_CamViewerMessage;
                cv.ParameterEditingStateChanged -= newCamCtrl_ParameterEditingStateChanged;
                cv.Destroy();
                _viewerPanel.Invoke(new MethodInvoker(() => { _viewerPanel.Controls.Remove((UserControl)cv);  }));
                //_viewerPanel.Controls.Remove((UserControl)cv);
            }
            //_viewerPanel.Controls.Clear();
            camCtrlList.Clear();
            currCamCtrlIndex = -1;
            if (DataSource != null && lastCamCtrlIndex >= 0) {
                BindToCamera(lastCamCtrlIndex, false);
            }
        }

        protected virtual void OnChangedDownloadedImages(object sender, NewFolderEventArgs e) {

            if (ChangedDownloadedImages != null) ChangedDownloadedImages(sender, e);
        }

        public bool CheckForUnappliedParameter() {

            bool ris = false;
            foreach (ICamViewer cw in camCtrlList.Values) {
                if (cw.ParameterEditingState == ParameterEditingStates.Editing) {
                    ris = true;
                    break;
                }
            }
            return ris;
        }

        public void ConfirmSavedParameters() {

            foreach (ICamViewer cw in camCtrlList.Values)
                cw.ConfirmSavedParameters();
        }
    }

    //public class InvalidWhenCameraInLiveModeException : Exception {
    //}

    public class NewFolderEventArgs : EventArgs {

        public string Path { get; private set; }

        public NewFolderEventArgs(string path) {

            Path = path;
        }
    }
}
