using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;

namespace MSIT147thGraduationTopic.Controllers
{
    public class EvaluationBackstageController : Controller
    {
        public IActionResult EBIndex()
        {
            List<Evaluation> model = new List<Evaluation>();

            return View(model);
        }      
    }

}
