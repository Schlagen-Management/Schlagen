using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schlagen.Services
{
    public interface IEmailService
    {
        public Task<Response> SendEmail(EmailAddress from, EmailAddress to,
            string subject, string plainTextContent, string htmlContent = null);
    }
}
