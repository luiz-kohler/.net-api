using Bogus;

namespace Tests.Unit.Helpers
{
    public class BaseTestFixture
    {
        public Faker Faker { get; }
        public DateTime CurrentDate { get; }

        public BaseTestFixture()
        {
            Faker = new Faker();
            CurrentDate = DateTime.UtcNow;
        }
    }
}
