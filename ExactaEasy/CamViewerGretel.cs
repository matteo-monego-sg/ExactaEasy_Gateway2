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
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using ZedGraph;

namespace ExactaEasy
{

    public partial class CamViewerGretel : UserControl, ICamViewer {

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

        CameraRecipe dataSource;
        //CameraRecipe originalDataSource;
        NodeRecipe nodeDataSource;
        //NodeRecipe originalNodeDataSource;

        ParameterTypeEnum currentParameterType = ParameterTypeEnum.Elaboration;
        ParameterCollection<Parameter> actualAcquisitionParams;
        ParameterCollection<Parameter> actualDigitizerParams;
        //ParameterCollection<Parameter> actualToolParams;
        //ParameterCollection<Parameter> actualRecipeAdvancedParams;
        //ParameterCollection<Parameter> actualROIParams;
        //ParameterCollection<Parameter> actualMachineParams;
        ParameterCollection<Parameter> toolsExported = new ParameterCollection<Parameter>();
        //ParameterCollection<Parameter> actualStroboParams;
        //Dictionary<int, string> roiPageLabel;
        VisionSystemManager _visionSystemMgr;
        MachineConfiguration _machineConfig;
        GridViewInspectionDisplay gviDisplay;

        int currToolIndex = 0;
        int nextToolIndex = 0;
        int lightIndex = 0;
        //int nextRoiIndex = 0;
        //bool isCameraOK = true;
        int stationId = -1;
        //int head = -1;
        int nodeId = -1;
        //int camViewerDebuggerOwner;

        Dictionary<ParameterTypeEnum, string> paramTitleLabel = new Dictionary<ParameterTypeEnum, string>() { 
            { ParameterTypeEnum.Acquisition, "" },     //frmBase.UIStrings.GetString("AcquisitionParameter") },
            { ParameterTypeEnum.Digitizer, "" },       //frmBase.UIStrings.GetString("Features") },
            { ParameterTypeEnum.Elaboration, "" },     //frmBase.UIStrings.GetString("RecipeParameter") },
            { ParameterTypeEnum.RecipeAdvanced, "" },  //frmBase.UIStrings.GetString("AdvancedRecipeParameter") },
            { ParameterTypeEnum.ROI, "" },             //frmBase.UIStrings.GetString("ROIDefinition") },
            { ParameterTypeEnum.Machine, "" },         //frmBase.UIStrings.GetString("MachineParameter") },
            { ParameterTypeEnum.Strobo, "" },          //frmBase.UIStrings.GetString("StroboParameter") }
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
                dataSource = value;
                //getROIPageLabel();
                //originalDataSource = dataSource.Clone(AppEngine.Current.ParametersInfo, AppEngine.Current.CurrentContext.CultureCode);
                currentParameterType = ParameterTypeEnum.Elaboration;
                loadParameters();
                setCameraStatus();
                //pcbLive.ImageAspectRatio = calcImageRatio();
            }
        }

        internal StationRecipe StationDataSource { get; private set; }

        NodeRecipe NodeDataSource {
            get {
                return nodeDataSource;
            }
            set {
                nodeDataSource = value;
                StationDataSource = NodeDataSource.Stations[_camera.StationId];
                DataSource = NodeDataSource.Stations[_camera.StationId].Cameras[0];
                populateToolList();
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

        CamViewerGretel() {
            InitializeComponent();

            btnApply.Text = frmBase.UIStrings.GetString("BtnApply");
            btnEdit.Text = frmBase.UIStrings.GetString("BtnEdit");
            btnSave.Text = frmBase.UIStrings.GetString("BtnSave");
            btnRestore.Text = frmBase.UIStrings.GetString("BtnUndo");
            cbViewResults.Text = frmBase.UIStrings.GetString("Results");
            //btnLiveStart.Text = frmBase.UIStrings.GetString("BtnLiveStart");
            //btnLiveStartFF.Text = frmBase.UIStrings.GetString("BtnLiveStartFF");
            btnRDP.Text = frmBase.UIStrings.GetString("BtnConfig");
            cbSaveResults.Text = frmBase.UIStrings.GetString("Dump");

            ParameterEditingState = ParameterEditingStates.Reading;
            pgCameraParams.BeginEdit += new DataGridViewCellCancelEventHandler(pgCameraParams_BeginEdit);
            pgCameraParams.EndEdit += new DataGridViewCellCancelEventHandler(pgCameraParams_EndEdit);
            pgCameraParams.ChangedParent += new DataGridViewCellEventHandler(pgCameraParams_ChangedParent);
            cbSaveResults.CheckedChanged += new EventHandler(cbSaveResults_CheckedChanged);
            cbViewResults.Checked = true;

            btnApply.Enabled = true;
        }

        public CamViewerGretel(VisionSystemManager visionSystemMgr, MachineConfiguration machineConfig, int cameraIndex)
            : this() {

            _visionSystemMgr = visionSystemMgr;
            _machineConfig = machineConfig;
            Settings = machineConfig.CameraSettings[cameraIndex];
            _camera = (Camera)_visionSystemMgr.Cameras[Settings.Id];
            _station = _visionSystemMgr.Nodes[_camera.NodeId].Stations[_camera.StationId];
            _node = _visionSystemMgr.Nodes[_camera.NodeId];
            stationId = _camera.StationId;
            nodeId = _camera.NodeId;
            _visionSystemMgr.Nodes[nodeId].NodeRecipeUpdate += CamViewerGretel_NodeRecipeUpdate;
            lblCameraDescription.Text = _camera.CameraDescription;
            //AppContext ctx = AppEngine.Current.CurrentContext;
            RefreshStationStatus();
            pcbLive.VisibleChanged += new EventHandler(pcbLive_VisibleChanged);
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
            gviDisplay = new GridViewInspectionDisplay("GridInspRes_" + Settings.Id.ToString(), resGrid, _visionSystemMgr.Nodes[nodeId].Stations[stationId], machineConfig.RejectionCauses, rsRecImages);
            visionSystemMgr.MeasuresContainer.Add(gviDisplay);

            _station.SetParametersCompleted += _station_SetParametersCompleted;
            _station.ExportedParametersUpdated += _station_ExportedParametersUpdated;
            //timerCameraStatus.Start();
            //btnLiveStart.Enabled = _camera.ROIModeAvailable;

            // TODO: Rimuovere il test non appena risolto bug di Tattile
            //if (_camera.CameraType.Contains("M9"))
            //    btnLiveStartFF.Enabled = false;

            pnlCameraCommand.Visible = false;
            pnlCameraCommand.Visible = true;

        }

        void _station_ExportedParametersUpdated(object sender, EventArgs e) {

            loadParameters();
            if (IsHandleCreated) {
                try {
                    RecalcButton();
                } catch { }
            }
        }

        void _station_SetParametersCompleted(object sender, DisplayManager.MessageEventArgs e) {

            setApplyEnabled(true);
            if (e.Message.Contains("SUCCESS") == true) {
                pgCameraParams.UpdateLogCtrl("PARAMETERS SET SUCCESSFULLY", Color.Green);
                AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.ReadyToRun);
            }
            else {
                pgCameraParams.UpdateLogCtrl("PARAMETERS SET FAILED: " + e.Message, Color.Red);
                AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Error);
            }
            loadParameters();
            RecalcButton();
            OnCamViewerMessage(this, new CamViewerMessageEventArgs("ResumeLayout"));
        }

        //~CamViewerGretel() {

        //    Debug.WriteLine(Name + " DISPOSED!");
        //}

        void CamViewerGretel_NodeRecipeUpdate(object sender, NodeRecipeEventArgs e) {

            //if (NodeDataSource != null) {
            //    gviDisplay.CurrRecipe = AppEngine.Current.CurrentContext.ActiveRecipe;
            //}
        }

        public void UpdateLogCtrl(string message, Color color) {

            throw new NotImplementedException();
        }

        static DataGridViewHeaderBorderStyle properColumnHeadersBorderStyle {
            get {
                return (SystemFonts.MessageBoxFont.Name == "Segoe UI") ?
                DataGridViewHeaderBorderStyle.None :
                DataGridViewHeaderBorderStyle.Raised;
            }
        }

        public void Destroy() {

            _visionSystemMgr.Nodes[nodeId].NodeRecipeUpdate -= CamViewerGretel_NodeRecipeUpdate;
            pcbLive.VisibleChanged -= pcbLive_VisibleChanged;
            pcbLive.Dispose();
            pgCameraParams.BeginEdit -= pgCameraParams_BeginEdit;
            pgCameraParams.EndEdit -= pgCameraParams_EndEdit;
            pgCameraParams.ChangedParent -= pgCameraParams_ChangedParent;
            cbSaveResults.CheckedChanged -= cbSaveResults_CheckedChanged;
            _visionSystemMgr.MeasuresContainer.Remove(gviDisplay);
            _station.SetParametersCompleted -= _station_SetParametersCompleted;
            _station.ExportedParametersUpdated -= _station_ExportedParametersUpdated;
            gviDisplay.Dispose();
            resGrid.Dispose();
            pgCameraParams.Dispose();
            Dispose(true);
        }

        public void ConfirmSavedParameters() {

            ParameterEditingState = ParameterEditingStates.Reading;
        }

        public IntPtr GetPictureBoxHandle() {

            return pcbLive.Handle;
        }

        public ImageBox GetLivePictureBox() {

            return pcbLive;
        }

        public Panel GetThumbPanel() {

            return pnlThumbs;
        }

        public ZedGraphControl[] GetGraphBox() {

            return null;
        }

        void pgCameraParams_EndEdit(object sender, DataGridViewCellCancelEventArgs e) {

            //e.Cancel = !AppEngine.Current.TrySetSupervisorInReadyToRun();
        }

        void pgCameraParams_BeginEdit(object sender, DataGridViewCellCancelEventArgs e) {

            //if (AppEngine.Current.CurrentContext.SupervisorMode != SupervisorModeEnum.Editing)
            //    e.Cancel = true;

            //    e.Cancel = !AppEngine.Current.TrySetSupervisorInEditing();
        }

        void pgCameraParams_ChangedParent(object sender, DataGridViewCellEventArgs e) {

            nextToolIndex = Convert.ToInt32(pgCameraParams.ChildId.Split(new string[] { " ", "-", ";", ",", "." }, StringSplitOptions.RemoveEmptyEntries).Last()) - 1;

            OnCamViewerMessage(this, new CamViewerMessageEventArgs("SuspendLayout"));
            //currentParameterType = ParameterTypeEnum.Elaboration;
            loadParameters();
            OnCamViewerMessage(this, new CamViewerMessageEventArgs("ResumeLayout"));
        }

        public new void Hide() {

            base.Hide();
        }

        /// <summary>
        /// Permette di impostare la pagina dei parametri visualizzata
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

            btnCamera.Text = frmBase.UIStrings.GetString("Camera");
            btnDigitizer.Text = frmBase.UIStrings.GetString("Digitizer");
            btnElaboration.Text = frmBase.UIStrings.GetString("Tools");
            btnStrobo.Text = frmBase.UIStrings.GetString("Strobo");

            if (AppEngine.Current.CurrentContext.UserLevel >= UserLevelEnum.AssistantSupervisor) {
                pgCameraParams.SetColumnPercentageWidth("Value", 25);
                pgCameraParams.SetColumnPercentageWidth("ActualValue", 25);
            } else
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
                Log.Line(LogLevels.Error, "CamViewerGretel.getAcquisitionParam", _camera.IP4Address + ": Get acquisition parameters failed. Error: " + ex.Message);
                setCameraWarning(string.Format(frmBase.UIStrings.GetString("AcquisitionGetError"), Camera.IdCamera));
            }
            if (currentParameterType == ParameterTypeEnum.Acquisition) {
                pgCameraParams.ShowDataGridViewParent = false;
                pgCameraParams.ShowTitleBar = false;
                refreshData(new DAL.ParametersDataBinder<Parameter>(DataSource.AcquisitionParameters, actualAcquisitionParams, AppEngine.Current.CurrentContext.UserLevel));
            }
        }

        void getDigitizerParam() {

            //resetCameraWarning();
            if (DataSource == null) return;

            try {
                /*if (actualDigitizerParams == null && _node.Connected)*/
                actualDigitizerParams = _camera.GetDigitizerParameters();//readParams.Stations[_camera.StationId].Cameras[_camera.Head].AcquisitionParameters;
                DataSource.DigitizerParameters.PopulateParametersInfo(actualDigitizerParams);
                //foreach (Parameter dp in actualDigitizerParams) {
                //    Log.Line(LogLevels.Pass, "CamViewerGretel.getDigitizerParam", "Id: " + dp.Id + "\tLabel: " + dp.Label + "\tValue: " + dp.Value + "\tType: " + dp.ValueType);
                //}
                //btnApply.Enabled = true;
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "CamViewerGretel.getDigitizerParam", _camera.IP4Address + ": Get digitizer parameters failed. Error: " + ex.Message);
                setCameraWarning(string.Format(frmBase.UIStrings.GetString("AcquisitionGetError"), Camera.IdCamera));   //PIER: CAMBIARE ERRORE
            }
            if (currentParameterType == ParameterTypeEnum.Digitizer) {
                pgCameraParams.ShowDataGridViewParent = false;
                pgCameraParams.ShowTitleBar = false;
                refreshData(new DAL.ParametersDataBinder<Parameter>(DataSource.DigitizerParameters, actualDigitizerParams, AppEngine.Current.CurrentContext.UserLevel));
            }
        }

        int getElaborationParameter(bool refresh, int toolIndex) {

            //resetCameraWarning();
            int res = -1;
            Tool currTool = null;
            if (StationDataSource == null || StationDataSource.Tools == null || toolIndex >= StationDataSource.Tools.Count) return res;

            try {
                /*if (actualElaborationParams == null && _node.Connected)*/
                currTool = _station.GetTool(toolIndex);
                if (currTool == null) {
                    currTool = new Tool();
                }
                if (currTool != null && currTool.ToolParMerge != null) {
                    res = currTool.ToolParMerge.Count;
                    StationDataSource.Tools[toolIndex].ToolParMerge.PopulateParametersInfo(currTool.ToolParMerge);
                }
                //btnApply.Enabled = true;
            }
            catch (Exception ex) {
                if (refresh == true) {
                    Log.Line(LogLevels.Error, "CamViewerGretel.getElaborationParameter", _camera.IP4Address + ": Get elaboration parameters failed. Error: " + ex.Message);
                    setCameraWarning(string.Format(frmBase.UIStrings.GetString("ElaborationGetError"), Camera.IdCamera));
                }
            }

            if (currentParameterType == ParameterTypeEnum.Elaboration && (refresh == true) && res > -1) {

                pgCameraParams.ShowDataGridViewParent = true;
                pgCameraParams.ShowTitleBar = false;
                populateToolList();
                refreshData(new DAL.ParametersDataBinder<Parameter>(StationDataSource.Tools[toolIndex].ToolParMerge, currTool.ToolParMerge, AppEngine.Current.CurrentContext.UserLevel),
                    new DAL.ParametersDataBinder<Parameter>(toolsExported, toolsExported, AppEngine.Current.CurrentContext.UserLevel));
                pgCameraParams.Title = frmBase.UIStrings.GetString("Tool") + " " + (currToolIndex + 1).ToString("d2") + " - " + currTool.Label /*+ " (" + actualTool.TypeName + ")"*/;
            }
            return res;
        }

        void getRecipeAdvancedParameter() {

            //DataSource.RecipeAdvancedParameters.PopulateParametersInfo();
            //try {
            //    if (actualRecipeAdvancedParams == null && _node.Connected)
            //        actualRecipeAdvancedParams = _camera.GetRecipeAdvancedParameters();//readParams.Stations[_camera.StationId].Cameras[0].RecipeAdvancedParameters;//_camera.GetRecipeAdvancedParameters();
            //    //btnApply.Enabled = true;
            //}
            //catch (Exception ex) {
            //    Log.Line(LogLevels.Error, "CamViewerGretel.getRecipeAdvancedParameter", _camera.IP4Address + ": Get parametri di ricetta avanzati fallito. Errore: " + ex.Message);
            //    setCameraWarning(string.Format(frmBase.UIStrings.GetString("RecipeAdvancedGetError"), Camera.IdCamera));
            //}
            //if (currentParameterType == ParameterTypeEnum.RecipeAdvanced)
            //    refreshData(new DAL.ParametersDataBinder<Parameter>(DataSource.RecipeAdvancedParameters, actualRecipeAdvancedParams, AppEngine.Current.CurrentContext.UserLevel));
        }

        public void getROIParameter() {

            //    //resetCameraWarning();
            //    if (roiIndex < DataSource.ROIParameters.Count)
            //        DataSource.ROIParameters[roiIndex].PopulateParametersInfo();

            //    try {
            //        int numberOfROI = _camera.ROICount;
            //        if (numberOfROI > 0 && roiIndex < numberOfROI && _node.Connected) {
            //            //if (actualROIParams == null)
            //            //actualROIParams = readParams.Stations[_camera.StationId].Cameras[_camera.IdCamera].ROIParameters;//_camera.GetROIParameters(roiIndex);
            //            //btnApply.Enabled = true;
            //        }
            //    }
            //    catch (Exception ex) {
            //        Log.Line(LogLevels.Error, "CamViewerGretel.getROIParameter", _camera.IP4Address + ": Get parametri di ROI fallito. Errore: " + ex.Message);
            //        setCameraWarning();
            //    }
            //    if (roiIndex < DataSource.ROIParameters.Count && currentParameterType == ParameterTypeEnum.ROI) {
            //        refreshData(new DAL.ParametersDataBinder<Parameter>(DataSource.ROIParameters[roiIndex], actualROIParams, AppEngine.Current.CurrentContext.UserLevel));
            //    }
        }

        void getMachineParameter() {

        }

        void setCameraWarning() {

            setCameraWarning(frmBase.UIStrings.GetString("CameraError"));
        }

        void setCameraWarning(string message) 
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (this.InvokeRequired && IsHandleCreated) {
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

            if (this.InvokeRequired && IsHandleCreated) {
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

                    cbViewResults.Image = global::ExactaEasy.ResourcesArtic.search;
                    //cbViewResults.BackColor = Color.Moccasin;
                    pgCameraParams.Visible = false;
                    pnlParamSelector.Visible = false;
                    pnlParams.Visible = false;
                    pnlCameraTables.Visible = true;
                    resGrid.Visible = true;
                    resGrid.BringToFront();
                }
                else {

                    cbViewResults.Image = global::ExactaEasy.ResourcesArtic.search;
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
                    btnDigitizer.Visible = true;        // (DataSource.DigitizerParameters.Count != 0) ? true : false;
                    btnElaboration.Visible = true;            // (DataSource.RecipeSimpleParameters.Count != 0) ? true : false;
                    btnAdvanced.Visible = false;        // (DataSource.RecipeAdvancedParameters.Count != 0) ? true : false;
                    btnStrobo.Visible = true;           // (DataSource.Lights.Count != 0) ? true : false;
                    btnROI.Visible = false;             // (DataSource.ROIParameters.Count != 0) ? true : false;
                    btnMachine.Visible = false;         // (DataSource.MachineParameters.Count != 0) ? true : false;
                }
                //if (AppEngine.Current.CurrentContext.UserLevel < UserLevelEnum.Optrel) {
                //    btnMachine.Visible = false;
                //    btnAdvanced.Visible = false;
                //}
                //else {
                //    btnMachine.Visible = true;
                //    btnAdvanced.Visible = true;
                //}


                //if (AppEngine.Current.CurrentContext.UserLevel < UserLevelEnum.Supervisor) {
                //    //btnStopOnCondition.Enabled = false;
                //    //bttDownloadImages.Visible = false;
                //    //bttAnalysis.Enabled = false;
                //    //bttStopAnalysis.Enabled = false;
                //}
                //else {
                //    //btnStopOnCondition.Enabled = true;
                //    //bttDownloadImages.Visible = true;
                //    //bttAnalysis.Enabled = true;
                //    //bttStopAnalysis.Enabled = true;
                //}

                //btnSave.Enabled = false; // Nuova gestione save esterna
                //lastBtnApplyEnableValue = btnApply.Enabled;
                MachineModeEnum machineMode = AppEngine.Current.CurrentContext.MachineMode;
                if (machineMode == MachineModeEnum.Running || AppEngine.Current.CurrentContext.UserLevel < UserLevelEnum.AssistantSupervisor) {
                    setApplyEnabled(false);
                    //btnEdit.Enabled = false;
                    //btnRestore.Enabled = false;
                    btnZoom.Enabled = false;
                    //btnRDP.Enabled = false;
                }
                else {
                    setApplyEnabled(true);
                    switch (ParameterEditingState) {
                        case ParameterEditingStates.Reading:
                        case ParameterEditingStates.Applied:
                            //btnApply.Enabled = true;
                            //btnEdit.Enabled = true;
                            //btnRestore.Enabled = false;
                            break;
                        case ParameterEditingStates.Editing:
                            //btnApply.Enabled = false;
                            //btnEdit.Enabled = false;
                            //btnRestore.Enabled = true;
                            break;
                    }
                    //btnZoom.Enabled = true;   // 26/11/2014 not used until now
                    //btnRDP.Enabled = true;
                    //ManageLiveButton();
                }
                if (machineMode == MachineModeEnum.Running || AppEngine.Current.CurrentContext.UserLevel < UserLevelEnum.Engineer) {    //24/06/2016 richiesta Menarini
                    btnRDP.Enabled = false;
                }
                else {
                    btnRDP.Enabled = true;
                }
            }
        }

        public void ReloadParameters(ParameterTypeEnum parType) {

            if ((parType & ParameterTypeEnum.Acquisition) == ParameterTypeEnum.Acquisition)
                actualAcquisitionParams = null;
            if ((parType & ParameterTypeEnum.Digitizer) == ParameterTypeEnum.Digitizer)
                actualDigitizerParams = null;
            //if ((parType & ParameterTypeEnum.Elaboration) == ParameterTypeEnum.Elaboration)
            //    actualToolParams = null;
            //if ((parType & ParameterTypeEnum.RecipeAdvanced) == ParameterTypeEnum.RecipeAdvanced)
            //    actualRecipeAdvancedParams = null;
            //if ((parType & ParameterTypeEnum.ROI) == ParameterTypeEnum.ROI)
            //    actualROIParams = null;
            //if ((parType & ParameterTypeEnum.Machine) == ParameterTypeEnum.Machine)
            //    actualMachineParams = null;
            //if ((parType & ParameterTypeEnum.Strobo) == ParameterTypeEnum.Strobo)
            //    actualStroboParams = null;

            loadParameters(parType);
        }


        void loadParameters() {

            loadParameters(currentParameterType);
        }


        void loadParameters(ParameterTypeEnum parType) 
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (InvokeRequired && IsHandleCreated) {
                Invoke(new MethodInvoker(() => loadParameters(parType)));
            }
            else 
            {
                //if (!isCameraReady()) {
                //    OnCamViewerMessage(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("CameraNotReady")));
                //    return;
                //}

                try
                {
                    OnCamViewerMessage(this, new CamViewerMessageEventArgs(""));
                    //isCameraOK = true;
                    pgCameraParams.Title = paramTitleLabel[currentParameterType];

                    try
                    {
                        _node.GetParameters();
                    }
                    catch (Exception ex)
                    {
                        Log.Line(LogLevels.Error, "CamViewerGretel.loadParameters", _node.Address + ": GetParameters failed! Error: " + ex.Message);
                        setCameraWarning(string.Format(_node.Address + ": " + frmBase.UIStrings.GetString("GetParametersFailed")));
                    }
                    foreach (Control c in pnlParamSelector.Controls)
                    {
                        if (c is Button)
                            c.BackColor = Color.FromArgb(50, 50, 50);
                    }
                    if ((parType & ParameterTypeEnum.Acquisition) != 0)
                    {
                        getAcquisitionParam();
                    }
                    if ((parType & ParameterTypeEnum.Digitizer) != 0)
                    {
                        getDigitizerParam();
                    }
                    if ((parType & ParameterTypeEnum.Elaboration) != 0)
                    {
                        int maxToolsNum = _station.ToolsCount;
                        currToolIndex = nextToolIndex;
                        for (int i = 0; i < maxToolsNum; i++)
                        {
                            getElaborationParameter(false, i);
                        }
                        for (int i = 0; i < maxToolsNum; i++)
                        {
                            int toolId = (currToolIndex + i) % maxToolsNum;
                            if (getElaborationParameter(false, toolId) > 0)
                            {
                                currToolIndex = toolId;
                                break;
                            }
                        }
                        getElaborationParameter(true, currToolIndex);
                    }
                    else
                    {
                        nextToolIndex = currToolIndex;
                    }
                    if ((parType & ParameterTypeEnum.RecipeAdvanced) != 0)
                    {
                        getRecipeAdvancedParameter();
                    }
                    if ((parType & ParameterTypeEnum.ROI) != 0)
                    {
                        getROIParameter();
                    }
                    if ((parType & ParameterTypeEnum.Machine) != 0)
                    {
                        getMachineParameter();
                    }

                    if (currentParameterType == ParameterTypeEnum.Acquisition)
                    {
                        btnCamera.BackColor = Color.FromArgb(28, 28, 28);
                    }
                    if (currentParameterType == ParameterTypeEnum.Digitizer)
                    {
                        btnDigitizer.BackColor = Color.FromArgb(28, 28, 28);
                    }
                    if (currentParameterType == ParameterTypeEnum.Elaboration)
                    {
                        btnElaboration.BackColor = Color.FromArgb(28, 28, 28);
                    }
                    if (currentParameterType == ParameterTypeEnum.RecipeAdvanced)
                    {
                        btnAdvanced.BackColor = Color.FromArgb(28, 28, 28);
                    }
                    if (currentParameterType == ParameterTypeEnum.ROI)
                    {
                        btnROI.BackColor = Color.FromArgb(28, 28, 28);
                        //if (roiPageLabel.ContainsKey(roiIndex))
                        //    pgCameraParams.Title = roiPageLabel[roiIndex] + " (" + (roiIndex + 1).ToString() + "/" + DataSource.ROIParameters.Count.ToString() + ")";
                        //else
                        //    pgCameraParams.Title += " (" + (roiIndex + 1).ToString() + "/" + DataSource.ROIParameters.Count.ToString() + ")";
                    }
                    if (currentParameterType == ParameterTypeEnum.Machine)
                    {
                        btnMachine.BackColor = Color.FromArgb(28, 28, 28);
                    }
                }
                catch (ObjectDisposedException odex)
                {
                    Log.Line(LogLevels.Error, "CamViewerGretel.loadParameters", $"ObjectDisposedException raised: {odex}");
                }
                catch (InvalidOperationException ioex)
                {
                    Log.Line(LogLevels.Error, "CamViewerGretel.loadParameters", $"InvalidOperationException raised: {ioex}");
                }
                catch (Exception ex)
                {
                    Log.Line(LogLevels.Error, "CamViewerGretel.loadParameters", $"Exception raised: {ex}");
                }
            }
            //if (isTattileOK)
            //    btnApply.Enabled = true;
        }

        private void btnApply_Click(object sender, EventArgs e) {

            if (AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Busy)) {
                setApplyEnabled(false);
                OnCamViewerMessage(this, new CamViewerMessageEventArgs("SuspendLayout"));
                if (applyParameters(ParameterTypeEnum.All) == true) {
                    //ParameterEditingState = ParameterEditingStates.Applied;
                    pgCameraParams.UpdateLogCtrl("", Color.White);
                }
                else {
                    //ParameterEditingState = ParameterEditingStates.Reading;
                    pgCameraParams.UpdateLogCtrl("PARAMETERS SET FAILED", Color.Red);
                }
                RecalcButton();
                pgCameraParams.Refresh();
                //if (ParameterEditingState == ParameterEditingStates.Applied)
                //    AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.ReadyToRun);
                //else
                //    AppEngine.Current.TrySetSupervisorInEditing();
                //AppEngine.Current.TrySetSupervisorInReadyToRun();
            }
            else {
                Log.Line(LogLevels.Warning, "CamViewerGretel.btnApply_Click", _camera.IP4Address + ": Cannot apply parameters, machine mode: " + AppEngine.Current.CurrentContext.MachineMode.ToString());
            }
        }

        private void setApplyEnabled(bool enable) 
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(btnApply))
                return;

            if (btnApply.InvokeRequired && btnApply.IsHandleCreated) {
                Invoke(new MethodInvoker(() => setApplyEnabled(enable)));
            }
            else {
                btnApply.Enabled = enable;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e) {

            //if (AppEngine.Current.TrySetSupervisorInEditing()) {
            //    ParameterEditingState = ParameterEditingStates.Editing;
            //    RecalcButton();
            //}
            //else {
            //    OnCamViewerError(this, new CamViewerErrorEventArgs(_camera, _camera.IP4Address + ": " + frmBase.UIStrings.GetString("NoEditingAvailable")));
            //}
        }

        private void btnSave_Click(object sender, EventArgs e) {

            //if ((AppEngine.Current.CurrentContext.SupervisorMode == SupervisorModeEnum.ReadyToRun ||
            //    AppEngine.Current.CurrentContext.SupervisorMode == SupervisorModeEnum.Editing) &&
            //    AppEngine.Current.CurrentContext.ActiveRecipe != null) {

            //    if (AppEngine.Current.TrySaveRecipe()) {
            //        if (AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.ReadyToRun)) {
            //            ParameterEditingState = ParameterEditingStates.Reading;
            //            RecalcButton();
            //            AppEngine.Current.SendSupervisorMode(SupervisorModeEnum.ReadyToRun);
            //        }
            //        else {
            //            OnCamViewerError(this, new CamViewerErrorEventArgs(_camera, _camera.IP4Address + ": " + frmBase.UIStrings.GetString("NoReadyToRunAvailable")));
            //        }
            //    }
            //    else {
            //        OnCamViewerError(this, new CamViewerErrorEventArgs(_camera, _camera.IP4Address + ": " + frmBase.UIStrings.GetString("SavingCurrentRecipeFailed")));
            //        //OnCamViewerError(this, new CamViewerErrorEventArgs(_camera.IP4Address + frmBase.UIStrings.GetString("NoReadyToRunAvailable")));
            //    }
            //}
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
            resetCameraWarning();
            //timerCameraStatus.Stop();
            int tempToolIndex = nextToolIndex;
            nextToolIndex = currToolIndex;
            loadParameters(paramType);
            //Log.Line(LogLevels.Pass, "CamViewerGretel.applyParameters", "Node: {0} Station {1} SetParameters", _station.NodeId, _station.IdStation);
            try {
                //_visionSystemMgr.Nodes[nodeId].SetParameters("", NodeDataSource, AppEngine.Current.ParametersInfo, AppEngine.Current.CurrentContext.CultureCode);   //pier: modificare recipeName
                ParameterCollection<Parameter> parameters = populateSetParameterList();
                _station.SetParameters(parameters);
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "CamViewerGretel.applyParameters", _camera.IP4Address + ": Error while sending parameters: " + ex.Message);
                OnCamViewerError(this, new CamViewerErrorEventArgs(_camera, _camera.IP4Address + ": " + frmBase.UIStrings.GetString("ErrorSendingParameters")));
                ris = false;
            }
            if ((paramType & ParameterTypeEnum.Acquisition) != 0) actualAcquisitionParams = null;
            if ((paramType & ParameterTypeEnum.Digitizer) != 0) actualDigitizerParams = null;
            //if ((paramType & ParameterTypeEnum.Elaboration) != 0) actualToolParams = null;
            //if ((paramType & ParameterTypeEnum.RecipeAdvanced) != 0) actualRecipeAdvancedParams = null;
            //if ((paramType & ParameterTypeEnum.ROI) != 0) actualROIParams = null;
            //if ((paramType & ParameterTypeEnum.Machine) != 0) actualMachineParams = null;
            //if ((paramType & ParameterTypeEnum.Strobo) != 0) actualStroboParams = null;
            loadParameters(paramType);
            nextToolIndex = tempToolIndex;
            //timerCameraStatus.Start();
            OnCamViewerMessage(this, new CamViewerMessageEventArgs(""));
            this.Cursor = Cursors.Default;
            return ris;
        }

        private ParameterCollection<Parameter> populateSetParameterList() {

            ParameterCollection<Parameter> res = new ParameterCollection<Parameter>();
            foreach (Parameter p in DataSource.AcquisitionParameters) {
                if (p.IsVisible > 0) {
                    res.Add(p);
                }
            }
            foreach (Parameter p in DataSource.DigitizerParameters) {
                if (p.IsVisible > 0) {
                    res.Add(p);
                }
            }
            for (int i = 0; i < _station.ToolsCount; i++) {
                if (StationDataSource.Tools.Count > i) {
                    foreach (Parameter p in StationDataSource.Tools[i].ToolParMerge) {
                        if (p.IsVisible > 0) {
                            res.Add(p);
                        }
                    }
                }
            }
            return res;
        }

        private void populateToolList() {

            if (_station == null) return;
            try
            {
                toolsExported.Clear();
                for (int i = 0; i < _station.ToolsCount; i++)
                {

                    Tool currTool = _station.GetTool(i);
                    if (currTool == null)
                        continue;
                    if (toolsExported.Contains(currTool.Id) == false)
                    {
                        Parameter toolPar = new Parameter();
                        toolPar.Id = frmBase.UIStrings.GetString("Tool") + " " + (currTool.Id + 1).ToString();    //tradurre
                        toolPar.ExportName = toolPar.Label = (string.IsNullOrEmpty(currTool.Label) == true) ? "---" : currTool.Label;
                        //toolPar.Description = currTool.TypeName;
                        toolPar.Value = currTool.Active.ToString();
                        toolPar.ValueType = "bool";
                        toolPar.IsVisible = 1;
                        toolsExported.Add(toolPar);
                    }
                }
            } catch (Exception ex) {
                Log.Line(LogLevels.Error, "CamViewerGretel.populateToolList", _camera.IP4Address + ": error: " + ex.Message);
            }
        }

        private void btnRDP_Click(object sender, EventArgs e) {

            //if (!isCameraReady()) {
            //    OnCamViewerMessage(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("CameraNotReady")));
            //    return;
            //}
            OnCamViewerMessage(this, new CamViewerMessageEventArgs("RemoteDesktopConnect", _camera.IdCamera.ToString()));
            //    ManageLiveButton();
        }

        void OnParameterEditingStatusChanged(object sender, EventArgs e) {

            if (ParameterEditingStateChanged != null)
                ParameterEditingStateChanged(sender, e);
        }

        private void CamViewer_Load(object sender, EventArgs e) {
        }

        public string SaveFolderRoot { get; set; }
        private void cbRecordImages_CheckedChanged(object sender, EventArgs e) {

            //try {
            //    _visionSystemMgr.Nodes[nodeId].Stations[stationId].SaveBufferedImages(_machineConfig.BufferedInspectionImagesPath, cbRecordImages.Checked);
            //}
            //catch (Exception ex) {
            //    Log.Line(LogLevels.Error, "CamViewerGretel.cbRecordImages_CheckedChanged", Description + ": Error: " + ex.Message);
            //    OnCamViewerMessage(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("RecordImagesError" + ".")));
            //}
        }

        private void bttInfo_Click(object sender, EventArgs e) {

            try {
                _station.HardReset();
                applyParameters(ParameterTypeEnum.All);
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "CamViewer.bttInfo_Click", _camera.IP4Address + ": Reset error: " + ex.Message);
                OnCamViewerError(this, new CamViewerErrorEventArgs(_camera, _camera.IP4Address + ": " + frmBase.UIStrings.GetString("ResetError")));
            }
        }

        decimal calcImgRatio = 1;
        decimal calcImageRatio() {

            if (_camera != null) {
                try {
                    CameraInfo camInfo = _camera.GetCameraInfo();
                    calcImgRatio = (camInfo.heightImage > 0) ? (decimal)camInfo.widthImage / camInfo.heightImage : 1;
                    return calcImgRatio;
                }
                catch (Exception ex) {
                    Log.Line(LogLevels.Error, "CamViewer.calcImageRatio", _camera.IP4Address + ": Error: " + ex.Message);
                }
            }
            return calcImgRatio;
            /*decimal resX = 1;
            decimal resY = 1;
            if (DataSource.AcquisitionParameters["aoiResX"] != null)
                resX = (decimal)DataSource.AcquisitionParameters["aoiResX"].GetValue();
            if (DataSource.AcquisitionParameters["aoiResY"] != null)
                resY = (decimal)DataSource.AcquisitionParameters["aoiResY"].GetValue();
            return resX / resY;*/
        }

        private void pnlCenter_Resize(object sender, EventArgs e) {

            pcbLive.Height = pnlCenter.Height;
            if (pnlCenter.Width > pcbLive.Width)
                pcbLive.Left = (pnlCenter.Width - pcbLive.Width) / 2;
            else
                pcbLive.Left = 0;
        }

        private void lblCameraStatus_Click(object sender, EventArgs e) {

            if (AppEngine.Current.CurrentContext.UserLevel == UserLevelEnum.Administrator) {
                DataSource.Enabled = !DataSource.Enabled;
                setCameraStatus();
            }
        }

        void setCameraStatus() {

            //if (lblCameraStatus.InvokeRequired) {
            //    lblCameraStatus.Invoke(new MethodInvoker(setCameraStatus));
            //}
            //else {
            //    CameraNewStatus camStatus = _camera.GetCameraStatus();
            //    if (DataSource.Enabled && isCameraOK && (camStatus == CameraNewStatus.Ready))
            //        lblCameraStatus.Image = global::ExactaEasy.Properties.Resources.green_on_32;
            //    else if (DataSource.Enabled && (!isCameraOK || (camStatus != CameraNewStatus.Ready)))
            //        lblCameraStatus.Image = global::ExactaEasy.Properties.Resources.amber_on_32;
            //    else
            //        lblCameraStatus.Image = global::ExactaEasy.Properties.Resources.red_on_32;
            //}
        }

        private void btnCounterReset_Click(object sender, EventArgs e) {

            resetCameraWarning();
            try {
                _camera.SoftReset();
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "CamViewer.btnCounterReset_Click", _camera.IP4Address + ": Reset error: " + ex.Message);
                OnCamViewerError(this, new CamViewerErrorEventArgs(_camera, _camera.IP4Address + ": " + frmBase.UIStrings.GetString("ResetError")));
            }
        }

        private void lblInfo_Click(object sender, EventArgs e) {

        }

        protected virtual void OnCamViewerError(object sender, CamViewerErrorEventArgs e) {

            if (CamViewerError != null)
                CamViewerError(sender, e);
        }

        protected virtual void OnCamViewerMessage(object sender, CamViewerMessageEventArgs e) {

            if (CamViewerMessage != null)
                CamViewerMessage(sender, e);
        }

        private void buttonConnect_Click(object sender, EventArgs e) {
            ((IStation)_camera).Connect();
        }

        private void buttonDisconnect_Click(object sender, EventArgs e) {
            ((IStation)_camera).StopGrab();
            Thread.Sleep(5000);
            ((IStation)_camera).Disconnect();
        }

        //bool isCameraReady() {
        //    try {
        //        CameraNewStatus camStatus = _camera.GetCameraStatus();
        //        if (camStatus != CameraNewStatus.Ready) {
        //            isCameraOK = false;
        //            return false;
        //        }
        //    }
        //    catch {
        //        return false;
        //    }
        //    return true;
        //}

        //void getROIPageLabel() {

        //    //roiPageLabel = new Dictionary<int, string>();
        //    //string language = AppEngine.Current.CurrentContext.CultureCode;
        //    //for (int i = 0; i < dataSource.ROIParameters.Count(); i++) {
        //    //    Parameter p = dataSource.ROIParameters[i].Find(pr => { return pr.Id.ToLower() == "label_" + language; });
        //    //    if (p != null)
        //    //        roiPageLabel.Add(i, p.Value);
        //    //}
        //}

        private void cbSaveResults_CheckedChanged(object sender, EventArgs e) {

            if (cbSaveResults.Checked) {
                OnCamViewerMessage(this, new CamViewerMessageEventArgs("EnableSavingResultsOnFile"));
                cbSaveResults.BackColor = Color.Moccasin;
            }
            else {
                OnCamViewerMessage(this, new CamViewerMessageEventArgs("DisableSavingResultsOnFile"));
                cbSaveResults.BackColor = SystemColors.Control;
            }
        }

        public bool DumpResultsChecked {
            get {
                return cbSaveResults.Checked;
            }
            set {
                cbSaveResults.Checked = value;
                if (value == true) cbSaveResults.BackColor = Color.Moccasin;
                else cbSaveResults.BackColor = SystemColors.Control;
            }
        }


        public void SetDataSource(object dataSource) {
            if (dataSource is NodeRecipe) {
                NodeDataSource = (NodeRecipe)dataSource;
                //DataSource = NodeDataSource.Stations[_camera.StationId].Cameras[0];
                if (gviDisplay != null)
                    gviDisplay.CurrRecipe = AppEngine.Current.CurrentContext.ActiveRecipe;
            }
        }

        private void cbViewResults_CheckedChanged(object sender, EventArgs e) {

            RecalcButton();
        }

        public void StartRunningVisualization() {

            //cbViewResults_CheckedChanged(this, EventArgs.Empty);
            RecalcButton();
        }


        private void pnlThumbs_SizeChanged(object sender, EventArgs e) {

            Display d = _visionSystemMgr.Displays["Cam_" + Settings.Id];
            if (_visionSystemMgr.Displays != null && _visionSystemMgr.Displays.Count > 0 && d != null)
                d.Resize(pnlThumbs.Width, pnlThumbs.Height);
        }

        public void CreateGraphs(Graph g, object recipe) {
            throw new NotImplementedException();
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
