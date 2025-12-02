using Learnix.Others;

namespace Learnix.Services.Interfaces
{
    public interface IEmailService
    {
        //void SendEmail(Email Email);
        Task SendEmailAsync(Email email);
    }
}
