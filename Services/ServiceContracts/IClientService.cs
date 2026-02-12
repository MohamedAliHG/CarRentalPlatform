using AgenceLocationVoiture.Models;

namespace AgenceLocationVoiture.Services.ServiceContracts
{
    public interface IClientService
    {
        Task<Client?> GetClientByIdAsync(string id);
        
        Task<bool> ActivateClientAsync(string clientId);
        Task<IEnumerable<Client>> GetAllClientsAsync();
       
        Task<bool> CreateClientAsync(Client client);
      
        Task<bool> DeleteClientAsync(string id);
        Task<bool> VerifierPermisAsync(string id);
        Task<bool> ExistsByNumeroPermisAsync(string numeroPermis);
       
    }
}