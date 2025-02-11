﻿using AdminService.Application.Common.Interfaces;
using AdminService.Infrastructure.Redis;
using AdminService.Infrastructure.SensitiveWords.Persistence;
using BleepGuard.Infrastructure.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace AdminService.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        if (string.IsNullOrWhiteSpace(connectionString)) throw new ApplicationException("No database connection string found.");
        
        services.AddSingleton<IDbConnectionFactory>(_ => new MsSqlDbConnectionFactory(connectionString));

        services.AddScoped<ISensitiveWordsRepository, SensitiveWordsRepository>();
                
        #region Redis

        var redisConnectionString = configuration["Redis:ConnectionString"]!;
        
        if (string.IsNullOrWhiteSpace(redisConnectionString)) throw new ApplicationException("No redis connection string found.");
        
        var redis = ConnectionMultiplexer.Connect(redisConnectionString);
        services.AddSingleton<IConnectionMultiplexer>(redis);
        
        services.AddSingleton<ICacheRepository, CacheRepository>();
        #endregion
    }
}