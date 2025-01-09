

using mptc.dgc.isaac.verify.dal.Models;

namespace mptc.dgc.isaac.verify.service.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TodoContext _context;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public UnitOfWork(TodoContext context)
        {
            _context = context;
        }

        public IGenericRepository<TEntity, TId> Repository<TEntity, TId>() where TEntity : class
        {
            if (_repositories.ContainsKey(typeof(TEntity)))
            {
                return (IGenericRepository<TEntity, TId>)_repositories[typeof(TEntity)];
            }

            var repository = new GenericRepository<TEntity, TId>(_context);
            _repositories.Add(typeof(TEntity), repository);
            return repository;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}