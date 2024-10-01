using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ExactaEasyEng;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Drawing;

namespace ExEyGateway {

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class HMITcpSvc : IHMITcpSvc {

        ExEyGatewayCtrl _control = null;
        ServiceHost sHost = null;
        Thread serviceTh = null;
        string _listeningAddress = "";

        public HMITcpSvc(ExEyGatewayCtrl control, string listeningAddress) {

            _control = control;
            _listeningAddress = listeningAddress;
            serviceTh = new Thread(new ThreadStart(serviceHostSR));
            serviceTh.Start();
            //startTCPService(listeningAddress);
        }

        void serviceHostSR() {

            startTCPService(_listeningAddress);
        }

        void startTCPService(string listeningAddress) {

            sHost = new ServiceHost(this, new Uri(listeningAddress));

            NetTcpBinding tcpBind = new NetTcpBinding(SecurityMode.None);
            tcpBind.OpenTimeout = new TimeSpan(0, 10, 0);
            tcpBind.CloseTimeout = new TimeSpan(0, 10, 0);
            tcpBind.MaxReceivedMessageSize = 104857600;
            tcpBind.ReaderQuotas.MaxStringContentLength = 104857600;
            tcpBind.ReaderQuotas.MaxBytesPerRead = 104857600;

            sHost.AddServiceEndpoint(typeof(ExactaEasyEng.IHMITcpSvc), tcpBind, "");
            ServiceMetadataBehavior smb = sHost.Description.Behaviors.Find<ServiceMetadataBehavior>();

            if (smb == null)
                smb = new ServiceMetadataBehavior();

            //sHost.Opening += new EventHandler(sHost_Opening);
            //sHost.Opened += new EventHandler(sHost_Opened);
            //sHost.Closing += new EventHandler(sHost_Closing);
            //sHost.Closed += new EventHandler(sHost_Closed);
            //sHost.Faulted += new EventHandler(sHost_Faulted);

            sHost.Open();
        }

        //public override System.Drawing.Rectangle GetSupervisorPos() {
        System.Drawing.Rectangle getSupervisorPos() {
            throw new NotImplementedException();
        }

        //public override void SaveRecipeModification(Recipe recipe) {
        void saveRecipeModification(Recipe recipe) {

            _control.raiseSaveRecipeModification(recipe.ToString());
        }

        //public override void SetSupervisorMode(SupervisorModeEnum hmiMode) {
        void setSupervisorMode(SupervisorModeEnum hmiMode) {

            _control.Status = (int)hmiMode;
            _control.raiseSetSupervisorMode((int)hmiMode);
        }

        void setSupervisorBypassSendRecipe(bool bypass)
        {
            _control.BypassSendRecipe = bypass;
            _control.raiseSetSupervisorBypassSendRecipe(bypass);
        }

        //public override void SetHMIOnTop() {
        void setHMIOnTop() {
            throw new NotImplementedException();
        }

        //public override void WaitForKnappStart() {
        void waitForKnappStart() {

            _control.raiseWaitForKnappStart();
        }

        void sendResults(string results) {

            _control.raiseSendResults(results);
        }

        //public override void SetMemoryFull(int isMemoryFull) {

        //    _control.raiseSetMemoryFull(isMemoryFull);
        //}

        //public override void SetSupIsAlive() {
        void setSupIsAlive() {

            _control.raiseSetSupIsAlive();
        }

        //public override string GetErrorString(int errorCode) {
        string GetErrorString(int errorCode) {

            return _control.raiseGetError(errorCode);
        }

        //public override void SupervisorButtonClicked(string buttonID) {
        void SupervisorButtonClicked(string buttonID) {

            _control.raiseSupervisorButtonClick(buttonID);
        }

        string IHMITcpSvc.GetErrorString(int errorCode) {

            return "";
        }

        string IHMITcpSvc.GetSupervisorPos() {

            Rectangle posRect = getSupervisorPos();
            return string.Format("{0};{1};{2};{3}", posRect.X, posRect.Y, posRect.Width, posRect.Height);
        }

        void IHMITcpSvc.SaveRecipeModification(string recipe) {

            Recipe tmpRecipe = Recipe.LoadFromXml(recipe);
            saveRecipeModification(tmpRecipe);
        }

        void IHMITcpSvc.SetHMIOnTop() {

            setHMIOnTop();
        }

        //void IHMITcpSvc.SetMemoryFull(int isMemoryFull) {

        //    SetMemoryFull(isMemoryFull);
        //}

        void IHMITcpSvc.SetSupIsAlive() {

            setSupIsAlive();
        }

        void IHMITcpSvc.SetSupervisorMode(int hmiMode) {

            //Task execCmd = new Task(() => {
            //    SupervisorModeEnum tmpSupMode = (SupervisorModeEnum)hmiMode;
            //    SetSupervisorMode(tmpSupMode);
            //});
            //execCmd.Start();
            SupervisorModeEnum tmpSupMode = (SupervisorModeEnum)hmiMode;
            setSupervisorMode(tmpSupMode);
        }

        void IHMITcpSvc.SetSupervisorBypassSendRecipe(bool bypass)
        {
            setSupervisorBypassSendRecipe(bypass);
        }

        void IHMITcpSvc.SupervisorButtonClicked(string buttonID) {

            SupervisorButtonClicked(buttonID);
        }

        //public override int AuditMessage(int typeId, int messageCount, string messageToAudit) {
        int auditMessage(int totalMessageCount, int partialMessageCount, string messageToAudit) {

            return _control.raiseAuditMessage(totalMessageCount, partialMessageCount, messageToAudit);
        }

        int IHMITcpSvc.AuditMessage(int totalMessageCount, int partialMessageCount, string messageToAudit) {

            return auditMessage(totalMessageCount, partialMessageCount, messageToAudit);
        }

        void IHMITcpSvc.WaitForKnappStart() {


        }

        void IHMITcpSvc.SendResults(string results) {

            sendResults(results);
        }

        void IHMITcpSvc.SetBatchId(int id)
        {
            _control.SetBatchId(id);
        }

        public void Dispose() {

            try {
                sHost.Close(new TimeSpan(0, 0, 3));
            }
            catch {
                sHost.Abort();
            }
        }
    }
}
