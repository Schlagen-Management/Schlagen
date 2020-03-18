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
        private IConfiguration _configuration { get; set; }

        private SendGridClient _sendGridClient { get; set; }

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;

            // Instantiate the client.  Get the API key from
            // AppSettings
            _sendGridClient
                = new SendGridClient(
                _configuration.GetSection("AppSettings")["SendGridApiKey"]);
        }

        public async Task<Response> SendEmail(EmailAddress from, EmailAddress to,
            string subject, string plainTextContent, string htmlContent = null)
        {
            // Build the message
            var message = MailHelper.CreateSingleEmail(from, to, subject,
                plainTextContent, htmlContent);

            // Send the message and return the results
            return await _sendGridClient.SendEmailAsync(message);
        }

    }
}
