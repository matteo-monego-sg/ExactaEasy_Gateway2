using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Activation;
using System.ServiceModel;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Threading;

namespace HMISimulator {


    enum RequestMethodEnum {
        GET = 1,
        POST
    }

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class HMIService : IHMIService {

        public event EventHandler SetSupIsAliveInvoked;
        public event EventHandler SetSupervisorModeInvoked;
        public string RemoteServiceUrl { get; set; }
        public string SupervisorMode { get; private set; }

        public bool SimulaErroreRicezione { get; set; }

        public ResponseBase SetSupIsAlive() {

            if (SetSupIsAliveInvoked != null)
                SetSupIsAliveInvoked(this, new EventArgs());

            return new ResponseBase { error = "0" };
        }

        public int SetSupervisorMode(string mode) {

            SupervisorMode = mode;

            if (SetSupervisorModeInvoked != null)
                SetSupervisorModeInvoked(this, new EventArgs());

            return 0;
        }

        public ResponseBase SaveRecipeModification(string recipeXml) {

            StreamWriter fRecipe = new StreamWriter("temp_" + DateTime.Now.ToString("yyMMddhhmmss") + ".xml");
            fRecipe.Write(recipeXml);
            fRecipe.Flush();
            fRecipe.Close();
            if (SimulaErroreRicezione)
                return new ResponseBase { error = "1" };
            else
                return new ResponseBase { error = "0" };

        }


        public void SetSupervisorPos(int startX, int startY, int sizeX, int sizeY) {

            int error = 0;

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("startX", startX.ToString());
            parameters.Add("startY", startY.ToString());
            parameters.Add("sizeX", sizeX.ToString());
            parameters.Add("sizeY", sizeY.ToString());
            string jsonResponse = execRequest(RequestMethodEnum.POST, @"SetSupervisorPos", parameters);

            JavaScriptSerializer jsonSer = new JavaScriptSerializer();
            var objects = jsonSer.DeserializeObject(jsonResponse);
            if (objects is Dictionary<string, object>) {
                Dictionary<string, object> data = (Dictionary<string, object>)objects;
                if (data.ContainsKey("SetSupervisorPosResult"))
                    error = Convert.ToInt32(data["SetSupervisorPosResult"]);
                else
                    throw new Exception("SetSupervisorPos");

            }
            if (error != 0) {
                //string errorMessage = GetErrorString(error);
                throw new Exception(error.ToString());
            }

        }

        public void SetLanguage(string languageCode) {

            int error = 0;

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("languageCode", languageCode);
            string jsonResponse = execRequest(RequestMethodEnum.POST, @"SetLanguage", parameters);

            JavaScriptSerializer jsonSer = new JavaScriptSerializer();
            var objects = jsonSer.DeserializeObject(jsonResponse);
            if (objects is Dictionary<string, object>) {
                Dictionary<string, object> data = (Dictionary<string, object>)objects;
                if (data.ContainsKey("SetLanguageResult"))
                    error = Convert.ToInt32(data["SetLanguageResult"]);
                else
                    throw new Exception("SetLanguage");

            }
            if (error != 0) {
                //string errorMessage = GetErrorString(error);
                throw new Exception(error.ToString());
            }

        }

        public void SetUserLevel(string userLevel) {

            int error = 0;

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("userLevel", userLevel);
            string jsonResponse = execRequest(RequestMethodEnum.POST, @"SetUserLevel", parameters);

            JavaScriptSerializer jsonSer = new JavaScriptSerializer();
            var objects = jsonSer.DeserializeObject(jsonResponse);
            if (objects is Dictionary<string, object>) {
                Dictionary<string, object> data = (Dictionary<string, object>)objects;
                if (data.ContainsKey("SetUserLevelResult"))
                    error = Convert.ToInt32(data["SetUserLevelResult"]);
                else
                    throw new Exception("SetUserLevelResult");

            }
            if (error != 0) {
                //string errorMessage = GetErrorString(error);
                throw new Exception(error.ToString());
            }
        }

        public void SetActiveRecipe(string recipeXml) {

            int error = 0;

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("recipeXml", recipeXml);
            string jsonResponse = execRequest(RequestMethodEnum.POST, @"SetActiveRecipe", parameters);

            JavaScriptSerializer jsonSer = new JavaScriptSerializer();
            var objects = jsonSer.DeserializeObject(jsonResponse);
            if (objects is Dictionary<string, object>) {
                Dictionary<string, object> data = (Dictionary<string, object>)objects;
                if (data.ContainsKey("SetActiveRecipeResult"))
                    error = Convert.ToInt32(data["SetActiveRecipeResult"]);
                else
                    throw new Exception("SetActiveRecipeResult");

            }
            if (error != 0) {
                //string errorMessage = GetErrorString(error);
                //throw new Exception(error.ToString());
            }

        }

        public void SetSupervisorMachineInfo(string stationInfo, string speed) {

            int error = 0;

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("stationInfo", stationInfo);
            parameters.Add("speed", speed);
            string jsonResponse = execRequest(RequestMethodEnum.POST, @"SetSupervisorMachineInfo", parameters);

            JavaScriptSerializer jsonSer = new JavaScriptSerializer();
            var objects = jsonSer.DeserializeObject(jsonResponse);
            if (objects is Dictionary<string, object>) {
                Dictionary<string, object> data = (Dictionary<string, object>)objects;
                if (data.ContainsKey("SetSupervisorMachineInfoResult"))
                    error = Convert.ToInt32(data["SetSupervisorMachineInfoResult"]);
                else
                    throw new Exception("SetSupervisorMachineInfoResult");

            }
            if (error != 0) {
                //string errorMessage = GetErrorString(error);
                throw new Exception(error.ToString());
            }

        }

        public void SetMachineMode(string machineMode) {

            int error = 0;

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mode", machineMode);
            string jsonResponse = execRequest(RequestMethodEnum.POST, @"SetMachineMode", parameters);

            JavaScriptSerializer jsonSer = new JavaScriptSerializer();
            var objects = jsonSer.DeserializeObject(jsonResponse);
            if (objects is Dictionary<string, object>) {
                Dictionary<string, object> data = (Dictionary<string, object>)objects;
                if (data.ContainsKey("SetMachineModeResult"))
                    error = Convert.ToInt32(data["SetMachineModeResult"]);
                else
                    throw new Exception("SetMachineModeResult");

            }
            if (error != 0) {
                //string errorMessage = GetErrorString(error);
                throw new Exception(error.ToString());
            }
        }

        public ResponseBase1 GetErrorString(int errorCode) {

            return new ResponseBase1 { ErrorString = "" };
        }

        public void SetSupervisorOnTop() {

            int error = 0;

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string jsonResponse = execRequest(RequestMethodEnum.POST, @"SetSupervisorOnTop", parameters);

            JavaScriptSerializer jsonSer = new JavaScriptSerializer();
            var objects = jsonSer.DeserializeObject(jsonResponse);
            if (objects is Dictionary<string, object>) {
                Dictionary<string, object> data = (Dictionary<string, object>)objects;
                if (data.ContainsKey("SetSupervisorOnTopResult"))
                    error = Convert.ToInt32(data["SetSupervisorOnTopResult"]);
                else
                    throw new Exception("SetSupervisorOnTopResult");

            }
            if (error != 0) {
                //string errorMessage = GetErrorString(error);
                throw new Exception(error.ToString());
            }
        }

        public void SetSupervisorHide() {

            int error = 0;

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string jsonResponse = execRequest(RequestMethodEnum.POST, @"SetSupervisorHide", parameters);

            JavaScriptSerializer jsonSer = new JavaScriptSerializer();
            var objects = jsonSer.DeserializeObject(jsonResponse);
            if (objects is Dictionary<string, object>) {
                Dictionary<string, object> data = (Dictionary<string, object>)objects;
                if (data.ContainsKey("SetSupervisorHideResult"))
                    error = Convert.ToInt32(data["SetSupervisorHideResult"]);
                else
                    throw new Exception("SetSupervisorHideResult");

            }
            if (error != 0) {
                //string errorMessage = GetErrorString(error);
                throw new Exception(error.ToString());
            }
        }

        string execRequest(RequestMethodEnum method, string methodName) {

            return execRequest(method, methodName, null);
        }

        string execRequest(RequestMethodEnum method, string methodName, Dictionary<string, string> parameters) {

            string paramBody = string.Empty; ;
            string jsonResponse = string.Empty; ;

            WebRequest req = null;
            if (parameters != null)
                req = WebRequest.Create(string.Format(RemoteServiceUrl + "/" + methodName, parameters.Values.ToArray<object>()));
            else
                req = WebRequest.Create(string.Format(RemoteServiceUrl + "/" + methodName));

            req.Method = method.ToString();
            req.ContentType = @"application/json; charset=utf-8";

            try {
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
            }

            catch (Exception ex) {
                throw new Exception(methodName, ex);
            }
            return jsonResponse;
        }

    }
}
