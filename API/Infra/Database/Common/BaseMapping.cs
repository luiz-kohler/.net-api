using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Database.Common
{
    public abstract class BaseMapping<TEntity> : IBaseMapping
            where TEntity : BaseEntity
    {
        public abstract string TableName { get; }

        public void MapearEntidade(ModelBuilder modelBuilder)
        {
            var entityBuilder = modelBuilder.Entity<TEntity>();
            MapearBase(entityBuilder);
            MapearEntidade(entityBuilder);
            MapearIndices(entityBuilder);
        }

        private void MapearBase(EntityTypeBuilder<TEntity> builder)
        {
            builder.ToTable(TableName);

            builder.Property(e => e.CreatedAt).HasColumnName("CREATED_AT").HasColumnType("DATETIME").IsRequired();
            builder.Property(e => e.UpdatedAt).HasColumnName("UPDATED_AT").HasColumnType("DATETIME").IsRequired(false);
            builder.Property(e => e.IsActive).HasColumnName("IS_ACTIVE").IsRequired();
        }

        protected abstract void MapearEntidade(EntityTypeBuilder<TEntity> builder);
        protected virtual void MapearIndices(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasIndex(e => e.Id);
        }
    }
}