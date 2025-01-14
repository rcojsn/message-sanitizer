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
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnSensitiveWord_WhenValid()
    {
        // Arrange
        var sensitiveWord = new SensitiveWord(Guid.NewGuid(), "GET");
        _dbConnection.SetupDapperAsync(
                c => c.QuerySingleOrDefaultAsync<SensitiveWord>(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null,
                    null,
                    null))
            .ReturnsAsync(sensitiveWord);

        // Act
        var result = await _sut.GetByIdAsync(sensitiveWord.Id);

        // // Assert
        Assert.NotNull(result);
        Assert.Equal(sensitiveWord.Id, result.Id);
        Assert.Equal(sensitiveWord.Word, result.Word);
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllSensitiveWords_WhenValid()
    {
        // Arrange
        List<SensitiveWord> sensitiveWords =
        [
            new(Guid.NewGuid(), "GET"),
            new(Guid.NewGuid(), "POST"),
            new(Guid.NewGuid(), "PUT")
        ];
        _dbConnection.SetupDapperAsync(
                c => c.QueryAsync<SensitiveWord>(
                    It.IsAny<string>(),
                    null,
                    null,
                    null,
                    null))
            .ReturnsAsync(sensitiveWords);

        // Act
        var result = await _sut.GetAllAsync();

        // // Assert
        Assert.Equal(sensitiveWords.Count, result.Count);
        sensitiveWords.ForEach(sensitiveWord =>
        {
            var (id, word) = sensitiveWord;
            
            var resultSensitiveWord = result.Single(x => x.Id == id);
            var (resultId, resultWord) = resultSensitiveWord;
            
            Assert.Equal(id, resultId);
            Assert.Equal(word, resultWord);
        });
    }
    
    [Fact]
    public async Task UpdateSensitiveWordAsync_ShouldUpdateSensitiveWord_WhenValid()
    {
        // Arrange
        var sensitiveWord = new SensitiveWord(Guid.NewGuid(), "PUT");
        _dbConnection.SetupDapperAsync(
                c => c.ExecuteAsync(
                    It.IsAny<string>(),
                    It.IsAny<object>(),
                    null,
                    null,
                    null))
            .ReturnsAsync(1);

        // Act
        var result = await _sut.UpdateSensitiveWordAsync(sensitiveWord);

        // // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task DeleteSensitiveWordAsync_ShouldDeleteSensitiveWord_WhenValid()
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
        var result = await _sut.DeleteSensitiveWordAsync(sensitiveWord);

        // // Assert
        Assert.True(result);
    }
}