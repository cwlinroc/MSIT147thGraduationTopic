using System;
using System.Collections.Generic;
using System.Data;
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
    public class BrandsController : Controller
    {
        private readonly GraduationTopicContext _context;

        public BrandsController(GraduationTopicContext context)
        {
            _context = context;
        }

        // GET: Brands
        [Authorize(Roles = "管理員,經理,員工")]
        public IActionResult Index(string txtKeyword, int PageIndex = 1)
        {
            ViewBag.txtKeyword = txtKeyword;
            ViewBag.PageIndex = PageIndex;

            IEnumerable<Brand> datas = string.IsNullOrEmpty(txtKeyword) ? from b in _context.Brands select b
                : _context.Brands.Where(b => b.BrandName.Contains(txtKeyword));

            datas = datas.Skip((PageIndex - 1) * 20).Take(20).ToList();

            List<BrandVM> list = new List<BrandVM>();
            foreach (Brand b in datas)
            {
                BrandVM brandvm = new BrandVM();
                brandvm.brand = b;
                list.Add(brandvm);
            }

            return View(list);
        }

        // GET: Brands/Create
        [Authorize(Roles = "管理員,經理,員工")]
        public IActionResult Create()//todo DEMO
        {
            BrandVM brandvm = new BrandVM();
            return View(brandvm);
        }

        // POST: Brands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "管理員,經理,員工")]
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
        [Authorize(Roles = "管理員,經理,員工")]
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
        [Authorize(Roles = "管理員,經理,員工")]
        public async Task<IActionResult> Edit(int id, [Bind("BrandId,BrandName")] BrandVM brandvm)
        {
            if (id != brandvm.BrandId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
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
        [Authorize(Roles = "管理員,經理")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.Merchandises.Where(m => m.BrandId == id).Count() > 0)
                return RedirectToAction(nameof(Index));

            if (id == null || _context.Brands == null)
                return Problem("找不到品牌資料");

            var brand = await _context.Brands.FirstOrDefaultAsync(m => m.BrandId == id);
            if (brand == null) return Problem("找不到品牌資料");

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
