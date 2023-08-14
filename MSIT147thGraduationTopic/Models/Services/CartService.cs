using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.Models.Infra.Repositories;

namespace MSIT147thGraduationTopic.Models.Services
{
    public class CartService
    {
        private readonly GraduationTopicContext _context;
        private readonly CartRepository _repo;

        public CartService(GraduationTopicContext context)
        {
            _context = context;
            _repo = new CartRepository(context);
        }

        public async Task<List<CartItemDisplayDto>?> GetCartItemsByMeberId(int meberId)
        {
            return await _repo.GetCartItemsByMeberId(meberId);
        }

        public async Task ChangeCartItemQuantity(CartItemDto dto)
        {
            if (dto.Quantity < 0) dto.Quantity = 0;
            await _repo.ChangeCartItemQuantity(dto);
        }

        public async Task DeleteCartItem(int cartItemId)
        {
            await _repo.DeleteCartItem(cartItemId);
        }

        public async Task<int> GetCartCount(int memberId)
        {
            return await _repo.GetCartCount(memberId);
        }

        public async Task<(bool enough, string message)> CheckStockQuantity(int[]? cartItemIds)
        {
            if (cartItemIds.IsNullOrEmpty()) return (false, "沒有查詢的商品");

            var cartItems = await _repo.GetCartItems(cartItemIds!);
            var message = await _repo.CheckStockQuantity(cartItems);

            return (message.IsNullOrEmpty(), message ?? string.Empty);
        }


    }
}
