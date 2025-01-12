using BleepGuard.Contracts.SensitiveWords;

namespace CensorshipService.Application.Common.Interfaces;

public interface ICacheRepository
{
    Task AddSensitiveWords(IList<SensitiveWordResponse> response);
}