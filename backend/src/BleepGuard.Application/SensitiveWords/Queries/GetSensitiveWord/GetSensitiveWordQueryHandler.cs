using BleepGuard.Application.Common.Interfaces;
using BleepGuard.Domain.SensitiveWords;
using ErrorOr;
using MediatR;

namespace BleepGuard.Application.SensitiveWords.Queries.GetSensitiveWord;

public class GetSensitiveWordQueryHandler(ISensitiveWordsRepository sensitiveWordsRepository) : IRequestHandler<GetSensitiveWordQuery, ErrorOr<SensitiveWord>>
{
    public async Task<ErrorOr<SensitiveWord>> Handle(GetSensitiveWordQuery request, CancellationToken cancellationToken)
    {
        var sensitiveWord = await sensitiveWordsRepository.GetByIdAsync(request.Id);
        
        return sensitiveWord is null
            ? Error.NotFound(description: "Sensitive word not found")
            : sensitiveWord;
    }
}