using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos.Recommend;

namespace MSIT147thGraduationTopic.Models.Infra.Repositories
{
    public class RecommandRepositoy
    {
        private readonly GraduationTopicContext _context;

        public RecommandRepositoy(GraduationTopicContext context)
        {
            _context = context;
        }

        public async Task<double> GetAverageEvaluationScoreOfAll()
        {
            return await _context.Evaluations.AverageAsync(o => o.Score);
        }
        public async Task<int> GetEvaluationCountOfAll()
        {
            return await _context.Evaluations.CountAsync();
        }
        public async Task<List<RecommendationSpecsDto>> GetAllSpecsWithEvaluation()
        {
            var specs = _context.Specs.Select(o => new RecommendationSpecsDto
            {
                SpecId = o.SpecId,
                MerchandiseId = o.MerchandiseId,
                EvaluateCount = o.Merchandise.Evaluations.Count,
                AverageScore = o.Merchandise.Evaluations.DefaultIfEmpty().Average(evaluation => evaluation.Score),
                PurchasedAmount = o.OrderLists.Sum(o => o.Quantity)
            });
            return await specs.ToListAsync();
        }

        public async Task<int> UpdateSpecsPopularity(IEnumerable<RecommendationSpecsDto> specs)
        {
            foreach (var specDto in specs.ToList())
            {
                var spec = await _context.Specs.FindAsync(specDto.SpecId);
                if (spec == null || specDto.Popularity == null) continue;
                spec.Popularity = specDto.Popularity.Value;
            }

            return await _context.SaveChangesAsync();
        }

    }
}
