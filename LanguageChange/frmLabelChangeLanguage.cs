using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LanguageChange {
    public partial class frmLabelChangeLanguage : Form {

        Thread waitingTh;
        bool exit = false;

        public frmLabelChangeLanguage() {

            InitializeComponent();
            waitingTh = new Thread(new ThreadStart(ExactaEasySentinelThread));
            waitingTh.Start();
        }

        private void frmLabel_KeyDown(object sender, KeyEventArgs e) {

            if (e.Control && e.KeyCode == Keys.H) {
                Trace.WriteLine("Language change: CTRL + H pressed");
                Destroy();
            }
        }

        private void frmLabel_FormClosed(object sender, FormClosedEventArgs e) {

            //pbLoading.Value = 100;
            //pbLoading.Refresh();
            //Thread.Sleep(500);
            Destroy();
        }

        private void Destroy() {

            if (this.InvokeRequired) {
                this.Invoke(new MethodInvoker(() => Destroy()));
            }
            else {
                exit = true;
                if ((waitingTh != null) && (waitingTh.IsAlive == true)) {
                    waitingTh.Join(5000);
                }
                this.Close();
            }
        }

        void ExactaEasySentinelThread() {

            DateTime startTime = DateTime.Now;
            bool SPVAliveCheck = false;
            while (exit == false) {
                DateTime currentTime = DateTime.Now;
                double timeElapsed = (currentTime - startTime).TotalSeconds;
                if (timeElapsed > 30 && SPVAliveCheck==false) {
                    Process[] pname = Process.GetProcessesByName("ExactaEasy");
                    if (pname.Length == 0) {
                        string processPath = Environment.CurrentDirectory + @"\" + "ExactaEasy.exe";
                        if (File.Exists(processPath) == true) {
                            Process.Start(processPath);
                            Trace.WriteLine("Language change launched ExactaEasy");
                        }
                    }
                    SPVAliveCheck = true;
                }
                if (timeElapsed > 60) {
                    Trace.WriteLine("Language change: Timeout");
                    break;
                }
                double percent = ((int)(Math.Max(Math.Min(timeElapsed / 60 * 100, 100), 0) / 10)) * 10;
                //refreshProgressBar(percent);
                Thread.Sleep(100);
            }
            if (exit == false) {
                Destroy();
            }
        }

        //void refreshProgressBar(double percent) {

        //    if (pbLoading.InvokeRequired) {
        //        this.Invoke(new MethodInvoker(() => refreshProgressBar(percent)));
        //    }
        //    else {
        //        pbLoading.Value = (int)(percent + 0.5);
        //    }
        //}

    }
}
