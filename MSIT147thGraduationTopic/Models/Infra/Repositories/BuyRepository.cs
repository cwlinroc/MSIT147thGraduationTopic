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
                        CartId = cartItem.CartId,
                        CartItemName = merchandise.MerchandiseName + spec.SpecName,
                        CartItemPrice = spec.Price * spec.DiscountPercentage / 100,
                        MerchandiseImageName = merchandise.ImageUrl,
                        CartItemId = cartItem.CartItemId,
                        SpecId = cartItem.SpecId,
                        Quantity = cartItem.Quantity,
                    }).ToList();
        }

        public int GetMemberIdByCartItemId(int cartItemId)
        {
            var id = (from cartItem in _context.CartItems
                      join cart in _context.Carts on cartItem.CartId equals cart.CartId
                      where cartItem.CartItemId == cartItemId
                      select cart.MemberId).FirstOrDefault();
            return id;
        }

        public (string, string) GetMemberAddressAndPhone(int memberId)
        {
            var result = _context.Members.Select(m => new Tuple<string, string>(m.Address, m.Phone)).FirstOrDefault();
            if (result == null) return ("", "");
            return (result.Item1, result.Item2);
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


    }
}
