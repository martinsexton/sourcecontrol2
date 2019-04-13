using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public void SendMail(string fromAddress, string toAddress, string subject, string plainTextContent, string htmlContent, string attachmentName, string attachmentContent)
        {
            var mailMessage = new System.Net.Mail.MailMessage();
            mailMessage.From = new System.Net.Mail.MailAddress(fromAddress);
            mailMessage.To.Add(toAddress);
            mailMessage.ReplyToList.Add(fromAddress);
            mailMessage.Subject = subject;
            mailMessage.Body = plainTextContent;
            mailMessage.IsBodyHtml = false;

            var mimeMessage = MimeKit.MimeMessage.CreateFromMailMessage(mailMessage);

            var gmailMessage = new Google.Apis.Gmail.v1.Data.Message
            {
                Raw = Encode(mimeMessage.ToString())
            };

            Google.Apis.Gmail.v1.UsersResource.MessagesResource.SendRequest request = CreateGmailService().Users.Messages.Send(gmailMessage, Configuration["calendar:client_email"]);

            request.Execute();
        }

        private string Encode(string text)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);

            return System.Convert.ToBase64String(bytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");
        }

        private GmailService CreateGmailService()
        {
            var clientEmail = Configuration["calendar:client_email"];
            string privatekey = Configuration["calendar:private_key"];
            string privateKeyToUse = "-----BEGIN PRIVATE KEY-----" + privatekey + "-----END PRIVATE KEY-----";

            ServiceAccountCredential credential;

            credential = new ServiceAccountCredential(
            new ServiceAccountCredential.Initializer(clientEmail)
            {
                Scopes = Scopes
            }.FromPrivateKey(privateKeyToUse));

            return new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }
    }
}
