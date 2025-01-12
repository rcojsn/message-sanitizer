using BleepGuard.Domain.SanitizedMessages;

namespace BleepGuard.Application.Common.Interfaces;

public interface ISanitizedMessagesRepository
{
    Task AddSanitizedMessageAsync(SanitizedMessage sanitizedMessage);
}