using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.API;
using DomainModel;
using Repository.API;

namespace Repository.Implementation
{
    class BabySitterRepository : IBabySitterRepository
    {

        public BabySitter getBabySitter(string firstname, string surname)
        {
            return new BabySitter();
        }
    }
}
