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
using System.Reflection;

namespace machineConfiguration
{
    public partial class MngGeneral : UserControl, IMng
    {
        public MngGeneral()
        {
            InitializeComponent();
            Mng.AdjustDatagridView(dataGridView1);
        }

        DataGridViewCell _currentCell;
        bool _IsInError = false;
        int _CountGen;


        //IMng actions
        bool _AllowAdd = true;
        public bool AllowAdd { get => _AllowAdd; }

        bool _AllowDelete = false;
        public bool AllowDelete { get => _AllowDelete; }

        bool _AllowMoveUpDown = false;
        public bool AllowMoveUpDown { get => _AllowMoveUpDown; }

        public bool AllowDeleteAll { get => false; }
        public bool AllowCreateFromStations { get => false; }
        public bool AllowAutoCompleteId { get => false; }
        public bool AllowAddSetStationNamesToNode { get => false; }
        public bool AllowRemoveStationNamesToNode { get => false; }
        public bool IsDisplay { get => false; }
        public int TypeDisplaySelected { get; set; }
        public event EventHandler<IMng> RefreshControlCommands;


        public void SetUI()
        {
            Eng.Current.GetCheckGenerals(out List<GeneralElement> generals);
            _CountGen = generals.Count;

            while (generals.Count > dataGridView1.Rows.Count)
            {
                dataGridView1.Rows.Add();

                //for bug
                if(dataGridView1.RowCount == 1)
                {
                    dataGridView1["name", 0].Selected = false;
                    dataGridView1["value", 0].Selected = true;
                }
            }
            while (generals.Count < dataGridView1.Rows.Count)
            {
                dataGridView1.Rows.RemoveAt(0);
            }

            //to reset the colrs
            if (_IsInError)
            {
                Mng.SetDGVRowsDefaultMode(dataGridView1);
                _IsInError = false;
            }

            for (int i = 0; i < generals.Count; i++)
            {
                if (generals[i].IsManagedByProgram)
                {
                    dataGridView1["name", i].Value = generals[i].Name;
                    dataGridView1["name", i].ReadOnly = true;
                    dataGridView1["value", i].Value = generals[i].Value;
                    dataGridView1["value", i].ReadOnly = !generals[i].ValueIsEditable;
                    dataGridView1["desc", i].Value = generals[i].Title;
                    dataGridView1["desc", i].ReadOnly = true;

                    //color and font
                    foreach (DataGridViewCell cell in dataGridView1.Rows[i].Cells)
                    {
                        cell.Style.ForeColor = SystemColors.Control;
                        cell.Style.Font = new Font(new FontFamily(System.Drawing.Text.GenericFontFamilies.SansSerif), 8, FontStyle.Regular);
                    }
                }
                else
                {
                    dataGridView1["name", i].Value = generals[i].Name;
                    dataGridView1["name", i].ReadOnly = false;
                    dataGridView1["value", i].Value = generals[i].Value;
                    dataGridView1["value", i].ReadOnly = false;
                    dataGridView1["desc", i].Value = "field not managed in the program";
                    dataGridView1["desc", i].ReadOnly = true;

                    //color and font
                    foreach (DataGridViewCell cell in dataGridView1.Rows[i].Cells)
                    {
                        cell.Style.ForeColor = Color.Yellow;
                        cell.Style.Font = new Font(new FontFamily(System.Drawing.Text.GenericFontFamilies.SansSerif), 8, FontStyle.Italic);
                    }
                }
            }
        }


        public void SendCommand(string comm)
        {
            switch (comm)
            {
                case "add":
                    Eng.Current.AddGeneral();
                    SetUI();
                    dataGridView1["name", dataGridView1.RowCount - 1].Selected = true;
                    break;
                case "remove":
                    Eng.Current.DeleteGeneral(_currentCell.RowIndex);
                    SetUI();
                    break;
                case "moveup":
                    if(Eng.Current.MoveGeneralElement(_currentCell.RowIndex, -1))
                    {
                        dataGridView1[_currentCell.ColumnIndex, _currentCell.RowIndex - 1].Selected = true;
                    }
                    SetUI();
                    break;
                case "movedown":
                    if(Eng.Current.MoveGeneralElement(_currentCell.RowIndex, +1))
                    {
                        dataGridView1[_currentCell.ColumnIndex, _currentCell.RowIndex + 1].Selected = true;
                    }
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
            _currentCell = dataGridView1[e.ColumnIndex, e.RowIndex];

            //for bug
            if(_CountGen < dataGridView1.RowCount)
            {
                return;
            }

            if (_IsInError)
            {
                _IsInError = false;
                Mng.SetDGVRowsDefaultMode(dataGridView1);
            }

            if (Eng.Current.GetGeneralElement(e.RowIndex).IsManagedByProgram == true)
            {
                _AllowAdd = true;
                _AllowDelete = false;
                _AllowMoveUpDown = false;
                RefreshControlCommands?.Invoke(this, this);
            }
            else
            {
                _AllowAdd = true;
                _AllowDelete = true;
                _AllowMoveUpDown = true;
                RefreshControlCommands?.Invoke(this, this);
            }
        }


        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            string value = Mng.GetVal(dataGridView1[_currentCell.ColumnIndex, _currentCell.RowIndex].EditedFormattedValue);

            if(dataGridView1.Columns[_currentCell.ColumnIndex].Name == "name")
            {
                Eng.Current.SetGeneralElementName(_currentCell.RowIndex, value);
            }

            if (dataGridView1.Columns[_currentCell.ColumnIndex].Name == "value")
            {
                Eng.Current.SetGeneralElementValue(_currentCell.RowIndex, value);
            }
        }
    }
}
