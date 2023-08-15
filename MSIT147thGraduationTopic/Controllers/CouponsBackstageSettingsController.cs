using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Infra.Repositories;
using System.Data;

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

        [Authorize(Roles = "管理員,經理,員工")]
        public IActionResult Index()
        {
            var couponlistA = _repo.ShowCoupons(0);
            var couponlistB = _repo.ShowCoupons(1);
            
            return View((couponlistA, couponlistB));
        }


        [Authorize(Roles = "管理員,經理,員工")]
        public IActionResult DiscountCreate()
        {
            return View();
        }


        [Authorize(Roles = "管理員,經理,員工")]
        public IActionResult RebateCreate()
        {
            return View();
        }


        [Authorize(Roles = "管理員,經理,員工")]
        public IActionResult DiscountEdit(int id)
        {
            var couponData = _repo.GetCouponById(id);
            return View(couponData);
        }


        [Authorize(Roles = "管理員,經理,員工")]
        public IActionResult RebateEdit(int id)
        {
            var couponData = _repo.GetCouponById(id);
            return View(couponData);
        }
    }
}
