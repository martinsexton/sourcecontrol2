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

            return View(convertModels(existingGuest));
        }

        [HttpPost]
        public ActionResult Rsvp(Models.Guest guest)
        {
            if (ModelState.IsValid)
            {
                IGuest existingCustomer = service.RetrieveGuestByIdentifier(guest.Id);
                if (existingCustomer != null)
                {
                    //Need to update here
                    WeddingServices.Implementation.Guest g = new WeddingServices.Implementation.Guest();
                    g.Firstname = existingCustomer.getFirstname();
                    g.Surname = existingCustomer.getSurname();
                    g.Email = existingCustomer.getEmail();
                    g.MobileNumber = existingCustomer.getMobile();
                    g.Status = guest.Status;
                    g.Id = existingCustomer.getIdentifier();
                    g.AttendingGuestIdentifier = existingCustomer.getAttendingGuestIdentifier();

                    if (guest.RelatedGuest.Id > 0)
                    {
                        WeddingServices.Implementation.Relationship r = new WeddingServices.Implementation.Relationship();
                        WeddingServices.Implementation.Guest cg = existingCustomer.getRelatedGuest() as WeddingServices.Implementation.Guest;

                        cg.Status = guest.RelatedGuest.Status;
                        cg.Firstname = guest.RelatedGuest.Firstname;
                        cg.Surname = guest.RelatedGuest.Surname;

                        r.RelatedGuest = cg;
                        g.Relationship = r;
                    }
                    
                    service.UpdateGuest(g);
                    if (!String.IsNullOrEmpty(guest.Email))
                    {
                        if (guest.Status.Equals("Accepted"))
                        {
                            emailService.sendMail("Big Day", "Thanks for coming "+guest.NickName, guest.Email);
                        }
                        else
                        {
                            emailService.sendMail("Big Day", "Sorry you cant come " + guest.NickName, guest.Email);
                        }
                        
                    }
                }
                return View("RsvpResult", guest);
            }
            else
            {
                return View(guest);
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
                List<Models.Guest> modelGuests = new List<Models.Guest>();
                foreach (var g in guests)
                {
                    Models.Guest gm = new Models.Guest();
                    gm.Id = g.getIdentifier();
                    gm.Email = g.getEmail();
                    gm.Firstname = g.getFirstname();
                    gm.Surname = g.getSurname();
                    gm.Status = g.getStatus();
                    gm.MobileNumber = g.getMobile();
                    gm.ReferenceIdentifier = g.getReferenceIdentifier();
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
            List<Models.Guest> modelGuests = new List<Models.Guest>();
            foreach (var g in guests)
            {
                Models.Guest gm = new Models.Guest();
                gm.Id = g.getIdentifier();
                gm.Email = g.getEmail();
                gm.Firstname = g.getFirstname();
                gm.Surname = g.getSurname();
                gm.Status = g.getStatus();
                gm.MobileNumber = g.getMobile();
                gm.ReferenceIdentifier = g.getReferenceIdentifier();
                gm.NickName = g.getNickname();
                modelGuests.Add(gm);
            }

            return View(modelGuests);
        }

                public ActionResult Guest(int id)
        {
            IGuest retrievedGuest = service.RetrieveGuestByIdentifier(id);

            Models.Guest g = new Models.Guest();
            if (retrievedGuest != null)
            {
                g.Firstname = retrievedGuest.getFirstname();
                g.Surname = retrievedGuest.getSurname();
                g.Email = retrievedGuest.getEmail();
                g.MobileNumber = retrievedGuest.getMobile();
                g.Status = retrievedGuest.getStatus();
                g.ReferenceIdentifier = retrievedGuest.getReferenceIdentifier();
                g.NickName = retrievedGuest.getNickname();

                if (retrievedGuest.getRelatedGuest() != null)
                {
                    Models.Guest relatedGuest = new Models.Guest();
                    relatedGuest.RelatedGuest = convertModels(retrievedGuest.getRelatedGuest());
                }
            }
            ViewBag.Message = g.Firstname + " " + g.Surname;
            return View(g);
        }

                private Models.Guest convertModels(IGuest retrievedGuest)
                {
                    Models.Guest portalGuest = new Models.Guest();
                    portalGuest.Id = retrievedGuest.getIdentifier();
                    portalGuest.Firstname = retrievedGuest.getFirstname();
                    portalGuest.Surname = retrievedGuest.getSurname();
                    portalGuest.Email = retrievedGuest.getEmail();
                    portalGuest.MobileNumber = retrievedGuest.getMobile();
                    portalGuest.Status = retrievedGuest.getStatus();
                    portalGuest.NickName = retrievedGuest.getNickname();

                    if (retrievedGuest.getRelatedGuest() != null)
                    {
                        Models.Guest relatedPortalGuest = new Models.Guest();

                        relatedPortalGuest.Id = retrievedGuest.getRelatedGuest().getIdentifier();
                        relatedPortalGuest.Firstname = retrievedGuest.getRelatedGuest().getFirstname();
                        relatedPortalGuest.Surname = retrievedGuest.getRelatedGuest().getSurname();
                        relatedPortalGuest.Email = retrievedGuest.getRelatedGuest().getEmail();
                        relatedPortalGuest.MobileNumber = retrievedGuest.getRelatedGuest().getMobile();
                        relatedPortalGuest.Status = retrievedGuest.getRelatedGuest().getStatus();

                        portalGuest.RelatedGuest = relatedPortalGuest;
                    }
                    else
                    {
                        Models.Guest relatedPortalGuest = new Models.Guest();
                        relatedPortalGuest.Id = 0;
                        portalGuest.RelatedGuest = relatedPortalGuest;
                    }

                    return portalGuest;
                }
    }
}