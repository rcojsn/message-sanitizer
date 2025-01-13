using AdminService.Domain.SensitiveWords;
using AdminService.Infrastructure.Redis;
// using NSubstitute;
using StackExchange.Redis;

namespace AdminService.Infrastructure.Tests;

public class CacheRepositoryTests
{
    private readonly CacheRepository _sut;
    // private readonly IConnectionMultiplexer _redis = Substitute.For<IConnectionMultiplexer>();

    // public CacheRepositoryTests() => _sut = new CacheRepository(_redis);

    [Fact]
    public async Task AddOrUpdateSensitiveWordAsync_ShouldAddNewSensitiveWord_WhenValid()
    {
        // Arrange
        // _redis
        //     .GetDatabase()
        //     .HashSetAsync(Arg.Any<RedisKey>(), Arg.Any<HashEntry[]>(), Arg.Any<string>())
        //     .Returns(Task.CompletedTask);
        
        // Act
        // await _sut.AddOrUpdateSensitiveWordAsync(new SensitiveWord(Guid.NewGuid(), "DELETE"));
        
        // Assert
        // await _redis
        //     .GetDatabase()
        //     .Received(1)
        //     .HashSetAsync(Arg.Any<RedisKey>(), Arg.Any<HashEntry[]>());
    }
}