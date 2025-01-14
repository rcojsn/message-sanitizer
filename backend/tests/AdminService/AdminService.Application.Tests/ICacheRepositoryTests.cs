using AdminService.Application.Common.Interfaces;
using AdminService.Domain.SensitiveWords;
using NSubstitute;

namespace AdminService.Application.Tests;

public class CacheRepositoryTests
{
    private readonly ICacheRepository _cacheRepository = Substitute.For<ICacheRepository>();
    
    [Fact]
    public async Task AddOrUpdateSensitiveWordAsync_ShouldCallMethod_WhenPassingSensitiveWord()
    {
        // Arrange
        SensitiveWord sensitiveWord = new(Guid.NewGuid(), "DOUBLETABLE");
        
        _cacheRepository
            .AddOrUpdateSensitiveWordAsync(Arg.Any<SensitiveWord>())
            .Returns(true);
        
        // Act
        await _cacheRepository.AddOrUpdateSensitiveWordAsync(sensitiveWord);
        
        // Assert
        await _cacheRepository.Received(1).AddOrUpdateSensitiveWordAsync(sensitiveWord);
    }
    
    [Fact]
    public async Task DeleteSensitiveWordByIdAsync_ShouldCallMethod_WhenPassingId()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        _cacheRepository
            .DeleteSensitiveWordByIdAsync(Arg.Any<Guid>())
            .Returns(true);
        
        // Act
        await _cacheRepository.DeleteSensitiveWordByIdAsync(id);
        
        // Assert
        await _cacheRepository.Received(1).DeleteSensitiveWordByIdAsync(id);
    }
}