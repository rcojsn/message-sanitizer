using CensorshipService.Application.Common.Interfaces;
using CensorshipService.Infrastructure.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace CensorshipService.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        var redisConnectionString = configuration["Redis:ConnectionString"];
        
        if (string.IsNullOrWhiteSpace(redisConnectionString)) throw new ApplicationException("No redis connection string found.");
        
        var redis = ConnectionMultiplexer.Connect(redisConnectionString);
        services.AddSingleton<IConnectionMultiplexer>(redis);
        
        services.AddSingleton<ICacheRepository, CacheRepository>();
    }
}