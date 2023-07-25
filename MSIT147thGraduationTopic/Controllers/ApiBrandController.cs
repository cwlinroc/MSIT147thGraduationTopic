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
        public IActionResult CheckBrand(BrandVM brandvm)
        {
            var exists = _context.Brands.Any(b => b.BrandName == brandvm.BrandName);

            return Json(exists);
        }
    }
}
