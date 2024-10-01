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

namespace ExactaEasy
{
    public partial class LiveImageFilterPage : UserControl
    {
        VisionSystemManager _visionSys;
        List<KeyValuePair<LiveImageFilterMode, string>> _dataFilterMode; //use to bind dgv
        List<KeyValuePair<LiveImageFilterFrequency, string>> _dataFilterFrequency; //use to bind dgv

        DataGridViewTextBoxColumn _columnStationName;
        DataGridViewComboBoxColumn _columnMode;
        DataGridViewComboBoxColumn _columnFrequency;

        public LiveImageFilterPage(VisionSystemManager visionSystemManager)
        {
            InitializeComponent();
            _visionSys = visionSystemManager;

            _dataFilterMode = new List<KeyValuePair<LiveImageFilterMode, string>>();
            foreach (LiveImageFilterMode mode in Enum.GetValues(typeof(LiveImageFilterMode)))
            {
                string translated = frmBase.UIStrings.GetString(mode.ToString());
                if (string.IsNullOrEmpty(translated))
                    translated = "--error translated--";
                else
                    translated = translated.ToUpper();

                _dataFilterMode.Add(new KeyValuePair<LiveImageFilterMode, string>(mode, translated));
            }

            _dataFilterFrequency = new List<KeyValuePair<LiveImageFilterFrequency, string>>();
            foreach (LiveImageFilterFrequency freq in Enum.GetValues(typeof(LiveImageFilterFrequency)))
            {
                _dataFilterFrequency.Add(new KeyValuePair<LiveImageFilterFrequency, string>(freq, ((int)freq).ToString()));
            }

            //fill dgv
            FillDGV();
        }


        void FillDGV()
        {
            //clear
            dataGridViewFilterLive.Rows.Clear();
            dataGridViewFilterLive.Columns.Clear();

            //prop dgv
            dataGridViewFilterLive.SuspendLayout();
            dataGridViewFilterLive.ColumnHeadersBorderStyle = (SystemFonts.MessageBoxFont.Name == "Segoe UI") ? DataGridViewHeaderBorderStyle.None : DataGridViewHeaderBorderStyle.Raised;
            dataGridViewFilterLive.EnableHeadersVisualStyles = false;
            dataGridViewFilterLive.Font = new Font("Nirmala UI", 12, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewFilterLive.BackgroundColor = SystemColors.ControlDarkDark;//System.Drawing.SystemColors.ControlLightLight;
            dataGridViewFilterLive.DefaultCellStyle.BackColor = Color.LightGray;// System.Drawing.SystemColors.ControlLightLight;
            dataGridViewFilterLive.DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewFilterLive.RowHeadersVisible = false;
            dataGridViewFilterLive.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridViewFilterLive.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridViewFilterLive.ResumeLayout();
            dataGridViewFilterLive.AutoGenerateColumns = false;
            dataGridViewFilterLive.MultiSelect = false;
            dataGridViewFilterLive.ReadOnly = false;
            dataGridViewFilterLive.AllowUserToAddRows = false;
            dataGridViewFilterLive.AllowUserToDeleteRows = false;
            dataGridViewFilterLive.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            //columns
            _columnStationName = new DataGridViewTextBoxColumn
            {
                Name = "colStationName",
                HeaderText = frmBase.UIStrings.GetString("Station"),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 60,
                ReadOnly = true,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft },
                Visible = true,
            };
            dataGridViewFilterLive.Columns.Add(_columnStationName);

            _columnMode = new DataGridViewComboBoxColumn
            {
                Name = "colMode",
                HeaderText = frmBase.UIStrings.GetString("Mode"),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 20,
                ReadOnly = false,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft },
                Visible = true,
                FlatStyle = FlatStyle.Flat,
                //data
                ValueMember = "Key",
                DisplayMember = "Value",
                DataSource = _dataFilterMode,
            };
            dataGridViewFilterLive.Columns.Add(_columnMode);

            _columnFrequency = new DataGridViewComboBoxColumn
            {
                Name = "colFrequency",
                HeaderText = frmBase.UIStrings.GetString("Frequency"),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 20,
                ReadOnly = false,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft },
                Visible = true,
                FlatStyle = FlatStyle.Flat,
                //data
                ValueMember = "Key",
                DisplayMember = "Value",
                DataSource = _dataFilterFrequency,
            };
            dataGridViewFilterLive.Columns.Add(_columnFrequency);

            //all columns
            foreach (DataGridViewColumn col in dataGridViewFilterLive.Columns)
            {
                col.Frozen = false;
                col.HeaderCell.Style.BackColor = Color.Black;
                col.HeaderCell.Style.ForeColor = Color.White;
                //col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //rows
            foreach(Station st in _visionSys.Stations)
            {
                LiveImageFilter liveFiltrer = st.LiveImageFilter;
                dataGridViewFilterLive.Rows.Add();
                DataGridViewRow row = dataGridViewFilterLive.Rows[dataGridViewFilterLive.RowCount - 1];
                INode node = _visionSys.Nodes[st.NodeId];

                row.Cells[_columnStationName.Index].Value = $"{node.Description} - {st.Description}";
                ((DataGridViewComboBoxCell)row.Cells[_columnMode.Index]).Value = liveFiltrer.Mode;
                ((DataGridViewComboBoxCell)row.Cells[_columnFrequency.Index]).Value = liveFiltrer.Frequency;

                row.Tag = liveFiltrer; //add to the tag of row
            }

            //event
            dataGridViewFilterLive.CellValueChanged += DataGridViewFilterLive_CellValueChanged;
        }


        private void DataGridViewFilterLive_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = ((DataGridView)sender).Rows[e.RowIndex];
            if(e.ColumnIndex == _columnMode.Index)
            {
                LiveImageFilter live = (LiveImageFilter)row.Tag;
                live.Mode = (LiveImageFilterMode)row.Cells[_columnMode.Index].Value;
                live.ResetCounter();
            }
            else if (e.ColumnIndex == _columnFrequency.Index)
            {
                LiveImageFilter live = (LiveImageFilter)row.Tag;
                live.Frequency = (LiveImageFilterFrequency)row.Cells[_columnFrequency.Index].Value;
            }
        }
    }
}
