using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Security.Cryptography.X509Certificates;

namespace WCFServiceWebRole1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IBomi4Service
    {


        class PPSNdata
        {
            public string SurName { get; set; }
            public DateTime Birthday { get; set; }
            public string PPSN { get; set; }
        }

        Dictionary<int, PPSNdata> ppsnDictionary = new Dictionary<int, PPSNdata>()
        {
                { 1, new PPSNdata { SurName="Sexton", Birthday= new DateTime(1978,8,23), PPSN="6351198M"}},
                { 2, new PPSNdata { SurName="Curtin", Birthday= new DateTime(1988,7,22), PPSN="5799938S"} },
                { 3, new PPSNdata { SurName="Ramos", Birthday= new DateTime(1986,11,01), PPSN="1658495L"}},
                { 4, new PPSNdata { SurName="Gallagher", Birthday= new DateTime(1986,11,01), PPSN="8905842P"}},
                { 5, new PPSNdata { SurName="Ball", Birthday= new DateTime(1949,01,04), PPSN="7679732S"}}
        };
        public string Register(String deviceIdentifier, String userName, String base64Image)
        {
            using (DeviceGateway_v1r1Client DeviceClient = new DeviceGateway_v1r1Client())
            {
                try
                {
                    DeviceClient.ClientCredentials.ClientCertificate.Certificate = RetrieveCertificateFromStore();

                    EnrollProfileRequest enrollRequest = BuildEnrollProfileRequest(userName, deviceIdentifier);
                    EnrollProfileResponse enrollmentResponse = DeviceClient.EnrollProfile(enrollRequest);

                    //If enrollment was successfull then execute update, passing the image
                    if (enrollmentResponse.ResponseStatus.ReturnCode == 0)
                    {
                        UpdateProfileDetailsRequest updateProfileRequest = BuildUpdateProfileDetailsRequest(userName, deviceIdentifier, base64Image);
                        UpdateProfileDetailsResponse profileUpdateResponse = DeviceClient.UpdateProfileDetails(updateProfileRequest);
                        return profileUpdateResponse.ResponseStatus.Description;
                    }
                    else return enrollmentResponse.ResponseStatus.Description;

                  
                }
                catch (Exception ex)
                {
                    //Log something somewhere..
                    return ex.Message;
                }
               
            }
        }

        public string Authenticate(String deviceIdentifier, String userName, String base64Image)
        {
            using (DeviceGateway_v1r1Client DeviceClient = new DeviceGateway_v1r1Client())
            {
                try
                {
                    DeviceClient.ClientCredentials.ClientCertificate.Certificate = RetrieveCertificateFromStore();
                    VerifyIdentityRequest request = BuildVerifyRequest(userName, deviceIdentifier, base64Image);
                    VerifyIdentityResponse response = DeviceClient.VerifyIdentity(request);

                    return response.ResponseStatus.Message;
                }
                catch (Exception ex)
                {
                    //Log something somewhere..
                    return ex.Message;
                }
               
            }
        }

        private EnrollProfileRequest BuildEnrollProfileRequest(String userName, String deviceIdentifier)
        {
            EnrollProfileRequest request = new EnrollProfileRequest();
            request.GenericRequestParameters = new GenericRequestParameters();
            request.GenericRequestParameters.ApplicationIdentifier = "BearingPoint";
            request.GenericRequestParameters.ApplicationUserIdentifier = userName;
            request.ProfileID = userName;
            request.DeviceInfo = new DeviceInfo();
            request.DeviceInfo.DeviceIdentifier = deviceIdentifier;

            return request;
        }

        private UpdateProfileDetailsRequest BuildUpdateProfileDetailsRequest(string userName, String deviceIdentifier, String base64Image)
        {
            UpdateProfileDetailsRequest request = new UpdateProfileDetailsRequest();
            request.GenericRequestParameters = new GenericRequestParameters();
            request.GenericRequestParameters.ApplicationIdentifier = "BearingPoint";
            request.GenericRequestParameters.ApplicationUserIdentifier = userName;
            request.DeviceInfo = new DeviceInfo();
            request.DeviceInfo.DeviceIdentifier = deviceIdentifier;
            request.FaceData = new UpdateProfileDetailsRequestFaceData();
            request.FaceData.ImageType = ImageType.JPG;
            request.FaceData.Data = Convert.FromBase64String(base64Image);

            return request;
        }

        private VerifyIdentityRequest BuildVerifyRequest(string userName, String deviceIdentifier, String base64Image)
        {
            VerifyIdentityRequest request = new VerifyIdentityRequest();
            request.GenericRequestParameters = new GenericRequestParameters();
            request.GenericRequestParameters.ApplicationIdentifier = "BearingPoint";
            request.GenericRequestParameters.ApplicationUserIdentifier = userName;
            request.ServiceProviderTransactionID = Guid.NewGuid().ToString();
            request.ServiceProviderTransactionDescription = "BearingPointTransactions";
            request.ServiceProviderIID = "BearingPoint";
            request.PolicyIdentifier = "face";
            request.DeviceInfo = new DeviceInfo();
            request.DeviceInfo.DeviceIdentifier = deviceIdentifier;

            request.Face = new Face();
            request.Face.ImageType = ImageType.JPG;
            request.Face.Data = Convert.FromBase64String(base64Image);

            request.CaptureScriptResults = new CaptureScriptResults[1];
            request.CaptureScriptResults[0] = new CaptureScriptResults();
            request.CaptureScriptResults[0].CaptureScriptIdentifier = "identityx";
            request.CaptureScriptResults[0].CaptureStepResult = new CaptureStepResult[1];
            request.CaptureScriptResults[0].CaptureStepResult[0] = new CaptureStepResult();
            request.CaptureScriptResults[0].CaptureStepResult[0].Identifier = "face";
            request.CaptureScriptResults[0].CaptureStepResult[0].DataItem = new CaptureStepDataItem[1];
            request.CaptureScriptResults[0].CaptureStepResult[0].DataItem[0] = new CaptureStepDataItem();
            request.CaptureScriptResults[0].CaptureStepResult[0].DataItem[0].Identifier = "image";
            request.CaptureScriptResults[0].CaptureStepResult[0].DataItem[0].Data = new byte[1][];
            request.CaptureScriptResults[0].CaptureStepResult[0].DataItem[0].Data[1] = Convert.FromBase64String(base64Image);

            return request;
        }

        //Method used when WCF service deployed on Azure
        private X509Certificate2 RetrieveCertificateFromStore()
        {
            X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            certStore.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certCollection = certStore.Certificates.Find(
                         X509FindType.FindByThumbprint, "FAEB59CB8A806F47FEA5A706DE93D9E0DEECFF52", false);
            return certCollection[0];
        }

        //Method is used when running WCF locally as opposed to on Azure
        private X509Certificate2 RetrieveCertificateFromFile()
        {
            return new X509Certificate2("C:\\Users\\patricia.ramos\\Documents\\DAON\\SoapUI Project\\BearingPoint.p12", "bearing2016");
        }
        public string GetPPSNData()
        {
            Random random = new Random();
            PPSNdata data = ppsnDictionary[random.Next(1, 5)];

            return data.PPSN + "," + data.SurName + "," + data.Birthday.ToShortDateString();

        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
