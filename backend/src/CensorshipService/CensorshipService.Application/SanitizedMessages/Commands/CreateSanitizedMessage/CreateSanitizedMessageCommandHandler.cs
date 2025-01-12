using BleepGuard.Contracts.SensitiveWords;
using CensorshipService.Application.Common.Interfaces;
using CensorshipService.Application.Common.Interfaces.External;
using CensorshipService.Domain.SanitizedMessages;
using ErrorOr;
using MediatR;

namespace CensorshipService.Application.SanitizedMessages.Commands.CreateSanitizedMessage;

public class CreateSanitizedMessageCommandHandler(
    IAdminServiceApiClient adminServiceApi,
    ISanitizedMessagesRepository sanitizedMessagesRepository,
    ICacheRepository cacheRepository)
    : IRequestHandler<CreateSanitizedMessageCommand, ErrorOr<SanitizedMessage>>
{
    public async Task<ErrorOr<SanitizedMessage>> Handle(CreateSanitizedMessageCommand request, CancellationToken cancellationToken)
    {
        IList<SensitiveWordResponse> sensitiveWordResponses = [];
        
        var cachedSensitiveWords = await cacheRepository
            .GetCacheValueAsync<IList<SensitiveWordResponse>>("SensitiveWords");

        if (cachedSensitiveWords is not null)
        {
            sensitiveWordResponses = cachedSensitiveWords;
        }
        else
        {
            var response = await adminServiceApi
                .GetSensitiveWords();
            
            await cacheRepository.SetCacheValueAsync("SensitiveWords", response, TimeSpan.FromMinutes(10));
        }
        
        var sanitizedMessage = new SanitizedMessage(Guid.NewGuid(), request.Message);
        
        await sanitizedMessagesRepository.AddSanitizedMessageAsync(sanitizedMessage);

        return sanitizedMessage;
    }
}