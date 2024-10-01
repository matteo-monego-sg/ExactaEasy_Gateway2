using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using SPAMI.Util.Logger;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DisplayManager
{
    public class ThumbScreenDisplay : Display
    {

        protected int ThumbPixSize { get; set; }
        //int lastThumb;
        const int margin = 3;
        List<Image<Rgb, byte>> thumbQueue = new List<Image<Rgb, byte>>();
        SolidBrush blackBrush = new SolidBrush(Color.Black);
        protected ImageBox ImageCtrl { get; set; }
        protected Image<Rgb, byte> bufferBm;
        protected Image<Rgb, byte> lastThumbImg;
        System.Windows.Forms.Timer timerInvalidate = new System.Windows.Forms.Timer();
        Image<Rgb, byte> thumbBlackImage;
        Rgb redColor = new Rgb(255, 0, 0);
        Rgb greenColor = new Rgb(0, 255, 0);
        Rgb orangeColor = new Rgb(255, 102, 0);
        Rgb fucsiaColor = new Rgb(255, 0, 255);
        Rgb blackColor = new Rgb(0, 0, 0);
        Panel _pnlThumbs;
        int _thumbQueueSize = 1;
        List<ImageBox> thumbCtrlList = new List<ImageBox>();
        //int currThumbDisplay = 0;
        int lastQueueThumbIdx;
        int thumbDisplayNum = 0;
        readonly object _syncImgObj = new object();
        readonly object _syncThumbObj = new object();
        bool initialized = false;
        bool _keepAspectRatio;
        bool _showColoredBorders;
        int _numberThumbinails;

        public ThumbScreenDisplay(string name, ImageBox imageCtrl, IStation station, Panel pnlThumbs, int thumbPixSize, int thumbQueueSize, bool keepAspectRatio, int interpolationType, bool showColoredBorders, int numberThumbinails)
            : base(name, new StationCollection() { station })
        {

            thumbBlackImage = new Image<Rgb, byte>(thumbPixSize, thumbPixSize);
            _pnlThumbs = pnlThumbs;
            _thumbQueueSize = thumbQueueSize;
            _keepAspectRatio = keepAspectRatio;
            _showColoredBorders = showColoredBorders;
            _numberThumbinails = numberThumbinails;
            ThumbPixSize = thumbPixSize;
            ImageCtrl = imageCtrl;
            InterpolationType = (Emgu.CV.CvEnum.INTER)interpolationType;
            for (int j = 0; j < _thumbQueueSize; j++)
            {
                Image<Rgb, byte> blankImage = new Image<Rgb, byte>(ThumbPixSize, ThumbPixSize);
                thumbQueue.Add(blankImage);
            }
            DisplayAOIUsed = new Dictionary<int, Rectangle>();
            DisplayAOIUsed.Add(0, new Rectangle());
            createThumbs();
            for (int i = 0; i < thumbDisplayNum; i++)
                DisplayAOIUsed.Add(i + 1, new Rectangle());
            //lastThumb = 0;
            bufferBm = new Image<Rgb, byte>(ImageCtrl.Width, ImageCtrl.Height);
            timerInvalidate.Interval = 20;
            timerInvalidate.Tick += timerInvalidate_Tick;
            timerInvalidate.Start();
            initialized = true;
            Suspend();
        }

        //~ThumbScreenDisplay() {
        //    Debug.WriteLine(Name + " DISPOSED!");
        //}

        //public override void Dispose() {
        //    initialized = false;
        //    base.Dispose();
        //    ImageCtrl = null;
        //    thumbBlackImage.Dispose();
        //    thumbBlackImage = null;
        //    for (int i = 0; i < thumbQueue.Count; i++) {
        //        thumbQueue[i].Dispose();
        //        thumbQueue[i] = null;
        //    }
        //    thumbQueue.Clear();
        //    thumbQueue = null;
        //    bufferBm.Dispose();
        //    bufferBm = null;
        //    timerInvalidate.Stop();
        //    timerInvalidate.Dispose();
        //    timerInvalidate = null;
        //    thumbCtrlList.Clear();
        //    thumbCtrlList = null;
        //}

        public override void Dispose()
        {

            timerInvalidate.Stop();
            foreach (ImageBox ib in thumbCtrlList)
            {
                ib.Dispose();
            }
            thumbCtrlList.Clear();
            foreach (Image<Rgb, byte> thumb in thumbQueue)
            {
                if (thumb != null)
                    thumb.Dispose();
            }
            thumbQueue.Clear();
            if (bufferBm != null)
                bufferBm.Dispose();
            if (thumbBlackImage != null)
                thumbBlackImage.Dispose();
            if (lastThumbImg != null)
                lastThumbImg.Dispose();
            if (_pnlThumbs != null)
                _pnlThumbs.Dispose();

            ImageCtrl = null;
            base.Dispose();
        }

        void createThumbs()
        {

            if (ThumbPixSize <= 0)
            {
                Log.Line(LogLevels.Warning, "ThumbScreenDisplay.createThumbs", "Thumbnail pixel size <= 0!");
                return;
            }
            thumbDisplayNum = _pnlThumbs.Height / ThumbPixSize;
            if (_numberThumbinails > 4)
                thumbDisplayNum = _numberThumbinails;
            else
                _pnlThumbs.AutoScroll = false;
            //thumbDisplayNum = 14;
            //if (thumbDisplayNum < 1)
            //    Log.Line(LogLevels.Warning, "ThumbScreenDisplay.createThumbs", "Thumbnail display count <= 0!");
            int spareSpaceTotY = _pnlThumbs.Height - ThumbPixSize * thumbDisplayNum;
            int spareSpaceY = (int)((float)spareSpaceTotY / (thumbDisplayNum + 1));
            thumbCtrlList.Clear();
            _pnlThumbs.SuspendLayout();
            _pnlThumbs.Controls.Clear();
            for (int i = 0; i < thumbDisplayNum; i++)
            {
                ImageBox thumbImgCtrl = new ImageBox();
                thumbImgCtrl.BackColor = System.Drawing.Color.Black;
                thumbImgCtrl.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
                thumbImgCtrl.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal;
                thumbImgCtrl.Name = "thumbImgCtrl" + i;
                thumbImgCtrl.TabStop = false;
                thumbImgCtrl.Location = new Point(0, spareSpaceY + i * (spareSpaceY + ThumbPixSize));
                thumbImgCtrl.Size = new Size(ThumbPixSize, ThumbPixSize);
                thumbImgCtrl.BorderStyle = BorderStyle.None;
                _pnlThumbs.Controls.Add(thumbImgCtrl);
                thumbCtrlList.Add(thumbImgCtrl);
            }
            _pnlThumbs.ResumeLayout();
            lastQueueThumbIdx = 0;
        }

        public override void Resize(int w, int h)
        {
            if (bufferBm != null && ImageCtrl != null)
            {
                bool prevSuspend = displaySuspended;
                Suspend();
                lock (_syncThumbObj)
                {
                    //Debug.WriteLine("INIZIO RESIZE");
                    createThumbs();
                    //Debug.WriteLine("FINE RESIZE");
                }
                if (prevSuspend == false)
                    Resume();

            }
        }

        public override void Suspend()
        {
            base.Suspend();
            timerInvalidate.Stop();
        }

        public override void Resume()
        {
            base.Resume();
            timerInvalidate.Start();
        }

        void timerInvalidate_Tick(object sender, EventArgs e)
        {

            if (newImageToShow)
            {
                lock (_syncImgObj)
                {
                    //bufferBm.Draw(new Rectangle(0, 0, bufferBm.Width, bufferBm.Height), greenColor, 2);
                    if (bufferBm != null)
                    {
                        ImageCtrl.Image = bufferBm;
                        newImageToShow = false;
                    }
                }
                lock (_syncThumbObj)
                {
                    int thumbToShow = thumbCtrlList.Count;
                    for (int iT = 0; iT < thumbToShow; iT++)
                    {
                        int id = lastQueueThumbIdx - 1 - iT;
                        if (id < 0) id += thumbQueue.Count;
                        //Rgb color = (iT == 0) ? redColor : orangeColor;
                        thumbQueue[id].Draw(new Rectangle(0, 0, thumbQueue[id].Width, thumbQueue[id].Height), redColor, 2);
                        thumbCtrlList[iT].Image = thumbQueue[id];
                    }
                }
            }
        }

        public override void IncrementThumbId(IStation currentStation)
        {

            if (!initialized)
                return;

            if (currentStation.CurrentThumbnail != null && thumbCtrlList != null && thumbCtrlList.Count > 0 && lastThumbImg != null)
            {
                lastQueueThumbIdx = (lastQueueThumbIdx + 1) % thumbQueue.Count;
            }
        }

        public override void RenderImage(IStation currentStation)
        {

            if (!initialized)
                return;

            Rectangle displayAOIUsed;
            try
            {
                if (currentStation.CurrentImage != null)
                {
                    ImageCtrl.BackgroundImage = null;
                    lock (currentStation.CurrentImage)
                    {
                        currentStation.AdjustImage(currentStation.CurrentImage, ImageCtrl.Width, ImageCtrl.Height, true, InterpolationType, ref bufferBm, out displayAOIUsed);
                        ImageShown = currentStation.CurrentImage;
                        if (DisplayAOIUsed.ContainsKey(0))
                            DisplayAOIUsed[0] = new Rectangle(displayAOIUsed.X, displayAOIUsed.Y, displayAOIUsed.Width, displayAOIUsed.Height);

                        // Matteo 20/02/2024. border should be shown dynamically.
                        if (currentStation.CurrentIsReject.HasValue)
                        {
                            //bufferBm = currentStation.CurrentImage.Resize((double)Math.Min((float)ImageCtrl.Width / currentStation.CurrentImage.Width, (float)ImageCtrl.Height / currentStation.CurrentImage.Height), Emgu.CV.CvEnum.INTER.CV_INTER_NN);
                            Rgb color = redColor;
                            if (!currentStation.CurrentIsReject.Value)
                                color = greenColor;
                            else
                            {
                                if (currentStation.CurrentRejectionBit == 16384) //LIGHT ALARM
                                    color = fucsiaColor;
                                else if (currentStation.CurrentRejectionBit == 32768) //TECHNICAL
                                    color = orangeColor;
                            }

                            if (_showColoredBorders && bufferBm != null)
                                bufferBm.Draw(new Rectangle(0, 0, bufferBm.ROI.Width, bufferBm.ROI.Height), color, 2);
                        }
                        newImageToShow = true;
                    }
                }

                if (currentStation.CurrentThumbnail != null && thumbCtrlList != null && thumbCtrlList.Count > 0)
                {

                    lock (currentStation.CurrentThumbnail)
                    {
                        currentStation.AdjustImage(currentStation.CurrentThumbnail, ThumbPixSize, ThumbPixSize, true, InterpolationType, ref lastThumbImg, out displayAOIUsed);
                    }
                    if (lastThumbImg != null)
                    {
                        lock (_syncThumbObj)
                        {
                            Image<Rgb, byte> destThumb = thumbQueue[lastQueueThumbIdx];
                            destThumb.ROI = new Rectangle(Convert.ToInt32((destThumb.Width - lastThumbImg.Width) / 2.0F), Convert.ToInt32((destThumb.Height - lastThumbImg.Height) / 2.0F), lastThumbImg.Width, lastThumbImg.Height);
                            CvInvoke.cvCopy(lastThumbImg.Ptr, destThumb.Ptr, IntPtr.Zero);
                            destThumb.ROI = Rectangle.Empty;
                            if (DisplayAOIUsed.ContainsKey(lastQueueThumbIdx + 1))
                                DisplayAOIUsed[lastQueueThumbIdx + 1] = new Rectangle(displayAOIUsed.X, displayAOIUsed.Y, displayAOIUsed.Width, displayAOIUsed.Height);
                            lastThumbImg.Dispose();
                        }
                    }
                }
            }
            catch (OutOfMemoryException ex)
            {
                Log.Line(LogLevels.Error, "ThumbScreenDisplay.RenderImage", "OutOfMemoryException: " + ex.Message);
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Warning, "ThumbScreenDisplay.RenderImage", "Warning: " + ex.Message);
            }
        }

        //public override void RenderImagesResults(Camera currentCamera, ImagesResults imgsResults) {

        //    throw new NotImplementedException();
        //}

        public override void ClearBackground()
        {

            if (ImageCtrl != null)
                ImageCtrl.Image = new Image<Rgb, byte>(ImageCtrl.Width, ImageCtrl.Height);
        }
    }
}
