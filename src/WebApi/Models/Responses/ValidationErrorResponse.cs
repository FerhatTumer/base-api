namespace TaskManagement.WebApi.Models.Responses;

public record ValidationErrorResponse : ApiResponse
{
    private ValidationErrorResponse(string message, IDictionary<string, string[]> errors)
        : base(false, message)
    {
        Errors = errors;
    }

    public IDictionary<string, string[]> Errors { get; init; }

    public static ValidationErrorResponse Failure(string message, IDictionary<string, string[]> errors)
        => new(message, errors);
}