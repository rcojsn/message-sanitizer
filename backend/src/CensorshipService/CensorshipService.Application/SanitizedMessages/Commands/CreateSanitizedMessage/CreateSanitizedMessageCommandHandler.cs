using System.Text.RegularExpressions;
using CensorshipService.Application.Common.Interfaces;
using CensorshipService.Domain.SanitizedMessages;
using ErrorOr;
using MediatR;

namespace CensorshipService.Application.SanitizedMessages.Commands.CreateSanitizedMessage;

public class CreateSanitizedMessageCommandHandler(
    ISanitizedMessagesRepository sanitizedMessagesRepository,
    ICacheRepository cacheRepository)
    : IRequestHandler<CreateSanitizedMessageCommand, ErrorOr<SanitizedMessage>>
{
    public async Task<ErrorOr<SanitizedMessage>> Handle(CreateSanitizedMessageCommand request, CancellationToken cancellationToken)
    {
        var cachedSensitiveWords = await cacheRepository.GetSensitiveWordsAsync();
        
        var pattern = string.Join("|", cachedSensitiveWords.Select(Regex.Escape));
        
        var sanitizedMessage = Regex.Replace(
            request.Message,
            pattern,
            match => new string('*', match.Value.Length), 
            RegexOptions.IgnoreCase);
        
        var sanitizedMessageEntry = new SanitizedMessage(Guid.NewGuid(), sanitizedMessage);
        
        await sanitizedMessagesRepository.AddSanitizedMessageAsync(sanitizedMessageEntry);
        
        return sanitizedMessageEntry;
    }
}