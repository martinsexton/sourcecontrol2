using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace persistancelayer.api.model
{
    public interface IProject
    {
        string getName();
        string getDetails();
        string getContactNumber();
        bool getIsActive();
        DateTime getStartDate();
        int getIdentifier();
        int getCode();
    }
}
