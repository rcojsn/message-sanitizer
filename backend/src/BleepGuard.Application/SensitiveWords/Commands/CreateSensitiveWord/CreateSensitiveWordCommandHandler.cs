using BleepGuard.Application.Common.Interfaces;
using BleepGuard.Domain.SensitiveWords;
using ErrorOr;
using MediatR;

namespace BleepGuard.Application.SensitiveWords.Commands.CreateSensitiveWord;

public class CreateSensitiveWordCommandHandler(ISensitiveWordsRepository sensitiveWordsRepository) 
    : IRequestHandler<CreateSensitiveWordCommand, ErrorOr<SensitiveWord>>
{
    public async Task<ErrorOr<SensitiveWord>> Handle(CreateSensitiveWordCommand request, CancellationToken cancellationToken)
    {
        var sensitiveWord = new SensitiveWord(Guid.NewGuid(), request.SensitiveWord);
        
        await sensitiveWordsRepository.AddSensitiveWordAsync(sensitiveWord);

        return sensitiveWord;
    }
}