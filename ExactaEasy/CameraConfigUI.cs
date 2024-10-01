using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DisplayManager;
using ExactaEasyCore;
using ExactaEasyEng;
using ExactaEasyEng.Utilities;

namespace ExactaEasy
{
    public partial class CameraConfigUI : UserControl
    {

        //CameraSettings Cameras = new CameraCollection();
        readonly MachineConfiguration _machineConfig;
        readonly VisionSystemManager _visionSystemMgr;
        frmMain _mainForm;

        public event EventHandler StartRescan;
        public event EventHandler StopRescan;

        public CameraConfigUI(MachineConfiguration machineConfig, VisionSystemManager visionSystemMgr, frmMain mainForm) {
            InitializeComponent();
            _machineConfig = machineConfig;
            _visionSystemMgr = visionSystemMgr;
            _mainForm = mainForm;

            btnRecipe.Text = frmBase.UIStrings.GetString("Redownload");
            btnRescan.Text = frmBase.UIStrings.GetString("Refresh");


            lblSection.Text = frmBase.UIStrings.GetString("Devices");
            CameraDescription.HeaderText = frmBase.UIStrings.GetString("Devices");
            CameraIP.HeaderText = @"IP";
            CameraHWStatus.HeaderText = frmBase.UIStrings.GetString("Status");
            HMIEnable.HeaderText = frmBase.UIStrings.GetString("HMIEnable");
            RecipeEnable.HeaderText = frmBase.UIStrings.GetString("RecipeEnable");

            dgvCameras.SuspendLayout();
            dgvCameras.ColumnHeadersBorderStyle = ProperColumnHeadersBorderStyle;
            dgvCameras.Font = new Font("Nirmala UI", 12, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvCameras.BackgroundColor = SystemColors.ControlDarkDark;
            dgvCameras.DefaultCellStyle.BackColor = SystemColors.ControlDarkDark;
            dgvCameras.DefaultCellStyle.ForeColor = Color.White;//System.Drawing.SystemColors.ControlDarkDark;
            dgvCameras.DefaultCellStyle.Padding = new Padding(3);
            dgvCameras.ResumeLayout();
            TaskObserver.TheObserver.DispatchObservation += TheObserver_DispatchObservation;
            AppEngine.Current.ContextChanged += Current_ContextChanged;

            _visionSystemMgr.OnStartRescan += visionSystemMgr_OnStartRescan;
            _visionSystemMgr.RescanCompleted += visionSystemMgr_RescanCompleted;
        }

        void Current_ContextChanged(object sender, ContextChangedEventArgs e) {
            if (((e.ContextChanges & ContextChangesEnum.MachineInfo) != 0) &&
                (AppEngine.Current.CurrentContext.EnabledStation != null)) {
                for (int sAbsId = 0; sAbsId < AppEngine.Current.CurrentContext.EnabledStation.Length; sAbsId++) {
                    if (_machineConfig.StationSettings == null || sAbsId >= _machineConfig.StationSettings.Count) continue;
                    StationSetting statSet = _machineConfig.StationSettings[sAbsId];
                    foreach (CameraSetting camSetting in _machineConfig.CameraSettings) {
                        if (camSetting.Station == statSet.Id && camSetting.Node == statSet.Node)
                            UpdateEnableStatus(camSetting.Node, _visionSystemMgr.IsNodeEnabled(camSetting.Node), HMIEnable.Name);
                    }
                    //foreach (Camera camera in visionSystemMgr.Cameras) {
                    //    if (camera.StationId==stationId)
                    //        updateEnableStatus(camera.IdCamera, enable, HMIEnable.Name);
                    //}
                }
            }
        }

        static DataGridViewHeaderBorderStyle ProperColumnHeadersBorderStyle {
            get {
                return (SystemFonts.MessageBoxFont.Name == "Segoe UI") ?
                DataGridViewHeaderBorderStyle.None :
                DataGridViewHeaderBorderStyle.Raised;
            }
        }

        void TheObserver_DispatchObservation(object sender, TaskStatusEventArgs e) {
            if (sender is VisionSystemManager) {
                //Camera camera = (Camera)sender;
                UpdateTaskStatus(e.TaskID);
                //Cameras(camera.IdCamera)
            }
        }

        public void AddCamera(int cameraId, int stationId, string cameraStatusTaskId) {

            var newLine = new object[] {
                cameraId,
                ArchitectureLevel.Camera,
                cameraStatusTaskId,
                _machineConfig.CameraSettings[cameraId].IP4Address,
                _machineConfig.CameraSettings[cameraId].CameraDescription,
                GetVersion(false, -1),
                GetIcon(CheckHw(cameraId, cameraStatusTaskId, ArchitectureLevel.Camera)),     //stato camera
                GetIcon(_visionSystemMgr.IsStationEnabled(cameraId)),                         //HMI enable
                GetIcon(false),                                                               //Recipe enable
            };
            dgvCameras.Rows.Add(newLine);
        }

        public void AddNode(int nodeId, string nodeStatusTaskId) {

            //DataGridViewButtonCell connectionBtn = new DataGridViewButtonCell();
            bool hw_status = CheckHw(nodeId, nodeStatusTaskId, ArchitectureLevel.Node);
            string version = GetVersion(hw_status, nodeId);
            var newLine = new object[] {
                nodeId,
                ArchitectureLevel.Node,
                nodeStatusTaskId,
                _machineConfig.NodeSettings[nodeId].NodeDescription + "   @ " + _machineConfig.NodeSettings[nodeId].IP4Address,
                _machineConfig.NodeSettings[nodeId].NodeDescription,
                version,
                GetIcon(hw_status),                                                           //stato nodo
                GetIcon(true),                                                                //HMI enable
                GetIcon(true)                                                                 //Recipe enable
            };
            int rowIndex = dgvCameras.Rows.Add(newLine);
            //dgvCameras.Rows[rowIndex].Cells[dgvCameras.Columns["CameraIP"].Index].Value = _machineConfig.NodeSettings[nodeId].NodeDescription + "   @ " + _machineConfig.NodeSettings[nodeId].IP4Address;
            dgvCameras.Rows[rowIndex].Cells[dgvCameras.Columns["CameraIP"].Index].Tag = nodeId;
            dgvCameras.Rows[rowIndex].Cells[dgvCameras.Columns["GretelVersion"].Index].Tag = nodeId;
            paintVersionCell(rowIndex, version);
        }

        private void dgvCameras_CellClick(object sender, DataGridViewCellEventArgs e) {
            //access only if the machine is not running
            if (AppEngine.Current.CurrentContext.MachineMode == MachineModeEnum.Running)
                return;
            if (e.ColumnIndex == dgvCameras.Columns["CameraIP"].Index) {
                try {
                    int nodeId = Convert.ToInt32(dgvCameras.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag);
                    _visionSystemMgr.Nodes[nodeId].SendUserLevelClass(AppEngine.Current.CurrentContext.UserLevel);
                    if (_mainForm.InvokeRequired && _mainForm.IsHandleCreated)
                        _mainForm.Invoke(new Action(() => { _mainForm.TopMost = false; }));
                    else
                        _mainForm.TopMost = false;
                    _visionSystemMgr.Nodes[nodeId].RemoteDesktopConnect(!AppEngine.Current.MachineConfiguration.HideRemoteDesktopToolbar);
                } catch (Exception ex) {
                    _mainForm.headerStrip.ErrorText = frmBase.UIStrings.GetString("RemoteDesktopError") + " " + ex.Message;
                    _mainForm.setSupervisorOnTop();
                }
            }
        }

        void UpdateTaskStatus(string taskId) 
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(dgvCameras))
                return;

            if (dgvCameras.InvokeRequired && dgvCameras.IsHandleCreated) 
            {
                dgvCameras.Invoke(new MethodInvoker(() => UpdateTaskStatus(taskId)));
            } 
            else 
            {
                IEnumerable<DataGridViewRow> rows = dgvCameras.Rows
                    .Cast<DataGridViewRow>()
                    .Where(r => r.Cells[TaskID.Name].Value.ToString().Equals(taskId));
                if (rows.Count() > 0) {
                    DataGridViewRow row = rows.First();
                    int rowIndex = row.Index;
                    //dgvCameras.Rows[rowIndex].Cells[CameraDescription.Name].Value = machineConfig.CameraSettings[cameraId].CameraDescription;
                    //dgvCameras.Rows[rowIndex].Cells[CameraIP.Name].Value = machineConfig.CameraSettings[cameraId].IP4Address;
                    int idDevice = Convert.ToInt32(dgvCameras.Rows[rowIndex].Cells[CameraID.Name].Value);
                    //int nodeId = Convert.ToInt32(dgvCameras.Rows[rowIndex].Cells[CameraID.Name].Tag);
                    ArchitectureLevel level;
                    if (Enum.TryParse(dgvCameras.Rows[rowIndex].Cells[ArchLevel.Name].Value.ToString(), out level)) {
                        bool hw_status = CheckHw(idDevice, taskId, level);
                        dgvCameras.Rows[rowIndex].Cells[CameraHWStatus.Name].Value = GetIcon(hw_status);
                        string version = GetVersion(hw_status, idDevice);
                        paintVersionCell(rowIndex, version);
                    }
                    dgvCameras.Refresh();
                }
            }
        }

        void paintVersionCell(int rowIndex, string version) {

            dgvCameras.Rows[rowIndex].Cells[GretelVersion.Name].Value = version;
            DataGridViewCellStyle style = dgvCameras.Rows[rowIndex].Cells[GretelVersion.Name].Style;
            if (version.Contains("BETA")) {
                style.ForeColor = Color.Orange;
            } else {
                style.ForeColor = dgvCameras.DefaultCellStyle.ForeColor;
            }
            dgvCameras.Rows[rowIndex].Cells[GretelVersion.Name].Style = style;
        }

        void UpdateEnableStatus(int cameraId, bool enable, string columnName)
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(dgvCameras))
                return;

            if (dgvCameras.InvokeRequired && dgvCameras.IsHandleCreated) {
                dgvCameras.Invoke(new MethodInvoker(() => UpdateEnableStatus(cameraId, enable, columnName)));
            } else {
                IEnumerable<DataGridViewRow> rows = dgvCameras.Rows
                    .Cast<DataGridViewRow>()
                    .Where(r => Convert.ToInt32(r.Cells[CameraID.Name].Value) == cameraId);
                if (rows.Count<DataGridViewRow>() > 0) {
                    DataGridViewRow row = rows.First();
                    int rowIndex = row.Index;
                    dgvCameras.Rows[rowIndex].Cells[columnName].Value = GetIcon(enable);
                    dgvCameras.Refresh();
                }
            }
        }

        bool CheckHw(int idDevice, string taskId, ArchitectureLevel archLvl) {

            if (TaskObserver.TheObserver.Tasks[taskId] != null && TaskObserver.TheObserver.Tasks[taskId].TaskStatus == TaskStatus.Completed)
                if (archLvl == ArchitectureLevel.Camera && _visionSystemMgr.CameraReady(idDevice) == CameraNewStatus.Ready)
                    return true;
            if (archLvl == ArchitectureLevel.Node && _visionSystemMgr.Nodes[idDevice].Connected)
                return true;
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        static Bitmap GetIcon(bool status) 
        {
            switch (status) 
            {
                case false:
                    return Properties.Resources.button_fewer1;
                default:
                    return Properties.Resources.checkmark1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        string GetVersion(bool status, int nodeId) 
        {
            string res = "?";
            if (nodeId < 0)
                return res;
            switch (status) 
            {
                case false:
                    return res;
                default:
                    return _visionSystemMgr.Nodes[nodeId].Version;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void dgvCameras_SelectionChanged(object sender, EventArgs e) 
        {
            dgvCameras.ClearSelection();
        }
        /// <summary>
        /// 
        /// </summary>
        private void visionSystemMgr_OnStartRescan(object sender, EventArgs e)
        {
            OnStartRescan(this, new EventArgs());

            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (InvokeRequired && IsHandleCreated)
                Invoke(new MethodInvoker(() => visionSystemMgr_OnStartRescan(sender, e)));
            else
            {
                btnRescan.Enabled = false;
                btnRescan.BackColor = Color.Red;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void btnRescan_Click(object sender, EventArgs e) 
        {
            _visionSystemMgr.StartRescan();
        }
        /// <summary>
        /// 
        /// </summary>
        public void ChangeRecipeEnable(Recipe recipe) 
        {
            if (recipe.Nodes != null) {
                //todo
            } else {
                foreach (Cam cam in recipe.Cams) {
                    UpdateEnableStatus(cam.Id, cam.Enabled, RecipeEnable.Name);
                }
            }
        }

        protected virtual void OnStartRescan(object sender, EventArgs e) {

            if (StartRescan != null)
                StartRescan(sender, e);
        }

        protected virtual void OnStopRescan(object sender, EventArgs e) {

            if (StopRescan != null)
                StopRescan(sender, e);
        }

        void visionSystemMgr_RescanCompleted(object sender, EventArgs e) {

            OnStopRescan(sender, new EventArgs());
            setBtnRescanEnable(true);
        }

        void setBtnRescanEnable(bool enable) 
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (InvokeRequired && IsHandleCreated)
                Invoke(new MethodInvoker(() => setBtnRescanEnable(enable)));
            else
            {
                btnRescan.Enabled = enable;
                btnRescan.BackColor = enable ? Color.FromArgb(50, 50, 50) : Color.Red;
            }
        }

        private void btnRecipe_Click(object sender, EventArgs e)
        {
            if(AppEngine.Current.CurrentContext.ActiveRecipe != null)
            {
                try
                {
                    AppEngine.Current.SetActiveRecipe(AppEngine.Current.CurrentContext.ActiveRecipe, true);
                    _mainForm.headerStrip.SetErrorText("");
                }
                catch (Exception ex)
                {
                    _mainForm.headerStrip.SetErrorText("Recipe loading failed! " + ex.ToString());
                }
            }

        }
    }

    public enum ArchitectureLevel
    {
        Camera,
        Station,
        Node
    }
}
