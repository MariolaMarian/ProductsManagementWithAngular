using ProductsMngmt.ViewModels.DTOs.Email;

namespace ProductsMngmtAPI.Services.Emails
{
    public interface IEmailService
    {
         bool SendEmail(EmailForSentDTO email);
    }
}