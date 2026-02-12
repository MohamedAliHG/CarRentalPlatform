using AgenceLocationVoiture.Models;
using Microsoft.AspNetCore.Identity;
using AgenceLocationVoiture.Services.ServiceContracts;

namespace AgenceLocationVoiture.Services.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<Utilisateur> _userManager;
        private readonly SignInManager<Utilisateur> _signInManager;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(
            UserManager<Utilisateur> userManager,
            SignInManager<Utilisateur> signInManager,
            ILogger<AuthenticationService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<bool> RegisterAgenceAsync(Agence agence, string password)
        {
            try
            {
                agence.UserName = agence.Email;
                agence.DateInscription = DateTime.Now;
                agence.EstActif = true;
                agence.EstVerifiee = false;
                agence.TypeUtilisateur = "Agence";

                var result = await _userManager.CreateAsync(agence, password);
                
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(agence, "Agence");
                    _logger.LogInformation("Agence {AgenceNom} enregistrée avec succès", agence.NomAgence);
                    return true;
                }

                foreach (var error in result.Errors)
                {
                    _logger.LogWarning("Erreur lors de l'enregistrement de l'agence: {Error}", error.Description);
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'enregistrement de l'agence");
                return false;
            }
        }

        public async Task<bool> RegisterClientAsync(Client client, string password)
        {
            try
            {
                client.UserName = client.Email;
                client.DateInscription = DateTime.Now;
                client.EstActif = true;
                client.PermisVerifie = false;
                client.TypeUtilisateur = "Client";

                var result = await _userManager.CreateAsync(client, password);
                
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(client, "Client");
                    _logger.LogInformation("Client {ClientNom} enregistré avec succès", 
                        $"{client.Prenom} {client.Nom}");
                    return true;
                }

                foreach (var error in result.Errors)
                {
                    _logger.LogWarning("Erreur lors de l'enregistrement du client: {Error}", error.Description);
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'enregistrement du client");
                return false;
            }
        }

        public async Task<Utilisateur?> LoginAsync(string email, string password)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    _logger.LogWarning("Tentative de connexion avec un email inexistant: {Email}", email);
                    return null;
                }

                if (!user.EstActif)
                {
                    _logger.LogWarning("Tentative de connexion avec un compte désactivé: {Email}", email);
                    return null;
                }

                var result = await _signInManager.PasswordSignInAsync(
                    user.UserName!, 
                    password, 
                    isPersistent: false, 
                    lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Utilisateur {Email} connecté avec succès", email);
                    return user;
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Compte verrouillé: {Email}", email);
                }
                else if (result.RequiresTwoFactor)
                {
                    _logger.LogInformation("Authentification à deux facteurs requise pour {Email}", email);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la connexion de l'utilisateur {Email}", email);
                return null;
            }
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
                _logger.LogInformation("Utilisateur déconnecté avec succès");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la déconnexion");
                return false;
            }
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return false;
                }

                return await _userManager.IsInRoleAsync(user, role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la vérification du rôle pour l'utilisateur {UserId}", userId);
                return false;
            }
        }

        
    }
}