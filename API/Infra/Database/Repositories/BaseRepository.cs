using Domain.Entities;
using Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infra.Database.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
           where TEntity : BaseEntity
    {
        protected readonly Context Context;
        protected readonly DbSet<TEntity> DbSet;

        public BaseRepository(Context context)
        {
            Context = context;
            DbSet = Context.Set<TEntity>();
        }

        public async Task SaveChanges() => await Context.SaveChangesAsync();

        public Task Delete(TEntity entity)
        {
            entity.Disable();
            return Update(entity);
        }

        public Task DeleteMany(IEnumerable<TEntity> entities)
        {
            entities.ToList().ForEach(entity => entity.Disable());
            return UpdateMany(entities);
        }

        public async Task Insert(TEntity entity)
        {
            await DbSet.AddAsync(entity);
        }

        public async Task InsertMany(IEnumerable<TEntity> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        public async Task<IList<TEntity>> SelectMany(Expression<Func<TEntity, bool>> filter = null)
        {

            if (filter == null)
                return await Query.AsNoTracking().ToListAsync();

            return await Query.AsNoTracking().Where(filter).ToListAsync();

        }

        public async Task<TEntity> SelectOne(Expression<Func<TEntity, bool>> filter)
        {
            return await Query.AsNoTracking().FirstOrDefaultAsync(filter);
        }

        public Task Update(TEntity entity)
        {
            entity.UpdateBaseEntity();
            DbSet.Update(entity);
            return Task.CompletedTask;
        }

        public Task UpdateMany(IEnumerable<TEntity> entities)
        {
            DbSet.UpdateRange(entities);
            return Task.CompletedTask;
        }

        public async Task<TEntity> SelectOneWithTracking(Expression<Func<TEntity, bool>> filter)
        {
            return await Query.FirstOrDefaultAsync(filter);
        }

        public async Task<IList<TEntity>> SelectManyWithTracking(Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter == null)
                return await Query.ToListAsync();

            return await Query.Where(filter).ToListAsync();
        }

        private IQueryable<TEntity> Query => DbSet.Where(entity => entity.IsActive);
    }
}
