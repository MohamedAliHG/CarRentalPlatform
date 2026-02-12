using AgenceLocationVoiture.Models;
using AgenceLocationVoiture.Services;
using AgenceLocationVoiture.Services.ServiceContracts;
using AgenceLocationVoiture.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgenceLocationVoiture.Controllers
{
    public class AvisController : Controller
    {
        private readonly IAvisService _avisService;
        private readonly IAgenceService _agenceService;
        private readonly ILogger<AvisController> _logger;

        public AvisController(
            IAvisService avisService,
            IAgenceService agenceService,
            ILogger<AvisController> logger)
        {
            _avisService = avisService;
            _agenceService = agenceService;
            _logger = logger;
        }

        

        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Create(string agenceId)
        {
            try
            {
                var agence = await _agenceService.GetAgenceByIdAsync(agenceId);
                if (agence == null)
                {
                    TempData["Error"] = "Agence introuvable";
                    return RedirectToAction("Index", "Agence");
                }

                var model = new CreateAvisViewModel
                {
                    AgenceId = agenceId
                };

                ViewBag.Agence = agence;

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement du formulaire d'avis");
                return RedirectToAction("Index", "Agence");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Create(CreateAvisViewModel model)
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

                    var avis = new Avis
                    {
                        Note = model.Note,
                        Commentaire = model.Commentaire,
                        ClientId = userId,
                        AgenceId = model.AgenceId
                    };

                    var success = await _avisService.CreateAvisAsync(avis);
                    if (success)
                    {
                        TempData["Success"] = "Merci pour votre avis !";
                        return RedirectToAction("Details", "Agence", new { id = model.AgenceId });
                    }

                    ModelState.AddModelError("", "Erreur lors de la création de l'avis");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erreur lors de la création de l'avis");
                    ModelState.AddModelError("", "Une erreur s'est produite");
                }
            }

            var agence = await _agenceService.GetAgenceByIdAsync(model.AgenceId);
            ViewBag.Agence = agence;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Agence")]
        public async Task<IActionResult> Respond(int id, string reponseAgence)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(reponseAgence))
                {
                    TempData["Error"] = "La réponse ne peut pas être vide";
                    return RedirectToAction("Dashboard", "Agence");
                }

                var avis = await _avisService.GetAvisByIdAsync(id);
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (avis == null || avis.AgenceId != userId)
                {
                    return Forbid();
                }

                var success = await _avisService.RepondreAvisAsync(id, reponseAgence);
                if (success)
                {
                    TempData["Success"] = "Réponse publiée avec succès";
                }
                else
                {
                    TempData["Error"] = "Erreur lors de la publication de la réponse";
                }

                return RedirectToAction("Dashboard", "Agence");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la réponse à l'avis {AvisId}", id);
                TempData["Error"] = "Une erreur s'est produite";
                return RedirectToAction("Dashboard", "Agence");
            }
        }

       
    }
}