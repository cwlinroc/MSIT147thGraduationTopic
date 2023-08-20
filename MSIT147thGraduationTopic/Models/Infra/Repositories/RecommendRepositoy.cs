using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos.Recommend;
using NuGet.Protocol;
using static MSIT147thGraduationTopic.Controllers.Recommend.ApiRecommendController;

namespace MSIT147thGraduationTopic.Models.Infra.Repositories
{
    public class RecommendRepositoy
    {
        private readonly GraduationTopicContext _context;

        public RecommendRepositoy(GraduationTopicContext context)
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
        public async Task<List<RecommendationSpecsDto>> GetAllSpecsWithEvaluation(RatingDataDto ratingData)
        {
            bool getRecentEvaluation = ratingData.RecentEvaluationTimes.HasValue 
                && ratingData.RecentEvaluationDays.HasValue && ratingData.RecentEvaluationTimes !=0;
            bool getRecentPurchased = ratingData.RecentPurchasedTimes.HasValue 
                && ratingData.RecentPurchasedDays.HasValue && ratingData.RecentPurchasedTimes != 0;
            DateTime? evaluationMinDate = ratingData.RecentEvaluationDays.HasValue ?
                DateTime.Now.AddDays((double)-ratingData.RecentEvaluationDays) : null;
            DateTime? purchasedMinDate = ratingData.RecentPurchasedDays.HasValue ?
                DateTime.Now.AddDays((double)-ratingData.RecentPurchasedDays) : null;

            var specs = _context.Specs.Select(o => new RecommendationSpecsDto
            {
                SpecId = o.SpecId,
                MerchandiseId = o.MerchandiseId,
                EvaluateCount = o.Evaluations.Count,
                AverageScore = (o.Evaluations.Any()) ? o.Evaluations.Average(ev => ev.Score) : 0,
                PurchasedAmount = o.OrderLists.Sum(o => o.Quantity),
                CustomRating = 0.5,
                RecentAverageScore = getRecentEvaluation ?
                    (o.Evaluations.Where(x => x.Order.PurchaseTime > evaluationMinDate).Any()) ?
                    o.Evaluations.Where(x => x.Order.PurchaseTime > evaluationMinDate).Average(ev => ev.Score)
                    : 0 : null,
                RecentEvaluateCount = getRecentEvaluation ?
                    o.Evaluations.Where(x => x.Order.PurchaseTime > evaluationMinDate).Count() : null,
                RecentPurchasedAmount = getRecentPurchased ?
                    o.OrderLists.Where(x => x.Order.PurchaseTime > purchasedMinDate).Sum(o => o.Quantity) : null,
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
            return await _context.Specs.OrderByDescending(o => o.Popularity)
                .Select(o => o.Merchandise.MerchandiseName + o.SpecName).Take(top).ToListAsync();
        }

        public async Task<List<SearchedItemsDto>> GetSearchedItems(string text, string type)
        {
            var items = type switch
            {
                "tag" => _context.Tags.Where(o => o.TagName.Contains(text))
                    .Select(o => new SearchedItemsDto { Id = o.TagId, Name = o.TagName }).Take(100),
                "merchandise" => _context.Merchandises.Where(o => o.MerchandiseName.Contains(text))
                    .Select(o => new SearchedItemsDto { Id = o.MerchandiseId, Name = o.MerchandiseName }).Take(100),
                "spec" => _context.Specs.Where(o => (o.Merchandise.MerchandiseName + o.SpecName).Contains(text))
                    .Select(o => new SearchedItemsDto { Id = o.SpecId, Name = o.Merchandise.MerchandiseName + o.SpecName }).Take(100),
                _ => null
            };
            if (items == null) return new();
            return await items.ToListAsync();
        }


        public async Task<int> InsertWeightedEntries(InsertEntriesRecord record)
        {
            var entries = record.Type switch
            {
                "tag" => record.Ids.Select(o => new ManuallyWeightedEntry() { TagId = o, Weight = record.Weight }),
                "merchandise" => record.Ids.Select(o => new ManuallyWeightedEntry() { MerchandiseId = o, Weight = record.Weight }),
                "spec" => record.Ids.Select(o => new ManuallyWeightedEntry() { SpecId = o, Weight = record.Weight }),
                _ => null
            };
            if (entries == null) return -1;
            _context.ManuallyWeightedEntries.AddRange(entries);
            return await _context.SaveChangesAsync();
        }


        public async Task<List<WeightedEntryDisplayDto>> GetAllWeightedEntries()
        {
            var entries = await _context.ManuallyWeightedEntries
                .Select(o => new { entry = o, name = o.Tag.TagName + o.Merchandise.MerchandiseName + o.Spec.Merchandise.MerchandiseName + o.Spec.SpecName })
                .ToListAsync();
            return entries.Select(o =>
            {
                string type = ((o.entry.TagId != null) ? "tag" : "")
                + ((o.entry.MerchandiseId != null) ? "merchandise" : "")
                + ((o.entry.SpecId != null) ? "spec" : "");
                return new WeightedEntryDisplayDto
                {
                    Id = o.entry.EntryId,
                    Name = o.name,
                    Type = type,
                    Weight = o.entry.Weight
                };
            }).ToList();
        }

        public async Task<int> UpdateEntryWeight(int id, int weight)
        {
            var entry = await _context.ManuallyWeightedEntries.FindAsync(id);
            if (entry == null) return -1;
            entry.Weight = weight;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteWeightedEntry(int id)
        {
            var entry = await _context.ManuallyWeightedEntries.FindAsync(id);
            if (entry == null) return -1;
            _context.ManuallyWeightedEntries.Remove(entry);
            return await _context.SaveChangesAsync();
        }


    }
}
