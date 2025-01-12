using BleepGuard.Application.Common.Interfaces;
using BleepGuard.Domain.SanitizedMessages;
using ErrorOr;
using MediatR;

namespace BleepGuard.Application.SanitizedMessages.Commands.CreateSanitizedMessage;

public class CreateSanitizedMessageCommandHandler(
    ISensitiveWordsRepository sensitiveWordsRepository,
    ISanitizedMessagesRepository sanitizedMessagesRepository
) : IRequestHandler<CreateSanitizedMessageCommand, ErrorOr<SanitizedMessage>>
{
    public async Task<ErrorOr<SanitizedMessage>> Handle(CreateSanitizedMessageCommand request, CancellationToken cancellationToken)
    {
        var sanitizedMessage = new SanitizedMessage(Guid.NewGuid(), request.Message);
        
        await sanitizedMessagesRepository.AddSanitizedMessageAsync(sanitizedMessage);

        return sanitizedMessage;
    }
}