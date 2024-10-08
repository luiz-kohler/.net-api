using Infra.Database;
using Infra.Database.Common;
using Infra.Database.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;

namespace Tests.Integration.Helpers
{
    public static class TestDatabaseManager
    {
        public static void RebuildDatabase(IServiceProvider serviceProvider)
        {
            var databaseContext = serviceProvider.GetService<Context>();
            databaseContext!.Database.EnsureDeleted();
            databaseContext.Database.Migrate();
        }

        public static void TruncateAllTables(IServiceProvider serviceProvider)
        {
            var databaseContext = serviceProvider.GetService<Context>();

            var tableNames = GetTableNames();
            foreach (var tableName in tableNames)
                databaseContext!.Database.ExecuteSqlRaw($"TRUNCATE TABLE {tableName};");
        }

        private static IEnumerable GetTableNames()
        {
            var tableNames = typeof(Infra.IoC).Assembly
                .GetTypes()
                .Where(x => x.IsSubclassOfRawGeneric(typeof(BaseMapping<>)))
                .Where(x => x.IsAbstract is false)
                .Select(x => Activator.CreateInstance(x))
                .Select(x => x.GetType().GetProperty(nameof(ChoreMapping.TableName)).GetValue(x))
                .ToList();

            return tableNames;
        }
    }

    public static class TypeExtensions
    {
        public static bool IsSubclassOfRawGeneric(this Type toCheck, Type generic)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }

                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}
