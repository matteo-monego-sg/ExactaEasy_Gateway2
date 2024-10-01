using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Drawing;
using System.ServiceModel.Channels;
using System.Diagnostics;
using System.Reflection;
using SPAMI.Util.Logger;
using System.Threading;

namespace ExactaEasyEng {

    public class HMITcp : HMI {

        public string ServiceUrl { get; internal set; }
        DateTime lastHMICallTime = new DateTime();

        IHMITcpSvc _hmi;
        ChannelFactory<IHMITcpSvc> factory;

        public HMITcp(string serviceURL) {

            ServiceUrl = serviceURL;
            factory = new ChannelFactory<IHMITcpSvc>(new NetTcpBinding(SecurityMode.None), new EndpointAddress(serviceURL));
            factory.Faulted += new EventHandler(factory_Faulted);
            lastHMICallTime = DateTime.Now;
            connectToHmi();
        }

        void factory_Faulted(object sender, EventArgs e) {

            ((ICommunicationObject)_hmi).Close();
        }

        //void doConnection() {

        //    _hmi = factory.CreateChannel();
        //}

        public override Rectangle GetSupervisorPos() {

            string[] strData = _hmi.GetSupervisorPos().Split(new char[] { ';' });
            Rectangle tmpRect = Rectangle.Empty;
            if (strData.Length == 4)
                tmpRect = new Rectangle(Convert.ToInt32(strData[0]), Convert.ToInt32(strData[1]), Convert.ToInt32(strData[2]), Convert.ToInt32(strData[3]));
            return tmpRect;
        }

        public override bool SaveRecipeModification(Recipe recipe) {

            if (recipe != null) {
                try {
                    invokeHmi("SaveRecipeModification", new object[] { recipe.ToString() });
                }
                catch {
                    Log.Line(LogLevels.Error, "HMITcp.SaveRecipeModification", "Unable to execute command!");
                    return false;
                }
            }
            return true;
        }

        public override bool SetSupervisorMode(SupervisorModeEnum hmiMode) {

            try {
                invokeHmi("SetSupervisorMode", new object[] { (int)hmiMode });
            }
            catch {                
                Log.Line(LogLevels.Error, "HMITcp.SetHMIOnTop", "Unable to execute command!");
                return false;
            }
            return true;
        }

        public override bool SetSupervisorBypassSendRecipe(bool bypass)
        {
            try
            {
                invokeHmi("SetSupervisorBypassSendRecipe", new object[] { bypass });
            }
            catch
            {
                Log.Line(LogLevels.Error, "HMITcp.SetSupervisorBypassSendRecipe", "Unable to execute command!");
                return false;
            }
            return true;
        }

        public override void SetHMIOnTop() {

            try {
                invokeHmi("SetHMIOnTop", null);
            }
            catch {
                Log.Line(LogLevels.Error, "HMITcp.SetHMIOnTop", "Unable to execute command!");
            }
        }

        public override void SetSupIsAlive() {

            invokeHmi("SetSupIsAlive", null);
        }

        public override string GetErrorString(int errorCode) {

            string ris = "";
            try {
                ris = (string)invokeHmi("GetErrorString", new object[] { errorCode });
            }
            catch {
                Log.Line(LogLevels.Error, "HMITcp.GetErrorString", "Unable to execute command!");
                throw;
            }
            return ris;
        }

        public override void SupervisorButtonClicked(string buttonID) {

            try {
                invokeHmi("SupervisorButtonClicked", new object[] { buttonID });
            }
            catch {
                Log.Line(LogLevels.Error, "HMITcp.SupervisorButtonClicked", "Unable to execute command!");
            }
        }

        public override int AuditMessage(int totalMessageCount, int partialMessageCount, string messageToAudit) {

            int ris = -1;
            try {
                ris = (int)invokeHmi("AuditMessage", new object[] { totalMessageCount, partialMessageCount, messageToAudit });
            }
            catch {
                Log.Line(LogLevels.Error, "HMITcp.AuditMessage", "Unable to execute command!");
            }
            return ris;
        }

        public override void WaitForKnappStart() {
            
            try {
               invokeHmi("WaitForKnappStart", null);
            }
            catch {
                Log.Line(LogLevels.Error, "HMITcp.WaitForKnappStart", "Unable to execute command!");
            }            
        }

        public override void SendResults(string results) {

            try {
                invokeHmi("SendResults", new object[] { results });
            }
            catch {
                Log.Line(LogLevels.Error, "HMITcp.SendResults", "Unable to execute command!");
            } 
        }

        //public override void SetMemoryFull(int isMemoryFull) {

        //    invokeHmi("SetMemoryFull", null);
        //}

        object invokeHmi(string methodName, object[] parameters) {

            if (methodName.Contains("GetErrorString") == false) {
                Log.Line(LogLevels.Debug, "HMITcp.invokeHmi", "Invoking \"" + methodName + "\" method...");
            }
            Type hmiType = _hmi.GetType();
            if (connectToHmi()) {
                int retryTimes = 3;
                do {
                    try {
                        while ((DateTime.Now - lastHMICallTime).TotalMilliseconds < 400) {
                            Thread.Sleep(50);
                        }
                        return hmiType.GetMethod(methodName).Invoke(_hmi, parameters);
                    }
                    catch (TargetInvocationException ex) {
                        if (ex.InnerException != null && ex.InnerException is CommunicationException)
                            if (!connectToHmi()) {
                                Log.Line(LogLevels.Warning, "HMITcp.invokeHmi", "Communication exception while invoking \"" + methodName + "\": " + ex.InnerException.Message);
                                break;
                            }
                            else
                                retryTimes--;
                        else {
                            Log.Line(LogLevels.Warning, "HMITcp.invokeHmi", "Communication exception while invoking \"" + methodName + "\": " + ex.InnerException.Message);
                            break;
                        }
                    }
                    finally {
                        lastHMICallTime = DateTime.Now;
                    }
                }
                while (retryTimes >= 0);
            }
            Log.Line(LogLevels.Error, "HMITcp.invokeHmi", "Communication exception while invoking \"" + methodName + "\"");
            throw new CommunicationException();
            //return -1;
        }

        bool connectToHmi() {

            if (_hmi == null || ((ICommunicationObject)_hmi).State != CommunicationState.Opened) {
                _hmi = factory.CreateChannel();
                try {
                    ((ICommunicationObject)_hmi).Open();
                }
                catch {
                    return false;
                }
            }
            return true;
        }

    }
}
