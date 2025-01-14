using AdminService.Application.Common.Interfaces;
using AdminService.Application.Mappings;
using ErrorOr;
using MediatR;

namespace AdminService.Application.Features.SensitiveWords.Commands.UpdateSensitiveWord;

public class UpdateSensitiveWordCommandHandler(
    ISensitiveWordsRepository sensitiveWordsRepository,
    ICacheRepository cacheRepository) : IRequestHandler<UpdateSensitiveWordCommand, ErrorOr<Updated>>
{
    public async Task<ErrorOr<Updated>> Handle(UpdateSensitiveWordCommand request, CancellationToken cancellationToken)
    {
        var sensitiveWord = await sensitiveWordsRepository.GetByIdAsync(request.Id);

        if (sensitiveWord == null) return Error.NotFound(description: "Sensitive word not found");

        var exists = await sensitiveWordsRepository.Exists(request.Word);
        
        if (exists) return Error.Conflict(description: "Sensitive word already exists");
        
        sensitiveWord.MapFrom(request);

        var updated = await sensitiveWordsRepository.UpdateSensitiveWordAsync(sensitiveWord);
        
        if (!updated) return Error.Failure(description: "Failed to update sensitive word");

        var cacheUpdated = await cacheRepository.AddOrUpdateSensitiveWordAsync(sensitiveWord);
        
        if (!cacheUpdated) return Error.Failure(description: "Failed to update cache");
        
        return Result.Updated;
    }
}