using CensorshipService.Domain.SanitizedMessages;
using ErrorOr;
using MediatR;

namespace CensorshipService.Application.SanitizedMessages.Commands.CreateSanitizedMessage;

public record CreateSanitizedMessageCommand(string Message) : IRequest<ErrorOr<SanitizedMessage>>;