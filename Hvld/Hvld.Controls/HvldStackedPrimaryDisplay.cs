using ExactaEasyEng.Utilities;
using Hvld.Parser;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Hvld.Controls
{
    /// <summary>
    /// HVLD 2.0 signals visualizer, the signals are stacked one above the other.
    /// Signals are selectable and the highlighted one is larger.
    /// </summary>
    public partial class HvldStackedPrimaryDisplay : HvldBaseDisplay
    {
        /// <summary>
        /// Highlighted signal ID.
        /// </summary>
        private byte _highlightedSignalId = 0;
        /// <summary>
        /// Highlighted signal height in percent of the control height.
        /// </summary>
        private float _highlightedSignalPercentHeight = 0f;
        /// <summary>
        /// Highlighted signal height in percent of the control height.
        /// </summary>
        public float HighlightedSignalHeightPercent
        {
            get { return _highlightedSignalPercentHeight; }
            set
            {
                _highlightedSignalPercentHeight = value;
                HighlightSignal(_highlightedSignalId, _highlightedSignalPercentHeight);
            }
        }
        /// <summary>
        /// Highlighted signal ID.
        /// </summary>
        public byte HighlightedSignalId
        {
            get { return _highlightedSignalId; }
            set
            {
                _highlightedSignalId = value;
                HighlightSignal(_highlightedSignalId);
            }
        }
        /// <summary>
        /// Class constructor.
        /// </summary>
        public HvldStackedPrimaryDisplay()
        {
            // Boilerplate Winforms code.
            InitializeComponent();
            // Clears all the TablePanel's RowStyles.
            TablePanel.RowStyles.Clear();
        }
        /// <summary>
        /// Highlights a signal.
        /// </summary>>
        private void HighlightSignal(byte signalId)
        {
            // Check if the requested signal is already highlighted.
            if (signalId.Equals(_highlightedSignalId))
                return;
            // Highlights the signal.
            HighlightSignal(signalId, _highlightedSignalPercentHeight);
            // Backups the highlighted signal ID.
            _highlightedSignalId = signalId;
        }
        /// <summary>
        /// Changes the highlight height of a signal.
        /// </summary>
        private void HighlightSignal(byte signalId, float heightPercent)
        {
            // Calculates the un-highlighted signals height in percent.
            var remainingPercent = (PERCENT_MAX_HEIGHT - heightPercent) / (_loadedSignals.Count() - 1);
            // Changes the heights accordingly.
            for (var i = 0; i < TablePanel.RowStyles.Count; i++)
            {
                if (i.Equals(signalId))
                    TablePanel.RowStyles[i].Height = heightPercent;
                else
                    TablePanel.RowStyles[i].Height = remainingPercent;
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
                // Adds all the signals to the control.
                UpdateSignals(signals);
            }
        }
        /// <summary>
        /// Updates/adds the signals to the control.
        /// </summary>
        protected override void UpdateSignals(IEnumerable<OptrelSignal> signals)
        {
            SuspendLayout();
            try
            {
                var heightPercent = PERCENT_MAX_HEIGHT;
                var remainingPercent = (PERCENT_MAX_HEIGHT - _highlightedSignalPercentHeight) / (signals.Count() - 1);

                foreach (var signal in signals)
                {
                    // Sets the antialiasing if globally enabled.
                    signal.IsAntiAlias = _enableGlobalAntialiasing;

                    if (signal.Id.Equals(_highlightedSignalId))
                        heightPercent = _highlightedSignalPercentHeight == 0f ? PERCENT_MAX_HEIGHT : _highlightedSignalPercentHeight;
                    else
                        heightPercent = remainingPercent;

                    if (_loadedSignals.ContainsKey(signal.Id))
                    {
                        // Gets the signal from the list of loaded signals.
                        var current = _loadedSignals[signal.Id].SignalControl;
                        // Gets the current table panel row.
                        var currentRow = TablePanel.GetRow(current);

                        if (currentRow < 0)
                        {
                            // Adds a new table row at the bottom.
                            var rowIndex = TablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, heightPercent));
                            // Adds a new optrel control at the bottom.
                            TablePanel.Controls.Add(signal, 0, rowIndex);
                        }
                        else
                        {
                            TablePanel.Controls.Remove(current);
                            TablePanel.Controls.Add(signal, 0, currentRow);
                        }
                    }
                    else
                    {
                        // Adds a new table row at the bottom.
                        var rowIndex = TablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, heightPercent));
                        // Adds a new optrel control at the bottom.
                        TablePanel.Controls.Add(signal, 0, rowIndex);
                    }
                    // Subscription to the click event.
                    signal.Click += Signal_Click;
                    // Triggers an AxisChange to apply the changes.
                    signal.AxisChange();
                    // Saves the added/updated signal.
                    if (_loadedSignals.ContainsKey(signal.Id))
                        _loadedSignals[signal.Id].SignalControl = signal;
                    else
                    {
                        var ssdd = new HvldSignalDisplayData(signal.Name, signal.Id, signal);
                        _loadedSignals.Add(signal.Id, ssdd);
                    }
                }
            }
            finally
            {
                ResumeLayout();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void Signal_Click(object sender, EventArgs e)
        {
            var os = sender as OptrelSignal;
            HighlightSignal(os.Id);
        }
        /// <summary>
        /// Does not apply for this control.
        /// </summary>
        /// <param name="signalId"></param>
        public override void ShowSignal(int signalId)
        {
            return;
        }
        /// <summary>
        /// Returns a bitmap image of the current visualization of the control.
        /// </summary>
        public override Bitmap GetImage()
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return null;

            if (!IsHandleCreated) return null;

            if (_loadedSignals.ContainsKey(_highlightedSignalId))
            {
                var ctrl = _loadedSignals[_highlightedSignalId].SignalControl;

                return (Bitmap)ctrl.Invoke(new Func<Bitmap>(() =>
                {
                    Bitmap b = new Bitmap(ctrl.Width, ctrl.Height);
                    DrawToBitmap(b, new Rectangle(ctrl.Location.X, ctrl.Location.Y, b.Width, b.Height));
                    return b;
                }));
            }
            return null;
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
                    // Removes all the signal controls.
                    TablePanel.Controls.Clear();
                    // Clears all the RowStyles.
                    TablePanel.RowStyles.Clear();
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
    }
}
