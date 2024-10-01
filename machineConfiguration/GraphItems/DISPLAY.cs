using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace machineConfiguration
{
    public partial class DISPLAY : UserControl
    {
        public DISPLAY()
        {
            InitializeComponent();
        }

        //string _id = "-1";
        public string Id
        {
            //get => _id;
            //set
            //{
            //    _id = value;
            //    labelIndex.Text = _id;
            //}
            get => labelIndex.Text;
            private set
            {
                labelIndex.Text = value;
            }
        }

        List<DIAL> _Dials = new List<DIAL>();
        public List<DIAL> Dials
        {
            get => _Dials;
        }

        int _NColumns = -1;
        public int NColumns
        {
            get => _NColumns;
        }

        int _NRows = -1;
        public int NRows
        {
            get => _NRows;
        }


        public List<DIAL> SetDials(string id, string stCols, string stRows)
        {
            Id = id;

            if(Int32.TryParse(stCols, out int ncols) && Int32.TryParse(stRows, out int nrows))
            {
                if (ncols == _NColumns && nrows == _NRows)
                    return _Dials;

                _NColumns = ncols;
                _NRows = nrows;

                //set table layout
                //clear
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.RowCount = 0;
                tableLayoutPanel1.ColumnCount = 0;

                //add columns and rows
                tableLayoutPanel1.RowCount = nrows;
                tableLayoutPanel1.ColumnCount = ncols;
                tableLayoutPanel1.RowStyles.Clear();
                for (int i = 0; i < nrows; i++)
                {
                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                }
                tableLayoutPanel1.ColumnStyles.Clear();
                for (int i = 0; i < ncols; i++)
                {
                    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                }

                //add dial 
                _Dials.Clear();
                for (int i = 0; i < nrows; i++)
                {
                    for (int s = 0; s < ncols; s++)
                    {
                        DIAL dial = new DIAL();
                        tableLayoutPanel1.Controls.Add(dial);
                        dial.Dock = DockStyle.Fill;
                        dial.IdRow = i.ToString();
                        dial.IdColumn = s.ToString();
                        dial.DisplayParent = this;
                        _Dials.Add(dial);
                    }
                }

                return _Dials;
            }
            else
            {
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.RowCount = 0;
                tableLayoutPanel1.ColumnCount = 0;
                //Id = "%err";
                _Dials.Clear();

                return _Dials;
            }
        }
    }
}
