using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Infra.Repositories;
using MSIT147thGraduationTopic.Models.ViewModels;
using MSIT147thGraduationTopic.ViewComponents;

namespace MSIT147thGraduationTopic.Controllers
{
    public class ApiEvaluationController : Controller
    {
        private readonly GraduationTopicContext _context;
        private readonly EvaluationRepository _repo;
       public ApiEvaluationController(GraduationTopicContext context)
        {
            _context = context;
            _repo = new EvaluationRepository(context);
        }
        public async Task<IActionResult> ShowMoreEvaliation(int id, int evaluationPageCounts)
        {
            
            var model = await _repo.ShowMoreEvaliation(id, evaluationPageCounts);
            
            return Json(model);
        }


        // GET: ApiEvaluation
        public async Task<IActionResult> Index()
        {
            var graduationTopicContext = _context.Evaluations.Include(e => e.Merchandise).Include(e => e.Order);
            return View(await graduationTopicContext.ToListAsync());
        }

        // GET: ApiEvaluation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Evaluations == null)
            {
                return NotFound();
            }

            var evaluation = await _context.Evaluations
                
                .Include(e => e.Merchandise)
                .Include(e => e.Order)
                .FirstOrDefaultAsync(m => m.EvaluationId == id);
            if (evaluation == null)
            {
                return NotFound();
            }

            return View(evaluation);
        }

        // GET: ApiEvaluation/Create
        public IActionResult Create()
        {
            ViewData["MemberId"] = new SelectList(_context.Members, "MemberId", "Account");
            ViewData["MerchandiseId"] = new SelectList(_context.Merchandises, "MerchandiseId", "MerchandiseName");
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "ContactPhoneNumber");
            return View();
        }

        // POST: ApiEvaluation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EvaluationId,MerchandiseId,MemberId,OrderId,Comment,Score")] Evaluation evaluation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(evaluation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["MerchandiseId"] = new SelectList(_context.Merchandises, "MerchandiseId", "MerchandiseName", evaluation.MerchandiseId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "ContactPhoneNumber", evaluation.OrderId);
            return View(evaluation);
        }

        // GET: ApiEvaluation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Evaluations == null)
            {
                return NotFound();
            }

            var evaluation = await _context.Evaluations.FindAsync(id);
            if (evaluation == null)
            {
                return NotFound();
            }
            
            ViewData["MerchandiseId"] = new SelectList(_context.Merchandises, "MerchandiseId", "MerchandiseName", evaluation.MerchandiseId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "ContactPhoneNumber", evaluation.OrderId);
            return View(evaluation);
        }

        // POST: ApiEvaluation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EvaluationId,MerchandiseId,MemberId,OrderId,Comment,Score")] Evaluation evaluation)
        {
            if (id != evaluation.EvaluationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(evaluation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EvaluationExists(evaluation.EvaluationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
          
            ViewData["MerchandiseId"] = new SelectList(_context.Merchandises, "MerchandiseId", "MerchandiseName", evaluation.MerchandiseId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "ContactPhoneNumber", evaluation.OrderId);
            return View(evaluation);
        }

        // GET: ApiEvaluation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Evaluations == null)
            {
                return NotFound();
            }

            var evaluation = await _context.Evaluations
                
                .Include(e => e.Merchandise)
                .Include(e => e.Order)
                .FirstOrDefaultAsync(m => m.EvaluationId == id);
            if (evaluation == null)
            {
                return NotFound();
            }

            return View(evaluation);
        }

        // POST: ApiEvaluation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Evaluations == null)
            {
                return Problem("Entity set 'GraduationTopicContext.Evaluations'  is null.");
            }
            var evaluation = await _context.Evaluations.FindAsync(id);
            if (evaluation != null)
            {
                _context.Evaluations.Remove(evaluation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EvaluationExists(int id)
        {
          return (_context.Evaluations?.Any(e => e.EvaluationId == id)).GetValueOrDefault();
        }
    }
}
