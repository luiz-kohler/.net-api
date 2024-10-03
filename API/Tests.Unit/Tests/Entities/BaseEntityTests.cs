using Domain.Entities;
using FluentAssertions;
using Tests.Unit.Helpers;

namespace Tests.Unit.Tests.Entities
{
    public class BaseEntityTests : IClassFixture<BaseTestFixture>
    {
        private readonly BaseTestFixture _fixture;

        public BaseEntityTests(BaseTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Should_Create_Entity()
        {
            var entity = new BaseEntityForTests();

            entity.Id.Should().NotBeEmpty();
            entity.IsActive.Should().BeTrue();
            entity.UpdatedAt.Should().BeNull();
            entity.CreatedAt.Should().BeCloseTo(_fixture.CurrentDate, TimeSpan.FromSeconds(10));
        }

        [Fact]
        public void Should_Update_Entity()
        {
            var entity = new BaseEntityForTests();
            entity.UpdatedAt.Should().BeNull();

            entity.UpdateBaseEntity();

            entity.Id.Should().NotBeEmpty();
            entity.IsActive.Should().BeTrue();
            entity.UpdatedAt.Should().BeCloseTo(_fixture.CurrentDate, TimeSpan.FromSeconds(10));
            entity.CreatedAt.Should().BeCloseTo(_fixture.CurrentDate, TimeSpan.FromSeconds(10));
        }

        [Fact]
        public void Should_Disable_Entity()
        {
            var entity = new BaseEntityForTests();
            entity.IsActive.Should().BeTrue();

            entity.Disable();

            entity.Id.Should().NotBeEmpty();
            entity.IsActive.Should().BeFalse();
            entity.UpdatedAt.Should().BeCloseTo(_fixture.CurrentDate, TimeSpan.FromSeconds(10));
            entity.CreatedAt.Should().BeCloseTo(_fixture.CurrentDate, TimeSpan.FromSeconds(10));
        }
    }
}
