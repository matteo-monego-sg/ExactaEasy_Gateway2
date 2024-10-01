using System;

namespace OptrelInterProcessComm.Gateways
{
    /// <summary>
    /// 
    /// </summary>
    public class GatewayClientErrorEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="ex"></param>
        public GatewayClientErrorEventArgs(string clientId, Exception ex)
        {
            ClientId = clientId;
            ClientException = ex;
        }
        /// <summary>
        /// 
        /// </summary>
        public string ClientId { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Exception ClientException { get; private set; }
    }
}
