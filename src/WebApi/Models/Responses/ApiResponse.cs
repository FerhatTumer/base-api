namespace TaskManagement.WebApi.Models.Responses;

public record ApiResponse
{
    public bool Success { get; init; }

    public string Message { get; init; } = string.Empty;

    public DateTimeOffset Timestamp { get; init; }

    protected ApiResponse(bool success, string message)
    {
        Success = success;
        Message = message;
        Timestamp = DateTimeOffset.UtcNow;
    }

    public static ApiResponse SuccessResponse(string message = "Operation completed successfully")
        => new(true, message);

    public static ApiResponse Failure(string message)
        => new(false, message);
}