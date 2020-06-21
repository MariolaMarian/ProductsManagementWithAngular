using System;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using ProductsMngmt.ViewModels.DTOs.Email;

namespace ProductsMngmtAPI.Services.Emails
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly IConfiguration _configuration;
        public EmailService(SmtpClient smtpClient, IConfiguration configuration)
        {
            _smtpClient = smtpClient;
            _configuration = configuration;
        }
        public bool SendEmail(EmailForSentDTO email)
        {
            try
            {
                string emailMainBody = $"Company: {email.Company}{System.Environment.NewLine}Owner: {email.Owner}{System.Environment.NewLine}Phone: {email.Phone}{System.Environment.NewLine}Email: {email.Email}{System.Environment.NewLine}Description: {email.Content}{System.Environment.NewLine}";

                var mailMessageToClient = new MailMessage(to: email.Email, subject: "Products management ", body: $"Your request has been registered.{System.Environment.NewLine}{emailMainBody}", from: _configuration.GetValue<string>("Email:SiteEmail"));

                var mailMessageToSystem = new MailMessage(to: _configuration.GetValue<string>("Email:SiteEmail"), subject: "NEW ORDER - Products management", body: $"There is new order created.{System.Environment.NewLine}{emailMainBody}", from: _configuration.GetValue<string>("Email:SiteEmail"));

                _smtpClient.Send(mailMessageToClient);
                _smtpClient.Send(mailMessageToSystem);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}