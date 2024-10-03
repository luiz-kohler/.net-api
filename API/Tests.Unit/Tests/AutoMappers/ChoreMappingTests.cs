using Application.Services.Chore.GetMany;
using Application.Services.Chore.GetOne;
using Bogus;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Tests.Unit.Tests.Helpers;

namespace Tests.Unit.Tests.AutoMappers
{
    public class ChoreMappingTests : IClassFixture<AutoMappersTestFixture>
    {
        private readonly AutoMappersTestFixture _fixture;

        public ChoreMappingTests(AutoMappersTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Should_Map_Chore_To_GetOneChoreResponse()
        {
            var chore = new Faker<Chore>()
                .RuleFor(chore => chore.Title, faker => faker.Random.Word())
                .RuleFor(chore => chore.Description, faker => faker.Random.Words())
                .RuleFor(chore => chore.Status, faker => faker.Random.Enum<EChoreStatus>())
                .Generate();

            var response = _fixture.Mapper.Map<GetOneChoreResponse>(chore);

            response.Should().NotBeNull();
            response.Id.Should().Be(chore.Id);
            response.Title.Should().Be(chore.Title);
            response.Description.Should().Be(chore.Description);
            response.Status.Should().Be(chore.Status);
        }

        [Fact]
        public void Should_Map_ListOfChores_To_GetManyChoresResponse()
        {
            var chores = new Faker<Chore>()
                .RuleFor(chore => chore.Title, faker => faker.Random.Word())
                .RuleFor(chore => chore.Description, faker => faker.Random.Words())
                .RuleFor(chore => chore.Status, faker => faker.Random.Enum<EChoreStatus>())
                .GenerateBetween(0, _fixture.Faker.Random.Number(10));

            var response = _fixture.Mapper.Map<GetManyChoresResponse>(chores);

            response.Should().NotBeNull();
            response.Chores.Should().NotBeNull();
            response.Chores.Count.Should().Be(chores.Count);
            response.Chores
                .Zip(chores, (actual, expected) => new { actual, expected })
                .ToList()
                .ForEach(pair =>
                {
                    pair.actual.Title.Should().Be(pair.expected.Title);
                    pair.actual.Description.Should().Be(pair.expected.Description);
                    pair.actual.Status.Should().Be(pair.expected.Status);
                    pair.actual.Id.Should().Be(pair.expected.Id);
                });
        }
    }
}
