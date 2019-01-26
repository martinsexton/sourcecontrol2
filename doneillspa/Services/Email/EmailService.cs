﻿using System;
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

            SendGridMessage msg = BuildMessage(fromAddress, toAddress, subject, plainTextContent, htmlContent, attachmentName, attachmentContent);
            client.SendEmailAsync(msg);
        }

        private SendGridMessage BuildMessage(string fromAddress, string toAddress, string subject, string plainTextContent, string htmlContent, string attachmentName, string attachmentContent)
        {
            var from = new EmailAddress(fromAddress, "Example User");
            var to = new EmailAddress(toAddress, "Example User");
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            if (!String.IsNullOrEmpty(attachmentContent))
            {
                msg.AddAttachment(attachmentName, attachmentContent);
            }

            return msg;
        }
    }
}
