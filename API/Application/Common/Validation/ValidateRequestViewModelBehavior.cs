using FluentValidation;
using MediatR;

namespace Application.Common.Validation
{
    public class ValidateRequestViewModelBehavior<TRequest, TResponse>
            : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidateRequestViewModelBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var validationContext = new ValidationContext<TRequest>(request);

                var validationFailures = _validators
                    .Select(v => v.Validate(validationContext))
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();

                if (validationFailures.Any())
                    throw new Exceptions.ValidationException(validationFailures);
            }

            return next();
        }
    }
}
