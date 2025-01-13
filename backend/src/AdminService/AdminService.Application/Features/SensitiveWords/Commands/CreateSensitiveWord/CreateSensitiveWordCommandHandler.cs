using AdminService.Application.Common.Interfaces;
using AdminService.Application.Mappings;
using AdminService.Domain.SensitiveWords;
using BleepGuard.Contracts.SensitiveWords;
using ErrorOr;
using MediatR;

namespace AdminService.Application.Features.SensitiveWords.Commands.CreateSensitiveWord;

public class CreateSensitiveWordCommandHandler(
    ISensitiveWordsRepository sensitiveWordsRepository,
    ICacheRepository cacheRepository)
    : IRequestHandler<CreateSensitiveWordCommand, ErrorOr<SensitiveWordResponse>>
{
    public async Task<ErrorOr<SensitiveWordResponse>> Handle(CreateSensitiveWordCommand request, CancellationToken cancellationToken)
    {
        var exists = await sensitiveWordsRepository.Exists(request.SensitiveWord);
        
        if (exists) return Error.Conflict("Sensitive word already exists");
        
        var sensitiveWord = new SensitiveWord(Guid.NewGuid(), request.SensitiveWord);
        
        var added = await sensitiveWordsRepository.AddSensitiveWordAsync(sensitiveWord);

        if (!added) return Error.Failure("Failed to add sensitive word");
        
        await cacheRepository.AddOrUpdateSensitiveWordAsync(sensitiveWord);

        return sensitiveWord.MapToSensitiveWordResponse();
    }
}