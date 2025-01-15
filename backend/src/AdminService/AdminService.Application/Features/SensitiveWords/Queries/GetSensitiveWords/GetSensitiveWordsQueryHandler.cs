using AdminService.Application.Common.Interfaces;
using AdminService.Application.Mappings;
using BleepGuard.Contracts.SensitiveWords;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AdminService.Application.Features.SensitiveWords.Queries.GetSensitiveWords;

public class GetSensitiveWordsQueryHandler(
    ISensitiveWordsRepository sensitiveWordsRepository,
    ILogger<GetSensitiveWordsQueryHandler> logger
) : IRequestHandler<GetSensitiveWordsQuery, IList<SensitiveWordResponse>>
{
    public async Task<IList<SensitiveWordResponse>> Handle(GetSensitiveWordsQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching all sensitive words");

        var sensitiveWords = await sensitiveWordsRepository.GetAllAsync();

        if (!sensitiveWords.Any())
        {
            logger.LogWarning("No sensitive words found");
            return [];
        }

        logger.LogInformation("Retrieved {Count} sensitive words", sensitiveWords.Count);
        return sensitiveWords.MapToSensitiveWordsResponse();
    }
}