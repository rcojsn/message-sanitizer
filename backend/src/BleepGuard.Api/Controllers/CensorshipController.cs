using BleepGuard.Contracts.SanitizedMessages;
using BleepGuard.Contracts.SanitizedMessages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace BleepGuard.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CensorshipController(IPublishEndpoint publishEndpoint) : ControllerBase
{
    [HttpPost("sanitize")]
    public async Task<IActionResult> Sanitize([FromBody] CreateSanitizedMessageRequest request)
    {
        var notifySanitizedMessageCreated = publishEndpoint.Publish(new CreateSanitizedMessage(request.Message));
        
        return Ok("sanitized");
    }
}