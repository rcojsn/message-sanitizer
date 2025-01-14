using AdminService.Application.Common.Interfaces;
using AdminService.Domain.SensitiveWords;
using BleepGuard.Application.Common;
using StackExchange.Redis;

namespace AdminService.Infrastructure.Redis;

public class CacheRepository(IConnectionMultiplexer redis) : ICacheRepository
{
    public async Task<bool> AddOrUpdateSensitiveWordAsync(SensitiveWord sensitiveWord)
    {
        var db = redis.GetDatabase();
        var (id, word) = sensitiveWord;
        return await db
            .HashSetAsync(Constants.RedisSensitiveWordsKey, id.ToString(), word);
    }

    public async Task<bool> DeleteSensitiveWordByIdAsync(Guid id)
    {
        var db = redis.GetDatabase();
        return await db.HashDeleteAsync(Constants.RedisSensitiveWordsKey, id.ToString());
    }

}