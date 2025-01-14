using System.Text.RegularExpressions;
using CensorshipService.Application.Common.Interfaces;
using MediatR;

namespace CensorshipService.Application.Features.SanitizedMessages.Commands.CreateSanitizedMessage;

public class CreateSanitizedMessageCommandHandler(ICacheRepository cacheRepository)
    : IRequestHandler<CreateSanitizedMessageCommand, string?>
{
    public async Task<string?> Handle(CreateSanitizedMessageCommand request, CancellationToken cancellationToken)
    {
        var cachedSensitiveWords = await cacheRepository.GetAllSensitiveWordsAsync();
        
        var pattern = string.Join("|", cachedSensitiveWords.Select(Regex.Escape));
        
        return Regex.Replace(
            request.Message,
            pattern,
            match => new string('*', match.Value.Length), 
            RegexOptions.IgnoreCase);
    }
}