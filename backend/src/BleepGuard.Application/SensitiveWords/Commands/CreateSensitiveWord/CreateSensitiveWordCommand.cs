using BleepGuard.Domain.SensitiveWords;
using ErrorOr;
using MediatR;

namespace BleepGuard.Application.SensitiveWords.Commands.CreateSensitiveWord;

public record CreateSensitiveWordCommand(string SensitiveWord) : IRequest<ErrorOr<SensitiveWord>>;
