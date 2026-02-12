using AgenceLocationVoiture.Data;
using AgenceLocationVoiture.Models;
using Microsoft.EntityFrameworkCore;


namespace AgenceLocationVoiture.Repositories.Repositories
{
    public class AgenceRepository : Repository<Agence>, IAgenceRepository
    {
        public AgenceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Agence?> GetByIdWithDetailsAsync(string id)
        {
            return await _context.Agences
                .Include(a => a.Voitures)
                    .ThenInclude(v => v.FicheTechnique)
                .Include(a => a.OffresLocation)
                .Include(a => a.AvisRecus)
                    .ThenInclude(av => av.Client)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Agence>> GetAgencesVerifieesAsync()
        {
            return await _context.Agences
                .Where(a => a.EstVerifiee && a.EstActif)
                .Include(a => a.AvisRecus)
                .ToListAsync();
        }

        public async Task<IEnumerable<Agence>> GetAgencesByVilleAsync(string ville)
        {
            return await _context.Agences
                .Where(a => a.Ville == ville && a.EstActif)
                .ToListAsync();
        }

    

        public async Task<IEnumerable<Agence>> SearchAgencesAsync(string searchTerm)
        {
            return await _context.Agences
                .Where(a => a.EstActif && (
                    a.NomAgence.Contains(searchTerm) ||
                    a.Ville.Contains(searchTerm) ||
                    a.Description.Contains(searchTerm)
                ))
                .ToListAsync();
        }

        public async Task<double> GetNoteMoyenneAsync(string agenceId)
        {
            var avis = await _context.Avis
                .Where(a => a.AgenceId == agenceId && a.EstVisible)
                .Select(a => a.Note)
                .ToListAsync();

            return avis.Any() ? avis.Average() : 0;
        }
    }
}