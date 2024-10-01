using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using machineConfiguration.MachineConfig;

namespace machineConfiguration
{
    public partial class MngHistoricizedTools : UserControl, IMng
    {
        public MngHistoricizedTools()
        {
            InitializeComponent();
            Mng.AdjustDatagridView(dataGridView1);
        }

        DataGridViewCell _CurrentCell;
        string _ColumnNameSelected = null;
        bool _IsInError = false;

        public bool AllowAdd { get => true; }
        public bool AllowDelete { get => true; }
        public bool AllowMoveUpDown { get => false; }
        public bool AllowDeleteAll { get => true; }
        public bool AllowCreateFromStations { get => false; }
        public bool AllowAutoCompleteId { get => false; }
        public bool AllowAddSetStationNamesToNode { get => false; }
        public bool AllowRemoveStationNamesToNode { get => false; }
        public bool IsDisplay { get => false; }
        public int TypeDisplaySelected { get; set; }
        public event EventHandler<IMng> RefreshControlCommands;


        public void SetUI()
        {
            Eng.Current.GetCheckHistoricizedTools(out List<HistoricizedToolSetting> histoTools);

            while (histoTools.Count > dataGridView1.Rows.Count)
            {
                dataGridView1.Rows.Add();
            }
            while (histoTools.Count < dataGridView1.Rows.Count)
            {
                dataGridView1.Rows.RemoveAt(0);
            }

            //to reset the colrs
            if (_IsInError)
            {
                Mng.SetDGVRowsDefaultMode(dataGridView1);
                _IsInError = false;
            }

            for (int i = 0; i < histoTools.Count; i++)
            {
                dataGridView1["index", i].Value = i;
                Mng.SetValuesComboboxCellDGV((DataGridViewComboBoxCell)dataGridView1["nodename", i], Eng.Current.GetArrayAvaibleNameNodes(), Eng.Current.GetNodeDescription(histoTools[i].NodeId));
                Mng.SetValuesComboboxCellDGV((DataGridViewComboBoxCell)dataGridView1["stationname", i], Eng.Current.GetArrayAvaibleNameStations(histoTools[i].NodeId), Eng.Current.GetstationDescription(histoTools[i].NodeId, histoTools[i].StationId));
                dataGridView1["toolindex", i].Value = histoTools[i].ToolIndex;
                dataGridView1["parameterindex", i].Value = histoTools[i].ParameterIndex;
            }
        }


        public void SendCommand(string comm)
        {
            switch (comm)
            {
                case "add":
                    Eng.Current.AddHistoricizedTool();
                    SetUI();
                    dataGridView1["index", dataGridView1.RowCount - 1].Selected = true;
                    break;
                case "remove":
                    Eng.Current.DeleteHistoricizedTool(_CurrentCell.RowIndex);
                    SetUI();
                    break;
                case "deleteall":
                    Eng.Current.DeleteAllHistoricizedTools();
                    SetUI();
                    break;
                default: throw new ArgumentException($"unknown command: {comm}");
            }
        }


        public void SelectItemsInError(ErrorElement err)
        {
            if (err.PosIndex != null)
            {
                Mng.SetDGVRowsDefaultMode(dataGridView1);
                for (int i = 0; i < err.PosIndex.Count; i++)
                {
                    if (err.PosIndex[i] < dataGridView1.RowCount)
                    {
                        if (i == 0)
                        {
                            dataGridView1.FirstDisplayedScrollingRowIndex = err.PosIndex[i];
                        }
                        Mng.SetDGVRowErrorMode(dataGridView1, err.PosIndex[i]);
                    }
                }
                _IsInError = true;
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            _CurrentCell = dataGridView1[e.ColumnIndex, e.RowIndex];
            _ColumnNameSelected = dataGridView1.Columns[e.ColumnIndex].Name;

            if (_IsInError)
            {
                _IsInError = false;
                Mng.SetDGVRowsDefaultMode(dataGridView1);
            }
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            int i = _CurrentCell.RowIndex;
            Eng.Current.SetHistoricizedTool(i,
                Mng.GetVal(dataGridView1["label", i].EditedFormattedValue),
                Mng.GetVal(dataGridView1["nodename", i].EditedFormattedValue),
                Mng.GetVal(dataGridView1["stationname", i].EditedFormattedValue),
                Mng.GetVal(dataGridView1["toolindex", i].EditedFormattedValue),
                Mng.GetVal(dataGridView1["parameterindex", i].EditedFormattedValue)
                );

            SetUI();
        }
    }
}
