using ExactaEasyCore;
using ExactaEasyEng.Utilities;
using SPAMI.Util.Logger;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ExactaEasy
{

    public partial class frmSplash : Form {

        public string Message {
            get {
                return lblWaitTxt.Text;
            }
            set {
                lblWaitTxt.Text = value;
            }
        }

        public bool ProgrammaticClose { get; set; }

        System.Timers.Timer closeSplashTimer;

        public frmSplash() {

            InitializeComponent();
            dgvTasks.ColumnHeadersBorderStyle = properColumnHeadersBorderStyle;
            dgvTasks.Font = new System.Drawing.Font("Nirmala UI", 11.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dgvTasks.BackgroundColor = Color.Black;
            dgvTasks.DefaultCellStyle.BackColor = Color.Black;
            dgvTasks.DefaultCellStyle.ForeColor = Color.White;
            dgvTasks.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(3);
            //timerCheck.Start();
            TaskObserver.TheObserver.DispatchObservation += new EventHandler<TaskStatusEventArgs>(TheObserver_DispatchObservation);
            foreach (TaskInfo taskInfo in TaskObserver.TheObserver.Tasks) {
                object[] newLine = new object[] { taskInfo.TaskId, taskInfo.AdditionalInfo, getIcon(-1) };
                dgvTasks.Rows.Add(newLine);
                //if ((taskInfo.TaskStatus != TaskStatus.Completed) && (taskInfo.TaskStatus != TaskStatus.Failed)) {
                //    exit = false;
                //}
            }
            //tryExit();
        }

        public void UpdateWaitPanel(string message) 
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (InvokeRequired && IsHandleCreated)
                Invoke(new MethodInvoker(() => UpdateWaitPanel(message)));
            else {
                Message = message;
                pnlWait.Visible = true;
                pnlTasks.Visible = false;
                pnlWait.BringToFront();
            }
        }

        public void HideWaitPanel() 
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (InvokeRequired && IsHandleCreated)
                Invoke(new MethodInvoker(HideWaitPanel));
            else {
                pnlWait.Visible = false;
                pnlTasks.Visible = true;
                pnlTasks.BringToFront();
            }
        }

        int checkHW(string taskID) {
            if (TaskObserver.TheObserver.Tasks[taskID].TaskStatus == TaskStatus.Completed)
                return 1;
            return 0;
        }

        void updateTaskStatus(string id, TaskStatus ts) 
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(dgvTasks))
                return;

            if (dgvTasks.InvokeRequired && dgvTasks.IsHandleCreated) {
                dgvTasks.Invoke(new MethodInvoker(() => updateTaskStatus(id, ts)));
            }
            else {
                int rowIndex = -1;
                TaskInfo taskInfo = TaskObserver.TheObserver.Tasks[id];
                IEnumerable<DataGridViewRow> rows = dgvTasks.Rows
                      .Cast<DataGridViewRow>()
                      .Where(r => r.Cells[colID.Name].Value.ToString().Equals(id));
                if (rows != null && rows.Count() > 0) {
                    DataGridViewRow row = rows.First<DataGridViewRow>();

                    rowIndex = row.Index;
                    dgvTasks.Rows[rowIndex].Cells[colDescription.Name].Value = taskInfo.AdditionalInfo;
                    int iconId = (ts == TaskStatus.Running) ? -1 : checkHW(taskInfo.TaskId);
                    dgvTasks.Rows[rowIndex].Cells[colStatus.Name].Value = getIcon(checkHW(taskInfo.TaskId));
                    dgvTasks.Refresh();
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e) {
            this.Close();
        }

        bool exit = false;
        void TheObserver_DispatchObservation(object sender, TaskStatusEventArgs e) {
            Log.Line(LogLevels.Debug, "frmSplash.TheObserver_DispatchObservation", "SPLASH TASK " + e.TaskID + ": " + e.TaskStatus.ToString());
            Debug.WriteLine("SPLASH TASK " + e.TaskID + ": " + e.TaskStatus.ToString());
            updateTaskStatus(e.TaskID, e.TaskStatus);
            //tryExit();    //pier: 30/12/2014 COMMENTATO PER CHIUDERE DA FUORI
        }

        void tryExit() {
            exit = true;
            foreach (TaskInfo taskInfo in TaskObserver.TheObserver.Tasks) {
                if ((taskInfo.TaskStatus != TaskStatus.Completed) && (taskInfo.TaskStatus != TaskStatus.Failed)) {
                    exit = false;
                }
            }
            if (exit && (closeSplashTimer == null)) {
                closeSplashTimer = new System.Timers.Timer(3 * 1000D);
                closeSplashTimer.Elapsed += new System.Timers.ElapsedEventHandler(closeSplashTimer_Elapsed);
                closeSplashTimer.Enabled = true;
            }
        }

        void refreshTaskGrid() 
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(dgvTasks))
                return;

            if (dgvTasks.InvokeRequired && dgvTasks.IsHandleCreated)
                dgvTasks.Invoke(new MethodInvoker(refreshTaskGrid));
            else
                dgvTasks.Refresh();
        }

        static Bitmap getIcon(int status) {

            switch (status) {
                case -1:
                    return global::ExactaEasy.Properties.Resources.bluQuestionMark1;
                case 0:
                    return global::ExactaEasy.Properties.Resources.button_fewer1;
                case 1:
                    return global::ExactaEasy.Properties.Resources.checkmark1;
                default:
                    return global::ExactaEasy.Properties.Resources.dialog_warning1;
            }
        }

        static DataGridViewHeaderBorderStyle properColumnHeadersBorderStyle {
            get {
                return (SystemFonts.MessageBoxFont.Name == "Segoe UI") ?
                DataGridViewHeaderBorderStyle.None :
                DataGridViewHeaderBorderStyle.Raised;
            }
        }


        //private void timerCheck_Tick(object sender, EventArgs e) {

        //    bool cumulStatus = true;
        //    foreach (SplashTask st in splashTasks) {
        //        if (st.Update)
        //            updateTaskStatus(st.ID);
        //        if (!st.Status.Value) {
        //            cumulStatus = false;
        //        }
        //    }
        //    if (!cumulStatus && closeSplashTimer != null) {
        //        closeSplashTimer.Enabled = false;
        //        closeSplashTimer = null;
        //    }
        //    if (cumulStatus && closeSplashTimer == null) {
        //        closeSplashTimer = new System.Timers.Timer(3 * 1000D);
        //        closeSplashTimer.Elapsed += new System.Timers.ElapsedEventHandler(closeSplashTimer_Elapsed);
        //        closeSplashTimer.Enabled = true;
        //    }
        //}

        void closeSplashTimer_Elapsed(object sender, EventArgs e) 
        {
            //timerCheck.Stop();
            if (closeSplashTimer != null) {
                closeSplashTimer.Enabled = false;
                closeSplashTimer = null;
            }

            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (InvokeRequired && IsHandleCreated)
                this.Invoke(new MethodInvoker(Close));
            else
                Close();
        }

        private void dgvTasks_SelectionChanged(object sender, EventArgs e) {
            dgvTasks.ClearSelection();
        }

        private void frmSplash_FormClosed(object sender, FormClosedEventArgs e) {
            TaskObserver.TheObserver.DispatchObservation -= TheObserver_DispatchObservation;
        }

        private void frmSplash_FormClosing(object sender, FormClosingEventArgs e) {

            if (e.CloseReason == CloseReason.UserClosing && ProgrammaticClose == false)
                e.Cancel = true;
        }
    }

    //public class SplashTask {

    //    public bool Update {
    //        get {
    //            bool ret = (Status.Value != prevStatus.Value);
    //            prevStatus.Value = Status.Value;
    //            return ret;
    //        }
    //    }

    //    public string ID;
    //    public string Description;
    //    public Checker Status;
    //    Checker prevStatus;

    //    public SplashTask(ref Checker status) {
    //        Status = status;
    //        prevStatus = new Checker();
    //    }
    //}

    //public class SplashTaskCollection : List<SplashTask> {

    //    public SplashTask this[string id] {
    //        get { return this.Find(st => { return st.ID == id; }); }
    //    }
    //}
}
