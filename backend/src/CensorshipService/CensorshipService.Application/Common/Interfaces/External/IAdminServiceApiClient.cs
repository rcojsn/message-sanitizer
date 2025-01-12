using BleepGuard.Contracts.SensitiveWords;
using Refit;

namespace CensorshipService.Application.Common.Interfaces.External;

public interface IAdminServiceApiClient
{
    [Get("/sensitiveWords")]
    Task<IList<SensitiveWordResponse>> GetSensitiveWords();
}