using AgenceLocationVoiture.Models;
using AgenceLocationVoiture.Services.ServiceContracts;
using AgenceLocationVoiture.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgenceLocationVoiture.Controllers
{
    public class VoitureController : Controller
    {
        private readonly IVoitureService _voitureService;
        private readonly IOffreLocService _offreLocService;
        private readonly IFileUploadService _fileUploadService;
        private readonly ILogger<VoitureController> _logger;

        public VoitureController(
            IVoitureService voitureService,
            IOffreLocService offreLocService,
            IFileUploadService fileUploadService,
            ILogger<VoitureController> logger)
        {
            _voitureService = voitureService;
            _offreLocService = offreLocService;
            _fileUploadService = fileUploadService;
            _logger = logger;
        }


        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var voiture = await _voitureService.GetVoitureWithDetailsAsync(id);
                if (voiture == null)
                {
                    return NotFound();
                }

                var offres = await _offreLocService.GetOffresActiveByVoitureAsync(id);
                ViewBag.Offres = offres;

                return View(voiture);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des détails de la voiture {VoitureId}", id);
                return NotFound();
            }
        }


        [Authorize(Roles = "Agence")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Agence")]
        public async Task<IActionResult> Create(VoitureViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }

   
                var voiture = new Voiture
                {
                    Marque = model.Marque,
                    Modele = model.Modele,
                    Annee = model.Annee,
                    Immatriculation = model.Immatriculation,
                    Categorie = model.Categorie,
                    TypeCarburant = model.TypeCarburant,
                    Transmission = model.Transmission,
                    NombrePlaces = model.NombrePlaces,
                    NombrePortes = model.NombrePortes,
                    Kilometrage = model.Kilometrage,
                    Couleur = model.Couleur,
                    EstDisponible = model.EstDisponible,
                    AgenceId = userId
                };

   
                if (model.PhotoPrincipaleFile != null)
                {
                    voiture.PhotoPrincipaleUrl = await _fileUploadService.UploadImageAsync(model.PhotoPrincipaleFile, "voitures");
                }
                else if (!string.IsNullOrEmpty(model.PhotoPrincipaleUrl))
                {
                    voiture.PhotoPrincipaleUrl = model.PhotoPrincipaleUrl;
                }
          
                if (model.PhotosFiles != null && model.PhotosFiles.Any())
                {
                    voiture.PhotosUrls = await _fileUploadService.UploadMultipleImagesAsync(model.PhotosFiles, "voitures");
                }


                if (model.AjouterFicheTechnique && 
                    (model.Puissance.HasValue || !string.IsNullOrEmpty(model.MoteurType)))
                {
                    voiture.FicheTechnique = new FicheTechnique
                    {
                        MoteurType = model.MoteurType ?? string.Empty,
                        Puissance = model.Puissance ?? 0,
                        Consommation = model.Consommation ?? 0,
                        EmissionCO2 = model.EmissionCO2 ?? 0,
                        CapaciteReservoir = model.CapaciteReservoir ?? 0,
                        VolumeCodeffre = model.VolumeCodeffre ?? 0,
                        Longueur = model.Longueur ?? 0,
                        Largeur = model.Largeur ?? 0,
                        Hauteur = model.Hauteur ?? 0,
                        PoidsVide = model.PoidsVide ?? 0,
                        Equipements = model.Equipements,
                        OptionsSecurite = model.OptionsSecurite
                    };
                }

                var success = await _voitureService.CreateVoitureAsync(voiture);
                
                if (success)
                {
                    TempData["Success"] = "Véhicule ajouté avec succès";
                    return RedirectToAction("Dashboard", "Agence");
                }

                ModelState.AddModelError("", "Erreur lors de l'ajout du véhicule");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création de la voiture");
                ModelState.AddModelError("", "Une erreur s'est produite");
                return View(model);
            }
        }

        [Authorize(Roles = "Agence")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var voiture = await _voitureService.GetVoitureWithDetailsAsync(id);
                if (voiture == null)
                {
                    return NotFound();
                }

                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (voiture.AgenceId != userId)
                {
                    return Forbid();
                }

                var model = new VoitureViewModel
                {
                    Id = voiture.Id,
                    Marque = voiture.Marque,
                    Modele = voiture.Modele,
                    Annee = voiture.Annee,
                    Immatriculation = voiture.Immatriculation,
                    Categorie = voiture.Categorie,
                    TypeCarburant = voiture.TypeCarburant,
                    Transmission = voiture.Transmission,
                    NombrePlaces = voiture.NombrePlaces,
                    NombrePortes = voiture.NombrePortes,
                    Kilometrage = voiture.Kilometrage,
                    Couleur = voiture.Couleur,
                    EstDisponible = voiture.EstDisponible,
                    PhotoPrincipaleUrl = voiture.PhotoPrincipaleUrl,
                    PhotosUrls = voiture.PhotosUrls,
                    AgenceId = voiture.AgenceId
                };

                if (voiture.FicheTechnique != null)
                {
                    model.AjouterFicheTechnique = true;
                    model.MoteurType = voiture.FicheTechnique.MoteurType;
                    model.Puissance = voiture.FicheTechnique.Puissance;
                    model.Consommation = voiture.FicheTechnique.Consommation;
                    model.EmissionCO2 = voiture.FicheTechnique.EmissionCO2;
                    model.CapaciteReservoir = voiture.FicheTechnique.CapaciteReservoir;
                    model.VolumeCodeffre = voiture.FicheTechnique.VolumeCodeffre;
                    model.Longueur = voiture.FicheTechnique.Longueur;
                    model.Largeur = voiture.FicheTechnique.Largeur;
                    model.Hauteur = voiture.FicheTechnique.Hauteur;
                    model.PoidsVide = voiture.FicheTechnique.PoidsVide;
                    model.Equipements = voiture.FicheTechnique.Equipements;
                    model.OptionsSecurite = voiture.FicheTechnique.OptionsSecurite;
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de la voiture {VoitureId} pour édition", id);
                return NotFound();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Agence")]
        public async Task<IActionResult> Edit(int id, VoitureViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                    var voiture = await _voitureService.GetVoitureWithDetailsAsync(id);

                    if (voiture == null || voiture.AgenceId != userId)
                    {
                        return Forbid();
                    }

     
                    voiture.Marque = model.Marque;
                    voiture.Modele = model.Modele;
                    voiture.Annee = model.Annee;
                    voiture.Immatriculation = model.Immatriculation;
                    voiture.Categorie = model.Categorie;
                    voiture.TypeCarburant = model.TypeCarburant;
                    voiture.Transmission = model.Transmission;
                    voiture.NombrePlaces = model.NombrePlaces;
                    voiture.NombrePortes = model.NombrePortes;
                    voiture.Kilometrage = model.Kilometrage;
                    voiture.Couleur = model.Couleur;
                    voiture.EstDisponible = model.EstDisponible;

         
                    if (model.PhotoPrincipaleFile != null)
                    {
                        voiture.PhotoPrincipaleUrl = await _fileUploadService.UploadImageAsync(model.PhotoPrincipaleFile, "voitures");
                    }
                    else if (!string.IsNullOrEmpty(model.PhotoPrincipaleUrl))
                    {
                        voiture.PhotoPrincipaleUrl = model.PhotoPrincipaleUrl;
                    }

     
                    if (model.PhotosFiles != null && model.PhotosFiles.Any())
                    {
                        var newPhotos = await _fileUploadService.UploadMultipleImagesAsync(model.PhotosFiles, "voitures");
              
                        if (voiture.PhotosUrls == null)
                        {
                            voiture.PhotosUrls = newPhotos;
                        }
                        else
                        {
                            voiture.PhotosUrls.AddRange(newPhotos);
                        }
                    }


                    if (model.AjouterFicheTechnique && 
                        (model.Puissance.HasValue || !string.IsNullOrEmpty(model.MoteurType)))
                    {
                        if (voiture.FicheTechnique == null)
                        {
                           
                            voiture.FicheTechnique = new FicheTechnique();
                        }


                        voiture.FicheTechnique.MoteurType = model.MoteurType ?? string.Empty;
                        voiture.FicheTechnique.Puissance = model.Puissance ?? 0;
                        voiture.FicheTechnique.Consommation = model.Consommation ?? 0;
                        voiture.FicheTechnique.EmissionCO2 = model.EmissionCO2 ?? 0;
                        voiture.FicheTechnique.CapaciteReservoir = model.CapaciteReservoir ?? 0;
                        voiture.FicheTechnique.VolumeCodeffre = model.VolumeCodeffre ?? 0;
                        voiture.FicheTechnique.Longueur = model.Longueur ?? 0;
                        voiture.FicheTechnique.Largeur = model.Largeur ?? 0;
                        voiture.FicheTechnique.Hauteur = model.Hauteur ?? 0;
                        voiture.FicheTechnique.PoidsVide = model.PoidsVide ?? 0;
                        voiture.FicheTechnique.Equipements = model.Equipements;
                        voiture.FicheTechnique.OptionsSecurite = model.OptionsSecurite;
                    }
                    else if (!model.AjouterFicheTechnique && voiture.FicheTechnique != null)
                    {
                        voiture.FicheTechnique = null;
                        voiture.FicheTechniqueId = null;
                    }

                    var success = await _voitureService.UpdateVoitureAsync(voiture);
                    if (success)
                    {
                        TempData["Success"] = "Voiture mise à jour avec succès !";
                        return RedirectToAction("Dashboard", "Agence");
                    }

                    ModelState.AddModelError("", "Erreur lors de la mise à jour de la voiture");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erreur lors de la mise à jour de la voiture {VoitureId}", id);
                    ModelState.AddModelError("", "Une erreur s'est produite");
                }
            }

            return View(model);
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Agence")]
        public async Task<IActionResult> ToggleActive(int id)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var voiture = await _voitureService.GetVoitureWithDetailsAsync(id);


                if (voiture == null || voiture.AgenceId != userId)
                {
                    return Forbid();
                }
                if (voiture != null && voiture.EstDisponible && voiture.OffresLocation.FirstOrDefault(o=>o.EstActive) != null)
                {
                    TempData["Error"] = "Impossible de désactiver une voiture dont une offre associé est active";
                    return RedirectToAction("Dashboard", "Agence");
                }

                var success = await _voitureService.ChangerStatutVoitureAsync(id, !voiture.EstDisponible);
                if (success)
                {
                    TempData["Success"] = !voiture.EstDisponible ? "Voiture désactivée" : "Voiture activée";
                }

                return RedirectToAction("Dashboard", "Agence");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du changement de statut de Voiture {VoitureId}", id);
                TempData["Error"] = "Une erreur s'est produite";
                return RedirectToAction("Dashboard", "Agence");
            }
        }
    


}
}