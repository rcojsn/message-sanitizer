using AdminService.Application.Common.Interfaces;
using ErrorOr;
using MediatR;

namespace AdminService.Application.SensitiveWords.Commands.DeleteSensitiveWord;

public class DeleteSensitiveWordCommandHandler(ISensitiveWordsRepository sensitiveWordsRepository) : IRequestHandler<DeleteSensitiveWordCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(DeleteSensitiveWordCommand request, CancellationToken cancellationToken)
    {
        var sensitiveWord = await sensitiveWordsRepository.GetByIdAsync(request.Id);
        
        if (sensitiveWord == null) return Error.NotFound(description: "Sensitive word not found");
        
        await sensitiveWordsRepository.DeleteSensitiveWordAsync(sensitiveWord);
        
        return Result.Deleted;
    }
}