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
    public partial class SaveMenu : UserControl {

        public event EventHandler<CamViewerMessageEventArgs> SaveMenuCondition;

        public SaveMenu() {
            InitializeComponent();

            rbtGood.Text = frmBase.UIStrings.GetString("Good");
            rbtReject.Text = frmBase.UIStrings.GetString("Reject");
            rbtAny.Text = frmBase.UIStrings.GetString("Any");
            //rbtPUM.Text = frmBase.UIStrings.GetString("PUM");
            //label2.Text = frmBase.UIStrings.GetString("Head").ToUpper();
            label3.Text = frmBase.UIStrings.GetString("Condition").ToUpper();
            label1.Text = frmBase.UIStrings.GetString("ToSave").ToUpper();
            btnExit.Text = frmBase.UIStrings.GetString("Exit");
        }

        public void UncheckAll() {

            rbtGood.Checked = false;
            rbtReject.Checked = false;
            rbtAny.Checked = false;
        }

        private void rbtGood_CheckedChanged(object sender, EventArgs e) {

            if (rbtGood.Checked)
                OnSaveMenuCondition(this, new CamViewerMessageEventArgs("Good", ntbHowMuch.Text));
        }

        private void rbtReject_CheckedChanged(object sender, EventArgs e) {

            if (rbtReject.Checked)
                OnSaveMenuCondition(this, new CamViewerMessageEventArgs("Reject", ntbHowMuch.Text));
        }

        private void rbtAny_CheckedChanged(object sender, EventArgs e) {
            
            if (rbtAny.Checked)
                OnSaveMenuCondition(this, new CamViewerMessageEventArgs("Any", ntbHowMuch.Text));
        }

        private void btnToSaveUp_Click(object sender, EventArgs e) {

            if (ntbHowMuch.Value < ntbHowMuch.Maximum)
                ntbHowMuch.Value += 1;
            ntbHowMuch.Validate();
            ntbHowMuch.Focus();
        }

        private void btnToSaveDown_Click(object sender, EventArgs e) {

            if (ntbHowMuch.Value > ntbHowMuch.Minimum)
                ntbHowMuch.Value -= 1;
            ntbHowMuch.Validate();
            ntbHowMuch.Focus();
        }

        private void btnExitStopCondMenu_Click(object sender, EventArgs e) {

            OnSaveMenuCondition(this, new CamViewerMessageEventArgs("Exit"));
        }

        protected virtual void OnSaveMenuCondition(object sender, CamViewerMessageEventArgs e) {

            if (SaveMenuCondition != null) SaveMenuCondition(sender, e);
        }

    }
}
