using CensorshipService.Application.Common.Interfaces;
using CensorshipService.Application.Features.SanitizedMessages.Commands.CreateSanitizedMessage;
using FluentAssertions;
using NSubstitute;

namespace CensorshipService.Application.Tests.Features;

public class SanitizedMessagesTests
{
    private readonly CreateSanitizedMessageCommandHandler _sut;
    private readonly ICacheRepository _cacheRepository = Substitute.For<ICacheRepository>();

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
}