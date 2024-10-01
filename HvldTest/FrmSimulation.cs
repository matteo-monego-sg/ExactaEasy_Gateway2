using Hvld.Controls;
using Hvld.Parser;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HvldTest
{
    public partial class FrmSimulation : Form
    {
        /// <summary>
        /// 
        /// </summary>
        private struct AssociatedControls
        {
            public HvldDisplayControl MainControl;
            public PictureBox BoundPictureBox;
        }
        /// <summary>
        /// 
        /// </summary>
        private ConcurrentDictionary<int, AssociatedControls> _controlBindDictionary = new ConcurrentDictionary<int, AssociatedControls>();
        /// <summary>
        /// 
        /// </summary>
        private static readonly Random _rnd = new Random();
        /// <summary>
        /// 
        /// </summary>
        private readonly string[] _dumpFileNames;
        /// <summary>
        /// 
        /// </summary>
        private List<HvldFrame> _loadedDumpFrames = new List<HvldFrame>();
        /// <summary>
        /// 
        /// </summary>
        private FrmWait _waitPanel = new FrmWait();
        /// <summary>
        /// 
        /// </summary>
        private BackgroundWorker _heavyDutyThread;
        /// <summary>
        /// 
        /// </summary>
        private AutoResetEvent _threadStopSignal = new AutoResetEvent(false);
        /// <summary>
        /// 
        /// </summary>
        public FrmSimulation(string[] dumpFileNames) 
        {
            InitializeComponent();
            _dumpFileNames = dumpFileNames;

            _heavyDutyThread = new BackgroundWorker()
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = false
            };
            _heavyDutyThread.DoWork += HeavyDutyThread_DoWork;
        }
        /// <summary>
        /// 
        /// </summary>
        private void HeavyDutyThread_DoWork(object sender, DoWorkEventArgs e)
        {
            var bw = sender as BackgroundWorker;

            try
            {
                while (true)
                {
                    if (bw.CancellationPending)
                    {
                        e.Cancel = true;
                        break;
                    }

                    for (var i = 0; i < 4; i++)
                    {
                        if (bw.CancellationPending)
                        {
                            e.Cancel = true;
                            break;
                        }
                        // Pick a random signal from the list.
                        var index = _rnd.Next(_loadedDumpFrames.Count);
                        HvldHelper.LoadSignalsFromHvldFrame(_loadedDumpFrames[index], _controlBindDictionary[i].MainControl, out _);
                        HvldHelper.ShotHvldIntoPictureBox(_controlBindDictionary[i].MainControl, _controlBindDictionary[i].BoundPictureBox, out _);
                    }
                }
            }
            finally
            {
                _threadStopSignal.Set();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void FrmSimulation_Load(object sender, EventArgs e)
        {
            Task.Run(() => 
            {
                _controlBindDictionary.TryAdd(0, new AssociatedControls()
                {
                    MainControl = hvldDisplayControl1,
                    BoundPictureBox = pictureBox1
                });

                _controlBindDictionary.TryAdd(1, new AssociatedControls()
                {
                    MainControl = hvldDisplayControl2,
                    BoundPictureBox = pictureBox2
                });

                _controlBindDictionary.TryAdd(2, new AssociatedControls()
                {
                    MainControl = hvldDisplayControl3,
                    BoundPictureBox = pictureBox3
                });

                _controlBindDictionary.TryAdd(3, new AssociatedControls()
                {
                    MainControl = hvldDisplayControl4,
                    BoundPictureBox = pictureBox4
                });

                foreach (var dumpFile in _dumpFileNames)
                {
                    if (!File.Exists(dumpFile))
                        continue;
                    var dumpBytes = HvldHelper.ReadDumpByteArray(dumpFile, out _);
                    var dumpFrame = HvldHelper.CreateHvldFrameFromByteArray(dumpBytes, out _);
                    _loadedDumpFrames.Add(dumpFrame);
                }
            }).ContinueWith(t => 
            {
                _waitPanel.Invoke(new MethodInvoker(delegate { _waitPanel.Close(); }));
                _heavyDutyThread.RunWorkerAsync();
            });
            _waitPanel.ShowDialog(this);
        }
        /// <summary>
        /// 
        /// </summary>
        private void BtnStop_Click(object sender, EventArgs e)
        {
            _heavyDutyThread.CancelAsync();
            Task.Run(() =>
            {
                _threadStopSignal.WaitOne();
                Invoke(new MethodInvoker(() =>
                {
                    Close();
                }));
            });
        }
    }
}
