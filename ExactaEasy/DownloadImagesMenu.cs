using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DisplayManager;
using System.Threading;
using SPAMI.Util.Logger;
using ExactaEasyEng;
using System.Threading.Tasks;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using ExactaEasyCore;

namespace ExactaEasy {
    public partial class DownloadImagesMenu : UserControl {

        Camera _camera;
        //Cam _dataSource;
        string saveFolderRoot, openFolderRoot;
        Display _display;
        public event EventHandler<CamViewerMessageEventArgs> ConditionUpdated;
        public event EventHandler<CamViewerErrorEventArgs> Error;
        public bool BtnFramesEnabled {
            get {
                return btnFramesDownload.Enabled;
            }
            set {
                btnFramesDownload.Enabled = value;
            }
        }
        public bool BtnCurrentEnabled {
            get {
                return btnCurrentResDownload.Enabled;
            }
            set {
                btnCurrentResDownload.Enabled = value;
            }
        }
        public bool BtnRejectsBuffer {
            get {
                return btnRejectsBufferDownload.Enabled;
            }
            set {
                btnRejectsBufferDownload.Enabled = value;
            }
        }

        public DownloadImagesMenu() {

            InitializeComponent();

            label2.Text = frmBase.UIStrings.GetString("ImagesDownloads").ToUpper();
            label1.Text = frmBase.UIStrings.GetString("ImagesUploads").ToUpper();
            btnFramesDownload.Text = frmBase.UIStrings.GetString("Frames");
            btnCurrentResDownload.Text = frmBase.UIStrings.GetString("Current");
            btnRejectsBufferDownload.Text = frmBase.UIStrings.GetString("RejectsBuffer");
            btnFramesUpload.Text = frmBase.UIStrings.GetString("Upload");
            btnExitImagesDownloadsMenu.Text = frmBase.UIStrings.GetString("Exit");
        }

        public void SetCamera(Camera camera) {

            _camera = camera;
        }

        public void SetDisplay(Display display) {

            _display = display;
        }

        void OnConditionUpdated(object sender, CamViewerMessageEventArgs e) {
            if (ConditionUpdated != null)
                ConditionUpdated(sender, e);
        }

        void OnError(object sender, CamViewerErrorEventArgs e) {
            if (Error != null)
                Error(sender, e);
        }

        private void btnFramesDownload_Click(object sender, EventArgs e) {

            try {
                CameraWorkingMode camWorkingMode = _camera.GetWorkingMode();
                if (camWorkingMode != CameraWorkingMode.Timed) {
                    saveFolderRoot = openFolderBrowserDialog(frmBase.UIStrings.GetString("SelectFolderForSavingImages"));
                    OnConditionUpdated(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("DownloadingImages") + "..."));
                    Task downloadImgsTask = new Task(new Action(downloadImages));
                    downloadImgsTask.Start();
                }
                else {
                    Log.Line(LogLevels.Error, "DownloadImagesMenu.btnFramesDownload_Click", _camera.IP4Address + ": Camera in live");
                    OnConditionUpdated(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("ImagesNotDownloaded" + "!")));
                }
            }
            catch (CameraException ex) {
                Log.Line(LogLevels.Error, "DownloadImagesMenu.btnFramesDownload_Click", _camera.IP4Address + ": Error: " + ex.Message);
                OnConditionUpdated(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("ImagesNotDownloaded" + "!")));
            }
        }

        private void btnCurrentResDownload_Click(object sender, EventArgs e) {

            if (_display.ImageShown != null) {
                using (SaveFileDialog saveFileDlg = new SaveFileDialog()) {
                    saveFileDlg.RestoreDirectory = true;
                    saveFileDlg.Filter = "BMP File (*.bmp)|*.bmp";
                    if (DialogResult.OK == saveFileDlg.ShowDialog()) {
                        try {
                            _display.ImageShown.Save(saveFileDlg.FileName);
                            Log.Line(LogLevels.Pass, "DownloadImagesMenu.btnCurrentResDownload_Click", _camera.IP4Address + ": Image saved");
                            OnConditionUpdated(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("ImageSaved")));
                        }
                        catch (Exception ex) {
                            Log.Line(LogLevels.Error, "DownloadImagesMenu.btnCurrentResDownload_Click", _camera.IP4Address + ": Error: " + ex.Message);
                            OnConditionUpdated(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("ImagesNotDownloaded" + "!")));
                        }
                    }
                }
            }
            else {
                Log.Line(LogLevels.Error, "DownloadImagesMenu.btnCurrentResDownload_Click", _camera.IP4Address + ": Error: No current image");
                OnConditionUpdated(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("ImagesNotDownloaded" + "!")));
            }
        }

        private void btnRejectsBufferDownload_Click(object sender, EventArgs e) {

            try {
                saveFolderRoot = openFolderBrowserDialog(frmBase.UIStrings.GetString("SelectFolderForSavingImages"));
                OnConditionUpdated(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("DownloadingImages") + "..."));
                Task downloadImgsTask = new Task(new Action(downloadRejectsImages));
                downloadImgsTask.Start();
            }
            catch (CameraException ex) {
                Log.Line(LogLevels.Error, "DownloadImagesMenu.btnFramesDownload_Click", _camera.IP4Address + ": Error: " + ex.Message);
                OnConditionUpdated(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("ImagesNotDownloaded" + "!")));
            }

        }

        string openFolderBrowserDialog(string description) {

            FolderBrowserDialog folderBrowserDlg = new FolderBrowserDialog();
            folderBrowserDlg.Description = description;
            if (DialogResult.OK == folderBrowserDlg.ShowDialog()) {
                if (Directory.Exists(folderBrowserDlg.SelectedPath)) {
                    return folderBrowserDlg.SelectedPath;
                }
            }
            return null;
        }

        void downloadRejectsImages() {
            //try {
            //    if (saveFolderRoot != null) {
            //        Directory.CreateDirectory(saveFolderRoot);
            //        (_camera as INode).SaveBufferedImages(saveFolderRoot + "\\", SaveConditions.Reject);
            //        OnConditionUpdated(this, new CamViewerMessageEventArgs("DownloadedImagesFolder", saveFolderRoot));
            //    }
            //    else
            //        OnConditionUpdated(this, new CamViewerMessageEventArgs("DownloadedImagesFolder", null));
            //}
            //catch {
            //    OnConditionUpdated(this, new CamViewerMessageEventArgs("DownloadedImagesFolder", null));
            //}
        }

        void downloadImages() {
            try {
                if (saveFolderRoot != null) {
                    string outDir = _camera.DownloadImages(saveFolderRoot, AppEngine.Current.CurrentContext.UserLevel);
                    OnConditionUpdated(this, new CamViewerMessageEventArgs("DownloadedImagesFolder", outDir));
                }
                else
                    OnConditionUpdated(this, new CamViewerMessageEventArgs("DownloadedImagesFolder", null));
            }
            catch {
                OnConditionUpdated(this, new CamViewerMessageEventArgs("DownloadedImagesFolder", null));
            }
        }

        private void btnExitImagesDownloadsMenu_Click(object sender, EventArgs e) {

            this.Visible = false;
        }

        private void btnFramesUpload_Click(object sender, EventArgs e) {

            try {
                CameraWorkingMode camWorkingMode = _camera.GetWorkingMode();
                if (camWorkingMode != CameraWorkingMode.Timed) {
                    openFolderRoot = openFolderBrowserDialog(frmBase.UIStrings.GetString("SelectFolderFromWhichToUpload"));
                    OnConditionUpdated(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("UploadingImages") + "..."));
                    Task uploadImgsTask = new Task(new Action(uploadImages));
                    uploadImgsTask.Start();
                }
                else {
                    Log.Line(LogLevels.Error, "DownloadImagesMenu.btnFramesUpload_Click", _camera.IP4Address + ": Camera in live");
                    OnConditionUpdated(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("ImagesNotUploaded" + "!")));
                }
            }
            catch (CameraException ex) {
                Log.Line(LogLevels.Error, "DownloadImagesMenu.btnFramesUpload_Click", _camera.IP4Address + ": Error: " + ex.Message);
                OnConditionUpdated(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("ImagesNotUploaded" + "!")));
            }
        }

        void uploadImages() {
            try {
                if (openFolderRoot != null) {
                    string outDir = _camera.UploadImages(openFolderRoot, AppEngine.Current.CurrentContext.UserLevel);
                    OnConditionUpdated(this, new CamViewerMessageEventArgs("DownloadedImagesFolder", outDir));
                }
                else
                    OnConditionUpdated(this, new CamViewerMessageEventArgs("DownloadedImagesFolder", null));
            }
            catch {
                OnConditionUpdated(this, new CamViewerMessageEventArgs("DownloadedImagesFolder", null));
            }
        }
    }
}
