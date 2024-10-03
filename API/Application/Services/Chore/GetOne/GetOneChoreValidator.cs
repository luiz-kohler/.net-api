using FluentValidation;

namespace Application.Services.Chore.GetOne
{
    public class GetOneChoreValidator : AbstractValidator<GetOneChoreRequest>
    {
        public GetOneChoreValidator()
        {
            RuleFor(request => request.Id)
                .NotEmpty()
                .WithMessage("The Id must not be empty");
        }
    }
}
