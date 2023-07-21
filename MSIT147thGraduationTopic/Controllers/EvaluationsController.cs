using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;

namespace MSIT147thGraduationTopic.Controllers
{
    public class EvaluationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult BackStage()
        {
            List<Evaluation> model = new List<Evaluation>();

            return View(model);
        }
    }
}
