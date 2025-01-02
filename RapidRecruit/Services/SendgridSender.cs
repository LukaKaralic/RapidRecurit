using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace RapidRecruit.Services
{
    public class SendgridSender : IEmailSender
    {
        private readonly ILogger _logger;

        public SendgridSender(ILogger<SendgridSender> logger)
        {
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SENDGRID_API_KEY")))
            {
                throw new Exception("Null SendGridKey");
            }
            await Execute(subject, message, toEmail);
        }

        public async Task Execute(string subject, string message, string toEmail)
        {
            var client = new SendGridClient(Environment.GetEnvironmentVariable("SENDGRID_API_KEY"));
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("rapidrecruit@lukakaralic.com", "RapidRecruit"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(toEmail));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            var response = await client.SendEmailAsync(msg);
            _logger.LogInformation(response.IsSuccessStatusCode
                                   ? $"Email to {toEmail} queued successfully!"
                                   : $"Failure Email to {toEmail}");
        }
    }
}
