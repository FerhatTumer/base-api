namespace TaskManagement.Application.Common.Exceptions;

public sealed class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException()
        : base("Access denied.")
    {
    }
}