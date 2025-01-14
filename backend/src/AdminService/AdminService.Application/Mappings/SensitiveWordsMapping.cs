using AdminService.Application.Features.SensitiveWords.Commands.UpdateSensitiveWord;
using AdminService.Domain.SensitiveWords;
using BleepGuard.Contracts.SensitiveWords;

namespace AdminService.Application.Mappings;

public static class SensitiveWordsMapping
{
    public static SensitiveWordResponse MapToSensitiveWordResponse(this SensitiveWord sensitiveWord)
    {
        var  (id , word) = sensitiveWord;
        return new SensitiveWordResponse(id, word);
    }

    public static IList<SensitiveWordResponse> MapToSensitiveWordsResponse(this IList<SensitiveWord> sensitiveWords)
    => sensitiveWords
        .Select(MapToSensitiveWordResponse)
        .ToList();

    public static SensitiveWord MapFrom(this SensitiveWord sensitiveWord, UpdateSensitiveWordCommand request)
    => sensitiveWord with { Word = request.Word };
}