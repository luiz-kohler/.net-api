using FluentAssertions;
using UnitTest.Helpers;
using Xunit;

namespace UnitTest.Entities
{
    public class BaseEntityTests
    {
        private readonly DateTime _dataAtual;

        public BaseEntityTests()
        {
            _dataAtual = DateTime.UtcNow;
        }

        [Fact]
        public void Should_create_entity()
        {
            var entity = new BaseEntityForTests();

            entity.Id.Should().NotBeEmpty();
            entity.IsActive.Should().BeTrue();
            entity.UpdatedAt.Should().BeNull();
            entity.CreatedAt.Should().BeCloseTo(_dataAtual, TimeSpan.FromSeconds(10));
        }

        [Fact]
        public void Should_update_entity()
        {
            var entity = new BaseEntityForTests();
            entity.UpdatedAt.Should().BeNull();

            entity.UpdateBaseEntity();

            entity.Id.Should().NotBeEmpty();
            entity.IsActive.Should().BeTrue();
            entity.UpdatedAt.Should().BeCloseTo(_dataAtual, TimeSpan.FromSeconds(10));
            entity.CreatedAt.Should().BeCloseTo(_dataAtual, TimeSpan.FromSeconds(10));
        }

        [Fact]
        public void Should_disable_entity()
        {
            var entity = new BaseEntityForTests();
            entity.IsActive.Should().BeTrue();

            entity.Disable();

            entity.Id.Should().NotBeEmpty();
            entity.IsActive.Should().BeFalse();
            entity.UpdatedAt.Should().BeCloseTo(_dataAtual, TimeSpan.FromSeconds(10));
            entity.CreatedAt.Should().BeCloseTo(_dataAtual, TimeSpan.FromSeconds(10));
        }
    }
}
