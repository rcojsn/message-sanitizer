using BleepGuard.Application.Common;
using BleepGuard.Contracts.SensitiveWords;
using CensorshipService.Application.Common.Interfaces;
using StackExchange.Redis;

namespace CensorshipService.Infrastructure.Redis;

public class CacheRepository(IConnectionMultiplexer redis) : ICacheRepository
{
    public async Task<HashSet<string>> GetAllSensitiveWordsAsync()
    {
        var db = redis.GetDatabase();
        var sensitiveWords = await db.HashGetAllAsync(Constants.RedisSensitiveWordsKey);
        
        return sensitiveWords
            .Select(s => s.Value.ToString())
            .OrderByDescending(o => o.Length)
            .ToHashSet();
    }

    public async Task AddSensitiveWordsAsync(IList<SensitiveWordResponse> response)
    {
        if (!response.Any())
        {
            throw new ArgumentNullException(
                nameof(response), 
                "The list of sensitive words cannot be null or empty");
        }
        
        var db = redis.GetDatabase();
        
        foreach (var sensitiveWord in response)
        {
            var (id, word) = sensitiveWord;
            await db.HashSetAsync(
                Constants.RedisSensitiveWordsKey, 
                id.ToString(), 
                word
            );
        }
    }
}