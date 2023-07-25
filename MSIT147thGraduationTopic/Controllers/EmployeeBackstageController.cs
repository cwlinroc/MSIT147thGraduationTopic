using Microsoft.AspNetCore.Mvc;

namespace MSIT147thGraduationTopic.Controllers
{
    public class EmployeeBackstageController : Controller
    {
        //TODO add in appsettings
        private readonly string[] _roles = { "管理員", "經理", "員工" };
        public IActionResult Index()
        {
            return View(_roles);
        }

    }
}
