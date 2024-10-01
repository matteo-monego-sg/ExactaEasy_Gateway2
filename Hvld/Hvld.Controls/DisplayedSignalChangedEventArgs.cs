using Hvld.Parser;

namespace Hvld.Controls
{
    /// <summary>
    /// Event args for the DisplayedSignalChanged event.
    /// </summary>
    public class DisplayedSignalChangedEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public DisplayedSignalChangedEventArgs(HvldFrame frame, OptrelSignal signal)
        {
            Frame = frame;
            Signal = signal;
        }
        /// <summary>
        /// 
        /// </summary>
        public HvldFrame Frame { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public OptrelSignal Signal { get; private set; }
    }
}
