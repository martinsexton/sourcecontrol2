using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace doneillspa.Services.Email
{
    public class GmailMailService : IEmailService
    {
        private IConfiguration Configuration;
        static string ApplicationName = "Doneill Gmail API";
        static string[] Scopes = { GmailService.Scope.GmailSend, GmailService.Scope.GmailCompose };

        public GmailMailService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //Implementation uses SMTP client to send mail via Gmail. You will need to change security settings in your
        //gmail account to allow less secure apps sign into your account to allow this to work.
        //https://support.google.com/accounts/answer/6010255?hl=en
        public void SendMail(string fromAddress, string toAddress, string subject, string plainTextContent, string htmlContent, string attachmentName, string attachmentContent)
        {
            try
            {
                // Credentials
                var credentials = new NetworkCredential("<email here>", "<password here>");

                // Mail message
                var mail = new MailMessage()
                {
                    From = new MailAddress("<from address>"),
                    Subject = "Test email.",
                    Body = "Test email body"
                };

                mail.To.Add(new MailAddress("<to address>"));

                // Smtp client
                var client = new SmtpClient()
                {
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = "smtp.gmail.com",
                    EnableSsl = true,
                    Credentials = credentials
                };

                // Send it...         
                client.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in sending email: " + ex.Message);
                Console.ReadKey();
                return;
            }
        }
    }
}
