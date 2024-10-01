using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using DisplayManager;

namespace EoptisClient {

    public class EoptisStationBase : Station {

        readonly ConcurrentQueue<InspectionResults> _resultsBuffer = new ConcurrentQueue<InspectionResults>();
        readonly ManualResetEvent _killEv = new ManualResetEvent(false);
        readonly AutoResetEvent _readyResultsEv = new AutoResetEvent(false);
        readonly WaitHandle[] _alertResultsEvs = new WaitHandle[2];
        Thread _alertResultsThread;
        int rejectionCause, lastRejectionCause;

        enum AlertEvents {
            Kill = 0,
            Ready
        }

        public EoptisStationBase(StationDefinition stationDefinition)
            : base(stationDefinition) {

            NodeId = stationDefinition.Node;
            HasMeasures = true;
            _alertResultsEvs[(int)AlertEvents.Kill] = _killEv;
            _alertResultsEvs[(int)AlertEvents.Ready] = _readyResultsEv;

            _resultsBuffer.Clear();
        }

        public override void Dispose() {
            Destroy();
            base.Dispose();
        }

        void Destroy() {
            _killEv.Set();
            Thread.Sleep(100);
            Disconnect();
        }

        public override void Connect() {

            _resultsBuffer.Clear();
            if (HasMeasures) {
                if (_alertResultsThread == null || !_alertResultsThread.IsAlive) {
                    _alertResultsThread = new Thread(AlertResultsThread) {
                        Name = "EoptisStation" + IdStation.ToString("d2") + " Alert Results Thread"
                    };
                }
                if (!_alertResultsThread.IsAlive)
                    _alertResultsThread.Start();
            }
        }

        public override void Disconnect() {

            _killEv.Set();
            if (_alertResultsThread != null && _alertResultsThread.IsAlive) {
                _alertResultsThread.Join(1000);
            }
        }

        public override void Grab() {

            //PIER: PATCH TEMPORANEA DA TOGLIERE!!!!!!
            //Cameras[0].SetWorkingMode(CameraWorkingMode.ExternalSource);
        }

        public override void StopGrab() {
        }

        public override Bitmap Snap() {
            return null;
        }

        public override void SetMainImage() {
        }

        public override void SetPrevImage() {
        }

        public override void SetStreamMode(int headNumber, CameraWorkingMode cwm) {
        }

        internal void OnInspectionResults(InspectionResults e) {

            rejectionCause = e.RejectionCause;
            _resultsBuffer.Enqueue(e);
            _readyResultsEv.Set();
        }

        void AlertResultsThread() {

            while (true) {
                if (_resultsBuffer.Count > 0)
                    _readyResultsEv.Set();
                var res = WaitHandle.WaitAny(_alertResultsEvs);
                var ret = (AlertEvents)Enum.Parse(typeof(AlertEvents), res.ToString(CultureInfo.InvariantCulture));
                if (ret == AlertEvents.Kill) break; //kill ev
                if (ret != AlertEvents.Ready) continue;
                InspectionResults measuresRes;
                if (_resultsBuffer.TryDequeue(out measuresRes)) {
                    OnMeasuresAvailable(this, new MeasuresAvailableEventArgs(measuresRes));
                }
            }
        }

        public override void HardReset() {
            throw new NotImplementedException();
        }
    }


    internal static class ConcurrentQueueExtensions {

        public static void Clear<T>(this ConcurrentQueue<T> queue) {
            T item;
            while (queue.TryDequeue(out item)) {
                // do nothing
            }
        }
    }
}
