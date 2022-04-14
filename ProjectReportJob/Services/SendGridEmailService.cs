using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectReportJob.Services
{
    public sealed class SendGridEmailService : IEmailService
    {
        private static string apiKey = "";
        private static SendGridEmailService instance = null;
        private static readonly object locker = new object ();  

        private SendGridEmailService(){}

        public static IEmailService Instance
        {
            get
            {
                lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new SendGridEmailService(ConfigurationManager.ConnectionStrings["SendGridApi"].ConnectionString);
                        }
                        return instance;
                    }
            }
        }

        public SendGridEmailService(string key)
        {
            apiKey = key;
        }

        public void SendMail(string fromAddress, string toAddress, string subject, string plainTextContent, string htmlContent, string attachmentName, string attachmentContent)
        {
            var client = new SendGridClient(apiKey);

            SendGridMessage msg = BuildMessage(fromAddress, toAddress, subject, plainTextContent, htmlContent, attachmentName, attachmentContent);
            client.SendEmailAsync(msg);
        }

        private SendGridMessage BuildMessage(string fromAddress, string toAddress, string s, string plainTextContent, string htmlContent, string attachmentName, string attachmentContent)
        {
            var from = new EmailAddress(fromAddress);
            var to = new EmailAddress(toAddress);
            var msg = MailHelper.CreateSingleEmail(from, to, s, plainTextContent, htmlContent);

            if (!String.IsNullOrEmpty(attachmentContent))
            {
                msg.AddAttachment(attachmentName, attachmentContent);
            }

            return msg;
        }
    }
}
