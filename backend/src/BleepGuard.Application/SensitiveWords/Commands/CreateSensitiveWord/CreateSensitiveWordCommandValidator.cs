﻿using FluentValidation;

namespace BleepGuard.Application.SensitiveWords.Commands.CreateSensitiveWord;

public class CreateSensitiveWordCommandValidator : AbstractValidator<CreateSensitiveWordCommand>
{
    public CreateSensitiveWordCommandValidator()
    {
        RuleFor(x => x.SensitiveWord)
            .NotNull()
            .NotEmpty()
            .WithMessage("Word is required");
    }
}