using CensorshipService.Domain.SanitizedMessages;

namespace CensorshipService.Application.Common.Interfaces;

public interface ISanitizedMessagesRepository
{
    Task AddSanitizedMessageAsync(SanitizedMessage sanitizedMessage);
}