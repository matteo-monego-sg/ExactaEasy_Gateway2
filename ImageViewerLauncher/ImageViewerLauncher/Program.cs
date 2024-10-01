using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ImageViewerLauncher {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {

            using (Mutex mutex = new Mutex(false, "Global\\" + appGuid)) {

                if (!mutex.WaitOne(0, false)) {
                    Debug.WriteLine("Instance already running");
                    //MessageBox.Show("Instance already running", "FATAL ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain());
            }
        }

        static string appGuid = "LaXXXinaNonEunAnimaleIntelligente";  //Assembly.GetExecutingAssembly().GetType().GUID.ToString();
    }
}
