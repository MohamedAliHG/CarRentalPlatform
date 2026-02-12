using AgenceLocationVoiture.Models;

namespace AgenceLocationVoiture.Services.ServiceContracts
{
    public interface IAuthenticationService
    {
        Task<bool> RegisterAgenceAsync(Agence agence, string password);
        Task<bool> RegisterClientAsync(Client client, string password);
        Task<Utilisateur?> LoginAsync(string email, string password);
        Task<bool> LogoutAsync();
        Task<bool> IsInRoleAsync(string userId, string role);
       
    }
}