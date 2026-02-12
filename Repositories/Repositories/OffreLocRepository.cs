using AgenceLocationVoiture.Data;
using AgenceLocationVoiture.Models;
using Microsoft.EntityFrameworkCore;

namespace AgenceLocationVoiture.Repositories.Repositories
{
    public class OffreLocRepository : Repository<OffreLoc>, IOffreLocRepository
    {
        public OffreLocRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<OffreLoc?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.OffresLocation
                .Include(o => o.Voiture)
                    .ThenInclude(v => v.FicheTechnique)
                .Include(o => o.Agence)
           
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<OffreLoc>> GetOffresActivesAsync()
        {
            return await _context.OffresLocation
                .Include(o => o.Voiture)
                .Include(o => o.Agence)
                //.Where(o => o.EstActive )
                .Where(o => o.EstActive && o.DateFin >= DateTime.Now.AddDays(2))
                .OrderByDescending(o => o.DateCreation)
                .ToListAsync();
        }

        public async Task<IEnumerable<OffreLoc>> GetOffresByAgenceAsync(string agenceId)
        {
            return await _context.OffresLocation
                .Include(o => o.Voiture)
                .Where(o => o.AgenceId == agenceId)
                .OrderByDescending(o => o.DateCreation)
                .ToListAsync();
        }
        public async Task<IEnumerable<OffreLoc>> GetAllWithDetailsAsync()
        {
            return await _context.OffresLocation
                .Include(o => o.Voiture)
                .Include(o => o.Agence)
                .OrderByDescending(o => o.DateCreation)
                .ToListAsync();
        }
        public async Task<IEnumerable<OffreLoc>> GetOffresActiveByAgenceAsync(string agenceId)
        {
            return await _context.OffresLocation
                .Include(o => o.Voiture)
                .Where(o => o.AgenceId == agenceId)
                .Where(o => o.EstActive)
                .OrderByDescending(o => o.DateCreation)
                .ToListAsync();
        }

        public async Task<IEnumerable<OffreLoc>> GetOffresByVoitureAsync(int voitureId)
        {
            return await _context.OffresLocation
                .Where(o => o.VoitureId == voitureId && o.EstActive)
                .OrderByDescending(o => o.DateCreation)
                .ToListAsync();
        }

        public async Task<IEnumerable<OffreLoc>> SearchOffresAsync(
            DateTime? dateDebut = null,
            DateTime? dateFin = null,
            decimal? prixMax = null,
            string? ville = null)
        {
            var query = _context.OffresLocation
                .Include(o => o.Voiture)
                    .ThenInclude(v => v.FicheTechnique)
                .Include(o => o.Agence)
                .Where(o => o.EstActive)
                .AsQueryable();

            if (dateDebut.HasValue)
                query = query.Where(o => o.DateDebut <= dateDebut.Value);

            if (dateFin.HasValue)
                query = query.Where(o => o.DateFin >= dateFin.Value);

            if (prixMax.HasValue)
                query = query.Where(o => o.PrixParJour <= prixMax.Value);

            if (!string.IsNullOrEmpty(ville))
                query = query.Where(o => o.Agence.Ville == ville);

            return await query
                .OrderBy(o => o.PrixParJour)
                .ToListAsync();
        }
    }
}