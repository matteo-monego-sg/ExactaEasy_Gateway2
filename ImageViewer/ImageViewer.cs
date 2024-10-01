using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Threading;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;

using SPAMI.Util;
using SPAMI.Util.Logger;
using SPAMI.Util.UI;

using ZedGraph;

namespace SPAMI.Util.EmguImageViewer {
    public partial class ImageViewer : UserControl, ICommon {
        #region Properties
        [Browsable(false)]
        public string ClassName { get; set; }
        [Browsable(true), Category("Fullframe"), Description("TODO")]
        public PictureBoxSizeMode FullframeZoomMode {
            get {
                imageBox1.SizeMode = fullframeZoomMode;
                return fullframeZoomMode;
            }
            set {
                fullframeZoomMode = value;
                imageBox1.SizeMode = value;
            }
        }
        private PictureBoxSizeMode fullframeZoomMode;
        [Browsable(true), Category("Options"), Description("TODO")]
        public bool ZoomStretch { get; set; }
        [Browsable(true), Category("Options"), Description("TODO")]
        public bool ZoomActive { get; set; }
        [Browsable(true), Category("Options"), Description("TODO")]
        public bool ShowAdjacentPixel { get; set; }
        [Browsable(true), Category("Options"), Description("TODO")]
        public bool ShowHorizontalPixel { get; set; }
        [Browsable(true), Category("Options"), Description("TODO")]
        public bool ShowVerticalPixel { get; set; }
        [Browsable(true), Category("Options"), Description("TODO")]
        public bool ShowHistogram { get; set; }
        [Browsable(true), Category("Options"), Description("TODO")]
        public bool ShowBigHistogram { get; set; }
        [Browsable(true), Category("Options"), Description("TODO")]
        public int PixInfoWidth { get; set; }
        [Browsable(true), Category("Options"), Description("TODO")]
        public int PixInfoHeight { get; set; }
        [Browsable(true), Category("Offline"), Description("TODO")]
        public bool Online { get; set; }


        [Browsable(true), Category("Offline"), Description("TODO")]
        public bool ContinuousRecycle { get; set; }
        [Browsable(true), Category("Offline"), Description("TODO")]
        public double Fps {
            get {
                return fps;
            }
            set {
                fps = (value == 0) ? 1 : value;
            }
        }
        private double fps;
        private double OfflineInterImagesTimeMs {
            get {
                return 1000 / Fps;
            }
            set {
                Fps = 1000 / value;
            }
        }
        [Browsable(true), Category("Offline"), Description("TODO")]
        public int QueueCapacity { get; set; }
        /*[Browsable(true), Category("Camera"), Description("TODO")]
        public CameraType CameraModel
        {
            get
            {
                return cameraModel;
            }
            set
            {
                cameraModel = value;
                switch (cameraModel)
                {
                    case CameraType.Tattile:
                        Tattile = new SPAMI.Grabber.Tattile.TattileCmd();
                        Tattile.Location = new System.Drawing.Point(3, 6);
                        Tattile.Name = "TattileCmdBar";
                        Tattile.Size = new System.Drawing.Size(309, 80);
                        Tattile.TabIndex = 0;
                        Tattile.Dock = DockStyle.Fill;
                        groupBoxCameraCmd.Controls.Add(Tattile);
                        break;
                    case CameraType.WebCam:
                        break;
                    default:
                        //Log.Line(LogLevels.Error, ClassName + ".CameraModel.set", "Modello telecamera non contemplato");
                        break;
                }
            }
        }
        private CameraType cameraModel;*/
        #endregion

        public event EventHandler OnTattilePostGetImages;
        public event EventHandler OnTattilePreGetImages;
        const double MaxFps = 100;
        const double minFps = 1;

        public string OfflineRecycleFolderPath { get; set; }
        private FolderBrowserDialog OfflineRecycleFolder = new FolderBrowserDialog();
        public ImageInfo CurrentImage = new ImageInfo();
        public ImageViewerMode ImgViewerMode = ImageViewerMode.Idle;
        private bool pause = false, blink = false, next = false, prev = false;
        private ConcurrentQueue<ImageInfo> ImagesQueue;
        private Thread EnqueuerTh;
        private Thread DequeuerTh;
        private AutoResetEvent ImagesEnqueuedEv = new AutoResetEvent(false);
        private AutoResetEvent ImagesDequeuedEv = new AutoResetEvent(false);
        private ManualResetEvent KillEv = new ManualResetEvent(false);
        private WaitHandle[] EnqueueEvs = new WaitHandle[2];
        private WaitHandle[] DequeueEvs = new WaitHandle[2];
        private List<RadioButton> radioButtonPlayPauseStopGroup = new List<RadioButton>();
        private int OfflineImgIdx = 0;
        private List<FileSystemInfo> FilePathsList;
        private List<KeyValuePair<int, Image<Hsv, Byte>>> RecycleImageList { get; set; }
        private bool forward = true;
        private MatrixForm AdjPixForm;
        private BigHistForm BigHistForm;
        private ZedgraphForm ZedForm;
        //public SPAMI.Grabber.Tattile.TattileCmd Tattile;

        private bool EnqueuerRunning {
            get {
                return !(EnqueuerTh == null || !EnqueuerTh.IsAlive || pause);
            }
        }

        #region Initialization
        public ImageViewer() {
            ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
            InitializeComponent();

            //if (DesignMode) customTabControlMenu.Visible = false;
            //else customTabControlMenu.Visible = true;
            #region Properties default values
            FullframeZoomMode = PictureBoxSizeMode.Zoom;
            ZoomStretch = false;
            ZoomActive = true;
            ShowAdjacentPixel = true;
            ShowHorizontalPixel = false;
            ShowVerticalPixel = false;
            ShowHistogram = false;
            ShowBigHistogram = false;
            PixInfoWidth = 5;
            PixInfoHeight = 5;
            Online = false;
            OfflineRecycleFolderPath = "";
            ContinuousRecycle = true;
            QueueCapacity = 10;
            //CameraModel = CameraType.Offline;
            #endregion
            if (!this.DesignMode) {
                this.Load += new System.EventHandler(this.ImageViewer_Load);
                this.Paint += new System.Windows.Forms.PaintEventHandler(this.ImageViewer_Paint);
                this.imageBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.imageBox1_Paint);
                this.imageBox2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.imageBox2_MouseDown);
                this.buttonFpsDec.Click += new System.EventHandler(this.buttonFpsDec_Click);
                this.buttonFpsInc.Click += new System.EventHandler(this.buttonFpsInc_Click);
                this.buttonFile.Click += new System.EventHandler(this.buttonFile_Click);
                this.ntbFps.TextChanged += new System.EventHandler(this.ntbFps_ValueChanged);
                this.buttonFolder.Click += new System.EventHandler(this.buttonFolder_Click);
                this.checkBoxContinuousRecycle.CheckedChanged += new System.EventHandler(this.checkBoxContinuousRecycle_CheckedChanged);
                this.radioButtonZoomOff.CheckedChanged += new System.EventHandler(this.radioButtonZoomMode_CheckedChanged);
                this.radioButtonZoomOrig.CheckedChanged += new System.EventHandler(this.radioButtonZoomMode_CheckedChanged);
                this.radioButtonZoomStretch.CheckedChanged += new System.EventHandler(this.radioButtonZoomMode_CheckedChanged);
                this.checkBoxVerticalPixel.CheckedChanged += new System.EventHandler(this.checkBoxVerticalPixel_CheckedChanged);
                this.checkBoxHorizontalPixel.CheckedChanged += new System.EventHandler(this.checkBoxHorizontalPixel_CheckedChanged);
                this.checkBoxViewInfoPixel.CheckedChanged += new System.EventHandler(this.checkBoxViewInfoPixel_CheckedChanged);
                this.checkBoxBigHist.CheckedChanged += new System.EventHandler(this.checkBoxBigHist_CheckedChanged);
                this.radioButtonInfo.CheckedChanged += new System.EventHandler(this.radioButtonHistInfo_CheckedChanged);
                this.radioButtonHist.CheckedChanged += new System.EventHandler(this.radioButtonHistInfo_CheckedChanged);
                this.timerBlink.Tick += new System.EventHandler(this.timerBlink_Tick);
                //this.timerZoomInfo.Tick += new System.EventHandler(this.timerZoomInfo_Tick);
                this.radioButtonPlay.CheckedChanged += new System.EventHandler(this.radioButtonPlayPauseStop_CheckedChanged);
                this.radioButtonPause.CheckedChanged += new System.EventHandler(this.radioButtonPlayPauseStop_CheckedChanged);
                this.radioButtonStop.CheckedChanged += new System.EventHandler(this.radioButtonPlayPauseStop_CheckedChanged);
                this.radioButtonPrev.CheckedChanged += new System.EventHandler(this.radioButtonPlayPauseStop_CheckedChanged);
                this.radioButtonNext.CheckedChanged += new System.EventHandler(this.radioButtonPlayPauseStop_CheckedChanged);
                this.radioButtonRec.CheckedChanged += new System.EventHandler(this.radioButtonPlayPauseStop_CheckedChanged);
                this.imageBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.imageBox1_MouseDown);
                this.imageBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.imageBox1_MouseMove);
                this.imageBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(imageBox1_MouseUp);
                this.Disposed += new EventHandler(ImageViewer_Disposed);
                if (customTabControlMenu.TabPages.Contains(tabPageOptions))
                    customTabControlMenu.TabPages.Remove(tabPageOptions);
            }
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            if (!this.DesignMode) {
                //if (Tattile != null) Tattile.OnPreGetImages += new EventHandler(Tattile_OnPreGetImages);
                //if (Tattile != null) Tattile.OnPostGetImages += new Grabber.Tattile.TattileCmd.ImageListEventHandler(Tattile_OnPostGetImages);
                //Multilang.AddMeToMultilanguage(this);
                //Multilang.LanguageChanged += new EventHandler(OnLanguageChanged);
                Par2Form();
                CurrentImage.PixAdjacentValHeight = PixInfoHeight;
                CurrentImage.PixAdjacentValWidth = PixInfoWidth;
                CurrentImage.Init();
                //CurrentImage.ZoomInfoChanged += CurrentImage_ZoomInfoChanged;
                customTabControlMenu.SelectedTab.BackColor = Color.Gainsboro;

                radioButtonPlayPauseStopGroup.Add(radioButtonPlay);
                radioButtonPlayPauseStopGroup.Add(radioButtonPause);
                radioButtonPlayPauseStopGroup.Add(radioButtonStop);
                radioButtonPlayPauseStopGroup.Add(radioButtonPrev);
                radioButtonPlayPauseStopGroup.Add(radioButtonNext);
                radioButtonPlayPauseStopGroup.Add(radioButtonRec);

                ImagesQueue = new ConcurrentQueue<ImageInfo>();
                DequeueEvs[0] = KillEv;
                DequeueEvs[1] = ImagesDequeuedEv;
                EnqueueEvs[0] = KillEv;
                EnqueueEvs[1] = ImagesEnqueuedEv;
                radioButtonStop.Checked = true;
                DequeuerTh = new Thread(new ThreadStart(DequeuerThread));
                DequeuerTh.Name = "Image Viewer Dequeuer Thread";
                DequeuerTh.Start();
                //timerZoomInfo.Start();
                AdjPixForm = new MatrixForm();
                AdjPixForm.Font = new Font("Arial", 8.0F, FontStyle.Bold);
                BigHistForm = new BigHistForm();
                //AdjPixForm.HexDisplay = false;    //pier: aggiungere opzione a video
                AdjPixForm.Init(this, CurrentImage.PixAdjacentValHeight, CurrentImage.PixAdjacentValWidth, 0, 255, 0);
                ZedForm = new ZedgraphForm();
                ZedForm.Init("Line Pixel Values", "Length [pixel]", "Value", Color.Blue, Color.Blue, Color.Blue, false, false, Color.LightGray, Color.LightGray, ZedGraph.LegendPos.Bottom);
                ZedForm.FormClosing += ZedForm_FormClosing;
                //apf.Show(button1.PointToScreen(rbOrig));

                ((Control)imageBox1).AllowDrop = true;
            }
        }

        /*public void OnLanguageChanged(object sender, EventArgs args)
        {
            string sAssName = Multilang.AssemblyNameExtract(System.Reflection.Assembly.GetExecutingAssembly());
            if (Multilang.IsAssemblyStringTableLoaded(sAssName))
            {
                //buttonFile.Text = Multilanguage.GetString(sAssName, "TrovaController");
                this.toolTipCmd.SetToolTip(this.buttonFile, Multilang.GetString(sAssName, "AAA"));
                this.toolTipCmd.SetToolTip(this.buttonFolder, Multilang.GetString(sAssName, "AAA"));
                this.toolTipCmd.SetToolTip(this.checkBoxContinuousRecycle, Multilang.GetString(sAssName, "AAA"));
                this.toolTipCmd.SetToolTip(this.radioButtonPlay, Multilang.GetString(sAssName, "AAA"));
                this.toolTipCmd.SetToolTip(this.radioButtonPause, Multilang.GetString(sAssName, "AAA"));
                this.toolTipCmd.SetToolTip(this.radioButtonStop, Multilang.GetString(sAssName, "AAA"));
                this.toolTipCmd.SetToolTip(this.radioButtonPrev, Multilang.GetString(sAssName, "AAA"));
                this.toolTipCmd.SetToolTip(this.radioButtonNext, Multilang.GetString(sAssName, "AAA"));
                this.toolTipCmd.SetToolTip(this.radioButtonRec, Multilang.GetString(sAssName, "AAA"));
                this.toolTipCmd.SetToolTip(this.radioButtonZoomStretch, Multilang.GetString(sAssName, "AAA"));
                this.toolTipCmd.SetToolTip(this.radioButtonZoomOrig, Multilang.GetString(sAssName, "AAA"));
                this.toolTipCmd.SetToolTip(this.radioButtonZoomOff, Multilang.GetString(sAssName, "AAA"));
                this.toolTipCmd.SetToolTip(this.radioButtonHist, Multilang.GetString(sAssName, "AAA"));
                this.toolTipCmd.SetToolTip(this.radioButtonInfo, Multilang.GetString(sAssName, "AAA"));
                this.toolTipCmd.SetToolTip(this.checkBoxBigHist, Multilang.GetString(sAssName, "AAA"));
                this.toolTipCmd.SetToolTip(this.checkBoxViewInfoPixel, Multilang.GetString(sAssName, "AAA"));
                this.toolTipCmd.SetToolTip(this.checkBoxHexDisplay, Multilang.GetString(sAssName, "AAA"));
                this.toolTipCmd.SetToolTip(this.checkBoxVerticalPixel, Multilang.GetString(sAssName, "AAA"));
                this.toolTipCmd.SetToolTip(this.checkBoxHorizontalPixel, Multilang.GetString(sAssName, "AAA"));
                this.toolTipCmd.SetToolTip(this.tabPageFile, Multilang.GetString(sAssName, "AAA"));
                //this.toolTipCmd.SetToolTip(this.tabPageCamera, Multilang.GetString(sAssName, "AAA"));
                this.toolTipCmd.SetToolTip(this.tabPagePlay, Multilang.GetString(sAssName, "AAA"));
                this.toolTipCmd.SetToolTip(this.tabPageZoom, Multilang.GetString(sAssName, "AAA"));
                this.toolTipCmd.SetToolTip(this.tabPageOptions, Multilang.GetString(sAssName, "AAA"));
            }
        }*/
        #endregion

        void ZedForm_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = true;
        }

        void CurrentImage_ZoomInfoChanged(object sender, EventArgs e) {
            /*if (richTextBoxZoomInfo != null)
            {
                lock (richTextBoxZoomInfo)
                {
                    richTextBoxZoomInfo.Text = CurrentImage.ZoomInfo + "\n" + CurrentImage.PixInfo;
                    richTextBoxZoomInfo.Refresh();
                }
            }*/
        }

        #region Form 2 Par & viceversa
        private void Par2Form() {
            ntbFps.Text = Fps.ToString();
            radioButtonZoomStretch.Checked = ZoomStretch;
            radioButtonZoomOrig.Checked = !ZoomStretch;
            radioButtonZoomOff.Checked = !ZoomActive;
            checkBoxContinuousRecycle.Checked = ContinuousRecycle;
            radioButtonHist.Checked = ShowHistogram;
            radioButtonInfo.Checked = !ShowHistogram;
            checkBoxBigHist.Checked = ShowBigHistogram;
            checkBoxViewInfoPixel.Checked = ShowAdjacentPixel;
            checkBoxHorizontalPixel.Checked = ShowHorizontalPixel;
            checkBoxVerticalPixel.Checked = ShowVerticalPixel;
            Refresh();
        }

        private void Form2Par() {
            Fps = Convert.ToDouble(ntbFps.Text);
            ZoomStretch = radioButtonZoomStretch.Checked;
            ZoomActive = !radioButtonZoomOff.Checked;
            ContinuousRecycle = checkBoxContinuousRecycle.Checked;
            ShowHistogram = radioButtonHist.Checked;
            ShowBigHistogram = checkBoxBigHist.Checked;
            ShowAdjacentPixel = checkBoxViewInfoPixel.Checked;
            //CurrentImage.AdjacentZoomInfo = ShowAdjacentPixel;
            ShowHorizontalPixel = checkBoxHorizontalPixel.Checked;
            //CurrentImage.HorizontalLineInfo = ShowHorizontalPixel;
            ShowVerticalPixel = checkBoxVerticalPixel.Checked;
            //CurrentImage.VerticalLineInfo = ShowVerticalPixel;
        }

        private void checkBoxContinuousRecycle_CheckedChanged(object sender, EventArgs e) {
            ContinuousRecycle = checkBoxContinuousRecycle.Checked;
        }

        private void ntbFps_ValueChanged(object sender, EventArgs e) {
            double fps;
            Double.TryParse(ntbFps.Text, out fps);
            Fps = fps;
        }

        private void checkBoxViewInfoPixel_CheckedChanged(object sender, EventArgs e) {
            ShowAdjacentPixel = checkBoxViewInfoPixel.Checked;
            CurrentImage.AdjacentZoomInfo = ShowAdjacentPixel;
            if (ShowAdjacentPixel) {
                if (!EnqueuerRunning) {
                    ChangeGraphInfo();
                }
                if (AdjPixForm != null && this.Visible)
                    AdjPixForm.Show();
            }
            else {
                if (AdjPixForm != null) AdjPixForm.Hide();
            }
        }

        private void checkBoxHorizontalPixel_CheckedChanged(object sender, EventArgs e) {
            ShowHorizontalPixel = checkBoxHorizontalPixel.Checked;
            if (!ShowHorizontalPixel)
                ZedForm.RemoveChart(HorizontalLegendString);
            HorizVertPixelChanged();
        }

        private void checkBoxVerticalPixel_CheckedChanged(object sender, EventArgs e) {
            ShowVerticalPixel = checkBoxVerticalPixel.Checked;
            if (!ShowVerticalPixel)
                ZedForm.RemoveChart(VerticalLegendString);
            HorizVertPixelChanged();
        }

        private const string SelPixelLegendString = "Sel. pixel";
        private const string HorizontalLegendString = "H line";
        private const string VerticalLegendString = "V line";
        private void HorizVertPixelChanged() {
            CurrentImage.HorizontalLineInfo = ShowHorizontalPixel;
            CurrentImage.VerticalLineInfo = ShowVerticalPixel;
            if (ShowHorizontalPixel) {
                ZedForm.AddChart(HorizontalLegendString, 1, 1, Color.Blue, Color.Blue, ZedGraph.SymbolType.None, true);
                ZedForm.AddChart(SelPixelLegendString, 1, 4, Color.Red, Color.Black, ZedGraph.SymbolType.Circle, false);
            }
            if (ShowVerticalPixel) {
                ZedForm.AddChart(VerticalLegendString, 1, 1, Color.DarkGreen, Color.DarkGreen, ZedGraph.SymbolType.None, true);
                ZedForm.AddChart(SelPixelLegendString, 1, 4, Color.Red, Color.Black, ZedGraph.SymbolType.Circle, false);
            }
            if (ShowHorizontalPixel || ShowVerticalPixel) {
                if (!EnqueuerRunning) {
                    ChangeGraphInfo();
                }
                ZedForm.Show();
            }
            else {
                ZedForm.Hide();
            }
        }
        #endregion

        #region Events Handlers
        private void ImageViewer_Load(object sender, EventArgs e) {
            this.DoubleBuffered = true;
            //propertyGrid1.SelectedObject = pictureBox1;
            //pictureBox1.ImageLocation = @"D:\ExactaEasy\esempi\Device_192.168.12.22_fiala_4_GOOD\Dump_.bmp";
            Log.Line(LogLevels.Debug, ClassName + ".ImageViewer_Load", "ImageViewer loaded.");
        }

        private void ImageViewer_Disposed(object sender, EventArgs args) {
            //timerZoomInfo.Stop();
            KillEv.Set();
            ContinuousRecycle = false;
            if (BigHistForm != null) {
                BigHistForm.Close();
                BigHistForm.Dispose();
            }
            if (DequeuerTh != null && DequeuerTh.IsAlive)
                DequeuerTh.Join();
            if (EnqueuerTh != null && EnqueuerTh.IsAlive)
                EnqueuerTh.Join();
            KillEv.Reset();
            if (AdjPixForm != null) {
                AdjPixForm.Close();
                AdjPixForm.Dispose();
            }
            if (ZedForm != null) {
                ZedForm.Close();
                ZedForm.Dispose();
            }
            base.Dispose();
        }
        #endregion

        public void ChangeGraphInfo() {
            richTextBoxGenInfo.Clear();
            richTextBoxGenInfo.Text = CurrentImage.GenInfo;
            richTextBoxGenInfo.Refresh();
            if (richTextBoxZoomInfo != null) {
                lock (richTextBoxZoomInfo) {
                    richTextBoxZoomInfo.Text = CurrentImage.ZoomInfo + "\n" + CurrentImage.PixInfo;
                    richTextBoxZoomInfo.Refresh();
                }
            }
            if (histogramBoxZoom != null && !histogramBoxZoom.IsDisposed) {
                lock (histogramBoxZoom) {
                    histogramBoxZoom.ClearHistogram();
                    if (CurrentImage.ZoomHist != null) {
                        histogramBoxZoom.AddHistogram("", Color.DarkBlue, CurrentImage.ZoomHist);
                        ((LineItem)histogramBoxZoom.ZedGraphControl.GraphPane.CurveList[0]).Symbol.Type = SymbolType.None;
                    }
                    histogramBoxZoom.Refresh();
                }
            }
            if (ShowBigHistogram) {
                if (CurrentImage.ZoomHist != null) {
                    BigHistForm.Refresh(CurrentImage.ZoomHist);
                    if (!BigHistForm.IsShown && this.Visible) BigHistForm.Show();
                    else BigHistForm.Hide();
                }
            }
            if (AdjPixForm != null) {
                if (this.Visible && ShowAdjacentPixel && LastPixelInfoPointPBRelative != null && LastPixelInfoPointPBRelative.X > -1) {
                    AdjPixForm.Update(ref CurrentImage.PixAdjacentValues);
                    AdjPixForm.Location = imageBox2.PointToScreen(AdjPixForm.GetLocationClient(ref LastPixelInfoPointPBRelative, imageBox2.Width, imageBox2.Height));
                }
                else AdjPixForm.Hide();
            }
            if (ZedForm != null) //pier
            {
                ZedForm.ChangeScale(0, Math.Max(CurrentImage.CurrentROI.Width, CurrentImage.CurrentROI.Height), 0, 255);
                if ((ShowHorizontalPixel || ShowVerticalPixel) && LastPixelInfoPointPBRelative != null && LastPixelInfoPointPBRelative.X > -1) {
                    ZedForm.RemovePoints(SelPixelLegendString);
                    if (ShowHorizontalPixel) {
                        ZedForm.RemovePoints(HorizontalLegendString);
                        ZedForm.AddPoints(HorizontalLegendString, CurrentImage.PixHorizontalLineValuesProg, CurrentImage.PixHorizontalLineValues);
                        ZedForm.AddPoint(SelPixelLegendString, CurrentImage.PixInfoCoord.X, CurrentImage.GetZoomValue(CurrentImage.PixInfoCoord));
                    }
                    if (ShowVerticalPixel) {
                        ZedForm.RemovePoints(VerticalLegendString);
                        ZedForm.AddPoints(VerticalLegendString, CurrentImage.PixVerticalLineValuesProg, CurrentImage.PixVerticalLineValues);
                        ZedForm.AddPoint(SelPixelLegendString, CurrentImage.PixInfoCoord.Y, CurrentImage.GetZoomValue(CurrentImage.PixInfoCoord));
                    }
                    //ZedForm.Location = imageBox2.PointToScreen(AdjPixForm.GetLocationClient(ref LastPixelInfoPointPBRelative, imageBox2.Width, imageBox2.Height));
                    ZedForm.Show();
                }
                else ZedForm.Hide();
            }
        }

        #region Mouse Events
        int ImgOrigClickCounter;
        Point ImgOrigPt1;
        Point ImgOrigPt2;
        RectangleF ImgOrigZoomRectRelative = new RectangleF(0F, 0F, 1F, 1F);
        object Mouse = new object();
        private void imageBox1_MouseDown(object sender, MouseEventArgs e) {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
                if (ImgOrigClickCounter == 0) {
                    ImgOrigPt1 = e.Location;
                    LastPixelInfoPointPBRelative.X = LastPixelInfoPointPBRelative.Y = -1F;
                }
                if (ImgOrigClickCounter == 1) {
                    ImgOrigPt2 = e.Location;
                    ImgOrigZoomRectRelative.X = (float)Math.Min(ImgOrigPt1.X, ImgOrigPt2.X) / (imageBox1.Width - 1);
                    ImgOrigZoomRectRelative.Y = (float)Math.Min(ImgOrigPt1.Y, ImgOrigPt2.Y) / (imageBox1.Height - 1);
                    ImgOrigZoomRectRelative.Width = (float)Math.Abs(ImgOrigPt2.X - ImgOrigPt1.X) / (imageBox1.Width - 1);
                    ImgOrigZoomRectRelative.Height = (float)Math.Abs(ImgOrigPt2.Y - ImgOrigPt1.Y) / (imageBox1.Height - 1);
                }
                ImgOrigClickCounter = (ImgOrigClickCounter + 1) % 2;
                CurrentImage.PixInfoCoord = new Point(-1, -1);
            }
            if ((e.Button & MouseButtons.Right) == MouseButtons.Right) {
                ImgOrigClickCounter = 0;
                ImgOrigZoomRectRelative.X = 0F;
                ImgOrigZoomRectRelative.Y = 0F;
                ImgOrigZoomRectRelative.Width = 1F;
                ImgOrigZoomRectRelative.Height = 1F;
                CurrentImage.PixInfoCoord = new Point(-1, -1);
                LastPixelInfoPointPBRelative.X = LastPixelInfoPointPBRelative.Y = -1F;
            }
            if (!EnqueuerRunning) {
                CurrentImage.Refresh(imageBox1, ImgOrigZoomRectRelative);
                ChangeGraphInfo();
            }
        }

        private void imageBox1_MouseMove(object sender, MouseEventArgs e) {
            if (ImgOrigClickCounter == 1) {
                ImgOrigPt2 = e.Location;
                ImgOrigZoomRectRelative.X = (float)Math.Min(ImgOrigPt1.X, ImgOrigPt2.X) / (imageBox1.Width - 1);
                ImgOrigZoomRectRelative.Y = (float)Math.Min(ImgOrigPt1.Y, ImgOrigPt2.Y) / (imageBox1.Height - 1);
                ImgOrigZoomRectRelative.Width = (float)Math.Abs(ImgOrigPt2.X - ImgOrigPt1.X) / (imageBox1.Width - 1);
                ImgOrigZoomRectRelative.Height = (float)Math.Abs(ImgOrigPt2.Y - ImgOrigPt1.Y) / (imageBox1.Height - 1);
                if (!EnqueuerRunning) {
                    CurrentImage.Refresh(imageBox1, ImgOrigZoomRectRelative);
                    //ChangeGraphInfo();
                }
                LastPixelInfoPointPBRelative.X = LastPixelInfoPointPBRelative.Y = -1F;
            }
        }

        private void imageBox1_MouseUp(object sender, MouseEventArgs e) {
            if (ImgOrigClickCounter == 1) {
                imageBox1_MouseDown(sender, e);
            }
        }

        PointF LastPixelInfoPointPBRelative = new PointF(-1F, -1F);
        private void imageBox2_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == System.Windows.Forms.MouseButtons.Right) {
                if (CurrentImage.MyZoomImage != null) {
                    Point pixXY = Utilities.PicBox2ImgCoords(imageBox2.SizeMode, e.Location, CurrentImage.MyZoomImage.Width, CurrentImage.MyZoomImage.Height, imageBox2.Width, imageBox2.Height);
                    LastPixelInfoPointPBRelative.X = (float)(e.Location.X + 10) / imageBox2.Width;
                    LastPixelInfoPointPBRelative.Y = (float)(e.Location.Y + 10) / imageBox2.Height;
                    CurrentImage.PixInfoCoord = pixXY;
                    if (!EnqueuerRunning) {
                        CurrentImage.Refresh(imageBox1, ImgOrigZoomRectRelative);
                        ChangeGraphInfo();
                    }
                }
            }
        }
        #endregion

        #region Recycle
        private void buttonFile_Click(object sender, EventArgs e) {
            Form2Par();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select an image file...";
            //ofd.Filter
            ofd.InitialDirectory = Directory.GetCurrentDirectory();   //pier: togliere
            if (DialogResult.OK == ofd.ShowDialog()) {
                if (File.Exists(ofd.FileName)) {
                    radioButtonStop.Checked = true;
                    if (EnqueuerTh != null && EnqueuerTh.IsAlive)
                        EnqueuerTh.Abort();
                    CurrentImage.LoadImage(ofd.FileName);
                    UpdateCurrImageFlags();
                    CurrentImage.Refresh(imageBox1, ImgOrigZoomRectRelative);
                    ChangeGraphInfo();
                }
            }
        }

        private void buttonFolder_Click(object sender, EventArgs e) {
            Form2Par();
            OfflineRecycleFolder.Description = "Select an image folder...";
            Stop();
            // @"D:\ExactaEasy\esempi\Device_192.168.12.22_fiala_4_GOOD";   //pier: togliere
            if (DialogResult.OK == OfflineRecycleFolder.ShowDialog()) {
                if (Directory.Exists(OfflineRecycleFolder.SelectedPath)) {
                    ImgViewerMode = ImageViewerMode.FileRecycle;
                    OfflineRecycleFolderPath = OfflineRecycleFolder.SelectedPath;
                    if (radioButtonPlay.Checked == true) Play();
                    else radioButtonPlay.Checked = true;
                }
            }
        }
        #endregion

        //System.Diagnostics.Stopwatch MyStopWatch = new System.Diagnostics.Stopwatch();
        private void EnqueuerOfflineFileThread(object path) {
            string Path = (string)path;
            OfflineImgIdx = 0;
            //List<string> Filepaths = new List<string>();
            DirectoryInfo di = new DirectoryInfo(Path);
            FileSystemInfo[] files = di.GetFileSystemInfos();
            var orderedFiles = files.OrderBy(f => f.FullName);      //ordina per creation time, fullname, ...
            FilePathsList = orderedFiles.ToList();
            int limit = (QueueCapacity > 0) ? Math.Min(QueueCapacity, FilePathsList.Count) : FilePathsList.Count;
            bool finished = false;
            if (FilePathsList.Count == 0) finished = true;
            ImageInfo EnqueueImg = new ImageInfo();
            EnqueueImg.PixAdjacentValHeight = PixInfoHeight;
            EnqueueImg.PixAdjacentValWidth = PixInfoWidth;
            EnqueueImg.Init();
            //MyStopWatch.Reset();
            //MyStopWatch.Start();
            int counter = 0;
            while (!finished) {
                if (ImagesQueue.Count >= limit) {
                    int ev = WaitHandle.WaitAny(DequeueEvs, (int)(OfflineInterImagesTimeMs + 0.5));
                    if (ev == 0) break;     //kill event
                }
                else {
                    if (KillEv.WaitOne((int)(OfflineInterImagesTimeMs + 0.5))) break;   //kill event
                }
                if (!pause || (pause && next) || (pause && prev)) {
                    lock (ImagesQueue) {
                        EnqueueImg.LoadImage(FilePathsList[OfflineImgIdx].FullName);
                        ImagesQueue.Enqueue(EnqueueImg);
                    }
                    counter++;
                    ImagesEnqueuedEv.Set();
                    if (!prev) {
                        OfflineImgIdx = (OfflineImgIdx + 1) % FilePathsList.Count;
                        forward = true;
                    }
                    else {
                        OfflineImgIdx = (OfflineImgIdx > 0) ? (OfflineImgIdx - 1) : FilePathsList.Count - 1;
                        forward = false;
                    }
                    if (((!ContinuousRecycle) && (OfflineImgIdx == 0)) ||
                        (FilePathsList.Count == 1))
                        finished = true;
                    if (next || prev) {
                        if (radioButtonPause.InvokeRequired)
                            radioButtonPause.Invoke(new MethodInvoker(Pause));
                        else
                            radioButtonPause.Checked = true;
                        next = false;
                        prev = false;
                    }
                }
            }
            ImgViewerMode = ImageViewerMode.Idle;
            //MyStopWatch.Stop();
            //Log.Line(LogLevels.Debug, ClassName + ".EnqueuerThread", "Thread di push su coda terminato. Framerate medio: {0}", (double)(counter) * 1000 / MyStopWatch.ElapsedMilliseconds);
        }

        private void Pause() {
            radioButtonPause.Checked = true;
        }

        private void UpdateCurrImageFlags() {
            CurrentImage.AdjacentZoomInfo = ShowAdjacentPixel;
            CurrentImage.HorizontalLineInfo = ShowHorizontalPixel;
            CurrentImage.VerticalLineInfo = ShowVerticalPixel;
        }

        //System.Diagnostics.Stopwatch MyStopWatch = new System.Diagnostics.Stopwatch();
        private void EnqueuerOfflineListThread(object list) {
            List<KeyValuePair<int, Image<Hsv, Byte>>> List = (List<KeyValuePair<int, Image<Hsv, Byte>>>)list;
            OfflineImgIdx = 0;
            int limit = (QueueCapacity > 0) ? Math.Min(QueueCapacity, List.Count) : List.Count;
            bool finished = false;
            ImageInfo EnqueueImg = new ImageInfo();
            EnqueueImg.PixAdjacentValHeight = PixInfoHeight;
            EnqueueImg.PixAdjacentValWidth = PixInfoWidth;
            EnqueueImg.Init();
            //MyStopWatch.Reset();
            //MyStopWatch.Start();
            int counter = 0;
            while (!finished) {
                if (ImagesQueue.Count >= limit) {
                    int ev = WaitHandle.WaitAny(DequeueEvs, (int)(OfflineInterImagesTimeMs + 0.5));
                    if (ev == 0) break;     //kill event
                }
                else {
                    if (KillEv.WaitOne((int)(OfflineInterImagesTimeMs + 0.5))) break;   //kill event
                }
                if (!pause || (pause && next) || (pause && prev)) {
                    lock (ImagesQueue) {
                        EnqueueImg.PushImage(List[OfflineImgIdx]);
                        ImagesQueue.Enqueue(EnqueueImg);
                    }
                    counter++;
                    ImagesEnqueuedEv.Set();
                    if (!prev) {
                        OfflineImgIdx = (OfflineImgIdx + 1) % List.Count;
                        forward = true;
                    }
                    else {
                        OfflineImgIdx = (OfflineImgIdx > 0) ? (OfflineImgIdx - 1) : List.Count - 1;
                        forward = false;
                    }
                    if (((!ContinuousRecycle) && (OfflineImgIdx == 0)) ||
                        (List.Count == 1))
                        finished = true;
                    if (next || prev) {
                        if (radioButtonPause.InvokeRequired)
                            radioButtonPause.Invoke(new MethodInvoker(Pause));
                        else
                            radioButtonPause.Checked = true;
                        next = false;
                        prev = false;
                    }
                }
            }
            ImgViewerMode = ImageViewerMode.Idle;
            RecycleImageList = null;
            //MyStopWatch.Stop();
            //Log.Line(LogLevels.Debug, ClassName + ".EnqueuerThread", "Thread di push su coda terminato. Framerate medio: {0}", (double)(counter) * 1000 / MyStopWatch.ElapsedMilliseconds);
        }

        private void DequeuerThread() {
            while (true) {
                int ev = WaitHandle.WaitAny(EnqueueEvs);
                if (ev == 0) break;     //kill event
                //ImageInfo DequeueImg;
                lock (ImagesQueue)  //pier: dovrei cercare di toglierlo
                {
                    if (ImagesQueue.TryDequeue(out CurrentImage)) {
                        try {
                            UpdateCurrImageFlags();
                            CurrentImage.Refresh(imageBox1, ImgOrigZoomRectRelative);
                            if (richTextBoxGenInfo.InvokeRequired)
                                richTextBoxGenInfo.BeginInvoke(new MethodInvoker(ChangeGraphInfo));
                            else
                                ChangeGraphInfo();
                            ImagesDequeuedEv.Set();
                        }
                        catch (Exception ex) {
                            Log.Line(LogLevels.Debug, ClassName + ".DequeuerThread", "Eccezione: " + ex.ToString());
                        }
                    }
                }
            }
            Log.Line(LogLevels.Debug, ClassName + ".DequeuerThread", "Thread di pop da coda terminato.");
        }

        #region Tattile
        private void Tattile_OnPreGetImages(object sender, EventArgs args) {
            Stop();
            if (OnTattilePreGetImages != null) OnTattilePreGetImages(this, EventArgs.Empty);
        }

        /*private void Tattile_OnPostGetImages(object sender, SPAMI.Grabber.Tattile.ImageListEventArgs args)
        {
            if (args.ImageList != null && args.ImageList.Count > 0)
            {
                StartListRecycle(args);
            }
            if (OnTattilePostGetImages != null) OnTattilePostGetImages(this, EventArgs.Empty);
        }*/

        /*private void StartListRecycle(SPAMI.Grabber.Tattile.ImageListEventArgs args)
        {
            if (EnqueuerTh != null && EnqueuerTh.IsAlive)
                EnqueuerTh.Abort();
            RecycleImageList = args.ImageList;
            ImgViewerMode = ImageViewerMode.ListRecycle;
            if (radioButtonPlay.Checked == true) Play();
            else radioButtonPlay.Checked = true;
        }*/
        #endregion


        #region Play/Pause/Stop
        private void radioButtonPlayPauseStop_CheckedChanged(object sender, EventArgs e) {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked) {
                foreach (RadioButton other in radioButtonPlayPauseStopGroup) {
                    if (other == rb) {
                        continue;
                    }
                    other.Checked = false;
                    other.Enabled = true;
                }
                PlayPauseStopPressed(sender, e);
            }
        }

        private void PlayPauseStopPressed(object sender, EventArgs e) {
            if (radioButtonPlay.Checked) {
                switch (ImgViewerMode) {
                    /*case ImageViewerMode.ListRecycle:
                        if (pause) pause = false;
                        else (RecycleImageList != null && RecycleImageList.Count>0) Play();
                        else Tattile.GetImages();
                        break;*/
                    case ImageViewerMode.FileRecycle:
                    default:
                        if (pause) pause = false;
                        else if (Directory.Exists(OfflineRecycleFolderPath)) Play();
                        else buttonFolder_Click(sender, e);
                        break;
                }
            }
            else if (radioButtonPause.Checked) {
                if (EnqueuerTh != null && EnqueuerTh.IsAlive) {
                    radioButtonPause.Enabled = false;
                    pause = true;
                }
            }
            else if (radioButtonStop.Checked) {
                Stop();
            }
            else if (radioButtonPrev.Checked) {
                switch (ImgViewerMode) {
                    case ImageViewerMode.ListRecycle:
                        if (RecycleImageList != null && RecycleImageList.Count > 0) {
                            if (forward) {
                                //decremento 2 volte
                                OfflineImgIdx = (OfflineImgIdx > 0) ? (OfflineImgIdx - 1) : RecycleImageList.Count - 1;
                                OfflineImgIdx = (OfflineImgIdx > 0) ? (OfflineImgIdx - 1) : RecycleImageList.Count - 1;
                            }
                            pause = true;
                            prev = true;
                        }
                        break;
                    case ImageViewerMode.FileRecycle:
                    default:
                        if (FilePathsList != null && FilePathsList.Count > 0) {
                            if (forward) {
                                //decremento 2 volte
                                OfflineImgIdx = (OfflineImgIdx > 0) ? (OfflineImgIdx - 1) : FilePathsList.Count - 1;
                                OfflineImgIdx = (OfflineImgIdx > 0) ? (OfflineImgIdx - 1) : FilePathsList.Count - 1;
                            }
                            pause = true;
                            prev = true;
                        }
                        break;
                }
            }
            else if (radioButtonNext.Checked) {
                switch (ImgViewerMode) {
                    case ImageViewerMode.ListRecycle:
                        if (RecycleImageList != null && RecycleImageList.Count > 0) {
                            if (!forward) {
                                //incremento 2 volte
                                OfflineImgIdx = (OfflineImgIdx + 1) % RecycleImageList.Count;
                                OfflineImgIdx = (OfflineImgIdx + 1) % RecycleImageList.Count;
                            }
                        }
                        pause = true;
                        next = true;
                        break;
                    case ImageViewerMode.FileRecycle:
                    default:
                        if (FilePathsList != null && FilePathsList.Count > 0) {
                            if (!forward) {
                                //incremento 2 volte
                                OfflineImgIdx = (OfflineImgIdx + 1) % FilePathsList.Count;
                                OfflineImgIdx = (OfflineImgIdx + 1) % FilePathsList.Count;
                            }
                        }
                        pause = true;
                        next = true;
                        break;
                }
            }
            else if (radioButtonRec.Checked) {
                //radioButtonRec.Enabled = false;
                timerBlink.Start();
                //TODO
            }
        }

        private void Play() {
            radioButtonPlay.Enabled = false;
            pause = false;
            if (EnqueuerTh != null && EnqueuerTh.IsAlive)
                EnqueuerTh.Abort();
            int counter = 0;
            switch (ImgViewerMode) {
                case ImageViewerMode.FileRecycle:
                    EnqueuerTh = new Thread(new ParameterizedThreadStart(EnqueuerOfflineFileThread));
                    EnqueuerTh.Name = "Image Viewer File Enqueuer Thread";
                    EnqueuerTh.Start(OfflineRecycleFolderPath);
                    if (FilePathsList != null && FilePathsList.Count > 0) {
                        counter = FilePathsList.Count;
                    }
                    break;
                case ImageViewerMode.ListRecycle:
                    EnqueuerTh = new Thread(new ParameterizedThreadStart(EnqueuerOfflineListThread));
                    EnqueuerTh.Name = "Image Viewer List Enqueuer Thread";
                    EnqueuerTh.Start(RecycleImageList);
                    counter = RecycleImageList.Count;
                    break;
                default:
                    Log.Line(LogLevels.Error, ClassName + ".Play", "Modalità non contemplata");
                    break;
            }

            if (!forward) {
                //incremento 2 volte
                OfflineImgIdx = (OfflineImgIdx + 1) % counter;
                OfflineImgIdx = (OfflineImgIdx + 1) % counter;
            }
        }

        private void Stop() {
            radioButtonStop.Enabled = false;
            if (EnqueuerTh != null && EnqueuerTh.IsAlive)
                EnqueuerTh.Abort();
            timerBlink.Stop();
            this.radioButtonRec.Image = global::SPAMI.Util.EmguImageViewer.Properties.Resources.mediaRecord;
            pause = false;
        }
        #endregion

        private void timerBlink_Tick(object sender, EventArgs e) {
            blink = !blink;
            this.radioButtonRec.Image = (blink == true) ? global::SPAMI.Util.EmguImageViewer.Properties.Resources.mediaRecord : global::SPAMI.Util.EmguImageViewer.Properties.Resources.mediaRecordGray;
        }

        private void imageBox1_Paint(object sender, PaintEventArgs e) {
            if (!this.DesignMode && CurrentImage != null && ZoomActive)
                CurrentImage.RefreshZoom(imageBox2/*, ImgOrigZoomRect*/);
            if (!DesignMode && !EnqueuerRunning)
                ChangeGraphInfo();
        }

        private void radioButtonZoomMode_CheckedChanged(object sender, EventArgs e) {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked) {
                ZoomActive = !radioButtonZoomOff.Checked;
                ZoomStretch = radioButtonZoomStretch.Checked;
                if (ZoomStretch) {
                    if (!this.DesignMode && CurrentImage != null && ZoomActive)
                        CurrentImage.RefreshZoom(imageBox2/*, ImgOrigZoomRect*/);
                    imageBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                else if (!ZoomActive) {
                    imageBox2.Image = null;
                }
                else {
                    if (!this.DesignMode && CurrentImage != null && ZoomActive)
                        CurrentImage.RefreshZoom(imageBox2/*, ImgOrigZoomRect*/);
                    imageBox2.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
        }

        //private void timerZoomInfo_Tick(object sender, EventArgs e)
        //{
        //if (!EnqueuerRunning)
        //    ChangeGraphInfo();
        //}

        #region Histogram
        private System.Windows.Forms.RichTextBox richTextBoxZoomInfo;
        private Emgu.CV.UI.HistogramBox histogramBoxZoom;
        //private Thread BigHistTh;
        private void ShowHist() {
            if (BigHistForm != null && BigHistForm.IsShown)
                BigHistForm.Hide();
            //SuspendLayout();
            if (ShowHistogram) {
                if (richTextBoxZoomInfo != null) {
                    lock (richTextBoxZoomInfo) {
                        richTextBoxZoomInfo.Dispose();
                        this.tableLayoutPanel3.Controls.Remove(this.richTextBoxZoomInfo);
                    }
                }
                this.histogramBoxZoom = new Emgu.CV.UI.HistogramBox();
                if (histogramBoxZoom != null) {
                    lock (histogramBoxZoom) {
                        this.tableLayoutPanel3.Controls.Add(this.histogramBoxZoom, 1, 0);
                        this.histogramBoxZoom.Location = new System.Drawing.Point(181, 3);
                        this.histogramBoxZoom.Name = "histogramBox1";
                        this.histogramBoxZoom.Size = new System.Drawing.Size(270, 105);
                        this.histogramBoxZoom.TabIndex = 1;
                        this.histogramBoxZoom.Dock = DockStyle.Fill;
                    }
                }
            }
            else {
                if (histogramBoxZoom != null) {
                    lock (histogramBoxZoom) {
                        histogramBoxZoom.Dispose();
                        this.tableLayoutPanel3.Controls.Remove(this.histogramBoxZoom);
                    }
                }
                if (richTextBoxZoomInfo != null) {
                    lock (richTextBoxZoomInfo) {
                        richTextBoxZoomInfo.Dispose();
                        this.tableLayoutPanel3.Controls.Remove(this.richTextBoxZoomInfo);
                    }
                }
                this.richTextBoxZoomInfo = new System.Windows.Forms.RichTextBox();
                if (richTextBoxZoomInfo != null) {
                    lock (richTextBoxZoomInfo) {
                        this.richTextBoxZoomInfo.BackColor = System.Drawing.SystemColors.Info;
                        this.richTextBoxZoomInfo.Dock = System.Windows.Forms.DockStyle.Fill;
                        this.richTextBoxZoomInfo.Location = new System.Drawing.Point(181, 3);
                        this.richTextBoxZoomInfo.ReadOnly = true;
                        this.richTextBoxZoomInfo.Name = "richTextBoxZoomInfo";
                        this.richTextBoxZoomInfo.Size = new System.Drawing.Size(320, 105);
                        this.richTextBoxZoomInfo.TabIndex = 1;
                        this.richTextBoxZoomInfo.Text = "";
                        this.richTextBoxZoomInfo.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        this.richTextBoxZoomInfo.WordWrap = false;
                        this.tableLayoutPanel3.Controls.Add(this.richTextBoxZoomInfo, 1, 0);
                    }
                }
            }
            //ResumeLayout();
        }

        private void checkBoxBigHist_CheckedChanged(object sender, EventArgs e) {
            ShowBigHistogram = checkBoxBigHist.Checked;
            if (ShowBigHistogram) {
                Point rbOrig = new Point(0, 0);
                Point rbScreenOrig = checkBoxBigHist.PointToScreen(rbOrig);
                if (BigHistForm != null) {
                    BigHistForm.Width = Math.Min(this.ParentForm.Location.X + this.ParentForm.Width - rbScreenOrig.X, 1000);
                    BigHistForm.Height = Math.Min(rbScreenOrig.Y - this.ParentForm.Location.Y, 1000);
                    BigHistForm.StartPosition = FormStartPosition.Manual;
                    BigHistForm.Location = new Point(rbScreenOrig.X, rbScreenOrig.Y - BigHistForm.Height);
                    BigHistForm.Show();
                }
                if (!EnqueuerRunning) {
                    ChangeGraphInfo();
                }
            }
            else {
                if (BigHistForm != null)
                    BigHistForm.Hide();
            }
        }

        private void radioButtonHistInfo_CheckedChanged(object sender, EventArgs e) {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked) {
                ShowHistogram = radioButtonHist.Checked;
                ShowHist();
                if (!EnqueuerRunning) {
                    ChangeGraphInfo();
                }
                if (radioButtonInfo.Checked) richTextBoxZoomInfo.Refresh();

            }
        }
        #endregion

        private delegate void CheckRBDel(RadioButton rb, bool value);
        private void CheckRB(RadioButton rb, bool value) {
            rb.Checked = value;
        }

        private void buttonFpsInc_Click(object sender, EventArgs e) {
            //try
            //{
            Fps = Math.Min(Fps + 1, MaxFps);
            ntbFps.Text = Fps.ToString();
            //}
            //catch (Exception ex)
            //{
            //    Log.Line(LogLevels.Warning, ClassName + ".buttonFpsInc_Click", "Sforato valore massimo: {0} ({1})", (double)ntbFps.Value, (double)ntbFps.Maximum);
            //    Log.Line(LogLevels.Debug, ClassName + ".buttonFpsInc_Click", ex.ToString());
            //    ntbFps.Value = ntbFps.Maximum;
            //}
        }

        private void buttonFpsDec_Click(object sender, EventArgs e) {
            //try
            //{
            Fps = Math.Max(Fps - 1, minFps);
            ntbFps.Text = Fps.ToString();
            //}
            //catch (Exception ex)
            //{
            //    Log.Line(LogLevels.Warning, ClassName + ".buttonFpsDec_Click", "Sforato valore minimo: {0} ({1})", (double)ntbFps.Value, (double)ntbFps.Minimum);
            //    Log.Line(LogLevels.Debug, ClassName + ".buttonFpsDec_Click", ex.ToString());
            //    ntbFps.Value = ntbFps.Minimum;
            //}
        }

        private void ImageViewer_Paint(object sender, PaintEventArgs e) {
            if (!DesignMode) {
                //if (Tattile != null)
                //    Tattile.Refresh();
                if (AdjPixForm != null && AdjPixForm.IsShown)
                    imageBox2.PointToScreen(AdjPixForm.GetLocationClient(ref LastPixelInfoPointPBRelative, imageBox2.Width, imageBox2.Height));
            }
        }

        private void checkBoxHexDisplay_CheckedChanged(object sender, EventArgs e) {
            if (AdjPixForm != null) {
                AdjPixForm.HexDisplay = checkBoxHexDisplay.Checked;
                AdjPixForm.Update(ref CurrentImage.PixAdjacentValues);
            }
        }

        public void HideAll() {
            Pause();
            //ShowBigHistogram = false;
            //if (AdjPixForm != null) AdjPixForm.Hide();
        }

        private void ImageViewer_VisibleChanged(object sender, EventArgs e) {
            if (this.Visible == false)
                HideAll();
        }

        private void ImageViewer_DragDrop(object sender, DragEventArgs e) {
            MessageBox.Show("ImageViewer_DragDrop!", "Evento!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tableLayoutPanel2_DragDrop(object sender, DragEventArgs e) {
            MessageBox.Show("tableLayoutPanel2_DragDrop!", "Evento!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tableLayoutPanel1_DragDrop(object sender, DragEventArgs e) {
            MessageBox.Show("tableLayoutPanel2_DragDrop!", "Evento!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void imageBox1_DragDrop(object sender, DragEventArgs e) {
            MessageBox.Show("imageBox1_DragDrop!", "Evento!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /*private delegate void AddHistBoxToFormDel(Form form, HistogramBox hist);
        private void AddHistBoxToForm(Form form, HistogramBox hist)
        {
            form.Controls.Add(hist); l
        }*/
    }

    public enum CameraType {
        Offline,
        WebCam,
        Tattile
    }

    public enum ImageViewerMode {
        Idle,
        FileRecycle,
        ListRecycle
    }
}
