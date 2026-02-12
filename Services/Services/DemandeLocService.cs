using AgenceLocationVoiture.Models;
using AgenceLocationVoiture.Repositories;
using AgenceLocationVoiture.Services.ServiceContracts;

namespace AgenceLocationVoiture.Services.Services
{
    public class DemandeLocService : IDemandeLocService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DemandeLocService> _logger;
        private readonly IOffreLocService _offreLocService;

        public DemandeLocService(
            IUnitOfWork unitOfWork, 
            ILogger<DemandeLocService> logger,
            IOffreLocService offreLocService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _offreLocService = offreLocService;
        }

        public async Task<DemandeLoc?> GetDemandeByIdAsync(int id)
        {
            try
            {
                return await _unitOfWork.DemandesLocation.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de la demande {DemandeId}", id);
                return null;
            }
        }

        public async Task<DemandeLoc?> GetDemandeWithDetailsAsync(int id)
        {
            try
            {
                return await _unitOfWork.DemandesLocation.GetByIdWithDetailsAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des détails de la demande {DemandeId}", id);
                return null;
            }
        }

        public async Task<IEnumerable<DemandeLoc>> GetAllDemandesAsync()
        {
            try
            {
                return await _unitOfWork.DemandesLocation.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de toutes les demandes");
                return Enumerable.Empty<DemandeLoc>();
            }
        }

        public async Task<IEnumerable<DemandeLoc>> GetAllDemandesWithDetailsAsync()
        {
            try
            {
                return await _unitOfWork.DemandesLocation.GetDemandesWithDetailsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de toutes les demandes");
                return Enumerable.Empty<DemandeLoc>();
            }
        }

        public async Task<IEnumerable<DemandeLoc>> GetDemandesByClientAsync(string clientId)
        {
            try
            {
                return await _unitOfWork.DemandesLocation.GetDemandesByClientAsync(clientId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des demandes du client {ClientId}", clientId);
                return Enumerable.Empty<DemandeLoc>();
            }
        }

        public async Task<IEnumerable<DemandeLoc>> GetDemandesByAgenceAsync(string agenceId)
        {
            try
            {
                return await _unitOfWork.DemandesLocation.GetDemandesByAgenceAsync(agenceId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des demandes de l'agence {AgenceId}", agenceId);
                return Enumerable.Empty<DemandeLoc>();
            }
        }

        public async Task<IEnumerable<DemandeLoc>> GetDemandesByStatutAsync(StatutDemande statut)
        {
            try
            {
                return await _unitOfWork.DemandesLocation.GetDemandesByStatutAsync(statut);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des demandes avec le statut {Statut}", statut);
                return Enumerable.Empty<DemandeLoc>();
            }
        }

      

        public async Task<bool> CreateDemandeAsync(DemandeLoc demande)
        {
            try
            {
                if (demande.DateDebut >= demande.DateFin)
                {
                    _logger.LogWarning("La date de début doit être antérieure à la date de fin");
                    return false;
                }

                if (demande.DateDebut < DateTime.Now.Date)
                {
                    _logger.LogWarning("La date de début ne peut pas être dans le passé");
                    return false;
                }

                demande.DateDemande = DateTime.Now;
                demande.Statut = StatutDemande.EnAttente;

                demande.MontantTotal = await CalculerPrixTotalAsync(
                    demande.OffreLocId, demande.DateDebut, demande.DateFin);

                await _unitOfWork.DemandesLocation.AddAsync(demande);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Demande créée avec succès pour le client {ClientId}", demande.ClientId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création de la demande");
                return false;
            }
        }

        public async Task<decimal?> CalculerPrixTotalAsync(int offreId, DateTime dateDebut, DateTime dateFin)
        {
            try
            {
                var offre = await _unitOfWork.OffresLocation.GetByIdAsync(offreId);
                if (offre == null)
                {
                    _logger.LogWarning("Offre {OffreId} non trouvée pour calcul du prix", offreId);
                    return 0;
                }

                var nombreJours = (dateFin - dateDebut).Days;
                if (nombreJours < 1)
                {
                    _logger.LogWarning("Nombre de jours invalide pour le calcul du prix");
                    return 0;
                }

                var nombreSemaines = nombreJours / 7;
                var joursRestants = nombreJours % 7;

                var prixTotal = nombreSemaines * offre.PrixParSemaine +
                               joursRestants * offre.PrixParJour;

                _logger.LogInformation("Prix total calculé: {PrixTotal} pour {NombreJours} jours",
                    prixTotal, nombreJours);

                return prixTotal;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du calcul du prix total");
                return 0;
            }
        }

          

        public async Task<bool> AccepterDemandeAsync(int id, string reponseAgence, decimal montantTotal)
        {
            try
            {
                var demande = await GetDemandeByIdAsync(id);
                if (demande == null)
                {
                    _logger.LogWarning("Demande {DemandeId} non trouvée", id);
                    return false;
                }

                if (demande.Statut != StatutDemande.EnAttente)
                {
                    _logger.LogWarning("La demande {DemandeId} n'est pas en attente", id);
                    return false;
                }

                demande.Statut = StatutDemande.Acceptee;
                demande.ReponseAgence = reponseAgence;
                demande.MontantTotal = montantTotal;
                demande.DateReponse = DateTime.Now;

                _unitOfWork.DemandesLocation.Update(demande);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Demande {DemandeId} acceptée avec succès", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'acceptation de la demande {DemandeId}", id);
                return false;
            }
        }

        public async Task<bool> RefuserDemandeAsync(int id, string reponseAgence)
        {
            try
            {
                var demande = await GetDemandeByIdAsync(id);
                if (demande == null)
                {
                    _logger.LogWarning("Demande {DemandeId} non trouvée", id);
                    return false;
                }

                if (demande.Statut != StatutDemande.EnAttente)
                {
                    _logger.LogWarning("La demande {DemandeId} n'est pas en attente", id);
                    return false;
                }

                demande.Statut = StatutDemande.Refusee;
                demande.ReponseAgence = reponseAgence;
                demande.DateReponse = DateTime.Now;

                _unitOfWork.DemandesLocation.Update(demande);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Demande {DemandeId} refusée", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du refus de la demande {DemandeId}", id);
                return false;
            }
        }

        public async Task<bool> AnnulerDemandeAsync(int id)
        {
            try
            {
                var demande = await GetDemandeByIdAsync(id);
                if (demande == null)
                {
                    _logger.LogWarning("Demande {DemandeId} non trouvée", id);
                    return false;
                }

                demande.Statut = StatutDemande.Annulee;
                _unitOfWork.DemandesLocation.Update(demande);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Demande {DemandeId} annulée", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'annulation de la demande {DemandeId}", id);
                return false;
            }
        }

        
    }
}