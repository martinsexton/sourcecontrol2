using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomiRepository.api
{
    public interface IAuthenticationRequest
    {
        void setDeviceIdentifier(string identifier);
        void setUsername(string username);
        void setBase64Image(string image);
        void setLongitude(string longitude);
        void setLatitude(string latitude);
        void setSuccess(bool s);

        string getDeviceIdentifier();
        string getUsername();
        string getBase64Image();
        string getLongitude();
        string getLatitude();
        bool getSuccess();
    }
}
