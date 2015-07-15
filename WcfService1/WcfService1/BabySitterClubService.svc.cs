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
            return convertToServiceBabySitter(_babySitterRepository.getBabySitter(eircode));
        }

        //TODO need structure mapper to do this
        private BabySitter convertToServiceBabySitter(DomainModel.BabySitter sitter)
        {
            return new WcfService1.BabySitter();
        }
    }
}
