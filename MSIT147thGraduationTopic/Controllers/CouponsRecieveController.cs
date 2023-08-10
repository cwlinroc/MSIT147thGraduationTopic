using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Infra.Repositories;

namespace MSIT147thGraduationTopic.Controllers
{
    public class CouponsRecieveController : Controller
    {
        private readonly GraduationTopicContext _context;
        private readonly CouponRepository _repo;
        public CouponsRecieveController(GraduationTopicContext context)
        {
            _context = context;
            _repo = new CouponRepository(context);
        }

        public IActionResult Index()
        {
            var couponlistA = _repo.ShowCoupons(0);
            var couponlistB = _repo.ShowCoupons(1);

            return View((couponlistA, couponlistB));
        }
    }
}
