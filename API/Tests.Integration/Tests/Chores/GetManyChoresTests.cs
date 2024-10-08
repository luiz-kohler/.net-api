using Application.Services.Chore.GetMany;
using Application.Services.Chore.GetOne;
using Bogus;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;

namespace Tests.Integration.Tests.Chores
{
    public class GetManyChoresTests : ApplicationTestBase
    {
        [Fact]
        public async Task Should_Return_Success_When_Get_Many_Chores()
        {
            var chores = CreateManyChores(5);

            var request = new GetManyChoresRequest();

            var resposne = await Handle<GetManyChoresRequest, GetManyChoresResponse>(request);

            var choresDb = GetEntities<Chore>();
            choresDb.Should().HaveCount(chores.Count);
            choresDb.Should().BeEquivalentTo(chores, options => options.Excluding(chore => chore.CreatedAt));
        }

        [Fact]
        public async Task Should_Return_Sucess_When_Try_To_Get_Empty_List_Of_Chores()
        {
            var request = new GetManyChoresRequest();

            var response = await Handle<GetManyChoresRequest, GetManyChoresResponse>(request);

            var chores = GetEntities<Chore>();
            chores.Should().BeNullOrEmpty();
            response.Should().NotBeNull();
            response.Chores.Should().BeNullOrEmpty();
        }

        private List<Chore> CreateManyChores(int quantity)
        {
            var chores = new Faker<Chore>()
            .RuleFor(chore => chore.Id, f => f.Random.Guid())
            .RuleFor(chore => chore.Title, f => f.Lorem.Sentence())
            .RuleFor(chore => chore.Description, f => f.Lorem.Paragraph())
            .RuleFor(chore => chore.Status, f => f.PickRandom<EChoreStatus>())
            .Generate(quantity);

            InsertMany(chores);

            return chores;
        }
    }
}
