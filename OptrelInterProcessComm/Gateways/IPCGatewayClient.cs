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
        public event EventHandler<GatewayClientConnectedEventArgs> OnClientConnected;
        /// <summary>
        /// Raised when the client disconnected from the IpcGatewayServer.
        /// </summary>
        public event EventHandler<GatewayClientDisconnectedEventArgs> OnClientDisconnected;
        /// <summary>
        /// Raised when the gateway client encounters an error.
        /// </summary>
        public event EventHandler<GatewayClientErrorEventArgs> OnClientError;
        /// <summary>
        /// Raised when serialization/deserialization fails due to an exception.
        /// </summary>
        public event EventHandler<GatewayClientSerializationErrorEventArgs> OnClientSerializationError;
        /// <summary>
        /// Class constructor.
        /// </summary>
        public IPCGatewayClient(string clientId, K logic, int port = IPCConstants.DEFAULT_IPC_COMM_PORT) : base(clientId, logic)
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
                OnGatewayExecutionError -= IPCGatewayClient_OnGatewayExecutionError;
                // Preventively stops the server if running.
                _socketClient.Stop();
                _socketClient.Dispose();
            }
            _socketClient = new SocketClient(IPAddress.Loopback.ToString(), _comPort, false);
            _socketClient.MessageReceived += SocketClient_MessageReceived;
            _socketClient.ServerStopping += SocketClient_ServerStopping;
            _socketClient.ConnectionStatusChanged += SocketClient_ConnectionStatusChanged;
            _socketClient.ExceptionRaised += SocketClient_ExceptionRaised;
            OnGatewayExecutionError += IPCGatewayClient_OnGatewayExecutionError;
        }
        /// <summary>
        /// Pass the base class event to the client class.
        /// </summary>
        private void IPCGatewayClient_OnGatewayExecutionError(object sender, GatewayExecutionExceptionEventArgs e)
        {
            OnClientError?.Invoke(this, new GatewayClientErrorEventArgs(_id, e.ExecutionException));
        }
        /// <summary>
        /// Raised when a communication error occours.
        /// </summary>
        private void SocketClient_ExceptionRaised(object sender, ExceptionEventArgs e)
        {
            Log.Line(
                LogLevels.Warning, 
                "IPCGatewayClient::SocketClient_ExceptionRaised", 
                $"Socket client exception: {e.Exception}");
            // Raise an error event to the client class.
            OnClientError?.Invoke(this, new GatewayClientErrorEventArgs(_id, e.Exception));
        }
        /// <summary>
        /// Raised when the connection status changes.
        /// </summary>
        private void SocketClient_ConnectionStatusChanged(object sender, EventArgs e)
        {
            switch (_socketClient.ConnectionStatus)
            {
                case SocketClient.ConnectionStatuses.Connected:
                    OnClientConnected?.Invoke(this, new GatewayClientConnectedEventArgs(_id));
                    break;

                case SocketClient.ConnectionStatuses.Disconnected:
                    OnClientDisconnected?.Invoke(this, new GatewayClientDisconnectedEventArgs(_id));
                    break;
            }
        }
        /// <summary>
        /// The remote IPC server is stopping.
        /// </summary>
        private void SocketClient_ServerStopping(object sender, EventArgs e)
        {
            OnClientDisconnected?.Invoke(this, new GatewayClientDisconnectedEventArgs(_id));
        }
        /// <summary>
        /// Raised when the client receives a message from the server.
        /// </summary>
        private void SocketClient_MessageReceived(object sender, SocketClient.MessageReceivedEventArgs e)
        {
            // Check the parameters validity and length.
            if (e is null || e.Parameters is null || e.Parameters.Length == 0)
            {
                Log.Line(
                    LogLevels.Warning, 
                    "IPCGatewayClient::SocketClient_MessageReceived", 
                    $"Invalid MessageReceivedEventArgs parameter.");
                return;
            }
            // Deserializes the byte array into an IPC object.
            if (!IPCSerialization.Deserialize((byte[])e.Parameters[0], out IPCMessage message, out Exception ex))
            {
                // Raise an error event to the client class.
                OnClientSerializationError?.Invoke(this, new GatewayClientSerializationErrorEventArgs(_id, ex));
                return;
            }
            // Checks the type of the message.
            switch (message.MessageType)
            {
                case IPCMessageTypeEnum.Unknown:
                    // Raise an error event to the client class.
                    OnClientError?.Invoke(this, new GatewayClientErrorEventArgs(_id, new Exception("unknown IPC message type")));
                    break;

                case IPCMessageTypeEnum.Request:
                    // Calls the corresponding function of the logic class (T).
                    ExecuteLogic(message.Request.FunctionName, message.Request.Parameters, out object _);
                    break;

                case IPCMessageTypeEnum.RequestWithResponse:
                    {
                        // The message is a request.
                        var request = message.Request;
                        // Calls the corresponding function of the logic class (T).
                        if (ExecuteLogic(message.Request.FunctionName, request.Parameters, out object result))
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
                            byte[] data = null;
                            if (!IPCSerialization.Serialize(message, out data, out Exception serEx))
                            {
                                OnClientSerializationError?.Invoke(this, new GatewayClientSerializationErrorEventArgs(_id, serEx));
                                return;
                            }
                            e.Response = data;
                        }
                        else
                        {
                            message.Response = new IPCResponse(
                                request.FunctionName,
                                null,
                                null,
                                IPCResponseStatusEnum.KO,
                                $"Remote IPC function exception.");
                            // Message is now a response.
                            message.MessageType = IPCMessageTypeEnum.Response;
                            // Check if the request can be dropped.
                            // Fill in the request only with the id and functionName.
                            message.Request = new IPCRequest()
                            {
                                FunctionName = request.FunctionName
                            };
                            // Sends back to the server (ARTIC) the function result.
                            byte[] data = null;
                            if (!IPCSerialization.Serialize(message, out data, out Exception serEx))
                            {
                                OnClientSerializationError?.Invoke(this, new GatewayClientSerializationErrorEventArgs(_id, serEx));
                                return;
                            }
                            e.Response = data;
                        }
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
            if (!IsConnected) 
                return;
            _socketClient.Stop();
            _socketClient.Dispose();
        }
        /// <summary>
        /// Sends an IPCMessage with a remote function execution request over the IPC channel.
        /// Does not wait for a response message from the other end.
        /// </summary>
        public void PushRequest(IPCRequest request)
        {
            if (!IsConnected) 
                return;

            if (request is null) 
                return;

            var msg = new IPCMessage()
            {
                MessageType = IPCMessageTypeEnum.Request,
                Request = request
            };

            try
            {
                byte[] data = null;
                if (!IPCSerialization.Serialize(msg, out data, out Exception ex))
                {
                    OnClientSerializationError?.Invoke(this, new GatewayClientSerializationErrorEventArgs(_id, ex));
                    return;
                }
                _socketClient.SendMessage(new object[] { data });
            }
            catch(Exception ex)
            {
                Log.Line(
                    LogLevels.Error, 
                    "IPCGatewayClient::PushRequest", 
                    $"Push failed: {ex}");
                OnClientError?.Invoke(this, new GatewayClientErrorEventArgs(_id, ex));
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
            // Check if this cient is connected.
            if (!IsConnected)
            {
                msg.Response = new IPCResponse(string.Empty, null, null, IPCResponseStatusEnum.KO, "client not connected");
                msg.MessageType = IPCMessageTypeEnum.Response;
                return msg;
            }
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
                byte[] data = null;
                if (!IPCSerialization.Serialize(msg, out data, out Exception serEx))
                {
                    OnClientSerializationError?.Invoke(this, new GatewayClientSerializationErrorEventArgs(_id, serEx));
                    msg.Response = new IPCResponse(request.FunctionName, null, null, IPCResponseStatusEnum.KO, "serialization exception thrown");
                    msg.MessageType = IPCMessageTypeEnum.Response;
                    return msg;
                }
                var response = _socketClient.SendMessage(new object[] { data }, responseTimeoutMs, true);

                if (response is null)
                {
                    msg.Response = new IPCResponse(request.FunctionName, null, null, IPCResponseStatusEnum.KO, "response timeout");
                    msg.MessageType = IPCMessageTypeEnum.Response;
                    return msg;
                }
                else
                {
                    if (!IPCSerialization.Deserialize(response, out msg, out Exception desEx))
                    {
                        OnClientSerializationError?.Invoke(this, new GatewayClientSerializationErrorEventArgs(_id, desEx));
                        msg.Response = new IPCResponse(request.FunctionName, null, null, IPCResponseStatusEnum.KO, "deserialization exception thrown");
                        msg.MessageType = IPCMessageTypeEnum.Response;
                        return msg;
                    }
                    return msg;
                }
            }
            catch(Exception ex)
            {
                Log.Line(
                    LogLevels.Error, 
                    "IPCGatewayClient::PushRequestWithResponse", 
                    $"PushRequestWithResponse failed: {ex}");
                OnClientError?.Invoke(this, new GatewayClientErrorEventArgs(_id, ex));
                msg.Response = new IPCResponse(request.FunctionName, null, null, IPCResponseStatusEnum.KO, "serialization/push exception");
                msg.MessageType = IPCMessageTypeEnum.Response;
                return msg;
            }
        }
    }
}
