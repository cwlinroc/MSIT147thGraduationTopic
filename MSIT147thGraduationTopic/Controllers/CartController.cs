using Microsoft.AspNetCore.Mvc;

namespace MSIT147thGraduationTopic.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
