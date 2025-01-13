using System.Data;
using AdminService.Domain.SensitiveWords;
using AdminService.Infrastructure.SensitiveWords.Persistence;
using BleepGuard.Infrastructure.Common;
using Dapper;
using Moq;
using Moq.Dapper;

namespace AdminService.Infrastructure.Tests;

public class SensitiveWordsRepositoryTests
{
    private readonly SensitiveWordsRepository _sut;
    private readonly Mock<IDbConnectionFactory> _connectionFactory = new();
    private readonly Mock<IDbConnection> _dbConnection = new();

    public SensitiveWordsRepositoryTests()
    {
        _connectionFactory
            .Setup(x => x.CreateConnectionAsync(CancellationToken.None))
            .ReturnsAsync(_dbConnection.Object);
        _sut = new SensitiveWordsRepository(_connectionFactory.Object);
    }
    
    [Fact]
    public async Task AddSensitiveWordAsync_ShouldAddSensitiveWord_WhenValid()
    {
        // Arrange
        var sensitiveWord = new SensitiveWord(Guid.NewGuid(), "DELETE");
        _dbConnection.SetupDapperAsync(
                c => c.ExecuteAsync(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null,
                    null,
                    null))
            .ReturnsAsync(1);

        // Act
        var result = await _sut.AddSensitiveWordAsync(sensitiveWord);

        // // Assert
        Assert.True(result);
    }
}