using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.Globalization;
using SPAMI.Util.Logger;

namespace ExactaEasyEng {

    public delegate void SetSupervisorActiveRecipeEventHandler(object sender, SetSupervisorActiveRecipeEventArgs e);
    public delegate void SetSupervisorActiveRecipeVersionEventHandler(object sender, SetSupervisorActiveRecipeVersionEventArgs e);
    public delegate void SetSupervisorActiveRecipeStatusEventHandler(object sender, SetSupervisorActiveRecipeStatusEventArgs e);
    public delegate void SetSupervisorActiveRecipeAllParamsEventHandler(object sender, SetSupervisorActiveRecipeAllParamsEventArgs e);
    public delegate void SetSupervisorLanguageCodeEventHandler(object sender, SetSupervisorLanguageCodeEventArgs e);
    public delegate void SetSupervisorMachineModeEventHandler(object sender, SetSupervisorMachineModeEventArgs e);
    public delegate void SetSupervisorPosEventHandler(object sender, SetSupervisorPosEventArgs e);
    public delegate void SetSupervisorUserLevelEventHandler(object sender, SetSupervisorUserLevelEventArgs e);
    public delegate void SetSupervisorDataBaseEventHandler(object sender, SetSupervisorDataBaseEventArgs e);
    public delegate void SetHMIIsAliveEventHandler(object sender, EventArgs e);
    public delegate void SetSupervisorMachineInfoEventHandler(object sender, SetSupervisorMachineInfoEventArgs e);


    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Supervisor : ISupervisor {

        public event EventHandler<SupervisorOperationInvokedEventArgs> SetSupervisorOnTopInvoked;
        public event EventHandler<SupervisorOperationInvokedEventArgs> SetSupervisorHideInvoked;
        public event SetSupervisorActiveRecipeEventHandler SetSupervisorActiveRecipeInvoked;
        public event SetSupervisorActiveRecipeVersionEventHandler SetSupervisorActiveRecipeVersionInvoked;
        public event SetSupervisorActiveRecipeStatusEventHandler SetSupervisorActiveRecipeStatusInvoked;
        public event SetSupervisorActiveRecipeAllParamsEventHandler SetSupervisorActiveRecipeAllParamsInvoked;
        public event SetSupervisorLanguageCodeEventHandler SetSupervisorLanguageCodeInvoked;
        public event SetSupervisorMachineModeEventHandler SetSupervisorMachineModeInvoked;
        public event SetSupervisorPosEventHandler SetSupervisorPosInvoked;
        public event SetSupervisorUserLevelEventHandler SetSupervisorUserLevelInvoked;
        public event SetSupervisorDataBaseEventHandler SetSupervisorDataBaseInvoked;
        public event SetHMIIsAliveEventHandler SetHMIIsAliveInvoked;
        public event SetSupervisorMachineInfoEventHandler SetSupervisorMachineInfoInvoked;
        public event EventHandler<SupervisorOperationInvokedEventArgs> ResetErrorRQSInvoked;
        public event EventHandler PrintActiveRecipeInvoked;
        public event EventHandler<StartBatchEventArgs> StartBatchInvoked;
        public event EventHandler StopBatchInvoked;
        public event EventHandler<SetBatchIdEventArgs> SetBatchIdInvoked;
        public event EventHandler<StartKnappEventArgs> StartKnappInvoked;
        public event EventHandler<SetSupervisorWorkingModeEventArgs> SetSupervisorWorkingModeInvoked;

        #region ISupervisor Membri di

        public int SetSupervisorOnTop() {

            Log.Line(LogLevels.Debug, "Supervisor.SetSupervisorOnTop", "SetSupervisorOnTop");
            int ris = 0;
            try {
                SupervisorOperationInvokedEventArgs e = new SupervisorOperationInvokedEventArgs();
                OnSetSupervisorOnTopInvoked(this, e);
                if (e.Cancel)
                    ris = 1;
            }
            catch (Exception) {
                Log.Line(LogLevels.Error, "Supervisor.SetSupervisorOnTop", "SetSupervisorOnTop failed");
                ris = 1;
            }
            return ris;
        }

        public int SetSupervisorHide() {

            Log.Line(LogLevels.Debug, "Supervisor.SetSupervisorHide", "SetSupervisorHide");
            int ris = 0;
            try {
                SupervisorOperationInvokedEventArgs e = new SupervisorOperationInvokedEventArgs();
                OnSetSupervisorHideInvoked(this, e);
                if (e.Cancel)
                    ris = 1;
            }
            catch (Exception) {
                Log.Line(LogLevels.Error, "Supervisor.SetSupervisorHide", "SetSupervisorHide failed");
                ris = 1;
            }
            return ris;
        }

        public int SetActiveRecipe(string recipeName, string recipeXml) {

            Log.Line(LogLevels.Debug, "Supervisor.SetActiveRecipe", "SetActiveRecipe");
            int ris = 0;
            try {
                Recipe activeRecipe = Recipe.LoadFromXml(recipeXml);
                activeRecipe.RecipeName = recipeName;
                SetSupervisorActiveRecipeEventArgs e = new SetSupervisorActiveRecipeEventArgs(activeRecipe, recipeXml);
                OnSetSupervisorActiveRecipeInvoked(this, e);
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "Supervisor.SetActiveRecipe", "SetActiveRecipe failed: " + ex.Message);
                SetSupervisorActiveRecipeEventArgs e = new SetSupervisorActiveRecipeEventArgs(null, recipeXml);
                OnSetSupervisorActiveRecipeInvoked(this, e);
                ris = 1;
            }
            return ris;
        }

        public int SetActiveRecipeVersion(int recipeVersion)
        {
            Log.Line(LogLevels.Debug, "Supervisor.SetActiveRecipeVersion", "SetActiveRecipeVersion");
            int ris = 0;
            try
            {
                SetSupervisorActiveRecipeVersionEventArgs e = new SetSupervisorActiveRecipeVersionEventArgs(recipeVersion);
                OnSetSupervisorActiveRecipeVersionInvoked(this, e);
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "Supervisor.SetActiveRecipeVersion", "SetActiveRecipeVersion failed: " + ex.Message);
                SetSupervisorActiveRecipeVersionEventArgs e = new SetSupervisorActiveRecipeVersionEventArgs(-1);
                OnSetSupervisorActiveRecipeVersionInvoked(this, e);
                ris = 1;
            }
            return ris;
        }

        public int SetActiveRecipeStatus(string recipestatus)
        {
            Log.Line(LogLevels.Debug, "Supervisor.SetActiveRecipeStatus", "SetActiveRecipeStatus");
            int ris = 0;
            try
            {
                SetSupervisorActiveRecipeStatusEventArgs e = new SetSupervisorActiveRecipeStatusEventArgs(recipestatus);
                OnSetSupervisorActiveRecipeStatusInvoked(this, e);
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "Supervisor.SetActiveRecipeStatus", "SetActiveRecipeStatus failed: " + ex.Message);
                SetSupervisorActiveRecipeStatusEventArgs e = new SetSupervisorActiveRecipeStatusEventArgs("");
                OnSetSupervisorActiveRecipeStatusInvoked(this, e);
                ris = 1;
            }
            return ris;
        }

        public int SetActiveRecipeAllParams(string recipeName, string recipeXml, int recipeVersion, string recipeStatus)
        {

            Log.Line(LogLevels.Debug, "Supervisor.SetActiveRecipeAllParams", "SetActiveRecipeAllParams");
            int ris = 0;
            try
            {
                Recipe activeRecipe = Recipe.LoadFromXml(recipeXml);
                activeRecipe.RecipeName = recipeName;
                SetSupervisorActiveRecipeAllParamsEventArgs e = new SetSupervisorActiveRecipeAllParamsEventArgs(activeRecipe, recipeXml, recipeVersion, recipeStatus);
                OnSetSupervisorActiveRecipeAllParamsInvoked(this, e);
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "Supervisor.SetActiveRecipeAllParams", "SetActiveRecipeAllParams failed: " + ex.Message);
                SetSupervisorActiveRecipeAllParamsEventArgs e = new SetSupervisorActiveRecipeAllParamsEventArgs(null, recipeXml, -1, "");
                OnSetSupervisorActiveRecipeAllParamsInvoked(this, e);
                ris = 1;
            }
            return ris;
        }

        public int SetLanguage(string languageCode) {

            Log.Line(LogLevels.Debug, "Supervisor.SetLanguage", "SetLanguage");
            int ris = 0;
            try {
                SetSupervisorLanguageCodeEventArgs e = new SetSupervisorLanguageCodeEventArgs(languageCode);
                OnSetSupervisorLanguageCodeInvoked(this, e);
                if (e.Cancel)
                    ris = 1;
            }
            catch (Exception) {
                Log.Line(LogLevels.Error, "Supervisor.SetLanguage", "SetLanguage failed");
                ris = 1;
            }
            return ris;
        }

        public int SetSupervisorPos(int startX, int startY, int sizeX, int sizeY) {

            Log.Line(LogLevels.Debug, "Supervisor.SetSupervisorPos", "SetSupervisorPos");
            int ris = 0;
            try {
                SetSupervisorPosEventArgs e = new SetSupervisorPosEventArgs(new Rectangle(startX, startY, sizeX, sizeY));
                OnSetSupervisorPosInvoked(this, e);
                if (e.Cancel)
                    ris = 1;
            }
            catch (Exception) {
                Log.Line(LogLevels.Error, "Supervisor.SetSupervisorPos", "SetSupervisorPos failed");
                ris = 1;
            }
            return ris;
        }

        public int SetUserLevel(int userLevel) {

            Log.Line(LogLevels.Debug, "Supervisor.SetUserLevel", "SetUserLevel");
            int ris = 0;
            try {
                SetSupervisorUserLevelEventArgs e = new SetSupervisorUserLevelEventArgs((UserLevelEnum)userLevel);
                OnSetSupervisorUserLevelInvoked(this, e);
                if (e.Cancel)
                    ris = 1;
            }
            catch (Exception) {
                Log.Line(LogLevels.Error, "Supervisor.SetUserLevel", "SetUserLevel failed");
                ris = 1;
            }
            return ris;
        }

        public int SetDataBase(int dataBase)
        {

            Log.Line(LogLevels.Debug, "Supervisor.DataBase", "SetDataBase");
            int ris = 0;
            try
            {
                SetSupervisorDataBaseEventArgs e = new SetSupervisorDataBaseEventArgs(dataBase);
                OnSetSupervisorDataBaseInvoked(this, e);
                if (e.Cancel)
                    ris = 1;
            }
            catch (Exception)
            {
                Log.Line(LogLevels.Error, "Supervisor.SetDataBase", "SetDataBase failed");
                ris = 1;
            }
            return ris;
        }

        public int SetMachineMode(int mode) {

            Log.Line(LogLevels.Debug, "Supervisor.SetMachineMode", "SetMachineMode");
            int ris = 0;
            try {
                SetSupervisorMachineModeEventArgs e = new SetSupervisorMachineModeEventArgs((MachineModeEnum)mode);
                OnSetSupervisorMachineModeInvoked(this, e);
                if (e.Cancel)
                    ris = 1;
            }
            catch (Exception) {
                Log.Line(LogLevels.Error, "Supervisor.SetMachineMode", "SetMachineMode failed");
                ris = 1;
            }
            return ris;
        }

        public int SetHMIIsAlive(int mode) {

            Log.Line(LogLevels.Debug, "Supervisor.SetHMIIsAlive", "SetHMIIsAlive");
            int ris = 0;
            try {
                EventArgs e = new EventArgs();
                OnSetHMIIsAliveInvoked(this, e);
                //if (e.Cancel)
                //    ris = 1;
            }
            catch (Exception) {
                Log.Line(LogLevels.Error, "Supervisor.SetHMIIsAlive", "SetHMIIsAlive failed");
                ris = 1;
            }
            return ris;
        }

        public string GetErrorString(int errorCode) {

            Log.Line(LogLevels.Debug, "Supervisor.GetErrorString", "GetErrorString");
            return "E: " + errorCode.ToString();
        }

        public int SetSupervisorMachineInfo(string stationInfo, string speed) {

            Log.Line(LogLevels.Debug, "Supervisor.SetSupervisorMachineInfo", "SetSupervisorMachineInfo");
            int ris = 0;
            try {
                string[] info = stationInfo.Split(new char[] { ';' });
                if (info.Length > 0) {
                    bool[] enabledStation = new bool[info.Length];
                    for (int i = AppEngine.Current.MachineConfiguration.FirstStationIndex; i < info.Length; i++)
                        enabledStation[i - AppEngine.Current.MachineConfiguration.FirstStationIndex] = (info[i] == "1");
                    decimal _speed = 0;
                    Decimal.TryParse(speed, System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfo.GetCultureInfo("en-US"), out _speed);
                    SetSupervisorMachineInfoEventArgs e = new SetSupervisorMachineInfoEventArgs(enabledStation, _speed);
                    OnSetMachineInfoInvoked(this, e);
                    if (e.Cancel)
                        ris = 1;
                }
            }
            catch (Exception) {
                Log.Line(LogLevels.Error, "Supervisor.SetSupervisorMachineInfo", "SetSupervisorMachineInfo failed");
                ris = 1;
            }
            return ris;
        }

        public int ResetErrorRQS() {

            Log.Line(LogLevels.Debug, "Supervisor.ResetErrorRQS", "ResetErrorRQS");
            int ris = 0;
            try {
                SupervisorOperationInvokedEventArgs e = new SupervisorOperationInvokedEventArgs();
                OnResetErrorRQSInvoked(this, e);
                if (e.Cancel)
                    ris = 1;
            }
            catch (Exception) {
                Log.Line(LogLevels.Error, "Supervisor.ResetErrorRQS", "ResetErrorRQS failed");
                ris = 1;
            }
            return ris;
        }

        public int PrintActiveRecipe() {

            Log.Line(LogLevels.Debug, "Supervisor.PrintActiveRecipe", "PrintActiveRecipe");
            int ris = 0;
            try {
                OnPrintActiveRecipe(this, new EventArgs());
            }
            catch {
                Log.Line(LogLevels.Error, "Supervisor.PrintActiveRecipe", "PrintActiveRecipe failed");
                ris = 1;
            }
            return ris;
        }

        public int StartBatch(int saveType) {

            Log.Line(LogLevels.Debug, "Supervisor.StartBatch", "StartBatch");
            int ris = 0;
            try {
                StartBatchEventArgs e = new StartBatchEventArgs(SaveModeEnum.Production);
                OnStartBatchInvoked(this, e);
            }
            catch (Exception) {
                Log.Line(LogLevels.Error, "Supervisor.StartBatch", "StartBatch failed");
                ris = 1;
            }
            return ris;
        }

        public int StopBatch() {

            Log.Line(LogLevels.Debug, "Supervisor.StopBatch", "StopBatch");
            int ris = 0;
            try {
                OnStopBatchInvoked(this, new EventArgs());
            }
            catch (Exception) {
                Log.Line(LogLevels.Error, "Supervisor.StopBatch", "StopBatch failed");
                ris = 1;
            }
            return ris;
        }

        public int SetBatchId(int id)
        {
            Log.Line(LogLevels.Debug, "Supervisor.SetBatchId", "SetBatchId");
            int ris = 0;
            try
            {
                OnSetBatchIdInvoked(this, new SetBatchIdEventArgs(id));
            }
            catch (Exception)
            {
                Log.Line(LogLevels.Error, "Supervisor.SetBatchId", "SetBatchId failed");
                ris = 1;
            }
            return ris;
        }

        public int StartKnapp(int saveType, int currentTurn, int totalRoundsCount, int warmupRoundsCount) {

            Log.Line(LogLevels.Debug, "Supervisor.StartKnapp", "StartKnapp");
            int ris = 0;
            try {
                StartKnappEventArgs e = new StartKnappEventArgs((SaveModeEnum)saveType, currentTurn, totalRoundsCount, warmupRoundsCount);
                OnStartKnappInvoked(this, e);
            }
            catch (Exception) {
                Log.Line(LogLevels.Error, "Supervisor.StartKnapp", "StartKnapp failed");
                ris = 1;
            }
            return ris;
        }

        public int SetWorkingMode(int wm) {

            Log.Line(LogLevels.Debug, "Supervisor.SetWorkingMode", "SetWorkingMode");
            int ris = 0;
            try {
                SetSupervisorWorkingModeEventArgs e = new SetSupervisorWorkingModeEventArgs((SupervisorWorkingModeEnum)wm);
                OnSetSupervisorWorkingMode(this, e);
            }
            catch (Exception) {
                Log.Line(LogLevels.Error, "Supervisor.SetWorkingMode", "SetWorkingMode failed");
                ris = 1;
            }
            return ris;
        }

        #endregion

        protected void OnSetSupervisorOnTopInvoked(object sender, SupervisorOperationInvokedEventArgs e) {

            if (SetSupervisorOnTopInvoked != null)
                SetSupervisorOnTopInvoked(sender, e);
        }

        protected void OnSetSupervisorHideInvoked(object sender, SupervisorOperationInvokedEventArgs e) {

            if (SetSupervisorHideInvoked != null)
                SetSupervisorHideInvoked(sender, e);
        }

        protected void OnSetSupervisorActiveRecipeInvoked(object sender, SetSupervisorActiveRecipeEventArgs e) {

            if (SetSupervisorActiveRecipeInvoked != null)
                SetSupervisorActiveRecipeInvoked(sender, e);
        }

        protected void OnSetSupervisorActiveRecipeVersionInvoked(object sender, SetSupervisorActiveRecipeVersionEventArgs e)
        {
            if (SetSupervisorActiveRecipeVersionInvoked != null)
                SetSupervisorActiveRecipeVersionInvoked(sender, e);
        }

        protected void OnSetSupervisorActiveRecipeStatusInvoked(object sender, SetSupervisorActiveRecipeStatusEventArgs e)
        {
            if (SetSupervisorActiveRecipeStatusInvoked != null)
                SetSupervisorActiveRecipeStatusInvoked(sender, e);
        }

        protected void OnSetSupervisorActiveRecipeAllParamsInvoked(object sender, SetSupervisorActiveRecipeAllParamsEventArgs e)
        {

            if (SetSupervisorActiveRecipeAllParamsInvoked != null)
                SetSupervisorActiveRecipeAllParamsInvoked(sender, e);
        }

        protected void OnSetSupervisorLanguageCodeInvoked(object sender, SetSupervisorLanguageCodeEventArgs e) {

            if (SetSupervisorLanguageCodeInvoked != null)
                SetSupervisorLanguageCodeInvoked(sender, e);
        }

        protected void OnSetSupervisorMachineModeInvoked(object sender, SetSupervisorMachineModeEventArgs e) {

            if (SetSupervisorMachineModeInvoked != null)
                SetSupervisorMachineModeInvoked(sender, e);
        }

        protected void OnSetSupervisorPosInvoked(object sender, SetSupervisorPosEventArgs e) {

            if (SetSupervisorPosInvoked != null)
                SetSupervisorPosInvoked(sender, e);
        }

        protected void OnSetSupervisorUserLevelInvoked(object sender, SetSupervisorUserLevelEventArgs e) {

            if (SetSupervisorUserLevelInvoked != null)
                SetSupervisorUserLevelInvoked(sender, e);
        }

        protected void OnSetSupervisorDataBaseInvoked(object sender, SetSupervisorDataBaseEventArgs e)
        {

            if (SetSupervisorDataBaseInvoked != null)
                SetSupervisorDataBaseInvoked(sender, e);
        }

        protected void OnSetHMIIsAliveInvoked(object sender, EventArgs e) {

            if (SetHMIIsAliveInvoked != null)
                SetHMIIsAliveInvoked(sender, e);
        }

        protected void OnSetMachineInfoInvoked(object sender, SetSupervisorMachineInfoEventArgs e) {

            if (SetSupervisorMachineInfoInvoked != null)
                SetSupervisorMachineInfoInvoked(sender, e);
        }

        protected void OnResetErrorRQSInvoked(object sender, SupervisorOperationInvokedEventArgs e) {

            if (ResetErrorRQSInvoked != null)
                ResetErrorRQSInvoked(sender, e);
        }

        protected void OnPrintActiveRecipe(object sender, EventArgs e) {

            if (PrintActiveRecipeInvoked != null)
                PrintActiveRecipeInvoked(sender, e);
        }

        protected void OnStartBatchInvoked(object sender, StartBatchEventArgs e) {

            if (StartBatchInvoked != null)
                StartBatchInvoked(sender, e);
        }

        protected void OnStopBatchInvoked(object sender, EventArgs e) {

            if (StopBatchInvoked != null)
                StopBatchInvoked(sender, e);
        }

        protected void OnSetBatchIdInvoked(object sender, SetBatchIdEventArgs e)
        {
            if (SetBatchIdInvoked != null)
                SetBatchIdInvoked(sender, e);
        }

        protected void OnStartKnappInvoked(object sender, StartKnappEventArgs e) {

            if (StartKnappInvoked != null)
                StartKnappInvoked(sender, e);
        }

        protected void OnSetSupervisorWorkingMode(object sender, SetSupervisorWorkingModeEventArgs e) {

            if (SetSupervisorWorkingModeInvoked != null)
                SetSupervisorWorkingModeInvoked(sender, e);
        }
    }

    public class SupervisorOperationInvokedEventArgs : EventArgs {

        public bool Cancel { get; set; }

        public SupervisorOperationInvokedEventArgs() {

            Cancel = false;
        }
    }

    public class SetSupervisorActiveRecipeEventArgs : SupervisorOperationInvokedEventArgs {

        public Recipe ActiveRecipe { get; internal set; }
        public string RawRecipeXml { get; internal set; }

        public SetSupervisorActiveRecipeEventArgs(Recipe activeRecipe, string rawRecipeXml)
            : base() {

            ActiveRecipe = activeRecipe;
            RawRecipeXml = rawRecipeXml;
        }
    }

    public class SetSupervisorActiveRecipeVersionEventArgs : SupervisorOperationInvokedEventArgs
    {
        public int ActiveRecipeVersion { get; internal set; }

        public SetSupervisorActiveRecipeVersionEventArgs(int activeRecipeVersion) : base()
        {
            ActiveRecipeVersion = activeRecipeVersion;
        }
    }

    public class SetSupervisorActiveRecipeStatusEventArgs : SupervisorOperationInvokedEventArgs
    {
        public string ActiveRecipeStatus { get; internal set; }

        public SetSupervisorActiveRecipeStatusEventArgs(string activeRecipeStatus) : base()
        {
            ActiveRecipeStatus = activeRecipeStatus;
        }
    }

    public class SetSupervisorActiveRecipeAllParamsEventArgs : SupervisorOperationInvokedEventArgs
    {

        public Recipe ActiveRecipe { get; internal set; }
        public string RawRecipeXml { get; internal set; }
        public int ActiveRecipeVersion { get; internal set; }
        public string ActiveRecipeStatus { get; internal set; }

        public SetSupervisorActiveRecipeAllParamsEventArgs(Recipe activeRecipe, string rawRecipeXml, int activeRecipeVersion, string activeRecipeStatus)
            : base()
        {
            ActiveRecipe = activeRecipe;
            RawRecipeXml = rawRecipeXml;
            ActiveRecipeVersion = activeRecipeVersion;
            ActiveRecipeStatus = activeRecipeStatus;
        }
    }

    public class SetSupervisorLanguageCodeEventArgs : SupervisorOperationInvokedEventArgs {

        public string LanguageCode { get; internal set; }

        public SetSupervisorLanguageCodeEventArgs(string languageCode)
            : base() {

            LanguageCode = languageCode;
        }
    }

    public class SetSupervisorMachineModeEventArgs : SupervisorOperationInvokedEventArgs {

        public MachineModeEnum MachineMode { get; internal set; }

        public SetSupervisorMachineModeEventArgs(MachineModeEnum machineMode)
            : base() {

            MachineMode = machineMode;
        }
    }

    public class SetSupervisorUserLevelEventArgs : SupervisorOperationInvokedEventArgs {

        public UserLevelEnum UserLevel { get; internal set; }

        public SetSupervisorUserLevelEventArgs(UserLevelEnum userLevel)
            : base() {

            UserLevel = userLevel;
        }
    }

    public class SetSupervisorDataBaseEventArgs : SupervisorOperationInvokedEventArgs
    {

        public int DataBase { get; internal set; }

        public SetSupervisorDataBaseEventArgs(int dataBase)
            : base()
        {

            DataBase = dataBase;
        }
    }

    
    public class SetSupervisorPosEventArgs : SupervisorOperationInvokedEventArgs {

        public Rectangle ApplicationArea { get; internal set; }

        public SetSupervisorPosEventArgs(Rectangle applicationArea)
            : base() {

            ApplicationArea = applicationArea;
        }
    }

    public class SetSupervisorMachineInfoEventArgs : SupervisorOperationInvokedEventArgs {

        public bool[] StationEnabled { get; internal set; }
        public decimal Speed { get; internal set; }

        public SetSupervisorMachineInfoEventArgs(bool[] stationEnabled, decimal speed)
            : base() {

            StationEnabled = stationEnabled;
            Speed = speed;
        }
    }

    public class StartBatchEventArgs : SupervisorOperationInvokedEventArgs {

        public SaveModeEnum SaveType { get; private set; }

        public StartBatchEventArgs(SaveModeEnum saveType)
            : base() {

            SaveType = saveType;
        }
    }

    public class SetBatchIdEventArgs : SupervisorOperationInvokedEventArgs
    {
        public int Id { get; private set; }

        public SetBatchIdEventArgs(int id) : base()
        {
            Id = id;
        }
    }

    public class StartKnappEventArgs : SupervisorOperationInvokedEventArgs {

        public SaveModeEnum SaveType { get; private set; }
        public int CurrentTurn { get; private set; }
        public int TotalRoundsCount { get; private set; }
        public int WarmupRoundsCount { get; private set; }

        public StartKnappEventArgs(SaveModeEnum saveType, int currentTurn, int totalTurnsCount, int warmupTurnsCount)
            : base() {

            SaveType = saveType;
            CurrentTurn = currentTurn;
            TotalRoundsCount = totalTurnsCount;
            WarmupRoundsCount = warmupTurnsCount;
        }
    }

    public class SetSupervisorWorkingModeEventArgs : SupervisorOperationInvokedEventArgs {

        public SupervisorWorkingModeEnum SupervisorWorkingMode { get; private set; }

        public SetSupervisorWorkingModeEventArgs(SupervisorWorkingModeEnum supervisorWorkingMode) {

            SupervisorWorkingMode = supervisorWorkingMode;
        }
    }
}
