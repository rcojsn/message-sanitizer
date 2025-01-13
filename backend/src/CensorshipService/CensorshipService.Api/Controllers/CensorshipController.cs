using CensorshipService.Application.Features.SanitizedMessages.Commands.CreateSanitizedMessage;
using CensorshipService.Contracts.SanitizedMessages;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CensorshipService.Api.Controllers;

/// <summary>
/// Middleware api : handles the sanitize requests
/// </summary>
/// <param name="mediator">The mediator instance used for handling requests</param>
[ApiController]
[Route("[controller]")]
public class CensorshipController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Sanitizes the provided message by replacing sensitive words with a *
    /// </summary>
    /// <param name="request">The message to sanitize, containing the content to be processed</param>
    /// <returns>
    /// Returns the sanitized message as a string with a 200 OK status.
    /// Returns a 400 Bad Request status if the input request is invalid or malformed.
    /// </returns>
    [HttpPost("sanitize")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSanitizedMessage([FromBody] CreateSanitizedMessageRequest request)
    {
        var command = new CreateSanitizedMessageCommand(request.Message);
        var response = await mediator.Send(command);

        if (response == null) BadRequest("Failed to sanitize message");

        return Ok(response);

    }
}