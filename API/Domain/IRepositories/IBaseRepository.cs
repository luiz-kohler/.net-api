using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.IRepositories
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task SaveChanges();
        Task Insert(TEntity entity);
        Task InsertMany(IEnumerable<TEntity> entities);
        Task Update(TEntity entity);
        Task UpdateMany(IEnumerable<TEntity> entities);
        Task Delete(TEntity entity);
        Task DeleteMany(IEnumerable<TEntity> entities);
        Task<TEntity> SelectOne(Expression<Func<TEntity, bool>> filter);
        Task<IList<TEntity>> SelectMany(Expression<Func<TEntity, bool>> filter = null);
        Task<TEntity> SelectOneWithTracking(Expression<Func<TEntity, bool>> filter);
        Task<IList<TEntity>> SelectManyWithTracking(Expression<Func<TEntity, bool>> filter = null);
    }
}
