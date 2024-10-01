using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DisplayManager;
using ExactaEasyEng;

namespace ExactaEasy {
    public partial class ResultsGrid : UserControl, IResultsGrid {

        public Label Header {
            get {
                return lblResultsHeader;
            }
        }

        public DataGridView DataGrid {
            get {
                return dgvInspectionResults;
            }
        }



        List<ITool> ListTools = new List<ITool>();

        public ResultsGrid()
        {
            InitializeComponent();
            lblResultsHeader.Text = "---";  //frmBase.UIStrings.GetString("Good")

            //passes through the tag the images of trend data save
            dgvInspectionResults.Tag = new Bitmap[] 
            {
                Properties.Resources.trendBtnSave_Uncheck,
                Properties.Resources.trendBtnSave_Uncheck_crossed,
                Properties.Resources.trendBtnSave_Check,
                Properties.Resources.trendBtnSave_Check_crossed,
                Properties.Resources.trendBtnSave_Error,
                Properties.Resources.trendBtnSave_Error_crossed,
            };

            // Matteo  - 05/02/2024: 
            if (AppEngine.Current is null || AppEngine.Current.MachineConfiguration is null)
            {
                ListTools.Add(new ToolAverage());
                ListTools.Add(new ToolMax());
                ListTools.Add(new ToolMin());
                ListTools.Add(new ToolVariance());
            }
            else
            {
                if (AppEngine.Current.MachineConfiguration.ResultAverageCalculation) ListTools.Add(new ToolAverage());
                if (AppEngine.Current.MachineConfiguration.ResultMaxCalculation) ListTools.Add(new ToolMax());
                if (AppEngine.Current.MachineConfiguration.ResultMinCalculation) ListTools.Add(new ToolMin());
                if (AppEngine.Current.MachineConfiguration.ResultMinCalculation) ListTools.Add(new ToolVariance());
            }

            foreach (ITool tool in ListTools)
            {
                dataGridViewTool.Rows.Add("", tool.Name, 0, "---", null, null);
            }

            //if there are no rows return and hide the panel
            if (dataGridViewTool.Rows.Count < 1)
            {
                panelTools.Visible = false;
                return;
            }
            else
            {
                //apparence datagridTool
                foreach (DataGridViewColumn dgvc in dataGridViewTool.Columns)
                {
                    dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvc.Frozen = false;
                    dgvc.HeaderCell.Style.BackColor = Color.Black;
                    dgvc.HeaderCell.Style.ForeColor = Color.White;
                    if (dgvc is DataGridViewTextBoxColumn) dgvc.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    if (dgvc is DataGridViewImageColumn) dgvc.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvc.ReadOnly = true;
                }

                dataGridViewTool.AutoGenerateColumns = false;
                dataGridViewTool.MultiSelect = false;
                dataGridViewTool.ReadOnly = true;
                dataGridViewTool.AllowUserToAddRows = false;
                dataGridViewTool.AllowUserToDeleteRows = false;
                dataGridViewTool.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                //dataGridViewTool.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
                //dataGridViewTool.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridViewTool.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                dataGridViewTool.ColumnHeadersHeight = 19;

                dataGridViewTool.Columns["tName"].HeaderText = frmBase.UIStrings.GetString("Name");
                dataGridViewTool.Columns["tType"].HeaderText = frmBase.UIStrings.GetString("Type");
                dataGridViewTool.Columns["tSample"].HeaderText = frmBase.UIStrings.GetString("NSample");
                dataGridViewTool.Columns["tVal"].HeaderText = frmBase.UIStrings.GetString("Value");
                dataGridViewTool.Columns["tBtnStart"].HeaderText = "";
                dataGridViewTool.Columns["tBtnStop"].HeaderText = "";


                //set size height panel based on number of rows
                panelTools.Height = dataGridViewTool.Rows.Count * dataGridViewTool.Rows[0].Height + dataGridViewTool.ColumnHeadersHeight + 5;
                dataGridViewTool.Height = panelTools.Height;
                dgvInspectionResults.CellClick += DgvInspectionResults_CellClick;
                dgvInspectionResults.CellValueChanged += DgvInspectionResults_CellValueChanged;
                dataGridViewTool.CellClick += DataGridViewTool_CellClick;
            }
        }

        int LastIndexSelected { get; set; } //last selected rown index of dgvInspectionResults

        void ChangeImageButtonsRow(bool run, int index)
        {
            if (run)
            {
                dataGridViewTool.Rows[index].Cells["tBtnStart"].Value = Properties.Resources.checkmark;
                dataGridViewTool.Rows[index].Cells["tBtnStop"].Value = Properties.Resources.button_fewer1;
            }
            else
            {
                dataGridViewTool.Rows[index].Cells["tBtnStart"].Value = Properties.Resources.checkmark1;
                dataGridViewTool.Rows[index].Cells["tBtnStop"].Value = Properties.Resources.button_fewer;
            }
        }


        private void DgvInspectionResults_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != LastIndexSelected)
            {
                if (e.RowIndex >= 0) LastIndexSelected = e.RowIndex;
                foreach (DataGridViewRow dgvr in dataGridViewTool.Rows)
                {
                    dgvr.Cells["tName"].Value = dgvInspectionResults.Rows[LastIndexSelected].Cells[0].Value;
                    dgvr.Cells["tVal"].Value = "---";
                    dgvr.Cells["tSample"].Value = 0;
                    ListTools[dgvr.Index].Run = false;
                    ChangeImageButtonsRow(false, dgvr.Index);
                }
            }
        }

        private void DataGridViewTool_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridViewTool.Columns["tBtnStart"].Index && e.RowIndex >= 0) //start
            {
                if (ListTools[e.RowIndex].Run == false)
                {
                    ListTools[e.RowIndex].Run = true;
                    dataGridViewTool.Rows[e.RowIndex].Cells["tVal"].Value = "---";
                    dataGridViewTool.Rows[e.RowIndex].Cells["tSample"].Value = 0;
                    ChangeImageButtonsRow(true, e.RowIndex);
                }
            }
            if (e.ColumnIndex == dataGridViewTool.Columns["tBtnStop"].Index && e.RowIndex >= 0) //stop
            {
                if (ListTools[e.RowIndex].Run == true)
                {
                    ListTools[e.RowIndex].Run = false;
                    ChangeImageButtonsRow(false, e.RowIndex);
                }
            }
        }

        private void DgvInspectionResults_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == 0 && LastIndexSelected <= dgvInspectionResults.RowCount - 1) //spindless
            {
                foreach (DataGridViewRow dgvr in dataGridViewTool.Rows)
                {
                    if (ListTools[dgvr.Index].Run && dgvInspectionResults.Rows[LastIndexSelected].Cells[2].Value != null)
                    {
                        dgvr.Cells["tVal"].Value = ListTools[dgvr.Index].Trig(dgvInspectionResults.Rows[LastIndexSelected].Cells[2].Value.ToString());
                        dgvr.Cells["tSample"].Value = ListTools[dgvr.Index].Sample;
                    }
                }
            }
        }

        #region ToolClasses

        internal interface ITool
        {
            string Name { get; }
            bool Run { get; set; }
            int Sample { get; }
            double Trig(string value);
        }

        internal class ToolAverage : ITool
        {
            ulong CounterTrig = 0;
            double ComulativeSum = 0;

            string ITool.Name { get { return frmBase.UIStrings.GetString("Average"); } }

            int _Sample = 0;
            int ITool.Sample
            {
                get { return _Sample; }
            }

            bool _Run = false;
            bool ITool.Run {
                get { return _Run; }
                set
                {
                    if(!_Run && value)
                    {
                        CounterTrig = 0;
                        ComulativeSum = 0;
                        _Sample = 0;
                    }
                    _Run = value;
                }
            }

            double ITool.Trig(string value)
            {
                if(Double.TryParse(value, out double val))
                {
                    CounterTrig++;
                    ComulativeSum += val;
                    _Sample++;
                }
                double res = ComulativeSum / CounterTrig;
                return Math.Round(res, 4);
            }
        }

        internal class ToolMax : ITool
        {
            double MaxValNow = Double.MinValue;

            string ITool.Name { get { return frmBase.UIStrings.GetString("MaxValue"); } }

            int _Sample = 0;
            int ITool.Sample
            {
                get { return _Sample; }
            }

            bool _Run = false;
            bool ITool.Run
            {
                get { return _Run; }
                set
                {
                    if (!_Run && value)
                    {
                        MaxValNow = Double.MinValue;
                        _Sample = 0;
                    }
                    _Run = value;
                }
            }

            double ITool.Trig(string value)
            {
                if (Double.TryParse(value, out double val))
                {
                    if (MaxValNow < val) MaxValNow = val;
                    _Sample++;
                }
                return Math.Round(MaxValNow, 4);
            }
        }

        internal class ToolMin : ITool
        {
            double MinValNow = Double.MaxValue;

            string ITool.Name { get { return frmBase.UIStrings.GetString("MinValue"); } }

            int _Sample = 0;
            int ITool.Sample
            {
                get { return _Sample; }
            }

            bool _Run = false;
            bool ITool.Run
            {
                get { return _Run; }
                set
                {
                    if (!_Run && value)
                    {
                        MinValNow = Double.MaxValue;
                        _Sample = 0;
                    }
                    _Run = value;
                }
            }

            double ITool.Trig(string value)
            {
                if (Double.TryParse(value, out double val))
                {
                    if (MinValNow > val) MinValNow = val;
                    _Sample++;
                }
                return Math.Round(MinValNow, 4);
            }
        }

        internal class ToolVariance : ITool
        {
            string ITool.Name { get { return frmBase.UIStrings.GetString("VarianceValue"); } }
            List<double> _values = new List<double>();
            double _comulativeSum = 0;
            double _average = 0;
            double _variance = 0;

            int _Sample = 0;
            int ITool.Sample
            {
                get { return _Sample; }
            }

            bool _Run = false;
            bool ITool.Run
            {
                get { return _Run; }
                set
                {
                    if (!_Run && value)
                    {
                        _values.Clear();
                        _comulativeSum = 0;
                        _average = 0;
                        _variance = 0;
                        _Sample = 0;
                    }
                    _Run = value;
                }
            }

            double ITool.Trig(string value)
            {
                if (Double.TryParse(value, out double val))
                {
                    _values.Add(val);
                    _comulativeSum += _values[_values.Count - 1];
                    _average = _comulativeSum / _values.Count;
                    double sumSquare = 0;
                    foreach (double valSingle in _values)
                        sumSquare += Math.Pow(valSingle - _average, 2);
                    _variance = sumSquare / _values.Count;

                    _Sample++;
                }
                return Math.Round(_variance, 4);
            }
        }
        #endregion
    }
}
