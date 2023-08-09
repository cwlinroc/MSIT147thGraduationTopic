using Dapper;
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
                EvaluateCount = o.Evaluations.Count,
                AverageScore = o.Evaluations.DefaultIfEmpty().Average(evaluation => evaluation.Score),
                PurchasedAmount = o.OrderLists.Sum(o => o.Quantity),
                CustomRating = 0.5
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



        public async Task<List<ManuallyWeightedEntryDto>> GetAllManuallyWeightedEntries()
        {
            return await _context.ManuallyWeightedEntries.Select(o => o.ToDto()).ToListAsync();
        }


        public async Task<List<ManuallyWeightedEntryDto>> GetConvertedTagEntries(List<ManuallyWeightedEntryDto> tagEntries)
        {
            List<ManuallyWeightedEntryDto> newlist = new();

            foreach (var tagEntry in tagEntries)
            {
                int tagId = tagEntry.TagId!.Value;
                var specIds = await _context.SpecTags.Where(o => o.TagId == tagId).Select(o => o.SpecId).ToListAsync();
                var dtos = specIds.Select(o => new ManuallyWeightedEntryDto { Weight = tagEntry.Weight, SpecId = o });
                newlist.AddRange(dtos);
            }

            return newlist.GroupBy(o => o.SpecId)
                .Select(o => new ManuallyWeightedEntryDto { SpecId = o.Last().SpecId, Weight = o.Last().Weight }).ToList();
        }


        public async Task<List<ManuallyWeightedEntryDto>> GetConvertedMerchandiseEntries(List<ManuallyWeightedEntryDto> merchandiseEntries)
        {
            List<ManuallyWeightedEntryDto> newlist = new();

            foreach (var merchandiseEntry in merchandiseEntries)
            {
                int merchandiseId = merchandiseEntry.MerchandiseId!.Value;
                var specIds = await _context.Specs.Where(o => o.MerchandiseId == merchandiseId).Select(o => o.SpecId).ToListAsync();
                var dtos = specIds.Select(o => new ManuallyWeightedEntryDto { Weight = merchandiseEntry.Weight, SpecId = o });
                newlist.AddRange(dtos);
            }

            return newlist.GroupBy(o => o.SpecId)
                .Select(o => new ManuallyWeightedEntryDto { SpecId = o.Last().SpecId, Weight = o.Last().Weight }).ToList();
        }

        public async Task<RatingDataDto> GetRatingData()
        {
            return (await _context.RatingDatas.FirstAsync()).ToDto();
        }

        public async Task<int> UpdateRatingData(int number, string col)
        {
            using var conn = _context.Database.GetDbConnection();
            string sql = $"UPDATE RatingDatas SET {col} = @number";
            return await conn.ExecuteAsync(sql, new { number });
        }

        public async Task<List<string>> GetMostPopularSpecsName(int top)
        {
            return await _context.Specs.OrderBy(o => o.Popularity)
                .Select(o => o.Merchandise.MerchandiseName + o.SpecName).Take(top).ToListAsync();
        }


    }
}
