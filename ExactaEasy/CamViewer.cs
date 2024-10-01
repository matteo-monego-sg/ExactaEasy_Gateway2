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

    public enum ParameterEditingStates {

        Reading = 0,
        Editing = 1,
        Applied = 2
    }

    public interface ICamViewer {

        event EventHandler<CamViewerErrorEventArgs> CamViewerError;
        event EventHandler<CamViewerMessageEventArgs> CamViewerMessage;
        event EventHandler ParameterEditingStateChanged;
        event EventHandler<CamViewerMessageEventArgs> ParametersUploaded;

        //bool IsInLiveMode { get; }
        string SaveFolderRoot { get; set; }
        bool Enabled { get; set; }
        string Description { get; set; }
        ParameterEditingStates ParameterEditingState { get; }
        bool DumpResultsChecked { get; set; }
        CameraSetting Settings { get; }

        void DoRefresh();
        void RefreshStationStatus();
        void StartRunningVisualization();
        void ReloadParameters(ParameterTypeEnum parType);
        void SetParametersPage(ParameterTypeEnum parType);
        IntPtr GetPictureBoxHandle();
        ImageBox GetLivePictureBox();
        Panel GetThumbPanel();
        ZedGraphControl[] GetGraphBox();
        HvldDisplayControl GetSignalStacker();
        void Destroy();

        void BringToFront();
        void Hide();
        void Show();
        void SetDataSource(object dataSource);
        void ConfirmSavedParameters();
        void CreateGraphs(Graph g, object recipe);
        void UpdateLogCtrl(string message, Color color);
    }

    public partial class CamViewer : UserControl, ICamViewer {

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

        Cam dataSource;
        //Cam dataSourceForROI;
        Cam originalDataSource;

        ParameterTypeEnum currentParameterType;
        ParameterCollection<Parameter> actualAcquisitionParams;
        ParameterCollection<Parameter> actualDigitizerParams;
        ParameterCollection<Parameter> actualElaborationParams;
        ParameterCollection<Parameter> actualRecipeAdvancedParams;
        //ParameterCollection<Parameter> actualROIParams;
        ParameterCollection<Parameter> actualMachineParams;
        ParameterCollection<Parameter> actualStroboParams;
        Dictionary<int, string> roiPageLabel;
        VisionSystemManager _visionSystemMgr;

        int roiIndex = 0;
        int lightIndex = 0;
        //int nextRoiIndex = 0;
        bool isCameraOK = true;
        private Panel pnlStopOnCond;
        private StopOnCond stopOnCond1;
        private Panel pnlVialAxisMenu;
        private VialAxisMenu vialAxisMenu;
        private Panel pnlResetMenu;
        private CamResetMenu resetMenu;
        private Panel pnlDownloadsMenu;
        private DownloadImagesMenu downloadMenu;
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

        Cam DataSource {
            get {
                return dataSource;
            }
            set {
                dataSource = value;
                getROIPageLabel();
                originalDataSource = dataSource.Clone(AppEngine.Current.ParametersInfo, AppEngine.Current.CurrentContext.CultureCode);
                currentParameterType = ParameterTypeEnum.Acquisition;
                loadParameters();
                setCameraStatus();
                //pcbLive.ImageAspectRatio = calcImageRatio();
            }
        }

        Camera _camera;
        public Camera Camera {
            get {
                return _camera;
            }
        }

        //public bool IsInLiveMode {
        //    get { return (pcbLive.IsConnected && pcbLive.IsLiveConnection); }
        //}

        enum StatusEvents {
            Kill = 0,
            Poll,
        }

        WaitHandle[] statusThEvts;
        ManualResetEvent killEv;
        ManualResetEvent pollEv;
        Thread statusTh;

        CamViewer() {
            InitializeComponent();
            createStopOnConditionMenu();
            createVialAxisMenu();
            createResetMenu();
            createDownloadMenu();

            btnApply.Text = frmBase.UIStrings.GetString("BtnApply");
            btnEdit.Text = frmBase.UIStrings.GetString("BtnEdit");
            btnSave.Text = frmBase.UIStrings.GetString("BtnSave");
            btnRestore.Text = frmBase.UIStrings.GetString("BtnUndo");
            btnLiveStart.Text = frmBase.UIStrings.GetString("BtnLiveStart");
            btnLiveStartFF.Text = frmBase.UIStrings.GetString("BtnLiveStartFF");
            lblPixInfo.Text = frmBase.UIStrings.GetString("PixelInfo");
            cbZoom.Text = frmBase.UIStrings.GetString("Zoom");
            btnStopOnCondition.Text = frmBase.UIStrings.GetString("StopOnCondition");
            bttAnalysis.Text = frmBase.UIStrings.GetString("OfflineAnalysis");
            bttStopAnalysis.Text = frmBase.UIStrings.GetString("StopOffAnalysis");
            bttDownloadImages.Text = frmBase.UIStrings.GetString("DownloadImages");
            btnRemoteDesktop.Text = frmBase.UIStrings.GetString("Config");
            btnVialAxis.Text = frmBase.UIStrings.GetString("VialAxis");
            btnReset.Text = frmBase.UIStrings.GetString("Reset");
            btnPrev.Text = frmBase.UIStrings.GetString("Prev");
            btnLast.Text = frmBase.UIStrings.GetString("Last");

            ParameterEditingState = ParameterEditingStates.Reading;
            pgCameraParams.BeginEdit += new DataGridViewCellCancelEventHandler(pgCameraParams_BeginEdit);
            pgCameraParams.EndEdit += new DataGridViewCellCancelEventHandler(pgCameraParams_EndEdit);

            cbZoom.Checked = false;
        }

        public CamViewer(VisionSystemManager visionSystemMgr, MachineConfiguration machineConfig, int cameraIndex)
            : this() {

            _visionSystemMgr = visionSystemMgr;
            Settings = machineConfig.CameraSettings[cameraIndex];
            _camera = (Camera)_visionSystemMgr.Cameras[Settings.Id];
            lblCameraDescription.Text = _camera.CameraDescription;
            //AppContext ctx = AppEngine.Current.CurrentContext;
            RefreshStationStatus();
            pcbLive.VisibleChanged += new EventHandler(pcbLive_VisibleChanged);
            //timerCameraStatus.Start();
            //btnLiveStart.Enabled = _camera.ROIModeAvailable;
            stopOnCond1.SpindleCount = machineConfig.NumberOfSpindles;
            // TODO: Rimuovere il test non appena risolto bug di Tattile
            //if (_camera.CameraType.Contains("M9"))
            //    btnLiveStartFF.Enabled = false;
            btnRemoteDesktop.Visible = (Settings.CameraProviderName == "TattileCameraM12") ? true : false;
            btnVialAxis.Visible = (Settings.CameraProviderName == "TattileCameraM9") ? true : false;

            pnlCameraCommand.Visible = false;
            pnlCameraCommand.Visible = true;

            killEv = new ManualResetEvent(false);
            pollEv = new ManualResetEvent(false);
            statusThEvts = new WaitHandle[2];
            statusThEvts[0] = killEv;
            statusThEvts[1] = pollEv;

            if (statusTh == null || !statusTh.IsAlive) {
                if (statusTh != null) {
                    statusTh.Interrupt();
                    statusTh.Abort();
                }
                statusTh = new Thread(new ThreadStart(statusThread));
                statusTh.Name = "CamViewer Camera Status Thread " + Settings.Id;
                statusTh.IsBackground = true;
            }
            if (!statusTh.IsAlive)
                statusTh.Start();
        }

        void createStopOnConditionMenu() {

            stopOnCond1 = new StopOnCond();
            pnlStopOnCond = new Panel();
            pnlStopOnCond.SuspendLayout();
            pnlControls.SuspendLayout();
            SuspendLayout();
            pnlControls.Controls.Add(this.pnlStopOnCond);
            stopOnCond1.Dock = System.Windows.Forms.DockStyle.Fill;
            stopOnCond1.Location = new System.Drawing.Point(0, 0);
            stopOnCond1.MaxTimeoutSec = 9999;
            stopOnCond1.Name = "stopOnCond1";
            stopOnCond1.Size = new System.Drawing.Size(496, 76);
            stopOnCond1.SpindleCount = 40;
            stopOnCond1.TabIndex = 0;
            stopOnCond1.VisibleChanged += new EventHandler(stopOnCond1_VisibleChanged);
            stopOnCond1.ConditionUpdated += new EventHandler(stopOnCond1_ConditionUpdated);
            pnlStopOnCond.Controls.Add(this.stopOnCond1);
            pnlStopOnCond.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlStopOnCond.Location = new System.Drawing.Point(0, 0);
            pnlStopOnCond.Name = "pnlStopOnCond";
            pnlStopOnCond.Size = new System.Drawing.Size(496, 76);
            pnlStopOnCond.TabIndex = 15;
            pnlStopOnCond.Visible = false;
            pnlStopOnCond.ResumeLayout(false);
            pnlControls.ResumeLayout(false);
            ResumeLayout(false);
        }

        void createVialAxisMenu() {

            vialAxisMenu = new VialAxisMenu();
            pnlVialAxisMenu = new Panel();
            pnlVialAxisMenu.SuspendLayout();
            pnlControls.SuspendLayout();
            SuspendLayout();
            pnlControls.Controls.Add(this.pnlVialAxisMenu);
            vialAxisMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            vialAxisMenu.Location = new System.Drawing.Point(0, 0);
            vialAxisMenu.Name = "vialAxisMenu";
            vialAxisMenu.Size = new System.Drawing.Size(496, 76);
            vialAxisMenu.TabIndex = 0;
            vialAxisMenu.VisibleChanged += new EventHandler(vialAxisMenu_VisibleChanged);
            vialAxisMenu.ConditionUpdated += vialAxisMenu_ConditionUpdated;
            pnlVialAxisMenu.Controls.Add(this.vialAxisMenu);
            pnlVialAxisMenu.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlVialAxisMenu.Location = new System.Drawing.Point(0, 0);
            pnlVialAxisMenu.Name = "pnlVialAxisMenu";
            pnlVialAxisMenu.Size = new System.Drawing.Size(496, 76);
            pnlVialAxisMenu.TabIndex = 15;
            pnlVialAxisMenu.Visible = false;
            pnlVialAxisMenu.ResumeLayout(false);
            pnlControls.ResumeLayout(false);
            ResumeLayout(false);
        }

        void createResetMenu() {

            resetMenu = new CamResetMenu();
            pnlResetMenu = new Panel();
            pnlResetMenu.SuspendLayout();
            pnlControls.SuspendLayout();
            SuspendLayout();
            pnlControls.Controls.Add(this.pnlResetMenu);
            resetMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            resetMenu.Location = new System.Drawing.Point(0, 0);
            resetMenu.Name = "resetMenu";
            resetMenu.Size = new System.Drawing.Size(496, 76);
            resetMenu.TabIndex = 0;
            resetMenu.VisibleChanged += new EventHandler(resetMenu_VisibleChanged);
            resetMenu.Error += resetMenu_Error;
            resetMenu.ConditionUpdated += resetMenu_ConditionUpdated;
            resetMenu.ApplyParameters += resetMenu_ApplyParameters;
            pnlResetMenu.Controls.Add(this.resetMenu);
            pnlResetMenu.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlResetMenu.Location = new System.Drawing.Point(0, 0);
            pnlResetMenu.Name = "pnlResetMenu";
            pnlResetMenu.Size = new System.Drawing.Size(496, 76);
            pnlResetMenu.TabIndex = 15;
            pnlResetMenu.Visible = false;
            pnlResetMenu.ResumeLayout(false);
            pnlControls.ResumeLayout(false);
            ResumeLayout(false);
        }

        void createDownloadMenu() {

            downloadMenu = new DownloadImagesMenu();
            pnlDownloadsMenu = new Panel();
            pnlDownloadsMenu.SuspendLayout();
            pnlControls.SuspendLayout();
            SuspendLayout();
            pnlControls.Controls.Add(this.pnlDownloadsMenu);
            downloadMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            downloadMenu.Location = new System.Drawing.Point(0, 0);
            downloadMenu.Name = "resetMenu";
            downloadMenu.Size = new System.Drawing.Size(496, 76);
            downloadMenu.TabIndex = 0;
            downloadMenu.VisibleChanged += new EventHandler(downloadMenu_VisibleChanged);
            downloadMenu.Error += downloadMenu_Error;
            downloadMenu.ConditionUpdated += downloadMenu_ConditionUpdated;
            pnlDownloadsMenu.Controls.Add(this.downloadMenu);
            pnlDownloadsMenu.Dock = System.Windows.Forms.DockStyle.Bottom;
            pnlDownloadsMenu.Location = new System.Drawing.Point(0, 0);
            pnlDownloadsMenu.Name = "pnlDownloadsMenu";
            pnlDownloadsMenu.Size = new System.Drawing.Size(496, 76);
            pnlDownloadsMenu.TabIndex = 15;
            pnlDownloadsMenu.Visible = false;
            pnlDownloadsMenu.ResumeLayout(false);
            pnlControls.ResumeLayout(false);
            ResumeLayout(false);
        }

        public void SetDataSource(object dataSource) {

            if (dataSource is Cam)
                DataSource = (Cam)dataSource;
        }

        public void UpdateLogCtrl(string message, Color color) {

            throw new NotImplementedException();
        }

        public void Destroy() {
            exit = true;
            killEv.Set();
            Application.DoEvents();     //serve altrimenti rimaniamo appesi
            if (statusTh != null) {
                if (statusTh.IsAlive)
                    statusTh.Join(4000);
                statusTh.Interrupt();
                statusTh.Abort();
            }
        }

        public IntPtr GetPictureBoxHandle() {

            return pcbLive.Handle;
        }

        public ImageBox GetLivePictureBox() {

            return pcbLive;
        }

        public Panel GetThumbPanel() {

            return null;
        }

        public ZedGraphControl[] GetGraphBox() {

            return null;
        }

        void pgCameraParams_EndEdit(object sender, DataGridViewCellCancelEventArgs e) {

            //e.Cancel = !AppEngine.Current.TrySetSupervisorInReadyToRun();
        }

        void pgCameraParams_BeginEdit(object sender, DataGridViewCellCancelEventArgs e) {

            if (AppEngine.Current.CurrentContext.SupervisorMode != SupervisorModeEnum.Editing)
                e.Cancel = true;

            //    e.Cancel = !AppEngine.Current.TrySetSupervisorInEditing();
        }

        //void pgCameraParams_ROIChange(object sender, EventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        //public CamViewer(CameraSetting cameraSettings)
        //    : this() {

        //    GuiContext = SynchronizationContext.Current;
        //    Settings = cameraSettings;
        //    lblCameraDescription.Text = cameraSettings.CameraDescription;//cameraSettings.Id + "(" + cameraSettings.CameraType + ")";
        //    AppContext ctx = AppEngine.Current.CurrentContext;
        //    if (ctx.EnabledStation != null && ctx.EnabledStation.Length > Settings.Station && ctx.EnabledStation[Settings.Station])
        //        lblStationStatus.Image = global::ExactaEasy.Properties.Resources.green_on_32;
        //    else
        //        lblStationStatus.Image = global::ExactaEasy.Properties.Resources.red_on_32;
        //    timerCameraStatus.Start();
        //}

        public new void Hide() {
            //timerCameraStatus.Stop();
            pollEv.Reset();
            cbZoom.Checked = false;
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

            //try {
            //    if (_camera.Connected) {
            //        pcbLive.ImageAspectRatio = calcImageRatio();
            //        //if (IsInLiveMode)
            //        //if (!this.Visible)
            //        //    _camera.SetWorkingMode(CameraWorkingMode.ExternalSource);
            //        //else
            //        //    _camera.SetWorkingMode(CameraWorkingMode.Timed);
            //    }
            //}
            //catch (CameraException ex) {
            //    Log.Line(LogLevels.Error, "CamViewer.pcbLive_VisibleChanged", _camera.IP4Address + ": Error: " + ex.Message);
            //    OnCamViewerError(this, new CamViewerErrorEventArgs(_camera.IP4Address + ": " + frmBase.UIStrings.GetString("ResetError")));
            //}
        }

        public void RefreshStationStatus() {

            if (_visionSystemMgr.IsStationEnabled(Settings.Station))
                lblStationStatus.Image = global::ExactaEasy.Properties.Resources.green_on_32;
            else
                lblStationStatus.Image = global::ExactaEasy.Properties.Resources.red_on_32;

        }

        void refreshData(IDataBinder paramData) {

            pgCameraParams.Hide();
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

            pgCameraParams.Show();
        }

        void getAcquisitionParam() {

            //resetCameraWarning();
            if (DataSource == null) return;
            DataSource.AcquisitionParameters.PopulateParametersInfo();
            try {
                if (actualAcquisitionParams == null && _camera.Connected)
                    actualAcquisitionParams = _camera.GetAcquisitionParameters();
                //btnApply.Enabled = true;
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "CamViewer.getAcquisitionParam", _camera.IP4Address + ": Get parametri di acquisizione fallito. Errore: " + ex.Message);
                setCameraWarning(string.Format(frmBase.UIStrings.GetString("AcquisitionGetError"), Camera.IdCamera));
            }
            if (currentParameterType == ParameterTypeEnum.Acquisition)
                refreshData(new DAL.ParametersDataBinder<Parameter>(DataSource.AcquisitionParameters, actualAcquisitionParams, AppEngine.Current.CurrentContext.UserLevel));
        }

        void getDigitizerParam() {

            //resetCameraWarning();
            DataSource.DigitizerParameters.PopulateParametersInfo();
            try {
                if (actualDigitizerParams == null)
                    actualDigitizerParams = _camera.GetDigitizerParameters();
                //btnApply.Enabled = true;
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "CamViewer.getDigitizerParam", _camera.IP4Address + ": Get parametri di abilitazione fallito. Errore: " + ex.Message);
                setCameraWarning(string.Format(frmBase.UIStrings.GetString("FeaturesGetError"), Camera.IdCamera));
            }
            if (currentParameterType == ParameterTypeEnum.Digitizer)
                refreshData(new DAL.ParametersDataBinder<Parameter>(DataSource.DigitizerParameters, actualDigitizerParams, AppEngine.Current.CurrentContext.UserLevel));
        }

        void getElaborationParameter() {

            //resetCameraWarning();
            DataSource.RecipeSimpleParameters.PopulateParametersInfo();
            try {
                if (actualElaborationParams == null)
                    actualElaborationParams = _camera.GetRecipeSimpleParameters();
                //btnApply.Enabled = true;
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "CamViewer.getElaborationParameter", _camera.IP4Address + ": Get parametri di ricetta semplici fallito. Errore: " + ex.Message);
                setCameraWarning(string.Format(frmBase.UIStrings.GetString("ElaborationGetError"), Camera.IdCamera));
            }
            if (currentParameterType == ParameterTypeEnum.Elaboration)
                refreshData(new DAL.ParametersDataBinder<Parameter>(DataSource.RecipeSimpleParameters, actualElaborationParams, AppEngine.Current.CurrentContext.UserLevel));
        }

        void getRecipeAdvancedParameter() {

            //resetCameraWarning();
            DataSource.RecipeAdvancedParameters.PopulateParametersInfo();
            try {
                if (actualRecipeAdvancedParams == null)
                    actualRecipeAdvancedParams = _camera.GetRecipeAdvancedParameters();
                //btnApply.Enabled = true;
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "CamViewer.getRecipeAdvancedParameter", _camera.IP4Address + ": Get parametri di ricetta avanzati fallito. Errore: " + ex.Message);
                setCameraWarning(string.Format(frmBase.UIStrings.GetString("RecipeAdvancedGetError"), Camera.IdCamera));
            }
            if (currentParameterType == ParameterTypeEnum.RecipeAdvanced)
                refreshData(new DAL.ParametersDataBinder<Parameter>(DataSource.RecipeAdvancedParameters, actualRecipeAdvancedParams, AppEngine.Current.CurrentContext.UserLevel));
        }

        void getStroboParameter() {

            //resetCameraWarning();
            Light dataSourceLight = null;
            if (_camera.Lights != null && _camera.Lights.Count > 0) {
                if (lightIndex < _camera.Lights.Count) {
                    dataSourceLight = DataSource.Lights.Find((Light l) => { return l.Id == _camera.Lights[lightIndex].Id; });
                    if (dataSourceLight == null) {
                        dataSourceLight = new Light();
                        DataSource.Lights.Add(dataSourceLight);
                    }
                    dataSourceLight.StroboParameters.PopulateParametersInfo();
                }
                try {
                    int numberOfLights = _camera.Lights.Count;
                    if (numberOfLights > 0 && lightIndex < numberOfLights) {
                        //if (actualROIParams == null)
                        actualStroboParams = _camera.GetStrobeParameters(_camera.Lights[lightIndex].Id);
                        //btnApply.Enabled = true;
                    }
                }
                catch (Exception ex) {
                    Log.Line(LogLevels.Error, "CamViewer.getStroboParameter", _camera.IP4Address + ": Get parametri di strobo fallito. Errore: " + ex.Message);
                    setCameraWarning(ex.Message);
                }
                if (currentParameterType == ParameterTypeEnum.Strobo)
                    if (dataSourceLight != null)
                        refreshData(new DAL.ParametersDataBinder<Parameter>(dataSourceLight.StroboParameters, actualStroboParams, AppEngine.Current.CurrentContext.UserLevel));
            }
            else {
                DataSource.StroboParameters.PopulateParametersInfo();
                try {
                    if (actualStroboParams == null)
                        actualStroboParams = _camera.GetStrobeParameters(Settings);
                    //btnApply.Enabled = true;
                }
                catch (Exception ex) {
                    Log.Line(LogLevels.Error, "CamViewer.getStroboParameter", _camera.IP4Address + ": Get parametri di strobo fallito. Errore: " + ex.Message);
                    setCameraWarning(ex.Message);
                }
                if (currentParameterType == ParameterTypeEnum.Strobo)
                    refreshData(new DAL.ParametersDataBinder<Parameter>(DataSource.StroboParameters, actualStroboParams, AppEngine.Current.CurrentContext.UserLevel));
            }
        }

        public void getROIParameter() {

            ////resetCameraWarning();
            //if (roiIndex < DataSource.ROIParameters.Count)
            //    DataSource.ROIParameters[roiIndex].PopulateParametersInfo();
            ////roiIndex = nextRoiIndex;
            //try {
            //    int numberOfROI = _camera.ROICount;
            //    if (numberOfROI > 0 && roiIndex < numberOfROI) {
            //        //if (actualROIParams == null)
            //        actualROIParams = _camera.GetROIParameters(roiIndex);
            //        //btnApply.Enabled = true;
            //    }
            //}
            //catch (Exception ex) {
            //    Log.Line(LogLevels.Error, "CamViewer.getROIParameter", _camera.IP4Address + ": Get parametri di ROI fallito. Errore: " + ex.Message);
            //    setCameraWarning();
            //}
            //if (roiIndex < DataSource.ROIParameters.Count && currentParameterType == ParameterTypeEnum.ROI) {
            //    refreshData(new DAL.ParametersDataBinder<Parameter>(DataSource.ROIParameters[roiIndex], actualROIParams, AppEngine.Current.CurrentContext.UserLevel));
            //}
        }

        void getMachineParameter() {

            //resetCameraWarning();
            DataSource.MachineParameters.PopulateParametersInfo();
            try {
                if (actualMachineParams == null)
                    actualMachineParams = _camera.GetMachineParameters();
                //btnApply.Enabled = true;
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "CamViewer.getMachineParameter", _camera.IP4Address + ": Get parametri macchina fallito. Errore: " + ex.Message);
                setCameraWarning();
            }
            if (currentParameterType == ParameterTypeEnum.Machine)
                refreshData(new DAL.ParametersDataBinder<Parameter>(DataSource.MachineParameters, actualMachineParams, AppEngine.Current.CurrentContext.UserLevel));
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
                isCameraOK = false;
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
                isCameraOK = true;
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

            roiIndex = (roiIndex + 1) % DataSource.ROIParameters.Count;
            currentParameterType = ParameterTypeEnum.ROI;
            loadParameters();
            //nextRoiIndex++;
            //if (nextRoiIndex >= DataSource.ROIParameters.Count)
            //    nextRoiIndex = 0;
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

            if (pgCameraParams.DataSource != null) {
                RefreshStationStatus();
                pgCameraParams.DataSource.UserLevel = AppEngine.Current.CurrentContext.UserLevel;
                loadParameters();
                RecalcButton();
            }
        }

        public void RecalcButton() {

            if (AppEngine.Current.CurrentContext == null)
                return;

            //if (AppEngine.Current.CurrentContext.UserLevel < UserLevelEnum.Optrel) {
            //    btnMachine.Visible = false;
            //    btnAdvanced.Visible = false;
            //}
            //else {
            //    btnMachine.Visible = true;
            //    btnAdvanced.Visible = true;
            //}


            if (AppEngine.Current.CurrentContext.UserLevel < UserLevelEnum.AssistantSupervisor) {
                btnStopOnCondition.Enabled = false;
                //bttDownloadImages.Visible = false;
                bttAnalysis.Enabled = false;
                bttStopAnalysis.Enabled = false;
                btnRemoteDesktop.Enabled = false;
            }
            else {
                btnStopOnCondition.Enabled = true;
                //bttDownloadImages.Visible = true;
                bttAnalysis.Enabled = true;
                bttStopAnalysis.Enabled = true;
                btnRemoteDesktop.Enabled = true;
            }

            btnSave.Enabled = false; // Nuova gestione save esterna
            //lastBtnApplyEnableValue = btnApply.Enabled;
            MachineModeEnum machineMode = AppEngine.Current.CurrentContext.MachineMode;
            if (machineMode == MachineModeEnum.Running || AppEngine.Current.CurrentContext.UserLevel < UserLevelEnum.AssistantSupervisor) {
                btnApply.Enabled = false;
                btnEdit.Enabled = false;
                btnRestore.Enabled = false;
                //cbZoom.Enabled = false;
                btnLiveStart.Enabled = false;
                btnLiveStartFF.Enabled = false;
                btnVialAxis.Enabled = false;
            }
            else {
                switch (ParameterEditingState) {
                    case ParameterEditingStates.Reading:
                    case ParameterEditingStates.Applied:
                        btnApply.Enabled = false;
                        //btnEdit.Visible = true;
                        btnEdit.Enabled = true;
                        btnRestore.Enabled = false;
                        break;
                    case ParameterEditingStates.Editing:
                        btnApply.Enabled = true;
                        btnEdit.Enabled = false;
                        //btnEdit.Visible = false;
                        btnRestore.Enabled = true;
                        break;
                    //case ParameterEditingStates.Applied:
                    //    btnApply.Enabled = true;
                    //    //btnEdit.Visible = false;
                    //    btnRestore.Enabled = true;
                    //    break;
                }
                //cbZoom.Enabled = true;
                //btnLiveStart.Enabled = _camera.ROIModeAvailable;
                //btnLiveStartFF.Enabled = !_camera.ROIModeAvailable;
                btnVialAxis.Enabled = true;
                //ManageLiveButton();
            }
            //pier: scommentare per gestire no live su stop on condition
            //CameraProcessingMode procMode = _camera.GetCameraProcessingMode();
            //if (procMode == CameraProcessingMode.GoingToStopOnCondition ||
            //    procMode == CameraProcessingMode.StopOnCondition) {
            //    btnLiveStart.Enabled = false;
            //    btnLiveStartFF.Enabled = false;
            //}
            btnReset.Enabled = true; //pier: per test reset camere
        }

        public void ReloadParameters(ParameterTypeEnum parType) {

            if ((parType & ParameterTypeEnum.Acquisition) == ParameterTypeEnum.Acquisition)
                actualAcquisitionParams = null;
            if ((parType & ParameterTypeEnum.Digitizer) == ParameterTypeEnum.Digitizer)
                actualDigitizerParams = null;
            if ((parType & ParameterTypeEnum.Elaboration) == ParameterTypeEnum.Elaboration)
                actualElaborationParams = null;
            if ((parType & ParameterTypeEnum.RecipeAdvanced) == ParameterTypeEnum.RecipeAdvanced)
                actualRecipeAdvancedParams = null;
            //if ((parType & ParameterTypeEnum.ROI) == ParameterTypeEnum.ROI)
            //    actualROIParams = null;
            if ((parType & ParameterTypeEnum.Machine) == ParameterTypeEnum.Machine)
                actualMachineParams = null;
            if ((parType & ParameterTypeEnum.Strobo) == ParameterTypeEnum.Strobo)
                actualStroboParams = null;

            loadParameters(parType);
        }


        void loadParameters() {

            loadParameters(currentParameterType);
        }

        void loadParameters(ParameterTypeEnum parType) {

            //if (!isCameraReady()) {
            //    OnCamViewerMessage(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("CameraNotReady")));
            //    return;
            //}

            OnCamViewerMessage(this, new CamViewerMessageEventArgs(""));
            isCameraOK = true;
            pgCameraParams.Title = paramTitleLabel[currentParameterType];
            if ((parType & ParameterTypeEnum.Acquisition) != 0)
                getAcquisitionParam();
            if ((parType & ParameterTypeEnum.Digitizer) != 0)
                getDigitizerParam();
            if ((parType & ParameterTypeEnum.Elaboration) != 0)
                getElaborationParameter();
            if ((parType & ParameterTypeEnum.RecipeAdvanced) != 0)
                getRecipeAdvancedParameter();
            if ((parType & ParameterTypeEnum.ROI) != 0)
                getROIParameter();
            if ((parType & ParameterTypeEnum.Machine) != 0)
                getMachineParameter();
            if ((parType & ParameterTypeEnum.Strobo) != 0) {
                getStroboParameter();
                if (_camera.Lights != null && _camera.Lights.Count > 0)
                    pgCameraParams.Title += " " + _camera.Lights[lightIndex].Description;
            }

            if (currentParameterType == ParameterTypeEnum.ROI) {
                if (roiPageLabel.ContainsKey(roiIndex))
                    pgCameraParams.Title = roiPageLabel[roiIndex] + " (" + (roiIndex + 1).ToString() + "/" + DataSource.ROIParameters.Count.ToString() + ")";
                else
                    pgCameraParams.Title += " (" + (roiIndex + 1).ToString() + "/" + DataSource.ROIParameters.Count.ToString() + ")";
            }
            //if (isTattileOK)
            //    btnApply.Enabled = true;
        }

        private void btnApply_Click(object sender, EventArgs e) {

            if (AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Busy)) {
                if (applyParameters(ParameterTypeEnum.All)) {
                    ParameterEditingState = ParameterEditingStates.Applied;
                    RecalcButton();
                }
                pgCameraParams.Refresh();
                if (ParameterEditingState == ParameterEditingStates.Applied)
                    AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.ReadyToRun);
                else
                    AppEngine.Current.TrySetSupervisorInEditing();
                //AppEngine.Current.TrySetSupervisorInReadyToRun();
            }
            else
                Log.Line(LogLevels.Warning, "CamViewer.btnApply_Click", _camera.IP4Address + ": Cannot apply parameters, machine mode: " + AppEngine.Current.CurrentContext.MachineMode.ToString());
        }

        private void btnEdit_Click(object sender, EventArgs e) {

            if (AppEngine.Current.TrySetSupervisorInEditing()) {
                ParameterEditingState = ParameterEditingStates.Editing;
                RecalcButton();
            }
            else {
                OnCamViewerError(this, new CamViewerErrorEventArgs(_camera, _camera.IP4Address + ": " + frmBase.UIStrings.GetString("NoEditingAvailable")));
            }
        }

        private void btnSave_Click(object sender, EventArgs e) {
            if ((AppEngine.Current.CurrentContext.SupervisorMode == SupervisorModeEnum.ReadyToRun ||
                AppEngine.Current.CurrentContext.SupervisorMode == SupervisorModeEnum.Editing) &&
                AppEngine.Current.CurrentContext.ActiveRecipe != null) {

                if (AppEngine.Current.TrySaveRecipe()) {
                    if (AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.ReadyToRun)) {
                        ParameterEditingState = ParameterEditingStates.Reading;
                        RecalcButton();
                        AppEngine.Current.SendSupervisorMode(SupervisorModeEnum.ReadyToRun);
                    }
                    else {
                        OnCamViewerError(this, new CamViewerErrorEventArgs(_camera, _camera.IP4Address + ": " + frmBase.UIStrings.GetString("NoReadyToRunAvailable")));
                    }
                }
                else {
                    OnCamViewerError(this, new CamViewerErrorEventArgs(_camera, _camera.IP4Address + ": " + frmBase.UIStrings.GetString("SavingCurrentRecipeFailed")));
                    //OnCamViewerError(this, new CamViewerErrorEventArgs(_camera.IP4Address + frmBase.UIStrings.GetString("NoReadyToRunAvailable")));
                }
            }
        }

        private void btnRestore_Click(object sender, EventArgs e) {

            if (AppEngine.Current.CurrentContext.SupervisorMode == SupervisorModeEnum.Editing) {
                DataSource = originalDataSource.Clone(AppEngine.Current.ParametersInfo, AppEngine.Current.CurrentContext.CultureCode);
                loadParameters();
                if (AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.ReadyToRun)) {
                    ParameterEditingState = _prevParameterEditingState;
                    RecalcButton();
                }
            }
        }

        bool applyParameters(ParameterTypeEnum paramType) {

            bool ris = true;
            this.Cursor = Cursors.WaitCursor;
            OnCamViewerMessage(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("SendParam")));
            Application.DoEvents();
            pollEv.Reset();
            resetCameraWarning();
            //timerCameraStatus.Stop();
            try {
                if (_camera.Lights.Count > 0)
                    _camera.ApplyParameters(paramType, DataSource);
                else
                    _camera.ApplyParameters(Settings, paramType, DataSource);
                if ((paramType & ParameterTypeEnum.Acquisition) != 0) actualAcquisitionParams = null;
                if ((paramType & ParameterTypeEnum.Digitizer) != 0) actualDigitizerParams = null;
                if ((paramType & ParameterTypeEnum.Elaboration) != 0) actualElaborationParams = null;
                if ((paramType & ParameterTypeEnum.RecipeAdvanced) != 0) actualRecipeAdvancedParams = null;
                //if ((paramType & ParameterTypeEnum.ROI) != 0) actualROIParams = null;
                if ((paramType & ParameterTypeEnum.Machine) != 0) actualMachineParams = null;
                if ((paramType & ParameterTypeEnum.Strobo) != 0) actualStroboParams = null;
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "CamViewer.applyParameters", _camera.IP4Address + ": Error while sending parameters: " + ex.Message);
                OnCamViewerError(this, new CamViewerErrorEventArgs(_camera, _camera.IP4Address + ": " + frmBase.UIStrings.GetString("ErrorSendingParameters")));
                ris = false;
            }
            loadParameters(paramType);
            pollEv.Set();
            //timerCameraStatus.Start();
            OnCamViewerMessage(this, new CamViewerMessageEventArgs(""));
            this.Cursor = Cursors.Default;
            return ris;
        }

        private void btnLive_Click(object sender, EventArgs e) {

            if (!isCameraReady()) {
                OnCamViewerMessage(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("CameraNotReady")));
                return;
            }
            OnCamViewerMessage(this, new CamViewerMessageEventArgs(""));
            manageLiveButton();
        }

        void OnParameterEditingStatusChanged(object sender, EventArgs e) {

            if (ParameterEditingStateChanged != null)
                ParameterEditingStateChanged(sender, e);
        }

        void OnParametersUploaded(object sender, CamViewerMessageEventArgs e) {

            if (ParametersUploaded != null)
                ParametersUploaded(sender, e);
        }

        //private void btnExitLiveMenu_Click(object sender, EventArgs e) {

        //    pollEv.Reset();
        //    //timerCameraStatus.Stop();
        //    try {
        //        //if (rbtFullFrame.Checked)
        //        //    _camera.SetClipMode(CameraClipMode.None);
        //        //else
        //        //    _camera.SetClipMode(CameraClipMode.Custom);
        //    }
        //    catch (Exception ex) {
        //        Log.Line(LogLevels.Error, "CamViewer.ManageLiveButton", _camera.IP4Address + ": Error: " + ex.Message);
        //        setCameraWarning();
        //    }
        //    _camera.SetWorkingMode(CameraWorkingMode.Timed);
        //    pollEv.Set();
        //    //timerCameraStatus.Start();
        //    btnLiveStart.Image = global::ExactaEasy.Properties.Resources.media_playback_stop;
        //    lblInfo.Text = frmBase.UIStrings.GetString("LiveMode");
        //    Log.Line(LogLevels.Warning, "CamViewer.ManageLiveButton", _camera.IP4Address + ": Live Mode");
        //    btnLiveStart.Text = frmBase.UIStrings.GetString("StopLive");
        //    pnlLiveMenu.Visible = false;
        //    pnlCameraCommand.Visible = true;
        //}

        private void manageLiveButton() {

            try {
                switch (_camera.GetWorkingMode()) {
                    case CameraWorkingMode.ExternalSource:
                        //ButtonLiveStatus WantedMode = ButtonLiveStatus.Live;
                        //if (//WantedMode == ButtonLiveStatus.Live && 
                        //!imageViewer.Visible) {
                        //Point pp = new Point(0, -LiveModeMenu.Height);
                        //LiveModeMenu.Location = btnLiveStart.PointToScreen(pp);
                        try {

                            //bool aoiEn = false;
                            //AcquisitionParameter aoiEnable = actualAcquisitionParams.Find(c => c.Id == "aoiEnable");
                            //aoiEn = System.Convert.ToInt32(aoiEnable.Value) > 0 ? true : false;
                            ////LiveModeMenu.FullFrame = !aoiEn;
                            //if (aoiEn)
                            //    rbtROI.Checked = true;
                            //else
                            //    rbtFullFrame.Checked = true;
                            pollEv.Reset();
                            //timerCameraStatus.Stop();
                            _camera.SetClipMode(CameraClipMode.Custom);
                            _camera.SetWorkingMode(CameraWorkingMode.Timed);
                            pollEv.Set();
                            //timerCameraStatus.Start();
                            btnLiveStart.Image = global::ExactaEasy.Properties.Resources.media_playback_stop;
                            lblInfo.Text = frmBase.UIStrings.GetString("LiveMode");
                            Log.Line(LogLevels.Warning, "CamViewer.ManageLiveButton", _camera.IP4Address + ": Live mode ROI");
                            btnLiveStart.Text = frmBase.UIStrings.GetString("BtnLiveStop");
                            //pnlLiveMenu.Visible = false;
                            //pnlCameraCommand.Visible = true;

                            //LiveModeMenu.ShowDialog();
                            //pnlCameraCommand.Visible = false;
                            //pnlLiveMenu.Visible = true;
                            //setLiveMode(LiveModeMenu.FullFrame);
                            /* timerCameraStatus.Stop();
                            if (LiveModeMenu.FullFrame)
                                _camera.SetClipMode(CameraClipMode.None);
                            else
                                _camera.SetClipMode(CameraClipMode.Custom);
                            _camera.SetWorkingMode(CameraWorkingMode.Timed);
                            timerCameraStatus.Start();
                            btnLiveStart.Image = global::ExactaEasy.Properties.Resources.media_playback_stop;
                            lblInfo.Text = frmBase.UIStrings.GetString("LiveMode");
                            Log.Line(LogLevels.Warning, "CamViewer.ManageLiveButton", _camera.IP4Address + ": Live Mode");
                            btnLiveStart.Text = frmBase.UIStrings.GetString("StopLive"); */
                        }
                        catch (Exception ex) {
                            Log.Line(LogLevels.Error, "CamViewer.ManageLiveButton", _camera.IP4Address + ": Error: " + ex.Message);
                            setCameraWarning();
                        }
                        //}
                        break;
                    case CameraWorkingMode.Idle:
                    case CameraWorkingMode.Timed:
                        //pcbLive.DisconnectFromLiveDisplay();
                        //pcbLive.ConnectToRunningSingleCamera(Settings.Id);
                        try {
                            pollEv.Reset();
                            //pollEv.Reset();
                            //timerCameraStatus.Stop();
                            _camera.SetClipMode(CameraClipMode.None);
                            _camera.SetWorkingMode(CameraWorkingMode.ExternalSource);
                            pollEv.Set();
                            //timerCameraStatus.Start();
                            btnLiveStart.Image = global::ExactaEasy.Properties.Resources.media_playback_start;
                            lblInfo.Text = frmBase.UIStrings.GetString("RunningMode");
                            Log.Line(LogLevels.Pass, "CamViewer.ManageLiveButton", _camera.IP4Address + ": Running Mode");
                            btnLiveStart.Text = frmBase.UIStrings.GetString("BtnLiveStart");
                        }
                        catch (Exception ex) {
                            Log.Line(LogLevels.Error, "CamViewer.ManageLiveButton", _camera.IP4Address + ": Error: " + ex.Message);
                            setCameraWarning();
                        }
                        break;
                    //case CameraStatus.STOP_ON_CONDITION:
                    //case CameraStatus.START_ANALYSIS:
                    //    //if (!imageViewer.Visible) {
                    //    try {
                    //        _camera.StopAnalysisOffline(false);
                    //        btnLiveStart.Image = global::ExactaEasy.Properties.Resources.media_playback_start;
                    //        lblInfo.Text = frmBase.UIStrings.GetString("RunningMode");
                    //        Log.Line(LogLevels.Pass, "CamViewer.ManageLiveButton", _camera.IP4Address + ": Running Mode");
                    //        btnLiveStart.Text = frmBase.UIStrings.GetString("BtnStartLive");
                    //    }
                    //    catch (Exception ex) {
                    //        Log.Line(LogLevels.Error, "CamViewer.ManageLiveButton", _camera.IP4Address + ": Error: " + ex.Message);
                    //        setCameraWarning();
                    //    }
                    //    //}
                    //    break;
                    default:
                        Log.Line(LogLevels.Debug, "CamViewer.manageLiveButton", "Working mode unmanaged...");
                        break;
                }
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "CamViewer.ManageLiveButton", _camera.IP4Address + ": Error: " + ex.Message);
                setCameraWarning();
            }
            btnLiveStart.Refresh();
        }

        private void manageLiveButtonFF() {

            try {
                switch (_camera.GetWorkingMode()) {
                    case CameraWorkingMode.ExternalSource:
                        try {
                            pollEv.Reset();
                            //timerCameraStatus.Stop();
                            _camera.SetClipMode(CameraClipMode.Full);
                            _camera.SetWorkingMode(CameraWorkingMode.Timed);
                            pollEv.Set();
                            //timerCameraStatus.Start();
                            btnLiveStartFF.Image = global::ExactaEasy.Properties.Resources.media_playback_stop;
                            lblInfo.Text = frmBase.UIStrings.GetString("LiveModeFF");
                            Log.Line(LogLevels.Warning, "CamViewer.ManageLiveButton", _camera.IP4Address + ": Live mode fullframe");
                            btnLiveStartFF.Text = frmBase.UIStrings.GetString("BtnLiveStopFF");
                        }
                        catch (CameraException ex) {
                            Log.Line(LogLevels.Error, "CamViewer.ManageLiveButton", _camera.IP4Address + ": Error: " + ex.Message);
                            setCameraWarning();
                        }
                        //}
                        break;
                    case CameraWorkingMode.Idle:
                    case CameraWorkingMode.Timed:
                        try {
                            pollEv.Reset();
                            //timerCameraStatus.Stop();
                            _camera.SetClipMode(CameraClipMode.None);
                            _camera.SetWorkingMode(CameraWorkingMode.ExternalSource);
                            pollEv.Set();
                            //timerCameraStatus.Start();
                            btnLiveStartFF.Image = global::ExactaEasy.Properties.Resources.media_playback_start;
                            lblInfo.Text = frmBase.UIStrings.GetString("RunningMode");
                            Log.Line(LogLevels.Pass, "CamViewer.ManageLiveButton", _camera.IP4Address + ": Running mode");
                            btnLiveStartFF.Text = frmBase.UIStrings.GetString("BtnLiveStartFF");
                        }
                        catch (Exception ex) {
                            Log.Line(LogLevels.Error, "CamViewer.ManageLiveButton", _camera.IP4Address + ": Error: " + ex.Message);
                            setCameraWarning();
                        }
                        break;
                    default:
                        Log.Line(LogLevels.Debug, "CamViewer.manageLiveButtonFF", "Working mode unmanaged...");
                        break;
                }
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "CamViewer.ManageLiveButton", _camera.IP4Address + ": Error: " + ex.Message);
                setCameraWarning();
            }
            btnLiveStartFF.Refresh();

            //19.05 GR: non lascia la possibilità di modificare parametri mentre la camera è in live
            // DA SPOSTARE NELLA GESTIONE TASTI????
            CameraWorkingMode camWorkMode = _camera.GetWorkingMode();
            if (camWorkMode == CameraWorkingMode.Timed) {
                btnEdit.Enabled = false;
                btnApply.Enabled = false;
            }
            else {
                btnEdit.Enabled = true;
            }

        }

        void LiveModeMenu_ConditionUpdated(object sender, EventArgs e) {
        }

        private void CamViewer_Load(object sender, EventArgs e) {
        }

        public void StartRunningVisualization() {

            try {
                pollEv.Set();
                lblInfo.Text = frmBase.UIStrings.GetString("RunningMode");
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "CamViewer.StartRunningVisualization", _camera.IP4Address + ": Error: " + ex.Message);
            }
        }

        private void btnStopOnCondition_Click(object sender, EventArgs e) {

            if (!isCameraReady()) {
                OnCamViewerMessage(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("CameraNotReady")));
                return;
            }
            OnCamViewerMessage(this, new CamViewerMessageEventArgs(""));
            pnlCamControls.Visible = false;
            pnlVialAxisMenu.Visible = false;
            pnlResetMenu.Visible = false;
            pnlDownloadsMenu.Visible = false;
            pnlStopOnCond.Visible = true;
            stopOnCond1.Visible = true;
            stopOnCond1.SetCamera(_camera);
            stopOnCond1.UncheckedAllButton();
        }

        void stopOnCond1_VisibleChanged(object sender, EventArgs e) {

            if (!stopOnCond1.Visible) {
                pnlStopOnCond.Visible = false;
                pnlCamControls.Visible = true;
            }
        }

        void vialAxisMenu_VisibleChanged(object sender, EventArgs e) {

            if (!vialAxisMenu.Visible) {
                pnlVialAxisMenu.Visible = false;
                pnlCamControls.Visible = true;
            }
        }

        void resetMenu_VisibleChanged(object sender, EventArgs e) {

            if (!resetMenu.Visible) {
                pnlResetMenu.Visible = false;
                pnlCamControls.Visible = true;
            }
        }

        void downloadMenu_VisibleChanged(object sender, EventArgs e) {

            if (!downloadMenu.Visible) {
                pnlDownloadsMenu.Visible = false;
                pnlCamControls.Visible = true;
            }
        }

        void resetMenu_Error(object sender, CamViewerErrorEventArgs e) {

            OnCamViewerError(sender, e);
        }

        void resetMenu_ConditionUpdated(object sender, CamViewerMessageEventArgs e) {

            OnCamViewerMessage(sender, e);
        }

        void resetMenu_ApplyParameters(object sender, EventArgs e) {

            applyParameters(ParameterTypeEnum.All);
        }

        void downloadMenu_Error(object sender, CamViewerErrorEventArgs e) {

            OnCamViewerError(sender, e);
        }

        void downloadMenu_ConditionUpdated(object sender, CamViewerMessageEventArgs e) {

            OnCamViewerMessage(sender, e);
        }

        private void bttDownloadImages_Click(object sender, EventArgs e) {

            if (!isCameraReady()) {
                OnCamViewerMessage(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("CameraNotReady")));
                return;
            }
            OnCamViewerMessage(this, new CamViewerMessageEventArgs(""));
            pnlCamControls.Visible = false;
            pnlStopOnCond.Visible = false;
            pnlResetMenu.Visible = false;
            pnlVialAxisMenu.Visible = false;
            pnlDownloadsMenu.Visible = true;
            downloadMenu.Visible = true;
            downloadMenu.SetCamera(_camera);
            Display currDisplay = _visionSystemMgr.Displays["Cam_" + Settings.Id];
            if (currDisplay != null && currDisplay.ImageShown != null)
                downloadMenu.SetDisplay(currDisplay);
            else
                downloadMenu.BtnCurrentEnabled = false;
            CameraProcessingMode camProcessingMode = _camera.GetCameraProcessingMode();
            //if (camProcessingMode == CameraProcessingMode.StopOnCondition || camProcessingMode == CameraProcessingMode.OfflineAnalysis)
            downloadMenu.BtnFramesEnabled = true;
            //else
            //    downloadMenu.BtnFramesEnabled = false;

        }

        void stopOnCond1_ConditionUpdated(object sender, EventArgs e) {

            try {
                if (_camera.GetCameraProcessingMode() == CameraProcessingMode.Processing) {
                    btnLiveStart.Image = global::ExactaEasy.Properties.Resources.media_playback_stop;
                }
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "CamViewer.StopOnCondition_ConditionUpdated", _camera.IP4Address + ": Get camera status fallito. Errore: " + ex.Message);
                setCameraWarning();
            }
        }



        void vialAxisMenu_ConditionUpdated(object sender, CamViewerMessageEventArgs e) {

            OnCamViewerMessage(sender, e);
        }

        private void bttAnalysis_Click(object sender, EventArgs e) {

            if (ParameterEditingState == ParameterEditingStates.Editing) {
                btnApply_Click(sender, e);
            }

            if (!isCameraReady()) {
                OnCamViewerMessage(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("CameraNotReady")));
                return;
            }
            OnCamViewerMessage(this, new CamViewerMessageEventArgs(""));
            try {
                CameraProcessingMode camProcessingMode = _camera.GetCameraProcessingMode();

                //if (camProcessingMode == CameraProcessingMode.StopOnCondition ||
                //    camProcessingMode == CameraProcessingMode.OfflineAnalysis) {

                _camera.StartAnalysisOffline();
                lblInfo.Text = frmBase.UIStrings.GetString("StartOfflineAnalysis");
                //}
                Thread.Sleep(1000);
                ReloadParameters(ParameterTypeEnum.All);
            }
            catch (CameraException ex) {
                Log.Line(LogLevels.Error, "CamViewer.bttAnalysis_Click", _camera.IP4Address + ": Error: " + ex.Message);
                setCameraWarning();
            }
            //QUI C'ERA UN manageLiveButtonFF....perchè?????aveva chiesto Ale????
        }

        private void bttStopAnalysis_Click(object sender, EventArgs e) {

            if (!isCameraReady()) {
                OnCamViewerMessage(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("CameraNotReady")));
                return;
            }
            OnCamViewerMessage(this, new CamViewerMessageEventArgs(""));
            try {
                //CameraProcessingMode camProcessingMode = _camera.GetCameraProcessingMode();

                //if (camProcessingMode == CameraProcessingMode.StopOnCondition ||
                //    camProcessingMode == CameraProcessingMode.OfflineAnalysis) {

                _camera.StopAnalysisOffline();
                lblInfo.Text = frmBase.UIStrings.GetString("StopOfflineAnalysis");
                //}
                //Thread.Sleep(1000);
                //ReloadParameters(ParameterTypeEnum.All);
            }
            catch (CameraException ex) {
                Log.Line(LogLevels.Error, "CamViewer.bttStopAnalysis_Click", _camera.IP4Address + ": Error: " + ex.Message);
                setCameraWarning();
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
        }

        private void pnlCenter_Resize(object sender, EventArgs e) {

            //pcbLive.Height = pnlCenter.Height;
            //if (pnlCenter.Width > pcbLive.Width)
            //    pcbLive.Left = (pnlCenter.Width - pcbLive.Width) / 2;
            //else
            //    pcbLive.Left = 0;
            if (_visionSystemMgr.Displays != null && _visionSystemMgr.Displays.Count > 0 && _visionSystemMgr.Displays["Cam_" + Settings.Id] != null)
                _visionSystemMgr.Displays["Cam_" + Settings.Id].Resize(pnlCenter.Width, pnlCenter.Height);
        }

        enum ButtonLiveStatus {
            Run,
            Live,
            StopOnCondition
        }

        private void lblCameraStatus_Click(object sender, EventArgs e) {

            if (AppEngine.Current.CurrentContext.UserLevel == UserLevelEnum.Administrator) {
                DataSource.Enabled = !DataSource.Enabled;
                setCameraStatus();
            }
        }

        void setCameraStatus() 
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(lblCameraStatus))
                return;

            if (lblCameraStatus.InvokeRequired && lblCameraStatus.IsHandleCreated) {
                lblCameraStatus.Invoke(new MethodInvoker(setCameraStatus));
            }
            else {
                CameraNewStatus camStatus = _camera.GetCameraStatus();
                if (DataSource != null && DataSource.Enabled && isCameraOK && (camStatus == CameraNewStatus.Ready))
                    lblCameraStatus.Image = global::ExactaEasy.Properties.Resources.green_on_32;
                else if (DataSource != null && DataSource.Enabled && (!isCameraOK || (camStatus != CameraNewStatus.Ready)))
                    lblCameraStatus.Image = global::ExactaEasy.Properties.Resources.amber_on_32;
                else
                    lblCameraStatus.Image = global::ExactaEasy.Properties.Resources.red_on_32;
            }
        }

        void statusThread() {

            //bool statusError = false;
            while (true) {
                try {
                    int res = WaitHandle.WaitAny(statusThEvts);
                    StatusEvents ret = (StatusEvents)Enum.Parse(typeof(StatusEvents), res.ToString());
                    if (ret == StatusEvents.Kill) return; //kill ev
                    CameraNewStatus statusCamera = _camera.GetCameraStatus();
                    CameraProcessingMode camProcessingMode = _camera.GetCameraProcessingMode();
                    CameraWorkingMode camWorkingMode = _camera.GetWorkingMode();
                    Color textColor = Color.Black;
                    if (statusCamera != CameraNewStatus.Ready || camProcessingMode != CameraProcessingMode.Processing || camWorkingMode != CameraWorkingMode.ExternalSource)
                        textColor = Color.Red;
                    setStatusInfoLabel(frmBase.UIStrings.GetString("CameraStatus") + ": " + statusCamera.ToString() + " / " +
                        //frmBase.UIStrings.GetString("CameraProcessingMode") + ": " + 
                        camProcessingMode.ToString() + " / " + camWorkingMode.ToString(), textColor);
                    //if (camProcessingMode == CameraProcessingMode.StopOnCondition)    //pier: commentato da verificare
                    //    btnLiveStart.Text = frmBase.UIStrings.GetString("StartRun");
                    Log.Line(LogLevels.Debug, "CamViewer.statusThread", "Camera status: " + statusCamera.ToString() + " / " + camProcessingMode.ToString() + " / " + camWorkingMode.ToString());
                    //if (statusError) {
                    //    statusError = false;
                    //    resetCameraWarning();
                    //}
                }
                catch {
                    Log.Line(LogLevels.Warning, "CamViewer.statusThread", "Camera status not available");
                    setCameraWarning(frmBase.UIStrings.GetString("CameraStatusNotAvailable"));
                    setStatusInfoLabel(frmBase.UIStrings.GetString("CameraStatusNotAvailable"), Color.Red);
                    //statusError = true;
                }
                Thread.Sleep(1000);
            }
        }

        bool exit = false;
        void setStatusInfoLabel(string text, Color txtColor) {
            if (exit) return;
            if (lblStatusInfo.InvokeRequired && !lblStatusInfo.IsDisposed)
                lblStatusInfo.Invoke(new MethodInvoker(() => setStatusInfoLabel(text, txtColor)));
            else {
                lblStatusInfo.ForeColor = txtColor;
                lblStatusInfo.Text = text;
            }
        }

        private void btnReset_Click(object sender, EventArgs e) {

            OnCamViewerMessage(this, new CamViewerMessageEventArgs(""));
            pnlCamControls.Visible = false;
            pnlVialAxisMenu.Visible = false;
            pnlStopOnCond.Visible = false;
            pnlDownloadsMenu.Visible = false;
            pnlResetMenu.Visible = true;
            resetMenu.Visible = true;
            resetMenu.SetCamera(_camera);
            resetMenu.SetDataSource(dataSource);
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

        private void btnLiveStartFF_Click(object sender, EventArgs e) {

            if (!isCameraReady()) {
                OnCamViewerMessage(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("CameraNotReady")));
                return;
            }
            OnCamViewerMessage(this, new CamViewerMessageEventArgs(""));
            manageLiveButtonFF();
        }

        bool isCameraReady() {
            try {
                CameraNewStatus camStatus = _camera.GetCameraStatus();
                if (camStatus != CameraNewStatus.Ready) {
                    isCameraOK = false;
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

        private void btnRemoteDesktop_Click(object sender, EventArgs e) {

            DisplayManager.FormVNCClient frmVNC = new FormVNCClient();
            frmVNC.TopMost = true;
            frmVNC.Show();
            try {
                frmVNC.Connect(Settings.IP4Address, "", "tattile", -1);
                frmVNC.BringToFront();
            }
            catch {
                OnCamViewerMessage(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("VNCConnectionError")));
            }
        }

        private void btnPrev_Click(object sender, EventArgs e) {

            ((IStation)_camera).SetPrevImage();
        }

        private void btnLast_Click(object sender, EventArgs e) {

            ((IStation)_camera).SetMainImage();
        }

        private void cbZoom_CheckedChanged(object sender, EventArgs e) {

            cbZoom.BackColor = (cbZoom.Checked) ? Color.Red : SystemColors.Control;
            Display currDisplay = _visionSystemMgr.Displays["Cam_" + Settings.Id];
            if (currDisplay != null) {
                if (!cbZoom.Checked) {
                    pcbLive.ResetZoom(currDisplay, (IStation)_camera, Settings.Rotation);
                    lblPixInfo.Text = "Pixel info";
                }
            }
        }


        private void pcbLive_MouseDown(object sender, MouseEventArgs e) {

            if (cbZoom.Checked) {
                Display currDisplay = _visionSystemMgr.Displays["Cam_" + Settings.Id];
                if (currDisplay != null)
                    pcbLive.Zoom(currDisplay, (IStation)_camera, Settings.Rotation, e);
                if (e.Button==MouseButtons.Right)
                    lblPixInfo.Text = "Pixel info";
            }
        }

        private void pcbLive_MouseMove(object sender, MouseEventArgs e) {

            if (cbZoom.Checked) {
                Display currDisplay = _visionSystemMgr.Displays["Cam_" + Settings.Id];
                if (currDisplay != null) {
                    if (currDisplay.ZoomLayerRectVisible) {
                        Point ptOffset = new Point(currDisplay.DisplayAOIUsed[0].X, currDisplay.DisplayAOIUsed[0].Y);
                        int x = Math.Min(currDisplay.ZoomLayerRect.X + ptOffset.X, e.X);
                        int y = Math.Min(currDisplay.ZoomLayerRect.Y + ptOffset.Y, e.Y);
                        int w = Math.Abs(currDisplay.ZoomLayerRect.X + ptOffset.X - e.X);
                        int h = Math.Abs(currDisplay.ZoomLayerRect.Y + ptOffset.Y - e.Y);
                        int xRel = Math.Max(0, x - ptOffset.X);
                        int yRel = Math.Max(0, y - ptOffset.Y);
                        currDisplay.ZoomLayerRect = new Rectangle(xRel, yRel, w, h);
                        currDisplay.DoRender(false);
                    }
                    lblPixInfo.Text = currDisplay.GetPixelInfo(e.Location, Settings.Rotation, (_camera as IStation).ZoomAOI);
                }
            }
        }

        private void btnVialAxis_Click(object sender, EventArgs e) {

            if (!isCameraReady()) {
                OnCamViewerMessage(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("CameraNotReady")));
                return;
            }
            OnCamViewerMessage(this, new CamViewerMessageEventArgs(""));
            pnlCamControls.Visible = false;
            pnlStopOnCond.Visible = false;
            pnlResetMenu.Visible = false;
            pnlDownloadsMenu.Visible = false;
            pnlVialAxisMenu.Visible = true;
            vialAxisMenu.Visible = true;
            vialAxisMenu.SetCamera(_camera);
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

    public class ParameterInfoEventArgs : EventArgs {

        public ParameterTypeEnum ParameterType { get; internal set; }

        public ParameterInfoEventArgs(ParameterTypeEnum parameterType) {

            ParameterType = parameterType;
        }
    }

    public class CamViewerErrorEventArgs : EventArgs {

        public ICamera Camera { get; private set; }
        public string ErrorText { get; private set; }

        public CamViewerErrorEventArgs(ICamera camera, string errorText) {

            Camera = camera;
            ErrorText = errorText;
        }
    }

    public class CamViewerMessageEventArgs : EventArgs {

        public string Message { get; private set; }
        public string Parameter { get; private set; }

        public CamViewerMessageEventArgs(string message) {

            Message = message;
            Parameter = null;
        }

        public CamViewerMessageEventArgs(string message, string parameter) {

            Message = message;
            Parameter = parameter;
        }
    }

}
