namespace InterProcessComm
{
    /// <summary>
    /// IPC communication global constants.
    /// </summary>
    internal class IPCConstants
    {
        /// <summary>
        /// Default IPC communication port.
        /// </summary>
        public const int DEFAULT_IPC_COMM_PORT = 6666;
        /// <summary>
        /// Default timeout interval for an IPC request.
        /// </summary>
        public const int DEFAULT_RESPONSE_TIMEOUT_MS = 5000;
        /// <summary>
        /// Connection to server wait timeout for ConnectAsync().
        /// </summary>
        public const int DEFAULT_CONNECTION_TIMEOUT_MS = 5000;
    }
}
