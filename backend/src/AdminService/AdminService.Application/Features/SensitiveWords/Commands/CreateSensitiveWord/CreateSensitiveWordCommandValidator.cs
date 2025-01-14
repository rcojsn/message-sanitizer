using FluentValidation;

namespace AdminService.Application.Features.SensitiveWords.Commands.CreateSensitiveWord;

public class CreateSensitiveWordCommandValidator : AbstractValidator<CreateSensitiveWordCommand>
{
    public CreateSensitiveWordCommandValidator()
    {
        RuleFor(x => x.SensitiveWord)
            .NotNull()
            .NotEmpty();
    }
}