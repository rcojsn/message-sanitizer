using CensorshipService.Application.Common.Interfaces.External;
using CensorshipService.Infrastructure;
using CensorshipService.Worker;
using Refit;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.RegisterRedis(builder.Configuration);

var adminServiceHost = builder.Configuration["AdminService:Host"];

if (adminServiceHost is null) throw new ApplicationException("Missing Admin Service Host");

builder
    .Services
    .AddRefitClient<IAdminServiceApiClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(adminServiceHost));

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();