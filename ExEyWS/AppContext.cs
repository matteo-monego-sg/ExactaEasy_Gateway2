using ExEyGateway;
using System.Drawing;

namespace ExactaEasyEng
{
    public class AppContext 
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly AppContext EmptyContext = new AppContext();
        /// <summary>
        /// 
        /// </summary>
        public string CultureCode { get; private set; }
        public Rectangle AppClientRect { get; private set; }
        public UserLevelEnum UserLevel { get; private set; }
        public int DataBase { get; private set; }
        public Recipe ActiveRecipe { get; private set; }
        public int ActiveRecipeVersion { get; private set; }
        public RecipeStatusEnum ActiveRecipeStatus { get; private set; }
        public MachineModeEnum MachineMode { get; private set; }
        public bool[] EnabledStation { get; private set; }
        public SupervisorModeEnum SupervisorMode { get; private set; }
        public SupervisorWorkingModeEnum SupervisorWorkingMode { get; private set; }
        public bool IsBatchStarted { get; private set; }
        public int CurrentBatchId { get; private set; }
        public decimal MachineSpeed { get; private set; }
        public string CSVRotationParameters { get; private set; }

        AppContext() {
            CultureCode = "en"; // Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            AppClientRect = Rectangle.Empty;
            UserLevel = UserLevelEnum.None;
            DataBase = 0;
            ActiveRecipe = null;
            ActiveRecipeVersion = -1;
            ActiveRecipeStatus = RecipeStatusEnum.Unknown;
            MachineMode = MachineModeEnum.Unknown;
            SupervisorMode = SupervisorModeEnum.Unknown;
            SupervisorWorkingMode = SupervisorWorkingModeEnum.Normal;

            //03.07 GR abilitazione per default di tutte le station
            //l'HMI comunicherà quelle correttamente abilitate
            EnabledStation = new bool[32];
            for (int i = 0; i < 32; i++)
                EnabledStation[i] = true;

            MachineSpeed = 0;
            CSVRotationParameters = null;
        }

        public AppContext(
            string cultureCode, 
            Rectangle appClientRect, 
            UserLevelEnum userLevel, 
            Recipe activeRecipe, 
            int activeRecipeVersion, 
            RecipeStatusEnum activeRecipeStatus, 
            MachineModeEnum machineMode, 
            bool[] enabledStation, 
            decimal machineSpeed, 
            SupervisorModeEnum supervisorMode, 
            SupervisorWorkingModeEnum supervisorWorkingMode, 
            int dataBase, 
            bool isBatchStarted, 
            int currentBatchId, 
            string csvRotParameters) {

            CultureCode = cultureCode;
            AppClientRect = appClientRect;
            UserLevel = userLevel;
            DataBase = dataBase;
            ActiveRecipe = activeRecipe;
            ActiveRecipeVersion = activeRecipeVersion;
            ActiveRecipeStatus = activeRecipeStatus;
            MachineMode = machineMode;
            EnabledStation = enabledStation;
            MachineSpeed = machineSpeed;
            SupervisorMode = supervisorMode;
            SupervisorWorkingMode = SupervisorWorkingMode;
            IsBatchStarted = isBatchStarted;
            CurrentBatchId = currentBatchId;
            CSVRotationParameters = csvRotParameters;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsContextComplete() 
        {
            return CultureCode != string.Empty
                && !AppClientRect.Equals(default(ExactaEasyAppArea))
                && UserLevel > 0
                && ActiveRecipe != null
                && MachineMode > 0
                && EnabledStation != null
                && EnabledStation.Length > 0;
        }

        //public bool IsStationEnabled(int IdNode, int IdStation) {
            
            //// da nodo-stazione ad indice assoluto
            //if (AppEngine.Current == null || AppEngine.Current.CurrentContext == null || AppEngine.Current.CurrentContext.EnabledStation == null ||
            //    AppEngine.Current.MachineConfiguration==null || AppEngine.Current.MachineConfiguration.StationSettings==null)
            //    return false;
            //int stationAbsId = -1;
            //for (int sAbsId = 0; sAbsId < AppEngine.Current.MachineConfiguration.StationSettings.Count; sAbsId++) {
            //    StationSetting statSet = AppEngine.Current.MachineConfiguration.StationSettings[sAbsId];
            //    if (IdNode == statSet.Node && IdStation == statSet.Id) {
            //        stationAbsId = sAbsId;
            //        break;
            //    }
            //}
            ////try {
            ////    if (
            ////        (AppEngine.Current.CurrentContext.EnabledStation.Length > IdStation) &&
            ////        (AppEngine.Current.CurrentContext.EnabledStation[IdStation]))
            ////        return true;
            ////    return false;
            ////}
            ////catch {
            ////    return false;
            ////}
        //}

        public bool IsCameraRecipeEnabled(int IdCamera) {
            try {
                if ((AppEngine.Current != null) &&
                    (AppEngine.Current.CurrentContext != null) &&
                    (AppEngine.Current.CurrentContext.ActiveRecipe != null)) {
                    Cam cam = AppEngine.Current.CurrentContext.ActiveRecipe.Cams.Find(c => c.Id == IdCamera);
                    if (cam!=null && cam.Enabled)
                        return true;
                }
                return false;
            }
            catch {
                return false;
            }
        }

        //public bool IsContextCompleteWithoutRecipe() {

        //    return CultureCode != "" && AppClientRect != Rectangle.Empty && UserLevel > 0 && MachineMode > 0 && EnabledStation != null && EnabledStation.Length > 0;
        //}
    }
}
