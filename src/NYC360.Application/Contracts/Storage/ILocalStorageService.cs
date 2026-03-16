using Microsoft.AspNetCore.Http;

namespace NYC360.Application.Contracts.Storage;

public interface ILocalStorageService
{
    Task<string> SaveFileAsync(IFormFile file, string subfolder, CancellationToken ct = default);
    void DeleteFile(string? fileUrl, string subfolder);
}