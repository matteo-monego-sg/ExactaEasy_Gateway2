using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using machineConfiguration.MachineConfig;
using System.Xml.Linq;
using System.Xml.XPath;

namespace machineConfiguration
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            _TimeLoadDefault.Tick += _TimeLoadDefault_Tick;
            _TimeLoadDefault.Start();
            SelectMenu(0);
            Eng.Current.LoadedMachineConfig += CurrentEng_LoadedMachineConfig;
            _memTitle = this.Text.Clone().ToString();
        }

        Timer _TimeLoadDefault = new Timer { Interval = 1 };
        string _memTitle;

        private void _TimeLoadDefault_Tick(object sender, EventArgs e)
        {
            _TimeLoadDefault.Stop();
            try
            {
                string path = @"C:\ARTIC\ExactaEasy\machineConfig\machineConfig.xml";
                if (File.Exists(path))
                    Eng.Current.LoadXml(path);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //SelectMenu(0);
            CheckErrors();
        }


        void SetTitle(string title)
        {
            if(title == null || title == "")
                this.Text = $"{_memTitle}";
            else
                this.Text = $"{_memTitle} ({title})";
        }



        //FILE MANAGMENT
        private void buttonOpen_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
                if (op.ShowDialog() == DialogResult.OK)
                {
                    Eng.Current.LoadXml(op.FileName);
                    //SelectMenu(_lastSelect); //to update the current UI
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error to open the file:\n" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            //check errors and ask
            int nErrs = CheckErrors();
            if(nErrs != 0)
            {
                if(MessageBox.Show("There are one or more errors, do you want to save anyway?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    return;
                }
            }

            try
            {
                SaveFileDialog sv = new SaveFileDialog();
                sv.Filter = "XML files (*.xml)|*.xml";
                sv.FileName = "machineConfig";
                if (sv.ShowDialog() == DialogResult.OK)
                {
                    Eng.Current.SaveXml(sv.FileName);
                    MessageBox.Show("Done!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error to save the file:\n" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSaveDumpImg_Click(object sender, EventArgs e)
        {
            //check errors and ask
            int nErrs = CheckErrors();
            if (nErrs != 0)
            {
                if (MessageBox.Show("There are one or more errors, do you want to save anyway?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    return;
                }
            }

            try
            {
                SaveFileDialog sv = new SaveFileDialog();
                sv.Filter = "XML files (*.xml)|*.xml";
                sv.FileName = "dumpImagesConfig";
                if (sv.ShowDialog() == DialogResult.OK)
                {
                    Eng.Current.SaveDumpImageConfig(sv.FileName);
                    MessageBox.Show("Done!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error to save the file:\n" + ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            if(Eng.Current.NLoaded != 0)
            {
                if(MessageBox.Show("Are you sure?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    return;
                }
            }

            Eng.Current.NewMachineConfig();
            SetTitle("");
        }


        //eng loaded
        private void CurrentEng_LoadedMachineConfig(object sender, EventArgs e)
        {
            if(_mngselect == null)
            {
                SelectMenu(MenuEnum.GENERAL);
            }
            else
            {
                SelectMenu(_lastSelect);
            }
            CheckErrors();

            SetTitle(Eng.Current.PathFile);
        }





        //MENUS
        IMng _mngselect = null;
        Color _btnUnselect = Color.FromArgb(60, 60, 60);
        Color _btnSelect = Color.FromArgb(120, 120, 120);
        Color _dispUnselect = Color.FromArgb(60, 60, 60);
        Color _dispSelect = Color.FromArgb(160, 160, 160);
        MenuEnum _lastSelect;

        void SelectMenu(MenuEnum menu)
        {
            //last select
            _lastSelect = menu;

            //btn
            buttonMenGeneral.BackColor = _btnUnselect;
            buttonMenNode.BackColor = _btnUnselect;
            buttonMenStation.BackColor = _btnUnselect;
            buttonMenCamera.BackColor = _btnUnselect;
            buttonMenDisplay.BackColor = _btnUnselect;
            buttonMenHistoricizedTools.BackColor = _btnUnselect;
            //mng
            foreach(IMng mngs in this.Controls.OfType<IMng>())
            {
                ((UserControl)mngs).Visible = false;
            }

            switch (menu)
            {
                case MenuEnum.GENERAL:
                    buttonMenGeneral.BackColor = _btnSelect;
                    _mngselect = mngGeneral1;
                    break;
                case MenuEnum.NODE:
                    buttonMenNode.BackColor = _btnSelect;
                    _mngselect = mngNode1;
                    break;
                case MenuEnum.STATION:
                    buttonMenStation.BackColor = _btnSelect;
                    _mngselect = mngStation1;
                    break;
                case MenuEnum.CAMERA:
                    buttonMenCamera.BackColor = _btnSelect;
                    _mngselect = mngCamera1;
                    break;
                case MenuEnum.DISPLAY:
                    buttonMenDisplay.BackColor = _btnSelect;
                    _mngselect = mngDisplay1;
                    break;
                case MenuEnum.HISTORICIZED_TOOLS:
                    buttonMenHistoricizedTools.BackColor = _btnSelect;
                    _mngselect = mngHistoricizedTools1;
                    break;
                default: throw new ArgumentException("menu not valid");
            }

            ((UserControl)_mngselect).Visible = true;
            ((UserControl)_mngselect).Dock = DockStyle.Fill;
            _mngselect.SetUI();
            _mngselect.RefreshControlCommands -= _mngselect_RefreshControlCommands;
            _mngselect.RefreshControlCommands += _mngselect_RefreshControlCommands;
            SetVisibleBtns(_mngselect);
        }

        private void _mngselect_RefreshControlCommands(object sender, IMng e)
        {
            SetVisibleBtns(e);
        }

        void SetVisibleBtns(IMng mng)
        {
            buttonAdd.Visible = mng.AllowAdd;
            buttonDelete.Visible = mng.AllowDelete;
            buttonMoveUp.Visible = mng.AllowMoveUpDown;
            buttonMoveDown.Visible = mng.AllowMoveUpDown;
            panelDysp.Visible = mng.IsDisplay;
            if (panelDysp.Visible)
            {
                SelectDisplayTable(null, 0);
            }
            buttonDeleteAll.Visible = mng.AllowDeleteAll;
            buttonCreateFromStation.Visible = mng.AllowCreateFromStations;
            buttonAutoCompleteId.Visible = mng.AllowAutoCompleteId;
            buttonAddStationNameToNode.Visible = mng.AllowAddSetStationNamesToNode;
            buttonDeleteStationNamesToNodes.Visible = mng.AllowRemoveStationNamesToNode;
        }



        //btns
        private void buttonMenGeneral_Click(object sender, EventArgs e)
        {
            SelectMenu(MenuEnum.GENERAL);
        }

        private void buttonMenNode_Click(object sender, EventArgs e)
        {
            SelectMenu(MenuEnum.NODE);
        }

        private void buttonMenStation_Click(object sender, EventArgs e)
        {
            SelectMenu(MenuEnum.STATION);
        }

        private void buttonMenCamera_Click(object sender, EventArgs e)
        {
            SelectMenu(MenuEnum.CAMERA);
        }

        private void buttonMenDisplay_Click(object sender, EventArgs e)
        {
            SelectMenu(MenuEnum.DISPLAY);
        }

        private void buttonMenHistoricisedTools_Click(object sender, EventArgs e)
        {
            SelectMenu(MenuEnum.HISTORICIZED_TOOLS);
        }





        //actions
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            _mngselect.SendCommand("add");
            if (_mngselect.IsDisplay)
                SelectDisplayTable(null, 0);
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            _mngselect.SendCommand("remove");
        }

        private void buttonMoveUp_Click(object sender, EventArgs e)
        {
            _mngselect.SendCommand("moveup");
        }

        private void buttonMoveDown_Click(object sender, EventArgs e)
        {
            _mngselect.SendCommand("movedown");
        }

        private void buttonDeleteAll_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Are you sure?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(res == DialogResult.Yes)
            {
                _mngselect.SendCommand("deleteall");
            }
        }

        private void buttonCreateFromStation_Click(object sender, EventArgs e)
        {
            _mngselect.SendCommand("createfromstations");
        }

        private void buttonAutoCompleteId_Click(object sender, EventArgs e)
        {
            _mngselect.SendCommand("autocompleteid");
        }

        private void buttonAddStationNameToNode_Click(object sender, EventArgs e)
        {
            _mngselect.SendCommand("addsetstationnametonode");
        }

        private void buttonDeleteStationNamesToNodes_Click(object sender, EventArgs e)
        {
            _mngselect.SendCommand("removestationnametonode");
        }









        //display selected
        private void tableLayoutPanel1_Click(object sender, EventArgs e)
        {
            SelectDisplayTable(sender, 1);
        }

        private void tableLayoutPanel2_Click(object sender, EventArgs e)
        {
            SelectDisplayTable(sender, 2);
        }

        private void tableLayoutPanel3_Click(object sender, EventArgs e)
        {
            SelectDisplayTable(sender, 3);
        }

        private void tableLayoutPanel4_Click(object sender, EventArgs e)
        {
            SelectDisplayTable(sender, 4);
        }

        private void tableLayoutPanel5_Click(object sender, EventArgs e)
        {
            SelectDisplayTable(sender, 5);
        }

        private void tableLayoutPanel6_Click(object sender, EventArgs e)
        {
            SelectDisplayTable(sender, 6);
        }

        void SelectDisplayTable(object sender, int type)
        {
            if(sender == null)
            {
                foreach (TableLayoutPanel pan in panelDysp.Controls.OfType<TableLayoutPanel>())
                {
                    pan.BackColor = _dispUnselect;
                }
                _mngselect.TypeDisplaySelected = type;
                switch (_mngselect.TypeDisplaySelected)
                {
                    case 0: break;
                    case 1: tableLayoutPanel1.BackColor = _dispSelect; break;
                    case 2: tableLayoutPanel2.BackColor = _dispSelect; break;
                    case 3: tableLayoutPanel3.BackColor = _dispSelect; break;
                    case 4: tableLayoutPanel4.BackColor = _dispSelect; break;
                    case 5: tableLayoutPanel5.BackColor = _dispSelect; break;
                    case 6: tableLayoutPanel6.BackColor = _dispSelect; break;
                }
            }
            else
            {
                foreach (TableLayoutPanel pan in panelDysp.Controls.OfType<TableLayoutPanel>())
                {
                    pan.BackColor = _dispUnselect;
                }
                ((TableLayoutPanel)sender).BackColor = _dispSelect;
                _mngselect.TypeDisplaySelected = type;
            }
        }



        //errors
        List<ErrorElement> _LastErrors;

        private void buttonCheck_Click(object sender, EventArgs e)
        {
            CheckErrors();
        }

        int CheckErrors()
        {
            dataGridViewErrors.Rows.Clear();
            _LastErrors = Eng.Current.CheckErrors();

            if(_LastErrors.Count > 0)
            {
                labelNErrors.Text = $"Errors: {_LastErrors.Count}";
                labelNErrors.ForeColor = Color.Red;
            }
            else
            {
                labelNErrors.Text = "Errors: 0";
                labelNErrors.ForeColor = SystemColors.Control;
            }

            foreach(ErrorElement err in _LastErrors)
            {
                dataGridViewErrors.Rows.Add(err.Subject, err.Message);
            }

            return dataGridViewErrors.RowCount;
        }

        private void dataGridViewErrors_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ErrorElement err = _LastErrors[e.RowIndex];
            SelectMenu(err.Menu);
            _mngselect.SelectItemsInError(err);
        }
    }
}
