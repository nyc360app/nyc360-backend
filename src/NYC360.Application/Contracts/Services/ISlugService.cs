namespace NYC360.Application.Contracts.Services;

public interface ISlugService
{
    Task<string> GenerateUniqueSlugAsync(string input, Func<string, Task<bool>> exists, CancellationToken ct);
}