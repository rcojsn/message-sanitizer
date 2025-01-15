using AdminService.Application.Common.Interfaces;
using AdminService.Application.Mappings;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AdminService.Application.Features.SensitiveWords.Commands.UpdateSensitiveWord;

public class UpdateSensitiveWordCommandHandler(
    ISensitiveWordsRepository sensitiveWordsRepository,
    ILogger<UpdateSensitiveWordCommandHandler> logger,
    ICacheRepository cacheRepository) : IRequestHandler<UpdateSensitiveWordCommand, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdateSensitiveWordCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting update for sensitive word with ID {SensitiveWordId}", request.Id);

    var sensitiveWord = await sensitiveWordsRepository.GetByIdAsync(request.Id);

    if (sensitiveWord is null)
    {
        logger.LogWarning("Sensitive word with ID {SensitiveWordId} not found", request.Id);
        return Error.NotFound(description: "Sensitive word not found");
    }

    logger.LogInformation("Checking if sensitive word {SensitiveWord} already exists", request.Word);
    var exists = await sensitiveWordsRepository.Exists(request.Word);

    if (exists)
    {
        logger.LogWarning("Sensitive word {SensitiveWord} already exists", request.Word);
        return Error.Conflict(description: "Sensitive word already exists");
    }

    logger.LogInformation("Mapping updated data to sensitive word with ID {SensitiveWordId}", request.Id);
    sensitiveWord.MapFrom(request);

    logger.LogInformation("Attempting to update sensitive word with ID {SensitiveWordId} in the database", request.Id);
    var updated = await sensitiveWordsRepository.UpdateSensitiveWordAsync(sensitiveWord);

    if (!updated)
    {
        logger.LogError("Failed to update sensitive word with ID {SensitiveWordId} in the database", request.Id);
        return Error.Failure(description: "Failed to update sensitive word");
    }

    logger.LogInformation("Sensitive word with ID {SensitiveWordId} updated successfully in the database", request.Id);

    logger.LogInformation("Attempting to update sensitive word with ID {SensitiveWordId} in the cache", request.Id);
    var cacheUpdated = await cacheRepository.AddOrUpdateSensitiveWordAsync(sensitiveWord);

    if (!cacheUpdated)
    {
        logger.LogError("Failed to update sensitive word with ID {SensitiveWordId} in the cache", request.Id);
        return Error.Failure(description: "Failed to update cache");
    }

    logger.LogInformation("Sensitive word with ID {SensitiveWordId} updated successfully in the cache", request.Id);

    return Result.Updated;
    }
}