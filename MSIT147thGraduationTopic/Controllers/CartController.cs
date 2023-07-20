using Microsoft.AspNetCore.Mvc;

namespace MSIT147thGraduationTopic.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index(int id = 5)
        {
            return View(id);
        }
    }
}
