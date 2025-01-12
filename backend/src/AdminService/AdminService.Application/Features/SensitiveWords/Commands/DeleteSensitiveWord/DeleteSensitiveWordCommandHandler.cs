using AdminService.Application.Common.Interfaces;
using ErrorOr;
using MediatR;

namespace AdminService.Application.Features.SensitiveWords.Commands.DeleteSensitiveWord;

public class DeleteSensitiveWordCommandHandler(
    ISensitiveWordsRepository sensitiveWordsRepository,
    ICacheRepository cacheRepository) : IRequestHandler<DeleteSensitiveWordCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(DeleteSensitiveWordCommand request, CancellationToken cancellationToken)
    {
        var sensitiveWord = await sensitiveWordsRepository.GetByIdAsync(request.Id);
        
        if (sensitiveWord == null) return Error.NotFound(description: "Sensitive word not found");
        
        await sensitiveWordsRepository.DeleteSensitiveWordAsync(sensitiveWord);
        
        await cacheRepository.DeleteSensitiveWordByIdAsync(request.Id);
        
        return Result.Deleted;
    }
}