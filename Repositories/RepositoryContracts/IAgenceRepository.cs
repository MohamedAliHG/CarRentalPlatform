using AgenceLocationVoiture.Models;

namespace AgenceLocationVoiture.Repositories
{
    public interface IAgenceRepository : IRepository<Agence>
    {
        Task<Agence?> GetByIdWithDetailsAsync(string id);
        Task<IEnumerable<Agence>> GetAgencesVerifieesAsync();
        Task<IEnumerable<Agence>> GetAgencesByVilleAsync(string ville);
        Task<IEnumerable<Agence>> SearchAgencesAsync(string searchTerm);
        Task<double> GetNoteMoyenneAsync(string agenceId);
    }
}