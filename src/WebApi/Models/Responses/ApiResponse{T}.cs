namespace TaskManagement.WebApi.Models.Responses;

public record ApiResponse<T> : ApiResponse
{
    private ApiResponse(bool success, string message, T? data)
        : base(success, message)
    {
        Data = data;
    }

    public T? Data { get; init; }

    public static new ApiResponse<T> Success(T data, string message = "Operation completed successfully")
        => new(true, message, data);

    public static new ApiResponse<T> Failure(string message)
        => new(false, message, default);
}