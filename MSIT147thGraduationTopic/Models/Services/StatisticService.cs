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


        public async Task<SaleChartDto?> GetSaleChart(string measurement, string classification, int daysBefore)
        {
            var timeBefore = DateTime.Now.AddDays(-daysBefore);
            measurement = measurement.Trim().ToLower();
            classification = classification.Trim().ToLower();

            var dto = await _repo.GetSaleChart(measurement, classification, timeBefore);
            if (dto != null) dto.MeasurementUnit = classification switch
            {
                "quantity" => "購買數量",
                "profit" => "訂單總額",
                _ => string.Empty
            };
            return dto;
        }

    }
}
