using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos.Recommend;

namespace MSIT147thGraduationTopic.Models.Infra.Repositories
{
    public class HomeRepository
    {

        private readonly GraduationTopicContext _context;
        public HomeRepository(GraduationTopicContext context)
        {
            _context = context;
        }

        public IEnumerable<RecommendSpecDisplayDto> GetMostPopularSpecs(int top = 20)
        {
            if (top <= 0) return new List<RecommendSpecDisplayDto>();

            var specs = (from spec in _context.Specs
                         join merchandise in _context.Merchandises on spec.MerchandiseId equals merchandise.MerchandiseId
                         orderby spec.Popularity descending
                         select new RecommendSpecDisplayDto
                         {
                             SpecId = spec.SpecId,
                             MerchandiseId = spec.MerchandiseId,
                             MerchandiseName = merchandise.MerchandiseName + spec.SpecName,
                             SpecImageName = spec.ImageUrl,
                             MerchandiseImageName = merchandise.ImageUrl,
                             Price = spec.Price,
                             DiscountPercentage = spec.DiscountPercentage
                         }).Take(top).ToList();
            foreach (var spec in specs)
            {
                var tags = from specTag in _context.SpecTags
                           join tag in _context.Tags on specTag.TagId equals tag.TagId
                           where specTag.SpecId == spec.SpecId
                           select tag.TagName;
                spec.Tags = tags.ToList();
                var evaluations = _context.Evaluations
                    .Where(o => o.SpecId == spec.SpecId).ToList();
                spec.EvaluationScore = !evaluations.IsNullOrEmpty() ? evaluations.Average(o => o.Score) : 4.0;
            }
            return specs;
        }
        public IEnumerable<RecommendSpecDisplayDto> GetMostFavorableSpecs(int top = 20)
        {
            if (top <= 0) return new List<RecommendSpecDisplayDto>();

            var specs = _context.Specs.OrderByDescending(o => (o.Evaluations.Any()) ? o.Evaluations.Average(ev => ev.Score) : 0)
                .Select(o => new RecommendSpecDisplayDto
                {
                    SpecId = o.SpecId,
                    MerchandiseId = o.MerchandiseId,
                    MerchandiseName = o.Merchandise.MerchandiseName + o.SpecName,
                    SpecImageName = o.ImageUrl,
                    MerchandiseImageName = o.Merchandise.ImageUrl,
                    Price = o.Price,
                    DiscountPercentage = o.DiscountPercentage,
                    EvaluationScore = (o.Evaluations.Any()) ? o.Evaluations.Average(ev => ev.Score) : 0
                }).Take(top).ToList();

            foreach (var spec in specs)
            {
                var tags = from specTag in _context.SpecTags
                           join tag in _context.Tags on specTag.TagId equals tag.TagId
                           where specTag.SpecId == spec.SpecId
                           select tag.TagName;
                spec.Tags = tags.ToList();
            }
            return specs;
        }
        public IEnumerable<RecommendSpecDisplayDto> GetNewestSpecs(int top = 50)
        {
            if (top <= 0) return new List<RecommendSpecDisplayDto>();

            var specs = (from spec in _context.Specs
                         join merchandise in _context.Merchandises on spec.MerchandiseId equals merchandise.MerchandiseId
                         orderby spec.SpecId descending
                         select new RecommendSpecDisplayDto
                         {
                             SpecId = spec.SpecId,
                             MerchandiseId = spec.MerchandiseId,
                             MerchandiseName = merchandise.MerchandiseName + spec.SpecName,
                             SpecImageName = spec.ImageUrl,
                             MerchandiseImageName = merchandise.ImageUrl,
                             Price = spec.Price,
                             DiscountPercentage = spec.DiscountPercentage,
                         }).Take(top).ToList();
            foreach (var spec in specs)
            {
                var tags = from specTag in _context.SpecTags
                           join tag in _context.Tags on specTag.TagId equals tag.TagId
                           where specTag.SpecId == spec.SpecId
                           select tag.TagName;
                spec.Tags = tags.ToList();
                var evaluations = _context.Evaluations
                    .Where(o => o.SpecId == spec.SpecId).ToList();
                spec.EvaluationScore = !evaluations.IsNullOrEmpty() ? evaluations.Average(o => o.Score) : 4.0;
            }
            return specs;
        }
                
    }
}
