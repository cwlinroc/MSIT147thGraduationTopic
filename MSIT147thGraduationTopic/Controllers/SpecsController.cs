using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.ViewModels;

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
        public IActionResult Index(int merchandiseid)
        {
            var datas = _context.Specs.Where(s => s.MerchandiseId == merchandiseid);

            List<SpecVM> list = new List<SpecVM>();

            if (datas.Count() == 0)
                return RedirectToAction("IndexForNoSpec", new { merchandiseid = merchandiseid });

            foreach (Spec s in datas)
            {
                SpecVM specvm = new SpecVM();
                specvm.spec = s;
                specvm.merchandiseIdCarrier = merchandiseid;
                list.Add(specvm);
            }

            return View(list);
        }
        public IActionResult IndexForNoSpec(int merchandiseid)
        {
            SpecVM specvmforCarrier = new SpecVM();
            specvmforCarrier.merchandiseIdCarrier = merchandiseid;
            specvmforCarrier.SpecName = "**此商品尚無規格，請新增規格資料**";
            return View(specvmforCarrier);
        }

        // GET: Specs/Create
        public IActionResult Create(int merchandiseIdCarrier)
        {
            ViewData["MerchandiseId"] = new SelectList(_context.Merchandises, "MerchandiseId", "MerchandiseName");
            SpecVM specvm = new SpecVM();
            specvm.MerchandiseId = merchandiseIdCarrier;
            specvm.Popularity = 0;
            return View(specvm);
        }

        // POST: Specs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create
            ([Bind("SpecId,SpecName,MerchandiseId,Price,Amount,ImageUrl,DisplayOrder,Popularity,OnShelf,DiscountPercentage,photo")] SpecVM specvm)
        {
            if (ModelState.IsValid)
            {
                if (specvm.photo != null)
                {
                    specvm.ImageUrl = Guid.NewGuid().ToString() + specvm.photo.FileName;
                    saveSpecImageToUploads(specvm.ImageUrl, specvm.photo);
                }

                _context.Add(specvm.spec);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { merchandiseid = specvm.MerchandiseId });
            }
            ViewData["MerchandiseId"] = new SelectList(_context.Merchandises, "MerchandiseId", "MerchandiseName", specvm.MerchandiseId);
            return View(specvm);
        }

        // GET: Specs/Edit/5
        public async Task<IActionResult> Edit(int merchandiseid, string merchandisename, int? id)
        {
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
                    specvm.ImageUrl = Guid.NewGuid().ToString() + specvm.photo.FileName;
                    saveSpecImageToUploads(specvm.ImageUrl, specvm.photo);
                }
                //有圖→新圖
                if (specvm.ImageUrl != null && specvm.photo != null)
                {
                    deleteSpecImageFromUploads(specvm.ImageUrl);
                    specvm.ImageUrl = Guid.NewGuid().ToString() + specvm.photo.FileName;
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
                return RedirectToAction("Index", new { merchandiseid = specvm.MerchandiseId });
            }
            ViewData["MerchandiseId"] = new SelectList(_context.Merchandises, "MerchandiseId", "MerchandiseName", specvm.MerchandiseId);
            return View(specvm);
        }

        // GET: Specs/Delete/5
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

            _context.Specs.Remove(spec);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { merchandiseid = merchandiseid });
        }


        public async Task<IActionResult> AddTag(string tagName, int specId, int merchandiseId)
        {
            bool checkName = _context.Tags.Where(t => t.TagName == tagName).Any();

            //若為新標籤則新增
            if (!checkName)
            {
                Tag tag = new Tag();
                tag.TagName = tagName;
                _context.Add(tag);
                await _context.SaveChangesAsync();
            }

            bool chaeckSame = _context.SpecTagWithTagNames.Where(sttn => sttn.SpecId == specId)
                                                    .Where(sttn => sttn.TagName == tagName).Any();
            if(chaeckSame) 
                return RedirectToAction("Index", new { merchandiseid = merchandiseId });

            int tagId = await _context.Tags.Where(t => t.TagName == tagName).Select(t => t.TagId).FirstAsync();

            //資料表無主索引鍵，無法使用EF新增
            //SpecTag specTag = new SpecTag()
            //{
            //    SpecId = specId,
            //    TagId = tagId,
            //}; 
            //_context.SpecTags.Add(specTag);
            //await _context.SaveChangesAsync();

            //Dapper語法
            using var conn = new SqlConnection(_context.Database.GetConnectionString());
            string str = "INSERT INTO SpecTags (SpecId,TagId) VALUES (@SpecId,@TagId)";
            conn.Execute(str, new { SpecId = specId, TagId = tagId });

            return RedirectToAction("Index", new { merchandiseid = merchandiseId });
        }

        public async Task<IActionResult> DeleteTag(int specId, int tagId, int merchandiseId)
        {
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
                _context.SpecTags.Remove(target);
                await _context.SaveChangesAsync();
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
    }
}
