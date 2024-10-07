using Domain.IRepositories;
using NSubstitute;

namespace Tests.Unit.Helpers
{
    public class ChoreServiceTestFixture : ServiceTestFixture
    {
        public IChoreRepository ChoreRepository { get; }

        public ChoreServiceTestFixture() : base()
        {
            ChoreRepository = Substitute.For<IChoreRepository>();
        }
    }
}
