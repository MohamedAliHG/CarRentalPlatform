using AgenceLocationVoiture.Models;

namespace AgenceLocationVoiture.Repositories
{
    public interface IAvisRepository : IRepository<Avis>
    {
        Task<Avis?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Avis>> GetAvisByAgenceAsync(string agenceId);      
       
    }
}