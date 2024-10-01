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

namespace ExactaEasy {
    public partial class VialAxisMenu : UserControl {

        Camera _camera;
        public event EventHandler<CamViewerMessageEventArgs> ConditionUpdated;

        public VialAxisMenu() {
            InitializeComponent();

            label2.Text = frmBase.UIStrings.GetString("BackupRestore");
            btnExitVialAxisMenu.Text = frmBase.UIStrings.GetString("Exit");
            btnBackup.Text = frmBase.UIStrings.GetString("Backup");
            btnRestore.Text = frmBase.UIStrings.GetString("Restore");
        }

        public void SetCamera(Camera camera) {
            _camera = camera;
        }

        private void btnBackup_Click(object sender, EventArgs e) {

            using (SaveFileDialog saveFileDlg = new SaveFileDialog()) {
                saveFileDlg.RestoreDirectory = true;
                saveFileDlg.Filter = "XML File (*.xml)|*.xml";
                if (DialogResult.OK == saveFileDlg.ShowDialog()) {
                    try {
                        _camera.ExportParameters(saveFileDlg.FileName);
                        Log.Line(LogLevels.Pass, "VialAxisMenu.btnBackup_Click", _camera.IP4Address + ": camera vial axis saved successfully");
                        OnConditionUpdated(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("VialAxisExportedOk")));
                    }
                    catch (Exception ex) {
                        Log.Line(LogLevels.Error, "VialAxisMenu.btnBackup_Click", "Error: " + ex.Message);
                        OnConditionUpdated(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("BackupVialAxisFailed")));
                    }
                }
            }
        }

        private void btnRestore_Click(object sender, EventArgs e) {

            using (OpenFileDialog oDialog = new OpenFileDialog()) {
                oDialog.RestoreDirectory = true;
                oDialog.Filter = "Recipe Files (*.xml)|*.xml";
                if (oDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    try {
                        _camera.ImportParameters(oDialog.FileName);
                        Log.Line(LogLevels.Pass, "VialAxisMenu.btnRestore_Click", _camera.IP4Address + ": camera vial axis backuped successfully");
                        OnConditionUpdated(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("VialAxisImportedOk")));
                    }
                    catch (Exception ex) {
                        Log.Line(LogLevels.Error, "VialAxisMenu.btnRestore_Click", "Error: " + ex.Message);
                        OnConditionUpdated(this, new CamViewerMessageEventArgs(frmBase.UIStrings.GetString("RestoreVialAxisFailed")));
                    }
                }
            }
        }

        void OnConditionUpdated(object sender, CamViewerMessageEventArgs e) {
            if (ConditionUpdated != null)
                ConditionUpdated(sender, e);
        }

        private void btnExitVialAxisMenu_Click(object sender, EventArgs e) {

            this.Visible = false;
        }
    }
}
