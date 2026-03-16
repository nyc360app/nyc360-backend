namespace NYC360.Domain.Wrappers;

public record StandardResponse
{
    public bool IsSuccess { get; init; }
    public ApiError? Error { get; init; }
    
    public static StandardResponse Success() => new() { IsSuccess = true };
    public static StandardResponse Failure(ApiError error) => new() { IsSuccess = false, Error = error };
}

public record StandardResponse<T>
{
    public bool IsSuccess { get; init; }
    public T? Data { get; init; }
    public ApiError? Error { get; init; }

    // Static factory methods for clean creation
    public static StandardResponse<T> Success(T data) => new() { IsSuccess = true, Data = data };
    public static StandardResponse<T> Failure(ApiError error) => new() { IsSuccess = false, Error = error };
}

public record ApiError(string Code, string Message);