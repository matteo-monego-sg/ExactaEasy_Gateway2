namespace Hvld.Controls
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class SignalDisplayData
    {
        /// <summary>
        /// 
        /// </summary>
        public string SignalName { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int TableLayoutPanelRow { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public OptrelSignal SignalControl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SignalDisplayData(string signalName, OptrelSignal signalControl, int row)
        {
            SignalName = signalName;
            SignalControl = signalControl;
            TableLayoutPanelRow = row;
        }
    }
}
