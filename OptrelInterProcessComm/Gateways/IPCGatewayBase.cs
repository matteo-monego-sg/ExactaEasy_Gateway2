using InterProcessComm.Messaging;
using SPAMI.Util.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OptrelInterProcessComm.Gateways
{
    /// <summary>
    /// Wraps a type K containing the logic that should be executed by IPC.
    /// </summary>
    public abstract class IPCGatewayBase<K>
    {
        /// <summary>
        /// The _logic object Type.
        /// </summary>
        private readonly Type _logicClassType;
        /// <summary>
        /// Caching dictionary where KEY=function name, VALUE=MethodInfo object for reflection.
        /// </summary>
        private IDictionary<string, MethodInfo> _logicMethods = new Dictionary<string, MethodInfo>();
        /// <summary>
        /// The object containing the logic that must be executed via remote IPC.
        /// </summary>
        protected readonly K _logic;
        /// <summary>
        /// Communication port to be used.
        /// </summary>
        protected int _comPort;
        /// <summary>
        /// States if the gateway is connected or not.
        /// </summary>
        public abstract bool IsConnected { get; }
        /// <summary>
        /// Class constructor 1.
        /// </summary>
        public IPCGatewayBase(K serverLogic) : base()
        {
            _logic = serverLogic;
            _logicClassType = _logic.GetType();
            // Cache all the functions of the logic type.
            _logicMethods = CacheLogicClassFunctions(_logicClassType);
        }
        /// <summary>
        /// Gets a dictionary [KEY=func_name, VALUE=MethodInfo obj] of all 
        /// the possible public functions available on the type classType.
        /// </summary>
        private IDictionary<string, MethodInfo> CacheLogicClassFunctions(Type classType)
        {
            // Gets a collection of all the regular public methods of the type.
            var methods = classType
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => !m.IsSpecialName);
            // Fills a dictionary with all the MethodInfo object.
            foreach (var m in methods)
            {
                if (!_logicMethods.ContainsKey(m.Name))
                    _logicMethods.Add(m.Name, m);
            }
            return _logicMethods;
        }
        /// <summary>
        /// Calls a function of the logic interface K.
        /// </summary>
        protected bool ExecuteLogic(string func, IEnumerable<IPCParameter> parms, out object result, out Exception exception)
        {
            exception = null;
            result = null;
            try
            {
                // ---------------
                // OPTIMIZATION 1: calls _logic.GetType() in the constructor only once.
                //var method = _logic.GetType().GetMethod(func); // ===> _logicClassType.GetMethod(func);
                // OPTIMIZATION 2: build a dictionary of MethodInfo to cache all the possible callable methods of the type.
                //var method = _logicClassType.GetMethod(func); // ====> var method = _logicMethods[func];
                // ---------------
                // Gets the type of the derived class and the function to be called by reflection.
                var method = _logicMethods[func];
                // Aligns the parameter value type to the correct one.
                foreach (var p in parms)
                {
                    Type t = p.Value.GetType();
                    if (t.IsEnum)
                        t = Enum.GetUnderlyingType(t);
                    p.Value = Convert.ChangeType(p.Value, t);
                }
                // Executes the function.
                result = method.Invoke(_logic, parms.Select(x => x.Value).ToArray());
                return true;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Warning, "BaseIpcGateway::ExecuteLogic", $"Could not execute function '{func}': {ex.Message}");
                exception = ex;
                return false;
            }
        }
    }
}
