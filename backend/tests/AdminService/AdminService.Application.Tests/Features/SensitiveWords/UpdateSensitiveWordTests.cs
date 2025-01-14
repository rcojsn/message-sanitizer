using AdminService.Application.Common.Interfaces;
using AdminService.Application.Features.SensitiveWords.Commands.UpdateSensitiveWord;
using AdminService.Contracts.SensitiveWords;
using AdminService.Domain.SensitiveWords;
using ErrorOr;
using FluentAssertions;
using FluentValidation;
using NSubstitute;

namespace AdminService.Application.Tests.Features.SensitiveWords;

public class UpdateSensitiveWordTests
{
    private readonly UpdateSensitiveWordCommandHandler _sut;
    private readonly ISensitiveWordsRepository _sensitiveWordsRepository = Substitute.For<ISensitiveWordsRepository>();
    private readonly ICacheRepository _cacheRepository = Substitute.For<ICacheRepository>();
    private readonly IValidator<UpdateSensitiveWordCommand> _validator = new UpdateSensitiveWordCommandValidator();
    
    public UpdateSensitiveWordTests()
    => _sut = new UpdateSensitiveWordCommandHandler(_sensitiveWordsRepository, _cacheRepository);
    
    [Fact]
    public async Task UpdateSensitiveWord_WhenSensitiveWordDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var command = new UpdateSensitiveWordCommand(Guid.NewGuid(), "UPDATE");
        _sensitiveWordsRepository
            .GetByIdAsync(command.Id)
            .Returns((SensitiveWord)null!);
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        // Assert
        result.FirstError.Description.Should().Be("Sensitive word not found");
    }
    
    [Fact]
    public async Task UpdateSensitiveWord_WhenSensitiveWordAlreadyExists_ShouldReturnConflict()
    {
        // Arrange
        var sensitiveWord = new SensitiveWord(Guid.NewGuid(),"UPDATE");
        var (id, word) = sensitiveWord;
        var command = new UpdateSensitiveWordCommand(id, word);
        _sensitiveWordsRepository
            .GetByIdAsync(id)
            .Returns(sensitiveWord);
        _sensitiveWordsRepository
            .Exists(word)
            .Returns(true);
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        // Assert
        result.FirstError.Description.Should().Be("Sensitive word already exists");
    }
    
    [Fact]
    public async Task UpdateSensitiveWord_WhenSensitiveWordFailedToUpdate_ShouldReturnFailure()
    {
        // Arrange
        var sensitiveWord = new SensitiveWord(Guid.NewGuid(),"UPDATE");
        var (id, word) = sensitiveWord;
        var command = new UpdateSensitiveWordCommand(id, word);
        _sensitiveWordsRepository
            .GetByIdAsync(id)
            .Returns(sensitiveWord);
        _sensitiveWordsRepository
            .Exists(word)
            .Returns(false);
        _sensitiveWordsRepository
            .UpdateSensitiveWordAsync(sensitiveWord)
            .Returns(false);
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        // Assert
        result.FirstError.Description.Should().Be("Failed to update sensitive word");
    }
    
    [Fact]
    public async Task UpdateSensitiveWord_WhenCacheFailedToUpdate_ShouldReturnFailure()
    {
        // Arrange
        var sensitiveWord = new SensitiveWord(Guid.NewGuid(),"UPDATE");
        var (id, word) = sensitiveWord;
        var command = new UpdateSensitiveWordCommand(id, word);
        _sensitiveWordsRepository
            .GetByIdAsync(id)
            .Returns(sensitiveWord);
        _sensitiveWordsRepository
            .Exists(word)
            .Returns(false);
        _sensitiveWordsRepository
            .UpdateSensitiveWordAsync(sensitiveWord)
            .Returns(true);
        _cacheRepository
            .AddOrUpdateSensitiveWordAsync(sensitiveWord)
            .Returns(false);
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        // Assert
        result.FirstError.Description.Should().Be("Failed to update cache");
    }
    
    [Fact]
    public async Task UpdateSensitiveWord_WhenSensitiveWordUpdated_ShouldReturnUpdated()
    {
        // Arrange
        var sensitiveWord = new SensitiveWord(Guid.NewGuid(),"UPDATE");
        var (id, word) = sensitiveWord;
        var command = new UpdateSensitiveWordCommand(id, word);
        _sensitiveWordsRepository
            .GetByIdAsync(id)
            .Returns(sensitiveWord);
        _sensitiveWordsRepository
            .Exists(word)
            .Returns(false);
        _sensitiveWordsRepository
            .UpdateSensitiveWordAsync(sensitiveWord)
            .Returns(true);
        _cacheRepository
            .AddOrUpdateSensitiveWordAsync(sensitiveWord)
            .Returns(true);
        
        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        // Assert
        result.Value.Should().Be(Result.Updated);
    }
    
    [Fact]
    public async Task ValidateUpdateSensitiveWordCommand_ShouldReturnFailure_WhenSensitiveWordIsNull()
    {
        // Arrange
        var request = new UpdateSensitiveWordCommand(Guid.NewGuid(), null!);
        
        // Act
        var result = await _validator.ValidateAsync(request, CancellationToken.None);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.First().ErrorMessage.Should().Be("'Word' must not be empty.");
        result.Errors.Last().ErrorMessage.Should().Be("'Word' must not be empty.");
        
    }
}