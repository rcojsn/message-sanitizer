using BleepGuard.Domain.SensitiveWords;
using MediatR;

namespace BleepGuard.Application.SensitiveWords.Queries.GetSensitiveWords;

public record GetSensitiveWordsQuery : IRequest<IList<SensitiveWord>>;