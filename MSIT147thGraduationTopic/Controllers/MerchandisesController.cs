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
    public class MerchandisesController : Controller
    {
        private readonly GraduationTopicContext _context;
        private readonly IWebHostEnvironment _host;

        public MerchandisesController(GraduationTopicContext context, IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }

        // GET: Merchandises
        public async Task<IActionResult> Index(string txtKeyword, int searchCondition)
        {
            IEnumerable<MerchandiseSearch> datas = null;
            datas = from m in _context.MerchandiseSearches
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

            List<MerchandiseSearchVM> list = new List<MerchandiseSearchVM>();
            foreach (MerchandiseSearch ms in datas)
            {
                MerchandiseSearchVM merchandisesearchvm = new MerchandiseSearchVM();
                merchandisesearchvm.merchandisesearch = ms;
                list.Add(merchandisesearchvm);
            }

            return (list != null) ? View(list) : Problem("找不到商品資料");
        }

        // GET: Merchandises/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            MerchandiseVM merchandisevm = new MerchandiseVM();
            return View(merchandisevm);
        }

        // POST: Merchandises/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create
            ([Bind("MerchandiseId,MerchandiseName,BrandId,CategoryId,Description,ImageUrl,Display,photo")]
                MerchandiseVM merchandisevm)
        {
            merchandisevm.ImageUrl = null;
            if (merchandisevm.photo != null)
            {
                // 使用隨機數改名，避免資料庫內名稱重複
                merchandisevm.ImageUrl = Guid.NewGuid().ToString() + merchandisevm.photo.FileName;
                string savepath = Path.Combine(_host.WebRootPath, "uploads/merchandisePicture", merchandisevm.ImageUrl);
                // 複製圖片到資料夾
                using (var fileStream = new FileStream(savepath, FileMode.Create))
                {
                    merchandisevm.photo.CopyTo(fileStream);
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(merchandisevm.merchandise); //todo 無法成功上傳，還差最後一步
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", merchandisevm.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", merchandisevm.CategoryId);
            return View(merchandisevm);
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

            MerchandiseVM merchandisevm = new MerchandiseVM();
            merchandisevm.merchandise = merchandise;
            return View(merchandisevm);
        }

        // POST: Merchandises/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, 
            [Bind("MerchandiseId,MerchandiseName,BrandId,CategoryId,Description,ImageUrl,Display")] MerchandiseVM merchandisevm)
        {
            if (id != merchandisevm.MerchandiseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(merchandisevm.merchandise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MerchandiseExists(merchandisevm.MerchandiseId))
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
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", merchandisevm.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", merchandisevm.CategoryId);
            return View(merchandisevm);
        }

        // GET: Merchandises/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.Specs.Where(s => s.MerchandiseId == id).Count() > 0)
            {
                return Problem("商品尚有規格資料，因此無法刪除");
            }

            if (id == null || _context.Merchandises == null)
            {
                return Problem("找不到商品資料");
            }

            var merchandise = await _context.Merchandises
                .FirstOrDefaultAsync(m => m.MerchandiseId == id);
            if (merchandise == null)
            {
                return Problem("找不到商品資料");
            }

            _context.Merchandises.Remove(merchandise);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
    }

        private bool MerchandiseExists(int id)
        {
          return (_context.Merchandises?.Any(e => e.MerchandiseId == id)).GetValueOrDefault();
        }
    }
}
