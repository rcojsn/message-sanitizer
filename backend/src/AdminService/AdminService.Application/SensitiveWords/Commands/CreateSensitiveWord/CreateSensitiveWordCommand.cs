using AdminService.Domain.SensitiveWords;
using ErrorOr;
using MediatR;

namespace AdminService.Application.SensitiveWords.Commands.CreateSensitiveWord;

public record CreateSensitiveWordCommand(string SensitiveWord) : IRequest<ErrorOr<SensitiveWord>>;
