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

        public IActionResult Cities()
        {
            var datas = _context.Address.Select(a => new
            {
                a.City
            }).Distinct().OrderBy(a => a.City);

            return Json(datas);
        }
    }
}
