using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MartinAndOrlasWeddingApplication.Models
{
    public class Address
    {
        private string addressLine1;
        private string addressLine2;
        private string addressLine3;

        private void setAddressLine1(string line1)
        {
            addressLine1 = line1;
        }

        private string getAddressLine1()
        {
            return addressLine1;
        }

        private void setAddressLine2(string line2)
        {
            addressLine2 = line2;
        }

        private string getAddressLine2()
        {
            return addressLine2;
        }

        private void setAddressLine3(string line3)
        {
            addressLine3 = line3;
        }

        private string getAddressLine3()
        {
            return addressLine3;
        }
    }
}