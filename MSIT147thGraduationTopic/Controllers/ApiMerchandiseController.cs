using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.ViewModels;

namespace MSIT147thGraduationTopic.Controllers
{
    public class ApiMerchandiseController : Controller
    {
        private readonly GraduationTopicContext _context;

        public ApiMerchandiseController(GraduationTopicContext context)
        {
            _context = context;
        }

        public IActionResult Merchandises()
        {
            var datas = _context.Merchandises.OrderBy(a => a.MerchandiseId);

            return Json(datas);
        }

        public IActionResult GenerateBrandOptions()
        {
            var datas = _context.Brands.OrderBy(b => b.BrandId);

            return Json(datas);
        }

        public IActionResult GenerateCategoryOptions()
        {
            var datas = _context.Categories.OrderBy(c => c.CategoryId);

            return Json(datas);
        }

        [HttpPost]
        public IActionResult CheckMerchandiseforCreate(MerchandiseVM merchandisevm)
        {
            var exists = _context.Merchandises.Any(m => m.MerchandiseName == merchandisevm.MerchandiseName);

            return Json(exists);
        }









        [HttpPost]
        public IActionResult CheckMerchandiseforEdit(MerchandiseVM merchandisevm)
        {
            var exists = _context.Merchandises
                .Where(m => m.MerchandiseId != merchandisevm.MerchandiseId)
                .Any(m => m.MerchandiseName == merchandisevm.MerchandiseName);

            return Json(exists);
        }
        public IActionResult CheckSpecforDeleteMerchandise(int id)
        {
            var exists = _context.Specs.Any(s => s.SpecId == id);

            return Json(exists);
        }
    }
}
