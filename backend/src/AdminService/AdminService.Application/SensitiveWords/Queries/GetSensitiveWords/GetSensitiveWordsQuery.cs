using AdminService.Domain.SensitiveWords;
using MediatR;

namespace AdminService.Application.SensitiveWords.Queries.GetSensitiveWords;

public record GetSensitiveWordsQuery : IRequest<IList<SensitiveWord>>;