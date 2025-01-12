namespace CensorshipService.Application.Common.Interfaces;

public interface ICacheRepository
{
    Task SetCacheValueAsync<T>(string key, T value, TimeSpan expiration);
    Task<T> GetCacheValueAsync<T>(string key);
}