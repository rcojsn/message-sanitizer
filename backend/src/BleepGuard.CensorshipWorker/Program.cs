using BleepGuard.Infrastructure.SanitizedMessages.Consumers;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<SanitizedMessageCreatedConsumer>();
        
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
});

var host = builder.Build();
host.Run();