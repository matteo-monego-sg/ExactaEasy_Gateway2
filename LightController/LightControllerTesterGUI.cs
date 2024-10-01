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

using SPAMI.Util.Logger;

namespace SPAMI.LightControllers.GUI
{
    public partial class LightControllerTesterGUI : UserControl
    {
        public LightControllerTesterGUI()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!this.DesignMode)
            {
                if (LightController != null)
                    LightController.OnPostMessageReceived += new LightControllers.LightController.DelegateMessageReceived(LightController_OnPostMessageReceived);
            }
        }

        private LightController lightController;
        [Browsable(true), Category("Controller"), Description("TODO")]
        public LightController LightController
        {
            get
            {
                return lightController;
            }
            set 
            {
                lightController = value;
                if (lightControllerConfigurator1 != null) lightControllerConfigurator1.LightController = value;
                if (lightControllerSummaryGUI1 != null) lightControllerSummaryGUI1.LightController = value;
                if (lightControllerSearcher1 != null) lightControllerSearcher1.LightController = value;
            }
        }


        private void LightController_OnPostMessageReceived(object sender, MessageReceivedEventArgs args)
        {
            AppendComLog(args);
        }

        private void tabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            tabControl.SelectedTab.BackColor = Color.Black;
        }

        string comLogNewline = "";
        Color comLogForeColor = Color.Blue;
        public void AppendComLog(MessageReceivedEventArgs MexRec)
        {
            comLogForeColor = (MexRec.complete == true) ? Color.Blue : Color.Red;
            comLogNewline = MexRec.message;
            if (richTextBoxComLog.InvokeRequired)
                richTextBoxComLog.BeginInvoke(new MethodInvoker(ComLogAppend));
            else
                ComLogAppend();
        }

        private void ComLogAppend()
        {
            int lengthBefore = richTextBoxComLog.Text.Length;
            richTextBoxComLog.AppendText(comLogNewline+"\n");
            int lengthAfter = richTextBoxComLog.Text.Length;
            richTextBoxComLog.Select(lengthBefore, lengthAfter - lengthBefore);
            richTextBoxComLog.SelectionColor = comLogForeColor;
            //per scroll automatico verso il basso...
            richTextBoxComLog.SelectionStart = richTextBoxComLog.Text.Length;
            richTextBoxComLog.ScrollToCaret();
        }

        private void buttonClearLog_Click(object sender, EventArgs e)
        {
            richTextBoxComLog.Clear();
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabPageConfig)
                LightController.AsyncLoadFromController(-1);
        }


        
    }
}
