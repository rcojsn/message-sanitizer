using CensorshipService.Application.Common.Behaviors;
using CensorshipService.Application.Common.Interfaces.External;
using CensorshipService.Application.SanitizedMessages.Commands.CreateSanitizedMessage;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace CensorshipService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(options =>
            options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection))
        );
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        services.AddValidatorsFromAssemblyContaining<CreateSanitizedMessageCommandValidator>();

        services.AddRefitClient<IAdminServiceApiClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://localhost:5010"));
        
        return services;
    }
}