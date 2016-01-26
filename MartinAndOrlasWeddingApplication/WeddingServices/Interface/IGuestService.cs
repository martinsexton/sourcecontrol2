using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeddingServices.Interface
{
    public interface IGuestService
    {
        List<IGuest> RetrieveAllGuests();
        List<IGuest> RetrieveGuestsByStatus(string status);
        IGuest RetrieveGuestByIdentifier(int id);
        IGuest RetrieveGuestByName(string firstname, string surname);
        IGuest RetrievePartner(int id);
        void AddGuest(IGuest guest);
        
    }
}
