using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;

namespace MSIT147thGraduationTopic.Models.Infra.Repositories
{
    public class ShoppingHistoryRepository
    {
        private readonly GraduationTopicContext _context;

        public ShoppingHistoryRepository(GraduationTopicContext context)
        {
            _context = context;
        }

        public IEnumerable<ShoppingHistoryDto> GetOrderByMemberId(int memberId)
        {
            var order = _context.OrderWithMembers
                .Where(o => o.MemberId == memberId);
            return order.Select(o => o.ToDto());
        }
    }
}
