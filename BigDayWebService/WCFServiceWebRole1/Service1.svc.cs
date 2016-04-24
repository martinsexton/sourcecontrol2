using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using WeddingService;
using WeddingService.Implementation;
using WeddingService.Interface;

namespace WCFServiceWebRole1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class BigDaySOAPService : IBigDaySOAPService
    {
        void IBigDaySOAPService.AddGuest(string fname, string sname, string status, string guestName)
        {
            IGuestService service = new GuestService();
            Guest g = new Guest();
            g.Firstname = fname;
            g.Surname = sname;
            g.Status = status;
            g.GuestName = guestName;

            service.AddGuest(g);
        }


        List<WeddingGuest> IBigDaySOAPService.GetListOfGuests()
        {
            IGuestService service = new GuestService();
            List<WeddingGuest> listOfGuests = new List<WeddingGuest>();
            List<IGuest> guests = service.RetrieveAllGuests();
            foreach (IGuest g in guests)
            {
                WeddingGuest wg = new WeddingGuest();
                wg.Id = g.getIdentifier();
                wg.Firstname = g.getFirstname();
                wg.Surname = g.getSurname();
                wg.Status = g.getStatus();
                wg.DietComment = g.getDietComment();
                wg.GuestName = g.getGuestName();

                listOfGuests.Add(wg);
            }

            return listOfGuests;
        }
    }
}
