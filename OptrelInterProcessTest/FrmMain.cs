using InterProcessComm.Messaging;
using OptrelInterProcessComm.Gateways;
using System;
using System.Text;
using System.Windows.Forms;

namespace OptrelInterProcessTest
{
    public partial class FrmMain : Form
    {
        /// <summary>
        /// Simulates ARTIC gateway, with a test logic.
        /// </summary>
        private IPCGatewayServer<TestGatewayLogic> _ipcGateway;
        /// <summary>
        /// 
        /// </summary>
        private TestGatewayLogic _testGatewayLogic;
        /// <summary>
        /// 
        /// </summary>
        public FrmMain()
        {
            InitializeComponent();
            _testGatewayLogic = new TestGatewayLogic(this);
            _ipcGateway = new IPCGatewayServer<TestGatewayLogic>("artic_gateway2_server", _testGatewayLogic);
        }
        /// <summary>
        /// 
        /// </summary>
        private void IpcGateway_OnGatewayExecutionError(object sender, GatewayExecutionExceptionEventArgs e)
        {
            _ipcGateway.Stop();
            Log($"SERVER '{e.GatewayId}' EXECUTION ERROR:{Environment.NewLine}{Environment.NewLine}{e.ExecutionException}.");
        }
        /// <summary>
        /// 
        /// </summary>
        private void IpcGateway_OnServerSerializationError(object sender, GatewayServerSerializationErrorEventArgs e)
        {
            _ipcGateway.Stop();
            Log($"SERVER '{e.ServerId}' SERIALIZATION ERROR:{Environment.NewLine}{Environment.NewLine}{e.SerializationException}.");
        }
        /// <summary>
        /// 
        /// </summary>
        private void IpcGateway_OnServerError(object sender, GatewayServerErrorEventArgs e)
        {
            _ipcGateway.Stop();
            Log($"SERVER '{e.ServerId}' ERROR:{Environment.NewLine}{Environment.NewLine}{e.ServerException}.");
        }
        /// <summary>
        /// 
        /// </summary>
        private void IpcGateway_OnClientDisconnected(object sender, EventArgs e)
        {
            Log($"CLIENT DISCONNECTED.");
        }
        /// <summary>
        /// 
        /// </summary>
        private void IpcGateway_OnClientConnected(object sender, EventArgs e)
        {
            Log($"CLIENT CONNECTED.");
        }
        /// <summary>
        /// 
        /// </summary>
        private void IpcGateway_OnServerStarted(object sender, EventArgs e)
        {
            _ipcGateway.OnServerError -= IpcGateway_OnServerError;
            _ipcGateway.OnServerError += IpcGateway_OnServerError;
            PnlCommands.Enabled = true;
            Log($"GATEWAY2 SERVER STARTED SUCCESSFULLY.");
        }
        /// <summary>
        /// 
        /// </summary>
        private void FrmMain_Load(object sender, EventArgs e)
        {
            PnlCommands.Enabled = false;
        }
        /// <summary>
        /// 
        /// </summary>
        private void BtnStartGateway2Server_Click(object sender, EventArgs e)
        {
            if (_ipcGateway.IsConnected)
            {
                Log("GATEWAY2 SERVER IS ALREADY STARTED AND CONNECTED TO A CLIENT."); 
                return;
            }
            _ipcGateway.OnServerStarted += IpcGateway_OnServerStarted;
            _ipcGateway.OnClientConnected += IpcGateway_OnClientConnected;
            _ipcGateway.OnClientDisconnected += IpcGateway_OnClientDisconnected;
            _ipcGateway.OnServerError += IpcGateway_OnServerError;
            _ipcGateway.OnServerSerializationError += IpcGateway_OnServerSerializationError;
            _ipcGateway.OnGatewayExecutionError += IpcGateway_OnGatewayExecutionError;
            // Starts the gateway server.
            _ipcGateway.Start();
        }
        private void BtnStopGateway2Server_Click(object sender, EventArgs e)
        {
            if (_ipcGateway is null)
                return;

            _ipcGateway.OnServerStarted -= IpcGateway_OnServerStarted;
            _ipcGateway.OnClientConnected -= IpcGateway_OnClientConnected;
            _ipcGateway.OnClientDisconnected -= IpcGateway_OnClientDisconnected;
            _ipcGateway.OnServerError -= IpcGateway_OnServerError;
            _ipcGateway.OnServerSerializationError -= IpcGateway_OnServerSerializationError;
            _ipcGateway.OnGatewayExecutionError -= IpcGateway_OnGatewayExecutionError;
            // Starts the gateway server.
            _ipcGateway.Stop();
            PnlCommands.Enabled = false;
            Log("GATEWAY2 SERVER STOPPED.");
        }

        /// <summary>
        /// 
        /// </summary>
        private void BtnSetSupervisorHide_Click(object sender, EventArgs e)
        {
            var ipcRequest = new IPCRequest()
            {
                FunctionName = "SetSupervisorHide"
            };
            var respMex = _ipcGateway.PushRequestWithResponse(ipcRequest);

            if (respMex is null)
                return;

            switch (respMex.Response.Status)
            {
                case IPCResponseStatusEnum.OK:
                    Log($"SetSupervisorHide RETURNED: {respMex.Response.ReturnValueAs<int>()}.");
                    break;

                case IPCResponseStatusEnum.KO:
                    Log($"SetSupervisorHide ERROR: {respMex.Response.ErrorDescription}");
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void BtnSetSupervisorOnTop_Click(object sender, EventArgs e)
        {
            var ipcRequest = new IPCRequest()
            {
                FunctionName = "SetSupervisorOnTop"
            };
            var respMex = _ipcGateway.PushRequestWithResponse(ipcRequest);

            if (respMex is null)
                return;

            switch (respMex.Response.Status)
            {
                case IPCResponseStatusEnum.OK:
                    Log($"SetSupervisorOnTop RETURNED: {respMex.Response.ReturnValueAs<int>()}.");
                    break;

                case IPCResponseStatusEnum.KO:
                    Log($"SetSupervisorOnTop ERROR: {respMex.Response.ErrorDescription}");
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        internal void Log(string line)
        {
            if (TxtLog.InvokeRequired)
            {
                TxtLog.Invoke(new MethodInvoker(() => { Log(line); }));
            }
            else
            {
                if (TxtLog.Lines.Length > 200)
                    TxtLog.Clear();
                TxtLog.AppendText($"{Environment.NewLine}{DateTime.Now:HH:mm:ss.fff} - {line}");
            }
        }
    }
}
