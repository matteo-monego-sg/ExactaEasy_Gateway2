using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web.UI;
using System.IO;

using SPAMI.Util;
using SPAMI.Util.Logger;
using SPAMI.Util.XML;

namespace SPAMI.LightControllers
{
    public partial class LightControllerConfigurator : System.Windows.Forms.UserControl, ICommon
    {
        [Browsable(false)]
        public string ClassName { get; set; }
        public LightControllerConfigurator()
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
                if (LightController != null && LightController.ControllerSpecs != null)
                {
                    for (int ich = 0; ich < LightController.ControllerSpecs.FixedOutputChannelNumber; ich++)
                        comboBoxChannel.Items.Add("CH " + (ich + 1).ToString());

                    if (LightController.ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_RT_SERIES)
                    {
                        string[] LightModeStringVec = Enum.GetNames(typeof(SPAMI.LightControllers.LightModeGardasoftRT));
                        comboBoxMode.Items.AddRange(LightModeStringVec);
                    }
                    if (LightController.ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_PP_SERIES)
                    {
                        string[] LightModeStringVec = Enum.GetNames(typeof(SPAMI.LightControllers.LightModeGardasoftPP));
                        comboBoxMode.Items.AddRange(LightModeStringVec);
                    }
                    //for (int im = 0; im < LightModeStringVec.Length; im++)
                    //    comboBoxMode.Items.Add(LightModeStringVec[i]);
                    if (comboBoxChannel.Items.Count > 0)
                        comboBoxChannel.SelectedIndex = 0;
                }
                else
                    Log.Line(LogLevels.Error, ClassName + ".OnLoad", "Errore! LightController o ControllerSpecs==null");

                OperatingModeChanged();
                PopulateInputComboBox(0);
                if (LightController.ControllerSpecs != null &&
                    LightController.ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_RT_SERIES)
                    numericUpDownTriggerType.Enabled = false;	//RT
                if (LightController.ControllerSpecs!=null &&
                    LightController.ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_PP_SERIES)
                    numericUpDownTriggerType.Enabled = true;	//PP
                comboBoxChannel_SelectedIndexChanged(this, EventArgs.Empty);
                if (LightController != null)
                {
                    LightController.OnPostConnection += new LightControllers.LightController.DelegateConnected(LightController_OnPostConnection);
                    LightController.OnPostDisconnection += new LightControllers.LightController.DelegateConnected(LightController_OnPostDisconnection);
                    LightController.OnPostRestoreFactorySettings += new EventHandler(LightController_OnPostRestoreFactorySettings);
                    LightController.OnPostLoadSettingsFromController += new LightControllers.LightController.DelegateChannel(LightController_OnPostLoadSettingsFromController);
                    LightController.OnPostSendSettingsToController += new LightControllers.LightController.DelegateChannel(LightController_OnPostSendSettingsToController);
                    LightController.OnPostSaveSettingsToController += new EventHandler(LightController_OnPostSaveSettingsToController);
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
                for (int ich = 0; ich < LightController.ControllerSpecs.FixedOutputChannelNumber; ich++)
                    comboBoxChannel.Items.Add("CH " + (ich + 1).ToString());
                string[] LightModeStringVec = Enum.GetNames(typeof(SPAMI.LightControllers.LightMode));
                comboBoxMode.Items.AddRange(LightModeStringVec);
                //for (int im = 0; im < LightModeStringVec.Length; im++)
                //    comboBoxMode.Items.Add(LightModeStringVec[i]);
                if (comboBoxChannel.Items.Count > 0)
                    comboBoxChannel.SelectedIndex = 0;

                OperatingModeChanged();
                PopulateInputComboBox(0);
                comboBoxChannel_SelectedIndexChanged(this, EventArgs.Empty);
                LightController.OnPostConnection += new LightControllers.LightController.DelegateConnected(LightController_OnPostConnection);
                LightController.OnPostDisconnection += new LightControllers.LightController.DelegateConnected(LightController_OnPostDisconnection);
                LightController.OnPostRestoreFactorySettings += new EventHandler(LightController_OnPostRestoreFactorySettings);
                LightController.OnPostLoadSettingsFromController += new EventHandler(LightController_OnPostLoadSettingsFromController);
                LightController.OnPostSendSettingsToController += new EventHandler(LightController_OnPostSendSettingsToController);
                LightController.OnPostSaveSettingsToController += new EventHandler(LightController_OnPostSaveSettingsToController);
            }
            catch (System.Exception ex)
            {
                if (LightController == null)
                {
                    Log.Line(LogLevels.Error, ClassName + ".Init", "FATAL ERROR! Check develop connection between modules.\r\n" + ex.ToString());
                    MessageBox.Show("FATAL ERROR! Check develop connection between modules.", "FATAL ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }*/

        /*public void OnLanguageChanged(object sender, EventArgs args)
        {
            string sAssName = Multilang.AssemblyNameExtract(System.Reflection.Assembly.GetExecutingAssembly());
            if (Multilang.IsAssemblyStringTableLoaded(sAssName))
            {
                groupBoxChannelsConfigurator.Text = Multilang.GetString(sAssName, "ConfiguratoreCanaliController");
                labelChannel.Text = Multilang.GetString(sAssName, "Canale");
                labelMode.Text = Multilang.GetString(sAssName, "Modalita");
                labelInput.Text = Multilang.GetString(sAssName, "Ingresso");
                labelCurrent.Text = Multilang.GetString(sAssName, "CorrenteA");
                labelMaxCurrent.Text = Multilang.GetString(sAssName, "CorrenteMaxA");
                labelDelay.Text = Multilang.GetString(sAssName, "RitardoMs");
                labelWidth.Text = Multilang.GetString(sAssName, "DurataMs");
                labelRetrigger.Text = Multilang.GetString(sAssName, "RetriggerMs");
                labelE.Text = Multilang.GetString(sAssName, "E");
                labelP.Text = Multilang.GetString(sAssName, "P");
                checkBoxInternalTrigger.Text = Multilang.GetString(sAssName, "InternalTrigger");
                buttonSimulateInputTrigger.Text = Multilang.GetString(sAssName, "Pulse1");
                buttonExport.Text = Multilang.GetString(sAssName, "Esporta");
                buttonImport.Text = Multilang.GetString(sAssName, "Importa");
                buttonSend.Text = Multilang.GetString(sAssName, "Invia");
                buttonLoad.Text = Multilang.GetString(sAssName, "Carica");
                buttonSave.Text = Multilang.GetString(sAssName, "Salva");
                buttonRestore.Text = Multilang.GetString(sAssName, "Ripristina");
            }
        }*/

        [Obsolete]
        private void CreateHelpPage()
        {
            //pier: sarebbe bello fare una paginetta html di help...
            //PRENDERE SCREENSHOT DEL CONTROLLO...DA SISTEMARE
            System.Windows.Forms.Control c = this;
            Bitmap bmp = SPAMI.Util.Utilities.ControlScreenshot(new LightControllerConfigurator());
            //Bitmap bmp = SPAMI.Util.Utilities.ControlScreenshot(this);
            bmp.Save(@"C:\ctrlbmp.bmp");

            //TODO
            string[] _words = { "Sam", "Dot", "Perls" };
            // Initialize StringWriter instance.
            StringWriter stringWriter = new StringWriter();
            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter, string.Empty))
            {
                // Loop over some strings.
                foreach (var word in _words)
                {
                    // Some strings for the attributes.
                    string classValue = "ClassName";
                    string urlValue = "http://www.dotnetperls.com/";
                    string imageValue = "image.jpg";

                    // The important part:
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, classValue);
                    writer.RenderBeginTag(HtmlTextWriterTag.Div); // Begin #1

                    writer.AddAttribute(HtmlTextWriterAttribute.Href, urlValue);
                    writer.RenderBeginTag(HtmlTextWriterTag.A); // Begin #2

                    writer.AddAttribute(HtmlTextWriterAttribute.Src, imageValue);
                    writer.AddAttribute(HtmlTextWriterAttribute.Width, "60");
                    writer.AddAttribute(HtmlTextWriterAttribute.Height, "60");
                    writer.AddAttribute(HtmlTextWriterAttribute.Alt, "");

                    writer.RenderBeginTag(HtmlTextWriterTag.Img); // Begin #3
                    writer.RenderEndTag(); // End #3

                    writer.Write(word);

                    writer.RenderEndTag(); // End #2
                    writer.RenderEndTag(); // End #1
                }
                using (StreamWriter sw = new StreamWriter(@"C:\temp.html"))
                {
                    sw.Write(stringWriter.ToString());
                }
            }
            
        }

        private void Form2ChanSettings(int idx)
        {
            try
            {
                if (comboBoxMode.SelectedItem != null)
                {
                    LightController.ControllerSpecs.ChanConfig[idx].OperatingMode = comboBoxMode.SelectedIndex;
                    if (comboBoxInput.SelectedItem != null && comboBoxInput.SelectedItem.ToString().Length > 2)
                        LightController.ControllerSpecs.ChanConfig[idx].OperatingInputTrigger = System.Convert.ToInt32(comboBoxInput.SelectedItem.ToString().Substring(2));
                    //LightController.ControllerSpecs.ChanConfig[...].Trigger = comboBoxInput...  //todo
                    LightController.ControllerSpecs.ChanConfig[idx].OperatingCurrent = (double)numericUpDownSetBright.Value;
                    LightController.ControllerSpecs.SetMaxBright(idx, (double)numericUpDownMaxBright.Value);
                    LightController.ControllerSpecs.SetOperatingDelay(idx, (double)numericUpDownDelayMs.Value);
                    LightController.ControllerSpecs.SetOperatingPulseWidth(idx, (double)numericUpDownWidthMs.Value);
                    LightController.ControllerSpecs.ChanConfig[idx].OperatingRetrigger = (double)numericUpDownRetriggerMs.Value;
                    LightController.ControllerSpecs.ChanConfig[idx].E = checkBoxE.Checked;
                    LightController.ControllerSpecs.ChanConfig[idx].P = checkBoxP.Checked;
                    LightController.ControllerSpecs.InternalTrigger = checkBoxInternalTrigger.Checked;
                    LightController.ControllerSpecs.InternalTriggerMs = (double)numericUpDownInternalTrigger.Value;
                }
            }
            catch (System.Exception ex)
            {
                Log.Line(LogLevels.Error, ClassName + ".Form2ChanSettings", "Errore: " + ex.ToString());
            }
        }

        private void ChanSettings2Form()
        {
            int idx = comboBoxChannel.SelectedIndex;
            try
            {
                if (LightController != null)
                {
                    for (int it = 0; it < comboBoxMode.Items.Count; it++)
                    {
                        //if (comboBoxMode.Items[it].ToString().ToLower().CompareTo(LightController.ControllerSpecs.ChanConfig[idx].OperatingMode.ToString().ToLower()) == 0)
                        if (it==LightController.ControllerSpecs.ChanConfig[idx].OperatingMode)
                        {
                            comboBoxMode.SelectedIndex = it;
                        }
                    }
                    if (comboBoxInput.SelectedItem!=null && comboBoxInput.SelectedItem.ToString().Length > 2)
                    {
                        for (int it = 0; it < comboBoxInput.Items.Count; it++)
                        {
                            if (System.Convert.ToInt32(comboBoxInput.Items[it].ToString().Substring(2)) == LightController.ControllerSpecs.ChanConfig[idx].OperatingInputTrigger)
                            {
                                comboBoxInput.SelectedIndex = it;
                            }
                        }
                    }
                    numericUpDownSetBright.Value = (decimal)LightController.ControllerSpecs.ChanConfig[idx].OperatingCurrent;
                    textBoxCurrBright.Text = LightController.ControllerSpecs.ChanConfig[idx].OperatingCurrent.ToString("f3");
                    numericUpDownMaxBright.Value = (decimal)LightController.ControllerSpecs.GetMaxBright(idx);
                    numericUpDownDelayMs.Value = (decimal)LightController.ControllerSpecs.ChanConfig[idx].OperatingDelay;
                    numericUpDownWidthMs.Value = (decimal)LightController.ControllerSpecs.ChanConfig[idx].OperatingPulseWidth;
                    numericUpDownRetriggerMs.Value = (decimal)LightController.ControllerSpecs.ChanConfig[idx].OperatingRetrigger;
                    checkBoxE.Checked = LightController.ControllerSpecs.ChanConfig[idx].E;
                    checkBoxP.Checked = LightController.ControllerSpecs.ChanConfig[idx].P;
                    checkBoxInternalTrigger.Checked = LightController.ControllerSpecs.InternalTrigger;
                    numericUpDownInternalTrigger.Value = (decimal)LightController.ControllerSpecs.InternalTriggerMs;
                }
            }
            catch (System.Exception ex)
            {
                Log.Line(LogLevels.Error, ClassName + ".ChanSettings2Form", "Errore: " + ex.ToString());
            }
        }

        #region LightController events
        private void LightController_OnPostConnection(object sender, ConnectedEventArgs args)
        {
            if (groupBoxChannelsConfigurator.InvokeRequired)
                groupBoxChannelsConfigurator.BeginInvoke(new EnableConfiguratorDel(EnableConfigurator), new object[] { true });
            else
                EnableConfigurator(true);
        }

        private void LightController_OnPostDisconnection(object sender, ConnectedEventArgs args)
        {
            if (groupBoxChannelsConfigurator.InvokeRequired)
                groupBoxChannelsConfigurator.BeginInvoke(new EnableConfiguratorDel(EnableConfigurator), new object[] { false });
            else
                EnableConfigurator(false);
        }

        private delegate void EnableConfiguratorDel(bool enable);
        private void EnableConfigurator(bool enable)
        {
            groupBoxChannelsConfigurator.Enabled = enable;
        }

        private void LightController_OnPostRestoreFactorySettings(object sender, EventArgs args)
        {
            LightController.AsyncLoadFromController(-1);
        }

        private void LightController_OnPostSendSettingsToController(object sender, ChannelEventArgs args)
        {
            if (saveAfterSend)
            {
                LightController.SaveToFlash();
            }
            LightController.AsyncLoadFromController(-1);
        }

        private void LightController_OnPostLoadSettingsFromController(object sender, ChannelEventArgs args)
        {
            if (this.InvokeRequired)
                this.BeginInvoke(new MethodInvoker(ChanSettings2Form));
            else
                ChanSettings2Form();

            if (buttonExport.InvokeRequired)
                buttonExport.BeginInvoke(new ButtonEnableDel(ButtonEnable), new object[]{true});
            else
                ButtonEnable(true);
        }

        private void LightController_OnPostSaveSettingsToController(object sender, EventArgs args)
        {
            
        }
        #endregion

        #region Button Click Functions
        private void buttonSimulateInputTrigger_Click(object sender, EventArgs e)
        {
            LightController.SimulateInputTrigger(comboBoxChannel.SelectedIndex);
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            ButtonEnable(false);
            SaveFileDialog saveFileDlg = new SaveFileDialog();
            saveFileDlg.Filter = "XML files|*.xml";
            if (DialogResult.OK == saveFileDlg.ShowDialog())
            {
                LightController.SaveToXml(saveFileDlg.FileName);
            }
            ButtonEnable(true);
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            ButtonEnable(false);
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.Filter = "XML files|*.xml";
            if (DialogResult.OK == openFileDlg.ShowDialog())
            {
                LightController.LoadFromXml(openFileDlg.FileName);
            }
            ChanSettings2Form();
            ButtonEnable(true);
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            ButtonEnable(false);
            Form2ChanSettings(comboBoxChannel.SelectedIndex);
            LightController.AsyncSendToController(-1);
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            ButtonEnable(false);
            LightController.AsyncLoadFromController(-1);
        }

        bool saveAfterSend = false;
        private void buttonSave_Click(object sender, EventArgs e)
        {
            ButtonEnable(false);
            saveAfterSend = true;
            Form2ChanSettings(comboBoxChannel.SelectedIndex);
            LightController.AsyncSendToController(-1);
        }

        private void buttonRestore_Click(object sender, EventArgs e)
        {
            ButtonEnable(false);
            LightController.RestoreFactorySettings();
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            try
            {
                CreateHelpPage();
                //System.Diagnostics.Process.Start(LightController.ControllerSpecs.HelpStringPath);
                System.Diagnostics.Process.Start(@"C:\temp.html");
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, ClassName + ".buttonHelp_Click", "Impossibile aprire la pagina di help. Path: " + LightController.ControllerSpecs.HelpStringPath + "\r\n" + ex.ToString());
            }
        }

        private delegate void ButtonEnableDel(bool enable);
        private void ButtonEnable(bool enable)
        {
            if (enable) this.UseWaitCursor = false;
            else this.UseWaitCursor = true;
            buttonSimulateInputTrigger.Enabled = enable;
            buttonExport.Enabled = enable;
            buttonImport.Enabled = enable;
            buttonSend.Enabled = enable;
            buttonLoad.Enabled = enable;
            buttonSave.Enabled = enable;
            buttonRestore.Enabled = enable;
        }
        #endregion

        int comboBoxChannelSelected = 0;
        private void comboBoxChannel_SelectedIndexChanged(object sender, EventArgs e)
        {     
            Form2ChanSettings(comboBoxChannelSelected);
            ChanSettings2Form();
            comboBoxChannelSelected = comboBoxChannel.SelectedIndex;
            PopulateInputComboBox(comboBoxChannel.SelectedIndex);
        }

        private void comboBoxMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Form2ChanSettings(comboBoxChannel.SelectedIndex);
            numericUpDownMaxBright.Value = (decimal)LightController.ControllerSpecs.GetMaxBright(comboBoxChannel.SelectedIndex);
            OperatingModeChanged();
        }

        private void comboBoxInput_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Form2ChanSettings(comboBoxChannel.SelectedIndex);
        }

        private void PopulateInputComboBox(int channel)
        {
            comboBoxInput.Items.Clear();
            if (LightController.ControllerSpecs!=null &&
                LightController.ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_RT_SERIES)
            {
                comboBoxInput.Enabled = true;
                for (int i = 0; i < LightController.ControllerSpecs.ChanConfig[channel].PossibleInputTrigger.Count; i++)
                {
                    int IN = LightController.ControllerSpecs.ChanConfig[channel].PossibleInputTrigger[i];
                    comboBoxInput.Items.Add("IN " + (IN + 1).ToString());
                    if (IN == channel)
                        comboBoxInput.SelectedIndex = i;
                }
            }
            if (LightController.ControllerSpecs != null &&
                LightController.ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_PP_SERIES)
            {
                comboBoxInput.Enabled = false;
            }
        }

        private void OperatingModeChanged()
        {
            if (LightController != null &&
                LightController.ControllerSpecs != null &&
                LightController.ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_RT_SERIES)
            {
                LightModeGardasoftRT lm = (LightModeGardasoftRT)comboBoxMode.SelectedIndex;
                switch (lm)
                {
                    case LightModeGardasoftRT.OFF:
                    case LightModeGardasoftRT.Continuous:
                        numericUpDownDelayMs.Enabled = false;
                        numericUpDownWidthMs.Enabled = false;
                        numericUpDownRetriggerMs.Enabled = false;
                        break;
                    case LightModeGardasoftRT.Pulsed:
                        numericUpDownDelayMs.Enabled = true;
                        numericUpDownWidthMs.Enabled = true;
                        numericUpDownRetriggerMs.Enabled = true;
                        break;
                    case LightModeGardasoftRT.Selected:
                        //TODO
                        numericUpDownDelayMs.Enabled = true;
                        numericUpDownWidthMs.Enabled = true;
                        numericUpDownRetriggerMs.Enabled = true;
                        break;
                    case LightModeGardasoftRT.Switched:
                        //TODO
                        numericUpDownDelayMs.Enabled = true;
                        numericUpDownWidthMs.Enabled = true;
                        numericUpDownRetriggerMs.Enabled = true;
                        break;
                }
            }
            if (LightController != null &&
                LightController.ControllerSpecs!=null &&
                LightController.ControllerSpecs.ControllerFam == ControllerFamily.Gardasoft_PP_SERIES)
            {
                LightModeGardasoftPP lm = (LightModeGardasoftPP)comboBoxMode.SelectedIndex;
                switch (lm)
                {
                    case LightModeGardasoftPP.None:
                    case LightModeGardasoftPP.OFF:
                    case LightModeGardasoftPP.Continuous:
                        numericUpDownDelayMs.Enabled = false;
                        numericUpDownWidthMs.Enabled = false;
                        numericUpDownRetriggerMs.Enabled = false;
                        break;
                    case LightModeGardasoftPP.Pulsed:
                        numericUpDownDelayMs.Enabled = true;
                        numericUpDownWidthMs.Enabled = true;
                        numericUpDownRetriggerMs.Enabled = true;
                        break;
                    case LightModeGardasoftPP.Switched:
                        //TODO
                        numericUpDownDelayMs.Enabled = true;
                        numericUpDownWidthMs.Enabled = true;
                        numericUpDownRetriggerMs.Enabled = true;
                        break;
                }
            }
        }

        private void LightControllerConfigurator_Load(object sender, EventArgs e)
        {
        }

        private void numericUpDownSetBright_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownSetBright.Value > numericUpDownMaxBright.Value)
                numericUpDownSetBright.Value = numericUpDownMaxBright.Value;
        }

        private void numericUpDownMaxBright_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownSetBright.Value > numericUpDownMaxBright.Value)
                numericUpDownSetBright.Value = numericUpDownMaxBright.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int n = (int)numericUpDownTriggerMultipli.Value;
            for (int i = 0; i < n; i++)
            {
                LightController.SimulateInputTrigger(comboBoxChannel.SelectedIndex);
                System.Threading.Thread.Sleep(5);
            }
        }
    }
}
