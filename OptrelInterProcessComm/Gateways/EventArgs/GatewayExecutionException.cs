using System;

namespace OptrelInterProcessComm.Gateways
{
    /// <summary>
    /// 
    /// </summary>
    public class GatewayExecutionExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public GatewayExecutionExceptionEventArgs(string gatewayId, Exception ex)
        {
            GatewayId = gatewayId;
            ExecutionException = ex;
        }
        /// <summary>
        /// 
        /// </summary>
        public string GatewayId { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Exception ExecutionException { get; private set; }
    }
}
