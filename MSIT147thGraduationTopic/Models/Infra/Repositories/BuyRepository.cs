using Dapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.Models.ViewModels;
using System.Transactions;

namespace MSIT147thGraduationTopic.Models.Infra.Repositories
{
    public class BuyRepository
    {
        private readonly GraduationTopicContext _context;
        public BuyRepository(GraduationTopicContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CartItemDisplayDto>> GetCartItems(int[] cartItemIds)
        {
            var result = await (from cartItem in _context.CartItems
                                join spec in _context.Specs on cartItem.SpecId equals spec.SpecId
                                join merchandise in _context.Merchandises on spec.MerchandiseId equals merchandise.MerchandiseId
                                join specTags in _context.SpecTags on spec.SpecId equals specTags.SpecId into tags
                                from specTags in tags.DefaultIfEmpty()
                                where cartItemIds.Contains(cartItem.CartItemId)
                                select new
                                {
                                    cartItem.MemberId,
                                    merchandise.MerchandiseName,
                                    spec.SpecName,
                                    spec.Price,
                                    spec.DiscountPercentage,
                                    merchandise.ImageUrl,
                                    cartItem.CartItemId,
                                    spec.SpecId,
                                    cartItem.Quantity,
                                    TagId = (int?)specTags.TagId,
                                }).ToListAsync();
            return result.GroupBy(o => o.CartItemId).Select(o => new CartItemDisplayDto
            {
                MemberId = o.First().MemberId,
                CartItemName = o.First().MerchandiseName + o.First().SpecName,
                CartItemPrice = o.First().Price,
                DiscountPercentage = o.First().DiscountPercentage,
                MerchandiseImageName = o.First().ImageUrl,
                CartItemId = o.First().CartItemId,
                SpecId = o.First().SpecId,
                Quantity = o.First().Quantity,
                Tags = o.Where(t => t.TagId != null).Select(t => t.TagId.Value).ToArray(),
            });
        }

        public MemberDto? GetMemberData(int memberId)
        {
            var member = _context.Members.FirstOrDefault(o => o.MemberId == memberId);
            return member?.ToDto();
        }

        public async Task<IEnumerable<(int CouponId, string CouponName)>> GetAllCouponsAvalible(int memberId)
        {
            var coupons = await (from owner in _context.CouponOwners
                                 join coupon in _context.Coupons on owner.CouponId equals coupon.CouponId
                                 where owner.MemberId == memberId
                                 && coupon.CouponEndDate > DateTime.Now
                                 && coupon.CouponStartDate < DateTime.Now
                                 select new { coupon.CouponId, coupon.CouponName }).ToListAsync();
            return coupons.Select(o => (o.CouponId, o.CouponName));
        }

        public async Task<CouponDto?> GetCouponById(int? couponId)
        {
            if (couponId == null) return null;
            return (await _context.Coupons.FindAsync(couponId))?.ToDto();
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

        public async Task<List<CartItemCheckoutDto>> GetCheckoutInformation(int[] cartItemIds)
        {
            var cartItems = await _context.CartItems.Where(o => cartItemIds.Contains(o.CartItemId)).Select(o => new CartItemCheckoutDto
            {
                CartItemId = o.CartItemId,
                SpecId = o.SpecId,
                MerchandiseId = o.Spec.MerchandiseId,
                ItemName = o.Spec.Merchandise.MerchandiseName + o.Spec.SpecName,
                DiscountPercentage = o.Spec.DiscountPercentage,
                Price = o.Spec.Price,
                Quantity = o.Quantity,
                Amount = o.Spec.Amount,
                OnShelf = o.Spec.OnShelf
            }).ToListAsync();

            foreach (var cartItem in cartItems)
            {
                cartItem.TagIds = await _context.SpecTags.Where(o => o.SpecId == cartItem.SpecId)
                    .Select(o => o.TagId).ToListAsync();
            }
            return cartItems;
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

        public async Task<int> ReduceSpecStorage(IEnumerable<OrderListDto> orderlists)
        {
            foreach (var item in orderlists)
            {
                var spec = await _context.Specs.FindAsync(item.SpecId);
                if (spec == null) continue;

                spec.Amount -= item.Quantity;
                spec.Amount = Math.Max(0, spec.Amount);
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<int> MakeCouponUsed(int memberId, int couponId)
        {
            return await _context.Database
                .ExecuteSqlAsync($"UPDATE CouponOwners SET CouponUsed = 1  WHERE CouponId = {couponId} AND MemberId = {memberId}");
        }


        public async Task<int> ConfirmOrder(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return -1;
            order.Payed = true;
            return await _context.SaveChangesAsync();
        }
    }
}
