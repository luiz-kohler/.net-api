using Application.Common.Exceptions;
using Application.Services.Chore.Delete;
using Bogus;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;

namespace Tests.Integration.Tests.Chores
{
    public class DeleteChoreTests : ApplicationTestBase
    {
        [Fact]
        public async Task Should_Return_Success_When_Delete_Chore()
        {
            var chore = CreateChore();

            var request = new DeleteChoreRequest { Id = chore.Id };

            await Handle<DeleteChoreRequest, DeleteChoreResponse>(request);

            var chores = GetEntities<Chore>();
            chores.Should().HaveCount(1);
            chores[0].Id.Should().Be(request.Id);
            chores[0].IsActive.Should().BeFalse();
            chores[0].UpdatedAt.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Return_Failure_When_Try_To_Delete_A_Non_Existed_Delete_Chore()
        {
            var request = new DeleteChoreRequest { Id = Guid.NewGuid() };


            var act = async () => await Handle<DeleteChoreRequest, DeleteChoreResponse>(request);

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Chore not found");
            var chores = GetEntities<Chore>();
            chores.Should().BeNullOrEmpty();
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
