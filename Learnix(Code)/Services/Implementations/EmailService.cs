using Learnix.Others;
using Learnix.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;


namespace Learnix.Services.Implementations
{
    public class EmailService : IEmailService
    {
        //public void SendEmail(Email Email)
        //{
        //    var fromAddress = "f3340819@gmail.com";
        //    var appPassword = "ltfa ruws aecy qszy";
        //    var toAddress = Email.ReceiverEmail;

        //    var subject = Email.Subject;
        //    var body = Email.Body;

        //    var smtp = new SmtpClient
        //    {
        //        Host = "smtp.gmail.com",
        //        Port = 587,
        //        EnableSsl = true,
        //        DeliveryMethod = SmtpDeliveryMethod.Network,
        //        UseDefaultCredentials = false,
        //        Credentials = new NetworkCredential(fromAddress, appPassword)
        //    };

        //    using (var message = new MailMessage(fromAddress, toAddress)
        //    {
        //        Subject = subject,
        //        Body = body
        //    })
        //    {
        //        smtp.Send(message);
        //    }
        //}

        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(Email email)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_settings.From));
            message.To.Add(MailboxAddress.Parse(email.ReceiverEmail));
            message.Subject = email.Subject;
            message.Body = new TextPart("plain") { Text = email.Body };

            using var smtp = new SmtpClient();

            try
            {
                await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_settings.From, _settings.AppPassword);
                await smtp.SendAsync(message);
            }
            catch (Exception ex)
            {
                // Log the exception here (e.g., ILogger)
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }
        }
    }
}

