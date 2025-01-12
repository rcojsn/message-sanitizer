using BleepGuard.Application.Common.Interfaces;
using BleepGuard.Domain.SanitizedMessages;
using BleepGuard.Infrastructure.Common;
using Dapper;

namespace BleepGuard.Infrastructure.SanitizedMessages.Persistence;

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