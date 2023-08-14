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
        private readonly IWebHostEnvironment _environment;

        public CouponService(GraduationTopicContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _repo = new CouponRepository(context);
            _environment = environment;
        }

        public IEnumerable<CouponVM> GetAllCoupons()
        {
            return _repo.GetAllCoupons().Select(dto =>
            {
                return dto.ToVM();
            });
        }

        public int CreateCoupon(CouponDto cDto)
        {
            return _repo.CreateCoupon(cDto);
        }

        public int CouponReceive(int memberId,int couponId)
        {
            return _repo.CouponReceive(memberId,couponId);
        }

        public int EditCoupon(CouponEditDto cEDto)
        {
            return _repo.EditCoupon(cEDto);
        }

        public int DeleteCoupon(int couponId)
        {
            return _repo.DeleteCoupon(couponId);
        }

        public CouponDto GetCouponById(int couponId)
        {
            return _repo.GetCouponById(couponId);
        }

        public CouponFrontDto GetCouponByMemberId(int memberId)
        {
            return _repo.GetCouponByMemberID(memberId);
        }
    }
}
