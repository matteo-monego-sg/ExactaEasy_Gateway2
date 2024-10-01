using System.Drawing;

namespace Hvld.Parser
{
    /// <summary>
    /// 
    /// </summary>
    public static class HvldUtilities
    {
        /// <summary>
        /// Converts an RGBA color code into a System.Drawing.Color.
        /// </summary>
        public static Color GretelRGBAToColor(uint rgba)
        {
            var a = 0xFF;
            var r = int.Parse(((rgba & 0x00FF0000L) >> 16).ToString());
            var g = int.Parse(((rgba & 0x0000FF00L) >> 8).ToString());
            var b = int.Parse((rgba & 0x000000FFL).ToString());
            return Color.FromArgb(a, r, g, b);
        }
    }
}
