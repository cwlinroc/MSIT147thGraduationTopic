using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
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

        //public async Task<List<CartItem>?> GetCartItemsByMeberId(int meberId)
        //{
        //    return await _repo.GetCartItemsByMeberId(meberId);
        //}






    }
}
