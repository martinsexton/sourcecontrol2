using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel.API;

namespace DomainModel
{
    public class BabySitter : IBabySitter
    {
        private String firstName;
        private String surname;

        public String Firstname
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public String Surname
        {
            get { return surname; }
            set { surname = value; }
        }

        public string getFirstName()
        {
            return Firstname;
        }

        public string getSurname()
        {
            return Surname;
        }
    }
}
