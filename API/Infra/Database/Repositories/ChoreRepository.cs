using Domain.Entities;
using Domain.IRepositories;

namespace Infra.Database.Repositories
{
    public class ChoreRepository : BaseRepository<Chore>, IChoreRepository
    {
        public ChoreRepository(Context context) : base(context)
        {
        }
    }
}
