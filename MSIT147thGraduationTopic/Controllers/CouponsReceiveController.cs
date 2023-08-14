using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Infra.Repositories;
using MSIT147thGraduationTopic.Models.ViewModels;
using System.Data;
using System.Security.Claims;

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
            if (!int.TryParse(HttpContext.User.FindFirstValue("MemberId"), out int memberId))
            {
                return BadRequest();
            }

            var couponlistA = _repo.ShowReceivableCoupons(0, memberId);
            var couponlistB = _repo.ShowReceivableCoupons(1, memberId);

            return View((couponlistA, couponlistB));
        }
    }
}
