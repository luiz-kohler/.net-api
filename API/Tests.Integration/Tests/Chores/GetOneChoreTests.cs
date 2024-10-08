using Application.Common.Exceptions;
using Application.Services.Chore.Create;
using Application.Services.Chore.GetOne;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;

namespace Tests.Integration.Tests.Chores
{
    public class GetOneChoreTests : ApplicationTestBase
    {
        [Fact]
        public async Task Should_Return_Success_When_Get_One_Chore()
        {
            var chore = CreateChore();

            var request = new GetOneChoreRequest { Id = chore.Id };

            var resposne = await Handle<GetOneChoreRequest, GetOneChoreResponse>(request);

            var chores = GetEntities<Chore>();
            chores.Should().HaveCount(1);
            chores[0].Should().BeEquivalentTo(request, options => options
                .ExcludingMissingMembers()
                .WithMapping<GetOneChoreResponse, Chore>(response => response.Id, request => request.Title)
                .WithMapping<GetOneChoreResponse, Chore>(response => response.Description, request => request.Description)
                .WithMapping<GetOneChoreResponse, Chore>(response => response.Status, request => request.Status)
                .WithMapping<GetOneChoreResponse, Chore>(response => response.Title, request => request.Title));
        }

        [Fact]
        public async Task Should_Return_Sucess_When_Try_To_Get_A_Non_Existed_Chore()
        {
            var request = new GetOneChoreRequest { Id = Guid.NewGuid() };

            var response = await Handle<GetOneChoreRequest, GetOneChoreResponse>(request);

            var chores = GetEntities<Chore>();
            chores.Should().BeNullOrEmpty();
            response.Should().BeNull();
        }

        private Chore CreateChore()
        {
            var chore = new Chore
            {
                Title = Faker.Random.Word(),
                Description = Faker.Random.Words(),
                Status = Faker.Random.Enum<EChoreStatus>()
            };

            InsertOne(chore);

            return chore;
        }

    }
}
