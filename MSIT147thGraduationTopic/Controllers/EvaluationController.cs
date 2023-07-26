using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.ViewModels;

namespace MSIT147thGraduationTopic.Controllers
{
    public class EvaluationController : Controller
    {
        public IActionResult EIndex()
        {
            EvaluationInput evaluationInput = new EvaluationInput();
            return View(evaluationInput);
        }
        [HttpPost]
        public IActionResult EIndex(List<Comments> comments)
        {
            for (int i = 0; i < comments.Count; i++)
            {
                string comment = comments[i].Comment;
                var score = comments[i].Score;
            }
            return View();
        }


    }
}
