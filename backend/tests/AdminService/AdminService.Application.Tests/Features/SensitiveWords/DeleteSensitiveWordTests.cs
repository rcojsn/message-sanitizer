using AdminService.Application.Common.Interfaces;
using AdminService.Application.Features.SensitiveWords.Commands.DeleteSensitiveWord;
using AdminService.Domain.SensitiveWords;
using ErrorOr;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace AdminService.Application.Tests.Features.SensitiveWords;

public class DeleteSensitiveWordTests
{
    private readonly DeleteSensitiveWordCommandHandler _sut;
    private readonly ISensitiveWordsRepository _sensitiveWordsRepository = Substitute.For<ISensitiveWordsRepository>();
    private readonly ICacheRepository _cacheRepository = Substitute.For<ICacheRepository>();
    private readonly ILogger<DeleteSensitiveWordCommandHandler> _logger = Substitute.For<ILogger<DeleteSensitiveWordCommandHandler>>();
    
    public DeleteSensitiveWordTests()
    => _sut = new DeleteSensitiveWordCommandHandler(
        _sensitiveWordsRepository,
        _logger,
        _cacheRepository);
    
    [Fact]
    public async Task DeleteSensitiveWord_WhenSensitiveWordNotFound_ReturnsErrorNotFound()
    {
        // Arrange
        var request = new DeleteSensitiveWordCommand(Guid.NewGuid());
        _sensitiveWordsRepository
            .GetByIdAsync(Arg.Any<Guid>())
            .Returns((SensitiveWord)null!);
        
        // Act
        var result = await _sut.Handle(request, CancellationToken.None);
        
        // Assert
        result.FirstError.Description.Should().Be("Sensitive word not found");
    }
    
    [Fact]
    public async Task DeleteSensitiveWord_WhenSensitiveWordFoundButNotDeleted_ReturnsErrorFailure()
    {
        // Arrange
        var request = new DeleteSensitiveWordCommand(Guid.NewGuid());
        var sensitiveWord = new SensitiveWord(Guid.NewGuid(), "DELETE");
        _sensitiveWordsRepository
            .GetByIdAsync(Arg.Any<Guid>())
            .Returns(sensitiveWord);
        _sensitiveWordsRepository
            .DeleteSensitiveWordAsync(Arg.Any<SensitiveWord>())
            .Returns(false);
        
        // Act
        var result = await _sut.Handle(request, CancellationToken.None);
        
        // Assert
        result.FirstError.Description.Should().Be("Failed to delete sensitive word");
    }
    
    [Fact]
    public async Task DeleteSensitiveWord_WhenSensitiveWordFoundAndDeletedButNotDeletedFromCache_ReturnsErrorFailure()
    {
        // Arrange
        var request = new DeleteSensitiveWordCommand(Guid.NewGuid());
        var sensitiveWord = new SensitiveWord(Guid.NewGuid(), "DELETE");
        _sensitiveWordsRepository
            .GetByIdAsync(Arg.Any<Guid>())
            .Returns(sensitiveWord);
        _sensitiveWordsRepository
            .DeleteSensitiveWordAsync(Arg.Any<SensitiveWord>())
            .Returns(true);
        _cacheRepository
            .DeleteSensitiveWordByIdAsync(Arg.Any<Guid>())
            .Returns(false);
        
        // Act
        var result = await _sut.Handle(request, CancellationToken.None);
        
        // Assert
        result.FirstError.Description.Should().Be("Failed to delete sensitive word from cache");
    }
    
    [Fact]
    public async Task DeleteSensitiveWord_WhenSensitiveWordFoundAndDeletedCompletely_ReturnsDeleted()
    {
        // Arrange
        var request = new DeleteSensitiveWordCommand(Guid.NewGuid());
        var sensitiveWord = new SensitiveWord(Guid.NewGuid(), "DELETE");
        _sensitiveWordsRepository
            .GetByIdAsync(Arg.Any<Guid>())
            .Returns(sensitiveWord);
        _sensitiveWordsRepository
            .DeleteSensitiveWordAsync(Arg.Any<SensitiveWord>())
            .Returns(true);
        _cacheRepository
            .DeleteSensitiveWordByIdAsync(Arg.Any<Guid>())
            .Returns(true);
        
        // Act
        var result = await _sut.Handle(request, CancellationToken.None);
        
        // Assert
        result.Value.Should().Be(Result.Deleted);
    }
}