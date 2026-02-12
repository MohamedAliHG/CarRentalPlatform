using AgenceLocationVoiture.Models;
using AgenceLocationVoiture.ViewModels;

namespace AgenceLocationVoiture.Services.ServiceContracts
{
    public interface IOffreLocService
    {
        Task<OffreLoc?> GetOffreByIdAsync(int id);
        Task<OffreLoc?> GetOffreWithDetailsAsync(int id);
        Task<IEnumerable<OffreLoc>> GetAllOffresAsync();
        Task<IEnumerable<OffreLoc>> GetAllWithDetailsAsync();
        Task<IEnumerable<OffreLoc>> GetOffresActivesAsync();
        Task<IEnumerable<OffreLoc>> GetOffresByAgenceAsync(string agenceId);
        Task<IEnumerable<OffreLoc>> GetOffresActiveByAgenceAsync(string agenceId);
        Task<IEnumerable<OffreLoc>> GetOffresActiveByVoitureAsync(int voitureId);
        Task<IEnumerable<OffreLoc>> SearchOffresAsync(
            DateTime? dateDebut = null,
            DateTime? dateFin = null,
            decimal? prixMax = null,
            string? ville = null);
        Task<RechercheViewModel> FilterResearch(RechercheViewModel model);
        Task<bool> CreateOffreAsync(OffreLoc offre);
        Task<bool> UpdateOffreAsync(OffreLoc offre);
     
        Task<bool> ChangerStatutOffreAsync(int id, bool active);
        
    }
}