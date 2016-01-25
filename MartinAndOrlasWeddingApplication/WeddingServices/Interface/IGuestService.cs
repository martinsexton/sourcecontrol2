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
        void AddGuest(IGuest guest);
        IGuest RetrieveGuestByIdentifier(int id);
    }
}
