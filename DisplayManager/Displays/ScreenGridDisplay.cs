using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using ExactaEasyCore;
using ExactaEasyEng.Utilities;
using SPAMI.Util.Logger;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DisplayManager
{
    public class ScreenGridDisplay : Display
    {
        protected IntPtr WindowHandle { get; set; }
        protected ImageBox ImageCtrl { get; set; }
        //readonly Dictionary<int, Image<Rgb, byte>> _imageBuffer;
        readonly protected object _syncImgObj = new object();
        Font _headerFont = new Font("Arial", 16);
        readonly SolidBrush _headerBrush = new SolidBrush(Color.Navy);
        readonly SolidBrush _blackBrush = new SolidBrush(Color.Black);
        protected Rgb redColor = new Rgb(255, 0, 0);
        protected Rgb greenColor = new Rgb(0, 255, 0);
        protected Rgb orangeColor = new Rgb(255, 102, 0);
        protected Rgb fucsiaColor = new Rgb(255, 0, 255);
        protected Rgb blackColor = new Rgb(0, 0, 0);
        protected MCvFont font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 0.75, 0.75);

        public int RowsCount { get; private set; }
        public int ColumnsCount { get; private set; }

        public int CurrentPage { get; set; }
        public int PageCount { get { return _maxPages; } }
        readonly int _maxPages;

        protected readonly bool _keepAspectRatio;
        Dictionary<int, ImagePositionInfo> imageLocation;
        protected Image<Rgb, byte> bufferBm;

        static object syncMainBm = new object();
        //bool dirty = true;
        System.Windows.Forms.Timer timerInvalidate = new System.Windows.Forms.Timer();

        const int headerOffsetX = 20;
        const int headerOffsetY = 10;
        int headerHeight = 0;
        bool _showColoredBorders;
        //Image<Rgb, byte> _newImage;
        StationCollection _sourceStations;
        List<ScreenGridDisplaySettings> _displaySettings;
        List<CameraSetting> _cameraSettings;

        public ScreenGridDisplay(
            string name,
            ImageBox imageCtrl,
            StationCollection sourceStations,
            List<ScreenGridDisplaySettings> displaySettings,
            List<CameraSetting> cameraSettings,
            bool keepAspectRatio,
            int interpolationType,
            bool showColoredBorders) : base(name, sourceStations)
        {

            ImageCtrl = imageCtrl;
            bufferBm = new Image<Rgb, byte>(ImageCtrl.Width, ImageCtrl.Height);
            _sourceStations = sourceStations;
            InterpolationType = (Emgu.CV.CvEnum.INTER)interpolationType;
            _cameraSettings = cameraSettings;
            _displaySettings = displaySettings;
            //RowsCount = rowsCount;
            //ColumnsCount = columnsCount;
            CurrentPage = 0;
            //_camPerPage = rowsCount * columnsCount;
            _keepAspectRatio = keepAspectRatio;
            //maxPages = sourceStations.Count;
            //foreach (IStation stat in sourceStations)
            //    maxPages = Math.Max(maxPages, stat.DisplayPosition / camPerPage + 1);
            _maxPages = _displaySettings.Count;
            _showColoredBorders = showColoredBorders;
            //_imageBuffer = new Dictionary<int, Image<Rgb, byte>>();
            //ImageCtrl.Paint += new PaintEventHandler(ImageCtrl_Paint);
            DisplayAOIUsed = new Dictionary<int, Rectangle>();
            createImgLoc();
            drawHeader();

            timerInvalidate.Interval = 20;
            timerInvalidate.Tick += timerInvalidate_Tick;
            timerInvalidate.Start();
            newImageToShow = true;
        }

        public override void Dispose()
        {

            //ImageCtrl = null;
            //timerInvalidate.Stop();
            //timerInvalidate.Dispose();
            timerInvalidate.Stop();
            if (bufferBm != null)
                bufferBm.Dispose();
            imageLocation.Clear();
            base.Dispose();
        }

        public override void Resize(int w, int h)
        {
            //Trace.WriteLine("RESIZE REQUEST. W=" + w + " H=" + h);
            if (bufferBm != null && ImageCtrl != null && w > 0 && h > 0)
            {
                bool prevSuspend = displaySuspended;
                Suspend();
                lock (syncImage)
                {
                    //Debug.WriteLine("INIZIO RESIZE");
                    lock (_syncImgObj)
                    {
                        ImageCtrl.Image = null;
                        ImageCtrl.Width = w;
                        ImageCtrl.Height = h;
                        bufferBm.Dispose();
                        bufferBm = new Image<Rgb, byte>(w, h);
                        //Trace.WriteLine("NEW BUFFER BM. W=" + w + " H=" + h);
                        createImgLoc();
                        ClearBackground();
                    }
                    //Debug.WriteLine("FINE RESIZE");
                }
                if (prevSuspend == false)
                    Resume();
            }
        }

        void timerInvalidate_Tick(object sender, EventArgs e)
        {

            lock (_syncImgObj)
            {
                if (newImageToShow && bufferBm != null && bufferBm.Width > 0 && bufferBm.Height > 0)
                {
                    ImageCtrl.Image = bufferBm;
                    newImageToShow = false;
                }
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

        public override void RenderImage(IStation currentStation)
        {
            if (currentStation.CurrentImage == null || !currentStation.Enabled)
                return;
            //int camIdx = currentStation.DisplayPosition;
            CameraSetting camSetting = _cameraSettings.Find(cs => cs.Node == currentStation.NodeId && cs.Station == currentStation.IdStation);
            if (imageLocation == null || !imageLocation.ContainsKey(camSetting.Id) || imageLocation[camSetting.Id].PageNumber != CurrentPage)
                return;

            RectangleF rect = imageLocation[camSetting.Id].ImagePosition;
            Image<Rgb, byte> _newImage = null;
            Rectangle displayAOIUsed;
            try
            {
                lock (currentStation.CurrentImage)
                {
                    currentStation.AdjustImage(currentStation.CurrentImage, (int)rect.Width, (int)rect.Height, _keepAspectRatio, InterpolationType, ref _newImage, out displayAOIUsed);
                    ImageShown = currentStation.CurrentImage;
                }
                if (_newImage != null)
                {
                    lock (_syncImgObj)
                    {
                        bufferBm.ROI = new Rectangle((int)rect.X, (int)rect.Y + headerHeight, (int)rect.Width, (int)rect.Height);
                        bufferBm.SetZero();
                        //DA CONFRONTARE CON MASTER
                        bufferBm.ROI = new Rectangle(Convert.ToInt32(rect.Left + (rect.Width - _newImage.Width) / 2.0F), Convert.ToInt32(rect.Top + (rect.Height - _newImage.Height) / 2.0F) + headerHeight, _newImage.Width, _newImage.Height);
                        //Debug.WriteLine("RENDER COPY");
                        CvInvoke.cvCopy(_newImage, bufferBm, IntPtr.Zero);

                        if (currentStation.CurrentIsReject.HasValue)
                        {
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
                            if (_showColoredBorders) bufferBm.Draw(new Rectangle(0, 0, bufferBm.ROI.Width, bufferBm.ROI.Height), color, 2);
                        }
                        bufferBm.ROI = Rectangle.Empty;
                        _newImage.Dispose();
                        newImageToShow = true;
                    }
                }
            }
            catch (OutOfMemoryException ex)
            {
                Log.Line(LogLevels.Error, "Display.RenderImage", "OutOfMemoryException: " + ex.Message);
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Warning, "Display.RenderImage", "Warning: " + ex.Message);
            }
        }

        void createImgLoc()
        {
            imageLocation = new Dictionary<int, ImagePositionInfo>();
            int width = ImageCtrl.Width;
            int height = ImageCtrl.Height;

            headerHeight = 2 * headerOffsetY + _headerFont.Height;
            //const int headerOffsetX = 20;
            //const int headerOffsetY = 10;
            //int headerHeight = 2 * headerOffsetY + 20;// _headerFont.Height;

            foreach (IStation s in _sourceStations)
            {

                if (s == null) continue;
                //int colSpan = 1;

                // TODO: Gestione ColSpan PROVVISORIA! Da sistemare
                //if (s.Cameras != null && s.Cameras.First().GetType().Name.Contains("M12"))
                //    colSpan = ColumnsCount;

                //int camIdx = s.DisplayPosition;
                CameraSetting camSetting = _cameraSettings.Find(cs => cs.Node == s.NodeId && cs.Station == s.IdStation);
                ScreenGridDisplaySettings dispSettings = _displaySettings[camSetting.PageNumberPosition];
                if (camSetting.DisplayPositionCol > dispSettings.Cols - 1 || camSetting.DisplayPositionRow > dispSettings.Rows - 1)
                {
                    Log.Line(LogLevels.Error, "ScreenGridDisplay.createImgLoc", "Display configuration error. Position out of bounds of this view.");
                    continue;
                }
                //int pageNo = Convert.ToInt32(Math.Truncate((decimal)camIdx / (ColumnsCount * RowsCount)));
                int pageNo = camSetting.PageNumberPosition;

                ImagePositionInfo imagePos = new ImagePositionInfo() { RowIndex = camSetting.DisplayPositionRow, ColIndex = camSetting.DisplayPositionCol, PageNumber = pageNo, Station = s };
                //imagePos.Station = s;
                //imagePos.Index = camIdx;
                //imagePos.PageNumber = pageNo;
                //RectangleF rect = new RectangleF(0, 0, width / ColumnsCount * colSpan, height / RowsCount - headerHeight);
                RectangleF rect = new RectangleF(0, 0, (float)width / Math.Max(1, dispSettings.Cols), height / Math.Max(1, dispSettings.Rows) - headerHeight);

                //int pageCamIdx = camIdx - pageNo * (ColumnsCount * RowsCount);
                //int rowPos = (int)Math.Truncate((decimal)pageCamIdx / ColumnsCount);
                //int colPos = pageCamIdx - rowPos * ColumnsCount;
                imagePos.HeaderAbsPosition = new PointF(camSetting.DisplayPositionCol * rect.Width + headerOffsetX, camSetting.DisplayPositionRow * (rect.Height + headerHeight) + headerOffsetY);
                imagePos.HeaderRelPosition = new PointF(camSetting.DisplayPositionCol * rect.Width + headerOffsetX, headerOffsetY);

                rect.Location = new PointF(camSetting.DisplayPositionCol * rect.Width, camSetting.DisplayPositionRow * (rect.Height + headerHeight));

                imagePos.ImagePosition = rect;
                if (!imageLocation.ContainsKey(camSetting.Id))
                    imageLocation.Add(camSetting.Id, imagePos);
            }
        }

        public override void ClearBackground()
        {

            if (bufferBm.Width == 0 || bufferBm.Height <= headerHeight) return;
            //importante per non mostrare immagini di altre camere...
            bufferBm.ROI = new Rectangle(0, headerHeight, bufferBm.Width, Math.Max(0, bufferBm.Height - headerHeight));
            bufferBm.SetZero();
            bufferBm.ROI = Rectangle.Empty;
            drawHeader();
            newImageToShow = true;
        }

        public void NextPage()
        {

            CurrentPage = (CurrentPage + 1) % _maxPages;
            /*CurrentPage++;
            if (CurrentPage * (RowsCount * ColumnsCount) >= SourceStations.Count)
                CurrentPage = 0;*/
            drawHeader();
            DoRender(true);
        }

        public override void DrawHeader()
        {

            drawHeader();
        }

        void drawHeader()
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(ImageCtrl))
                return;

            if (ImageCtrl.InvokeRequired && ImageCtrl.IsHandleCreated)
            {
                ImageCtrl.Invoke(new MethodInvoker(drawHeader));
            }
            else
            {
                try
                {
                    int rows = 1;
                    foreach (ImagePositionInfo posInfo in imageLocation.Values)
                    {
                        if (posInfo.PageNumber == CurrentPage)
                            rows = Math.Max(posInfo.RowIndex + 1, rows);
                    }
                    Bitmap[] header = new Bitmap[rows];
                    float[] yOffsets = new float[rows];
                    for (int ir = 0; ir < rows; ir++)
                    {
                        header[ir] = new Bitmap(bufferBm.Width, headerHeight);
                        using (Graphics g = Graphics.FromImage(header[ir]))
                        {
                            g.Clear(Color.FromArgb(28, 28, 28));
                            foreach (ImagePositionInfo posInfo in imageLocation.Values)
                            {
                                if (posInfo.PageNumber == CurrentPage && posInfo.RowIndex == ir)
                                {
                                    yOffsets[ir] = posInfo.HeaderAbsPosition.Y - headerOffsetY;
                                    if (posInfo.Station.Enabled)
                                        g.DrawString(posInfo.Station.Description, _headerFont, new SolidBrush(SystemColors.Control), posInfo.HeaderRelPosition);
                                    else
                                        g.DrawString(posInfo.Station.Description + " OFF-LINE", _headerFont, new SolidBrush(Color.Red), posInfo.HeaderRelPosition);
                                }
                            }
                        }
                    }
                    for (int ir = 0; ir < rows; ir++)
                    {
                        lock (_syncImgObj)
                        {
                            using (Image<Rgb, byte> _tmpHeader = new Image<Rgb, byte>(header[ir]))
                            {
                                bufferBm.ROI = new Rectangle(0, (int)yOffsets[ir], bufferBm.Width, headerHeight);
                                CvInvoke.cvCopy(_tmpHeader, bufferBm, IntPtr.Zero);
                                bufferBm.ROI = Rectangle.Empty;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Trace.WriteLine("DRAW HEADER FAILED");
                    Log.Line(LogLevels.Warning, "ScreenGridDisplay.DrawHeader", "Error: " + ex.Message);
                }
            }
        }
    }
}
