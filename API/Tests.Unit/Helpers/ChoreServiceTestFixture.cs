using Domain.IRepositories;
using NSubstitute;

namespace Tests.Unit.Helpers
{
    public class ChoreServiceTestFixture : ServiceTestFixture
    {
        public IChoreRepository ChoreRepository { get; private set; }

        public ChoreServiceTestFixture() : base()
        {
            ChoreRepository = Substitute.For<IChoreRepository>();
        }

        public void ResetChoreRepository()
        {
            ChoreRepository = Substitute.For<IChoreRepository>();
        }
    }
}
