using AgenceLocationVoiture.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace AgenceLocationVoiture.Repositories.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        public IAgenceRepository Agences { get; }
        public IClientRepository Clients { get; }
        public IVoitureRepository Voitures { get; }
        public IOffreLocRepository OffresLocation { get; }
        public IDemandeLocRepository DemandesLocation { get; }
        public IAvisRepository Avis { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            
            Agences = new AgenceRepository(_context);
            Clients = new ClientRepository(_context);
            Voitures = new VoitureRepository(_context);
            OffresLocation = new OffreLocRepository(_context);
            DemandesLocation = new DemandeLocRepository(_context);
            Avis = new AvisRepository(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}