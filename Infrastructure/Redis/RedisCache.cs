using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.Redis;

public class RedisCache(IDistributedCache cache) : IRedisCache
{
    public async Task<T?> GetDataAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var jsonData = await cache.GetStringAsync(key, cancellationToken);
        return jsonData != null
            ? JsonSerializer.Deserialize<T>(jsonData)
            : default;
    }

    public async Task SetDataAsync<T>(string key, T value, int timeSpan = 10, CancellationToken cancellationToken = default)
    {
        var option = new DistributedCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromHours(timeSpan));
        var jsonData = JsonSerializer.Serialize(value);
        await cache.SetStringAsync(key, jsonData, option, cancellationToken);
    }

    public async Task RemoveDataAsync(string key, CancellationToken cancellationToken = default)
    {
        await cache.RemoveAsync(key, cancellationToken);
    }
}