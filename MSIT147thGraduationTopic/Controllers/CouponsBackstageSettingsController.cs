using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Infra.Repositories;

namespace MSIT147thGraduationTopic.Controllers
{
    public class CouponsBackstageSettingsController : Controller
    {
        private readonly GraduationTopicContext _context;
        private readonly CouponRepository _repo;
        public CouponsBackstageSettingsController(GraduationTopicContext context)
        {
            _context = context;
            _repo = new CouponRepository(context);


        }
        public IActionResult Index()
        {
            var couponlistA = _repo.showCoupons(0);
            var couponlistB = _repo.showCoupons(1);
            
            return View((couponlistA, couponlistB));
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }
    }
}
