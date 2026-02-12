using System.Diagnostics;
using AgenceLocationVoiture.Services.ServiceContracts;
using AgenceLocationVoiture.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AgenceLocationVoiture.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOffreLocService _offreLocService;
        private readonly IAgenceService _agenceService;
        private readonly IAvisService _avisService;
        private readonly IEmailService _emailService;

        public HomeController(
            ILogger<HomeController> logger,
            IOffreLocService offreLocService,
            IAgenceService agenceService,
            IAvisService avisService,
            IEmailService emailService)
        {
            _logger = logger;
            _offreLocService = offreLocService;
            _agenceService = agenceService;
            _avisService = avisService;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index(string? ville = null, DateTime? dateDebut = null, DateTime? dateFin = null)
        {
            try
            {
                
                bool hasSearched = !string.IsNullOrEmpty(ville) || dateDebut.HasValue || dateFin.HasValue;
                ViewBag.HasSearched = hasSearched;

                if (hasSearched)
                {
                    
                    var offresRecherche = await _offreLocService.SearchOffresAsync(
                        dateDebut,
                        dateFin,
                        null,
                        ville
                    );

                    ViewBag.OffresRecherche = offresRecherche.ToList();
                }
                else
                {
                  
                    var offres = await _offreLocService.GetOffresActivesAsync();
                    var offresVedette = offres.Take(6).ToList();

                                

                    ViewBag.OffresVedette = offresVedette;
                   
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement de la page d'accueil");
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(string nom, string email, string message)
        {
            if (string.IsNullOrWhiteSpace(nom) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(message))
            {
                TempData["Error"] = "Tous les champs sont obligatoires.";
                return RedirectToAction(nameof(Contact));
            }

            var emailSent = await _emailService.SendContactEmailAsync(nom, email, message);
            
            if (emailSent)
            {
                TempData["Message"] = "Votre message a été envoyé avec succès !";
            }
            else
            {
                TempData["Error"] = "Une erreur s'est produite lors de l'envoi du message.";
            }
            
            return RedirectToAction(nameof(Contact));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
