using BleepGuard.Domain.SanitizedMessages;
using ErrorOr;
using MediatR;

namespace BleepGuard.Application.SanitizedMessages.Commands.CreateSanitizedMessage;

public record CreateSanitizedMessageCommand(string Message) : IRequest<ErrorOr<SanitizedMessage>>;