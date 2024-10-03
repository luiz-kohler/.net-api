using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Tests.Unit.Helpers;
using Xunit;

namespace Tests.Unit.Tests.Entities
{
    public class ChoreTests : IClassFixture<BaseTestFixture>
    {
        private readonly BaseTestFixture _fixture;

        public ChoreTests(BaseTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Should_Create_Chore()
        {
            var expectedTitle = _fixture.Faker.Random.Word();
            var expectedDescription = _fixture.Faker.Random.Words();
            var expectedStatus = _fixture.Faker.Random.Enum<EChoreStatus>();

            var chore = new Chore()
            {
                Title = expectedTitle,
                Description = expectedDescription,
                Status = expectedStatus
            };

            chore.Id.Should().NotBeEmpty();
            chore.IsActive.Should().BeTrue();
            chore.UpdatedAt.Should().BeNull();
            chore.CreatedAt.Should().BeCloseTo(_fixture.CurrentDate, TimeSpan.FromSeconds(10));
            chore.Title.Should().Be(expectedTitle);
            chore.Description.Should().Be(expectedDescription);
            chore.Status.Should().Be(expectedStatus);
        }
    }
}
