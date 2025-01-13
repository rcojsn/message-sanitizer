using ErrorOr;
using MediatR;

namespace CensorshipService.Application.Features.SanitizedMessages.Commands.CreateSanitizedMessage;

public record CreateSanitizedMessageCommand(string Message) : IRequest<string?>;