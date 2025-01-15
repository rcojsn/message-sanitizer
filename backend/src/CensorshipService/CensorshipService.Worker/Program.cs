using System.Text.Json;
using CensorshipService.Application.Common.Interfaces;
using CensorshipService.Application.Common.Interfaces.External;
using CensorshipService.Infrastructure.Redis;
using CensorshipService.Worker;
using Refit;
using StackExchange.Redis;

var builder = Host.CreateApplicationBuilder(args);

using var loggerFactory = LoggerFactory.Create(x =>
{
    x.AddJsonConsole(options =>
    {
        options.IncludeScopes = false;
        options.TimestampFormat = "HH:mm:ss ";
        options.JsonWriterOptions = new JsonWriterOptions
        {
            Indented = true
        };
    });
    x.SetMinimumLevel(LogLevel.Debug);
});

loggerFactory.CreateLogger<Program>();

#region Redis

var redisConnectionString = builder.Configuration["Redis:ConnectionString"]!;
        
if (string.IsNullOrWhiteSpace(redisConnectionString)) throw new ApplicationException("No redis connection string found.");
        
var redis = ConnectionMultiplexer.Connect(redisConnectionString);
builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
        
builder.Services.AddSingleton<ICacheRepository, CacheRepository>();

#endregion

#region Configure AdminService Api Client
var adminServiceHost = builder.Configuration["AdminService:Host"];

if (adminServiceHost is null) throw new ApplicationException("Missing Admin Service Host");

builder
    .Services
    .AddRefitClient<IAdminServiceApiClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(adminServiceHost));

#endregion

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();