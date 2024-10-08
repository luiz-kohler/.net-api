using Application.Common.UnitOfWork;
using Infra.Database;
using Infra.Database.Common;
using Infra.Database.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Integration.Helpers
{
    public static class IoC
    {
        public static void AddTestDatabase(this IServiceCollection services, string dbConnection)
        {
            services.AddScoped<IScopedDatabaseContext, ScopedDatabaseContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContextPool<Context>(opt => opt.UseSqlServer(dbConnection));
        }
    }
}
