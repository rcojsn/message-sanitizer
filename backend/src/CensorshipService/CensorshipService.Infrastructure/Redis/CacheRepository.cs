using BleepGuard.Contracts.SensitiveWords;
using CensorshipService.Application.Common.Interfaces;
using StackExchange.Redis;

namespace CensorshipService.Infrastructure.Redis;

public class CacheRepository(IConnectionMultiplexer redis) : ICacheRepository
{
    public async Task AddSensitiveWords(IList<SensitiveWordResponse> response)
    {
        var sensitiveWords = response.Select(s => s.Word);
        var db = redis.GetDatabase();
        foreach (var sensitiveWord in sensitiveWords)
        {
            await db.SetAddAsync("sensitiveWords", sensitiveWord);
        }
    }
}