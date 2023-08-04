using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.Models.Infra.Repositories;

namespace MSIT147thGraduationTopic.Models.Services
{
    public class ShoppingHistoryService
    {
        private readonly GraduationTopicContext _context;
        private readonly ShoppingHistoryRepository _shrepo;
        private readonly IWebHostEnvironment _environment;

        public ShoppingHistoryService(GraduationTopicContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _shrepo = new ShoppingHistoryRepository(context);
        }

        public IEnumerable<ShoppingHistoryDto> GetOrdersByMemberId(int memberId)
        {
            //抓該會員所有order
            var orders = _shrepo.GetOrdersByMemberId(memberId).ToList();

            //抓訂單裡的商品內容
            foreach (var order in orders)
            {
                var orderlists = _shrepo.GetSpecsByOrderId(order.OrderId);

                order.ListOfSpecs = orderlists.ToList();
            };

            return orders;
        }
    }
}
