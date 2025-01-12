using CensorshipService.Application.Common.Interfaces;
using CensorshipService.Domain.SanitizedMessages;
using ErrorOr;
using MediatR;

namespace CensorshipService.Application.SanitizedMessages.Commands.CreateSanitizedMessage;

public class CreateSanitizedMessageCommandHandler(ISanitizedMessagesRepository sanitizedMessagesRepository)
    : IRequestHandler<CreateSanitizedMessageCommand, ErrorOr<SanitizedMessage>>
{
    public async Task<ErrorOr<SanitizedMessage>> Handle(CreateSanitizedMessageCommand request, CancellationToken cancellationToken)
    {
        var sanitizedMessage = new SanitizedMessage(Guid.NewGuid(), request.Message);
        
        await sanitizedMessagesRepository.AddSanitizedMessageAsync(sanitizedMessage);
        
        return sanitizedMessage;
    }
}