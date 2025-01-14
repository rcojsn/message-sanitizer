using CensorshipService.Api.Controllers;
using CensorshipService.Application.Features.SanitizedMessages.Commands.CreateSanitizedMessage;
using CensorshipService.Contracts.SanitizedMessages;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace CensorshipService.Api.Tests;

public class CensorshipControllerTests
{
    private readonly CensorshipController _sut;
    private readonly IMediator _mediator = Substitute.For<IMediator>();

    public CensorshipControllerTests() => _sut = new CensorshipController(_mediator);

    [Fact]
    public async Task CreateSanitizedMessageAsync_ShouldReturnBadRequest_WhenMessageFailedToBeSanitized()
    {
        // Arrange
        var request = new CreateSanitizedMessageRequest(string.Empty);
        _mediator.Send(Arg.Any<CreateSanitizedMessageCommand>())
            .Returns(Task.FromResult<string?>(null));
        
        // Act
        var response = await _sut.CreateSanitizedMessageAsync(request);
        
        // Assert
        response.Should().BeOfType<BadRequestObjectResult>();
        response.As<BadRequestObjectResult>().StatusCode.Should().Be(400);
        response.As<BadRequestObjectResult>().Value.Should().BeEquivalentTo("Failed to sanitize message");
    }

    [Fact]
    public async Task CreateSanitizedMessageAsync_ShouldReturnOk_WhenValid()
    {
        // Arrange
        const string expectedSanitizedMessage = "**** query is found, ************* Users;";
        var request = new CreateSanitizedMessageRequest("When query is found, SELECT * FROM Users;");
        _mediator.Send(Arg.Any<CreateSanitizedMessageCommand>())
            .Returns(Task.FromResult<string?>(expectedSanitizedMessage));
        
        // Act
        var response = await _sut.CreateSanitizedMessageAsync(request);
        
        // Assert
        response.Should().BeOfType<OkObjectResult>();
        response.As<OkObjectResult>().StatusCode.Should().Be(200);
        response.As<OkObjectResult>().Value.Should().BeEquivalentTo(expectedSanitizedMessage);
    }
}