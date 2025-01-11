using MediatR;

namespace BleepGuard.Application.SensitiveWords.Commands.DeleteSensitiveWord;

public record DeleteSensitiveWordCommand(Guid Id) : IRequest;