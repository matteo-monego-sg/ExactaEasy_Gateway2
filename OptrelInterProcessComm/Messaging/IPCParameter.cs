using MessagePack;

namespace InterProcessComm.Messaging
{
    /// <summary>
    /// An IPC remote procedure call parameter.
    /// </summary>
    [MessagePackObject]
    public class IPCParameter
    {
        /// <summary>
        /// Parameterless constructor: needed if using Newtonsoft.JSON serializer.
        /// </summary>
        public IPCParameter() { }
        /// <summary>
        /// 
        /// </summary>
        public IPCParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }
        /// <summary>
        /// Name of the parameter.
        /// </summary>
        [Key(0)]
        public string Name { get; set; }
        /// <summary>
        /// Value of the parameter.
        /// </summary>
        [Key(1)]
        public object Value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Name) || Value is null)
                return $"IPCParameter::ToString: malformed parameter (invalid Name/Value properties)";
            return $"{Name}: {Value} [{Value.GetType()}]";
        }
    }
}


