using AgenceLocationVoiture.Data;
using AgenceLocationVoiture.Models;
using Microsoft.EntityFrameworkCore;

namespace AgenceLocationVoiture.Repositories.Repositories
{
    public class AvisRepository : Repository<Avis>, IAvisRepository
    {
        public AvisRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Avis?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Avis
                .Include(a => a.Client)
                .Include(a => a.Agence)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Avis>> GetAvisByAgenceAsync(string agenceId)
        {
            return await _context.Avis
                .Include(a => a.Client)
                .Where(a => a.AgenceId == agenceId && a.EstVisible)
                .OrderByDescending(a => a.DateAvis)
                .ToListAsync();
        }

       

       
    }
}