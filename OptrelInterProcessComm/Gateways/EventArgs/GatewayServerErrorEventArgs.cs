using System;

namespace OptrelInterProcessComm.Gateways
{
    /// <summary>
    /// 
    /// </summary>
    public class GatewayServerErrorEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="ex"></param>
        public GatewayServerErrorEventArgs(string serverId, Exception ex)
        {
            ServerId = serverId;
            ServerException = ex;
        }
        /// <summary>
        /// 
        /// </summary>
        public string ServerId { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Exception ServerException { get; private set; }
    }
}
