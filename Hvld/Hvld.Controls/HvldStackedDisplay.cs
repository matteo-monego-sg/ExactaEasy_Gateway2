using ExactaEasyEng.Utilities;
using Hvld.Parser;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Hvld.Controls
{
    /// <summary>
    /// HVLD 2.0 signals visualizer, the signals are stacked one above the other.
    /// </summary>
    public partial class HvldStackedDisplay : HvldBaseDisplay
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        public HvldStackedDisplay()
        {
            // Boilerplate Winforms code.
            InitializeComponent();
            // Clears all the TablePanel's RowStyles.
            TablePanel.RowStyles.Clear();
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
        /// 
        /// </summary>
        protected override void UpdateSignals(IEnumerable<OptrelSignal> signals)
        {
            SuspendLayout();
            try
            {
                foreach (var signal in signals)
                {
                    // Sets the antialiasing if globally enabled.
                    signal.IsAntiAlias = _enableGlobalAntialiasing;

                    if (_loadedSignals.ContainsKey(signal.Id))
                    {
                        // Gets the signal from the list of loaded signals.
                        var current = _loadedSignals[signal.Id].SignalControl;
                        // Gets the current table panel row.
                        var currentRow = TablePanel.GetRow(current);

                        if (currentRow < 0)
                        {
                            // Adds a new table row at the bottom.
                            var rowIndex = TablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, PERCENT_MAX_HEIGHT));
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
                        var rowIndex = TablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, PERCENT_MAX_HEIGHT));
                        // Adds a new optrel control at the bottom.
                        TablePanel.Controls.Add(signal, 0, rowIndex);
                    }
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

            if (!IsHandleCreated || (Width + Height == 0)) return null;

            return (Bitmap)TablePanel.Invoke(new Func<Bitmap>(() =>
            {
                Bitmap b = new Bitmap(TablePanel.Width, TablePanel.Height);
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
