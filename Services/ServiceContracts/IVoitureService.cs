using AgenceLocationVoiture.Models;

namespace AgenceLocationVoiture.Services.ServiceContracts
{
    public interface IVoitureService
    {
        Task<Voiture?> GetVoitureByIdAsync(int id);
        Task<Voiture?> GetVoitureWithDetailsAsync(int id);
        Task<IEnumerable<Voiture>> GetAllVoituresAsync();
        Task<IEnumerable<Voiture>> GetVoituresByAgenceAsync(string agenceId);
        Task<IEnumerable<Voiture>> GetVoituresDispoByAgenceAsync(string agenceId);
        Task<bool> CreateVoitureAsync(Voiture voiture);
        Task<bool> UpdateVoitureAsync(Voiture voiture);
        Task<bool> ExistsByImmatriculationAsync(string immatriculation);
        Task<bool> ChangerStatutVoitureAsync(int id, bool active);
    }
}