using MediatR;
using Microsoft.Extensions.Logging;

namespace TaskManagement.Application.Common.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;
        DateTimeOffset startedAt = DateTimeOffset.UtcNow;

        _logger.LogInformation("Handling {RequestName} at {StartedAt} with {@Request}", requestName, startedAt, request);

        long start = Environment.TickCount64;
        TResponse response = await next();
        long durationMs = Environment.TickCount64 - start;

        _logger.LogInformation("Handled {RequestName} in {DurationMs}ms with {@Response}", requestName, durationMs, response);

        return response;
    }
}