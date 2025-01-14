using AdminService.Application.Common.Interfaces;
using AdminService.Application.Features.SensitiveWords.Commands.CreateSensitiveWord;
using AdminService.Domain.SensitiveWords;
using FluentAssertions;
using FluentValidation;
using NSubstitute;

namespace AdminService.Application.Tests.Features.SensitiveWords;

public class CreateSensitiveWordTests
{
    private readonly CreateSensitiveWordCommandHandler _sut;
    private readonly ISensitiveWordsRepository _sensitiveWordsRepository = Substitute.For<ISensitiveWordsRepository>();
    private readonly ICacheRepository _cacheRepository = Substitute.For<ICacheRepository>();
    private readonly IValidator<CreateSensitiveWordCommand> _validator = new CreateSensitiveWordCommandValidator();
    
    public CreateSensitiveWordTests() 
        => _sut = new CreateSensitiveWordCommandHandler(_sensitiveWordsRepository, _cacheRepository);
    
    [Fact]
    public async Task CreateSensitiveWord_ShouldReturnConflict_WhenSensitiveWordExists()
    {
        // Arrange
        _sensitiveWordsRepository
            .Exists(Arg.Any<string>())
            .Returns(true);
        var request = new CreateSensitiveWordCommand("SensitiveWord");
        
        // Act
        var result = await _sut.Handle(request, CancellationToken.None);
        
        // Assert
        result.FirstError.Description.Should().Be("Sensitive word already exists");
    }
    
    [Fact]
    public async Task CreateSensitiveWord_ShouldReturnFailure_WhenSensitiveWordNotAdded()
    {
        // Arrange
        _sensitiveWordsRepository
            .Exists(Arg.Any<string>())
            .Returns(false);
        _sensitiveWordsRepository
            .AddSensitiveWordAsync(Arg.Any<SensitiveWord>())
            .Returns(false);
        var request = new CreateSensitiveWordCommand("SensitiveWord");
        
        // Act
        var result = await _sut.Handle(request, CancellationToken.None);
        
        // Assert
        result.FirstError.Description.Should().Be("Failed to add sensitive word");
    }
    
    [Fact]
    public async Task CreateSensitiveWord_ShouldReturnFailure_WhenSensitiveWordNotCached()
    {
        // Arrange
        _sensitiveWordsRepository
            .Exists(Arg.Any<string>())
            .Returns(false);
        _sensitiveWordsRepository
            .AddSensitiveWordAsync(Arg.Any<SensitiveWord>())
            .Returns(true);
        _cacheRepository
            .AddOrUpdateSensitiveWordAsync(Arg.Any<SensitiveWord>())
            .Returns(false);
        var request = new CreateSensitiveWordCommand("SensitiveWord");
        
        // Act
        var result = await _sut.Handle(request, CancellationToken.None);
        
        // Assert
        result.FirstError.Description.Should().Be("Failed to cache sensitive word");
    }
    
    [Fact]
    public async Task CreateSensitiveWord_ShouldReturnSensitiveWordResponse_WhenSensitiveWordAdded()
    {
        // Arrange
        _sensitiveWordsRepository
            .Exists(Arg.Any<string>())
            .Returns(false);
        _sensitiveWordsRepository
            .AddSensitiveWordAsync(Arg.Any<SensitiveWord>())
            .Returns(true);
        _cacheRepository
            .AddOrUpdateSensitiveWordAsync(Arg.Any<SensitiveWord>())
            .Returns(true);
        var request = new CreateSensitiveWordCommand("SensitiveWord");
        
        // Act
        var result = await _sut.Handle(request, CancellationToken.None);
        
        // Assert
        result.Value.Word.Should().Be("SensitiveWord");
    }
    
    [Fact]
    public async Task ValidateCreateSensitiveWordCommand_ShouldReturnFailure_WhenSensitiveWordIsNull()
    {
        // Arrange
        var request = new CreateSensitiveWordCommand(null!);
        
        // Act
        var result = await _validator.ValidateAsync(request, CancellationToken.None);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.First().ErrorMessage.Should().Be("'Sensitive Word' must not be empty.");
        result.Errors.Last().ErrorMessage.Should().Be("'Sensitive Word' must not be empty.");
    }
}