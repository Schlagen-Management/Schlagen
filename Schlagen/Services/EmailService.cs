using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schlagen.Services
{
    public class EmailService : IEmailService
    {
        private IConfiguration _configuration;

        private SendGridClient _sendGridClient;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;

            _sendGridClient
                = new SendGridClient(
                _configuration.GetSection("AppSettings")["SendGridApiKey"]);
        }


        public async Task<Response> SendEmail(EmailAddress from, EmailAddress to,
            string subject, string plainTextContent, string htmlContent = null)
        {
            var message = MailHelper.CreateSingleEmail(from, to, subject,
                plainTextContent, htmlContent);

            return await _sendGridClient.SendEmailAsync(message);
        }

    }
}
