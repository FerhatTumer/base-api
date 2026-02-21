using MediatR;
using TaskManagement.Application.Common.Models;
using TaskManagement.Domain.Common;

namespace TaskManagement.Application.Common.Behaviors;

public sealed class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IUnitOfWork _unitOfWork;

    public TransactionBehavior(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!IsResultResponse())
        {
            return await next();
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            TResponse response = await next();
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return response;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    private static bool IsResultResponse()
    {
        Type responseType = typeof(TResponse);
        if (responseType == typeof(Result))
        {
            return true;
        }

        return responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>);
    }
}