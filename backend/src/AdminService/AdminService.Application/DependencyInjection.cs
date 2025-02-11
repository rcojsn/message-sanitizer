﻿using AdminService.Application.Common.Behaviors;
using AdminService.Application.Features.SensitiveWords.Commands.CreateSensitiveWord;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AdminService.Application;

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