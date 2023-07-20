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
    public class BrandsController : Controller
    {
        private readonly GraduationTopicContext _context;

        public BrandsController(GraduationTopicContext context)
        {
            _context = context;
        }

        // GET: Brands
        public async Task<IActionResult> Index(KeywordVM vm)
        {
            IEnumerable<Brand> datas = null;
            datas = (string.IsNullOrEmpty(vm.txtKeyword)) ? from b in _context.Brands select b 
                : _context.Brands.Where(b => b.BrandName.Contains(vm.txtKeyword));

            //todo 轉換為Wrap/Servise以讀取自訂屬性(ex.資料名稱)
            //List<CProductWrap> list = new List<CProductWrap>();
            //foreach (TProduct t in datas)
            //{
            //    CProductWrap c = new CProductWrap();
            //    c.product = t;
            //    list.Add(c);
            //}

            return (datas != null) ? View(datas) : Problem("找不到品牌資料");//Entity set 'GraduationTopicContext.Brands'  is null.
        }

        // GET: Brands/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Brands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BrandId,BrandName")] Brand brand)
        {
            if (ModelState.IsValid) //todo 檢查名稱重複
            {
                _context.Add(brand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
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
            return View(brand);
        }

        // POST: Brands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BrandId,BrandName")] Brand brand)
        {
            if (id != brand.BrandId)
            {
                return NotFound();
            }

            if (ModelState.IsValid) //todo 檢查名稱重複
            {
                try
                {
                    _context.Update(brand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExists(brand.BrandId))
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
            return View(brand);
        }

        // GET: Brands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
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
