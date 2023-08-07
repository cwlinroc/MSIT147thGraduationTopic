using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;

namespace MSIT147thGraduationTopic.Models.Infra.Repositories
{
    public class HomeRepository
    {

        private readonly GraduationTopicContext _context;
        public HomeRepository(GraduationTopicContext context)
        {
            _context = context;
        }

        public IEnumerable<RecommandSpecDisplayDto> GetMostPopularSpecs(int top = 20)
        {
            if (top <= 0) return new List<RecommandSpecDisplayDto>();

            var specs = (from spec in _context.Specs
                         join merchandise in _context.Merchandises on spec.MerchandiseId equals merchandise.MerchandiseId
                         orderby spec.Popularity
                         select new RecommandSpecDisplayDto
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
                    .Where(o => o.MerchandiseId == spec.MerchandiseId).ToList();
                spec.EvaluationScore = !evaluations.IsNullOrEmpty() ? evaluations.Average(o => o.Score) : 4.0;
            }
            return specs;
        }
        public IEnumerable<RecommandSpecDisplayDto> GetMostFavorableSpecs(int top = 20)
        {
            if (top <= 0) return new List<RecommandSpecDisplayDto>();

            var specs = (from spec in _context.Specs
                         join merchandise in _context.Merchandises on spec.MerchandiseId equals merchandise.MerchandiseId
                         join evaluation in _context.Evaluations on spec.MerchandiseId equals evaluation.MerchandiseId
                         group new { spec, merchandise, evaluation } by merchandise.MerchandiseId into g
                         orderby g.Average(o => o.evaluation.Score) descending
                         where g.Count() > 2
                         select new RecommandSpecDisplayDto
                         {
                             SpecId = g.First().spec.SpecId,
                             MerchandiseId = g.First().spec.MerchandiseId,
                             MerchandiseName = g.First().merchandise.MerchandiseName + g.First().spec.SpecName,
                             SpecImageName = g.First().spec.ImageUrl,
                             MerchandiseImageName = g.First().merchandise.ImageUrl,
                             Price = g.First().spec.Price,
                             DiscountPercentage = g.First().spec.DiscountPercentage,
                             EvaluationScore = g.Average(o => o.evaluation.Score)
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
        public IEnumerable<RecommandSpecDisplayDto> GetNewestSpecs(int top = 50)
        {
            if (top <= 0) return new List<RecommandSpecDisplayDto>();

            var specs = (from spec in _context.Specs
                         join merchandise in _context.Merchandises on spec.MerchandiseId equals merchandise.MerchandiseId
                         orderby spec.SpecId descending
                         select new RecommandSpecDisplayDto
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
                    .Where(o => o.MerchandiseId == spec.MerchandiseId).ToList();
                spec.EvaluationScore = !evaluations.IsNullOrEmpty() ? evaluations.Average(o => o.Score) : 4.0;
            }
            return specs;
        }

    }
}
