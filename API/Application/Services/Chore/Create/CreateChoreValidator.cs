using FluentValidation;

namespace Application.Services.Chore.Create
{
    public class CreateChoreValidator : AbstractValidator<CreateChoreRequest>
    {
        public CreateChoreValidator()
        {
            RuleFor(request => request.Title)
                .NotNull()
                .WithMessage("The title must be informed")
                .Length(2, 600)
                .WithMessage("The title must have between 2 and 600 characters");

            RuleFor(request => request.Description)
                .NotNull()
                .WithMessage("The description must be informed")
                .Length(2, 2000)
                .WithMessage("The description must have between 2 and 2000 characters");

            RuleFor(request => request.Status)
                .IsInEnum()
                .WithMessage("The status must be valid");
        }
    }
}
