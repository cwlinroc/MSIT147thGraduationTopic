using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.ViewModels;

namespace MSIT147thGraduationTopic.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly GraduationTopicContext _context;

        public CategoriesController(GraduationTopicContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index(string txtKeyword)
        {
            IEnumerable<Category> datas = null;
            datas = (string.IsNullOrEmpty(txtKeyword)) ? from c in _context.Categories select c
                : _context.Categories.Where(c => c.CategoryName.Contains(txtKeyword));

            //todo 轉換為Wrap/VM以讀取自訂屬性(ex.資料名稱)
            //List<CProductWrap> list = new List<CProductWrap>();
            //foreach (TProduct t in datas)
            //{
            //    CProductWrap c = new CProductWrap();
            //    c.product = t;
            //    list.Add(c);
            //}

            return (datas != null) ? View(datas) : Problem("找不到商品類別資料");
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName")] Category category)
        {
            if (ModelState.IsValid) //todo 檢查名稱重複
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid) //todo 檢查名稱重複
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return Problem("找不到商品類別資料");
            }

            var brand = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (brand == null)
            {
                return Problem("找不到商品類別資料");
            }

            _context.Categories.Remove(brand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
          return (_context.Categories?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }
    }
}
