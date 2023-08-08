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

    }
}
