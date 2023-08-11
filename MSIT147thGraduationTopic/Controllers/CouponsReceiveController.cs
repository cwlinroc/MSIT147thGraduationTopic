using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Infra.Repositories;
using System.Data;

namespace MSIT147thGraduationTopic.Controllers
{
    public class CouponsReceiveController : Controller
    {
        private readonly GraduationTopicContext _context;
        private readonly CouponRepository _repo;
        public CouponsReceiveController(GraduationTopicContext context)
        {
            _context = context;
            _repo = new CouponRepository(context);
        }

        [Authorize(Roles = "會員")]
        public IActionResult Index()
        {
            
            var couponlistA = _repo.ShowCoupons(0);
            var couponlistB = _repo.ShowCoupons(1);

            return View((couponlistA, couponlistB));
        }
    }
}
