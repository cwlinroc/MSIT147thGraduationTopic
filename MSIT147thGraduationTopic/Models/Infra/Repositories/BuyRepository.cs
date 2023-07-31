using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;

namespace MSIT147thGraduationTopic.Models.Infra.Repositories
{
    public class BuyRepository
    {
        private readonly GraduationTopicContext _context;
        public BuyRepository(GraduationTopicContext context)
        {
            _context = context;
        }

        public IEnumerable<CartItemDisplayDto> GetCartItems(int[] cartItemIds)
        {
            return (from cartItem in _context.CartItems
                    join spec in _context.Specs on cartItem.SpecId equals spec.SpecId
                    join merchandise in _context.Merchandises on spec.MerchandiseId equals merchandise.MerchandiseId
                    where cartItemIds.Contains(cartItem.CartItemId)
                    select new CartItemDisplayDto
                    {
                        MemberId = cartItem.MemberId,
                        CartItemName = merchandise.MerchandiseName + spec.SpecName,
                        CartItemPrice = spec.Price,
                        DiscountPercentage = spec.DiscountPercentage,
                        MerchandiseImageName = merchandise.ImageUrl,
                        CartItemId = cartItem.CartItemId,
                        SpecId = cartItem.SpecId,
                        Quantity = cartItem.Quantity,
                    }).ToList();
        }


        public MemberDto? GetMemberAddressAndPhone(int memberId)
        {
            var member = _context.Members.FirstOrDefault(o => o.MemberId == memberId);
            return member?.ToDto();
        }

        public IEnumerable<CouponDto> GetAllCouponsAvalible(int memberId)
        {
            var coupons = from owner in _context.CouponOwners
                          join coupon in _context.Coupons on owner.CouponId equals coupon.CouponId
                          where owner.MemberId == memberId
                          && coupon.CouponEndDate < DateTime.Now
                          && coupon.CouponStartDate > DateTime.Now
                          select coupon;
            return coupons.Select(o => o.ToDto()).ToList();
        }

        public int GetMemberIdByCartItemId(int cartItemId)
        {
            return _context.CartItems.Where(o => o.CartItemId == cartItemId)
                .Select(o => o.MemberId).FirstOrDefault();
        }

        public IEnumerable<(SpecDto, CartItemDto)> GetCartItemsAndSpecs(int[] cartItemIds)
        {
            var cartItems = _context.CartItems.Where(o => cartItemIds.Contains(o.CartItemId))
                .Select(o => o.ToDto()).ToArray();

            var specIds = cartItems.Select(o => o.SpecId);
            var specs = _context.Specs.Where(o => specIds.Contains(o.SpecId))
                .Select(o => o.ToDto()).ToArray();

            return cartItems.Select(c => (specs.First(s => s.SpecId == c.SpecId), c));
        }

        public int CreateOrder(OrderDto dto)
        {
            var order = dto.ToEF();
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order.OrderId;
        }

        public int CreateOrderLists(IEnumerable<OrderListDto> orderlists)
        {
            _context.OrderLists.AddRange(orderlists.Select(o => o.ToEF()));
            return _context.SaveChanges();
        }

        public int ClearCartItems(int[] cartItemIds)
        {
            var cartItems = _context.CartItems.Where(o => cartItemIds.Contains(o.CartItemId));
            _context.CartItems.RemoveRange(cartItems);
            return _context.SaveChanges();
        }

    }
}
