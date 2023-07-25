using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;

namespace MSIT147thGraduationTopic.Models.Infra.Repositories
{
    public class CouponRepository
    {
        private readonly GraduationTopicContext _context;
        public CouponRepository(GraduationTopicContext context)
        {
            _context = context;
        }

        public IEnumerable<CouponDto> showCoupons(int id)
        {
            var Coupons = _context.Coupons.Where(c=>c.CouponDiscountTypeId == id ).ToList();
            return Coupons.Select(c => c.ToDto());
        }
    }
}
