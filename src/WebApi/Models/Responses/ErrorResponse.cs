namespace TaskManagement.WebApi.Models.Responses;

public record ErrorResponse
{
    public string Type { get; init; } = string.Empty;

    public string Title { get; init; } = string.Empty;

    public int Status { get; init; }

    public string Detail { get; init; } = string.Empty;

    public string Instance { get; init; } = string.Empty;

    public string TraceId { get; init; } = string.Empty;

    public DateTimeOffset Timestamp { get; init; }

    public ErrorResponse(string type, string title, int status, string detail, string instance, string traceId)
    {
        Type = type;
        Title = title;
        Status = status;
        Detail = detail;
        Instance = instance;
        TraceId = traceId;
        Timestamp = DateTimeOffset.UtcNow;
    }
}