namespace NYC360.Domain.Wrappers;

public record PagedRequest(int Page = 1, int PageSize = 20);