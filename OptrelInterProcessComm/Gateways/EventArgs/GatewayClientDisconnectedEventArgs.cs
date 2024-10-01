using System;

namespace OptrelInterProcessComm.Gateways
{
    /// <summary>
    /// 
    /// </summary>
    public class GatewayClientDisconnectedEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public GatewayClientDisconnectedEventArgs(string clientId)
        {
            ClientId = clientId;
        }
        /// <summary>
        /// 
        /// </summary>
        public string ClientId { get; private set; }
    }
}
