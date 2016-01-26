using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeddingServices.Interface;

namespace WeddingServices.Implementation
{
    public class Guest : IGuest
    {
        public int Id { get; set; }
        public string FoodChoice { set; get; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string MobileNumber { get; set; }
        public string Status { get; set; }
        public string AttendingGuestName { get; set; }
        public int ReferenceIdentifier { get; set; }

        public string getFirstname()
        {
            return Firstname;
        }

        public string getSurname()
        {
            return Surname;
        }

        public string getStatus()
        {
            return Status;
        }

        public string getEmail()
        {
            return Email;
        }

        public string getMobile()
        {
            return MobileNumber;
        }

        public int getIdentifier()
        {
            return Id;
        }

        public string getAttendingGuestName()
        {
            return AttendingGuestName;
        }

        public int getReferenceIdentifier()
        {
            return ReferenceIdentifier;
        }
    }
}
