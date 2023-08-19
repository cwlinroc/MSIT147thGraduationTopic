using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos.Recommend;

namespace MSIT147thGraduationTopic.Models.Infra.Repositories
{
    public class RecommendPartialRepository
    {

        private readonly GraduationTopicContext _context;

        public RecommendPartialRepository(GraduationTopicContext context)
        {
            _context = context;
        }

        public async Task<int[]> GetVisitedTagIds(int[] visitedMerchandiseIds)
        {
            if (visitedMerchandiseIds.IsNullOrEmpty()) return Array.Empty<int>();

            var specIds = await _context.Specs.Where(o => visitedMerchandiseIds
                .Contains(o.MerchandiseId)).Select(o => o.SpecId).ToListAsync();

            return await _context.SpecTags.Where(o => specIds.Contains(o.SpecId))
                .Select(o => o.TagId).Distinct().ToArrayAsync();

        }

        public async Task<int[]> GetInCartTagIds(int? memberId)
        {
            if (!memberId.HasValue) return Array.Empty<int>();
            var specIds = await _context.CartItems.Where(o => o.MemberId == memberId)
                .Select(o => o.SpecId).ToListAsync();

            return await _context.SpecTags.Where(o => specIds.Contains(o.SpecId))
                .Select(o => o.TagId).Distinct().ToArrayAsync();
        }

        public async Task<int[]> GetMerchandiseSpecIds(int? merchandiseId)
        {
            if (!merchandiseId.HasValue) return Array.Empty<int>();

            return await _context.Specs.Where(o => o.MerchandiseId == merchandiseId)
                .Select(o => o.SpecId).ToArrayAsync();
        }

        public async Task<List<int>> GetRecommendSpecIds(IEnumerable<int>? tagIds, IEnumerable<int>? collisionSpecIds = null, int amount = 20)
        {
            var specs = (tagIds.IsNullOrEmpty()) ? _context.SpecTags.OrderBy(x => Guid.NewGuid()).Take(amount) : tagIds!
                .Select(tagId => _context.SpecTags.Where(x => x.TagId == tagId).OrderBy(x => Guid.NewGuid()).Take(10))
                .Aggregate((accum, next) => accum.Concat(next));
            
            if (!collisionSpecIds.IsNullOrEmpty())
            {
                specs = specs.Where(o => !collisionSpecIds!.Contains(o.SpecId));
            }
            return await specs.Select(o => o.SpecId).ToListAsync();
        }


        public async Task<List<SpecDisplyDto>> GetSpecDisplayDtos(IEnumerable<int> specIds)
        {
            return await _context.Specs.Where(o => specIds.Contains(o.SpecId)).Select(o => new SpecDisplyDto
            {
                SpecId = o.SpecId,
                MerchandiseId = o.MerchandiseId,
                Name = o.Merchandise.MerchandiseName + o.SpecName,
                Price = o.Price,
                DiscountPercentage = o.DiscountPercentage,
                Score = o.Evaluations.Any() ? o.Evaluations.Average(o => o.Score) : 0,
                SpecImageName = o.ImageUrl,
                MerchandiseImageName = o.Merchandise.ImageUrl,
            }).ToListAsync();
        }


    }
}
