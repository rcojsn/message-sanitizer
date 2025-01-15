using CensorshipService.Infrastructure.Redis;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using StackExchange.Redis;

namespace CensorshipService.Infrastructure.Tests;

public class CacheRepositoryTests
{
    private readonly CacheRepository _sut;
    private readonly ILogger<CacheRepository> _logger = Substitute.For<ILogger<CacheRepository>>();
    private readonly IConnectionMultiplexer _redis = Substitute.For<IConnectionMultiplexer>();

    public CacheRepositoryTests() => _sut = new CacheRepository(_redis, _logger);
    
    [Fact]
    public async Task GetAllSensitiveWordsAsync_ShouldReturnEmptyHashSet_WhenNoSensitiveWordsExist() 
    {
        // Arrange
        _redis.GetDatabase()
            .HashGetAllAsync(Arg.Any<RedisKey>())
            .Returns([]);
        // Act
        var result = await _sut.GetAllSensitiveWordsAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllSensitiveWordsAsync_ShouldReturnSensitiveWords_WhenSensitiveWordsExist()
    {
        // Arrange
        HashSet<string> expectedSensitiveWords = ["DOUBLETABLE", "SELECT * FROM"];
        _redis.GetDatabase()
            .HashGetAllAsync(Arg.Any<RedisKey>())
            .Returns([
                new HashEntry(1, expectedSensitiveWords.First()),
                new HashEntry(2, expectedSensitiveWords.Last())
            ]);
        // Act
        var result = await _sut.GetAllSensitiveWordsAsync();

        // Assert
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expectedSensitiveWords);
    }
    
    [Fact]
    public async Task AddSensitiveWordsAsync_ShouldThrowArgumentNullException_WhenPassingNoSensitiveWords()
    {
        // Act
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.AddSensitiveWordsAsync([]));
        
        // Assert
        Assert.Equal("The list of sensitive words cannot be null or empty (Parameter 'response')", exception.Message);
    }


}