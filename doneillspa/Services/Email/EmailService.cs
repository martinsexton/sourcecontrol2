using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace doneillspa.Services.Email
{
    public class EmailService : IEmailService
    {
        private IConfiguration Configuration;

        public EmailService(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void SendMail(string fromAddress, string toAddress, string subject, string plainTextContent, string htmlContent, string attachmentName, string attachmentContent)
        {
            var apiKey = Configuration["Data:Baby:SendGridKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromAddress, "Example User");
            var to = new EmailAddress(toAddress, "Example User");
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            if (!String.IsNullOrEmpty(attachmentContent))
            {
                msg.AddAttachment(attachmentName, attachmentContent);
            }

            //We dont need to call await here, as we will not bother doing anything with the asyn response from send grid.
            //if it fails to send mail, then its not the end of the world.

            //If in the future we want to wait for the response, then we need to add an await here and wait for the Task<Response>
            //tom come back from the call to SendEmailAsync
            client.SendEmailAsync(msg);
        }
    }
}
