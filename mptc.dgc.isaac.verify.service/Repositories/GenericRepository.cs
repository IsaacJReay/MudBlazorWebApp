
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using mptc.dgc.isaac.verify.dal.Models;
using mptc.dgc.isaac.verify.service.Dtos.Commons;
using mptc.dgc.isaac.verify.service.Helpers;

namespace mptc.dgc.isaac.verify.service.Repositories
{
    public class GenericRepository<T, TId> : IGenericRepository<T, TId> where T : class
    {
        private readonly TodoContext _context;
        private readonly DbSet<T> _entities;

        public GenericRepository(TodoContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<T> CreateAsync(T newEntity, CancellationToken cancellationToken = default)
        {
            await _entities.AddAsync(newEntity, cancellationToken);
            return newEntity;
        }

        public async Task<bool> CreateMultipleAsync(List<T> entities, CancellationToken cancellationToken = default)
        {
            await _entities.AddRangeAsync(entities, cancellationToken);
            return true;
        }

        public async Task<bool> DeleteAsync(TId id, CancellationToken cancellationToken = default)
        {
            var entity = await _entities.FindAsync(new object[] { id }, cancellationToken);
            if (entity == null) return false;
            _entities.Remove(entity);
            return true;
        }

        public async Task<T?> FindByCompositeKeyAsync(CancellationToken cancellationToken = default, params object[] keys)
        {
            return await _entities.FindAsync(keys, cancellationToken);
        }

        public async Task<IQueryable<T>> GetAllAsync(bool asNoTracking = true, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[]? includes)
        {
            IQueryable<T> query = _entities;

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }
            return await Task.FromResult(query);
        }

        public async Task<T?> GetByIdAsync(TId id, bool asNoTracking = true, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[]? includes)
        {
            IQueryable<T> query = _entities;
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }
            return await query.SingleOrDefaultAsync(e => EF.Property<TId>(e, "Id")!.Equals(id), cancellationToken);
        }

        public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _entities;
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<PaginationResultDto<T>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[]? includes)
        {
            IQueryable<T> query = _entities;
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            var totalRecords = await query.CountAsync(cancellationToken);
            var entities = await query.OrderByDescending(o => EF.Property<int>(o, "Id")).ToPaginatedListAsync(pageNumber, pageSize, cancellationToken);
            return entities;
        }

        public async Task<IQueryable<T>> QueryAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = true, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[]? includes)
        {
            IQueryable<T> query = _entities;
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            query = query.Where(predicate);
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }
            return await Task.FromResult(query);
        }

        public async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _entities.Update(entity);
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateMultipleAsync(List<T> entities, CancellationToken cancellationToken = default)
        {
            _entities.UpdateRange(entities);
            return await Task.FromResult(true);
        }

        public async Task<bool> SoftDeleteAsync(TId id, CancellationToken cancellationToken = default)
        {
            var entity = await _entities.FindAsync(new object[] { id }, cancellationToken);
            if (entity == null) return false;
            // Assuming a property "IsDeleted" exists for soft delete
            var propertyInfo = entity.GetType().GetProperty("IsDeleted");
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(entity, true);
                _entities.Update(entity);
                return await Task.FromResult(true);
            }
            return false;
        }

        public async Task<bool> DeleteMultipleAsync(List<TId> ids, CancellationToken cancellationToken = default)
        {
            var entities = await _entities.Where(e => ids.Contains(EF.Property<TId>(e, "Id"))).ToListAsync(cancellationToken);
            if (!entities.Any()) return false;
            _entities.RemoveRange(entities);
            return true;
        }

        public async Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                await action();
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}