using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DisplayManager;
using ExactaEasyCore;
using ExactaEasyEng;
using System.IO;
using System.Threading;
using SPAMI.Util.Logger;
using System.Xml.Linq;
using GretelClients;

namespace ExactaEasy
{
    public partial class DumpUI2Cnt : UserControl
    {
        VisionSystemManager _visionMng;
        DumpImagesUserSettings2 _currSettings;
        UserLevelEnum _currUserLevel;
        //cols
        DataGridViewTextBoxColumn _colDescription;
        DataGridViewComboBoxColumn _colSpindle;
        DataGridViewComboBoxColumn _colType;
        DataGridViewComboBoxColumn _colCondition;
        DataGridViewComboBoxColumn _colLogic;
        DataGridViewTextBoxColumn _colToSave;
        DataGridViewButtonColumn _colMoreSettings;
        //data
        List<DumpImagesUserSettings2> _listCBValues;
        List<KeyValuePair<int, string>> _dicSpindleKV;
        List<KeyValuePair<StationDumpTypes2, string>> _dicTypesKV;
        List<KeyValuePair<StationDumpConditions2, string>> _dicConditionKV;
        List<KeyValuePair<StationDumpLogics2, string>> _dicLogicKV;

        public DumpUI2Cnt(VisionSystemManager visionMng)
        {
            InitializeComponent();
            _visionMng = visionMng;
            _currSettings = null;
            _currUserLevel = AppEngine.Current.CurrentContext.UserLevel;

            _listCBValues = new List<DumpImagesUserSettings2>();

            _dicSpindleKV = new List<KeyValuePair<int, string>>();
            for(int i = -1; i < 60; i++)
            {
                if (i == 0)
                    continue;
                if (i == -1)
                    _dicSpindleKV.Add(new KeyValuePair<int, string>(i, "Any"));
                else
                    _dicSpindleKV.Add(new KeyValuePair<int, string>(i, $"{i}"));
            }
            _dicTypesKV = new List<KeyValuePair<StationDumpTypes2, string>>();
            foreach (StationDumpTypes2 val in Enum.GetValues(typeof(StationDumpTypes2)))
                _dicTypesKV.Add(new KeyValuePair<StationDumpTypes2, string>(val, val.ToString()));

            _dicConditionKV = new List<KeyValuePair<StationDumpConditions2, string>>();
            foreach (StationDumpConditions2 val in Enum.GetValues(typeof(StationDumpConditions2)))
                _dicConditionKV.Add(new KeyValuePair<StationDumpConditions2, string>(val, val.ToString()));

            _dicLogicKV = new List<KeyValuePair<StationDumpLogics2, string>>();
            foreach (StationDumpLogics2 val in Enum.GetValues(typeof(StationDumpLogics2)))
                _dicLogicKV.Add(new KeyValuePair<StationDumpLogics2, string>(val, val.ToString()));

            //events
            this.Load += DumpUI2Cnt_Load;
            this.VisibleChanged += DumpUI2Cnt_VisibleChanged;
            dgvDumpInspectionSettings.CurrentCellDirtyStateChanged += DgvDumpInspectionSettings_CurrentCellDirtyStateChanged;
            dgvDumpInspectionSettings.CellClick += DgvDumpInspectionSettings_CellClick;
            dgvDumpInspectionSettings.EditingControlShowing += DgvDumpInspectionSettings_EditingControlShowing;
            cbUserSettings.SelectedIndexChanged += CbUserSettings_SelectedIndexChanged;
            AppEngine.Current.ContextChanged += Current_ContextChanged;
        }


        private void DumpUI2Cnt_Load(object sender, EventArgs e)
        {
            //translations
            labelUsrSett.Text = frmBase.UIStrings.GetString("UserSettings").ToUpper();

            RefreshCB();
            InitDGV();
            RefreshDGV();
        }


        private void DumpUI2Cnt_VisibleChanged(object sender, EventArgs e)
        {
            RefreshCB();
            RefreshDGV();
        }



        //PRIVATE
        void RefreshCB()
        {
            cbUserSettings.SelectedIndexChanged -= CbUserSettings_SelectedIndexChanged;
            _listCBValues.Clear();
            _currSettings = null;
            DumpImagesConfiguration2 dmp = AppEngine.Current.DumpImagesConfiguration2;
            int id = dmp.CurrentUserSettingsId;
            bool isAdmin = (int)_currUserLevel >= 9 ? true : false; //admin - optrel
            if (dmp.BatchSettings != null && isAdmin)
            {
                _listCBValues.Add(dmp.BatchSettings);
                if (dmp.BatchSettings.Id == id)
                    _currSettings = dmp.BatchSettings;
            }
            foreach (DumpImagesUserSettings2 user in dmp.UserSettings)
            {
                _listCBValues.Add(user);
                if (user.Id == id)
                    _currSettings = user;
            }
            //if _currSettings = null select the first avaible
            if(_currSettings == null)
            {
                if (dmp.UserSettings.Count > 0)
                {
                    _currSettings = dmp.UserSettings[0];
                    dmp.CurrentUserSettingsId = _currSettings.Id;
                }
            }
            cbUserSettings.DataSource = null;
            cbUserSettings.DataSource = _listCBValues;
            cbUserSettings.SelectedItem = _currSettings;
            cbUserSettings.SelectedIndexChanged += CbUserSettings_SelectedIndexChanged;
        }

        void InitDGV()
        {
            dgvDumpInspectionSettings.SuspendLayout();
            dgvDumpInspectionSettings.ColumnHeadersBorderStyle = SystemFonts.MessageBoxFont.Name == "Segoe UI" ? DataGridViewHeaderBorderStyle.None : DataGridViewHeaderBorderStyle.Raised;
            dgvDumpInspectionSettings.EnableHeadersVisualStyles = false;
            dgvDumpInspectionSettings.Font = new Font("Nirmala UI", 12, FontStyle.Regular, GraphicsUnit.Point, 0);
            dgvDumpInspectionSettings.BackgroundColor = SystemColors.ControlDarkDark;//System.Drawing.SystemColors.ControlLightLight;
            dgvDumpInspectionSettings.DefaultCellStyle.BackColor = Color.LightGray;// System.Drawing.SystemColors.ControlLightLight;
            dgvDumpInspectionSettings.DefaultCellStyle.ForeColor = Color.Black;
            dgvDumpInspectionSettings.RowHeadersVisible = false;
            dgvDumpInspectionSettings.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvDumpInspectionSettings.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvDumpInspectionSettings.ResumeLayout();
            dgvDumpInspectionSettings.AutoGenerateColumns = false;
            dgvDumpInspectionSettings.MultiSelect = false;
            dgvDumpInspectionSettings.ReadOnly = false;
            dgvDumpInspectionSettings.AllowUserToAddRows = false;
            dgvDumpInspectionSettings.AllowUserToDeleteRows = false;
            dgvDumpInspectionSettings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            _colDescription = new DataGridViewTextBoxColumn
            {
                Name = "colDescription",
                HeaderText = frmBase.UIStrings.GetString("Inspection"),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft },
                Visible = true
            };
            dgvDumpInspectionSettings.Columns.Add(_colDescription);

            _colSpindle = new DataGridViewComboBoxColumn
            {
                Name = "colSpindle",
                HeaderText = frmBase.UIStrings.GetString("Spindle"),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 120,
                ReadOnly = false,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft },
                Visible = true
            };
            dgvDumpInspectionSettings.Columns.Add(_colSpindle);

            _colType = new DataGridViewComboBoxColumn
            {
                Name = "colType",
                HeaderText = frmBase.UIStrings.GetString("Type"),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 120,
                ReadOnly = false,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft },
                Visible = true
            };
            dgvDumpInspectionSettings.Columns.Add(_colType);

            _colCondition = new DataGridViewComboBoxColumn
            {
                Name = "colCondition",
                HeaderText = frmBase.UIStrings.GetString("Condition"),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 120,
                ReadOnly = false,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft },
                Visible = true
            };
            dgvDumpInspectionSettings.Columns.Add(_colCondition);

            _colLogic = new DataGridViewComboBoxColumn
            {
                Name = "colLogic",
                HeaderText = frmBase.UIStrings.GetString("Logic"),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 120,
                ReadOnly = false,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft },
                Visible = true
            };
            dgvDumpInspectionSettings.Columns.Add(_colLogic);

            _colToSave = new DataGridViewTextBoxColumn
            {
                Name = "colToSave",
                HeaderText = frmBase.UIStrings.GetString("ToSave"),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 120,
                ReadOnly = false,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft },
                Visible = true
            };
            _colToSave.ValueType = typeof(int);
            dgvDumpInspectionSettings.Columns.Add(_colToSave);

            _colMoreSettings = new DataGridViewButtonColumn()
            {
                Name = "colMoreSettings",
                HeaderText = "",// frmBase.UIStrings.GetString("MoreSettings"),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                Width = 150,
                ReadOnly = false,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter },
                Visible = true
            };
            dgvDumpInspectionSettings.Columns.Add(_colMoreSettings);

            foreach (DataGridViewColumn col in dgvDumpInspectionSettings.Columns)
            {
                col.Frozen = false;
                col.HeaderCell.Style.BackColor = Color.Black;
                col.HeaderCell.Style.ForeColor = Color.White;
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        void RefreshDGV()
        {
            if (_currSettings == null)
            {
                dgvDumpInspectionSettings.Rows.Clear();
                return;
            }

            while (_currSettings.StationsDumpSettings.Count > dgvDumpInspectionSettings.Rows.Count)
                dgvDumpInspectionSettings.Rows.Add();
            while (_currSettings.StationsDumpSettings.Count < dgvDumpInspectionSettings.Rows.Count)
                dgvDumpInspectionSettings.Rows.RemoveAt(0);

            for(int i = 0; i < dgvDumpInspectionSettings.RowCount; i++)
            {
                DataGridViewRow row = dgvDumpInspectionSettings.Rows[i];
                StationDumpSettings2 sett = _currSettings.StationsDumpSettings[i];

                //camera type
                CameraVisulizerType camType = CameraVisulizerType.Undefine;
                foreach (ICamera cam in _visionMng.Cameras)
                    if (cam != null && cam.NodeId == sett.Node && cam.StationId == sett.Station)
                        camType = cam.GetVisualizerType();

                //description
                row.Cells[_colDescription.Index].Value = sett.Description;
                //spindle
                DataGridViewComboBoxCell cmbSpindle = (DataGridViewComboBoxCell)row.Cells[_colSpindle.Index];
                cmbSpindle.DisplayMember = "Value";
                cmbSpindle.ValueMember = "Key";
                cmbSpindle.DataSource = _dicSpindleKV;
                cmbSpindle.Value = sett.ID;
                //type - set based on camType
                DataGridViewComboBoxCell cmbType = (DataGridViewComboBoxCell)row.Cells[_colType.Index];
                cmbType.DisplayMember = "Value";
                cmbType.ValueMember = "Key";
                List<KeyValuePair<StationDumpTypes2, string>> listTypeKV = new List<KeyValuePair<StationDumpTypes2, string>>();
                if (camType == CameraVisulizerType.Hvld)
                {
                    foreach (KeyValuePair<StationDumpTypes2, string> item in _dicTypesKV)
                        if (item.Key == StationDumpTypes2.Hvld)
                            listTypeKV.Add(new KeyValuePair<StationDumpTypes2, string>(item.Key, (string)item.Value.Clone()));
                    if (sett.Type != StationDumpTypes2.Hvld)
                        sett.Type = StationDumpTypes2.Hvld;
                }
                else
                {
                    foreach (KeyValuePair<StationDumpTypes2, string> item in _dicTypesKV)
                        if (item.Key != StationDumpTypes2.Hvld)
                            listTypeKV.Add(new KeyValuePair<StationDumpTypes2, string>(item.Key, (string)item.Value.Clone()));
                    if (sett.Type == StationDumpTypes2.Hvld)
                        sett.Type = StationDumpTypes2.Frames;
                }
                cmbType.DataSource = listTypeKV;
                cmbType.Value = sett.Type;
                //condition
                DataGridViewComboBoxCell cmbCondition = (DataGridViewComboBoxCell)row.Cells[_colCondition.Index];
                cmbCondition.DisplayMember = "Value";
                cmbCondition.ValueMember = "Key";
                cmbCondition.DataSource = _dicConditionKV;
                cmbCondition.Value = sett.Condition;
                //logic
                DataGridViewComboBoxCell cmbLogic = (DataGridViewComboBoxCell)row.Cells[_colLogic.Index];
                cmbLogic.DisplayMember = "Value";
                cmbLogic.ValueMember = "Key";
                cmbLogic.DataSource = _dicLogicKV;
                cmbLogic.Value = sett.Logic;
                //to save
                row.Cells[_colToSave.Index].Value = sett.ToSave;
                //btn
                row.Cells[_colMoreSettings.Index].Value = frmBase.UIStrings.GetString("MoreSettings");
            }
        }

        private void DgvDumpInspectionSettings_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            int rowIndex = dgv.CurrentCell.RowIndex;
            int colIndex = dgv.CurrentCell.ColumnIndex;
            if (_currSettings == null || rowIndex < 0 || colIndex < 0)
                return;

            StationDumpSettings2 sds = _currSettings.StationsDumpSettings[rowIndex];
            DataGridViewCell cell = dgv[colIndex, rowIndex];

            //spindle
            if (colIndex == _colSpindle.Index)
                sds.ID = (int)cell.Value;
            //type
            if (colIndex == _colType.Index)
                sds.Type = (StationDumpTypes2)cell.Value;
            //condition
            if (colIndex == _colCondition.Index)
                sds.Condition = (StationDumpConditions2)cell.Value;
            //logic
            if (colIndex == _colLogic.Index)
                sds.Logic = (StationDumpLogics2)cell.Value;
            //to save
            if (colIndex == _colToSave.Index)
            {
                sds.ToSave = (int)cell.Value;
                if (sds.ToSave < 0)
                {
                    sds.ToSave = 0;
                    RefreshDGV();
                }
            }
        }

        private void DgvDumpInspectionSettings_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_currSettings == null || e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (e.ColumnIndex == _colMoreSettings.Index) //click button
            {
                StationDumpSettings2 sds = _currSettings.StationsDumpSettings[e.RowIndex];
                DumpUI2MoreSet form = new DumpUI2MoreSet(sds);
                form.ShowDialog();
            }
        }

        private void DgvDumpInspectionSettings_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            e.Control.KeyPress -= Textbox_KeyPress;
            if(dgv.CurrentCell.ColumnIndex == _colToSave.Index)
            {
                TextBox textbox = (TextBox)e.Control;
                if(textbox != null)
                    textbox.KeyPress += Textbox_KeyPress;
            }
        }

        private void Textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8))
            {
                e.Handled = true;
                return;
            }
        }

        //CHANGE SET
        private void CbUserSettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            _currSettings = (DumpImagesUserSettings2)cb.SelectedItem;
            AppEngine.Current.DumpImagesConfiguration2.CurrentUserSettingsId = _currSettings.Id;
            RefreshDGV();
        }


        private void Current_ContextChanged(object sender, ContextChangedEventArgs e)
        {
            if (e.ContextChanges != ContextChangesEnum.UserLevel)
                return;
            UserLevelEnum actUserLevel = AppEngine.Current.CurrentContext.UserLevel;
            if(_currUserLevel != actUserLevel)
            {
                _currUserLevel = actUserLevel;
                RefreshCB();
                RefreshDGV();
            }
        }



        //PUBLIC
        public void Save(string path)
        {
            try
            {
                AppEngine.Current.DumpImagesConfiguration2.SaveXml(path);
            }
            catch(Exception ex)
            {
                Log.Line(LogLevels.Error, "DumpUI2Cnt.Save", "Error: " + ex.Message);
                MessageBox.Show(ex.Message, "ERROR WHILE SAVE", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void StartDump()
        {
            try
            {
                foreach (INode node in _visionMng.Nodes)
                {
                    List<StationDumpSettings2> list = new List<StationDumpSettings2>();
                    foreach (StationDumpSettings2 sett in _currSettings.StationsDumpSettings)
                        if (sett.Node == node.IdNode)
                            list.Add(sett);

                    node.StartImagesDump2(list);
                }
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "DumpUI2Cnt.StartDump", "Error: " + ex.Message);
                MessageBox.Show(ex.Message, "ERROR WHILE START DUMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void StopDump()
        {
            try
            {
                foreach (INode node in _visionMng.Nodes)
                {
                    node.StopImagesDump2();
                }
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "DumpUI2Cnt.StopDump", "Error: " + ex.Message);
                MessageBox.Show(ex.Message, "ERROR WHILE STOP DUMP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
