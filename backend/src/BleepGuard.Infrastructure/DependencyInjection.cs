using BleepGuard.Application.Common.Interfaces;
using BleepGuard.Infrastructure.Common;
using BleepGuard.Infrastructure.SanitizedMessages.Persistence;
using BleepGuard.Infrastructure.SensitiveWords.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace BleepGuard.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ => new MsSqlDbConnectionFactory(connectionString));

        services.AddScoped<ISensitiveWordsRepository, SensitiveWordsRepository>();
        services.AddScoped<ISanitizedMessagesRepository, SanitizedMessagesRepository>();
    }
}