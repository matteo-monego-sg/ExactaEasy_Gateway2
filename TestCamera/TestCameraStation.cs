using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using DisplayManager;
using Emgu.CV;
using Emgu.CV.Structure;
using ExactaEasyCore;

namespace TestCamera {
    public class TestCameraStation : Station {

        static Dictionary<int, Image<Rgb, byte>> extImages = new Dictionary<int, Image<Rgb, byte>>();
        bool stopGrab = false;
        bool isGrabbing = false;
        internal Thread resultTh;
        internal bool exit = false;
        Bitmap bm = new Bitmap(640, 480);
        object syncBmp = new object();

        public TestCameraStation(StationDefinition stationDefinition)
            : base(stationDefinition) {

            NodeId = stationDefinition.Node;
            if (extImages.Count == 0) {
                string[] files = System.IO.Directory.GetFiles(@"D:\IMMAGINI\Good\ST0 A\Frames");
                for (int i = 0; i < files.Length; i++) {
                    Image<Rgb, byte> bm = new Image<Rgb, byte>(files[i]);
                    extImages.Add(i, bm);
                }
            }

            Grab();
            resultTh = new Thread(new ThreadStart(resultThread));
            resultTh.Start();
        }

        public override void Connect() {
            throw new NotImplementedException();
        }

        public override void Disconnect() {
            throw new NotImplementedException();
        }

        public override void Grab() {

            if (!isGrabbing) {
                stopGrab = false;
                Thread grabThread = new Thread(new ThreadStart(grabSvc));
                grabThread.Name = "TestCameraGrabSvc";
                grabThread.Start();
            }
        }

        public override Bitmap Snap() {

            buildDummyBitmap();
            return (Bitmap)bm.Clone();
        }

        void grabSvc() {

            Random rnd = new Random();

            bool isReject = false;
            while (!stopGrab) {
                isGrabbing = true;
                Image<Rgb, Byte> bm = extImages[rnd.Next(extImages.Count - 1)];
                //buildDummyBitmap();
                isReject = !isReject;
                OnImageAvailable(this, new ImageAvailableEventArgs(bm, bm, isReject));
                System.Threading.Thread.Sleep(100);
            }
            isGrabbing = false;
        }

        void buildDummyBitmap() {

            lock (syncBmp) {
                try {
                    using (Graphics g = Graphics.FromImage(bm)) {
                        g.FillRectangle(new SolidBrush(Color.White), new Rectangle(10, 10, 620, 460));
                        g.DrawString("Node: " + NodeId.ToString() + " Station: " + IdStation.ToString() + " - " + DateTime.Now.ToString("hh:mm:ss.fff"), new Font("Arial", 24), new SolidBrush(Color.Black), 150, 200);
                        //g.DrawString("Working mode: " + currWorkingMode.ToString(), new Font("Arial", 12), new SolidBrush(Color.Black), 150, 230);
                    }
                }
                catch (InvalidOperationException ex) {
                }
            }
        }

        public override void SetStreamMode(int headNumber, CameraWorkingMode cwm) {
        }

        public override void StopGrab() {

            stopGrab = true;
        }

        //protected virtual void OnMeasuresAvailable(object sender, MeasuresAvailableEventArgs e) {

        //    EventHandler<MeasuresAvailableEventArgs> handler = _measuresAvailable;
        //    if (handler != null)
        //        handler(sender, e);
        //}

        void resultThread() {
            while (!exit) {
                try {
                    InspectionResults inspectionRes = generateFakeResults();
                    OnMeasuresAvailable(this, new MeasuresAvailableEventArgs(inspectionRes));
                    Thread.Sleep(100000);
                }
                catch (Exception ex) {
                    Debug.WriteLine("");
                }
            }
        }

        int spinId = 0;
        int vialId = 0;
        bool isRej = false;
        InspectionResults generateFakeResults() {
            ToolResultsCollection trc = new ToolResultsCollection();
            for (int it = 0; it < 3; it++) {
                MeasureResultsCollection mrc = new MeasureResultsCollection();
                bool toolIsRej = false;
                for (int im = 0; im < 6; im++) {
                    bool isOk = true;
                    bool isUsed = im < 4;
                    MeasureTypeEnum measType = (MeasureTypeEnum)(im % 4);
                    string measure = "";
                    switch (measType) {
                        case MeasureTypeEnum.BOOL:
                            measure = true.ToString();
                            break;
                        case MeasureTypeEnum.DOUBLE:
                            double val = Math.Sin(vialId + it + im);
                            measure = val.ToString();
                            isOk = Math.Abs(val) < 0.5F;
                            break;
                        case MeasureTypeEnum.INT:
                            measure = (im * 1000).ToString();
                            break;
                        case MeasureTypeEnum.STRING:
                            measure = "Veni vidi vici " + im.ToString();
                            break;
                        default:
                            measure = "TIPO NON SUPPORTATO!!!!";
                            break;
                    }
                    if (!isOk) toolIsRej = true;
                    mrc.Add(new MeasureResults(im, "Measure_" + im.ToString(), "m^2", isOk, isUsed, measType, measure));
                }
                bool toolIsActive = (it % 2 == 0) ? true : false;
                bool toolIsDispayed = true;
                trc.Add(new ToolResults(it, toolIsActive, toolIsRej, toolIsDispayed, mrc));
            }
            //spinId = (spinId + 1) % 40;
            vialId = (vialId + 1) % 255;
            bool isActive = ((IdStation % 2) == 0) ? true : false;
            isRej = (IdStation % 2 == 0);//!isRej;//(_cameraDefinition.Id > 0) ? true : false;
            int defectCode = (isRej == false) ? 0 : 255;
            return new InspectionResults(NodeId, IdStation, spinId, Convert.ToUInt32(vialId), isActive, isRej, defectCode, trc);
        }

        public override void SetMainImage() {
            throw new NotImplementedException();
        }

        public override void SetPrevImage() {
            throw new NotImplementedException();
        }
    }
}
