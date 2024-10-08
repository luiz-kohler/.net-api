using Application.Common.Exceptions;
using Application.Services.Chore.Create;
using Application.Services.Chore.Update;
using Bogus;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using System;

namespace Tests.Integration.Tests.Chores
{
    public class UpdateChoreTests : ApplicationTestBase
    {
        [Fact]
        public async Task Should_Return_Success_When_Update_Chore()
        {
            var chore = CreateChore();

            var request = new Faker<UpdateChoreRequest>()
              .RuleFor(chore => chore.Id, faker => chore.Id)
              .RuleFor(chore => chore.Title, faker => faker.Random.Word())
              .RuleFor(chore => chore.Description, faker => faker.Random.Words())
              .RuleFor(chore => chore.Status, faker => faker.Random.Enum<EChoreStatus>());


            await Handle<UpdateChoreRequest, UpdateChoreResponse>(request);

            var chores = GetEntities<Chore>();
            chores.Should().HaveCount(1);
            chores[0].UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(50));
            chores[0].Should().BeEquivalentTo(request, options => options
            .ExcludingMissingMembers()
            .WithMapping<Chore, UpdateChoreRequest>(chore => chore.Id, request => request.Id)
            .WithMapping<Chore, UpdateChoreRequest>(chore => chore.Title, request => request.Title)
            .WithMapping<Chore, UpdateChoreRequest>(chore => chore.Description, request => request.Description)
            .WithMapping<Chore, UpdateChoreRequest>(chore => chore.Status, request => request.Status));
        }

        [Fact]
        public async Task Should_Return_Failure_When_Try_To_Update_A_Non_Existed_Delete_Chore()
        {
            var request = new Faker<UpdateChoreRequest>()
              .RuleFor(chore => chore.Id, faker => Guid.NewGuid())
              .RuleFor(chore => chore.Title, faker => faker.Random.Word())
              .RuleFor(chore => chore.Description, faker => faker.Random.Words())
              .RuleFor(chore => chore.Status, faker => faker.Random.Enum<EChoreStatus>());

            var act = async () => await Handle<UpdateChoreRequest, UpdateChoreResponse>(request);

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
