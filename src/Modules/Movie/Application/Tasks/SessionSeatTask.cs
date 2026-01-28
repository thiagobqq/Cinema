using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Movie.Infra.Data;
using Movie.Domain.Enums;

namespace Movie.Application.Tasks
{
    public class SessionSeatTask : BackgroundService
    {
        private readonly ILogger<SessionSeatTask> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _intervaloVerificacao = TimeSpan.FromMinutes(5); 
        private readonly int _batchSize = 100;

        public SessionSeatTask(
            ILogger<SessionSeatTask> logger,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await HandleSessionSeatsExpiration(stoppingToken);
                    await Task.Delay(_intervaloVerificacao, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception)
                {
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
            }
        }

        private async Task HandleSessionSeatsExpiration(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MovieDbContext>();

            var agora = DateTime.UtcNow;
            
            var count = await dbContext.SessionSeats
                .Where(e => e.ReservedUntil <= agora && e.Status == SeatStatus.Locked)
                .Take(_batchSize)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(e => e.Status, SeatStatus.Available)
                    .SetProperty(e => e.ReservedUntil, (DateTime?)null),
                stoppingToken);

            if (count > 0)
            {
                _logger.LogInformation(
                    "{Count} Expired session seats have been released.", 
                    count);
            }
        }
    }    
}