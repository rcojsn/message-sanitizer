using AdminService.Api.Controllers;
using AdminService.Application.Features.SensitiveWords.Commands.CreateSensitiveWord;
using AdminService.Application.Features.SensitiveWords.Commands.DeleteSensitiveWord;
using AdminService.Application.Features.SensitiveWords.Commands.UpdateSensitiveWord;
using AdminService.Application.Features.SensitiveWords.Queries.GetSensitiveWord;
using AdminService.Application.Features.SensitiveWords.Queries.GetSensitiveWords;
using AdminService.Contracts.SensitiveWords;
using BleepGuard.Contracts.SensitiveWords;
using ErrorOr;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace AdminService.Api.Tests;

public class SensitiveWordsControllerTests
{
    private readonly SensitiveWordsController _sut;
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly SensitiveWordResponse _expectedSensitiveWordResponse = new(Guid.NewGuid(), "DELETE");

    public SensitiveWordsControllerTests() => _sut = new SensitiveWordsController(_mediator);

    [Fact]
    public async Task CreateSensitiveWord_ShouldReturnCreatedResult_WhenSensitiveWordCreated()
    {
        // Arrange
        var request = new CreateSensitiveWordRequest(_expectedSensitiveWordResponse.Word);
        _mediator.Send(Arg.Any<CreateSensitiveWordCommand>())
            .Returns(_expectedSensitiveWordResponse);
        
        // Act
        var response = await _sut.CreateSensitiveWord(request);
        
        // Assert
        response.Should().BeOfType<CreatedAtActionResult>();
        response.As<CreatedAtActionResult>().StatusCode.Should().Be(201);
        response.As<CreatedAtActionResult>().Value.Should().BeEquivalentTo(_expectedSensitiveWordResponse);
    }

    #region Get Sensitive Word By Id
    [Fact]
    public async Task GetSensitiveWordById_ShouldReturnOkResult_WhenSensitiveWordFound()
    {
        // Arrange
        _mediator.Send(Arg.Any<GetSensitiveWordQuery>())
            .Returns(_expectedSensitiveWordResponse);
        
        // Act
        var response = await _sut.GetSensitiveWordById(_expectedSensitiveWordResponse.Id);
        
        // Assert
        response.Should().BeOfType<OkObjectResult>();
        response.As<OkObjectResult>().StatusCode.Should().Be(StatusCodes.Status200OK);
        response.As<OkObjectResult>().Value.Should().BeEquivalentTo(_expectedSensitiveWordResponse);
    }

    [Fact]
    public async Task GetSensitiveWordById_ShouldReturnNotFoundResult_WhenSensitiveWordNotFound()
    {
        // Arrange
        _mediator.Send(Arg.Any<GetSensitiveWordQuery>())
            .Returns(Error.NotFound(description: "Sensitive word not found"));
        
        // Act
        var response = await _sut.GetSensitiveWordById(_expectedSensitiveWordResponse.Id);
        
        // Assert
        response.Should().BeOfType<NotFoundObjectResult>();
        response.As<NotFoundObjectResult>().StatusCode.Should().Be(StatusCodes.Status404NotFound);
        response.As<NotFoundObjectResult>().Value.Should().Be("Sensitive word not found");
    }
    #endregion
    
    #region Get Sensitive Words
    [Fact]
    public async Task GetSensitiveWords_ShouldReturnOkResult_WhenSensitiveWordsFound()
    {
        // Arrange
        List<SensitiveWordResponse> sensitiveWords = [_expectedSensitiveWordResponse];
        _mediator.Send(Arg.Any<GetSensitiveWordsQuery>())
            .Returns(sensitiveWords);
        
        // Act
        var response = await _sut.GetSensitiveWords();
        
        // Assert
        response.Should().BeOfType<OkObjectResult>();
        response.As<OkObjectResult>().StatusCode.Should().Be(StatusCodes.Status200OK);
        response.As<OkObjectResult>().Value.Should().BeEquivalentTo(sensitiveWords);
    }
    #endregion
    
    #region Update Sensitive Word
    [Fact]
    public async Task UpdateSensitiveWord_ShouldReturnNoContent_WhenSensitiveWordUpdated()
    {
        // Arrange
        _mediator.Send(Arg.Any<UpdateSensitiveWordCommand>())
            .Returns(Result.Updated);
        var request = new UpdateSensitiveWordRequest("PUT");
        
        // Act
        var response = await _sut.UpdateSensitiveWord(_expectedSensitiveWordResponse.Id, request);
        
        // Assert
        response.Should().BeOfType<NoContentResult>();
        response.As<NoContentResult>().StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }
    
    [Fact]
    public async Task UpdateSensitiveWord_ShouldReturnNotFound_WhenSensitiveWordNotFound()
    {
        // Arrange
        _mediator.Send(Arg.Any<UpdateSensitiveWordCommand>())
            .Returns(Error.NotFound(description: "Sensitive word not found"));
        var request = new UpdateSensitiveWordRequest("PUT");
        
        // Act
        var response = await _sut.UpdateSensitiveWord(_expectedSensitiveWordResponse.Id, request);
        
        // Assert
        response.Should().BeOfType<NotFoundObjectResult>();
        response.As<NotFoundObjectResult>().StatusCode.Should().Be(StatusCodes.Status404NotFound);
        response.As<NotFoundObjectResult>().Value.Should().Be("Sensitive word not found");
    }
    
    [Fact]
    public async Task UpdateSensitiveWord_ShouldReturnConflict_WhenSensitiveWordAlreadyExists()
    {
        // Arrange
        _mediator.Send(Arg.Any<UpdateSensitiveWordCommand>())
            .Returns(Error.Conflict(description: "Sensitive word already exists"));
        var request = new UpdateSensitiveWordRequest("PUT");
        
        // Act
        var response = await _sut.UpdateSensitiveWord(_expectedSensitiveWordResponse.Id, request);
        
        // Assert
        response.Should().BeOfType<ConflictObjectResult>();
        response.As<ConflictObjectResult>().StatusCode.Should().Be(StatusCodes.Status409Conflict);
        response.As<ConflictObjectResult>().Value.Should().Be("Sensitive word already exists");
    }
    #endregion

    #region Delete Sensitive Word
    [Fact]
    public async Task DeleteSensitiveWord_ShouldReturnNoContent_WhenSensitiveWordDeleted()
    {
        // Arrange
        _mediator.Send(Arg.Any<DeleteSensitiveWordCommand>())
            .Returns(Result.Deleted);
        
        // Act
        var response = await _sut.DeleteSensitiveWord(_expectedSensitiveWordResponse.Id);
        
        // Assert
        response.Should().BeOfType<NoContentResult>();
        response.As<NoContentResult>().StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }
    
    [Fact]
    public async Task DeleteSensitiveWord_ShouldReturnNotFound_WhenSensitiveWordNotFound()
    {
        // Arrange
        _mediator.Send(Arg.Any<DeleteSensitiveWordCommand>())
            .Returns(Error.NotFound(description: "Sensitive word not found"));
        
        // Act
        var response = await _sut.DeleteSensitiveWord(_expectedSensitiveWordResponse.Id);
        
        // Assert
        response.Should().BeOfType<NotFoundObjectResult>();
        response.As<NotFoundObjectResult>().StatusCode.Should().Be(StatusCodes.Status404NotFound);
        response.As<NotFoundObjectResult>().Value.Should().Be("Sensitive word not found");
    }
    #endregion
    
}