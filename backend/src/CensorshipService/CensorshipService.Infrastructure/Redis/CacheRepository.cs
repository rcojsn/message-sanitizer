using BleepGuard.Application.Common;
using BleepGuard.Contracts.SensitiveWords;
using CensorshipService.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace CensorshipService.Infrastructure.Redis;

public class CacheRepository(IConnectionMultiplexer redis, ILogger<CacheRepository> logger) : ICacheRepository
{
    public async Task<HashSet<string>> GetAllSensitiveWordsAsync()
    {
        logger.LogInformation("Obtain an interactive connection to a database inside redis");
        var db = redis.GetDatabase();
        
        logger.LogInformation("Fetching all sensitive words from Redis");
        var sensitiveWords = await db.HashGetAllAsync(Constants.RedisSensitiveWordsKey);
        
        if (sensitiveWords.Length is 0)
        {
            logger.LogWarning("No sensitive words found in Redis");
            return [];
        }
        
        logger.LogInformation("[{sensitiveWordsLength}] Sensitive words found in Redis",sensitiveWords.Length);
        
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