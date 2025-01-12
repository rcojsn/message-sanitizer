using ErrorOr;
using MediatR;

namespace AdminService.Application.SensitiveWords.Commands.DeleteSensitiveWord;

public record DeleteSensitiveWordCommand(Guid Id) : IRequest<ErrorOr<Deleted>>;