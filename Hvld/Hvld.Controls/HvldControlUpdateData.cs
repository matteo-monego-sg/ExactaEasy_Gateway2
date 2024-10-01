using System.Drawing;

namespace Hvld.Controls
{
    /// <summary>
    /// HvldControlUpdateData is a pass-through class to define the behavior, colors and text
    /// of an HVLD control. All this tweaks can be changed in a frame update call. 
    /// </summary>
    public class HvldControlUpdateData
    {
        /// <summary>
        /// X-Axis title for the signal.
        /// </summary>
        public string XAxisTitle { get; set; }
        /// <summary>
        /// Y-Axis title for the signal.
        /// </summary>
        public string YAxisTitle { get; set; }
        /// <summary>
        /// Display or not the bounding lines (min-max) of the signal.
        /// </summary>
        public bool ShowSignalBoundingLines { get; set; }
        /// <summary>
        /// Use scientific notation for the Y-axis values.
        /// </summary>
        public bool ScientificNotationOnYAxis { get; set; }
        /// <summary>
        /// Size of the border around the signal display rectangle.
        /// </summary>
        public int BorderSize { get; set; }
        /// <summary>
        /// Color of the border when the signal is in-spec.
        /// </summary>
        public Color InSpecBorderColor { get; set; }
        /// <summary>
        /// Color of the border when the signal is out-spec.
        /// </summary>
        public Color OutSpecBorderColor { get; set; }
        /// <summary>
        /// Optionally overrides the frame-defined signal color.
        /// </summary>
        public Color? SignalColorOverride { get; set; } = null;
    }
}
