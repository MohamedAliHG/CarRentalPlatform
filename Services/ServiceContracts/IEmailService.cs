namespace AgenceLocationVoiture.Services.ServiceContracts
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string message, string? fromName = null);
        Task<bool> SendContactEmailAsync(string nom, string email, string message);
    }
}