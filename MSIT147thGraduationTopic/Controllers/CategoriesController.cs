using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "管理員,經理,員工")]
        public IActionResult Index(string txtKeyword)
        {
            IEnumerable<Category> datas = string.IsNullOrEmpty(txtKeyword) ? from c in _context.Categories select c
                : _context.Categories.Where(c => c.CategoryName.Contains(txtKeyword));

            List<CategoryVM> list = new List<CategoryVM>();
            foreach (Category c in datas)
            {
                CategoryVM categoryvm = new CategoryVM();
                categoryvm.category = c;
                list.Add(categoryvm);
            }

            return View(list);
        }

        // GET: Categories/Create
        [Authorize(Roles = "管理員,經理,員工")]
        public IActionResult Create()
        {
            CategoryVM categoryvm = new CategoryVM();
            return View(categoryvm);
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "管理員,經理,員工")]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName")] CategoryVM categoryvm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoryvm.category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoryvm);
        }

        // GET: Categories/Edit/5
        [Authorize(Roles = "管理員,經理,員工")]
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

            CategoryVM categoryvm = new CategoryVM();
            categoryvm.category = category;

            return View(categoryvm);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "管理員,經理,員工")]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName")] CategoryVM categoryvm)
        {
            if (id != categoryvm.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoryvm.category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(categoryvm.CategoryId))
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
            return View(categoryvm);
        }

        // GET: Categories/Delete/5
        [Authorize(Roles = "管理員,經理")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.Merchandises.Where(m => m.CategoryId == id).Count() > 0)
            {
                return RedirectToAction(nameof(Index));
            }
            //if (_context.Categories.Count() == 1)
            //{
            //    //類別總數不可為零，因此無法刪除
            //    return RedirectToAction(nameof(Index));
            //}

            if (id == null || _context.Categories == null)
            {
                return Problem("找不到商品類別資料");
            }

            var brand = await _context.Categories.FirstOrDefaultAsync(m => m.CategoryId == id);
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
