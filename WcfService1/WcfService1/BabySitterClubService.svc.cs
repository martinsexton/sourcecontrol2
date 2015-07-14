using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Repository.API;

namespace WcfService1
{
    public class BabySitterClubService : IBabySittersClubService
    {
        private IBabySitterRepository _babySitterRepository;

        public BabySitterClubService(IBabySitterRepository babySitterRepository)
        {
            _babySitterRepository = babySitterRepository;
        }

        public BabySitter GetBabySittersByEircode(string eircode)
        {
            //TODO user structure mapper to inject repository here.
            return new BabySitter();
        }
    }
}
