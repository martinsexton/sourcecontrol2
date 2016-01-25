using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeddingServices.Interface
{
    public interface IGuest
    {
        string getFirstname();
        string getSurname();
        string getStatus();
        string getEmail();
        string getMobile();
        int getIdentifier();
        string getAttendingGuestName();
    }
}
