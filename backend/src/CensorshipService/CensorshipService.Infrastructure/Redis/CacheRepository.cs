using BleepGuard.Contracts.SensitiveWords;
using CensorshipService.Application.Common.Interfaces;
using StackExchange.Redis;

namespace CensorshipService.Infrastructure.Redis;

public class CacheRepository(IConnectionMultiplexer redis) : ICacheRepository
{
    public async Task AddSensitiveWordsAsync(IList<SensitiveWordResponse> response)
    {
        var db = redis.GetDatabase();
        foreach (var sensitiveWord in response)
        {
            var (id, word) = sensitiveWord;
            await db.HashSetAsync("sensitiveWords", id.ToString(), word);
        }
    }
}