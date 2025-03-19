namespace SNS.Facebook.Application.Common.Mailing
{
    public interface IMailService : ITransientService
    {
        Task SendAsync(MailRequest request);
    }
}