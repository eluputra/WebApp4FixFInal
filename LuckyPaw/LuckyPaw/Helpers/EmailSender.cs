using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace LuckyPaw.Helpers
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return ExecuteAsync(Options.SendGridKey, subject, message, email);
        }

        public async Task ExecuteAsync(string apiKey, string subject, string message, string email)
        {
            var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
            var Message = new MailMessage();
            Message.To.Add(new MailAddress(email));  // replace with valid value 
            Message.From = new MailAddress("ejluputra@gmail.com");  // replace with valid value
            Message.Subject = subject;
            Message.Body = string.Format(body, "Anthony", "mathew6verse33@gmail.com", message);
            Message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "ejluputra@gmail.com",  // replace with valid value
                    Password = "zzzzzzz"  // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(Message);
                
            }
        }
    }
}
