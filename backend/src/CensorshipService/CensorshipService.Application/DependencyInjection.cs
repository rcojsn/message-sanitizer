﻿using CensorshipService.Application.Common.Behaviors;
using CensorshipService.Application.Features.SanitizedMessages.Commands.CreateSanitizedMessage;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

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
        
        return services;
    }
}