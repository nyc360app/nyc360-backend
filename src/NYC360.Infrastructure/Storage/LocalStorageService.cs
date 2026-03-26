using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NYC360.Application.Contracts.Storage;

namespace NYC360.Infrastructure.Storage;

public class LocalStorageService(IWebHostEnvironment environment) : ILocalStorageService
{
    public async Task<string> SaveFileAsync(IFormFile file, string subfolder, CancellationToken ct = default)
    {
        var uploadPath = Path.Combine(GetStorageRootPath(), subfolder);
        
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        var rawFileName = file.FileName;
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitizedFileName = new string(rawFileName
            .Where(c => !invalidChars.Contains(c))
            .ToArray())
            .Replace(" ", "_"); // Also replace spaces for better URL handling if needed
        
        var fileExtension = Path.GetExtension(sanitizedFileName);
        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine(uploadPath, uniqueFileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream, ct);

        return uniqueFileName;
    }

    public void DeleteFile(string? fileUrl, string subfolder)
    {
        if (string.IsNullOrEmpty(fileUrl))
        {
            return;
        }

        var normalized = fileUrl.Trim().Replace("\\", "/");
        if (normalized.StartsWith("@local://", StringComparison.OrdinalIgnoreCase))
            normalized = normalized["@local://".Length..];
        if (normalized.StartsWith('/'))
            normalized = normalized.TrimStart('/');

        var fileName = Path.GetFileName(normalized);
        var filePath = Path.Combine(GetStorageRootPath(), subfolder, fileName);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    private string GetStorageRootPath()
    {
        var webRoot = environment.WebRootPath;
        if (!string.IsNullOrWhiteSpace(webRoot))
            return webRoot;

        return Path.Combine(environment.ContentRootPath, "wwwroot");
    }
}
