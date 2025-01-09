using System.Linq.Expressions;
using mptc.dgc.isaac.verify.service.Dtos.Commons;

namespace mptc.dgc.isaac.verify.service.Repositories
{
    public interface IGenericRepository<T, TId> where T : class
    {
        Task<IQueryable<T>> GetAllAsync(bool asNoTracking = true, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[]? includes);
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = true, CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(TId id, bool asNoTracking = true, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[]? includes);
        Task<T> CreateAsync(T newEntity, CancellationToken cancellationToken = default);
        Task<bool> CreateMultipleAsync(List<T> entities, CancellationToken cancellationToken = default);
        Task<T?> FindByCompositeKeyAsync(CancellationToken cancellationToken = default, params object[] keys);
        Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task<bool> UpdateMultipleAsync(List<T> entities, CancellationToken cancellationToken = default);
        Task<bool> SoftDeleteAsync(TId id, CancellationToken cancellationToken = default);
        Task<bool> DeleteMultipleAsync(List<TId> ids, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(TId id, CancellationToken cancellationToken = default);
        Task<PaginationResultDto<T>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[]? includes);
        Task<IQueryable<T>> QueryAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = true, CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[]? includes);
        Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default);
    }
}