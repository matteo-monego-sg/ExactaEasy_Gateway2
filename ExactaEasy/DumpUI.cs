using DisplayManager;
using ExactaEasyCore;
using ExactaEasyEng;
using ExEyGateway;
using GretelClients;
using SPAMI.Util.Logger;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ExactaEasy
{
    public partial class DumpUI : UserControl {

        //List<CameraSetting> _cameraSettings;
        readonly VisionSystemManager _visionSystemManager;
        DumpImagesUserSettings _currSettings;
        int _currNodeId = -1, _currStatId = -1;
        // ISPEZIONI
        DataGridViewTextBoxColumn _inspectionNodeIdColumn;
        DataGridViewTextBoxColumn _inspectionStatIdColumn;
        DataGridViewTextBoxColumn _inspectionDescriptionColumn;
        DataGridViewComboBoxColumn _inspectionTypeColumn;
        DataGridViewComboBoxColumn _inspectionConditionColumn;
        DataGridViewTextBoxColumn _inspectionToSaveColumn;
        DataGridViewTextBoxColumn _inspectionSavedColumn;
        // TOOL
        DataGridViewTextBoxColumn _toolIdColumn;
        DataGridViewTextBoxColumn _toolDescriptionColumn;
        DataGridViewCheckBoxColumn _toolSelectColumn;
        readonly DumpImagesConfiguration _dumpImagesConfiguration;
        //string _addString;
        //bool _toolsAvailable;
        BackgroundWorker backgroundWorker1 = new BackgroundWorker();

        frmMain _frmMain;
        internal frmMain frmMain
        {
            set => _frmMain = value;
        }

        DumpUI2Cnt _dump2;


        public DumpUI() {
            InitializeComponent();
        }

        public DumpUI(VisionSystemManager visionSystemManager, DumpImagesConfiguration dumpImagesConfiguration/*List<CameraSetting> cameraSettings*/)
            : this() {

            _visionSystemManager = visionSystemManager;
            _visionSystemManager.SavingBatchChanged += _visionSystemManager_SavingBatchChanged;
            _dumpImagesConfiguration = dumpImagesConfiguration;
            //_toolsAvailable = toolsAvailable;
            pnlToolSettings.Visible = false; // _dumpImagesConfiguration.DumpImagesToolsAvailable;
            //_cameraSettings = cameraSettings;


            // To report progress from the background worker we need to set this property
            backgroundWorker1.WorkerReportsProgress = true;
            // This event will be raised on the worker thread when the worker starts
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            // This event will be raised when we call ReportProgress
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            backgroundWorker1.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;

            //visible type dump
            if (AppEngine.Current.MachineConfiguration.TypeDumpImages == 1)
            {
                pnlDumpSettings.Controls.Clear();
                DumpUI2Cnt dmp2 = new DumpUI2Cnt(_visionSystemManager)
                {
                    Dock = DockStyle.Fill,
                };
                _dump2 = dmp2;
                pnlDumpSettings.Controls.Add(dmp2);
            }
        }



        void _visionSystemManager_SavingBatchChanged(object sender, EventArgs e) {

            RecalcButtons();
        }

        private void DumpUI_Load(object sender, EventArgs e) {

            //STRINGHE
            lblUserSettings.Text = frmBase.UIStrings.GetString("UserSettings").ToUpper();
            btnStartDump.Text = frmBase.UIStrings.GetString("Start");
            btnStopDump.Text = frmBase.UIStrings.GetString("Stop");
            btnSaveSettings.Text = frmBase.UIStrings.GetString("SaveSettings");
            btnCaronteEnable.Text = $"Caronte {frmBase.UIStrings.GetString("Enable")}";
            btnCaronteDisable.Text = $"Caronte {frmBase.UIStrings.GetString("Disable")}";
            btnStopDump.Visible = false;
            if (pnlToolSettings.Visible) {
                InitToolsDumpSettingsTable();
            }
            if(AppEngine.Current.MachineConfiguration.TypeDumpImages == 0)
            {
                InitInspectionsDumpSettingsTable();
                _currSettings = _dumpImagesConfiguration.UserSettings.Find(us => us.Id == _dumpImagesConfiguration.CurrentUserSettingsId);
                dgvDumpInspectionSettings.DataSource = _currSettings.StationsDumpSettings;
                for (int it = 0; it < _dumpImagesConfiguration.UserSettings.Count; it++)
                {
                    DumpImagesUserSettings dius = _dumpImagesConfiguration.UserSettings[it];
                    cbUserSettings.Items.Add(dius.Label);
                    if (dius.Id == _currSettings.Id)
                        cbUserSettings.SelectedIndex = it;
                }
            }

            //_addString = "<" + frmBase.UIStrings.GetString("Add") + ">";
            //cbUserSettings.Items.Add(_addString);
            //foreach (INode n in _visionSystemManager.Nodes) {
            //    foreach (IStation s in n.Stations) {
            //        addLine(n, s);
            //    }
            //}

            //set vidible buttons to enable/disable caronte
            btnCaronteEnable.Visible = AppEngine.Current.MachineConfiguration.IsRedisCaronteEnable;
            btnCaronteDisable.Visible = AppEngine.Current.MachineConfiguration.IsRedisCaronteEnable;
        }

        void InitInspectionsDumpSettingsTable() {

            dgvDumpInspectionSettings.SuspendLayout();
            dgvDumpInspectionSettings.ColumnHeadersBorderStyle = ProperColumnHeadersBorderStyle;
            dgvDumpInspectionSettings.EnableHeadersVisualStyles = false;
            dgvDumpInspectionSettings.Font = new Font("Nirmala UI", 12, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvDumpInspectionSettings.BackgroundColor = SystemColors.ControlDarkDark;//System.Drawing.SystemColors.ControlLightLight;
            dgvDumpInspectionSettings.DefaultCellStyle.BackColor = Color.LightGray;// System.Drawing.SystemColors.ControlLightLight;
            dgvDumpInspectionSettings.DefaultCellStyle.ForeColor = Color.Black;
            //dgvResults.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(3);
            dgvDumpInspectionSettings.RowHeadersVisible = false;
            dgvDumpInspectionSettings.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvDumpInspectionSettings.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvDumpInspectionSettings.ResumeLayout();

            //dgvKnapp.VirtualMode = true;
            dgvDumpInspectionSettings.AutoGenerateColumns = false;
            dgvDumpInspectionSettings.MultiSelect = false;
            dgvDumpInspectionSettings.ReadOnly = false;
            dgvDumpInspectionSettings.AllowUserToAddRows = false;
            dgvDumpInspectionSettings.AllowUserToDeleteRows = false;
            dgvDumpInspectionSettings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //dataGrid.ScrollBars = ScrollBars.Both;

            _inspectionNodeIdColumn = new DataGridViewTextBoxColumn {
                DataPropertyName = "Node",
                Name = "Node",
                HeaderText = frmBase.UIStrings.GetString("NodeId"),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 70,
                ReadOnly = true,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight },
                Visible = false
            };
            dgvDumpInspectionSettings.Columns.Add(_inspectionNodeIdColumn);

            _inspectionStatIdColumn = new DataGridViewTextBoxColumn {
                DataPropertyName = "Id",
                Name = "Id",
                HeaderText = frmBase.UIStrings.GetString("StationId"),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 70,
                ReadOnly = true,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight },
                Visible = false
            };
            dgvDumpInspectionSettings.Columns.Add(_inspectionStatIdColumn);

            _inspectionDescriptionColumn = new DataGridViewTextBoxColumn {
                DataPropertyName = "Description",
                Name = "Description",
                HeaderText = frmBase.UIStrings.GetString("Inspection"),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft },
                Visible = true
            };
            dgvDumpInspectionSettings.Columns.Add(_inspectionDescriptionColumn);

            _inspectionTypeColumn = new DataGridViewComboBoxColumn {
                DataSource = Enum.GetValues(typeof(ImagesDumpTypes)),
                ValueType = typeof(ImagesDumpTypes),
                DataPropertyName = "Type",
                Name = "Type",
                HeaderText = frmBase.UIStrings.GetString("Type"),
                Width = 150,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                ReadOnly = false,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft },
                FlatStyle = FlatStyle.Flat,
                Visible = true
            };
            //typeCombo.Columns.Add("Name");
            dgvDumpInspectionSettings.Columns.Add(_inspectionTypeColumn);

            _inspectionConditionColumn = new DataGridViewComboBoxColumn {
                DataSource = Enum.GetValues(typeof(SaveConditions)),
                ValueType = typeof(SaveConditions),
                DataPropertyName = "Condition",
                Name = "Condition",
                HeaderText = frmBase.UIStrings.GetString("Condition"),
                Width = 150,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                ReadOnly = false,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft },
                FlatStyle = FlatStyle.Flat,
                Visible = true
            };
            dgvDumpInspectionSettings.Columns.Add(_inspectionConditionColumn);

            _inspectionToSaveColumn = new DataGridViewTextBoxColumn {
                DataPropertyName = "VialsToSave",
                Name = "VialsToSave",
                HeaderText = frmBase.UIStrings.GetString("ToSave"),
                Width = 100,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                ReadOnly = false,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft },
                Visible = true
            };
            dgvDumpInspectionSettings.Columns.Add(_inspectionToSaveColumn);

            _inspectionSavedColumn = new DataGridViewTextBoxColumn {
                DataPropertyName = "VialsSaved",
                Name = "VialsSaved",
                HeaderText = frmBase.UIStrings.GetString("Saved"),
                Width = 100,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                ReadOnly = true,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft },
                Visible = false
            };
            dgvDumpInspectionSettings.Columns.Add(_inspectionSavedColumn);

            //dataGrid.Refresh();
            foreach (DataGridViewColumn col in dgvDumpInspectionSettings.Columns) {
                col.Frozen = false;
                col.HeaderCell.Style.BackColor = Color.Black;
                col.HeaderCell.Style.ForeColor = Color.White;
                //col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

        }

        void InitToolsDumpSettingsTable() {

            dgvToolSettings.SuspendLayout();
            dgvToolSettings.ColumnHeadersBorderStyle = ProperColumnHeadersBorderStyle;
            dgvToolSettings.EnableHeadersVisualStyles = false;
            dgvToolSettings.Font = new Font("Nirmala UI", 10, FontStyle.Bold, GraphicsUnit.Point, 0);
            dgvToolSettings.BackgroundColor = Color.LightGray;//System.Drawing.SystemColors.ControlLightLight;
            dgvToolSettings.DefaultCellStyle.BackColor = Color.LightGray;// System.Drawing.SystemColors.ControlLightLight;
            dgvToolSettings.DefaultCellStyle.ForeColor = Color.Black;
            //dgvResults.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(3);
            dgvToolSettings.RowHeadersVisible = false;
            dgvToolSettings.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvToolSettings.ResumeLayout();

            //dgvKnapp.VirtualMode = true;
            dgvToolSettings.AutoGenerateColumns = false;
            dgvToolSettings.MultiSelect = false;
            dgvToolSettings.ReadOnly = false;
            dgvToolSettings.AllowUserToAddRows = false;
            dgvToolSettings.AllowUserToDeleteRows = false;
            dgvToolSettings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //dataGrid.ScrollBars = ScrollBars.Both;

            _toolIdColumn = new DataGridViewTextBoxColumn {
                DataPropertyName = "IdTool",
                Name = "IdTool",
                HeaderText = frmBase.UIStrings.GetString("ToolId"),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 70,
                ReadOnly = true,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight }
            };
            dgvToolSettings.Columns.Add(_toolIdColumn);

            _toolDescriptionColumn = new DataGridViewTextBoxColumn {
                DataPropertyName = "ToolDescription",
                Name = "ToolDescription",
                HeaderText = frmBase.UIStrings.GetString("ToolDescription"),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft }
            };
            dgvToolSettings.Columns.Add(_toolDescriptionColumn);

            _toolSelectColumn = new DataGridViewCheckBoxColumn {
                DataPropertyName = "ToolSelect",
                Name = "ToolSelect",
                HeaderText = frmBase.UIStrings.GetString("Select"),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 100,
                ReadOnly = false,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter },
                TrueValue = true,
                FalseValue = false
            };
            //_toolSelectColumn.FlatStyle = FlatStyle.Flat;
            dgvToolSettings.Columns.Add(_toolSelectColumn);

            foreach (DataGridViewColumn col in dgvToolSettings.Columns) {
                col.Frozen = false;
                col.HeaderCell.Style.BackColor = Color.Black;
                col.HeaderCell.Style.ForeColor = Color.White;
                //col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.Visible = true;
            }
        }

        //void addLine(INode node, IStation station, ICamera camera) {

        //    //DataGridViewComboBoxCell comboType = new DataGridViewComboBoxCell();
        //    //comboType.Items.AddRange(station.GetImagesDumpTypes().Values.ToList<string>());
        //    //DataGridViewComboBoxCell comboCondition = new DataGridViewComboBoxCell();
        //    //comboCondition.Items.AddRange(station.GetSaveConditions().Values.ToList<string>());

        //    object[] newLine = new object[] { 
        //                node.IdNode,
        //                station.IdStation,
        //                node.Description + " - " + station.Description,
        //                null,
        //                null,
        //                333,
        //            };
        //    //addComboBoxItems(dgvDumpInspectionSettings.Rows.Count - 1);
        //    //dgvKnapp.Sort(dgvKnapp.Columns[vialIdColumn.Name], ListSortDirection.Ascending);
        //}

        //void addComboBoxItems(int rowIndex) {

        //    int nodeId = (int)dgvDumpInspectionSettings[_inspectionNodeIdColumn.Index, rowIndex].Value;
        //    int stationId = (int)dgvDumpInspectionSettings[_inspectionStatIdColumn.Index, rowIndex].Value;
        //    IStation currStat = _visionSystemManager.Nodes[nodeId].Stations[stationId];
        //    DataGridViewComboBoxCell combo = null;
        //    //TYPES
        //    combo = this.dgvDumpInspectionSettings[_inspectionTypeColumn.Index, rowIndex] as DataGridViewComboBoxCell;
        //    combo.FlatStyle = FlatStyle.Flat;
        //    combo.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
        //    string[] typeOptions = currStat.GetImagesDumpTypes().Values.ToArray<string>();
        //    if (typeOptions.Length > 0) {
        //        combo.DataSource = typeOptions;
        //        combo.Value = typeOptions[0];
        //    }
        //    //CONDITIONS
        //    combo = this.dgvDumpInspectionSettings[_inspectionConditionColumn.Index, rowIndex] as DataGridViewComboBoxCell;
        //    combo.FlatStyle = FlatStyle.Flat;
        //    combo.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
        //    string[] conditionsOptions = currStat.GetSaveConditions().Values.ToArray<string>();
        //    if (conditionsOptions.Length > 0) {
        //        combo.DataSource = conditionsOptions;
        //        combo.Value = conditionsOptions[0];
        //    }
        //}

        static DataGridViewHeaderBorderStyle ProperColumnHeadersBorderStyle {
            get {
                return (SystemFonts.MessageBoxFont.Name == "Segoe UI") ?
                DataGridViewHeaderBorderStyle.None :
                DataGridViewHeaderBorderStyle.Raised;
            }
        }

        private void dgvDumpInspectionSettings_CellClick(object sender, DataGridViewCellEventArgs e) {

            //int nodeId = (int)dgvDumpInspectionSettings[_inspectionNodeIdColumn.Index, e.RowIndex].Value;
            //int stationId = (int)dgvDumpInspectionSettings[_inspectionStatIdColumn.Index, e.RowIndex].Value;
            //IStation currStat = _visionSystemManager.Nodes[nodeId].Stations[stationId];
            ////TYPES
            //if (e.ColumnIndex == _inspectionTypeColumn.Index) {

            //    DataGridViewComboBoxCell combo = this.dgvDumpInspectionSettings[_inspectionTypeColumn.Index, e.RowIndex] as DataGridViewComboBoxCell;
            //    string[] options = currStat.GetImagesDumpTypes().Values.ToArray<string>();
            //    combo.DataSource = options;
            //}
            ////CONDITIONS
            //if (e.ColumnIndex == _inspectionConditionColumn.Index) {

            //    DataGridViewComboBoxCell combo = this.dgvDumpInspectionSettings[_inspectionConditionColumn.Index, e.RowIndex] as DataGridViewComboBoxCell;
            //    string[] options = currStat.GetSaveConditions().Values.ToArray<string>();
            //    combo.DataSource = options;

            //}
        }

        private void btnStartDump_Click(object sender, EventArgs e) {

            if(AppEngine.Current.MachineConfiguration.TypeDumpImages == 0)
            {
                //try {
                //    if (AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Busy)) {
                //        _visionSystemManager.StartStopBatch(_currSettings, true);
                //        RecalcButtons();
                //        AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.ReadyToRun);
                //    }
                //}
                //catch {
                //    AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Error);
                //}


                //CleanImages(); //IN TEORIA GRETEL LO FA DA SOLO

                foreach (var n in _visionSystemManager.Nodes)
                {
                    var sdsList = n.Stations.Select(s => _currSettings.StationsDumpSettings.Find(ss => ss.Node == n.IdNode && ss.Id == s.IdStation)).ToList();
                    n.StartImagesDump(sdsList);
                }
            }
            else if(AppEngine.Current.MachineConfiguration.TypeDumpImages == 1)
            {
                _dump2.StartDump();
            }
        }

        private void CleanImages()
        {
            foreach (var n in _visionSystemManager.Nodes)
            {
                try
                {
                    string sourcePath = @"\\" + n.Address + @"\Images";
                    // Delete all files in a directory    
                    System.IO.DirectoryInfo di = new DirectoryInfo(sourcePath);
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                }
                catch (Exception ex)
                {
                    Log.Line(LogLevels.Error, "CleanImages", "Clean images error. "+n.Address+": " + ex.Message);
                }

                var sdsList = n.Stations.Select(s => _currSettings.StationsDumpSettings.Find(ss => ss.Node == n.IdNode && ss.Id == s.IdStation)).ToList();
                n.StartImagesDump(sdsList);
            }
        }

        private void btnStopDump_Click(object sender, EventArgs e) {

            if (AppEngine.Current.MachineConfiguration.TypeDumpImages == 0)
            {
                //try {
                //    if (AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Busy)) {
                //        _visionSystemManager.StartStopBatch(null, false);
                //        RecalcButtons();
                //        AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.ReadyToRun);
                //    }
                //}
                //catch {
                //    AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Error);
                //}
                foreach (var n in _visionSystemManager.Nodes)
                {
                    //foreach (IStation s in n.Stations) {
                    //    StationDumpSettings statDumpSettings = currSettings.StationsDumpSettings.Find(ss => ss.Node == n.IdNode && ss.Id == s.IdStation);
                    //    s.StopImagesDump();
                    //}
                    n.StopImagesDump();
                }
            }
            else if(AppEngine.Current.MachineConfiguration.TypeDumpImages == 1)
            {
                _dump2.StopDump();
            }
        }

        public void RecalcButtons() 
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (InvokeRequired && IsHandleCreated) {
                Invoke(new MethodInvoker(RecalcButtons));
            }
            else {
                btnStopDump.Visible = _visionSystemManager.SavingBatch;
                btnStartDump.Visible = !_visionSystemManager.SavingBatch;

                if (!AppEngine.Current.MachineConfiguration.SaveImagesWhileRunning)
                {
                    btnStopDump.Enabled = btnStartDump.Enabled = btnSaveSettings.Enabled = btnExportImages.Enabled = btnExportImagesDigTwin.Enabled = (AppEngine.Current.CurrentContext.MachineMode != MachineModeEnum.Running);
                    if (AppEngine.Current.CurrentContext.ActiveRecipeStatus != RecipeStatusEnum.Valid && AppEngine.Current.CurrentContext.ActiveRecipeStatus != RecipeStatusEnum.Unknown) //enabed only with recipe status != Valid and status != unknown
                        btnImportRecipeDTwin.Enabled = (AppEngine.Current.CurrentContext.MachineMode != MachineModeEnum.Running);
                    else
                        btnImportRecipeDTwin.Enabled = false;
                }
            }
        }

        private void dgvDumpInspectionSettings_SelectionChanged(object sender, EventArgs e) {

            RefreshToolSettings();
        }

        void RefreshToolSettings() {

            //_inspectionNodeIdColumn.[datagridview.CurrentCell.RowIndex];
            DataGridViewSelectedRowCollection rows = dgvDumpInspectionSettings.SelectedRows;
            if (rows.Count <= 0) return;
            var rowIndex = rows[0].Index;
            int nodeId;
            if (
                !int.TryParse(
                    dgvDumpInspectionSettings.Rows[rowIndex].Cells[_inspectionNodeIdColumn.Name].Value.ToString(),
                    out nodeId)) return;
            int statId;
            if (
                !int.TryParse(
                    dgvDumpInspectionSettings.Rows[rowIndex].Cells[_inspectionStatIdColumn.Name].Value.ToString(),
                    out statId)) return;
            StationDumpSettings statDumpSettings = _currSettings.StationsDumpSettings.Find(ss => ss.Node == nodeId && ss.Id == statId);
            //dgvToolSettings.SuspendLayout();
            _currNodeId = nodeId;
            _currStatId = statId;
            dgvToolSettings.Rows.Clear();
            if (AppEngine.Current.CurrentContext.ActiveRecipe == null ||
                AppEngine.Current.CurrentContext.ActiveRecipe.Nodes == null ||
                _currNodeId > AppEngine.Current.CurrentContext.ActiveRecipe.Nodes.Count ||
                _currStatId > AppEngine.Current.CurrentContext.ActiveRecipe.Nodes[_currNodeId].Stations.Count)
                return;
            // commentato per non mettere salvataggio su tool
            //StationRecipe stationRecipe = AppEngine.Current.CurrentContext.ActiveRecipe.Nodes[_currNodeId].Stations[_currStatId];
            //if (stationRecipe != null) {
            //    //int rowIndex = 1;
            //    for (int iTool = 0; iTool < stationRecipe.Tools.Count; iTool++) {
            //        bool selected = false;
            //        if (statDumpSettings != null) {
            //            if (!statDumpSettings.ToolsSelectedForDump.ContainsKey(iTool)) {
            //                statDumpSettings.ToolsSelectedForDump.Add(iTool, false);
            //            }
            //            else
            //                selected = statDumpSettings.ToolsSelectedForDump[iTool];
            //            dgvToolSettings.Enabled = true;
            //            //pier: to change
            //            //statDumpSettings.Condition == SaveConditions.OnToolReject;
            //        }
            //        Tool tool = stationRecipe.Tools[iTool];
            //        Parameter label = tool.ToolParameters.Find(pp => pp.Id == "Label");
            //        if (label != null) {
            //            object[] newLine =
            //            {
            //                tool.Id,
            //                label.Value,
            //                selected
            //            };
            //            dgvToolSettings.Rows.Add(newLine);
            //        }
            //    }
            //}
            //dgvToolSettings.ResumeLayout();
        }

        private void cbUserSettings_SelectedIndexChanged(object sender, EventArgs e) {

            //if (cbUserSettings.SelectedItem.ToString().StartsWith("<") && cbUserSettings.SelectedItem.ToString().EndsWith(">")) {
            //    var copySettings = _dumpImagesConfiguration.UserSettings.Find(us => us.Id == _dumpImagesConfiguration.CurrentUserSettingsId);
            //    var newSettings = copySettings.Clone();
            //    int id = -1;
            //    foreach (var dius in _dumpImagesConfiguration.UserSettings)
            //        id = Math.Max(dius.Id + 1, id);
            //    newSettings.Id = id;
            //    newSettings.Label = "Custom";
            //    cbUserSettings.Items.Insert(cbUserSettings.SelectedIndex, newSettings.Id.ToString(CultureInfo.InvariantCulture) + " - " + newSettings.Label);
            //    _dumpImagesConfiguration.UserSettings.Add(newSettings);
            //    cbUserSettings.SelectedIndex -= 1;
            //    cbUserSettings.Items.Add(addString);
            //}
            //else {
            _currSettings = _dumpImagesConfiguration.UserSettings[cbUserSettings.SelectedIndex];
            dgvDumpInspectionSettings.DataSource = _currSettings.StationsDumpSettings;
            _dumpImagesConfiguration.CurrentUserSettingsId = _currSettings.Id;
            //}
        }

        private void btnSaveSettings_Click(object sender, EventArgs e) {

            if(AppEngine.Current.MachineConfiguration.TypeDumpImages == 0)
            {
                var path = AppEngine.Current.GlobalConfig.SettingsFolder + "/dumpImagesConfig.xml";
                _dumpImagesConfiguration.SaveXml(path);
            }
            else if(AppEngine.Current.MachineConfiguration.TypeDumpImages == 1)
            {
                string path = AppEngine.Current.GlobalConfig.SettingsFolder + "/dumpImagesConfig2.xml";
                _dump2.Save(path);
            }
        }

        private void dgvDumpInspectionSettings_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e) {


            DataGridView dgv = sender as DataGridView;

            e.Control.KeyPress -= new KeyPressEventHandler(Column_KeyPress);
            if (dgv.CurrentCell.ColumnIndex == _inspectionToSaveColumn.Index) //COLONNA VALUE
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Column_KeyPress);
                }
            }

            //if (e.Control is ComboBox) {
            //    ComboBox cb = e.Control as ComboBox;
            //    cb.SelectedIndexChanged -= new EventHandler(ComboBox_SelectedIndexChanged);
            //    cb.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);
            //}
        }

        private void dgvToolSettings_CellValueChanged(object sender, DataGridViewCellEventArgs e) {

        }

        private void dgvToolSettings_CellContentClick(object sender, DataGridViewCellEventArgs e) {

            StationDumpSettings statDumpSettings = _currSettings.StationsDumpSettings.Find(ss => ss.Node == _currNodeId && ss.Id == _currStatId);
            int toolId;
            if (int.TryParse(dgvToolSettings.Rows[e.RowIndex].Cells[_toolIdColumn.Name].Value.ToString(), out toolId)) {
                statDumpSettings.ToolsSelectedForDump[toolId] = (bool)dgvToolSettings.Rows[e.RowIndex].Cells[_toolSelectColumn.Name].EditedFormattedValue;
            }
        }

        private void dgvDumpInspectionSettings_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == _inspectionToSaveColumn.Index)
            {
                try
                {
                    DataGridViewCell dataGridViewCell = dgvDumpInspectionSettings.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    StationDumpSettings statDumpSettings = _currSettings.StationsDumpSettings.Find(ss => ss.Node == _currNodeId && ss.Id == _currStatId);
                    int tmp;
                    if (int.TryParse(dataGridViewCell.Value.ToString(), out tmp))
                    {
                        if (tmp > statDumpSettings.MaxImages)
                            dataGridViewCell.Value = statDumpSettings.MaxImages;
                    }
                    else
                        dataGridViewCell.Value = statDumpSettings.MaxImages;


                    //int nodeId = (int)dgvDumpInspectionSettings[_inspectionNodeIdColumn.Index, e.RowIndex].Value;
                    //int stationId = (int)dgvDumpInspectionSettings[_inspectionStatIdColumn.Index, e.RowIndex].Value;
                    //IStation currStat = _visionSystemManager.Nodes[nodeId].Stations[stationId];
                    ////TYPES
                    //if (e.ColumnIndex == _inspectionTypeColumn.Index) {

                    //    DataGridViewComboBoxCell combo = this.dgvDumpInspectionSettings[_inspectionTypeColumn.Index, e.RowIndex] as DataGridViewComboBoxCell;
                    //    string[] options = currStat.GetImagesDumpTypes().Values.ToArray<string>();
                    //    combo.DataSource = options;
                    //}
                    ////CONDITIONS
                    //if (e.ColumnIndex == _inspectionConditionColumn.Index) {

                    //    DataGridViewComboBoxCell combo = this.dgvDumpInspectionSettings[_inspectionConditionColumn.Index, e.RowIndex] as DataGridViewComboBoxCell;
                    //    string[] options = currStat.GetSaveConditions().Values.ToArray<string>();
                    //    combo.DataSource = options;

                    //}
                }
                catch (Exception)
                {
                    DataGridViewCell dataGridViewCell = dgvDumpInspectionSettings.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    dataGridViewCell.Value = "0";

                    StationDumpSettings statDumpSettings = _currSettings.StationsDumpSettings.Find(ss => ss.Node == _currNodeId && ss.Id == _currStatId);
                    dataGridViewCell.Value = statDumpSettings.MaxImages;
                }

            }

            if (e.ColumnIndex == _inspectionConditionColumn.Index)
            {
                RefreshToolSettings();
            }
        }




        private void Column_KeyPress(object sender, KeyPressEventArgs e)
        {
            // allows 0-9, backspace

            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 ))
            {
                e.Handled = true;
                return;
            }
        }

        public string destinationPath = "";
        private void btnExportImages_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = true;
            folderBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                destinationPath = folderBrowserDialog.SelectedPath;
                if(!backgroundWorker1.IsBusy)
                {
                    if (AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Busy))
                    {
                        btnExportImages.Enabled = false;
                        btnSaveSettings.Enabled = false;
                        btnStartDump.Enabled = false;
                        btnStopDump.Enabled = false;
                        lblStatusDump.Text = "-------------------------";
                        lblStatusDump.ForeColor = SystemColors.ControlText;
                        lastError = "";

                        backgroundWorker1.RunWorkerAsync();
                    }
                }

            }
        }

         
        // On worker thread so do our thing!
        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Log.Line(LogLevels.Pass, "btnExportImages_Click", " Starting images export");


                int actualPercentage = 0;
                int countNodes = _visionSystemManager.Nodes.Count;
                double perc = 100 / countNodes;
                int percentage = Convert.ToInt32(Math.Floor(perc));

                foreach (var n in _visionSystemManager.Nodes)
                {
                 
                    string sourcePath = @"\\"+n.Address+@"\Images";
                    //string sourcePath = @"E:\COMMESSE_ARTIC\PSP992002 - VRU Gsk\IMG VRU\GRETEL 0";
                    string destPath = destinationPath + @"\" + n.Description.Replace("/", " ");
                    DirectoryCopy(sourcePath, destPath, true);
                    actualPercentage = actualPercentage + percentage;
                    backgroundWorker1.ReportProgress(actualPercentage);

                    Thread.Sleep(1000);
                }

                backgroundWorker1.ReportProgress(100);
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "btnExportImages_Click", "Images export failed. Error: " + ex.Message);
                lastError = ex.Message;
                AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Error);

            }
        }
        // Back on the 'UI' thread so we can update the progress bar
        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // The progress percentage is a property of e
            progressBar_dump.Value = e.ProgressPercentage;
        }

        string lastError = "";

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnExportImages.Enabled = true;
            btnSaveSettings.Enabled = true;
            btnStartDump.Enabled = true;
            btnStopDump.Enabled = true;
            if (progressBar_dump.Value == 100)
            {
                Log.Line(LogLevels.Pass, "btnExportImages_Click", " Finished images export");
                lblStatusDump.Text = "COMPLETED";
                lblStatusDump.ForeColor = Color.ForestGreen;
                AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.ReadyToRun);
            }
            else
            {
                //progressBar_dump.BackColor = Color.Red;
                progressBar_dump.Value = 0;
                lblStatusDump.Text = lastError.ToUpper();
                lblStatusDump.ForeColor = Color.Red;
                AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Error);
            }
        }

        private void dgvDumpInspectionSettings_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //
        }

        private void btnExportImagesDigTwin_Click(object sender, EventArgs e)
        {
            SaveFileDialog sv;

            try
            {
                if (AppEngine.Current.CurrentContext.ActiveRecipe != null)
                {
                    FolderBrowserDialog fb = new FolderBrowserDialog();
                    if(fb.ShowDialog() == DialogResult.OK)
                    {
                        string fullPathSave = null;
                        string recipeName = null;

                        //recipe
                        {
                            string recipeVersion = null;
                            if (AppEngine.Current.CurrentContext.ActiveRecipeVersion >= 0)
                                recipeVersion = $"_v{AppEngine.Current.CurrentContext.ActiveRecipeVersion}";
                            recipeName = $"{AppEngine.Current.CurrentContext.ActiveRecipe.RecipeName}{recipeVersion}";
                            fullPathSave = $"{fb.SelectedPath}\\Recipe_{recipeName}.xml";
                            if (File.Exists(fullPathSave))
                                MessageBox.Show($"File already exists: {fullPathSave}", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            else
                                AppEngine.Current.CurrentContext.ActiveRecipe.SaveXml(fullPathSave);
                        }

                        //aditional info
                        {
                            //act inspections
                            string actInspections = "";
                            foreach(bool b in AppEngine.Current.CurrentContext.EnabledStation)
                            {
                                if (b)
                                    actInspections += "1";
                                else
                                    actInspections += "0";
                            }
                            //node ip
                            List<XElement> xNodeIp = new List<XElement>();
                            foreach (NodeSetting node in AppEngine.Current.MachineConfiguration.NodeSettings)
                                xNodeIp.Add(new XElement($"Node{node.Id}", node.IP4Address));

                            XElement xele = new XElement("root",
                                                new XElement("ActiveInspections", actInspections),
                                                new XElement("Spindles", AppEngine.Current.MachineConfiguration.NumberOfSpindles),
                                                new XElement("MachineSpeed", AppEngine.Current.CurrentContext.MachineSpeed),
                                                new XElement("Ips", xNodeIp));

                            XDocument xdoc = new XDocument();
                            xdoc.Add(xele);
                            fullPathSave = $"{fb.SelectedPath}\\AdditionalInfo_{recipeName}.xml";
                            if (File.Exists(fullPathSave))
                                MessageBox.Show($"File already exists: {fullPathSave}", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            else
                                xdoc.Save(fullPathSave);
                        }

                        //csv rotation parameters
                        {
                            string csv = AppEngine.Current.CurrentContext.CSVRotationParameters;
                            if (string.IsNullOrEmpty(csv))
                            {
                                MessageBox.Show(frmBase.UIStrings.GetString("DigitalTwinNoCSV"), "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                fullPathSave = $"{fb.SelectedPath}\\RotationParameters_{recipeName}.csv";
                                if (File.Exists(fullPathSave))
                                    MessageBox.Show($"File already exists: {fullPathSave}", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                else
                                    File.WriteAllText(fullPathSave, csv);
                            }
                        }
                    }
                }
                else
                    MessageBox.Show(frmBase.UIStrings.GetString("NoActiveRecipe"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnImportRecipeDTwin_Click(object sender, EventArgs e)
        {
            OpenFileDialog op;
            SupervisorModeEnum memMode = AppEngine.Current.CurrentSupervisorMode;

            try
            {
                //recipe
                op = new OpenFileDialog();
                //op.Title = "DigitalTwin recipe";
                op.Filter = "Recipe File (*.xml)|*.xml";
                if (op.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    if (memMode != SupervisorModeEnum.Busy)
                        AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Busy);

                    Recipe recipe = Recipe.LoadFromFile(op.FileName);
                    for(int i = 0; i < recipe.Nodes.Count; i++)
                    {
                        int memId = recipe.Nodes[i].Id;
                        string memDesc = recipe.Nodes[i].Description;
                        recipe.Nodes[i] = GretelNodeBase.ReadNodeRecipeV2(recipe.Nodes[i].RawRecipe);
                        recipe.Nodes[i].Id = memId;
                        recipe.Nodes[i].Description = memDesc;
                    }

                    AppEngine.Current.SetActiveRecipe(recipe, true);
                    if (AppEngine.Current.CurrentContext.ActiveRecipe != null)
                    {
                        _frmMain.changeRecipe2HMI(true, null, false);
                        //_frmMain.headerStrip.SetErrorText("");
                    }
                    else
                        throw new ApplicationException("Active recipe is null");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (memMode != AppEngine.Current.CurrentSupervisorMode)
                AppEngine.Current.TrySetSupervisorStatus(memMode);
            Cursor.Current = Cursors.Default;
        }




        private void btnCaronteEnable_Click(object sender, EventArgs e)
        {
            _frmMain.SetCaronteEnable(true);
        }

        private void btnCaronteDisable_Click(object sender, EventArgs e)
        {
            _frmMain.SetCaronteEnable(false);
        }






        //DIRECTORY COPY
        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {

            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            FileInfo[] files = dir.GetFiles();

            // Get the files in the directory and copy them to the new location.
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }

        }

        //private void ComboBox_SelectedIndexChanged(object sender, EventArgs e) {

        //    if (sender is ComboBox) {
        //        ComboBox cb = (ComboBox)sender;
        //        dgvDumpInspectionSettings.Select();
        //        cb.Update();
        //        //string item = cb.Text;
        //        //if (item != null)
        //        //    MessageBox.Show(item);
        //        cb.SelectedIndexChanged -= new EventHandler(ComboBox_SelectedIndexChanged);
        //    }
        //}
    }
}
