using Microsoft.EntityFrameworkCore;
using Notifications.Repository.Entities;
using Notifications.Repository.Interfaces;
using System.Linq.Expressions;

namespace Notifications.Repository.Repositories
{
    public abstract class BaseRepository<TEntity>(AppDbContext context)
        : IBaseRepository<TEntity> where TEntity : BaseEntity
    { 
        protected DbSet<TEntity> _dbSet = context.Set<TEntity>();
        protected AppDbContext _context = context;
        private static readonly char[] separator = [','];

        public virtual async Task<TEntity?> GetByID(Guid? id)
        {
            return id.HasValue ? await _dbSet.FindAsync(id) : default;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (separator, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual async Task DeleteAsync(Guid? id)
        {
            TEntity? entityToDelete = id.HasValue ? await _dbSet.FindAsync(id) : default;
            if (entityToDelete != null)
            {
                Delete(entityToDelete);
            }
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual async Task CommitAsync()
        {
        }
    }
}
