using FluentValidation;

namespace AdminService.Application.Features.SensitiveWords.Commands.UpdateSensitiveWord;

public class UpdateSensitiveWordCommandValidator : AbstractValidator<UpdateSensitiveWordCommand>
{
    public UpdateSensitiveWordCommandValidator()
    {
        RuleFor(x => x.Word)
            .NotNull()
            .NotEmpty();
    }
}