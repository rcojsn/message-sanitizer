using CensorshipService.Application.Common.Interfaces;
using CensorshipService.Application.Features.SanitizedMessages.Commands.CreateSanitizedMessage;
using FluentAssertions;
using NSubstitute;

namespace CensorshipService.Application.Tests.Features;

public class SanitizedMessagesTests
{
    private readonly CreateSanitizedMessageCommandHandler _sut;
    private readonly ICacheRepository _cacheRepository = Substitute.For<ICacheRepository>();
    private readonly CreateSanitizedMessageCommandValidator _validator = new ();

    public SanitizedMessagesTests() => _sut = new CreateSanitizedMessageCommandHandler(_cacheRepository);

    [Fact]
    public async Task CreateSanitizedMessage_ShouldReturnSanitizedMessage_WhenInvoked()
    {
        // Arrange
        _cacheRepository.GetAllSensitiveWordsAsync().Returns(["WHEN", "SELECT * FROM"]);
        var request = new CreateSanitizedMessageCommand("When query is found, SELECT * FROM Users;");
        
        // Act
        var result = await _sut.Handle(request, CancellationToken.None);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo("**** query is found, ************* Users;");
    }
    
    [Fact]
    public async Task ValidateCreateSanitizedMessageCommand_ShouldReturnFailure_WhenSensitiveMessageIsNull()
    {
        // Arrange
        var request = new CreateSanitizedMessageCommand(null!);
        
        // Act
        var result = await _validator.ValidateAsync(request, CancellationToken.None);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.First().ErrorMessage.Should().Be("'Message' must not be empty.");
        result.Errors.Last().ErrorMessage.Should().Be("'Message' must not be empty.");
    }
}