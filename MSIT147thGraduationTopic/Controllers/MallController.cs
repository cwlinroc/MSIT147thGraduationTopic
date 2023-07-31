using Microsoft.AspNetCore.Mvc;

namespace MSIT147thGraduationTopic.Controllers
{
    public class MallController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
