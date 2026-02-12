using AgenceLocationVoiture.Models;

namespace AgenceLocationVoiture.Repositories
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<Client?> GetByNumeroPermisAsync(string numeroPermis);
    }
}