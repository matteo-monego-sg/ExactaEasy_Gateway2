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
    public partial class StopOnCond : UserControl {

        public event EventHandler ConditionUpdated;

        Camera _camera;

        [Browsable(false)]
        public int SpindleCount {
            get {
                return (int)ntbHead.Maximum;
            }
            set {
                ntbHead.Maximum = value;
            }
        }

        [Browsable(true)]
        public int MaxTimeoutSec { get; set; }

        public StopOnCond() {
            InitializeComponent();

            rbtGood.Text = frmBase.UIStrings.GetString("Good");
            rbtReject.Text = frmBase.UIStrings.GetString("Reject");
            rbtAny.Text = frmBase.UIStrings.GetString("Any");
            rbtPUM.Text = frmBase.UIStrings.GetString("PUM");
            label2.Text = frmBase.UIStrings.GetString("Head").ToUpper();
            label3.Text = frmBase.UIStrings.GetString("Condition").ToUpper();
            label1.Text = frmBase.UIStrings.GetString("Timeout").ToUpper() + " [sec]";
            btnExitStopCondMenu.Text = frmBase.UIStrings.GetString("Exit");
        }

        public void SetCamera(Camera camera) {
            _camera = camera;
        }

        public void UncheckedAllButton() {
            foreach (Control rdb in this.Controls) {
                if (rdb is RadioButton)
                    ((RadioButton)rdb).Checked = false;
            }
        }

        private void stopOnConditionReturn(int condition) {
            int headNumber = 0;
            int timeout = 0;
            try {
                headNumber = Convert.ToInt32(ntbHead.Value);
                timeout = Convert.ToInt32(ntbTimeout.Value);
                if (_camera.GetCameraProcessingMode() == CameraProcessingMode.Processing)
                     _camera.SetStopCondition(headNumber, condition, timeout);
                else
                {
                //TODO: gestire il corretto start dell'analisi della camera senza funzioni "pagliative"
                }
                //else {
                //    _camera.StopAnalysisOffline();
                //    int noSleepCounter = 0;
                //    //AT/mod "Servono circa 600 ms perchè la camera torni alla normale modalità di funzionamento"
                //    while (noSleepCounter < 10) {
                //        if (_camera.GetCameraProcessingMode() == CameraProcessingMode.Processing) {
                //            _camera.SetStopCondition(headNumber, condition, timeout);
                //            break;
                //        }
                //        Thread.Sleep(100);
                //        noSleepCounter++;
                //    }
                //}
            }
            catch (FormatException formatEx) {
                headNumber = -1;
                Log.Line(LogLevels.Error, "StopOnCondition.stopOnConditionReturn", "Error: " + formatEx.Message);
            }
            catch (CameraException ex) {
                Log.Line(LogLevels.Error, "StopOnCondition.stopOnConditionReturn", "Error: " + ex.Message);
            }

            if (ConditionUpdated != null)
                ConditionUpdated(this, EventArgs.Empty);

            this.Visible = false;
        }

        private void rbtGood_CheckedChanged(object sender, EventArgs e) {
            const int checkCondition = 0;
            if (rbtGood.Checked)
                stopOnConditionReturn(checkCondition);
        }

        private void rbtReject_CheckedChanged(object sender, EventArgs e) {
            const int checkCondition = 1;
            if (rbtReject.Checked)
                stopOnConditionReturn(checkCondition);
        }

        private void rbtAny_CheckedChanged(object sender, EventArgs e) {
            const int checkCondition = 2;
            if (rbtAny.Checked)
                stopOnConditionReturn(checkCondition);
        }

        private void rbtPUM_CheckedChanged(object sender, EventArgs e) {
            const int checkCondition = 3;
            if (rbtPUM.Checked)
                stopOnConditionReturn(checkCondition);
        }

        private void bttHeadUp_Click(object sender, EventArgs e) {

            if (ntbHead.Value < ntbHead.Maximum)
                ntbHead.Value += 1;
            ntbHead.Validate();
            ntbHead.Focus();
        }

        private void bttHeadDown_Click(object sender, EventArgs e) {

            if (ntbHead.Value > ntbHead.Minimum)
                ntbHead.Value -= 1;
            ntbHead.Validate();
            ntbHead.Focus();
        }

        private void bttTimeoutUp_Click(object sender, EventArgs e) {

            if (ntbTimeout.Value < ntbTimeout.Maximum)
                ntbTimeout.Value += 1;
            ntbTimeout.Validate();
            ntbTimeout.Focus();
        }

        private void bttTimeoutDown_Click(object sender, EventArgs e) {

            if (ntbTimeout.Value > ntbTimeout.Minimum)
                ntbTimeout.Value -= 1;
            ntbTimeout.Validate();
            ntbTimeout.Focus();
        }

        private void btnExitStopCondMenu_Click(object sender, EventArgs e) {

            this.Visible = false;
        }

    }
}
