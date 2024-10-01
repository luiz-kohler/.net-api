namespace Infra.Database.Contexts
{
    public interface IScopedDatabaseContext
    {
        Context Context { get; }
    }
}
