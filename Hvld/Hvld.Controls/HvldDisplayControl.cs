using Hvld.Parser;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace Hvld.Controls
{
    /// <summary>
    /// Scrollable HVLD 2.0 display control.
    /// ---------------------------------------
    /// BUGFix: the parameterless class constructor of the ZedGraph control is too slow.
    /// So creating a new OptrelSignal (which derives from ZedGraph) took almost 500ms.
    /// Created an OptrelSignal pool of size SIGNALS_POOL_SIZE, initialized with constructed OptrelSignal objects.
    /// </summary>
    public partial class HvldDisplayControl : UserControl
    {
        /// <summary>
        /// Size of the OptrelSignal pool (used as a workaround of the slow creation time of ZedGraph).
        /// </summary>
        private int SIGNALS_POOL_SIZE = 15;
        /// <summary>
        /// Right button pane button sizes.
        /// </summary>
        private readonly int[] ROLLING_WINDOW_BUTTON_SIZES = new int[] { 20, 25, 40 };
        /// <summary>
        /// Object containing a group of properties to tweak the appareance of the control.
        /// </summary>
        private HvldControlUpdateData _hvldControlUpdateData = new HvldControlUpdateData();
        /// <summary>
        /// Enables the antialiasing effect on all signals.
        /// </summary>
        private bool _rollingWindowBarVisible = true;
        /// <summary>
        /// Enables the antialiasing effect on all signals.
        /// </summary>
        private bool _enableGlobalAntialiasing;
        /// <summary>
        /// Creates a pool of preloaded signals.
        /// </summary>
        private Stack<OptrelSignal> _signalsPool;
        /// <summary>
        /// This has been moved here to avoid its creation everytime.
        /// Works good as long as ALL the properties are overwritten each time.
        /// </summary>
        private readonly OptrelSignalParameters _osp = new OptrelSignalParameters();
        /// <summary>
        /// Structure to store the already created and configured OptrelSignals.
        /// </summary>
        private readonly ConcurrentDictionary<byte, OptrelSignal> _signals = new ConcurrentDictionary<byte, OptrelSignal>();
        /// <summary>
        /// 
        /// </summary>
        private readonly ConcurrentDictionary<byte, OptrelSignal> _renderedSignals = new ConcurrentDictionary<byte, OptrelSignal>();
        /// <summary>
        /// Enables the antialiasing feature on all signals.
        /// </summary>
        [Description("Applies the antialiasing feature to every signal within the control"), Category("Optrel")]
        public bool EnableGlobalAntialising
        {
            get { return _enableGlobalAntialiasing; }
            set
            {
                _enableGlobalAntialiasing = value;
                ChangeGlobalAntialiasing(_enableGlobalAntialiasing);
            }
        }
        /// <summary>
        /// Enables/disables the antialiasing fature for the signal.
        /// </summary>
        protected void ChangeGlobalAntialiasing(bool enabled)
        {
            if (_displayedSignal is null)
                return;
            _displayedSignal.IsAntiAlias = enabled;
            // An Invalidate is mandatory to apply the antialiasing effect.
            _displayedSignal.Invalidate();
        }
        /// <summary>
        /// 
        /// </summary>
        private HvldFrame _currentFrame;
        /// <summary>
        /// 
        /// </summary>
        public HvldFrame CurrentFrame
        {
            get
            {
                return _currentFrame;
            }
        }
        /// <summary>
        /// Currently displayed signal.
        /// </summary>
        private OptrelSignal _displayedSignal;
        /// <summary>
        /// Currently displayed signal.
        /// </summary>
        public OptrelSignal DisplayedSignal
        {
            get
            {
                return _displayedSignal;
            }

            private set
            {
                _displayedSignal = value;

                if (DisplayedSignalChanged is null)
                    return;

                Task.Run(() => {
                    DisplayedSignalChanged.Invoke(this, new DisplayedSignalChangedEventArgs(_currentFrame, _displayedSignal));
                });
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public delegate void DisplayedSignalChangedEventHandler(object source, DisplayedSignalChangedEventArgs e);
        /// <summary>
        /// 
        /// </summary>
        public event DisplayedSignalChangedEventHandler DisplayedSignalChanged;
        /// <summary>
        /// Class constructor.
        /// </summary>
        public HvldDisplayControl()
        {
            // Boilerplate Winforms code.
            InitializeComponent();
            // Creates a new signal pool to speed up the control.
            _signalsPool = new Stack<OptrelSignal>(SIGNALS_POOL_SIZE);
            // Initializes the signal pool.
            for (var i = 0; i < SIGNALS_POOL_SIZE; i++)
                _signalsPool.Push(new OptrelSignal());
            // Sets the size of the rolling window buttons at the maximum value.
            var startBtnSize = ROLLING_WINDOW_BUTTON_SIZES[ROLLING_WINDOW_BUTTON_SIZES.Length - 1];
            BtnNext.Size = new Size(startBtnSize, startBtnSize);
            BtnPrev.Size = new Size(startBtnSize, startBtnSize);
            // Subscription to the SizeChanged event to manage the size of the side buttons.
            SizeChanged += OptrelSignalStacker_SizeChanged;
            // Hides the side vertical bar with the buttons.
            ShowRollingWindowBar(false);
        }
        /// <summary>
        /// Sets the DoubleBuffered property to true, even if private.
        /// </summary>
        private void SetDoubleBuffered()
        {
            if (SystemInformation.TerminalServerSession)
                return;
            System.Reflection.PropertyInfo aProp = typeof(Control).GetProperty(
                "DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);
            aProp.SetValue(this, true, null);
        }
        /// <summary>
        /// Turns on the WS_EX_COMPOSITED option to avoid flickering.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // Turn on WS_EX_COMPOSITED.
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
        /// <summary>
        /// Handles the size changed event of the control.
        /// </summary>
        private void OptrelSignalStacker_SizeChanged(object sender, EventArgs e)
        {
            // Then resizes the next/prev buttons.
            ResizeScrollingWindowButtons();
        }
        /// <summary>
        /// Show/hide the right-side scroll bar.
        /// </summary>
        private void ShowRollingWindowBar(bool show)
        {
            if (show && !_rollingWindowBarVisible)
            {
                MainTablePanel.ColumnStyles[0].SizeType = SizeType.Percent;
                MainTablePanel.ColumnStyles[0].Width = 100f;
                MainTablePanel.ColumnStyles[1].SizeType = SizeType.AutoSize;
                MainTablePanel.ColumnStyles[1].Width = 1;
                BtnNext.Visible = true;
                BtnPrev.Visible = true;
                _rollingWindowBarVisible = true;
                return;
            }
            else if (!show && _rollingWindowBarVisible)
            {
                MainTablePanel.ColumnStyles[0].SizeType = SizeType.Percent;
                MainTablePanel.ColumnStyles[0].Width = 100f;
                BtnNext.Visible = false;
                BtnPrev.Visible = false;
                MainTablePanel.ColumnStyles[1].SizeType = SizeType.Absolute;
                MainTablePanel.ColumnStyles[1].Width = 0f;
                _rollingWindowBarVisible = false;
            }
        }
        /// <summary>
        /// Resizes the scrolling window buttons to an acceptale ratio for visualization.
        /// </summary>
        private void ResizeScrollingWindowButtons()
        {
            var ratioA = MainPanel.Size.Width / ROLLING_WINDOW_BUTTON_SIZES[2];
            var ratioB = MainPanel.Size.Height / ROLLING_WINDOW_BUTTON_SIZES[1];

            int btnSize;

            if (ratioA < 3 || ratioB < 3)
                btnSize = ROLLING_WINDOW_BUTTON_SIZES[0];
            else if (ratioA < 10 || ratioB < 10)
                btnSize = ROLLING_WINDOW_BUTTON_SIZES[1];
            else
                btnSize = ROLLING_WINDOW_BUTTON_SIZES[2];
            //var newSize = (int)(btnCurrentSize * 0.9);
            BtnNext.Size = new Size(btnSize, btnSize);
            BtnPrev.Size = new Size(btnSize, btnSize);
        }
        /// <summary>
        /// Displays the signal with ID signalId.
        /// </summary>
        public void ShowSignal(byte signalId)
        {
            // Extracts only the currently shown signal.
            var signal = ExtractSignalFromHvld2Frame(
                signalId,
                _currentFrame,
                _hvldControlUpdateData);

            if (signal is null)
                return;

            try
            {
                if (!_renderedSignals.ContainsKey(signalId))
                {
                    foreach (var ds in _renderedSignals.Values)
                        ds.Visible = false;
                    signal.Visible = true;
                    MainPanel.Controls.Add(signal);
                    _renderedSignals.TryAdd(signalId, signal);
                }
                else
                {
                    foreach (var ds in _renderedSignals.Values)
                        ds.Visible = false;
                    _signals[signalId].Visible = true;
                }
                // Sets the antialiasing if globally enabled.
                signal.IsAntiAlias = _enableGlobalAntialiasing;
                // Shows the scrolling window bar if necessary.
                ShowRollingWindowBar(true);
                // Redraws the control.
                signal.Redraw();
            }
            finally
            {
                // Updates the current shown control, raises an event to all the subscribers.
                DisplayedSignal = signal;
            }
        }
        /// <summary>
        /// Creates the OptrelSignal objects from the HvldFrame and adds them to the signal stacker control.
        /// </summary>
        public void UpdateControlFromHvldFrame(
           HvldFrame frame,
           HvldControlUpdateData controlUpdateData)
        {
            if (frame is null) 
                return;
            // Saves the last received frame.
            _currentFrame = frame;
            // Updates the controls properties.
            _hvldControlUpdateData = controlUpdateData;
            // Updates the UI for the selected signal.
            if (_displayedSignal is null)
                ShowSignal(0);
            else
                ShowSignal(_displayedSignal.Id);
        }
        /// <summary>
        /// Returns a bitmap image of the current visualization of the control.
        /// </summary>
        public Bitmap GetImage()
        {
            if (!IsHandleCreated || (Width + Height == 0)) 
                return null;

            if (InvokeRequired)
            {
                return (Bitmap)MainPanel.Invoke(new Func<Bitmap>(() => GetImage()));
            }
            else
            {
                var b = new Bitmap(MainPanel.Width, MainPanel.Height);
                DrawToBitmap(b, new Rectangle(0, 0, b.Width, b.Height));
                return b;            
            }
        }
        /// <summary>
        /// Clears the control of all signals.
        /// </summary>
        public void Clear()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() =>
                {
                    Clear();
                }));
            }
            else
            {
                MainPanel.Controls.Clear();
                // Hides the side vertical bar with the buttons.
                ShowRollingWindowBar(false);
                // Invalidates the control to redraw.
                Invalidate();
            }
        }
        /// <summary>
        /// Handles a click on the PREV button.
        /// </summary>
        private void BtnPrev_Click(object sender, EventArgs e)
        {
            if (_displayedSignal is null)
                return;

            if (_displayedSignal.Id == 0)
                ShowSignal((byte)(_currentFrame.NumberOfSignals - 1));
            else
                ShowSignal((byte)(_displayedSignal.Id - 1));
        }
        /// <summary>
        /// Handles a click on the NEXT button.
        /// </summary>
        private void BtnNext_Click(object sender, EventArgs e)
        {
            if (_displayedSignal is null)
                return;

            if (_displayedSignal.Id == _currentFrame.NumberOfSignals - 1)
                ShowSignal(0);
            else
                ShowSignal((byte)(_displayedSignal.Id + 1));
        }
        /// <summary>
        /// This function is specific to the HVLD 2.0 implementations for ExactaEasy.
        /// Adds an OptrelSignal created from an HvldFrame object, with a defined border 
        /// color that is shown only when the signal is given by a failed test. 
        /// All the necessary parameters are inferred from the HvldFrame object or are constant values.
        /// Returns the created OptrelSignal.
        /// </summary>
        private OptrelSignal ExtractSignalFromHvld2Frame(
            byte signalKey,
            HvldFrame hvld,
            HvldControlUpdateData hvldControlUpdateData)
        {
            if (hvld is null)
                return null;
            if (hvld.Payload is null)
                return null;
            if (hvldControlUpdateData is null)
                return null;
            // Gets the SignalInfo of the signal with ID signalKey.
            if (!hvld.Payload.GetSignal(signalKey, out SignalInfo s))
                return null;
            // Those are the default values for each HVLD 2.0 signal.
            _osp.SignalId = s.GretelId;
            _osp.SignalName = s.Title;
            _osp.XAxisTitle = hvldControlUpdateData.XAxisTitle;
            _osp.YAxisTitle = hvldControlUpdateData.YAxisTitle;
            _osp.Antialiasing = false;
            _osp.BorderVisible = hvldControlUpdateData.BorderSize > 0 ? true : false;
            _osp.BorderSize = hvldControlUpdateData.BorderSize;
            _osp.ShowSignalBoundingLines = hvldControlUpdateData.ShowSignalBoundingLines;
            _osp.MaxBoundingLineColor = Color.Gray;
            _osp.MinBoundingLineColor = Color.Gray;
            _osp.UseScientificNotationOnYAxisMeasure = hvldControlUpdateData.ScientificNotationOnYAxis;
            _osp.YAxisMaxClamp = s.YAxisMaxClamp;
            _osp.YAxisMinClamp = s.YAxisMinClamp;
            _osp.XAxisValues = s.Points.Select(p => p.X).ToArray();
            _osp.YAxisValues = s.Points.Select(p => p.Y).ToArray();
            _osp.SignalColor = hvldControlUpdateData.SignalColorOverride ?? s.SignalColor;
            // Defines the border color.
            switch (s.Classification)
            {
                case ClassificationEnum.InSpecs:
                    _osp.BorderColor = hvldControlUpdateData.InSpecBorderColor;
                    break;

                case ClassificationEnum.OutSpecs:
                    _osp.BorderColor = hvldControlUpdateData.OutSpecBorderColor;
                    break;
            }
            // Defines the y-axis scale of the signal.
            switch (signalKey)
            {
                case 0:
                case 1:
                case 2:
                    // HVLD signals with a fixed scale of [-32768, 32767].
                    _osp.YAxisScalingMode = YAxisScalingModeEnum.Fixed;
                    _osp.YAxisScaleMinValue = short.MinValue;
                    _osp.YAxisScaleMaxValue = short.MaxValue;
                    break;

                case 3:
                case 4:
                    // HVLD signals with a fixed scale of [0, 65535].
                    _osp.YAxisScalingMode = YAxisScalingModeEnum.Fixed;
                    _osp.YAxisScaleMinValue = ushort.MinValue;
                    _osp.YAxisScaleMaxValue = ushort.MaxValue;
                    break;

                default:
                    // HVLD signals with automatic scaling.
                    _osp.YAxisScalingMode = YAxisScalingModeEnum.Auto;
                    break;
            }
            // Normalization of the clamping boundaries values coming from GRETEL.
            if (_osp.YAxisMinClamp <= int.MinValue || _osp.YAxisMinClamp.Value == 0)
                _osp.YAxisMinClamp = null;

            if (_osp.YAxisMaxClamp.Value >= int.MaxValue || _osp.YAxisMaxClamp.Value == 0)
                _osp.YAxisMaxClamp = null;
            // Gets a signal from the pool or from the dictionary.
            var os = CreateOptrelSignalFromParameters(_osp);
            // Checks if clamping is required, if so, clamps the signal immediately.
            if (_osp.YAxisMinClamp.HasValue && _osp.YAxisMaxClamp.HasValue)
            {
                os.ClampLowerBound = _osp.YAxisMinClamp.Value;
                os.ClampUpperBound = _osp.YAxisMaxClamp.Value;
                os.ApplyClamp(
                    ClampingModeEnum.Simple,
                    ClampingSide.Both);
            }
            else if (_osp.YAxisMinClamp.HasValue)
            {
                os.ClampLowerBound = _osp.YAxisMinClamp.Value;
                os.ClampUpperBound = null;
                os.ApplyClamp(
                    ClampingModeEnum.Simple,
                    ClampingSide.Lower);
            }
            else if (_osp.YAxisMaxClamp.HasValue)
            {
                os.ClampLowerBound = null;
                os.ClampUpperBound = _osp.YAxisMaxClamp.Value;
                os.ApplyClamp(
                    ClampingModeEnum.Simple,
                    ClampingSide.Upper);
            }
            return os;
        }
        /// <summary>
        /// Creates (either gets from the pool or from the dictionary) an OptrelSignal.
        /// </summary>
        private OptrelSignal CreateOptrelSignalFromParameters(OptrelSignalParameters osp)
        {
            OptrelSignal os;
            LineItem curve;
            // Check if the signal has already been added. 
            if (_signals.ContainsKey(osp.SignalId))
                os = _signals[osp.SignalId];
            else
            {
                // Get the OptrelSignal from the pool
                os = _signalsPool.Pop();
                _signals.TryAdd(osp.SignalId, os);
            }
            // Initializes the signal with the custom Optrel configuration.
            os.InitializeSignal(osp.SignalName, osp.SignalId, osp);
           
            if (os.GraphPane.CurveList.Count > 0)
            {
                curve = os.GraphPane.CurveList[0] as LineItem;
                var points = curve.Points as IPointListEdit;
                points.Clear();
                for (var i = 0; i < osp.XAxisValues.Length; i++)
                    points.Add(osp.XAxisValues[i], osp.YAxisValues[i]);
                curve.Color = osp.SignalColor;
            }
            else
                curve = os.GraphPane.AddCurve(osp.SignalName, osp.XAxisValues, osp.YAxisValues, osp.SignalColor);

            curve.Symbol.Type = SymbolType.None;
            // Sets the X max/min scale.
            os.GraphPane.XAxis.Scale.MinAuto = true;
            os.GraphPane.XAxis.Scale.MaxAuto = false;
            os.GraphPane.XAxis.Scale.Max = osp.XAxisValues.Length;
            // Sets the scientific notation to visualize wide values on the Y-axis.
            os.GraphPane.YAxis.Scale.MagAuto = os.UseScientificNotationOnYAxis;
            os.GraphPane.XAxis.Scale.MagAuto = false;
            // Sets the Y max/min scale as non automatic: we calculate the max and min and set those values,
            // so the graph is perfectly fit in the draw area.
            os.GraphPane.YAxis.Scale.MinAuto = false;
            os.GraphPane.YAxis.Scale.MaxAuto = false;
            // Checks the y-axis scaling mode and sets the bounds accordingly.
            switch (osp.YAxisScalingMode)
            {
                case YAxisScalingModeEnum.Auto:
                    // Sets the min/max bounds by calculating the effetctive max/min.
                    if (!(osp.YAxisValues is null) && osp.YAxisValues.Length > 0)
                    {
                        var min = osp.YAxisValues.Min();
                        var max = osp.YAxisValues.Max();
                        // Bugfix: if osp.YAxisMinValue = osp.YAxisMaxValue (that means, all the points have the same Y value),
                        // the graph cannot be displayed. 
                        if (min.Equals(max))
                        {
                            // No need to show the bounding lines in this case.
                            os.ShowSignalBoundingLines = false;
                            os.GraphPane.YAxis.Scale.Min = min - 1.0 - min * 0.5;
                            os.GraphPane.YAxis.Scale.Max = max + 1.0 + max * 0.5;
                        }
                        else
                        {
                            os.GraphPane.YAxis.Scale.Min = min;
                            os.GraphPane.YAxis.Scale.Max = max;
                        }
                    }
                    break;

                case YAxisScalingModeEnum.Fixed:

                    if (osp.YAxisScaleMinValue.Equals(osp.YAxisScaleMaxValue))
                    {
                        var min = osp.YAxisValues.Min();
                        var max = osp.YAxisValues.Max();
                        os.GraphPane.YAxis.Scale.Min = min;
                        os.GraphPane.YAxis.Scale.Max = max;
                    }
                    else
                    {
                        // Use fixed values for the max/min.
                        os.GraphPane.YAxis.Scale.Min = osp.YAxisScaleMinValue;
                        os.GraphPane.YAxis.Scale.Max = osp.YAxisScaleMaxValue;
                    }
                    break;
            }
            // Check if there are points or the signal has been filtered out.
            if (osp.XAxisValues is null || osp.XAxisValues.Equals(0))
            {
                // No need to show the bounding lines in this case.
                // Nothing to display.
                os.ShowSignalBoundingLines = false;
            }
            return os;
        }
    }
}
