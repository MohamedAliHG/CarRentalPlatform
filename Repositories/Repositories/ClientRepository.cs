using AgenceLocationVoiture.Data;
using AgenceLocationVoiture.Models;
using Microsoft.EntityFrameworkCore;

namespace AgenceLocationVoiture.Repositories.Repositories
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Client?> GetByNumeroPermisAsync(string numeroPermis)
        {
            return await _context.Clients
                .FirstOrDefaultAsync(c => c.NumeroPermis == numeroPermis);
        }
    }
}