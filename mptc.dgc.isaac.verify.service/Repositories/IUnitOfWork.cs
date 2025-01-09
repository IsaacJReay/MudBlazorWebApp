namespace mptc.dgc.isaac.verify.service.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity, TId> Repository<TEntity, TId>() where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}