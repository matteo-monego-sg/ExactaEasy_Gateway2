using DisplayManager;
using ExactaEasyCore;
using ExactaEasyEng;
using ExactaEasyEng.Utilities;
using Microsoft.Win32;
using SPAMI.Util.Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExactaEasy
{
    static class Program 
    {
        ///// <summary>
        ///// fmt (EtherComm dependency) DLL file name. Used for version check.
        ///// </summary>
        //private const string FMT_LIBRARY_NAME = "fmt.dll";
        ///// <summary>
        ///// spdlog (EtherComm dependency) DLL file name. Used for version check.
        ///// </summary>
        //private const string SPDLOG_LIBRARY_NAME = "spdlog.dll";
        ///// <summary>
        ///// EtherComm DLL file name. Used for version check.
        ///// </summary>
        //private const string ETHERCOMM_LIBRARY_NAME = "ethercomm.dll";
        ///// <summary>
        ///// Base version of EtherComm.dll required for the program to operate.
        ///// </summary>
        //private const string ETHERCOMM_VERSION_BASE = "1.1.2.0";

        static frmSplash splashScreen;
        static VisionSystemManager vsm = null;

        static bool showTransparentHeader = false;
        static bool showTransparentFooter = false;
        static bool hideUI = true;
        static bool changeLanguage = false;

        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

            //NodeRecipe nr = Recipe.LoadFromFile("BootRecipe.xml"); //rimuovere
            //Recipe testRecipe = new Recipe();                   //rimuovere
            //testRecipe.Nodes = new List<NodeRecipe>();          //rimuovere
            //testRecipe.Nodes.Add(nr);                           //rimuovere
            //Cam c = new Cam();
            //c.Id = 999;
            //c.Program = "XXXXXXX";
            //testRecipe.Cams = new List<Cam>();
            //testRecipe.Cams.Add(c);
            //testRecipe.SaveXml("BootRecipe_out_scada.xml");   //rimuovere
            //Recipe.SaveXmlV2("BootRecipe_out.xml", nr);
            //Recipe.SaveXmlV2("prova_1.xml", nr);
            Trace.WriteLine("ExactaEasy START");

            foreach (string arg in args) {
                if (arg.ToLower().Contains("-changelanguage")) {
                    Trace.WriteLine("Change language");
                    changeLanguage = true;
                    //hideUI = true;
                }
                if (arg.ToLower().Contains("-showtransparentheader"))
                    showTransparentHeader = true;
                if (arg.ToLower().Contains("-showtransparentfooter"))
                    showTransparentFooter = true;
                if (arg.ToLower().Contains("-hideui"))
                    hideUI = true;
            }
            bool hasHandle = false;
            using (Mutex mutex = new Mutex(false, "Global\\" + appGuid)) {

                if (changeLanguage == false)
                    hasHandle = mutex.WaitOne(0, false);
                else
                    hasHandle = mutex.WaitOne(20000, false);
                if (hasHandle == false) {
                    Trace.WriteLine("Instance already running");
                    return;
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Executes a check against a specific version of EtherComm.dll.
                //if (!CheckEtherCommVersion(ETHERCOMM_VERSION_BASE, out string log))
                //{
                //    Log.Line(LogLevels.Error, "Program.Main", log);
                //    MessageBox.Show(
                //        log, 
                //        "FATAL ERROR",
                //        MessageBoxButtons.OK, 
                //        MessageBoxIcon.Error);
                //    return;
                //}
                //else
                //{
                //    Log.Line(LogLevels.Debug, "Program.Main", log);
                //}

                try {
                    vsm = VisionSystemManager.CreateVisionSystemManager(AppEngine.Current.MachineConfiguration);
                }
                catch {
                    MessageBox.Show("Cannot find or wrong global (\"globalConfig.xml\") or machine (\"machineConfig.xml\") configuration file . Check it and restart application.", "FATAL ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (hasHandle == true)
                        mutex.ReleaseMutex();
                    return;
                }
                splashScreen = new frmSplash();
                if (AppEngine.Current.MachineConfiguration.ViewSplashScreen > 0)
                    splashScreen.TopMost = true;
                if (AppEngine.Current.MachineConfiguration.ViewSplashScreen < 0)
                    splashScreen.WindowState = FormWindowState.Minimized;
                splashScreen.Text = "Vision SPV splash screen";
                Task tkWait = new Task(new Action(() => { waitAtStartup(AppEngine.Current.MachineConfiguration.StartupDelaySec); }));
                tkWait.Start();
                Application.Run(splashScreen);

                //do {
                if (hideUI) {
                    frmMain.ForceWindowHidden = true;
                }
                frmMain.ChangeLanguage = changeLanguage;
                frmMain mainForm = new frmMain(vsm);
                if (showTransparentHeader)
                    mainForm.ShowTransparentHeader();
                if (showTransparentFooter)
                    mainForm.ShowTransparentFooter();
                
                frmMain.SendRecipeReq = !frmMain.ChangeLanguage;

                Process proc = null;
                Process[] pname = Process.GetProcessesByName("LanguageChange");
                if (pname.Length > 0) {
                    proc = pname[0];
                    proc.Kill();
                    Trace.WriteLine("ExactaEasy killed LanguageChange");
                }
                //IntPtr hwnd = FindWindow("LanguageChange", "");
                //SendMessage(hwnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                Application.Run(mainForm);
                if (frmMain.RestartForm == true) {
                    string languageChangePath = Environment.CurrentDirectory + @"\" + "LanguageChange.exe";
                    if (File.Exists(languageChangePath) == true) {
                        System.Diagnostics.Process.Start(languageChangePath);
                        Trace.WriteLine("ExactaEasy launched LanguageChange");
                    }
                    string arguments = "";
                    if (AppEngine.Current.CurrentContext.ActiveRecipe != null)
                        arguments = "-changelanguage";
                    System.Diagnostics.Process.Start(Application.ExecutablePath, arguments);
                    Trace.WriteLine("ExactaEasy launched ExactaEasy in change language mode");
                }
                //}
                //while (frmMain.RestartForm);
                if (frmMain.RestartForm == false) {
                    Task tk = new Task(new Action(() => { showClosingForm(); }));
                    tk.Start();
                }
                AppEngine.Current.StopEngine();
                vsm.Dispose();
                Log.Destroy();
                if (hasHandle == true)
                    mutex.ReleaseMutex();
            }
        }

        static void Current_ContextChanged(object sender, ContextChangedEventArgs e) {

            // LE STAZIONI HMI HANNO ID ASSOLUTO, NEL SUPERVISORE E' RELATIVO
            // QUANDO STAZIONI/ NODI / CAMERE VERRANNO ISTANZIATI ANCHE SE NON PRESENTI CONTROLLARE DIRETTAMENTE GLI OGGETTI
            // INVECE DEI SETTINGS
            if (e.ContextChanges == ContextChangesEnum.MachineInfo && vsm != null) {
                if (AppEngine.Current.MachineConfiguration.StationSettings != null) {
                    for (int sAbsId = 0; sAbsId < AppEngine.Current.CurrentContext.EnabledStation.Length; sAbsId++) {
                        if (sAbsId >= AppEngine.Current.MachineConfiguration.StationSettings.Count) continue;
                        StationSetting statSet = AppEngine.Current.MachineConfiguration.StationSettings[sAbsId];
                        if (vsm.Nodes != null && statSet.Node < vsm.Nodes.Count && vsm.Nodes[statSet.Node].Stations != null /*&& statSet.Id < vsm.Nodes[statSet.Node].Stations.Count*/)
                            vsm.Nodes[statSet.Node].Stations[statSet.Id].Enabled = AppEngine.Current.CurrentContext.EnabledStation[sAbsId];
                    }
                }
                else {
                    for (int i = 0; i < AppEngine.Current.CurrentContext.EnabledStation.Length; i++) {
                        if (i >= AppEngine.Current.MachineConfiguration.StationSettings.Count) continue;
                        IStation s = vsm.Stations.Find((IStation st) => { return st.IdStation == i; });
                        if (s != null)
                            s.Enabled = AppEngine.Current.CurrentContext.EnabledStation[i];
                    }
                }
            }
            if ((e.ContextChanges & ContextChangesEnum.DataBase) != 0) {
                vsm.OpenDatabase(AppEngine.Current.CurrentContext.DataBase);
            }
            if ((e.ContextChanges & ContextChangesEnum.MachineMode) != 0) {
                vsm.SetWorkingMode(AppEngine.Current.CurrentContext.MachineMode);
            }
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e) {

            StreamWriter w = new StreamWriter("TEX_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".log");
            w.WriteLine("--------THREAD EXCEPTION----------------------------------");
            if (System.Threading.Thread.CurrentThread != null && System.Threading.Thread.CurrentThread.Name != null)
                w.WriteLine("Thread name: " + System.Threading.Thread.CurrentThread.Name);
            writeException(e.Exception, w);
            w.Flush();
            w.Close();
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {

            StreamWriter w = new StreamWriter("UEX_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".log");
            w.WriteLine("--------UNHANDLED EXCEPTION-------------------------------");
            writeException((Exception)e.ExceptionObject, w);
            w.Flush();
            w.Close();
        }

        static void writeException(Exception ex, StreamWriter w) {

            w.WriteLine("Message: " + ex.Message);
            w.WriteLine("Source: " + ex.Source);
            if (ex.Data != null) {
                w.WriteLine("Data: ");
                w.WriteLine(ex.Data);
            }
            w.WriteLine("Stack trace: ");
            w.WriteLine(ex.StackTrace);
            if (ex.InnerException != null) {
                w.WriteLine("----------------------------------------------------------");
                writeException(ex.InnerException, w);
            }
        }

        static void waitAtStartup(int waitFromStartupSec) {

            //REGISTRY KEYS
            foreach (RegistryKeyCtrl regKey in AppEngine.Current.MachineConfiguration.RegKeyToCheckAtStartup) {
                if (!checkRegistryKey(regKey)) {//pier: parametrizzare
                    splashScreen.UpdateWaitPanel(regKey.Label + " " +
                        frmBase.UIStrings.GetString("Missing").ToUpper() + "!\n\n" +
                        frmBase.UIStrings.GetString("VisionSpvCannotBeExecuted").ToUpper() + " ...");
                    Thread.Sleep(10000);
                    closeSplashScreen();
                    Application.Exit();
                }
            }
            //WINDOWS SERVICES
            ServiceController[] scList = ServiceController.GetServices();
            List<string> scNames = new List<string>();
            foreach (ServiceController sc in scList)
                scNames.Add(sc.ServiceName);
            List<string> serviceNames = new List<string>();
            // COMMENTATO PER WINDOWS 10
            //if (scNames.Contains("Netman"))     //parametrizzare??
            //    serviceNames.Add("Netman");     //parametrizzare??
            if (scNames.Contains("Nla"))     //parametrizzare??
                serviceNames.Add("Nla");     //parametrizzare??
            if (scNames.Contains("NlaSvc"))     //parametrizzare??
                serviceNames.Add("NlaSvc");     //parametrizzare??
            List<ServiceController> serviceControllers = new List<ServiceController>();
            foreach (string scName in serviceNames)
                serviceControllers.Add(new ServiceController(scName));
            bool ok = false;
            while (!ok) {
                ok = true;
                bool notify = true;
                foreach (ServiceController sc in serviceControllers) {
                    sc.Refresh();
                    if (sc.Status != ServiceControllerStatus.Running) {
                        ok = false;
                        if (notify)
                            splashScreen.UpdateWaitPanel(frmBase.UIStrings.GetString("VisionSpvIsWaitingForSystemStartup").ToUpper() +
                                "\n\n" + frmBase.UIStrings.GetString("PleaseWaitWindowsService") + ":\n" + sc.DisplayName + " ...");
                        notify = false;
                    }
                }
                Thread.Sleep(200);
            }

            //TIMEOUT
            double timeoutMs = Math.Max(0, (waitFromStartupSec * 1000F) - (UpTime.TotalSeconds * 1000F));       // timeout in millisecondi
            while (timeoutMs > 0) {
                splashScreen.UpdateWaitPanel(frmBase.UIStrings.GetString("VisionSpvIsWaitingForSystemStartup").ToUpper() + "\n\n" + frmBase.UIStrings.GetString("PleaseWait") + " " + ((int)(timeoutMs / 1000F)).ToString() + "\" ...");
                Log.Line(LogLevels.Warning, "Program.waitAtStartup", "Wait {0} seconds for software startup...", timeoutMs / 1000);
                Thread.Sleep(1000);
                timeoutMs -= 1000;
            }

            splashScreen.HideWaitPanel();

            vsm.Init();
            vsm.StartStopBatch(null, false);

            try {
                AppEngine.Current.ContextChanged += new EventHandler<ContextChangedEventArgs>(Current_ContextChanged);
                AppEngine.Current.StartEngine();
                Thread.Sleep(5000); //pier: bisogna capire perchè serve (al momento è necessario) e rimuovere questo SLEEP!
            }
            catch {
            }

            closeSplashScreen();
        }

        static void closeSplashScreen() 
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(splashScreen))
                return;

            if (splashScreen.InvokeRequired && splashScreen.IsHandleCreated)
                splashScreen.Invoke(new MethodInvoker(closeSplashScreen));
            else {
                splashScreen.ProgrammaticClose = true;
                splashScreen.Close();
            }
        }

        static bool checkRegistryKey(RegistryKeyCtrl regKeyToCtrl) {

            RegistryKey root;
            bool res = false;
            switch (regKeyToCtrl.HKLM_or_HKCU.ToUpper()) {
                case "HKLM":
                    root = Registry.LocalMachine.OpenSubKey(regKeyToCtrl.Root, false);
                    break;
                case "HKCU":
                    root = Registry.CurrentUser.OpenSubKey(regKeyToCtrl.Root, false);
                    break;
                default:
                    Log.Line(LogLevels.Error, "Program.checkRegistryKey", "Parameter registry root must be either \"HKLM\" or \"HKCU\"");
                    return res;
            }
            if (root == null)
                return false;
            object value = root.GetValue(regKeyToCtrl.Value);
            if (value == null || regKeyToCtrl.ValueExpected.ToLower() != value.ToString().ToLower())
                return false;

            return true;
        }

        static TimeSpan UpTime {
            get {
                var ticks = Stopwatch.GetTimestamp();
                var uptime = ((double)ticks) / Stopwatch.Frequency;
                return TimeSpan.FromSeconds(uptime);
            }
        }

        static void showClosingForm() {
            frmClosing closingForm = new frmClosing();
            closingForm.TopMost = true;
            closingForm.ShowDialog();
        }
        /// <summary>
        /// EtherComm and its dependencies version check.
        /// </summary>
        //private static bool CheckEtherCommVersion(string version, out string log)
        //{
        //    // If no version is provided, then skips the check.
        //    if (string.IsNullOrWhiteSpace(version))
        //    {
        //        log = "EtherComm version check: skipped [no version to check against provided].";
        //        return true;
        //    }

        //    if (!File.Exists(ETHERCOMM_LIBRARY_NAME))
        //    {
        //        log = "EtherComm version check failed: missing 'ethercomm.dll'.";
        //        return false;
        //    }

        //    if (!File.Exists(SPDLOG_LIBRARY_NAME))
        //    {
        //        log = "EtherComm version check failed: missing dependency 'spdlog.dll'.";
        //        return false;
        //    }

        //    if (!File.Exists(FMT_LIBRARY_NAME))
        //    {
        //        log = "EtherComm version check failed: missing dependency 'fmt.dll'.";
        //        return false;
        //    }
        //    // This doesn't load the DLL into the application yet.
        //    var etherVer = FileVersionInfo.GetVersionInfo(ETHERCOMM_LIBRARY_NAME).FileVersion;
        //    if (string.IsNullOrWhiteSpace(etherVer))
        //    {
        //        log = "EtherComm version check failed: could not retrieve file version from 'EtherComm.dll'.";
        //        return false;
        //    }

        //    if (!etherVer.Equals(version))
        //    {
        //        log = $"EtherComm version check failed: required >=[{version}], found [{etherVer}].";
        //        return false;
        //    }

        //    log = $"EtherComm version check passed: required >=[{version}], found [{etherVer}].";
        //    return true;
        //}

        //static void copyExternalDll(string folderPath) {
        //    string exeDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        //    if (Directory.Exists(folderPath)) {
        //        string[] files = Directory.GetFiles(folderPath);
        //        foreach (string filePath in files) {
        //            string fileName = Path.GetFileName(filePath);
        //            File.Copy(filePath, exeDir + "/" + fileName, true);
        //        }
        //    }
        //}

        //static string OSV() {
        //    if (Environment.OSVersion.Platform == System.PlatformID.Win32NT) {
        //        Version ver = Environment.OSVersion.Version;
        //        if (ver.Major == 5)
        //            return "XP";
        //        else if (ver.Major == 6) {
        //            if (ver.Minor == 1)
        //                return "Win7";
        //            return "Vista";
        //        }
        //    }
        //    return "";
        //}

        static string appGuid = "s4nCh0p4nZ4";  //Assembly.GetExecutingAssembly().GetType().GUID.ToString();

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string className, string windowText);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        const UInt32 WM_CLOSE = 0x0010;
    }
}
