using AdminService.Application.Common.Interfaces;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AdminService.Application.Features.SensitiveWords.Commands.DeleteSensitiveWord;

public class DeleteSensitiveWordCommandHandler(
    ISensitiveWordsRepository sensitiveWordsRepository,
    ILogger<DeleteSensitiveWordCommandHandler> logger,
    ICacheRepository cacheRepository) : IRequestHandler<DeleteSensitiveWordCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(DeleteSensitiveWordCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting deletion of sensitive word with ID {SensitiveWordId}", request.Id);

        var sensitiveWord = await sensitiveWordsRepository.GetByIdAsync(request.Id);
        
        if (sensitiveWord == null)
        {
            logger.LogWarning("Sensitive word with ID {SensitiveWordId} not found", request.Id);
            return Error.NotFound(description: "Sensitive word not found");
        }

        logger.LogInformation("Attempting to delete sensitive word with ID {SensitiveWordId}", request.Id);
        var deleted = await sensitiveWordsRepository.DeleteSensitiveWordAsync(sensitiveWord);

        if (!deleted)
        {
            logger.LogError("Failed to delete sensitive word with ID {SensitiveWordId}", request.Id);
            return Error.Failure(description: "Failed to delete sensitive word");
        }

        logger.LogInformation("Sensitive word with ID {SensitiveWordId} deleted successfully", request.Id);

        logger.LogInformation("Attempting to delete sensitive word with ID {SensitiveWordId} from cache", request.Id);
        var cacheDeleted = await cacheRepository.DeleteSensitiveWordByIdAsync(request.Id);

        if (!cacheDeleted)
        {
            logger.LogError("Failed to delete sensitive word with ID {SensitiveWordId} from cache", request.Id);
            return Error.Failure(description: "Failed to delete sensitive word from cache");
        }

        logger.LogInformation("Sensitive word with ID {SensitiveWordId} deleted from cache successfully", request.Id);

        return Result.Deleted;
    }
}