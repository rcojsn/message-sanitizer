using AdminService.Application.Common.Interfaces;
using AdminService.Application.Mappings;
using BleepGuard.Contracts.SensitiveWords;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AdminService.Application.Features.SensitiveWords.Queries.GetSensitiveWord;

public class GetSensitiveWordQueryHandler(
    ISensitiveWordsRepository sensitiveWordsRepository,
    ILogger<GetSensitiveWordQueryHandler> logger
) : IRequestHandler<GetSensitiveWordQuery, ErrorOr<SensitiveWordResponse>>
{
    public async Task<ErrorOr<SensitiveWordResponse>> Handle(GetSensitiveWordQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching sensitive word with ID: {Id}", request.Id);
        
        var sensitiveWord = await sensitiveWordsRepository.GetByIdAsync(request.Id);

        if (sensitiveWord is null)
        {
            logger.LogWarning("Sensitive word with ID: {Id} not found", request.Id);
            return Error.NotFound(description: "Sensitive word not found");
        }

        logger.LogInformation("Sensitive word found: {SensitiveWord}", sensitiveWord.Word);
        return sensitiveWord.MapToSensitiveWordResponse();
    }
}