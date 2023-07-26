using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.Models.Infra.Repositories;
using MSIT147thGraduationTopic.Models.ViewModels;

namespace MSIT147thGraduationTopic.Models.Services
{
    public class CouponService
    {
        private readonly GraduationTopicContext _context;
        private readonly CouponRepository _repo;

        public CouponService(GraduationTopicContext context)
        {
            _context = context;
            _repo = new CouponRepository(context);
        }

        public int CreateCoupon(CouponDto cDto)
        {
            return _repo.CraeteCoupon(cDto);
        }

        public int EditCoupon(CouponEditDto cEDto,int couponId)
        {
            return _repo.EditCoupon(cEDto, couponId);
        }

        public int DeleteCoupon(int couponId)
        {
            return _repo.DeleteCoupon(couponId);
        }
    }
}
