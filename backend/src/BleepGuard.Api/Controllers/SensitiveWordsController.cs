using Microsoft.AspNetCore.Mvc;

namespace BleepGuard.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SensitiveWordsController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetSensitiveWords()
    {
        return Ok("Sensitive words are sensitive");
    }
}