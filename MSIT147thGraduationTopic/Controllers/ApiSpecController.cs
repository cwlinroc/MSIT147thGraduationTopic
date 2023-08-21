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

        [HttpGet]
        public IActionResult GetSearchResultLength(int merchandiseid, int displaymode = 1)
        {
            IEnumerable<Spec> datas = datas = _context.Specs.Where(s => s.MerchandiseId == merchandiseid);
            datas = displaymode switch
            {
                0 => datas = datas.Where(s => s.OnShelf == true),
                2 => datas = datas.Where(s => s.OnShelf == false),
                _ => datas = datas.Select(s => s)
            };

            int resultLength = datas.Count();

            return Json(resultLength);
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
        public IActionResult CheckDataLinkforDeleteSpec(int id)
        {
            bool result = false;
            var existEvaluation = _context.Evaluations.Any(e => e.SpecId == id);
            var existCartItem = _context.CartItems.Any(e => e.SpecId == id);
            var existOrderList = _context.OrderLists.Any(e => e.SpecId == id);
            var existManually = _context.ManuallyWeightedEntries.Any(e => e.SpecId == id);

            if (existEvaluation || existCartItem || existOrderList || existManually) result = true;

            return Json(result);
        }
    }
}
