using BleepGuard.Domain.SensitiveWords;

namespace BleepGuard.Application.Common.Interfaces;

public interface ISensitiveWordsRepository
{
    Task AddSensitiveWordAsync(SensitiveWord sensitiveWord);
    Task<SensitiveWord?> GetByIdAsync(Guid sensitiveWordId);
    Task<IList<SensitiveWord>> GetAllAsync();
    Task<bool> UpdateSensitiveWordAsync(SensitiveWord sensitiveWord);
    Task DeleteSensitiveWordAsync(SensitiveWord sensitiveWord);
}