namespace Hvld.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class HvldSignalDisplayData
    {
        /// <summary>
        /// 
        /// </summary>
        public string SignalName { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public byte SignalId { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public OptrelSignal SignalControl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public HvldSignalDisplayData(string signalName, byte signalId, OptrelSignal signalControl)
        {
            SignalName = signalName;
            SignalId = signalId;
            SignalControl = signalControl;
        }
    }
}
