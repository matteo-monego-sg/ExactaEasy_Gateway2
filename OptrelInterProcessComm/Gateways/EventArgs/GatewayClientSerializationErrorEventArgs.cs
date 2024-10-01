using System;

namespace OptrelInterProcessComm.Gateways
{
    /// <summary>
    /// 
    /// </summary>
    public class GatewayClientSerializationErrorEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public GatewayClientSerializationErrorEventArgs(string clientId, Exception ex)
        {
            ClientId = clientId;
            SerializationException = ex;
        }
        /// <summary>
        /// 
        /// </summary>
        public string ClientId { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Exception SerializationException { get; private set; }
    }
}
