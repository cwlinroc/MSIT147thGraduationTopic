using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.ViewModels;
using System.Data;

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
                            EvaluationId = x.EvaluationId,
                            OrderId = x.OrderId,
                            MerchandiseName = x.Merchandise.MerchandiseName,
                            Comment = x.Comment,
                            Score = x.Score,
                        };            
            return View(model);

        }
        [HttpPost]
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
        [HttpPost]
        public IActionResult Search(string keyword)
        {
            var model = _context.Evaluations
                        .Where(x => string.IsNullOrEmpty(keyword) || x.Merchandise.MerchandiseName.Contains(keyword))
                        .Select(x => new EvaluationVM
                        {
                            OrderId = x.OrderId,
                            MerchandiseId = x.MerchandiseId,
                            Score = x.Score,
                            Comment = x.Comment,
                        }).ToList();
            return View("EBIndex", model);
        }
    }

}
