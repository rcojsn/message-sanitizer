using AdminService.Application.Common.Interfaces;
using AdminService.Application.Features.SensitiveWords.Queries.GetSensitiveWord;
using AdminService.Application.Mappings;
using AdminService.Domain.SensitiveWords;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace AdminService.Application.Tests.Features.SensitiveWords;

public class GetSensitiveWordTests
{
    private readonly GetSensitiveWordQueryHandler _sut;
    private readonly ISensitiveWordsRepository _sensitiveWordsRepository = Substitute.For<ISensitiveWordsRepository>();
    private readonly ILogger<GetSensitiveWordQueryHandler> _logger = Substitute.For<ILogger<GetSensitiveWordQueryHandler>>();
    
    public GetSensitiveWordTests() 
        => _sut = new GetSensitiveWordQueryHandler(_sensitiveWordsRepository, _logger);
    
    [Fact]
    public async Task GetSensitiveWord_ShouldReturnNotFound_WhenSensitiveWordNotFound()
    {
        // Arrange
        var query = new GetSensitiveWordQuery(Guid.NewGuid());
        _sensitiveWordsRepository
            .GetByIdAsync(query.Id)
            .Returns((SensitiveWord)null!);
        
        // Act
        var result = await _sut.Handle(query, CancellationToken.None);
        
        // Assert
        result.FirstError.Description.Should().Be("Sensitive word not found");
    }
    
    [Fact]
    public async Task GetSensitiveWord_ShouldReturnOk_WhenSensitiveWordFound()
    {
        // Arrange
        var sensitiveWord = new SensitiveWord(Guid.NewGuid(), "SensitiveWord");
        var query = new GetSensitiveWordQuery(sensitiveWord.Id);
        _sensitiveWordsRepository
            .GetByIdAsync(sensitiveWord.Id)
            .Returns(sensitiveWord);
        
        // Act
        var result = await _sut.Handle(query, CancellationToken.None);
        
        // Assert
        result.Value.Should().BeEquivalentTo(sensitiveWord.MapToSensitiveWordResponse());
    }
}