using ErrorOr;
using MediatR;

namespace AdminService.Application.Features.SensitiveWords.Commands.UpdateSensitiveWord;

public record UpdateSensitiveWordCommand(Guid Id, string Word) : IRequest<ErrorOr<Updated>>;