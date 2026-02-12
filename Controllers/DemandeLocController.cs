using AgenceLocationVoiture.Models;
using AgenceLocationVoiture.Services;
using AgenceLocationVoiture.Services.ServiceContracts;
using AgenceLocationVoiture.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgenceLocationVoiture.Controllers
{
    [Authorize]
    public class DemandeLocController : Controller
    {
        private readonly IDemandeLocService _demandeLocService;
        private readonly IOffreLocService _offreLocService;
        private readonly ILogger<DemandeLocController> _logger;

        public DemandeLocController(
            IDemandeLocService demandeLocService,
            IOffreLocService offreLocService,
            ILogger<DemandeLocController> logger)
        {
            _demandeLocService = demandeLocService;
            _offreLocService = offreLocService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }

                IEnumerable<DemandeLoc> demandes;

                if (User.IsInRole("Client"))
                {
                    demandes = await _demandeLocService.GetDemandesByClientAsync(userId);
                }
                else if (User.IsInRole("Agence"))
                {
                    demandes = await _demandeLocService.GetDemandesByAgenceAsync(userId);
                }
                else
                {
                    return RedirectToAction("Demandes", "Admin");
                }

                return View(demandes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des demandes");
                return View(new List<DemandeLoc>());
            }
        }


        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var demande = await _demandeLocService.GetDemandeWithDetailsAsync(id);
                if (demande == null)
                {
                    return NotFound();
                }

                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                // Vérifier les permissions
                if (!User.IsInRole("Admin") &&
                    demande.ClientId != userId &&
                    demande.OffreLoc?.AgenceId != userId)
                {
                    return Forbid();
                }

                return View(demande);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des détails de la demande {DemandeId}", id);
                return NotFound();
            }
        }

        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Create(int offreId)
        {
            try
            {
                var offre = await _offreLocService.GetOffreWithDetailsAsync(offreId);
                if (offre == null || !offre.EstActive)
                {
                    TempData["Error"] = "Cette offre n'est plus disponible";
                    return RedirectToAction("Index", "OffreLoc");
                }

                var model = new CreateDemandeLocViewModel
                {
                    OffreLocId = offreId,
                    DateDebut = DateTime.Now.Date.AddDays(1),
                    DateFin = offre.DateFin 
                };

                ViewBag.Offre = offre;

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement du formulaire de demande");
                return RedirectToAction("Index", "OffreLoc");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Create(CreateDemandeLocViewModel model)
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

                    var demande = new DemandeLoc
                    {
                        DateDebut = model.DateDebut,
                        DateFin = model.DateFin,
                        LieuPriseEnCharge = model.LieuPriseEnCharge,
                        LieuRetour = model.LieuRetour,
                        LivraisonDemandee = model.LivraisonDemandee,
                        AdresseLivraison = model.AdresseLivraison,
                        MessageClient = model.MessageClient,
                        ClientId = userId,
                        OffreLocId = model.OffreLocId
                    };

                    var success = await _demandeLocService.CreateDemandeAsync(demande);
                    if (success)
                    {
                        TempData["Success"] = "Demande envoyée avec succès ! L'agence vous répondra bientôt.";
                        return RedirectToAction(nameof(Index));
                    }

                    ModelState.AddModelError("", "Erreur lors de la création de la demande");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erreur lors de la création de la demande");
                    ModelState.AddModelError("", "Une erreur s'est produite");
                }
            }

            var offre = await _offreLocService.GetOffreWithDetailsAsync(model.OffreLocId);
            ViewBag.Offre = offre;

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Agence")]
        public async Task<IActionResult> Accept(int id, string reponseAgence, decimal montantTotal)
        {
            try
            {
                var success = await _demandeLocService.AccepterDemandeAsync(id, reponseAgence, montantTotal);
                if (success)
                {
                    TempData["Success"] = "Demande acceptée avec succès !";
                }
                else
                {
                    TempData["Error"] = "Erreur lors de l'acceptation de la demande";
                }

                return RedirectToAction(nameof(Details), new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'acceptation de la demande {DemandeId}", id);
                TempData["Error"] = "Une erreur s'est produite";
                return RedirectToAction(nameof(Index));
            }
        }

 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Agence")]
        public async Task<IActionResult> Refuse(int id, string reponseAgence)
        {
            try
            {
                var success = await _demandeLocService.RefuserDemandeAsync(id, reponseAgence);
                if (success)
                {
                    TempData["Success"] = "Demande refusée";
                }
                else
                {
                    TempData["Error"] = "Erreur lors du refus de la demande";
                }

                return RedirectToAction(nameof(Details), new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du refus de la demande {DemandeId}", id);
                TempData["Error"] = "Une erreur s'est produite";
                return RedirectToAction(nameof(Index));
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                var demande = await _demandeLocService.GetDemandeByIdAsync(id);
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (demande == null || demande.ClientId != userId)
                {
                    return Forbid();
                }

                var success = await _demandeLocService.AnnulerDemandeAsync(id);
                if (success)
                {
                    TempData["Success"] = "Demande annulée avec succès";
                }
                else
                {
                    TempData["Error"] = "Erreur lors de l'annulation de la demande";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'annulation de la demande {DemandeId}", id);
                TempData["Error"] = "Une erreur s'est produite";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}