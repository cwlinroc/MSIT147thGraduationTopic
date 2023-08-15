using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;

namespace MSIT147thGraduationTopic.ViewComponents
{
    public class EvaluationVC : ViewComponent
    {

        private readonly GraduationTopicContext _context;

        public EvaluationVC(GraduationTopicContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewData["age"] = 20;
            ViewBag.name = "測試";
            return View();
        }
    }
}
