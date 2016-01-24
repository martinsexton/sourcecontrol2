using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.API;
using DomainModel;

namespace Repository.Implementation
{
    public class BabySitterRepository : IBabySitterRepository
    {

        DomainModel.API.IBabySitter IBabySitterRepository.getBabySitter(string eircode)
        {
            //TODO hook up data source
            return new BabySitter();
        }
    }
}
