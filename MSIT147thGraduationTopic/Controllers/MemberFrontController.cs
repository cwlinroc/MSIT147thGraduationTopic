using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;

namespace MSIT147thGraduationTopic.Controllers
{
    public class MemberFrontController : Controller
    {
        public IActionResult Register()
        {

            return View();
        }

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
    }
}
