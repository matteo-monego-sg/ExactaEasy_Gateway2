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
    public partial class MngDisplay : UserControl, IMng
    {
        public MngDisplay()
        {
            InitializeComponent();
        }


        DISPLAY[] _displaysInControl;
        DISPLAY _dispSelect = null;
        bool _IsInError;


        //IMng actions
        public bool AllowAdd { get => true; }
        public bool AllowDelete { get => true; }
        public bool AllowMoveUpDown { get => true; }
        public bool AllowDeleteAll { get => true; }
        public bool AllowCreateFromStations { get => false; }
        public bool AllowAutoCompleteId { get => false; }
        public bool AllowAddSetStationNamesToNode { get => false; }
        public bool AllowRemoveStationNamesToNode { get => false; }
        public bool IsDisplay { get => true; }
        public int TypeDisplaySelected { get; set; }
        public event EventHandler<IMng> RefreshControlCommands;


        public void SetUI()
        {
            Eng.Current.GetCheckDisplays(out List<ScreenGridDisplaySettings> displays);

            while (displays.Count > flowLayoutPanel1.Controls.OfType<DISPLAY>().Count())
            {
                flowLayoutPanel1.Controls.Add(new DISPLAY());

            }
            while (displays.Count < flowLayoutPanel1.Controls.OfType<DISPLAY>().Count())
            {
                flowLayoutPanel1.Controls.RemoveAt(0);
            }

            //to reset the colrs
            if (_IsInError)
            {
                DisplaysDefaultBackColor();
                _IsInError = false;
            }

            _displaysInControl = flowLayoutPanel1.Controls.OfType<DISPLAY>().ToArray();

            for (int i = 0; i < displays.Count; i++)
            {
                _displaysInControl[i].Enter -= MngDisplay_Enter; //to avoid many subscribtions
                _displaysInControl[i].Enter += MngDisplay_Enter;
                List<DIAL> dials = _displaysInControl[i].SetDials(displays[i].Id, displays[i].Cols, displays[i].Rows);

                foreach (DIAL dial in dials)
                {
                    /*
                     ENTERING DATA INTO THE DIAL COMBOBOX IS VERY SLOW SO I LOAD THE DATA IN REAL TIME WHEN THE USER DOES THE DROPDOWN INTO DIAL.
                    */

                    CameraSetting cam = Eng.Current.GetCameraDisplayAssociated(displays[i].Id, dial.IdColumn, dial.IdRow);
                    dial.ChangedCam -= Dial_ChangedCam;
                    if (cam != null)
                    {
                        dial.Stations = Eng.Current.GetArrayAvaibleNameStations();
                        dial.StationSelected = Eng.Current.GetstationDescription(cam.Node, cam.Station);
                        dial.Defect = cam.CameraDescription;
                    }
                    else
                    {
                        dial.Stations = Eng.Current.GetArrayAvaibleNameStations();
                        dial.StationSelected = "";
                        dial.Defect = "";
                    }
                    dial.ChangedCam += Dial_ChangedCam; //put it at the end
                }
            }

            if (_dispSelect == null && _displaysInControl.Length > 0)
                _displaysInControl[0].Focus();
        }


        public void SendCommand(string comm)
        {
            switch (comm)
            {
                case "add":
                    ScreenGridDisplaySettings screen = new ScreenGridDisplaySettings();
                    bool isOk = false;
                    if (TypeDisplaySelected == 1)
                    {
                        Eng.Current.AddDisplay("1", "1");
                        isOk = true;
                    }
                    if (TypeDisplaySelected == 2)
                    {
                        Eng.Current.AddDisplay("2", "1");
                        isOk = true;
                    }
                    if (TypeDisplaySelected == 3)
                    {
                        Eng.Current.AddDisplay("3", "1");
                        isOk = true;
                    }
                    if (TypeDisplaySelected == 4)
                    {
                        Eng.Current.AddDisplay("1", "2");
                        isOk = true;
                    }
                    if (TypeDisplaySelected == 5)
                    {
                        Eng.Current.AddDisplay("2", "2");
                        isOk = true;
                    }
                    if (TypeDisplaySelected == 6)
                    {
                        Eng.Current.AddDisplay("3", "2");
                        isOk = true;
                    }
                    SetUI();
                    if (isOk)
                    {
                        ScrollToControl(_displaysInControl[_displaysInControl.Length - 1]);
                        SelectDisplay(_displaysInControl[_displaysInControl.Length - 1]);
                    }
                    break;
                case "remove":
                    if (int.TryParse(_dispSelect.Id, out int val))
                    {
                        Eng.Current.DeleteDisplay(val);
                        SetUI();
                    }
                    break;
                case "moveup":
                    if(int.TryParse(_dispSelect.Id, out int Nup))
                    {
                        if (Nup > 0)
                        {
                            Eng.Current.MoveDisplay(Nup, -1);
                            SetUI();
                            ScrollToControl(_displaysInControl[Nup - 1]);
                            SelectDisplay(_displaysInControl[Nup - 1]);
                        }
                    }
                    break;
                case "movedown":
                    if (int.TryParse(_dispSelect.Id, out int Ndown))
                    {
                        if(Ndown < _displaysInControl.Length - 1)
                        {
                            Eng.Current.MoveDisplay(Ndown, +1);
                            SetUI();
                            ScrollToControl(_displaysInControl[Ndown + 1]);
                            SelectDisplay(_displaysInControl[Ndown + 1]);
                        }
                    }
                    break;
                case "deleteall":
                    Eng.Current.DeleteAllDisplays();
                    SetUI();
                    break;
                default: throw new ArgumentException($"unknown command: {comm}");
            }
        }


        public void SelectItemsInError(ErrorElement err)
        {
            DisplaysDefaultBackColor();
            _IsInError = true;
            //_displaysInControl[err.PosIndex[0]].BackColor = Color.DarkRed;
            int index = 0;
            foreach(DISPLAY disp in _displaysInControl)
            {
                foreach(DIAL dial in disp.Dials)
                {
                    if(index == err.PosIndex[0])
                    {
                        disp.BackColor = Color.DarkRed;
                        dial.BackColor = Color.DarkRed;

                        //scroll
                        ScrollToControl(disp);
                    }
                    index++;
                }
            }
        }



        private void Dial_ChangedCam(object sender, DIAL e)
        {
            Eng.Current.SetDisplayDial(e.DisplayParent.Id, e.IdColumn, e.IdRow, e.StationSelected);
            SetUI();
        }



        //display usercontrol enter
        private void MngDisplay_Enter(object sender, EventArgs e)
        {
            //DisplaysDefaultBackColor();
            //((DISPLAY)sender).BackColor = Color.FromArgb(120, 120, 120);
            //_dispSelect = (DISPLAY)sender;
            SelectDisplay((DISPLAY)sender);
        }


        //private methods
        void DisplaysDefaultBackColor()
        {
            foreach (DISPLAY disp in _displaysInControl)
            {
                disp.BackColor = Color.FromArgb(60, 60, 60);
                foreach(DIAL dial in disp.Dials)
                {
                    dial.BackColor = dial.ThisDefaultBackColor;
                }
            }
        }

        void ScrollToControl(DISPLAY disp)
        {
            //flowLayoutPanel1.VerticalScroll.Value = flowLayoutPanel1.VerticalScroll.Maximum - disp.Location.Y;
            //flowLayoutPanel1.VerticalScroll.Value = disp.Location.Y;
            int actScroll = flowLayoutPanel1.VerticalScroll.Value;
            int actControl = disp.Location.Y;

            while(actControl < 3)
            {
                if(actScroll > flowLayoutPanel1.VerticalScroll.Minimum)
                {
                    actScroll--;
                    actControl++;
                }
                else
                {
                    break;
                }
            }

            while (actControl > 3)
            {
                if (actScroll < flowLayoutPanel1.VerticalScroll.Maximum)
                {
                    actScroll++;
                    actControl--;
                }
                else
                {
                    break;
                }
            }

            flowLayoutPanel1.VerticalScroll.Value = actScroll;
            flowLayoutPanel1.PerformLayout(); //to update the UI of scroll bar!!!!!
        }

        void SelectDisplay(DISPLAY disp)
        {
            DisplaysDefaultBackColor();
            disp.BackColor = Color.FromArgb(120, 120, 120);
            _dispSelect = disp;
        }
    }
}
