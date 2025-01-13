using BleepGuard.Contracts.SensitiveWords;

namespace CensorshipService.Application.Common.Interfaces;

public interface ICacheRepository
{
    Task<HashSet<string>> GetAllSensitiveWordsAsync();
    Task AddSensitiveWordsAsync(IList<SensitiveWordResponse> response);
}