using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos.Statistic;
using MSIT147thGraduationTopic.Models.Infra.Repositories;

namespace MSIT147thGraduationTopic.Models.Services
{
    public class StatisticService
    {

        private readonly GraduationTopicContext _context;
        private readonly StatisticRepository _repo;

        public StatisticService(GraduationTopicContext context)
        {
            _context = context;
            _repo = new StatisticRepository(context);
        }


        public async Task<SaleChartDto?> GetSaleChart(string measurement, string classification, int monthBefore)
        {
            var timeBefore = DateTime.Now.AddMonths(-monthBefore);
            measurement = measurement.Trim().ToLower();
            classification = classification.Trim().ToLower();
            return await _repo.GetSaleChart(measurement, classification, timeBefore);
        }

    }
}
