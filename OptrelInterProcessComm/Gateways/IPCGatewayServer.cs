using InterProcessComm;
using InterProcessComm.Messaging;
using OptrelInterProcessComm.Utils;
using SocketMeister;
using SPAMI.Util.Logger;
using System;

namespace OptrelInterProcessComm.Gateways
{
    /// <summary>
    /// The gateway server (ARTIC).
    /// A gateway implements the communication logic (in this case, with the IPCGatewayClient) and
    /// the constructor accepts a type that implements the business logic of the IPC calls.
    /// </summary>
    /// <typeparam name="K">The business logic object.</typeparam>
    public class IPCGatewayServer<K> : IPCGatewayBase<K>
    {
        /// <summary>
        /// SocketMeister communication object.
        /// </summary>
        private SocketServer _socketServer;
        /// <summary>
        /// Raised when a client connects to the server.
        /// </summary>
        public event EventHandler<GatewayClientConnectedEventArgs> OnClientConnected;
        /// <summary>
        ///  Raised when a client disconnects from the server.
        /// </summary>
        public event EventHandler<GatewayClientDisconnectedEventArgs> OnClientDisconnected;
        /// <summary>
        /// Raised when the server starts listening for client connections.
        /// </summary>
        public event EventHandler<GatewayServerStartedEventArgs> OnServerStarted;
        /// <summary>
        /// Raised when the server encounters an error.
        /// </summary>
        public event EventHandler<GatewayServerErrorEventArgs> OnServerError;
        /// <summary>
        /// Raised when the gateway encounters a serialization error.
        /// </summary>
        public event EventHandler<GatewayServerSerializationErrorEventArgs> OnServerSerializationError;
        /// <summary>
        /// Class constructor.
        /// </summary>
        public IPCGatewayServer(string serverId, K logic, int port = IPCConstants.DEFAULT_IPC_COMM_PORT) : base(serverId, logic)        
        {
            _comPort = port;
        }
        /// <summary>
        /// 
        /// </summary>
        private void IPCGatewayServer_OnGatewayExecutionError(object sender, GatewayExecutionExceptionEventArgs e)
        {
            OnServerError?.Invoke(this, new GatewayServerErrorEventArgs(_id, e.ExecutionException));
        }
        /// <summary>
        /// Is the server actually connected to an IPCGatewayClient?
        /// If there is at least one client connected, then it is.
        /// </summary>
        public override bool IsConnected
        {
            get
            {
                if (_socketServer is null)
                    return false;
                return _socketServer.ClientCount > 0;
            }
        }
        /// <summary>
        /// Starts the IPCGatewayServer.
        /// </summary>
        public bool Start()
        {
            Stop();
            try
            { 
                // Creates a new instance of the socket server.
                _socketServer = new SocketServer(_comPort, false);
                // Listen to RequestReceived events.
                _socketServer.MessageReceived += SocketServer_MessageReceived;
                _socketServer.ClientConnected += SocketServer_ClientConnected;
                _socketServer.ClientDisconnected += SocketServer_ClientDisconnected;
                OnGatewayExecutionError += IPCGatewayServer_OnGatewayExecutionError;

                _socketServer.Start();
                OnServerStarted?.Invoke(this, new GatewayServerStartedEventArgs(_id));
                return true;
            }
            catch (Exception ex)
            {
                Log.Line(
                    LogLevels.Error, 
                    "IPCGatewayServer::Start",
                    $"IPCGatewayServer could not start: {ex}");
                OnServerError?.Invoke(this, new GatewayServerErrorEventArgs(_id, ex));
                return false;
            }
        }
        /// <summary>
        /// Stops the IPCGatewayServer.
        /// </summary>
        public void Stop()
        {
            if (_socketServer is null)
                return;

            try
            {
                switch (_socketServer.Status)
                {
                    case SocketServerStatus.Stopping:
                    case SocketServerStatus.Stopped:
                        // If the server is stopping ar already stopped, exit here or
                        // meister hangs.
                        return;
                }
                _socketServer.MessageReceived -= SocketServer_MessageReceived;
                _socketServer.ClientConnected -= SocketServer_ClientConnected;
                _socketServer.ClientDisconnected -= SocketServer_ClientDisconnected;
                OnGatewayExecutionError -= IPCGatewayServer_OnGatewayExecutionError;
                _socketServer.Stop();
            }
            catch (Exception ex)
            {
                Log.Line(
                    LogLevels.Error,
                    "IPCGatewayServer::Stop", 
                    $"IPCGatewayServer could not stop: {ex}");
                OnServerError?.Invoke(this, new GatewayServerErrorEventArgs(_id, ex));
            }
        }
        /// <summary>
        /// Raised when a client disconnects from the server.
        /// </summary>
        private void SocketServer_ClientDisconnected(object sender, SocketServer.ClientEventArgs e)
        {
            OnClientDisconnected?.Invoke(this, new GatewayClientDisconnectedEventArgs(e.Client.ClientId.ToString()));
        }
        /// <summary>
        /// Raised when a client connects to the server.
        /// </summary>
        private void SocketServer_ClientConnected(object sender, SocketServer.ClientEventArgs e)
        {
           OnClientConnected?.Invoke(this, new GatewayClientConnectedEventArgs(e.Client.ClientId.ToString()));
        }
        /// <summary>
        /// Raised when the server receives a message from the client.
        /// </summary>
        private void SocketServer_MessageReceived(object sender, SocketServer.MessageReceivedEventArgs e)
        {
            // Check the parameters validity and length.
            if (e is null || e.Parameters is null || e.Parameters.Length == 0)
            {
                Log.Line(
                    LogLevels.Warning,
                    "IPCGatewayClient::SocketServer_MessageReceived",
                    $"Invalid MessageReceivedEventArgs parameter.");
                return;
            }
            // Deserializes the byte array into an IPC object.
            if (!IPCSerialization.Deserialize((byte[])e.Parameters[0], out IPCMessage message, out Exception ex))
            {
                // Raise an error event to the client class.
                OnServerSerializationError?.Invoke(this, new GatewayServerSerializationErrorEventArgs(_id, ex));
                return;
            }
            // Checks the type of the message.
            switch (message.MessageType)
            {
                case IPCMessageTypeEnum.Unknown:
                    // Raise an error event to the client class.
                    OnServerError?.Invoke(this, new GatewayServerErrorEventArgs(_id, new Exception("unknown IPC message type")));
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
                                OnServerSerializationError?.Invoke(this, new GatewayServerSerializationErrorEventArgs(_id, serEx));
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
                                OnServerSerializationError?.Invoke(this, new GatewayServerSerializationErrorEventArgs(_id, serEx));
                                return;
                            }
                            e.Response = data;
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// Sends an IPCMessage with a remote function execution over the IPC channel.
        /// Does not wait for a response message from the other end.
        /// </summary>
        public void PushRequest(IPCRequest request)
        {
            if (request is null) 
                return;

            var msg = new IPCMessage()
            {
                MessageType = IPCMessageTypeEnum.Request,
                Request = request
            };

            try
            {
                var clients = _socketServer.GetClients();
                // If there are no clients connected, exit immediately.
                if (clients is null || clients.Count == 0) 
                    return;

                byte[] data = null;
                if (!IPCSerialization.Serialize(msg, out data, out Exception ex))
                {
                    OnServerSerializationError?.Invoke(this, new GatewayServerSerializationErrorEventArgs(_id, ex));
                    return;
                }
                // Sends the message to the first client only.
                clients[0].SendMessage(new object[] { data });
            }
            catch(Exception ex)
            {
                Log.Line(
                    LogLevels.Error, 
                    "IPCGatewayServer::PushRequest", 
                    $"PushRequest failed: {ex}");
                OnServerError?.Invoke(this, new GatewayServerErrorEventArgs(_id, ex));
            }
        }
        /// <summary>
        /// Sends an IPCMessage with a remote function execution over the IPC channel.
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

            try
            {
                var clients = _socketServer.GetClients();
                // If there are no clients connected, exit immediately.
                if (clients is null || clients.Count == 0)
                {
                    msg.Response = new IPCResponse(string.Empty, null, null, IPCResponseStatusEnum.KO, "no clients connected");
                    msg.MessageType = IPCMessageTypeEnum.Response;
                    return msg;
                }
                   
                byte[] data = null;
                if (!IPCSerialization.Serialize(msg, out data, out Exception serEx))
                {
                    OnServerSerializationError?.Invoke(this, new GatewayServerSerializationErrorEventArgs(_id, serEx));
                    msg.Response = new IPCResponse(request.FunctionName, null, null, IPCResponseStatusEnum.KO, "serialization exception thrown");
                    msg.MessageType = IPCMessageTypeEnum.Response;
                    return msg;
                }
                // Sends the message to all the currently connected clients.
                var response = clients[0].SendMessage(new object[] { data }, responseTimeoutMs, true);

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
                        OnServerSerializationError?.Invoke(this, new GatewayServerSerializationErrorEventArgs(_id, desEx));
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
                    "IPCGatewayServer::PushRequestWithResponse", 
                    $"PushRequestWithResponse failed: {ex}");
                OnServerError?.Invoke(this, new GatewayServerErrorEventArgs(_id, ex));
                msg.Response = new IPCResponse(request.FunctionName, null, null, IPCResponseStatusEnum.KO, "serialization/push exception");
                msg.MessageType = IPCMessageTypeEnum.Response;
                return msg;
            }
        }
    }
}
