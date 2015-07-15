using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel;



namespace Repository.API
{
    public interface IBabySitterRepository
    {
        BabySitter getBabySitter(String eircode);
    }
}
