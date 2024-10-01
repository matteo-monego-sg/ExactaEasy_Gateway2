using System.ServiceModel;
using System.ServiceModel.Web;

namespace ExactaEasyEng
{

    [ServiceContract]
    public interface ISupervisor
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "SetSupervisorOnTop", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        int SetSupervisorOnTop();

        [OperationContract]
        [WebInvoke(UriTemplate = "SetSupervisorHide", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        int SetSupervisorHide();

        [OperationContract]
        [WebInvoke(UriTemplate = "SetSupervisorPos", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        int SetSupervisorPos(int startX, int startY, int sizeX, int sizeY);

        [OperationContract]
        [WebInvoke(UriTemplate = "SetActiveRecipe", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        int SetActiveRecipe(string recipeName, string recipeXml);

        [OperationContract]
        [WebInvoke(UriTemplate = "SetActiveRecipeVersion", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        int SetActiveRecipeVersion(int recipeVersion);

        [OperationContract]
        [WebInvoke(UriTemplate = "SetActiveRecipeAllParams", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        int SetActiveRecipeAllParams(string recipeName, string recipeXml, int recipeVersion, string recipeStatus);

        [OperationContract]
        [WebInvoke(UriTemplate = "SetUserLevel", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        int SetUserLevel(int userLevel);

        [OperationContract]
        [WebInvoke(UriTemplate = "SetDataBase", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        int SetDataBase(int dataBase);

        [OperationContract]
        [WebInvoke(UriTemplate = "SetLanguage", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        int SetLanguage(string languageCode);

        [OperationContract]
        [WebInvoke(UriTemplate = "SetMachineMode", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        int SetMachineMode(int mode);

        [OperationContract]
        [WebInvoke(UriTemplate = "SetHMIIsAlive", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        int SetHMIIsAlive(int mode);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetErrorString", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        string GetErrorString(int errrorCode);

        [OperationContract]
        [WebInvoke(UriTemplate = "SetSupervisorMachineInfo", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        int SetSupervisorMachineInfo(string stationInfo, string machineSpeed);

        [OperationContract]
        [WebInvoke(UriTemplate = "ResetErrorRQS", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        int ResetErrorRQS();

        [OperationContract]
        [WebInvoke(UriTemplate = "PrintActiveRecipe", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        int PrintActiveRecipe();

        [OperationContract]
        [WebInvoke(UriTemplate = "StartBatch", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        int StartBatch(int saveType);

        [OperationContract]
        [WebInvoke(UriTemplate = "StopBatch", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        int StopBatch();

        [OperationContract]
        [WebInvoke(UriTemplate = "SetBatchId", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        int SetBatchId(int id);

        [OperationContract]
        [WebInvoke(UriTemplate = "StartKnapp", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        int StartKnapp(int saveType, int currentTurn, int totalTurnsCount, int warmupTurnsCount);

        [OperationContract]
        [WebInvoke(UriTemplate = "SetWorkingMode", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        int SetWorkingMode(int workingMode);

    }
}
