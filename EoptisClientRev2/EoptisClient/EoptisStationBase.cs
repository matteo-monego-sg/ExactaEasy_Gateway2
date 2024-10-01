using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using DisplayManager;
using ExactaEasyCore;
using SPAMI.Util.Logger;

namespace EoptisClient {

    public class EoptisStationBase : Station {

        readonly ConcurrentQueue<InspectionResults> _resultsBuffer = new ConcurrentQueue<InspectionResults>();
        readonly ManualResetEvent _killEv = new ManualResetEvent(false);
        readonly AutoResetEvent _readyResultsEv = new AutoResetEvent(false);
        readonly WaitHandle[] _alertResultsEvs = new WaitHandle[2];
        Thread _alertResultsThread;
        int rejectionCause/*, lastRejectionCause*/;

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

        //private void createStationSavePath(string path) {

        //    _imgSaved = _imgToSave = 0;
        //    _stationSavePath = path + Description.Replace("-", "");
        //    if (!Directory.Exists(_stationSavePath))
        //        Directory.CreateDirectory(_stationSavePath);
        //    if (!Directory.Exists(_stationSavePath + @"\Results"))
        //        Directory.CreateDirectory(_stationSavePath + @"\Results");
        //    if (!Directory.Exists(_stationSavePath + @"\Thumbnails"))
        //        Directory.CreateDirectory(_stationSavePath + @"\Thumbnails");

        //}

        //public override void SaveBufferedImages(string path, bool consensus) {

        //    createStationSavePath(path);
        //    _rec = consensus;
        //    OnRecBufferState(this, new RecordStateEventArgs(_rec, RecBufferSize));

        //    if (_rec == true) {
        //        ResetImagesBuffer(path, false);
        //    }
        //    if (_imgToSave != _imgSaved) {
        //        Log.Line(LogLevels.Warning, "GretelStationBase.SaveBufferedImages", "Images to save: {0}, Images saved: {1}", _imgToSave, _imgSaved);
        //    }
        //}

        //void writeImage(RawInspectionData inspectionData, string path) {

        //    try {
        //        GretelSvc.InspResultInfo resInfo = DecodeImagesResultDataHeader(inspectionData.Image);
        //        DecodeImagesResultsData(resInfo, inspectionData.Image);
        //        string dateStr = DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss");
        //        string statDescr = Description.Replace("-", "");
        //        imgToShow.Save(path + @"\Results\Result_" + statDescr + "_" + dateStr + "_SP_" + inspectionData.Result.SpindleId + "_ID_" + inspectionData.VialId.ToString().PadLeft(3, '0') + ".tiff");
        //        thumbToShow.Save(path + @"\Thumbnails\Thumb_" + statDescr + "_" + dateStr + "_SP_" + +inspectionData.Result.SpindleId + "_ID_" + inspectionData.VialId.ToString().PadLeft(3, '0') + ".tiff");
        //    }
        //    catch (Exception ex) {
        //        Log.Line(LogLevels.Error, "GretelStationBase.OnClientImagesResults", Description + ": Error saving Gretel images: " + ex.Message);
        //        _rec = false;
        //        OnRecBufferState(this, new RecordStateEventArgs(_rec, RecBufferSize));
        //    }
        //}

        //public override void ResetImagesBuffer(string path, bool freeFolders) {

        //    createStationSavePath(path);

        //    if (inspectionBuffer != null) {
        //        inspectionBuffer.Reset();
        //    }
        //    if (freeFolders == true && Directory.Exists(_stationSavePath) == true) {
        //        foreach (string fileName in Directory.GetFiles(_stationSavePath + @"\Results"))
        //            File.Delete(fileName);

        //        foreach (string fileName in Directory.GetFiles(_stationSavePath + @"\Thumbnails"))
        //            File.Delete(fileName);
        //    }
        //}

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

            OnImageAvailable(this, new ImageAvailableEventArgs(CurrentImage, CurrentThumbnail, CurrentIsReject, CurrentRejectionBit));
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

        public override void SaveBufferedImages(string path, ExactaEasyCore.SaveConditions saveCondition, int toSave) {

            EoptisDeviceBase d = Cameras.First() as EoptisDeviceBase;
            EoptisParameterCollection parameters = new EoptisParameterCollection();
            EoptisParameter par1 = new EoptisParameter(SYSTEM_ID.PAR_RECORDING_MODE, EoptisSvc.ParamsDictionary.Parameters[SYSTEM_ID.PAR_RECORDING_MODE].Type, EoptisSvc.ParamsDictionary.Parameters[SYSTEM_ID.PAR_RECORDING_MODE].ByteLength);
            par1.Value = ((int)saveCondition).ToString();
            parameters.Add(par1);
            EoptisParameter par2 = new EoptisParameter(SYSTEM_ID.PAR_RECORDING_TO_SAVE, EoptisSvc.ParamsDictionary.Parameters[SYSTEM_ID.PAR_RECORDING_TO_SAVE].Type, EoptisSvc.ParamsDictionary.Parameters[SYSTEM_ID.PAR_RECORDING_TO_SAVE].ByteLength);
            par2.Value = (saveCondition == SaveConditions.Never) ? 0.ToString() : toSave.ToString();
            parameters.Add(par2);
            EoptisSvc.SetParameters(d.IP4Address, parameters);
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
                    //saveSpectrum();
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
