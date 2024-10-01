using System.Diagnostics;
using System.Linq;

namespace OptrelInterProcessComm.Utils
{
    /// <summary>
    /// Utility class acting on the communicating processes of the IPC.
    /// </summary>
    public static class IPCProcess
    {
        /// <summary>
        /// Returns true if the process procName is running.
        /// </summary>
        public static bool IsRunning(string procName)
        {
            if (string.IsNullOrWhiteSpace(procName))
                return false;
            return Process.GetProcesses().Where(p => p.ProcessName.ToUpper().StartsWith(procName.ToUpper())).Count() > 0;
        }
        /// <summary>
        /// Returns true if any of the specified processes is running.
        /// </summary>
        public static bool IsAnyRunning(params string[] processNames)
        {
            foreach (var pn in processNames)
            {
                if (IsRunning(pn))
                    return true;
            }
            return false;
        }
    }
}
