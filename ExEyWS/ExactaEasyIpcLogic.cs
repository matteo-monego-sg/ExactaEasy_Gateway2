using ExEyGateway;
using SPAMI.Util.Logger;
using System;
using System.Drawing;
using System.Globalization;

namespace ExactaEasyEng
{
    /// <summary>
    /// Implements the logic of remote execution control from Artic.
    /// </summary>
    public class ExactaEasyIpcLogic : IExactaEasyIpcLogic
    {
        /// <summary>
        /// Return value of a function in case of success.
        /// </summary>
        private const int EXEC_OK = 0;
        /// <summary>
        /// Return value of a function in case of failure.
        /// </summary>
        private const int EXEC_KO = 1;
        /// <summary>
        /// 
        /// </summary>
        private AppEngine _appEngine;
        /// <summary>
        /// 
        /// </summary>
        public ExactaEasyIpcLogic(AppEngine appEngine)
        {
            _appEngine = appEngine;
        }
        /// <summary>
        /// 
        /// </summary>
        public string GetErrorString(int errorCode)
        {
            Log.Line(LogLevels.Debug, "ExactaEasyIpcLogic.GetSupervisorErrorString", "GetSupervisorErrorString");
            return $"E: {errorCode}";
        }
        /// <summary>
        /// 
        /// </summary>
        public int PrintActiveRecipe()
        {
            Log.Line(LogLevels.Debug, "ExactaEasyIpcLogic.PrintActiveRecipe", "PrintActiveRecipe");
            try
            {
                _appEngine.OnPrintExactaEasyActiveRecipeRequested(new ExactaEasyOperationInvokedEventArgs());
                return EXEC_OK;
            }
            catch(Exception ex)
            {
                Log.Line(LogLevels.Error, "ExactaEasyIpcLogic.PrintActiveRecipe", $"PrintActiveRecipe failed: {ex.Message}");
                return EXEC_KO;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ResetErrorRQS()
        {
            Log.Line(LogLevels.Debug, "ExactaEasyIpcLogic.ResetErrorRQS", "ResetErrorRQS");
            try
            {
                var e = new ExactaEasyOperationInvokedEventArgs();
                _appEngine.OnResetExactaEasyErrorRQSRequested(e);
                if (e.Cancel) return EXEC_KO;
                return EXEC_OK;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "ExactaEasyIpcLogic.ResetErrorRQS", $"ResetErrorRQS failed: {ex.Message}");
                return EXEC_KO;
            }
        }
        /// <summary>
        /// Sets the current recipe given the name and the path of the recipe's XML file.
        /// </summary>
        public int SetActiveRecipe(string recipeName, string recipeXml)
        {
            Log.Line(LogLevels.Debug, "ExactaEasyIpcLogic.SetActiveRecipe", "SetActiveRecipe");
            SetExactaEasyActiveRecipeEventArgs eeArgs;
            
            try
            {
                var activeRecipe = Recipe.LoadFromXml(recipeXml);
                activeRecipe.RecipeName = recipeName;

                eeArgs = new SetExactaEasyActiveRecipeEventArgs(activeRecipe, recipeXml);
                _appEngine.OnSetExactaEasyActiveRecipeRequested(eeArgs);
              
                if (eeArgs.Cancel) return EXEC_KO;
                return EXEC_OK;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "ExactaEasyIpcLogic.SetActiveRecipe", $"SetActiveRecipe failed: {ex.Message}");
                eeArgs = new SetExactaEasyActiveRecipeEventArgs(null, recipeXml);
                _appEngine.OnSetExactaEasyActiveRecipeRequested(eeArgs);
                return EXEC_KO;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        //public int SetActiveRecipeFromFile(string recipePath)
        //{
        //    Log.Line(LogLevels.Debug, "ExactaEasyIpcLogic.SetActiveRecipeFromFile", "SetActiveRecipeFromFile");
        //    SetExactaEasyActiveRecipeEventArgs eeArgs;
        //    var recipeXml = string.Empty;

        //    try
        //    {
        //        using (var reader = new StreamReader(recipePath, Encoding.UTF8, false, 4096))
        //            recipeXml = reader.ReadToEnd();
        //        var activeRecipe = Recipe.LoadFromXml(recipeXml);
        //        activeRecipe.RecipeName = Path.GetFileNameWithoutExtension(recipePath);

        //        eeArgs = new SetExactaEasyActiveRecipeEventArgs(activeRecipe, recipeXml);
        //        _appEngine.OnSetExactaEasyActiveRecipeRequested(eeArgs);

        //        if (eeArgs.Cancel) return EXEC_KO;
        //        return EXEC_OK;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Line(LogLevels.Error, "ExactaEasyIpcLogic.SetActiveRecipeFromFile", $"SetActiveRecipeFromFile failed: {ex.Message}");
        //        eeArgs = new SetExactaEasyActiveRecipeEventArgs(null, recipeXml);
        //        _appEngine.OnSetExactaEasyActiveRecipeRequested(eeArgs);
        //        return EXEC_KO;
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        public int SetActiveRecipeAllParams(string recipeName, string recipeXml, int recipeVersion, RecipeStatusEnum recipeStatus)
        {
            Log.Line(LogLevels.Debug, "ExactaEasyIpcLogic.SetActiveRecipeAllParams", "SetActiveRecipeAllParams");
            try
            {
                var activeRecipe = Recipe.LoadFromXml(recipeXml);
                activeRecipe.RecipeName = recipeName;

                var e = new SetExactaEasyActiveRecipeAllParamsEventArgs(activeRecipe, recipeName, recipeXml, recipeVersion, recipeStatus);
                _appEngine.OnSetExactaEasyActiveRecipeAllParamsRequested(e);
               
                if (e.Cancel) return EXEC_KO;
                return EXEC_OK;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "ExactaEasyIpcLogic.SetActiveRecipeAllParams", $"SetActiveRecipeAllParams failed: {ex.Message}");
                var e = new SetExactaEasyActiveRecipeAllParamsEventArgs(null, null, recipeXml, -1, RecipeStatusEnum.Unknown);
                _appEngine.OnSetExactaEasyActiveRecipeAllParamsRequested(e);
                return EXEC_KO;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SetActiveRecipeStatus(RecipeStatusEnum recipeStatus)
        {
            Log.Line(LogLevels.Debug, "ExactaEasyIpcLogic.SetActiveRecipeStatus", "SetActiveRecipeStatus");
            try
            {
                var e = new SetExactaEasyActiveRecipeStatusEventArgs(recipeStatus);
                _appEngine.OnSetExactaEasyActiveRecipeStatusRequested(e);

                if (e.Cancel) return EXEC_KO;
                return EXEC_OK;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "ExactaEasyIpcLogic.SetActiveRecipeStatus", $"SetActiveRecipeStatus failed: {ex.Message}");
                return EXEC_KO;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SetActiveRecipeVersion(int recipeVersion)
        {
            Log.Line(LogLevels.Debug, "ExactaEasyIpcLogic.SetActiveRecipeVersion", "SetActiveRecipeVersion");
            try
            {
                var e = new SetExactaEasyActiveRecipeVersionEventArgs(recipeVersion);
                _appEngine.OnSetExactaEasyActiveRecipeVersionRequested(e);

                if (e.Cancel) return EXEC_KO;
                return EXEC_OK;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "ExactaEasyIpcLogic.SetActSetActiveRecipeVersioniveRecipe", $"SetActiveRecipeVersion failed: {ex.Message}");
                return EXEC_KO;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SetBatchId(int id)
        {
            Log.Line(LogLevels.Debug, "ExactaEasyIpcLogic.SetBatchId", "SetBatchId");
            try
            {
                var e = new SetBatchIdEventArgs(id);
                _appEngine.OnSetBatchIdRequested(e);

                if (e.Cancel) return EXEC_KO;
                return EXEC_OK;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "ExactaEasyIpcLogic.SetBatchId", $"SetBatchId failed: {ex.Message}");
                return EXEC_KO;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SetDatabase(int database)
        {
            Log.Line(LogLevels.Debug, "ExactaEasyIpcLogic.SetDatabase", "SetDatabase");

            try
            {
                var e = new SetExactaEasyDatabaseEventArgs(database);
                _appEngine.OnSetExactaEasyDatabaseRequested(e);

                if (e.Cancel) return EXEC_KO;
                return EXEC_OK;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "ExactaEasyIpcLogic.SetDatabase", $"SetDatabase failed: {ex.Message}");
                return EXEC_KO;
            }
        }
        /// <summary>
        /// Nevermind the useless parameter: it's from the old code.
        /// </summary>
        public int SetHMIIsAlive(int mode)
        {
            Log.Line(LogLevels.Debug, "ExactaEasyIpcLogic.SetHMIIsAlive", "SetHMIIsAlive");
            try
            {
                var e = new ExactaEasyOperationInvokedEventArgs();
                _appEngine.OnSetHMIIsAliveInvoked(e);

                if (e.Cancel) return EXEC_KO;
                return EXEC_OK;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "ExactaEasyIpcLogic.SetHMIIsAlive", $"SetHMIIsAlive failed: {ex.Message}");
                return EXEC_KO;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SetLanguage(string languageCode)
        {
            Log.Line(LogLevels.Debug, "ExactaEasyIpcLogic.SetLanguage", "SetLanguage");
            try
            {
                var e = new SetExactaEasyLanguageCodeEventArgs(languageCode);
                _appEngine.OnSetExactaEasyLanguageCodeRequested(e);

                if (e.Cancel) return EXEC_KO;
                return EXEC_OK;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "ExactaEasyIpcLogic.SetLanguage", $"SetLanguage failed: {ex.Message}");
                return EXEC_KO;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SetMachineMode(int mode)
        {
            Log.Line(LogLevels.Debug, "ExactaEasyIpcLogic.SetMachineMode", "SetMachineMode");
            try
            {
                var e = new SetExactaEasyMachineModeEventArgs((MachineModeEnum)mode);
                _appEngine.OnSetExactaEasyMachineModeRequested(e);

                if (e.Cancel) return EXEC_KO;
                return EXEC_OK;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "ExactaEasyIpcLogic.SetMachineMode", $"SetMachineMode failed: {ex.Message}");
                return EXEC_KO;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SetSupervisorPos(int startX, int startY, int sizeX, int sizeY)
        {
            Log.Line(LogLevels.Debug, "ExactaEasyIpcLogic.SetSupervisorArea", "SetSupervisorPos");
            try
            {
                var e = new SetExactaEasyPosEventArgs(
                    new Rectangle(startX, startY, sizeX, sizeY));
                _appEngine.OnSetExactaEasyAreaRequested(e);

                if (e.Cancel) return EXEC_KO;
                return EXEC_OK;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "ExactaEasyIpcLogic.SetSupervisorArea", $"SetSupervisorArea failed: {ex.Message}");
                return EXEC_KO;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SetSupervisorHide()
        {
            Log.Line(LogLevels.Debug, "ExactaEasyIpcLogic.SetSupervisorHide", "SetSupervisorHide");
            try
            {
                var e = new ExactaEasyOperationInvokedEventArgs();
                _appEngine.OnHideExactaEasyRequested(e);

                if (e.Cancel) return EXEC_KO;
                return EXEC_OK;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "ExactaEasyIpcLogic.SetSupervisorHide", $"SetSupervisorHide failed: {ex.Message}");
                return EXEC_KO;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SetSupervisorMachineInfo(string stationInfo, string machineSpeed)
        {
            Log.Line(LogLevels.Debug, "ExactaEasyIpcLogic.SetSupervisorMachineInfo", "SetSupervisorMachineInfo");

            try
            {
                string[] info = stationInfo.Split(new char[] { ';' });
                if (info.Length > 0)
                {
                    bool[] enabledStation = new bool[info.Length];
                    for (int i = AppEngine.Current.MachineConfiguration.FirstStationIndex; i < info.Length; i++)
                        enabledStation[i - AppEngine.Current.MachineConfiguration.FirstStationIndex] = (info[i] == "1");
                    // Old code didn't give a slap about the result of the TryParse, code executed anyway with machineSpeed = 0.
                    decimal.TryParse(
                        machineSpeed,
                        NumberStyles.AllowDecimalPoint,
                        CultureInfo.GetCultureInfo("en-US"), out decimal speed);
                  
                    var e = new SetExactaEasyMachineInfoEventArgs(enabledStation, speed);
                    _appEngine.OnSetExactaEasyMachineInfoRequested(e);

                    if (e.Cancel) return EXEC_KO;
                    return EXEC_OK;
                }
                return EXEC_OK;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "ExactaEasyIpcLogic.SetSupervisorMachineInfo", $"SetSupervisorMachineInfo failed: {ex.Message}");
                return EXEC_KO;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SetSupervisorOnTop()
        {
            Log.Line(LogLevels.Debug, "ExactaEasyIpcLogic.SetSupervisorOnTop", "SetSupervisorOnTop");
            try
            {
                var e = new ExactaEasyOperationInvokedEventArgs();
                _appEngine.OnSetExactaEasyOnTopRequested(e);

                if (e.Cancel) return EXEC_KO;
                return EXEC_OK;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "ExactaEasyIpcLogic.SetSupervisorOnTop", $"SetSupervisorOnTop failed: {ex.Message}");
                return EXEC_KO;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SetUserLevel(int userLevel)
        {
            Log.Line(LogLevels.Debug, "ExactaEasyIpcLogic.SetUserLevel", "SetUserLevel");
            try
            {
                var e = new SetExactaEasyUserLevelEventArgs((UserLevelEnum)userLevel);
                _appEngine.OnSetExactaEasyUserLevelRequested(e);

                if (e.Cancel) return EXEC_KO;
                return EXEC_OK;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "ExactaEasyIpcLogic.SetUserLevel", $"SetUserLevel failed : {ex.Message}");
                return EXEC_KO;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SetWorkingMode(int workingMode)
        {
            Log.Line(LogLevels.Debug, "ExactaEasyIpcLogic.SetWorkingMode", "SetWorkingMode");
            try
            {
                var e = new SetExactaEasyWorkingModeEventArgs((SupervisorWorkingModeEnum)workingMode);
                _appEngine.OnSetExactaEasyWorkingModeRequested(e);

                if (e.Cancel) return EXEC_KO;
                return EXEC_OK;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "ExactaEasyIpcLogic.SetWorkingMode", $"SetWorkingMode failed: {ex.Message}");
                return EXEC_KO;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SetCSVRotationParameters(string csv)
        {
            Log.Line(LogLevels.Debug, "ExactaEasyIpcLogic.SetCSVRotationParameters", "SetCSVRotationParameters");
            try
            {
                var e = new SetExactaEasyCsvRotationEventArgs(csv);
                _appEngine.OnSetCsvRotationParamsRequested(e);

                if (e.Cancel) return EXEC_KO;
                return EXEC_OK;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "ExactaEasyIpcLogic.SetCSVRotationParameters", $"SetCSVRotationParameters failed: {ex.Message}");
                return EXEC_KO;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int StartBatch(int saveType)
        {
            Log.Line(LogLevels.Debug, "ExactaEasyIpcLogic.StartBatch", "StartBatch");
            try
            {
                var e = new StartBatchEventArgs(SaveModeEnum.Production);
                _appEngine.OnStartBatchRequested(e);

                if (e.Cancel) return EXEC_KO;
                return EXEC_OK;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "ExactaEasyIpcLogic.StartBatch", $"StartBatch failed: {ex.Message}");
                return EXEC_KO;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int StartKnapp(int saveType, int currentTurn, int totalTurnsCount, int warmupTurnsCount)
        {
            Log.Line(LogLevels.Debug, "ExactaEasyIpcLogic.StartKnapp", "StartKnapp");
            try
            {
                var e = new StartKnappEventArgs((SaveModeEnum)saveType, currentTurn, totalTurnsCount, warmupTurnsCount);
                _appEngine.OnStartKnappRequested(e);

                if (e.Cancel) return EXEC_KO;
                return EXEC_OK;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "ExactaEasyIpcLogic.StartKnapp", $"StartKnapp failed: {ex.Message}");
                return EXEC_KO;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int StopBatch()
        {
            Log.Line(LogLevels.Debug, "ExactaEasyIpcLogic.StopBatch", "StopBatch");
            try
            {
                var e = new StopBatchEventArgs(SaveModeEnum.Unknown);
                _appEngine.OnStopBatchRequested(e);

                if (e.Cancel) return EXEC_KO;
                return EXEC_OK;
            }
            catch (Exception ex)
            {
                Log.Line(LogLevels.Error, "ExactaEasyIpcLogic.StopBatch", $"StopBatch failed: {ex.Message}");
                return EXEC_KO;
            }
        }
    }
}
