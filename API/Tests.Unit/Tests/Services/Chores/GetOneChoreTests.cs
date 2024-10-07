using Application.Common.Exceptions;
using Application.Services.Chore.GetOne;
using Bogus;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tests.Unit.Helpers;

namespace Tests.Unit.Tests.Services.Chores
{
    public class GetOneChoreTests : IClassFixture<ChoreServiceTestFixture>
    {
        private readonly ChoreServiceTestFixture _fixture;
        private readonly GetOneChoreHandler _handler;

        public GetOneChoreTests(ChoreServiceTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.ResetChoreRepository();
            _handler = new GetOneChoreHandler(_fixture.ChoreRepository, _fixture.Mapper);
        }

        [Fact]
        public async Task Should_Return_Chore_When_Chore_Exists()
        {
            var chore = CreateChore();
            var request = CreateValidGetOneChoreRequest(chore.Id);

            _fixture.ChoreRepository.SelectOne(Arg.Any<Expression<Func<Chore, bool>>>()).Returns(chore);

            var response = await _handler.Handle(request, _fixture.CancellationToken);

            await _fixture.ChoreRepository.Received().SelectOne(Arg.Any<Expression<Func<Chore, bool>>>());
            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(_fixture.Mapper.Map<GetOneChoreResponse>(chore));
        }

        [Theory]
        [MemberData(nameof(CreateInvalidRequestsWithMessagesError))]
        public void Should_Return_Failure_When_Validate_GetOne_Chore_Request_Invalid(
            GetOneChoreRequest request, List<string> expectedMessages)
        {
            var validator = new GetOneChoreValidator();

            var result = validator.Validate(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Count.Should().Be(expectedMessages.Count);
            result.Errors.Select(error => error.ErrorMessage)
                .Should().BeEquivalentTo(expectedMessages);
        }

        private static GetOneChoreRequest CreateValidGetOneChoreRequest(Guid choreId)
        {
            return new GetOneChoreRequest { Id = choreId };
        }

        private static Chore CreateChore()
        {
            return new Faker<Chore>()
                .RuleFor(chore => chore.Id, f => f.Random.Guid())
                .RuleFor(chore => chore.Title, f => f.Lorem.Sentence())
                .RuleFor(chore => chore.Description, f => f.Lorem.Paragraph())
                .RuleFor(chore => chore.Status, f => f.PickRandom<EChoreStatus>())
                .Generate();
        }

        public static IEnumerable<object[]> CreateInvalidRequestsWithMessagesError()
        {
            var errors = new List<string>();
            var requestWithEmptyId = new GetOneChoreRequest { Id = Guid.Empty };
            errors.Add("The Id must not be empty");
            yield return new object[] { requestWithEmptyId, errors };
        }
    }
}