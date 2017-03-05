using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignOnRepository.api
{
    public interface ISignOnRequest
    {
        void setId(int id);
        void setDeviceIdentifier(string identifier);
        void setUsername(string username);
        void setBase64Image(string image);
        void setLongitude(string longitude);
        void setLatitude(string latitude);
        void setSuccess(bool s);

        int getId();
        string getDeviceIdentifier();
        string getUsername();
        string getBase64Image();
        string getLongitude();
        string getLatitude();
        bool getSuccess();
    }
}
