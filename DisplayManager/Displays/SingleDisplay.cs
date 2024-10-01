using Emgu.CV.Structure;
using Emgu.CV.UI;
using ExactaEasyCore;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DisplayManager
{
    public class SingleDisplay : ScreenGridDisplay
    {

        object syncImgObj = new object();
        bool initialized = false;

        public SingleDisplay(string name, ImageBox imageCtrl, IStation station, CameraSetting cameraSetting, bool keepAspectRatio, int interpolationType, bool showColoredBorders)
            : base(name, imageCtrl, new StationCollection() { station }, new List<ScreenGridDisplaySettings> { new ScreenGridDisplaySettings() { Rows = 1, Cols = 1, Id = 0 } }, new List<CameraSetting>() { cameraSetting }, keepAspectRatio, interpolationType, showColoredBorders)
        {

            //bufferBm = new Image<Rgb, byte>(ImageCtrl.Width, ImageCtrl.Height);
            ClearBackground();
            initialized = true;
            DisplayAOIUsed.Add(0, new Rectangle());
        }

        public override void RenderImage(IStation currentStation)
        {

            if (currentStation.CurrentImage == null || !initialized)
                return;

            ImageCtrl.BackgroundImage = null;
            Rectangle displayAOIUsed;
            lock (currentStation.CurrentImage)
            {
                lock (_syncImgObj)
                {
                    if (bufferBm != null)
                    {
                        bufferBm.Dispose();
                        bufferBm = null;
                    }
                    currentStation.AdjustImage(currentStation.CurrentImage, ImageCtrl.Width, ImageCtrl.Height, _keepAspectRatio, InterpolationType, ref bufferBm, out displayAOIUsed);
                    ImageShown = currentStation.CurrentImage;
                    if (DisplayAOIUsed.ContainsKey(0))
                        DisplayAOIUsed[0] = new Rectangle(displayAOIUsed.X, displayAOIUsed.Y, displayAOIUsed.Width, displayAOIUsed.Height);
                    if (ZoomLayerRectVisible)
                    {
                        bufferBm.Draw(ZoomLayerRect, redColor, 1);
                    }
                    if (ZoomLayerStringVisible)
                    {
                        bufferBm.Draw("[X=" + currentStation.ZoomAOI.X.ToString() + "; Y=" + currentStation.ZoomAOI.Y.ToString() + "; W=" + currentStation.ZoomAOI.Width.ToString() + "; H=" + currentStation.ZoomAOI.Height.ToString() + "]", ref font, new Point(10, bufferBm.Height - 20), redColor);
                    }
                    newImageToShow = true;
                }
            }
        }

        public override void RenderAlternativeImage(IStation currentStation)
        {

            if (currentStation.AlternativeImage == null || !initialized)
                return;

            ImageCtrl.BackgroundImage = null;
            Rectangle displayAOIUsed;
            lock (currentStation.AlternativeImage)
            {
                lock (_syncImgObj)
                {
                    if (bufferBm != null)
                    {
                        bufferBm.Dispose();
                        bufferBm = null;
                    }
                    currentStation.AdjustImage(currentStation.AlternativeImage, ImageCtrl.Width, ImageCtrl.Height, true, InterpolationType, ref bufferBm, out displayAOIUsed);
                    ImageShown = currentStation.AlternativeImage;
                    if (DisplayAOIUsed.ContainsKey(0))
                        DisplayAOIUsed[0] = new Rectangle(displayAOIUsed.X, displayAOIUsed.Y, displayAOIUsed.Width, displayAOIUsed.Height);
                    newImageToShow = true;
                }
            }
        }

        public override void ClearBackground()
        {

            //importante per non mostrare immagini di altre camere...
            bufferBm.ROI = new Rectangle(0, 0, bufferBm.Width, bufferBm.Height);
            bufferBm.SetZero();
            bufferBm.ROI = Rectangle.Empty;
        }

        public override Rectangle SetZoom(IStation currentStation, Rectangle AOI, int rotation)
        {

            if (DisplayAOIUsed != null && ImageShown != null)
            {
                AOI.X = AOI.X - DisplayAOIUsed[0].X;
                AOI.Y = AOI.Y - DisplayAOIUsed[0].Y;
                int ImageShownWidth = ImageShown.Width;
                int ImageShownHeight = ImageShown.Height;
                if (rotation == 90 || rotation == 270)
                {
                    ImageShownWidth = ImageShown.Height;
                    ImageShownHeight = ImageShown.Width;
                }
                double zoomScaleX = (double)ImageShownWidth / ImageCtrl.Width;
                double zoomScaleY = (double)ImageShownHeight / ImageCtrl.Height;

                if (_keepAspectRatio)
                {
                    zoomScaleX = Math.Max(zoomScaleX, zoomScaleY);
                    zoomScaleY = zoomScaleX;
                }
                int X = Math.Min(Math.Max((int)(zoomScaleX * AOI.X + 0.5), 0), ImageShownWidth - 1);
                int Y = Math.Min(Math.Max((int)(zoomScaleY * AOI.Y + 0.5), 0), ImageShownHeight - 1);
                int W = Math.Min(Math.Max((int)(zoomScaleX * AOI.Width + 0.5), 0), ImageShownWidth - X);
                int H = Math.Min(Math.Max((int)(zoomScaleY * AOI.Height + 0.5), 0), ImageShownHeight - Y);
                return new Rectangle(X, Y, W, H);
            }
            return ResetZoom(rotation);
        }

        public override string GetPixelInfo(Point point, int rotation, Rectangle AOI)
        {

            if (DisplayAOIUsed != null && ImageShown != null)
            {
                point.X = point.X - DisplayAOIUsed[0].X;
                point.Y = point.Y - DisplayAOIUsed[0].Y;
                int ImageShownWidth = /*ImageShown.Width*/AOI.Width;
                int ImageShownHeight = /*ImageShown.Height*/AOI.Height;
                if (rotation == 90 || rotation == 270)
                {
                    ImageShownWidth = /*ImageShown.Height*/AOI.Height;
                    ImageShownHeight = /*ImageShown.Width*/AOI.Width;
                    int temp = point.X;
                    point.Y = point.X;
                    point.X = temp;
                }
                double zoomScaleX = (double)ImageShownWidth / ImageCtrl.Width;
                double zoomScaleY = (double)ImageShownHeight / ImageCtrl.Height;

                if (_keepAspectRatio)
                {
                    zoomScaleX = Math.Max(zoomScaleX, zoomScaleY);
                    zoomScaleY = zoomScaleX;
                }
                int X = Math.Min(Math.Max((int)(zoomScaleX * point.X + AOI.X + 0.5), 0), ImageShownWidth - 1);
                int Y = Math.Min(Math.Max((int)(zoomScaleY * point.Y + AOI.Y + 0.5), 0), ImageShownHeight - 1);
                Rgb value = new Rgb();
                //if (rotation == 90 || rotation == 270)
                //    value = ImageShown[X, Y];
                //else
                if (Y > 0 && X > 0)
                    value = ImageShown[Y, X];
                return "(" + X.ToString() + ", " + Y.ToString() + ") = " + value.Red + "R, " + value.Green + "G, " + value.Blue + "B";
            }
            return "";
        }
    }
}
