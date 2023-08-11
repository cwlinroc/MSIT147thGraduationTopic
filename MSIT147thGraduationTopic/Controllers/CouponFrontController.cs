using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Infra.Repositories;

namespace MSIT147thGraduationTopic.Controllers
{
    public class CouponFrontController : Controller
    {
        private readonly GraduationTopicContext _context;
        private readonly CouponRepository _repo;
        public CouponFrontController(GraduationTopicContext context)
        {
            _context = context;
            _repo = new CouponRepository(context);
        }

        public IActionResult CouponList()
        {
            var couponlistA = _repo.ShowCouponsFront(0);
            var couponlistB = _repo.ShowCouponsFront(1);

            return View((couponlistA, couponlistB));
        }

        [HttpPost]
        public IActionResult CouponList(Member member)
        {
            return View();
        }
    }
}
