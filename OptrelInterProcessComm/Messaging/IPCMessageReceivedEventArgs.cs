namespace InterProcessComm.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    public class IPCMessageReceivedEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public IPCMessage Message { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public IPCMessageReceivedEventArgs(IPCMessage message)
        {
            Message = message;
        }
    }
}
