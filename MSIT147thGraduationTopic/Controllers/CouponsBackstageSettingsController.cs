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
            var couponlistA = _repo.ShowCoupons(0);
            var couponlistB = _repo.ShowCoupons(1);
            
            return View((couponlistA, couponlistB));
        }

        public IActionResult DiscountCreate()
        {
            return View();
        }

        public IActionResult RebateCreate()
        {
            return View();
        }

        public IActionResult DiscountEdit(int id)
        {
            var couponData = _repo.GetCouponById(id);
            return View(couponData);
        }

        public IActionResult RebateEdit(int id)
        {
            var couponData = _repo.GetCouponById(id);
            return View(couponData);
        }
    }
}
