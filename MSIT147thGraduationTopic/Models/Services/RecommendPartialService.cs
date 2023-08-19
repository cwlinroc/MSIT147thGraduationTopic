using MessagePack.Formatters;
using Microsoft.IdentityModel.Tokens;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos.Recommend;
using MSIT147thGraduationTopic.Models.Infra.Repositories;
using MSIT147thGraduationTopic.Models.Infra.Utility;
using System.Security.Claims;

namespace MSIT147thGraduationTopic.Models.Services
{
    public class RecommendPartialService
    {

        private readonly GraduationTopicContext _context;
        private readonly RecommendPartialRepository _repo;
        private readonly IHttpContextAccessor _accessor;

        public RecommendPartialService(GraduationTopicContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _repo = new(context);
            _accessor = accessor;

        }

        public async Task<List<SpecDisplyDto>> GetFavorSpecs(int? merchandiseId)
        {
            var generator = new RandomGenerator();

            int[] visitedMerchandiseIds = GetVisitedMerchandiseIds(merchandiseId);
            var visitedTagIds = await _repo.GetVisitedTagIds(visitedMerchandiseIds);
            
            bool logIn = int.TryParse(_accessor.HttpContext!.User.FindFirstValue("MemberId"), out int parseNumber);
            int? memberId = logIn ? parseNumber : null;
            var inCartTagIds = await _repo.GetInCartTagIds(memberId);
            inCartTagIds = inCartTagIds.Where(o => o > 4).ToArray();
            int[] conflictSpecIds = await _repo.GetMerchandiseSpecIds(merchandiseId);

            if (visitedTagIds.IsNullOrEmpty() && inCartTagIds.IsNullOrEmpty())
            {
                var specIds = await _repo.GetRecommendSpecIds(null, conflictSpecIds, 40);
                specIds = generator.RandomCollectionFrom(specIds, 8).ToList();
                return await _repo.GetSpecDisplayDtos(specIds);
            }

            var recommendByVisitSpecIds = !visitedTagIds.IsNullOrEmpty() ?
                await _repo.GetRecommendSpecIds(visitedTagIds, conflictSpecIds) : new();
            var recommendByCartSpecIds = !inCartTagIds.IsNullOrEmpty() ?
                await _repo.GetRecommendSpecIds(inCartTagIds, recommendByVisitSpecIds.Concat(conflictSpecIds)) : new();

            var recSpecIds = recommendByVisitSpecIds
                .Concat(recommendByCartSpecIds.Distinct().OrderBy(o=>Guid.NewGuid()).Take(8));
            var choosendIds = generator.RandomCollectionFrom(recSpecIds, 8);

            while (choosendIds.Distinct().Count() < 8)
            {
                choosendIds = choosendIds.Distinct().Append(generator.RandomFrom(recSpecIds));
            }

            return await _repo.GetSpecDisplayDtos(choosendIds);
        }

        private int[] GetVisitedMerchandiseIds(int? merchandiseId)
        {
            int? last1 = _accessor.HttpContext!.Session.GetInt32("Last_1");
            int? last2 = _accessor.HttpContext.Session.GetInt32("Last_2");
            //int? last3 = _accessor.HttpContext.Session.GetInt32("Last_3");
            int? last3 = null;
            return new int?[] { last1, last2, last3, merchandiseId }
                .Where(o => o != null).Select(o => o!.Value).ToArray();
        }


        public async Task<List<SpecDisplyDto>> GetPopularSpecs(int? merchandiseId)
        {
            var generator = new RandomGenerator();

            var specIds = await _repo.GetRecommendSpecIds(null, null, 40);
            specIds = generator.RandomCollectionFrom(specIds, 8).ToList();
            return await _repo.GetSpecDisplayDtos(specIds);

        }


    }
}
