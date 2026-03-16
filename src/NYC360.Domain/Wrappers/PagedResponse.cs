namespace NYC360.Domain.Wrappers;

public class PagedResponse<T>
{
    public bool IsSuccess { get; init; } = true;

    public IEnumerable<T>? Data { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }

    public int TotalCount { get; init; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    public ApiError? Error { get; init; }

    private PagedResponse() {}

    public static PagedResponse<T> Create(IEnumerable<T> data, int page, int pageSize, int totalCount)
        => new()
        {
            Data = data,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            IsSuccess = true
        };

    public static PagedResponse<T> Failure(ApiError error)
        => new()
        {
            IsSuccess = false,
            Error = error
        };
}