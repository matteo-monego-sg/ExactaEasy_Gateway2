namespace ExEyGateway
{
    /// <summary>
    /// These functions are remotely called (IPC) by the supervisor to execute actions on ARTIC.
    /// </summary>
    public interface IArticIpcLogic
    {
        /// <summary>
        /// 
        /// </summary>
        void HmiGetSupervisorPos();
        /// <summary>
        /// 
        /// </summary>
        void HmiSaveRecipeModification(string recipe);
        /// <summary>
        /// 
        /// </summary>
        void HmiSetSupervisorMode(int hmiMode);
        /// <summary>
        /// 
        /// </summary>
        void HmiSetOnTop();
        /// <summary>
        /// 
        /// </summary>
        void HmiSetSupIsAlive();
        /// <summary>
        /// 
        /// </summary>
        bool HmiSetSupIsAliveEx();
        /// <summary>
        /// 
        /// </summary>
        string HmiGetErrorString(int errorCode);
        /// <summary>
        /// 
        /// </summary>
        void HmiSupervisorButtonClicked(string buttonID);
        /// <summary>
        /// 
        /// </summary>
        void HmiSupervisorUIClosed(bool restart);
        /// <summary>
        /// 
        /// </summary>
        void HmiAuditMessage(int totalMessageCount, int partialMessageCount, string messageToAudit);
        /// <summary>
        /// 
        /// </summary>
        void HmiWaitForKnappStart();
        /// <summary>
        /// 
        /// </summary>
        void HmiSendResults(string results);
        /// <summary>
        /// 
        /// </summary>
        void HmiSetSupervisorBypassSendRecipe(bool bypass);
    }
}
