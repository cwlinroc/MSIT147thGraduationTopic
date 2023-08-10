using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.Models.Dtos.Recommend;
using MSIT147thGraduationTopic.Models.Infra.Repositories;

namespace MSIT147thGraduationTopic.Models.Services
{
    public class RecommendService
    {
        private readonly GraduationTopicContext _context;
        private readonly RecommandRepositoy _repo;

        public RecommendService(GraduationTopicContext context)
        {
            _context = context;
            _repo = new(context);
        }



        public async Task<int> CalculatePopularities(RecommendCalculateBo bo)
        {
            var specs = await _repo.GetAllSpecsWithEvaluation();

            //自訂評分
            var taskWeightedEntries = _repo.GetAllManuallyWeightedEntries();
            var taskConvertedEntries = ConvertEntries(taskWeightedEntries);

            //顧客評價轉換分數
            bo.RateEvaluation?.Invoke(specs);
            //購買數量轉換分數
            bo.RatePurchased?.Invoke(specs);

            //將自訂評分對應到spec
            var weightedEntries = await taskConvertedEntries;
            foreach (var entry in weightedEntries)
            {
                specs.First(o => o.SpecId == entry.SpecId).CustomRating = 0.5 + 0.05 * entry.Weight;
            }

            //依權重計算popularity
            RecommandFunctions.CalculatePopularity(specs, bo.EvaluationWeight, bo.PurchasedWeight, bo.CustomWeight);

            return await _repo.UpdateSpecsPopularity(specs);
        }

        private async Task<List<ManuallyWeightedEntryDto>> ConvertEntries(Task<List<ManuallyWeightedEntryDto>> taskWeightedEntries)
        {
            var weightedEntries = await taskWeightedEntries;

            var tagEntries = weightedEntries.Where(o => o.TagId != null).ToList();
            var tagConvertEntries = await _repo.GetConvertedTagEntries(tagEntries);

            var merchandiseEntries = weightedEntries.Where(o => o.MerchandiseId != null).ToList();
            var merchandiseConvertEntries = await _repo.GetConvertedMerchandiseEntries(merchandiseEntries);

            var specEntries = weightedEntries.Where(o => o.SpecId != null).ToList();

            var newEntries = tagConvertEntries.Concat(merchandiseConvertEntries).Concat(specEntries);
            return newEntries.GroupBy(o => o.SpecId)
                .Select(o => new ManuallyWeightedEntryDto { SpecId = o.Last().SpecId, Weight = o.Last().Weight }).ToList();
        }


        public async Task<RatingDataDto> GetRatingData()
        {
            return await _repo.GetRatingData();
        }

        public async Task<int> UpdateRatingData(int number, string col)
        {
            return await _repo.UpdateRatingData(number, col);
        }

        public async Task<List<string>> GetMostPopularSpecsName(int top)
        {
            return await _repo.GetMostPopularSpecsName(top);
        }


    }
}
