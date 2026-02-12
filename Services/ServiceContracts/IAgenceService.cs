using AgenceLocationVoiture.Models;

namespace AgenceLocationVoiture.Services.ServiceContracts
{
    public interface IAgenceService
    {
        Task<Agence?> GetAgenceByIdAsync(string id);
        Task<Agence?> GetAgenceWithDetailsAsync(string id);
        Task<IEnumerable<Agence>> GetAllAgencesAsync();
        Task<IEnumerable<Agence>> GetAgencesVerifieesAsync();
        Task<IEnumerable<Agence>> GetAgencesByVilleAsync(string ville);
        Task<IEnumerable<Agence>> SearchAgencesAsync(string searchTerm);
        Task<bool> UpdateAgenceAsync(Agence agence);
        Task<bool> DeleteAgenceAsync(string id);
        Task<bool> VerifierAgenceAsync(string id);
        Task<double> GetNoteMoyenneAsync(string agenceId);
        Task<bool> ActivateAgenceAsync(string agenceId);
    }
}