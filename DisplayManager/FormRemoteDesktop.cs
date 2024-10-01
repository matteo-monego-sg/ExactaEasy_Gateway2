using System;
using System.Globalization;
using System.Windows.Forms;
using ExactaEasyEng.Utilities;
using MSTSCLib;
using SPAMI.Util.Logger;

namespace DisplayManager {

    public interface IRemoteDesktopForm {
        event EventHandler<MessageEventArgs> RemoteConnectionError;
        //event EventHandler<MessageEventArgs> RemoteDisconnection;
        bool Connected { get; }

        void Connect(string server, string user, string password, int port);
        void Disconnect();
    }

    public partial class FormRemoteDesktop : Form, IRemoteDesktopForm {

        private string _currServer, _currUser;//, _currPassword;
        private bool _showToolbar;
        public event EventHandler<MessageEventArgs> RemoteConnectionError;

        public bool Connected {
            get {
                bool ret;
                try {
                    ret = rdp.Connected == 1 ? true : true;
                }
                catch {
                    ret = false;
                }
                return ret;
            }
        }

        public FormRemoteDesktop() {

            InitializeComponent();
            rdp.OnConnected += rdp_OnConnected;
            rdp.OnDisconnected += rdp_OnDisconnected;
            //GretelSvc.ClientDisconnectRD += new EventHandler<ConnectionEventArgs>(GretelSvc_ClientDisconnectRD);
        }

        public FormRemoteDesktop(bool showToolbar)
            : this() {
            _showToolbar = showToolbar;
        }

        public void Connect(string server, string user, string password, int port) 
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(rdp))
                return;

            if (rdp.InvokeRequired && rdp.IsHandleCreated)
                rdp.Invoke((MethodInvoker)(() => Connect(server, user, password, port)));
            else {
                try {
                    rdp.Server = server;
                    rdp.UserName = user;

                    var secured = (IMsTscNonScriptable)rdp.GetOcx();
                    secured.ClearTextPassword = password;
                    //rdp.DesktopHeight = Screen.PrimaryScreen.Bounds.Height;
                    //rdp.DesktopWidth = Screen.PrimaryScreen.Bounds.Width;
                    rdp.ConnectingText = VisionSystemManager.UIStrings.GetString("Connecting") + " ...";
                    rdp.DesktopWidth = Screen.PrimaryScreen.Bounds.Width;
                    rdp.DesktopHeight = Screen.PrimaryScreen.Bounds.Height;
                    rdp.AdvancedSettings7.PerformanceFlags = 0x80 | 
                                                             0x10 | 
                                                             0x100;
                    /* PERFORMANCE FLAGS:
                     * 0x010: Enable enhanced graphics
                     * 0x080: Enable font smoothing
                     * 0x100: Enable desktop composition
                     */
                    rdp.ColorDepth = 32;    //default = 16, causava crash splash Gretel
                    rdp.OnLogonError += rdp_OnLogonError;
                    rdp.OnFatalError += rdp_OnFatalError;
                    rdp.OnWarning += rdp_OnWarning;
                    rdp.Connect();

                    _currServer = server;
                    _currUser = user;
                    //_currPassword = password;
                }
                catch (Exception ex) {
                    OnRemoteConnectionError(this, new MessageEventArgs(server + "/" + user + ": " + ex.Message));
                    //MessageBox.Show("Error Connecting", "Error connecting to remote desktop " + server + "/" + user + " Error:  " + Ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        void rdp_OnWarning(object sender, AxMSTSCLib.IMsTscAxEvents_OnWarningEvent e) {
            OnRemoteConnectionError(this, new MessageEventArgs(_currServer + "/" + _currUser + ": Warning " + e.warningCode));
        }

        void rdp_OnFatalError(object sender, AxMSTSCLib.IMsTscAxEvents_OnFatalErrorEvent e) {
            OnRemoteConnectionError(this, new MessageEventArgs(_currServer + "/" + _currUser + ": Fatal error " + e.errorCode));
        }

        void rdp_OnLogonError(object sender, AxMSTSCLib.IMsTscAxEvents_OnLogonErrorEvent e) {
            OnRemoteConnectionError(this, new MessageEventArgs(_currServer + "/" + _currUser + ": Logon error " + e.lError));
        }

        public void Disconnect() 
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(rdp))
                return;

            if (rdp.InvokeRequired && rdp.IsHandleCreated)
                rdp.Invoke(new MethodInvoker(Disconnect));
            else {
                try {
                    // Check if connected before disconnecting
                    if (rdp.Connected.ToString(CultureInfo.InvariantCulture) == "1")
                        rdp.Disconnect();
                    Hide();
                }
                catch (Exception ex) {
                    OnRemoteConnectionError(this, new MessageEventArgs(_currServer + "/" + _currUser + ": " + ex.Message));
                    //MessageBox.Show("Error Disconnecting", "Error disconnecting from remote desktop " + currServer + "/" + currUser + " Error:  " + Ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        protected void rdp_OnConnected(object sender, EventArgs e) {
            Log.Line(LogLevels.Pass, "FormRemoteDesktop.rdp_OnConnected", "Remote desktop connected to " + rdp.Server + "/" + rdp.UserName);
            //Opacity = 100;
            Refresh();
        }

        protected void rdp_OnDisconnected(object sender, AxMSTSCLib.IMsTscAxEvents_OnDisconnectedEvent e) {
            Log.Line(e.discReason == 1 ? LogLevels.Debug : LogLevels.Warning, "FormRemoteDesktop.rdp_OnDisconnected",
                "Remote desktop disconnected from " + rdp.Server + "/" + rdp.UserName + ": " +
                e.discReason.ToString(CultureInfo.InvariantCulture));
            // 07/05/2024: hiding instead of closing leaves the RDP window alive.
            Close();
        }

        protected virtual void OnRemoteConnectionError(object sender, MessageEventArgs e) {
            if (RemoteConnectionError != null) RemoteConnectionError(sender, e);
        }

        private void FormRemoteDesktop_FormClosed(object sender, FormClosedEventArgs e) {

            Disconnect();
        }

        //private void GretelSvc_ClientDisconnectRD(object sender, ConnectionEventArgs e) {
        //    Disconnect();
        //}
    }

    public class MessageEventArgs : EventArgs {
        public string Message;
        public MessageEventArgs(string message) {
            Message = message;
        }
    }
}
