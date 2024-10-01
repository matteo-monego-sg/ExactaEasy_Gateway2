using InterProcessComm;
using InterProcessComm.Messaging;
using OptrelInterProcessComm.Utils;
using SocketMeister;
using SPAMI.Util.Logger;
using System;
using System.Linq;

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
        private readonly SocketServer _socketServer;
        /// <summary>
        /// Raised when a client connects to the server.
        /// </summary>
        public event EventHandler OnClientConnected;
        /// <summary>
        ///  Raised when a client disconnects from the server.
        /// </summary>
        public event EventHandler OnClientDisconnected;
        /// <summary>
        /// Raised when the server starts listening for client connections.
        /// </summary>
        public event EventHandler OnServerStarted;
        /// <summary>
        /// Class constructor.
        /// </summary>
        public IPCGatewayServer(K logic, int port = IPCConstants.DEFAULT_IPC_COMM_PORT) : base(logic)        
        {
            _comPort = port;
            // Creates a new instance of the socket server.
            _socketServer = new SocketServer(_comPort, false);
            // Listen to RequestReceived events.
            _socketServer.MessageReceived += SocketServer_MessageReceived;
            _socketServer.ClientConnected += SocketServer_ClientConnected;
            _socketServer.ClientDisconnected += SocketServer_ClientDisconnected;
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
            try
            {
                _socketServer.Start();
                OnServerStarted?.Invoke(this, EventArgs.Empty);
                return true;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "IPCGatewayServer::Start", $"IPCGatewayServer could not start: {ex}");
                return false;
            }
        }
        /// <summary>
        /// Stops the IPCGatewayServer.
        /// </summary>
        public void Stop()
        {
            try
            {
                _socketServer.Stop();
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "IPCGatewayServer::Stop", $"IPCGatewayServer could not stop: {ex}");
            }
        }
        /// <summary>
        /// Raised when a client disconnects from the server.
        /// </summary>
        private void SocketServer_ClientDisconnected(object sender, SocketServer.ClientEventArgs e)
        {
            OnClientDisconnected?.Invoke(this, e);
        }
        /// <summary>
        /// Raised when a client connects to the server.
        /// </summary>
        private void SocketServer_ClientConnected(object sender, SocketServer.ClientEventArgs e)
        {
            OnClientConnected?.Invoke(this, e);
        }
        /// <summary>
        /// Raised when the server receives a message from the client.
        /// </summary>
        private void SocketServer_MessageReceived(object sender, SocketServer.MessageReceivedEventArgs e)
        {
            var message = IPCSerialization.Deserialize((byte[])e.Parameters[0]);

            if (message is null)
            {
                Log.Line(LogLevels.Warning, "IPCGatewayServer::SocketServer_MessageReceived", $"IPCGatewayServer received an IPCMessage null reference.");
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
                        e.Response = IPCSerialization.Serialize(message).ToArray();
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
                        e.Response = IPCSerialization.Serialize(message).ToArray();
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
            if (request is null) return;
            var msg = new IPCMessage()
            {
                MessageType = IPCMessageTypeEnum.Request,
                Request = request
            };

            try
            {
                var client = _socketServer.GetClients().FirstOrDefault();
                if (client is null) return;

                var data = IPCSerialization.Serialize(msg);
                client.SendMessage(new object[] { data });
            }
            catch(Exception ex)
            {
                Log.Line(LogLevels.Error, "IPCGatewayServer::PushRequest", $"PushRequest failed: {ex}");
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
                var client = _socketServer.GetClients().FirstOrDefault();

                if (client is null)
                {
                    msg.Response = new IPCResponse(string.Empty, null, null, IPCResponseStatusEnum.KO, "no client connected");
                    msg.MessageType = IPCMessageTypeEnum.Response;
                    return msg;
                }

                var data = IPCSerialization.Serialize(msg);
                var response = client.SendMessage(new object[] { data }, responseTimeoutMs, true);

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
                Log.Line(LogLevels.Error, "IPCGatewayServer::PushRequestWithResponse", $"PushRequestWithResponse failed: {ex}");
                msg.Response = new IPCResponse(request.FunctionName, null, null, IPCResponseStatusEnum.KO, "Serialization/Push exception");
                msg.MessageType = IPCMessageTypeEnum.Response;
                return msg;
            }
        }
    }
}
