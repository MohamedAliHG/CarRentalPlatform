using AgenceLocationVoiture.Services.ServiceContracts;
using System.Net;
using System.Net.Mail;

namespace AgenceLocationVoiture.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string message, string? fromName = null)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("EmailSettings");
                var fromEmail = smtpSettings["FromEmail"];
                var fromPassword = smtpSettings["FromPassword"];
                var smtpHost = smtpSettings["SmtpHost"];
                var smtpPort = int.Parse(smtpSettings["SmtpPort"] ?? "587");

                using var smtpClient = new SmtpClient(smtpHost, smtpPort)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(fromEmail, fromPassword)
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail!, fromName ?? "Agence Location Voiture"),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation($"Email envoyé avec succès à {toEmail}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de l'envoi de l'email à {toEmail}");
                return false;
            }
        }

        public async Task<bool> SendContactEmailAsync(string nom, string email, string message)
        {
            var adminEmail = _configuration["EmailSettings:AdminEmail"];
            var subject = $"Nouveau message de contact de {nom}";
            var body = $@"
                <h2>Nouveau message de contact</h2>
                <p><strong>Nom:</strong> {nom}</p>
                <p><strong>Email:</strong> {email}</p>
                <p><strong>Message:</strong></p>
                <p>{message}</p>
            ";

            return await SendEmailAsync(adminEmail!, subject, body);
        }
    }
}