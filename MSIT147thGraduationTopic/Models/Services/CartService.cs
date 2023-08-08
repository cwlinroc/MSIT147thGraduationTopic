using Microsoft.EntityFrameworkCore;
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

    }
}
