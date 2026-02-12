using AgenceLocationVoiture.Models;
using AgenceLocationVoiture.Services;
using AgenceLocationVoiture.Services.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgenceLocationVoiture.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAgenceService _agenceService;
        private readonly IClientService _clientService;
        private readonly IVoitureService _voitureService;
        private readonly IOffreLocService _offreLocService;
        private readonly IDemandeLocService _demandeLocService;
        private readonly IAvisService _avisService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            IAgenceService agenceService,
            IClientService clientService,
            IVoitureService voitureService,
            IOffreLocService offreLocService,
            IDemandeLocService demandeLocService,
            IAvisService avisService,
            ILogger<AdminController> logger)
        {
            _agenceService = agenceService;
            _clientService = clientService;
            _voitureService = voitureService;
            _offreLocService = offreLocService;
            _demandeLocService = demandeLocService;
            _avisService = avisService;
            _logger = logger;
        }

       
        public async Task<IActionResult> Index()
        {
            try
            {
                var agences = await _agenceService.GetAllAgencesAsync();
                var clients = await _clientService.GetAllClientsAsync();
                var voitures = await _voitureService.GetAllVoituresAsync();
                var offres = await _offreLocService.GetAllOffresAsync();
                var demandes = await _demandeLocService.GetAllDemandesAsync();
                var avis = await _avisService.GetAllAvisAsync();

                ViewBag.NombreAgences = agences.Count();
                ViewBag.NombreClients = clients.Count();
                ViewBag.NombreVoitures = voitures.Count();
                ViewBag.NombreOffres = offres.Count();
                ViewBag.NombreDemandes = demandes.Count();
                ViewBag.NombreAvis = avis.Count();

                ViewBag.DemandesEnAttente = await _demandeLocService.GetDemandesByStatutAsync(StatutDemande.EnAttente);
                ViewBag.AvisNonVerifies = avis.Where(a => !a.EstVerifie).ToList();
                ViewBag.AgencesNonVerifiees = agences.Where(a => !a.EstVerifiee).ToList(); 

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement du tableau de bord admin");
                return View();
            }
        }

       
        public async Task<IActionResult> Agences()
        {
            try
            {
                var agences = await _agenceService.GetAllAgencesAsync();
                return View(agences);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des agences");
                return View(new List<Models.Agence>());
            }
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyAgence(string id)
        {
            try
            {
                var success = await _agenceService.VerifierAgenceAsync(id);
                if (success)
                {
                    TempData["Success"] = "Agence vérifiée avec succès";
                }
                else
                {
                    TempData["Error"] = "Erreur lors de la vérification de l'agence";
                }

                return RedirectToAction(nameof(Agences));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la vérification de l'agence {AgenceId}", id);
                TempData["Error"] = "Une erreur s'est produite";
                return RedirectToAction(nameof(Agences));
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAgence(string id)
        {
            try
            {
                var success = await _agenceService.DeleteAgenceAsync(id);
                if (success)
                {
                    TempData["Success"] = "Agence désactivée avec succès";
                }
                else
                {
                    TempData["Error"] = "Erreur lors de la désactivation de l'agence";
                }

                return RedirectToAction(nameof(Agences));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la désactivation de l'agence {AgenceId}", id);
                TempData["Error"] = "Une erreur s'est produite";
                return RedirectToAction(nameof(Agences));
            }
        }

    
        public async Task<IActionResult> Clients()
        {
            try
            {
                var clients = await _clientService.GetAllClientsAsync();
                return View(clients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des clients");
                return View(new List<Models.Client>());
            }
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyClient(string id)
        {
            try
            {
                var success = await _clientService.VerifierPermisAsync(id);
                if (success)
                {
                    TempData["Success"] = "Permis du client vérifié avec succès";
                }
                else
                {
                    TempData["Error"] = "Erreur lors de la vérification du permis";
                }

                return RedirectToAction(nameof(Clients));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la vérification du permis du client {ClientId}", id);
                TempData["Error"] = "Une erreur s'est produite";
                return RedirectToAction(nameof(Clients));
            }
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteClient(string id)
        {
            try
            {
                var success = await _clientService.DeleteClientAsync(id);
                if (success)
                {
                    TempData["Success"] = "Client désactivé avec succès";
                }
                else
                {
                    TempData["Error"] = "Erreur lors de la désactivation du client";
                }

                return RedirectToAction(nameof(Clients));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la désactivation du client {ClientId}", id);
                TempData["Error"] = "Une erreur s'est produite";
                return RedirectToAction(nameof(Clients));
            }
        }

       
        public async Task<IActionResult> Voitures()
        {
            try
            {
                var voitures = await _voitureService.GetAllVoituresAsync();
                return View(voitures);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des voitures");
                return View(new List<Models.Voiture>());
            }
        }

        
        public async Task<IActionResult> Offres()
        {
            try
            {
                var offres = await _offreLocService.GetAllWithDetailsAsync();
                return View(offres);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des offres");
                return View(new List<Models.OffreLoc>());
            }
        }

      
        public async Task<IActionResult> Demandes()
        {
            try
            {
                var demandes = await _demandeLocService.GetAllDemandesWithDetailsAsync();
                return View(demandes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération des demandes");
                return View(new List<Models.DemandeLoc>());
            }
        }

        

     

    
        public async Task<IActionResult> Statistics()
        {
            try
            {
                var agences = await _agenceService.GetAllAgencesAsync();
                var clients = await _clientService.GetAllClientsAsync();
                var voitures = await _voitureService.GetAllVoituresAsync();
                var offres = await _offreLocService.GetAllOffresAsync();
                var demandes = await _demandeLocService.GetAllDemandesAsync();

                ViewBag.AgencesVerifiees = agences.Count(a => a.EstVerifiee);
                ViewBag.ClientsVerifies = clients.Count(c => c.PermisVerifie);
                ViewBag.VoituresDisponibles = voitures.Count(v => v.EstDisponible);
                ViewBag.OffresActives = offres.Count(o => o.EstActive);
                ViewBag.DemandesEnAttente = demandes.Count(d => d.Statut == Models.StatutDemande.EnAttente);
                ViewBag.DemandesAcceptees = demandes.Count(d => d.Statut == Models.StatutDemande.Acceptee);
                ViewBag.AgencesNonVerifiees = agences.Count(a => !a.EstVerifiee);
                ViewBag.ClientsNonVerifies = clients.Count(c => !c.PermisVerifie);
                ViewBag.OffresInactives = offres.Count(o => !o.EstActive);

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du chargement des statistiques");
                return View();
            }
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleAgenceStatus(string id)
        {
            try
            {
                var agence = await _agenceService.GetAgenceByIdAsync(id);
                if (agence == null)
                {
                    TempData["Error"] = "Agence introuvable";
                    return RedirectToAction(nameof(Agences));
                }

              
                bool nouveauStatut = !agence.EstActif;
                
                if (nouveauStatut)
                {
                    
                    var success = await _agenceService.ActivateAgenceAsync(id);
                    if (success)
                    {
                        TempData["Success"] = $"Agence {agence.NomAgence} activée avec succès";
                    }
                    else
                    {
                        TempData["Error"] = "Erreur lors de l'activation de l'agence";
                    }
                }
                else
                {
               
                    var success = await _agenceService.DeleteAgenceAsync(id);
                    if (success)
                    {
                        TempData["Success"] = $"Agence {agence.NomAgence} désactivée avec succès";
                    }
                    else
                    {
                        TempData["Error"] = "Erreur lors de la désactivation de l'agence";
                    }
                }

                return RedirectToAction(nameof(Agences));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du changement de statut de l'agence {AgenceId}", id);
                TempData["Error"] = "Une erreur s'est produite";
                return RedirectToAction(nameof(Agences));
            }
        }

 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleClientStatus(string id)
        {
            try
            {
                var client = await _clientService.GetClientByIdAsync(id);
                if (client == null)
                {
                    TempData["Error"] = "Client introuvable";
                    return RedirectToAction(nameof(Clients));
                }

              
                bool nouveauStatut = !client.EstActif;
                
                if (nouveauStatut)
                {
                
                    var success = await _clientService.ActivateClientAsync(id);
                    if (success)
                    {
                        TempData["Success"] = $"Client {client.Prenom} {client.Nom} activé avec succès";
                    }
                    else
                    {
                        TempData["Error"] = "Erreur lors de l'activation du client";
                    }
                }
                else
                {
                  
                    var success = await _clientService.DeleteClientAsync(id);
                    if (success)
                    {
                        TempData["Success"] = $"Client {client.Prenom} {client.Nom} désactivé avec succès";
                    }
                    else
                    {
                        TempData["Error"] = "Erreur lors de la désactivation du client";
                    }
                }

                return RedirectToAction(nameof(Clients));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du changement de statut du client {ClientId}", id);
                TempData["Error"] = "Une erreur s'est produite";
                return RedirectToAction(nameof(Clients));
            }
        }
    }
}