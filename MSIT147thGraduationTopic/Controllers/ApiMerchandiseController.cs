using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;

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

        public IActionResult Brands()
        {
            var datas = _context.Brands.OrderBy(a => a.BrandId);

            return Json(datas);
        }

        public IActionResult Categories()
        {
            var datas = _context.Categories.OrderBy(a => a.CategoryId);

            return Json(datas);
        }
    }
}
