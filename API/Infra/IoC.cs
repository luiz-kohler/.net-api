using Domain.IRepositories;
using Infra.Database.Common;
using Infra.Database.Contexts;
using Infra.Database.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Reflection;

namespace Infra
{
    public static class IoC
    {
        public static IServiceCollection AddInfra(this IServiceCollection services)
        {
            RegisterRepositories(services);
            AddSqlServer(services);

            return services;
        }

        public static void RegisterRepositories(IServiceCollection services)
        {
            //var derivedClasses = FindConcreteDerivedClasses(typeof(BaseRepository<>));

            //foreach (var derivedClass in derivedClasses)
            //{
            //    var interfaceType = derivedClass.GetInterfaces().LastOrDefault();

            //    if (interfaceType != null)
            //    {
            //        services.AddScoped(interfaceType, derivedClass);
            //    }
            //}

            services.AddScoped<IChoreRepository, ChoreRepository>();
        }

        public static List<Type> FindConcreteDerivedClasses(Type baseType)
        {
            return Assembly.GetExecutingAssembly()
                           .GetTypes()
                           .Where(t => !t.IsAbstract &&
                                       t.BaseType != null &&
                                       t.BaseType.IsGenericType &&
                                       t.BaseType.GetGenericTypeDefinition() == baseType)
                           .ToList();
        }

        private static void AddSqlServer(this IServiceCollection services)
        {
            services.AddScoped<IScopedDatabaseContext, ScopedDatabaseContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
