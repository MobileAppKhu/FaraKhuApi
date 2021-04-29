using System;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Resources;
using Domain.BaseModels;
using Domain.Enum;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using MimeKit;
using MimeKit.Text;

namespace Infrastructure.Persistence.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly string _emailUserName;
        private readonly string _emailPassword;

        public EmailService(IConfiguration configuration, IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
            _emailUserName = configuration["EmailService:Address"];
            _emailPassword = configuration["EmailService:Password"];
        }

        public void SendEmail(string destinationMail, string name, string subject, string emailTitle,
            string emailContent)
        {
            var content = new TextPart(TextFormat.Html)
            {
                Text = @"
                <!DOCTYPE html>
                <html xmlns='http://www.w3.org/1999/xhtml' xmlns:v='urn:schemas-microsoft-com:vml' xmlns:o='urn:schemas-microsoft-com:office:office'>
                    <head>
                    </head>
                        <body>
                            <h1>" + emailContent + @"</h1>
                       </body>
                    </html>"
            };
            var message = new MimeMessage();
            message.To.Add(new MailboxAddress(name, destinationMail));
            message.From.Add(new MailboxAddress("FaraKhu", _emailUserName));
            message.Subject = subject;
            message.Body = content;
            try
            {
                using var client = new SmtpClient();
                client.Connect("mail.markop.ir", 587, SecureSocketOptions.StartTls);
                client.Authenticate(_emailUserName, _emailPassword);
                client.Send(message);
                client.Disconnect(true);
            }
            catch (Exception)
            {
                throw new CustomException(new Error
                {
                    Message = "Email Exception", //TODO
                    ErrorType = ErrorType.Unexpected
                });
            }
        }
    }
}