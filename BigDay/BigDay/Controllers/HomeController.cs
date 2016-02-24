using BigDay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeddingServices.Interface;
using System.Security.Claims;
using EmailService.Interface;

namespace BigDay.Controllers
{
    public class HomeController : Controller
    {
        IGuestService service;
        IEmailService emailService;

        //Inject relevant services into controller via constructor
        public HomeController(IGuestService s, IEmailService es)
        {
            service = s;
            emailService = es;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult Rsvp()
        {
            ClaimsIdentity claimsIdentity = User.Identity as ClaimsIdentity;
            Claim firstnameClaim = claimsIdentity.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            Claim surnameClaim = claimsIdentity.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname");
            Claim identifierClaim = claimsIdentity.Claims.First(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid");

            IGuest existingGuest = service.RetrieveGuestByIdentifier(Int32.Parse(identifierClaim.Value));
            if(!existingGuest.getStatus().Equals("Unknown")){
                return View("RsvpReadOnly",convertToViewModel(existingGuest));
            } else {
                return View(convertToViewModel(existingGuest));
            }
            
        }

        [HttpPost]
        public ActionResult Rsvp(Models.WeddingGuest guest)
        {
            if (ModelState.IsValid)
            {
                IGuest g = convertToPersistantModel(guest);
                service.UpdateGuest(g);
                sendRSVPMail(guest);

                return View("RsvpResult", guest);
            }
            else
            {
                return View(guest);
            }
        }

        private void sendRSVPMail(Models.WeddingGuest guest)
        {
            if (!String.IsNullOrEmpty(guest.Email))
            {
                if (guest.Status.Equals("Accepted"))
                {
                    emailService.sendMail("Big Day", "Thanks for coming " + guest.NickName, guest.Email);
                }
                else
                {
                    emailService.sendMail("Big Day", "Sorry you cant come " + guest.NickName, guest.Email);
                }

            }
        }

        [HttpPost]
        public ActionResult Guests(string filterbutton)
        {
            if (filterbutton.Equals("All"))
            {
                return Guests();
            }
            else
            {
                ViewBag.Message = "Wedding Guests";
                List<IGuest> guests = service.RetrieveGuestsByStatus(filterbutton);
                List<Models.WeddingGuest> modelGuests = new List<Models.WeddingGuest>();
                foreach (var g in guests)
                {
                    Models.WeddingGuest gm = new Models.WeddingGuest();
                    gm.Id = g.getIdentifier();
                    gm.Email = g.getEmail();
                    gm.Firstname = g.getFirstname();
                    gm.Surname = g.getSurname();
                    gm.Status = g.getStatus();
                    gm.NickName = g.getNickname();

                    modelGuests.Add(gm);
                }

                return View(modelGuests);
            }
        }

        [HttpGet]
        public ActionResult Guests()
        {
            ViewBag.Message = "Wedding Guests";
            List<IGuest> guests = service.RetrieveAllGuests();
            List<Models.WeddingGuest> modelGuests = new List<Models.WeddingGuest>();
            foreach (var g in guests)
            {
                Models.WeddingGuest gm = new Models.WeddingGuest();
                gm.Id = g.getIdentifier();
                gm.Email = g.getEmail();
                gm.Firstname = g.getFirstname();
                gm.Surname = g.getSurname();
                gm.Status = g.getStatus();
                gm.NickName = g.getNickname();
                modelGuests.Add(gm);
            }

            return View(modelGuests);
        }

        private Models.WeddingGuest convertToViewModel(IGuest g)
        {
            Models.WeddingGuest guest = new WeddingGuest();
            guest.Firstname = g.getFirstname();
            guest.Surname = g.getSurname();
            guest.Email = g.getEmail();
            guest.Status = g.getStatus();
            guest.NickName = g.getNickname();
            guest.Id = g.getIdentifier();
            guest.ReferenceId = g.getReferenceIdentifier();

            if (g.getRelatedGuest() != null)
            {  
                guest.PartnerFirstname = g.getRelatedGuest().getFirstname();
                guest.PartnerSurname = g.getRelatedGuest().getSurname();
                guest.PartnerId = g.getRelatedGuest().getIdentifier();
                guest.PartnerReferenceId = g.getRelatedGuest().getReferenceIdentifier();

                if (g.getStatus().Equals("Accepted") && g.getRelatedGuest().getStatus().Equals("Accepted"))
                {
                    guest.IncludeGuest = true;
                }
            }

            return guest;
        }

        private IGuest convertToPersistantModel(Models.WeddingGuest guest)
        {
            WeddingServices.Implementation.Guest g = new WeddingServices.Implementation.Guest();
            g.Firstname = guest.Firstname;
            g.Surname = guest.Surname;
            g.Email = guest.Email;
            g.Status = guest.Status;
            g.Id = guest.Id;
            g.ReferenceIdentifier = guest.ReferenceId;

            if (guest.IncludeGuest)
            {
                if (!String.IsNullOrEmpty(guest.PartnerFirstname) && !String.IsNullOrEmpty(guest.PartnerSurname))
                {
                    WeddingServices.Implementation.Relationship r = new WeddingServices.Implementation.Relationship();
                    WeddingServices.Implementation.Guest cg = new WeddingServices.Implementation.Guest();

                    cg.Status = guest.Status;
                    cg.Firstname = guest.PartnerFirstname;
                    cg.Surname = guest.PartnerSurname;
                    cg.Id = guest.PartnerId;
                    cg.ReferenceIdentifier = guest.PartnerReferenceId;

                    r.RelatedGuest = cg;
                    g.Relationship = r;
                }
            }

            return g;
        }

        public ActionResult Guest(int id)
        {
            IGuest retrievedGuest = service.RetrieveGuestByIdentifier(id);
            Models.WeddingGuest g = convertToViewModel(retrievedGuest);

            ViewBag.Message = g.Firstname + " " + g.Surname;
            return View(g);
        }
    }
}