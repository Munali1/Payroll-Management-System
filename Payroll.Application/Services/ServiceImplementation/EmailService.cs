using Microsoft.Extensions.Configuration;
using Payroll.Application.Services.ServiceInterface;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Payroll.Application.Services.ServiceImplementation
{
    public class EmailService : IEmailServiceInterface
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task sendEmail(string email, string body, string subject, byte[] attachment = null, string attachmentName = null)
        {
            var emailSettings = configuration.GetSection("EmailSettings");

            var mailMessage = new MailMessage
            {
                From = new MailAddress(emailSettings["SenderEmail"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);
            if (attachment != null && attachmentName != null)
            {
                var attachmentStream = new System.IO.MemoryStream(attachment);
                var attachmentData = new Attachment(attachmentStream, attachmentName, "application/pdf");
                mailMessage.Attachments.Add(attachmentData);
            }

            using (var smtpClient = new SmtpClient(emailSettings["SmtpServer"])
            {
                Port = int.Parse(emailSettings["SmtpPort"]),
                Credentials = new NetworkCredential(emailSettings["SenderEmail"], emailSettings["SenderPassword"]),
                EnableSsl = true
            })
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}
