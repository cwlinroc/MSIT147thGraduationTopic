using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.ViewModels;

namespace MSIT147thGraduationTopic.Controllers
{
    public class ApiCategoryController : Controller
    {
        private readonly GraduationTopicContext _context;

        public ApiCategoryController(GraduationTopicContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CheckCategoryforCreate(CategoryVM categoryvm)
        {
            var exists = _context.Categories.Any(c => c.CategoryName == categoryvm.CategoryName);

            return Json(exists);
        }

        [HttpPost]
        public IActionResult CheckCategoryforEdit(CategoryVM categoryvm)
        {
            var exists = _context.Categories
                .Where(c => c.CategoryId != categoryvm.CategoryId)
                .Any(c => c.CategoryName == categoryvm.CategoryName);

            return Json(exists);
        }
        public IActionResult CheckMerchandiseforDeleteCategory(int id)
        {
            var exists = _context.Merchandises.Any(m => m.CategoryId == id);

            return Json(exists);
        }
    }
}
