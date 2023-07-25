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
        public IActionResult CheckCategory(CategoryVM categoryvm)
        {
            var exists = _context.Categories.Any(c => c.CategoryName == categoryvm.CategoryName);

            return Json(exists);
        }
    }
}
