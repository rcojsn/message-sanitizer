using FluentValidation;

namespace CensorshipService.Application.Features.SanitizedMessages.Commands.CreateSanitizedMessage;

public class CreateSanitizedMessageCommandValidator : AbstractValidator<CreateSanitizedMessageCommand>
{
    public CreateSanitizedMessageCommandValidator()
    {
        RuleFor(x => x.Message)
            .NotNull()
            .NotEmpty();
    }
}