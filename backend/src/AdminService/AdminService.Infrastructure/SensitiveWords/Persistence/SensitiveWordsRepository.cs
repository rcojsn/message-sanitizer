using AdminService.Application.Common.Interfaces;
using AdminService.Domain.SensitiveWords;
using BleepGuard.Infrastructure.Common;
using Dapper;

namespace AdminService.Infrastructure.SensitiveWords.Persistence;

public class SensitiveWordsRepository(IDbConnectionFactory connectionFactory) : ISensitiveWordsRepository
{
    public async Task AddSensitiveWordAsync(SensitiveWord sensitiveWord)
    {
        using var dbConnection = await connectionFactory.CreateConnectionAsync();
        
        await dbConnection.ExecuteAsync(
        """
                INSERT INTO SensitiveWords (Id, Word)
                VALUES (@Id, @Word)
            """, sensitiveWord);
    }

    public async Task<SensitiveWord?> GetByIdAsync(Guid sensitiveWordId)
    {
        using var dbConnection = await connectionFactory.CreateConnectionAsync();
        var sensitiveWord = await dbConnection
            .QuerySingleOrDefaultAsync<SensitiveWord>(
            "SELECT  TOP 1 * FROM SensitiveWords WHERE Id = @Id", new { Id = sensitiveWordId });
        return sensitiveWord;
    }

    public async Task<IList<SensitiveWord>> GetAllAsync()
    {
        using var dbConnection = await connectionFactory.CreateConnectionAsync();
        var results = await dbConnection
            .QueryAsync<SensitiveWord>("SELECT * FROM SensitiveWords");
        return results.ToList();
    }

    public async Task UpdateSensitiveWordAsync(SensitiveWord sensitiveWord)
    {
        using var dbConnection = await connectionFactory.CreateConnectionAsync();
        await dbConnection.ExecuteAsync("""
            UPDATE SensitiveWords
            SET Word = @Word
            WHERE Id = @Id
        """, sensitiveWord);
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

    public async Task<bool> Exists(string word)
    {
        using var dbConnection = await connectionFactory.CreateConnectionAsync();
        var sensitiveWord = await dbConnection
            .QuerySingleOrDefaultAsync<SensitiveWord>(
                "SELECT  TOP 1 * FROM SensitiveWords WHERE LOWER(Word) = LOWER(@Word)", 
                new { Word = word }
            );
        return sensitiveWord is not null;
    }
}