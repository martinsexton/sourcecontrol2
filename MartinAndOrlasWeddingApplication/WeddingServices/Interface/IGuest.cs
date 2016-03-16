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
        string getDietComment();
        string getEmail();
        string getMobile();
        int getIdentifier();
        int getReferenceIdentifier();
        int getAttendingGuestIdentifier();
        string getNickname();
        IGuest getRelatedGuest();
    }
}
