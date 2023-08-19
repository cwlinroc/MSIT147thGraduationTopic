using Microsoft.IdentityModel.Tokens;
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
                "quantity" => "購買數量(個)",
                "profit" => "訂單總額($NTD)",
                _ => string.Empty
            };
            return dto;
        }
        public async Task<List<(string label, long data)>> GetMostSalesMerchandises(string classification, int daysBefore)
        {
            var timeBefore = DateTime.Now.AddDays(-daysBefore);
            classification = classification.Trim().ToLower();

            return await _repo.GetMostSalesMerchandises(classification, timeBefore);
        }



        public async Task<SaleTrendDto?> GetSalesTrend(
            string measurement,
            string classification,
            string timeUnit,
            int timeIntervals)
        {
            IEnumerable<(DateTime EndDate, DateTime StartDate)>? dateList = timeUnit.Trim().ToLower() switch
            {
                "month" => Enumerable.Range(0, timeIntervals).Select((o, i) => (DateTime.Now.AddMonths(-i), DateTime.Now.AddMonths(-i - 1))),
                "week" => Enumerable.Range(0, timeIntervals).Select((o, i) => (DateTime.Now.AddDays(-7 * i), DateTime.Now.AddDays(-7 * i - 7))),
                _ => null
            };
            if (dateList.IsNullOrEmpty()) return null;
            dateList = dateList!.Reverse();

            List<List<(string label, long data)>> fullData = new();

            foreach (var (endDate, startDate) in dateList!)
            {
                var result = await _repo.GetSalesTrendPeriod(measurement, classification, startDate, endDate);
                fullData.Add(result.ToList());
            }           

            var trendDto = new SaleTrendDto() { DataSets = new List<SaleTrendDataSetDto>() };

            int labelCount = fullData[0].Count;
            long test = fullData[0][0].data;
            for (int i = 0; i < labelCount; i++)
            {
                var datas = fullData.Select(o => o[i].data).ToList();
                var dto = new SaleTrendDataSetDto { Data = datas, Label = fullData[0][i].label };
                trendDto.DataSets.Add(dto);
            }

            trendDto.Labels = dateList.Select(o => o.StartDate.ToString("M") + "~" + o.EndDate.ToString("M")).ToList();

            return trendDto;
        }

        public async Task<int[]?> GetEvaluationScores(int merchandiseId)
        {
            return await _repo.GetEvaluationScores(merchandiseId);
        }

        public async Task<TimeTrendDto?> GetMerchandiseTrend(
            string measurement,
            string classification,
            string timeUnit,
            int id)
        {
            int intervalNum = timeUnit switch
            {
                "day" => 7,
                "month" => 1,
                _ => 1
            };

            string? name = await _repo.GetNameById(id, measurement);
            if (name == null) return new();

            IEnumerable<(DateTime start, DateTime end, long data)>? data = await _repo
                .GetMerchandiseTrend(measurement, classification, timeUnit, intervalNum, id);

            if (data == null) return new();

            return new TimeTrendDto
            {
                DataTitle = name,
                Labels = data.Select(o => o.start.ToString("M") + "~" + o.end.ToString("M")).ToArray(),
                Datas = data.Select(o => o.data).ToArray(),
            };

        }

        public async Task<List<string>?> GetAutoCompleteNames(string queryCol, string keyword)
        {
            return await _repo.GetAutoCompleteNames(queryCol, keyword);
        }

        public async Task<(int, string)> GetSearchedId(string queryCol, string keyword)
        {
            return await _repo.GetSearchedId(queryCol, keyword);
        }

        public async Task<MerchandiseRadarDto?> GetMerchandiseRadar(string measurement, int id)
        {
            return await _repo.GetMerchandiseRadar(measurement, id);
        }
    }
}
