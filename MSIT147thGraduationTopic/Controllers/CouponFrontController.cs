using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Dtos;
using MSIT147thGraduationTopic.Models.Infra.Repositories;
using System.Security.Claims;

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
            if (!int.TryParse(HttpContext.User.FindFirstValue("MemberId"), out int memberId))
            {
                return BadRequest();
            }
            var couponData = _repo.GetReceivableCoupon(memberId);

            return View(couponData);
        }
        
    }
}
