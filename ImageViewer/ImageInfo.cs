using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;

using SPAMI.Util;
using SPAMI.Util.Logger;

namespace SPAMI.Util.EmguImageViewer
{
    [Serializable]
    public class ImageInfo : ICommon
    {
        public string ClassName { get; set; }
        public string Filepath { get; private set; }
        public Image<Hsv, Byte> MyImage;
        public Image<Hsv, Byte> MyUndrawnImage;
        public Image<Hsv, Byte> MyZoomImage;
        public Image<Hsv, Byte> MyUndrawnZoomImage;
        public string GenInfo { get; private set; }
        public string ZoomInfo { get; private set; }
        public string PixInfo { get; private set; }
        public bool AdjacentZoomInfo { get; set; }
        public bool HorizontalLineInfo { get; set; }
        public bool VerticalLineInfo { get; set; }
        public Point PixInfoCoord { get; set; }

        public int PixAdjacentValWidth
        {
            get
            {
                pixAdjacentValWidth = Math.Max(1, pixAdjacentValWidth);         // solo valori > 0
                if (pixAdjacentValWidth % 2 == 0) pixAdjacentValWidth -= 1;     // solo valori dispari
                return pixAdjacentValWidth;
            }
            set
            {
                pixAdjacentValWidth = Math.Max(1, value);                       // solo valori > 0
                if (pixAdjacentValWidth % 2 == 0) pixAdjacentValWidth -= 1;     // solo valori dispari
            }
        }
        private int pixAdjacentValWidth;
        public int PixAdjacentValHeight
        {
            get
            {
                pixAdjacentValHeight = Math.Max(1, pixAdjacentValHeight);       // solo valori > 0
                if (pixAdjacentValHeight % 2 == 0) pixAdjacentValHeight -= 1;   // solo valori dispari
                return pixAdjacentValHeight;
            }
            set
            {
                pixAdjacentValHeight = Math.Max(1, value);                      // solo valori > 0
                if (pixAdjacentValHeight % 2 == 0) pixAdjacentValHeight -= 1;   // solo valori dispari
            }
        }
        private int pixAdjacentValHeight;
        public double[,] PixAdjacentValues;
        public double[] PixVerticalLineValues;
        public double[] PixHorizontalLineValues;
        public double[] PixVerticalLineValuesProg;
        public double[] PixHorizontalLineValuesProg;
        public Rectangle FullFrameROI;
        public Rectangle CurrentROI;
        public DenseHistogram ZoomHist;
        public Hsv Blu;
        public Hsv Green;
        public Hsv Red;
        private MCvFont f;
        private Point ptText;
        //private int PixX = 0, PixY = 0;
        private CircleF pixelInfoCircle;
        private PointF pixelInfoCenter;
        private LineSegment2D pixelInfoHorizLine;
        private LineSegment2D pixelInfoVertLine;
        private Point[] ptsHoriz = new Point[4];
        private Point[] ptsVert = new Point[4];
        //public event EventHandler ZoomInfoChanged;

        public ImageInfo()
        {
            ClassName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
        }

        public void Init()
        {
            ZoomHist = new DenseHistogram(255, new RangeF(0.0f, 255.0f));
            f = new MCvFont(FONT.CV_FONT_HERSHEY_COMPLEX, 1.0, 1.0);
            ptText = new Point(10, 80);
            Blu = new Hsv(120, 255, 255);
            Green = new Hsv(60, 255, 255);
            Red = new Hsv(0, 255, 255);
            
            PixAdjacentValues = new double[PixAdjacentValHeight, PixAdjacentValWidth];
            pixelInfoCircle = new CircleF();
            pixelInfoCenter = new PointF();
            pixelInfoHorizLine = new LineSegment2D();
            for (int i = 0; i < ptsHoriz.Length; i++)
            {
                ptsHoriz[i] = new Point(0, 0);
                ptsVert[i] = new Point(0, 0);
            }
            pixelInfoCircle.Radius = 1.0f;
        }

        public bool LoadImage(string filepath)
        {
            bool ret = true;
            Filepath = filepath;
            try
            {
                if (MyUndrawnImage != null)
                    MyUndrawnImage.Dispose();
                MyUndrawnImage = new Image<Hsv, byte>(Filepath);
                FullFrameROI.X = 0;
                FullFrameROI.Y = 0;
                FullFrameROI.Width = MyUndrawnImage.Width;
                FullFrameROI.Height = MyUndrawnImage.Height;
                ret &= ExtractGenInfo();
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, ClassName + ".LoadImage", "Errore nel caricamento dell'immagine. Path: " + filepath + " Error: " + ex.ToString());
            }
            //WriteOnImage();
            return true;
        }

        public bool PushImage(KeyValuePair<int, Image<Hsv,Byte>> img)
        {
            bool ret = true;
            Filepath = null;
            try
            {
                if (MyUndrawnImage != null)
                    MyUndrawnImage.Dispose();
                MyUndrawnImage = img.Value.Clone();
                FullFrameROI.X = 0;
                FullFrameROI.Y = 0;
                FullFrameROI.Width = MyUndrawnImage.Width;
                FullFrameROI.Height = MyUndrawnImage.Height;
                ret &= ExtractGenInfo();
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, ClassName + ".PushImage", "Errore nel caricamento dell'immagine. ID: {0}. Error: " + ex.ToString(), img.Key);
            }
            //WriteOnImage();
            return true;
        }

        public bool ExtractGenInfo()
        {
            if (MyUndrawnImage == null)
                return false;
            //Image.Size;
            //Image.PixelFormat;
            //Image.Flags;
            //Image.HorizontalResolution;
            //Image.VerticalResolution;
            //Image.RawFormat; 
            GenInfo = "";
            if (Filepath != null)
                GenInfo = "Path: " + Filepath + "\n";
            GenInfo += "Size: " + MyUndrawnImage.Width + " x " + MyUndrawnImage.Height;
            return true;
        }

        double[] minValues;
        double[] maxValues;
        Point[] minLocations;
        Point[] maxLocations;
        private bool ExtractZoomInfo(Rectangle rect)
        {
            if (MyUndrawnImage == null)
                return false;
            //ZoomInfo = "Avg: " + MyImage.GetAverage();
            CurrentROI = rect;
            ZoomInfo = string.Format("ROI: X={0} Y={1} W={2} H={3}", rect.X, rect.Y, rect.Width, rect.Height) + "\n";
            //Image<Gray, Byte> imgZoomGrayInfo = MyUndrawnImage.Convert<Gray, Byte>();
            Image<Gray, Byte>[] channels = MyUndrawnImage.Split();  //split into components
            Image<Gray, Byte> imgval = channels[2];            //hsv, so channels[2] is value.
            imgval.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);
            var average = imgval.GetAverage();
            ZoomInfo += "AVG: " + average.Intensity.ToString("f2");
            if (minValues.Length > 0) ZoomInfo += "   min: " + minValues[0].ToString("f2");
            if (maxValues.Length > 0) ZoomInfo += "   Max: " + maxValues[0].ToString("f2");
            UpdatePixInfo();
            //if (ZoomInfoChanged != null) ZoomInfoChanged(this, EventArgs.Empty);
            if (ZoomHist!=null)
                ZoomHist.Calculate<Byte>(new Image<Gray, byte>[] { imgval }, false, null);
            return true;
        }

        public bool WriteOnImage(string text)
        {
            if (text == null || text.Length < 1 || MyImage == null)
                return false;
            MyImage.Draw(text, ref f, ptText, Green);
            return true;
        }

        Rectangle rectAbsolute = new Rectangle(0, 0, 0, 0);
        public void Refresh(ImageBox imgBox, RectangleF rectRelative)
        {
            if (MyUndrawnImage != null)
            {
                rectAbsolute.X = (int)(rectRelative.X * imgBox.Width + 0.5F);
                rectAbsolute.Y = (int)(rectRelative.Y * imgBox.Height + 0.5F);
                //rectAbsolute.Width = (int)(rectRelative.Width * imgBox.Width + 0.5F);
                Point pixTL = Utilities.PicBox2ImgCoords(imgBox.SizeMode, new Point(rectAbsolute.X, rectAbsolute.Y), MyUndrawnImage.Width, MyUndrawnImage.Height, imgBox.Width, imgBox.Height);
                Point pixBR = Utilities.PicBox2ImgCoords(imgBox.SizeMode, new Point((int)((rectRelative.X + rectRelative.Width) * imgBox.Width + 0.5F - 1F), (int)((rectRelative.Y + rectRelative.Height) * imgBox.Height + 0.5F - 1F)), MyUndrawnImage.Width, MyUndrawnImage.Height, imgBox.Width, imgBox.Height);
                Draw(pixTL, pixBR);
                imgBox.Image = MyImage;
            }
        }

        public void RefreshZoom(ImageBox imgBox/*, Rectangle rect*/)
        {
            //CurrentROI = rect;
            if (MyZoomImage != null)
            {
                imgBox.Image = MyZoomImage;
                if (CurrentROI.Width > 1 && CurrentROI.Height > 1)
                    imgBox.Image = MyZoomImage;
                else
                    imgBox.Image = MyImage;
            }
        }

        public double GetZoomValue(Point point)
        {
            if ((point.X >= 0) &&
                (point.X < MyUndrawnZoomImage.Width) &&
                (point.Y >= 0) &&
                (point.Y < MyUndrawnZoomImage.Height))
            {
                return MyUndrawnZoomImage.Data[point.Y, point.X, 2];
            }
            else
            {
                Log.Line(LogLevels.Warning, ClassName + ".GetZoomValue", "Pixel selection out of bounds. X={0} Y={1}", point.X, point.Y);
                return -1;
            }
        }

        private bool UpdatePixInfo()
        {
            if (MyUndrawnZoomImage != null)
            {
                if ((PixInfoCoord.X >= 0) &&
                    (PixInfoCoord.X < MyUndrawnZoomImage.Width) &&
                    (PixInfoCoord.Y >= 0) &&
                    (PixInfoCoord.Y < MyUndrawnZoomImage.Height))
                {
                    PixInfo = string.Format("Sel. Pixel X: {0}({1}) Y: {2}({3}) Value: {4}", CurrentROI.X + PixInfoCoord.X, PixInfoCoord.X, CurrentROI.Y + PixInfoCoord.Y, PixInfoCoord.Y, MyUndrawnZoomImage.Data[PixInfoCoord.Y, PixInfoCoord.X, 2]);
                }
                else if ((PixInfoCoord.X == -1) && (PixInfoCoord.Y == -1))
                {
                    return false;
                }
                else
                {
                    for (int ix = 0; ix < PixAdjacentValWidth; ix++)
                    {
                        for (int iy = 0; iy < PixAdjacentValHeight; iy++)
                        {
                            PixAdjacentValues[iy, ix] = -1;
                        }
                    }
                    Log.Line(LogLevels.Warning, ClassName + ".UpdatePixInfo", "Pixel selection out of bounds. X={0} Y={1}", PixInfoCoord.X, PixInfoCoord.Y);
                    return false;
                }
                if (AdjacentZoomInfo)
                {
                    for (int ix = -(PixAdjacentValWidth - 1) / 2; ix <= (PixAdjacentValWidth - 1) / 2; ix++)
                    {
                        for (int iy = -(PixAdjacentValHeight - 1) / 2; iy <= (PixAdjacentValHeight - 1) / 2; iy++)
                        {
                            if ((PixInfoCoord.X + ix >= 0) &&
                                (PixInfoCoord.X + ix < MyUndrawnZoomImage.Width) &&
                                (PixInfoCoord.Y + iy >= 0) &&
                                (PixInfoCoord.Y + iy < MyUndrawnZoomImage.Height))
                            {
                                PixAdjacentValues[iy + (PixAdjacentValHeight - 1) / 2, ix + (PixAdjacentValWidth - 1) / 2] = (double)MyUndrawnZoomImage.Data[PixInfoCoord.Y + iy, PixInfoCoord.X + ix, 2];
                            }
                            else
                            {
                                PixAdjacentValues[iy + (PixAdjacentValHeight - 1) / 2, ix + (PixAdjacentValWidth - 1) / 2] = -1;
                            }
                        }
                    }
                }
                if (HorizontalLineInfo)
                {
                    PixHorizontalLineValuesProg = new double[CurrentROI.Width];
                    PixHorizontalLineValues = new double[CurrentROI.Width];
                    for (int ix = 0; ix < CurrentROI.Width; ix++)
                    {
                        if ((ix >= 0) &&
                            (ix < MyUndrawnZoomImage.Width))
                        {
                            PixHorizontalLineValuesProg[ix] = (double)ix;
                            PixHorizontalLineValues[ix] = (double)MyUndrawnZoomImage.Data[PixInfoCoord.Y, ix, 2];
                        }
                    }
                }
                if (VerticalLineInfo)
                {
                    PixVerticalLineValuesProg = new double[CurrentROI.Height];
                    PixVerticalLineValues = new double[CurrentROI.Height];
                    for (int iy = 0; iy < CurrentROI.Height; iy++)
                    {
                        if ((iy >= 0) &&
                            (iy < MyUndrawnZoomImage.Height))
                        {
                            PixVerticalLineValuesProg[iy] = (double)iy;
                            PixVerticalLineValues[iy] = (double)MyUndrawnZoomImage.Data[iy, PixInfoCoord.X, 2];
                        }
                    }
                }
                //if (ZoomInfoChanged != null) ZoomInfoChanged(this, EventArgs.Empty);
            }
            return true;
        }

        /*public void ChangePixInfoCoord(int x, int y)
        {
            PixX = x;
            PixY = y;
        }

        public void GetPixInfoCoord()
        {
            PixX = x;
            PixY = y;
        }*/

       

        private Rectangle zoomRect = new Rectangle();
        private bool Draw(Point tl, Point br)
        {
            if (MyUndrawnImage == null)
                return false;
            zoomRect.X = tl.X;
            zoomRect.Y = tl.Y;
            zoomRect.Width = br.X - tl.X + 1;
            zoomRect.Height = br.Y - tl.Y + 1;
            if (zoomRect.X <= 0 || zoomRect.X >= MyUndrawnImage.Size.Width) zoomRect.X = 0;
            if (zoomRect.Y <= 0 || zoomRect.Y >= MyUndrawnImage.Size.Height) zoomRect.Y = 0;
            if (zoomRect.Width <= 0 || (zoomRect.X + zoomRect.Width >= MyUndrawnImage.Size.Width)) zoomRect.Width = MyUndrawnImage.Size.Width - zoomRect.X;
            if (zoomRect.Height <= 0 || (zoomRect.Y + zoomRect.Height >= MyUndrawnImage.Size.Height)) zoomRect.Height = MyUndrawnImage.Size.Height - zoomRect.Y;
            MyImage = MyUndrawnImage.Clone();
            MyUndrawnImage.ROI = zoomRect;
            MyZoomImage = MyUndrawnImage.Copy();
            MyUndrawnZoomImage = MyUndrawnImage.Copy();
            ExtractZoomInfo(zoomRect);
            MyUndrawnImage.ROI = FullFrameROI;
            //WriteOnImage(string.Format("ROI: X={0}\tY={1}\tW={2}\tH={3}", zoomRect.X, zoomRect.Y, zoomRect.Width, zoomRect.Height));
            if (zoomRect.Width > 0 && zoomRect.Height > 0)
            {
                MyImage.Draw(zoomRect, Red, 3);
            }
            if (PixInfoCoord.X >= 0 && PixInfoCoord.Y >= 0)
            {
                pixelInfoCenter.X = (float)PixInfoCoord.X;
                pixelInfoCenter.Y = (float)PixInfoCoord.Y;
                pixelInfoCircle.Center = pixelInfoCenter;
                if (HorizontalLineInfo)
                {
                    ptsHoriz[0].X = 0;
                    ptsHoriz[1].X = PixInfoCoord.X - 3;
                    ptsHoriz[2].X = PixInfoCoord.X + 3;
                    ptsHoriz[3].X = CurrentROI.Width - 1;
                    for (int ip=0; ip<ptsHoriz.Length; ip++)
                        ptsHoriz[ip].Y = PixInfoCoord.Y;
                    pixelInfoHorizLine.P1 = ptsHoriz[0];
                    pixelInfoHorizLine.P2 = ptsHoriz[1];
                    MyZoomImage.Draw(pixelInfoHorizLine, Red, 1);
                    pixelInfoHorizLine.P1 = ptsHoriz[2];
                    pixelInfoHorizLine.P2 = ptsHoriz[3];
                    MyZoomImage.Draw(pixelInfoHorizLine, Red, 1);
                }
                if (VerticalLineInfo)
                {
                    ptsVert[0].Y = 0;
                    ptsVert[1].Y = PixInfoCoord.Y - 3;
                    ptsVert[2].Y = PixInfoCoord.Y + 3;
                    ptsVert[3].Y = CurrentROI.Height - 1;
                    for (int ip = 0; ip < ptsVert.Length; ip++)
                        ptsVert[ip].X = PixInfoCoord.X;
                    pixelInfoVertLine.P1 = ptsVert[0];
                    pixelInfoVertLine.P2 = ptsVert[1];
                    MyZoomImage.Draw(pixelInfoVertLine, Red, 1);
                    pixelInfoVertLine.P1 = ptsVert[2];
                    pixelInfoVertLine.P2 = ptsVert[3];
                    MyZoomImage.Draw(pixelInfoVertLine, Red, 1);
                }
                MyZoomImage.Draw(pixelInfoCircle, Green, 1);
            }
            return true;
        }
    }
}
