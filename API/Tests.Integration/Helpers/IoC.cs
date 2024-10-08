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
            //var environmentVariables = Substitute.For<IEnvironmentVariables>();
            //environmentVariables.GetEnvironmentVariable(Arg.Any<string>()).Returns((string)null);
            //var config = new[]
            //{
            //    ("DBConnection", dbConnection),
            //};

            //foreach (var (variable, value) in config)
            //{
            //    environmentVariables.GetEnvironmentVariable(variable).Returns(value);
            //}

            services.AddScoped<IScopedDatabaseContext, ScopedDatabaseContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddSingleton(environmentVariables);
            //services.AddSingleton<IConnectionStringFactory, ConnectionStringFactory>();

            services.AddDbContextPool<Context>(opt => opt.UseSqlServer(dbConnection));
        }
    }
}
