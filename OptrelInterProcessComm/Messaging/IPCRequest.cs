using MessagePack;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterProcessComm.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    [MessagePackObject]
    public class IPCRequest
    {
        /// <summary>
        /// Parameterless constructor: needed if using Newtonsoft.JSON serializer.
        /// </summary>
        public IPCRequest() { }
        /// <summary>
        /// Class initializer.
        /// </summary>
        public IPCRequest(string function, params IPCParameter[] parameters)
        {
            FunctionName = function;
            Parameters = parameters;
        }
        /// <summary>
        /// The message command. 
        /// Should be the name of the remote IPC procedure that must be called.
        /// </summary>
        [Key(0)]
        public string FunctionName { get; set; }
        /// <summary>
        /// A collection of parameters for the specified command.
        /// </summary>
        [Key(1)]
        public IEnumerable<IPCParameter> Parameters { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"CALL REQUEST: {FunctionName}");
            for (var i = 0; i < Parameters.Count(); ++i)
                sb.Append($";PARAM{i}: [{Parameters.ElementAt(i).Name}]=[{Parameters.ElementAt(i).Value}]");
            return sb.ToString();
        }
    }
}
