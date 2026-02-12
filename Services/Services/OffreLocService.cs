using AgenceLocationVoiture.Models;
using AgenceLocationVoiture.Repositories;
using AgenceLocationVoiture.Services.ServiceContracts;
using AgenceLocationVoiture.ViewModels;

namespace AgenceLocationVoiture.Services.Services
{
    public class OffreLocService : IOffreLocService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OffreLocService> _logger;

        public OffreLocService(IUnitOfWork unitOfWork, ILogger<OffreLocService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<OffreLoc?> GetOffreByIdAsync(int id)
        {
            try
            {
                return await _unitOfWork.OffresLocation.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de l'offre {OffreId}", id);
                return null;
            }
        }

        public async Task<OffreLoc?> GetOffreWithDetailsAsync(int id)
        {
            try
            {
                return await _unitOfWork.OffresLocation.GetByIdWithDetailsAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des détails de l'offre {OffreId}", id);
                return null;
            }
        }

        public async Task<IEnumerable<OffreLoc>> GetAllOffresAsync()
        {
            try
            {
                return await _unitOfWork.OffresLocation.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de toutes les offres");
                return Enumerable.Empty<OffreLoc>();
            }
        }

        public async Task<RechercheViewModel> FilterResearch(RechercheViewModel model)
        {
            try
            {
                var offres = await SearchOffresAsync(
                  model.DateDebut,
                  model.DateFin,
                  model.PrixMax,
                  model.Ville
              );

                var offresQuery = offres.AsQueryable();

                if (!string.IsNullOrEmpty(model.Marque))
                {
                    offresQuery = offresQuery.Where(o => o.Voiture != null &&
                        o.Voiture.Marque.Contains(model.Marque, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrEmpty(model.Modele))
                {
                    offresQuery = offresQuery.Where(o => o.Voiture != null &&
                        o.Voiture.Modele.Contains(model.Modele, StringComparison.OrdinalIgnoreCase));
                }

                if (model.Categorie.HasValue)
                {
                    offresQuery = offresQuery.Where(o => o.Voiture != null &&
                        o.Voiture.Categorie == model.Categorie.Value);
                }

                if (model.Carburant.HasValue)
                {
                    offresQuery = offresQuery.Where(o => o.Voiture != null &&
                        o.Voiture.TypeCarburant == model.Carburant.Value);
                }

                if (model.Transmission.HasValue)
                {
                    offresQuery = offresQuery.Where(o => o.Voiture != null &&
                        o.Voiture.Transmission == model.Transmission.Value);
                }

                if (model.AnneeMin.HasValue)
                {
                    offresQuery = offresQuery.Where(o => o.Voiture != null &&
                        o.Voiture.Annee >= model.AnneeMin.Value);
                }

                if (model.NombrePlacesMin.HasValue)
                {
                    offresQuery = offresQuery.Where(o => o.Voiture != null &&
                        o.Voiture.NombrePlaces >= model.NombrePlacesMin.Value);
                }

                if (model.AssuranceIncluse == true)
                {
                    offresQuery = offresQuery.Where(o => o.AssuranceIncluse);
                }

                if (model.LivraisonPossible == true)
                {
                    offresQuery = offresQuery.Where(o => o.LivraisonPossible);
                }

                var offresFiltered = offresQuery.ToList();

                model.Resultats = offresFiltered.Select(o => new OffreLocViewModel
                {
                    Id = o.Id,
                    Titre = o.Titre,
                    Description = o.Description,
                    PrixParJour = o.PrixParJour,

                    DateDebut = o.DateDebut,
                    DateFin = o.DateFin,
                    EstActive = o.EstActive,
                    VoitureId = o.VoitureId,
                    AgenceId = o.AgenceId,
                    AgenceNom = o.Agence?.NomAgence,
                    Voiture = o.Voiture != null ? new VoitureViewModel
                    {
                        Id = o.Voiture.Id,
                        Marque = o.Voiture.Marque,
                        Modele = o.Voiture.Modele,
                        Annee = o.Voiture.Annee,
                        PhotoPrincipaleUrl = o.Voiture.PhotoPrincipaleUrl,
                        Categorie = o.Voiture.Categorie,
                        TypeCarburant = o.Voiture.TypeCarburant,
                        Transmission = o.Voiture.Transmission,
                        NombrePlaces = o.Voiture.NombrePlaces
                    } : null
                }).ToList();

                model.TotalResultats = model.Resultats.Count;
                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la recherche d'offres");
                return model;
            }
        }

        public async Task<IEnumerable<OffreLoc>> GetAllWithDetailsAsync()
        {
            try
            {
                return await _unitOfWork.OffresLocation.GetAllWithDetailsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de toutes les offres");
                return Enumerable.Empty<OffreLoc>();
            }
        }
        public async Task<IEnumerable<OffreLoc>> GetOffresActivesAsync()
        {
            try
            {
                return await _unitOfWork.OffresLocation.GetOffresActivesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des offres actives");
                return Enumerable.Empty<OffreLoc>();
            }
        }

        public async Task<IEnumerable<OffreLoc>> GetOffresByAgenceAsync(string agenceId)
        {
            try
            {
                return await _unitOfWork.OffresLocation.GetOffresByAgenceAsync(agenceId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des offres de l'agence {AgenceId}", agenceId);
                return Enumerable.Empty<OffreLoc>();
            }
        }

        public async Task<IEnumerable<OffreLoc>> GetOffresActiveByAgenceAsync(string agenceId)
        {
            try
            {
                return await _unitOfWork.OffresLocation.GetOffresActiveByAgenceAsync(agenceId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des offres de l'agence {AgenceId}", agenceId);
                return Enumerable.Empty<OffreLoc>();
            }
        }

        public async Task<IEnumerable<OffreLoc>> GetOffresActiveByVoitureAsync(int voitureId)
        {
            try
            {
                return await _unitOfWork.OffresLocation.GetOffresByVoitureAsync(voitureId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des offres de la voiture {VoitureId}", voitureId);
                return Enumerable.Empty<OffreLoc>();
            }
        }

        public async Task<IEnumerable<OffreLoc>> SearchOffresAsync(
            DateTime? dateDebut = null,
            DateTime? dateFin = null,
            decimal? prixMax = null,
            string? ville = null)
        {
            try
            {
                return await _unitOfWork.OffresLocation.SearchOffresAsync(dateDebut, dateFin, prixMax, ville);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la recherche d'offres");
                return Enumerable.Empty<OffreLoc>();
            }
        }

        public async Task<bool> CreateOffreAsync(OffreLoc offre)
        {
            try
            {
                if (offre.DateDebut >= offre.DateFin)
                {
                    _logger.LogWarning("La date de début doit être antérieure à la date de fin");
                    return false;
                }

                offre.DateCreation = DateTime.Now;
                offre.EstActive = true;

                await _unitOfWork.OffresLocation.AddAsync(offre);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Offre {OffreTitre} créée avec succès", offre.Titre);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création de l'offre");
                return false;
            }
        }

        public async Task<bool> UpdateOffreAsync(OffreLoc offre)
        {
            try
            {
                _unitOfWork.OffresLocation.Update(offre);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Offre {OffreId} mise à jour avec succès", offre.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour de l'offre {OffreId}", offre.Id);
                return false;
            }
        }

       

        public async Task<bool> ChangerStatutOffreAsync(int id, bool active)
        {
            try
            {
                var offre = await GetOffreByIdAsync(id);
                if (offre == null)
                {
                    _logger.LogWarning("Offre {OffreId} non trouvée", id);
                    return false;
                }

                offre.EstActive = active;
                _unitOfWork.OffresLocation.Update(offre);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Statut de l'offre {OffreId} changé à {Active}", id, active);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du changement de statut de l'offre {OffreId}", id);
                return false;
            }
        }

       
    }
}