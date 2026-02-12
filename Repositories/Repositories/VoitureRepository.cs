using AgenceLocationVoiture.Data;
using AgenceLocationVoiture.Models;
using Microsoft.EntityFrameworkCore;

namespace AgenceLocationVoiture.Repositories.Repositories
{
    public class VoitureRepository : Repository<Voiture>, IVoitureRepository
    {
        public VoitureRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Voiture?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Voitures
                .Include(v => v.Agence)
                .Include(v => v.FicheTechnique)
                .Include(v => v.OffresLocation)
                .FirstOrDefaultAsync(v => v.Id == id);
        }


        public async Task<IEnumerable<Voiture>> GetVoituresDisponiblesAsync()
        {
            return await _context.Voitures
                .Include(v => v.Agence)
                .Include(v => v.FicheTechnique)
                .Where(v => v.EstDisponible)
                .ToListAsync();
        }

        public async Task<IEnumerable<Voiture>> GetVoituresByAgenceAsync(string agenceId)
        {
            return await _context.Voitures
                .Include(v => v.FicheTechnique)
                .Where(v => v.AgenceId == agenceId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Voiture>> GetVoituresDispoByAgenceAsync(string agenceId)
        {
            return await _context.Voitures
                .Include(v => v.FicheTechnique)
                .Where(v => v.AgenceId == agenceId)
                .Where(v => v.EstDisponible)
                .ToListAsync();
        }

       
        public async Task<Voiture?> GetByImmatriculationAsync(string immatriculation)
        {
            return await _context.Voitures
                .FirstOrDefaultAsync(v => v.Immatriculation == immatriculation);
        }
    }
}