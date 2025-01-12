using BleepGuard.Infrastructure.Common;
using CensorshipService.Application.Common.Interfaces;
using CensorshipService.Infrastructure.SanitizedMessages.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace CensorshipService.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ => new MsSqlDbConnectionFactory(connectionString));

        services.AddScoped<ISanitizedMessagesRepository, SanitizedMessagesRepository>();
    }
}