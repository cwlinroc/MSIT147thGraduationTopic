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

        [HttpGet]
        public IActionResult ShowMerchandise(int id)
        {
            var datas = _context.MerchandiseSearches.Where(m => m.MerchandiseId == id);

            return Json(datas);
        }
    }
}
