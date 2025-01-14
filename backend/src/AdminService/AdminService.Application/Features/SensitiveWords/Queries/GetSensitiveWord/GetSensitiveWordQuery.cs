using BleepGuard.Contracts.SensitiveWords;
using ErrorOr;
using MediatR;

namespace AdminService.Application.Features.SensitiveWords.Queries.GetSensitiveWord;

public record GetSensitiveWordQuery(Guid Id) : IRequest<ErrorOr<SensitiveWordResponse>>;