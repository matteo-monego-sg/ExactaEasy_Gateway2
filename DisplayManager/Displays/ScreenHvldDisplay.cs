using Emgu.CV;
using Emgu.CV.Structure;
using Hvld.Controls;

namespace DisplayManager
{
    /// <summary>
    /// MM-11/01/2024: HVLD2.0 evo.
    /// </summary>
    public class ScreenHvldDisplay : Display
    {
        /// <summary>
        /// 
        /// </summary>
        protected HvldDisplayControl HvldDisplay { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ScreenHvldDisplay(string name, HvldDisplayControl hvldDisplay, IStation station) : base(name, new StationCollection() { station })
        {
            HvldDisplay = hvldDisplay;
        }
        /// <summary>
        /// 
        /// </summary>
        public override void RenderImage(IStation currentStation)
        {
            HvldDisplay.Visible = true;
            HvldDisplay.BringToFront();
        }
        /// <summary>
        /// 
        /// </summary>
        public override void RenderImage(Camera currentStation, Image<Rgb, byte> newImage)
        {
            base.RenderImage(currentStation, newImage);
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
