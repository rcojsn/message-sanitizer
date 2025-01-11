using BleepGuard.Contracts.SanitizedMessages.Events;
using MassTransit;

namespace BleepGuard.Infrastructure.SanitizedMessages.Consumers;

public class SanitizedMessageCreatedConsumer : IConsumer<SanitizedMessageCreated>
{
    public Task Consume(ConsumeContext<SanitizedMessageCreated> context)
    {
        Console.WriteLine($"Sanitized message created: {context.Message}");
        return Task.CompletedTask;
    }
}