using BleepGuard.Contracts.SensitiveWords;

namespace CensorshipService.Application.Common.Interfaces;

public interface ICacheRepository
{
    Task<HashSet<string>> GetSensitiveWordsAsync();
    Task AddSensitiveWordsAsync(IList<SensitiveWordResponse> response);
}