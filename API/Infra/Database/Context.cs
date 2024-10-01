using Infra.Database.Common;
using Microsoft.EntityFrameworkCore;

namespace Infra.Database
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var mappingTypes = typeof(BaseMapping<>).Assembly
                .GetTypes()
                .Where(t => t.IsAssignableTo(typeof(IBaseMapping)))
                .Where(t => t.IsAbstract is false)
                .ToList();

            mappingTypes.ForEach(mappingType =>
            {
                var mapping = Activator.CreateInstance(mappingType);
                var initializeMethod = mapping.GetType().GetMethod(nameof(IBaseMapping.MapearEntidade));
                initializeMethod.Invoke(mapping, new object[] { modelBuilder });
            });
        }
    }
}
