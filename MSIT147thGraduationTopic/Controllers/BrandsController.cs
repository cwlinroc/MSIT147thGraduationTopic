using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.ViewModels;

namespace MSIT147thGraduationTopic.Controllers
{
    public class BrandsController : Controller
    {
        private readonly GraduationTopicContext _context;

        public BrandsController(GraduationTopicContext context)
        {
            _context = context;
        }

        // GET: Brands
        public async Task<IActionResult> Index(string txtKeyword)
        {
            IEnumerable<Brand> datas = null;
            datas = (string.IsNullOrEmpty(txtKeyword)) ? from b in _context.Brands select b 
                : _context.Brands.Where(b => b.BrandName.Contains(txtKeyword));

            List<BrandVM> list = new List<BrandVM>();
            foreach (Brand b in datas)
            {
                BrandVM brandvm = new BrandVM();
                brandvm.brand = b;
                list.Add(brandvm);
            }

            return (list != null) ? View(list) : Problem("找不到品牌資料");//Entity set 'GraduationTopicContext.Brands'  is null.
        }

        // GET: Brands/Create
        public IActionResult Create()
        {
            BrandVM brandvm = new BrandVM();
            return View(brandvm);
        }

        // POST: Brands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BrandId,BrandName")] BrandVM brandvm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(brandvm.brand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(brandvm);
        }

        // GET: Brands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Brands == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            BrandVM brandvm = new BrandVM();
            brandvm.brand = brand;
            return View(brandvm);
        }

        // POST: Brands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BrandId,BrandName")] BrandVM brandvm)
        {
            if (id != brandvm.BrandId)
            {
                return NotFound();
            }

            if (ModelState.IsValid) //todo 檢查名稱重複
            {
                try
                {
                    _context.Update(brandvm.brand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExists(brandvm.BrandId))
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
            return View(brandvm);
        }

        // GET: Brands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.Merchandises.Where(m => m.BrandId == id).Count() > 0)
            {
                return Problem("品牌中尚有商品，因此無法刪除");
            }
            if (_context.Brands.Count() == 1)
            {
                return Problem("品牌總數不可為零，因此無法刪除");
            }

            if (id == null || _context.Brands == null)
            {
                return Problem("找不到品牌資料");
            }

            var brand = await _context.Brands
                .FirstOrDefaultAsync(m => m.BrandId == id);
            if (brand == null)
            {
                return Problem("找不到品牌資料");
            }

                _context.Brands.Remove(brand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        private bool BrandExists(int id)
        {
          return (_context.Brands?.Any(e => e.BrandId == id)).GetValueOrDefault();
        }
    }
}
