using FluentValidation;

namespace Application.Services.Chore.Delete
{
    public class DeleteChoreValidator : AbstractValidator<DeleteChoreRequest>
    {
        public DeleteChoreValidator()
        {
            RuleFor(request => request.Id)
                .NotEmpty()
                .WithMessage("The Id must not be empty");
        }
    }
}
