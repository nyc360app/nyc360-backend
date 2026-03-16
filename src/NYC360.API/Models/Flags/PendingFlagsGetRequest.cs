namespace NYC360.API.Models.Flags;

public record PendingFlagsGetRequest(
    int Page = 1,
    int PageSize = 10
);