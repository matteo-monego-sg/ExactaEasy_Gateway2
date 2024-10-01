using ExEyGateway;
using System;
using System.Drawing;

namespace ExactaEasyEng
{
    /// <summary>
    /// 
    /// </summary>
    public class ExactaEasyOperationInvokedEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public bool Cancel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ExactaEasyOperationInvokedEventArgs()
        {
            Cancel = false;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SetExactaEasyActiveRecipeEventArgs : ExactaEasyOperationInvokedEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public Recipe ActiveRecipe { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string RecipeXml { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="activeRecipe"></param>
        /// <param name="rawRecipeXml"></param>

        public SetExactaEasyActiveRecipeEventArgs(Recipe activeRecipe, string recipeXml) : base()
        {
            ActiveRecipe = activeRecipe;
            RecipeXml = recipeXml;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SetExactaEasyActiveRecipeVersionEventArgs : ExactaEasyOperationInvokedEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public int ActiveRecipeVersion { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public SetExactaEasyActiveRecipeVersionEventArgs(int activeRecipeVersion) : base()
        {
            ActiveRecipeVersion = activeRecipeVersion;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SetExactaEasyActiveRecipeStatusEventArgs : ExactaEasyOperationInvokedEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public RecipeStatusEnum ActiveRecipeStatus { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public SetExactaEasyActiveRecipeStatusEventArgs(RecipeStatusEnum activeRecipeStatus) : base()
        {
            ActiveRecipeStatus = activeRecipeStatus;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SetExactaEasyActiveRecipeAllParamsEventArgs : ExactaEasyOperationInvokedEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public Recipe ActiveRecipe { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string RecipeName { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string RecipeXml { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int RecipeVersion { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public RecipeStatusEnum ActiveRecipeStatus { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public SetExactaEasyActiveRecipeAllParamsEventArgs(Recipe activeRecipe, string recipeName, string recipeXml, int recipeVersion, RecipeStatusEnum activeRecipeStatus) : base()
        {
            ActiveRecipe = activeRecipe;
            RecipeName = recipeName;
            RecipeXml = recipeXml;
            RecipeVersion = recipeVersion;
            ActiveRecipeStatus = activeRecipeStatus;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SetExactaEasyLanguageCodeEventArgs : ExactaEasyOperationInvokedEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public string LanguageCode { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="languageCode"></param>
        public SetExactaEasyLanguageCodeEventArgs(string languageCode) : base()
        {
            LanguageCode = languageCode;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SetExactaEasyMachineModeEventArgs : ExactaEasyOperationInvokedEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public MachineModeEnum MachineMode { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public SetExactaEasyMachineModeEventArgs(MachineModeEnum machineMode) : base()
        { 
            MachineMode = machineMode;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SetExactaEasyUserLevelEventArgs : ExactaEasyOperationInvokedEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public UserLevelEnum UserLevel { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public SetExactaEasyUserLevelEventArgs(UserLevelEnum userLevel) : base()
        {
            UserLevel = userLevel;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SetExactaEasyDatabaseEventArgs : ExactaEasyOperationInvokedEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public int Database { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public SetExactaEasyDatabaseEventArgs(int database) : base()
        {
            Database = database;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SetExactaEasyPosEventArgs : ExactaEasyOperationInvokedEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public Rectangle ApplicationArea { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public SetExactaEasyPosEventArgs(Rectangle applicationArea) : base()
        {
            ApplicationArea = applicationArea;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SetExactaEasyMachineInfoEventArgs : ExactaEasyOperationInvokedEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public bool[] StationEnabled { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Speed { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stationEnabled"></param>
        /// <param name="speed"></param>
        public SetExactaEasyMachineInfoEventArgs(bool[] stationEnabled, decimal speed) : base()
        {
            StationEnabled = stationEnabled;
            Speed = speed;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class StartBatchEventArgs : ExactaEasyOperationInvokedEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public SaveModeEnum SaveType { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public StartBatchEventArgs(SaveModeEnum saveType) : base()
        {
            SaveType = saveType;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class StopBatchEventArgs : ExactaEasyOperationInvokedEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public SaveModeEnum SaveType { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public StopBatchEventArgs(SaveModeEnum saveType) : base()
        {
            SaveType = saveType;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SetBatchIdEventArgs : ExactaEasyOperationInvokedEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public SetBatchIdEventArgs(int id) : base()
        {
            Id = id;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class StartKnappEventArgs : ExactaEasyOperationInvokedEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public SaveModeEnum SaveType { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int CurrentTurn { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int TotalRoundsCount { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int WarmupRoundsCount { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public StartKnappEventArgs(SaveModeEnum saveType, int currentTurn, int totalTurnsCount, int warmupTurnsCount) : base()
        {
            SaveType = saveType;
            CurrentTurn = currentTurn;
            TotalRoundsCount = totalTurnsCount;
            WarmupRoundsCount = warmupTurnsCount;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SetExactaEasyWorkingModeEventArgs : ExactaEasyOperationInvokedEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public SupervisorWorkingModeEnum SupervisorWorkingMode { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="supervisorWorkingMode"></param>
        public SetExactaEasyWorkingModeEventArgs(SupervisorWorkingModeEnum supervisorWorkingMode)
        { 
            SupervisorWorkingMode = supervisorWorkingMode;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SetExactaEasyCsvRotationEventArgs : ExactaEasyOperationInvokedEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public string Csv { get; private set; }
        /// <summary>
        /// 
        public SetExactaEasyCsvRotationEventArgs(string csv)
        {
            Csv = csv;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class AppEngineContextChangedEventArgs : EventArgs
    {
        public ContextChangesEnum ContextChanges { get; private set; }
        public bool ApplyParameters { get; private set; }
        public AppEngineContextChangedEventArgs(ContextChangesEnum contextChanges, bool applyParams)
        {
            ContextChanges = contextChanges;
            ApplyParameters = applyParams;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class AppEngineGatewayCommunicationStatusEventArgs : EventArgs 
    {
        public GatewayCommunicationStatusEnum GatewayCommunicationStatus { get; private set; }

        public AppEngineGatewayCommunicationStatusEventArgs(GatewayCommunicationStatusEnum commStatus)
        {
            GatewayCommunicationStatus = commStatus;
        }
    }
}
