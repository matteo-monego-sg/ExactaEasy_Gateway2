using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using System.IO;
using System.Threading;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ImageViewerGretel {

    public partial class ImageViewerGretel : UserControl {

        public List<Mat> ImageList { get; private set; }
        public int InitImageIndex { get; set; }
        public int ImageStep { get; set; }

        int sleepTime = 0;
        int imageIdx, currentImageId;
        string imageExt = "";
        ManualResetEvent killEv = new ManualResetEvent(false);
        Mat currentResultImage;
        Mat notFoundImage;
        Image<Bgr, Byte> currentMiniImage;
        Mat currentZoomImage;
        double _zoomScale = 1.0;
        MCvScalar redColor = new MCvScalar(0, 0, 255);
        MCvScalar blackColor = new MCvScalar(0, 0, 0);
        Point[][] miniRectangleCorners = new Point[1][];
        bool refreshDisplayedArea;
        bool refreshImagesFlag;
        bool refreshScroolbarsFlag;
        bool firstLoadFlag = true;
        Stopwatch diagnosticTimer = new Stopwatch();
        bool play = false;
        PixelInfo pixInfo;

        public ImageViewerGretel() {

            InitializeComponent();
            diagnosticTimer.Start();
            miniRectangleCorners[0] = new Point[4];
            for (int i = 0; i < 4; i++)
                miniRectangleCorners[0][i] = new Point();
            btnCollapse.Image = (splitContImage.Panel1Collapsed == true) ? global::ImageViewerGretel.Properties.Resources.arrow_32 : global::ImageViewerGretel.Properties.Resources.arrow_32back;
            InitImageIndex = 0;
            ImageStep = 1;
            currentImageId = imageIdx = InitImageIndex;
            sleepTime = 10000 / tbRate.Value;
            timerRefresh.Interval = 5;// Math.Min(sleepTime, 200);
            timerRefresh.Start();
            ImageList = new List<Mat>();
            imageBoxZoom.HorizontalScrollBar.Scroll += HorizontalScrollBar_Scroll;
            imageBoxZoom.VerticalScrollBar.Scroll += VerticalScrollBar_Scroll;
            checkPlayStatus();
        }

        public void Destroy() {

            play = false;
            killEv.Set();
            timerRefresh.Stop();
            Thread.Sleep(100);
            //lock (ImageList) {
            if (currentMiniImage != null)
                currentMiniImage.Dispose();
            currentMiniImage = null;
            if (currentZoomImage != null)
                currentZoomImage.Dispose();
            currentZoomImage = null;
            if (currentResultImage != null)
                currentResultImage.Dispose();
            currentResultImage = null;
            if (notFoundImage != null)
                notFoundImage.Dispose();
            notFoundImage = null;
            foreach (Mat img in ImageList)
                img.Dispose();
            ImageList.Clear();
            //}
        }

        /// <summary>
        /// Set current frame rate.
        /// </summary>
        /// <param name="fps"></param>
        public void SetFrameRate(float fps) {

            try {
                int newFr10 = (int)(fps * 10);
                tbRate.Value = newFr10;
                tbRate_Scroll(this, EventArgs.Empty);
            }
            catch /*(Exception ex)*/ {
                throw new Exception("An error occurred while setting framerate. Value must be between " + (tbRate.Minimum / 10.0).ToString() + " and " + (tbRate.Maximum / 10.0).ToString());
            }
        }

        /// <summary>
        /// Load an image from a given path.
        /// </summary>
        /// <param name="imagePath"></param>
        public void LoadImage(string imagePath) {

            LoadImage(imagePath, "", -1, -1, CutType.None);
        }

        /// <summary>
        /// Load an image from a given path and the corresponding result image.
        /// </summary>
        /// <param name="path"></param>
        public void LoadImage(string imagePath, string resultPath) {

            LoadImage(imagePath, resultPath, -1, -1, CutType.None);
        }

        /// <summary>
        /// Load an image to cut from a given path and the corresponding result image.
        /// </summary>
        /// <param name="bigImagePath"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="cutType"></param>
        public void LoadImage(string bigImagePath, string resultPath, int width, int height, CutType cutType) {

            if (firstLoadFlag == true) {
                notFoundImage = new Mat(1024, 1024, DepthType.Cv8U, 3);
                notFoundImage.SetTo(blackColor);
                CvInvoke.PutText(
                   notFoundImage,
                   "Result Image Not Found".ToUpper(),
                   new System.Drawing.Point(10, 80),
                   FontFace.HersheyDuplex,
                   2.0,
                   redColor);
                firstLoadFlag = false;
            }
            SuspendLayout();
            play = false;
            timerRefresh.Stop();
            refreshDisplayedArea = false;
            Thread.Sleep(100);
            try {
                lock (ImageList) {
                    foreach (Mat img in ImageList)
                        img.Dispose();
                    ImageList.Clear();
                    imageBoxMini.Image = null;
                    imageBoxZoom.Image = null;
                    if (currentMiniImage != null)
                        currentMiniImage.Dispose();
                    currentMiniImage = null;
                    if (currentZoomImage != null)
                        currentZoomImage.Dispose();
                    currentZoomImage = null;
                    pixInfo.Reset();

                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();

                    Mat newImage = CvInvoke.Imread(bigImagePath, Emgu.CV.CvEnum.LoadImageType.AnyColor);
                    imageExt = Path.GetExtension(bigImagePath);
                    width = Math.Min(width, newImage.Cols);
                    height = Math.Min(height, newImage.Rows);
                    if ((width == newImage.Cols) && (height == newImage.Rows))
                        ImageList.Add(newImage);
                    else if ((width > 0 && (newImage.Cols % width == 0)) &&
                        (height > 0 && (newImage.Rows % height == 0))) {
                        cutImage(newImage, width, height, cutType);
                        if (newImage != null)
                            newImage.Dispose();
                        newImage = null;
                    }
                    else
                        ImageList.Add(newImage);
                    if (File.Exists(resultPath) == false) {
                        currentResultImage = notFoundImage;
                    }
                    else {
                        currentResultImage = CvInvoke.Imread(resultPath, Emgu.CV.CvEnum.LoadImageType.AnyColor);
                    }
                    imageBoxResult.Image = currentResultImage;
                    currentImageId = imageIdx = Math.Max(0, Math.Min(InitImageIndex, ImageList.Count - 1));
                }
                updateImage();
                refreshScroolbarsFlag = true;
                refreshDisplayedArea = true;
            }
            catch (Exception ex) {
                throw new Exception("An error occurred while loading image: " + ex);
            }
            ResumeLayout(true);
            timerRefresh.Start();
        }

        public void LoadImageFolder(string folderPath) {

            SuspendLayout();
            play = false;
            timerRefresh.Stop();
            refreshDisplayedArea = false;
            Thread.Sleep(100);
            try {
                lock (ImageList) {
                    foreach (Mat img in ImageList)
                        img.Dispose();
                    ImageList.Clear();
                    imageBoxMini.Image = null;
                    imageBoxZoom.Image = null;
                    if (currentMiniImage != null)
                        currentMiniImage.Dispose();
                    currentMiniImage = null;
                    if (currentZoomImage != null)
                        currentZoomImage.Dispose();
                    currentZoomImage = null;
                    pixInfo.Reset();

                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();

                    DirectoryInfo di = new DirectoryInfo(folderPath);
                    var fileList = di.GetFiles("*.tif");//.OrderBy(f => f).Distinct().ToList();
                    currentResultImage = notFoundImage;
                    foreach (FileInfo fi in fileList) {// (int iF = 0; iF < fileList.Count; iF++) {
                        Mat newImage = CvInvoke.Imread(fi.FullName, Emgu.CV.CvEnum.LoadImageType.AnyColor);
                        if (fi.Name.Contains("Result")) {
                            currentResultImage = CvInvoke.Imread(fi.FullName, Emgu.CV.CvEnum.LoadImageType.AnyColor);
                        }
                        else {
                            ImageList.Add(newImage);
                        }
                    }
                    imageBoxResult.Image = currentResultImage;
                    currentImageId = imageIdx = Math.Max(0, Math.Min(InitImageIndex, ImageList.Count - 1));
                }
                updateImage();
                refreshScroolbarsFlag = true;
                refreshDisplayedArea = true;
            }
            catch (Exception ex) {
                throw new Exception("An error occurred while loading image: " + ex);
            }
            ResumeLayout(true);
            timerRefresh.Start();
        }

        public void CreateVideo(MergeType mergeType, int rotation, string folderPath) {

            Directory.CreateDirectory(folderPath);
            int videoImgCount = ImageList.Count / ImageStep;
            int currImg = 0;
            for (int iImg = 0; iImg < videoImgCount; iImg++) {
                List<Image<Bgr, Byte>> miniList = new List<Image<Bgr, Byte>>();
                Size newImgSize = new Size();
                for (int jImg = 0; jImg < ImageStep; jImg++) {
                    if (currImg < ImageList.Count) {
                        Image<Bgr, Byte> currentImage = ImageList[currImg].ToImage<Bgr, Byte>();
                        //currentImage = currentImage.Rotate(rotation, new Bgr(0, 0, 0));
                        //Image<Bgr, Byte> rotatedImage;
                        //if (rotation % 90 == 0) {
                        //    if (rotation == 90 || rotation == 270) {
                        //        rotatedImage = new Image<Bgr, byte>(currentImage.Height, currentImage.Width);
                        //    }
                        //    if (rotation == 180) {
                        //        rotatedImage = new Image<Bgr, byte>(currentImage.Width, currentImage.Height);
                        //    }
                        //}
                        //rotatedImage = currentImage.Rotate(rotation, new Bgr(0, 0, 0));
                        //currentImage.Save(@"C:\aaa.tif");
                        miniList.Add(currentImage);
                        if (mergeType == MergeType.Vertical) {
                            newImgSize.Height += currentImage.Height;
                            newImgSize.Width = Math.Max(newImgSize.Width, currentImage.Width);
                        }
                        if (mergeType == MergeType.Horizontal) {
                            newImgSize.Width += currentImage.Width;
                            newImgSize.Height = Math.Max(newImgSize.Width, currentImage.Height);
                        }
                    }
                    currImg++;
                    //Mat image = new Mat(bigImage, new Rectangle(iC * width, iR * height, width, height));
                    //ImageList[currImg].Width
                }
                Image<Bgr, Byte> newImage = new Image<Bgr, byte>(newImgSize);
                int x=0, y=0;
                foreach (Image<Bgr, Byte> miniImg in miniList) {
                    newImage.ROI = new Rectangle(x, y, miniImg.Width, miniImg.Height);
                    miniImg.CopyTo(newImage);
                    newImage.ROI = Rectangle.Empty;
                    if (mergeType == MergeType.Vertical) {
                        y += miniImg.Height;
                    }
                    if (mergeType == MergeType.Horizontal) {
                        x += miniImg.Width;
                    }
                }
                newImage.Save(folderPath + @"\" + iImg.ToString("d4") + ".tif");
            }
            currentResultImage.Save(folderPath + @"\" + "Result.tif");
        }

        public void Play() {

            if (ImageList.Count > ImageStep) {
                play = true;
            }
            checkPlayStatus();
        }

        public void Pause() {

            play = false;
            checkPlayStatus();
        }

        public void Previous() {

            if (ImageList.Count > ImageStep) {
                play = false;
                Thread.Sleep(100);
                imageIdx = currentImageId - ImageStep;
                if (imageIdx < 0) imageIdx = ImageList.Count - ImageStep;
                updateImage();
                refreshDisplayedArea = true;
            }
            checkPlayStatus();
        }

        public void Next() {

            if (ImageList.Count > ImageStep) {
                play = false;
                Thread.Sleep(100);
                imageIdx = (currentImageId + ImageStep) % ImageList.Count;
                updateImage();
                refreshDisplayedArea = true;
            }
            checkPlayStatus();
        }

        private void checkPlayStatus() {

            if (play) {
                rbPlay.Checked = true;
                rbPause.Checked = false;
            }
            else {
                rbPlay.Checked = false;
                rbPause.Checked = true;
            }
            rbPlay.Image = (rbPlay.Checked == true) ? global::ImageViewerGretel.Properties.Resources.media_playback_start_active : global::ImageViewerGretel.Properties.Resources.media_playback_start;
            rbPause.Image = (rbPause.Checked == true) ? global::ImageViewerGretel.Properties.Resources.media_playback_pause_active : global::ImageViewerGretel.Properties.Resources.media_playback_pause;
        }

        private void cutImage(Mat bigImage, int width, int height, CutType cutType) {

            int imagesCount = (bigImage.Cols * bigImage.Rows) / (width * height);
            int rowCount = bigImage.Rows / height;
            int colCount = bigImage.Cols / width;
            switch (cutType) {
                case CutType.None:
                    ImageList.Add(bigImage);
                    break;
                case CutType.TopBottom:
                    for (int iC = 0; iC < colCount; iC++) {
                        for (int iR = 0; iR < rowCount; iR++) {
                            Mat image = new Mat(bigImage, new Rectangle(iC * width, iR * height, width, height));
                            ImageList.Add(image);
                        }
                    }
                    break;
                case CutType.LeftRight:
                    for (int iR = 0; iR < rowCount; iR++) {
                        for (int iC = 0; iC < colCount; iC++) {
                            Mat image = new Mat(bigImage, new Rectangle(iC * width, iR * height, width, height));//(height, width, bigImage.Depth, bigImage.NumberOfChannels);
                            ImageList.Add(image);
                        }
                    }
                    break;
            }
        }

        private void updateImage() {

            try {
                //lock (ImageList) {

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();

                currentImageId = imageIdx;
                currentZoomImage = ImageList[imageIdx];

                refreshImagesFlag = true;
                //}

            }
            catch (Exception ex) {
                Trace.WriteLine("showImage: Error: " + ex.Message);
            }
        }

        private void drawMini() {

            if (refreshDisplayedArea == true) {
                //lock (ImageList) {
                if (currentZoomImage != null) {
                    if (currentMiniImage != null)
                        currentMiniImage.Dispose();
                    currentMiniImage = currentZoomImage.ToImage<Bgr, Byte>();

                    double horizScrollBarRatio = (imageBoxZoom.HorizontalScrollBar.Visible) ? ((double)imageBoxZoom.HorizontalScrollBar.Value / (imageBoxZoom.HorizontalScrollBar.Maximum - imageBoxZoom.HorizontalScrollBar.LargeChange + 1)) : 0;
                    double vertScrollBarRatio = (imageBoxZoom.VerticalScrollBar.Visible) ? ((double)imageBoxZoom.VerticalScrollBar.Value / (imageBoxZoom.VerticalScrollBar.Maximum - imageBoxZoom.VerticalScrollBar.LargeChange + 1)) : 0;
                    int offsetX = (int)(horizScrollBarRatio * (imageBoxZoom.HorizontalScrollBar.Maximum - imageBoxZoom.HorizontalScrollBar.LargeChange + 1));
                    int offsetY = (int)(vertScrollBarRatio * (imageBoxZoom.VerticalScrollBar.Maximum - imageBoxZoom.VerticalScrollBar.LargeChange + 1));
                    if (currentMiniImage != null) {
                        if (currentZoomImage != null) {
                            int width = (int)(Math.Max(0, imageBoxZoom.Width - (imageBoxZoom.VerticalScrollBar.Visible ? imageBoxZoom.VerticalScrollBar.Width : 0)) / _zoomScale);
                            int height = (int)(Math.Max(0, imageBoxZoom.Height - (imageBoxZoom.HorizontalScrollBar.Visible ? imageBoxZoom.HorizontalScrollBar.Height : 0)) / _zoomScale);
                            miniRectangleCorners[0][0].X = offsetX;
                            miniRectangleCorners[0][0].Y = offsetY;
                            miniRectangleCorners[0][1].X = offsetX + width - 1;
                            miniRectangleCorners[0][1].Y = offsetY;
                            miniRectangleCorners[0][2].X = offsetX + width - 1;
                            miniRectangleCorners[0][2].Y = offsetY + height - 1;
                            miniRectangleCorners[0][3].X = offsetX;
                            miniRectangleCorners[0][3].Y = offsetY + height - 1;
                            VectorOfVectorOfPoint miniRectangle = new VectorOfVectorOfPoint(miniRectangleCorners);
                            int rectangleThickness = (int)(1.0 / Math.Min(imageBoxMini.Width / (double)currentMiniImage.Width, imageBoxMini.Height / (double)currentMiniImage.Height));
                            CvInvoke.DrawContours(
                                currentMiniImage,
                                miniRectangle,
                                0,
                                redColor,
                                rectangleThickness,
                                LineType.EightConnected);
                            if (miniRectangle != null)
                                miniRectangle.Dispose();
                        }
                    }
                    refreshImagesFlag = true;
                }
                //}
                refreshDisplayedArea = false;
            }
        }

        void VerticalScrollBar_Scroll(object sender, ScrollEventArgs e) {

            refreshDisplayedArea = true;
        }

        void HorizontalScrollBar_Scroll(object sender, ScrollEventArgs e) {

            refreshDisplayedArea = true;
        }

        #region Menu

        private void rbPlay_CheckedChanged(object sender, EventArgs e) {

            if (rbPlay.Checked)
                Play();
        }

        private void rbPause_CheckedChanged(object sender, EventArgs e) {

            if (rbPause.Checked)
                Pause();
        }

        private void btnPrev_Click(object sender, EventArgs e) {

            Previous();
            refreshDisplayedArea = true;
        }

        private void btnNext_Click(object sender, EventArgs e) {

            Next();
            refreshDisplayedArea = true;
        }

        private void tbRate_Scroll(object sender, EventArgs e) {

            sleepTime = 10000 / tbRate.Value;
            timerRefresh.Interval = 5;// Math.Min(sleepTime, 200);
        }

        private void btnSaveImagesMenu_Click(object sender, EventArgs e) {

            SuspendLayout();
            pnlSaveImagesMenu.Visible = true;
            pnlControlsMenu.Visible = false;
            ResumeLayout(true);
        }

        private void btnSaveSingleImage_Click(object sender, EventArgs e) {

            using (SaveFileDialog saveFileDlg = new SaveFileDialog()) {
                saveFileDlg.CheckPathExists = true;
                saveFileDlg.Filter = "Images (*" + imageExt + ")|*" + imageExt;
                if (saveFileDlg.ShowDialog() == DialogResult.OK) {
                    play = false;
                    Thread.Sleep(100);
                    checkPlayStatus();
                    saveImage(imageIdx, saveFileDlg.FileName);
                }
            }
            btnSaveImagesMenuExit_Click(sender, e);
        }

        private void btnSaveAllImages_Click(object sender, EventArgs e) {

            using (FolderBrowserDialog folderBrowserDlg = new FolderBrowserDialog()) {
                if (folderBrowserDlg.ShowDialog() == DialogResult.OK) {
                    play = false;
                    Thread.Sleep(100);
                    checkPlayStatus();
                    if (Directory.Exists(folderBrowserDlg.SelectedPath) == true) {
                        for (int i = 0; i < ImageList.Count; i++) {
                            saveImage(i, folderBrowserDlg.SelectedPath + @"\" + i.ToString("d4") + imageExt);
                        }
                        // TODO: da verificare come salva Gretel i singoli frame...
                    }
                    else {
                        MessageBox.Show("Selected path does not exist!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Trace.WriteLine("btnSaveAllImages_Click: Selected path does not exist");
                    }
                }
            }
            btnSaveImagesMenuExit_Click(sender, e);
        }

        void saveImage(int id, string path) {

            //lock (ImageList) {
            ImageList[id].Save(path);
            // TODO: al momento salva nella versione standard delle EmguCv...
            //}
        }

        private void btnSaveImagesMenuExit_Click(object sender, EventArgs e) {

            SuspendLayout();
            pnlControlsMenu.Visible = true;
            pnlSaveImagesMenu.Visible = false;
            ResumeLayout(true);
        }

        #endregion


        private void setScrollBarVisibilityAndMaxMin() {

            imageBoxZoom.HorizontalScrollBar.Visible = false;
            imageBoxZoom.VerticalScrollBar.Visible = false;

            if (imageBoxZoom.Image == null) return;

            // If the image is wider than the PictureBox, show the HScrollBar.
            imageBoxZoom.HorizontalScrollBar.Visible =
               (int)(imageBoxZoom.Image.Size.Width * _zoomScale) > imageBoxZoom.Width;

            // If the image is taller than the PictureBox, show the VScrollBar.
            imageBoxZoom.VerticalScrollBar.Visible =
               (int)(imageBoxZoom.Image.Size.Height * _zoomScale) > imageBoxZoom.Height;

            // Set the Maximum, LargeChange and SmallChange properties.
            if (imageBoxZoom.HorizontalScrollBar.Visible) {  // If the offset does not make the Maximum less than zero, set its value.            
                imageBoxZoom.HorizontalScrollBar.Maximum =
                   imageBoxZoom.Image.Size.Width -
                   (int)(Math.Max(0, imageBoxZoom.Width - (imageBoxZoom.VerticalScrollBar.Visible ? imageBoxZoom.VerticalScrollBar.Width : 0)) / _zoomScale);
            }
            else {
                imageBoxZoom.HorizontalScrollBar.Maximum = 0;
            }
            imageBoxZoom.HorizontalScrollBar.LargeChange = (int)Math.Max(imageBoxZoom.HorizontalScrollBar.Maximum / 10, 1);
            imageBoxZoom.HorizontalScrollBar.SmallChange = (int)Math.Max(imageBoxZoom.HorizontalScrollBar.Maximum / 20, 1);
            imageBoxZoom.HorizontalScrollBar.Maximum += imageBoxZoom.HorizontalScrollBar.LargeChange - 1;

            if (imageBoxZoom.VerticalScrollBar.Visible) {  // If the offset does not make the Maximum less than zero, set its value.            
                imageBoxZoom.VerticalScrollBar.Maximum =
                   imageBoxZoom.Image.Size.Height -
                   (int)(Math.Max(0, imageBoxZoom.Height - (imageBoxZoom.HorizontalScrollBar.Visible ? imageBoxZoom.HorizontalScrollBar.Height : 0)) / _zoomScale);
            }
            else {
                imageBoxZoom.VerticalScrollBar.Maximum = 0;
            }
            imageBoxZoom.VerticalScrollBar.LargeChange = (int)Math.Max(imageBoxZoom.VerticalScrollBar.Maximum / 10, 1);
            imageBoxZoom.VerticalScrollBar.SmallChange = (int)Math.Max(imageBoxZoom.VerticalScrollBar.Maximum / 20, 1);
            imageBoxZoom.VerticalScrollBar.Maximum += imageBoxZoom.VerticalScrollBar.LargeChange - 1;

            imageBoxZoom.Refresh();
        }

        private void splitContImage_Panel2_SizeChanged(object sender, EventArgs e) {

            refreshScroolbarsFlag = true;
            refreshDisplayedArea = true;
        }

        private void imageBoxZoom_MouseMove(object sender, MouseEventArgs e) {

            double horizScrollBarRatio = (imageBoxZoom.HorizontalScrollBar.Visible) ? ((double)imageBoxZoom.HorizontalScrollBar.Value / (imageBoxZoom.HorizontalScrollBar.Maximum - imageBoxZoom.HorizontalScrollBar.LargeChange + 1)) : 0;
            double vertScrollBarRatio = (imageBoxZoom.VerticalScrollBar.Visible) ? ((double)imageBoxZoom.VerticalScrollBar.Value / (imageBoxZoom.VerticalScrollBar.Maximum - imageBoxZoom.VerticalScrollBar.LargeChange + 1)) : 0;
            int offsetX = (int)(horizScrollBarRatio * (imageBoxZoom.HorizontalScrollBar.Maximum - imageBoxZoom.HorizontalScrollBar.LargeChange + 1));
            int offsetY = (int)(vertScrollBarRatio * (imageBoxZoom.VerticalScrollBar.Maximum - imageBoxZoom.VerticalScrollBar.LargeChange + 1));
            if (currentZoomImage != null) {
                pixInfo.X = Math.Min(offsetX + e.X, currentZoomImage.Cols - 1);
                pixInfo.Y = Math.Min(offsetY + e.Y, currentZoomImage.Rows - 1);
            }
        }

        private void btnCollapse_Click(object sender, EventArgs e) {

            splitContImage.Panel1Collapsed = !splitContImage.Panel1Collapsed;
            btnCollapse.Image = (splitContImage.Panel1Collapsed == true) ? global::ImageViewerGretel.Properties.Resources.arrow_32 : global::ImageViewerGretel.Properties.Resources.arrow_32back;
        }

        protected override CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED
                return cp;
            }
        }

        private void timerRefresh_Tick(object sender, EventArgs e) {

            lock (ImageList) {
                if (play == true && diagnosticTimer.ElapsedMilliseconds >= sleepTime) {
                    //diagnosticTimer.Stop();
                    //Trace.WriteLine("timerRefresh_Tick: Sample time = " + diagnosticTimer.ElapsedMilliseconds + "ms.");
                    diagnosticTimer.Restart();
                    updateImage();
                    refreshDisplayedArea = true;
                    if (ImageStep > 0 && ImageList.Count > 0)
                        imageIdx = (imageIdx + ImageStep) % ImageList.Count;
                }
                drawMini();
                if (refreshImagesFlag == true) {
                    imageBoxMini.Image = currentMiniImage;
                    imageBoxZoom.Image = currentZoomImage;
                    lblMini.Text = "FRAME #" + currentImageId.ToString("d4");
                    refreshImagesFlag = false;
                }
                if (refreshScroolbarsFlag == true) {
                    setScrollBarVisibilityAndMaxMin();
                    refreshScroolbarsFlag = false;
                }
                if (currentZoomImage != null) {
                    string info = "X=" + pixInfo.X + ", Y=" + pixInfo.Y + ", ";
                    if (currentZoomImage.NumberOfChannels == 3) {
                        //pixInfo.Blue = currentZoomImage.Bytes[3 * (pixInfo.Y * currentZoomImage.Cols + pixInfo.X)];
                        //pixInfo.Green = currentZoomImage.Bytes[3 * (pixInfo.Y * currentZoomImage.Cols + pixInfo.X) + 1];
                        //pixInfo.Red = currentZoomImage.Bytes[3 * (pixInfo.Y * currentZoomImage.Cols + pixInfo.X) + 2];
                        var value = new byte[3];
                        Marshal.Copy(currentZoomImage.DataPointer + (pixInfo.Y * currentZoomImage.Step + pixInfo.X * currentZoomImage.ElementSize), value, 0, 1);
                        Marshal.Copy(currentZoomImage.DataPointer + (pixInfo.Y * currentZoomImage.Step + pixInfo.X * currentZoomImage.ElementSize + 1), value, 1, 1);
                        Marshal.Copy(currentZoomImage.DataPointer + (pixInfo.Y * currentZoomImage.Step + pixInfo.X * currentZoomImage.ElementSize + 2), value, 2, 1);
                        pixInfo.Blue = value[0];
                        pixInfo.Green = value[1];
                        pixInfo.Red = value[2];
                        info += "RGB=" + pixInfo.Red + "," + pixInfo.Green + "," + pixInfo.Blue;
                    }
                    if (currentZoomImage.NumberOfChannels == 1) {
                        //pixInfo.Value = currentZoomImage.Bytes[pixInfo.Y * currentZoomImage.Cols + pixInfo.X];
                        //Matrix<Byte> pixMatrix = new Matrix<Byte>(1, 1, currentZoomImage.NumberOfChannels);//(bigImage, new Rectangle(iC * width, iR * height, width, height));
                        //UMat m2 = new UMat(currentZoomImage, new Rectangle(pixInfo.X, pixInfo.Y, 1, 1));
                        //aaa = currentZoomImage.ToImage<Gray, Byte>();// = new Image<Gray, Byte>(1, 1);
                        //using (UMat m2 = new UMat(currentZoomImage, new Rectangle(pixInfo.X, pixInfo.Y, 3, 3))) {
                        //    aaa = m2.ToImage<Gray, Byte>();
                        //}
                        var value = new byte[1];
                        Marshal.Copy(currentZoomImage.DataPointer + (pixInfo.Y * currentZoomImage.Step + pixInfo.X) * currentZoomImage.ElementSize, value, 0, 1);
                        pixInfo.Value = value[0];
                        info += "VALUE=" + pixInfo.Value;
                        //aaa.Dispose();
                        //m2.Dispose();
                    }
                    lblZoom.Text = info;
                }
            }
        }
        //Image<Gray, Byte> aaa;
    }

    public enum CutType {
        None,
        TopBottom,
        LeftRight
    }

    public enum MergeType {
        None,
        Horizontal,
        Vertical
    }

    public struct PixelInfo {

        public int X;
        public int Y;
        public int Red;
        public int Green;
        public int Blue;
        public int Value;

        public void Reset() {

            X = Y = Red = Green = Blue = Value = 0;
        }
    }
}
