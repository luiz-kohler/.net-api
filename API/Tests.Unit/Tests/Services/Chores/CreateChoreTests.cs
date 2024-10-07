using Application.Services.Chore.Create;
using Bogus;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using NSubstitute;
using Tests.Unit.Helpers;

namespace Tests.Unit.Tests.Services.Chores
{
    public class CreateChoreTests : IClassFixture<ChoreServiceTestFixture>
    {
        private readonly ChoreServiceTestFixture _fixture;
        private readonly CreateChoreHandler _handler;

        public CreateChoreTests(ChoreServiceTestFixture fixture)
        {
            _fixture = fixture;
            _handler = new CreateChoreHandler(_fixture.ChoreRepository, _fixture.Mapper);
        }

        [Fact]
        public async Task Should_Return_Success_When_Create_Chore()
        {
            var createChoreRequest = CreateValidCreateChoreRequest();

            var response = await _handler.Handle(createChoreRequest, _fixture.CancellationToken);

            await _fixture.ChoreRepository.Received().Insert(Arg.Any<Chore>());
            response.Id.Should().NotBeEmpty();
        }

        [Theory]
        [MemberData(nameof(CreateInvalidRequestsWithMessageError))]
        public async Task Should_Return_Failure_When_Validate_Request_Invalid(CreateChoreRequest request, List<string> expectedMessages)
        {
            var validation = new CreateChoreValidator().Validate(request);

            validation.IsValid.Should().BeFalse();   
            validation.Errors.Count.Should().Be(expectedMessages.Count);   
        }

        private static CreateChoreRequest CreateValidCreateChoreRequest()
        {
            return new Faker<CreateChoreRequest>()
                .RuleFor(chore => chore.Title, faker => faker.Random.Word())
                .RuleFor(chore => chore.Description, faker => faker.Random.Words())
                .RuleFor(chore => chore.Status, faker => faker.Random.Enum<EChoreStatus>())
                .Generate();
        }

        public static IEnumerable<object[]> CreateInvalidRequestsWithMessageError()
        {
            var faker = new Faker();

            var errors = new List<string>();

            var requestWithTitleNull = CreateValidCreateChoreRequest();
            requestWithTitleNull.Title = null;
            errors.Add("The title must be informed");
            yield return new object[] { requestWithTitleNull, errors };

            errors = new List<string>();
            var requestWithTitleEmpty = CreateValidCreateChoreRequest();
            requestWithTitleEmpty.Title = string.Empty;
            errors.Add("The title must be informed");
            yield return new object[] { requestWithTitleEmpty, errors };

            errors = new List<string>();
            var requestWithTitleMoreThan600Characters = CreateValidCreateChoreRequest();
            requestWithTitleMoreThan600Characters.Title = faker.Random.String(601);
            errors.Add("The title must have between 2 and 600 characters");
            yield return new object[] { requestWithTitleMoreThan600Characters, errors };

            errors = new List<string>();
            var requestWithTitleLessThan2Characters = CreateValidCreateChoreRequest();
            requestWithTitleLessThan2Characters.Title = faker.Random.String(1);
            errors.Add("The title must have between 2 and 600 characters");
            yield return new object[] { requestWithTitleLessThan2Characters, errors };

            errors = new List<string>();
            var requestWithDescriptionNull = CreateValidCreateChoreRequest();
            requestWithDescriptionNull.Description = null;
            errors.Add("The description must be informed");
            yield return new object[] { requestWithDescriptionNull, errors };

            errors = new List<string>();
            var requestWithDescriptionEmpty = CreateValidCreateChoreRequest();
            requestWithDescriptionEmpty.Description = string.Empty;
            errors.Add("The description must be informed");
            yield return new object[] { requestWithDescriptionEmpty, errors };

            errors = new List<string>();
            var requestWithDescriptionMoreThan2000Characters = CreateValidCreateChoreRequest();
            requestWithDescriptionMoreThan2000Characters.Description = faker.Random.String(2001);
            errors.Add("The description must have between 2 and 2000 characters");
            yield return new object[] { requestWithDescriptionMoreThan2000Characters, errors };

            errors = new List<string>();
            var requestWithDescriptionLessThan2Characters = CreateValidCreateChoreRequest();
            requestWithDescriptionLessThan2Characters.Description = faker.Random.String(1);
            errors.Add("The description must have between 2 and 2000 characters");
            yield return new object[] { requestWithDescriptionLessThan2Characters, errors };
        }
    }
}
