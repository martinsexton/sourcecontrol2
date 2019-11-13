using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace doneillspa.Services.Email
{
    public class SendGridEmailService : IEmailService
    {
        private IConfiguration Configuration;

        public SendGridEmailService(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void SendMail(string fromAddress, string toAddress, string subject, string plainTextContent, string htmlContent, string attachmentName, string attachmentContent)
        {
            var apiKey = Configuration["Data:Baby:SendGridKey"];
            var client = new SendGridClient(apiKey);

            SendGridMessage msg = BuildMessage(fromAddress, toAddress, subject, plainTextContent, htmlContent, attachmentName, attachmentContent);
            client.SendEmailAsync(msg);
        }

        private SendGridMessage BuildMessage(string fromAddress, string toAddress, string s, string plainTextContent, string htmlContent, string attachmentName, string attachmentContent)
        {
            var from = new EmailAddress(fromAddress);
            var to = new EmailAddress(toAddress);
            var msg = MailHelper.CreateSingleEmail(from, to, s, plainTextContent, htmlContent);
            
            //Code to use SendGrid Templates
            msg.TemplateId = "d-4db3811e764f484fb9c0082304543975";
            msg.SetTemplateData(new { content = plainTextContent, subject = s });

            if (!String.IsNullOrEmpty(attachmentContent))
            {
                msg.AddAttachment(attachmentName, attachmentContent);
            }

            return msg;
        }
    }
}
