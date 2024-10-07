using Application.Common.Exceptions;
using Application.Services.Chore.Create;
using Application.Services.Chore.Delete;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Linq.Expressions;
using Tests.Unit.Helpers;

namespace Tests.Unit.Tests.Services.Chores
{
    public class DeleteChoreTests : IClassFixture<ChoreServiceTestFixture>
    {
        private readonly ChoreServiceTestFixture _fixture;
        private readonly DeleteChoreHandler _handler;

        public DeleteChoreTests(ChoreServiceTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.ResetChoreRepository();
            _handler = new DeleteChoreHandler(_fixture.ChoreRepository);
        }

        [Fact]
        public async Task Should_Return_Success_When_Delete_Chore()
        {
            var chore = CreateChore();
            var request = CreateValidDeleteChoreRequest();

            _fixture.ChoreRepository.SelectOne(Arg.Any<Expression<Func<Chore, bool>>>()).Returns(chore);

            var response = await _handler.Handle(request, _fixture.CancellationToken);

            await _fixture.ChoreRepository.Received().SelectOne(Arg.Any<Expression<Func<Chore, bool>>>());
            await _fixture.ChoreRepository.Received().Delete(Arg.Any<Chore>());
            response.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Return_Failure_When_Try_To_Delete_A_Non_Existent_Chore()
        {
            var deleteChoreRequest = CreateValidDeleteChoreRequest();
            _fixture.ChoreRepository.SelectOne(Arg.Any<Expression<Func<Chore, bool>>>()).ReturnsNull();

            var act = async () => await _handler.Handle(deleteChoreRequest, new CancellationToken());

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Chore not found");
            await _fixture.ChoreRepository.Received().SelectOne(Arg.Any<Expression<Func<Chore, bool>>>());
            await _fixture.ChoreRepository.DidNotReceive().Delete(Arg.Any<Chore>());
        }


        [Theory]
        [MemberData(nameof(CreateInvalidRequestsWithMessagesError))]
        public async Task Should_Return_Failure_When_Validate_Delete_Chore_Request_Invalid(DeleteChoreRequest request, List<string> expectedMessages)
        {
            var validation = new DeleteChoreValidator().Validate(request);

            validation.IsValid.Should().BeFalse();
            validation.Errors.Count.Should().Be(expectedMessages.Count);
            validation.Errors.Select(error => error.ErrorMessage).Should().BeEquivalentTo(expectedMessages);
        }

        private static DeleteChoreRequest CreateValidDeleteChoreRequest()
        {
            return new Faker<DeleteChoreRequest>()
                .RuleFor(chore => chore.Id, faker => faker.Random.Uuid())
                .Generate();
        }

        private static Chore CreateChore()
        {
            return new Faker<Chore>()
                .RuleFor(chore => chore.Id, faker => faker.Random.Uuid())
                .Generate();
        }

        public static IEnumerable<object[]> CreateInvalidRequestsWithMessagesError()
        {
            var errors = new List<string>();

            var requestWithIdEmpty = CreateValidDeleteChoreRequest();
            requestWithIdEmpty.Id = Guid.Empty;
            errors.Add("The Id must not be empty");
            yield return new object[] { requestWithIdEmpty, errors };
        }
    }
}
