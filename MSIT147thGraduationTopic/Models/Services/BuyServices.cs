using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.Models.Infra.Repositories;

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


    }
}
