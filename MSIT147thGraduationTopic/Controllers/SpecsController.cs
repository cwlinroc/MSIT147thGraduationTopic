using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.ViewModels;
using Newtonsoft.Json.Linq;

namespace MSIT147thGraduationTopic.Controllers
{
    public class SpecsController : Controller
    {
        private readonly GraduationTopicContext _context;
        private readonly IWebHostEnvironment _host;

        public SpecsController(GraduationTopicContext context, IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }

        // GET: Specs
        [Authorize(Roles = "管理員,經理,員工")]
        public IActionResult Index(int merchandiseid, int displaymode = 1, int displayorder = 0)
        {
            ViewBag.MerchandiseId = merchandiseid;
            ViewBag.displaymode = displaymode;
            ViewBag.displayorder = displayorder;
            HttpContext.Response.Cookies.Append("Spec_displaymode", displaymode.ToString());
            HttpContext.Response.Cookies.Append("Spec_displayorder", displayorder.ToString());

            var datas = _context.Specs.Where(s => s.MerchandiseId == merchandiseid);
            if (datas.Count() == 0)
                return RedirectToAction("IndexForNoSpec", new { merchandiseid });

            datas = displaymode switch
            {
                0 => datas = datas.Where(s => s.OnShelf == true),     //上架規格
                2 => datas = datas.Where(s => s.OnShelf == false),    //下架規格
                _ => datas = datas.Select(s => s)                     //全部規格
            };
            datas = displayorder switch
            {
                1 => datas = datas.OrderBy(s => s.SpecId),                   //由舊到新
                2 => datas = datas.OrderBy(s => s.SpecName),                 //依名稱遞增
                3 => datas = datas.OrderByDescending(s => s.SpecName),       //依名稱遞減
                4 => datas = datas.OrderByDescending(s => s.Popularity),     //熱門度高至低
                5 => datas = datas.OrderBy(s => s.Popularity),               //熱門度低至高
                _ => datas = datas.OrderByDescending(s => s.SpecId)          //由新到舊
            };

            List<SpecVM> list = new List<SpecVM>();

            foreach (Spec s in datas)
            {
                SpecVM specvm = new SpecVM();
                specvm.spec = s;
                specvm.Popularity = Math.Round((double)s.Popularity, 3);
                list.Add(specvm);
            }

            return View(list);
        }

        [Authorize(Roles = "管理員,經理,員工")]
        public IActionResult IndexForNoSpec(int merchandiseid)
        {
            ViewBag.MerchandiseId = merchandiseid;

            SpecVM specvmforCarrier = new SpecVM();
            specvmforCarrier.SpecName = "**此商品尚無規格，請新增規格資料**";
            return View(specvmforCarrier);
        }

        // GET: Specs/Create
        [Authorize(Roles = "管理員,經理,員工")]
        public IActionResult Create(int merchandiseId)
        {
            ViewBag.displaymode = int.TryParse(HttpContext.Request.Cookies["Spec_displaymode"], out int temp1) ? temp1 : 1;
            ViewBag.displayorder = int.TryParse(HttpContext.Request.Cookies["Spec_displayorder"], out int temp2) ? temp2 : 0;
            ViewData["MerchandiseId"] = new SelectList(_context.Merchandises, "MerchandiseId", "MerchandiseName");
            SpecVM specvm = new SpecVM();
            specvm.MerchandiseId = merchandiseId;
            specvm.Popularity = 0;
            return View(specvm);
        }

        // POST: Specs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "管理員,經理,員工")]
        public async Task<IActionResult> Create
            ([Bind("SpecId,SpecName,MerchandiseId,Price,Amount,ImageUrl,DisplayOrder,Popularity,OnShelf,DiscountPercentage,photo,selectTag")] SpecVM specvm)
        {
            if (ModelState.IsValid)
            {
                if (specvm.photo != null)
                {
                    int fileNameLangth = specvm.photo.FileName.Length;
                    specvm.ImageUrl = (fileNameLangth > 100)
                        ? Guid.NewGuid().ToString() + specvm.photo.FileName.Substring(fileNameLangth - 90, 90)
                        : Guid.NewGuid().ToString() + specvm.photo.FileName;
                    saveSpecImageToUploads(specvm.ImageUrl, specvm.photo);
                }

                _context.Add(specvm.spec);
                await _context.SaveChangesAsync();

                //新增寵物標籤
                if (specvm.selectTag != null)
                {
                    int SpecId = specvm.SpecId;
                    if (specvm.selectTag.Contains("Cat")) addSpecTag(SpecId, 1);
                    if (specvm.selectTag.Contains("Dog")) addSpecTag(SpecId, 2);
                    if (specvm.selectTag.Contains("Mouse")) addSpecTag(SpecId, 3);
                    if (specvm.selectTag.Contains("Rabbit")) addSpecTag(SpecId, 4);
                }

                return RedirectToAction("Index", new { merchandiseid = specvm.MerchandiseId });
            }
            ViewData["MerchandiseId"] = new SelectList(_context.Merchandises, "MerchandiseId", "MerchandiseName", specvm.MerchandiseId);
            return View(specvm);
        }

        // GET: Specs/Edit/5
        [Authorize(Roles = "管理員,經理,員工")]
        public async Task<IActionResult> Edit(int merchandiseid, string merchandisename, int? id)
        {
            ViewBag.displaymode = int.TryParse(HttpContext.Request.Cookies["Spec_displaymode"], out int temp1) ? temp1 : 1;
            ViewBag.displayorder = int.TryParse(HttpContext.Request.Cookies["Spec_displayorder"], out int temp) ? temp : 0;
            if (id == null || _context.Specs == null) return NotFound();

            var spec = await _context.Specs.FindAsync(id);
            if (spec == null) return NotFound();
            ViewData["MerchandiseId"] = new SelectList(_context.Merchandises, "MerchandiseId", "MerchandiseName", spec.MerchandiseId);

            SpecVM specvm = new SpecVM();
            specvm.spec = spec;
            return View(specvm);
        }

        // POST: Specs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "管理員,經理,員工")]
        public async Task<IActionResult> Edit(int id,
            [Bind("SpecId,SpecName,MerchandiseId,Price,Amount,ImageUrl,DisplayOrder,Popularity,OnShelf,DiscountPercentage,photo,deleteImageIndicater")] SpecVM specvm)
        {
            if (id != specvm.SpecId) return NotFound();

            if (ModelState.IsValid)
            {
                //(始終沒圖) or (有圖→沒變) => 不用動
                //沒圖→有圖
                if (specvm.ImageUrl == null && specvm.photo != null)
                {
                    int fileNameLangth = specvm.photo.FileName.Length;
                    specvm.ImageUrl = (fileNameLangth > 100)
                        ? Guid.NewGuid().ToString() + specvm.photo.FileName.Substring(fileNameLangth - 90, 90)
                        : Guid.NewGuid().ToString() + specvm.photo.FileName;
                    saveSpecImageToUploads(specvm.ImageUrl, specvm.photo);
                }
                //有圖→新圖
                if (specvm.ImageUrl != null && specvm.photo != null)
                {
                    deleteSpecImageFromUploads(specvm.ImageUrl);

                    int fileNameLangth = specvm.photo.FileName.Length;
                    specvm.ImageUrl = (fileNameLangth > 100)
                        ? Guid.NewGuid().ToString() + specvm.photo.FileName.Substring(fileNameLangth - 90, 90)
                        : Guid.NewGuid().ToString() + specvm.photo.FileName;
                    saveSpecImageToUploads(specvm.ImageUrl, specvm.photo);
                }
                //有圖→刪除
                if (specvm.ImageUrl != null && specvm.photo == null && specvm.deleteImageIndicater == true)
                {
                    deleteSpecImageFromUploads(specvm.ImageUrl);
                    specvm.ImageUrl = null;
                }

                try
                {
                    _context.Update(specvm.spec);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecExists(specvm.SpecId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { merchandiseid = specvm.MerchandiseId,
                    displaymode = int.TryParse(HttpContext.Request.Cookies["Spec_displaymode"], out int temp1) ? temp1 : 1,
                    displayorder = int.TryParse(HttpContext.Request.Cookies["Spec_displayorder"], out int temp) ? temp : 0 });
            }
            ViewData["MerchandiseId"] = new SelectList(_context.Merchandises, "MerchandiseId", "MerchandiseName", specvm.MerchandiseId);
            return View(specvm);
        }

        // GET: Specs/Delete/5
        [Authorize(Roles = "管理員,經理")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Specs == null) return Problem("找不到規格資料");

            var spec = await _context.Specs
                .FirstOrDefaultAsync(s => s.SpecId == id);
            if (spec == null) return Problem("找不到規格資料");

            if (!string.IsNullOrEmpty(spec.ImageUrl))
                deleteSpecImageFromUploads(spec.ImageUrl);

            var merchandiseid = _context.Specs
                .Where(s => s.SpecId == id).Select(s => s.MerchandiseId).FirstOrDefault();

            //刪除對應標籤資料
            using var conn = new SqlConnection(_context.Database.GetConnectionString());
            string str = "DELETE FROM SpecTags WHERE SpecId=@SpecId";
            conn.Execute(str, new { SpecId = id });

            _context.Specs.Remove(spec);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { merchandiseid,
                displaymode = int.TryParse(HttpContext.Request.Cookies["Spec_displaymode"], out int temp1) ? temp1 : 1,
                displayorder = int.TryParse(HttpContext.Request.Cookies["Spec_displayorder"], out int temp) ? temp : 0, });
        }

        public record TagRecord(string tagName, string specId, int merchandiseId);
        [HttpPost]
        [Authorize(Roles = "管理員,經理,員工")]
        public async Task<IActionResult> AddTag([FromBody] TagRecord record)
        {
            string tagName = record.tagName;
            int specId = int.Parse(record.specId);
            int merchandiseId = record.merchandiseId;

            if (!string.IsNullOrEmpty(tagName))
            {
                bool checkName = _context.Tags.Where(t => t.TagName == tagName).Any();
                //若為新標籤名稱則新增
                if (!checkName)
                {
                    Tag tag = new Tag();
                    tag.TagName = tagName;
                    _context.Add(tag);
                    await _context.SaveChangesAsync();
                }

                //bool chaeckSame = _context.SpecTagWithTagNames    //檢視表無主索引鍵，可能無法正確傳回結果
                //                   .Where(sttn => sttn.SpecId == specId).Where(sttn => sttn.TagName == tagName).Any();

                using var conn = new SqlConnection(_context.Database.GetConnectionString());
                var sql = "SELECT COUNT(*) FROM SpecTagWithTagName WHERE SpecId=@SpecId AND TagName=@TagName";
                int count = conn.QueryFirst<int>(sql, new { SpecId = specId, TagName = tagName });

                if (count > 0)
                    return RedirectToAction("Index", new { merchandiseid = merchandiseId });

                int tagId = await _context.Tags.Where(t => t.TagName == tagName).Select(t => t.TagId).FirstAsync();

                addSpecTag(specId, tagId);
            }

            return RedirectToAction("Index", new { merchandiseid = merchandiseId });
        }

        public record TagRecord_delete(string specId, string tagId, int merchandiseId);
        [Authorize(Roles = "管理員,經理,員工")]
        public async Task<IActionResult> DeleteTag([FromBody] TagRecord_delete record)
        {
            int specId = int.Parse(record.specId);
            int tagId = int.Parse(record.tagId);
            int merchandiseId = record.merchandiseId;

            if (_context.SpecTags == null) return Problem("找不到標籤資料");

            var spec = await _context.Specs
                .FirstOrDefaultAsync(s => s.SpecId == specId);
            if (spec == null) return Problem("找不到規格資料");
            var tag = await _context.Tags
                .FirstOrDefaultAsync(t => t.TagId == tagId);
            if (tag == null) return Problem("找不到標籤資料");

            var target = _context.SpecTags
                .Where(st => st.SpecId == specId && st.TagId == tagId).FirstOrDefault();

            if (target != null)
            {
                //資料表無主索引鍵，無法使用EF刪除 => 改使用Dapper語法
                using var conn = new SqlConnection(_context.Database.GetConnectionString());
                string str = "DELETE FROM SpecTags WHERE SpecId=@SpecId AND TagId=@TagId";
                conn.Execute(str, new { SpecId = specId, TagId = tagId });
            }
            return RedirectToAction("Index", new { merchandiseid = merchandiseId });
        }

        private bool SpecExists(int id)
        {
            return (_context.Specs?.Any(e => e.SpecId == id)).GetValueOrDefault();
        }
        private void saveSpecImageToUploads(string ImageUrl, IFormFile photo)
        {
            string savepath = Path.Combine(_host.WebRootPath, "uploads/specPicture", ImageUrl);
            using (var fileStream = new FileStream(savepath, FileMode.Create))
            {
                photo.CopyTo(fileStream);
            }
        }
        private void deleteSpecImageFromUploads(string ImageUrl)
        {
            string deletepath = Path.Combine(_host.WebRootPath, "uploads/specPicture", ImageUrl);
            System.IO.File.Delete(deletepath);
        }
        private void addSpecTag(int SpecId, int TagId)
        {
            //資料表無主索引鍵，無法使用EF新增 => 改使用Dapper語法
            using var conn = new SqlConnection(_context.Database.GetConnectionString());
            string str = "INSERT INTO SpecTags (SpecId,TagId) VALUES (@SpecId,@TagId)";
            conn.Execute(str, new { SpecId, TagId });
        }
    }
}
