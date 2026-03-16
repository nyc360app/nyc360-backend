using Microsoft.AspNetCore.Http;
using NYC360.Application.Contracts.Storage;

namespace NYC360.Infrastructure.Storage;

public class LocalStorageService : ILocalStorageService
{
    private static readonly string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
    
    public async Task<string> SaveFileAsync(IFormFile file, string subfolder, CancellationToken ct = default)
    {
        var uploadPath = Path.Combine(FolderPath, subfolder);
        
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

        var filePath = Path.Combine(FolderPath, subfolder, fileUrl);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}