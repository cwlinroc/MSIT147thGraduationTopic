using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.ViewModels;

namespace MSIT147thGraduationTopic.Controllers.Merchandise
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
        [Authorize(Roles = "管理員,經理,員工")]
        public IActionResult Index(string txtKeyword, int searchCondition = 1, int PageIndex = 1
                                                            , int displayorder = 0, int pageSize = 10)
        {
            //保存參數以供換頁時保留設定
            if (!string.IsNullOrEmpty(txtKeyword)) HttpContext.Response.Cookies.Append("txtKeyword", txtKeyword);
            if (string.IsNullOrEmpty(txtKeyword)) HttpContext.Response.Cookies.Append("txtKeyword", "");
            HttpContext.Response.Cookies.Append("searchCondition", searchCondition.ToString());
            HttpContext.Response.Cookies.Append("PageIndex", PageIndex.ToString());
            HttpContext.Response.Cookies.Append("displayorder", displayorder.ToString());
            HttpContext.Response.Cookies.Append("pageSize", pageSize.ToString());

            //傳遞初值以供介面呈現
            ViewBag.txtKeyword = txtKeyword;
            ViewBag.searchCondition = searchCondition;
            ViewBag.PageIndex = PageIndex;
            ViewBag.displayorder = displayorder;
            ViewBag.pageSize = pageSize;

            IEnumerable<MerchandiseSearch> datas;
            datas = from m in _context.MerchandiseSearches
                    select m;
            if (!string.IsNullOrEmpty(txtKeyword))
            {
                if (searchCondition == 1)
                    datas = datas.Where(ms => ms.MerchandiseName.Contains(txtKeyword));
                if (searchCondition == 2)
                {
                    IQueryable<int> merchandiseIdFormSpec = _context.Specs
                        .Where(s => s.SpecName.Contains(txtKeyword)).Select(s => s.MerchandiseId).Distinct();                   
                    datas = datas.Where(ms => merchandiseIdFormSpec.Contains(ms.MerchandiseId));
                }
                if (searchCondition == 3)
                    datas = datas.Where(ms => ms.BrandName.Contains(txtKeyword));
                if (searchCondition == 4)
                    datas = datas.Where(ms => ms.CategoryName.Contains(txtKeyword));
            }

            datas = displayorder switch
            {
                1 => datas.OrderBy(ms => ms.MerchandiseId),                //由舊到新熱門商品
                _ => datas.OrderByDescending(ms => ms.MerchandiseId)       //最新商品
            };

            datas = datas.Skip((PageIndex - 1) * pageSize).Take(pageSize).ToList();

            List<MerchandiseSearchVM> list = new List<MerchandiseSearchVM>();
            foreach (MerchandiseSearch ms in datas)
            {
                MerchandiseSearchVM merchandisesearchvm = new MerchandiseSearchVM();
                merchandisesearchvm.merchandisesearch = ms;
                list.Add(merchandisesearchvm);
            }

            return View(list);
        }

        // GET: Merchandises/Create
        [Authorize(Roles = "管理員,經理,員工")]
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
        [Authorize(Roles = "管理員,經理,員工")]
        public async Task<IActionResult> Create
            ([Bind("MerchandiseId,MerchandiseName,BrandId,CategoryId,Description,ImageUrl,Display,photo")]
                MerchandiseVM merchandisevm)
        {

            if (ModelState.IsValid)
            {
                if (merchandisevm.photo != null)
                {
                    int fileNameLangth = merchandisevm.photo.FileName.Length;                    
                    merchandisevm.ImageUrl = (fileNameLangth > 100) 
                        ? Guid.NewGuid().ToString() + merchandisevm.photo.FileName.Substring(fileNameLangth - 50, 50) 
                        : Guid.NewGuid().ToString() + merchandisevm.photo.FileName;
                    saveMerchandiseImageToUploads(merchandisevm.ImageUrl, merchandisevm.photo);
                }

                _context.Add(merchandisevm.merchandise);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", new
                {
                    txtKeyword = HttpContext.Request.Cookies["txtKeyword"],
                    searchCondition = int.Parse(HttpContext.Request.Cookies["searchCondition"]),
                    PageIndex = int.Parse(HttpContext.Request.Cookies["PageIndex"]),
                    displayorder = int.Parse(HttpContext.Request.Cookies["displayorder"]),
                    pageSize = int.Parse(HttpContext.Request.Cookies["pageSize"])
                });
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", merchandisevm.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", merchandisevm.CategoryId);
            return View(merchandisevm);
        }

        // GET: Merchandises/Edit/5
        [Authorize(Roles = "管理員,經理,員工")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Merchandises == null) return NotFound();

            var merchandise = await _context.Merchandises.FindAsync(id);
            if (merchandise == null) return NotFound();

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
        [Authorize(Roles = "管理員,經理,員工")]
        public async Task<IActionResult> Edit(int id,
            [Bind("MerchandiseId,MerchandiseName,BrandId,CategoryId,Description,ImageUrl,Display,photo,deleteImageIndicater")]
                    MerchandiseVM merchandisevm)
        {
            if (id != merchandisevm.MerchandiseId) return NotFound();

            if (ModelState.IsValid)
            {
                //(始終沒圖) or (有圖→沒變) => 不用動
                //沒圖→有圖
                if (merchandisevm.ImageUrl == null && merchandisevm.photo != null)
                {
                    int fileNameLangth = merchandisevm.photo.FileName.Length;
                    merchandisevm.ImageUrl = (fileNameLangth > 100)
                        ? Guid.NewGuid().ToString() + merchandisevm.photo.FileName.Substring(fileNameLangth - 50, 50)
                        : Guid.NewGuid().ToString() + merchandisevm.photo.FileName;
                    saveMerchandiseImageToUploads(merchandisevm.ImageUrl, merchandisevm.photo);
                }
                //有圖→新圖
                if (merchandisevm.ImageUrl != null && merchandisevm.photo != null)
                {
                    deleteMerchandiseImageFromUploads(merchandisevm.ImageUrl);

                    int fileNameLangth = merchandisevm.photo.FileName.Length;
                    merchandisevm.ImageUrl = (fileNameLangth > 100)
                        ? Guid.NewGuid().ToString() + merchandisevm.photo.FileName.Substring(fileNameLangth - 50, 50)
                        : Guid.NewGuid().ToString() + merchandisevm.photo.FileName;
                    saveMerchandiseImageToUploads(merchandisevm.ImageUrl, merchandisevm.photo);
                }
                //有圖→刪除
                if (merchandisevm.ImageUrl != null && merchandisevm.photo == null && merchandisevm.deleteImageIndicater == true)
                {
                    deleteMerchandiseImageFromUploads(merchandisevm.ImageUrl);
                    merchandisevm.ImageUrl = null;
                }

                try
                {
                    _context.Update(merchandisevm.merchandise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MerchandiseExists(merchandisevm.MerchandiseId)) return NotFound();
                    throw;
                }

                return RedirectToAction("Index", new
                {
                    txtKeyword = HttpContext.Request.Cookies["txtKeyword"],
                    searchCondition = int.Parse(HttpContext.Request.Cookies["searchCondition"]),
                    PageIndex = int.Parse(HttpContext.Request.Cookies["PageIndex"]),
                    displayorder = int.Parse(HttpContext.Request.Cookies["displayorder"]),
                    pageSize = int.Parse(HttpContext.Request.Cookies["pageSize"])
                });
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "BrandId", "BrandName", merchandisevm.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", merchandisevm.CategoryId);
            return View(merchandisevm);
        }

        // GET: Merchandises/Delete/5
        [Authorize(Roles = "管理員,經理")]
        public async Task<IActionResult> Delete(int? id)// todo 加上條件參數
        {
            if (_context.Specs.Where(s => s.MerchandiseId == id).Count() > 0)
                return RedirectToAction(nameof(Index));

            if (id == null || _context.Merchandises == null)
                return Problem("找不到商品資料");

            var merchandise = await _context.Merchandises
                .FirstOrDefaultAsync(m => m.MerchandiseId == id);
            if (merchandise == null) return Problem("找不到商品資料");

            if (!string.IsNullOrEmpty(merchandise.ImageUrl))
                deleteMerchandiseImageFromUploads(merchandise.ImageUrl);
            _context.Merchandises.Remove(merchandise);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new
            {
                txtKeyword = HttpContext.Request.Cookies["txtKeyword"],
                searchCondition = int.Parse(HttpContext.Request.Cookies["searchCondition"]),
                PageIndex = int.Parse(HttpContext.Request.Cookies["PageIndex"]),
                displayorder = int.Parse(HttpContext.Request.Cookies["displayorder"]),
                pageSize = int.Parse(HttpContext.Request.Cookies["pageSize"])
            });
        }

        private bool MerchandiseExists(int id)
        {
            return (_context.Merchandises?.Any(e => e.MerchandiseId == id)).GetValueOrDefault();
        }
        private void saveMerchandiseImageToUploads(string ImageUrl, IFormFile photo)
        {
            string savepath = Path.Combine(_host.WebRootPath, "uploads/merchandisePicture", ImageUrl);
            using (var fileStream = new FileStream(savepath, FileMode.Create))
            {
                photo.CopyTo(fileStream);
            }
        }
        private void deleteMerchandiseImageFromUploads(string ImageUrl)
        {
            string deletepath = Path.Combine(_host.WebRootPath, "uploads/merchandisePicture", ImageUrl);
            System.IO.File.Delete(deletepath);
        }
    }
}
