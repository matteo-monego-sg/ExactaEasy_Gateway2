using Hvld.Controls;
using Hvld.Parser;
using SPAMI.Util.Logger;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using ZedGraph;

namespace DisplayManager
{
    /// <summary>
    /// Graph control to visualize the HVLD 2.0 signals in an HvldSingleDisplay.
    /// NOTE: as in other classes here, the events fired from the station are running on a different thread
    /// then UI. We fill up a queue and we use a System.Windows.Forms Timer to keep the update work
    /// on the UI thread.
    /// </summary>
    public class Hvld2Graph : Graph
    {
        /// <summary>
        /// Dequeues and elaborates a frame every DEQUEUE_TIMEOUT_MS ms.
        /// </summary>
        private const int DEQUEUE_TIMEOUT_MS = 15;
        /// <summary>
        /// The queue acts like a circular buffer: if the size becomes bigger than FRAME_MICROQUEUE_SIZE,
        /// an element from the head of the queue is popped.
        /// </summary>
        private const int FRAME_MICROQUEUE_SIZE = 5;
        /// <summary>
        /// Queuable objects for later elaboration, containing the frame received
        /// and some other data used to update the control.
        /// </summary>
        private class HvldUiUpdate
        { 
            public HvldFrame Frame { get; set; }
            public HvldControlUpdateData Data { get; set; }
        }
        /// <summary>
        /// Container control showing all the signals contained in a frame.
        /// </summary>
        private readonly HvldDisplayControl _graphCtrl;
        /// <summary>
        /// Reference to the station this control is bound to.
        /// </summary>
        private readonly Station _currentStation;
        /// <summary>
        /// Queue holdin the HVLD frames recevived from the station.
        /// </summary>
        private readonly ConcurrentQueue<HvldUiUpdate> _uiUpdatesQueue = new ConcurrentQueue<HvldUiUpdate>();
        /// <summary>
        /// Update timer: dequeues and elaborates the frames.
        /// </summary>
        private readonly Timer _timerUpdate;
        /// <summary>
        /// Class constructor.
        /// </summary>
        public Hvld2Graph(string name, HvldDisplayControl graphCtrl, IStation station)
            : base(name, new StationCollection() { station })
        {
            _currentStation = station as Station;
            _graphCtrl = graphCtrl;
            _timerUpdate = new Timer();
            _timerUpdate.Interval = DEQUEUE_TIMEOUT_MS;
            _timerUpdate.Tick += TimerUpdate_Tick;
            _timerUpdate.Start();
        }
        /// <summary>
        /// At every tick whe check the queue and, if there are frames enqueued,
        /// we elaborate one of them.
        /// </summary>
        private void TimerUpdate_Tick(object sender, EventArgs e)
        {
            if (_uiUpdatesQueue.Count == 0)
                return;
            if (!_graphCtrl.IsHandleCreated)
                return;
            // Gets the data to elaborate from the queue.
            if (!_uiUpdatesQueue.TryDequeue(out HvldUiUpdate update))
                return;
            // Updates/adds the signals to the control.
            // This function actually updates the UI.
            _graphCtrl.UpdateControlFromHvldFrame(update.Frame, update.Data);
            // Asynchronous update of the interface, as the thumbnail of a signal is not very important.
            _graphCtrl.BeginInvoke(new Action(() => 
            {
                // Gets an image from the control.
                using (var currentGraph = _graphCtrl.GetImage())
                {
                    // Null-check on the image object.
                    if (!(currentGraph is null) && !(_currentStation is null))
                    {
                        if (!(_currentStation.CurrentImage is null))
                            _currentStation.CurrentImage.Dispose();
                        // Creates a new OpenCV image from the Bitmap.
                        _currentStation.CurrentImage = new Emgu.CV.Image<Emgu.CV.Structure.Rgb, byte>(currentGraph);
                        _currentStation.CurrentIsReject = null;
                        _currentStation.CurrentRejectionBit = 0;
                        // Sets the created image as main station image.
                        _currentStation.SetMainImage();
                    }
                }
            }));
        }
        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            if (!(_timerUpdate is null))
            {
                _timerUpdate.Stop();
                _timerUpdate.Dispose();
            }
            // The graph control is being disposed.
            if (!(_graphCtrl is null))
                _graphCtrl.Dispose();
            base.Dispose();
        }
        /// <summary>
        /// Station event, fired when new HVLD 2.0 data is available.
        /// </summary>
        protected override void station_HvldDataAvailable(object sender, HvldDataAvailableEventArgs e)
        {
            if (e.Frame is null)
                return;

            try
            {
                // Creates the settings to tweak the appearance of the control.
                var hvldSettings = new HvldControlUpdateData()
                {
                    XAxisTitle = "Sample Count",
                    YAxisTitle = "Measure",
                    BorderSize = 5,
                    ShowSignalBoundingLines = true,
                    ScientificNotationOnYAxis = false,
                    InSpecBorderColor = Color.Green,
                    OutSpecBorderColor = Color.Red,
                    SignalColorOverride = null
                };
                // Check if the circular buffer is full.
                if (_uiUpdatesQueue.Count > FRAME_MICROQUEUE_SIZE)
                    _uiUpdatesQueue.TryDequeue(out _);
                // Enqueues new ipdate data for the timer.
                _uiUpdatesQueue.Enqueue(new HvldUiUpdate()
                {
                    Frame = e.Frame,
                    Data = hvldSettings
                });
            }
            catch (Exception ex)
            {
                Log.Line(
                    LogLevels.Error,
                    "Hvld2Graph::station_HvldDataAvailable",
                    $"Error while handling an HVLD 2.0 frame: {ex.Message}");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void ClearBackground()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphLabel"></param>
        /// <param name="chartLabel"></param>
        public override void RemoveChart(string graphLabel, string chartLabel)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphLabel"></param>
        /// <param name="chartLabel"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="numOfPoints"></param>
        public override void AddPoints(string graphLabel, string chartLabel, double[] x, double[] y, int numOfPoints)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphLabel"></param>
        /// <param name="chartLabel"></param>
        /// <param name="visible"></param>
        /// <returns></returns>
        public override bool SetChartVisible(string graphLabel, string chartLabel, bool visible)
        {
            return visible;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphLabel"></param>
        /// <param name="chartLabel"></param>
        /// <returns></returns>
        public override bool GetChartVisible(string graphLabel, string chartLabel)
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        public override void ResetCounters()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphLabel"></param>
        /// <param name="chartLabel"></param>
        /// <returns></returns>
        protected override bool isChartActive(string graphLabel, string chartLabel)
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphLabel"></param>
        /// <param name="title"></param>
        /// <param name="xAxis"></param>
        /// <param name="yAxis"></param>
        /// <param name="xMin"></param>
        /// <param name="xMax"></param>
        /// <param name="yMin"></param>
        /// <param name="yMax"></param>
        public override void SetTitles(string graphLabel, string title, string xAxis, string yAxis, double xMin, double xMax, double yMin, double yMax)
        {
        
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphLabel"></param>
        /// <param name="name"></param>
        /// <param name="limitType"></param>
        /// <param name="limitValue"></param>
        /// <param name="fromPt"></param>
        /// <param name="toPt"></param>
        /// <param name="color"></param>
        /// <param name="dashStyle"></param>
        public override void SetLimit(string graphLabel, string name, LimitType limitType, double limitValue, double fromPt, double toPt, Color color, DashStyle dashStyle)
        {
          
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphLabel"></param>
        /// <param name="chartLabel"></param>
        /// <param name="color"></param>
        /// <param name="symbolType"></param>
        public override void AddChart(string graphLabel, string chartLabel, Color color, SymbolType symbolType)
        {
    
        }
    }
}
