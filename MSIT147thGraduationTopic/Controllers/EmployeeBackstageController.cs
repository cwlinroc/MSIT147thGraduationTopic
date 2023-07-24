using Microsoft.AspNetCore.Mvc;

namespace MSIT147thGraduationTopic.Controllers
{
    public class EmployeeBackstageController : Controller
    {
        public IActionResult Index()
        {
            string[] permissions = { "管理員", "經理", "員工" };
            return View(permissions);
        }

    }
}
