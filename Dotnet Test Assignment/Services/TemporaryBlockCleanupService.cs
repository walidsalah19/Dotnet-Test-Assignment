using Dotnet_Test_Assignment.Interfaces.IServices;

namespace Dotnet_Test_Assignment.Services
{
    public class TemporaryBlockCleanupService : BackgroundService
    {
        private readonly ITemporaryBlockService _tempService;

        public TemporaryBlockCleanupService(ITemporaryBlockService tempService)
        {
            _tempService = tempService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _tempService.CleanupExpiredBlocks();
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
