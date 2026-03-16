using NYC360.Application.Contracts.Services;
using System.Text.RegularExpressions;

namespace NYC360.Infrastructure.Services;

public class SlugService : ISlugService
{
    public async Task<string> GenerateUniqueSlugAsync(string input, Func<string, Task<bool>> exists, CancellationToken ct)
    {
        var baseSlug = Normalize(input);
        var slug = baseSlug;
        var counter = 1;

        while (await exists(slug))
        {
            slug = $"{baseSlug}-{counter}";
            counter++;
        }

        return slug;
    }

    private static string Normalize(string input)
    {
        input = input.ToLowerInvariant();
        input = Regex.Replace(input, @"[^a-z0-9\s-]", "");
        input = Regex.Replace(input, @"\s+", " ").Trim();
        return input.Replace(" ", "-");
    }
}