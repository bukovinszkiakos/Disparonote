using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class NoteCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<NoteCleanupService> _logger;
    private readonly TimeSpan _delay = TimeSpan.FromHours(1);

    public NoteCleanupService(IServiceProvider serviceProvider, ILogger<NoteCleanupService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var noteRepository = scope.ServiceProvider.GetRequiredService<INoteRepository>();
                    await noteRepository.DeleteExpiredNotesAsync();
                    _logger.LogInformation("Expired notes cleanup executed at {Time}.", DateTime.UtcNow);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during expired notes cleanup.");
            }

            await Task.Delay(_delay, stoppingToken);
        }
    }
}
