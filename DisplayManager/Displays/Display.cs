using Emgu.CV;
using Emgu.CV.Structure;
using SPAMI.Util.Logger;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DisplayManager
{

    public abstract class Display {

        protected object syncImage = new object();
        protected bool displaySuspended { get; set; }
        protected bool newImageToShow;

        public Image<Rgb, byte> ImageShown { get; protected set; }
        public string Name { get; private set; }
        public CameraCollection SourceCameras { get; private set; }
        public StationCollection SourceStations { get; private set; }
        public Emgu.CV.CvEnum.INTER InterpolationType { get; internal set; }
        public Dictionary<int, Rectangle> DisplayAOIUsed { get; protected set; }
        public bool ZoomLayerRectVisible { get; set; }
        public bool ZoomLayerStringVisible { get; set; }
        public Rectangle ZoomLayerRect { get; set; }

        public Display(string name, CameraCollection sourceCameras) {

            Name = name;
            SourceCameras = sourceCameras;
            InterpolationType = Emgu.CV.CvEnum.INTER.CV_INTER_NN;
            //foreach(Camera cam in sourceCameras)
            //    cam.ImageAvailable += new EventHandler<ImageAvailableEventArgs>(cam_ImageAvailable);
        }

        public Display(string name, StationCollection sourceStations) {

            Name = name;
            SourceStations = sourceStations;
            foreach (IStation station in sourceStations) {
                if (station != null) {
                    station.ImageAvailable += new EventHandler<ImageAvailableEventArgs>(station_ImageAvailable);
                    station.AlternativeImageAvailable += station_AlternativeImageAvailable;
                }
                //station.ImagesResultsAvailable += new EventHandler<ImagesResultsAvailableEventArgs>(station_ImagesResultsAvailable);
            }
        }

        public virtual void Dispose() {

            ImageShown = null;
            foreach (IStation station in SourceStations) {
                station.ImageAvailable -= station_ImageAvailable;
                station.AlternativeImageAvailable -= station_AlternativeImageAvailable;
                //station.ImagesResultsAvailable -= new EventHandler<ImagesResultsAvailableEventArgs>(station_ImagesResultsAvailable);
            }

            if(!(DisplayAOIUsed is null))
                DisplayAOIUsed.Clear();
            //if (SourceStations != null)
            //    SourceStations.Clear();
            //SourceStations = null;
            //if (SourceCameras != null)
            //    SourceCameras.Clear();
            //SourceCameras = null;
            //if (ImageShown != null)
            //    ImageShown.Dispose();
            //ImageShown = null;
            //DisplayAOIUsed.Clear();
            //syncImage = null;
        }

        void station_ImageAvailable(object sender, ImageAvailableEventArgs e) {
            if (displaySuspended == false) {
                lock (syncImage) {
                    if (e.Image != null) {
                        RenderImage((IStation)sender);
                        IncrementThumbId((IStation)sender);
                        //e.Image.Save(@"C:\renderImg.bmp");    //pier: eliminare
                    }
                }
            }
        }

        void station_AlternativeImageAvailable(object sender, ImageAvailableEventArgs e) {
            if (displaySuspended == false) {
                lock (syncImage) {
                    if (e.Image != null) {
                        RenderAlternativeImage((IStation)sender);
                    }
                }
            }
        }

        public virtual void DrawHeader() { }

        public void DoRender() {

            DoRender(false);
        }

        public void DoRender(bool clearBackground) {

            lock (syncImage) {
                if (clearBackground)
                    ClearBackground();
                foreach (IStation station in SourceStations)
                    if (station != null)
                        RenderImage(station);
            }
        }

        public virtual void Suspend() {

            displaySuspended = true;
            Log.Line(LogLevels.Debug, "Display.Suspend", "Display " + Name + " suspended");
        }

        public virtual void Resume() {

            displaySuspended = false;
            Log.Line(LogLevels.Debug, "Display.Resume", "Display " + Name + " resumed");
        }

        public virtual void Resize(int w, int h) { }

        public virtual void RenderImage(Camera currentStation, Image<Rgb, byte> newImage) { }
        public virtual void RenderImage(IStation currentStation) { }
        public virtual void IncrementThumbId(IStation currentStation) { }
        public virtual void RenderAlternativeImage(IStation currentStation) { }
        public virtual void ClearBackground() { }
        public virtual string GetPixelInfo(Point point, int rotation, Rectangle AOI) { return ""; }
        public virtual Rectangle SetZoom(IStation currentStation, Rectangle AOI, int rotation) {
            return ResetZoom(rotation);
        }

        public virtual Rectangle ResetZoom(int rotation) {

            ZoomLayerStringVisible = false;
            if (ImageShown != null) {
                if (rotation == 90 || rotation == 270)
                    return new Rectangle(0, 0, ImageShown.Height, ImageShown.Width);
                else
                    return new Rectangle(0, 0, ImageShown.Width, ImageShown.Height);
            }
            return Rectangle.Empty;
        }
    }
}
