using AgenceLocationVoiture.Models;

namespace AgenceLocationVoiture.Repositories
{
    public interface IDemandeLocRepository : IRepository<DemandeLoc>
    {
        Task<DemandeLoc?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<DemandeLoc>> GetDemandesByClientAsync(string clientId);
        Task<IEnumerable<DemandeLoc>> GetDemandesByAgenceAsync(string agenceId);
        Task<IEnumerable<DemandeLoc>> GetDemandesByStatutAsync(StatutDemande statut);
        Task<IEnumerable<DemandeLoc>> GetDemandesWithDetailsAsync();
    }
}