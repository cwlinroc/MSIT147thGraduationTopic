using Microsoft.AspNetCore.Mvc;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.ViewModels;

namespace MSIT147thGraduationTopic.Controllers
{
    public class ApiSpecController : Controller
    {
        private readonly GraduationTopicContext _context;

        public ApiSpecController(GraduationTopicContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Merchandise()
        {
            var datas = _context.MerchandiseSearches.Select(m => m);

            return Json(datas);
        }
    }
}
