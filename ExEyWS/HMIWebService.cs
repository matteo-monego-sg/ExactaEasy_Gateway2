using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.IO;
using System.Net;
using System.Drawing;
using SPAMI.Util.Logger;

namespace ExactaEasyEng {

    public class HMIWebService : HMI {

        enum RequestMethodEnum {
            GET = 1,
            POST
        }

        public string ServiceUrl { get; internal set; }

        public HMIWebService(string serviceURL) {

            ServiceUrl = serviceURL;
        }

        public override Rectangle GetSupervisorPos() {

            Rectangle rect = Rectangle.Empty;
            string jsonResponse = execRequest(RequestMethodEnum.GET, "GetSupervisorPos");
            JavaScriptSerializer jsonSer = new JavaScriptSerializer();
            var objects = jsonSer.DeserializeObject(jsonResponse);
            if (objects is Dictionary<string, object>) {
                Dictionary<string, object> data = (Dictionary<string, object>)objects;
                if (data.ContainsKey("StartX") && data.ContainsKey("StartY"))
                    rect.Location = new Point(Convert.ToInt32(data["StartX"]), Convert.ToInt32(data["StartY"]));
                else
                    throw new Exception("Invalid response");

                if (data.ContainsKey("SizeX") && data.ContainsKey("SizeY"))
                    rect.Size = new Size(Convert.ToInt32(data["SizeX"]), Convert.ToInt32(data["SizeY"]));
                else
                    throw new Exception("Invalid response");
            }
            else
                throw new Exception("Invalid response");

            return rect;
        }

        public override bool SaveRecipeModification(Recipe recipe) {

            int error = 0;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("recipeXml", recipe.ToString());
            string jsonResponse = execRequest(RequestMethodEnum.POST, @"SaveRecipeModification", parameters);

            JavaScriptSerializer jsonSer = new JavaScriptSerializer();
            var objects = jsonSer.DeserializeObject(jsonResponse);
            if (objects is Dictionary<string, object>) {
                Dictionary<string, object> data = (Dictionary<string, object>)objects;
                if (data.ContainsKey("error"))
                    error = Convert.ToInt32(data["error"]);
                else
                    throw new InvalidHMIResponseException("SaveRecipeModification");
            }
            if (error != 0) {
                string errorMessage = GetErrorString(error);
                throw new HMIException(error, errorMessage, "SaveRecipeModification");
            }
            return false;
        }

        public override bool SetSupervisorMode(SupervisorModeEnum hmiMode) {

            int error = 0;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mode", ((int)hmiMode).ToString());
            string jsonResponse = "";
            try {
                jsonResponse = execRequest(RequestMethodEnum.POST, @"SetSupervisorMode", parameters);
            }
            catch (Exception ex) {
                Log.Line(LogLevels.Error, "HMIWebService.SetSupervisorMode", ex.Message);
            }
            JavaScriptSerializer jsonSer = new JavaScriptSerializer();
            var objects = jsonSer.DeserializeObject(jsonResponse);
            if (objects is Dictionary<string, object>) {
                Dictionary<string, object> data = (Dictionary<string, object>)objects;
                if (data.ContainsKey("error"))
                    error = Convert.ToInt32(data["error"]);
                else if (data.ContainsKey("SetSupervisorModeResult"))
                    error = Convert.ToInt32(data["SetSupervisorModeResult"]);
                else
                    throw new InvalidHMIResponseException("SetSupervisorMode");
            }
            if (error != 0) {
                string errorMessage = GetErrorString(error);
                throw new HMIException(error, errorMessage, "SetSupervisorMode");
            }
            return true;
        }

        public override bool SetSupervisorBypassSendRecipe(bool bypass)
        {
            /*NOT USED - 05/04/2024   */
            return false;
        }

        public override void SetHMIOnTop() {

            int error = 0;
            string jsonResponse = execRequest(RequestMethodEnum.POST, @"SetHMIOnTop");

            JavaScriptSerializer jsonSer = new JavaScriptSerializer();
            var objects = jsonSer.DeserializeObject(jsonResponse);
            if (objects is Dictionary<string, object>) {
                Dictionary<string, object> data = (Dictionary<string, object>)objects;
                if (data.ContainsKey("error"))
                    error = Convert.ToInt32(data["error"]);
                else
                    throw new InvalidHMIResponseException("SetHMIOnTop");
            }
            if (error != 0) {
                string errorMessage = GetErrorString(error);
                throw new HMIException(error, errorMessage, "SetHMIOnTop");
            }
        }

        public override void SetSupIsAlive() {

            int error = 0;
            string jsonResponse = execRequest(RequestMethodEnum.POST, @"SetSupIsAlive");

            JavaScriptSerializer jsonSer = new JavaScriptSerializer();
            var objects = jsonSer.DeserializeObject(jsonResponse);
            if (objects is Dictionary<string, object>) {
                Dictionary<string, object> data = (Dictionary<string, object>)objects;
                if (data.ContainsKey("error"))
                    error = Convert.ToInt32(data["error"]);
                else
                    throw new InvalidHMIResponseException("SetSupIsAlive");
            }
            if (error != 0) {
                string errorMessage = GetErrorString(error);
                throw new HMIException(error, errorMessage, "SetSupIsAlive");
            }
        }

        public void AddRecipe(Recipe recipe) {

            int error = 0;

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("recipe", recipe.ToString());
            string jsonResponse = execRequest(RequestMethodEnum.POST, @"AddRecipe", parameters);

            JavaScriptSerializer jsonSer = new JavaScriptSerializer();
            var objects = jsonSer.DeserializeObject(jsonResponse);
            if (objects is Dictionary<string, object>) {
                Dictionary<string, object> data = (Dictionary<string, object>)objects;
                if (data.ContainsKey("error"))
                    error = Convert.ToInt32(data["error"]);
                else
                    throw new InvalidHMIResponseException("SaveRecipeModfication");

            }
            if (error != 0) {
                string errorMessage = GetErrorString(error);
                throw new HMIException(error, errorMessage, "SaveRecipeModfication");
            }
        }

        public override string GetErrorString(int errorCode) {

            string errorString = string.Empty;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("errorCode", errorCode.ToString());
            string jsonResponse = execRequest(RequestMethodEnum.POST, @"GetErrorString", parameters);

            JavaScriptSerializer jsonSer = new JavaScriptSerializer();
            var objects = jsonSer.DeserializeObject(jsonResponse);
            if (objects is Dictionary<string, object>) {
                Dictionary<string, object> data = (Dictionary<string, object>)objects;
                if (data.ContainsKey("ErrorString"))
                    errorString = (string)data["ErrorString"];
                else
                    throw new InvalidHMIResponseException("GetErrorString");
            }
            return errorString;
        }

        public override int AuditMessage(int totalMessageCount, int partialMessageCount, string messageToAudit) {
            
            throw new NotImplementedException();
            //return 0;
        }

        public override void WaitForKnappStart() {
            
            throw new NotImplementedException();
        }

        public override void SendResults(string results) {

            throw new NotImplementedException();
        }
        //public override void SetMemoryFull(int isMemoryFull) {

        //    throw new NotImplementedException();
        //    //return 0;
        //}

        string execRequest(RequestMethodEnum method, string methodName) {

            return execRequest(method, methodName, null);
        }

        string execRequest(RequestMethodEnum method, string methodName, Dictionary<string, string> parameters) {

            //string paramBody = string.Empty; ;
            string jsonResponse = string.Empty; ;
            int retryTimes = 3;
            Exception exx = null;

            while (retryTimes > 0) {
                try {
                    WebRequest req = null;
                    if (parameters != null)
                        req = WebRequest.Create(string.Format(ServiceUrl + "/" + methodName, parameters.Values.ToArray<object>()));
                    else
                        req = WebRequest.Create(string.Format(ServiceUrl + "/" + methodName));

                    req.Method = method.ToString();
                    req.ContentType = @"application/json; charset=utf-8";

                    if (parameters != null && method == RequestMethodEnum.POST) {
                        JavaScriptSerializer jsonSer = new JavaScriptSerializer();
                        StringBuilder sb = new StringBuilder();
                        jsonSer.Serialize(parameters, sb);
                        byte[] buffer = Encoding.ASCII.GetBytes(sb.ToString());
                        Stream postData = req.GetRequestStream();
                        postData.Write(buffer, 0, buffer.Length);
                        postData.Close();
                    }
                    else
                        req.ContentLength = 0;

                    HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                    using (StreamReader sr = new StreamReader(response.GetResponseStream())) {
                        jsonResponse = sr.ReadToEnd();
                    }
                    break;
                }
                catch (Exception ex) {
                    exx = new HMICommunicationException(methodName, ex);
                    retryTimes--;                    
                }                
            }
            if (exx != null)
                throw exx;
            return jsonResponse;
        }

        public override void SupervisorButtonClicked(string buttonID) {

            throw new NotImplementedException();
        }
    }
}
