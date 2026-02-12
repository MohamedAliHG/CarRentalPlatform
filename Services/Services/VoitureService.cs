using AgenceLocationVoiture.Models;
using AgenceLocationVoiture.Repositories;
using AgenceLocationVoiture.Services.ServiceContracts;

namespace AgenceLocationVoiture.Services.Services
{
    public class VoitureService : IVoitureService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<VoitureService> _logger;

        public VoitureService(IUnitOfWork unitOfWork, ILogger<VoitureService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Voiture?> GetVoitureByIdAsync(int id)
        {
            try
            {
                return await _unitOfWork.Voitures.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de la voiture {VoitureId}", id);
                return null;
            }
        }

        public async Task<Voiture?> GetVoitureWithDetailsAsync(int id)
        {
            try
            {
                return await _unitOfWork.Voitures.GetByIdWithDetailsAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des détails de la voiture {VoitureId}", id);
                return null;
            }
        }

        public async Task<IEnumerable<Voiture>> GetAllVoituresAsync()
        {
            try
            {
                return await _unitOfWork.Voitures.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de toutes les voitures");
                return Enumerable.Empty<Voiture>();
            }
        }

       

        public async Task<IEnumerable<Voiture>> GetVoituresByAgenceAsync(string agenceId)
        {
            try
            {
                return await _unitOfWork.Voitures.GetVoituresByAgenceAsync(agenceId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des voitures de l'agence {AgenceId}", agenceId);
                return Enumerable.Empty<Voiture>();
            }
        }

        public async Task<IEnumerable<Voiture>> GetVoituresDispoByAgenceAsync(string agenceId)
        {
            try
            {
                return await _unitOfWork.Voitures.GetVoituresDispoByAgenceAsync(agenceId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des voitures de l'agence {AgenceId}", agenceId);
                return Enumerable.Empty<Voiture>();
            }
        }

        public async Task<bool> ChangerStatutVoitureAsync(int id, bool active)
        {
            try
            {
                var voiture = await GetVoitureByIdAsync(id);
                if (voiture == null)
                {
                    _logger.LogWarning("Voiture {VoitureId} non trouvée", id);
                    return false;
                }

                voiture.EstDisponible = active;
                _unitOfWork.Voitures.Update(voiture);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Statut de la Voiture {VoitureId} changé à {Active}", id, active);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du changement de statut de la Voiture {VoitureId}", id);
                return false;
            }
        }

        

        public async Task<bool> CreateVoitureAsync(Voiture voiture)
        {
            try
            {
                if (await ExistsByImmatriculationAsync(voiture.Immatriculation))
                {
                    _logger.LogWarning("Une voiture avec l'immatriculation {Immatriculation} existe déjà", 
                        voiture.Immatriculation);
                    return false;
                }

                voiture.EstDisponible = true;
                await _unitOfWork.Voitures.AddAsync(voiture);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Voiture {Marque} {Modele} créée avec succès", 
                    voiture.Marque, voiture.Modele);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création de la voiture");
                return false;
            }
        }

        public async Task<bool> UpdateVoitureAsync(Voiture voiture)
        {
            try
            {
                _unitOfWork.Voitures.Update(voiture);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Voiture {VoitureId} mise à jour avec succès", voiture.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour de la voiture {VoitureId}", voiture.Id);
                return false;
            }
        }

       

      
        public async Task<bool> ExistsByImmatriculationAsync(string immatriculation)
        {
            try
            {
                var voiture = await _unitOfWork.Voitures.GetByImmatriculationAsync(immatriculation);
                return voiture != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la vérification de l'immatriculation {Immatriculation}", 
                    immatriculation);
                return false;
            }
        }

        
    }
}