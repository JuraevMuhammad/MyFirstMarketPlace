namespace Infrastructure.Redis;

public interface IRedisCache
{
    Task<T?> GetDataAsync<T>(string key, CancellationToken cancellationToken = default);
    Task SetDataAsync<T>(string key, T value, int timeSpan = 10, CancellationToken cancellationToken = default);
    Task RemoveDataAsync(string key, CancellationToken cancellationToken = default);
}