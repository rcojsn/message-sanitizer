using CensorshipService.Application.SanitizedMessages.Commands.CreateSanitizedMessage;
using CensorshipService.Contracts.SanitizedMessages;
using CensorshipService.Domain.SanitizedMessages;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CensorshipService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CensorshipController(IMediator mediator) : ControllerBase
{
    [HttpPost("sanitize")]
    [ProducesResponseType(typeof(SanitizedMessage), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSanitizedMessage([FromBody] CreateSanitizedMessageRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateSanitizedMessageCommand(request.Message);
        var createSanitizedMessageResult = await mediator.Send(command, cancellationToken);

        return createSanitizedMessageResult.MatchFirst(
            sanitizedMessage => Created("", new SanitizedMessageResponse(sanitizedMessage.Id, request.Message)),
            _ => Problem()
        );
    }
}