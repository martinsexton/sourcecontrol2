using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel.API;



namespace Repository.API
{
    public interface IBabySitterRepository
    {
        IBabySitter getBabySitter(String eircode);
    }
}
