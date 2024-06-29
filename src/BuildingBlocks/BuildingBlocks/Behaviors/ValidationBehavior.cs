using BuildingBlocks.CQRS;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace BuildingBlocks.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        ValidationContext<TRequest> context = new(request);

        ValidationResult[] validationResults = await Task.WhenAll(tasks: validators.Select(v => v.ValidateAsync(context: context, cancellation: cancellationToken)));

        List<ValidationFailure> failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Any())
            throw new ValidationException(errors: failures);

        return await next();

    }
}