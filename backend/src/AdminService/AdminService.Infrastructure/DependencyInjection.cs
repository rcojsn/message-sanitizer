using AdminService.Application.Common.Interfaces;
using AdminService.Infrastructure.SensitiveWords.Persistence;
using BleepGuard.Infrastructure.Common;
using Microsoft.Extensions.DependencyInjection;

namespace AdminService.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ => new MsSqlDbConnectionFactory(connectionString));

        services.AddScoped<ISensitiveWordsRepository, SensitiveWordsRepository>();
    }
}