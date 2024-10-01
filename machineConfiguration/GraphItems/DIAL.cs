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
    public partial class DIAL : UserControl
    {

        public DIAL()
        {
            InitializeComponent();
            comboBox1.MouseWheel += BlockMouseWheel;
            _DefaultBackColor = this.BackColor;
        }

        void BlockMouseWheel(object sender, EventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }


        public event EventHandler<DIAL> ChangedCam;


        DISPLAY _displayParent;
        public DISPLAY DisplayParent
        {
            get => _displayParent;
            set => _displayParent = value;
        }

        string[] _Stations;
        public string[] Stations
        {
            get => _Stations;
            set
            {
                _Stations = value;
                if (_Stations == null)
                    _Stations = new string[0];
                //if (_Cams == null)
                //    _Cams = new string[] { "" };
                //comboBox1.Items.Clear();
                //comboBox1.Items.AddRange(value);   //use dropdown
            }
        }

        public string StationSelected
        {
            get => comboBox1.Text;
            set
            {
                if (_Stations.Contains(value))
                {
                    comboBox1.Items.Add(value);
                    comboBox1.Text = value;
                }
                else
                {
                    comboBox1.Text = null;
                }
            }
        }

        public string Defect
        {
            get => labelDefect.Text;
            set => labelDefect.Text = value;
        }

        string _IdRow = "";
        public string IdRow
        {
            get => _IdRow;
            set
            {
                _IdRow = value;
                labelXY.Text = $"[{_IdColumn};{_IdRow}]";
            }
        }

        string _IdColumn = "";
        public string IdColumn
        {
            get => _IdColumn;
            set
            {
                _IdColumn = value;
                labelXY.Text = $"[{_IdColumn};{_IdRow}]";
            }
        }

        //public string PosParse
        //{
        //    get => $"{_displayParent.Id}{_IdColumn}{_IdRow}";
        //}

        Color _DefaultBackColor;
        public Color ThisDefaultBackColor
        {
            get => _DefaultBackColor;
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ChangedCam?.Invoke(this, this);
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(_Stations);
        }

        //for bug
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            ChangedCam?.Invoke(this, this);
        }
    }
}
