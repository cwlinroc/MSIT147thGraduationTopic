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
        public IActionResult CheckMerchandiseforDelete(int id)
        {
            var exists = _context.Merchandises.Any(m => m.BrandId == id);

            return Json(exists);
        }
    }
}
