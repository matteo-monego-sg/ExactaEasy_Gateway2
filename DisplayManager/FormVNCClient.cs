using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using ExactaEasyEng.Utilities;
using MSTSCLib;
using SPAMI.Util.Logger;
using VncSharp;

namespace DisplayManager {
    public partial class FormVNCClient : Form, IRemoteDesktopForm {

        private string _currServer, _currUser;
        //private int _currPort;
        public event EventHandler<MessageEventArgs> RemoteConnectionError;
        private string _password;
        private bool _showToolbar;

        public bool Connected {
            get {
                return rd.IsConnected;
            }
        }

        public FormVNCClient() {

            InitializeComponent();
            lblQuestion.Text = VisionSystemManager.UIStrings.GetString("RemoteRestartConfirm") + "?";
            lblRestarting.Text = VisionSystemManager.UIStrings.GetString("Restarting") + " ...";
            btnYes.Text = VisionSystemManager.UIStrings.GetString("Yes");
            btnNo.Text = VisionSystemManager.UIStrings.GetString("No");
            //GretelSvc.ClientDisconnectRD += new EventHandler<ConnectionEventArgs>(GretelSvc_ClientDisconnectRD);
        }

        public FormVNCClient(bool showToolbar)
            : this() {
            _showToolbar = showToolbar;
            pnlMenu.Visible = showToolbar;
        }

        public void Connect(string server, string user, string password, int port) 
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(rd))
                return;

            if (rd.InvokeRequired && rd.IsHandleCreated)
                rd.Invoke((MethodInvoker)(() => Connect(server, user, password, port)));
            else {
                _password = password;
                if (port < 1 | port > 65535) port = 5900;   //default VNCPort
                try {
                    rd.GetPassword = new AuthenticateDelegate(GetPassword);
                    rd.Connect(server, false, false);
                    _currServer = server;
                    _currUser = user;
                    //_currPort = port;
                }
                catch {
                    Hide();
                    OnRemoteConnectionError(this, new MessageEventArgs(server + ": " + port + ": " + VisionSystemManager.UIStrings.GetString("RemoteConnectionError")));
                    //MessageBox.Show("Error Connecting", "Error connecting to remote desktop " + server + "/" + user + " Error:  " + Ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void Disconnect() 
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(rd))
                return;

            if (rd.InvokeRequired && rd.IsHandleCreated)
                rd.Invoke(new MethodInvoker(Disconnect));
            else {
                try {
                    // Check if connected before disconnecting
                    if (rd.IsConnected)
                        rd.Disconnect();
                    Hide();
                }
                catch (Exception ex) {
                    OnRemoteConnectionError(this, new MessageEventArgs(rd.Hostname + ": " + rd.VncPort + ": " + ex.Message));
                    //MessageBox.Show("Error Disconnecting", "Error disconnecting from remote desktop " + currServer + "/" + currUser + " Error:  " + Ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        protected virtual void OnRemoteConnectionError(object sender, MessageEventArgs e) {
            if (RemoteConnectionError != null) RemoteConnectionError(sender, e);
        }

        private void FormVNCClient_FormClosed(object sender, FormClosedEventArgs e) {

            Disconnect();
        }

        private void rd_ClipboardChanged(object sender, EventArgs e) {
            Log.Line(LogLevels.Debug, "FormVNCClient.rd_ClipboardChanged", "VNC clipboard changed from " + rd.Hostname + ": " + rd.VncPort);
        }

        private void rd_ConnectComplete(object sender, VncSharp.ConnectEventArgs e) {
            Log.Line(LogLevels.Pass, "FormVNCClient.rd_ConnectComplete", "VNC connected to " + rd.Hostname + ": " + rd.VncPort);
            //Opacity = 100;
            //Refresh();
            int height = e.DesktopHeight;
            if (_showToolbar) height += pnlMenu.Height;
            ClientSize = new Size(e.DesktopWidth, height);
            Text = e.DesktopName;
            rd.Focus();
        }

        private void rd_ConnectionLost(object sender, EventArgs e) {
            Log.Line(LogLevels.Warning, "FormVNCClient.rd_ConnectionLost",
                "Remote desktop disconnected from " + rd.Hostname + ": " + rd.VncPort);
            Hide();
        }

        private string GetPassword() {
            return _password;
        }

        private void btnExit_Click(object sender, EventArgs e) {

            Close();
        }

        private void btnRestart_Click(object sender, EventArgs e) {

            pnlMain.Visible = false;
            pnlRestarting.Visible = false;
            pnlConfirm.Visible = true;
            pnlConfirm.BringToFront();
        }

        private void btnYes_Click(object sender, EventArgs e) {

            pnlConfirm.Visible = false;
            pnlRestarting.Visible = true;
            pnlRestarting.BringToFront();
            lblRestarting.Text = "Sorry, not implemented yet" + " ...";
            Process.Start("psshutdown.exe", @"\\" + _currServer + " -u " + _currUser + " -p " + _password + " -r -f -t 0");
        }

        private void btnNo_Click(object sender, EventArgs e) {

            pnlMain.Visible = true;
            pnlRestarting.Visible = false;
            pnlConfirm.Visible = false;
            pnlMain.BringToFront();
        }
    }
}
