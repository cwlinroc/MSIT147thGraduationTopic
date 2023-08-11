using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.ViewModels;
using System.Data;
using System.Diagnostics.Metrics;

namespace MSIT147thGraduationTopic.Controllers
{
    public class ApiBrandController : Controller
    {
        private readonly GraduationTopicContext _context;

        public ApiBrandController(GraduationTopicContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetSearchResultLength(string txtKeyword)
        {
            IEnumerable<Brand> datas = string.IsNullOrEmpty(txtKeyword) ? from b in _context.Brands select b
                : _context.Brands.Where(b => b.BrandName.Contains(txtKeyword));

            var resultLength = datas.Count();

            return Json(resultLength);
        }

        [HttpPost]
        public IActionResult CheckBrandforCreate(BrandVM brandvm)
        {
            var exists = _context.Brands.Any(b => b.BrandName == brandvm.BrandName);

            return Json(exists);
        }

        [HttpPost]
        public IActionResult CheckBrandforEdit(BrandVM brandvm)
        {
            var exists = _context.Brands
                .Where(b => b.BrandId != brandvm.BrandId)
                .Any(b => b.BrandName == brandvm.BrandName);

            return Json(exists);
        }
        public IActionResult CheckMerchandiseforDeleteBrand(int id)
        {
            var exists = _context.Merchandises.Any(m => m.BrandId == id);

            return Json(exists);
        }
    }
}
