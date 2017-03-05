using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignOnMonitor.Models
{
    public class SignOnRequest
    {
        public int Id { get; set; }
        public string DeviceIdentifier { get; set; }
        public string Username { get; set; }
        public string Base64Image { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool Success { get; set; }
        public string ImageAsHtml { get; set; }

    }
}