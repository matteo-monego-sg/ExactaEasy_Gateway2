using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SPAMI.Util;
using SPAMI.Util.Logger;

namespace SPAMI.LightControllers
{
    public partial class LightControllerSummaryGUI : UserControl, ICommon
    {
        [Browsable(false)]
        public string ClassName { get; set; }
        public LightControllerSummaryGUI()
        {
            ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
            //Multilang.LanguageChanged += new EventHandler(OnLanguageChanged);
            //Multilang.AddMeToMultilanguage(this);
            InitializeComponent();
        }

        [Browsable(true), Category("Light Controller"), Description("TODO")]
        public LightController LightController { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!this.DesignMode)
            {
                RefreshListView();
                if (LightController != null)
                {
                    //LightController.OnPostRestoreFactorySettings += new EventHandler(LightController_OnPostRestoreFactorySettings);
                    LightController.OnPostLoadSettingsFromController += new LightControllers.LightController.DelegateChannel(LightController_OnPostLoadSettingsFromController);
                    //LightController.OnPostSaveSettingsToController += new EventHandler(LightController_OnPostSaveSettingsToController);
                }
            }
        }
        /*public void Init(LightController lightController)
        {
            LightController = lightController;
            Init();
        }

        public void Init()
        {
            try
            {
                RefreshListView(); RefreshListView();
                //LightController.OnPostRestoreFactorySettings += new EventHandler(LightController_OnPostRestoreFactorySettings);
                LightController.OnPostLoadSettingsFromController += new EventHandler(LightController_OnPostLoadSettingsFromController);
                //LightController.OnPostSaveSettingsToController += new EventHandler(LightController_OnPostSaveSettingsToController);
            }
            catch (System.Exception ex)
            {
                if (LightController == null)
                {
                    Log.Line(LogLevels.Error, ClassName + ".Init", "FATAL ERROR! Check develop connection between modules." + "\r\n" + ex.ToString());
                    MessageBox.Show("FATAL ERROR! Check develop connection between modules.", "FATAL ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }*/

        /*public void OnLanguageChanged(object sender, EventArgs args)
        {
            string sAssName = Multilang.AssemblyNameExtract(System.Reflection.Assembly.GetExecutingAssembly());
            if (Multilang.IsAssemblyStringTableLoaded(sAssName))
            {
                columnHeaderChannel.Text = Multilang.GetString(sAssName, "Canale");
                columnHeaderMode.Text = Multilang.GetString(sAssName, "Modalita");
                columnHeaderInput.Text = Multilang.GetString(sAssName, "Ingresso");
                columnHeaderBright.Text = Multilang.GetString(sAssName, "CorrenteA");
                columnHeaderMaxBright.Text = Multilang.GetString(sAssName, "CorrenteMaxA");
                columnHeaderDelay.Text = Multilang.GetString(sAssName, "RitardoMs");
                columnHeaderWidth.Text = Multilang.GetString(sAssName, "DurataMs");
                columnHeaderRetrigger.Text = Multilang.GetString(sAssName, "RetriggerMs");
                columnHeaderE.Text = Multilang.GetString(sAssName, "E");
                columnHeaderP.Text = Multilang.GetString(sAssName, "P");
            }
        }*/

        private void LightController_OnPostLoadSettingsFromController(object sender, ChannelEventArgs args)
        {
            if (listView.InvokeRequired)
                listView.BeginInvoke(new MethodInvoker(RefreshListView));
            else
                RefreshListView();
        }

        private void RefreshListView()
        {
            SuspendLayout();
            listView.ShowGroups = false;
            listView.Items.Clear();
            if (LightController.ControllerSpecs != null)
            {
                for (int ich = 0; ich < LightController.ControllerSpecs.FixedOutputChannelNumber; ich++)
                {
                    ListViewItem lvit = new ListViewItem();
                    lvit.UseItemStyleForSubItems = true;
                    lvit.Text = "CH " + (ich + 1).ToString();
                    lvit.SubItems.Add(LightController.ControllerSpecs.ChanConfig[ich].OperatingMode.ToString());
                    lvit.SubItems.Add(LightController.ControllerSpecs.ChanConfig[ich].OperatingInputTrigger.ToString());
                    lvit.SubItems.Add(LightController.ControllerSpecs.ChanConfig[ich].OperatingCurrent.ToString("f3"));
                    lvit.SubItems.Add(LightController.ControllerSpecs.GetMaxBright(ich).ToString("f3"));
                    lvit.SubItems.Add(LightController.ControllerSpecs.ChanConfig[ich].OperatingDelay.ToString("f3"));
                    lvit.SubItems.Add(LightController.ControllerSpecs.ChanConfig[ich].OperatingPulseWidth.ToString("f3"));
                    lvit.SubItems.Add(LightController.ControllerSpecs.ChanConfig[ich].OperatingRetrigger.ToString("f3"));
                    if (LightController.ControllerSpecs.ChanConfig[ich].E == false)
                        lvit.SubItems.Add("EN");
                    else
                        lvit.SubItems.Add("DIS");
                    if (LightController.ControllerSpecs.ChanConfig[ich].P == false)
                        lvit.SubItems.Add("POS");
                    else
                        lvit.SubItems.Add("NEG");
                    if (ich % 2 == 0)
                        lvit.BackColor = Color.FromArgb(220, 220, 220);
                    else
                        lvit.BackColor = Color.White;
                    listView.Items.Add(lvit);
                }
            }
            ResumeLayout();
        }
    }
}
