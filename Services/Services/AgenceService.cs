using AgenceLocationVoiture.Models;
using AgenceLocationVoiture.Repositories;
using AgenceLocationVoiture.Services.ServiceContracts;

namespace AgenceLocationVoiture.Services.Services
{
    public class AgenceService : IAgenceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AgenceService> _logger;

        public AgenceService(IUnitOfWork unitOfWork, ILogger<AgenceService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Agence?> GetAgenceByIdAsync(string id)
        {
            try
            {
                return await _unitOfWork.Agences.FirstOrDefaultAsync(a => a.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de l'agence {AgenceId}", id);
                return null;
            }
        }

        public async Task<Agence?> GetAgenceWithDetailsAsync(string id)
        {
            try
            {
                return await _unitOfWork.Agences.GetByIdWithDetailsAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des détails de l'agence {AgenceId}", id);
                return null;
            }
        }

        public async Task<IEnumerable<Agence>> GetAllAgencesAsync()
        {
            try
            {
                return await _unitOfWork.Agences.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de toutes les agences");
                return Enumerable.Empty<Agence>();
            }
        }

        public async Task<IEnumerable<Agence>> GetAgencesVerifieesAsync()
        {
            try
            {
                return await _unitOfWork.Agences.GetAgencesVerifieesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des agences vérifiées");
                return Enumerable.Empty<Agence>();
            }
        }

        public async Task<IEnumerable<Agence>> GetAgencesByVilleAsync(string ville)
        {
            try
            {
                return await _unitOfWork.Agences.GetAgencesByVilleAsync(ville);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des agences de {Ville}", ville);
                return Enumerable.Empty<Agence>();
            }
        }

        public async Task<IEnumerable<Agence>> SearchAgencesAsync(string searchTerm)
        {
            try
            {
                return await _unitOfWork.Agences.SearchAgencesAsync(searchTerm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la recherche d'agences avec le terme {SearchTerm}", searchTerm);
                return Enumerable.Empty<Agence>();
            }
        }

       

        public async Task<bool> UpdateAgenceAsync(Agence agence)
        {
            try
            {
                _unitOfWork.Agences.Update(agence);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Agence {AgenceId} mise à jour avec succès", agence.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour de l'agence {AgenceId}", agence.Id);
                return false;
            }
        }

        public async Task<bool> DeleteAgenceAsync(string id)
        {
            try
            {
                var agence = await GetAgenceByIdAsync(id);
                if (agence == null)
                {
                    _logger.LogWarning("Agence {AgenceId} non trouvée pour suppression", id);
                    return false;
                }

                agence.EstActif = false;
                _unitOfWork.Agences.Update(agence);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Agence {AgenceId} désactivée avec succès", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression de l'agence {AgenceId}", id);
                return false;
            }
        }

        public async Task<bool> VerifierAgenceAsync(string id)
        {
            try
            {
                var agence = await GetAgenceByIdAsync(id);
                if (agence == null)
                {
                    _logger.LogWarning("Agence {AgenceId} non trouvée pour vérification", id);
                    return false;
                }

                agence.EstVerifiee = true;
                _unitOfWork.Agences.Update(agence);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Agence {AgenceId} vérifiée avec succès", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la vérification de l'agence {AgenceId}", id);
                return false;
            }
        }

        public async Task<double> GetNoteMoyenneAsync(string agenceId)
        {
            try
            {
                return await _unitOfWork.Agences.GetNoteMoyenneAsync(agenceId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du calcul de la note moyenne de l'agence {AgenceId}", agenceId);
                return 0;
            }
        }

       
        public async Task<bool> ActivateAgenceAsync(string agenceId)
        {
            try
            {
                var agence = await _unitOfWork.Agences.GetByIdAsync(agenceId);
                if (agence == null)
                {
                    return false;
                }

                agence.EstActif = true;
                _unitOfWork.Agences.Update(agence);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Agence {AgenceId} activée", agenceId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'activation de l'agence {AgenceId}", agenceId);
                return false;
            }
        }
    }
}