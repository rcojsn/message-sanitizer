using AdminService.Application.Common.Interfaces;
using AdminService.Application.Features.SensitiveWords.Queries.GetSensitiveWords;
using AdminService.Application.Mappings;
using AdminService.Domain.SensitiveWords;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace AdminService.Application.Tests.Features.SensitiveWords;

public class GetSensitiveWordsTests
{
    private readonly GetSensitiveWordsQueryHandler _sut;
    private readonly ISensitiveWordsRepository _sensitiveWordsRepository = Substitute.For<ISensitiveWordsRepository>();
    private readonly ILogger<GetSensitiveWordsQueryHandler> _logger = Substitute.For<ILogger<GetSensitiveWordsQueryHandler>>();
    
    public GetSensitiveWordsTests()
    => _sut = new GetSensitiveWordsQueryHandler(_sensitiveWordsRepository, _logger);
    
    [Fact]
    public async Task GetSensitiveWords_WhenCalled_ShouldReturnSensitiveWords()
    {
        // Arrange
        List<SensitiveWord> sensitiveWords =
        [
            new(Guid.NewGuid(), "GET1"),
            new(Guid.NewGuid(), "GET2")
        ];
        _sensitiveWordsRepository
            .GetAllAsync()
            .Returns(sensitiveWords);
        var mappedResponse = sensitiveWords.MapToSensitiveWordsResponse();
        
        // Act
        var result = await _sut.Handle(new GetSensitiveWordsQuery(), CancellationToken.None);
        
        // Assert
        result.Should().BeEquivalentTo(mappedResponse);
    }
}