using AdminService.Application.Common.Interfaces;
using AdminService.Domain.SensitiveWords;
using StackExchange.Redis;

namespace AdminService.Infrastructure.Redis;

public class CacheRepository(IConnectionMultiplexer redis) : ICacheRepository
{
    public async Task AddOrUpdateSensitiveWordAsync(SensitiveWord sensitiveWord)
    {
        var db = redis.GetDatabase();
        var (id, word) = sensitiveWord;
        await db.HashSetAsync("sensitiveWords", id.ToString(), word);
    }

    public async Task DeleteSensitiveWordByIdAsync(Guid id)
    {
        var db = redis.GetDatabase();
        await db.HashDeleteAsync("sensitiveWords", id.ToString());
    }

}