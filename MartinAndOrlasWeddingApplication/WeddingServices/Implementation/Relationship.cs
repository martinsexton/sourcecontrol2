using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeddingServices.Interface;

namespace WeddingServices.Implementation
{
    public class Relationship : IRelationship
    {
        public IGuest RelatedGuest {set; get;}

        public IGuest getRelatedGuest()
        {
            return RelatedGuest;
        }
    }
}
