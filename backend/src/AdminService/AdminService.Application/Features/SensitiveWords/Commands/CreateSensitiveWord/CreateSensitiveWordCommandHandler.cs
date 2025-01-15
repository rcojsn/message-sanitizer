using AdminService.Application.Common.Interfaces;
using AdminService.Application.Mappings;
using AdminService.Domain.SensitiveWords;
using BleepGuard.Contracts.SensitiveWords;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AdminService.Application.Features.SensitiveWords.Commands.CreateSensitiveWord;

public class CreateSensitiveWordCommandHandler(
    ISensitiveWordsRepository sensitiveWordsRepository,
    ICacheRepository cacheRepository,
    ILogger<CreateSensitiveWordCommandHandler> logger)
    : IRequestHandler<CreateSensitiveWordCommand, ErrorOr<SensitiveWordResponse>>
{
    public async Task<ErrorOr<SensitiveWordResponse>> Handle(CreateSensitiveWordCommand request, CancellationToken cancellationToken)
    {
        var exists = await sensitiveWordsRepository.Exists(request.SensitiveWord);
        if (exists)
        {
            logger.LogWarning("Attempted to create a sensitive word that already exists: {SensitiveWord}", request.SensitiveWord);
            return Error.Conflict(description: "Sensitive word already exists");
        }

        var sensitiveWord = new SensitiveWord(Guid.NewGuid(), request.SensitiveWord);
        
        var added = await sensitiveWordsRepository.AddSensitiveWordAsync(sensitiveWord);
        if (!added)
        {
            logger.LogError("Failed to add sensitive word: {SensitiveWord}", request.SensitiveWord);
            return Error.Failure(description: "Failed to add sensitive word");
        }

        var cached = await cacheRepository.AddOrUpdateSensitiveWordAsync(sensitiveWord);
        if (!cached)
        {
            logger.LogError("Failed to cache sensitive word: {SensitiveWord}", request.SensitiveWord);
            return Error.Failure(description: "Failed to cache sensitive word");
        }

        logger.LogInformation("Successfully created and cached sensitive word: {SensitiveWord}", request.SensitiveWord);

        return sensitiveWord.MapToSensitiveWordResponse();
    }
}