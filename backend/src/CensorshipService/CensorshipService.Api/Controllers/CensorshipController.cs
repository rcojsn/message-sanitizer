using CensorshipService.Contracts.SanitizedMessages;
using Microsoft.AspNetCore.Mvc;

namespace CensorshipService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CensorshipController : ControllerBase
{
    [HttpPost("sanitize")]
    public async Task<IActionResult> Sanitize([FromBody] CreateSanitizedMessageRequest request)
    {
        return Ok("sanitized");
    }
}