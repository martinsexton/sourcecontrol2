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
        public string DietComment { get; set; }
        public string MobileNumber { get; set; }
        public string Status { get; set; }
        public string AttendingGuestName { get; set; }
        public int ReferenceIdentifier { get; set; }
        public int AttendingGuestIdentifier { get; set; }
        public string Nickname { get; set; }
        public IRelationship Relationship { get; set; }

        public string getFirstname()
        {
            return Firstname;
        }

        public string getSurname()
        {
            return Surname;
        }

        public string getDietComment()
        {
            return DietComment;
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

        public int getReferenceIdentifier()
        {
            return ReferenceIdentifier;
        }

        public int getAttendingGuestIdentifier()
        {
            return AttendingGuestIdentifier;
        }

        public string getNickname()
        {
            return Nickname;
        }

        public IGuest getRelatedGuest()
        {
            if (Relationship != null)
            {
                return Relationship.getRelatedGuest();
            }
            else
            {
                return null;
            }
        }
    }
}
