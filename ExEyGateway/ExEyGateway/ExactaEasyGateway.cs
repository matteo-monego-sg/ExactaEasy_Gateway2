using InterProcessComm.Messaging;
using OptrelInterProcessComm.Gateways;
using SPAMI.Util.Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;

namespace ExEyGateway
{
    #region Back-compat with ArticOE

    // Kept the old delegates for back-compatibility with ArticOE. Must be placed here.
    public delegate void GetErrorDelegate(int errorCode);
    public delegate void SaveRecipeModificationDelegate(string recipe);
    public delegate void SetSupervisorModeDelegate(int supervisorMode);
    public delegate void SetSupervisorBypassSendRecipe(bool bypass);
    public delegate void SetSupIsAliveDelegate();
    // Matteo: advanced SetSupIsAlive used by new GATEWAY2 communication. It is not used
    // by the old WCF system, but the type is used by the new SupervisorModule in ARTIC, so
    // it needs to be exposed here too to ensure back-compatibility.
    public delegate bool SetSupIsAliveExDelegate();
    public delegate void SupervisorButtonClickDelegate(string button);
    public delegate void SupervisorUIClosedDelegate(bool restartForm);
    public delegate void AuditMessageDelegate(int totalMessageCount, int partialMessageCount, string messageToAudit);
    public delegate void WaitForKnappStartDelegate();
    public delegate void SendResultsDelegate(string results);

    #endregion

    /// <summary>
    /// 
    /// </summary>
    public class ExEyGatewayCtrl : UserControl, IArticIpcLogic
    {
        /// <summary>
        /// 
        /// </summary>
        private IPCGatewayServer<IArticIpcLogic> _ipcGateway;
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, string> errorsDescription = new Dictionary<int, string>();
        /// <summary>
        /// 
        /// </summary>
        private object auditMsgLock = new object();
        /// <summary>
        /// 
        /// </summary>
        private int lastAuditMessageReturnCode = 0;
        /// <summary>
        /// 
        /// </summary>
        private string logFilePath;
        /// <summary>
        /// 
        /// </summary>
        private int logFilePostfix = 0;
        /// <summary>
        /// 
        /// </summary>
        private int logFileSizeCheckCount = 0;
        /// <summary>
        /// 
        /// </summary>
        private const int MaxLogSize = 20 * 1024 * 1024;
        /// <summary>
        /// 
        /// </summary>
        private StreamWriter logFileStream;
        /// <summary>
        /// 
        /// </summary>
        private readonly object _logLock = new object();
        /// <summary>
        /// 
        /// </summary>
        private Label lblVersion;
        /// <summary>
        /// 
        /// </summary>
        private static object _startStopLock = new object();
        /// <summary>
        /// 
        /// </summary>
        public bool SupervisorUILoaded { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string winccRecipe { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string winccAudit { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public bool ExternalProcess { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool BypassSendRecipe { get; set; }

        #region Events

        // Kept the old events for back-compatibility with ArticOE.
        public event GetErrorDelegate GetError;
        public event SaveRecipeModificationDelegate SaveRecipeModification;
        public event SetSupervisorModeDelegate SetSupervisorMode;
        public event SetSupervisorBypassSendRecipe SetSupervisorBypassSendRecipe;
        // Matteo: advanced SetSupIsAlive used by new GATEWAY2 communication. It is not used
        // by the old WCF system, but the type is used by the new SupervisorModule in ARTIC, so
        // it needs to be exposed here too to ensure back-compatibility.
        public event SetSupIsAliveDelegate SetSupIsAlive;
        // Used by the new GATEWAY2 system.
        public event SetSupIsAliveExDelegate SetSupIsAliveEx;
        public event SupervisorButtonClickDelegate SupervisorButtonClick;
        public event SupervisorUIClosedDelegate SupervisorUIClosed;
        public event AuditMessageDelegate AuditMessage;
        public event WaitForKnappStartDelegate WaitForKnappStart;
        public event SendResultsDelegate SendResults;
        // Matteo: SupervisorConnected/SupervisorDisconnected events used by new GATEWAY2 communication.
        // They are not used by the old WCF system, but the events are
        // used by the new SupervisorModule in ARTIC, so they need to be exposed here too to ensure back-compatibility.
        public event EventHandler SupervisorConnected;
        public event EventHandler SupervisorDisconnected;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public ExEyGatewayCtrl() {

            InitializeComponent();

            Version currVer = Assembly.GetExecutingAssembly().GetName().Version;
            if (currVer != null && lblVersion != null)
                lblVersion.Text = "Version: " + currVer.ToString();
            createLog();
        }
        /// <summary>
        /// 
        /// </summary>
        private void InitializeComponent()
        {
            lblVersion = new Label();
            SuspendLayout();
            // 
            // lblVersion
            // 
            lblVersion.Anchor = AnchorStyles.Left;
            lblVersion.AutoSize = true;
            lblVersion.Font = new System.Drawing.Font("Nirmala UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblVersion.Location = new System.Drawing.Point(37, 63);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new System.Drawing.Size(77, 25);
            lblVersion.TabIndex = 1;
            lblVersion.Text = "version";
            // 
            // ExEyGatewayCtrl
            // 
            Controls.Add(this.lblVersion);
            Name = "ExEyGatewayCtrl";
            ResumeLayout(false);
            PerformLayout();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void log(string message)
        {
            lock (_logLock)
            {
                createLog();
                if (logFileStream != null)
                {
                    DateTime lineTime = DateTime.Now;
                    string line = lineTime.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + ": " + message;
                    logFileStream.WriteLine(line);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void createLog()
        {
            try
            {
                logFilePath = Environment.CurrentDirectory + @"\ExEyGatewayLog_" + logFilePostfix + ".txt";
                if ((logFileStream == null) || (logFileSizeCheckCount == 0))
                {
                    FileInfo fi = new FileInfo(logFilePath);
                    if (File.Exists(logFilePath) == true && fi.Length > MaxLogSize)
                    {
                        if (logFileStream != null)
                            logFileStream.Close();
                        logFilePostfix = (logFilePostfix + 1) % 2;
                        logFilePath = Environment.CurrentDirectory + @"\ExEyGatewayLog_" + logFilePostfix + ".txt";
                        fi = new FileInfo(logFilePath);
                        if (File.Exists(logFilePath) == true && fi.Length > MaxLogSize)
                        {
                            File.Delete(logFilePath);
                        }
                    }
                    if (logFileStream == null || logFileStream.BaseStream == null)
                    {
                        logFileStream = new StreamWriter(logFilePath, true);
                        if (logFileStream != null)
                            logFileStream.AutoFlush = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("ExEyGateway.createLog: An error occurred while creating log file. Error: " + ex.Message);
            }
            finally
            {
                logFileSizeCheckCount = (logFileSizeCheckCount + 1) % 10;
            }
        }
        /// <summary>
        /// Returns true if the gateway is currently connected to a client,
        /// otherwise false.
        /// </summary>
        public bool IsConnected
        {
            get 
            {
                if (_ipcGateway is null) return false;
                return _ipcGateway.IsConnected;
            }
        }
        /// <summary>
        /// Starts the supervisor communication.
        /// </summary>
        public void StartSupervisor() 
        {
            lock (_startStopLock)
            {
                if (!ExternalProcess)
                    throw new NotImplementedException("This version run only as external process");
                else
                {
                    try
                    {
                        // Run all the necessary windows services and wait for them to be up.
                        WaitWindowsServices();
                        InitializeGateway();
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine($"ExEyGateway.StartSupervisor: An error occurred while initializing the IPC gateway. Exception: {ex.Message}");
                        //return false;
                    }
                }
            }
        }
        /// <summary>
        /// Stops the supervisor communication.
        /// </summary>
        public void StopSupervisor()
        {
            lock (_startStopLock)
            {
                if (!(_ipcGateway is null))
                    _ipcGateway.Stop();
            }
        }
        /// <summary>
        /// Creates a new gateway (IpcGatewayServerEx) to communicate with ExactaEasy clients.
        /// </summary>
        private bool InitializeGateway()
        {
            try
            {
                // Creates a new IPC gateway, passing this instance as logic container.
                _ipcGateway = new IPCGatewayServer<IArticIpcLogic>("artic_gateway2_server", this);
                _ipcGateway.OnClientConnected += IpcGateway_ExactaEasyConnected;
                _ipcGateway.OnClientDisconnected += IpcGateway_ExactaEasyDisconnected;
                // Asynchronously listens for a new ExactaEasy connection.
                return _ipcGateway.Start();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("ExEyGateway.InitializeGateway: An error occurred while initializing the IPC gateway. Error: " + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// Raised when an ExactaEasy client disconnects.
        /// </summary>
        private void IpcGateway_ExactaEasyDisconnected(object sender, EventArgs e)
        {
            // Raise an ASYNCHRONOUS disconnected event to the client class.
            SupervisorDisconnected?.BeginInvoke(this, EventArgs.Empty, null, null);
        }
        /// <summary>
        /// Raised when an ExactaEasy client connects.
        /// </summary>
        private void IpcGateway_ExactaEasyConnected(object sender, EventArgs e)
        {
            // Raise an ASYNCHRONOUS connected event.
            SupervisorConnected?.BeginInvoke(this, EventArgs.Empty, null, null);
        }
        /// <summary>
        /// Executes a check of the required windows services.
        /// </summary>
        private void WaitWindowsServices()
        {
            //WINDOWS SERVICES
            ServiceController[] scList = ServiceController.GetServices();

            List<string> scNames = new List<string>();
            foreach (ServiceController sc in scList)
                scNames.Add(sc.ServiceName);
            List<string> serviceNames = new List<string>();
            //if (scNames.Contains("Netman"))     //parametrizzare??
            //    serviceNames.Add("Netman");     //parametrizzare??
            //if (scNames.Contains("RasMan"))     //parametrizzare??
            //    serviceNames.Add("RasMan");     //parametrizzare??
            if (scNames.Contains("Nla"))     //parametrizzare??
                serviceNames.Add("Nla");     //parametrizzare??
            if (scNames.Contains("NlaSvc"))     //parametrizzare??
                serviceNames.Add("NlaSvc");     //parametrizzare??
            List<ServiceController> serviceControllers = new List<ServiceController>();
            foreach (string scName in serviceNames)
                serviceControllers.Add(new ServiceController(scName));
            bool ok = false;
            while (!ok)
            {
                ok = true;
                foreach (ServiceController sc in serviceControllers)
                {
                    sc.Refresh();
                    if (sc.Status != ServiceControllerStatus.Running)
                    {
                        ok = false;
                    }
                }
                Thread.Sleep(200);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void ShowSupervisorUI()
        {
            if (!SupervisorUILoaded)
                SupervisorUILoaded = true;
        }
        /// <summary>
        /// 
        /// </summary>
        public void SetErrorDescription(int errorCode, string errorDescription)
        {
            if (errorsDescription.ContainsKey(errorCode))
                errorsDescription[errorCode] = errorDescription;
            else
                errorsDescription.Add(errorCode, errorDescription);
        }
        /// <summary>
        /// 
        /// </summary>
        public void SetLastAuditMessageResult(int errorCode)
        {
            lastAuditMessageReturnCode = errorCode;
        }

        #region ExactaEasy Commands
       /*
       * NOTE: the following commands are called by ArticOE.
       * To maintain the backward compatibility with the actual ArticOE, all the 
       * functions are changed to return an integer value instead of a bool.
       */
        /// <summary>
        /// Sets the supervisor above all other windows.
        /// </summary>
        public int SetSupervisorOnTop()
        {
            if (_ipcGateway is null) return 1;
            _ipcGateway.PushRequest(
                new IPCRequest("SetSupervisorOnTop"));
            return 0;
        }
        /// <summary>
        /// Hides the supervisor.
        /// </summary>
        public int SetSupervisorHide()
        {
            if (_ipcGateway is null) return 1;
            _ipcGateway.PushRequest(
                new IPCRequest("SetSupervisorHide"));
            return 0;
        }
        /// <summary>
        /// Old version of SetActiveRecipe.
        /// </summary>
        public int SetActiveRecipe(string recipeName, string recipeXml)
        {
            if (_ipcGateway is null) return 1;
            // Sends a request to the supervisor to load the recipe file.
            var respMsg = _ipcGateway.PushRequestWithResponse(
            new IPCRequest("SetActiveRecipe",
                new IPCParameter("recipeName", recipeName),
                new IPCParameter("recipeXml", recipeXml)));

            switch (respMsg.Response.Status)
            {
                case IPCResponseStatusEnum.OK:
                    return respMsg.Response.ReturnValueAs<int>();
                default:
                    return 1;
            }
        }
        /// <summary>
        /// Sets the current active recipe version.
        /// </summary>
        /// <param name="recipeVersion">Recipe version.</param>
        public int SetActiveRecipeVersion(int recipeVersion)
        {
            if (_ipcGateway is null) return 1;

            var respMsg = _ipcGateway.PushRequestWithResponse(
               new IPCRequest("SetActiveRecipeVersion",
                   new IPCParameter("recipeVersion", recipeVersion)));

            switch (respMsg.Response.Status)
            {
                case IPCResponseStatusEnum.OK:
                    return respMsg.Response.ReturnValueAs<int>();
                default:
                    return 1;
            }
        }
        /// <summary>
        /// Sets the language of the supervisor.
        /// </summary>
        public int SetLanguage(string languageCode) 
        {
            if (_ipcGateway is null) 
                return 1;

            var respMsg = _ipcGateway.PushRequestWithResponse(
              new IPCRequest("SetLanguage",
                  new IPCParameter("languageCode", languageCode)));

            switch (respMsg.Response.Status)
            {
                case IPCResponseStatusEnum.OK:
                    return respMsg.Response.ReturnValueAs<int>();
                default:
                    return 1;
            }
        }
        /// <summary>
        /// Sets the position and area of the supervisor.
        /// </summary>
        /// <param name="startX">Horizontal position of the top left corner of the window.</param>
        /// <param name="startY">Vertical position of the top left cornet of the window.</param>
        /// <param name="sizeX">Width of the window.</param>
        /// <param name="sizeY">Height of the window.</param>
        public int SetSupervisorPos(int startX, int startY, int sizeX, int sizeY) 
        {
            if (_ipcGateway is null) 
                return 1;

            var respMsg = _ipcGateway.PushRequestWithResponse(
              new IPCRequest("SetSupervisorPos",
                  new IPCParameter("startX", startX),
                  new IPCParameter("startY", startY),
                  new IPCParameter("sizeX", sizeX),
                  new IPCParameter("sizeY", sizeY)));

            switch (respMsg.Response.Status)
            {
                case IPCResponseStatusEnum.OK:
                    return respMsg.Response.ReturnValueAs<int>();
                default:
                    return 1;
            }
        }
        /// <summary>
        /// Sets the user level of the supervisor.
        /// </summary>
        /// <param name="userLevel">User level.</param>
        public int SetUserLevel(int userLevel)
        {
            if (_ipcGateway is null) 
                return 1;

            var respMsg = _ipcGateway.PushRequestWithResponse(
              new IPCRequest("SetUserLevel",
                  new IPCParameter("userLevel", userLevel)));

            switch (respMsg.Response.Status)
            {
                case IPCResponseStatusEnum.OK:
                    return respMsg.Response.ReturnValueAs<int>();
                default:
                    return 1;
            }
        }
        /// <summary>
        /// Sets the database used by the supervisor.
        /// </summary>
        public int SetDataBase(int dataBase) 
        {
            if (_ipcGateway is null) 
                return 1;

            var respMsg = _ipcGateway.PushRequestWithResponse(
              new IPCRequest("SetDataBase",
                  new IPCParameter("dataBase", dataBase)));

            switch (respMsg.Response.Status)
            {
                case IPCResponseStatusEnum.OK:
                    return respMsg.Response.ReturnValueAs<int>();
                default:
                    return 1;
            }
        }
        /// <summary>
        /// Sets the machine mode of the supervisor.
        /// </summary>
        /// <param name="mode">Machine mode to be set.</param>
        public int SetMachineMode(int mode) 
        {
            if (_ipcGateway is null) 
                return 1;

            var respMsg = _ipcGateway.PushRequestWithResponse(
             new IPCRequest("SetMachineMode",
                 new IPCParameter("mode", mode)));

            switch (respMsg.Response.Status)
            {
                case IPCResponseStatusEnum.OK:
                    return respMsg.Response.ReturnValueAs<int>();
                default:
                    return 1;
            }
        }
        /// <summary>
        /// Warns the supervisor that the HMI is alive.
        /// </summary>
        public int SetHMIIsAlive(int mode) 
        {
            if (_ipcGateway is null) 
                return 1;

            var respMsg = _ipcGateway.PushRequestWithResponse(
                new IPCRequest("SetHMIIsAlive",
                    new IPCParameter("mode", mode)));

            switch (respMsg.Response.Status)
            {
                case IPCResponseStatusEnum.OK:
                    return respMsg.Response.ReturnValueAs<int>();
                default:
                    return 1;
            }
        }
        /// <summary>
        /// Obtains the error code description.
        /// </summary>
        public string GetErrorString(int errorCode) 
        {
            if (_ipcGateway is null) 
                return string.Empty;

            var respMsg = _ipcGateway.PushRequestWithResponse(
                new IPCRequest("GetErrorString",
                    new IPCParameter("errorCode", errorCode)));

            switch (respMsg.Response.Status)
            {
                case IPCResponseStatusEnum.OK:
                    return respMsg.Response.ReturnValueAs<string>();
                default:
                    return string.Empty;
            }
        }
        /// <summary>
        /// Communicates the supervisor the work parameters.
        /// </summary>
        /// <param name="stationInfo">Informations about all the stations of the machine.</param>
        /// <param name="speed">Machine working speed.</param>
        public int SetSupervisorMachineInfo(string stationInfo, string speed) 
        {
            if (_ipcGateway is null) 
                return 1;

            var respMsg = _ipcGateway.PushRequestWithResponse(
                new IPCRequest("SetSupervisorMachineInfo",
                    new IPCParameter("stationInfo", stationInfo),
                    new IPCParameter("speed", speed)));

            switch (respMsg.Response.Status)
            {
                case IPCResponseStatusEnum.OK:
                    return respMsg.Response.ReturnValueAs<int>();
                default:
                    return 1;
            }
        }
        /// <summary>
        /// ?
        /// </summary>
        public int ResetErrorRQS(string errorSource, string errorCode)
        {
            if (_ipcGateway is null) 
                return 1;

            var respMsg = _ipcGateway.PushRequestWithResponse(
               new IPCRequest("ResetErrorRQS",
                   new IPCParameter("errorSource", errorSource),
                   new IPCParameter("errorCode", errorCode)));

            switch (respMsg.Response.Status)
            {
                case IPCResponseStatusEnum.OK:
                    return respMsg.Response.ReturnValueAs<int>();
                default:
                    return 1;
            }
        }
        /// <summary>
        /// Tells the supervisor to print the active recipe.
        /// </summary>
        public int PrintActiveRecipe() 
        {
            if (_ipcGateway is null) 
                return 1;

            var respMsg = _ipcGateway.PushRequestWithResponse(
               new IPCRequest("PrintActiveRecipe"));

            switch (respMsg.Response.Status)
            {
                case IPCResponseStatusEnum.OK:
                    return respMsg.Response.ReturnValueAs<int>();
                default:
                    return 1;
            }
        }
        /// <summary>
        /// Starts the current batch.
        /// </summary>
        public int StartBatch(int saveType) 
        {
            if (_ipcGateway is null) 
                return 1;

            var respMsg = _ipcGateway.PushRequestWithResponse(
               new IPCRequest("StartBatch", 
                    new IPCParameter("saveType", saveType)));

            switch (respMsg.Response.Status)
            {
                case IPCResponseStatusEnum.OK:
                    return respMsg.Response.ReturnValueAs<int>();
                default:
                    return 1;
            }
        }
        /// <summary>
        /// Set the current working batch.
        /// </summary>>
        public int SetBatchId(int id)
        {
            if (_ipcGateway is null) 
                return 1;

            _ipcGateway.PushRequest(
               new IPCRequest("SetBatchId",
                    new IPCParameter("id", id)));
            return 0;
        }
        /// <summary>
        /// Stops the current batch.
        /// </summary>
        public int StopBatch() 
        {
            if (_ipcGateway is null) 
                return 1;

            var respMsg = _ipcGateway.PushRequestWithResponse(
               new IPCRequest("StopBatch"));

            switch (respMsg.Response.Status)
            {
                case IPCResponseStatusEnum.OK:
                    return respMsg.Response.ReturnValueAs<int>();
                default:
                    return 1;
            }
        }
        /// <summary>
        /// Start a KNAPP session.
        /// </summary>
        public int StartKnapp(int saveType, int currentTurn, int totalTurnsCount, int warmupTurnsCount) 
        {
            if (_ipcGateway is null) 
                return 1;

            var respMsg = _ipcGateway.PushRequestWithResponse(
               new IPCRequest("StartKnapp",
                    new IPCParameter("saveType", saveType),
                    new IPCParameter("currentTurn", currentTurn),
                    new IPCParameter("totalTurnsCount", totalTurnsCount),
                    new IPCParameter("warmupTurnsCount", warmupTurnsCount)));

            switch (respMsg.Response.Status)
            {
                case IPCResponseStatusEnum.OK:
                    return respMsg.Response.ReturnValueAs<int>();
                default:
                    return 1;
            }
        }
        /// <summary>
        /// Sets the working mode of the supervisor.
        /// </summary>
        public int SetWorkingMode(int workingMode)
        {
            if (_ipcGateway is null)
                return 1;

            var respMsg = _ipcGateway.PushRequestWithResponse(
               new IPCRequest("SetWorkingMode",
                    new IPCParameter("workingMode", workingMode)));

            switch (respMsg.Response.Status)
            {
                case IPCResponseStatusEnum.OK:
                    return respMsg.Response.ReturnValueAs<int>();
                default:
                    return 1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SetCSVRotationParameters(string csv)
        {
            if (_ipcGateway is null)
                return 1;

            var respMsg = _ipcGateway.PushRequestWithResponse(
               new IPCRequest("SetCSVRotationParameters",
                    new IPCParameter("csv", csv)));

            switch (respMsg.Response.Status)
            {
                case IPCResponseStatusEnum.OK:
                    return respMsg.Response.ReturnValueAs<int>();
                default:
                    return 1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SetActiveRecipeStatus(string recipeStatus)
        {
            if (_ipcGateway is null)
                return 1;

            if (!Enum.TryParse(recipeStatus, true, out RecipeStatusEnum recipeStatusEnum))
                return 1;

            var respMsg = _ipcGateway.PushRequestWithResponse(
               new IPCRequest("SetActiveRecipeStatus",
                    new IPCParameter("recipeStatus", recipeStatusEnum)));

            switch (respMsg.Response.Status)
            {
                case IPCResponseStatusEnum.OK:
                    return respMsg.Response.ReturnValueAs<int>();
                default:
                    return 1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SetActiveRecipeAllParams(string recipeName, string recipeXml, int recipeVersion, string recipeStatus)
        {
            if (_ipcGateway is null)
            {
                Log.Line(
                    LogLevels.Warning,
                    "SetActiveRecipeAllParams", $"Inter-process gateway object reference is null.");
                return 1;
            }

            if (!Enum.TryParse(recipeStatus, true, out RecipeStatusEnum recipeStatusEnum))
            {
                Log.Line(
                    LogLevels.Warning,
                    "SetActiveRecipeAllParams", $"Could not convert '{recipeStatus}' to a RecipeStatusEnum member: setting recipeStatusEnum=UNKNOWN.");
                recipeStatusEnum = RecipeStatusEnum.Unknown;
            }

            var respMsg = _ipcGateway.PushRequestWithResponse(
               new IPCRequest("SetActiveRecipeAllParams",
                    new IPCParameter("recipeName", recipeName),
                    new IPCParameter("recipeXml", recipeXml),
                    new IPCParameter("recipeVersion", recipeVersion),
                    new IPCParameter("recipeStatus", recipeStatusEnum)));

            switch (respMsg.Response.Status)
            {
                case IPCResponseStatusEnum.OK:
                    return respMsg.Response.ReturnValueAs<int>();
                default:
                    return 1;
            }
        }
        #endregion

        #region ArticOE functions remotely called via IPC by ExactaEasy
        /// <summary>
        /// 
        /// </summary>
        public void HmiAuditMessage(int totalMessageCount, int partialMessageCount, string messageToAudit)
        {
            int ris = 0;
            lock (auditMsgLock)
            {
                log($"HmiAuditMessage - Message to audit length: {messageToAudit.Length}");
                if (!(AuditMessage is null))
                {
                    winccAudit = messageToAudit;
                    //var e = new AuditMessageEventArgs(totalMessageCount, partialMessageCount, messageToAudit);
                    //AuditMessage.Invoke(this, e);
                    AuditMessage.Invoke(totalMessageCount, partialMessageCount, messageToAudit);

                    log("HmiAuditMessage - Delivered");
                    ris = lastAuditMessageReturnCode;
                }
            }
        }
        /// <summary>
        /// Never used.
        /// </summary>
        public void HmiSupervisorUIClosed(bool restart)
        {
            log($"HmiSupervisorUIClosed - Restart: {restart}");
            if (!(SupervisorUIClosed is null))
            {
                //var e = new SupervisorUIClosedEventArgs(restart);
                //SupervisorUIClosed.Invoke(this, e);
                SupervisorUIClosed.Invoke(restart);
                log("HmiSupervisorUIClosed - Delivered");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HmiGetErrorString(int errorCode)
        {
            //var e = new GetErrorEventArgs(errorCode);
            //GetError?.Invoke(this, e);
            GetError?.Invoke(errorCode);
            var ris = string.Empty;
            if (errorsDescription.ContainsKey(errorCode))
                ris = errorsDescription[errorCode];
            return ris;
        }
        /// <summary>
        /// 
        /// </summary>
        public void HmiGetSupervisorPos()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        public void HmiSaveRecipeModification(string recipe)
        {
            log($"HmiSaveRecipeModification - Recipe length: {recipe.Length}");

            if (!(SaveRecipeModification is null))
            {
                //winccRecipe = recipe;
                //var e = new SaveRecipeModificationEventArgs(recipe);
                //SaveRecipeModification.Invoke(this, e);
                SaveRecipeModification.Invoke(recipe);
                log("HmiSaveRecipeModification - Delivered");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public void HmiSendResults(string results)
        {
            log($"HmiSendResults - Results length: {results.Length}");
            if (!(SendResults is null))
            {
                //var e = new SendResultsEventArgs(results);
                //SendResults.Invoke(this,e);
                SendResults.Invoke(results);
                log("HmiSendResults - Delivered");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void HmiSetOnTop()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        public void HmiSetSupervisorMode(int hmiMode)
        {
            log($"HmiSetSupervisorMode - Supervisor mode: {hmiMode}");
            Status = hmiMode;

            if (!(SetSupervisorMode is null))
            {
                SetSupervisorMode.Invoke(hmiMode);
                log("HmiSetSupervisorMode - Delivered");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void HmiSetSupIsAlive()
        {
            log("HmiSetSupIsAlive");
            if (!(SetSupIsAlive is null))
            {
                SetSupIsAlive.Invoke();
                log("HmiSetSupIsAlive - Delivered");
            }
        }
        /// <summary>
        /// Same as HmiSetSupIsAlive, but returns a value with the
        /// execution result.
        /// </summary>
        public bool HmiSetSupIsAliveEx()
        {
            log("HmiSetSupIsAliveEx");
            if (!(SetSupIsAliveEx is null))
            {
                return SetSupIsAliveEx.Invoke();
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        public void HmiSetSupervisorBypassSendRecipe(bool bypass)
        {
            log("OnSetSupervisorBypassSendRecipe - Bypass: " + bypass);
            BypassSendRecipe = bypass;

            if (!(SetSupervisorBypassSendRecipe is null))
            {
                SetSupervisorBypassSendRecipe.Invoke(bypass);
                log("OnSetSupervisorBypassSendRecipe - Delivered");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void HmiSupervisorButtonClicked(string buttonID)
        {
            log($"HmiSupervisorButtonClicked - Button ID: {buttonID}");
            if (!(SupervisorButtonClick is null))
            {
                //var e = new SupervisorButtonClickedEventArgs(buttonID);
                //SupervisorButtonClick.Invoke(this, e);
                SupervisorButtonClick.Invoke(buttonID);
                log("HmiSupervisorButtonClicked - Delivered");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void HmiWaitForKnappStart()
        {
            log("HmiWaitForKnappStart");
            if (!(WaitForKnappStart is null))
            {
                //var e = new WaitForKnappStartEventArgs();
                //WaitForKnappStart.Invoke(this, e);
                WaitForKnappStart.Invoke();

                log("HmiWaitForKnappStart - Delivered");
            }
        }
        #endregion
    }
}
