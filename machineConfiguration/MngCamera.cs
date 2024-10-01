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
    public partial class MngCamera : UserControl, IMng
    {
        public MngCamera()
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
        public bool AllowCreateFromStations { get => true; }
        public bool AllowAutoCompleteId { get => true; }
        public bool AllowAddSetStationNamesToNode { get => false; }
        public bool AllowRemoveStationNamesToNode { get => false; }
        public bool IsDisplay { get => false; }
        public int TypeDisplaySelected { get; set; }
        public event EventHandler<IMng> RefreshControlCommands;


        public void SetUI()
        {
            Eng.Current.GetCheckCameras(out List<CameraSetting> cameras);

            while (cameras.Count > dataGridView1.Rows.Count)
            {
                dataGridView1.Rows.Add();
            }
            while (cameras.Count < dataGridView1.Rows.Count)
            {
                dataGridView1.Rows.RemoveAt(0);
            }

            //to reset the colrs
            if (_IsInError)
            {
                Mng.SetDGVRowsDefaultMode(dataGridView1);
                _IsInError = false;
            }

            for (int i = 0; i < cameras.Count; i++)
            {
                dataGridView1["index", i].Value = i;
                Mng.SetValuesComboboxCellDGV((DataGridViewComboBoxCell)dataGridView1["nodename", i], Eng.Current.GetArrayAvaibleNameNodes(), Eng.Current.GetNodeDescription(cameras[i].Node));
                Mng.SetValuesComboboxCellDGV((DataGridViewComboBoxCell)dataGridView1["stationname", i], Eng.Current.GetArrayAvaibleNameStations(cameras[i].Node), Eng.Current.GetstationDescription(cameras[i].Node, cameras[i].Station));
                dataGridView1["id", i].Value = cameras[i].Id;
                dataGridView1["defect", i].Value = cameras[i].CameraDescription;
                Mng.SetValuesComboboxCellDGV((DataGridViewComboBoxCell)dataGridView1["type", i], CameraSetting.Types, cameras[i].CameraType);
                dataGridView1["rotation", i].Value = cameras[i].Rotation;
            }
        }


        public void SendCommand(string comm)
        {
            switch (comm)
            {
                case "add":
                    Eng.Current.AddCamera();
                    SetUI();
                    dataGridView1["index", dataGridView1.RowCount - 1].Selected = true;
                    break;
                case "remove":
                    Eng.Current.DeleteCamera(_CurrentCell.RowIndex);
                    SetUI();
                    break;
                case "moveup":
                    if (_CurrentCell.RowIndex > 0)
                    {
                        Eng.Current.MoveCamera(_CurrentCell.RowIndex, -1);
                        dataGridView1.Rows[_CurrentCell.RowIndex - 1].Cells[0].Selected = true;
                        SetUI();
                    }
                    break;
                case "movedown":
                    if (_CurrentCell.RowIndex < dataGridView1.RowCount - 1)
                    {
                        Eng.Current.MoveCamera(_CurrentCell.RowIndex, 1);
                        dataGridView1.Rows[_CurrentCell.RowIndex + 1].Cells[0].Selected = true;
                        SetUI();
                    }
                    break;
                case "deleteall":
                    Eng.Current.DeleteAllCameras();
                    SetUI();
                    break;
                case "createfromstations":
                    Eng.Current.CreateCamerasFromStations();
                    SetUI();
                    break;
                case "autocompleteid":
                    Eng.Current.AutoCompleteCameraId();
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
            Eng.Current.SetCamera(i,
                Mng.GetVal(dataGridView1["nodename", i].EditedFormattedValue),
                Mng.GetVal(dataGridView1["stationname", i].EditedFormattedValue),
                Mng.GetVal(dataGridView1["id", i].EditedFormattedValue),
                Mng.GetVal(dataGridView1["defect", i].EditedFormattedValue),
                Mng.GetVal(dataGridView1["type", i].EditedFormattedValue),
                Mng.GetVal(dataGridView1["rotation", i].EditedFormattedValue)
                );

            SetUI();
        }
    }
}
