﻿using BleepGuard.Application.Common.Behaviors;
using BleepGuard.Application.SensitiveWords.Commands.CreateSensitiveWord;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BleepGuard.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(options =>
            options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection))
        );
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        services.AddValidatorsFromAssemblyContaining<CreateSensitiveWordCommandValidator>();
        
        return services;
    }
}