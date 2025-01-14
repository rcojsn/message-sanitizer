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
        
        var deleted = await sensitiveWordsRepository.DeleteSensitiveWordAsync(sensitiveWord);
        
        if (!deleted) return Error.Failure(description: "Failed to delete sensitive word");
        
        var cacheDeleted = await cacheRepository.DeleteSensitiveWordByIdAsync(request.Id);
        
        if (!cacheDeleted) return Error.Failure(description: "Failed to delete sensitive word from cache");
        
        return Result.Deleted;
    }
}