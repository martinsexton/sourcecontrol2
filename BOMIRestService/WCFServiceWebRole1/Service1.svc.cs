using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using BomiRepository.impl;
using BomiRepository.api;
using BomiRepository;

namespace WCFServiceWebRole1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {

        public string GetData()
        {
            return "Works";
        }

        public string Register(RequestData request)
        {
            using (DeviceGateway_v1r1Client DeviceClient = new DeviceGateway_v1r1Client())
            {
                try
                {
                    DeviceClient.ClientCredentials.ClientCertificate.Certificate = RetrieveCertificateFromStore();

                    EnrollProfileRequest enrollRequest = BuildEnrollProfileRequest(request);
                    EnrollProfileResponse enrollmentResponse = DeviceClient.EnrollProfile(enrollRequest);

                    //If enrollment was successfull then execute update, passing the image
                    if (enrollmentResponse.ResponseStatus.ReturnCode == 0)
                    {
                        UpdateProfileDetailsRequest updateProfileRequest = BuildUpdateProfileDetailsRequest(request);
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

        public string Authenticate(RequestData request)
        {
            System.Diagnostics.Trace.WriteLine("Authenticate for user in location: Longitude: "+request.longitude + ", Latitude: "+request.latitude);

            using (DeviceGateway_v1r1Client DeviceClient = new DeviceGateway_v1r1Client())
            {
                try
                {
                    DeviceClient.ClientCredentials.ClientCertificate.Certificate = RetrieveCertificateFromStore();
                    VerifyIdentityRequest verifyRequest = BuildVerifyRequest(request);
                    VerifyIdentityResponse response = DeviceClient.VerifyIdentity(verifyRequest);

                    IRepository r = new Repository();
                    IAuthenticationRequest autheRequest = convertModel(request);
                    autheRequest.setSuccess(response.ResponseData.Verified);

                    r.recordAuthenticationRequest(autheRequest);

                    if (response.ResponseData.Verified)
                    {
                        return Boolean.TrueString;
                    }
                    else
                    {
                        return Boolean.FalseString;
                    }
                }
                catch (Exception ex)
                {
                    //Log something somewhere..
                    return ex.Message;
                }

            }
        }

        private IAuthenticationRequest convertModel(RequestData request)
        {
            IAuthenticationRequest authRequest = new AuthenticationRequest();
            authRequest.setUsername(request.username);
            authRequest.setDeviceIdentifier(request.deviceIdentifier);
            authRequest.setLongitude(request.longitude);
            authRequest.setLatitude(request.latitude);
            authRequest.setBase64Image(request.base64Image);

            return authRequest;
        }

        private EnrollProfileRequest BuildEnrollProfileRequest(RequestData data)
        {
            EnrollProfileRequest request = new EnrollProfileRequest();
            request.GenericRequestParameters = new GenericRequestParameters();
            request.GenericRequestParameters.ApplicationIdentifier = "BearingPoint";
            request.GenericRequestParameters.ApplicationUserIdentifier = data.username;
            request.ProfileID = data.username;
            request.DeviceInfo = new DeviceInfo();
            request.DeviceInfo.DeviceIdentifier = data.deviceIdentifier;

            return request;
        }

        private VerifyIdentityRequest BuildVerifyRequest(RequestData data)
        {
            VerifyIdentityRequest request = new VerifyIdentityRequest();
            request.GenericRequestParameters = new GenericRequestParameters();
            request.GenericRequestParameters.ApplicationIdentifier = "BearingPoint";
            request.GenericRequestParameters.ApplicationUserIdentifier = data.username;
            request.ServiceProviderIID = "BearingPoint";
            request.PolicyIdentifier = "face";
            
            request.DeviceInfo = new DeviceInfo();
            request.DeviceInfo.DeviceIdentifier = data.deviceIdentifier;
            request.DeviceInfo.DeviceData = new DeviceData[1];
            request.DeviceInfo.DeviceData[0] = new DeviceData();
            request.DeviceInfo.DeviceData[0].Name = "Capabilities";
            request.DeviceInfo.DeviceData[0].Value = "{\"activityList\":[\"device\",\"pin\",\"face\",\"voice\",\"fingerprint\"]}";

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
            request.CaptureScriptResults[0].CaptureStepResult[0].DataItem[0].Data[0] = Convert.FromBase64String(data.base64Image);

            return request;
        }


        private UpdateProfileDetailsRequest BuildUpdateProfileDetailsRequest(RequestData data)
        {
            UpdateProfileDetailsRequest request = new UpdateProfileDetailsRequest();
            request.GenericRequestParameters = new GenericRequestParameters();
            request.GenericRequestParameters.ApplicationIdentifier = "BearingPoint";
            request.GenericRequestParameters.ApplicationUserIdentifier = data.username;
            request.DeviceInfo = new DeviceInfo();
            request.DeviceInfo.DeviceIdentifier = data.deviceIdentifier;
            request.FaceData = new UpdateProfileDetailsRequestFaceData();
            request.FaceData.ImageType = ImageType.JPG;
            request.FaceData.Data = Convert.FromBase64String(data.base64Image);

            return request;
        }

        private X509Certificate2 RetrieveCertificateFromStore()
        {
            X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            certStore.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certCollection = certStore.Certificates.Find(
                         X509FindType.FindByThumbprint, "FAEB59CB8A806F47FEA5A706DE93D9E0DEECFF52", false);
            return certCollection[0];
        }

    }
}
