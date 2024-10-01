using ExactaEasyEng.Utilities;
using Hvld.Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Hvld.Controls
{
    public partial class HvldBaseDisplay : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        protected const float PERCENT_MIN_HEIGHT = 0f;
        /// <summary>
        /// 
        /// </summary>
        protected const float PERCENT_MAX_HEIGHT = 100f;
        /// <summary>
        /// 
        /// </summary>
        public HvldBaseDisplay()
        {
            InitializeComponent();
            // Enables the double buffering to reduce the flicker.
            // https://learn.microsoft.com/en-us/dotnet/desktop/winforms/advanced/how-to-reduce-graphics-flicker-with-double-buffering-for-forms-and-controls?view=netframeworkdesktop-4.8
            SetDoubleBuffered();
        }
        /// <summary>
        /// NOTE: the key to be used in this dictionary is the SIGNAL ID, and not the
        /// signal title. Sometimes GRETEL sends the same signal, but with different ID.
        /// </summary>
        protected IDictionary<int, HvldSignalDisplayData> _loadedSignals = new Dictionary<int, HvldSignalDisplayData>();
        /// <summary>
        /// Backups the currently displayed signal KEY (title) and ID.
        /// </summary>
        protected HvldSignalKeyId _displayedSignalKeyId = new HvldSignalKeyId();
        /// <summary>
        /// 
        /// </summary>
        [Description("A collection of currently loaded OptrelSignals controls"), Category("Optrel")]
        public IEnumerable<OptrelSignal> SignalControls
        {
            get { return _loadedSignals.Values.Select(x => x.SignalControl); }
        }
        /// <summary>
        /// Enables the antialiasing effect on all signals.
        /// </summary>
        protected bool _enableGlobalAntialiasing;
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
            SuspendLayout();
            try
            {
                foreach (var p in _loadedSignals.Values)
                {
                    p.SignalControl.IsAntiAlias = enabled;
                    // An Invalidate is mandatory to apply the antialiasing effect.
                    p.SignalControl.Invalidate();
                }
            }
            finally
            {
                ResumeLayout();
            }
        }
        /// <summary>
        /// Returns the OptrelSignal control with the specified ID.
        /// </summary>
        public OptrelSignal GetSignalControl(int signalId)
        {
            if (_loadedSignals is null)
                return null;
            return _loadedSignals[signalId].SignalControl;
        }
        /// <summary>
        /// Adds/updates the signal controls to/in the list of loaded signals.
        /// </summary>
        protected virtual void UpdateSignals(IEnumerable<OptrelSignal> signals)
        {
            foreach (var signal in signals)
            {
                if (_loadedSignals.ContainsKey(signal.Id))
                    _loadedSignals[signal.Id].SignalControl = signal;
                else
                {
                    var ssdd = new HvldSignalDisplayData(signal.Name, signal.Id, signal);
                    _loadedSignals.Add(signal.Id, ssdd);
                }
            }
        }
        /// <summary>
        /// Displays the signals with title signalKey.
        /// </summary>
        public void ShowSignal(string signalKey)
        {
            var controlToShow = _loadedSignals.Values.Where(x => x.SignalName == signalKey).FirstOrDefault();

            if (controlToShow is null)
            {
                // LOG: signal not loaded!
                return;
            }
            ShowSignal(controlToShow.SignalId);
        }
        /// <summary>
        /// Displays the signal with ID signalId: MUST BE IMPLEMENTED BY THE BASE CLASSES.
        /// </summary>
        public virtual void ShowSignal(int signalId)
        { 
        
        }
        /// <summary>
        /// Creates the OptrelSignal objects from the HvldFrame and adds them to the signal stacker control.
        /// </summary>
        public virtual void UpdateControlFromHvldFrame(
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
                // Updates all the signals.
                UpdateSignals(signals);
                // if this is the first load (ID < 0) then shows the 1st signal by ID.
                if (_displayedSignalKeyId.Id < 0)
                    ShowSignal(0);
                else
                    ShowSignal(_displayedSignalKeyId.Id);
            }
        }
        /// <summary>
        /// Returns a bitmap image of the current visualization of the control.
        /// </summary>
        public virtual Bitmap GetImage()
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return null;

            if (!IsHandleCreated || (Width + Height == 0)) return null;

            return (Bitmap)Invoke(new Func<Bitmap>(() =>
            {
                Bitmap b = new Bitmap(Width, Height);
                DrawToBitmap(b, new Rectangle(0, 0, b.Width, b.Height));
                return b;
            }));
        }
        /// <summary>
        /// Clears the control of all signals.
        /// </summary>
        public virtual void Clear()
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
        /// 
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
    }
}
