using Notifications.Repository.Entities;
using System.Linq.Expressions;

namespace Notifications.Repository.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity?> GetByID(Guid? id);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "");
        Task InsertAsync(TEntity entity);
        void Update(TEntity entityToUpdate);
        Task DeleteAsync(Guid? id);
        void Delete(TEntity entityToDelete);

        Task CommitAsync();
    }
}
