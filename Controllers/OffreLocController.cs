using AgenceLocationVoiture.Models;
using AgenceLocationVoiture.Services;
using AgenceLocationVoiture.ViewModels;
using AgenceLocationVoiture.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AgenceLocationVoiture.Services.ServiceContracts;

namespace AgenceLocationVoiture.Controllers
{
    public class OffreLocController : Controller
    {
        private readonly IOffreLocService _offreLocService;
        private readonly IVoitureService _voitureService;
        private readonly IClientService _clientService;
        private readonly ILogger<OffreLocController> _logger;

        public OffreLocController(
            IOffreLocService offreLocService,
            IVoitureService voitureService,
            ILogger<OffreLocController> logger,
            IClientService clientService)
        {
            _offreLocService = offreLocService;
            _voitureService = voitureService;
            _logger = logger;
            _clientService = clientService;
        }

      
        public async Task<IActionResult> Index(string? ville = null, DateTime? dateDebut = null, DateTime? dateFin = null, int pageNumber = 1)
        {
            try
            {
                const int pageSize = 3; 
                
                IEnumerable<OffreLoc> offres;

                
                if (!string.IsNullOrEmpty(ville) || dateDebut.HasValue || dateFin.HasValue)
                {
                    offres = await _offreLocService.SearchOffresAsync(dateDebut, dateFin, null, ville);
                }
                else
                {
                   
                    offres = await _offreLocService.GetOffresActivesAsync();
                }

                
                var paginatedList = PaginatedList<OffreLoc>.Create(offres, pageNumber, pageSize);

           
                ViewBag.CurrentVille = ville;
                ViewBag.CurrentDateDebut = dateDebut?.ToString("yyyy-MM-dd");
                ViewBag.CurrentDateFin = dateFin?.ToString("yyyy-MM-dd");

                return View(paginatedList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des offres");
                return View(new PaginatedList<OffreLoc>(new List<OffreLoc>(), 0, 1, 9));
            }
        }

  
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var offre = await _offreLocService.GetOffreWithDetailsAsync(id);

                if (offre == null)
                {
                    return NotFound();
                }

                if (User.IsInRole("Client"))
                {
                    var clientId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                    if (!string.IsNullOrEmpty(clientId))
                    {
                        var client = await _clientService.GetClientByIdAsync(clientId);
                        ViewBag.CurrentClient = client;
                    }
                }

                return View(offre);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des détails de l'offre {OffreId}", id);
                return NotFound();
            }
        }

        public async Task<IActionResult> Search(RechercheViewModel model)
        {
            try
            {
                model = await _offreLocService.FilterResearch(model);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la recherche d'offres");
                return View(new RechercheViewModel());
            }
        }

 
        [Authorize(Roles = "Agence")]
        public async Task<IActionResult> Create()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }

                var voitures = await _voitureService.GetVoituresByAgenceAsync(userId);
                ViewBag.Voitures = voitures;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement du formulaire de création d'offre");
                return RedirectToAction("Dashboard", "Agence");
            }
        }

 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Agence")]
        public async Task<IActionResult> Create(OffreLocViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userId))
                    {
                        return RedirectToAction("Login", "Account");
                    }

                    var offre = new OffreLoc
                    {
                        Titre = model.Titre,
                        Description = model.Description,
                        PrixParJour = model.PrixParJour,
                       
                        CautionMontant = model.CautionMontant,
                        KilometrageInclus = model.KilometrageInclus,
                        FraisKmSupplementaire = model.FraisKmSupplementaire,
                        AgeMinimumConducteur = model.AgeMinimumConducteur,
                        AnciennetePermisMinimum = model.AnciennetePermisMinimum,
                        AssuranceIncluse = model.AssuranceIncluse,
                        LivraisonPossible = model.LivraisonPossible,
                        FraisLivraison = model.FraisLivraison,
                        DateDebut = model.DateDebut,
                        DateFin = model.DateFin,
                        VoitureId = model.VoitureId,
                        AgenceId = userId
                    };

                    var success = await _offreLocService.CreateOffreAsync(offre);
                    if (success)
                    {
                        TempData["Success"] = "Offre créée avec succès !";
                        return RedirectToAction("Dashboard", "Agence");
                    }

                    ModelState.AddModelError("", "Erreur lors de la création de l'offre");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erreur lors de la création de l'offre");
                    ModelState.AddModelError("", "Une erreur s'est produite");
                }
            }

            var userId2 = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var voitures = await _voitureService.GetVoituresByAgenceAsync(userId2!);
            ViewBag.Voitures = voitures;

            return View(model);
        }

 
        [Authorize(Roles = "Agence")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var offre = await _offreLocService.GetOffreByIdAsync(id);
                if (offre == null)
                {
                    return NotFound();
                }

                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (offre.AgenceId != userId)
                {
                    return Forbid();
                }

                var model = new OffreLocViewModel
                {
                    Id = offre.Id,
                    Titre = offre.Titre,
                    Description = offre.Description,
                    PrixParJour = offre.PrixParJour,
                  
                    CautionMontant = offre.CautionMontant,
                    KilometrageInclus = offre.KilometrageInclus,
                    FraisKmSupplementaire = offre.FraisKmSupplementaire,
                    AgeMinimumConducteur = offre.AgeMinimumConducteur,
                    AnciennetePermisMinimum = offre.AnciennetePermisMinimum,
                    AssuranceIncluse = offre.AssuranceIncluse,
                    LivraisonPossible = offre.LivraisonPossible,
                    FraisLivraison = offre.FraisLivraison,
                    DateDebut = offre.DateDebut,
                    DateFin = offre.DateFin,
                    EstActive = offre.EstActive,
                    VoitureId = offre.VoitureId,
                    AgenceId = offre.AgenceId
                };

                var voitures = await _voitureService.GetVoituresByAgenceAsync(userId!);
                ViewBag.Voitures = voitures;

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de l'offre {OffreId} pour édition", id);
                return NotFound();
            }
        }

  
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Agence")]
        public async Task<IActionResult> Edit(int id, OffreLocViewModel model)
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
                    var offre = await _offreLocService.GetOffreByIdAsync(id);

                    if (offre == null || offre.AgenceId != userId)
                    {
                        return Forbid();
                    }

                    offre.Titre = model.Titre;
                    offre.Description = model.Description;
                    offre.PrixParJour = model.PrixParJour;
                   
                    offre.CautionMontant = model.CautionMontant;
                    offre.KilometrageInclus = model.KilometrageInclus;
                    offre.FraisKmSupplementaire = model.FraisKmSupplementaire;
                    offre.AgeMinimumConducteur = model.AgeMinimumConducteur;
                    offre.AnciennetePermisMinimum = model.AnciennetePermisMinimum;
                    offre.AssuranceIncluse = model.AssuranceIncluse;
                    offre.LivraisonPossible = model.LivraisonPossible;
                    offre.FraisLivraison = model.FraisLivraison;
                    offre.DateDebut = model.DateDebut;
                    offre.DateFin = model.DateFin;
                    offre.EstActive = model.EstActive;
                    offre.VoitureId = model.VoitureId;

                    var success = await _offreLocService.UpdateOffreAsync(offre);
                    if (success)
                    {
                        TempData["Success"] = "Offre mise à jour avec succès !";
                        return RedirectToAction("Dashboard", "Agence");
                    }

                    ModelState.AddModelError("", "Erreur lors de la mise à jour de l'offre");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erreur lors de la mise à jour de l'offre {OffreId}", id);
                    ModelState.AddModelError("", "Une erreur s'est produite");
                }
            }

            var userId2 = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var voitures = await _voitureService.GetVoituresByAgenceAsync(userId2!);
            ViewBag.Voitures = voitures;

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
                var offre = await _offreLocService.GetOffreWithDetailsAsync(id);
                

                if (offre == null || offre.AgenceId != userId)
                {
                    return Forbid();
                }
                if (offre != null && !offre.EstActive && offre.Voiture != null && !offre.Voiture.EstDisponible)
                {
                    TempData["Error"] = "Impossible d'activer une offre dont la voiture n'est pas disponible.";
                    return RedirectToAction("Dashboard", "Agence");
                }

                var success = await _offreLocService.ChangerStatutOffreAsync(id, !offre.EstActive);
                if (success)
                {
                    TempData["Success"] = !offre.EstActive ? "Offre désactivée" : "Offre activée";
                }

                return RedirectToAction("Dashboard", "Agence");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du changement de statut de l'offre {OffreId}", id);
                TempData["Error"] = "Une erreur s'est produite";
                return RedirectToAction("Dashboard", "Agence");
            }
        }
    }
}