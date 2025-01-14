using AdminService.Domain.SensitiveWords;

namespace AdminService.Application.Common.Interfaces;

public interface ISensitiveWordsRepository
{
    Task<bool> AddSensitiveWordAsync(SensitiveWord sensitiveWord);
    Task<SensitiveWord?> GetByIdAsync(Guid sensitiveWordId);
    Task<IList<SensitiveWord>> GetAllAsync();
    Task<bool> UpdateSensitiveWordAsync(SensitiveWord sensitiveWord);
    Task<bool> DeleteSensitiveWordAsync(SensitiveWord sensitiveWord);
    Task<bool> Exists(string word);
}