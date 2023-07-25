using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;

namespace MSIT147thGraduationTopic.Controllers
{
    public class EvaluationController : Controller
    {
        public IActionResult EIndex()
        {
            return View();
        }

        
    }
}
