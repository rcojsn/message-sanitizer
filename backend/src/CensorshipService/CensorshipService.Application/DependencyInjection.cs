using Microsoft.Extensions.DependencyInjection;

namespace CensorshipService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(options =>
            options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection))
        );
        
        return services;
    }
}