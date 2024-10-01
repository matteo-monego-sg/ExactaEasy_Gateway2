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
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZedGraph;

namespace ExactaEasy
{

    public partial class DataViewer : UserControl, ICamViewer {

        public event EventHandler<CamViewerErrorEventArgs> CamViewerError;
        public event EventHandler<CamViewerMessageEventArgs> CamViewerMessage;
        public event EventHandler ParameterEditingStateChanged;
        public event EventHandler<CamViewerMessageEventArgs> ParametersUploaded;

        public static List<ValuesTurbidimeter> listValuesTB = new List<ValuesTurbidimeter>();

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

        int spindleStart = 0;
        int increment = 2;

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
        ZedGraphControl[] extGraphs = new ZedGraphControl[1];

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

        int turretLowLimit, turretHighLimit;

        DataViewer() {
            InitializeComponent();

            extGraphs[0] = extGraphBox;

            btnApply.Text = frmBase.UIStrings.GetString("BtnApply");
            btnEdit.Text = frmBase.UIStrings.GetString("BtnEdit");
            btnSave.Text = frmBase.UIStrings.GetString("BtnSave");
            btnRestore.Text = frmBase.UIStrings.GetString("BtnUndo");
            cbViewResults.Text = frmBase.UIStrings.GetString("Results");

            lblNumSamples.Text = frmBase.UIStrings.GetString("Cal_NumSamples");
            rbLow.Text = frmBase.UIStrings.GetString("Cal_Light");
            rbMedium.Text = frmBase.UIStrings.GetString("Cal_Good");
            rbHigh.Text = frmBase.UIStrings.GetString("Cal_Dark");
            cbAcqCalibrationTurbidimeter.Text = frmBase.UIStrings.GetString("Cal_Start");

            ParameterEditingState = ParameterEditingStates.Reading;
            pgCameraParams.BeginEdit += new DataGridViewCellCancelEventHandler(pgCameraParams_BeginEdit);
            pgCameraParams.EndEdit += new DataGridViewCellCancelEventHandler(pgCameraParams_EndEdit);
            yesNoPanel.YesNoAnswer += yesNoPanel_YesNoAnswer;
            saveMenu.SaveMenuCondition += saveMenu_SaveMenuCondition;

            extGraphBox.ContextMenuBuilder += extGraphBox_ContextMenuBuilder;
            cbViewResults.Checked = false;
        }

        public DataViewer(VisionSystemManager visionSystemMgr, MachineConfiguration machineConfig, int cameraIndex)
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
            //timerCameraStatus.Start();
            //btnLiveStart.Enabled = _camera.ROIModeAvailable;

            // TODO: Rimuovere il test non appena risolto bug di Tattile
            //if (_camera.CameraType.Contains("M9"))
            //    btnLiveStartFF.Enabled = false;

            //pnlCameraCommand.Visible = false;
            //pnlCameraCommand.Visible = true;

            if (_camera.CameraType == "EoptisTurbidimeter")
            {
                calibrationTurbidimeter.Visible = true;
                GenerateGraph();

                int nSpindle = _machineConfig.NumberOfSpindles;
                int tmp = nSpindle / 3; //60spindles = 20, 36spindles = 12, 40spindles = 13

                turretLowLimit = tmp;
                turretHighLimit = tmp + tmp;

                nudNumbersOfItems.Maximum = tmp;

                if (_camera.eoptisPoints == null)
                    _camera.eoptisPoints = new List<ValuesTurbidimeter>();



                //PRESUMO CHE EoptisStationBase SIANO TUTTI Turbidimetri. Taccon temporaneo 
                if (_visionSystemMgr.Stations.Count(x => x.ProviderName == "EoptisStationBase") == 2)
                {
                    if (_camera.Spindle % 2 == 0)
                        spindleStart = 2;
                    else
                        spindleStart = 1;
                }
                else
                {
                    spindleStart = 1;
                    increment = 1;
                    nudNumbersOfItems.Minimum = 5;
                    nudNumbersOfItems.Increment = 1;
                }

                GenerateCalibrationRows();

                if (AppEngine.Current.CurrentContext.UserLevel < UserLevelEnum.Engineer)
                    calibrationTurbidimeter.Enabled = false;
                if (AppEngine.Current.CurrentContext.UserLevel == UserLevelEnum.Optrel)
                {
                    rbLow.Visible = true;
                    rbMedium.Visible = true;
                    rbHigh.Visible = true;
                }
            }

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
            timerValuesTurbidimeter.Stop();
            yesNoPanel.YesNoAnswer -= yesNoPanel_YesNoAnswer;
            saveMenu.SaveMenuCondition -= saveMenu_SaveMenuCondition;
            pgCameraParams.BeginEdit -= pgCameraParams_BeginEdit;
            pgCameraParams.EndEdit -= pgCameraParams_EndEdit;
            extGraphBox.ContextMenuBuilder -= extGraphBox_ContextMenuBuilder;
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
                Log.Line(LogLevels.Error, "DataViewer.getAcquisitionParam", _camera.IP4Address + ": Get acquisition parameters failed. Error: " + ex.Message);
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
                Log.Line(LogLevels.Error, "DataViewer.getElaborationParameter", _camera.IP4Address + ": Get elaboration parameters failed. Error: " + ex.Message);
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
                Log.Line(LogLevels.Error, "DataViewer.getMachineParameter", _camera.IP4Address + ": Get machine parameters failed. Error: " + ex.Message);
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

            if (_camera.CameraType == "EoptisTurbidimeter")
            {
                if (AppEngine.Current.CurrentContext.UserLevel < UserLevelEnum.Engineer)
                    calibrationTurbidimeter.Enabled = false;
                else
                    calibrationTurbidimeter.Enabled = true;
                if (AppEngine.Current.CurrentContext.UserLevel == UserLevelEnum.Optrel || AppEngine.Current.CurrentContext.UserLevel == UserLevelEnum.Administrator)
                {
                    rbLow.Visible = true;
                    rbMedium.Visible = true;
                    rbHigh.Visible = true;
                    btnImport.Visible = true;
                    btnExport.Visible = true;

                }
                else
                {
                    rbLow.Visible = false;
                    rbMedium.Visible = false;
                    rbHigh.Visible = false;
                    btnImport.Visible = false;
                    btnExport.Visible = false;
                }
            }

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
                    btnSaveData.Enabled = false;
                    cbOfflineMode.Enabled = false;
                    //calibrationTurbidimeter.Enabled = false;
                    extGraphBox.IsShowContextMenu = false;
                    extGraphBox.IsShowPointValues = true;  // false se si vuole disabilitare visualizzazione valori a runtime

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
                    btnSaveData.Enabled = true;
                    cbOfflineMode.Enabled = true;
                    //calibrationTurbidimeter.Enabled = false;
                    extGraphBox.IsShowContextMenu = true;
                    extGraphBox.IsShowPointValues = true;
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
                Log.Line(LogLevels.Error, "DataViewer.loadParameters", _node.Address + ": GetParameters failed! Error: " + ex.Message);
                setCameraWarning(string.Format(_node.Address + ": " + frmBase.UIStrings.GetString("GetParametersFailed")));
            }
            foreach (Control c in pnlParamSelector.Controls) {
                if (c is Button)
                    c.BackColor = Color.FromArgb(60, 60, 60);
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
                btnCamera.BackColor = Color.FromArgb(28, 28, 28);
                }
            if (currentParameterType == ParameterTypeEnum.Digitizer) {
                btnDigitizer.BackColor = Color.FromArgb(28, 28, 28);
                }
            if (currentParameterType == ParameterTypeEnum.Elaboration) {
                btnElaboration.BackColor = Color.FromArgb(28, 28, 28);
                }
            if (currentParameterType == ParameterTypeEnum.RecipeAdvanced) {
                btnAdvanced.BackColor = Color.FromArgb(28, 28, 28);
                }
            if (currentParameterType == ParameterTypeEnum.ROI) {
                btnROI.BackColor = Color.FromArgb(28, 28, 28);
                    if (roiPageLabel.ContainsKey(roiIndex))
                    pgCameraParams.Title = roiPageLabel[roiIndex] + " (" + (roiIndex + 1).ToString() + "/" + DataSource.ROIParameters.Count.ToString() + ")";
                else
                    pgCameraParams.Title += " (" + (roiIndex + 1).ToString() + "/" + DataSource.ROIParameters.Count.ToString() + ")";
            }
            if (currentParameterType == ParameterTypeEnum.Machine) {
                btnMachine.BackColor = Color.FromArgb(28, 28, 28);
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
                Log.Line(LogLevels.Warning, "DataViewer.btnApply_Click", _camera.IP4Address + ": Cannot apply parameters, machine mode: " + AppEngine.Current.CurrentContext.MachineMode.ToString());
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
                Log.Line(LogLevels.Error, "DataViewer.applyParameters", _camera.IP4Address + ": Error while sending parameters: " + ex.Message);
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
                    Log.Line(LogLevels.Error, "DataViewer.yesNoPanel_YesNoAnswer", _camera.IP4Address + ": Error while uploading parameters: " + ex.Message);
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
            toSave *= 2;
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

            string cultureCode = AppEngine.Current.CurrentContext.CultureCode;
            ParameterInfoCollection pDict = AppEngine.Current.ParametersInfo;
            NodeRecipe nr = (NodeRecipe)recipe;
            int idCamera;
            if (!int.TryParse(g.Name.Substring("Cam_".Length), out idCamera))
                return;
            Camera c = (Camera)_visionSystemMgr.Cameras[idCamera];
            int station = c.StationId;
            int node = c.NodeId;
            //int head = c.Head;
            if (nr == null) {
                Log.Line(LogLevels.Warning, "DataViewer.CreateGraph", "Wrong node recipe to create graphs");
                return;
            }
            StationRecipe sr = nr.Stations.Find(ss => ss.Id == station);
            CameraRecipe cr = sr.Cameras.First();
            if (sr == null) {
                Log.Line(LogLevels.Warning, "DataViewer.CreateGraph", "Wrong station recipe to create graphs");
                return;
            }
            foreach (Tool tool in sr.Tools) {
                string yAxisLabel = "";
                foreach (ToolOutput to in tool.ToolOutputs) {
                    if (to.EnableGraph) {
                        string to_key = to.Label.Replace("$", "");
                        bool isLocalized = (pDict[to_key] != null) && (pDict[to_key].LocalizedInfo != null) && (pDict[to_key].LocalizedInfo[cultureCode] != null) && (string.IsNullOrEmpty(pDict[to_key].LocalizedInfo[cultureCode].Label) == false);
                        string toolOutputName = (isLocalized == false) ? to.Label : pDict[to_key].LocalizedInfo[cultureCode].Label;
                        yAxisLabel += toolOutputName + " " + to.MeasureUnit + "/";
                        string chartLabel = nodeId + "_" + stationId + "_" + to.Id;
                        g.AddChart("", chartLabel, Color.Blue, ZedGraph.SymbolType.Circle);
                    }
                }
                yAxisLabel = yAxisLabel.TrimEnd(new char[] { '/' });
                g.SetTitles("", sr.Description, "Spindle", yAxisLabel, 0, _machineConfig.NumberOfSpindles + 1, 0, 10);
                //g.DataToShowMax = 2 * _machineConfig.NumberOfSpindles;    //parametrizzare?
                //g.OldDataToRemove = _machineConfig.NumberOfSpindles;    //parametrizzare?
            }
        }

        void drawLimits() {

            //ora setto le soglie sul grafico...andrebbero messe nei ToolOutput invece che nei SimpleParameters...
            Graph g = _visionSystemMgr.Graphs["Cam_" + Settings.Id];
            if ((g != null) && (NodeDataSource != null)) {
                CameraRecipe cr = NodeDataSource.Stations[0].Cameras.First();
                if (cr != null) {
                    string chartLabel = "";
                    List<Parameter> thresholds = cr.RecipeSimpleParameters.FindAll(pp => pp.Id.Contains("TH_"));
                    foreach (Parameter threshold in thresholds) {
                        if (threshold.Id != null) {
                            chartLabel = nodeId + "_" + stationId + "_" + threshold.Id;
                            g.RemoveChart("", chartLabel);
                            if (threshold.Id.Contains("WARNING")) {
                                //scommentare se si vogliono mostrare anche i warning...
                                //g.SetLimit(threshold.Id, LimitType.yLimit, Convert.ToDouble(threshold.Value), Color.Orange, System.Drawing.Drawing2D.DashStyle.Dash);
                            }
                            else if (threshold.Id.Contains("ERROR")) {
                                g.SetLimit("", chartLabel, LimitType.yLimit, Convert.ToDouble(threshold.Value, CultureInfo.InvariantCulture), -1, -1, Color.Red, System.Drawing.Drawing2D.DashStyle.Solid);
                            }
                            else
                                Log.Line(LogLevels.Warning, "DataViewer.drawLimits", "Threshold label unknown");
                        }
                    }
                }
            }
        }

        void extGraphBox_ContextMenuBuilder(ZedGraphControl sender, ContextMenuStrip menuStrip, Point mousePt, ZedGraphControl.ContextMenuObjectState objState) {

            menuStrip.Items.RemoveAt(7);
        }


        private void btnUpload_Click(object sender, EventArgs e) {

            pnlUp.Visible = false;
            yesNoPanel.Tag = "btnUpload";
            yesNoPanel.Visible = true;
        }

        private void cbOfflineMode_CheckedChanged(object sender, EventArgs e) {

            if (cbOfflineMode.Checked == true) {
                Camera.StartAnalysisOffline();
            } else {
                Camera.StopAnalysisOffline();
            }
            cbOfflineMode.Image = (cbOfflineMode.Checked == true) ? global::ExactaEasy.Properties.Resources.application_x_trash_red : global::ExactaEasy.Properties.Resources.application_x_trash;
        }

        private void btnSaveData_Click(object sender, EventArgs e) {

            pnlUp.Visible = false;
            saveMenu.Tag = "btnSaveData";
            saveMenu.UncheckAll();
            saveMenu.Visible = true;
        }







        private void calibrationTurbidimeter_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Control cntr in pnlCenter.Controls)
            {
                if (cntr.Name != "tlpCalibrationTurbidimeter")
                    cntr.Visible = !calibrationTurbidimeter.Checked;
                else
                    tlpCalibrationTurbidimeter.Visible = calibrationTurbidimeter.Checked;
            }
        }

        private void GenerateCalibrationRows()
        {
            //PRESUMO CHE EoptisStationBase SIANO TUTTI Turbidimetri. Taccon temporaneo 

            if (_visionSystemMgr.Stations.Count(x=> x.ProviderName == "EoptisStationBase") == 1)
            {
                for (int j = 1; j <= _machineConfig.NumberOfSpindles; j = j + 1)
                {
                    dgvCalibrationTurbidimeter.Rows.Add(j.ToString());
                }
            }
            else if (_visionSystemMgr.Stations.Count(x => x.ProviderName == "EoptisStationBase") == 2)
            {
                for (int j = spindleStart; j <= _machineConfig.NumberOfSpindles; j = j + 2)
                {
                    dgvCalibrationTurbidimeter.Rows.Add(j.ToString());
                }
            }


        }

        private void HideRowsUnused()
        {
            if (_visionSystemMgr.Stations.Count(x => x.ProviderName == "EoptisStationBase") == 1)
            {
                for (int j = 1; j <= _machineConfig.NumberOfSpindles; j = j + 1)
                {
                    if (j <= turretLowLimit) //PRIMO SET
                    {
                        if (j > numSamples)
                            dgvCalibrationTurbidimeter.Rows[j - 1].DefaultCellStyle.BackColor = Color.Gray;
                        else
                        {
                            dgvCalibrationTurbidimeter.Rows[j - 1].DefaultCellStyle.BackColor = SystemColors.Window;
                            dgvCalibrationTurbidimeter.Rows[j - 1].Cells[0].Style.BackColor = SystemColors.ControlLight;
                            dgvCalibrationTurbidimeter.Rows[j - 1].Cells[11].Style.BackColor = SystemColors.ControlLight;
                        }

                    }
                    else if (j >= turretLowLimit + 1 && j <= turretHighLimit)
                    {
                        if (j > numSamples + turretLowLimit)
                            dgvCalibrationTurbidimeter.Rows[j - 1].DefaultCellStyle.BackColor = Color.Gray;
                        else
                        {
                            dgvCalibrationTurbidimeter.Rows[j - 1].DefaultCellStyle.BackColor = SystemColors.Window;
                            dgvCalibrationTurbidimeter.Rows[j - 1].Cells[0].Style.BackColor = SystemColors.ControlLight;
                            dgvCalibrationTurbidimeter.Rows[j - 1].Cells[11].Style.BackColor = SystemColors.ControlLight;
                        }
                    }
                    else if (j >= turretHighLimit + 1 && j <= _machineConfig.NumberOfSpindles)
                    {
                        if (j > numSamples + turretHighLimit)
                            dgvCalibrationTurbidimeter.Rows[j - 1].DefaultCellStyle.BackColor = Color.Gray;
                        else
                        {
                            dgvCalibrationTurbidimeter.Rows[j - 1].DefaultCellStyle.BackColor = SystemColors.Window;
                            dgvCalibrationTurbidimeter.Rows[j - 1].Cells[0].Style.BackColor = SystemColors.ControlLight;
                            dgvCalibrationTurbidimeter.Rows[j - 1].Cells[11].Style.BackColor = SystemColors.ControlLight;
                        }
                    }
                    
                }
            }
            else if (_visionSystemMgr.Stations.Count(x => x.ProviderName == "EoptisStationBase") == 2)
            {
                for (int j = 1; j <= (_machineConfig.NumberOfSpindles/2); j = j + 1)
                {
                    if (j <= (turretLowLimit/2)) //PRIMO SET
                    {
                        if (j > (numSamples / 2))
                            dgvCalibrationTurbidimeter.Rows[j - 1].DefaultCellStyle.BackColor = Color.Gray;
                        else
                        {
                            dgvCalibrationTurbidimeter.Rows[j - 1].DefaultCellStyle.BackColor = SystemColors.Window;
                            dgvCalibrationTurbidimeter.Rows[j - 1].Cells[0].Style.BackColor = SystemColors.ControlLight;
                            dgvCalibrationTurbidimeter.Rows[j - 1].Cells[11].Style.BackColor = SystemColors.ControlLight;
                        }

                    }
                    else if (j >= (turretLowLimit/2) + 1 && j <= (turretHighLimit/2))
                    {
                        if (j > (numSamples / 2) + (turretLowLimit/2))
                            dgvCalibrationTurbidimeter.Rows[j - 1].DefaultCellStyle.BackColor = Color.Gray;
                        else
                        {
                            dgvCalibrationTurbidimeter.Rows[j - 1].DefaultCellStyle.BackColor = SystemColors.Window;
                            dgvCalibrationTurbidimeter.Rows[j - 1].Cells[0].Style.BackColor = SystemColors.ControlLight;
                            dgvCalibrationTurbidimeter.Rows[j - 1].Cells[11].Style.BackColor = SystemColors.ControlLight;
                        }
                    }
                    else if (j >= (turretHighLimit/2) + 1 && j <= (_machineConfig.NumberOfSpindles/2))
                    {
                        if (j > (numSamples / 2) + (turretHighLimit/2))
                            dgvCalibrationTurbidimeter.Rows[j - 1].DefaultCellStyle.BackColor = Color.Gray;
                        else
                        {
                            dgvCalibrationTurbidimeter.Rows[j - 1].DefaultCellStyle.BackColor = SystemColors.Window;
                            dgvCalibrationTurbidimeter.Rows[j - 1].Cells[0].Style.BackColor = SystemColors.ControlLight;
                            dgvCalibrationTurbidimeter.Rows[j - 1].Cells[11].Style.BackColor = SystemColors.ControlLight;
                        }
                    }

                }
            }
        }

        int expectedTot = 0; int partialTot = 0;
        int numSamples;
        private void cbAcqCalibrationTurbidimeter_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAcqCalibrationTurbidimeter.Checked == true)
            {
                //Camera.StartAnalysisOffline();
                dgvCalibrationTurbidimeter.Enabled = true;
                flowLayoutPanel1.Enabled = false;
                //cbSimulation.Enabled = true;

                if(_camera.eoptisPoints != null)
                {
                    _camera.eoptisPoints.Clear();
                    dgvCalibrationTurbidimeter.Rows.Clear();
                    GenerateCalibrationRows();
                }

                int nSpindle = _machineConfig.NumberOfSpindles;
                int tmp = nSpindle / 3; //60spindles = 20, 36spindles = 12, 40spindles = 13
                turretLowLimit = tmp;
                turretHighLimit = tmp + tmp;


                numSamples = (int)nudNumbersOfItems.Value;
                expectedTot = 10 * (numSamples * 3); //10 giri * (numero campioni nel set * 3 Set)
                if (_visionSystemMgr.Stations.Count(x => x.ProviderName == "EoptisStationBase") == 2)
                    expectedTot = 10 * ((numSamples/2) * 3); //10 giri * ((numero campioni nel set / 2) * 3 Set)
                partialTot = 0;
                nudNumbersOfItems.Enabled = false;
                HideRowsUnused();
                lblSuggestedLimits.Text = rbLow.Text + " : From 1 to " + numSamples + Environment.NewLine + rbMedium.Text + " : From " + (turretLowLimit + 1) + " to " + (turretLowLimit + numSamples) + Environment.NewLine + rbHigh.Text + " : From " + (turretHighLimit + 1) + " to " + (turretHighLimit + numSamples);
                btnImport.Enabled = true;
                btnExport.Enabled = false;
                
                timerValuesTurbidimeter.Start();
            }
            else
            {
                if (expectedTot == partialTot)
                {
                    flowLayoutPanel1.Enabled = true;
                    foreach (DataGridViewRow dgvr in dgvCalibrationTurbidimeter.Rows)
                    {
                        if(dgvr.DefaultCellStyle.BackColor != Color.Gray)
                            dgvr.Cells[11].Value = Math.Round(((Convert.ToDouble(dgvr.Cells[1].Value) +
                                Convert.ToDouble(dgvr.Cells[2].Value) +
                                Convert.ToDouble(dgvr.Cells[3].Value) +
                                Convert.ToDouble(dgvr.Cells[4].Value) +
                                Convert.ToDouble(dgvr.Cells[5].Value) +
                                Convert.ToDouble(dgvr.Cells[6].Value) +
                                Convert.ToDouble(dgvr.Cells[7].Value) +
                                Convert.ToDouble(dgvr.Cells[8].Value) +
                                Convert.ToDouble(dgvr.Cells[9].Value) +
                                Convert.ToDouble(dgvr.Cells[10].Value)) / 10), 6);
                    }
                    //Camera.StopAnalysisOffline();
                    //dgvCalibrationTurbidimeter.Enabled = false;
                    //cbSimulation.Enabled = false;
                    rbAllStdDev.Checked = false;
                    rbAllStdDev.Checked = true;
                    //lblSuggestedLimits.Text = "Threshold Height: " + limitLow + Environment.NewLine + "Threshold Low: " + limitHigh; //INVERTITI ED È GIUSTO COSI 
                }
                else
                    setCameraWarning(string.Format(_node.Address + ": Acquisition not completed (" + partialTot + " / " + expectedTot + ")"));


                btnImport.Enabled = false;
                btnExport.Enabled = true;
                nudNumbersOfItems.Enabled = true;
                timerValuesTurbidimeter.Stop();
            }
            cbAcqCalibrationTurbidimeter.Text = (cbAcqCalibrationTurbidimeter.Checked == true) ? frmBase.UIStrings.GetString("Cal_Stop") : frmBase.UIStrings.GetString("Cal_Start"); 
        }

        public void GenerateGraph()
        {
            zgCalibration.GraphPane.Title.IsVisible = false;
            zgCalibration.GraphPane.Legend.IsVisible = false;

            zgCalibration.GraphPane.XAxis.Title.Text = "MEASURE";
            zgCalibration.GraphPane.YAxis.Title.Text = "N°";

            zgCalibration.GraphPane.YAxis.Scale.FontSpec.FontColor = Color.White;
            zgCalibration.GraphPane.XAxis.Scale.FontSpec.FontColor = Color.White;
            zgCalibration.GraphPane.YAxis.Scale.FontSpec.Size = 8f;
            zgCalibration.GraphPane.XAxis.Scale.FontSpec.Size = 8f;
            zgCalibration.GraphPane.YAxis.Title.FontSpec.FontColor = Color.White;
            zgCalibration.GraphPane.XAxis.Title.FontSpec.FontColor = Color.White;

            zgCalibration.GraphPane.YAxis.Scale.MajorStepAuto = true;
            zgCalibration.GraphPane.XAxis.Scale.MajorStepAuto = true;
            zgCalibration.GraphPane.XAxis.Scale.MinorStep = 0.1;
            zgCalibration.GraphPane.YAxis.Scale.MinorStep = 0.1;


            zgCalibration.GraphPane.YAxis.MajorGrid.IsVisible = true;
            zgCalibration.GraphPane.XAxis.MajorGrid.IsVisible = true;
            zgCalibration.GraphPane.YAxis.MajorGrid.DashOff = 0.5f;
            zgCalibration.GraphPane.XAxis.MajorGrid.DashOff = 0.5f;
            zgCalibration.GraphPane.YAxis.MajorGrid.Color = Color.FromArgb(55, 55, 56);
            zgCalibration.GraphPane.XAxis.MajorGrid.Color = Color.FromArgb(55, 55, 56);


            //Color
            zgCalibration.GraphPane.Fill = new Fill(Color.FromArgb(55, 55, 56));
            zgCalibration.GraphPane.Chart.Fill = new Fill(Color.FromArgb(31, 31, 32));

            zgCalibration.GraphPane.YAxis.Scale.FontSpec.FontColor = Color.White;
            zgCalibration.GraphPane.XAxis.Scale.FontSpec.FontColor = Color.White;
            zgCalibration.GraphPane.YAxis.Title.FontSpec.FontColor = Color.White;
            zgCalibration.GraphPane.XAxis.Title.FontSpec.FontColor = Color.White;

            zgCalibration.AxisChange();
        }

        public void UpdateGraph(bool isHisto,int category)
        {
            zgCalibration.GraphPane.CurveList.Clear();
            zgCalibration.GraphPane.GraphObjList.Clear();
            if(isHisto) //-------- HISTO ------------
            {
                PointPairList ppl = new PointPairList();
                double minimum = 0, maximum = 0;
                foreach (DataGridViewRow dgvr in dgvCalibrationTurbidimeter.Rows)
                {
                    if (dgvr.DefaultCellStyle.BackColor != Color.Gray)
                    {
                        int y = 1;
                        double x = Convert.ToDouble(dgvr.Cells[11].Value);
                        if (x != 0)
                        {
                            if (ppl.FindAll(s => s.X == x).Count == 0)
                            {
                                ppl.Add(x, y);
                                //PaintValue(y, x);
                            }
                            else
                            {
                                PointPair tmpY = ppl.Find(z => z.X == x);
                                ppl.Add(x, tmpY.Y + 1);
                                ppl.Remove(tmpY);
                                //PaintValue(y+1, x);
                            }

                            if (dgvr.Index == 0)
                                minimum = maximum = x;

                            if (x < minimum)
                                minimum = x;

                            if (x > maximum)
                                maximum = x;
                        }
                    }

                }


                zgCalibration.GraphPane.BarSettings.ClusterScaleWidth = 0.05;

                BarItem myCurve2 = new BarItem("Area", ppl, Color.Green);
                myCurve2.Bar.Border.IsVisible = false;
                myCurve2.Bar.Fill.Color = Color.Green;
                zgCalibration.GraphPane.CurveList.Add(myCurve2);

                zgCalibration.GraphPane.YAxis.Scale.MajorStepAuto = true;
                zgCalibration.GraphPane.XAxis.Scale.MajorStepAuto = true;
                zgCalibration.GraphPane.XAxis.Scale.MinorStep = 0.2;
                zgCalibration.GraphPane.YAxis.Scale.MinorStep = 1;
                zgCalibration.GraphPane.XAxis.Scale.Min = minimum - 0.2;
                zgCalibration.GraphPane.XAxis.Scale.Max = maximum + 0.2;

                LimitsToSet();
            }
            else //--------- GAUSS -----------
            {
                if(category==0)
                {
                    PaintGauss(1, turretLowLimit, Color.Red,1);
                    PaintGauss(turretLowLimit +1, turretHighLimit, Color.Green,2);
                    PaintGauss(turretHighLimit +1, _machineConfig.NumberOfSpindles, Color.Blue,3);

                    zgCalibration.GraphPane.YAxis.Scale.MajorStepAuto = true;
                    //zgCalibration.GraphPane.YAxis.Scale.MinorStep = 0.2;
                    //zgCalibration.GraphPane.YAxis.Scale.MajorStep = 5;
                    zgCalibration.GraphPane.XAxis.Scale.MajorStepAuto = true;
                    //zgCalibration.GraphPane.XAxis.Scale.MinorStep = 0.1;
                    //zgCalibration.GraphPane.YAxis.Scale.MinorStep = 0.1;

                    LimitsToSet();
                }
                else if(category==1)
                {
                    PaintGauss(1, turretLowLimit, Color.Red,1);
                    zgCalibration.GraphPane.YAxis.Scale.MaxAuto = true;
                    zgCalibration.GraphPane.XAxis.Scale.MaxAuto = true;
                    zgCalibration.GraphPane.YAxis.Scale.MinAuto = true;
                    zgCalibration.GraphPane.XAxis.Scale.MinAuto = true;
                }
                else if (category == 2)
                {
                    PaintGauss(turretLowLimit+1, turretHighLimit, Color.Green,2);
                    zgCalibration.GraphPane.YAxis.Scale.MaxAuto = true;
                    zgCalibration.GraphPane.XAxis.Scale.MaxAuto = true;
                    zgCalibration.GraphPane.YAxis.Scale.MinAuto = true;
                    zgCalibration.GraphPane.XAxis.Scale.MinAuto = true;
                }
                else if (category == 3)
                {
                    PaintGauss(turretHighLimit+1, _machineConfig.NumberOfSpindles, Color.Blue, 3);
                    zgCalibration.GraphPane.YAxis.Scale.MaxAuto = true;
                    zgCalibration.GraphPane.XAxis.Scale.MaxAuto = true;
                    zgCalibration.GraphPane.YAxis.Scale.MinAuto = true;
                    zgCalibration.GraphPane.XAxis.Scale.MinAuto = true;
                }
            }


            zgCalibration.AxisChange();
            zgCalibration.Invalidate();
        }

        private void PaintValue(int y, double x)
        {
            double heightY = y + 0.2;

            if (y == 0)
                return;

            TextObj text = new TextObj(x.ToString(), x, heightY, CoordType.AxisXYScale, AlignH.Center, AlignV.Center);
            text.FontSpec.Border.IsVisible = false;
            text.FontSpec.Size = 10f;
            text.FontSpec.FontColor = Color.White;
            text.FontSpec.Fill.Color = Color.Transparent;
            text.IsClippedToChartRect = true;
            zgCalibration.GraphPane.GraphObjList.Add(text);
        }

        double distrNormLightMin, distrNormGoodMin, distrNormGoodMax, distrNormDarkMax;

        private void PaintGauss(int min, int max,Color clr, int type)
        {
            List<double> tmp = new List<double>();
            foreach (DataGridViewRow dgvr in dgvCalibrationTurbidimeter.Rows)
            {
                if (Convert.ToInt32(dgvr.Cells[0].Value) >= min && Convert.ToInt32(dgvr.Cells[0].Value) <= max)
                    if (dgvr.DefaultCellStyle.BackColor != Color.Gray)
                        if (Convert.ToDouble(dgvr.Cells[11].Value)!=0)
                            tmp.Add(Convert.ToDouble(dgvr.Cells[11].Value));
            }
            tmp.Sort();
            PointPairList pplAll = new PointPairList();
            double avg = tmp.Average();
            double stdDev = StandardDeviation(tmp, avg);
            foreach (double item in tmp)
            {
                //double yPoint = NormalDistr(item, 0.15915494309, avg, stdDev, (stdDev * stdDev));
                double yPoint = NormalDistr(item, avg, stdDev, (stdDev * stdDev));
                pplAll.Add(item, yPoint);
            }

            LineItem curve = new LineItem("", pplAll, clr, SymbolType.Circle);
            curve.Symbol.Size = 10;
            zgCalibration.GraphPane.CurveList.Add(curve);


            //LineItem curve = zgCalibration.GraphPane.AddCurve("", pplAll, clr, SymbolType.Circle);

            //STD DEV LIMITS
            double limitMin = avg - stdDev;
            double limitMax = avg + stdDev;

            //LineObj threshHoldMin = new LineObj(clr, limitMin, 0, limitMin, 10);
            //threshHoldMin.IsClippedToChartRect = true;
            //zgCalibration.GraphPane.GraphObjList.Add(threshHoldMin);
            //LineObj threshHoldMax = new LineObj(clr, limitMax, 0, limitMax, 10);
            //threshHoldMax.IsClippedToChartRect = true;
            //zgCalibration.GraphPane.GraphObjList.Add(threshHoldMax);

            //AREA PAINT
            PointPairList pplNew = new PointPairList();
            foreach (PointPair pp in pplAll)
            {
                if (pp.X >= limitMin && pp.X <= limitMax)
                    pplNew.Add(pp);
            }

            pplNew.Add(limitMin, pplAll.InterpolateX(limitMin));
            pplNew.Add(limitMax, pplAll.InterpolateX(limitMax));
            pplNew.Sort();
            LineItem myCurve = zgCalibration.GraphPane.AddCurve("", pplNew, clr, SymbolType.None);
            myCurve.Line.Fill = new Fill(Color.FromArgb(55, 55, 56));

            if(type == 1)
            {
                distrNormLightMin = limitMin;
            }
            else if (type == 2)
            {
                distrNormGoodMin = limitMin;
                distrNormGoodMax = limitMax;
            }
            else if (type == 3)
            {
                distrNormDarkMax = limitMax;
            }
        }

        double limitLow=0; double limitHigh=0;

        private void LimitsToSet()
        {
            List<double> tmp = new List<double>();
            tmp.Clear();
            //---- LOW ---
            foreach (DataGridViewRow dgvr in dgvCalibrationTurbidimeter.Rows)
            {
                if (dgvr.DefaultCellStyle.BackColor != Color.Gray)
                    if (Convert.ToInt32(dgvr.Cells[0].Value) >= 0 && Convert.ToInt32(dgvr.Cells[0].Value) <= turretLowLimit)
                        if (Convert.ToDouble(dgvr.Cells[11].Value) != 0)
                            tmp.Add(Convert.ToDouble(dgvr.Cells[11].Value));
            }
            tmp.Sort();

            double avgLow = tmp.Average();
            double stdDevLow = StandardDeviation(tmp, avgLow);
            tmp.Clear();


            //---- MEDIUM ---
            foreach (DataGridViewRow dgvr in dgvCalibrationTurbidimeter.Rows)
            {
                if (dgvr.DefaultCellStyle.BackColor != Color.Gray)
                    if (Convert.ToInt32(dgvr.Cells[0].Value) >= turretLowLimit+1 && Convert.ToInt32(dgvr.Cells[0].Value) <= turretHighLimit)
                        if (Convert.ToDouble(dgvr.Cells[11].Value) != 0)
                            tmp.Add(Convert.ToDouble(dgvr.Cells[11].Value));
            }
            tmp.Sort();

            double avgMedium= tmp.Average();
            double stdDevMedium = StandardDeviation(tmp, avgMedium);
            tmp.Clear();


            //----  ---
            foreach (DataGridViewRow dgvr in dgvCalibrationTurbidimeter.Rows)
            {
                if (dgvr.DefaultCellStyle.BackColor != Color.Gray)
                    if (Convert.ToInt32(dgvr.Cells[0].Value) >= turretHighLimit+1 && Convert.ToInt32(dgvr.Cells[0].Value) <= _machineConfig.NumberOfSpindles)
                        if (Convert.ToDouble(dgvr.Cells[11].Value) != 0)
                            tmp.Add(Convert.ToDouble(dgvr.Cells[11].Value));
            }
            tmp.Sort();

            double avgHigh = tmp.Average();
            double stdDevHigh = StandardDeviation(tmp, avgHigh);


            //---- LIMITS ----

            limitHigh = (distrNormLightMin + distrNormGoodMax) / 2;
            limitLow = (distrNormGoodMin + distrNormDarkMax) / 2;
            //limitLow = ((avgLow + stdDevLow) + (avgMedium - stdDevMedium)) / 2;
            //limitHigh = ((avgMedium + stdDevMedium) + (avgHigh - stdDevHigh)) / 2;

            LineObj Min = new LineObj(Color.Yellow, limitLow, 0, limitLow, 1000);
            Min.IsClippedToChartRect = true;
            zgCalibration.GraphPane.GraphObjList.Add(Min);
            LineObj Max = new LineObj(Color.Yellow, limitHigh, 0, limitHigh, 1000);
            Max.IsClippedToChartRect = true;
            zgCalibration.GraphPane.GraphObjList.Add(Max);


            lblSuggestedLimits.Text = "Threshold High: " + limitHigh + Environment.NewLine + "Threshold Low: " + limitLow; 
        }

        public double StandardDeviation(IEnumerable<double> values, double avg)
        {
            return Math.Sqrt(values.Average(v => Math.Pow(v - avg, 2)));
        }

        // The normal distribution function.
        private float NormalDistr(double x,  double mean, double stddev, double var)
        {
            double one_over_2pi = (float)(1.0 / (stddev * Math.Sqrt(2 * Math.PI)));
            return (float)(one_over_2pi * Math.Exp(-(x - mean) * (x - mean) / (2 * var)));
        }


        private void rb_CheckedChanged(object sender, EventArgs e)
        {
            if(listValuesTB.Count != 0 || _camera.eoptisPoints != null) //POI RIMANE SOLO eoptispoint
            {
                if (rbAllStdDev.Checked)
                    UpdateGraph(false, 0);
                else if (rbHisto.Checked)
                    UpdateGraph(true, 0);
                else if (rbLow.Checked)
                    UpdateGraph(false, 1);
                else if (rbMedium.Checked)
                    UpdateGraph(false, 2);
                else if (rbHigh.Checked)
                    UpdateGraph(false, 3);
            }

        }

        int countMeasure = 0;

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = "Table";
            saveFileDialog1.Filter = "CSV File|*.csv";
            saveFileDialog1.Title = "Save Table";
            DialogResult dr = saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "" && dr == DialogResult.OK)
            {
                var sb = new StringBuilder();

                var headers = dgvCalibrationTurbidimeter.Columns.Cast<DataGridViewColumn>();
                sb.AppendLine(string.Join(";", headers.Select(column => " " + column.HeaderText + " ").ToArray()));

                foreach (DataGridViewRow row in dgvCalibrationTurbidimeter.Rows)
                {
                    string rowToAppend = "";
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value != null)
                            rowToAppend = rowToAppend + cell.Value.ToString().TrimEnd().TrimStart() + ";";
                        else
                            rowToAppend = rowToAppend + cell.Value + ";";

                    }
                    sb.AppendLine(rowToAppend);

                    //var cells = row.Cells.Cast<DataGridViewCell>();
                    //sb.AppendLine(string.Join(";", cells.Select(cell =>cell.Value).ToArray()));
                }

                StreamWriter swOut = new StreamWriter(saveFileDialog1.FileName);
                swOut.Write(sb);
                swOut.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "CSV files (*.csv)|*.csv";
            openFileDialog1.Title = "IMPORT table";

            if (openFileDialog1.ShowDialog() == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;

                string[] lines = System.IO.File.ReadAllLines(file);
                if (lines.Length > 0)
                {
                    //first line to create header
                    string firstLine = lines[0];
                    string[] headerLabels = firstLine.Split(';');
                    //For Data
                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] dataWords = lines[i].Split(';');
                        int columnIndex = 0;
                        foreach (string headerWord in headerLabels)
                        {
                            dgvCalibrationTurbidimeter.Rows[i-1].Cells[columnIndex].Value = dataWords[columnIndex];
                            columnIndex++;
                        }
                    }
                }

                partialTot = expectedTot;
            }
        }

        private void timerValuesTurbidimeter_Tick(object sender, EventArgs e)
        {
            if(_camera.eoptisPoints != null) {
                if(_camera.eoptisPoints.Count != countMeasure)
                {

                    for (int i = countMeasure; i< _camera.eoptisPoints.Count; i++)
                    {

                        int spindle = _camera.eoptisPoints[i].Spindle;
                        int turn = _camera.eoptisPoints[i].Turn;
                        double measure = _camera.eoptisPoints[i].Value;

                        int indexRow = Convert.ToInt32(Math.Floor((decimal)(spindle / increment)));
                        if (spindleStart == 2 || increment == 1)
                            indexRow = indexRow - 1;
                        if(indexRow< dgvCalibrationTurbidimeter.Rows.Count && turn <=10)
                        {
                            if (spindle <= turretLowLimit) //PRIMO SET
                            {
                                if (spindle <= numSamples)
                                {
                                    dgvCalibrationTurbidimeter.Rows[indexRow].Cells[turn].Value = measure;
                                    partialTot++;
                                }                                   
                            }
                            else if (spindle >= turretLowLimit + 1 && spindle <= turretHighLimit)
                            {
                                if (spindle <= numSamples + turretLowLimit)
                                {
                                    dgvCalibrationTurbidimeter.Rows[indexRow].Cells[turn].Value = measure;
                                    partialTot++;
                                }

                            }
                            else if(spindle >= turretHighLimit + 1 && spindle <= _machineConfig.NumberOfSpindles)
                            {
                                if (spindle <= numSamples + turretHighLimit)
                                {
                                    dgvCalibrationTurbidimeter.Rows[indexRow].Cells[turn].Value = measure;
                                    partialTot++;
                                }

                            }

                        }
                            
                    }
                    countMeasure = _camera.eoptisPoints.Count;
                }
            }
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
