using AgenceLocationVoiture.Models;

namespace AgenceLocationVoiture.Services.ServiceContracts
{
    public interface IDemandeLocService
    {
        Task<DemandeLoc?> GetDemandeByIdAsync(int id);
        Task<DemandeLoc?> GetDemandeWithDetailsAsync(int id);
        Task<IEnumerable<DemandeLoc>> GetAllDemandesWithDetailsAsync();
        Task<IEnumerable<DemandeLoc>> GetAllDemandesAsync();
        Task<IEnumerable<DemandeLoc>> GetDemandesByClientAsync(string clientId);
        Task<IEnumerable<DemandeLoc>> GetDemandesByAgenceAsync(string agenceId);
        Task<IEnumerable<DemandeLoc>> GetDemandesByStatutAsync(StatutDemande statut);
        Task<bool> CreateDemandeAsync(DemandeLoc demande);
        Task<bool> AccepterDemandeAsync(int id, string reponseAgence, decimal montantTotal);
        Task<bool> RefuserDemandeAsync(int id, string reponseAgence);
        Task<bool> AnnulerDemandeAsync(int id);
        Task<decimal?> CalculerPrixTotalAsync(int offreId, DateTime dateDebut, DateTime dateFin);
    }
}