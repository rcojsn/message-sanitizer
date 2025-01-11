using BleepGuard.Application.Common.Interfaces;
using BleepGuard.Domain.SensitiveWords;
using MediatR;

namespace BleepGuard.Application.SensitiveWords.Queries.GetSensitiveWords;

public class GetSensitiveWordsQueryHandler(ISensitiveWordsRepository sensitiveWordsRepository) : IRequestHandler<GetSensitiveWordsQuery, IList<SensitiveWord>>
{
    public async Task<IList<SensitiveWord>> Handle(GetSensitiveWordsQuery request, CancellationToken cancellationToken)
    => await sensitiveWordsRepository.GetAllAsync();
}