using InterProcessComm;
using InterProcessComm.Messaging;
using OptrelInterProcessComm.Utils;
using SocketMeister;
using SPAMI.Util.Logger;
using System;
using System.Net;

namespace OptrelInterProcessComm.Gateways
{
    /// <summary>
    /// The gateway client (supervisor).
    /// A gateway implements the communication logic (in this case, with the IPCGatewayServer) and
    /// the constructor accepts a type that implements the business logic of the IPC calls.
    /// </summary>
    /// <typeparam name="K">The business logic object.</typeparam>
    public class IPCGatewayClient<K> : IPCGatewayBase<K>
    {
        /// <summary>
        /// SocketMeister communication object.
        /// </summary>
        private SocketClient _socketClient;
        /// <summary>
        /// Raised when the client connected to an IpcGatewayServer.
        /// </summary>
        public event EventHandler OnConnectedToServer;
        /// <summary>
        /// Raised when the client disconnected from the IpcGatewayServer.
        /// </summary>
        public event EventHandler OnDisconnectedFromServer;
        /// <summary>
        /// Class constructor.
        /// </summary>
        public IPCGatewayClient(K logic, int port = IPCConstants.DEFAULT_IPC_COMM_PORT) : base(logic)
        {
            _comPort = port;
        }
        /// <summary>
        /// Connects to a remote IPC server.
        /// </summary>
        public void ConnectToServer()
        {
            if (!(_socketClient is null))
            {
                _socketClient.MessageReceived -= SocketClient_MessageReceived;
                _socketClient.ServerStopping -= SocketClient_ServerStopping;
                _socketClient.ConnectionStatusChanged -= SocketClient_ConnectionStatusChanged;
                _socketClient.ExceptionRaised -= SocketClient_ExceptionRaised;
                _socketClient.Stop();
                _socketClient.Dispose();
            }
            _socketClient = new SocketClient(IPAddress.Loopback.ToString(), _comPort, false);
            _socketClient.MessageReceived += SocketClient_MessageReceived;
            _socketClient.ServerStopping += SocketClient_ServerStopping;
            _socketClient.ConnectionStatusChanged += SocketClient_ConnectionStatusChanged;
            _socketClient.ExceptionRaised += SocketClient_ExceptionRaised;
        }
        /// <summary>
        /// Raised when a communication error occours.
        /// </summary>
        private void SocketClient_ExceptionRaised(object sender, ExceptionEventArgs e)
        {
            Log.Line(LogLevels.Warning, "IPCGatewayClient::SocketClient_ExceptionRaised", $"Socket client exception: {e.Exception}");
        }
        /// <summary>
        /// Raised when the connection status changes.
        /// </summary>
        private void SocketClient_ConnectionStatusChanged(object sender, EventArgs e)
        {
            switch (_socketClient.ConnectionStatus)
            {
                case SocketClient.ConnectionStatuses.Connected:
                    OnConnectedToServer?.Invoke(this, e);
                    break;

                case SocketClient.ConnectionStatuses.Disconnected:
                    OnDisconnectedFromServer?.Invoke(this, e);
                    break;
            }
        }
        /// <summary>
        /// The remote IPC server is stopping.
        /// </summary>
        private void SocketClient_ServerStopping(object sender, EventArgs e)
        {
            OnDisconnectedFromServer?.Invoke(this, e);
        }
        /// <summary>
        /// Raised when the client receives a message from the server.
        /// </summary>
        private void SocketClient_MessageReceived(object sender, SocketClient.MessageReceivedEventArgs e)
        {
            var message = IPCSerialization.Deserialize((byte[])e.Parameters[0]);

            if (message is null)
            {
                Log.Line(LogLevels.Warning, "IPCGatewayClient::SocketClient_MessageReceived", $"IPCGatewayClient received an IPCMessage null reference.");
                return;
            }
            // Checks the type of the message.
            switch (message.MessageType)
            {
                case IPCMessageTypeEnum.Unknown:
                    throw new Exception("Unknown IPC message type!");

                case IPCMessageTypeEnum.Request:
                    // Calls the corresponding function of the logic class (T).
                    ExecuteLogic(message.Request.FunctionName, message.Request.Parameters, out object _, out Exception _);
                    break;

                case IPCMessageTypeEnum.RequestWithResponse:
                    // The message is a request.
                    var request = message.Request;
                    // Calls the corresponding function of the logic class (T).
                    if (ExecuteLogic(message.Request.FunctionName, request.Parameters, out object result, out Exception ex))
                    {
                        // If there is a function return value.
                        if (!(result is null))
                            message.Response = new IPCResponse(
                                request.FunctionName,
                                result,
                                result.GetType(),
                                IPCResponseStatusEnum.OK);
                        else
                            message.Response = new IPCResponse(
                                request.FunctionName,
                                null,
                                null,
                                IPCResponseStatusEnum.OK);
                        // Message is now a response.
                        message.MessageType = IPCMessageTypeEnum.Response;
                        // Fill in the request only with the id and functionName.
                        message.Request.Parameters = null;
                        // Sends back to the server (ARTIC) the function result.
                        e.Response = IPCSerialization.Serialize(message);
                    }
                    else
                    {
                        message.Response = new IPCResponse(
                            request.FunctionName,
                            null,
                            null,
                            IPCResponseStatusEnum.KO,
                            $"Remote IPC function exception: {ex.Message}");
                        // Message is now a response.
                        message.MessageType = IPCMessageTypeEnum.Response;
                        // Check if the request can be dropped.
                        // Fill in the request only with the id and functionName.
                        message.Request = new IPCRequest()
                        {
                            FunctionName = request.FunctionName
                        };
                        // Sends back to the server (ARTIC) the function result.
                        e.Response = IPCSerialization.Serialize(message);
                    }
                    break;
            }
        }
        /// <summary>
        /// Is the client actually connected to an IPCGatewayServer?
        /// </summary>
        public override bool IsConnected 
        {
            get 
            {
                if (_socketClient is null) 
                    return false;

                switch (_socketClient.ConnectionStatus)
                {
                    case SocketClient.ConnectionStatuses.Connected:
                        return true;
                }
                return false;
            }
        }
        /// <summary>
        /// Disconnects from the IPCGatewayServer.
        /// </summary>
        public void DisconnectFromServer()
        {
            if (!IsConnected) return;
            _socketClient.Stop();
            _socketClient.Dispose();
        }
        /// <summary>
        /// Sends an IPCMessage with a remote function execution request over the IPC channel.
        /// Does not wait for a response message from the other end.
        /// </summary>
        public void PushRequest(IPCRequest request)
        {
            if (!IsConnected) return;
            if (request is null) return;

            var msg = new IPCMessage()
            {
                MessageType = IPCMessageTypeEnum.Request,
                Request = request
            };

            try
            {
                var data = IPCSerialization.Serialize(msg);
                _socketClient.SendMessage(new object[] { data });
            }
            catch(Exception ex)
            {
                Log.Line(LogLevels.Error, "IPCGatewayClient::PushRequest", $"Push failed: {ex}");
            }
        }
        /// <summary>
        /// Sends an IPCMessage with a remote function execution request over the IPC channel.
        /// Waits for a response message from the other end before returning.
        /// </summary>
        public IPCMessage PushRequestWithResponse(IPCRequest request, int responseTimeoutMs = IPCConstants.DEFAULT_RESPONSE_TIMEOUT_MS)
        {
            // Creates a new IPC message.
            var msg = new IPCMessage()
            {
                MessageType = IPCMessageTypeEnum.RequestWithResponse,
                Request = request
            };
            // Checks if the request is valid.
            if (request is null)
            {
                msg.Response = new IPCResponse(string.Empty, null, null, IPCResponseStatusEnum.KO, "invalid request");
                msg.MessageType = IPCMessageTypeEnum.Response;
                return msg;
            }

            if (!IsConnected)
            {
                msg.Response = new IPCResponse(string.Empty, null, null, IPCResponseStatusEnum.KO, "socket client not instantiated/connected");
                msg.MessageType = IPCMessageTypeEnum.Response;
                return msg;
            }

            try
            {
                var data = IPCSerialization.Serialize(msg);
                var response = _socketClient.SendMessage(new object[] { data }, responseTimeoutMs, true);

                if (response is null)
                {
                    msg.Response = new IPCResponse(request.FunctionName, null, null, IPCResponseStatusEnum.KO, "Response timeout");
                    msg.MessageType = IPCMessageTypeEnum.Response;
                    return msg;
                }
                else
                {
                    return IPCSerialization.Deserialize(response);
                }
            }
            catch(Exception ex)
            {
                Log.Line(LogLevels.Error, "IPCGatewayClient::PushRequestWithResponse", $"PushRequestWithResponse failed: {ex}");
                msg.Response = new IPCResponse(request.FunctionName, null, null, IPCResponseStatusEnum.KO, "Serialize/Push exception");
                msg.MessageType = IPCMessageTypeEnum.Response;
                return msg;
            }
        }
    }
}
