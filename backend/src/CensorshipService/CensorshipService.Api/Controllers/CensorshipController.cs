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
    /// <returns>The sanitized message</returns>
    /// <response code="200">Returns the sanitized message</response>
    /// <response code="400">If the input is invalid</response>
    [HttpPost("sanitize")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSanitizedMessageAsync([FromBody] CreateSanitizedMessageRequest request)
    {
        var command = new CreateSanitizedMessageCommand(request.Message);
        var response = await mediator.Send(command);

        if (response == null) return BadRequest("Failed to sanitize message");

        return Ok(response);
    }
}