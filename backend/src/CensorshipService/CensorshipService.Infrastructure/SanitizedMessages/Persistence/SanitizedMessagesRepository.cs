using BleepGuard.Infrastructure.Common;
using CensorshipService.Application.Common.Interfaces;
using CensorshipService.Domain.SanitizedMessages;
using Dapper;

namespace CensorshipService.Infrastructure.SanitizedMessages.Persistence;

public class SanitizedMessagesRepository(IDbConnectionFactory connectionFactory) : ISanitizedMessagesRepository
{
    public async Task AddSanitizedMessageAsync(SanitizedMessage sanitizedMessage)
    {
        using var dbConnection = await connectionFactory.CreateConnectionAsync();
        
        await dbConnection.ExecuteAsync(
            """
                INSERT INTO SanitizedMessages
                VALUES(@Id, @Message)
            """, sanitizedMessage);
    }
}