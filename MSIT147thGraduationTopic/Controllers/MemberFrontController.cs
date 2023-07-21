using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;

namespace MSIT147thGraduationTopic.Controllers
{
    public class MemberFrontController : Controller
    {  
        public IActionResult MemberRegister()
        {
            return View();
        }

        [HttpPost]
        public IActionResult MemberRegister(Member member)
        {
            if (ModelState.IsValid)
            {
                
            }
            return View();
        }

        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Register(Member member)
        {

            return View();
        }

        public IActionResult MemberCenter()
        {
            return View();
        }

        [HttpPost]
        public IActionResult MemberCenter(Member member)
        {
            return View();
        }

        public IActionResult CouponList()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CouponList(Member member)
        {
            return View();
        }

        public IActionResult ShoppingHistory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ShoppingHistory(Member member)
        {
            return View();
        }
    }
}
