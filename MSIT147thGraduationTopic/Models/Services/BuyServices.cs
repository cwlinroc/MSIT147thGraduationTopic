using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.Models.Infra.Repositories;
using static MSIT147thGraduationTopic.Controllers.BuyController;

namespace MSIT147thGraduationTopic.Models.Services
{
    public class BuyServices
    {
        private readonly GraduationTopicContext _context;
        private readonly BuyRepository _repo;

        public BuyServices(GraduationTopicContext context)
        {
            _context = context;
            _repo = new BuyRepository(context);
        }

        public IEnumerable<CartItemDisplayDto> GetCartItems(int[] cartItemIds)
        {
            return _repo.GetCartItems(cartItemIds);
        }

        public (int, string, string) GetMemberAddressAndPhone(int cartItemId)
        {
            int memberId = _repo.GetMemberIdByCartItemId(cartItemId);

            var result = _repo.GetMemberAddressAndPhone(memberId);

            return (memberId, result.Item1.Trim(), result.Item2.Trim());
        }

        public IEnumerable<CouponDto> GetAllCouponsAvalible(int memberId)
        {
            return _repo.GetAllCouponsAvalible(memberId);
        }



        public int CreateOrder(int[] cartItemIds, int memberId, OrderRecord record)
        {
            var combined = _repo.GetCartItemsAndSpecs(cartItemIds);

            //TODO-cw coupon
            int totalPayment = combined.Select(o => o.Item1.Price * o.Item2.Quantity * o.Item1.DiscountPercentage / 100).Sum();

            var order = new OrderDto
            {
                MemberId = memberId,
                PaymentMethodId = int.Parse(record.Payment),
                Payed = true,
                PurchaseTime = DateTime.Now,
                UsedCouponId = (record.CouponId != null) ? int.Parse(record.CouponId) : null,
                PaymentAmount = totalPayment,
                DeliveryAddress = record.Address,
                ContactPhoneNumber = record.Phone,
                Remark = record.Remark
            };

            //TODO-cw Transaction

            int orderId = _repo.CreateOrder(order);

            if (orderId <= 0) return -1;

            var orderLists = combined.Select(o =>
            {
                var spec = o.Item1;
                var cartItem = o.Item2;
                var dto = new OrderListDto
                {
                    OrderId = orderId,
                    SpecId = spec.SpecId,
                    Quantity = cartItem.Quantity,
                    Price = spec.Price,
                    Discount = spec.DiscountPercentage
                };
                return dto;
            });

            int listCreated = _repo.CreateOrderLists(orderLists);
            if (listCreated <= 0) return -1;

            int cartItemsDeleted = _repo.ClearCartItems(cartItemIds);
            if (cartItemsDeleted <= 0) return -1;

            return orderId;
        }

    }
}
