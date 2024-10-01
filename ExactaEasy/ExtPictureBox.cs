using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using SPAMI.Util.Logger;
using ExactaEasy;
using Emgu.CV.UI;
using Emgu.CV;
using Emgu.CV.Structure;
using DisplayManager;

namespace ExactaEasy {
    public partial class ExtPictureBox : ImageBox {

        //[Browsable(false)]
        //public decimal ImageAspectRatio { get; set; }
        int leftClickCounter = 0;
        Point imgOrigPt1, imgOrigPt2;

        public ExtPictureBox() {

            InitializeComponent();
        }

        protected override void OnResize(EventArgs e) {

            base.OnResize(e);
        }

        public void ResetZoom(Display display, IStation station, int rotation) {
            station.ZoomAOI = display.ResetZoom(rotation);
            display.DoRender(false);
            leftClickCounter = 0;
            display.ZoomLayerRectVisible = false;
        }

        public void Zoom(Display display, IStation station, int rotation, MouseEventArgs e) {

            if (e.Button == MouseButtons.Right) {
                ResetZoom(display, station, rotation);
            }
            if (e.Button == MouseButtons.Left) {
                if (leftClickCounter == 0) {
                    imgOrigPt1 = e.Location;
                    display.ZoomLayerRect = new Rectangle(e.X, e.Y, 0, 0);
                    display.ZoomLayerRectVisible = true;
                    display.ZoomLayerStringVisible = false;
                }
                if (leftClickCounter == 1) {
                    imgOrigPt2 = e.Location;
                    int a = Math.Abs(imgOrigPt1.X - imgOrigPt2.X);
                    int b = Math.Abs(imgOrigPt1.Y - imgOrigPt2.Y);
                    Point ptTL = new Point(Math.Min(imgOrigPt1.X, imgOrigPt2.X), Math.Min(imgOrigPt1.Y, imgOrigPt2.Y));
                    station.ZoomAOI = display.SetZoom(station, new Rectangle(ptTL.X, ptTL.Y, a, b), rotation);
                    display.ZoomLayerRectVisible = false;
                    display.ZoomLayerStringVisible = true;
                    display.DoRender(false);
                }
                leftClickCounter = leftClickCounter + 1;//) % 2;

                //if (e.Button == System.Windows.Forms.MouseButtons.Left) {
                //    //SetZoomScale(ZoomScale * 2, e.Location);
                //    if (leftClickCounter == 0) {
                //        imgOrigPt1 = e.Location;
                //    }
                //    if (leftClickCounter == 1) {
                //        imgOrigPt2 = e.Location;
                //        double a = Math.Abs(imgOrigPt1.X - imgOrigPt2.X);
                //        double b = Math.Abs(imgOrigPt1.Y - imgOrigPt2.Y);
                //        Point ptTL = new Point(Math.Min(imgOrigPt1.X, imgOrigPt2.X), Math.Min(imgOrigPt1.Y, imgOrigPt2.Y));
                //        Point ptBR = new Point(Math.Max(imgOrigPt1.X, imgOrigPt2.X), Math.Max(imgOrigPt1.Y, imgOrigPt2.Y));
                //        Point ptFixed = new Point((int)(ptTL.X + a / 2 + 0.5), (int)(ptTL.Y + b / 2 + 0.5));
                //        //double newZoomScale = Math.Max(imgOrig.Width / a, imgOrig.Height / b) / Math.Max(imgOrig.Width/Width, imgOrig.Height/Height);
                //        double zoomScaleX = Width / a;
                //        double zoomScaleY = Height / b;
                //        double newZoomScale = Math.Min(zoomScaleX, zoomScaleY);
                //        if (zoomScaleX < zoomScaleY) {
                //            b = Height / newZoomScale;
                //            ptBR.Y = (int)(ptTL.Y + b + 0.5);
                //            int outOfBound = Height - ptBR.Y;
                //            if (outOfBound < 0) {
                //                ptTL.Y += outOfBound;
                //                ptBR.Y += outOfBound;
                //            }
                //        }
                //        if (zoomScaleX > zoomScaleY) {
                //            a = Width / newZoomScale;
                //            ptBR.X = (int)(ptTL.X + a + 0.5);
                //            int outOfBound = Width - ptBR.X;
                //            if (outOfBound < 0) {
                //                ptTL.X += outOfBound;
                //                ptBR.X += outOfBound;
                //            }
                //        }
                //        SetZoomScale(newZoomScale, ptTL);


                //    }
                //    leftClickCounter = (leftClickCounter + 1) % 2;
            }


            //PIER: ZOOM TODO
            //if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
            //    if (ImgOrigClickCounter == 0) {
            //        ImgOrigPt1 = e.Location;
            //        LastPixelInfoPointPBRelative.X = LastPixelInfoPointPBRelative.Y = -1F;
            //    }
            //    if (ImgOrigClickCounter == 1) {
            //        ImgOrigPt2 = e.Location;
            //        ImgOrigZoomRectRelative.X = (float)Math.Min(ImgOrigPt1.X, ImgOrigPt2.X) / (imageBox1.Width - 1);
            //        ImgOrigZoomRectRelative.Y = (float)Math.Min(ImgOrigPt1.Y, ImgOrigPt2.Y) / (imageBox1.Height - 1);
            //        ImgOrigZoomRectRelative.Width = (float)Math.Abs(ImgOrigPt2.X - ImgOrigPt1.X) / (imageBox1.Width - 1);
            //        ImgOrigZoomRectRelative.Height = (float)Math.Abs(ImgOrigPt2.Y - ImgOrigPt1.Y) / (imageBox1.Height - 1);
            //    }
            //    ImgOrigClickCounter = (ImgOrigClickCounter + 1) % 2;
            //    CurrentImage.PixInfoCoord = new Point(-1, -1);
            //}
            //if ((e.Button & MouseButtons.Right) == MouseButtons.Right) {
            //    ImgOrigClickCounter = 0;
            //    ImgOrigZoomRectRelative.X = 0F;
            //    ImgOrigZoomRectRelative.Y = 0F;
            //    ImgOrigZoomRectRelative.Width = 1F;
            //    ImgOrigZoomRectRelative.Height = 1F;
            //    CurrentImage.PixInfoCoord = new Point(-1, -1);
            //    LastPixelInfoPointPBRelative.X = LastPixelInfoPointPBRelative.Y = -1F;
            //}
            //if (!EnqueuerRunning) {
            //    CurrentImage.Refresh(imageBox1, ImgOrigZoomRectRelative);
            //    ChangeGraphInfo();
            //}
        }
    }
}
