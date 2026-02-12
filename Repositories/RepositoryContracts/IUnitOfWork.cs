namespace AgenceLocationVoiture.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IAgenceRepository Agences { get; }
        IClientRepository Clients { get; }
        IVoitureRepository Voitures { get; }
        IOffreLocRepository OffresLocation { get; }
        IDemandeLocRepository DemandesLocation { get; }
        IAvisRepository Avis { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}