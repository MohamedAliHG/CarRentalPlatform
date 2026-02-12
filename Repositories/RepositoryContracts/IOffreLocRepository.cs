using AgenceLocationVoiture.Models;

namespace AgenceLocationVoiture.Repositories
{
    public interface IOffreLocRepository : IRepository<OffreLoc>
    {
        Task<OffreLoc?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<OffreLoc>> GetAllWithDetailsAsync();
        Task<IEnumerable<OffreLoc>> GetOffresActivesAsync();
        Task<IEnumerable<OffreLoc>> GetOffresByAgenceAsync(string agenceId);
        Task<IEnumerable<OffreLoc>> GetOffresActiveByAgenceAsync(string agenceId);
        Task<IEnumerable<OffreLoc>> GetOffresByVoitureAsync(int voitureId);
        Task<IEnumerable<OffreLoc>> SearchOffresAsync(
            DateTime? dateDebut = null,
            DateTime? dateFin = null,
            decimal? prixMax = null,
            string? ville = null);
    }
}