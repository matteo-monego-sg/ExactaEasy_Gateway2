using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using machineConfiguration.MachineConfig;

namespace machineConfiguration
{
    public partial class MngStation : UserControl, IMng
    {
        public MngStation()
        {
            InitializeComponent();
            Mng.AdjustDatagridView(dataGridView1);
        }

        DataGridViewCell _CurrentCell;
        string _ColumnNameSelected = null;
        bool _IsInError = false;


        public bool AllowAdd { get => true; }
        public bool AllowDelete { get => true; }
        public bool AllowMoveUpDown { get => true; }
        public bool AllowDeleteAll { get => true; }
        public bool AllowCreateFromStations { get => false; }
        public bool AllowAutoCompleteId { get => true; }
        public bool AllowAddSetStationNamesToNode { get => false; }
        public bool AllowRemoveStationNamesToNode { get => false; }
        public bool IsDisplay { get => false; }
        public int TypeDisplaySelected { get; set; }
        public event EventHandler<IMng> RefreshControlCommands;


        public void SetUI()
        {
            Eng.Current.GetCheckStations(out List<StationSetting> stations);

            while (stations.Count > dataGridView1.Rows.Count)
            {
                dataGridView1.Rows.Add();
            }
            while (stations.Count < dataGridView1.Rows.Count)
            {
                dataGridView1.Rows.RemoveAt(0);
            }

            //to reset the colrs
            if (_IsInError)
            {
                Mng.SetDGVRowsDefaultMode(dataGridView1);
                _IsInError = false;
            }

            for (int i = 0; i < stations.Count; i++)
            {
                dataGridView1["index", i].Value = i;
                Mng.SetValuesComboboxCellDGV((DataGridViewComboBoxCell)dataGridView1["nodename", i], Eng.Current.GetArrayAvaibleNameNodes(), Eng.Current.GetNodeDescription(stations[i].Node));
                dataGridView1["id", i].Value = stations[i].Id;
                dataGridView1["name", i].Value = stations[i].StationDescription;
                Mng.SetValuesComboboxCellDGV((DataGridViewComboBoxCell)dataGridView1["providername", i], StationSetting.Providers, stations[i].StationProviderName);
            }
        }


        public void SendCommand(string comm)
        {
            switch (comm)
            {
                case "add":
                    Eng.Current.AddStation();
                    SetUI();
                    dataGridView1["index", dataGridView1.RowCount - 1].Selected = true;
                    break;
                case "remove":
                    Eng.Current.DeleteStation(_CurrentCell.RowIndex);
                    SetUI();
                    break;
                case "moveup":
                    if (_CurrentCell.RowIndex > 0)
                    {
                        Eng.Current.MoveStation(_CurrentCell.RowIndex, -1);
                        dataGridView1.Rows[_CurrentCell.RowIndex - 1].Cells[0].Selected = true;
                        SetUI();
                    }
                    break;
                case "movedown":
                    if (_CurrentCell.RowIndex < dataGridView1.RowCount - 1)
                    {
                        Eng.Current.MoveStation(_CurrentCell.RowIndex, 1);
                        dataGridView1.Rows[_CurrentCell.RowIndex + 1].Cells[0].Selected = true;
                        SetUI();
                    }
                    break;
                case "deleteall":
                    Eng.Current.DeleteAllStations();
                    SetUI();
                    break;
                case "autocompleteid":
                    Eng.Current.AutoCompleteStationId();
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
            Eng.Current.SetStation(i,
                Mng.GetVal(dataGridView1["nodename", i].EditedFormattedValue),
                Mng.GetVal(dataGridView1["id", i].EditedFormattedValue),
                Mng.GetVal(dataGridView1["name", i].EditedFormattedValue),
                Mng.GetVal(dataGridView1["providername", i].EditedFormattedValue)
                );

            SetUI();
        }
    }
}
