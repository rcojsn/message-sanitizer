using AdminService.Application.Common.Interfaces;
using AdminService.Domain.SensitiveWords;
using MediatR;

namespace AdminService.Application.SensitiveWords.Queries.GetSensitiveWords;

public class GetSensitiveWordsQueryHandler(ISensitiveWordsRepository sensitiveWordsRepository) : IRequestHandler<GetSensitiveWordsQuery, IList<SensitiveWord>>
{
    public async Task<IList<SensitiveWord>> Handle(GetSensitiveWordsQuery request, CancellationToken cancellationToken)
    => await sensitiveWordsRepository.GetAllAsync();
}