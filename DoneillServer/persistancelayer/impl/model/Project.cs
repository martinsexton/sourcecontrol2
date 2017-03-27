using persistancelayer.api.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace persistancelayer.impl.model
{
    class Project : IProject
    {
        public int Id { get; set; }
        public bool isActive { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public DateTime StartDate { get; set; }
        public string ContactNumber { get; set; }

        public string getName()
        {
            return Name;
        }

        public string getDetails()
        {
            return Details;
        }

        public string getContactNumber()
        {
            return ContactNumber;
        }

        public DateTime getStartDate()
        {
            return StartDate;
        }


        public int getIdentifier()
        {
            return Id;
        }

        bool IProject.getIsActive()
        {
            return isActive;
        }
    }
}
