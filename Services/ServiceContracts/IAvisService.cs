using AgenceLocationVoiture.Models;

namespace AgenceLocationVoiture.Services.ServiceContracts
{
    public interface IAvisService
    {
        Task<Avis?> GetAvisByIdAsync(int id);
        Task<IEnumerable<Avis>> GetAllAvisAsync();
        Task<IEnumerable<Avis>> GetAvisByAgenceAsync(string agenceId);
        Task<bool> CreateAvisAsync(Avis avis);
        Task<bool> RepondreAvisAsync(int id, string reponseAgence);
       
    }
}