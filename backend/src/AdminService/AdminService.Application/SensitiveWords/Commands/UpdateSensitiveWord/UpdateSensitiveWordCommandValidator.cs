using FluentValidation;

namespace AdminService.Application.SensitiveWords.Commands.UpdateSensitiveWord;

public class UpdateSensitiveWordCommandValidator : AbstractValidator<UpdateSensitiveWordCommand>
{
    public UpdateSensitiveWordCommandValidator()
    {
        RuleFor(x => x.Word)
            .NotNull()
            .NotEmpty()
            .WithMessage("Word is required");
    }
}