using BleepGuard.Application.Common.Interfaces;
using ErrorOr;
using MediatR;

namespace BleepGuard.Application.SensitiveWords.Commands.UpdateSensitiveWord;

public class UpdateSensitiveWordCommandHandler(ISensitiveWordsRepository sensitiveWordsRepository) : IRequestHandler<UpdateSensitiveWordCommand, ErrorOr<bool>>
{
    public async Task<ErrorOr<bool>> Handle(UpdateSensitiveWordCommand request, CancellationToken cancellationToken)
    {
        var sensitiveWord = await sensitiveWordsRepository.GetByIdAsync(request.Id);

        if (sensitiveWord == null) return Error.NotFound(description: "Sensitive word not found");

        await sensitiveWordsRepository.UpdateSensitiveWordAsync(sensitiveWord);
    }
}