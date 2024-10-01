using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using SPAMI.Util;
using SPAMI.Util.Logger;

namespace SPAMI.LightControllers
{
    public partial class LightControllerSearcher : UserControl, ICommon
    {
        [Browsable(false)]
        public string ClassName { get; set; }
        public LightControllerSearcher()
        {
            ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
            InitializeComponent();
        }

        [Browsable(true), Category("Light Controller"), Description("TODO")]
        public LightController LightController { get; set; }
        bool Connected = false;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!this.DesignMode)
            {
                //Multilang.AddMeToMultilanguage(this);
                //Multilang.LanguageChanged += new EventHandler(OnLanguageChanged);
                textBoxStatus.Font = new System.Drawing.Font(textBoxStatus.Font, FontStyle.Bold);
                textBoxStatus.ForeColor = Color.Blue;
                if (LightController != null)
                {
                    LightController.OnPostConnection += new LightControllers.LightController.DelegateConnected(LightController_OnPostConnection);
                    LightController.OnPostDisconnection += new LightControllers.LightController.DelegateConnected(LightController_OnPostDisconnection);
                }
            }
        }

        /*public void Init(LightController lightController)
        {
            LightController = lightController;
            Multilanguage.AddMeToMultilanguage(this);
            Multilanguage.LanguageChanged += new EventHandler(OnLanguageChanged);
            textBoxStatus.Font = new System.Drawing.Font(textBoxStatus.Font, FontStyle.Bold);
            textBoxStatus.ForeColor = Color.Blue;
            try
            {
                LightController.OnPostConnection += new LightControllers.LightController.DelegateConnected(LightController_OnPostConnection);
                LightController.OnPostDisconnection += new LightControllers.LightController.DelegateConnected(LightController_OnPostDisconnection);
            }
            catch (System.Exception ex)
            {
                if (LightController == null)
                {
                    Log.Line(LogLevels.Error, "LightControllerTesterGUI-MyInitializeComponent", "FATAL ERROR! Check develop connection between modules." + "\r\n" + ex.ToString());
                    MessageBox.Show("FATAL ERROR! Check develop connection between modules.", "FATAL ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }*/

        /*public void OnLanguageChanged(object sender, EventArgs args)
        {
            string sAssName = Multilang.AssemblyNameExtract(System.Reflection.Assembly.GetExecutingAssembly());
            if (Multilang.IsAssemblyStringTableLoaded(sAssName))
            {
                groupBoxFindControllers.Text = Multilang.GetString(sAssName, "TrovaController");
                buttonSearch.Text = Multilang.GetString(sAssName, "Cerca");
                groupBoxConnection.Text = Multilang.GetString(sAssName, "Connessione");
                labelIPaddr.Text = Multilang.GetString(sAssName, "IndirizzoIP");
                buttonConnect.Text = Multilang.GetString(sAssName, "Connetti");
                buttonDisconnect.Text = Multilang.GetString(sAssName, "Disconnetti");
                labelSTATUS.Text = Multilang.GetString(sAssName, "STATUS");
                groupBoxCmd.Text = Multilang.GetString(sAssName, "InvioComandi");
                buttonSendCmd.Text = Multilang.GetString(sAssName, "Invia");
                ChangeStatus(Connected);
            }
        }*/

        private delegate void ChangeStatusDel(bool connected);
        private void ChangeStatus(bool connected)
        {
            /*string sAssName = Multilang.AssemblyNameExtract(System.Reflection.Assembly.GetExecutingAssembly());
            if (connected)
                textBoxStatus.Text = Multilang.GetString(sAssName, "Connesso");
            else
                textBoxStatus.Text = Multilang.GetString(sAssName, "Disconnesso");*/
            if (connected)
                textBoxStatus.Text = "Connesso";
            else
                textBoxStatus.Text = "Disconnesso";
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (LightController != null)
            {
                string ipAddress = ipAddressControl.IPAddress.ToString();
                try
                {
                    LightController.Bind(ipAddress, ControllerVendor.NotSet, ControllerName.None);
                    if (LightController.ControllerSpecs != null)
                    {
                        bool OK = (LightController.ControllerSpecs.Port > 0 && LightController.ControllerSpecs.Port < Math.Pow(2, 16)) ? true : false;
                        IPAddress address;
                        if (IPAddress.TryParse(ipAddress, out address))
                        {
                            switch (address.AddressFamily)
                            {
                                case System.Net.Sockets.AddressFamily.InterNetwork:
                                    // we have IPv4
                                    OK &= true;
                                    break;
                                case System.Net.Sockets.AddressFamily.InterNetworkV6:
                                    // we have IPv6
                                    break;
                                default:
                                    // umm... yeah... I'm going to need to take your red packet and...
                                    break;
                            }
                        }
                        LightController.Connect(ipAddress, LightController.ControllerSpecs.Port);
                    }
                    else
                        Log.Line(LogLevels.Error, ClassName + ".buttonConnect_Click", "ControllerSpecs = null.");
                }
                catch (Exception ex)
                {
                    Log.Line(LogLevels.Error, ClassName + ".buttonConnect_Click", "Errore! " + ex.Message);
                }
            }
            else
                Log.Line(LogLevels.Error, ClassName + ".buttonConnect_Click", "LightController = null.");
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            if (LightController != null)
                LightController.Disconnect();
            else
                Log.Line(LogLevels.Error, ClassName + ".buttonConnect_Click", "LightController = null.");
        }

        private void buttonSendCmd_Click(object sender, EventArgs e)
        {
            if (LightController != null)
                LightController.CmdSend(textBoxSendCmd.Text);
        }

        private void LightController_OnPostConnection(object sender, ConnectedEventArgs args)
        {
            if (textBoxStatus.InvokeRequired)
                textBoxStatus.BeginInvoke(new ChangeStatusDel(ChangeStatus), new object[]{true});
            else
                ChangeStatus(args.connected);
            Connected = args.connected;
        }

        private void LightController_OnPostDisconnection(object sender, ConnectedEventArgs args)
        {
            if (textBoxStatus.InvokeRequired)
                textBoxStatus.BeginInvoke(new ChangeStatusDel(ChangeStatus), new object[] { false });
            else
                ChangeStatus(args.connected);
            Connected = args.connected;
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            buttonSearch.Enabled = false;
            this.UseWaitCursor = true;
            listBoxDevice.Items.Clear();

            if ((LightController==null) || !LightController.SearchControllers())
            {
                Log.Line(LogLevels.Error, ClassName + ".buttonSearch_Click", "Error while searching connected controllers.");
            }

            Thread.Sleep(1000);

            if (LightController.DeviceConnectedList != null && LightController.DeviceConnectedList.Count > 0)
            {
                foreach (DeviceDetected dd in LightController.DeviceConnectedList)
                    listBoxDevice.Items.Add(dd.ToString());
            }
            this.UseWaitCursor = false;
            buttonSearch.Enabled = true;
        }

        private void listBoxDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxDevice != null && listBoxDevice.SelectedIndex > -1 && LightController.DeviceConnectedList!=null)
                ipAddressControl.Text = LightController.DeviceConnectedList[listBoxDevice.SelectedIndex].IP.ToString();
        }

    }

}
