using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCFServiceWebRole1
{
    [ServiceContract]
    public interface IBigDaySOAPService
    {
        [OperationContract]
        void AddGuest(string fname, string sname, string status, string guestName);

        [OperationContract]
        List<WeddingGuest> GetListOfGuests();
    }

    [DataContract]
    public class WeddingGuest
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Firstname { get; set; }

        [DataMember]
        public string Surname { get; set; }

        [DataMember]
        public string DietComment { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string GuestName { get; set; }
    }
}
