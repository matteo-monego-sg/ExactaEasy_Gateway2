using System;

namespace OptrelInterProcessComm.Gateways
{
    /// <summary>
    /// 
    /// </summary>
    public class GatewayServerSerializationErrorEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public GatewayServerSerializationErrorEventArgs(string serverId, Exception ex)
        {
            ServerId = serverId;
            SerializationException = ex;
        }
        /// <summary>
        /// 
        /// </summary>
        public string ServerId { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Exception SerializationException { get; private set; }
    }
}
