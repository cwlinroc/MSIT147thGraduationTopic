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
                            //MerchandiseId = x.MerchandiseId,
                            MerchandiseName = x.Merchandise.MerchandiseName,
                            Comment = x.Comment,
                            Score = x.Score,
                        };

            //List<EvaluationVM> e = new List<EvaluationVM>();
            
            return View(model);

        }
         public async Task<IActionResult> Delete(int? evaluationId)
        {
            if (evaluationId == null || _context.Evaluations == null)
            {
                return NotFound();
            }

            var evaluation = await _context.Evaluations
                
                .Include(e => e.Merchandise)
                .Include(e => e.Order)
                .FirstOrDefaultAsync(m => m.EvaluationId == evaluationId);
            if (evaluation == null)
            {
                return NotFound();
            }

            return View(evaluation);
        }
    }

}
