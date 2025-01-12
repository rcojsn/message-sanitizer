using FluentValidation;

namespace CensorshipService.Application.SanitizedMessages.Commands.CreateSanitizedMessage;

public class CreateSanitizedMessageCommandValidator : AbstractValidator<CreateSanitizedMessageCommand>
{
    public CreateSanitizedMessageCommandValidator()
    {
        RuleFor(x => x.Message)
            .NotNull()
            .NotEmpty()
            .WithMessage("Message cannot be empty.");
    }
}