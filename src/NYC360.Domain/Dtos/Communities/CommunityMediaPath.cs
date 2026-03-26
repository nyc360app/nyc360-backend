namespace NYC360.Domain.Dtos.Communities;

public static class CommunityMediaPath
{
    private const string LocalPrefix = "@local://";
    private const string CommunitiesFolder = "communities/";

    public static string? ToPublicPath(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var path = value.Trim();

        if (path.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            return path;

        if (path.StartsWith(LocalPrefix, StringComparison.OrdinalIgnoreCase))
            path = path[LocalPrefix.Length..];

        if (path.StartsWith('/'))
            path = path.TrimStart('/');

        if (path.Contains('/'))
            return path;

        return CommunitiesFolder + path;
    }
}
