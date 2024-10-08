using Bogus;
using Infra.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Tests.Integration.Helpers;

namespace Tests.Integration
{
    [Collection("Sequential")]
    [assembly: CollectionBehavior(DisableTestParallelization = true)]
    public abstract partial class IntegrationTestsBase : IDisposable
    {
        private const string ConnectionString = "Server=localhost,5434;Database=db;User Id=sa;Password=DbForTests@123;Trusted_Connection=False;TrustServerCertificate=True; MultipleActiveResultSets=true";

        private static IServiceProvider GetServiceProvider(IServiceCollection serviceCollection)
        {
            var defaultServiceProviderFactory = new DefaultServiceProviderFactory(new ServiceProviderOptions());
            return defaultServiceProviderFactory.CreateServiceProvider(serviceCollection);
        }

        private static IServiceCollection GetServices()
        {
            return TestServiceCollecionFactory.BuildIntegrationTestInfrastructure(
                ConnectionString
            );
        }

        public static Faker Faker { get; private set; }
        private readonly Lazy<IServiceProvider> _rootServiceProvider;
        private readonly Lazy<IServiceScope> _serviceScope;
        private readonly Lazy<Context> _databaseContext;
        protected readonly IServiceCollection Services;
        protected IServiceScope ServiceScope => _serviceScope.Value;
        protected IServiceProvider ServiceProvider => ServiceScope.ServiceProvider;
        protected Context DatabaseContext => _databaseContext.Value;
        protected IServiceProvider RootServiceProvider => _rootServiceProvider.Value;


        static IntegrationTestsBase()
        {
            var serviceProvider = GetServiceProvider(GetServices());
            TestDatabaseManager.RebuildDatabase(serviceProvider);
        }

        protected IntegrationTestsBase()
        {
            Services = TestServiceCollecionFactory.BuildIntegrationTestInfrastructure(
                ConnectionString
            );

            TestDatabaseManager.TruncateAllTables(GetServiceProvider(Services));

            Faker = new Faker();

            _rootServiceProvider = new Lazy<IServiceProvider>(() => GetServiceProvider(Services));
            _serviceScope = new Lazy<IServiceScope>(() => _rootServiceProvider.Value.CreateScope());
            _databaseContext = new Lazy<Context>(() => ServiceProvider.GetService<Context>());
        }

        protected T Mock<T>() where T : class
        {
            var mock = Substitute.For<T>();
            Services.AddTransient(provider => mock);
            return mock;
        }

        protected T GetService<T>() => ServiceProvider.GetService<T>();

        public void Dispose()
        {
            if (_serviceScope.IsValueCreated)
            {
                _serviceScope.Value.Dispose();
            }
        }
    }

    public partial class IntegrationTestsBase
    {
        protected DbSet<TEntity> GetTable<TEntity>()
            where TEntity : class
        {
            return DatabaseContext!.Set<TEntity>();
        }

        protected IList<TEntity> GetEntities<TEntity>()
            where TEntity : class
        {
            return DatabaseContext!
                .Set<TEntity>()
                .AsNoTracking()
                .IgnoreQueryFilters()
                .ToList();
        }

        protected TEntity InsertOne<TEntity>(TEntity entity)
            where TEntity : class
        {
            var entityEntry = DatabaseContext!.Set<TEntity>().Add(entity);
            DatabaseContext.SaveChanges();
            entityEntry.State = EntityState.Detached;
            return entity;
        }

        protected void InsertMany<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class
        {
            DatabaseContext!.Set<TEntity>().AddRange(entities);
            DatabaseContext.SaveChanges();
        }
    }
}
