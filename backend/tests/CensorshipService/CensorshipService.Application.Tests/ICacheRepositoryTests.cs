using BleepGuard.Contracts.SensitiveWords;
using CensorshipService.Application.Common.Interfaces;
using FluentAssertions;
using NSubstitute;

namespace CensorshipService.Application.Tests;

public class CacheRepositoryTests
{
    private readonly ICacheRepository _cacheRepository = Substitute.For<ICacheRepository>();
    
    [Fact]
    public async Task GetAllSensitiveWordsAsync_ShouldReturnEmptyHashSet_WhenNoSensitiveWordsExist() 
    {
        // Arrange
        _cacheRepository
            .GetAllSensitiveWordsAsync()
            .Returns([]);
            
        // Act
        var result = await _cacheRepository.GetAllSensitiveWordsAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllSensitiveWordsAsync_ShouldReturnSensitiveWords_WhenSensitiveWordsExist()
    {
        // Arrange
        HashSet<string> expectedSensitiveWords = ["DOUBLETABLE", "SELECT * FROM"];
        _cacheRepository
            .GetAllSensitiveWordsAsync()
            .Returns(expectedSensitiveWords);
        
        // Act
        var result = await _cacheRepository.GetAllSensitiveWordsAsync();
        
        // Assert
        result.Should().BeEquivalentTo(expectedSensitiveWords);
    }

    [Fact]
    public async Task AddSensitiveWordsAsync_ShouldCallMethod_WhenPassingSensitiveWords()
    {
        // Arrange
        List<SensitiveWordResponse> sensitiveWords = [
            new (Guid.NewGuid(), "DOUBLETABLE")
        ];
        
        _cacheRepository
            .AddSensitiveWordsAsync(Arg.Any<IList<SensitiveWordResponse>>())
            .Returns(Task.CompletedTask);
        
        // Act
        await _cacheRepository.AddSensitiveWordsAsync(sensitiveWords);
        
        // Assert
        await _cacheRepository.Received(1).AddSensitiveWordsAsync(sensitiveWords);
    }
    
    [Fact]
    public async Task AddSensitiveWordsAsync_ShouldThrowArgumentNullException_WhenPassingNoSensitiveWords()
    {
        // Arrange
        _cacheRepository
            .When(x =>
                x.AddSensitiveWordsAsync(Arg.Any<IList<SensitiveWordResponse>>())
            ).Do(_ => throw new ArgumentNullException(nameof(IList<SensitiveWordResponse>),
                "The list of sensitive words cannot be null or empty"));
        
        // Act
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            async () => await _cacheRepository.AddSensitiveWordsAsync([])
        );
        
        // Assert
        exception.Message.Should().Be("The list of sensitive words cannot be null or empty (Parameter 'IList')");
    }

}