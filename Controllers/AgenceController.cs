using AgenceLocationVoiture.Services;
using AgenceLocationVoiture.ViewModels;
using AgenceLocationVoiture.Helpers;
using AgenceLocationVoiture.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AgenceLocationVoiture.Services.ServiceContracts;

namespace AgenceLocationVoiture.Controllers
{
    public class AgenceController : Controller
    {
        private readonly IAgenceService _agenceService;
        private readonly IVoitureService _voitureService;
        private readonly IOffreLocService _offreLocService;
        private readonly IAvisService _avisService;
        private readonly IFileUploadService _fileUploadService;
        private readonly ILogger<AgenceController> _logger;

        public AgenceController(
            IAgenceService agenceService,
            IVoitureService voitureService,
            IOffreLocService offreLocService,
            IAvisService avisService,
            IFileUploadService fileUploadService,
            ILogger<AgenceController> logger)
        {
            _agenceService = agenceService;
            _voitureService = voitureService;
            _offreLocService = offreLocService;
            _avisService = avisService;
            _fileUploadService = fileUploadService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string? search = null, string? ville = null, int pageNumber = 1)
        {
            try
            {
                const int pageSize = 12;

                IEnumerable<Agence> agences;

                if (!string.IsNullOrEmpty(search))
                {
                    agences = await _agenceService.SearchAgencesAsync(search);
                }
                else if (!string.IsNullOrEmpty(ville))
                {
                    agences = await _agenceService.GetAgencesByVilleAsync(ville);
                }
                else
                {
                    agences = await _agenceService.GetAgencesVerifieesAsync();
                }

                var agencesViewModels = new List<AgenceViewModel>();
                foreach (var agence in agences.Where(a => a.EstActif))
                {
                    var noteMoyenne = await _agenceService.GetNoteMoyenneAsync(agence.Id);
                    var voitures = await _voitureService.GetVoituresDispoByAgenceAsync(agence.Id);
                    var offres = await _offreLocService.GetOffresActiveByAgenceAsync(agence.Id);

                    agencesViewModels.Add(new AgenceViewModel
                    {
                        Id = agence.Id,
                        NomAgence = agence.NomAgence,
                        Ville = agence.Ville,
                        CodePostal = agence.CodePostal,
                        Telephone = agence.PhoneNumber,
                        LogoUrl = agence.LogoUrl,
                        Description = agence.Description,
                        EstVerifiee = agence.EstVerifiee,
                        NoteMoyenne = noteMoyenne,
                        NombreAvis = agence.AvisRecus?.Count ?? 0,
                        NombreVoitures = voitures.Count(),
                        NombreOffres = offres.Count()
                    });
                }

                var paginatedList = PaginatedList<AgenceViewModel>.Create(agencesViewModels, pageNumber, pageSize);

                ViewBag.CurrentSearch = search;
                ViewBag.CurrentVille = ville;

                return View(paginatedList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des agences");
                return View(new PaginatedList<AgenceViewModel>(new List<AgenceViewModel>(), 0, 1, 12));
            }
        }

        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var agence = await _agenceService.GetAgenceWithDetailsAsync(id);
                if (agence == null)
                {
                    return NotFound();
                }

                var noteMoyenne = await _agenceService.GetNoteMoyenneAsync(id);
                var avis = await _avisService.GetAvisByAgenceAsync(id);
                var voitures = await _voitureService.GetVoituresDispoByAgenceAsync(id);
                var offres = await _offreLocService.GetOffresActiveByAgenceAsync(id);

                var viewModel = new AgenceViewModel
                {
                    Id = agence.Id,
                    NomAgence = agence.NomAgence,
                    Description = agence.Description,
                    Ville = agence.Ville,
                    CodePostal = agence.CodePostal,
                    Adresse = agence.Adresse,
                    Email = agence.Email!,
                    Telephone = agence.PhoneNumber!,
                    SiteWeb = agence.SiteWeb,
                    LogoUrl = agence.LogoUrl,
                    EstVerifiee = agence.EstVerifiee,
                    NoteMoyenne = noteMoyenne,
                    NombreAvis = avis.Count(),
                    NombreVoitures = voitures.Count(),
                    NombreOffres = offres.Count()
                };

                ViewBag.Avis = avis;
                ViewBag.Voitures = voitures;
                ViewBag.Offres = offres;

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des détails de l'agence {AgenceId}", id);
                return NotFound();
            }
        }

        [Authorize(Roles = "Agence")]
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }

                var agence = await _agenceService.GetAgenceWithDetailsAsync(userId);
                if (agence == null)
                {
                    return NotFound();
                }

                var voitures = await _voitureService.GetVoituresByAgenceAsync(userId);
                var offres = await _offreLocService.GetOffresByAgenceAsync(userId);
                var avis = await _avisService.GetAvisByAgenceAsync(userId);
                var noteMoyenne = await _agenceService.GetNoteMoyenneAsync(userId);

                ViewBag.Voitures = voitures;
                ViewBag.Offres = offres;
                ViewBag.Avis = avis;
                ViewBag.NoteMoyenne = noteMoyenne;

                return View(agence);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement du tableau de bord agence");
                return RedirectToAction("Index", "Home");
            }
        }


        [Authorize(Roles = "Agence")]
        public async Task<IActionResult> Edit()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }

                var agence = await _agenceService.GetAgenceWithDetailsAsync(userId);
                if (agence == null)
                {
                    return NotFound();
                }

                var viewModel = new AgenceEditViewModel
                {
                    Id = agence.Id,
                    NomAgence = agence.NomAgence,
                    Description = agence.Description,
                    Adresse = agence.Adresse,
                    Ville = agence.Ville,
                    CodePostal = agence.CodePostal,
                    Telephone = agence.PhoneNumber ?? string.Empty,
                    SiteWeb = agence.SiteWeb,
                    LogoUrl = agence.LogoUrl
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement du formulaire de modification");
                return RedirectToAction(nameof(Dashboard));
            }
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Agence")]
        public async Task<IActionResult> Edit(AgenceEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId) || userId != model.Id)
                {
                    return Forbid();
                }

                var agence = await _agenceService.GetAgenceByIdAsync(userId);
                if (agence == null)
                {
                    return NotFound();
                }

                agence.NomAgence = model.NomAgence;
                agence.Description = model.Description ?? string.Empty;
                agence.Adresse = model.Adresse;
                agence.Ville = model.Ville;
                agence.CodePostal = model.CodePostal;
                agence.PhoneNumber = model.Telephone;
                agence.SiteWeb = model.SiteWeb ?? string.Empty;
               

                if (model.LogoFile != null)
                {
                    agence.LogoUrl = await _fileUploadService.UploadImageAsync(model.LogoFile, "agences");
                }
                else if (!string.IsNullOrEmpty(model.LogoUrl))
                {
                    agence.LogoUrl = model.LogoUrl;
                }

                var success = await _agenceService.UpdateAgenceAsync(agence);
                if (success)
                {
                    TempData["Success"] = "Informations mises à jour avec succès !";
                    return RedirectToAction(nameof(Dashboard));
                }

                ModelState.AddModelError("", "Erreur lors de la mise à jour");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la modification de l'agence");
                ModelState.AddModelError("", "Une erreur s'est produite");
                return View(model);
            }
        }
    }
    }