using AdminService.Application.Common.Interfaces;
using AdminService.Domain.SensitiveWords;
using ErrorOr;
using MediatR;

namespace AdminService.Application.SensitiveWords.Commands.CreateSensitiveWord;

public class CreateSensitiveWordCommandHandler(ISensitiveWordsRepository sensitiveWordsRepository) 
    : IRequestHandler<CreateSensitiveWordCommand, ErrorOr<SensitiveWord>>
{
    public async Task<ErrorOr<SensitiveWord>> Handle(CreateSensitiveWordCommand request, CancellationToken cancellationToken)
    {
        var exists = await sensitiveWordsRepository.Exists(request.SensitiveWord);
        
        if (exists) return Error.Conflict("Sensitive word already exists");
        
        var sensitiveWord = new SensitiveWord
        {
            Id = Guid.NewGuid(),
            Word = request.SensitiveWord
        };
        
        await sensitiveWordsRepository.AddSensitiveWordAsync(sensitiveWord);

        return sensitiveWord;
    }
}