using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;

namespace MSIT147thGraduationTopic.Controllers
{
    public class CouponFrontController : Controller
    {
        public IActionResult CouponList()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CouponList(Member member)
        {
            return View();
        }
    }
}
