using BleepGuard.Application.SanitizedMessages.Commands.CreateSanitizedMessage;
using BleepGuard.Contracts.SanitizedMessages;
using MassTransit;
using MediatR;

namespace BleepGuard.Infrastructure.SanitizedMessages.Consumers;

public class CreateSanitizedMessageConsumer(IMediator mediator) : IConsumer<CreateSanitizedMessageRequest>
{
    public async Task Consume(ConsumeContext<CreateSanitizedMessageRequest> context)
    {
        var command = new CreateSanitizedMessageCommand(context.Message.Message);
        var response = await mediator.Send(command, CancellationToken.None);
        await context.RespondAsync<SanitizedMessageResponse>(response);
    }
}