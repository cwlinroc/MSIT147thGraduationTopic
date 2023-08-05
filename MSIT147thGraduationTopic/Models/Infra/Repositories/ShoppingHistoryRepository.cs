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

        public IEnumerable<ShoppingHistoryDto> GetOrdersByMemberId(int memberId)
        {
            var order = _context.OrderWithMembers
                .Where(o => o.MemberId == memberId);
            return order.Select(o => o.ToShDto());
        }

        public IEnumerable<SpecInOrder> GetSpecsByOrderId(int orderId)
        {
            var list = _context.OrderWithMembers
                .Where(o => o.OrderId == orderId);
            return (IEnumerable<SpecInOrder>)list.Select(o => o.ToShDto());
        }
    }
}
