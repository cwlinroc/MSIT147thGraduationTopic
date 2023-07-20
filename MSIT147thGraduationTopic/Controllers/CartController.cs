using Microsoft.AspNetCore.Mvc;

namespace MSIT147thGraduationTopic.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index(int id = 1)
        {
            return View(id);
        }
    }
}
