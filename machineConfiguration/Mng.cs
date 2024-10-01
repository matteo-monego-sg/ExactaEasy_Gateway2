using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace machineConfiguration
{
    public static class  Mng
    {
        public static void AdjustDatagridView(DataGridView dgv)
        {
            dgv.AllowUserToOrderColumns = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.BackgroundColor = Color.FromArgb(60, 60, 60);
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.Sunken;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Sunken;
            dgv.ColumnHeadersHeight = 40;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.BorderStyle = BorderStyle.Fixed3D;

            //dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(60, 60, 60);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.Control;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(60, 60, 60);
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = SystemColors.Control;
            dgv.ColumnHeadersDefaultCellStyle.Format = "";
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgv.Dock = DockStyle.Fill;
            dgv.EditMode = DataGridViewEditMode.EditOnEnter;
            dgv.EnableHeadersVisualStyles = false;
            dgv.GridColor = SystemColors.Control;
            dgv.MultiSelect = false;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgv.RowTemplate.Height = 24;

            dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;  //<---------- To copy (CTRL + C) only selected cell value

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
                col.DefaultCellStyle.ForeColor = SystemColors.Control;
                col.DefaultCellStyle.SelectionBackColor = Color.FromArgb(120, 120, 120);
                col.DefaultCellStyle.SelectionForeColor = SystemColors.Control;
                //col.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                //col.DefaultCellStyle.NullValue = "";
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            foreach (DataGridTextBoxColumn col in dgv.Columns.OfType<DataGridTextBoxColumn>())
            {

            }

            foreach (DataGridViewComboBoxColumn col in dgv.Columns.OfType<DataGridViewComboBoxColumn>())
            {
                col.FlatStyle = FlatStyle.System;

                col.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                col.FlatStyle = FlatStyle.Flat;
                //col.DefaultCellStyle.NullValue = "";
            }
        }


        public static string GetVal(object obj)
        {
            return obj == null ? "" : obj.ToString();
        }

        public static void MsgBoxWarning(string mex, string title = "")
        {
            MessageBox.Show(mex, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void MsgBoxInsertInteger()
        {
            MessageBox.Show("Insert an integer!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static bool IsEmptyOrNull(object obj)
        {
            if (obj == null)
            {
                return true;
            }
            else
            {
                string val = obj.ToString();
                if(val == "" || val.All(Char.IsWhiteSpace))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }



        //foer error mode datagridview
        public static void SetDGVRowErrorMode(DataGridView dgv, int indexRow)
        {
            if(indexRow < dgv.RowCount)
            {
                foreach (DataGridViewCell cell in dgv.Rows[indexRow].Cells)
                {
                    cell.Style.BackColor = Color.DarkRed;
                    cell.Style.SelectionBackColor = Color.DarkRed;
                }
            }
        }

        public static void SetDGVRowsDefaultMode(DataGridView dgv)
        {
            foreach(DataGridViewRow dgvr in dgv.Rows)
            {
                foreach (DataGridViewCell cell in dgvr.Cells)
                {
                    cell.Style.BackColor = Color.FromArgb(30, 30, 30);
                    cell.Style.SelectionBackColor = Color.FromArgb(120, 120, 120);
                }
            }
        }

        public static void SetValuesComboboxCellDGV(DataGridViewComboBoxCell cmbx, string[] values, string selectedVal)
        {
            //if(values.Contains(selectedVal) == true)
            //{

            //}
            //else
            //{
            //    cmbx.Value = null;
            //    cmbx.Items.Clear();
            //    cmbx.Items.AddRange(values);
            //}
            cmbx.Value = null;
            cmbx.Items.Clear();
            cmbx.Items.AddRange(values);
            cmbx.Value = selectedVal;
        }
    }
}
