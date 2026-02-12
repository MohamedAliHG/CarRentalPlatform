using AgenceLocationVoiture.Models;
using AgenceLocationVoiture.Repositories;
using AgenceLocationVoiture.Services.ServiceContracts;

namespace AgenceLocationVoiture.Services.Services
{
    public class AvisService : IAvisService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AvisService> _logger;

        public AvisService(IUnitOfWork unitOfWork, ILogger<AvisService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Avis?> GetAvisByIdAsync(int id)
        {
            try
            {
                return await _unitOfWork.Avis.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de l'avis {AvisId}", id);
                return null;
            }
        }

       

        public async Task<IEnumerable<Avis>> GetAllAvisAsync()
        {
            try
            {
                return await _unitOfWork.Avis.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de tous les avis");
                return Enumerable.Empty<Avis>();
            }
        }

        public async Task<IEnumerable<Avis>> GetAvisByAgenceAsync(string agenceId)
        {
            try
            {
                return await _unitOfWork.Avis.GetAvisByAgenceAsync(agenceId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des avis de l'agence {AgenceId}", agenceId);
                return Enumerable.Empty<Avis>();
            }
        }

      

        public async Task<bool> CreateAvisAsync(Avis avis)
        {
            try
            {
                if (avis.Note < 1 || avis.Note > 5)
                {
                    _logger.LogWarning("La note doit être entre 1 et 5");
                    return false;
                }

                if (avis.NoteQualiteVehicule.HasValue && 
                    (avis.NoteQualiteVehicule < 1 || avis.NoteQualiteVehicule > 5))
                {
                    _logger.LogWarning("Les notes détaillées doivent être entre 1 et 5");
                    return false;
                }

                avis.DateAvis = DateTime.Now;
                avis.EstVerifie = false;
                avis.EstVisible = true;

                await _unitOfWork.Avis.AddAsync(avis);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Avis créé avec succès pour l'agence {AgenceId} par le client {ClientId}", 
                    avis.AgenceId, avis.ClientId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création de l'avis");
                return false;
            }
        }

       
       
        public async Task<bool> RepondreAvisAsync(int id, string reponseAgence)
        {
            try
            {
                var avis = await GetAvisByIdAsync(id);
                if (avis == null)
                {
                    _logger.LogWarning("Avis {AvisId} non trouvé pour réponse", id);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(reponseAgence))
                {
                    _logger.LogWarning("La réponse ne peut pas être vide");
                    return false;
                }

                avis.ReponseAgence = reponseAgence;
                avis.DateReponse = DateTime.Now;
                _unitOfWork.Avis.Update(avis);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Réponse ajoutée à l'avis {AvisId}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'ajout de la réponse à l'avis {AvisId}", id);
                return false;
            }
        }

        
    }
}