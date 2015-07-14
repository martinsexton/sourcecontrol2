using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel;



namespace Repository.API
{
    interface IBabySitterRepository
    {
        BabySitter getBabySitter(String firstname, String surname);
    }
}
