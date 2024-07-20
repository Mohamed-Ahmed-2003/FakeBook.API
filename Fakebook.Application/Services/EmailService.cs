using System.Net.Mail;
using System.Net;
using Fakebook.Application.Options;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace Fakebook.Application.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body, bool isBodyHTML);
        Task SendConfirmationEmail(string email, string confirmationLink);
    }
    public class EmailService(IOptions< EmailSettings> emailSettings) : IEmailService
    {
        private readonly EmailSettings _emailSettings = emailSettings.Value;

        public Task SendEmailAsync(string toEmail, string subject, string body, bool isBodyHTML)
        {
   
            var client = new SmtpClient(_emailSettings.MailServer, _emailSettings.MailPort)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailSettings.FromEmail, _emailSettings.Password),
                EnableSsl = true,

            };
            MailMessage mailMessage = new MailMessage(_emailSettings.FromEmail, toEmail, subject, body)
            {
                IsBodyHtml = isBodyHTML
            };
            return client.SendMailAsync(mailMessage);
        }
        public async Task SendConfirmationEmail(string email , string confirmationLink )
        {

            await SendEmailAsync(email, "Confirm Your Email", $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>;.", true);
            
        }
    }
}
