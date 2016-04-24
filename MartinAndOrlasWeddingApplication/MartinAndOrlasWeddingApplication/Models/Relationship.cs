using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MartinAndOrlasWeddingApplication.Models
{
    public class Relationship
    {
        public Guest relatedGuest { set; get; }
        public string relationship { set; get; }
    }
}