using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExactaEasyEng;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using SPAMI.Util.Logger;
using System.Reflection;
using DisplayManager;
using System.Globalization;
using ExactaEasyCore;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ExactaEasy
{

    public partial class frmMain : frmBase
    {

        CameraViewerController camViewCtrl;
        MachineConfiguration machineConfiguration;
        MachineModeEnum currMachineMode = MachineModeEnum.Unknown;
        UserLevelEnum currUserLevel = UserLevelEnum.None;

        ManualResetEvent systemInitialized = new ManualResetEvent(false);
        //CameraManager _cameraManager;
        VisionSystemManager visionSystemMgr;

        ToolsEnum currentTool = ToolsEnum.Unknown;
        DataTable logDataTable;
        DataTable logDataTableToShow;
        DataTable swVerDataTable;

        string lastButtonPressedName = "";
        object lockGrid = new object();
        DumpUI dump1;
        Trending trending1;
        LiveImageFilterPage liveFilter1;
        DebugPage debugPage1;

        //to historicies tools
        Dictionary<HistoricizedToolSetting, string> _dicHistoricizeToolsPath;
        Dictionary<HistoricizedToolSetting, int> _dicHistoricizeToolsCount;

        //REDIS
        Redis.RedisComm _redisComm;

        public frmMain()
        {

            InitializeComponent();

            this.FormClosed += new FormClosedEventHandler(frmMain_FormClosed);
            headerStrip.ErrorText = "---";

            this.pcbMain.SizeChanged += new EventHandler(this.pcbMain_SizeChanged);
            AppEngine.Current.ContextChanged += new EventHandler<ContextChangedEventArgs>(Current_ContextChanged);
            // TODO: not sure if this is really needed for GATEWAY2.
            //AppEngine.Current.HMIContextCommunicationError += new EventHandler<ContextChangedEventArgs>(Current_HMIContextCommunicationError);
            AppEngine.Current.PrintExactaEasyActiveRecipeRequested += new EventHandler<ExactaEasyOperationInvokedEventArgs>(Current_PrintActiveRecipe);
            AppEngine.Current.AppEngineStartStopBatch += Current_StartStopBatch;
            AppEngine.Current.AppEngineStartStopBatch2 += Current_StartStopBatch2;
            AppEngine.Current.AppEngineStartStopKnapp += Current_StartStopKnapp;


            statusStrip.MachineStatus = AppEngine.Current.CurrentContext.MachineMode.ToString();

            buildLogDataTable();
            buildSwVerDataTable();
            populateSwVerDataTable();

            try
            {
                machineConfiguration = AppEngine.Current.MachineConfiguration;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "frmMain.frmMain_Load", frmBase.UIStrings.GetString("UnableReadMacConf") + " Error: " + ex.Message);
                MessageBox.Show(frmBase.UIStrings.GetString("UnableReadMacConf"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            //creazione logger e imageviewer
            LogLevels logLevel;
            if (!DesignMode)
            {
                if (machineConfiguration.UseConsole)
                {
                    logger1.WriteToConsole = true;
                    if (Enum.TryParse(machineConfiguration.ConsoleLevel.ToString(), out logLevel))
                    {
                        logger1.WriteToConsoleLevel = logLevel;
                    }
                    else
                    {
                        logger1.WriteToConsoleLevel = LogLevels.Debug;
                    }
                }
                else
                    logger1.WriteToConsole = false;
                if (machineConfiguration.WriteLogToFile)
                {
                    logger1.WriteToFile = true;
                    if (Enum.TryParse(machineConfiguration.LogToFileLevel.ToString(), out logLevel))
                    {
                        logger1.WriteToFileLevel = logLevel;
                    }
                    else
                    {
                        logger1.WriteToFileLevel = LogLevels.Debug;
                    }
                }
                else
                    logger1.WriteToFile = false;
                logger1.OnNewMessage += new SPAMI.Util.Logger.Logger.NewMessageEventHandler(this.logger1_OnNewMessage);
                logger1.Init();
                createImageViewer();
            }
            reportViewer1.BeforePrintReport += reportViewer1_BeforePrintReport;

            // stringhe
            btnTabUtilities.Text = frmBase.UIStrings.GetString("Utilities");
            btnTabTrending.Text = frmBase.UIStrings.GetString("Trending");
            btnTabAssistants.Text = frmBase.UIStrings.GetString("Assistants");
            btnTabLogs.Text = frmBase.UIStrings.GetString("Logs");
            EntryInstant.HeaderText = frmBase.UIStrings.GetString("EventTime");
            EntryLevel.HeaderText = frmBase.UIStrings.GetString("Level");
            EntryMessage.HeaderText = frmBase.UIStrings.GetString("Message");
            btnResultsTable.Text = frmBase.UIStrings.GetString("Results");
            btnTabSwVersion.Text = frmBase.UIStrings.GetString("SwVersion");
            ModuleName.HeaderText = frmBase.UIStrings.GetString("ModuleName");
            Version.HeaderText = frmBase.UIStrings.GetString("Version");
            btnTabUtilities.Text = frmBase.UIStrings.GetString("Utilities");
            btnTabAssistants.Text = frmBase.UIStrings.GetString("Assistants");
            btnLoadRecipeAndApply.Text = frmBase.UIStrings.GetString("LoadRecipeAndApply");
            btnLoadRecipe.Text = frmBase.UIStrings.GetString("LoadRecipeWithoutApply");
            btnSaveCurrentRecipe.Text = frmBase.UIStrings.GetString("SaveCurrentRecipe");
            cbBypassHMIsendRecipe.Text = frmBase.UIStrings.GetString("BypassHMIsendRecipe");
            btnExit.Text = frmBase.UIStrings.GetString("Exit");
            button1.Text = frmBase.UIStrings.GetString("DevicesParameters");
            btnExportSwVer.Text = frmBase.UIStrings.GetString("Export");

            if (AppEngine.Current.GlobalConfig == null)
            {
                MessageBox.Show(frmBase.UIStrings.GetString("CantFindGlobalConfig"), "FATAL ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                RestartForm = false;
                this.Close();
            }
            else
            {
                if (!System.IO.Directory.Exists(AppEngine.Current.GlobalConfig.SettingsFolder))
                {
                    MessageBox.Show("Cannot find settings folder. Path: \"" + AppEngine.Current.GlobalConfig.SettingsFolder + "\"", "FATAL ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    RestartForm = false;
                    this.Close();
                }
            }

            currUserLevel = AppEngine.Current.CurrentContext.UserLevel;
            btnTabTrending.Visible = AppEngine.Current.TrendTool.IsReady;

            //redis instance
            _redisComm = new Redis.RedisComm();

            //btnDebugPage
            btnTabDebugPage.Visible = false;
        }

        public frmMain(VisionSystemManager _visionSystemMgr)
            : this()
        {

            visionSystemMgr = _visionSystemMgr;
            visionSystemMgr.RescanCompleted += new EventHandler(visionSystemMgr_RescanCompleted);
            visionSystemMgr.InitializationCompleted += new EventHandler(visionSystemMgr_InitializationCompleted);
            visionSystemMgr.RemoteDesktopDisconnection += visionSystemMgr_RemoteDesktopDisconnection;
            visionSystemMgr.ChangeRecipe += visionSystemMgr_ChangeRecipe;
            //visionSystemMgr.DeviceError += visionSystemMgr_DeviceError;
            visionSystemMgr.NewNodeBootDone += visionSystemMgr_NewNodeBootDone;
            visionSystemMgr.SavingBufferedImages += new EventHandler<DisplayManager.MessageEventArgs>(visionSystemMgr_SavingBufferedImages);
            visionSystemMgr.NotifyGUI += new EventHandler<DisplayManager.MessageEventArgs>(visionSystemMgr_NotifyGUI);
            visionSystemMgr.EnableNavigation += visionSystemMgr_EnableNavigation;
            createMachineConfigUI();
            //pier: commentato per medline
            //cbSaveBufferedImages.CheckedChanged += new System.EventHandler(this.cbSaveBufferedImages_CheckedChanged);
            //cbSaveBufferedImages.Checked = machineConfiguration.EnableImgToSave;
            cbBypassHMIsendRecipe.CheckedChanged += new System.EventHandler(this.cbBypassHMIsendRecipe_CheckedChanged);
            cbBypassHMIsendRecipe.Checked = machineConfiguration.BypassHMIsendRecipe;
            Log.Line(LogLevels.Pass, "frmMain.frmMain", "Change language: " + ChangeLanguage);
            //if (_visionSystemMgr.VisionInitialized.Value)
            //    systemInitialized.Set();

            _dicHistoricizeToolsPath = new Dictionary<HistoricizedToolSetting, string>();
            _dicHistoricizeToolsCount = new Dictionary<HistoricizedToolSetting, int>();

            //avaible measure stations from system manager (historicize tools)
            foreach (DisplayManager.Station st in visionSystemMgr.Stations)
                st.MeasuresAvailable += St_MeasuresAvailable;

            //batch id setted from hmi
            AppEngine.Current.SetBatchIdRequested += AppEngine_Current_SetBatchId;
        }




        private void AppEngine_Current_SetBatchId(object sender, SetBatchIdEventArgs e)
        {
            int countHistoricised = AppEngine.Current.MachineConfiguration.HistoricizedToolsSettings.Count;
            if (countHistoricised <= 0)
                return;

            _dicHistoricizeToolsPath.Clear();
            _dicHistoricizeToolsCount.Clear();
            if (e.Id >= 0)
            {
                foreach (HistoricizedToolSetting hts in AppEngine.Current.MachineConfiguration.HistoricizedToolsSettings)
                {
                    string fullPath = AppEngine.Current.MachineConfiguration.HistoricizedToolsFolder;
                    if (fullPath.EndsWith("\\") == false)
                        fullPath += "\\";

                    if (hts.Label == null || hts.Label == "")
                        fullPath = $"{fullPath}Batch_{e.Id}_Tool_{hts.NodeId}{hts.StationId}{hts.ToolIndex}{hts.ParameterIndex}.csv";
                    else
                        fullPath = $"{fullPath}Batch_{e.Id}_Tool_{hts.Label}_{hts.NodeId}{hts.StationId}{hts.ToolIndex}{hts.ParameterIndex}.csv";

                    try
                    {
                        if (File.Exists(fullPath))
                        {
                            //get count of rows
                            string[] lines = File.ReadAllLines(fullPath);
                            _dicHistoricizeToolsPath.Add(hts, fullPath);
                            if (lines.Length >= 2)
                                _dicHistoricizeToolsCount.Add(hts, lines.Length - 1);
                            else
                                _dicHistoricizeToolsCount.Add(hts, 0);
                        }
                        else
                        {
                            //new file
                            using (FileStream fs = File.Create(fullPath)) { }
                            using (StreamWriter sw = new StreamWriter(fullPath))
                            {
                                sw.WriteLine(string.Join(";", "index", "val"));
                                sw.Close();
                            }
                            _dicHistoricizeToolsPath.Add(hts, fullPath);
                            _dicHistoricizeToolsCount.Add(hts, 0);
                        }
                    }
                    catch(Exception ex)
                    {
                        _dicHistoricizeToolsPath.Clear();
                        _dicHistoricizeToolsCount.Clear();
                        throw;
                    }
                }
            }
        }

        private void St_MeasuresAvailable(object sender, MeasuresAvailableEventArgs e)
        {
            if (_dicHistoricizeToolsPath.Count > 0 && _dicHistoricizeToolsPath.Count == _dicHistoricizeToolsCount.Count)
            {
                DisplayManager.Station stSender = (DisplayManager.Station)sender;
                foreach(HistoricizedToolSetting hts in _dicHistoricizeToolsPath.Keys)
                {
                    if(stSender.NodeId == hts.NodeId && stSender.IdStation == hts.StationId)
                    {
                        InspectionResults inspectRes = e.InspectionResults;
                        if(hts.ToolIndex < inspectRes.ResToolCollection.Count)
                        {
                            if(hts.ParameterIndex < inspectRes.ResToolCollection[hts.ToolIndex].ResMeasureCollection.Count)
                            {
                                string value = inspectRes.ResToolCollection[hts.ToolIndex].ResMeasureCollection[hts.ParameterIndex].MeasureValue;
                                string path = _dicHistoricizeToolsPath[hts];
                                int index = _dicHistoricizeToolsCount[hts];
                                try
                                {
                                    //append
                                    using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
                                    {
                                        using (StreamWriter sw = new StreamWriter(fs))
                                        {
                                            sw.WriteLine(string.Join(";", index, value));
                                            //increment count
                                            _dicHistoricizeToolsCount[hts]++;
                                        }
                                        fs.Close();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw;
                                }
                            }
                        }
                    }
                }
            }
        }

        void visionSystemMgr_EnableNavigation(object sender, DisplayManager.MessageEventArgs e)
        {

            bool enable = Boolean.Parse(e.Message);
            vertMenuBar1.SetNavigationAvailable(enable);
        }

        void reportViewer1_BeforePrintReport(object sender, EventArgs e)
        {
            TopMost = false;
        }

        void Current_StartStopKnapp(object sender, StartKnappEventArgs e)
        {

            if (machineConfiguration.BypassHMIKnapp || e.TotalRoundsCount > machineConfiguration.HMIKnappTurnsLimit)
            {
                if (e.SaveType == SaveModeEnum.Setup)
                {
                    if (machineConfiguration.BypassHMIKnapp)
                        Log.Line(LogLevels.Warning, "frmMain.Current_StartStopKnapp", "Save knapp images to disk will be ignored: bypass activated");
                    if (e.TotalRoundsCount > machineConfiguration.HMIKnappTurnsLimit)
                        Log.Line(LogLevels.Warning, "frmMain.Current_StartStopKnapp", "Save knapp images to disk will be ignored: too many rounds");
                }
                Task taskStartStopKnappBypass = new Task(new Action(() => {
                    AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Busy);
                    Thread.Sleep(500);
                    AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.ReadyToRun);
                }));
                taskStartStopKnappBypass.Start();
                return;
            }
            Task taskStartStopKnapp = new Task(new Action(() => {
                if (e.SaveType == SaveModeEnum.Setup)
                {
                    AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Busy);
                    Thread.Sleep(500);
                    if (machineConfiguration.KnappSettings == null || machineConfiguration.KnappSettings.KnappStationsSettings == null)
                    {
                        Log.Line(LogLevels.Error, "frmMain.Current_StartStopKnapp", "Error: can't find Knapp settings!");
                        headerStrip.ErrorText = frmBase.UIStrings.GetString("KnappSettingsError");
                        return;
                    }
                    //DumpImagesUserSettings diusKnapp = AppEngine.Current.DumpImagesConfiguration.UserSettings.Find(us => us.Id == 0);   // 0 == SETUP / KNAPP
                    DumpImagesUserSettings diusKnapp = new DumpImagesUserSettings();
                    diusKnapp.StationsDumpSettings = new List<StationDumpSettings>();
                    foreach (KnappStationSettings kss in machineConfiguration.KnappSettings.KnappStationsSettings)
                    {
                        if (kss.EnableKnapp)
                        {
                            StationDumpSettings sds = new StationDumpSettings();
                            sds.Condition = SaveConditions.Any;
                            sds.Type = ImagesDumpTypes.Frames;
                            sds.VialsToSave = e.TotalRoundsCount * machineConfiguration.NumberOfSpindles / kss.Divisor;
                            //sds.MaxImages = ???
                            sds.Id = kss.IdStation;
                            sds.Node = kss.IdNode;
                            diusKnapp.StationsDumpSettings.Add(sds);
                        }
                    }
                    visionSystemMgr.StartStopBatch(diusKnapp, true);
                    Log.Line(LogLevels.Pass, "frmMain.Current_StartStopKnapp", "Save knapp images to disk: please wait ...");
                    bool signal = visionSystemMgr.SavingBatchAllNodesEvt.WaitOne(10 * 60 * 1000);  //aspetto 10 minuti al max
                    if (!signal)
                    {
                        Log.Line(LogLevels.Error, "frmMain.Current_StartStopKnapp", "Error while starting Knapp");
                        foreach (INode n in visionSystemMgr.Nodes)
                            if (n.DumpingEnable && !n.Dumping)
                                Log.Line(LogLevels.Error, "frmMain.Current_StartStopKnapp", n.Address + ": not ready to record knapp");
                        headerStrip.ErrorText = frmBase.UIStrings.GetString("ErrorStartingKnapp");
                        AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Error);
                        //disabilito anche quelli che sono partiti correttamente
                        visionSystemMgr.StartStopBatch(null, false);
                    }
                    else
                    {
                        Log.Line(LogLevels.Pass, "frmMain.Current_StartStopKnapp", "Knapp ready to go");
                        headerStrip.ErrorText = "";
                        AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.ReadyToRun);
                    }
                }
                if (e.SaveType == SaveModeEnum.DontSave)
                {

                    AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Busy);
                    Thread.Sleep(500);
                    visionSystemMgr.StartStopBatch(null, false);
                    bool signal = visionSystemMgr.NotSavingBatchAllNodesEvt.WaitOne(1 * 60 * 1000);  //aspetto 1 minuto al max
                    if (!signal)
                    {
                        Log.Line(LogLevels.Error, "frmMain.Current_StartStopKnapp", "Error while stopping Knapp");
                        foreach (INode n in visionSystemMgr.Nodes)
                            if (n.DumpingEnable && n.Dumping)
                                Log.Line(LogLevels.Error, "frmMain.Current_StartStopKnapp", n.Address + ": still dumping");
                        headerStrip.ErrorText = frmBase.UIStrings.GetString("ErrorStoppingKnapp");
                        AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Error);
                        //disabilito anche quelli che sono partiti correttamente
                        //visionSystemMgr.StartStopBatch(null, false);
                    }
                    else
                    {
                        Log.Line(LogLevels.Pass, "frmMain.Current_StartStopKnapp", "Knapp stopped successfully");
                        headerStrip.ErrorText = "";
                        AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.ReadyToRun);
                    }
                }
            }));
            taskStartStopKnapp.Start();
        }

        void Current_StartStopBatch(object sender, StartBatchEventArgs e)
        {

            //visionSystemMgr.StartStopBatch(e.SaveType);
            if (machineConfiguration.DumpImagesAvailable &&
                AppEngine.Current.DumpImagesConfiguration != null &&
                AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Busy))
            {
                try
                {
                    DumpImagesUserSettings dius;
                    switch (e.SaveType)
                    {
                        case SaveModeEnum.DontSave:
                            visionSystemMgr.StartStopBatch(null, false);

                            break;
                        case SaveModeEnum.Setup:
                            dius = AppEngine.Current.DumpImagesConfiguration.UserSettings.Find(us => us.Id == 0);   // 0 == SETUP / KNAPP
                            if (dius != null)
                                visionSystemMgr.StartStopBatch(dius, true);
                            break;
                        case SaveModeEnum.Production:
                            dius = AppEngine.Current.DumpImagesConfiguration.UserSettings.Find(us => us.Id == 1);   // 1 == PRODUCTION
                            if (dius != null)
                                visionSystemMgr.StartStopBatch(dius, true);
                            break;
                    }
                    AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.ReadyToRun);
                }
                catch
                {
                    AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Error);
                }
            }
        }

        private void Current_StartStopBatch2(object sender, StartBatchEventArgs e)
        {
            if (machineConfiguration.DumpImagesAvailable && AppEngine.Current.DumpImagesConfiguration2 != null && AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Busy))
            {
                try
                {
                    switch (e.SaveType)
                    {
                        case SaveModeEnum.DontSave:
                            visionSystemMgr.StartStopBatch(null, false);
                            break;
                        default:
                            visionSystemMgr.StartStopBatch2(AppEngine.Current.DumpImagesConfiguration2.BatchSettings, true);
                            break;
                    }
                    AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.ReadyToRun);
                }
                catch
                {
                    AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Error);
                }
            }
        }


        void Current_PrintActiveRecipe(object sender, EventArgs e)
        {

            showReportPanel();
            printActiveRecipe();
        }

        void Current_HMIContextCommunicationError(object sender, ContextChangedEventArgs e)
        {

            if (e.ContextChanges == ContextChangesEnum.ActiveRecipe)
                headerStrip.ErrorText = frmBase.UIStrings.GetString("RecivedRecipeWithErrors");
        }

        void visionSystemMgr_NotifyGUI(object sender, DisplayManager.MessageEventArgs e)
        {

            headerStrip.ErrorText = e.Message;
        }

        void visionSystemMgr_NewNodeBootDone(object sender, EventArgs e)
        {
            //send user level
            visionSystemMgr.Nodes[(sender as INode).IdNode].SendUserLevelClass(AppEngine.Current.CurrentContext.UserLevel);

            if (machineConfiguration.BypassHMIsendRecipe)
            {
                Log.Line(LogLevels.Warning, "frmMain.visionSystemMgr_NewNodeBootDone", "Bypass apply parameters from HMI is ENABLED. Recipe will not be be applied to " + (sender as INode).Address);
                return;
            }

            Log.Line(LogLevels.Pass, "frmMain.visionSystemMgr_NewNodeBootDone", frmBase.UIStrings.GetString("SendingRecipeTo") + " " +
                (sender as INode).Address + " " + frmBase.UIStrings.GetString("PleaseWait") + " ...");
            headerStrip.ErrorText = frmBase.UIStrings.GetString("SendingRecipeTo") + " " +
                (sender as INode).Address + " " + frmBase.UIStrings.GetString("PleaseWait") + " ...";

            Task nodeRecipeUpdate = new Task(new Action(() => 
            {
                Exception cachedEx = null;
                Recipe recipe = AppEngine.Current.CurrentContext.ActiveRecipe;
                int idNode = (sender as INode).IdNode;
                if (recipe != null && recipe.Nodes != null && recipe.Nodes.Count > 0)
                {
                    NodeRecipe nodeRecipe = recipe.Nodes.Find(recNode => recNode.Id == idNode);
                    INode nn = visionSystemMgr.Nodes.Find(n => n.IdNode == nodeRecipe.Id);
                    if (nn != null && nodeRecipe != null)
                    {
                        // Check if we are already in uploading status.
                        if (nn.SetParametersStatus == SetParametersStatusEnum.Uploading)
                        {
                            Log.Line(LogLevels.Warning, "frmMain.newNode_BootDone", $"{nn.Address} is already in UPLOADING state. No recipe will be sent.");
                            return;
                        }

                        try
                        {
                            nn.SetParametersDone.Reset();
                            nn.SetParameters(recipe.RecipeName, nodeRecipe, AppEngine.Current.ParametersInfo, AppEngine.Current.CurrentContext.CultureCode);

                            if (!nn.SetParametersDone.WaitOne(90 * 1000, false))
                            {
                                // Mark this node as timeouted.
                                nn.SetParametersStatus = SetParametersStatusEnum.TimeoutError;
                                Log.Line(LogLevels.Warning, "frmMain.newNode_BootDone", nn.Address + ": Parameters set failed: timeout!");
                                throw new Exception("Parameters set failed: timeout!");
                            }

                            switch (nn.SetParametersStatus)
                            {
                                case SetParametersStatusEnum.Uploading:
                                case SetParametersStatusEnum.Undefined:
                                case SetParametersStatusEnum.TimeoutError:
                                case SetParametersStatusEnum.UploadError:
                                    // Matteo - 27/06/2024.
                                    Log.Line(LogLevels.Error, "frmMain.newNode_BootDone", nn.Address + ": Parameters set failed");
                                    throw new Exception("Parameters set to node " + nn.IdNode + " failed");

                                case SetParametersStatusEnum.UploadedOK:
                                    Log.Line(LogLevels.Pass, "frmMain.newNode_BootDone", nn.Address + ": Parameters set completed successfully");
                                    break;
                                   
                            }
                        }
                        catch (Exception ex)
                        {
                            if (visionSystemMgr.IsNodeEnabled(nn.IdNode))
                                cachedEx = ex;
                            else
                                Log.Line(LogLevels.Warning, "frmMain.newNode_BootDone", nn.Address + ": Error while sending parameters to a disabled node: " + ex.Message);
                        }
                    }
                    else
                    {
                        Log.Line(LogLevels.Warning, "frmMain.newNode_BootDone", nn.Address + ": No recipe found for this client.");
                    }
                }
                if (cachedEx != null)
                {
                    NotifyDeviceError(cachedEx.Message);
                }
                else
                {
                    headerStrip.ErrorText = "";
                    visionSystemMgr.TryChangeSupervisorStatus(machineConfiguration.BypassHMIsendRecipe);
                }
            }));
            nodeRecipeUpdate.Start();
        }

        void visionSystemMgr_SavingBufferedImages(object sender, DisplayManager.MessageEventArgs e)
        {

            if (e.Message == "1" && sender is INode)
            {
                headerStrip.ErrorText = (sender as INode).Address + ": " + frmBase.UIStrings.GetString("SavingBufferedImages");
            }
            else
            {
                Thread.Sleep(1000);
                headerStrip.ErrorText = "";
            }
            headerStrip.Refresh();
        }

        //void visionSystemMgr_DeviceError(object sender, EventArgs e) {
        void NotifyDeviceError(string errorMessage)
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (InvokeRequired && IsHandleCreated)
                Invoke(new MethodInvoker(() => { NotifyDeviceError(errorMessage); }));
            else
            {
                Log.Line(LogLevels.Error, "frmMain.NotifyDeviceError", "Error: " + errorMessage);
                headerStrip.ErrorText = UIStrings.GetString("ErrorSendingParameters");
                if (AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Error))
                {
                    statusStrip.StatusMessage = frmBase.UIStrings.GetString("ErrorSendingParameters");
                }
                else
                {
                    statusStrip.StatusMessage = frmBase.UIStrings.GetString("SpvErrorError");
                }
            }
        }

        void visionSystemMgr_ChangeRecipe(object sender, NodeRecipeEventArgs e)
        {

            changeRecipe2HMI(e.ToSave, e.NodeRecipe.Id);
            changeActiveRecipe(AppEngine.Current.CurrentContext.ActiveRecipe);
            //if (lastButtonPressedName == "SETTINGS")
            //    StartSettings(vertMenuBar1.Index, true);
        }

        void visionSystemMgr_RemoteDesktopDisconnection(object sender, EventArgs e)
        {

            setSupervisorOnTop();
        }

        void visionSystemMgr_InitializationCompleted(object sender, EventArgs e)
        {

            //systemInitialized.Set();
        }

        void visionSystemMgr_RescanCompleted(object sender, EventArgs e)
        {

            statusStrip.SetStatusMessage("");
            // ricreo i display
            //createDisplays();
            showMachineConfigPanel();
            //currUserLevel = UserLevelEnum.None;
            //refreshForm();
            visionSystemMgr.TryChangeSupervisorStatus(machineConfiguration.BypassHMIsendRecipe);

            //set redis viewcam
            if(AppEngine.Current.MachineConfiguration.IsRedisVisibleStationFilter)
                setRedisViewCam(true); //reset all values
        }

        public void ShowTransparentHeader()
        {

            pnlTransparentHeader.Visible = true;
        }

        public void ShowTransparentFooter()
        {

            pnlTransparentFooter.Visible = true;
        }

        void TheObserver_DispatchObservation(object sender, TaskStatusEventArgs e)
        {
            refreshForm();
        }

        void Current_SetSupervisorOnTopRequested(object sender, EventArgs e)
        {

            setSupervisorOnTop();
        }

        void Current_SetSupervisorHideRequested(object sender, EventArgs e)
        {

            setSupervisorHide();
        }

        internal void setSupervisorOnTop()
        {
            if (ForceWindowHidden)
                ForceWindowHidden = false;

            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (InvokeRequired && IsHandleCreated)
                this.Invoke(new Action(() => { setSupervisorOnTop(); }));
            else
            {
                this.WindowState = FormWindowState.Normal;
                TopMost = true;
                //if (!this.Visible)
                Show();

                //this.TopMost = true;
                if (pnlTransparentHeader.Visible)
                    this.TopMost = false;
                refreshForm();	//pier: in certi casi è indispensabile NON RIMUOVERE
                visionSystemMgr.RedrawDisplay("Main");
                //Log.Line(LogLevels.Pass, "frmMain.setSupervisorOnTop", "SUPERVISOR ON TOP");
                changeUserLevel(currUserLevel);
            }
        }

        internal void setSupervisorHide()
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (InvokeRequired && IsHandleCreated)
                this.Invoke(new Action(() => { setSupervisorHide(); }));
            else
            {
                this.TopMost = false;
                //this.SendToBack();
                //Hide();
                WindowState = FormWindowState.Minimized;
                //Log.Line(LogLevels.Pass, "frmMain.setSupervisorHide", "SUPERVISOR HIDE");
            }
        }

        void Current_ContextChanged(object sender, ContextChangedEventArgs e)
        {

            if ((e.ContextChanges & ContextChangesEnum.ActiveRecipe) != 0)
            {
                if (AppEngine.Current.CurrentContext.ActiveRecipe != null)
                {
                    AppEngine.Current.CurrentContext.ActiveRecipe.SaveXml(@"SCADARecipe.xml");
                    if (e.ApplyParameters && ChangeLanguage == false)
                    {
                        sendCameraParams();
                    }
                    else
                    {
                        visionSystemMgr.TryChangeSupervisorStatus(machineConfiguration.BypassHMIsendRecipe | ChangeLanguage);
                    }
                    vertMenuBar1.SetNavigationAvailable(true);
                    vertMenuBar1.SetSettingsEnabled(true);
                    foreach (MeasuresContainer mc in visionSystemMgr.MeasuresContainer)
                    {
                        mc.CurrRecipe = AppEngine.Current.CurrentContext.ActiveRecipe;
                    }
                    foreach (MeasuresContainer mc in visionSystemMgr.DBMeasures)
                    {
                        mc.CurrRecipe = AppEngine.Current.CurrentContext.ActiveRecipe;
                    }
                }
            }
            if((e.ContextChanges & ContextChangesEnum.ActiveRecipeVersion) != 0)
            {
                //footer (dispatcher)
                this.Invoke((MethodInvoker)delegate
                {
                    statusStrip.SetRecipe(AppEngine.Current.CurrentContext.ActiveRecipe, AppEngine.Current.CurrentContext.ActiveRecipeVersion);
                });
            }
            if((e.ContextChanges & ContextChangesEnum.ActiveRecipeStatus) != 0)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    dump1.RecalcButtons();
                });
            }
            if ((e.ContextChanges & ContextChangesEnum.UserLevel) != 0)
            {
                Log.Line(LogLevels.Pass, "frmMain.Current_ContextChanged", "User level changed to " + AppEngine.Current.CurrentContext.UserLevel.ToString());
                vertMenuBar1.SetAdminPanelVisible((AppEngine.Current.CurrentContext.UserLevel >= UserLevelEnum.Administrator));
                vertMenuBar1.SetDumpVisible((AppEngine.Current.CurrentContext.UserLevel >= UserLevelEnum.AssistantSupervisor));

                if (AppEngine.Current.CurrentContext.UserLevel < UserLevelEnum.Engineer)
                {
                    cameraConfigUI.Enabled = false;
                }
                else
                {
                    cameraConfigUI.Enabled = true;
                }

                foreach (INode n in visionSystemMgr.Nodes)
                {
                    visionSystemMgr.Nodes[n.IdNode].SendUserLevelClass(AppEngine.Current.CurrentContext.UserLevel);
                }



                // changeUserLevel(AppEngine.Current.CurrentContext.UserLevel);
            }
            if ((e.ContextChanges & ContextChangesEnum.MachineInfo) != 0)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < AppEngine.Current.CurrentContext.EnabledStation.Length; i++)
                {
                    sb.Append(Convert.ToInt32(AppEngine.Current.CurrentContext.EnabledStation[i]));
                }
                string enabledStationString = sb.ToString();
                Log.Line(LogLevels.Pass, "frmMain.Current_ContextChanged", "Machine Info changed: " + enabledStationString);
                bool error = false;
                try
                {
                    if (AppEngine.Current.CurrentContext.ActiveRecipe != null && AppEngine.Current.CurrentContext.ActiveRecipe.Cams != null)
                    {
                        foreach (Cam cam in AppEngine.Current.CurrentContext.ActiveRecipe.Cams)
                        {
                            if (cam.Id >= visionSystemMgr.Cameras.Count) continue;
                            int nodeId = visionSystemMgr.Cameras[cam.Id].NodeId;
                            int stationId = visionSystemMgr.Cameras[cam.Id].StationId;
                            if (visionSystemMgr.Nodes[stationId].Stations[stationId].Enabled &&
                                cam.Enabled && visionSystemMgr.CameraReady(cam.Id) != CameraNewStatus.Ready)
                            {
                                Log.Line(LogLevels.Error, "frmMain.Current_ContextChanged", "Camera ID {0} enabled by HMI or Recipe but out of work.", cam.Id);
                                error = true;
                            }
                        }
                    }
                }
                catch
                {
                    Log.Line(LogLevels.Error, "frmMain.Current_ContextChanged", "At least one camera enabled by HMI or Recipe but out of work.");
                    error = true;
                }
                if (error)
                {
                    //if (!AppEngine.Current.TrySetSupervisorInError()) {
                    //    Log.Line(LogLevels.Error, "frmMain.Current_ContextChanged", "Error setting spv in Error");
                    //}
                    Task sendToSpv = new Task(new Action(setSpvInError));
                    sendToSpv.Start();
                }
                camViewCtrl.RefreshStationStatus();
                visionSystemMgr.RedrawDisplay("Main");
                visionSystemMgr.RedrawHeader("Main");
                if (ChangeLanguage == true)
                {
                    foreach (INode n in visionSystemMgr.Nodes)
                    {
                        n.RefreshExportedParams();
                    }
                    ChangeLanguage = false;
                    Log.Line(LogLevels.Pass, "frmMain.Current_ContextChanged", "ChangeLanguage set to false.");
                }
                // Added to rescan the connection when a station is disabled or enabled 
                visionSystemMgr.StartRescan();
            }
            refreshForm();
        }

        void setSpvInError()
        {
            if (!AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Error))
            {
                Log.Line(LogLevels.Error, "frmMain.setSpvInError", "Error setting spv in Error");
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

            TaskObserver.TheObserver.DispatchObservation += TheObserver_DispatchObservation;

            if (machineConfiguration.DisplayResultsPage == true)
            {
                btnResultsTable.Visible = true;
            }
            else
            {
                btnResultsTable.Visible = false;
            }
            bool dumpOK = false;
            if (AppEngine.Current.MachineConfiguration.TypeDumpImages == 0)
                if (AppEngine.Current.DumpImagesConfiguration != null)
                    dumpOK = true;
            if (AppEngine.Current.MachineConfiguration.TypeDumpImages == 1)
                if (AppEngine.Current.DumpImagesConfiguration2 != null)
                    dumpOK = true;
            if (machineConfiguration.DumpImagesAvailable && dumpOK)
            {
                dump1 = new DumpUI(visionSystemMgr, AppEngine.Current.DumpImagesConfiguration/*machineConfiguration.CameraSettings*/);
                dump1.Dock = DockStyle.Fill;
                dump1.Name = "dump1";
                pnlDump.SuspendLayout();
                pnlDump.Controls.Add(dump1);
                pnlDump.ResumeLayout();


                trending1 = new Trending();
                trending1.Dock = DockStyle.Fill;
                trending1.Name = "trending1";
                pnlTabTrending.SuspendLayout();
                pnlTabTrending.Controls.Add(trending1);
                pnlTabTrending.ResumeLayout();

                liveFilter1 = new LiveImageFilterPage(visionSystemMgr);
                liveFilter1.Dock = DockStyle.Fill;
                liveFilter1.Name = "liveFilter1";
                pnlLiveFilter.SuspendLayout();
                pnlLiveFilter.Controls.Add(liveFilter1);
                pnlLiveFilter.ResumeLayout();

                debugPage1 = new DebugPage();
                debugPage1.Dock = DockStyle.Fill;
                debugPage1.Name = "debugPage1";
                pnlTabDebugPage.SuspendLayout();
                pnlTabDebugPage.Controls.Add(debugPage1);
                pnlTabDebugPage.ResumeLayout();
            }
            /*if (AppEngine.Current.CurrentContext == AppContext.EmptyContext || AppEngine.Current.CurrentContext.AppClientRect == Rectangle.Empty) {
                AppEngine.Current.SetSupervisorPos(new Rectangle(new Point(0, 0), new Size(1245, 1024)));
                //this.Location = new Point(0, 0);
                //this.Size = new Size(1245, 768);
            }
            else {
                AppContext context = AppEngine.Current.CurrentContext;
                this.Location = context.AppClientRect.Location;
                this.Size = context.AppClientRect.Size;
            }*/

            camViewCtrl = new CameraViewerController(this, visionSystemMgr, machineConfiguration);
            //camViewCtrl.ChangedDownloadedImages += new EventHandler<NewFolderEventArgs>(camViewCtrl_ChangedDownloadedImages);

            if (AppEngine.Current.GatewayCommunicationStatus != GatewayCommunicationStatusEnum.Connected &&
                 AppEngine.Current.CurrentContext.ActiveRecipe == null)
            {
                vertMenuBar1.SetNavigationAvailable(false);
                vertMenuBar1.SetSettingsEnabled(false);
                //Recipe ricetta = Recipe.LoadFromFile("ricettaXML.xml");
                //AppEngine.Current.SetActiveRecipe(ricetta);
                //changeActiveRecipe(ricetta);
            }
            else
                changeActiveRecipe(AppEngine.Current.CurrentContext.ActiveRecipe);

            //cameraManager.Displays.Add(new ScreenGridDisplay("Main", pcbMain.Handle, cameraManager.Cameras.Sort(machineConfiguration.CameraSettings), 1, 3));
            Task initSystemTask = new Task(new Action(initSystem));
            initSystemTask.Start();

            createDisplays();
            if (!machineConfiguration.BypassHMIsendResults && visionSystemMgr.MeasuresContainer["Results2Scada"] == null)
            {
                visionSystemMgr.MeasuresContainer.Add(new Results2Scada("Results2Scada", visionSystemMgr.Stations, machineConfiguration.StationSettings, 100));
                visionSystemMgr.MeasuresContainer["Results2Scada"].CurrRecipe = AppEngine.Current.CurrentContext.ActiveRecipe;
            }
            showHomePagePanel();
            lastButtonPressedName = "HOME";
            if (ForceWindowHidden)
            {
                WindowState = FormWindowState.Minimized;
                //Hide();
            }
            timer1.Start();

            //ALLA FINE DEL CARICAMENTO DELLA FORM SONO PRONTO A RECEPIRE I MOSTRA/NASCONDI DELLO SCADA....TEST!!!!
            AppEngine.Current.SetExactaEasyOnTopRequested += new EventHandler<ExactaEasyOperationInvokedEventArgs>(Current_SetSupervisorOnTopRequested);
            AppEngine.Current.HideExactaEasyRequested += new EventHandler<ExactaEasyOperationInvokedEventArgs>(Current_SetSupervisorHideRequested);

            // OnTop is obsolete.
            //if (machineConfiguration.EnableOnTopCommunication /*&& firstTime == true*/)
            //{
            //    //COMUNICO A ON TOP CHE SONO INIZIALIZZATO
            //    try
            //    {
            //        Gatew onTopGw = new Gatew();
            //        onTopGw.ExternalProcessSUP = true;
            //        if (onTopGw.StartSUPComm())
            //            onTopGw.SetSUPloaded(1);
            //        onTopGw.StopSUPComm();
            //    }
            //    catch (Exception ex)
            //    {
            //        Log.Line(LogLevels.Warning, "frmMain.frmMain_Load", "Error while trying to communicate with OnTop: " + ex.Message);
            //    }
            //}


            Current_ContextChanged(this, new ContextChangedEventArgs(ContextChangesEnum.All, false));

            //connect redis
            if (AppEngine.Current.MachineConfiguration.RedisEnable)
            {
                string hostname = AppEngine.Current.MachineConfiguration.RedisHostname;
                string port = AppEngine.Current.MachineConfiguration.RedisPort.ToString();
                string pass = AppEngine.Current.MachineConfiguration.RedisPassword;
                Log.Line(LogLevels.Warning, "frmMain.frmMain_Load", "connection to redis...");
                _redisComm.CreateConnection(hostname, port, pass);
            }

            //set redis view cam
            if (AppEngine.Current.MachineConfiguration.IsRedisVisibleStationFilter)
                setRedisViewCam(true); //reset all values

            refreshForm();
        }

        //static bool firstTime = true;

        void camViewCtrl_ChangedDownloadedImages(object sender, NewFolderEventArgs e)
        {

            changeImageViewerPath(e.Path);
        }

        void changeImageViewerPath(string path)
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(imageViewer1))
                return;

            if (imageViewer1.InvokeRequired && imageViewer1.IsHandleCreated)
            {
                imageViewer1.Invoke(new MethodInvoker(() => changeImageViewerPath(path)));
            }
            else
            {
                if (imageViewer1 != null)
                {
                    imageViewer1.ImgViewerMode = SPAMI.Util.EmguImageViewer.ImageViewerMode.FileRecycle;
                    imageViewer1.OfflineRecycleFolderPath = path;
                }
            }
        }

        SPAMI.Util.EmguImageViewer.ImageViewer imageViewer1;
        void createImageViewer()
        {

            imageViewer1 = new SPAMI.Util.EmguImageViewer.ImageViewer();
            pnlImageViewer.SuspendLayout();
            imageViewer1.SuspendLayout();
            pnlImageViewer.Controls.Add(this.imageViewer1);
            imageViewer1.AllowDrop = true;
            imageViewer1.BackColor = System.Drawing.Color.Transparent;
            imageViewer1.ClassName = "ImageViewer";
            imageViewer1.ContinuousRecycle = true;
            imageViewer1.Dock = DockStyle.Fill;
            imageViewer1.Fps = 5D;
            imageViewer1.FullframeZoomMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            imageViewer1.Name = "imageViewer1";
            imageViewer1.OfflineRecycleFolderPath = "";
            imageViewer1.Online = false;
            imageViewer1.PixInfoHeight = 5;
            imageViewer1.PixInfoWidth = 5;
            imageViewer1.QueueCapacity = 10;
            imageViewer1.ShowAdjacentPixel = true;
            imageViewer1.ShowBigHistogram = false;
            imageViewer1.ShowHistogram = false;
            imageViewer1.ShowHorizontalPixel = false;
            imageViewer1.ShowVerticalPixel = false;
            imageViewer1.ZoomActive = false;
            imageViewer1.ZoomStretch = false;
            pnlImageViewer.ResumeLayout();
            imageViewer1.ResumeLayout();
        }

        CameraConfigUI cameraConfigUI;
        void createMachineConfigUI()
        {

            cameraConfigUI = new CameraConfigUI(machineConfiguration, visionSystemMgr, this);
            pnlMachineConfig.SuspendLayout();
            cameraConfigUI.SuspendLayout();
            pnlMachineConfig.Controls.Add(this.cameraConfigUI);
            cameraConfigUI.AllowDrop = true;
            cameraConfigUI.BackColor = System.Drawing.Color.Transparent;
            cameraConfigUI.Dock = DockStyle.Fill;
            cameraConfigUI.Name = "cameraConfigUI";
            cameraConfigUI.Padding = new System.Windows.Forms.Padding(10);
            cameraConfigUI.StartRescan += new EventHandler(cameraConfigUI_StartRescan);
            cameraConfigUI.StopRescan += new EventHandler(cameraConfigUI_StopRescan);
            pnlMachineConfig.ResumeLayout();
            cameraConfigUI.ResumeLayout();
            if (machineConfiguration.NodeSettings == null)
            {
                foreach (CameraSetting cam in machineConfiguration.CameraSettings)
                {
                    cameraConfigUI.AddCamera(cam.Id, cam.Station, "VisionSysManagerCameraCreation" + cam.Id.ToString());
                }
            }
            else
            {
                foreach (NodeSetting node in machineConfiguration.NodeSettings)
                {
                    cameraConfigUI.AddNode(node.Id, "VisionSysManagerNodeConnection" + node.Id.ToString());
                }
            }
        }

        void cameraConfigUI_StartRescan(object sender, EventArgs e)
        {
            //AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Busy);
            statusStrip.SetStatusMessage("Scanning system ...");
        }

        void cameraConfigUI_StopRescan(object sender, EventArgs e)
        {

        }

        //void createResultsTable() {
        //    dgvResults.SuspendLayout();
        //    dgvResults.ColumnHeadersBorderStyle = properColumnHeadersBorderStyle;
        //    dgvResults.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //    dgvResults.BackgroundColor = System.Drawing.SystemColors.ControlDarkDark;
        //    dgvResults.DefaultCellStyle.BackColor = System.Drawing.SystemColors.ControlDarkDark;
        //    dgvResults.DefaultCellStyle.ForeColor = Color.White;
        //    //dgvResults.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(3);
        //    dgvResults.RowHeadersVisible = false;
        //    dgvResults.CellBorderStyle = DataGridViewCellBorderStyle.None;
        //    visionSystemMgr.MeasuresContainer.Add(new GridViewMeasuresDisplay("Results", dgvResults, visionSystemMgr.Stations));
        //    dgvResults.ResumeLayout();
        //}

        static DataGridViewHeaderBorderStyle properColumnHeadersBorderStyle
        {
            get
            {
                return (SystemFonts.MessageBoxFont.Name == "Segoe UI") ?
                DataGridViewHeaderBorderStyle.None :
                DataGridViewHeaderBorderStyle.Raised;
            }
        }

        void fatalError(Exception ex)
        {
            RestartForm = false;
            Log.Line(LogLevels.Error, "frmMain.frmMain_Load", ex.Message);
            headerStrip.ErrorText = frmBase.UIStrings.GetString("FatalError");
            MessageBox.Show(ex.ToString());
            MessageBox.Show(ex.StackTrace.ToString());
            MessageBox.Show(frmBase.UIStrings.GetString("FatalError"), "FATAL ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.Close();
        }


        void initSystem()
        {

            CultureInfo ci = new CultureInfo(AppEngine.Current.CurrentContext.CultureCode);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            statusStrip.StatusMessage = frmBase.UIStrings.GetString("ReadingDeviceInfo") + " ...";
            //16.07 lightcontroller vecchio proviamo a toglierlo
            //initLightControllers();
            //camera
            try
            {
                //TattileSvc.Connect(machineConfiguration.NumberOfCamera);
                if (visionSystemMgr.Cameras.Count == 0)
                {
                    statusStrip.StatusMessage = frmBase.UIStrings.GetString("NoCameraAvailable");
                }
                else
                {
                    statusStrip.UserLevel = string.Format(frmBase.UIStrings.GetString("UserLevelMsg"), currUserLevel);
                    //TattileSvc.numberOfDeviceDeclared = machineConfiguration.NumberOfCamera;
                    //pier: col nuovo display non serve mappare perchè viene fatto dentro
                    /*for (int i = 0; i < cameraManager.Cameras.Count; i++)
                    {
                        try {
                            TattileSvc.MapCamera(i, machineConfiguration.CameraSettings);
                        }
                        catch (CameraNotFoundException ex) {
                            statusStrip.setCameraStatus(ExStatuStripIconEnum.Warning);
                            statusStrip.StatusMessage = string.Format(frmBase.UIStrings.GetString("CameraNotFound"), i.ToString(), ex.Ip4Address);
                        }
                    }*/
                    statusStrip.StatusMessage = frmBase.UIStrings.GetString("ShowingLiveCam");
                    //connectToRunningDisplay();
                }
            }
            //catch (TattileNotConnectedException ex) {
            //    statusStrip.setCameraStatus(ExStatuStripIconEnum.Error);
            //    statusStrip.CameraInfo = string.Format(frmBase.UIStrings.GetString("UserLevelMsg"), cameraManager.Cameras.Count, machineConfiguration.NumberOfCamera);
            //    Log.Line(LogLevels.Error, "frmMain.initSystem", "Camera detected: " + cameraManager.Cameras.Count.ToString() + ". Camera expected: " + machineConfiguration.NumberOfCamera.ToString() + ". Error: " + ex.Message);
            //}
            catch (CameraException ex)
            {
                Log.Line(LogLevels.Error, "frmMain.initSystem", "Error: " + ex.Message);
            }
            finally
            {
                systemInitialized.Set();
                if ((SendRecipeReq == true) && !machineConfiguration.BypassHMIsendRecipe)
                {
                    sendCameraParams();
                }
                SendRecipeReq = false;
                //firstTime = false;
                //else
                //    statusStrip.setCameraStatus(ExtStatusStrip.CameraLastStatus);
            }

            if (machineConfiguration.DisplayResultsPage == true)
            {
                //if (dgvResults.InvokeRequired) {
                //    dgvResults.Invoke(new MethodInvoker(createResultsTable));
                //}
                //else {
                //    createResultsTable();
                //}
            }
            foreach (INode n in visionSystemMgr.Nodes)
            {
                foreach (IStation s in n.Stations)
                {
                    // SCOMMENTARE PER SALVARE RISULTATI SU FILE
                    //if (s.HasMeasures) {
                    //    s.DumpResultsEnabled = machineConfiguration.DumpResults;
                    //    string baseFilepath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    //    visionSystemMgr.MeasuresContainer.Add(new MeasuresFile("Node_" + n.IdNode + "_Station_" + s.IdStation, baseFilepath, ";", s));
                    //}
                    try
                    {
                        if (n.Connected)
                            s.SetStreamMode(-1, CameraWorkingMode.ExternalSource);
                    }
                    catch (Exception ex)
                    {
                        //se stazione non è abilitata da HMI => non dare errore
                        if (s.Enabled)
                        {
                            headerStrip.ErrorText = ex.Message;
                            Log.Line(LogLevels.Warning, "frmMain.initSystem", "Stazione ID {0} Error: " + ex.Message, s.IdStation);
                        }
                        else
                            Log.Line(LogLevels.Debug, "frmMain.initSystem", "Stazione ID {0} not enabled by HMI or Recipe.", s.IdStation);
                    }
                }

                if (AppEngine.Current.CurrentContext.SupervisorMode != SupervisorModeEnum.Busy)
                {
                    vertMenuBar1.Enabled = true;
                    statusStrip.StatusMessage = string.Empty;
                }
            }

            //statusStrip.StatusMessage = "Starting supervisor web service ...";
            //startSupervisorService();

        }

        //void connectToRunningDisplay() {

        //    if (InvokeRequired)
        //        this.Invoke(new MethodInvoker(connectToRunningDisplay));
        //    else
        //        pcbMain.ConnectToRunningDisplay();
        //}

        //        void startSupervisorService() {

        //            Supervisor supervisor = new Supervisor();
        //            Uri httpUrl = new Uri("http://localhost:8090/ExactaEasyUI/Supervisor");
        //            sHost = new WebServiceHost(sv, httpUrl);
        //            sHost.AddServiceEndpoint(typeof(ExactaEasyEng.ISupervisor), new WebHttpBinding(), "");
        //            sHost.Description.Endpoints[0].Behaviors.Add(new WebHttpBehavior { HelpEnabled = true });
        //#if DEBUG
        //            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
        //            smb.HttpGetEnabled = true;
        //            sHost.Description.Behaviors.Add(smb);
        //#endif
        //            sHost.Open();
        //        }

        private void vertMenuBar1_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {

            //if (!camViewCtrl.CameraUsed(vertMenuBar1.Index))
            //    return;

            switch (e.ButtonPressedName)
            {
                case "HOME":
                    showHomePagePanel();
                    break;
                //case "NEXT": 08/11/2016 invertiti PREV e NEXT per essere come Gretel
                case "PREV":
                    pnlUtilities.Visible = false;
                    pnlReports.Visible = false;
                    pnlImageViewer.Visible = false;
                    pnlMachineConfig.Visible = false;
                    pnlResults.Visible = false;
                    pnlKnapp.Visible = false;
                    pnlDump.Visible = false;
                    pnlLiveFilter.Visible = false;
                    if (vertMenuBar1.Index + 1 < machineConfiguration.NumberOfCamera)
                    {
                        visionSystemMgr.Displays["Main"].Suspend();
                        vertMenuBar1.Index++;
                        pnlCriticalError.Visible = false;
                        camViewCtrl.UnbindCurrentCamera();
                        StartSettings(vertMenuBar1.Index, true);
                    }
                    break;
                //case "PREV": 08/11/2016 invertiti PREV e NEXT per essere come Gretel
                case "NEXT":
                    pnlUtilities.Visible = false;
                    pnlReports.Visible = false;
                    pnlImageViewer.Visible = false;
                    pnlResults.Visible = false;
                    pnlKnapp.Visible = false;
                    pnlDump.Visible = false;
                    pnlMachineConfig.Visible = false;
                    pnlLiveFilter.Visible = false;
                    if (vertMenuBar1.index - 1 >= 0)
                    {
                        visionSystemMgr.Displays["Main"].Suspend();
                        vertMenuBar1.Index--;
                        pnlCriticalError.Visible = false;
                        camViewCtrl.UnbindCurrentCamera();
                        StartSettings(vertMenuBar1.Index, true);
                    }
                    break;
                case "SETTINGS":
                    //if (assistantUI != null)
                    //    assistantUI.UnbindCurrentCamera();
                    visionSystemMgr.Displays["Main"].Suspend();
                    pnlUtilities.Visible = false;
                    pnlReports.Visible = false;
                    pnlImageViewer.Visible = false;
                    pnlMachineConfig.Visible = false;
                    pnlCriticalError.Visible = false;
                    pnlResults.Visible = false;
                    pnlKnapp.Visible = false;
                    pnlDump.Visible = false;
                    pnlLiveFilter.Visible = false;
                    //camViewCtrl.UnbindCurrentCamera();
                    StartSettings(vertMenuBar1.Index, true);
                    //try {
                    //    pcbMain.Visible = false;
                    //    camViewCtrl.BindToCamera(vertMenuBar1.Index);

                    //}
                    //catch (InvalidWhenCameraInLiveModeException ex) {
                    //    MessageBox.Show("Operation non valid while camera is in live mode!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    vertMenuBar1.Index = camViewCtrl.CurrentCameraIndex;
                    //}
                    //catch (Exception ex) {
                    //    MessageBox.Show("Unable to get camera info for this recipe!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    vertMenuBar1.Index = camViewCtrl.CurrentCameraIndex;
                    //}
                    break;

                case "UTILITIES":
                    camViewCtrl.UnbindCurrentCamera();
                    recalcUtilities(ToolsEnum.Utilities);
                    visionSystemMgr.Displays["Main"].Suspend();
                    if (camViewCtrl.CurrentCameraIndex >= 0)
                    {
                        Display d = visionSystemMgr.Displays["Cam_" + camViewCtrl.CurrentCameraIndex.ToString()];
                        if (d != null)
                            d.Suspend();
                    }
                    pcbMain.Visible = false;
                    pnlReports.Visible = false;
                    pnlUtilities.Visible = true;
                    pnlImageViewer.Visible = false;
                    pnlResults.Visible = false;
                    pnlKnapp.Visible = false;
                    pnlDump.Visible = false;
                    pnlMachineConfig.Visible = false;
                    pnlLiveFilter.Visible = false;
                    pnlUtilities.BringToFront();
                    recalcUtilities(currentTool);
                    break;
                case "REPORT":
                    showReportPanel();
                    //printActiveRecipe();

                    //Modify **
                    reportViewer1.Display_Report();
                    reportViewer1.PrintingMexDialog += new EventHandler(PrintMexDialog);

                    break;
                case "SAVE2HMI":
                    DrawingControl.SuspendDrawing(GetMainPanel());
                    visionSystemMgr.ChangeRecipe -= visionSystemMgr_ChangeRecipe;
                    sendUploadRequest();
                    changeRecipe2HMI(true);
                    changeActiveRecipe(AppEngine.Current.CurrentContext.ActiveRecipe);
                    visionSystemMgr.ChangeRecipe += visionSystemMgr_ChangeRecipe;
                    DrawingControl.ResumeDrawing(GetMainPanel());
                    break;
                case "MACHINECONFIG":
                    camViewCtrl.UnbindCurrentCamera();
                    visionSystemMgr.Displays["Main"].Suspend();
                    if (camViewCtrl.CurrentCameraIndex >= 0)
                    {
                        Display d = visionSystemMgr.Displays["Cam_" + camViewCtrl.CurrentCameraIndex.ToString()];
                        if (d != null)
                            d.Suspend();
                    }
                    pcbMain.Visible = false;
                    pnlUtilities.Visible = false;
                    pnlReports.Visible = false;
                    pnlImageViewer.Visible = false;
                    pnlDump.Visible = false;
                    pnlMachineConfig.Visible = true;
                    pnlLiveFilter.Visible = false;
                    pnlMachineConfig.BringToFront();
                    //if (pnlMachineConfig.Visible && cameraConfigUI != null) {
                    //    visionSystemMgr.StartRescan();
                    //}
                    break;
                case "DUMPIMAGES":
                    camViewCtrl.UnbindCurrentCamera();

                    dump1.frmMain = this;
                    recalcUtilities(ToolsEnum.Dump);
                    visionSystemMgr.Displays["Main"].Suspend();
                    if (camViewCtrl.CurrentCameraIndex >= 0)
                    {
                        Display d = visionSystemMgr.Displays["Cam_" + camViewCtrl.CurrentCameraIndex.ToString()];
                        if (d != null)
                            d.Suspend();
                    }
                    pcbMain.Visible = false;
                    pnlReports.Visible = false;
                    pnlUtilities.Visible = true;
                    pnlImageViewer.Visible = false;
                    pnlResults.Visible = false;
                    pnlKnapp.Visible = false;
                    pnlDump.Visible = false;
                    pnlMachineConfig.Visible = false;
                    pnlLiveFilter.Visible = false;
                    pnlUtilities.BringToFront();
                    recalcUtilities(currentTool);
                    //pnlUtilities.Visible = false;
                    //pnlReports.Visible = false;
                    //pnlImageViewer.Visible = false;
                    //pnlMachineConfig.Visible = false;
                    //pnlDump.Visible = true;
                    //pnlDump.BringToFront();
                    //if (pnlMachineConfig.Visible && cameraConfigUI != null) {
                    //    visionSystemMgr.StartRescan();
                    //}
                    break;
                case "FILTER":
                    pcbMain.Visible = false;
                    pnlReports.Visible = false;
                    pnlUtilities.Visible = false;
                    pnlImageViewer.Visible = false;
                    pnlResults.Visible = false;
                    pnlKnapp.Visible = false;
                    pnlDump.Visible = false;
                    pnlMachineConfig.Visible = false;
                    pnlLiveFilter.Visible = true;
                    pnlLiveFilter.BringToFront();
                    break;
            }

            //redis view cam
            if (AppEngine.Current.MachineConfiguration.IsRedisVisibleStationFilter)
            {
                if (e.ButtonPressedName == "HOME" || e.ButtonPressedName == "PREV" || e.ButtonPressedName == "NEXT")
                    setRedisViewCam(false, e.ButtonPressedName);
                else
                    if(lastButtonPressedName == "HOME" || lastButtonPressedName == "PREV" || lastButtonPressedName == "NEXT")
                        setRedisViewCam(true); //reset all values
            }

            lastButtonPressedName = e.ButtonPressedName;
        }


        void setRedisViewCam(bool resetAllValues, string type = null)
        {
            try
            {
                Dictionary<string, string> dicToSet = new Dictionary<string, string>();
                foreach (NodeSetting node in AppEngine.Current.MachineConfiguration.NodeSettings)
                {
                    string key = $"OPTREL:Inspection:{node.IP4Address}";
                    if (dicToSet.ContainsKey(key) == false)
                        dicToSet.Add(key, "");
                }

                if (resetAllValues == false)
                {
                    foreach (CameraSetting cam in AppEngine.Current.MachineConfiguration.CameraSettings)
                    {
                        bool add = false;
                        if(type == "HOME")
                        {
                            if (cam.PageNumberPosition == vertMenuBar1.CurrentHomeScreen - 1)
                                add = true;
                        }
                        else if (type == "PREV" || type == "NEXT")
                        {
                            int camIndex = vertMenuBar1.Index;
                            if (cam.Id == camIndex)
                                add = true;
                        }

                        if (add)
                        {
                            string ip4 = null;
                            foreach (NodeSetting node in AppEngine.Current.MachineConfiguration.NodeSettings)
                                if (node.Id == cam.Node)
                                    ip4 = node.IP4Address;

                            string key = $"OPTREL:Inspection:{ip4}";
                            string value = cam.Station.ToString();
                            if (dicToSet.ContainsKey(key))
                            {
                                if (dicToSet[key] == "")
                                    dicToSet[key] = value;
                                else
                                    dicToSet[key] = dicToSet[key] += $";{value}";
                            }
                        }
                    }
                }

                //set values
                if (_redisComm.IsConnected)
                {
                    foreach (KeyValuePair<string, string> kvp in dicToSet)
                        _redisComm.SetValue(kvp.Key, kvp.Value);
                }
                else
                {
                    string errValues = "";
                    foreach (KeyValuePair<string, string> kvp in dicToSet)
                        errValues += $"key=[{kvp.Key}] value=[{kvp.Value}]; ";
                    Log.Line(LogLevels.Warning, "frmMain.setRedisViewCam", $"Redis is not connected, failed to set {errValues}");
                }
            }
            catch(Exception ex)
            {
                Log.Line(LogLevels.Error, "frmMain.setRedisViewCam", $"Fatal error to set redis: {ex.Message}");
            }
        }


        internal void SetCaronteEnable(bool enable)
        {
            try
            {
                if (_redisComm.IsConnected)
                {
                    if(enable)
                        _redisComm.PublishMessage("OPTREL:Caronte:all:enable", "");
                    else
                        _redisComm.PublishMessage("OPTREL:Caronte:all:disable", "");

                    Log.Line(LogLevels.Pass, "frmMain.SetCaronteEnable", $"Publish message to enable={enable}");
                }
                else
                {
                    Log.Line(LogLevels.Warning, "frmMain.SetCaronteEnable", $"Redis is not connected, failed to publish message to enable={enable}");
                }
            }
            catch(Exception ex)
            {
                Log.Line(LogLevels.Error, "frmMain.SetCaronteEnable", $"Fatal error to set redis: {ex.Message}");
            }
        }


        internal void changeRecipe2HMI(bool toSave, int? idNode = null, bool sendRecipeUpdatedAck = true)
        {

            if (AppEngine.Current.CurrentContext.MachineMode != MachineModeEnum.Running &&
                        AppEngine.Current.CurrentContext.ActiveRecipe != null)
            {

                Task taskSaveRecipe = new Task(new Action(() => {

                    CultureInfo ci = new CultureInfo(AppEngine.Current.CurrentContext.CultureCode);
                    Thread.CurrentThread.CurrentCulture = ci;
                    Thread.CurrentThread.CurrentUICulture = ci;

                    bool res = true;
                    if (!AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Busy))
                    {
                        Log.Line(LogLevels.Error, "frmMain.changeRecipe2HMI", "\"Busy\" mode not set");
                        headerStrip.SetErrorText(frmBase.UIStrings.GetString("NoEditingAvailable"));
                    }
                    else
                    {
                        Log.Line(LogLevels.Pass, "frmMain.changeRecipe2HMI", "\"Busy\" mode set successfully");
                        headerStrip.SetErrorText("");
                    }
              
                    if (toSave)
                    {
                        Log.Line(LogLevels.Pass, "frmMain.changeRecipe2HMI", "Saving current recipe");
                        statusStrip.SetStatusMessage(frmBase.UIStrings.GetString("SavingCurrentRecipe"));
                        if (!statusStrip.InvokeRequired)
                            statusStrip.Refresh();
                        res = AppEngine.Current.TrySaveRecipe();

                        // Matteo 06/09/2024: DTwin import recipe button should not send any ACK/NACK back to GRETEL.
                        if (sendRecipeUpdatedAck)
                        {
                            if (!res)
                            {
                                headerStrip.SetErrorText(frmBase.UIStrings.GetString("SavingCurrentRecipeFailed"));
                                Log.Line(LogLevels.Error, "frmMain.changeRecipe2HMI", "Recipe NOT saved. A scada error occurred");
                                if (idNode != null)
                                    visionSystemMgr.Nodes[(int)idNode].RecipeUpdatedError("exacta easy error: recipe not saved"); //max about 45 characters
                            }
                            else
                            {
                                Log.Line(LogLevels.Pass, "frmMain.changeRecipe2HMI", "Recipe saved successfully");
                                headerStrip.SetErrorText("");
                                if (idNode != null)
                                    visionSystemMgr.Nodes[(int)idNode].RecipeUpdatedOK();
                                res = true;
                            }
                        }
                    }
                    // OLD AUDIT
                    /*if (res == true) {
                        if (AppEngine.Current.TryAuditRecipe() == false) {
                            headerStrip.SetErrorText(frmBase.UIStrings.GetString("AuditFailed"));
                            Log.Line(LogLevels.Error, "frmMain.changeRecipe2HMI", "AUDIT sent failed");
                        }
                        else {
                            Log.Line(LogLevels.Pass, "frmMain.changeRecipe2HMI", "AUDIT sent successfully");
                            headerStrip.SetErrorText("");
                            res = true;
                        }
                    }*/
                    //Thread.Sleep(500);
                    if (!AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.ReadyToRun))
                    {
                        Log.Line(LogLevels.Error, "frmMain.changeRecipe2HMI", "\"Ready to Run\" mode not set");
                        if (res) 
                            headerStrip.SetErrorText(frmBase.UIStrings.GetString("NoReadyToRunAvailable"));
                    }
                    else
                    {
                        Log.Line(LogLevels.Pass, "frmMain.changeRecipe2HMI", "\"Ready to Run\" set successfully");
                        headerStrip.SetErrorText("");
                    }
                    //Thread.Sleep(500);
                    camViewCtrl.ConfirmSavedParameters();
                    statusStrip.SetStatusMessage("");
                }));
                taskSaveRecipe.Start();
            }
            else
            {
                if (toSave)
                {
                    string recipeName = "???";
                    if (AppEngine.Current.CurrentContext.ActiveRecipe != null) 
                        recipeName = AppEngine.Current.CurrentContext.ActiveRecipe.RecipeName;
                    Log.Line(LogLevels.Warning, "frmMain.vertMenuBar1_ButtonPressed", "Recipe NOT saved. Machine mode: " + AppEngine.Current.CurrentContext.MachineMode.ToString() + ";   Recipe: " + recipeName);
                    
                    if (AppEngine.Current.CurrentContext.MachineMode == MachineModeEnum.Running)
                    {
                        headerStrip.SetErrorText(frmBase.UIStrings.GetString("CantSaveRecipeWhileMachineRunning"));
                        if (idNode != null)
                            visionSystemMgr.Nodes[(int)idNode].RecipeUpdatedError("exacta easy error: machine is running"); //max about 45 characters
                    }
                    else
                    {
                        headerStrip.SetErrorText(frmBase.UIStrings.GetString("CantSaveRecipe"));
                        if (idNode != null)
                            visionSystemMgr.Nodes[(int)idNode].RecipeUpdatedError("exacta easy error: unknown error"); //max about 45 characters
                    }
                }
                // Matteo 06-09-2024: the task updateRecipe sets the supervisor in a BUSY state. If the
                // MachineMode is in RUNNING state, we need to set it to Error, otherwise the supervisor 
                // is stuck in BUSY state.
                if (!AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Error))
                    Log.Line(LogLevels.Error, "frmMain.changeRecipe2HMI", "\"Error\" mode not set");
                else
                    Log.Line(LogLevels.Pass, "frmMain.changeRecipe2HMI", "\"Error\" set successfully");
            }
        }

        internal void sendUploadRequest()
        {

            if (AppEngine.Current.CurrentContext.MachineMode != MachineModeEnum.Running &&
                        AppEngine.Current.CurrentContext.ActiveRecipe != null)
            {

                Task[] tasks = new Task[visionSystemMgr.Nodes.Count];
                int id = 0;
                foreach (INode n in visionSystemMgr.Nodes)
                {
                    tasks[id] = new Task(new Action(() => {

                        CultureInfo ci = new CultureInfo(AppEngine.Current.CurrentContext.CultureCode);
                        Thread.CurrentThread.CurrentCulture = ci;
                        Thread.CurrentThread.CurrentUICulture = ci;
                        if (n.ProviderName == "GretelNodeBase")
                        {
                            n.UploadParameters();
                        }
                    }));
                    tasks[id].Start();
                    id++;
                }
                Task.WaitAll(tasks);
            }
        }

        void createDisplays()
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (InvokeRequired && IsHandleCreated)
                this.Invoke(new MethodInvoker(createDisplays));
            else
            {
                Log.Line(LogLevels.Pass, "frmMain.createDisplays", "Creating display ...");
                //for (int i = 0; i < visionSystemMgr.Displays.Count; i++) {
                //    Display d = visionSystemMgr.Displays[i];
                //    d.Dispose();
                //    d = null;
                //}
                //visionSystemMgr.Displays.Clear();
                //if (camViewCtrl != null && camViewCtrl.camCtrlList != null) {
                //    for (int i = 0; i < camViewCtrl.camCtrlList.Count; i++)
                //        camViewCtrl.camCtrlList[i].Destroy();
                //}
                //camViewCtrl = new CameraViewerController(this, visionSystemMgr, machineConfiguration);
                //camViewCtrl.ChangedDownloadedImages += new EventHandler<NewFolderEventArgs>(camViewCtrl_ChangedDownloadedImages);
                int maxPages = 0;
                foreach (CameraSetting cs in machineConfiguration.CameraSettings)
                {
                    maxPages = Math.Max(maxPages, cs.PageNumberPosition + 1);
                }
                for (int i = 0; i < machineConfiguration.NumberOfCamera; i++)
                {
                    StartSettings(i, false);
                    camViewCtrl.UnbindCurrentCamera();
                }
                camViewCtrl.DataSource = AppEngine.Current.CurrentContext.ActiveRecipe;
                try
                {
                    //stationsOrdered = visionSystemMgr.Stations.Sort(machineConfiguration.CameraSettings, machineConfiguration.VisualizerRows * machineConfiguration.VisualizerCols);
                    visionSystemMgr.Displays.Add(
                        new ScreenGridDisplay(
                            "Main", 
                            pcbMain,
                            visionSystemMgr.Stations,
                            machineConfiguration.DisplaySettings, 
                            machineConfiguration.CameraSettings,
                            machineConfiguration.VisualizerKeepRatio, 
                            machineConfiguration.VisualizerImageQuality,
                            machineConfiguration.VisualizerShowColoredBorder));

                    vertMenuBar1.TotalScreens = ((ScreenGridDisplay)visionSystemMgr.Displays["Main"]).PageCount;   
                    vertMenuBar1.CurrentHomeScreen = ((ScreenGridDisplay)visionSystemMgr.Displays["Main"]).CurrentPage + 1;
                }
                catch (Exception ex)
                {
                    fatalError(ex);
                    return;
                }
            }
        }


        void showHomePagePanel()
        {

            if (camViewCtrl.CurrentCameraIndex >= 0)
            {
                Display d = visionSystemMgr.Displays["Cam_" + camViewCtrl.CurrentCameraIndex.ToString()];
                if (d != null)
                    d.Suspend();
            }
            pcbMain.Visible = true;
            pnlUtilities.Visible = false;
            pnlReports.Visible = false;
            pnlImageViewer.Visible = false;
            pnlMachineConfig.Visible = false;
            pnlDump.Visible = false;
            pnlLiveFilter.Visible = false;
            camViewCtrl.UnbindCurrentCamera();
            visionSystemMgr.Displays["Main"].Resume();
            pcbMain.BringToFront();
            if (lastButtonPressedName == "HOME")
                ((ScreenGridDisplay)visionSystemMgr.Displays["Main"]).NextPage();
            else
                visionSystemMgr.RedrawDisplay("Main");
            if (visionSystemMgr.Displays != null)
                vertMenuBar1.CurrentHomeScreen = ((ScreenGridDisplay)visionSystemMgr.Displays["Main"]).CurrentPage + 1;
            else
                vertMenuBar1.CurrentHomeScreen = 1;
        }

        void showMachineConfigPanel()
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (InvokeRequired && IsHandleCreated)
                this.Invoke(new MethodInvoker(showMachineConfigPanel));
            else
            {
                pcbMain.Visible = false;
                pnlUtilities.Visible = false;
                pnlReports.Visible = false;
                pnlImageViewer.Visible = false;
                pnlMachineConfig.Visible = true;
                pnlKnapp.Visible = false;
                pnlDump.Visible = false;
                pnlResults.Visible = false;

                if(!(camViewCtrl is null))
                    camViewCtrl.UnbindCurrentCamera();

                if (visionSystemMgr.Displays["Main"] != null)
                    visionSystemMgr.Displays["Main"].Suspend();
                pnlMachineConfig.BringToFront();
            }
        }

        void showReportPanel()
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (InvokeRequired && IsHandleCreated)
                this.Invoke(new MethodInvoker(showReportPanel));
            else
            {
                try
                {
                    visionSystemMgr.Displays["Main"].Suspend();
                    if (camViewCtrl.CurrentCameraIndex >= 0)
                    {
                        Display d = visionSystemMgr.Displays["Cam_" + camViewCtrl.CurrentCameraIndex.ToString()];
                        if (d != null)
                            d.Suspend();
                    }
                    headerStrip.SetErrorText("");
                    //camViewCtrl.UnbindCurrentCamera();
                    //if (assistantUI != null)
                    //    assistantUI.UnbindCurrentCamera();
                    pnlUtilities.Visible = false;
                    pnlReports.Visible = true;
                    pnlImageViewer.Visible = false;
                    pnlMachineConfig.Visible = false;
                    pnlKnapp.Visible = false;
                    pnlDump.Visible = false;
                    pnlResults.Visible = false;
                    pnlLiveFilter.Visible = false;
                    pnlReports.BringToFront();
                }
                catch
                {
                    Log.Line(LogLevels.Error, "frmMain.showReportPanel", "Errore visualizzazione report");
                    headerStrip.SetErrorText("Errore visualizzazione report");
                }
            }
        }


        private void StartSettings(int idCamera, bool show)
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (InvokeRequired && IsHandleCreated)
            {
                this.Invoke(new MethodInvoker(() => StartSettings(idCamera, show)));
            }
            else
            {
                try
                {
                    pcbMain.Visible = false;
                    camViewCtrl.BindToCamera(idCamera, show);
                    //if (assistantUI != null)
                    //    assistantUI.BindToCamera((Camera)visionSystemMgr.Cameras[vertMenuBar1.Index]);
                }
                //catch (InvalidWhenCameraInLiveModeException ex) {
                //    Log.Line(LogLevels.Error, "frmMain.StartSettings", "Operation non valid while camera is in live mode! Error: " + ex.Message);
                //    headerStrip.ErrorText = "Operation non valid while camera is in live mode!";
                //    vertMenuBar1.Index = camViewCtrl.CurrentCameraIndex;
                //}
                catch (Exception ex)
                {
                    if (AppEngine.Current.CurrentContext.IsCameraRecipeEnabled(idCamera) &&
                        visionSystemMgr.IsStationEnabled(idCamera))
                    {
                        Log.Line(LogLevels.Error, "frmMain.StartSettings", "Can't bind to camera! Error: " + ex.Message);
                        headerStrip.ErrorText = frmBase.UIStrings.GetString("CantBindToCamera") + " " + vertMenuBar1.Index.ToString();
                    }
                    if (ex.Message == "Camera not exists")
                        ShowCriticalPanel(frmBase.UIStrings.GetString("CameraNotFound2") + "!");
                    else if (ex.Message == "Camera recipe not exists")
                        ShowCriticalPanel(frmBase.UIStrings.GetString("CameraRecipeNotFound") + "!");
                    else if (ex.Message.Contains("Index was out of range") == true)
                        ShowCriticalPanel(frmBase.UIStrings.GetString("DeviceRecipeWrong") + "!");
                    else//if (ex.Message.Contains("not in ["))
                        ShowCriticalPanel(ex.Message);
                    //vertMenuBar1.Index = camViewCtrl.CurrentCameraIndex;
                }
            }
        }

        public void ShowCriticalPanel(string message)
        {

            int pLeft = (pnlMain.Width - pnlCriticalError.Width) / 2;
            pLeft = pLeft >= 0 ? pLeft : 0;
            pnlCriticalError.Left = pLeft;
            int pTop = (pnlMain.Height - pnlCriticalError.Height) / 2;
            pTop = pTop >= 0 ? pTop : 0;
            pnlCriticalError.Left = pLeft;
            pnlCriticalError.Top = pTop;
            pnlCriticalError.Visible = true;
            pnlCriticalError.BringToFront();
            lblCriticalMessage.Text = message;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            const int borderSize = 3;
            Form form = new Form();
            form.FormBorderStyle = FormBorderStyle.None;
            form.StartPosition = FormStartPosition.CenterParent;
            form.Size = new Size(420, 65);
            form.BackColor = SystemColors.Control;
            form.TopMost = true;
            form.BringToFront();

            YesNoPanel yesNo = new YesNoPanel();
            yesNo.Size = new Size(form.Width - borderSize * 2, form.Height - borderSize * 2);
            yesNo.Location = new Point(borderSize, borderSize);
            yesNo.YesNoAnswer += (obj, arg) =>
            {
                form.Close();
                if(arg.Message == "Yes")
                {
                    Destroy(false);
                }
                else
                {
                    yesNo.Dispose();
                    form.Dispose();
                }
            };

            form.Controls.Add(yesNo);
            form.ShowDialog();
        }

        public void Destroy(bool restart)
        {

            lock (lockGrid)
            {
                logger1.OnNewMessage -= logger1_OnNewMessage;
                logGridView.HandleCreated -= logGridView_HandleCreated;
            }
            Thread.Sleep(50);
            logger1.Dispose();
            reportViewer1.BeforePrintReport -= reportViewer1_BeforePrintReport;
            visionSystemMgr.RescanCompleted -= visionSystemMgr_RescanCompleted;
            visionSystemMgr.ChangeRecipe -= visionSystemMgr_ChangeRecipe;
            visionSystemMgr.InitializationCompleted -= visionSystemMgr_InitializationCompleted;
            visionSystemMgr.NewNodeBootDone -= visionSystemMgr_NewNodeBootDone;
            visionSystemMgr.SavingBufferedImages -= visionSystemMgr_SavingBufferedImages;
            visionSystemMgr.NotifyGUI -= visionSystemMgr_NotifyGUI;
            visionSystemMgr.EnableNavigation -= visionSystemMgr_EnableNavigation;

            //pnlMain.SizeChanged -= pnlMain_SizeChanged;
            pcbMain.SizeChanged -= pcbMain_SizeChanged;
            visionSystemMgr.RemoteDesktopDisconnection -= visionSystemMgr_RemoteDesktopDisconnection;
            AppEngine.Current.ContextChanged -= Current_ContextChanged;
            AppEngine.Current.PrintExactaEasyActiveRecipeRequested -= Current_PrintActiveRecipe;
            AppEngine.Current.SetExactaEasyOnTopRequested -= Current_SetSupervisorOnTopRequested;
            AppEngine.Current.HideExactaEasyRequested -= Current_SetSupervisorHideRequested;
            // TODO: not sure if this is really needed for GATEWAY2.
            //AppEngine.Current.HMIContextCommunicationError -= Current_HMIContextCommunicationError;
            AppEngine.Current.AppEngineStartStopBatch -= Current_StartStopBatch;
            AppEngine.Current.AppEngineStartStopKnapp -= Current_StartStopKnapp;
            TaskObserver.TheObserver.DispatchObservation -= TheObserver_DispatchObservation;

            //AppEngine.Current.SaveParameterInfoDictionary();
            RestartForm = restart;

            camViewCtrl.Destroy();

            if (visionSystemMgr != null)
            {
                foreach (Display d in visionSystemMgr.Displays)
                    d.Dispose();
                visionSystemMgr.Displays.Clear();
                foreach (Graph g in visionSystemMgr.Graphs)
                    g.Dispose();
                visionSystemMgr.Graphs.Clear();
                foreach (MeasuresContainer mc in visionSystemMgr.MeasuresContainer)
                    mc.Dispose();
                visionSystemMgr.MeasuresContainer.Clear();
                foreach (MeasuresContainer mc in visionSystemMgr.DBMeasures)
                    mc.Dispose();
                visionSystemMgr.DBMeasures.Clear();
            }
            this.Close();
        }

        void refreshForm()
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (InvokeRequired && IsHandleCreated)
                this.Invoke(new MethodInvoker(refreshForm));
            else
            {
                refreshForm(AppEngine.Current.CurrentContext);
            }
        }

        void refreshForm(ExactaEasyEng.AppContext context)
        {

            if (context.CultureCode != CultureCode)
            {
                if (context.MachineMode != MachineModeEnum.Running)
                {
                    frmMain.ForceWindowHidden = true;   //se è un cambio lingua mi riavvio nascosto...
                    this.Destroy(true);
                }
                else
                {
                    headerStrip.ErrorText = frmBase.UIStrings.GetString("CantChangeLanguageWhileMachineIsRunning");
                }
            }
            if (context.SupervisorMode == SupervisorModeEnum.Busy)
            {
                if (vertMenuBar1 != null)
                    vertMenuBar1.Enabled = false;
                //if (camViewCtrl != null)
                //    camViewCtrl.DisableCurrentCamera();
            }
            else
            {
                //if (camViewCtrl != null)
                //    camViewCtrl.EnableCurrentCamera();
                if (vertMenuBar1 != null)
                    vertMenuBar1.Enabled = true;
            }
            if (context.MachineMode == MachineModeEnum.Running)
            {
                vertMenuBar1.SetAdminPanelEnabled(false);
                vertMenuBar1.SetReportPanelEnabled(false);
                vertMenuBar1.SetConfigPanelEnabled(false);

                if(!AppEngine.Current.MachineConfiguration.SaveImagesWhileRunning)
                    vertMenuBar1.SetDumpEnabled(false);
            }
            else
            {
                vertMenuBar1.SetAdminPanelEnabled(true);
                vertMenuBar1.SetReportPanelEnabled(true);
                vertMenuBar1.SetConfigPanelEnabled(true);
                vertMenuBar1.SetDumpEnabled(true);

            }

            if (context.AppClientRect.X != this.Location.X || context.AppClientRect.Y != this.Location.Y)
            {
                //Log.Line(LogLevels.Pass, "frmMain.refreshForm", "Location change: X=" + context.AppClientRect.Location.X + ", Y=" + context.AppClientRect.Location.Y);
                this.Location = context.AppClientRect.Location;
            }
            if (((context.AppClientRect.Width != this.Size.Width) && (context.AppClientRect.Width > 640)) ||
                ((context.AppClientRect.Height != this.Size.Height) && (context.AppClientRect.Height > 480)))
            {
                //Log.Line(LogLevels.Pass, "frmMain.refreshForm", "Size change: W=" + context.AppClientRect.Size.Width + ", H=" + context.AppClientRect.Size.Height);
                this.Size = context.AppClientRect.Size;
            }
            if ((camViewCtrl != null) && (context.ActiveRecipe != camViewCtrl.DataSource))
            {
                changeActiveRecipe(context.ActiveRecipe);
            }
            if (currMachineMode != context.MachineMode)
            {
                currMachineMode = context.MachineMode;
                dump1.RecalcButtons();
                statusStrip.MachineStatus = currMachineMode.ToString();
                if (camViewCtrl != null)
                    camViewCtrl.UpdateCurrentCamViewer();
            }
            if (currUserLevel != context.UserLevel)
            {
                currUserLevel = context.UserLevel;
                changeUserLevel(currUserLevel);
                statusStrip.UserLevel = string.Format(frmBase.UIStrings.GetString("UserLevelMsg"), currUserLevel);
                if (camViewCtrl != null)
                    camViewCtrl.UpdateCurrentCamViewer();
                lastButtonPressedName = "";
                showHomePagePanel();
                camViewCtrl.SetParametersPage();
            }
            //headerStrip.ErrorText = "";
            try
            {
                if (context.ActiveRecipe != null && context.ActiveRecipe.Cams != null)
                {
                    foreach (Cam cam in context.ActiveRecipe.Cams)
                    {
                        if (visionSystemMgr.IsStationEnabled(cam.Id) &&
                            cam.Enabled && visionSystemMgr.CameraReady(cam.Id) != CameraNewStatus.Ready)
                        {

                            Log.Line(LogLevels.Error, "frmMain.refreshForm", "Camera ID {0} enabled by HMI or Recipe but out of work.", cam.Id);
                            headerStrip.ErrorText = frmBase.UIStrings.GetString("AtLeastOneCameraInError");
                        }
                    }
                }
            }
            catch
            {
                Log.Line(LogLevels.Error, "frmMain.refreshForm", "At least one camera enabled by HMI or Recipe but out of work.");
                headerStrip.ErrorText = frmBase.UIStrings.GetString("AtLeastOneCameraInError");
            }
        }

        internal void changeActiveRecipe(Recipe newRecipe)
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            if (InvokeRequired && IsHandleCreated)
            {
                this.Invoke(new MethodInvoker(() => changeActiveRecipe(newRecipe)));
            }
            else
            {
                if (newRecipe != null)
                {
                    camViewCtrl.DataSource = newRecipe;
                    //if (newRecipe.RecipeVersion != null && newRecipe.RecipeVersion != "")
                    //    statusStrip.RecipeName += " (ver " + newRecipe.RecipeVersion + ")";
                    vertMenuBar1.SetSettingsEnabled(true);
                    if (cameraConfigUI != null)
                        cameraConfigUI.ChangeRecipeEnable(newRecipe);
                }
                else
                {
                    vertMenuBar1.SetSettingsEnabled(false);
                }
                if (lastButtonPressedName == "PREV" || lastButtonPressedName == "NEXT" || lastButtonPressedName == "SETTINGS")
                {
                    StartSettings(vertMenuBar1.Index, true);
                }
            }
        }

        void changeUserLevel(UserLevelEnum userLevel)
        {

            if (userLevel >= UserLevelEnum.Administrator)
            {
                vertMenuBar1.SetAdminPanelVisible(true);
                btnTabUtilities.Visible = true;
            }
            else
            {
                vertMenuBar1.SetAdminPanelVisible(false);
                btnTabUtilities.Visible = false;
                if (currentTool == ToolsEnum.Utilities)
                    recalcUtilities(ToolsEnum.Log);
            }
            if (userLevel >= UserLevelEnum.Engineer)
            {
                //btnDump.Visible = true;
            }
            else
            {
                //btnDump.Visible = false;               
                if (currentTool == ToolsEnum.Dump)
                    recalcUtilities(ToolsEnum.Log);
            }
            if (userLevel >= UserLevelEnum.AssistantSupervisor)
            {
                vertMenuBar1.SetDumpVisible(true);
            }
            else
            {
                vertMenuBar1.SetDumpVisible(false);
            }
            if (userLevel < UserLevelEnum.Engineer)
            {
                cameraConfigUI.Enabled = false;
            }
            else
            {
                cameraConfigUI.Enabled = true;
            }
            if (userLevel < UserLevelEnum.Supervisor)
            {
                vertMenuBar1.SetReportPanelVisible(false);
            }
            else
            {
                vertMenuBar1.SetReportPanelVisible(true);
            }
        }

        void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void btnLoadRecipe_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog oDialog = new OpenFileDialog())
            {
                string currentPath = System.IO.Directory.GetCurrentDirectory();
                if (System.IO.Directory.Exists(AppEngine.Current.GlobalConfig.RecipesFolder))
                    oDialog.InitialDirectory = AppEngine.Current.GlobalConfig.RecipesFolder;
                oDialog.Filter = "Recipe Files (*.xml)|*.xml";
                oDialog.RestoreDirectory = true;
                if (oDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Recipe recipe = Recipe.LoadFromFile(oDialog.FileName);
                        if (sender == btnLoadRecipeAndApply)
                            AppEngine.Current.SetActiveRecipe(recipe, true);
                        else
                            AppEngine.Current.SetActiveRecipe(recipe, false);
                        headerStrip.SetErrorText("");
                    }
                    catch (Exception ex)
                    {
                        headerStrip.SetErrorText("Recipe loading failed! " + ex.ToString());
                    }
                }
                System.IO.Directory.SetCurrentDirectory(currentPath);
            }
        }

        //private void btnGetParamFromCamera_Click(object sender, EventArgs e) {

        //    createRecipeFromCameraParams();
        //}

        private void btnSaveCurrentRecipe_Click(object sender, EventArgs e)
        {
            saveCurrentRecipe();
        }

        //private void btnGetVialAxis_Click(object sender, EventArgs e) {

        //    exportVialAxis();
        //}

        void saveCurrentRecipe()
        {

            using (SaveFileDialog saveFileDlg = new SaveFileDialog())
            {
                if (AppEngine.Current.GlobalConfig.RecipesFolder != null)
                    saveFileDlg.InitialDirectory = AppEngine.Current.GlobalConfig.RecipesFolder;
                saveFileDlg.RestoreDirectory = true;
                saveFileDlg.Filter = "Recipe File (*.xml)|*.xml";
                if (AppEngine.Current.CurrentContext.ActiveRecipe != null)
                {
                    if (DialogResult.OK == saveFileDlg.ShowDialog())
                    {
                        AppEngine.Current.CurrentContext.ActiveRecipe.SaveXml(saveFileDlg.FileName);
                    }
                }
                else
                    headerStrip.ErrorText = frmBase.UIStrings.GetString("NoActiveRecipe");
            }
        }

        void sendCameraParams()
        {

            beginSendCameraParams(sendCameraParamsCompleted);
        }

        void sendCameraParamsCompleted(IAsyncResult ar)
        {

            //if (AppEngine.Current.TrySetSupervisorInReadyToRun()) {
            //    statusStrip.StatusMessage = "";
            //}
            //else {
            //    statusStrip.StatusMessage = "Error setting spv in Ready to run";
            //    //TODO: Gestire eccezione?
            //}
        }

        IAsyncResult beginSendCameraParams(AsyncCallback callback)
        {

            Task task = Task.Factory.StartNew(() => _sendCameraParams());
            if (callback != null) task.ContinueWith(_ => callback(task));
            return task;
        }

        void _sendCameraParams()
        {

            CultureInfo ci = new CultureInfo(AppEngine.Current.CurrentContext.CultureCode);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            bool ok = true;

            // Se il sistema è in fase di avvio, e tattile non è ancora inizializzata, no eseguo l'invio dei parametri
            // L'invio sarà eseguito sempre alla fine della fase di inzializzazione
            if (systemInitialized.WaitOne(0) && AppEngine.Current.CurrentContext.ActiveRecipe != null)
            {
                if (AppEngine.Current.TrySetSupervisorStatus(SupervisorModeEnum.Busy))
                {
                    statusStrip.StatusMessage = frmBase.UIStrings.GetString("SendParam");

                    if (AppEngine.Current.CurrentContext.ActiveRecipe != null)
                    {
                        headerStrip.ErrorText = "";
                        try
                        {
                            visionSystemMgr.SetParameters(AppEngine.Current.CurrentContext.ActiveRecipe);
                        }
                        catch (Exception ex)
                        {
                            Log.Line(LogLevels.Error, "frmMain.sendCameraParams", "Error: " + ex.Message);
                            headerStrip.ErrorText = frmBase.UIStrings.GetString("ErrorSendingParameters");
                            ok = false;
                        }
                    }
                }
                else
                {
                    statusStrip.StatusMessage = frmBase.UIStrings.GetString("SpvBusyError");
                }
            }

            visionSystemMgr.TryChangeSupervisorStatus(machineConfiguration.BypassHMIsendRecipe);

            if (ok)
            {
                statusStrip.StatusMessage = "";
            }
            else if (!ok)
            {
                statusStrip.StatusMessage = frmBase.UIStrings.GetString("ErrorSendingParameters");
            }
            else
            {
                if (ok) statusStrip.StatusMessage = frmBase.UIStrings.GetString("SpvReadyToRunError");
                else statusStrip.StatusMessage = frmBase.UIStrings.GetString("SpvErrorError");
                //TODO: Gestire eccezione?
            }
        }

        private void btnTabUtilities_Click(object sender, EventArgs e)
        {

            recalcUtilities(ToolsEnum.Utilities);
        }

        private void btnTabTrending_Click(object sender, EventArgs e)
        {
            recalcUtilities(ToolsEnum.Trending);
        }

        private void btnTabLogs_Click(object sender, EventArgs e)
        {

            recalcUtilities(ToolsEnum.Log);
        }

        private void btnResultsTable_Click(object sender, EventArgs e)
        {

            recalcUtilities(ToolsEnum.Results);
        }

        private void btnKnapp_Click(object sender, EventArgs e)
        {

            recalcUtilities(ToolsEnum.Knapp);
        }

        private void btnDump_Click(object sender, EventArgs e)
        {

            recalcUtilities(ToolsEnum.Dump);
        }

        private void btnTabDebugPage_Click(object sender, EventArgs e)
        {
            recalcUtilities(ToolsEnum.DebugPage);
        }

        void recalcUtilities(ToolsEnum chosenTool)
        {

            currentTool = chosenTool;
            foreach (Control ctrl in pnlTabs.Controls)
            {
                if (ctrl is Panel)
                    ((Panel)ctrl).Visible = false;
            }
            btnTabUtilities.BackColor = btnTabLogs.BackColor = btnResultsTable.BackColor =  btnTabSwVersion.BackColor = btnTabTrending.BackColor = btnTabDebugPage.BackColor = SystemColors.ButtonShadow;
            switch (currentTool)
            {
                case ToolsEnum.Utilities:
                    btnTabUtilities.BackColor = SystemColors.ControlDarkDark;
                    pnlTabUtilities.Visible = true;
                    pnlToolsMenu.Visible = true;
                    pnlTabUtilities.BringToFront();
                    break;
                case ToolsEnum.Trending:
                    btnTabTrending.BackColor = SystemColors.ControlDarkDark;
                    pnlTabTrending.Visible = true;
                    pnlToolsMenu.Visible = true;
                    pnlTabTrending.BringToFront();
                    break;
                //case ToolsEnum.Reports:
                //    btnTabUtilities.BackColor = SystemColors.ButtonShadow;
                //    //btnTabReports.BackColor = SystemColors.ControlDarkDark;
                //    btnTabAssistants.BackColor = SystemColors.ButtonShadow;
                //    break;
                //case ToolsEnum.Assistants:
                //    btnTabUtilities.BackColor = SystemColors.ButtonShadow;
                //    btnTabAssistants.BackColor = SystemColors.ControlDarkDark;
                //    btnTabLogs.BackColor = SystemColors.ButtonShadow;
                //    break;
                case ToolsEnum.Log:
                    btnTabLogs.BackColor = SystemColors.ControlDarkDark;
                    pnlTabLogs.Visible = true;
                    pnlToolsMenu.Visible = true;
                    pnlTabLogs.BringToFront();
                    break;
                case ToolsEnum.Results:
                    btnResultsTable.BackColor = SystemColors.ControlDarkDark;
                    pnlResults.Visible = true;
                    pnlToolsMenu.Visible = true;
                    pnlResults.BringToFront();
                    break;

                case ToolsEnum.SoftwareVersion:
                    btnTabSwVersion.BackColor = SystemColors.ControlDarkDark;
                    pnlTabSwVersion.Visible = true;
                    pnlToolsMenu.Visible = true;
                    pnlTabSwVersion.BringToFront();
                    break;
                case ToolsEnum.Dump:
                    pnlDump.Visible = true;
                    pnlToolsMenu.Visible = false;
                    pnlDump.BringToFront();
                    break;
                case ToolsEnum.DebugPage:
                    btnTabDebugPage.BackColor = SystemColors.ControlDarkDark;
                    pnlTabDebugPage.Visible = true;
                    pnlToolsMenu.Visible = true;
                    pnlTabDebugPage.BringToFront();
                    break;
                default:
                    Log.Line(LogLevels.Error, "frmMain.recalcUtilities", "Tool not yet implemented");
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            printActiveRecipe();
        }

        private void printActiveRecipe()
        {

            string reportPath = "";
            System.Reflection.Assembly startAssembly = System.Reflection.Assembly.GetEntryAssembly();
            if (startAssembly != null && startAssembly.ManifestModule.Name.ToLower() == "exactaeasy.exe")
                reportPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            else
                reportPath = Environment.CurrentDirectory + @"\DotNet Components\ExactaEasy"; // Specifico per IFIX

            reportViewer1.ReportTemplatePath = reportPath;
            reportViewer1.ReportTemplateName = "ParametersNew1.xsl";
            if (AppEngine.Current.CurrentContext.ActiveRecipe != null)
            {
                Recipe cloneRec = AppEngine.Current.CurrentContext.ActiveRecipe.Clone(AppEngine.Current.ParametersInfo, AppEngine.Current.CurrentContext.CultureCode);
                //if (cloneRec.Cams != null) {
                //    foreach (Cam cam in cloneRec.Cams) {
                //        if (cam.AcquisitionParameters != null)
                //            cam.AcquisitionParameters.RemoveAll((Parameter p) => { return p.IsVisible > Math.Max(10, (int)AppEngine.Current.CurrentContext.UserLevel); });
                //        if (cam.DigitizerParameters != null)
                //            cam.DigitizerParameters.RemoveAll((Parameter p) => { return p.IsVisible > Math.Max(10, (int)AppEngine.Current.CurrentContext.UserLevel); });
                //        if (cam.RecipeSimpleParameters != null)
                //            cam.RecipeSimpleParameters.RemoveAll((Parameter p) => { return p.IsVisible > Math.Max(10, (int)AppEngine.Current.CurrentContext.UserLevel); });
                //        if (cam.RecipeAdvancedParameters != null)
                //            cam.RecipeAdvancedParameters.RemoveAll((Parameter p) => { return p.IsVisible > Math.Max(10, (int)AppEngine.Current.CurrentContext.UserLevel); });
                //        if (cam.StroboParameters != null)
                //            cam.StroboParameters.RemoveAll((Parameter p) => { return p.IsVisible > Math.Max(10, (int)AppEngine.Current.CurrentContext.UserLevel); });
                //        if (cam.MachineParameters != null)
                //            cam.MachineParameters.RemoveAll((Parameter p) => { return p.IsVisible > Math.Max(10, (int)AppEngine.Current.CurrentContext.UserLevel); });
                //        if (cam.ROIParameters != null) {
                //            for (int i = 0; i < cam.ROIParameters.Count; i++)
                //                cam.ROIParameters[i].RemoveAll((Parameter p) => { return p.IsVisible > Math.Max(10, (int)AppEngine.Current.CurrentContext.UserLevel); });
                //        }
                //        if (cam.Lights != null) {
                //            for (int i = 0; i < cam.Lights.Count; i++)
                //                if (cam.Lights[i].StroboParameters != null)
                //                    cam.Lights[i].StroboParameters.RemoveAll((Parameter p) => { return p.IsVisible > Math.Max(10, (int)AppEngine.Current.CurrentContext.UserLevel); });
                //        }
                //    }
                //}
                if (reportViewer1.ShowReport(new string[] { cloneRec.ToString(), AppEngine.Current.MachineConfiguration.ToString() }))
                {
                    headerStrip.ErrorText = "";
                }
                else
                    headerStrip.ErrorText = frmBase.UIStrings.GetString("ErrorReport");
            }
        }

        //private void btnTabAssistants_Click(object sender, EventArgs e) {
        //    /*Assistants assistants = new Assistants();
        //    assistants.CamAssistant = new Assistant();
        //    StroboAssistantParameter sap = new StroboAssistantParameter();
        //    sap.Id = "bubble";
        //    sap.IsVisible = 0;
        //    sap.IsEditable = 0;
        //    sap.Value = "" + 7;
        //    assistants.CamAssistant.StroboAssistantParameters = new StroboAssistantParameterCollection();
        //    assistants.CamAssistant.StroboAssistantParameters.Add(sap);
        //    assistants.SaveXml("C:/aaa.xml");*/
        //    recalcUtilities(ToolsEnum.Assistants);
        //    Assistants assistants = null;
        //    string assistantsConfigPath = AppEngine.Current.GlobalConfig.SettingsFolder + "/assistantsConfig.xml";
        //    if (File.Exists(assistantsConfigPath))
        //        assistants = Assistants.LoadFromFile(assistantsConfigPath);
        //    if (assistantUI != null && assistants != null) {
        //        assistantUI.BindToCamera((Camera)visionSystemMgr.Cameras[vertMenuBar1.Index]);
        //        assistantUI.DataSource = assistants.CamAssistant;
        //        assistantUI.UserLevel = AppEngine.Current.CurrentContext.UserLevel;
        //        assistantUI.LoadAssistant();
        //    }
        //    //pnlTabAssistants.Visible = true;
        //    //pnlTabAssistants.BringToFront();
        //}

        private void logger1_OnNewMessage(object sender, SPAMI.Util.Logger.MessageEventArgs args)
        {

            lock (lockGrid)
            {
                addLogNewRow(args.Message.level, args.Message.message);
            }
        }

        void buildLogDataTable()
        {

            logDataTable = new DataTable();
            logDataTable.Columns.Add("LogEntryTypeImage", typeof(Image));
            logDataTable.Columns.Add("EntryLevel", typeof(string));
            //logDataTable.Columns.Add("LogTime", typeof(DateTime));
            logDataTable.Columns.Add("EntryMessage", typeof(string));
            logGridView.Columns["LogEntryTypeImage"].DataPropertyName = "LogEntryTypeImage";
            logGridView.Columns["EntryLevel"].DataPropertyName = "EntryLevel";
            logGridView.Columns["EntryMessage"].DataPropertyName = "EntryMessage";
            logGridView.HandleCreated += logGridView_HandleCreated;
            logGridView.ColumnHeadersVisible = false;
            //logGridView.DataSource = logDataTable;

        }

        void logGridView_HandleCreated(object sender, EventArgs e)
        {
            RefreshLogGrid();
        }

        void buildSwVerDataTable()
        {

            swVerDataTable = new DataTable();
            swVerDataTable.Columns.Add("ModuleName", typeof(string));
            swVerDataTable.Columns.Add("Version", typeof(string));
            swVerGridView.Columns["ModuleName"].DataPropertyName = "ModuleName";
            swVerGridView.Columns["Version"].DataPropertyName = "Version";
            swVerGridView.Columns["ModuleName"].HeaderText = frmBase.UIStrings.GetString("ModuleName");
            swVerGridView.Columns["Version"].HeaderText = frmBase.UIStrings.GetString("Version");
        }

        void populateSwVerDataTable()
        {

            swVerDataTable.Clear();
            foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!ass.GlobalAssemblyCache)
                {
                    AssemblyName assName = ass.GetName();
                    if (!assName.Name.StartsWith("Microsoft"))
                        swVerDataTable.LoadDataRow(new object[] { assName.Name, assName.Version }, true);
                }
            }
            foreach (ProcessModule pm in Process.GetCurrentProcess().Modules)
            {
                if (pm.FileVersionInfo.CompanyName == null || !pm.FileVersionInfo.CompanyName.Contains("Microsoft"))
                {
                    swVerDataTable.LoadDataRow(new object[] { pm.ModuleName, pm.FileVersionInfo.FileVersion == null ? "" : pm.FileVersionInfo.FileVersion }, true);
                }
            }
            swVerGridView.DataSource = swVerDataTable;
            swVerGridView.Sort(swVerGridView.Columns["ModuleName"], ListSortDirection.Ascending);
        }

        void addLogNewRow(LogLevels logLevel, string description)
        {

            const int logLineCount = 500;
            if (logLevel < LogLevels.Debug && imgListLog.Images.Count >= (int)LogLevels.Debug)
            {
                //System.Diagnostics.Debug.Print("A: " + DateTime.Now);
                if (logDataTable.Rows.Count > logLineCount)
                {
                    for (int i = 0; i < logDataTable.Rows.Count - logLineCount; i++)
                    {
                        logDataTable.Rows[logDataTable.Rows.Count - 1 - i].Delete();
                    }
                    logDataTable.AcceptChanges();
                }
                DataRow newRow = logDataTable.NewRow();
                Image icon = null;
                switch (logLevel)
                {
                    case LogLevels.Debug:
                        icon = imgListLog.Images[(int)LogLevels.Debug];
                        break;
                    case LogLevels.Pass:
                        icon = imgListLog.Images[(int)LogLevels.Pass];
                        break;
                    case LogLevels.Warning:
                        icon = imgListLog.Images[(int)LogLevels.Warning];
                        break;
                    case LogLevels.Error:
                        icon = imgListLog.Images[(int)LogLevels.Error];
                        break;
                    default:
                        icon = imgListLog.Images[(int)LogLevels.Debug];
                        break;
                }
                newRow.ItemArray = new object[] { icon, logLevel.ToString(), description };
                logDataTable.Rows.InsertAt(newRow, 0);
                logDataTable.AcceptChanges();
                //System.Diagnostics.Debug.Print("B: " + DateTime.Now);
                RefreshLogGrid();
            }
        }

        void RefreshLogGrid()
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(logGridView))
                return;
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(this))
                return;

            //rivedere: gestire log dopo creazione form
            if (logGridView.IsHandleCreated)
            {
                if (InvokeRequired && IsHandleCreated)
                    Invoke(new MethodInvoker(RefreshLogGrid));
                else
                {
                    logDataTableToShow = logDataTable.Copy();
                    logGridView.DataSource = logDataTableToShow;
                    logGridView.Refresh();
                }
            }
        }


        void pnlImageViewer_VisibleChanged(object sender, System.EventArgs e)
        {
            if (!pnlImageViewer.Visible && imageViewer1 != null)
            {
                imageViewer1.HideAll();
            }
        }

        private void lblTraspBtn_Click(object sender, EventArgs e)
        {

            Label btn = (Label)sender;
            AppEngine.Current.SendSupervisorButtonClicked(btn.Tag.ToString());
        }

        private void cbBypassHMIsendRecipe_CheckedChanged(object sender, EventArgs e)
        {
            machineConfiguration.BypassHMIsendRecipe = cbBypassHMIsendRecipe.Checked;
            AppEngine.Current.SendSupervisorBypassSendRecipe(machineConfiguration.BypassHMIsendRecipe);
            if (cbBypassHMIsendRecipe.Checked)
                cbBypassHMIsendRecipe.ForeColor = Color.Red;
            else
                cbBypassHMIsendRecipe.ForeColor = SystemColors.Control;
        }

        protected override void WndProc(ref Message m)
        {

            const int WM_SYSCOMMAND = 0x0112;
            const int SC_RESTORE = 0xF120;

            if (m.Msg == WM_SYSCOMMAND && (int)m.WParam == SC_RESTORE && IsHandleCreated)
            {
                this.WindowState = FormWindowState.Normal;
                visionSystemMgr.RedrawDisplay("Main");
            }

            base.WndProc(ref m);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED
                return cp;
            }
        }

        private void lblFooter_Click(object sender, EventArgs e)
        {

            AppEngine.Current.SendSupervisorButtonClicked("99");
        }

        //private void pnlMain_SizeChanged(object sender, EventArgs e) {

        //if (visionSystemMgr.Displays != null && visionSystemMgr.Displays.Count > 0 && visionSystemMgr.Displays["Main"] != null && pcbMain != null && pcbMain.IsHandleCreated)
        //    visionSystemMgr.Displays["Main"].Resize(pcbMain.Width, pcbMain.Height);
        //}

        private void btnTabSwVersion_Click(object sender, EventArgs e)
        {

            populateSwVerDataTable();
            recalcUtilities(ToolsEnum.SoftwareVersion);
        }

        // SHORTCUTS
        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {

            if (AppEngine.Current.CurrentContext == null || AppEngine.Current.CurrentContext.UserLevel > UserLevelEnum.Operator)
            {

                if (e.Control && e.KeyCode == Keys.H)
                {
                    Log.Line(LogLevels.Debug, "frmMain.frmMain_KeyDown", "Hide shortcut pressed");
                    setSupervisorHide();
                }
                if (e.Control && e.KeyCode == Keys.S)
                {
                    Log.Line(LogLevels.Debug, "frmMain.frmMain_KeyDown", "Show shortcut pressed");
                    setSupervisorOnTop();
                }
                if (e.Control && e.KeyCode == Keys.D)
                {
                    Log.Line(LogLevels.Debug, "frmMain.frmMain_KeyDown", "Toggle Debug Shell shortcut pressed");
                    //toggle console
                    if (logger1.WriteToConsole)
                    {
                        logger1.WriteToConsole = false;
                        logger1.DestroyConsole();
                    }
                    else
                    {
                        logger1.CreateConsole();
                        logger1.WriteToConsole = true;
                        LogLevels logLevel;
                        if (Enum.TryParse(machineConfiguration.ConsoleLevel.ToString(), out logLevel))
                        {
                            logger1.WriteToConsoleLevel = logLevel;
                        }
                        else
                        {
                            logger1.WriteToConsoleLevel = LogLevels.Debug;
                        }
                    }
                }
                if (e.Control && e.Alt && e.KeyCode == Keys.Q)
                {
                    Log.Line(LogLevels.Debug, "frmMain.frmMain_KeyDown", "Quit shortcut pressed");
                    Destroy(false);
                }
            }

            if (e.Control && e.Alt && e.KeyCode == Keys.I)
            {
                if(AppEngine.Current.CurrentContext.UserLevel == UserLevelEnum.Optrel)
                    btnTabDebugPage.Visible = !btnTabDebugPage.Visible;
                else
                    btnTabDebugPage.Visible = false;
            }
        }



        private void btnExportSwVer_Click(object sender, EventArgs e)
        {

            using (SaveFileDialog saveFileDlg = new SaveFileDialog())
            {
                saveFileDlg.RestoreDirectory = true;
                saveFileDlg.Filter = "CSV File (*.csv)|*.csv";
                if (DialogResult.OK == saveFileDlg.ShowDialog())
                {
                    StringBuilder sb = new StringBuilder();
                    IEnumerable<string> columnNames = swVerDataTable.Columns.Cast<DataColumn>().
                                                      Select(column => column.ColumnName);
                    sb.AppendLine(string.Join(";", columnNames));
                    foreach (DataRow row in swVerDataTable.Rows)
                    {
                        IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                        sb.AppendLine(string.Join(";", fields));
                    }
                    File.WriteAllText(saveFileDlg.FileName, sb.ToString());
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            System.GC.Collect();    // RISOLVE IL PROBLEMA DELLA DERIVA DELLA RAM!
            System.GC.WaitForPendingFinalizers();
        }

        private void btnClearResults_Click(object sender, EventArgs e)
        {
            if (visionSystemMgr.MeasuresContainer["Results"] != null)
                visionSystemMgr.MeasuresContainer["Results"].ResetStatistics();
        }

        private void pcbMain_SizeChanged(object sender, EventArgs e)
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(pcbMain))
                return;

            if (visionSystemMgr.Displays != null && visionSystemMgr.Displays.Count > 0 && visionSystemMgr.Displays["Main"] != null && pcbMain.IsHandleCreated)
                visionSystemMgr.Displays["Main"].Resize(pcbMain.Width, pcbMain.Height);
        }

        internal Panel GetMainPanel()
        {

            return pnlMain;
        }

        private void PrintMexDialog(object sender, EventArgs e)
        {
            switch (((string)sender))
            {
                case null:
                    headerStrip.ErrorText = String.Empty;
                    break;
                case "NoDefaultPrinter":
                    headerStrip.ErrorText = frmBase.UIStrings.GetString("NoDefaultPrinter");
                    break;
                case "Printing":
                    headerStrip.ErrorText = frmBase.UIStrings.GetString("Printing");
                    break;
            }
        }

        private void btnSaveToHMI_Click(object sender, EventArgs e)
        {

        }


        private void btnCaronteEnable_Click(object sender, EventArgs e)
        {
            SetCaronteEnable(true);
        }

        private void btnCaronteDisable_Click(object sender, EventArgs e)
        {
            SetCaronteEnable(false);
        }
    }

    public enum ToolsEnum
    {
        Unknown,
        Utilities,
        Assistants,
        Log,
        SoftwareVersion,
        Results,
        Knapp,
        Dump,
        Trending,
        DebugPage
    }

    internal class DrawingControl
    {
        [DllImport("user32.dll")]
        private extern static IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

        private const int WM_SETREDRAW = 0x000B;
        private const int WM_USER = 0x0400;
        private const int EM_GETEVENTMASK = (WM_USER + 59);
        private const int EM_SETEVENTMASK = (WM_USER + 69);
        static IntPtr eventMask = IntPtr.Zero;

        public static void SuspendDrawing(Control parent)
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(parent))
                return;

            if (parent.InvokeRequired && parent.IsHandleCreated)
            {
                parent.Invoke(new MethodInvoker(() => SuspendDrawing(parent)));
            }
            else
            {
                // Stop redrawing:
                SendMessage(parent.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
                // Stop sending of events:
                eventMask = SendMessage(parent.Handle, EM_GETEVENTMASK, 0, IntPtr.Zero);
            }
        }

        public static void ResumeDrawing(Control parent)
        {
            // Matteo 06-08-2024: check against null-refs and disposed controls.
            if (!UIInvoker.IsControlUiReady(parent))
                return;

            if (parent.InvokeRequired && parent.IsHandleCreated)
            {
                parent.Invoke(new MethodInvoker(() => ResumeDrawing(parent)));
            }
            else
            {
                // turn on events
                SendMessage(parent.Handle, EM_SETEVENTMASK, 0, eventMask);
                // turn on redrawing
                SendMessage(parent.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
                parent.Invalidate();
                parent.Refresh();
            }
        }
    }
}
