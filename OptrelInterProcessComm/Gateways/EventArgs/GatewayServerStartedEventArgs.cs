using System;

namespace OptrelInterProcessComm.Gateways
{
    /// <summary>
    /// 
    /// </summary>
    public class GatewayServerStartedEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public GatewayServerStartedEventArgs(string serverId)
        {
            ServerId = serverId;
        }
        /// <summary>
        /// 
        /// </summary>
        public string ServerId { get; private set; }
    }
}
