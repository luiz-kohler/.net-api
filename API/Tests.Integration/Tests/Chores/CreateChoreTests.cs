using Application.Services.Chore.Create;
using Bogus;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;

namespace Tests.Integration.Tests.Chores
{
    public class CreateChoreTests : ApplicationTestBase
    {
        [Fact]
        public async Task Should_Return_Success_When_Create_Chore()
        {
            var request = new Faker<CreateChoreRequest>()
              .RuleFor(chore => chore.Title, faker => faker.Random.Word())
              .RuleFor(chore => chore.Description, faker => faker.Random.Words())
              .RuleFor(chore => chore.Status, faker => faker.Random.Enum<EChoreStatus>())
              .Generate();

            await Handle<CreateChoreRequest, CreateChoreResponse>(request);

            var chores = GetEntities<Chore>();
            chores.Should().HaveCount(1);

            var chore = chores.First();

            chore.Should().BeEquivalentTo(request, options => options
                .ExcludingMissingMembers()
                .WithMapping<Chore, CreateChoreRequest>(chore => chore.Title, request => request.Title)
                .WithMapping<Chore, CreateChoreRequest>(chore => chore.Description, request => request.Description)
                .WithMapping<Chore, CreateChoreRequest>(chore => chore.Status, request => request.Status));
        }
    }
}
