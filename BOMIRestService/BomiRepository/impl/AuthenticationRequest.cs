using BomiRepository.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomiRepository.impl
{
    public class AuthenticationRequest : IAuthenticationRequest
    {
        private string deviceIdentifier;
        private string username;
        private string base64Image;
        private string latitude;
        private string longitude;
        private bool success;

        public void setDeviceIdentifier(string identifier)
        {
            deviceIdentifier = identifier;
        }

        public void setUsername(string un)
        {
            username = un;
        }

        public void setBase64Image(string image)
        {
            base64Image = image;
        }

        public void setLongitude(string l)
        {
            longitude = l;
        }

        public void setLatitude(string lat)
        {
            latitude = lat;
        }

        public void setSuccess(bool s)
        {
            success = s;
        }

        public string getDeviceIdentifier()
        {
            return deviceIdentifier;
        }

        public string getUsername()
        {
            return username;
        }

        public string getBase64Image()
        {
            return base64Image;
        }

        public string getLongitude()
        {
            return longitude;
        }

        public string getLatitude()
        {
            return latitude;
        }

        public bool getSuccess()
        {
            return success;
        }
    }
}
