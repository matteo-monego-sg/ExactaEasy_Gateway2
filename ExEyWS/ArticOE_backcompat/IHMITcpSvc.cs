using System.ServiceModel;

namespace ExactaEasyEng
{
    /// <summary>
    /// ----------------------------------------------------------------------
    /// NOTE: THIS INTERFACE IS KEPT ONLY FOR BACK-COMPATIBILITY WITH ARTICOE!
    /// ----------------------------------------------------------------------
    /// ArticOE is still using the ExEyGateway control to connect with ExactaEasy.
    /// ArticOE does not know that the underlying connection is not using WCF anymore,
    /// so it still requires this interface.
    /// </summary>
    [ServiceContract]
    public interface IHMITcpSvc {

        [OperationContract]
        string GetSupervisorPos();

        [OperationContract(IsOneWay=true)]
        void SaveRecipeModification(string recipe);

        [OperationContract(IsOneWay=true)]
        void SetSupervisorMode(int hmiMode);

        [OperationContract(IsOneWay=true)]
        void SetHMIOnTop();
        
        [OperationContract(IsOneWay=true)]
        void SetSupIsAlive();

        [OperationContract]
        string GetErrorString(int errorCode);

        [OperationContract(IsOneWay=true)]
        void SupervisorButtonClicked(string buttonID);

        [OperationContract]
        int AuditMessage(int totalMessageCount, int partialMessageCount, string messageToAudit);

        [OperationContract]
        void WaitForKnappStart();

        [OperationContract(IsOneWay = true)]
        void SendResults(string results);

        [OperationContract(IsOneWay = true)]
        void SetBatchId(int id);

        //[OperationContract(IsOneWay = true)]
        //void SetMemoryFull(int isMemoryFull);
    }

}
