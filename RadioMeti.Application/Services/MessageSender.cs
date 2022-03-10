using RadioMeti.Application.Interfaces;
using RadioMeti.Application.Utilities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RadioMeti.Application.Services
{


    public class MessageSender : IMessageSender
    {
        public Task SendEmailAsync(string email, string title, string text, bool isHTML = false)
        {
            using (var client = new SmtpClient())
            {

                var credentials = new NetworkCredential()
                {
                    UserName = "mahdikhodarahimi0", // without @gmail.com
                    Password = EmailSecurity.EmailPassword
                };

                client.Credentials = credentials;
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;

                using var emailMessage = new MailMessage()
                {
                    To = { new MailAddress(email) },
                    From = new MailAddress("mahdikhodarahimi0@gmail.com"), // with @gmail.com
                    Subject = title,
                    Body = text,
                    IsBodyHtml = isHTML
                };

                client.Send(emailMessage);
            }

            return Task.CompletedTask;
        }
    }
}
