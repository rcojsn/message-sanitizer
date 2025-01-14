using BleepGuard.Contracts.SensitiveWords;
using MediatR;

namespace AdminService.Application.Features.SensitiveWords.Queries.GetSensitiveWords;

public record GetSensitiveWordsQuery : IRequest<IList<SensitiveWordResponse>>;