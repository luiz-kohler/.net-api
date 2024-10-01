using Microsoft.EntityFrameworkCore;

namespace Infra.Database.Common
{
    public interface IBaseMapping
    {
        void MapearEntidade(ModelBuilder modelBuilder);
    }
}
