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
    public partial class MngNode : UserControl, IMng
    {
        public MngNode()
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
        public bool AllowAddSetStationNamesToNode { get => true; }
        public bool AllowRemoveStationNamesToNode { get => true; }
        public bool IsDisplay { get => false; }
        public int TypeDisplaySelected { get; set; }
        public event EventHandler<IMng> RefreshControlCommands;


        public void SetUI()
        {
            Eng.Current.GetCheckNodes(out List<NodeSetting> nodes);

            while (nodes.Count > dataGridView1.Rows.Count)
            {
                dataGridView1.Rows.Add();
            }
            while (nodes.Count < dataGridView1.Rows.Count)
            {
                dataGridView1.Rows.RemoveAt(0);
            }

            //to reset the colrs
            if (_IsInError)
            {
                Mng.SetDGVRowsDefaultMode(dataGridView1);
                _IsInError = false;
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                dataGridView1["index", i].Value = i.ToString();
                dataGridView1["id", i].Value = nodes[i].Id;
                dataGridView1["name", i].Value = nodes[i].NodeDescription;
                Mng.SetValuesComboboxCellDGV((DataGridViewComboBoxCell)dataGridView1["providername", i], NodeSetting.Providers, nodes[i].NodeProviderName);
                dataGridView1["serverip4address", i].Value = nodes[i].ServerIP4Address;
                dataGridView1["ip4address", i].Value = nodes[i].IP4Address;
                dataGridView1["port", i].Value = nodes[i].Port;
                dataGridView1["user", i].Value = nodes[i].User;
                dataGridView1["key", i].Value = nodes[i].Key;
                dataGridView1["desktoptype", i].Value = nodes[i].RemoteDesktopType;
            }
        }


        public void SendCommand(string comm)
        {
            switch (comm)
            {
                case "add":
                    Eng.Current.AddNode();
                    SetUI();
                    dataGridView1["index", dataGridView1.RowCount - 1].Selected = true;
                    break;
                case "remove":
                    Eng.Current.DeleteNode(_CurrentCell.RowIndex);
                    SetUI();
                    break;
                case "moveup":
                    if (_CurrentCell.RowIndex > 0)
                    {
                        Eng.Current.MoveNode(_CurrentCell.RowIndex, -1);
                        dataGridView1.Rows[_CurrentCell.RowIndex - 1].Cells[0].Selected = true;
                        SetUI();
                    }
                    break;
                case "movedown":
                    if (_CurrentCell.RowIndex < dataGridView1.RowCount - 1)
                    {
                        Eng.Current.MoveNode(_CurrentCell.RowIndex, +1);
                        dataGridView1.Rows[_CurrentCell.RowIndex + 1].Cells[0].Selected = true;
                        SetUI();
                    }
                    break;
                case "deleteall":
                    Eng.Current.DeleteAllNodes();
                    SetUI();
                    break;
                case "autocompleteid":
                    Eng.Current.AutoCompleteNodeId();
                    SetUI();
                    break;
                case "addsetstationnametonode":
                    Eng.Current.AddSetStationNamesToNode();
                    SetUI();
                    break;
                case "removestationnametonode":
                    Eng.Current.RemoveStationNamesToNode();
                    SetUI();
                    break;
                default: throw new ArgumentException($"unknown command: {comm}");
            }
        }


        public void SelectItemsInError(ErrorElement err)
        {
            if(err.PosIndex != null)
            {
                Mng.SetDGVRowsDefaultMode(dataGridView1);
                for (int i = 0; i < err.PosIndex.Count; i++)
                {
                    if(err.PosIndex[i] < dataGridView1.RowCount)
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
            Eng.Current.SetNode(i, 
                Mng.GetVal(dataGridView1["id", i].EditedFormattedValue),
                Mng.GetVal(dataGridView1["name", i].EditedFormattedValue),
                Mng.GetVal(dataGridView1["providername", i].EditedFormattedValue),
                Mng.GetVal(dataGridView1["serverip4address", i].EditedFormattedValue),
                Mng.GetVal(dataGridView1["ip4address", i].EditedFormattedValue),
                Mng.GetVal(dataGridView1["port", i].EditedFormattedValue),
                Mng.GetVal(dataGridView1["user", i].EditedFormattedValue),
                Mng.GetVal(dataGridView1["key", i].EditedFormattedValue),
                Mng.GetVal(dataGridView1["desktoptype", i].EditedFormattedValue)
                );

            SetUI();
        }
    }
}
