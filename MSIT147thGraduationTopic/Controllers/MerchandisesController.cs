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
    public class MerchandisesController : Controller
    {
        private readonly GraduationTopicContext _context;

        public MerchandisesController(GraduationTopicContext context)
        {
            _context = context;
        }

        // GET: Merchandises
        public async Task<IActionResult> Index(string txtKeyword, int searchCondition)
        {
            IEnumerable<MerchandiseSearch> datas = null;
            datas = from m in _context.MerchandiseSearches //todo 增加tag搜尋?
                    select m;
            if (!string.IsNullOrEmpty(txtKeyword))
            {
                if (searchCondition == 1)
                {
                    datas = datas.Where(ms => ms.MerchandiseName.Contains(txtKeyword));
                }
                if (searchCondition == 2)
                {
                    datas = datas.Where(ms => ms.BrandName.Contains(txtKeyword));
                }
                if (searchCondition == 3)
                {
                    datas = datas.Where(ms => ms.CategoryName.Contains(txtKeyword));
                }
            }

            //todo 轉換為Wrap/VM以讀取自訂屬性(ex.資料名稱)
            //List<CProductWrap> list = new List<CProductWrap>();
            //foreach (TProduct t in datas)
            //{
            //    CProductWrap c = new CProductWrap();
            //    c.product = t;
            //    list.Add(c);
            //}

            return (datas != null) ? View(datas) : Problem("找不到商品資料");
        }

        // GET: Merchandises/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.MerchandiseSearches == null)
        //    {
        //        return Problem("找不到商品資料");
        //    }
        //    var merchandise = await _context.MerchandiseSearches
        //        .FirstOrDefaultAsync(m => m.MerchandiseId == id);
        //    if (merchandise == null)
        //    {
        //        return Problem("找不到商品資料");
        //    }

        //    var spec = await _context.Specs
        //        .FirstOrDefaultAsync(s => s.MerchandiseId == id);
        //    if (spec == null)
        //    {
        //        return Problem("找不到商品規格資料");
        //    }

        //    return RedirectToAction("~/Specs/Index");//todo 確認這樣寫是否正確(參考Ajax作業)
        //}

        // GET: Merchandises/Create
        public IActionResult Create() //todo 上傳&預覽圖片還沒做
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Merchandises/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MerchandiseId,MerchandiseName,BrandId,CategoryId,Description,ImageUrl,Display")] Merchandise merchandise)
        {
            if (ModelState.IsValid)
            {
                _context.Add(merchandise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", merchandise.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", merchandise.CategoryId);
            return View(merchandise);
        }

        // GET: Merchandises/Edit/5
        public async Task<IActionResult> Edit(int? id) //todo 變更&預覽圖片還沒做
        {
            if (id == null || _context.Merchandises == null)
            {
                return NotFound();
            }

            var merchandise = await _context.Merchandises.FindAsync(id);
            if (merchandise == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", merchandise.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", merchandise.CategoryId);
            return View(merchandise);
        }

        // POST: Merchandises/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MerchandiseId,MerchandiseName,BrandId,CategoryId,Description,ImageUrl,Display")] Merchandise merchandise)
        {
            if (id != merchandise.MerchandiseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(merchandise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MerchandiseExists(merchandise.MerchandiseId))
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
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", merchandise.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", merchandise.CategoryId);
            return View(merchandise);
        }

        // GET: Merchandises/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Merchandises == null)
            {
                return NotFound();
            }

            var merchandise = await _context.Merchandises
                .Include(m => m.Brand)
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.MerchandiseId == id);
            if (merchandise == null)
            {
                return NotFound();
            }

            return View(merchandise);
        }

        // POST: Merchandises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Merchandises == null)
            {
                return Problem("Entity set 'GraduationTopicContext.Merchandises'  is null.");
            }
            var merchandise = await _context.Merchandises.FindAsync(id);
            if (merchandise != null)
            {
                _context.Merchandises.Remove(merchandise);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MerchandiseExists(int id)
        {
          return (_context.Merchandises?.Any(e => e.MerchandiseId == id)).GetValueOrDefault();
        }
    }
}
