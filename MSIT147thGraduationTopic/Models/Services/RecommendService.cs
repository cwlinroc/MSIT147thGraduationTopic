using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.Models.Dtos.Recommend;
using MSIT147thGraduationTopic.Models.Infra.Repositories;
using static MSIT147thGraduationTopic.Controllers.Recommend.ApiRecommendController;

namespace MSIT147thGraduationTopic.Models.Services
{
    public class RecommendService
    {
        private readonly GraduationTopicContext _context;
        private readonly RecommendRepositoy _repo;

        public RecommendService(GraduationTopicContext context)
        {
            _context = context;
            _repo = new(context);
        }



        public async Task<int> CalculatePopularities()
        {
            var rateData = await _repo.GetRatingData();
            RecommendCalculateBo bo = GetCalculateBO(rateData);

            var specs = await _repo.GetAllSpecsWithEvaluation();

            //自訂評分
            var taskWeightedEntries = _repo.GetAllManuallyWeightedEntries();
            var taskConvertedEntries = ConvertEntries(taskWeightedEntries);

            //顧客評價轉換分數
            bo.RateEvaluationFunc?.Invoke(specs);
            //購買數量轉換分數
            bo.RatePurchased?.Invoke(specs);

            //將自訂評分對應到spec
            var weightedEntries = await taskConvertedEntries;
            foreach (var entry in weightedEntries)
            {
                specs.First(o => o.SpecId == entry.SpecId).CustomRating = 0.5 + 0.05 * entry.Weight;
            }

            //依權重計算popularity
            RecommandFunctions.CalculatePopularity(specs, bo.EvaluationWeight, bo.PurchasedWeight, bo.ManuallyWeight);

            return await _repo.UpdateSpecsPopularity(specs);
        }

        private RecommendCalculateBo GetCalculateBO(RatingDataDto dto)
        {
            return new RecommendCalculateBo()
            {
                EvaluationWeight = dto.EvaluationWeight,
                PurchasedWeight = dto.PurchasedWeight,
                ManuallyWeight = dto.ManuallyWeight,
                RateEvaluationFunc = dto.RateEvaluationFunc switch
                {
                    1 => RecommandFunctions.RateEvaluationWithMathematicaMean,//數理平均
                    2 => RecommandFunctions.RateEvaluationWithBayesianAverage,//貝葉森平均
                    _ => RecommandFunctions.RateEvaluationWithMathematicaMean
                },
                RatePurchased = dto.RatePurchaseFunc switch
                {
                    1 => RecommandFunctions.RatePurchasedWithProportion,//線性比例轉換
                    2 => RecommandFunctions.RatePurchasedWithLogTransform,//log2對數轉換
                    _ => RecommandFunctions.RatePurchasedWithProportion
                }
            };
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

        public async Task<List<SearchedItemsDto>> GetSearchedItems(string text, string type)
        {
            if (string.IsNullOrEmpty(text)) return new();
            if (type != "tag" && type != "merchandise" && type != "spec") return new();
            return await _repo.GetSearchedItems(text, type);
        }

        public async Task<int> InsertWeightedEntries(InsertEntriesRecord record)
        {
            if (record.Ids.IsNullOrEmpty() || record.Weight < -10 || record.Weight > 10) return -1;
            if (record.Type != "tag" && record.Type != "merchandise" && record.Type != "spec") return -1;
            return await _repo.InsertWeightedEntries(record);
        }

        public async Task<List<WeightedEntryDisplayDto>> GetAllWeightedEntries()
        {
            return await _repo.GetAllWeightedEntries();
        }

        public async Task<int> UpdateEntryWeight(int id, int weight)
        {
            return await _repo.UpdateEntryWeight(id, weight);
        }

        public async Task<int> DeleteWeightedEntry(int id)
        {
            return await _repo.DeleteWeightedEntry(id);
        }
    }
}
