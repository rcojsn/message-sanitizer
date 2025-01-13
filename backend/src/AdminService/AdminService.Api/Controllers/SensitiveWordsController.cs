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
    /// <returns>The created sensitive word in the response, with a status code indicating success or failure.</returns>    [HttpPost]
    [ProducesResponseType(typeof(SensitiveWordResponse), StatusCodes.Status201Created)]
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
            _ => Problem()
        );
    }

    /// <summary>
    /// Retrieves a sensitive word by its id
    /// </summary>
    /// <param name="id">The unique identifier of the sensitive word to retrieve</param>
    /// <returns>
    /// Returns the sensitive word if found (StatusCode 200 OK),
    /// or a 404 Not Found status if the word is not found,
    /// or a 500 Internal Server Error if an unexpected error occurs
    /// </returns>
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
    /// <returns>
    /// Returns a 204 No Content status if the update is successful
    /// Returns a 400 Bad Request status if the input is invalid
    /// Returns a 404 Not Found status if the sensitive word with the specified ID does not exist
    /// </returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSensitiveWord([FromRoute] Guid id, [FromBody] UpdateSensitiveWordRequest request)
    {
        var command = new UpdateSensitiveWordCommand(id, request.Word);
        
        var response = await mediator.Send(command);

        return response.Match<IActionResult>(
            _ => NoContent(),
            _ => Problem()
        );
    }

    /// <summary>
    /// Deletes an existing sensitive word by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the sensitive word to delete</param>
    /// <returns>
    /// Returns a 204 No Content status if the deletion is successful
    /// Returns a 404 Not Found status if the sensitive word with the specified ID does not exist
    /// </returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteSensitiveWord([FromRoute] Guid id)
    {
        var command = new DeleteSensitiveWordCommand(id);
        
        var deleteSensitiveWordResult = await mediator.Send(command);

        return deleteSensitiveWordResult.Match<IActionResult>(
            _ => NoContent(),
            _ => Problem()
        );
    }
}