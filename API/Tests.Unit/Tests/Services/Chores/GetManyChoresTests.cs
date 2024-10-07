using Application.Services.Chore.GetMany;
using Application.Services.Chore.GetOne;
using Bogus;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using NSubstitute;
using Tests.Unit.Helpers;

namespace Tests.Unit.Tests.Services.Chores
{
    public class GetManyChoresTests : IClassFixture<ChoreServiceTestFixture>
    {
        private readonly ChoreServiceTestFixture _fixture;
        private readonly GetManyChoresHandler _handler;

        public GetManyChoresTests(ChoreServiceTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.ResetChoreRepository();
            _handler = new GetManyChoresHandler(_fixture.ChoreRepository, _fixture.Mapper);
        }

        [Fact]
        public async Task Should_Return_Chores_When_Chores_Exist()
        {
            var chores = CreateListOfChores();
            _fixture.ChoreRepository.SelectMany().Returns(chores);

            var request = new GetManyChoresRequest();

            var response = await _handler.Handle(request, _fixture.CancellationToken);

            await _fixture.ChoreRepository.Received().SelectMany();
            response.Should().NotBeNull();
            response.Chores.Should().NotBeNull();
            response.Chores.Count.Should().Be(chores.Count);
            response.Chores.Should().BeEquivalentTo(chores, options => options
                .ExcludingMissingMembers()
                .WithMapping<Chore, GetOneChoreResponse>(c => c.Id, dto => dto.Id)
                .WithMapping<Chore, GetOneChoreResponse>(c => c.Title, dto => dto.Title)
                .WithMapping<Chore, GetOneChoreResponse>(c => c.Description, dto => dto.Description)
                .WithMapping<Chore, GetOneChoreResponse>(c => c.Status, dto => dto.Status));
        }

        [Fact]
        public async Task Should_Return_Empty_List_When_No_Chores_Exist()
        {
            var chores = new List<Chore>();
            _fixture.ChoreRepository.SelectMany().Returns(chores);

            var request = new GetManyChoresRequest();

            var response = await _handler.Handle(request, _fixture.CancellationToken);

            await _fixture.ChoreRepository.Received().SelectMany();
            response.Should().NotBeNull();
            response.Chores.Should().NotBeNull();
            response.Chores.Should().BeEmpty();
        }

        private static List<Chore> CreateListOfChores()
        {
            return new Faker<Chore>()
                .RuleFor(chore => chore.Id, f => f.Random.Guid())
                .RuleFor(chore => chore.Title, f => f.Lorem.Sentence())
                .RuleFor(chore => chore.Description, f => f.Lorem.Paragraph())
                .RuleFor(chore => chore.Status, f => f.PickRandom<EChoreStatus>())
                .Generate(5);
        }
    }
}
