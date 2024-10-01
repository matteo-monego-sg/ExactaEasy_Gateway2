using ExactaEasyCore;
using ExactaEasyCore.TrendingTool;
using ExactaEasyEng.AppDebug;
using ExEyGateway;
using InterProcessComm.Messaging;
using OptrelInterProcessComm.Gateways;
using OptrelInterProcessComm.Utils;
using SPAMI.Util.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExactaEasyEng
{
    /// <summary>
    /// ExactaEasy Engine singleton class.
    /// </summary>
    public class AppEngine : IExactaEasyIpcEvents
    {
        /// <summary>
        /// 
        /// </summary>
        private const int DEFAULT_IPC_STATUS_THREAD_LOOP_TIME_MS = 50;
        /// <summary>
        /// Timeout for the call to HmiSetSupIsAliveEx
        /// </summary>
        private const int IPC_SETSUPISALIVEEX_TIMEOUT_MS = 15000;
        /// <summary>
        /// Interprocess communication connection timeout.
        /// </summary>
        private const int IPC_CONNECTION_RETRY_TIMEOUT_MS = 5000;
        /// <summary>
        /// Machine configuration XML file name. 
        /// </summary>
        private const string MACHINE_CONFIG_XML_FILE = "machineConfig.xml";
        /// <summary>
        /// Dump images configuration XML file name.
        /// </summary>
        private const string DUMP_IMAGES_COMFIG_XML_FILE = "dumpImagesConfig.xml";

        #region IExactaEasyIpcEvents
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ContextChangedEventArgs> ContextChanged;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ExactaEasyOperationInvokedEventArgs> SetExactaEasyOnTopRequested;
        /// <summary>
        /// 
        /// </summary>
        internal void OnSetExactaEasyOnTopRequested(ExactaEasyOperationInvokedEventArgs e)
        {
            SetExactaEasyOnTopRequested?.Invoke(this, e);
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ExactaEasyOperationInvokedEventArgs> HideExactaEasyRequested;
        /// <summary>
        /// 
        /// </summary>
        internal void OnHideExactaEasyRequested(ExactaEasyOperationInvokedEventArgs e)
        {
            HideExactaEasyRequested?.Invoke(this, e);
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<SetExactaEasyActiveRecipeEventArgs> SetExactaEasyActiveRecipeRequested;
        /// <summary>
        /// 
        /// </summary>
        internal void OnSetExactaEasyActiveRecipeRequested(SetExactaEasyActiveRecipeEventArgs e)
        {
            if (e.ActiveRecipe is null)
            {
                // If a path to the recipe exists, copy the recipe with errors.
                StreamWriter w = new StreamWriter("temp_err_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xml");
                w.WriteLine(e.RecipeXml);
                w.Flush();
                w.Close();
                OnAppEngineContextChangeError(new AppEngineContextChangedEventArgs(ContextChangesEnum.ActiveRecipe, false));
                return;
            }

            AppContext currCtx = CurrentContext;
            if (MachineConfiguration.BypassHMIsendRecipe)
                Log.Line(LogLevels.Warning, "AppEngine.OnSetExactaEasyActiveRecipeRequested", "Bypass apply parameters from HMI is ENABLED. The recipe will become active without being applied.");
            SetCurrentContext(new AppContext(
                currCtx.CultureCode,
                currCtx.AppClientRect,
                currCtx.UserLevel,
                e.ActiveRecipe,
                -1,
                RecipeStatusEnum.Unknown,
                currCtx.MachineMode,
                currCtx.EnabledStation,
                currCtx.MachineSpeed,
                currCtx.SupervisorMode,
                currCtx.SupervisorWorkingMode,
                currCtx.DataBase,
                currCtx.IsBatchStarted,
                currCtx.CurrentBatchId,
                currCtx.CSVRotationParameters), ContextChangesEnum.ActiveRecipe, !MachineConfiguration.BypassHMIsendRecipe);
            // Backups the last active recipe (what for?).
            // This can be done in a different thread.
            Task.Run(() => { _lastSavedRecipe = e.ActiveRecipe.Clone(ParametersInfo, currCtx.CultureCode);  });
            // Fires the client class event.
            SetExactaEasyActiveRecipeRequested?.Invoke(this, e);
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<SetExactaEasyActiveRecipeAllParamsEventArgs> SetExactaEasyActiveRecipeAllParamsRequested;
        /// <summary>
        /// 
        /// </summary>
        internal void OnSetExactaEasyActiveRecipeAllParamsRequested(SetExactaEasyActiveRecipeAllParamsEventArgs e)
        {
            if (!(e.ActiveRecipe is null))
            {
                AppContext currCtx = CurrentContext;
                if (MachineConfiguration.BypassHMIsendRecipe)
                    Log.Line(LogLevels.Warning, "AppEngine.OnSetExactaEasyActiveRecipeRequested", "Bypass apply parameters from HMI is ENABLED. The recipe will become active without being applied.");
                SetCurrentContext(new AppContext(
                    currCtx.CultureCode,
                    currCtx.AppClientRect,
                    currCtx.UserLevel,
                    e.ActiveRecipe,
                    e.RecipeVersion,
                    e.ActiveRecipeStatus,
                    currCtx.MachineMode,
                    currCtx.EnabledStation,
                    currCtx.MachineSpeed,
                    currCtx.SupervisorMode,
                    currCtx.SupervisorWorkingMode,
                    currCtx.DataBase,
                    currCtx.IsBatchStarted,
                    currCtx.CurrentBatchId, 
                    currCtx.CSVRotationParameters), ContextChangesEnum.ActiveRecipe, !MachineConfiguration.BypassHMIsendRecipe);
                //IsContextInitialized = CurrentContext.IsContextComplete();
                // Backups the last active recipe (what for?).
                Task.Run(() => { _lastSavedRecipe = e.ActiveRecipe.Clone(ParametersInfo, currCtx.CultureCode); });
                // Firest the client class event.
                SetExactaEasyActiveRecipeAllParamsRequested?.Invoke(this, e);
            }
            else
            {
                var filename = $"temp_err_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xml";
                File.WriteAllText(filename, e.RecipeXml);
                OnAppEngineContextChangeError(new AppEngineContextChangedEventArgs(ContextChangesEnum.ActiveRecipe, false));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<SetExactaEasyActiveRecipeVersionEventArgs> SetExactaEasyActiveRecipeVersionRequested;
        /// <summary>
        /// 
        /// </summary>
        internal void OnSetExactaEasyActiveRecipeVersionRequested(SetExactaEasyActiveRecipeVersionEventArgs e)
        {
            AppContext currCtx = CurrentContext;
            SetCurrentContext(new AppContext(
                currCtx.CultureCode, 
                currCtx.AppClientRect, 
                currCtx.UserLevel, 
                currCtx.ActiveRecipe, 
                e.ActiveRecipeVersion,
                currCtx.ActiveRecipeStatus,
                currCtx.MachineMode, 
                currCtx.EnabledStation,
                currCtx.MachineSpeed,
                currCtx.SupervisorMode, 
                currCtx.SupervisorWorkingMode,
                currCtx.DataBase, 
                currCtx.IsBatchStarted, 
                currCtx.CurrentBatchId,
                currCtx.CSVRotationParameters), ContextChangesEnum.ActiveRecipeVersion, false);
            //IsContextInitialized = CurrentContext.IsContextComplete();

            SetExactaEasyActiveRecipeVersionRequested?.Invoke(this, e);
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<SetExactaEasyActiveRecipeStatusEventArgs> SetExactaEasyActiveRecipeStatusRequested;
        /// <summary>
        /// 
        /// </summary>
        internal void OnSetExactaEasyActiveRecipeStatusRequested(SetExactaEasyActiveRecipeStatusEventArgs e)
        {
            AppContext currCtx = CurrentContext;
            SetCurrentContext(new AppContext(
                currCtx.CultureCode,
                currCtx.AppClientRect,
                currCtx.UserLevel,
                currCtx.ActiveRecipe,
                currCtx.ActiveRecipeVersion,
                e.ActiveRecipeStatus,
                currCtx.MachineMode,
                currCtx.EnabledStation,
                currCtx.MachineSpeed,
                currCtx.SupervisorMode,
                currCtx.SupervisorWorkingMode,
                currCtx.DataBase,
                currCtx.IsBatchStarted,
                currCtx.CurrentBatchId, 
                currCtx.CSVRotationParameters), ContextChangesEnum.ActiveRecipeVersion, false);
            //IsContextInitialized = CurrentContext.IsContextComplete();

            SetExactaEasyActiveRecipeStatusRequested?.Invoke(this, e);
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<SetExactaEasyLanguageCodeEventArgs> SetExactaEasyLanguageCodeRequested;
        /// <summary>
        /// 
        /// </summary>
        internal void OnSetExactaEasyLanguageCodeRequested(SetExactaEasyLanguageCodeEventArgs e)
        {
            AppContext currCtx = CurrentContext;
            SetCurrentContext(new AppContext(
                e.LanguageCode, 
                currCtx.AppClientRect, 
                currCtx.UserLevel, 
                currCtx.ActiveRecipe, 
                currCtx.ActiveRecipeVersion,
                currCtx.ActiveRecipeStatus,
                currCtx.MachineMode, 
                currCtx.EnabledStation, 
                currCtx.MachineSpeed, 
                currCtx.SupervisorMode,
                currCtx.SupervisorWorkingMode, 
                currCtx.DataBase, 
                currCtx.IsBatchStarted, 
                currCtx.CurrentBatchId, 
                currCtx.CSVRotationParameters), ContextChangesEnum.Language, false);
            //IsContextInitialized = CurrentContext.IsContextComplete();
            SetExactaEasyLanguageCodeRequested?.Invoke(this, e);
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<SetExactaEasyMachineModeEventArgs> SetExactaEasyMachineModeRequested;
        /// <summary>
        /// 
        /// </summary>
        internal void OnSetExactaEasyMachineModeRequested(SetExactaEasyMachineModeEventArgs e)
        {
            AppContext currCtx = CurrentContext;
            SetCurrentContext(new AppContext(
                currCtx.CultureCode, 
                currCtx.AppClientRect, 
                currCtx.UserLevel, 
                currCtx.ActiveRecipe,
                currCtx.ActiveRecipeVersion,
                currCtx.ActiveRecipeStatus,
                e.MachineMode, 
                currCtx.EnabledStation, 
                currCtx.MachineSpeed, 
                currCtx.SupervisorMode,
                currCtx.SupervisorWorkingMode,
                currCtx.DataBase,
                currCtx.IsBatchStarted, 
                currCtx.CurrentBatchId,
                currCtx.CSVRotationParameters), ContextChangesEnum.MachineMode, false);
            //IsContextInitialized = CurrentContext.IsContextComplete();

            if (currCtx.SupervisorWorkingMode == SupervisorWorkingModeEnum.Knapp && currCtx.MachineMode == MachineModeEnum.Stop)
            {
                // Must warn GRETEL about the stop (synchronous).
                OnStartKnappRequested(new StartKnappEventArgs(SaveModeEnum.DontSave, 0, 0, 0));

                _ipcGateway.PushRequest(new IPCRequest("HmiWaitForKnappStart"));

                SetCurrentContext(new AppContext(
                    currCtx.CultureCode, 
                    currCtx.AppClientRect, 
                    currCtx.UserLevel, 
                    currCtx.ActiveRecipe,
                    currCtx.ActiveRecipeVersion,
                    currCtx.ActiveRecipeStatus,
                    currCtx.MachineMode, 
                    currCtx.EnabledStation, 
                    currCtx.MachineSpeed, 
                    SupervisorModeEnum.WaitForKnappStart, 
                    currCtx.SupervisorWorkingMode, 
                    currCtx.DataBase, 
                    currCtx.IsBatchStarted, 
                    currCtx.CurrentBatchId,
                    currCtx.CSVRotationParameters), ContextChangesEnum.SupervisorWorkingMode, false);
                //IsContextInitialized = CurrentContext.IsContextComplete();
            }
            SetExactaEasyMachineModeRequested?.Invoke(this, e);
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<SetExactaEasyPosEventArgs> SetExactaEasyAreaRequested;
        /// <summary>
        /// 
        /// </summary>
        internal void OnSetExactaEasyAreaRequested(SetExactaEasyPosEventArgs e)
        {
            AppContext currCtx = CurrentContext;
            SetCurrentContext(new AppContext(
                currCtx.CultureCode, 
                e.ApplicationArea,
                currCtx.UserLevel, 
                currCtx.ActiveRecipe,
                currCtx.ActiveRecipeVersion,
                currCtx.ActiveRecipeStatus,
                currCtx.MachineMode, 
                currCtx.EnabledStation, 
                currCtx.MachineSpeed, 
                currCtx.SupervisorMode, 
                currCtx.SupervisorWorkingMode, 
                currCtx.DataBase, 
                currCtx.IsBatchStarted,
                currCtx.CurrentBatchId,
                currCtx.CSVRotationParameters), ContextChangesEnum.Position, false);
            //IsContextInitialized = CurrentContext.IsContextComplete();

            SetExactaEasyAreaRequested?.Invoke(this, e);
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<SetExactaEasyUserLevelEventArgs> SetExactaEasyUserLevelRequested;
        /// <summary>
        /// 
        /// </summary>
        internal void OnSetExactaEasyUserLevelRequested(SetExactaEasyUserLevelEventArgs e)
        {
            AppContext currCtx = CurrentContext;
            SetCurrentContext(new AppContext(
                currCtx.CultureCode,
                currCtx.AppClientRect, 
                e.UserLevel, 
                currCtx.ActiveRecipe,
                currCtx.ActiveRecipeVersion,
                currCtx.ActiveRecipeStatus,
                currCtx.MachineMode,
                currCtx.EnabledStation,
                currCtx.MachineSpeed, 
                currCtx.SupervisorMode,
                currCtx.SupervisorWorkingMode,
                currCtx.DataBase, 
                currCtx.IsBatchStarted,
                currCtx.CurrentBatchId,
                currCtx.CSVRotationParameters), ContextChangesEnum.UserLevel, false);
            //IsContextInitialized = CurrentContext.IsContextComplete();

            SetExactaEasyUserLevelRequested?.Invoke(this, e);
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<SetExactaEasyDatabaseEventArgs> SetExactaEasyDatabaseRequested;
        /// <summary>
        /// 
        /// </summary>
        internal void OnSetExactaEasyDatabaseRequested(SetExactaEasyDatabaseEventArgs e)
        {
            AppContext currCtx = CurrentContext;
            SetCurrentContext(new AppContext(
                currCtx.CultureCode, 
                currCtx.AppClientRect, 
                currCtx.UserLevel, 
                currCtx.ActiveRecipe,
                currCtx.ActiveRecipeVersion,
                currCtx.ActiveRecipeStatus,
                currCtx.MachineMode,
                currCtx.EnabledStation,
                currCtx.MachineSpeed, 
                currCtx.SupervisorMode,
                currCtx.SupervisorWorkingMode,
                e.Database, 
                currCtx.IsBatchStarted, 
                currCtx.CurrentBatchId,
                currCtx.CSVRotationParameters), ContextChangesEnum.DataBase, false);
            //IsContextInitialized = CurrentContext.IsContextComplete();

            SetExactaEasyDatabaseRequested?.Invoke(this, e);
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ExactaEasyOperationInvokedEventArgs> SetHMIIsAliveInvoked;
        /// <summary>
        /// 
        /// </summary>
        internal void OnSetHMIIsAliveInvoked(ExactaEasyOperationInvokedEventArgs e)
        {
            SetHMIIsAliveInvoked?.Invoke(this, e);
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<SetExactaEasyMachineInfoEventArgs> SetExactaEasyMachineInfoRequested;
        /// <summary>
        /// 
        /// </summary>
        internal void OnSetExactaEasyMachineInfoRequested(SetExactaEasyMachineInfoEventArgs e)
        {
            AppContext currCtx = CurrentContext;
            SetCurrentContext(new AppContext(
                currCtx.CultureCode, 
                currCtx.AppClientRect, 
                currCtx.UserLevel,
                currCtx.ActiveRecipe,
                currCtx.ActiveRecipeVersion,
                currCtx.ActiveRecipeStatus,
                currCtx.MachineMode, 
                e.StationEnabled,
                e.Speed, 
                currCtx.SupervisorMode, 
                currCtx.SupervisorWorkingMode,
                currCtx.DataBase, 
                currCtx.IsBatchStarted, 
                currCtx.CurrentBatchId,
                currCtx.CSVRotationParameters), ContextChangesEnum.MachineInfo, false);
            //IsContextInitialized = CurrentContext.IsContextComplete();

            SetExactaEasyMachineInfoRequested?.Invoke(this, e);
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ExactaEasyOperationInvokedEventArgs> ResetExactaEasyErrorRQSRequested;
        /// <summary>
        /// 
        /// </summary>
        internal void OnResetExactaEasyErrorRQSRequested(ExactaEasyOperationInvokedEventArgs e)
        {
            ResetExactaEasyErrorRQSRequested?.Invoke(this, e);
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ExactaEasyOperationInvokedEventArgs> PrintExactaEasyActiveRecipeRequested;
        /// <summary>
        /// 
        /// </summary>
        internal void OnPrintExactaEasyActiveRecipeRequested(ExactaEasyOperationInvokedEventArgs e)
        {
            PrintExactaEasyActiveRecipeRequested?.Invoke(this, e);
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<StartBatchEventArgs> StartBatchRequested;
        /// <summary>
        /// 
        /// </summary>
        internal void OnStartBatchRequested(StartBatchEventArgs e)
        {
            StartBatchRequested?.Invoke(this, e);
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<StopBatchEventArgs> StopBatchRequested;
        /// <summary>
        /// 
        /// </summary>
        internal void OnStopBatchRequested(StopBatchEventArgs e)
        {
            AppContext currCtx = CurrentContext;
            
            SetCurrentContext(new AppContext(
                currCtx.CultureCode, 
                currCtx.AppClientRect, 
                currCtx.UserLevel, 
                currCtx.ActiveRecipe, 
                currCtx.ActiveRecipeVersion,
                currCtx.ActiveRecipeStatus,
                currCtx.MachineMode, 
                currCtx.EnabledStation, 
                currCtx.MachineSpeed, 
                currCtx.SupervisorMode, 
                currCtx.SupervisorWorkingMode, 
                currCtx.DataBase, 
                false, 
                -1,
                currCtx.CSVRotationParameters), ContextChangesEnum.SupervisorWorkingMode, false);

            StopBatchRequested?.Invoke(this, new StopBatchEventArgs(SaveModeEnum.DontSave));
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<SetBatchIdEventArgs> SetBatchIdRequested;
        /// <summary>
        /// 
        /// </summary>
        internal void OnSetBatchIdRequested(SetBatchIdEventArgs e)
        {
            AppContext currCtx = CurrentContext;
            SetCurrentContext(new AppContext(
                currCtx.CultureCode, 
                currCtx.AppClientRect,
                currCtx.UserLevel,
                currCtx.ActiveRecipe,
                currCtx.ActiveRecipeVersion,
                currCtx.ActiveRecipeStatus,
                currCtx.MachineMode,
                currCtx.EnabledStation, 
                currCtx.MachineSpeed,
                currCtx.SupervisorMode, 
                currCtx.SupervisorWorkingMode,
                currCtx.DataBase,
                currCtx.IsBatchStarted,
                e.Id, 
                currCtx.CSVRotationParameters), ContextChangesEnum.SupervisorWorkingMode, false);

            SetBatchIdRequested?.Invoke(this, e);
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<StartKnappEventArgs> StartKnappRequested;
        /// <summary>
        /// 
        /// </summary>
        internal void OnStartKnappRequested(StartKnappEventArgs e)
        {
            // Avvisare Gretel per lo start knapp (sincrono!)
            StartKnappRequested?.Invoke(this, e);
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<SetExactaEasyWorkingModeEventArgs> SetExactaEasyWorkingModeRequested;
        /// <summary>
        /// 
        /// </summary>
        internal void OnSetExactaEasyWorkingModeRequested(SetExactaEasyWorkingModeEventArgs e)
        {
            AppContext currCtx = CurrentContext;
            SetCurrentContext(new AppContext(
                currCtx.CultureCode,
                currCtx.AppClientRect,
                currCtx.UserLevel, 
                currCtx.ActiveRecipe, 
                currCtx.ActiveRecipeVersion,
                currCtx.ActiveRecipeStatus,
                currCtx.MachineMode,
                currCtx.EnabledStation,
                currCtx.MachineSpeed, 
                currCtx.SupervisorMode, 
                e.SupervisorWorkingMode, 
                currCtx.DataBase, 
                currCtx.IsBatchStarted, 
                currCtx.CurrentBatchId,
                currCtx.CSVRotationParameters), ContextChangesEnum.SupervisorWorkingMode, false);
            //IsContextInitialized = CurrentContext.IsContextComplete();

            if (e.SupervisorWorkingMode == SupervisorWorkingModeEnum.Knapp)
            {
                _ipcGateway.PushRequest(new IPCRequest("HmiWaitForKnappStart"));

                currCtx = CurrentContext;
                SetCurrentContext(new AppContext(
                    currCtx.CultureCode, 
                    currCtx.AppClientRect, 
                    currCtx.UserLevel, 
                    currCtx.ActiveRecipe,
                    currCtx.ActiveRecipeVersion,
                    currCtx.ActiveRecipeStatus,
                    currCtx.MachineMode,
                    currCtx.EnabledStation,
                    currCtx.MachineSpeed, 
                    SupervisorModeEnum.WaitForKnappStart,
                    e.SupervisorWorkingMode, currCtx.DataBase, 
                    currCtx.IsBatchStarted, 
                    currCtx.CurrentBatchId,
                    currCtx.CSVRotationParameters), ContextChangesEnum.SupervisorWorkingMode, false);
                //IsContextInitialized = CurrentContext.IsContextComplete();
            }
            SetExactaEasyWorkingModeRequested?.Invoke(this, e);
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<SetExactaEasyCsvRotationEventArgs> SetCsvRotationParamsRequested;
        /// <summary>
        /// 
        /// </summary>
        internal void OnSetCsvRotationParamsRequested(SetExactaEasyCsvRotationEventArgs e)
        {
            AppContext currCtx = CurrentContext;
            SetCurrentContext(new AppContext(
                currCtx.CultureCode,
                currCtx.AppClientRect,
                currCtx.UserLevel,
                currCtx.ActiveRecipe,
                currCtx.ActiveRecipeVersion,
                currCtx.ActiveRecipeStatus,
                currCtx.MachineMode,
                currCtx.EnabledStation,
                currCtx.MachineSpeed,
                currCtx.SupervisorMode,
                currCtx.SupervisorWorkingMode,
                currCtx.DataBase,
                currCtx.IsBatchStarted,
                currCtx.CurrentBatchId,
                e.Csv), ContextChangesEnum.Position, false);

            SetCsvRotationParamsRequested?.Invoke(this, e);
        }
        #endregion 

        #region AppEngine Events
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<AppEngineGatewayCommunicationStatusEventArgs> AppEngineHmiCommunicationStatusChanged;
        /// <summary>
        /// 
        /// </summary>
        internal void OnAppEngineHmiCommunicationStatusChanged(AppEngineGatewayCommunicationStatusEventArgs e)
        {
            AppEngineHmiCommunicationStatusChanged?.Invoke(this, e);
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<AppEngineContextChangedEventArgs> AppEngineContextChanged;
        /// <summary>
        /// 
        /// </summary>
        internal void OnAppEngineContextChanged(AppEngineContextChangedEventArgs e)
        {
            AppEngineContextChanged?.Invoke(this, e);
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<MessageEventArgs> AppEngineNetworkInterfaceCardStatus;
        /// <summary>
        /// 
        /// </summary>
        internal void OnAppEngineNetworkInterfaceCardStatus(MessageEventArgs e)
        {
            AppEngineNetworkInterfaceCardStatus?.Invoke(this, e);
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<AppEngineContextChangedEventArgs> AppEngineContextChangeError;
        /// <summary>
        /// 
        /// </summary>
        internal void OnAppEngineContextChangeError(AppEngineContextChangedEventArgs e)
        {
            AppEngineContextChangeError?.Invoke(this, e);
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<StartBatchEventArgs> AppEngineStartStopBatch;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<StartBatchEventArgs> AppEngineStartStopBatch2;
        /// <summary>
        /// 
        /// </summary>
        internal void OnAppEngineStartStopBatch(StartBatchEventArgs e)
        {
            if (MachineConfiguration is null)
            {
                AppEngineStartStopBatch?.Invoke(this, e);
                return;
            }

            switch (MachineConfiguration.TypeDumpImages)
            {
                case 0:
                    AppEngineStartStopBatch?.Invoke(this, e);
                    break;
                case 1:
                    AppEngineStartStopBatch2?.Invoke(this, e);
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<StartKnappEventArgs> AppEngineStartStopKnapp;
        /// <summary>
        /// 
        /// </summary>
        internal void OnAppEngineStartStopKnapp(StartKnappEventArgs e)
        {
            AppEngineStartStopKnapp?.Invoke(this, e);
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<MessageEventArgs> NetworkInterfaceCardStatus;
        /// <summary>
        /// 
        /// </summary>
        internal void OnNetworkInterfaceCardStatus(MessageEventArgs e)
        {
           
        }
        #endregion

        /// <summary>
        /// Semaphore for singleton instantiation.
        /// </summary>
        private static readonly object _singletonSynchObject = new object();
        /// <summary>
        /// Semaphore for the AppEngine context switch.
        /// </summary>
        private static readonly object _contextChangeSynchObject = new object();
        /// <summary>
        /// Object that encapsulates the logic called remotely via IPC.
        /// </summary>
        private ExactaEasyIpcLogic _ipcLogic;
        /// <summary>
        /// Artic/ExactaEasy IPC communication gateway.
        /// </summary>
        private IPCGatewayClient<ExactaEasyIpcLogic> _ipcGateway;
        /// <summary>
        /// Current supervisor running mode.
        /// </summary>
        public SupervisorModeEnum CurrentSupervisorMode { get; private set; }
        /// <summary>
        /// Signals a request to stop the IPC communication with Artic.
        /// </summary>
        private AutoResetEvent _statusThreadKillEvt = new AutoResetEvent(false);
        /// <summary>
        /// A reference to the gateway communication status check thread.
        /// </summary>
        private Thread _gatewayCommStatusThread;
        /// <summary>
        /// Signals when a gateway connection is established.
        /// </summary>
        private AutoResetEvent _gatewayConnectionEstablished = new AutoResetEvent(false);
        /// <summary>
        /// The last saved recipe.
        /// </summary>
        private Recipe _lastSavedRecipe = null;
        /// <summary>
        /// The last saved recipe.
        /// </summary>
        public Recipe LastSavedRecipe
        {
            get { return _lastSavedRecipe; }
        }
        /// <summary>
        /// AppEngine's current context.
        /// </summary>
        private AppContext _context = AppContext.EmptyContext;
        /// <summary>
        /// AppEngine's current context.
        /// </summary>
        public AppContext CurrentContext
        {
            get
            {
                return _context;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsContextInitialized { get; private set; } = false;
        /// <summary>
        /// 
        /// </summary>
        private ParameterInfoCollection _parametersInfo;
        /// <summary>
        /// 
        /// </summary>
        public ParameterInfoCollection ParametersInfo
        {
            get
            {
                if (_parametersInfo == null)
                    LoadParameterInfoDictionary();
                return _parametersInfo;
            }
        }
        /// <summary>
        /// Status of the gateway client connection.
        /// </summary>
        private GatewayCommunicationStatusEnum _gatewayCommunicationStatus;
        /// <summary>
        /// Status of the gateway client connection.
        /// </summary>
        public GatewayCommunicationStatusEnum GatewayCommunicationStatus
        {
            get { return _gatewayCommunicationStatus; }
            set
            {
                // Check if the status is the same.
                if (_gatewayCommunicationStatus == value) return;
                _gatewayCommunicationStatus = value;
                OnAppEngineHmiCommunicationStatusChanged(
                    new AppEngineGatewayCommunicationStatusEventArgs(_gatewayCommunicationStatus));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public GlobalConfiguration GlobalConfig { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public MachineConfiguration MachineConfiguration { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public DumpImagesConfiguration DumpImagesConfiguration { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public DumpImagesConfiguration2 DumpImagesConfiguration2 { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public TrendTool TrendTool { get; private set; }
        /// <summary>
        /// Singleton instance.
        /// </summary>
        private static AppEngine _appEngineInstance = null;
        /// <summary>
        /// Accessor for the singleton instance.
        /// </summary>
        public static AppEngine Current
        {
            get
            {
                lock (_singletonSynchObject)
                {
                    if (_appEngineInstance is null)
                        _appEngineInstance = new AppEngine();
                    return _appEngineInstance;
                }
            }
        }
        /// <summary>
        /// Class instance constructor. 
        /// </summary>
        private AppEngine()
        {
            // The very first status of the connection: there are no instances of the gateway.
            GatewayCommunicationStatus = GatewayCommunicationStatusEnum.NotCreated;
            _context = AppContext.EmptyContext;

            if (!SPAMI.Util.Utilities.IsDesignMode())
            {
                string fileName = "";
                System.Reflection.Assembly startAssembly = System.Reflection.Assembly.GetEntryAssembly();
                string psName = startAssembly.ManifestModule.Name.ToLower();
                if (startAssembly != null && (psName == "exactaeasy.exe" || psName == "exeygtwtest.exe"))
                    fileName = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\globalConfig.xml";
                else
                    fileName = Environment.CurrentDirectory + @"\DotNet Components\ExactaEasy\globalConfig.xml"; // Specifico per IFIX

                Log.Line(LogLevels.Pass, "AppEngine.ctor", "Loading global config file. Path:" + fileName);
                GlobalConfig = GlobalConfiguration.LoadFromFile(fileName);

                MachineConfiguration = LoadMachineConfiguration();

                DumpImagesConfiguration = LoadDumpImagesConfiguration();

                TrendTool = LoadTrendTool();
                if (DumpImagesConfiguration != null)
                {
                    foreach (DumpImagesUserSettings dic in DumpImagesConfiguration.UserSettings)
                    {
                        foreach (StationDumpSettings sds in dic.StationsDumpSettings)
                        {
                            NodeSetting ns = MachineConfiguration.NodeSettings.Find(nn => nn.Id == sds.Node);
                            if (ns != null)
                                sds.Description = ns.NodeDescription;
                            StationSetting ss = MachineConfiguration.StationSettings.Find(sn => sn.Node == sds.Node && sn.Id == sds.Id);
                            if (ss != null)
                                sds.Description += " - " + ss.StationDescription;
                        }
                    }
                    //appDebug
                    foreach (NodeSetting nodeSet in MachineConfiguration.NodeSettings)
                        foreach (StationSetting stationSet in MachineConfiguration.StationSettings)
                            if (nodeSet.Id == stationSet.Node)
                                AppDebug.AppDebug.SizeImagesResults.Add(new DebugSizeImagesResults(nodeSet.Id, nodeSet.NodeDescription, stationSet.Id, stationSet.StationDescription));
                }
            }
            // Creates a new context for the engine with the default setup.
            SetCurrentContext(new AppContext(
                _context.CultureCode,
                _context.AppClientRect,
                _context.UserLevel,
                _context.ActiveRecipe,
                _context.ActiveRecipeVersion,
                _context.ActiveRecipeStatus,
                _context.MachineMode,
                _context.EnabledStation,
                _context.MachineSpeed,
                SupervisorModeEnum.ReadyToRun,
                _context.SupervisorWorkingMode,
                _context.DataBase,
                _context.IsBatchStarted,
                _context.CurrentBatchId,
                _context.CSVRotationParameters), ContextChangesEnum.SupervisorMode, false);
        }
        /// <summary>
        /// Loads the machine configuration from machineConfig.xml.
        /// </summary>
        private MachineConfiguration LoadMachineConfiguration()
        {
            var filePath = Path.Combine(GlobalConfig.SettingsFolder, MACHINE_CONFIG_XML_FILE);
            return MachineConfiguration.LoadFromFile(filePath);
        }
        /// <summary>
        /// Loads the dump images configuration from dumpImagesConfig.xml.
        /// </summary>
        private DumpImagesConfiguration LoadDumpImagesConfiguration()
        {
            DumpImagesConfiguration conf = null;

            var filePath = Path.Combine(GlobalConfig.SettingsFolder, DUMP_IMAGES_COMFIG_XML_FILE);

            if (File.Exists(filePath))
                conf = DumpImagesConfiguration.LoadFromFile(filePath, MachineConfiguration);

            return conf;
        }
        /// <summary>
        /// 
        /// </summary>
        private TrendTool LoadTrendTool()
        {
            TrendTool trend;
            try
            {
                trend = TrendTool.Load(MachineConfiguration.TrendDataEnable, MachineConfiguration.FolderStorageTrendData);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while loading the TrendTool.", ex);
            }
            return trend;
        }
        /// <summary>
        /// Sets the current AppEngine context and checks its completeness.
        /// </summary>
        public void SetCurrentContext(AppContext newContext, ContextChangesEnum contextChanges, bool applyParams)
        {
            // A mutual exclusion lock is mandatory here.
            lock (_contextChangeSynchObject)
            {
                _context = newContext;
                IsContextInitialized = CurrentContext.IsContextComplete();
                AppEngineContextChanged?.Invoke(this, new AppEngineContextChangedEventArgs(contextChanges, applyParams));
            }
        }
        /// <summary>
        /// Initializes the IPC communication with Artic.
        /// </summary>
        private void InitializeCommunication()
        {
            // Creates a new object with the business logic for the supervisor. An AppEngine
            // reference is required.
            _ipcLogic = new ExactaEasyIpcLogic(_appEngineInstance);
            // Creates a new communication gateway with the HMI.
            _ipcGateway = new IPCGatewayClient<ExactaEasyIpcLogic>("exacta_easy_gateway2_client", _ipcLogic);
            // Subscribes to the connected/disconnected events.
            _ipcGateway.OnClientConnected += IpcGateway_ConnectedToArtic;
            _ipcGateway.OnClientDisconnected += IpcGateway_DisconnectedFromArtic;
            // Creates as new Thread for the gateway communication status check.
            _gatewayCommStatusThread = new Thread(
                new ThreadStart(GatewayCommunicationStatusThread))
            {
                Name = "GatewayCommunicationStatusThread"
            };
            // Runs the connection status check thread.
            _gatewayCommStatusThread.Start();
        }
        /// <summary>
        /// IPC gateway communication status check thread.
        /// </summary>
        private void GatewayCommunicationStatusThread()
        {
            // Initial default state is NotConnected.
            GatewayCommunicationStatus = GatewayCommunicationStatusEnum.NotConnected;
            // Null-check on the IPC gateway component.
            if (_ipcGateway is null)
            {
                Log.Line(
                    LogLevels.Error,
                    "AppEngine.GatewayCommunicationStatusThread",
                    "IPC communication object not instantiated.");
                return;
            }

            while (true)
            {
                // Waits for a thread kill event for DEFAULT_IPC_STATUS_THREAD_LOOP_TIME_MS ms.
                if (_statusThreadKillEvt.WaitOne(DEFAULT_IPC_STATUS_THREAD_LOOP_TIME_MS))
                {
                    // A kill event has been caught, exit the thread.
                    break;
                }
                // Check if ARTICOE or the test program are running.
                //if (!IPCProcess.IsAnyRunning("artic", "exeygtwtest"))
                //    GatewayCommunicationStatus = GatewayCommunicationStatusEnum.NotConnected;

                switch (GatewayCommunicationStatus)
                {
                    case GatewayCommunicationStatusEnum.NotCreated:
                        Log.Line(
                            LogLevels.Error,
                            "AppEngine.GatewayCommunicationStatusThread",
                            "IPC communication object not created.");
                        break;

                    case GatewayCommunicationStatusEnum.NotConnected:
                        // Set the gateway status as CONNECTING to ARTIC.
                        GatewayCommunicationStatus = GatewayCommunicationStatusEnum.Connecting;
                        // Attempts a connection to ARTIC: the server (ARTIC) must be already started!
                        _ipcGateway.ConnectToServer();
                        break;

                    case GatewayCommunicationStatusEnum.Connecting:
                        // Waits here until the gateway connection is done.
                        // (SetSupIsAlive is called and succeeded).
                        if (_gatewayConnectionEstablished.WaitOne(IPC_CONNECTION_RETRY_TIMEOUT_MS))
                        {
                            // Connection succeeded.
                            GatewayCommunicationStatus = GatewayCommunicationStatusEnum.Connected;
                        }
                        else
                        {
                            // Connection failed: status is going back to NotConnected so we'll attempt 
                            // a new connection in the next loop.
                            GatewayCommunicationStatus = GatewayCommunicationStatusEnum.NotConnected;
                        }
                        break;

                    case GatewayCommunicationStatusEnum.Connected:
                        // We are connected to the gateway server, but that doesn't mean the connection is
                        // established. The connection is established only when HmiSetSupIsAliveEx is called with success.
                        var resp = _ipcGateway.PushRequestWithResponse(
                            new IPCRequest("HmiSetSupIsAliveEx"),
                            IPC_SETSUPISALIVEEX_TIMEOUT_MS);
                        // Check out the response.
                        if (resp is null ||
                            resp.Response is null ||
                            resp.Response.Status == IPCResponseStatusEnum.KO)
                        {
                            // The gateway is connected but HmiSetSupIsAliveEx failed.
                            GatewayCommunicationStatus = GatewayCommunicationStatusEnum.Connected;
                            Thread.Sleep(300);
                        }
                        else
                        {
                            if (resp.Response.ReturnValueAs<bool>())
                            {
                                // The communication with ARTIC is established.
                                GatewayCommunicationStatus = GatewayCommunicationStatusEnum.Established;
                                // Set bypass send recipe.
                                SendSupervisorBypassSendRecipe(MachineConfiguration.BypassHMIsendRecipe);
                            }
                            else
                            {
                                // The gateway is connected but HmiSetSupIsAliveEx failed.
                                GatewayCommunicationStatus = GatewayCommunicationStatusEnum.Connected;
                                Thread.Sleep(300);
                            }
                        }
                        break;

                    case GatewayCommunicationStatusEnum.Established:
                        // Just waits for a DisconnectedFromArtic event 
                        // or a failed ping (HmiGetErrorString) to happen to set the disconnected state.
                        var respMsg = _ipcGateway.PushRequestWithResponse(
                            new IPCRequest("HmiGetErrorString",
                                new IPCParameter("errorCode", 0)));

                        if (respMsg.Response.Status == IPCResponseStatusEnum.KO)
                            GatewayCommunicationStatus = GatewayCommunicationStatusEnum.NotConnected;
                        else
                            Thread.Sleep(500);
                        break;
                }
            }
            // A StopEngine() call has been issued.
            GatewayCommunicationStatus = GatewayCommunicationStatusEnum.NotConnected;
        }
        /// <summary>
        /// Handles a disconnection event.
        /// </summary>
        private void IpcGateway_DisconnectedFromArtic(object sender, EventArgs e)
        { 
            Log.Line(LogLevels.Debug, "AppEngine", "ExactaEasy disconnected from Artic");
            GatewayCommunicationStatus = GatewayCommunicationStatusEnum.NotConnected;
        }
        /// <summary>
        /// Handles a connection event.
        /// </summary>
        private void IpcGateway_ConnectedToArtic(object sender, EventArgs e)
        {
            Log.Line(LogLevels.Debug, "AppEngine", "ExactaEasy connected to Artic");
            // Wakes up the IPC connection status thread, so it can move to the next state (CONNECTED).
            _gatewayConnectionEstablished.Set();
        }
        /// <summary>
        /// Starts the AppEngine.
        /// </summary>
        public void StartEngine()
        {
            InitializeCommunication();
        }
        /// <summary>
        /// Stops the AppEngine.
        /// </summary>
        public void StopEngine()
        {
            // Signals the status update thread to stop.
            _statusThreadKillEvt.Set();
            // Waits until the thread is terminated.
            _gatewayCommStatusThread.Join(IPC_CONNECTION_RETRY_TIMEOUT_MS * 2);
            // Disposes of the gateway.
            if (!(_ipcGateway is null))
                _ipcGateway.DisconnectFromServer();
        }
        /// <summary>
        /// 
        /// </summary>
        public void CheckAdaptersPresence()
        {
            if (MachineConfiguration.AdaptersCheck.Count > 0)
            {
                Task adaptersCheckTask = new Task(new Action(AdaptersCheck));
                adaptersCheckTask.Start();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool TrySaveRecipe()
        {
            try
            {
                _ipcGateway.PushRequest(
                    new IPCRequest(
                        "HmiSaveRecipeModification",
                        new IPCParameter("recipe", CurrentContext.ActiveRecipe)));
                return true;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "AppEngine.TrySaveRecipe", "An error occurred while saving recipe: " + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SetActiveRecipe(Recipe newRecipe, bool applyParams)
        {
            return _ipcLogic.SetActiveRecipe(newRecipe.RecipeName, newRecipe.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        public bool TryAuditRecipe(string message)
        {
            bool res = true;
            try
            {
                File.WriteAllText(@".\Log\audit.txt", message);
                //  AuditMessage(int totalMessageCount, int partialMessageCount, string messageToAudit);
                var respMsg = _ipcGateway.PushRequestWithResponse(
                    new IPCRequest("HmiAuditMessage",
                        new IPCParameter("totalMessageCount", 0),
                        new IPCParameter("partialMessageCount", 1),
                        new IPCParameter("messageToAudit", message)));

                int auditRes = -1;
                switch(respMsg.Response.Status)
                {
                    case IPCResponseStatusEnum.OK:
                        auditRes = (int)respMsg.Response.Value;
                        break;
                }

                if (auditRes < 0)
                {
                    Log.Line(LogLevels.Error, "AppEngine.TryAuditRecipe", "Audit failed. Error: " + auditRes);
                    res = false;
                }
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "AppEngine.TryAuditRecipe", "An error occurred while auditing recipe: " + ex.Message);
                res = false;
            }
            return res;
        }
        /// <summary>
        /// 
        /// </summary>
        private List<List<ParameterDiff>> splitParamDiffList(List<ParameterDiff> paramDiffList, int size)
        {

            List<List<ParameterDiff>> list = new List<List<ParameterDiff>>();
            for (int i = 0; i < paramDiffList.Count; i += size)
            {
                list.Add(paramDiffList.GetRange(i, Math.Min(size, paramDiffList.Count - i)));
            }
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        private string buildAuditMessage(List<ParameterDiff> paramDiffList)
        {

            StringBuilder message = new StringBuilder();

            //bool isFirst = true;
            string msgStr = "";
            const string fieldSeparator = ";";
            const string nullValue = "NULL";
            foreach (ParameterDiff diff in paramDiffList)
            {
                message.Append(diff.DifferenceType + fieldSeparator);
                message.Append(diff.ParameterPosition + fieldSeparator);
                message.Append(diff.ParameterId + fieldSeparator);
                message.Append(diff.ParameterLabel + fieldSeparator);
                message.Append(diff.ParameterLocLabel + fieldSeparator);
                if (string.IsNullOrEmpty(diff.ComparedValue) == true)
                    message.Append(nullValue + fieldSeparator);
                else
                    message.Append(diff.ComparedValue + fieldSeparator);
                if (string.IsNullOrEmpty(diff.CurrentValue) == true)
                    message.Append(nullValue + fieldSeparator);
                else
                    message.Append(diff.CurrentValue + fieldSeparator);
                message.Append(fieldSeparator);
                message.Append(fieldSeparator);
                message.Append("\r\n");
            }
            if (message.Length > 2)
                msgStr = message.Remove(message.Length - 2, 2).ToString();
            return msgStr;
        }
        /// <summary>
        /// Sends a request to ARTIC to save the currently loaded recipe.
        /// </summary>
        public void SaveRecipe()
        {
            var currentRecipe = CurrentContext.ActiveRecipe;
            //  AuditMessage(int totalMessageCount, int partialMessageCount, string messageToAudit);
            _ipcGateway.PushRequest(
                new IPCRequest("HmiSaveRecipeModification",
                    new IPCParameter("currentRecipe", currentRecipe.ToString())));
        }
        /// <summary>
        /// 
        /// </summary>
        public void SendResults(string results)
        {
            _ipcGateway.PushRequest(
                new IPCRequest("HmiSendResults", 
                    new IPCParameter("results", results)));
        }
        /// <summary>
        /// 
        /// </summary>
        public bool SendSupervisorMode(SupervisorModeEnum supervisorMode)
        {
            try
            {
                _ipcGateway.PushRequest(
                    new IPCRequest("HmiSetSupervisorMode",
                        new IPCParameter("hmiMode", (int)supervisorMode)));
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Warning, "AppEngine.SendSupervisorMode", $"HmiSetSupervisorMode failed: {ex.Message}");
                return false;
            }

            try
            {
                SetCurrentContext(
                    new AppContext(
                        _context.CultureCode,
                        _context.AppClientRect,
                        _context.UserLevel,
                        _context.ActiveRecipe,
                        _context.ActiveRecipeVersion,
                        _context.ActiveRecipeStatus,
                        _context.MachineMode,
                        _context.EnabledStation,
                        _context.MachineSpeed,
                        supervisorMode,
                        _context.SupervisorWorkingMode,
                        _context.DataBase,
                        _context.IsBatchStarted,
                        _context.CurrentBatchId,
                        _context.CSVRotationParameters), ContextChangesEnum.SupervisorMode, false);
                CurrentSupervisorMode = supervisorMode;
                return true;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Warning, "AppEngine.SendSupervisorMode", $"SetCurrentContext failed: {ex.Message}");
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void SendSupervisorBypassSendRecipe(bool bypass)
        {
            lock (_contextChangeSynchObject)
            {
                _ipcGateway.PushRequest(
                      new IPCRequest("HmiSetSupervisorBypassSendRecipe",
                          new IPCParameter("bypass", bypass)));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void SendSupervisorButtonClicked(string buttonID)
        {
            _ipcGateway.PushRequest(
                   new IPCRequest("HmiSupervisorButtonClicked",
                       new IPCParameter("buttonID", buttonID)));
        }
        /// <summary>
        /// 
        /// </summary>
        public bool TrySetSupervisorInEditing()
        {
            bool ris = true;
            if (_context.SupervisorMode != SupervisorModeEnum.Editing)
            {
                if ((_context.SupervisorMode != SupervisorModeEnum.ReadyToRun &&
                    _context.SupervisorMode != SupervisorModeEnum.Busy &&
                    _context.SupervisorMode != SupervisorModeEnum.Error) || // Si permette la modifica dei parametri in caso di errore
                                                                            // perchè in effetti l'errore potrebbe essere causato da parametri
                                                                            // errati
                    (_context.MachineMode != MachineModeEnum.Unknown && //pier: patch da radere al suolo?
                    _context.MachineMode != MachineModeEnum.Error &&    //pier: patch da radere al suolo?
                    _context.MachineMode != MachineModeEnum.Stop))
                    ris = false;
                else
                {
                    try
                    {
                        _appEngineInstance.SendSupervisorMode(SupervisorModeEnum.Editing);
                    }
                    catch
                    {
                        ris = false;
                    }
                }
            }
            return ris;
        }
        /// <summary>
        /// 
        /// </summary>
        public bool TrySetSupervisorStatus(SupervisorModeEnum newStatus)
        {
            try
            {
                if (_appEngineInstance.SendSupervisorMode(newStatus))
                {
                    Log.Line(LogLevels.Pass, "AppEngine.TrySetSupervisorStatus", "Supervisor status changed to " + newStatus.ToString());
                    return true;
                }
                Log.Line(LogLevels.Warning, "AppEngine.TrySetSupervisorStatus", "TrySetSupervisorStatus failed while changing status to " + newStatus.ToString());
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Warning, "AppEngine.TrySetSupervisorStatus", "TrySetSupervisorStatus failed. Error: " + ex.ToString());
            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        private void LoadParameterInfoDictionary()
        {
            try
            {
                _parametersInfo = ParameterInfoCollection.LoadFromFile(GlobalConfig.SettingsFolder + "/parametersDictionary.xml");
                if (_parametersInfo == null)
                    Log.Line(LogLevels.Error, "AppEngine.loadParameterInfoDictionary", "Parameters dictionary not found or corrupted");
            }
            catch (Exception)
            {
                Log.Line(LogLevels.Error, "AppEngine.loadParameterInfoDictionary", "Parameters dictionary not found or corrupted");
                throw new ParameterDictionaryNotFoundException();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void SaveParameterInfoDictionary()
        {
            try
            {
                if (_parametersInfo != null)
                    _parametersInfo.SaveFile(GlobalConfig.SettingsFolder + "/parametersDictionary.xml");
            }
            catch (Exception)
            {
                throw new ParameterDictionaryNotFoundException();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void AdaptersCheck()
        {
            foreach (string ipToCheck in MachineConfiguration.AdaptersCheck)
            {
                bool passed = false;
                while (!NetworkInterface.GetIsNetworkAvailable())
                {
                    OnNetworkInterfaceCardStatus(new MessageEventArgs(new LogMessage(LogLevels.Error, "Network not available")));
                    Thread.Sleep(1000);
                }
                while (!passed)
                {
                    foreach (NetworkInterface netif in NetworkInterface.GetAllNetworkInterfaces())
                    {
                        IPInterfaceProperties properties = netif.GetIPProperties();
                        foreach (IPAddressInformation unicast in properties.UnicastAddresses)
                        {
                            if (unicast.Address.ToString() == ipToCheck)
                            {
                                if (netif.OperationalStatus == OperationalStatus.Up /*||
                                    netif.OperationalStatus == OperationalStatus.Down*/)
                                {
                                    passed = true;
                                    OnNetworkInterfaceCardStatus(new MessageEventArgs(new LogMessage(LogLevels.Pass, ipToCheck.ToString() + ": " + netif.OperationalStatus.ToString())));
                                }
                                else
                                    OnNetworkInterfaceCardStatus(new MessageEventArgs(new LogMessage(LogLevels.Error, ipToCheck.ToString() + ": " + netif.OperationalStatus.ToString())));
                            }
                        }
                    }
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
