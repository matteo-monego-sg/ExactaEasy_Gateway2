using ExactaEasyEng.Utilities;
using Hvld.Parser;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Hvld.Controls
{
    /// <summary>
    /// Scrollable HVLD 2.0 signals visualizer.
    /// </summary>
    public partial class HvldSingleDisplay : HvldBaseDisplay
    { 
        /// <summary>
        /// Right button pane button sizes.
        /// </summary>
        private readonly int[] ROLLING_WINDOW_BUTTON_SIZES = new int[] { 20, 25, 40 };
        /// <summary>
        /// Class constructor.
        /// </summary>
        public HvldSingleDisplay()
        {
            // Boilerplate Winforms code.
            InitializeComponent();
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
            if (show && _loadedSignals.Count > 0)
            {
                MainTablePanel.ColumnStyles[0].SizeType = SizeType.Percent;
                MainTablePanel.ColumnStyles[0].Width = 100f;
                MainTablePanel.ColumnStyles[1].SizeType = SizeType.AutoSize;
                MainTablePanel.ColumnStyles[1].Width = 1;
                BtnNext.Visible = true;
                BtnPrev.Visible = true;
                return;
            }
            MainTablePanel.ColumnStyles[0].SizeType = SizeType.Percent;
            MainTablePanel.ColumnStyles[0].Width = 100f;
            BtnNext.Visible = false;
            BtnPrev.Visible = false;
            MainTablePanel.ColumnStyles[1].SizeType = SizeType.Absolute;
            MainTablePanel.ColumnStyles[1].Width = 0f;
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
        public override void ShowSignal(int signalId)
        {
            SuspendLayout();
            try
            {
                if (!_loadedSignals.ContainsKey(signalId))
                {
                    // LOG: signal not loaded!
                    return;
                }
                // If there is a signal already displayed, clear the control.
                if (_displayedSignalKeyId.Id > -1)
                    MainPanel.Controls.Clear();

                var controlToShow = _loadedSignals[signalId].SignalControl;
                // Sets the antialiasing if globally enabled.
                controlToShow.IsAntiAlias = _enableGlobalAntialiasing;
                // Adds the control.
                MainPanel.Controls.Add(controlToShow);
                // Updates the ZedGraphControl.
                controlToShow.Redraw();
                // Saves the currently displayed control key.
                _displayedSignalKeyId = new HvldSignalKeyId() { Id = controlToShow.Id, Key = controlToShow.Name };
            }
            finally
            {
                ResumeLayout();
            }
        }
        /// <summary>
        /// Creates the OptrelSignal objects from the HvldFrame and adds them to the signal stacker control.
        /// </summary>
        public override void UpdateControlFromHvldFrame(
            HvldFrame frame,
            string xTitle,
            string yTitle,
            bool showBoundingLines,
            bool useScientificNotationOnYAxis,
            int borderSize,
            Color inSpecBorderColor,
            Color outSpecBorderColor,
            Color? curveColorOverride = null)
        {
            if (frame is null) return;
            // Note: checking InvokeRequired for thread safety.
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() =>
                {
                    UpdateControlFromHvldFrame(
                        frame,
                        xTitle,
                        yTitle,
                        showBoundingLines,
                        useScientificNotationOnYAxis,
                        borderSize,
                        inSpecBorderColor,
                        outSpecBorderColor,
                        curveColorOverride);
                }));
            }
            else
            {
                // Gets an enumerator of OptrelSignal controls.
                var signals = HvldUtilities.ExtractSignalsFromHvld2Frame(
                    frame,
                    xTitle,
                    yTitle,
                    showBoundingLines,
                    useScientificNotationOnYAxis,
                    borderSize,
                    inSpecBorderColor,
                    outSpecBorderColor,
                    curveColorOverride);
                // Updates all the signals of the control.
                UpdateSignals(signals);

                if(signals.Count() > 0)
                    ShowRollingWindowBar(true);

                if (_displayedSignalKeyId.Id < 0)
                    ShowSignal(0);
                else
                    ShowSignal(_displayedSignalKeyId.Id);
            }
        }
        /// <summary>
        /// Returns a bitmap image of the current visualization of the control.
        /// </summary>
        public override Bitmap GetImage()
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return null;

            if (!IsHandleCreated || (Width + Height == 0)) return null;

            return (Bitmap)MainPanel.Invoke(new Func<Bitmap>(() =>
            {
                Bitmap b = new Bitmap(MainPanel.Width, MainPanel.Height);
                DrawToBitmap(b, new Rectangle(0, 0, b.Width, b.Height));
                return b;
            }));
        }
        /// <summary>
        /// Clears the control of all signals.
        /// </summary>
        public override void Clear()
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
                SuspendLayout();
                try
                {

                    MainPanel.Controls.Clear();
                    // Hides the side vertical bar with the buttons.
                    ShowRollingWindowBar(false);
                    // Clears the displayed signals lookup table.
                    _loadedSignals.Clear();
                    // Invalidates the control to redraw.
                    Invalidate();
                }
                finally
                {
                    ResumeLayout();
                }
            }
        }
        /// <summary>
        /// Handles a click on the PREV button.
        /// </summary>
        private void BtnPrev_Click(object sender, EventArgs e)
        {
            // The prev button works only in ScrollableSingle mode.
            //if (_stackingType != StackingTypeEnum.ScrollableSingle)
            //    return;
            SuspendLayout();
            try
            {
                if (_displayedSignalKeyId.Id < 0)
                {
                    // No signal is shown.
                    if (_loadedSignals.Count > 0)
                    {
                        var minSignalId = _loadedSignals.Values.Min(x => x.SignalId);
                        ShowSignal(minSignalId);
                        return;
                    }

                    // LOG: no signals to whoe.
                    return;
                }
                else
                {
                    // A signal is shown.
                    if (_displayedSignalKeyId.Id - 1 < 0)
                    {
                        var maxSignalId = _loadedSignals.Values.Max(x => x.SignalId);
                        ShowSignal(maxSignalId);
                        return;
                    }

                    ShowSignal(_displayedSignalKeyId.Id - 1);
                }
            }
            finally
            {
                ResumeLayout();
            }
        }
        /// <summary>
        /// Handles a click on the NEXT button.
        /// </summary>
        private void BtnNext_Click(object sender, EventArgs e)
        {
            SuspendLayout();
            try
            {

                if (_displayedSignalKeyId.Id < 0)
                {
                    // No signal is shown.
                    if (_loadedSignals.Count > 0)
                    {
                        var minSignalId = _loadedSignals.Values.Min(x => x.SignalId);
                        ShowSignal(minSignalId);
                        return;
                    }
                    // LOG: no signals to show.
                    return;
                }
                else
                {
                    var maxSignalId = _loadedSignals.Values.Max(x => x.SignalId);
                    // A signal is shown.
                    if (_displayedSignalKeyId.Id + 1 > maxSignalId)
                    {
                        var minSignalId = _loadedSignals.Values.Min(x => x.SignalId);
                        ShowSignal(minSignalId);
                        return;
                    }
                    ShowSignal(_displayedSignalKeyId.Id + 1);
                }
            }
            finally
            {
                ResumeLayout();
            }
        }
    }
}
