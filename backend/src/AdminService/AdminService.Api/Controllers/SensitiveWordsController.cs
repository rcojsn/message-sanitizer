using AdminService.Application.SensitiveWords.Commands.CreateSensitiveWord;
using AdminService.Application.SensitiveWords.Commands.DeleteSensitiveWord;
using AdminService.Application.SensitiveWords.Commands.UpdateSensitiveWord;
using AdminService.Application.SensitiveWords.Queries.GetSensitiveWord;
using AdminService.Application.SensitiveWords.Queries.GetSensitiveWords;
using AdminService.Contracts.SensitiveWords;
using AdminService.Domain.SensitiveWords;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace AdminService.Api.Controllers;

/// <summary>
/// Handles requests related to sensitive words, such as retrieving, adding, updating, or deleting sensitive words.
/// </summary>
/// <param name="mediator"></param>
[ApiController]
[Route("[controller]")]
public class SensitiveWordsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Creates a new sensitive word
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the created sensitive word (HTTP 201 Created),
    /// or validation errors (HTTP 400 Bad Request) if the provided word is invalid
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(SensitiveWord), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSensitiveWord([FromBody] CreateSensitiveWordRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateSensitiveWordCommand(request.Word);
        var createSensitiveWordResult = await mediator.Send(command, cancellationToken);

        return createSensitiveWordResult.MatchFirst(
            sensitiveWord => CreatedAtAction(
                nameof(GetSensitiveWordById),
                new { id = sensitiveWord.Id },
                sensitiveWord),
            _ => Problem()
        );
    }

    /// <summary>
    /// Retrieves a sensitive word by its unique identifier
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the sensitive word if found (HTTP 200 OK),
    /// or a not found response (HTTP 404 Not Found) if the sensitive word does not exist
    /// </returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(SensitiveWord), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSensitiveWordById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetSensitiveWordQuery(id);
        
        var getSensitiveWordResult = await mediator.Send(query, cancellationToken);

        return getSensitiveWordResult.MatchFirst(
            sensitiveWord => Ok(new SensitiveWordResponse(id, sensitiveWord.Word)),
            error => error.Type == ErrorType.NotFound ? NotFound(error.Description) : Problem());
    }
    
    /// <summary>
    /// Retrieves all sensitive words
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing a list of all sensitive words (HTTP 200 OK).
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(IList<SensitiveWord>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSensitiveWords(CancellationToken cancellationToken)
    {
        var query = new GetSensitiveWordsQuery();
        
        var sensitiveWords = await mediator.Send(query, cancellationToken);
        
        return Ok(sensitiveWords);
    }

    /// <summary>
    /// Update an existing sensitive word
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSensitiveWord([FromRoute] Guid id, [FromBody] UpdateSensitiveWordRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateSensitiveWordCommand(id, request.Word);
        
        var updateSensitiveWordResult = await mediator.Send(command, cancellationToken);

        return updateSensitiveWordResult.Match<IActionResult>(
            _ => NoContent(),
            _ => Problem()
        );
    }

    /// <summary>
    /// Delete an existing sensitive word by its unique identifier
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteSensitiveWord([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteSensitiveWordCommand(id);
        
        var deleteSensitiveWordResult = await mediator.Send(command, cancellationToken);

        return deleteSensitiveWordResult.Match<IActionResult>(
            _ => NoContent(),
            _ => Problem()
        );
    }
}