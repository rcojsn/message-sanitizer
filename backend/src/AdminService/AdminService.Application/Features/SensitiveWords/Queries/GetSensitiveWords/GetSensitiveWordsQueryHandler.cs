using AdminService.Application.Common.Interfaces;
using AdminService.Application.Mappings;
using BleepGuard.Contracts.SensitiveWords;
using MediatR;

namespace AdminService.Application.Features.SensitiveWords.Queries.GetSensitiveWords;

public class GetSensitiveWordsQueryHandler(ISensitiveWordsRepository sensitiveWordsRepository) : IRequestHandler<GetSensitiveWordsQuery, IList<SensitiveWordResponse>>
{
    public async Task<IList<SensitiveWordResponse>> Handle(GetSensitiveWordsQuery request,
        CancellationToken cancellationToken)
    {
        var sensitiveWords = await sensitiveWordsRepository.GetAllAsync();
        return sensitiveWords.MapToSensitiveWordsResponse();
    }
}