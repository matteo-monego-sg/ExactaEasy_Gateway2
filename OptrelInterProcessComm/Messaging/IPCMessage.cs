using MessagePack;

namespace InterProcessComm.Messaging
{
    /// <summary>
    /// The possible types of IPCMessage.
    /// Serialization/deserialization:
    /// https://github.com/MessagePack-CSharp/MessagePack-CSharp?tab=readme-ov-file#multiple-messagepack-structures-on-a-single-stream
    /// </summary>
    public enum IPCMessageTypeEnum : byte 
    {
        Unknown = 0,
        Request,
        RequestWithResponse,
        Response
    }
    /// <summary>
    /// IPC base communication exchange block. 
    /// </summary>
    [MessagePackObject]
    public class IPCMessage
    {
        /// <summary>
        /// Parameterless constructor: needed if using Newtonsoft.JSON serializer.
        /// </summary>
        public IPCMessage() { }
        /// <summary>
        /// Type of the message.
        /// </summary>
        [Key(0)]
        public IPCMessageTypeEnum MessageType { get; set; }
        /// <summary>
        /// IPC request for remote function execution.
        /// </summary>
        [Key(1)]
        public IPCRequest Request { get; set; }
        /// <summary>
        /// IPC response to a remote function execution.
        /// </summary>
        [Key(2)]
        public IPCResponse Response { get; set; }
    }
}
