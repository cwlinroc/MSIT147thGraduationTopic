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
            var cart = await _context.Carts.FirstOrDefaultAsync(o => o.MemberId == meberId);
            if (cart == null) return null;

            return await (from cartItem in _context.CartItems
                          join spec in _context.Specs on cartItem.SpecId equals spec.SpecId
                          join merchandise in _context.Merchandises on spec.MerchandiseId equals merchandise.MerchandiseId
                          where cartItem.CartId == cart.CartId
                          select new CartItemDisplayDto
                          {
                              CartId = cart.CartId,
                              CartItemName = merchandise.MerchandiseName + spec.SpecName,
                              CartItemPrice = spec.Price,
                              MerchandiseImageName = merchandise.ImageUrl,
                              CartItemId = cartItem.CartItemId,
                              SpecId = cartItem.SpecId,
                              Quantity = cartItem.Quantity,
                          }).ToListAsync();
        }






    }
}
