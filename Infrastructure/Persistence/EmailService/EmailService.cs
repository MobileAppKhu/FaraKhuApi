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
                Text = @"<!DOCTYPE html>
                    <html xmlns='http://www.w3.org/1999/xhtml' xmlns:v='urn:schemas-microsoft-com:vml'
                        xmlns:o='urn:schemas-microsoft-com:office:office'>
                        <head>
                            <meta charset='UTF-8'>
                            <title></title>                     
                        </head>
                            <body style='margin: 0;'>
                                <div class='body' style='background: #f8f8f8;padding: 30px 0;'>
                                    <div class='main'
                                        style='background-color: #fff;border: 2px solid #ddd;text-align: center;max-width: 100%;width: 50%;margin: 0 auto;'>
                                            <div class='main-title'
                                                style='line-height: 90px;font-size: 36px;font-weight: bold;color: #276bc9;text-align: center;'>
                                                <p style='margin: 0;'>FaraKhu</p>
                                            </div>
                                            <div class='middle-part' style='background-color: #1346ae;color: #fff;height: 200px;padding-top: 30px;'>
                                                <img src='https://i.ibb.co/PD4zByD/image-3.png' alt='email' style='max-width: 50%;'>
                                                <br>
                                                <p
                                                    style='margin: 0;font-size: 28px;color: #fff;font-family: Verdana, Geneva, Tahoma;direction: rtl;margin-top: 20px;'>
                                                کد تایید شما:</p>
                                            </div>
                                            <div class='confirm-code' style='height: 100px;padding-top: 30px;'>
                                                <span
                                                    style='font-size: 28px;line-height: 70px;font-weight: bold;background-color: #393E46;padding: 16px;border-radius: 100px;color: #fff;margin: 0 auto;letter-spacing: 5px;'>"+emailContent+@"</span>
                                            </div>
                                    </div>
                                </div>
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
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                client.Authenticate(_emailUserName, _emailPassword);
                client.Send(message);
                client.Disconnect(true);
            }
            catch (Exception)
            {
                throw new CustomException(new Error
                {
                    Message = _localizer["EmailServiceException"],
                    ErrorType = ErrorType.EmailServiceException
                });
            }
        }
    }
}