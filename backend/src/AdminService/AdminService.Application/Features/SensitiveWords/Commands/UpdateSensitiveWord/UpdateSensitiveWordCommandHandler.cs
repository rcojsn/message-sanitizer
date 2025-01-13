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

        await sensitiveWordsRepository.UpdateSensitiveWordAsync(sensitiveWord);

        await cacheRepository.AddOrUpdateSensitiveWordAsync(sensitiveWord);
        
        return Result.Updated;
    }
}