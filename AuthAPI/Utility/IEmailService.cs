using AuthAPI.Models;

namespace AuthAPI.Utility
{
    public interface IEmailService
    {
        void SendEmail(EmailModel emailModel);
    }
}
