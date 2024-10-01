using SPAMI.Util.Logger;
using System;

namespace ExactaEasyEng
{
    /// <summary>
    /// Interface describing all the possible ExactaEasy events that can be raised by an IPC call.
    /// </summary>
    public interface IExactaEasyIpcEvents
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<AppEngineContextChangedEventArgs> AppEngineContextChangeError;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<ExactaEasyOperationInvokedEventArgs> SetExactaEasyOnTopRequested;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<ExactaEasyOperationInvokedEventArgs> HideExactaEasyRequested;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<SetExactaEasyActiveRecipeEventArgs> SetExactaEasyActiveRecipeRequested;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<SetExactaEasyActiveRecipeVersionEventArgs> SetExactaEasyActiveRecipeVersionRequested;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<SetExactaEasyActiveRecipeAllParamsEventArgs> SetExactaEasyActiveRecipeAllParamsRequested;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<SetExactaEasyLanguageCodeEventArgs> SetExactaEasyLanguageCodeRequested;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<SetExactaEasyMachineModeEventArgs> SetExactaEasyMachineModeRequested;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<SetExactaEasyPosEventArgs> SetExactaEasyAreaRequested;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<SetExactaEasyUserLevelEventArgs> SetExactaEasyUserLevelRequested;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<SetExactaEasyDatabaseEventArgs> SetExactaEasyDatabaseRequested;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<ExactaEasyOperationInvokedEventArgs> SetHMIIsAliveInvoked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<SetExactaEasyMachineInfoEventArgs> SetExactaEasyMachineInfoRequested;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<ExactaEasyOperationInvokedEventArgs> ResetExactaEasyErrorRQSRequested;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<ExactaEasyOperationInvokedEventArgs> PrintExactaEasyActiveRecipeRequested;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<StartBatchEventArgs> StartBatchRequested;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<StopBatchEventArgs> StopBatchRequested;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<SetBatchIdEventArgs> SetBatchIdRequested;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<StartKnappEventArgs> StartKnappRequested;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<SetExactaEasyWorkingModeEventArgs> SetExactaEasyWorkingModeRequested;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<MessageEventArgs> NetworkInterfaceCardStatus;
    }
}
