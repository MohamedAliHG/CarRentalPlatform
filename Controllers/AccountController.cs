using AgenceLocationVoiture.Models;
using AgenceLocationVoiture.Services;
using AgenceLocationVoiture.Services.ServiceContracts;
using AgenceLocationVoiture.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgenceLocationVoiture.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IFileUploadService _fileUploadService; 
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IAuthenticationService authenticationService,
            IFileUploadService fileUploadService, 
            ILogger<AccountController> logger)
        {
            _authenticationService = authenticationService;
            _fileUploadService = fileUploadService; 
            _logger = logger;
        }

        
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _authenticationService.LoginAsync(model.Email, model.Password);
                    if (user != null)
                    {
                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }

                      
                        if (await _authenticationService.IsInRoleAsync(user.Id, "Admin"))
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        else if (await _authenticationService.IsInRoleAsync(user.Id, "Agence"))
                        {
                            return RedirectToAction("Dashboard", "Agence");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }

                    ModelState.AddModelError("", "Email ou mot de passe incorrect");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erreur lors de la connexion");
                    ModelState.AddModelError("", "Une erreur s'est produite");
                }
            }

            return View(model);
        }

      
        [AllowAnonymous]
        public IActionResult RegisterAgence()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAgence(AgenceRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var agence = new Agence
                    {
                        Email = model.Email,
                        PhoneNumber = model.Telephone,
                        Nom = model.Nom,
                        Prenom = model.Prenom,
                        NomAgence = model.NomAgence,
                        NumeroSiret = model.NumeroSiret,
                        Adresse = model.Adresse,
                        Ville = model.Ville,
                        CodePostal = model.CodePostal,
                        Description = model.Description ?? string.Empty,
                        SiteWeb = model.SiteWeb ?? string.Empty,
                    };

                    
                    if (model.LogoFile != null)
                    {
                        agence.LogoUrl = await _fileUploadService.UploadImageAsync(model.LogoFile, "agences");
                    }
                    else if (!string.IsNullOrEmpty(model.LogoUrl))
                    {
                        agence.LogoUrl = model.LogoUrl;
                    }

                    var success = await _authenticationService.RegisterAgenceAsync(agence, model.Password);
                    if (success)
                    {
                        TempData["Success"] = "Inscription réussie ! Votre compte sera vérifié par un administrateur.";
                        return RedirectToAction(nameof(Login));
                    }

                    ModelState.AddModelError("", "Erreur lors de l'inscription");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erreur lors de l'inscription d'une agence");
                    ModelState.AddModelError("", "Une erreur s'est produite");
                }
            }

            return View(model);
        }

     
        [AllowAnonymous]
        public IActionResult RegisterClient()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

      
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterClient(ClientRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var client = new Client
                    {
                        Email = model.Email,
                        PhoneNumber = model.Telephone,
                        Nom = model.Nom,
                        Prenom = model.Prenom,
                        DateNaissance = model.DateNaissance,
                        NumeroPermis = model.NumeroPermis,
                        DateObtentionPermis = model.DateObtentionPermis,
                        Adresse = model.Adresse,
                        Ville = model.Ville,
                        CodePostal = model.CodePostal
                    };

                    var success = await _authenticationService.RegisterClientAsync(client, model.Password);
                    if (success)
                    {
                        TempData["Success"] = "Inscription réussie ! Vous pouvez maintenant vous connecter.";
                        return RedirectToAction(nameof(Login));
                    }

                    ModelState.AddModelError("", "Erreur lors de l'inscription");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erreur lors de l'inscription d'un client");
                    ModelState.AddModelError("", "Une erreur s'est produite");
                }
            }

            return View(model);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _authenticationService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

       
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}