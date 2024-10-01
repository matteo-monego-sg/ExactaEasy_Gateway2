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

namespace ExactaEasy {
    public partial class CamResetMenu : UserControl {

        Camera _camera;
        Cam _dataSource;
        public event EventHandler<CamViewerMessageEventArgs> ConditionUpdated;
        public event EventHandler<CamViewerErrorEventArgs> Error;
        public event EventHandler ApplyParameters;

        public CamResetMenu() {
            InitializeComponent();

            label2.Text = frmBase.UIStrings.GetString("Reset").ToUpper();
            btnSoftReset.Text = frmBase.UIStrings.GetString("SoftReset");
            btnHardReset.Text = frmBase.UIStrings.GetString("HardReset");
            btnExitResetMenu.Text = frmBase.UIStrings.GetString("Exit");
        }

        public void SetCamera(Camera camera) {
            _camera = camera;
        }

        public void SetDataSource(Cam dataSource) {
            _dataSource = dataSource;
        }

        void OnConditionUpdated(object sender, CamViewerMessageEventArgs e) {
            if (ConditionUpdated != null)
                ConditionUpdated(sender, e);
        }

        void OnError(object sender, CamViewerErrorEventArgs e) {
            if (Error != null)
                Error(sender, e);
        }

        void OnApplyParameters(object sender, EventArgs e) {
            if (ApplyParameters != null)
                ApplyParameters(sender, e);
        }

        private void btnSoftReset_Click(object sender, EventArgs e) {

            //resetCameraWarning();
            try {
                _camera.SoftReset();
                Log.Line(LogLevels.Pass, "CamResetMenu.btnSoftReset_Click", _camera.IP4Address + ": Camera SOFT reset completed successfully");
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "CamResetMenu.btnSoftReset_Click", _camera.IP4Address + ": SOFT Reset error: " + ex.Message);
                OnError(this, new CamViewerErrorEventArgs(_camera, _camera.IP4Address + ": " + frmBase.UIStrings.GetString("ResetError")));
            }
        }

        private void btnHardReset_Click(object sender, EventArgs e) {

            try {
                btnHardReset.Enabled = false;
                Log.Line(LogLevels.Pass, "CamResetMenu.btnHardReset_Click", _camera.IP4Address + ": Starting camera HARD reset...");
                OnConditionUpdated(this, new CamViewerMessageEventArgs("CameraResetBegin", "0"));
                (_camera as IStation).HardReset();
                if (_dataSource != null)
                    OnApplyParameters(this, EventArgs.Empty);
                OnConditionUpdated(this, new CamViewerMessageEventArgs("CameraResetEnd", "0"));
                Log.Line(LogLevels.Pass, "CamResetMenu.btnHardReset_Click", _camera.IP4Address + ": Camera HARD reset completed successfully");
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "CamResetMenu.btnHardReset_Click", _camera.IP4Address + ": HARD Reset error: " + ex.Message);
                OnError(this, new CamViewerErrorEventArgs(_camera, _camera.IP4Address + ": " + frmBase.UIStrings.GetString("ResetError")));
            }
            finally {
                btnHardReset.Enabled = true;
            }
        }

        private void btnExitResetMenu_Click(object sender, EventArgs e) {

            this.Visible = false;
        }
    }
}
