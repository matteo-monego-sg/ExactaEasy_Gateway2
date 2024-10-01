using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace ExactaEasyEng {

    //public enum _HMIModeEnum {
    //    Unknown = 0,
    //    ReadyToRun = 1,
    //    Editing = 2,
    //    Error = 3,
    //    Busy = 4,
    //    ApplicationStarted = 100
    //}

    public abstract class HMI {

        public abstract Rectangle GetSupervisorPos();
        public abstract bool SaveRecipeModification(Recipe recipe);
        public abstract bool SetSupervisorMode(SupervisorModeEnum hmiMode);
        public abstract bool SetSupervisorBypassSendRecipe(bool bypass);
        public abstract void SetHMIOnTop();
        public abstract void SetSupIsAlive();
        public abstract string GetErrorString(int errorCode);
        public abstract void SupervisorButtonClicked(string buttonID);
        public abstract int AuditMessage(int totalMessageCount, int partialMessageCount, string messageToAudit);
        public abstract void WaitForKnappStart();
        public abstract void SendResults(string results);
        //public abstract void SetMemoryFull(int isMemoryFull);
    }

    public class HMIException : ApplicationException {

        public int ErrorCode { get; internal set; }
        public string ErrorMessage { get; internal set; }

        public HMIException(int errorCode, string errorMessage, string methodName)
            : this(errorCode, errorMessage, methodName, null) {

        }

        public HMIException(int errorCode, string errorMessage, string methodName, Exception ex)
            : base("Error executing method: " + methodName, ex) {

            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }

    public class InvalidHMIResponseException : HMIException {

        public InvalidHMIResponseException(string methodName)
            : base(-11, "Invalid response from method", methodName) {
        }
    }

    public class HMICommunicationException : HMIException {

        public HMICommunicationException(string methodName, Exception ex)
            : base(-12, "Unable to communicate with HMI webseervice", methodName, ex) {

        }
    }


    // Vecchia HMI. Da eliminare dopo test

    //public class HMI {

    //    enum RequestMethodEnum {
    //        GET = 1,
    //        POST
    //    }

    //    public string ServiceUrl { get; internal set; }

    //    public HMI(string serviceURL) {

    //        ServiceUrl = serviceURL;
    //    }

    //    public Rectangle GetSupervisorPos() {

    //        Rectangle rect = Rectangle.Empty;
    //        string jsonResponse = execRequest(RequestMethodEnum.GET, "GetSupervisorPos");
    //        JavaScriptSerializer jsonSer = new JavaScriptSerializer();
    //        var objects = jsonSer.DeserializeObject(jsonResponse);
    //        if (objects is Dictionary<string, object>) {
    //            Dictionary<string, object> data = (Dictionary<string, object>)objects;
    //            if (data.ContainsKey("StartX") && data.ContainsKey("StartY"))
    //                rect.Location = new Point(Convert.ToInt32(data["StartX"]), Convert.ToInt32(data["StartY"]));
    //            else
    //                throw new Exception("Invalid response");

    //            if (data.ContainsKey("SizeX") && data.ContainsKey("SizeY"))
    //                rect.Size = new Size(Convert.ToInt32(data["SizeX"]), Convert.ToInt32(data["SizeY"]));
    //            else
    //                throw new Exception("Invalid response");
    //        }
    //        else
    //            throw new Exception("Invalid response");

    //        return rect;
    //    }

    //    public void SaveRecipeModification(Recipe recipe) {

    //        int error = 0;
    //        Dictionary<string, string> parameters = new Dictionary<string, string>();
    //        parameters.Add("recipeXml", recipe.ToString());
    //        string jsonResponse = execRequest(RequestMethodEnum.POST, @"SaveRecipeModification", parameters);

    //        JavaScriptSerializer jsonSer = new JavaScriptSerializer();
    //        var objects = jsonSer.DeserializeObject(jsonResponse);
    //        if (objects is Dictionary<string, object>) {
    //            Dictionary<string, object> data = (Dictionary<string, object>)objects;
    //            if (data.ContainsKey("error"))
    //                error = Convert.ToInt32(data["error"]);
    //            else
    //                throw new InvalidHMIResponseException("SaveRecipeModification");
    //        }
    //        if (error != 0) {
    //            string errorMessage = GetErrorString(error);
    //            throw new HMIException(error, errorMessage, "SaveRecipeModification");
    //        }
    //    }

    //    public void SetSupervisorMode(HMIModeEnum hmiMode) {

    //        int error = 0;
    //        Dictionary<string, string> parameters = new Dictionary<string, string>();
    //        parameters.Add("mode", ((int)hmiMode).ToString());
    //        string jsonResponse = execRequest(RequestMethodEnum.POST, @"SetSupervisorMode", parameters);

    //        JavaScriptSerializer jsonSer = new JavaScriptSerializer();
    //        var objects = jsonSer.DeserializeObject(jsonResponse);
    //        if (objects is Dictionary<string, object>) {
    //            Dictionary<string, object> data = (Dictionary<string, object>)objects;
    //            if (data.ContainsKey("error"))
    //                error = Convert.ToInt32(data["error"]);
    //            else if (data.ContainsKey("SetSupervisorModeResult"))
    //                error = Convert.ToInt32(data["SetSupervisorModeResult"]);
    //            else
    //                throw new InvalidHMIResponseException("SetSupervisorMode");
    //        }
    //        if (error != 0) {
    //            string errorMessage = GetErrorString(error);
    //            throw new HMIException(error, errorMessage, "SetSupervisorMode");
    //        }
    //    }

    //    public void SetHMIOnTop() {

    //        int error = 0;
    //        string jsonResponse = execRequest(RequestMethodEnum.POST, @"SetHMIOnTop");

    //        JavaScriptSerializer jsonSer = new JavaScriptSerializer();
    //        var objects = jsonSer.DeserializeObject(jsonResponse);
    //        if (objects is Dictionary<string, object>) {
    //            Dictionary<string, object> data = (Dictionary<string, object>)objects;
    //            if (data.ContainsKey("error"))
    //                error = Convert.ToInt32(data["error"]);
    //            else
    //                throw new InvalidHMIResponseException("SetHMIOnTop");
    //        }
    //        if (error != 0) {
    //            string errorMessage = GetErrorString(error);
    //            throw new HMIException(error, errorMessage, "SetHMIOnTop");
    //        }
    //    }

    //    public void SetSupIsAlive() {

    //        int error = 0;
    //        string jsonResponse = execRequest(RequestMethodEnum.POST, @"SetSupIsAlive");

    //        JavaScriptSerializer jsonSer = new JavaScriptSerializer();
    //        var objects = jsonSer.DeserializeObject(jsonResponse);
    //        if (objects is Dictionary<string, object>) {
    //            Dictionary<string, object> data = (Dictionary<string, object>)objects;
    //            if (data.ContainsKey("error"))
    //                error = Convert.ToInt32(data["error"]);
    //            else
    //                throw new InvalidHMIResponseException("SetSupIsAlive");
    //        }
    //        if (error != 0) {
    //            string errorMessage = GetErrorString(error);
    //            throw new HMIException(error, errorMessage, "SetSupIsAlive");
    //        }
    //    }

    //    public void AddRecipe(Recipe recipe) {

    //        int error = 0;

    //        Dictionary<string, string> parameters = new Dictionary<string, string>();
    //        parameters.Add("recipe", recipe.ToString());
    //        string jsonResponse = execRequest(RequestMethodEnum.POST, @"AddRecipe", parameters);

    //        JavaScriptSerializer jsonSer = new JavaScriptSerializer();
    //        var objects = jsonSer.DeserializeObject(jsonResponse);
    //        if (objects is Dictionary<string, object>) {
    //            Dictionary<string, object> data = (Dictionary<string, object>)objects;
    //            if (data.ContainsKey("error"))
    //                error = Convert.ToInt32(data["error"]);
    //            else
    //                throw new InvalidHMIResponseException("SaveRecipeModfication");

    //        }
    //        if (error != 0) {
    //            string errorMessage = GetErrorString(error);
    //            throw new HMIException(error, errorMessage, "SaveRecipeModfication");
    //        }
    //    }

    //    public string GetErrorString(int errorCode) {

    //        string errorString = string.Empty;
    //        Dictionary<string, string> parameters = new Dictionary<string, string>();
    //        parameters.Add("errorCode", errorCode.ToString());
    //        string jsonResponse = execRequest(RequestMethodEnum.POST, @"GetErrorString", parameters);

    //        JavaScriptSerializer jsonSer = new JavaScriptSerializer();
    //        var objects = jsonSer.DeserializeObject(jsonResponse);
    //        if (objects is Dictionary<string, object>) {
    //            Dictionary<string, object> data = (Dictionary<string, object>)objects;
    //            if (data.ContainsKey("ErrorString"))
    //                errorString = (string)data["ErrorString"];
    //            else
    //                throw new InvalidHMIResponseException("GetErrorString");
    //        }
    //        return errorString;
    //    }

    //    string execRequest(RequestMethodEnum method, string methodName) {

    //        return execRequest(method, methodName, null);
    //    }

    //    string execRequest(RequestMethodEnum method, string methodName, Dictionary<string, string> parameters) {

    //        string paramBody = string.Empty; ;
    //        string jsonResponse = string.Empty; ;

    //        try {
    //            WebRequest req = null;
    //            if (parameters != null)
    //                req = WebRequest.Create(string.Format(ServiceUrl + "/" + methodName, parameters.Values.ToArray<object>()));
    //            else
    //                req = WebRequest.Create(string.Format(ServiceUrl + "/" + methodName));

    //            req.Method = method.ToString();
    //            req.ContentType = @"application/json; charset=utf-8";

    //            if (parameters != null && method == RequestMethodEnum.POST) {
    //                JavaScriptSerializer jsonSer = new JavaScriptSerializer();
    //                StringBuilder sb = new StringBuilder();
    //                jsonSer.Serialize(parameters, sb);
    //                byte[] buffer = Encoding.ASCII.GetBytes(sb.ToString());
    //                Stream postData = req.GetRequestStream();
    //                postData.Write(buffer, 0, buffer.Length);
    //                postData.Close();
    //            }
    //            else
    //                req.ContentLength = 0;

    //            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
    //            using (StreamReader sr = new StreamReader(response.GetResponseStream())) {
    //                jsonResponse = sr.ReadToEnd();
    //            }
    //        }
    //        catch (Exception ex) {
    //            throw new HMICommunicationException(methodName, ex);
    //        }
    //        return jsonResponse;
    //    }
    //}

}
