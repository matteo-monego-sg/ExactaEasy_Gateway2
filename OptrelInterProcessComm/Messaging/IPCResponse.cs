using MessagePack;
using SPAMI.Util.Logger;
using System;
using System.Text;

namespace InterProcessComm.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    public enum IPCResponseStatusEnum : byte 
    {
        Unknown = 0,
        KO,
        OK
    }
    /// <summary>
    /// 
    /// </summary>
    [MessagePackObject]
    public class IPCResponse
    {
        /// <summary>
        /// Parameterless constructor: needed if using Newtonsoft.JSON serializer.
        /// </summary>
        public IPCResponse() { }
        /// <summary>
        /// 
        /// </summary>
        public IPCResponse(
            string function, 
            object returnValue, 
            Type returnValueType, 
            IPCResponseStatusEnum status = IPCResponseStatusEnum.Unknown,
            string errorDescription = "")
        {
            FunctionName = function;
            Value = returnValue;
            Status = status;
            ErrorDescription = errorDescription;
        }
        /// <summary>
        /// The response status: OK/KO.
        /// </summary>
        [Key(0)]
        public IPCResponseStatusEnum Status { get; set; }
        /// <summary>
        /// Error description if the status is KO.
        /// </summary>
        [Key(1)]
        public string ErrorDescription { get; set; }
        /// <summary>
        /// The IPC function called.
        /// </summary>
        [Key(2)]
        public string FunctionName { get; set; }
        /// <summary>
        /// Return value of the function, if not void.
        /// </summary>
        [Key(3)]
        public object Value { get; set; }
        /// <summary>
        /// Returns Value casted to the specified type.
        /// </summary>
        public T ReturnValueAs<T>()
        {
            try
            {
                return (T)Convert.ChangeType(Value, typeof(T));
            }
            catch (Exception ex)
            {
                Log.Line(
                    LogLevels.Warning, 
                    "IPCResponse::ReturnValueAs", $"Possible mismatch between IPC interfaces (ARTIC/ExactaEasy)! Convert.ChangeType failed: {ex}");
                return default(T);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"CALL RESPONSE: {FunctionName}");
            sb.Append($"; RETVAL=[{Value}],CALL_STATUS=[{Status}]");
            return sb.ToString();
        }
    }
}
