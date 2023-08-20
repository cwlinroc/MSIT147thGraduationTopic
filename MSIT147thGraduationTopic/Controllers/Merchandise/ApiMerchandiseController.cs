using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Infra.Repositories;
using MSIT147thGraduationTopic.Models.ViewModels;

namespace MSIT147thGraduationTopic.Controllers.Merchandise
{
    public class ApiMerchandiseController : Controller
    {
        private readonly GraduationTopicContext _context;
        private readonly MerchandiseRepository _repo;

        public ApiMerchandiseController(GraduationTopicContext context)
        {
            _context = context;
            _repo = new MerchandiseRepository(context);
        }

        [HttpGet]
        public IActionResult GetSearchResultLength(string txtKeyword, int searchCondition = 1, int displaymode = 1)
        {
            int resultLength = _repo.getBasicMerchandiseSearch(txtKeyword, searchCondition, displaymode).Count();

            return Json(resultLength);
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
        public IActionResult CheckforCreateMerchandise(MerchandiseVM merchandisevm)
        {
            bool[] package = new bool[2];

            package[0] = _context.Merchandises.Any(m => m.MerchandiseName == merchandisevm.MerchandiseName);
            package[1] = false;

            if (merchandisevm.photo != null)
            {
                if (!merchandisevm.photo.ContentType.Contains("image")) package[1] = true;
            }

            return Json(package);
        }

        [HttpPost]
        public IActionResult CheckforEditMerchandise(MerchandiseVM merchandisevm)
        {
            bool[] package = new bool[2];

            package[0] = _context.Merchandises
                .Where(m => m.MerchandiseId != merchandisevm.MerchandiseId)
                .Any(m => m.MerchandiseName == merchandisevm.MerchandiseName);
            package[1] = false;

            if (merchandisevm.photo != null)
            {
                if (!merchandisevm.photo.ContentType.Contains("image")) package[1] = true;
            }

            return Json(package);
        }

        public IActionResult CheckSpecforDeleteMerchandise(int id)
        {
            var exists = _context.Specs.Any(s => s.MerchandiseId == id);

            return Json(exists);
        }
    }
}
