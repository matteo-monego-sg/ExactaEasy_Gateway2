using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace HMISimulator {

    public class ResponseBase {

        public string error { get; set; }
    }

    public class ResponseBase1 {
        public string ErrorString { get; set; }
    }

    //public class SetSupervisorPosResponse : ResponseBase {

    //    public int StartX { get; set; }
    //    public int StartY { get; set; }
    //    public int SizeX { get; set; }
    //    public int SizeY { get; set; }
    //}


    [ServiceContract]
    public interface IHMIService {

        [OperationContract]
        [WebInvoke(UriTemplate = "SetSupIsAlive", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, Method = "POST")]
        ResponseBase SetSupIsAlive();

        [OperationContract]
        [WebInvoke(UriTemplate = "SetSupervisorMode", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        int SetSupervisorMode(string mode);

        [OperationContract]
        [WebInvoke(UriTemplate = "GetErrorString", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, Method = "POST")]
        ResponseBase1 GetErrorString(int errorCode);

        [OperationContract]
        [WebInvoke(UriTemplate = "SaveRecipeModification", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, Method = "POST")]
        ResponseBase SaveRecipeModification(string recipeXml);


        //[OperationContract]
        //[WebInvoke(UriTemplate = "GetErrorString", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, Method = "POST")]
        //ResponseBase1 GetErrorString(int errorCode);

    }
}
