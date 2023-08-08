using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
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



        public async Task<int> CalculatePopularities()
        {
            //權重
            int evaluateWeight = 7;
            int purchaseWeight = 3;

            var specs = await _repo.GetAllSpecsWithEvaluation();

            //評價 Bayesian Average
            RecommandFunctions.RateEvaluationWithBayesianAverage(specs);

            //購買數量評價 
            RecommandFunctions.RatePurchasedWithLogTransform(specs);

            //計算popularity
            RecommandFunctions.CalculatePopularity(specs, evaluateWeight, purchaseWeight);

            return await _repo.UpdateSpecsPopularity(specs);

        }















    }
}
