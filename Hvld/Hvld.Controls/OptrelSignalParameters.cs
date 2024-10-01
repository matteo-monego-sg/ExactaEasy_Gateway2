using Hvld.Parser;
using System.Drawing;
using System.Linq;

namespace Hvld.Controls
{
    /// <summary>
    /// Defines an OptrelSignal by its parameters.
    /// </summary>
    public sealed class OptrelSignalParameters
    {
        /// <summary>
        /// 
        /// </summary>
        public byte SignalId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SignalName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string XAxisTitle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string YAxisTitle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double[] XAxisValues { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double[] YAxisValues { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double YAxisMinValue 
        { 
            get 
            {
                if (YAxisValues is null || YAxisValues.Length.Equals(0))
                    return 0;
                return YAxisValues.Min();
            } 
        }
        /// <summary>
        /// 
        /// </summary>
        public double YAxisMaxValue
        {
            get
            {
                if (YAxisValues is null || YAxisValues.Length.Equals(0))
                    return 0;
                return YAxisValues.Max();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public YAxisScalingModeEnum YAxisScalingMode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double YAxisScaleMinValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double YAxisScaleMaxValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ClampingModeEnum ClampingMode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? YAxisMinClamp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? YAxisMaxClamp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Color SignalColor { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool BorderVisible { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int BorderSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Color? BorderColor { get; set; }    
        /// <summary>
        /// 
        /// </summary>
        public bool Antialiasing { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool ShowSignalBoundingLines { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Color? MinBoundingLineColor { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Color? MaxBoundingLineColor { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool UseScientificNotationOnYAxisMeasure { get; set; }
    }
}
