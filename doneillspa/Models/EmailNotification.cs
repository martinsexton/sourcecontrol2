using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doneillspa.Services.Email;

namespace doneillspa.Models
{
    public class EmailNotification : Notification
    {
        public string DestinationEmail { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }

        public void Send(IEmailService _emailService)
        {
            _emailService.SendMail("doneill@hotmail.com", DestinationEmail, Subject, Body, Body, string.Empty, string.Empty);
        }
    }
}
