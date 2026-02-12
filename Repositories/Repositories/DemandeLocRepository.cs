using AgenceLocationVoiture.Data;
using AgenceLocationVoiture.Models;
using Microsoft.EntityFrameworkCore;

namespace AgenceLocationVoiture.Repositories.Repositories
{
    public class DemandeLocRepository : Repository<DemandeLoc>, IDemandeLocRepository
    {
        public DemandeLocRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<DemandeLoc?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.DemandesLocation
                .Include(d => d.Client)
                .Include(d => d.OffreLoc)
                    .ThenInclude(o => o.Voiture)
                        .ThenInclude(v => v.Agence)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        

        public async Task<IEnumerable<DemandeLoc>> GetDemandesWithDetailsAsync()
        {
            return await _context.DemandesLocation
                .Include(d => d.Client)
                .Include(d => d.OffreLoc)
                .ThenInclude(v => v.Agence)
                .ToListAsync();

        }

        public async Task<IEnumerable<DemandeLoc>> GetDemandesByClientAsync(string clientId)
        {
            return await _context.DemandesLocation
                .Include(d => d.OffreLoc)
                    .ThenInclude(o => o.Voiture)
                        .ThenInclude(v => v.Agence)
                .Where(d => d.ClientId == clientId)
                .OrderByDescending(d => d.DateDemande)
                .ToListAsync();
        }

        public async Task<IEnumerable<DemandeLoc>> GetDemandesByAgenceAsync(string agenceId)
        {
            return await _context.DemandesLocation
                .Include(d => d.Client)
                .Include(d => d.OffreLoc)
                    .ThenInclude(o => o.Voiture)
                .Where(d => d.OffreLoc.AgenceId == agenceId)
                .OrderByDescending(d => d.DateDemande)
                .ToListAsync();
        }

        public async Task<IEnumerable<DemandeLoc>> GetDemandesByStatutAsync(StatutDemande statut)
        {
            return await _context.DemandesLocation
                .Include(d => d.Client)
                .Include(d => d.OffreLoc)
                    .ThenInclude(o => o.Voiture)
                .Where(d => d.Statut == statut)
                .ToListAsync();
        }

       
    }
}