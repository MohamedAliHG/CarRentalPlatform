using AgenceLocationVoiture.Models;
using AgenceLocationVoiture.Repositories;
using AgenceLocationVoiture.Services.ServiceContracts;

namespace AgenceLocationVoiture.Services.Services
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ClientService> _logger;

        public ClientService(IUnitOfWork unitOfWork, ILogger<ClientService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Client?> GetClientByIdAsync(string id)
        {
            try
            {
                return await _unitOfWork.Clients.FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération du client {ClientId}", id);
                return null;
            }
        }

        

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            try
            {
                return await _unitOfWork.Clients.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la récupération de tous les clients");
                return Enumerable.Empty<Client>();
            }
        }

       

        public async Task<bool> CreateClientAsync(Client client)
        {
            try
            {
                if (await ExistsByNumeroPermisAsync(client.NumeroPermis))
                {
                    _logger.LogWarning("Un client avec le permis {NumeroPermis} existe déjà", client.NumeroPermis);
                    return false;
                }

                client.DateInscription = DateTime.Now;
                client.EstActif = true;
                client.PermisVerifie = false;

                await _unitOfWork.Clients.AddAsync(client);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Client {ClientNom} créé avec succès", $"{client.Prenom} {client.Nom}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création du client");
                return false;
            }
        }

        

        public async Task<bool> DeleteClientAsync(string id)
        {
            try
            {
                var client = await GetClientByIdAsync(id);
                if (client == null)
                {
                    _logger.LogWarning("Client {ClientId} non trouvé pour suppression", id);
                    return false;
                }

                client.EstActif = false;
                _unitOfWork.Clients.Update(client);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Client {ClientId} désactivé avec succès", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression du client {ClientId}", id);
                return false;
            }
        }

        public async Task<bool> VerifierPermisAsync(string id)
        {
            try
            {
                var client = await GetClientByIdAsync(id);
                if (client == null)
                {
                    _logger.LogWarning("Client {ClientId} non trouvé pour vérification du permis", id);
                    return false;
                }

                client.PermisVerifie = true;
                _unitOfWork.Clients.Update(client);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Permis du client {ClientId} vérifié avec succès", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la vérification du permis du client {ClientId}", id);
                return false;
            }
        }

        public async Task<bool> ExistsByNumeroPermisAsync(string numeroPermis)
        {
            try
            {
                var client = await _unitOfWork.Clients.GetByNumeroPermisAsync(numeroPermis);
                return client != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la vérification du numéro de permis {NumeroPermis}", numeroPermis);
                return false;
            }
        }

           

        public async Task<bool> ActivateClientAsync(string clientId)
        {
            try
            {
                var client = await GetClientByIdAsync(clientId);
                if (client == null)
                {
                    _logger.LogWarning("Client {ClientId} non trouvé pour activation", clientId);
                    return false;
                }

                client.EstActif = true;
                _unitOfWork.Clients.Update(client);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Client {ClientId} activé avec succès", clientId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'activation du client {ClientId}", clientId);
                return false;
            }
        }
    }
}