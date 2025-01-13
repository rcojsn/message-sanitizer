using AdminService.Application.Common.Interfaces;
using AdminService.Domain.SensitiveWords;
using BleepGuard.Application.Common;
using StackExchange.Redis;

namespace AdminService.Infrastructure.Redis;

public class CacheRepository(IConnectionMultiplexer redis) : ICacheRepository
{
    public async Task AddOrUpdateSensitiveWordAsync(SensitiveWord sensitiveWord)
    {
        var db = redis.GetDatabase();
        var (id, word) = sensitiveWord;
        await db.HashSetAsync(Constants.RedisSensitiveWordsKey, id.ToString(), word);
    }

    public async Task DeleteSensitiveWordByIdAsync(Guid id)
    {
        var db = redis.GetDatabase();
        await db.HashDeleteAsync(Constants.RedisSensitiveWordsKey, id.ToString());
    }

}