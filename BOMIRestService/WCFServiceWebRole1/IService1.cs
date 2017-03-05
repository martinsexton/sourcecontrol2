using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCFServiceWebRole1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "/Register")]
        string Register(RequestData request);

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "/Authenticate")]
        string Authenticate(RequestData request);

        [WebGet(UriTemplate="/GetData",RequestFormat=WebMessageFormat.Json,
            ResponseFormat=WebMessageFormat.Json, BodyStyle=WebMessageBodyStyle.Bare)]
        string GetData();
    }

    [DataContract]
    public class RequestData
    {
        [DataMember]
        public string deviceIdentifier;
        [DataMember]
        public string username;
        [DataMember]
        public string base64Image;
        [DataMember]
        public string longitude;
        [DataMember]
        public string latitude;
    }
}
