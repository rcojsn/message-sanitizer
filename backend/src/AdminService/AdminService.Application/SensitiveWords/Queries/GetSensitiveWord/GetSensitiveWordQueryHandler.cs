using AdminService.Application.Common.Interfaces;
using AdminService.Domain.SensitiveWords;
using ErrorOr;
using MediatR;

namespace AdminService.Application.SensitiveWords.Queries.GetSensitiveWord;

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