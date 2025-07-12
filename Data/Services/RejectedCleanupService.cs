// Services/RejectedCleanupService.cs
using JapaneseLearningPlatform.Data;
using JapaneseLearningPlatform.Data.Enums;
using JapaneseLearningPlatform.Models.Partner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class RejectedCleanupService : IHostedService, IDisposable
{
    private readonly IServiceProvider _provider;
    private Timer? _timer;

    public RejectedCleanupService(IServiceProvider provider)
        => _provider = provider;

    public Task StartAsync(CancellationToken _)
    {
        // run once a day
        _timer = new Timer(DoCleanup, null,
                           TimeSpan.Zero,
                           TimeSpan.FromHours(24));
        return Task.CompletedTask;
    }

    private void DoCleanup(object? _)
    {
        using var scope = _provider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var cutoff = DateTime.UtcNow.AddDays(-7);
        var oldRejected = db.PartnerProfiles
            .Where(p => p.Status == PartnerStatus.Rejected
                     && p.DecisionAt < cutoff);

        db.PartnerProfiles.RemoveRange(oldRejected);
        db.SaveChanges();
    }

    public Task StopAsync(CancellationToken _)
    {
        _timer?.Dispose();
        return Task.CompletedTask;
    }

    public void Dispose() => _timer?.Dispose();
}
