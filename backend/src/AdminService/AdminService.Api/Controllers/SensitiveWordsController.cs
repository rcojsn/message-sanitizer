using System.Net.Mime;
using AdminService.Application.Features.SensitiveWords.Commands.CreateSensitiveWord;
using AdminService.Application.Features.SensitiveWords.Commands.DeleteSensitiveWord;
using AdminService.Application.Features.SensitiveWords.Commands.UpdateSensitiveWord;
using AdminService.Application.Features.SensitiveWords.Queries.GetSensitiveWord;
using AdminService.Application.Features.SensitiveWords.Queries.GetSensitiveWords;
using AdminService.Contracts.SensitiveWords;
using BleepGuard.Contracts.SensitiveWords;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace AdminService.Api.Controllers;

/// <summary>
/// Handles managing the sensitive words 
/// </summary>
/// <param name="mediator">The mediator instance used for handling requests</param>
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[ApiController]
[Route("[controller]")]
public class SensitiveWordsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Creates a new sensitive word
    /// </summary>
    /// <param name="request">The create sensitive word request object that expects a word</param>
    /// <returns>The created sensitive word</returns>
    /// <response code="201">Returns the created sensitive word</response>
    /// <response code="409">If the sensitive word already exists</response>
    /// <response code="400">If the input is invalid</response>
    [HttpPost]
    [ProducesResponseType(typeof(SensitiveWordResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSensitiveWord([FromBody] CreateSensitiveWordRequest request)
    {
        var command = new CreateSensitiveWordCommand(request.Word);
        var response = await mediator.Send(command);

        return response.MatchFirst(
            sensitiveWord => CreatedAtAction(
                nameof(GetSensitiveWordById),
                new { id = sensitiveWord.Id },
                sensitiveWord),
            error =>
            {
                return error.Type switch
                {
                    ErrorType.Conflict => Conflict(error.Description),
                    _ => Problem(error.Description)
                };
            }
        );
    }

    /// <summary>
    /// Retrieves a sensitive word by its id
    /// </summary>
    /// <param name="id">The unique identifier of the sensitive word to retrieve</param>
    /// <returns>Returns a sensitive word</returns>
    /// <response code="200">Returns the sensitive word</response>
    /// <response code="404">If the sensitive word with the specified ID does not exist</response>
    /// <response code="500">If an error occurs</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(SensitiveWordResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSensitiveWordById([FromRoute] Guid id)
    {
        var query = new GetSensitiveWordQuery(id);
        
        var response = await mediator.Send(query);

        return response.MatchFirst(
            sensitiveWord => Ok(sensitiveWord with { Id = id }),
            error => error.Type == ErrorType.NotFound ? NotFound(error.Description) : Problem());
    }
    
    /// <summary>
    /// Retrieves all sensitive words.
    /// </summary>
    /// <returns>
    /// Returns a list of all sensitive words (StatusCode 200 OK)
    /// If no sensitive words are found, an empty list is returned
    /// </returns>
    /// <response code="200">Returns a list of all sensitive words</response>
    [HttpGet]
    [ProducesResponseType(typeof(IList<SensitiveWordResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSensitiveWords()
    {
        var query = new GetSensitiveWordsQuery();
        var sensitiveWords = await mediator.Send(query);
        return Ok(sensitiveWords);
    }

    /// <summary>
    /// Updates an existing sensitive word
    /// </summary>
    /// <param name="id">The unique identifier of the sensitive word to update</param>
    /// <param name="request">The updated sensitive word details</param>
    /// <returns></returns>
    /// <response code="204">If the update is successful</response>
    /// <response code="400">If the input is invalid</response>
    /// <response code="404">If the sensitive word with the specified ID does not exist</response>
    /// <response code="409">If the sensitive word already exists</response> 
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateSensitiveWord([FromRoute] Guid id, [FromBody] UpdateSensitiveWordRequest request)
    {
        var command = new UpdateSensitiveWordCommand(id, request.Word);
        
        var response = await mediator.Send(command);

        return response.MatchFirst<IActionResult>(
            _ => NoContent(),
            error =>
            {
                return error.Type switch
                {
                    ErrorType.NotFound => NotFound(error.Description),
                    ErrorType.Conflict => Conflict(error.Description),
                    _ => BadRequest(error.Description)
                };
            }
        );
    }

    /// <summary>
    /// Deletes an existing sensitive word by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the sensitive word to delete</param>
    /// <returns></returns>
    /// <response code="204">If the deletion is successful</response>
    /// <response code="404">If the sensitive word with the specified ID does not exist</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteSensitiveWord([FromRoute] Guid id)
    {
        var command = new DeleteSensitiveWordCommand(id);
        
        var deleteSensitiveWordResult = await mediator.Send(command);

        return deleteSensitiveWordResult.MatchFirst<IActionResult>(
            _ => NoContent(),
            error =>
            {
                return error.Type switch
                {
                    ErrorType.NotFound => NotFound(error.Description),
                    _ => BadRequest(error.Description)
                };
            }
        );
    }
}