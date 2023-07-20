using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;

namespace MSIT147thGraduationTopic.Controllers
{
    public class SpecsController : Controller
    {
        private readonly GraduationTopicContext _context;

        public SpecsController(GraduationTopicContext context)
        {
            _context = context;
        }

        // GET: Specs
        public async Task<IActionResult> Index()
        {
            var graduationTopicContext = _context.Specs.Include(s => s.Merchandise);
            return View(await graduationTopicContext.ToListAsync());
        }

        // GET: Specs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Specs == null)
            {
                return NotFound();
            }

            var spec = await _context.Specs
                .Include(s => s.Merchandise)
                .FirstOrDefaultAsync(m => m.SpecId == id);
            if (spec == null)
            {
                return NotFound();
            }

            return View(spec);
        }

        // GET: Specs/Create
        public IActionResult Create()
        {
            ViewData["MerchandiseId"] = new SelectList(_context.Merchandises, "MerchandiseId", "MerchandiseName");
            return View();
        }

        // POST: Specs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SpecId,SpecName,MerchandiseId,Price,Amount,DisplayOrder,OnShelf,DiscountPercentage")] Spec spec)
        {
            if (ModelState.IsValid)
            {
                _context.Add(spec);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MerchandiseId"] = new SelectList(_context.Merchandises, "MerchandiseId", "MerchandiseName", spec.MerchandiseId);
            return View(spec);
        }

        // GET: Specs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Specs == null)
            {
                return NotFound();
            }

            var spec = await _context.Specs.FindAsync(id);
            if (spec == null)
            {
                return NotFound();
            }
            ViewData["MerchandiseId"] = new SelectList(_context.Merchandises, "MerchandiseId", "MerchandiseName", spec.MerchandiseId);
            return View(spec);
        }

        // POST: Specs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SpecId,SpecName,MerchandiseId,Price,Amount,DisplayOrder,OnShelf,DiscountPercentage")] Spec spec)
        {
            if (id != spec.SpecId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(spec);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecExists(spec.SpecId))
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
            ViewData["MerchandiseId"] = new SelectList(_context.Merchandises, "MerchandiseId", "MerchandiseName", spec.MerchandiseId);
            return View(spec);
        }

        // GET: Specs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Specs == null)
            {
                return NotFound();
            }

            var spec = await _context.Specs
                .Include(s => s.Merchandise)
                .FirstOrDefaultAsync(m => m.SpecId == id);
            if (spec == null)
            {
                return NotFound();
            }

            return View(spec);
        }

        // POST: Specs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Specs == null)
            {
                return Problem("Entity set 'GraduationTopicContext.Specs'  is null.");
            }
            var spec = await _context.Specs.FindAsync(id);
            if (spec != null)
            {
                _context.Specs.Remove(spec);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpecExists(int id)
        {
          return (_context.Specs?.Any(e => e.SpecId == id)).GetValueOrDefault();
        }
    }
}
