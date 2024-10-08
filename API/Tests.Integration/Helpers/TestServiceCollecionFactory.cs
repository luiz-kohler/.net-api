using Microsoft.Extensions.DependencyInjection;
using Application;
using Infra;

namespace Tests.Integration.Helpers
{
    public static class TestServiceCollecionFactory
    {
        public static IServiceCollection BuildIntegrationTestInfrastructure(string dbConnectionString)
        {
            var services = new ServiceCollection();
            services.AddTestDatabase(dbConnectionString);
            services.AddApplication();
            services.AddInfra();
            services.AddLogging();

            return services;
        }
    }
}
