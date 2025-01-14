using AdminService.Domain.SensitiveWords;

namespace AdminService.Application.Common.Interfaces;

public interface ICacheRepository
{
    Task<bool> AddOrUpdateSensitiveWordAsync(SensitiveWord sensitiveWord);
    Task<bool> DeleteSensitiveWordByIdAsync(Guid id);
}