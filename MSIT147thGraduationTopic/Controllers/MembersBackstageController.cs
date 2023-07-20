using Microsoft.AspNetCore.Mvc;

namespace MSIT147thGraduationTopic.Controllers
{
    public class MembersBackstageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
