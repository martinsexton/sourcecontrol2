using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Repository.API;
using StructureMap;
using Repository.Implementation;
using DomainModel.API;


namespace BabySitterClubService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class BabySitterClubService : IBabySitterClubService
    {
        private IBabySitterRepository _sitterRepository;

        public BabySitterClubService()
        {
            ObjectFactory.Initialize(x => x.For<IBabySitterRepository>().Use<BabySitterRepository>());
            _sitterRepository = (IBabySitterRepository)ObjectFactory.GetInstance(typeof(IBabySitterRepository));
            
        }

        public List<BabySitter> GetBabySittersByEircode(String eircode)
        {
            BabySitter sitter = convertToServiceDomain(_sitterRepository.getBabySitter(eircode));
            List<BabySitter> babySitters = new List<BabySitter>();
            babySitters.Add(sitter);

            return babySitters;
        }

        private BabySitter convertToServiceDomain(IBabySitter sitter)
        {
            BabySitter bs = new BabySitter();
            bs.FirstName = sitter.getFirstName();
            bs.Surname = sitter.getSurname();

            return bs;
        }
    }
}

