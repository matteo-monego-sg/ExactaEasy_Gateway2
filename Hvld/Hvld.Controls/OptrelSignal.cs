using Hvld.Parser;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ZedGraph;

namespace Hvld.Controls
{
    /// <summary>
    /// Extends the standard ZedGraphControl by adding the border properties.
    /// NOTE: the base control (ZedGraphControl) constructor is quite slow.
    /// </summary>
    public partial class OptrelSignal : ZedGraphControl
    {
        /// <summary>
        /// 
        /// </summary>
        private LineObj _minBoundingLine;
        /// <summary>
        /// 
        /// </summary>
        private LineObj _maxBoundingLine;
        /// <summary>
        /// 
        /// </summary>
        private bool _initialized;
        /// <summary>
        /// 
        /// </summary>
        public OptrelSignal() : base() 
        {
            ZoomEvent += OptrelSignal_ZoomEvent;
            MouseMove += OptrelSignal_MouseMove;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="initialParams"></param>
        public void InitializeSignal(string name, byte id, OptrelSignalParameters initialParams = null)
        {
            Name = name;
            Id = id;

            if (!(initialParams is null))
            {
                _xAxisTitle = initialParams.XAxisTitle;
                _yAxisTitle = initialParams.YAxisTitle;
                _borderVisible = initialParams.BorderVisible;
                _borderColor = initialParams.BorderColor.HasValue ? initialParams.BorderColor.Value : Color.Transparent;
                _borderSize = initialParams.BorderSize;
                _showSignalBoundingLines = initialParams.ShowSignalBoundingLines;
                _minBoundingLineColor = initialParams.MinBoundingLineColor.HasValue ? initialParams.MinBoundingLineColor.Value : Color.Transparent;
                _maxBoundingLineColor = initialParams.MaxBoundingLineColor.HasValue ? initialParams.MaxBoundingLineColor.Value : Color.Transparent;
                _useScientificNotationOnYAxis = initialParams.UseScientificNotationOnYAxisMeasure;
            }

            ApplyCustomOptrelGraphics();

            _initialized = true;
        }
        /// <summary>
        /// Apply the custom OPTREL graphic tweaks to the underlying ZedGraph control.
        /// </summary>
        private void ApplyCustomOptrelGraphics()
        {
            Dock = DockStyle.Fill;
            Margin = new Padding(0);
            // general
            GraphPane.Title.IsVisible = false;
            GraphPane.Fill = new Fill(Color.FromArgb(55, 55, 56));
            GraphPane.Chart.Fill = new Fill(Color.FromArgb(31, 31, 32));
            GraphPane.IsBoundedRanges = false;
            IsAutoScrollRange = true;
            GraphPane.Legend.Fill = new Fill(Color.FromArgb(31, 31, 32));
            GraphPane.Legend.FontSpec.FontColor = Color.White;
            GraphPane.Legend.FontSpec.StringAlignment = StringAlignment.Center;

            GraphPane.Legend.Location.AlignH = AlignH.Left;
            GraphPane.Legend.Location.AlignV = AlignV.Top;
            GraphPane.Legend.IsVisible = true;
            //Y Axis
            GraphPane.YAxis.Title.Text = YAxisTitle;
            GraphPane.YAxis.Title.FontSpec.Size = 9f;
            GraphPane.YAxis.Title.FontSpec.FontColor = Color.White;
            GraphPane.YAxis.Title.FontSpec.IsBold = true;
            GraphPane.YAxis.Scale.FontSpec.Size = 8f;
            GraphPane.YAxis.Scale.FontSpec.FontColor = Color.White;
            GraphPane.YAxis.Scale.FormatAuto = true;
            GraphPane.YAxis.Scale.MaxAuto = false;
            GraphPane.YAxis.Scale.MinAuto = false;
            GraphPane.YAxis.MajorGrid.IsVisible = true;
            GraphPane.YAxis.MajorGrid.DashOff = 0.5f;
            GraphPane.YAxis.MajorGrid.Color = Color.FromArgb(55, 55, 56);
            //X Axis
            GraphPane.XAxis.Title.Text = XAxisTitle;
            GraphPane.XAxis.Title.FontSpec.Size = 9f;
            GraphPane.XAxis.Title.FontSpec.FontColor = Color.White;
            GraphPane.XAxis.Title.FontSpec.IsBold = true;
            GraphPane.XAxis.Scale.FontSpec.Size = 8f;
            GraphPane.XAxis.Scale.FontSpec.FontColor = Color.White;
            GraphPane.XAxis.Scale.FormatAuto = true;
            GraphPane.XAxis.Scale.MaxAuto = true;
            GraphPane.XAxis.Scale.MinAuto = true;
            //ZedGraph.GraphPane.XAxis.ScaleFormatEvent += XAxis_ScaleFormatEvent;
            GraphPane.XAxis.MajorGrid.IsVisible = true;
            GraphPane.XAxis.MajorGrid.DashOff = 0.5f;
            GraphPane.XAxis.MajorGrid.Color = Color.FromArgb(55, 55, 56);
            //Sets the scientific notation to visualize wide values on the Y-axis.
            GraphPane.YAxis.Scale.MagAuto = false;
            GraphPane.XAxis.Scale.MagAuto = false;
            // Sets up a border if requested.
            if (BorderVisible)
            {
                GraphPane.Border.Style = System.Drawing.Drawing2D.DashStyle.Solid;
                GraphPane.Border = new Border(BorderColor.Value, BorderSize);
                GraphPane.Border.IsVisible = true;
            }
            else
            {
                if (!(GraphPane.Border is null))
                    GraphPane.Border.IsVisible = false;
            }
        }
        /// <summary>
        /// Handles the mouse move event.
        /// Used to render the Y-Axis bounding lines, if required.
        /// </summary>
        private void OptrelSignal_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Middle && _initialized)
                UpdateBoundingLines();
        }
        /// <summary>
        /// Handles a zoom event.
        /// Used to render the Y-Axis bounding lines, if required.
        /// </summary>
        private void OptrelSignal_ZoomEvent(ZedGraphControl sender, ZoomState oldState, ZoomState newState)
        {
            if(_initialized)
                UpdateBoundingLines();
        }
        /// <summary>
        /// 
        /// </summary>
        private string _xAxisTitle;
        /// <summary>
        /// X axis title.
        /// </summary>
        [Description("X-Axis title"), Category("Optrel")]
        public string XAxisTitle
        {
            get { return _xAxisTitle; }
            set
            {
                _xAxisTitle = value;
                ChangeXAxisTitle(_xAxisTitle);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private string _yAxisTitle;
        /// <summary>
        /// Y axis title. 
        /// </summary>
        [Description("Y-Axis title"), Category("Optrel")]
        public string YAxisTitle
        {
            get { return _yAxisTitle; }
            set
            {
                _yAxisTitle = value;
                ChangeYAxisTitle(_yAxisTitle);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private bool _useScientificNotationOnYAxis;
        /// <summary>
        /// Y axis title. 
        /// </summary>
        [Description("Use scientific notation for the measure unit of the Y-Axis"), Category("Optrel")]
        public bool UseScientificNotationOnYAxis
        {
            get { return _useScientificNotationOnYAxis; }
            set
            {
                _useScientificNotationOnYAxis = value;
                ChangeScientificNotationOnYAxis(_useScientificNotationOnYAxis);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private int _borderSize;
        /// <summary>
        /// Border color of the control.
        /// </summary>
        [Description("Border size, if visible"), Category("Optrel")]
        public int BorderSize
        {
            get { return _borderSize; }
            set
            {
                _borderSize = value;
                ChangeBorderSize(_borderSize);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private Color? _borderColor;
        /// <summary>
        /// Border color of the control.
        /// </summary>
        [Description("Border color, if visible"), Category("Optrel")]
        public Color? BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                ChangeBorderColor(_borderColor.Value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private bool _borderVisible;
        /// <summary>
        /// Indicates if the control should show a border. 
        /// </summary>
        [Description("Border visibility"), Category("Optrel")]
        public bool BorderVisible 
        {
            get { return _borderVisible; }
            set
            {
                _borderVisible = value;
                ChangeBorderVisibility(_borderVisible);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private bool _showSignalBoundingLines;
        /// <summary>
        /// 
        /// </summary>
        [Description("Enables the Y-Axis min/max bounding dashed lines"), Category("Optrel")]
        public bool ShowSignalBoundingLines
        {
            get { return _showSignalBoundingLines; }
            set
            {
                _showSignalBoundingLines = value;
                ChangeShowSignalBoundingLines();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private Color _minBoundingLineColor;
        /// <summary>
        /// 
        /// </summary>
        [Description("The color of the minimum Y-axis value bounding dashed line"), Category("Optrel")]
        public Color MinBoundingLineColor
        {
            get { return _minBoundingLineColor; }
            set
            {
                _minBoundingLineColor = value;
                ChangeMinBoundingLineColor(_minBoundingLineColor);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private Color _maxBoundingLineColor;
        /// <summary>
        /// 
        /// </summary>
        [Description("The color of the maximum Y-axis value bounding dashed line"), Category("Optrel")]
        public Color MaxBoundingLineColor
        {
            get { return _maxBoundingLineColor; }
            set
            {
                _maxBoundingLineColor = value;
                ChangeMaxBoundingLineColor(_maxBoundingLineColor);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("The lower bound of the clamped signal"), Category("Optrel")]
        public double? ClampLowerBound { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("The upper bound of the clamped signal"), Category("Optrel")]
        public double? ClampUpperBound { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Description("The signal ID"), Category("Optrel")]
        public byte Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public void ApplyClamp(ClampingModeEnum clampMode, ClampingSide side)
        {
            switch (side)
            {
                case ClampingSide.Upper:
                    if (!ClampUpperBound.HasValue) return;
                    break;

                case ClampingSide.Lower:
                    if (!ClampLowerBound.HasValue) return;
                    break;

                case ClampingSide.Both:
                    if (!ClampLowerBound.HasValue || !ClampUpperBound.HasValue) return;
                    break;
            }

            if (GraphPane.CurveList is null || GraphPane.CurveList.Count.Equals(0))
                return;
            if (GraphPane.CurveList[0].Points is null || GraphPane.CurveList[0].Points.Count.Equals(0))
                return;
            // Gets the points of the original signal.
            var pts = GraphPane.CurveList[0].Points as PointPairList;
            // If a clamped version of the signal already exists, removes it.
            if(GraphPane.CurveList.Count.Equals(2))
                GraphPane.CurveList.RemoveAt(1);

            switch (clampMode)
            {
                case ClampingModeEnum.Simple:
                    // Calculates the clamped datapoints.
                    var simpleClampedData = HvldSignalTransformer.ApplySimpleClamping(
                        pts, 
                        side,
                        ClampLowerBound.Value, 
                        ClampUpperBound.Value);
                    // Creates a clamped copy of the signal at index 0.
                    var simpleClampSignal = GraphPane.AddCurve(
                        string.Empty, 
                        simpleClampedData.Select(p => p.X).ToArray(), 
                        simpleClampedData.Select(p => p.Y).ToArray(),
                        GraphPane.CurveList[0].Color);
                    // Sets no waypoints.
                    simpleClampSignal.Symbol.Type = SymbolType.None;
                    break;

                case ClampingModeEnum.Accurate:
                    // Calculates the clamped datapoints.
                    var accurateClampedData = HvldSignalTransformer.ApplyAccurateClamping(
                        pts, 
                        side, 
                        ClampLowerBound.Value, 
                        ClampUpperBound.Value);
                    // Creates a clamped copy of the signal at index 0.
                    var accurateClampSignal = GraphPane.AddCurve(
                        string.Empty,
                        accurateClampedData.Select(p => p.X).ToArray(),
                        accurateClampedData.Select(p => p.Y).ToArray(),
                        GraphPane.CurveList[0].Color);
                    // Sets no waypoints.
                    accurateClampSignal.Symbol.Type = SymbolType.None;
                    break;

                default:
                    return;
            }
            // Hides the complete signal.
            GraphPane.CurveList[0].IsVisible = false;
            GraphPane.CurveList[1].IsVisible = true;
            // Updates the bounding lines.
            Redraw();
        }
        /// <summary>
        /// 
        /// </summary>
        public void RemoveClamp()
        {
            if (GraphPane.CurveList is null || GraphPane.CurveList.Count.Equals(0))
                return;
            // If there is a clamped signal, deletes it.
            if (GraphPane.CurveList.Count.Equals(2))
                GraphPane.CurveList.RemoveAt(1);
            // Hides the complete signal.
            GraphPane.CurveList[0].IsVisible = true;
            // Updates the bounding lines.
            UpdateBoundingLines();
        }
        /// <summary>
        /// 
        /// </summary>
        private void ChangeMaxBoundingLineColor(Color color)
        {
            if (!(_maxBoundingLine is null))
                _maxBoundingLine.Line.Color = color;
        }
        /// <summary>
        /// 
        /// </summary>
        private void ChangeMinBoundingLineColor(Color color)
        {
            if (!(_minBoundingLine is null))
                _minBoundingLine.Line.Color = color;
        }
        /// <summary>
        /// Removes the max/min bounding lines from the pane.
        /// </summary>
        private void ChangeShowSignalBoundingLines()
        {
            if (!_initialized)
                return;

            UpdateBoundingLines();
        }
        /// <summary>
        /// 
        /// </summary>
        private void UpdateBoundingLines()
        {
            // Check if there is at least one curve currently shown.
            if (GraphPane.CurveList.Count == 0) 
                return;
           
            // Removes the y-bound lines from the pane.
            GraphPane.GraphObjList.Clear();
            // Resets the references to the objects.
            _minBoundingLine = null;
            _maxBoundingLine = null;
            // Check if there's the need to draw the bounding lines.
            if (!_showSignalBoundingLines)
                return;
            // Gets the max value.
            var pts = GraphPane.CurveList[0].Points as PointPairList;

            if ((pts is null) || pts.Count.Equals(0))
                return;

            var yMax = pts.Max(p => p.Y);
            // Creates the maximum bounding line.
            _maxBoundingLine = new LineObj(
                _maxBoundingLineColor,
                GraphPane.XAxis.Scale.Min,
                yMax,
                GraphPane.XAxis.Scale.Max,
                yMax);
            _maxBoundingLine.Line.Style = System.Drawing.Drawing2D.DashStyle.DashDotDot;
            _maxBoundingLine.IsClippedToChartRect = true;

            var yMin = pts.Min(p => p.Y);
            // Creates the minimum bounding line.
            _minBoundingLine = new LineObj(
                _minBoundingLineColor,
                GraphPane.XAxis.Scale.Min,
                yMin,
                GraphPane.XAxis.Scale.Max,
                yMin);
            _minBoundingLine.Line.Style = System.Drawing.Drawing2D.DashStyle.DashDotDot;
            _minBoundingLine.IsClippedToChartRect = true;
            // Adds the graphic object to the pane.
            GraphPane.GraphObjList.Add(_minBoundingLine);
            // Adds the graphic object to the pane.
            GraphPane.GraphObjList.Add(_maxBoundingLine);
        }
        /// <summary>
        /// 
        /// </summary>
        private void ChangeBorderColor(Color color)
        {
            if (GraphPane is null || !_initialized) return;
            GraphPane.Border.Color = color;
        }
        /// <summary>
        /// 
        /// </summary>
        private void ChangeBorderSize(int size)
        {
            if (GraphPane is null || !_borderVisible || !_borderColor.HasValue || !_initialized) return;
            GraphPane.Border = new Border(_borderColor.Value, size);
        }
        /// <summary>
        /// 
        /// </summary>
        private void ChangeBorderVisibility(bool visible)
        {
            if (GraphPane is null || !_initialized) return;
            GraphPane.Border.IsVisible = visible;
        }
        /// <summary>
        /// 
        /// </summary>
        private void ChangeXAxisTitle(string title)
        {
            if (GraphPane is null || !_initialized) return;
            GraphPane.XAxis.Title.Text = title;
        }
        /// <summary>
        /// 
        /// </summary>
        private void ChangeYAxisTitle(string title)
        {
            if (GraphPane is null || !_initialized) return;
            GraphPane.YAxis.Title.Text = title;
        }
        /// <summary>
        /// 
        /// </summary>
        private void ChangeScientificNotationOnYAxis(bool scientificNotation)
        {
            if (GraphPane is null || !_initialized) return;
            GraphPane.YAxis.Scale.MagAuto = scientificNotation;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Redraw()
        {
            if (!_initialized)
                return;
            // Triggers an AxisChange to apply the changes.
            AxisChange();
            // Updates the graph bounding lines, if shown.
            UpdateBoundingLines();
            // This causes the control to be redrawn.
            Invalidate();
        }
    }
}
