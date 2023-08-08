using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.Models.Dtos.Recommend;
using MSIT147thGraduationTopic.Models.Infra.Repositories;

namespace MSIT147thGraduationTopic.Models.Services
{
    public class RecommandService
    {
        private readonly GraduationTopicContext _context;
        private readonly RecommandRepositoy _repo;

        public RecommandService(GraduationTopicContext context)
        {
            _context = context;
            _repo = new(context);
        }



        public async Task<int> CalculatePopularities(RecommendCalculateBo bo)
        {
            var specs = await _repo.GetAllSpecsWithEvaluation();

            //顧客評價轉換分數
            bo.RateEvaluation?.Invoke(specs);
            //購買數量轉換分數
            bo.RatePurchased?.Invoke(specs);

            //依權重計算popularity
            RecommandFunctions.CalculatePopularity(specs, bo.EvaluationWeight, bo.PurchasedWeight);

            return await _repo.UpdateSpecsPopularity(specs);
        }


    }
}
