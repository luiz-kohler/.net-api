using Application.Common.Exceptions;
using Application.Services.Chore.Update;
using Bogus;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using NSubstitute;
using System.Linq.Expressions;
using Tests.Unit.Helpers;

namespace Tests.Unit.Tests.Services.Chores
{
    public class UpdateChoreTests : IClassFixture<ChoreServiceTestFixture>
    {
        private readonly ChoreServiceTestFixture _fixture;
        private readonly UpdateChoreHandler _handler;

        public UpdateChoreTests(ChoreServiceTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.ResetChoreRepository();
            _handler = new UpdateChoreHandler(_fixture.ChoreRepository);
        }

        [Fact]
        public async Task Should_Return_Success_When_Update_Chore()
        {
            var chore = CreateChore();
            var request = CreateValidUpdateChoreRequest();

            _fixture.ChoreRepository.SelectOne(Arg.Any<Expression<Func<Chore, bool>>>()).Returns(chore);

            var response = await _handler.Handle(request, _fixture.CancellationToken);

            await _fixture.ChoreRepository.Received().SelectOne(Arg.Any<Expression<Func<Chore, bool>>>());
            await _fixture.ChoreRepository.Received().Update(Arg.Any<Chore>());

            response.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Return_Failure_When_Try_To_Update_A_Non_Existent_Chore()
        {
            var request = CreateValidUpdateChoreRequest();

            var act = async () => await _handler.Handle(request, _fixture.CancellationToken);

            await act.Should().ThrowAsync<NotFoundException>().WithMessage("Chore not found");

            await _fixture.ChoreRepository.Received().SelectOne(Arg.Any<Expression<Func<Chore, bool>>>());
            await _fixture.ChoreRepository.DidNotReceive().Update(Arg.Any<Chore>());
        }

        [Theory]
        [MemberData(nameof(CreateInvalidRequestsWithMessagesError))]
        public async Task Should_Return_Failure_When_Validate_Update_Chore_Request_Invalid(
            UpdateChoreRequest request, List<string> expectedMessages)
        {
            var validation = new UpdateChoreValidator().Validate(request);

            validation.IsValid.Should().BeFalse();
            validation.Errors.Count.Should().Be(expectedMessages.Count);
            validation.Errors.Select(error => error.ErrorMessage)
                .Should().BeEquivalentTo(expectedMessages);
        }

        private static UpdateChoreRequest CreateValidUpdateChoreRequest()
        {
            return new Faker<UpdateChoreRequest>()
                .RuleFor(chore => chore.Id, faker => faker.Random.Guid())
                .RuleFor(chore => chore.Title, faker => faker.Lorem.Sentence(5))
                .RuleFor(chore => chore.Description, faker => faker.Lorem.Paragraph())
                .RuleFor(chore => chore.Status, faker => faker.PickRandom<EChoreStatus>())
                .Generate();
        }

        private static Chore CreateChore()
        {
            return new Faker<Chore>()
                .RuleFor(chore => chore.Id, faker => faker.Random.Guid())
                .RuleFor(chore => chore.Title, faker => faker.Lorem.Sentence(5))
                .RuleFor(chore => chore.Description, faker => faker.Lorem.Paragraph())
                .RuleFor(chore => chore.Status, faker => faker.PickRandom<EChoreStatus>())
                .Generate();
        }

        public static IEnumerable<object[]> CreateInvalidRequestsWithMessagesError()
        {
            var faker = new Faker();

            var errors = new List<string>();
            var requestWithEmptyId = CreateValidUpdateChoreRequest();
            requestWithEmptyId.Id = Guid.Empty;
            errors.Add("The Id must not be empty");
            yield return new object[] { requestWithEmptyId, errors };

            errors = new List<string>();
            var requestWithTitleNull = CreateValidUpdateChoreRequest();
            requestWithTitleNull.Title = null;
            errors.Add("The title must be informed");
            yield return new object[] { requestWithTitleNull, errors };

            errors = new List<string>();
            var requestWithTitleEmpty = CreateValidUpdateChoreRequest();
            requestWithTitleEmpty.Title = string.Empty;
            errors.Add("The title must have between 2 and 600 characters");
            yield return new object[] { requestWithTitleEmpty, errors };

            errors = new List<string>();
            var requestWithTitleTooShort = CreateValidUpdateChoreRequest();
            requestWithTitleTooShort.Title = faker.Random.String2(1);
            errors.Add("The title must have between 2 and 600 characters");
            yield return new object[] { requestWithTitleTooShort, errors };

            errors = new List<string>();
            var requestWithTitleTooLong = CreateValidUpdateChoreRequest();
            requestWithTitleTooLong.Title = faker.Random.String2(601);
            errors.Add("The title must have between 2 and 600 characters");
            yield return new object[] { requestWithTitleTooLong, errors };

            errors = new List<string>();
            var requestWithDescriptionNull = CreateValidUpdateChoreRequest();
            requestWithDescriptionNull.Description = null;
            errors.Add("The description must be informed");
            yield return new object[] { requestWithDescriptionNull, errors };

            errors = new List<string>();
            var requestWithDescriptionEmpty = CreateValidUpdateChoreRequest();
            requestWithDescriptionEmpty.Description = string.Empty;
            errors.Add("The description must have between 2 and 2000 characters");
            yield return new object[] { requestWithDescriptionEmpty, errors };

            errors = new List<string>();
            var requestWithDescriptionTooShort = CreateValidUpdateChoreRequest();
            requestWithDescriptionTooShort.Description = faker.Random.String2(1);
            errors.Add("The description must have between 2 and 2000 characters");
            yield return new object[] { requestWithDescriptionTooShort, errors };

            errors = new List<string>();
            var requestWithDescriptionTooLong = CreateValidUpdateChoreRequest();
            requestWithDescriptionTooLong.Description = faker.Random.String2(2001);
            errors.Add("The description must have between 2 and 2000 characters");
            yield return new object[] { requestWithDescriptionTooLong, errors };

            errors = new List<string>();
            var requestWithInvalidStatus = CreateValidUpdateChoreRequest();
            requestWithInvalidStatus.Status = (EChoreStatus)999;
            errors.Add("The status must be valid");
            yield return new object[] { requestWithInvalidStatus, errors };
        }
    }
}