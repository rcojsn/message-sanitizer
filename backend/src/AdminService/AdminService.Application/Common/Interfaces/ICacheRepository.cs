using AdminService.Domain.SensitiveWords;

namespace AdminService.Application.Common.Interfaces;

public interface ICacheRepository
{
    Task AddOrUpdateSensitiveWordAsync(SensitiveWord sensitiveWord);
    Task DeleteSensitiveWordByIdAsync(Guid id);
}