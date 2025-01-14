using AdminService.Application.Common.Interfaces;
using AdminService.Application.Features.SensitiveWords.Queries.GetSensitiveWords;
using AdminService.Application.Mappings;
using AdminService.Domain.SensitiveWords;
using FluentAssertions;
using NSubstitute;

namespace AdminService.Application.Tests.Features.SensitiveWords;

public class GetSensitiveWordsTests
{
    private readonly GetSensitiveWordsQueryHandler _sut;
    private readonly ISensitiveWordsRepository _sensitiveWordsRepository = Substitute.For<ISensitiveWordsRepository>();
    
    public GetSensitiveWordsTests()
    => _sut = new GetSensitiveWordsQueryHandler(_sensitiveWordsRepository);
    
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