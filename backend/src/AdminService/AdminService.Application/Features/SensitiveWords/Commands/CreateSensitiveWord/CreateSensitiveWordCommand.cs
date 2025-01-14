using BleepGuard.Contracts.SensitiveWords;
using ErrorOr;
using MediatR;

namespace AdminService.Application.Features.SensitiveWords.Commands.CreateSensitiveWord;

public record CreateSensitiveWordCommand(string SensitiveWord) : IRequest<ErrorOr<SensitiveWordResponse>>;
