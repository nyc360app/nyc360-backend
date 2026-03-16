using Microsoft.Extensions.Caching.Distributed;
using NYC360.Application.Contracts;
using System.Text.Json;

namespace NYC360.Infrastructure.Cache;

public class RedisCachingService(IDistributedCache cache) : ICachingService
{
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var cachedValue = await cache.GetStringAsync(key, cancellationToken);

        return string.IsNullOrEmpty(cachedValue) ? default : JsonSerializer.Deserialize<T>(cachedValue);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null, CancellationToken cancellationToken = default)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpiration ?? TimeSpan.FromHours(24) // Default expiry
        };

        var jsonValue = JsonSerializer.Serialize(value);
        await cache.SetStringAsync(key, jsonValue, options, cancellationToken);
    }
}