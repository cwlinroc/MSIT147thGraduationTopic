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

        [HttpPost]
        public IActionResult CheckforCreateSpec(SpecVM specvm)
        {
            bool[] package = new bool[2];

            package[0] = _context.Specs
                .Where(s => s.MerchandiseId == specvm.MerchandiseId)
                .Any(s => s.SpecName == specvm.SpecName);

            package[1] = false;
            if (specvm.photo != null)
            {
                if (!specvm.photo.ContentType.Contains("image")) package[1] = true;
            }

            return Json(package);
        }

        [HttpPost]
        public IActionResult CheckforEditSpec(SpecVM specvm)
        {
            bool[] package = new bool[2];

            package[0] = _context.Specs
                .Where(s => s.MerchandiseId == specvm.MerchandiseId)
                .Where(s => s.SpecId != specvm.SpecId)
                .Any(s => s.SpecName == specvm.SpecName);

            package[1] = false;
            if (specvm.photo != null)
            {
                if (!specvm.photo.ContentType.Contains("image")) package[1] = true;
            }

            return Json(package);
        }
        public IActionResult CheckEvaluationforDeleteSpec(int id)
        {
            var exists = _context.Evaluations.Any(e => e.SpecId == id);

            return Json(exists);
        }
    }
}
