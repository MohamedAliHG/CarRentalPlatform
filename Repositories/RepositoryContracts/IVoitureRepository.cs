using AgenceLocationVoiture.Models;

namespace AgenceLocationVoiture.Repositories
{
    public interface IVoitureRepository : IRepository<Voiture>
    {
        Task<Voiture?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Voiture>> GetVoituresDisponiblesAsync();
        Task<IEnumerable<Voiture>> GetVoituresByAgenceAsync(string agenceId);
        Task<IEnumerable<Voiture>> GetVoituresDispoByAgenceAsync(string agenceId);
        Task<Voiture?> GetByImmatriculationAsync(string immatriculation);
    }
}