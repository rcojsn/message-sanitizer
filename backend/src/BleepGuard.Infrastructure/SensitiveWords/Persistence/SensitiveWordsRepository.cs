using BleepGuard.Application.Common.Interfaces;
using BleepGuard.Domain.SensitiveWords;
using BleepGuard.Infrastructure.Common;
using Dapper;

namespace BleepGuard.Infrastructure.SensitiveWords.Persistence;

public class SensitiveWordsRepository(IDbConnectionFactory connectionFactory) : ISensitiveWordsRepository
{
    public async Task AddSensitiveWordAsync(SensitiveWord sensitiveWord)
    {
        using var dbConnection = await connectionFactory.CreateConnectionAsync();
        
        await dbConnection.ExecuteAsync(
        """
                INSERT INTO SensitiveWords
                VALUES (@Id, @Word)
            """, sensitiveWord);
    }

    public async Task<SensitiveWord?> GetByIdAsync(Guid sensitiveWordId)
    {
        using var dbConnection = await connectionFactory.CreateConnectionAsync();
        var sensitiveWord = await dbConnection
            .QuerySingleOrDefaultAsync<SensitiveWord>(
            "SELECT * FROM SensitiveWords WHERE Id = @Id limit 1", new { Id = sensitiveWordId });
        return sensitiveWord;
    }

    public async Task<IList<SensitiveWord>> GetAllAsync()
    {
        using var dbConnection = await connectionFactory.CreateConnectionAsync();
        var results = await dbConnection
            .QueryAsync<SensitiveWord>("SELECT * FROM SensitiveWords");
        return results.ToList();
    }

    public async Task<bool> UpdateSensitiveWordAsync(SensitiveWord sensitiveWord)
    {
        using var dbConnection = await connectionFactory.CreateConnectionAsync();
        return await dbConnection.ExecuteAsync("""
            UPDATE SensitiveWords
            SET Word = @Word
            WHERE Id = @Id
        """, sensitiveWord) > 0;
    }
    
    public async Task DeleteSensitiveWordAsync(SensitiveWord sensitiveWord)
    {
        using var dbConnection = await connectionFactory.CreateConnectionAsync();
        await dbConnection.ExecuteAsync("""
            DELETE FROM SensitiveWords
            WHERE Id = @Id
        """
            , sensitiveWord);
    }
}