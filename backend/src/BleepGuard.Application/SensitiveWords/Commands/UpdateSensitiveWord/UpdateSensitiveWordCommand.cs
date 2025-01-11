using ErrorOr;
using MediatR;

namespace BleepGuard.Application.SensitiveWords.Commands.UpdateSensitiveWord;

public record UpdateSensitiveWordCommand(Guid Id, string Word) : IRequest<ErrorOr<Updated>>;