using BleepGuard.Domain.SensitiveWords;
using ErrorOr;
using MediatR;

namespace BleepGuard.Application.SensitiveWords.Queries.GetSensitiveWord;

public record GetSensitiveWordQuery(Guid Id) : IRequest<ErrorOr<SensitiveWord>>;