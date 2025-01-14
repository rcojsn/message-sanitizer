using AdminService.Application.Common.Interfaces;
using AdminService.Application.Mappings;
using BleepGuard.Contracts.SensitiveWords;
using ErrorOr;
using MediatR;

namespace AdminService.Application.Features.SensitiveWords.Queries.GetSensitiveWord;

public class GetSensitiveWordQueryHandler(ISensitiveWordsRepository sensitiveWordsRepository) : IRequestHandler<GetSensitiveWordQuery, ErrorOr<SensitiveWordResponse>>
{
    public async Task<ErrorOr<SensitiveWordResponse>> Handle(GetSensitiveWordQuery request, CancellationToken cancellationToken)
    {
        var sensitiveWord = await sensitiveWordsRepository.GetByIdAsync(request.Id);
        
        return sensitiveWord is null
            ? Error.NotFound(description: "Sensitive word not found")
            : sensitiveWord.MapToSensitiveWordResponse();
    }
}