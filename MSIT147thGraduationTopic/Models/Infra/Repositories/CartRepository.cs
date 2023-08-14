using Humanizer;
using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;

namespace MSIT147thGraduationTopic.Models.Infra.Repositories
{
    public class CartRepository
    {
        private GraduationTopicContext _context;
        public CartRepository(GraduationTopicContext context)
        {
            _context = context;
        }

        public async Task<List<CartItemDisplayDto>?> GetCartItemsByMeberId(int meberId)
        {
            return await (from cartItem in _context.CartItems
                          join spec in _context.Specs on cartItem.SpecId equals spec.SpecId
                          join merchandise in _context.Merchandises on spec.MerchandiseId equals merchandise.MerchandiseId
                          where cartItem.MemberId == meberId
                          select new CartItemDisplayDto
                          {
                              MemberId = meberId,
                              CartItemName = merchandise.MerchandiseName + spec.SpecName,
                              CartItemPrice = spec.Price * spec.DiscountPercentage / 100,
                              MerchandiseImageName = merchandise.ImageUrl,
                              CartItemId = cartItem.CartItemId,
                              SpecId = cartItem.SpecId,
                              Quantity = cartItem.Quantity,
                          }).ToListAsync();
        }

        public async Task ChangeCartItemQuantity(CartItemDto dto)
        {

            var cartItem = await _context.CartItems.FirstOrDefaultAsync(o => o.CartItemId == dto.CartItemId);

            if (cartItem == null) return;

            cartItem.Quantity = dto.Quantity;

            await _context.SaveChangesAsync();
        }


        public async Task DeleteCartItem(int cartItemId)
        {
            var cartItem = await _context.CartItems.FirstOrDefaultAsync(o => o.CartItemId == cartItemId);

            if (cartItem == null) return;

            _context.CartItems.Remove(cartItem);

            await _context.SaveChangesAsync();
        }


        public async Task<int> GetCartCount(int memberId)
        {
            return await _context.CartItems.CountAsync(o => o.MemberId == memberId);
        }

        public async Task<List<CartItemDto>> GetCartItems(int[] cartItemIds)
        {
            return await _context.CartItems.Where(o => cartItemIds.Contains(o.CartItemId))
                .Select(o => o.ToDto()).ToListAsync();
        }

        public async Task<string?> CheckStockQuantity(IEnumerable<CartItemDto> cartItems)
        {
            foreach (var cartItem in cartItems)
            {
                var spec = await _context.Specs.FirstOrDefaultAsync(o => o.SpecId == cartItem.SpecId && o.Amount < cartItem.Quantity);
                if (spec == null) continue;
                var name = (await _context.Merchandises.FindAsync(spec.MerchandiseId))!.MerchandiseName + spec.SpecName;
                return $"{name} 的庫存數量僅剩下{spec.Amount}個";
            }
            return null;
        }

    }
}
