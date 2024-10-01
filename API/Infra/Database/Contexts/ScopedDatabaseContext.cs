namespace Infra.Database.Contexts
{
    public class ScopedDatabaseContext : IScopedDatabaseContext
    {
        public Context Context { get; }

        public ScopedDatabaseContext(Context context)
        {
            Context = context;
        }
    }
}
