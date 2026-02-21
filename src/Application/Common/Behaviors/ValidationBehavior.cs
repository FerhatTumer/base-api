using FluentValidation;
using MediatR;
using TaskManagement.Application.Common.Exceptions;

namespace TaskManagement.Application.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        ValidationContext<TRequest> context = new(request);

        FluentValidation.Results.ValidationResult[] validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        Dictionary<string, string[]> errors = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .GroupBy(
                e => e.PropertyName,
                e => e.ErrorMessage)
            .ToDictionary(
                g => g.Key,
                g => g.Distinct().ToArray());

        if (errors.Count != 0)
        {
            throw new TaskManagement.Application.Common.Exceptions.ValidationException(errors);
        }

        return await next();
    }
}