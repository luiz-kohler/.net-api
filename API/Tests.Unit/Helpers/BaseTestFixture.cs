using Bogus;

namespace Tests.Unit.Helpers
{
    public class BaseTestFixture
    {
        public Faker Faker { get; private set; }
        public DateTime CurrentDate { get; private set; }

        public BaseTestFixture()
        {
            Faker = new Faker();
            CurrentDate = DateTime.UtcNow;
        }
    }
}
