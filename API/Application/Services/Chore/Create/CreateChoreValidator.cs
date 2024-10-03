using Domain.Enums;
using FluentValidation;

namespace Application.Services.Chore.Create
{
    public class CreateChoreValidator : AbstractValidator<CreateChoreRequest>
    {
        public CreateChoreValidator()
        {
            RuleFor(request => request.Title)
                .NotEmpty()
                .Length(2, 600)
                .WithMessage("The tittle must have between 2 and 600 characters");

            RuleFor(request => request.Description)
                .NotEmpty().Length(2, 2000)
                .WithMessage("The description must have between 2 and 2000 characters");

            RuleFor(request => request.Status)
                .IsInEnum()
                .WithMessage("The status must be valid");
        }
    }
}
