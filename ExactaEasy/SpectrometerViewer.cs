using DisplayManager;
using Emgu.CV.UI;
using ExactaEasy.DAL;
using ExactaEasyCore;
using ExactaEasyEng;
using ExactaEasyEng.Utilities;
using Hvld.Controls;
using SPAMI.Util.Logger;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using ZedGraph;

namespace ExactaEasy
{

    public partial class SpectrometerViewer : UserControl, ICamViewer {

        public event EventHandler<CamViewerErrorEventArgs> CamViewerError;
        public event EventHandler<CamViewerMessageEventArgs> CamViewerMessage;
        public event EventHandler ParameterEditingStateChanged;
        public event EventHandler<CamViewerMessageEventArgs> ParametersUploaded;

        string description;
        public string Description {
            get {
                return description;
            }
            set {
                description = value;
                lblCameraDescription.Text = description;
            }
        }

        public bool DumpResultsChecked { get; set; }
        public string SaveFolderRoot { get; set; }

        CameraRecipe dataSource;
        //CameraRecipe originalDataSource;
        NodeRecipe nodeDataSource;
        //NodeRecipe originalNodeDataSource;

        ParameterTypeEnum currentParameterType = ParameterTypeEnum.Elaboration;
        ParameterCollection<Parameter> actualAcquisitionParams;
        //ParameterCollection<Parameter> actualDigitizerParams;
        ParameterCollection<Parameter> actualElaborationParams;
        //ParameterCollection<Parameter> actualRecipeAdvancedParams;
        //ParameterCollection<Parameter> actualROIParams;
        ParameterCollection<Parameter> actualMachineParams;
        //ParameterCollection<Parameter> actualStroboParams;
        Dictionary<int, string> roiPageLabel;
        VisionSystemManager _visionSystemMgr;
        MachineConfiguration _machineConfig;
        GridViewInspectionDisplay gviDisplay;
        ZedGraphControl[] extGraphs = new ZedGraphControl[(int)SpectrometerGraphs.SpectrometerChartType.ChartsCount];

        int roiIndex = 0;
        int lightIndex = 0;
        //int nextRoiIndex = 0;
        //bool isCameraOK = true;
        int stationId = -1;
        //int head = -1;
        int nodeId = -1;
        //int camViewerDebuggerOwner;

        Dictionary<ParameterTypeEnum, string> paramTitleLabel = new Dictionary<ParameterTypeEnum, string>() { 
            { ParameterTypeEnum.Acquisition, frmBase.UIStrings.GetString("AcquisitionParameter") },
            { ParameterTypeEnum.Digitizer, frmBase.UIStrings.GetString("Features") },
            { ParameterTypeEnum.Elaboration, frmBase.UIStrings.GetString("RecipeParameter") },
            { ParameterTypeEnum.RecipeAdvanced, frmBase.UIStrings.GetString("AdvancedRecipeParameter") },
            { ParameterTypeEnum.ROI,frmBase.UIStrings.GetString("ROIDefinition") },
            { ParameterTypeEnum.Machine ,frmBase.UIStrings.GetString("MachineParameter") },
            { ParameterTypeEnum.Strobo,frmBase.UIStrings.GetString("StroboParameter") }
        };

        //bool lastBtnApplyEnableValue = false;

        ParameterEditingStates _prevParameterEditingState;
        ParameterEditingStates _parameterEditingState;
        public ParameterEditingStates ParameterEditingState {
            get { return _parameterEditingState; }
            private set {
                _prevParameterEditingState = _parameterEditingState;
                _parameterEditingState = value;
                OnParameterEditingStatusChanged(this, new EventArgs());
            }
        }

        public CameraSetting Settings { get; internal set; }

        CameraRecipe DataSource {
            get {
                return dataSource;
            }
            set {
                //Debug.WriteLine("DataSource AcquisitionParametersCount: " + value.AcquisitionParameters.Count);
                dataSource = value;
                //getROIPageLabel();
                //originalDataSource = dataSource.Clone(AppEngine.Current.ParametersInfo, AppEngine.Current.CurrentContext.CultureCode);
                currentParameterType = ParameterTypeEnum.Elaboration;
                loadParameters();
                //setCameraStatus();
                recalcInfo();
                //pcbLive.ImageAspectRatio = calcImageRatio();
            }
        }

        NodeRecipe NodeDataSource {

            get {
                return nodeDataSource;
            }
            set {
                nodeDataSource = value;
                DataSource = NodeDataSource.Stations.First().Cameras.First();
            }
        }

        Camera _camera;
        public Camera Camera {
            get {
                return _camera;
            }
        }

        IStation _station;
        public IStation Station {
            get {
                return _station;
            }
        }

        INode _node;
        public INode Node {
            get {
                return _node;
            }
        }

        //public bool IsInLiveMode {
        //    get { return (pcbLive.IsConnected && pcbLive.IsLiveConnection); }
        //}

        enum StatusEvents {
            Kill = 0,
            Poll,
        }

        //WaitHandle[] statusThEvts;
        //ManualResetEvent killEv;
        //ManualResetEvent pollEv;
        //Thread statusTh;

        SpectrometerViewer() {
            InitializeComponent();

            extGraphs[0] = zgcSpectra;
            extGraphs[1] = zgcElaboration;

            btnApply.Text = frmBase.UIStrings.GetString("BtnApply");
            btnEdit.Text = frmBase.UIStrings.GetString("BtnEdit");
            btnSave.Text = frmBase.UIStrings.GetString("BtnSave");
            btnRestore.Text = frmBase.UIStrings.GetString("BtnUndo");
            cbViewResults.Text = frmBase.UIStrings.GetString("Results");

            ParameterEditingState = ParameterEditingStates.Reading;
            pgCameraParams.BeginEdit += new DataGridViewCellCancelEventHandler(pgCameraParams_BeginEdit);
            pgCameraParams.EndEdit += new DataGridViewCellCancelEventHandler(pgCameraParams_EndEdit);
            yesNoPanel.YesNoAnswer += yesNoPanel_YesNoAnswer;
            saveMenu.SaveMenuCondition += saveMenu_SaveMenuCondition;
            loadSaveMenu.MenuAction += loadSaveMenu_MenuAction;

            zgcSpectra.ContextMenuBuilder += zgcSpectra_ContextMenuBuilder;
            zgcElaboration.ContextMenuBuilder += zgcElaboration_ContextMenuBuilder;

            cbViewResults.Checked = false;
        }

        public SpectrometerViewer(VisionSystemManager visionSystemMgr, MachineConfiguration machineConfig, int cameraIndex)
            : this() {

            _visionSystemMgr = visionSystemMgr;
            _machineConfig = machineConfig;
            Settings = machineConfig.CameraSettings[cameraIndex];
            _camera = (Camera)_visionSystemMgr.Cameras[Settings.Id];
            _station = _visionSystemMgr.Nodes[_camera.NodeId].Stations[_camera.StationId];
            _node = _visionSystemMgr.Nodes[_camera.NodeId];
            stationId = _camera.StationId;
            nodeId = _camera.NodeId;
            lblCameraDescription.Text = _camera.CameraDescription;
            //AppContext ctx = AppEngine.Current.CurrentContext;
            RefreshStationStatus();
            resGrid.DataGrid.SuspendLayout();
            resGrid.DataGrid.ColumnHeadersBorderStyle = properColumnHeadersBorderStyle;
            resGrid.DataGrid.EnableHeadersVisualStyles = false;
            resGrid.DataGrid.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            resGrid.DataGrid.BackgroundColor = Color.LightGray;//System.Drawing.SystemColors.ControlLightLight;
            resGrid.DataGrid.DefaultCellStyle.BackColor = Color.LightGray;// System.Drawing.SystemColors.ControlLightLight;
            resGrid.DataGrid.DefaultCellStyle.ForeColor = Color.Black;
            //dgvResults.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(3);
            resGrid.DataGrid.RowHeadersVisible = false;
            resGrid.DataGrid.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            resGrid.DataGrid.ResumeLayout();
            gviDisplay = new GridViewInspectionDisplay("GridInspRes_" + Settings.Id.ToString(), resGrid, _visionSystemMgr.Nodes[nodeId].Stations[stationId], machineConfig.RejectionCauses, null);
            visionSystemMgr.MeasuresContainer.Add(gviDisplay);
            btnUpload.Enabled = (_visionSystemMgr.Nodes[nodeId].ProviderName == "EoptisNodeBase2") ? true : false;
            btnRun.Enabled = (_visionSystemMgr.Nodes[nodeId].ProviderName == "EoptisNodeBase2") ? true : false;
            cbLive.Enabled = (_visionSystemMgr.Nodes[nodeId].ProviderName == "EoptisNodeBase2") ? true : false;
            //timerCameraStatus.Start();
            //btnLiveStart.Enabled = _camera.ROIModeAvailable;

            // TODO: Rimuovere il test non appena risolto bug di Tattile
            //if (_camera.CameraType.Contains("M9"))
            //    btnLiveStartFF.Enabled = false;

            //pnlCameraCommand.Visible = false;
            //pnlCameraCommand.Visible = true;

            timerDevStatus.Start();
        }

        //~CamViewerGretel() {

        //    Debug.WriteLine(Name + " DISPOSED!");
        //}

        public void UpdateLogCtrl(string message, Color color) {

            pgCameraParams.UpdateLogCtrl(message, color);
        }

        static DataGridViewHeaderBorderStyle properColumnHeadersBorderStyle {
            get {
                return (SystemFonts.MessageBoxFont.Name == "Segoe UI") ?
                DataGridViewHeaderBorderStyle.None :
                DataGridViewHeaderBorderStyle.Raised;
            }
        }

        public void Destroy() {

            timerDevStatus.Stop();
            yesNoPanel.YesNoAnswer -= yesNoPanel_YesNoAnswer;
            saveMenu.SaveMenuCondition -= saveMenu_SaveMenuCondition;
            pgCameraParams.BeginEdit -= pgCameraParams_BeginEdit;
            pgCameraParams.EndEdit -= pgCameraParams_EndEdit;
            zgcSpectra.ContextMenuBuilder -= zgcSpectra_ContextMenuBuilder;
            zgcElaboration.ContextMenuBuilder -= zgcElaboration_ContextMenuBuilder;
            _visionSystemMgr.MeasuresContainer.Remove(gviDisplay);
            gviDisplay.Dispose();
            resGrid.Dispose();
            pgCameraParams.Dispose();
            Dispose(true);
        }

        public IntPtr GetPictureBoxHandle() {

            return IntPtr.Zero;
        }

        public ImageBox GetLivePictureBox() {

            return null;
        }

        public Panel GetThumbPanel() {

            return null;
        }

        public ZedGraphControl[] GetGraphBox() {

            return extGraphs;
        }

        void pgCameraParams_EndEdit(object sender, DataGridViewCellCancelEventArgs e) {

            //e.Cancel = !AppEngine.Current.TrySetSupervisorInReadyToRun();
        }

        void pgCameraParams_BeginEdit(object sender, DataGridViewCellCancelEventArgs e) {

            //if (AppEngine.Current.CurrentContext.SupervisorMode != SupervisorModeEnum.Editing)
            //    e.Cancel = true;

            //    e.Cancel = !AppEngine.Current.TrySetSupervisorInEditing();
        }

        public new void Hide() {

            base.Hide();
        }

        /// <summary>
        /// Permette di impostare la pagina dei parametri visulizzata
        /// </summary>
        /// <param name="parameterType">Tipo dei parametri la cui pagina sarà visualizzata</param>
        public void SetParametersPage(ParameterTypeEnum parameterType) {

            currentParameterType = parameterType;
            loadParameters();
        }

        void pcbLive_VisibleChanged(object sender, EventArgs e) {

        }

        public void RefreshStationStatus() {

            if (_station.Enabled)
                lblStationStatus.Image = global::ExactaEasy.Properties.Resources.green_on_32;
            else
                lblStationStatus.Image = global::ExactaEasy.Properties.Resources.red_on_32;

        }

        void refreshData(IDataBinder paramData) {

            refreshData(paramData, null);
        }

        void refreshData(IDataBinder paramData, IDataBinder parentData) {

            pgCameraParams.SuspendLayout();

            if (parentData != null) {
                pgCameraParams.ParentDataSource = parentData;
                //pgCameraParams.HideParentColumn("PropertyId");
                pgCameraParams.HideParentColumn("Value");
                pgCameraParams.HideParentColumn("ActualValue");
                pgCameraParams.HideParentColumn("MinValue");
                pgCameraParams.HideParentColumn("MaxValue");
                pgCameraParams.SetParentColumnPercentageWidth("PropertyId", 20);
                pgCameraParams.SetParentColumnPercentageWidth("PropertyDescription", 80);
            }

            pgCameraParams.DataSource = paramData;
            pgCameraParams.HideColumn("PropertyId");
            //pgCameraParams.HideColumn("Editable");
            pgCameraParams.HideColumn("MinValue");
            pgCameraParams.HideColumn("MaxValue");
            if (AppEngine.Current.CurrentContext.UserLevel < UserLevelEnum.AssistantSupervisor)
                pgCameraParams.HideColumn("ActualValue");

            pgCameraParams.SetColumnCaption("PropertyDescription", frmBase.UIStrings.GetString("Parameter"));
            pgCameraParams.SetColumnCaption("Value", frmBase.UIStrings.GetString("Value"));
            pgCameraParams.SetColumnCaption("ActualValue", frmBase.UIStrings.GetString("Device"));
            pgCameraParams.SetColumnForeColor("ActualValue", Color.FromArgb(80, 80, 80));
            pgCameraParams.SetColumnPercentageWidth("PropertyDescription", 50);

            if (AppEngine.Current.CurrentContext.UserLevel >= UserLevelEnum.AssistantSupervisor) {
                pgCameraParams.SetColumnPercentageWidth("Value", 25);
                pgCameraParams.SetColumnPercentageWidth("ActualValue", 25);
            }
            else
                pgCameraParams.SetColumnPercentageWidth("Value", 50);

            pgCameraParams.ResumeLayout();
        }

        void getAcquisitionParam() {

            //resetCameraWarning();
            if (DataSource == null) return;

            try {
                /*if (actualAcquisitionParams == null && _node.Connected)*/
                actualAcquisitionParams = _camera.GetAcquisitionParameters();//readParams.Stations[_camera.StationId].Cameras[_camera.Head].AcquisitionParameters;
                DataSource.AcquisitionParameters.PopulateParametersInfo(actualAcquisitionParams);
                //btnApply.Enabled = true;
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "SpectrometerViewer.getAcquisitionParam", _camera.IP4Address + ": Get acquisition parameters failed. Error: " + ex.Message);
                setCameraWarning(string.Format(frmBase.UIStrings.GetString("AcquisitionGetError"), Camera.IdCamera));
            }
            if (currentParameterType == ParameterTypeEnum.Acquisition) {
                pgCameraParams.ShowDataGridViewParent = false;
                pgCameraParams.ShowTitleBar = false;
                refreshData(new DAL.ParametersDataBinder<Parameter>(DataSource.AcquisitionParameters, actualAcquisitionParams, AppEngine.Current.CurrentContext.UserLevel));
            }
        }

        void getDigitizerParam() {
        }

        void getElaborationParameter() {

            //resetCameraWarning();
            if (DataSource == null) return;

            try {
                //if (actualElaborationParams == null && _node.Connected)
                actualElaborationParams = _camera.GetRecipeSimpleParameters();//readParams.Stations[_camera.StationId].Cameras[0].RecipeSimpleParameters;//_camera.GetRecipeSimpleParameters();
                DataSource.RecipeSimpleParameters.PopulateParametersInfo(actualElaborationParams);
                //btnApply.Enabled = true;
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "SpectrometerViewer.getElaborationParameter", _camera.IP4Address + ": Get elaboration parameters failed. Error: " + ex.Message);
                setCameraWarning(string.Format(frmBase.UIStrings.GetString("ElaborationGetError"), Camera.IdCamera));
            }

            if (currentParameterType == ParameterTypeEnum.Elaboration) {

                pgCameraParams.ShowDataGridViewParent = false;
                pgCameraParams.ShowTitleBar = false;
                refreshData(new DAL.ParametersDataBinder<Parameter>(DataSource.RecipeSimpleParameters, actualElaborationParams, AppEngine.Current.CurrentContext.UserLevel));
            }
        }

        void getRecipeAdvancedParameter() {

        }

        public void getROIParameter() {

        }

        void getMachineParameter() {

            //resetCameraWarning();
            if (DataSource == null) return;

            try {
                //if (actualMachineParams == null && _node.Connected)
                actualMachineParams = _camera.GetMachineParameters();//readParams.Stations[_camera.StationId].Cameras[0].MachineParameters;//_camera.GetMachineParameters();
                DataSource.MachineParameters.PopulateParametersInfo(actualMachineParams);
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "SpectrometerViewer.getMachineParameter", _camera.IP4Address + ": Get machine parameters failed. Error: " + ex.Message);
                setCameraWarning(string.Format(frmBase.UIStrings.GetString("RecipeMachineGetError"), Camera.IdCamera));
            }
            if (currentParameterType == ParameterTypeEnum.Machine) {
                pgCameraParams.ShowDataGridViewParent = false;
                pgCameraParams.ShowTitleBar = false;
                refreshData(new DAL.ParametersDataBinder<Parameter>(DataSource.MachineParameters, actualMachineParams, AppEngine.Current.CurrentContext.UserLevel));
            }
        }

        void setCameraWarning() {

            setCameraWarning(frmBase.UIStrings.GetString("CameraError"));
        }

        void setCameraWarning(string message) 
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (InvokeRequired && IsHandleCreated) {
                this.Invoke(new MethodInvoker(() => setCameraWarning(message)));
            }
            else {
                //isCameraOK = false;
                setCameraStatus();
                OnCamViewerError(this, new CamViewerErrorEventArgs(_camera, message));
            }
        }

        void resetCameraWarning() 
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (InvokeRequired && IsHandleCreated) {
                this.Invoke(new MethodInvoker(() => resetCameraWarning()));
            }
            else {
                //isCameraOK = true;
                setCameraStatus();
                OnCamViewerError(this, new CamViewerErrorEventArgs(_camera, ""));
            }
        }

        private void btnCamera_Click(object sender, EventArgs e) {

            currentParameterType = ParameterTypeEnum.Acquisition;
            loadParameters();
        }

        private void btnDigitizer_Click(object sender, EventArgs e) {

            currentParameterType = ParameterTypeEnum.Digitizer;
            loadParameters();
        }

        private void btnElaboration_Click(object sender, EventArgs e) {

            currentParameterType = ParameterTypeEnum.Elaboration;
            loadParameters();
        }

        private void btnAdvanced_Click(object sender, EventArgs e) {

            currentParameterType = ParameterTypeEnum.RecipeAdvanced;
            loadParameters();
        }

        private void btnROI_Click(object sender, EventArgs e) {

            //if (DataSource.ROIParameters.Count <= 0) return;
            //roiIndex = (roiIndex + 1) % DataSource.ROIParameters.Count;
            //currentParameterType = ParameterTypeEnum.ROI;
            //loadParameters();

            ////nextRoiIndex++;
            ////if (nextRoiIndex >= DataSource.ROIParameters.Count)
            ////    nextRoiIndex = 0;
        }

        private void btnMachine_Click(object sender, EventArgs e) {
            currentParameterType = ParameterTypeEnum.Machine;
            loadParameters();
        }

        private void btnStrobo_Click(object sender, EventArgs e) {

            if (_camera.Lights != null && _camera.Lights.Count > 0)
                lightIndex = (lightIndex + 1) % _camera.Lights.Count;
            else
                lightIndex = 0;
            currentParameterType = ParameterTypeEnum.Strobo;
            loadParameters();
        }

        private void btnFormat_Click(object sender, EventArgs e) {


        }

        public void DoRefresh() {

            RefreshStationStatus();
            if (pgCameraParams.DataSource != null) {

                pgCameraParams.DataSource.UserLevel = AppEngine.Current.CurrentContext.UserLevel;
                loadParameters();
            }
            RecalcButton();
        }

        public void RecalcButton() 
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (InvokeRequired && IsHandleCreated) {
                Invoke(new MethodInvoker(() => RecalcButton()));
            }
            else {
                // pannello parametri / risultati
                if (cbViewResults.Checked) {

                    cbViewResults.Image = global::ExactaEasy.Properties.Resources.edit_find_red;
                    //cbViewResults.BackColor = Color.Moccasin;
                    pgCameraParams.Visible = false;
                    pnlParamSelector.Visible = false;
                    pnlParams.Visible = false;
                    pnlCameraTables.Visible = true;
                    resGrid.Visible = true;
                    resGrid.BringToFront();
                }
                else {

                    cbViewResults.Image = global::ExactaEasy.Properties.Resources.edit_find1;
                    //cbViewResults.BackColor = SystemColors.Control;
                    pgCameraParams.Visible = true;
                    pnlParamSelector.Visible = true;
                    pnlParams.Visible = true;
                    pnlCameraTables.Visible = false;
                    resGrid.Visible = false;
                    pnlParams.BringToFront();
                }

                if (AppEngine.Current.CurrentContext == null)
                    return;

                if (DataSource != null) {
                    btnCamera.Visible = true;           // (DataSource.AcquisitionParameters.Count != 0) ? true : false;
                    btnDigitizer.Visible = false;       // (DataSource.DigitizerParameters.Count != 0) ? true : false;
                    btnElaboration.Visible = true;      // (DataSource.RecipeSimpleParameters.Count != 0) ? true : false;
                    btnAdvanced.Visible = false;        // (DataSource.RecipeAdvancedParameters.Count != 0) ? true : false;
                    btnStrobo.Visible = false;          // (DataSource.Lights.Count != 0) ? true : false;
                    btnROI.Visible = false;             // (DataSource.ROIParameters.Count != 0) ? true : false;
                    btnMachine.Visible = true;          // (DataSource.MachineParameters.Count != 0) ? true : false;
                }
                //if (AppEngine.Current.CurrentContext.UserLevel < UserLevelEnum.Optrel) {
                //    btnMachine.Visible = false;
                //    btnAdvanced.Visible = false;
                //}
                //else {
                //    btnMachine.Visible = true;
                //    btnAdvanced.Visible = true;
                //}


                //if (AppEngine.Current.CurrentContext.UserLevel < UserLevelEnum.MinorSupervisor) {
                //    btnStopOnCondition.Enabled = false;
                //    //bttDownloadImages.Visible = false;
                //    bttAnalysis.Enabled = false;
                //    bttStopAnalysis.Enabled = false;
                //    btnRemoteDesktop.Enabled = false;
                //}
                //else {
                //    btnStopOnCondition.Enabled = true;
                //    //bttDownloadImages.Visible = true;
                //    bttAnalysis.Enabled = true;
                //    bttStopAnalysis.Enabled = true;
                //    btnRemoteDesktop.Enabled = true;
                //}

                //btnSave.Enabled = false; // Nuova gestione save esterna
                //lastBtnApplyEnableValue = btnApply.Enabled;
                MachineModeEnum machineMode = AppEngine.Current.CurrentContext.MachineMode;
                if (machineMode == MachineModeEnum.Running || AppEngine.Current.CurrentContext.UserLevel < UserLevelEnum.AssistantSupervisor || (_node != null && _node.Connected == false)) {
                    btnApply.Enabled = false;
                    btnEdit.Enabled = false;
                    //btnRestore.Enabled = false;
                    btnUpload.Enabled = false;
                    btnRun.Enabled = false;
                    cbLive.Enabled = false;
                    btnSaveSpectra.Enabled = false;
                    cbOfflineMode.Enabled = false;
                    zgcElaboration.IsShowContextMenu = false;
                    zgcElaboration.IsShowPointValues = false;
                    zgcSpectra.IsShowContextMenu = false;
                    zgcSpectra.IsShowPointValues = false;
                }
                else {
                    switch (ParameterEditingState) {
                        case ParameterEditingStates.Reading:
                        case ParameterEditingStates.Applied:
                            //btnApply.Enabled = false;
                            //btnEdit.Enabled = true;
                            //btnRestore.Enabled = false;
                            pnlUp.Enabled = true;
                            break;
                        case ParameterEditingStates.Editing:
                            //btnApply.Enabled = true;
                            //btnEdit.Enabled = false;
                            //btnRestore.Enabled = true;
                            pnlUp.Enabled = false;
                            break;
                        //case ParameterEditingStates.Applied:
                        //    btnApply.Enabled = true;
                        //    btnRestore.Enabled = true;
                        //    break;
                    }
                    btnApply.Enabled = true;
                    btnUpload.Enabled = true;
                    btnRun.Enabled = true;
                    cbLive.Enabled = true;
                    btnSaveSpectra.Enabled = true;
                    cbOfflineMode.Enabled = true;
                    zgcElaboration.IsShowContextMenu = true;
                    zgcElaboration.IsShowPointValues = true;
                    zgcSpectra.IsShowContextMenu = true;
                    zgcSpectra.IsShowPointValues = true;
                }
            }
        }

        public void ReloadParameters(ParameterTypeEnum parType) {

            if ((parType & ParameterTypeEnum.Acquisition) == ParameterTypeEnum.Acquisition)
                actualAcquisitionParams = null;
            //if ((parType & ParameterTypeEnum.Digitizer) == ParameterTypeEnum.Digitizer)
            //    actualDigitizerParams = null;
            if ((parType & ParameterTypeEnum.Elaboration) == ParameterTypeEnum.Elaboration)
                actualElaborationParams = null;
            //if ((parType & ParameterTypeEnum.RecipeAdvanced) == ParameterTypeEnum.RecipeAdvanced)
            //    actualRecipeAdvancedParams = null;
            //if ((parType & ParameterTypeEnum.ROI) == ParameterTypeEnum.ROI)
            //    actualROIParams = null;
            if ((parType & ParameterTypeEnum.Machine) == ParameterTypeEnum.Machine)
                actualMachineParams = null;
            //if ((parType & ParameterTypeEnum.Strobo) == ParameterTypeEnum.Strobo)
            //    actualStroboParams = null;

            loadParameters(parType);
        }


        void loadParameters() {

            loadParameters(currentParameterType);
        }

        //NodeRecipe readParams = null; //PIER
        void loadParameters(ParameterTypeEnum parType) {

            if (InvokeRequired == true) {
                Invoke(new MethodInvoker(() => loadParameters(parType)));
            }
            else {
            //if (!isCameraReady()) {
            //    OnCamViewerMessage(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("CameraNotReady")));
            //    return;
            //}

            OnCamViewerMessage(this, new CamViewerMessageEventArgs(""));
            //isCameraOK = true;
            pgCameraParams.Title = paramTitleLabel[currentParameterType];

            try {
                _node.GetParameters();
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "SpectrometerViewer.loadParameters", _node.Address + ": GetParameters failed! Error: " + ex.Message);
                setCameraWarning(string.Format(_node.Address + ": " + frmBase.UIStrings.GetString("GetParametersFailed")));
            }
            foreach (Control c in pnlParamSelector.Controls) {
                if (c is Button)
                    c.BackColor = SystemColors.Control;
            }
            if ((parType & ParameterTypeEnum.Acquisition) != 0) {
                getAcquisitionParam();
            }
            if ((parType & ParameterTypeEnum.Digitizer) != 0) {
                getDigitizerParam();
            }
            if ((parType & ParameterTypeEnum.Elaboration) != 0) {
                getElaborationParameter();
            }
            if ((parType & ParameterTypeEnum.RecipeAdvanced) != 0) {
                getRecipeAdvancedParameter();
            }
            if ((parType & ParameterTypeEnum.ROI) != 0) {
                getROIParameter();
            }
            if ((parType & ParameterTypeEnum.Machine) != 0) {
                getMachineParameter();
            }

            if (currentParameterType == ParameterTypeEnum.Acquisition) {
                btnCamera.BackColor = SystemColors.GradientActiveCaption;
            }
            if (currentParameterType == ParameterTypeEnum.Digitizer) {
                btnDigitizer.BackColor = SystemColors.GradientActiveCaption;
            }
            if (currentParameterType == ParameterTypeEnum.Elaboration) {
                btnElaboration.BackColor = SystemColors.GradientActiveCaption;
            }
            if (currentParameterType == ParameterTypeEnum.RecipeAdvanced) {
                btnAdvanced.BackColor = SystemColors.GradientActiveCaption;
            }
            if (currentParameterType == ParameterTypeEnum.ROI) {
                btnROI.BackColor = SystemColors.GradientActiveCaption;
                if (roiPageLabel.ContainsKey(roiIndex))
                    pgCameraParams.Title = roiPageLabel[roiIndex] + " (" + (roiIndex + 1).ToString() + "/" + DataSource.ROIParameters.Count.ToString() + ")";
                else
                    pgCameraParams.Title += " (" + (roiIndex + 1).ToString() + "/" + DataSource.ROIParameters.Count.ToString() + ")";
            }
            if (currentParameterType == ParameterTypeEnum.Machine) {
                btnMachine.BackColor = SystemColors.GradientActiveCaption;
                }
            }
            //if (isTattileOK)
            //    btnApply.Enabled = true;
        }

        private void btnApply_Click(object sender, EventArgs e) {

            if (AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Busy)) {
                btnApply.Enabled = false;
                btnApply.Refresh();
                pgCameraParams.UpdateLogCtrl("", Color.White);
                if (applyParameters(ParameterTypeEnum.All) == true) {
                    //ParameterEditingState = ParameterEditingStates.Applied;
                    AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.ReadyToRun);
                    pgCameraParams.UpdateLogCtrl("PARAMETERS SET SUCCESSFULLY", Color.Green);
                }
                else {
                    //ParameterEditingState = ParameterEditingStates.Reading;
                    AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Error);
                    pgCameraParams.UpdateLogCtrl("PARAMETERS SET FAILED", Color.Red);
                }
                RecalcButton();
                pgCameraParams.Refresh();
                drawLimits();
                //if (ParameterEditingState == ParameterEditingStates.Applied)
                //    AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.ReadyToRun);
                //else
                //    AppEngine.Current.TrySetSupervisorInEditing();
                //AppEngine.Current.TrySetSupervisorInReadyToRun();
            }
            else
                Log.Line(LogLevels.Warning, "SpectrometerViewer.btnApply_Click", _camera.IP4Address + ": Cannot apply parameters, machine mode: " + AppEngine.Current.CurrentContext.MachineMode.ToString());
        }

        private void btnEdit_Click(object sender, EventArgs e) {

        }

        private void btnSave_Click(object sender, EventArgs e) {

        }

        private void btnRestore_Click(object sender, EventArgs e) {

            //if (AppEngine.Current.CurrentContext.SupervisorMode == SupervisorModeEnum.Editing) {
            //    DataSource = originalDataSource.Clone(AppEngine.Current.ParametersInfo, AppEngine.Current.CurrentContext.CultureCode);
            //    loadParameters();
            //    if (AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.ReadyToRun)) {
            //        ParameterEditingState = _prevParameterEditingState;
            //        RecalcButton();
            //    }
            //}
        }

        bool applyParameters(ParameterTypeEnum paramType) {

            bool ris = true;
            this.Cursor = Cursors.WaitCursor;
            OnCamViewerMessage(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("SendParam")));
            Application.DoEvents();
            //pollEv.Reset();
            resetCameraWarning();
            //timerCameraStatus.Stop();
            try {
                _visionSystemMgr.Nodes[nodeId].SetParameters("", NodeDataSource, AppEngine.Current.ParametersInfo, AppEngine.Current.CurrentContext.CultureCode);   //pier: modificare recipeName
                if ((paramType & ParameterTypeEnum.Acquisition) != 0) actualAcquisitionParams = null;
                //if ((paramType & ParameterTypeEnum.Digitizer) != 0) actualDigitizerParams = null;
                if ((paramType & ParameterTypeEnum.Elaboration) != 0) actualElaborationParams = null;
                //if ((paramType & ParameterTypeEnum.RecipeAdvanced) != 0) actualRecipeAdvancedParams = null;
                //if ((paramType & ParameterTypeEnum.ROI) != 0) actualROIParams = null;
                if ((paramType & ParameterTypeEnum.Machine) != 0) actualMachineParams = null;
                //if ((paramType & ParameterTypeEnum.Strobo) != 0) actualStroboParams = null;
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "SpectrometerViewer.applyParameters", _camera.IP4Address + ": Error while sending parameters: " + ex.Message);
                OnCamViewerError(this, new CamViewerErrorEventArgs(_camera, _camera.IP4Address + ": " + frmBase.UIStrings.GetString("ErrorSendingParameters")));
                ris = false;
            }
            loadParameters(paramType);
            //pollEv.Set();
            //timerCameraStatus.Start();
            OnCamViewerMessage(this, new CamViewerMessageEventArgs(""));
            this.Cursor = Cursors.Default;
            return ris;
        }

        void yesNoPanel_YesNoAnswer(object sender, CamViewerMessageEventArgs e) {

            if (yesNoPanel.Tag.ToString() == "btnUpload" && e.Message == "Yes") {

                btnUpload.Enabled = false;
                btnUpload.Refresh();
                btnApply.Enabled = false;
                btnApply.Refresh();
                pgCameraParams.UpdateLogCtrl("", Color.White);
                OnCamViewerMessage(this, new CamViewerMessageEventArgs("SuspendLayout"));
                //SuspendLayout();
                try {
                    _visionSystemMgr.Nodes[nodeId].UploadParameters();
                    _visionSystemMgr.Nodes[nodeId].SetParameters("", NodeDataSource, AppEngine.Current.ParametersInfo, AppEngine.Current.CurrentContext.CultureCode);   //pier: modificare recipeName
                    //actualAcquisitionParams = null;
                    ////actualDigitizerParams = null;
                    //actualElaborationParams = null;
                    ////actualRecipeAdvancedParams = null;
                    ////actualROIParams = null;
                    //actualMachineParams = null;
                    //loadParameters();
                    OnParametersUploaded(this, new CamViewerMessageEventArgs("SUCCESSFULL"));
                }
                catch (Exception ex) {
                    Log.Line(LogLevels.Error, "SpectrometerViewer.yesNoPanel_YesNoAnswer", _camera.IP4Address + ": Error while uploading parameters: " + ex.Message);
                    OnCamViewerError(this, new CamViewerErrorEventArgs(_camera, _camera.IP4Address + ": " + frmBase.UIStrings.GetString("ErrorUploadingParameters")));
                    OnParametersUploaded(this, new CamViewerMessageEventArgs("FAILED"));
                }
                finally {
                    
                    //btnUpload.Enabled = true;
                    //OnCamViewerMessage(this, new CamViewerMessageEventArgs("UploadParametersDone"));
                    //OnCamViewerMessage(this, new CamViewerMessageEventArgs("ResumeLayout"));
                    //ResumeLayout(false);
                    Destroy();
                }
            }
            yesNoPanel.Tag = "";
            yesNoPanel.Visible = false;
            pnlUp.Visible = true;
            RecalcButton();
            pgCameraParams.Refresh();
        }

        void saveMenu_SaveMenuCondition(object sender, CamViewerMessageEventArgs e) {

            int toSave;
            int.TryParse(e.Parameter, out toSave);
            if (e.Message == "Any") {
                Node.SaveBufferedImages("", SaveConditions.Any, toSave);
            }
            if (e.Message == "Good") {
                Node.SaveBufferedImages("", SaveConditions.Good, toSave);
            }
            if (e.Message == "Reject") {
                Node.SaveBufferedImages("", SaveConditions.Reject, toSave);
            }
            saveMenu.Tag = "";
            saveMenu.Visible = false;
            pnlUp.Visible = true;
            RecalcButton();
        }

        void loadSaveMenu_MenuAction(object sender, CamViewerMessageEventArgs e) {

            if (e.Message == "Load") {
                using (OpenFileDialog oDialog = new OpenFileDialog()) {

                    oDialog.Filter = "Recipe Files (*.xml)|*.xml";
                    oDialog.RestoreDirectory = true;
                    if (oDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                        try {
                            NodeRecipe spectroRecipe = loadSpectroRecipe(oDialog.FileName);
                            // AppEngine.Current.CurrentContext.ActiveRecipe.Nodes[_node.IdNode] = spectroRecipe.Clone(AppEngine.Current.ParametersInfo, AppEngine.Current.CurrentContext.CultureCode);
                            //AppEngine.Current.SetActiveRecipe(AppEngine.Current.CurrentContext.ActiveRecipe, false);
                            // AppEngine.Current.CurrentContext.ActiveRecipe.Nodes[_node.IdNode] = spectroRecipe.Clone(AppEngine.Current.ParametersInfo, AppEngine.Current.CurrentContext.CultureCode
                            //SetDataSource(spectroRecipe);
                            //NodeDataSource = spectroRecipe;
                            (_node as DisplayManager.Node).OnNodeRecipeUpdate(this, new NodeRecipeEventArgs(spectroRecipe, false));
                            btnApply_Click(this, EventArgs.Empty);
                            //applyParameters(ParameterTypeEnum.All);
                            //_node.SetParameters("", spectroRecipe, AppEngine.Current.ParametersInfo, AppEngine.Current.CurrentContext.CultureCode);
                        }
                        catch (Exception ex) {
                            OnCamViewerError(this, new CamViewerErrorEventArgs(_camera, "Spectrophotometer recipe loading failed! " + ex.ToString()));
                        }
                    }
                }
            }
            if (e.Message == "Save") {
                using (SaveFileDialog saveFileDlg = new SaveFileDialog()) {
                    saveFileDlg.Filter = "Recipe Files (*.xml)|*.xml";
                    if (DialogResult.OK == saveFileDlg.ShowDialog()) {
                        try {
                            NodeRecipe currentParameters = _node.GetParameters();
                            Recipe newRecipe = new Recipe();
                            newRecipe.RecipeName = Description + " - " + _node.Stations.First().Description + " - " + _node.Stations.First().Cameras.First().CameraDescription;
                            newRecipe.Nodes = new List<NodeRecipe>();
                            newRecipe.Nodes.Add(currentParameters);
                            currentParameters.Description = "Eoptis Device parameters";
                            newRecipe.SaveXml(saveFileDlg.FileName);
                        }
                        catch (Exception ex) {
                            OnCamViewerError(this, new CamViewerErrorEventArgs(_camera, "Spectrophotometer recipe saving failed! " + ex.ToString()));
                        }
                    }
                }
            }
            loadSaveMenu.Tag = "";
            loadSaveMenu.Visible = false;
            pnlUp.Visible = true;
            RecalcButton();
        }

        NodeRecipe loadSpectroRecipe(string path) {

            Recipe recipe = Recipe.LoadFromFile(path, false);
            recipe.Nodes.First().Id = _node.IdNode;
            if (recipe == null ||
                recipe.Nodes == null ||
                recipe.Nodes.Count == 0 ||
                recipe.Nodes.Count > 1 ||
                recipe.Nodes.First().Description != "Eoptis Device parameters") {

                throw new Exception("Wrong spectrophotometer recipe");
            }
            return recipe.Nodes.First();
        }

        void OnParameterEditingStatusChanged(object sender, EventArgs e) {

            if (ParameterEditingStateChanged != null)
                ParameterEditingStateChanged(sender, e);
        }

        void LiveModeMenu_ConditionUpdated(object sender, EventArgs e) {
        }

        private void pnlCenter_Resize(object sender, EventArgs e) {

            //pcbLive.Height = pnlCenter.Height;
            //if (pnlCenter.Width > pcbLive.Width)
            //    pcbLive.Left = (pnlCenter.Width - pcbLive.Width) / 2;
            //else
            //    pcbLive.Left = 0;
            //if (_visionSystemMgr.Displays != null && _visionSystemMgr.Displays.Count > 0 && _visionSystemMgr.Displays["Cam_" + Settings.Id] != null)
            //    _visionSystemMgr.Displays["Cam_" + Settings.Id].Resize(pnlCenter.Width, pnlCenter.Height);
        }

        enum ButtonLiveStatus {
            Run,
            Live,
            StopOnCondition
        }

        void setCameraStatus() {

        }

        protected virtual void OnCamViewerError(object sender, CamViewerErrorEventArgs e) {

            if (CamViewerError != null)
                CamViewerError(sender, e);
        }

        protected virtual void OnCamViewerMessage(object sender, CamViewerMessageEventArgs e) {

            if (CamViewerMessage != null)
                CamViewerMessage(sender, e);
        }

        protected virtual void OnParametersUploaded(object sender, CamViewerMessageEventArgs e) {

            if (ParametersUploaded != null)
                ParametersUploaded(sender, e);
        }

        private void buttonConnect_Click(object sender, EventArgs e) {
            ((IStation)_camera).Connect();
        }

        private void buttonDisconnect_Click(object sender, EventArgs e) {
            ((IStation)_camera).StopGrab();
            Thread.Sleep(5000);
            ((IStation)_camera).Disconnect();
        }

        bool isCameraReady() {
            try {
                CameraNewStatus camStatus = _camera.GetCameraStatus();
                if (camStatus != CameraNewStatus.Ready) {
                    //isCameraOK = false;
                    return false;
                }
            }
            catch {
                return false;
            }
            return true;
        }

        void getROIPageLabel() {

            roiPageLabel = new Dictionary<int, string>();
            string language = AppEngine.Current.CurrentContext.CultureCode;
            for (int i = 0; i < dataSource.ROIParameters.Count(); i++) {
                Parameter p = dataSource.ROIParameters[i].Find(pr => { return pr.Id.ToLower() == "label_" + language; });
                if (p != null)
                    roiPageLabel.Add(i, p.Value);
            }
        }

        public void ConfirmSavedParameters() {

            ParameterEditingState = ParameterEditingStates.Reading;
        }

        public void SetDataSource(object dataSource) {

            if (dataSource is NodeRecipe) {
                NodeDataSource = (NodeRecipe)dataSource;
                //DataSource = NodeDataSource.Stations[_camera.StationId].Cameras[0];
                if (gviDisplay != null)
                    gviDisplay.CurrRecipe = AppEngine.Current.CurrentContext.ActiveRecipe;

                drawLimits();
            }
        }

        private void cbViewResults_CheckedChanged(object sender, EventArgs e) {

            RecalcButton();
        }

        public void StartRunningVisualization() {

            //cbViewResults_CheckedChanged(this, EventArgs.Empty);
            RecalcButton();
        }

        private void timerDevStatus_Tick(object sender, EventArgs e) {

            if (_camera != null) {
                CameraWorkingMode workingMode = _camera.GetWorkingMode();
                if (workingMode != CameraWorkingMode.ExternalSource &&
                    workingMode != CameraWorkingMode.Idle) {
                    lblStatus.ForeColor = Color.Red;
                }
                else
                    lblStatus.ForeColor = Color.Black;
                lblStatus.Text = frmBase.UIStrings.GetString("WorkingMode") + ": " + workingMode.ToString();
            }
        }

        private void btnReset_Click(object sender, EventArgs e) {

            Graph g = _visionSystemMgr.Graphs["Cam_" + Settings.Id];
            if (g != null) {
                g.ResetCounters();
            }
            //bool ris = true;
            //this.Cursor = Cursors.WaitCursor;
            //OnCamViewerMessage(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("SendParam")));
            //Application.DoEvents();
            //resetCameraWarning();
            //try {
            //    _visionSystemMgr.Nodes[nodeId].SetParameters("", NodeDataSource, AppEngine.Current.ParametersInfo, AppEngine.Current.CurrentContext.CultureCode);   //pier: modificare recipeName
            //    if ((paramType & ParameterTypeEnum.Acquisition) != 0) actualAcquisitionParams = null;
            //    if ((paramType & ParameterTypeEnum.Digitizer) != 0) actualDigitizerParams = null;
            //    if ((paramType & ParameterTypeEnum.Elaboration) != 0) actualElaborationParams = null;
            //    if ((paramType & ParameterTypeEnum.RecipeAdvanced) != 0) actualRecipeAdvancedParams = null;
            //    if ((paramType & ParameterTypeEnum.ROI) != 0) actualROIParams = null;
            //    if ((paramType & ParameterTypeEnum.Machine) != 0) actualMachineParams = null;
            //    if ((paramType & ParameterTypeEnum.Strobo) != 0) actualStroboParams = null;
            //}
            //catch (Exception ex) {
            //    Log.Line(LogLevels.Error, "CamViewer.btnReset_Click", _camera.IP4Address + ": Error while sending parameters: " + ex.Message);
            //    OnCamViewerError(this, new CamViewerErrorEventArgs(_camera, _camera.IP4Address + ": " + frmBase.UIStrings.GetString("ErrorSendingParameters")));
            //    ris = false;
            //}
            //OnCamViewerMessage(this, new CamViewerMessageEventArgs(""));
            //this.Cursor = Cursors.Default;
            //return ris;
        }

        public void CreateGraphs(Graph g, object recipe) {

            NodeRecipe nr = (NodeRecipe)recipe;
            int idCamera;
            if (!int.TryParse(g.Name.Substring("Cam_".Length), out idCamera))
                return;
            Camera c = (Camera)_visionSystemMgr.Cameras[idCamera];
            int station = c.StationId;
            int node = c.NodeId;
            //int head = c.Head;
            if (nr == null) {
                Log.Line(LogLevels.Warning, "SpectrometerViewer.CreateGraph", "Wrong node recipe to create graphs");
                return;
            }
            StationRecipe sr = nr.Stations.Find(ss => ss.Id == station);
            CameraRecipe cr = sr.Cameras.First();
            if (sr == null) {
                Log.Line(LogLevels.Warning, "SpectrometerViewer.CreateGraph", "Wrong station recipe to create graphs");
                return;
            }
            int initialWavelength = 0, finalWavelength = 0;
            Parameter initialWavelengthParam = cr.AcquisitionParameters.Find(pp => pp.Id == "INITIAL_WAVELENGTH");
            Parameter finalWavelengthParam = cr.AcquisitionParameters.Find(pp => pp.Id == "FINAL_WAVELENGTH");
            if (initialWavelengthParam != null && finalWavelengthParam != null) {
                if (int.TryParse(initialWavelengthParam.Value, out initialWavelength) == false || int.TryParse(finalWavelengthParam.Value, out finalWavelength) == false) {
                    Log.Line(LogLevels.Error, "SpectrometerViewer.CreateGraph", "Error while casting wavelength. Check parameter in recipe!");
                    return;
                }
            }
            else {
                Log.Line(LogLevels.Error, "SpectrometerViewer.CreateGraph", "Error while casting wavelength. Add parameter to recipe!");
                return;
            }
            foreach (Tool tool in sr.Tools) {
                string yAxisLabel = "";
                foreach (ToolOutput to in tool.ToolOutputs) {
                    if (to.EnableGraph) {
                        yAxisLabel += to.Label + " " + to.MeasureUnit + "/";
                        string chartLabel = nodeId + "_" + stationId + "_" + to.Id;
                        if (to.Id == "PAR_FULL_SPECTRUM_MIS") {
                            g.RemoveChart("Spectra", chartLabel);
                            g.AddChart("Spectra", chartLabel, Color.Green, ZedGraph.SymbolType.None);
                        }
                        if (to.Id == "PAR_FULL_SPECTRUM_BUFFER") {
                            g.RemoveChart("Spectra", chartLabel);
                            g.AddChart("Spectra", chartLabel, Color.Red, ZedGraph.SymbolType.None);
                        }
                        if (to.Id == "PAR_FULL_SPECTRUM_ELAB") {
                            g.RemoveChart("Elaboration", chartLabel);
                            g.AddChart("Elaboration", chartLabel, Color.Azure, ZedGraph.SymbolType.None);
                        }
                    }
                }
                yAxisLabel = yAxisLabel.TrimEnd(new char[] { '/' });
                g.SetTitles("Spectra", sr.Description, "Wavelength [nm]", "Measured vs Reference Spectrum", initialWavelength, finalWavelength, 0, 65000);      //parametrizzare
                g.SetTitles("Elaboration", sr.Description, "Wavelength [nm]", "Difference Spectrum", initialWavelength, finalWavelength, -10000, 10000);        //parametrizzare
            }
        }

        void drawLimits() {

            //ora setto le soglie sul grafico...todo17 andrebbero messe nei ToolOutput invece che nei SimpleParameters...
            Graph g = _visionSystemMgr.Graphs["Cam_" + Settings.Id];
            if ((g != null) && (NodeDataSource != null)) {
                CameraRecipe cr = NodeDataSource.Stations[0].Cameras.First();
                if (cr != null) {
                    string chartLabel = "";
                    List<Parameter> ranges = cr.RecipeSimpleParameters.FindAll(pp => pp.Id.StartsWith("PAR_RANGE_") && (pp.Id.EndsWith("_L") || pp.Id.EndsWith("_H")));
                    List<Parameter> limits = cr.RecipeSimpleParameters.FindAll(pp => pp.Id.StartsWith("PAR_RANGE_") && (pp.Id.EndsWith("_TH") || pp.Id.EndsWith("_PEAK_REJECT_TH")));
                    foreach (Parameter range in ranges) {
                        if (range.Id != null) {
                            chartLabel = nodeId + "_" + stationId + "_" + range.Id;
                            g.RemoveChart("Spectra", chartLabel);
                            g.RemoveChart("Elaboration", chartLabel);
                            g.SetLimit("Spectra", chartLabel, LimitType.xLimit, Convert.ToDouble(range.Value, CultureInfo.InvariantCulture), -1, -1, Color.Yellow, System.Drawing.Drawing2D.DashStyle.Dash);
                            g.SetLimit("Elaboration", chartLabel, LimitType.xLimit, Convert.ToDouble(range.Value, CultureInfo.InvariantCulture), -1, -1, Color.Yellow, System.Drawing.Drawing2D.DashStyle.Dash);
                        }
                    }
                    foreach (Parameter limit in limits) {
                        if (limit.Id != null) {
                            chartLabel = nodeId + "_" + stationId + "_" + limit.Id;
                            Color chartColor = Color.Green;
                            System.Drawing.Drawing2D.DashStyle dashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                            string strId = limit.Id.Replace("PAR_RANGE_", "");
                            if (limit.Id.EndsWith("_PEAK_REJECT_TH") == true) {
                                strId = strId.Replace("_PEAK_REJECT_TH", "");
                                chartColor = Color.Red;
                                dashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                            }
                            if (limit.Id.EndsWith("_TH") == true) {
                                strId = strId.Replace("_TH", "");
                            }
                            int id = 0;
                            int.TryParse(strId, out id);
                            Parameter rangeL = ranges.Find(pp => pp.Id == ("PAR_RANGE_" + id + "_L"));
                            Parameter rangeH = ranges.Find(pp => pp.Id == ("PAR_RANGE_" + id + "_H"));
                            if ((rangeL != null) && (rangeH != null)) {
                                //g.RemoveChart("Spectra", chartLabel);
                                g.RemoveChart("Elaboration", chartLabel + "_1");
                                g.RemoveChart("Elaboration", chartLabel + "_2");
                                if (Convert.ToDouble(rangeL.Value, CultureInfo.InvariantCulture) < Convert.ToDouble(rangeH.Value, CultureInfo.InvariantCulture)) {
                                    g.SetLimit("Elaboration", chartLabel + "_1", LimitType.yLimit, Convert.ToDouble(limit.Value, CultureInfo.InvariantCulture), Convert.ToDouble(rangeL.Value, CultureInfo.InvariantCulture), Convert.ToDouble(rangeH.Value, CultureInfo.InvariantCulture), chartColor, dashStyle);
                                    g.SetLimit("Elaboration", chartLabel + "_2", LimitType.yLimit, -Convert.ToDouble(limit.Value, CultureInfo.InvariantCulture), Convert.ToDouble(rangeL.Value, CultureInfo.InvariantCulture), Convert.ToDouble(rangeH.Value, CultureInfo.InvariantCulture), chartColor, dashStyle);
                                }
                            }
                        }
                    }
                    Parameter refSpectrum = cr.RecipeSimpleParameters.Find(pp => pp.Id == "PAR_FULL_SPECTRUM_REF_NORM");
                    if (refSpectrum != null) {
                        chartLabel = nodeId + "_" + stationId + "_" + refSpectrum.Id;
                        g.RemoveChart("Spectra", chartLabel);
                        g.AddChart("Spectra", chartLabel, Color.Yellow, ZedGraph.SymbolType.None);
                        int numOfPoints = refSpectrum.Value.Count(cc => cc == ';') / 2;
                        g.AddPoints("Spectra", chartLabel, refSpectrum.Value, numOfPoints);
                    }
                }
            }
        }

        void zgcSpectra_ContextMenuBuilder(ZedGraphControl sender, ContextMenuStrip menuStrip, Point mousePt, ZedGraphControl.ContextMenuObjectState objState) {

            menuStrip.Items.RemoveAt(7);
        }

        void zgcElaboration_ContextMenuBuilder(ZedGraphControl sender, ContextMenuStrip menuStrip, Point mousePt, ZedGraphControl.ContextMenuObjectState objState) {

            menuStrip.Items.RemoveAt(7);
        }


        private void btnUpload_Click(object sender, EventArgs e) {

            pnlUp.Visible = false;
            yesNoPanel.Tag = "btnUpload";
            yesNoPanel.Visible = true;
        }

        private void btnRun_Click(object sender, EventArgs e) {

            resetCameraWarning();
            pnlUp.Enabled = false;
            pnlUp.Refresh();
            try {
                _node.Run(stationId);
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "SpectrometerViewer.btnRun_Click", _node.Address + ": Run failed. Error: " + ex.Message);
                setCameraWarning(string.Format(_node.Address + ": " + frmBase.UIStrings.GetString("RunFailed")));
            }
            pnlUp.Enabled = true;
        }

        private void cbViewInfo_CheckedChanged(object sender, EventArgs e) {

            recalcInfo();
        }

        void recalcInfo() {

            bool curVisibility = false;
            Graph g = _visionSystemMgr.Graphs["Cam_" + Settings.Id];
            if (g != null) {
                string chartLabel = nodeId + "_" + stationId + "_" + "PAR_FULL_SPECTRUM_BUFFER";
                curVisibility = g.GetChartVisible("Spectra", chartLabel);
                if (curVisibility != cbViewInfo.Checked) {
                    if (g.SetChartVisible("Spectra", chartLabel, cbViewInfo.Checked) == true) {
                        cbViewInfo.Image = (cbViewInfo.Checked == true) ? global::ExactaEasy.Properties.Resources.dialog_information_red : global::ExactaEasy.Properties.Resources.dialog_information;
                        return;
                    }
                }
            }
            cbViewInfo.Checked = curVisibility;
        }

        private void btnAnalyze_Click(object sender, EventArgs e) {

            pnlUp.Enabled = false;
            pnlUp.Refresh();
            _node.Run(stationId);
            pnlUp.Enabled = true;
        }

        private void btnSaveSpectra_Click(object sender, EventArgs e) {

            pnlUp.Visible = false;
            saveMenu.Tag = "btnSaveSpectra";
            saveMenu.UncheckAll();
            saveMenu.Visible = true;

            //Graph g = _visionSystemMgr.Graphs["Cam_" + Settings.Id];
            //if ((g != null) && (NodeDataSource != null)) {
            //    using (SaveFileDialog saveFileDlg = new SaveFileDialog()) {
            //        saveFileDlg.RestoreDirectory = true;
            //        saveFileDlg.Filter = "Spectrum File (*.spe)|*.spe";
            //        if (DialogResult.OK == saveFileDlg.ShowDialog()) {
            //            try {
            //                g.Save("Spectra", saveFileDlg.FileName);
            //                //_display.ImageShown.Save(saveFileDlg.FileName);
            //                Log.Line(LogLevels.Pass, "SpectrometerViewer.btnRec_Click", _camera.IP4Address + ": Spectrum saved");
            //                //OnConditionUpdated(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("ImageSaved")));
            //            }
            //            catch (Exception ex) {
            //                Log.Line(LogLevels.Error, "SpectrometerViewer.btnRec_Click", _camera.IP4Address + ": Error: " + ex.Message);
            //                //OnConditionUpdated(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("ImagesNotDownloaded" + "!")));
            //            }
            //        }
            //    }
            //}
        }

        private void cbOfflineMode_CheckedChanged(object sender, EventArgs e) {

            if (cbOfflineMode.Checked == true) {
                Camera.StartAnalysisOffline();
            }
            else {
                Camera.StopAnalysisOffline();
            }
            cbOfflineMode.Image = (cbOfflineMode.Checked == true) ? global::ExactaEasy.Properties.Resources.application_x_trash_red : global::ExactaEasy.Properties.Resources.application_x_trash;
        }

        private void cbLive_CheckedChanged(object sender, EventArgs e) {

            //pnlUp.Enabled = false;
            //pnlUp.Refresh();
            //_liveMode = cbLive.Checked;
            if (cbLive.Checked == true) {
                foreach (Control c in pnlUp.Controls) {
                    if (c is Button || c is CheckBox || c is RadioButton) {
                        c.Enabled = false;
                    }
                }
                cbLive.Enabled = true;
                cbViewInfo.Enabled = true;
                //timerLive.Start();
            }
            else {
                //timerLive.Stop();
                foreach (Control c in pnlUp.Controls) {
                    if (c is Button || c is CheckBox || c is RadioButton) {
                        c.Enabled = true;
                    }
                }
            }
            _node.Live(stationId, cbLive.Checked);
            //pnlUp.Enabled = true;
            cbLive.Image = (cbLive.Checked == true) ? global::ExactaEasy.Properties.Resources.media_playback_stop : global::ExactaEasy.Properties.Resources.media_playback_start;
        }

        private void timerLive_Tick(object sender, EventArgs e) {

            //if (_liveMode == true) {
            _node.Run(stationId);
            //}
        }

        private void btnLoadSave_Click(object sender, EventArgs e) {

            pnlUp.Visible = false;
            loadSaveMenu.Tag = "btnLoadSave";
            loadSaveMenu.Visible = true;
        }
        /// <summary>
        /// MM-16/01/2024: sadly, GetSignalStacker must be implemented even here, as it is a member of the ICamViewer.
        /// </summary>
        public HvldDisplayControl GetSignalStacker()
        {
            return null;
        }
    }
}
