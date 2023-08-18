using MSIT147thGraduationTopic.Models.Services;

namespace MSIT147thGraduationTopic.Models.BackGroundServiecs
{
    public class AutoPopularityCalculationService : BackgroundService
    {

        private readonly ILogger<AutoPopularityCalculationService> _logger;
        private readonly TimeSpan _period = TimeSpan.FromSeconds(20);
        private readonly IServiceScopeFactory _factory;
        private int _executionCount = 0;

        public bool IsEnabled { get; set; } = true;

        public AutoPopularityCalculationService(
            ILogger<AutoPopularityCalculationService> logger,
            IServiceScopeFactory factory)
        {
            _logger = logger;
            _factory = factory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new PeriodicTimer(_period);
            while (
                !stoppingToken.IsCancellationRequested &&
                await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    if (IsEnabled && RecommendService.TimeToExecute)
                    {
                        await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
                        RecommendService service = asyncScope.ServiceProvider.GetRequiredService<RecommendService>();
                        await service.CalculatePopularities();

                        _executionCount++;
                        string message = $"執行 AutoPopularityCalculationService - 已執行次數: {_executionCount}";
                        _logger.LogInformation(message);
                    }
                    else
                    {
                        string message = $"跳過 AutoPopularityCalculationService - 已執行次數: {_executionCount}";
                        _logger.LogInformation(message);
                    }

                }
                catch (Exception ex)
                {
                    string message = $"Failed to execute AutoPopularityCalculationService, exception message : {ex.Message}.";
                    _logger.LogInformation(message);
                }
            }
        }

    }
}

