using ExEyGateway;

namespace ExactaEasyEng
{
    /// <summary>
    /// 
    /// </summary>
    public interface IExactaEasyIpcLogic
    {
        /// <summary>
        /// 
        /// </summary>
        int SetSupervisorOnTop();
        /// <summary>
        /// 
        /// </summary>
        int SetSupervisorHide();
        /// <summary>
        /// 
        /// </summary>
        int SetSupervisorPos(int startX, int startY, int sizeX, int sizeY);
        /// <summary>
        /// 
        /// </summary>
        int SetActiveRecipe(string recipeName, string recipeXml);
        /// <summary>
        /// 
        /// </summary>
        int SetActiveRecipeVersion(int recipeVersion);
        /// <summary>
        /// 
        /// </summary>
        int SetActiveRecipeStatus(RecipeStatusEnum recipeStatus);
        /// <summary>
        /// 
        /// </summary>
        int SetActiveRecipeAllParams(string recipeName, string recipeXml, int recipeVersion, RecipeStatusEnum resipeStatus);
        /// <summary>
        /// 
        /// </summary>
        int SetUserLevel(int userLevel);
        /// <summary>
        /// 
        /// </summary>
        int SetDatabase(int database);
        /// <summary>
        /// 
        /// </summary>
        int SetLanguage(string languageCode);
        /// <summary>
        /// 
        /// </summary>
        int SetMachineMode(int mode);
        /// <summary>
        /// 
        /// </summary>
        int SetHMIIsAlive(int mode);
        /// <summary>
        /// 
        /// </summary>
        string GetErrorString(int errorCode);
        /// <summary>
        /// 
        /// </summary>
        int SetSupervisorMachineInfo(string stationInfo, string machineSpeed);
        /// <summary>
        /// 
        /// </summary>
        int ResetErrorRQS();
        /// <summary>
        /// 
        /// </summary>
        int PrintActiveRecipe();
        /// <summary>
        /// 
        /// </summary>
        int StartBatch(int saveType);
        /// <summary>
        /// 
        /// </summary>
        int StopBatch();
        /// <summary>
        /// 
        /// </summary>
        int SetBatchId(int id);
        /// <summary>
        /// 
        /// </summary>
        int StartKnapp(int saveType, int currentTurn, int totalTurnsCount, int warmupTurnsCount);
        /// <summary>
        /// 
        /// </summary>
        int SetWorkingMode(int workingMode);
    }
}
