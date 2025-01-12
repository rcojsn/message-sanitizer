using AdminService.Domain.SensitiveWords;
using ErrorOr;
using MediatR;

namespace AdminService.Application.SensitiveWords.Queries.GetSensitiveWord;

public record GetSensitiveWordQuery(Guid Id) : IRequest<ErrorOr<SensitiveWord>>;