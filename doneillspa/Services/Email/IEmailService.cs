using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doneillspa.Services.Email
{
    public interface IEmailService
    {
        Task SendMail(string fromAddress, string toAddress, string subject, string plainTextContent, string htmlContent, string attachmentName, string attachmentContent);
    }
}
