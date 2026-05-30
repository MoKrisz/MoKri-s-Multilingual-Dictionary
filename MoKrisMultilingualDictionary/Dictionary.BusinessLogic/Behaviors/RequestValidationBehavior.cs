using Dictionary.BusinessLogic.Exceptions;
using FluentValidation;
using MediatR;

namespace Dictionary.BusinessLogic.Behaviors
{
    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var errors = validators
                .Select(x => x.Validate(context))
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .ToList();

            if (errors.Any())
            {
                throw new RequestValidationException(typeof(TRequest).ToString(), string.Join(", ", errors.Select(e => e.ErrorMessage)));
            }

            return await next();
        }
    }
}
