using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using core.Interfaces;

namespace infrastructure
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task SendEmailAsync(string toEmail, string subject, string body, bool isBodyHtml = true)
        {
            var mailServer = this.configuration["EmailSettings:MailServer"];
            var fromEmail = this.configuration["EmailSettings:FromEmail"];
            var password = this.configuration["EmailSettings:Password"];
            var port = int.Parse(this.configuration["EmailSettings:MailPort"]);

            var client = new SmtpClient(mailServer, port)
            {
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage(fromEmail, toEmail, subject, body)
            {
                IsBodyHtml = isBodyHtml
            };

            return client.SendMailAsync(mailMessage);
        }
    }
}
