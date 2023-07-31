using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.ViewModels;

namespace MSIT147thGraduationTopic.Controllers
{
    public class EvaluationBackstageController : Controller
    {
        private readonly GraduationTopicContext _context;

        public EvaluationBackstageController(GraduationTopicContext context)
        {
            _context = context;
        }

        public IActionResult EBIndex()
        {
            var model = from x in _context.Evaluations
                        
                        select new EvaluationVM
                        {
                            OrderId = x.OrderId,
                            MerchandiseName = x.Merchandise.MerchandiseName,
                            Comment = x.Comment,
                            Score = x.Score,
                        };            
            return View(model);

        }
        public IActionResult Delete(int id)
        {
            var evaluation = _context.Evaluations.FirstOrDefault(e => e.EvaluationId == id);
            
            if (evaluation != null)
            {
                _context.Evaluations.Remove(evaluation);
                _context.SaveChanges();
            }
            
            return RedirectToAction("EBIndex");

            
        }

        

    }

}
