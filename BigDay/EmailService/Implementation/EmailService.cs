using EmailService.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;

namespace EmailService.Implementation
{
    public class EmailService : IEmailService
    {
        public void sendMail(string subject, string message, string toAddress)
        {
            SmtpClient client = new SmtpClient();

            try
            {
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 10000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("sexton.martin@gmail.com", "yellowsub");

                MailMessage mm = new MailMessage("donotreply@domain.com", toAddress, subject, message);
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(mm);
            }
            catch(Exception ex){
                //Do nothing. If we cant email for some reason we dont want it to interfere with capturing
                //RSVP
            }
            finally
            {
                client.Dispose();    
            }
        }
    }
}
