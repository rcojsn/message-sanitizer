using CensorshipService.Application.Common.Interfaces;
using CensorshipService.Application.Common.Interfaces.External;

namespace CensorshipService.Worker;

public class Worker(
    IAdminServiceApiClient adminServiceApiClient,
    ICacheRepository cacheRepository
) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var sensitiveWordsResponse = await adminServiceApiClient.GetSensitiveWords();

        await cacheRepository.AddSensitiveWords(sensitiveWordsResponse);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}