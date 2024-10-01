using System;

namespace OptrelInterProcessComm.Gateways
{
    /// <summary>
    /// 
    /// </summary>
    public class GatewayClientConnectedEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public GatewayClientConnectedEventArgs(string clientId)
        {
            ClientId = clientId;
        }
        /// <summary>
        /// 
        /// </summary>
        public string ClientId { get; private set; }
    }
}
