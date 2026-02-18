using pingword.src.Interfaces.StudyState;

namespace pingword.src.Workers
{
    public class StudyStateWorker : BackgroundService
    {
        
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<StudyStateWorker> _logger;

        public StudyStateWorker(IServiceProvider serviceProvider, ILogger<StudyStateWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(">>> Inactivity Verification Service Started.");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var studyService = scope.ServiceProvider.GetRequiredService<IStudyStateService>();

                        DateTime limitDate = DateTime.UtcNow.AddDays(-7);

                        _logger.LogInformation("Checking inactive studies since: {date}", limitDate);

                        int totalUpdated = await studyService.ProcessExpireStateAsync(limitDate);

                        _logger.LogInformation("Success: {count} studies were marked as INACTIVE.", totalUpdated);

                    }
                } catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing inactivity check.");
                }
                await Task.Delay(TimeSpan.FromDays(2), stoppingToken);
            }
        }

    }
}
